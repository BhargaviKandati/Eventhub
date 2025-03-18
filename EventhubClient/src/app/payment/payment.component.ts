import { Component } from '@angular/core';
import { UsernavComponent } from "../usernav/usernav.component";
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-payment',
  imports: [UsernavComponent, CommonModule, FormsModule],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent {

  eventDetails: any = {};
  paymentDto = {
    paymentId: 0,
    ticketId: 0,
    paymentDate: '',
    method: ''
  };
  bookingInfo: any;
  noOfTickets: number = 0;
  paymentMethods = ['UPI', 'Credit Card', 'Debit Card'];

  constructor(private http: HttpClient, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const eventId = Number(params['eventId']);
      const bookingId = Number(params['bookingId']);
      this.getEventDetails(eventId);
      this.getNoOfTickets(bookingId);
    });

    // Set the current date as the default payment date
    const currentDate = new Date().toISOString().split('T')[0];
    this.paymentDto.paymentDate = currentDate;
  }

  getNoOfTickets(bookingId: number): void {
    // Subscribe to the HTTP GET request to fetch event info
    this.http.get(`https://localhost:44326/api/Booking/${bookingId}`)
      .subscribe(
        (data) => {
          console.log(data);
          this.bookingInfo = data;
          console.log('bookingInfo');
          console.log(this.bookingInfo);
          this.noOfTickets = this.bookingInfo.noOfTickets;
        },
        (error) => {
          console.error('Error fetching event info:', error);
          alert('Error fetching booking information: ' + error.message);
        }
      );
  }

  getEventDetails(eventId: number): void {
    this.http.get(`https://localhost:44326/api/Event/${eventId}`)
      .subscribe(response => {
        this.eventDetails = response;
        console.log(this.eventDetails);
      }, error => {
        console.error('Error fetching event details', error);
        alert('Error fetching event details: ' + error.message);
      });
  }

  createPayment(): void {
    console.log(this.paymentDto);
    this.http.post('https://localhost:44326/api/Payment', this.paymentDto)
      .subscribe(response => {
        console.log('Payment successful', response);
        alert('Payment successful!');
      }, error => {
        console.error('Error creating payment', error);
        alert('Error creating payment: ' + error.message);
      });
  }
}