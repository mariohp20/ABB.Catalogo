import { Component } from '@angular/core';
import { UsuarioLogin } from '../../core/models/UsuarioLogin';
import { AuthService } from '../../core/services/auth';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  usuario: UsuarioLogin = {
    userName: '',
    clave: ''
  };

  constructor(private authService: AuthService, private router: Router) { }

  onLogin(): void {
    // Invocamos el login de la API
    this.authService.login(this.usuario).subscribe({
      next: (res) => {
        console.log('Token generado:', res.token)
        this.router.navigate(['/usuarios']); // Navegamos al listado tras el éxito
      },
      error: (err) => {
        alert('Credenciales no válidas');
        console.error(err);
      }
    });
  }
}
