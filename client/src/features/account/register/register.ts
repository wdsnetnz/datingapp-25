import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterCreds, User } from '../../../types/user';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  //membersFromHome = input.required<User[]>();
  private accountService = inject(AccountService);
  cancelRegister = output<boolean>();
  protected creds = {} as RegisterCreds;

  register() {
    console.log('Registering user with credentials:', this.creds);
    // Add registration logic here  
    this.accountService.register(this.creds).subscribe({
      next: response => {
        console.log('Registration successful:', response);
        this.cancel();
      },
      error: error => {
        console.error('Registration failed:', error);
      }
    });

  }

  cancel() {
    console.log('Registration cancelled');
    // Add cancellation logic here
    this.cancelRegister.emit(false);
  }
}
