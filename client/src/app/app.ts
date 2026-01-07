import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { Nav } from "../layout/nav/nav";
//import { AccountService } from '../core/services/account-service';
import { User } from '../types/user';

@Component({
  selector: 'app-root',
  imports: [Nav, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App  {
  // private accountService = inject(AccountService);
  protected router = inject(Router);
  /* private  httpClient = inject(HttpClient);
  protected readonly title = signal('Dating App');
  protected members = signal<User[]>([]); */

  //before 
  //constructor(private httpClient: HttpClient) {  }

  // ngOnInit(): void {
  //   this.httpClient.get('https://localhost:5001/api/members').subscribe({
  //     next: response => {
  //       this.members.set(response);
  //       console.log(this.members);
  //     },
  //     error: error => {
  //       console.error(error);
  //     },
  //     complete: () => {
  //       console.log('Request completed');
  //     }
  //   });
  // }

 

  // Set the current user from local storage moved to InitService

  // Load members from API better way with async/await 
  
}