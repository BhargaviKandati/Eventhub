// import { Component, OnInit } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { ActivatedRoute, Router } from '@angular/router';
// import { NavComponent } from '../nav/nav.component';
// import { CommonModule } from '@angular/common';
// interface User {
//   userId: number;
//   fullName: string;
//   userName: string;
//   email: string;
//   phoneNumber: string;
//   role: string;
// }

// @Component({
//   selector: 'app-viewuser',
//   templateUrl: './viewuser.component.html',
//   styleUrls: ['./viewuser.component.css'],
//   imports: [CommonModule,NavComponent],
// })
// export class ViewUserComponent implements OnInit {
//   users: User[] = [];

//   constructor(private http: HttpClient) {}

//   ngOnInit(): void {
//     this.getUsers();
//   }

//   getUsers(): void {
//     this.http.get<User[]>('https://localhost:44326/api/User')
//       .subscribe(
//         data => this.users = data,
//         error => console.error('Error fetching user data:', error)
//       );
//   }
// }

import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NavComponent } from '../nav/nav.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface User {
  userId: number;
  fullName: string;
  userName: string;
  email: string;
  phoneNumber: string;
  role: string;
}

@Component({
  selector: 'app-viewuser',
  templateUrl: './viewuser.component.html',
  styleUrls: ['./viewuser.component.css'],
  imports: [CommonModule, NavComponent, FormsModule],
})
export class ViewUserComponent implements OnInit {
  users: User[] = [];
  filteredUsers: User[] = [];
  searchTerm: string = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(): void {
    this.http.get<User[]>('https://localhost:44326/api/User')
      .subscribe(
        data => {
          this.users = data;
          this.filteredUsers = data;
        },
        error => console.error('Error fetching user data:', error)
      );
  }

  filterUsers(): void {
    this.filteredUsers = this.users.filter(user =>
      user.userName.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }
}