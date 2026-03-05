import { Component, signal, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common'; 
import { RegistracijaComponent } from './komponente/registracija/registracija';
import { LoginComponent } from './komponente/login/login';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule, RegistracijaComponent, LoginComponent, DatePipe],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  protected readonly title = signal('client');
  
  private http = inject(HttpClient); 
  private cdr = inject(ChangeDetectorRef);

  categories: any[] = [];
  products: any[] = []; 
  porudzbine: any[] = []; 
  
  mojePorudzbine: any[] = [];
  sviKorisnici: any[] = [];
  
  selectedCategory: any = null;
  selectedSubcategory: any = null;

  tekstPretrage: string = '';
  kriterijumSortiranja: string = ''; 

  korpa: any[] = [];
  ukupnaCena: number = 0;
  
  otvorenaKorpa: boolean = false;
  prikaziPrijavu: boolean = true; 
  prikaziLoginProzor: boolean = false; 

  prikaziProfil: boolean = false;
  podaciProfila: any = {};

  noviProizvod = {
    naziv: '',
    cena: 0,
    slika: 'https://via.placeholder.com/150',
    kategorijaId: 1,
    potkategorija: '',
    kolicinaNaStanju: 10
  };

  ngOnInit() {
    this.pozoviBackend();
    if (this.isUlogovan && !this.isAdmin) {
      this.ucitajMojePorudzbine();
    }
    if (this.isAdmin) {
      this.ucitajSveKorisnike();
    }
  }

  toggleForma() {
    this.prikaziPrijavu = !this.prikaziPrijavu;
  }

  get isUlogovan() {
    const korisnik = localStorage.getItem('ulogovaniKorisnik');
    return !!korisnik && korisnik !== 'undefined' && korisnik !== 'null';
  }

  get isAdmin() {
    const korisnikSirov = localStorage.getItem('ulogovaniKorisnik');
    if (!korisnikSirov || korisnikSirov === 'undefined' || korisnikSirov === 'null') {
      return false;
    }
    try {
      const korisnik = JSON.parse(korisnikSirov);
      return korisnik && korisnik.uloga && korisnik.uloga.toLowerCase() === 'admin';
    } catch (e) {
      return false;
    }
  }

  get svePotkategorije() {
    const sve = this.categories.flatMap(c => c.subcategories || []);
    return [...new Set(sve)];
  }

  odjaviSe() {
    localStorage.removeItem('ulogovaniKorisnik');
    this.selectedCategory = null;
    this.prikaziProfil = false;
    this.otvorenaKorpa = false;
    this.cdr.detectChanges();
    window.location.reload(); 
  }

  otvoriProfil() {
    this.prikaziProfil = true;
    this.otvorenaKorpa = false; 
    this.selectedCategory = null; 
    
    const korisnikSirov = localStorage.getItem('ulogovaniKorisnik');
    if (korisnikSirov) {
      const korisnik = JSON.parse(korisnikSirov);
      this.http.get(`http://localhost:5261/api/korisnici/${korisnik.email}`).subscribe({
        next: (podaci: any) => {
          this.podaciProfila = podaci;
          this.cdr.detectChanges();
        }
      });
      this.ucitajMojePorudzbine();
    }
  }

  zatvoriProfil() {
    this.prikaziProfil = false;
  }

  sacuvajProfil() {
    this.http.put(`http://localhost:5261/api/korisnici/${this.podaciProfila.email}`, this.podaciProfila).subscribe({
      next: () => alert("Podaci su uspešno ažurirani! ✅"),
      error: () => alert("Greška pri čuvanju podataka.")
    });
  }

  ucitajMojePorudzbine() {
    const korisnikSirov = localStorage.getItem('ulogovaniKorisnik');
    if (korisnikSirov) {
      const korisnik = JSON.parse(korisnikSirov);
      this.http.get<any[]>(`http://localhost:5261/api/porudzbine/moje/${korisnik.email}`).subscribe({
        next: (podaci) => {
          this.mojePorudzbine = podaci;
          this.cdr.detectChanges();
        }
      });
    }
  }

  ucitajSveKorisnike() {
    this.http.get<any[]>('http://localhost:5261/api/korisnici').subscribe({
      next: (podaci) => {
        this.sviKorisnici = podaci;
        this.cdr.detectChanges();
      }
    });
  }

  obrisiKorisnika(id: string) {
    if (confirm("Jeste li sigurni da želite trajno obrisati ovog korisnika?")) {
      this.http.delete(`http://localhost:5261/api/korisnici/${id}`).subscribe({
        next: () => {
          alert("Korisnik je uspješno obrisan!");
          this.sviKorisnici = this.sviKorisnici.filter(k => k.id !== id);
          this.cdr.detectChanges();
        },
        error: () => alert("Greška pri brisanju korisnika.")
      });
    }
  }

  dodajProizvod() {
    if (!this.noviProizvod.naziv || this.noviProizvod.cena <= 0) return;
    this.http.post('http://localhost:5261/api/proizvodi', this.noviProizvod).subscribe({
      next: (proizvodIzBaze: any) => {
        this.products.push(proizvodIzBaze);
        alert("Proizvod uspešno dodat!");
        this.noviProizvod = { naziv: '', cena: 0, slika: 'https://via.placeholder.com/150', kategorijaId: 1, potkategorija: '', kolicinaNaStanju: 10 };
        this.cdr.detectChanges();
      }
    });
  }

  promeniCenu(proizvod: any) {
    const novaCena = prompt("Unesite novu cenu:", proizvod.cena);
    if (novaCena !== null && novaCena !== "" && !isNaN(Number(novaCena))) {
      proizvod.cena = Number(novaCena);
      this.http.put(`http://localhost:5261/api/proizvodi/${proizvod.id}`, proizvod).subscribe();
    }
  }

  obrisiProizvod(id: number) {
    if (confirm("Brisanje?")) {
      this.http.delete(`http://localhost:5261/api/proizvodi/${id}`).subscribe({
        next: () => this.products = this.products.filter(p => p.id !== id)
      });
    }
  }

  pozoviBackend() {
    this.http.get<any[]>('http://localhost:5261/api/kategorije').subscribe(p => this.categories = p);
    this.http.get<any[]>('http://localhost:5261/api/proizvodi').subscribe(p => this.products = p);
    this.http.get<any[]>('http://localhost:5261/api/porudzbine').subscribe(p => this.porudzbine = p);
  }

  get prikazaniProizvodi() {
    let rezultat = this.products;
    if (this.selectedCategory) rezultat = rezultat.filter(p => p.kategorijaId === this.selectedCategory.id);
    if (this.selectedSubcategory) rezultat = rezultat.filter(p => p.potkategorija === this.selectedSubcategory);
    if (this.tekstPretrage) rezultat = rezultat.filter(p => p.naziv.toLowerCase().includes(this.tekstPretrage.toLowerCase()));
    if (this.kriterijumSortiranja === 'jeftinije') rezultat = rezultat.sort((a, b) => a.cena - b.cena);
    else if (this.kriterijumSortiranja === 'skuplje') rezultat = rezultat.sort((a, b) => b.cena - a.cena);
    return rezultat;
  }

  dodajUKorpu(izabraniProizvod: any) {
    if (!this.isUlogovan) {
      alert("Morate se prijaviti ili registrovati da biste kupovali! 🔒");
      return;
    }
    if (izabraniProizvod.kolicinaNaStanju > 0) {
      this.korpa.push(izabraniProizvod); 
      this.ukupnaCena += izabraniProizvod.cena; 
      izabraniProizvod.kolicinaNaStanju -= 1;
      this.otvorenaKorpa = true; 
    }
  }

  ukloniIzKorpe(index: number) {
    const proizvodKojiSeBrise = this.korpa[index];
    this.ukupnaCena -= proizvodKojiSeBrise.cena;
    proizvodKojiSeBrise.kolicinaNaStanju += 1;
    this.korpa.splice(index, 1);
    if (this.korpa.length === 0) this.otvorenaKorpa = false;
  }

  zavrsiKupovinu() {
    if (this.korpa.length === 0) return;
    const korisnik = JSON.parse(localStorage.getItem('ulogovaniKorisnik') || '{}');
    const podaciZaStripe = {
      ukupnaCena: this.ukupnaCena,
      opisProizvoda: this.korpa.map(p => p.naziv).join(', '),
      emailKupca: korisnik.email || '',
      adresaKupca: korisnik.punaAdresa || ''
    };
    this.http.post('http://localhost:5261/api/checkout/create-checkout-session', podaciZaStripe)
      .subscribe({ next: (odgovor: any) => window.location.href = odgovor.url });
  }

  isprazniKorpu() {
    this.korpa = [];
    this.ukupnaCena = 0;
    this.otvorenaKorpa = false;
  }
}