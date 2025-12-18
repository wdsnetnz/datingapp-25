import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';


@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private readonly http = inject(HttpClient);
  protected readonly title = 'client Dinesh';

  ngOnInit(): void {
    this.http.get('http://localhost:4200/').subscribe({
      next: response => console.log('Response from server:', response),
      error: err => console.error('Error occurred:', err),
      complete: () => console.log('Request completed')
    });
  }
}
