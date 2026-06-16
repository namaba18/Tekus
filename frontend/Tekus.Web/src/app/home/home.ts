import { Component, signal } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../core/auth/auth.service';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home {
  protected readonly title = signal('Tekus.Web');
  protected readonly username = signal('');

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {
    this.username.set(this.authService.getUsername() ?? '');
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
