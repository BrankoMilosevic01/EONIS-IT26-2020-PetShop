import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth'; 

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html'
})
export class LoginComponent {
  podaci = {
    email: '',
    lozinka: ''
  };

  poruka: string = '';

  constructor(private authService: AuthService) { }

  prijaviSe() {
    this.authService.login(this.podaci).subscribe({
      next: (odgovor: any) => {
        localStorage.setItem('ulogovaniKorisnik', JSON.stringify(odgovor));
        window.location.reload();
      },
      error: (greska) => {
        this.poruka = 'Pogrešan email ili lozinka.';
      }
    });
  }
}