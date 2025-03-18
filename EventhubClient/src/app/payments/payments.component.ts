import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { NavComponent } from '../nav/nav.component';

interface Payment {
  paymentId: number;
  amount: number;
  paymentDate: string;
  method: string;
  ticketId: number;
}
@Component({

  selector: 'app-payments',
  imports:[CommonModule,NavComponent],
  templateUrl: './payments.component.html',
  styleUrls: ['./payments.component.css']

})

export class PaymentsComponent implements OnInit {
  payments: Payment[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getPayments();
  }

  getPayments(): void {
    this.http.get<Payment[]>('https://localhost:44326/api/Payment')
      .subscribe(data => {
        this.payments = data;
      });
  }
}