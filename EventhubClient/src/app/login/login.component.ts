import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { FormsModule } from '@angular/forms';
import { jwtDecode } from 'jwt-decode';
import { UsernavComponent } from '../usernav/usernav.component';
 
@Component({
  selector: 'app-login',
  imports: [FormsModule,UsernavComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  // styleUrls: ['./user-login.component.css'],
  standalone: true
})
export class LoginComponent {
  username: string = '';
  password: string = '';
 
  constructor(private authService: AuthService, private router: Router) {}
 
  onLogin(): void {
    const loginData = { username: this.username, password: this.password };
    console.log(loginData);
    this.authService.login(loginData).subscribe(
      response => {
        console.log('Login successful', response);
        const token = response.token;
        const decodedToken: any = jwtDecode(token);
        console.log('Decoded Token:', decodedToken); // Log the decoded token
 
        const userRole = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        console.log('User Role:', userRole); // Log the user role
 
        if (userRole === 'User') {
          // Navigate to user menu if the user is a regular user
          this.router.navigate(['/app-events']);
        } else {
          // Show error message if the user is not a regular user
          console.error('Permissions not granted. Only regular users can log in.');
          alert('Permissions not granted. Only regular users can log in.');
        }
      },
      error => {
        console.error('Login failed', error);
        // Show error message
        alert('Login failed. Please check your credentials and try again.');
      }
    );
  }
}