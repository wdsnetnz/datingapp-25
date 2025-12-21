import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  
  private  httpClient = inject(HttpClient);
  protected readonly title = signal('Dating App');
  protected members = signal<any>([]);

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
    this.members.set( await this.getMembers());
    console.log(this.members);
  }

  async getMembers() {
    try {
      return lastValueFrom(this.httpClient.get('https://localhost:5001/api/members'));
    } catch (error) {
      console.error(error);
      throw error;
    }
  }
}