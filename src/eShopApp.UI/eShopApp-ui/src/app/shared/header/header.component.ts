import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  isLoggedIn: boolean = false;

  constructor(private router: Router) {
    this.checkAuthenticationStatus();
  }

  checkAuthenticationStatus(): void {
    this.isLoggedIn = true;
  }

  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }
}
