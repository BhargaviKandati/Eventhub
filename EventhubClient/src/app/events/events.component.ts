import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { UsernavComponent } from '../usernav/usernav.component';
import { Router } from '@angular/router';
@Component({
  selector: 'app-events',
  imports :[CommonModule,UsernavComponent],
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {
  events: any[] = [];

  constructor(private http: HttpClient,private router:Router) {}

  ngOnInit(): void {
    this.getEvents();
  }

  getEvents(): void {
    this.http.get<any[]>('https://localhost:44326/api/Event')
      .subscribe(data => {
        this.events = data;
      });
  }
  navigateToBooking(eventId: number): void {
    // alert(eventId);
    this.router.navigate(['/app-booking',eventId]);
  }
}