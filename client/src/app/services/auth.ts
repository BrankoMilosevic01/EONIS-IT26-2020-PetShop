import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7097/api/Korisnici'; 

  constructor(private http: HttpClient) { }

  registracija(podaci: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/registracija`, podaci);
  }

  login(podaci: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, podaci);
  }
}