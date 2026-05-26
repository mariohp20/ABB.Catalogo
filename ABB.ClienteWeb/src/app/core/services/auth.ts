import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { UsuarioResponse } from '../models/UsuarioResponse';
import { UsuarioLogin } from '../models/UsuarioLogin';
import { environment } from '../../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  login(usuario: UsuarioLogin) {
    return this.http.post<UsuarioResponse>(`${this.apiUrl}/usuarios/login`, {
      codUsuario: usuario.userName,
      password: usuario.clave
    }).pipe(
      tap(user => {
        this.saveSession(user);
      })
    );
  }

  private saveSession(user: UsuarioResponse) {
    localStorage.setItem('token', user.token);
    localStorage.setItem('user', JSON.stringify(user));
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUser(): UsuarioResponse | null {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }

  logout() {
    localStorage.clear();
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
  getUserRole(): string | null {
    return this.getUser()?.desRol || null;
  }
}
