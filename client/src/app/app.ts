import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { Nav } from "../layout/nav/nav";
import { AccountService } from '../core/services/account-service';
import { Home } from "../features/home/home";
import { User } from '../types/user';

@Component({
  selector: 'app-root',
  imports: [Nav, Home],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private accountService = inject(AccountService);
  private  httpClient = inject(HttpClient);
  protected readonly title = signal('Dating App');
  protected members = signal<User[]>([]);

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

  async ngOnInit() {
    this.members.set(await this.getMembers());
    this.setCurrentUser();
    console.log(this.members);
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');

    if (!userString) return; 

    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }

  async getMembers() {
    try {
      return lastValueFrom(this.httpClient.get<User[]>('https://localhost:5001/api/members'));
    } catch (error) {
      console.error(error);
      throw error;
    }
  }
}