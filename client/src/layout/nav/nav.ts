import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink, RouterLinkActive } from "@angular/router";
import { ToastService } from '../../core/services/toast-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
  protected accountService = inject(AccountService);
  protected creds: any = {};
  private router = inject(Router);
  private toastService = inject(ToastService);

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
