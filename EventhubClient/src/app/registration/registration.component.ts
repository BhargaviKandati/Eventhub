import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../services/auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UsernavComponent } from "../usernav/usernav.component";

@Component({
  selector: 'app-registration',
  standalone: true,
  imports:[UsernavComponent,CommonModule, ReactiveFormsModule],
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
 })
 
export class RegistrationComponent {
onLogin() {
throw new Error('Method not implemented.');
}
  registrationForm: FormGroup;
  errormessage: string='';

  constructor(private authService: AuthService, private router: Router, private fb: FormBuilder) {
    this.registrationForm = this.fb.group({
      UserName: ['', Validators.required],
      Fullname: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]],
      PhoneNumber: ['', Validators.required],
      Password: ['', Validators.required],
      role: ['User', Validators.required]
    });
  }

  onRegister(): void {
    if (this.registrationForm.valid) {
      console.log(this.registrationForm.value);
      this.authService.register(this.registrationForm.value).subscribe(
        response => {
          console.log('Registration successful', response);
          this.router.navigate(['/app-login']);
        },
        error => {
          console.error('Registration failed', error);
        }
      );
    }
  }
}