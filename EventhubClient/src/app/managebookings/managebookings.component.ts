import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NavComponent } from '../nav/nav.component';

interface Booking {
  bookingId: number;
  eventTitle: string;
  venueName: string;
  bookingDate: string;
  noOfTickets: number;
}

@Component({
 selector: 'app-managebookings',
 imports: [CommonModule,NavComponent],
 templateUrl: './managebookings.component.html',
 styleUrl: './managebookings.component.css'
})
export class ManageBookingsComponent implements OnInit {
  bookings: Booking[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getBookings();
  }

  getBookings(): void {
    this.http.get<Booking[]>('https://localhost:44326/api/Booking')
      .subscribe(data => {
        this.bookings = data;
      });
  }
}