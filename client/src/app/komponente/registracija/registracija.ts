import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-registracija',
  standalone: true,            
  imports: [FormsModule],      
  templateUrl: './registracija.html' 
})
export class RegistracijaComponent {
  
  korisnik = {
    imePrezime: '',
    email: '',
    lozinka: '',
    ulica: '',
    kucniBroj: '',
    brojStana: '',
    mesto: '',
    postanskiBroj: ''
  };

  poruka: string = '';

  constructor(private authService: AuthService) { }

  registrujSe() {
    this.authService.registracija(this.korisnik).subscribe({
      next: (odgovor) => {
        this.poruka = 'Uspešna registracija! Sada možete da se prijavite.';
      },
      error: (greska) => {
        this.poruka = 'Greška pri registraciji: ' + (greska.error[0]?.description || 'Proverite podatke.');
      }
    });
  }
}