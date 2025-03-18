import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NavComponent } from '../nav/nav.component';
@Component({
  selector: 'app-createevent',
  imports: [FormsModule, CommonModule,NavComponent],
  
  standalone: true,
  templateUrl: './createevent.component.html',
  styleUrls: ['./createevent.component.css'],
})
export class CreateeventComponent implements OnInit {

  events: any[] = [];
  newEvent: any = {
    eventId: 0,
    title: '',
    price: 0,
    isActive: true,
    categoryId: 0,
    categoryName: '',
    venueName: '',
    startTime: '',
    endTime: ''
  };
  isLoading: boolean = true;
  errorMessage: string = '';
  private baseUrl = 'https://localhost:44326/api/Event';
  validationErrors: any;
categories: any;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.fetchEvents();
  }

  fetchEvents(): void {
    const headers = this.getAuthHeaders();
    this.http.get<any[]>(`${this.baseUrl}`, { headers }).subscribe({
      next: (events) => {
        this.events = events;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error fetching events:', error);
        this.errorMessage = 'Failed to load events.';
        this.isLoading = false;
      },
    });
  }

  createEvent(): void {
    const headers = this.getAuthHeaders();
    const endpoint = this.newEvent.eventId ? `${this.baseUrl}/${this.newEvent.eventId}` : this.baseUrl;
    const method = this.newEvent.eventId ? 'put' : 'post';
    this.http.request(method, endpoint, { body: this.newEvent, headers }).subscribe({
      next: () => {
        alert(this.newEvent.eventId ? 'Event updated successfully!' : 'Event created successfully!');
        this.newEvent = { eventId: 0, title: '', price: 0, isActive: true, categoryId: 0, categoryName: '', venueName: '', startTime: '', endTime: '' };
        this.fetchEvents();
      },
      error: (error) => {
        console.error('Error creating/updating event:', error);
        alert('Failed to create/update event. Please check the details and try again.');
      },
    });
  }

  deleteEvent(eventId: number): void {
    const headers = this.getAuthHeaders();
    if (confirm('Are you sure you want to delete this event?')) {
      this.http.delete(`${this.baseUrl}/${eventId}`, { headers }).subscribe({
        next: () => {
          alert('Event deleted successfully!');
          this.fetchEvents();
        },
        error: (error) => {
          console.error('Error deleting event:', error);
          alert('Failed to delete event.');
        },
      });
    }
  }

  editEvent(event: any): void {
    this.newEvent = { ...event };
    alert('You can now edit the event details in the "Create New Event" form and click Submit to save changes.');
  }

  private getAuthHeaders() {
    const token = localStorage.getItem('token');
    return new HttpHeaders({ Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' });
  }
}