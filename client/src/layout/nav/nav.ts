import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink, RouterLinkActive } from "@angular/router";
import { ToastService } from '../../core/services/toast-service';
import { themes } from '../theme';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav implements OnInit {
  protected accountService = inject(AccountService);
  protected creds: any = {};
  private router = inject(Router);
  private toastService = inject(ToastService);
  protected selectedTheme = signal<string>(localStorage.getItem('theme') || 'light');
  protected themes = themes;

  ngOnInit(): void {
    document.documentElement.setAttribute('data-theme', this.selectedTheme());
  }

handleSelectTheme(theme: string) {
  this.selectedTheme.set(theme);
  localStorage.setItem('theme', theme);
  document.documentElement.setAttribute('data-theme', theme);
  const elem = document.activeElement as HTMLDivElement;
  if (elem) elem.blur();
}

  login() {
    this.accountService.login(this.creds).subscribe({
      next: () => {
        this.router.navigateByUrl('/members');
        this.toastService.success('Login successful');
        alert('Login successful');
        console.log('logged in');    
        this.creds = {};        
      },
      error: error => {
        alert('Login failed' + error.error);
        this.toastService.error( error.error);
        console.log(error);
      }
    });   
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
    console.log('logged out');
  }

}
