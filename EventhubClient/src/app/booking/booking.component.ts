import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { UsernavComponent } from "../usernav/usernav.component";
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-booking',
  imports: [UsernavComponent, CommonModule, FormsModule],
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css'],
})
export class BookingComponent {
  bookingDto = {
    eventId: 0,
    userId: localStorage.getItem("id"), //
    noOfTickets: 0,
    bookingDate: '',
    bookingId: 0 
  };

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    // Subscribe to route parameters to get the eventId
    this.route.params.subscribe(params => {
      this.bookingDto.eventId = Number(params['eventId']);
      console.log(this.bookingDto.eventId); // Access the eventId from the route
    });
    const currentDate = new Date().toISOString().split('T')[0];
    this.bookingDto.bookingDate = currentDate;
  }

  createBooking(): void {
    console.log("booking");
    console.log(this.bookingDto);
    alert("Your booking is in process, Please wait!")
    this.http.post('https://localhost:44326/api/Booking', this.bookingDto)
      .subscribe((response: any) => {
        alert('Booking successful');
        const bookingId = response.bookingId; // Assuming the response contains the bookingId
        this.bookingDto.bookingId = bookingId; // Set the bookingId in bookingDto
        this.navigateTo(this.bookingDto.eventId, bookingId);
      }, error => {
        console.error('Error creating booking', error);
      });
  }

  navigateTo(eventId: number, bookingId: number): void {
    this.router.navigate(['app-tickets', eventId, bookingId]);
  }
}