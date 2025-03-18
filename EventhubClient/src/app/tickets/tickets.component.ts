import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { UsernavComponent } from "../usernav/usernav.component";
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-tickets',
  imports: [UsernavComponent, CommonModule, FormsModule],
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
})
export class TicketsComponent {
  ticketDto = {
    eventId: 0,
    userId: localStorage.getItem("id"), // Assuming user ID is stored in local storage
    purchaseDate: '',
    bookingId: 0,
    price: 0 // Added price to ticketDto
  };
  eventInfo: any;

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.ticketDto.eventId = Number(params['eventId']);
      this.ticketDto.bookingId = Number(params['bookingId']);
      console.log(this.ticketDto.eventId); // Access the eventId from the route
      console.log(this.ticketDto.bookingId); // Access the bookingId from the route
    });

    const currentDate = new Date().toISOString().split('T')[0];
    this.ticketDto.purchaseDate = currentDate;
    this.getEventsDetails();
  }

  getEventsDetails(): void {
    // Subscribe to the HTTP GET request to fetch event info
    this.http.get(`https://localhost:44326/api/Event/${this.ticketDto.eventId}`)
    .subscribe(
      (data) => {
        this.eventInfo = data;
        console.log('ticketInfo')
        console.log(this.eventInfo.bookingId);
      },
      (error) => {
        console.error('Error fetching event info:', error);
      }
    );
  }

  submitTicket(): void {
    console.log("Submitting ticket");
    console.log(this.ticketDto);
    this.http.post('https://localhost:44326/api/Ticket', this.ticketDto)
      .subscribe((response: any) => {
        console.log('Ticket submission successful', response);
        console.log(`API Response: ${JSON.stringify(response)}`);
        const ticketId = response.ticketId; // Assuming the response contains the ticketId
        const price = this.eventInfo.price; // Assuming the response contains the price
        if (price === undefined) {
          console.error('Price is undefined in the API response');
        }
        console.log(`Navigating to payment with eventId: ${this.ticketDto.eventId}, price: ${price}, ticketId: ${ticketId}`);
        this.navigateToPayment(this.ticketDto.eventId, price, this.ticketDto.bookingId, ticketId);
      }, error => {
        console.error('Error submitting ticket', error);
      });
  }

  navigateToPayment(eventId: number, price: number, bookingId: number, ticketId: number): void {
    if (eventId && price && ticketId) {
      this.router.navigate(['app-payment', eventId, price, bookingId, ticketId]);
    } else {
      console.error('Navigation parameters are undefined', { eventId, price, bookingId, ticketId });
    }
  }
  
}