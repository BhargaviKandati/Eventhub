import { Routes } from '@angular/router';
import { RegistrationComponent } from './registration/registration.component';
import { LoginComponent } from './login/login.component';
import { AboutComponent } from './about/about.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { AppComponent } from './app.component';
import { CreateeventComponent } from './createevent/createevent.component';
import { UsernavComponent } from './usernav/usernav.component';
import { AdminLoginComponent } from './admin-login/admin-login.component';
import { ContactComponent } from './contact/contact.component';
import { PaymentComponent } from './payment/payment.component';
import { ManageBookingsComponent } from './managebookings/managebookings.component';
import { ManageCategoryComponent } from './manage-category/manage-category.component';
import { PaymentsComponent } from './payments/payments.component';
import { EventsComponent } from './events/events.component';
import { BookingComponent } from './booking/booking.component';
import { ViewUserComponent } from './viewuser/viewuser.component';
import { TicketsComponent } from './tickets/tickets.component';
export const routes: Routes = [
    {path:'app-registration',component:RegistrationComponent},
    {path:'app-login',component:LoginComponent},
    {path:'app-about',component:AboutComponent},
    {path:'app-nav',component:NavComponent},
    {path:'app-home',component:HomeComponent},
    {path:'app-createevent',component:CreateeventComponent},
    {path:'app-usernav',component:UsernavComponent},
    {path:'app-adminlogin',component:AdminLoginComponent},
    {path:'app-contact',component:ContactComponent},
    { path: 'app-payment/:eventId/:price/:bookingId/:ticketId', component: PaymentComponent },
    {path:'app-managebookings',component:ManageBookingsComponent},
    {path:'app-manage-category',component:ManageCategoryComponent},
    {path:'app-payments',component:PaymentsComponent},
    {path:'app-events',component:EventsComponent},
    {path: 'app-booking/:eventId',component:BookingComponent},
    {path: 'app-viewuser',component:ViewUserComponent },
    {path: 'app-tickets/:eventId/:bookingId',component:TicketsComponent},
    {path:'', redirectTo:'/app-home', pathMatch:'full'}
];
