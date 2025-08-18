import { Routes } from '@angular/router';
import { SignupComponent } from './components/signup/signup';
import { ValidateOtpComponent } from './components/validate-otp/validate-otp';
import { SetPasswordComponent } from './components/set-password/set-password';
import { HomeComponent } from './components/home/home';
import { LoginComponent } from './components/login/login'; 
export const routes: Routes = [
  { path: '', redirectTo: 'signup', pathMatch: 'full' },
  { path: 'signup', component: SignupComponent },
  { path: 'validate-otp', component: ValidateOtpComponent },
  { path: 'set-password', component: SetPasswordComponent },
  { path: 'login', component: LoginComponent }, 
  { path: 'home', component: HomeComponent }
];
