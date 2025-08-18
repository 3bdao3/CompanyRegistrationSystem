import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CompanyService } from '../../services/company.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-validate-otp',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './validate-otp.html',
  styleUrls: ['./validate-otp.sass']
})
export class ValidateOtpComponent {
  otpForm: FormGroup;
  message: string = '';
  email: string = ''; // ➡ نخزن الإيميل هنا

  constructor(private fb: FormBuilder, private companyService: CompanyService, private router: Router) {
    // ناخد الإيميل من الـ state اللي جاي من SignUp
    const navigation = this.router.getCurrentNavigation();
    this.email = navigation?.extras.state?.['email'] || '';

    this.otpForm = this.fb.group({
      otp: ['', Validators.required] // ➡ بس OTP
    });
  }

  onSubmit() {
    if (this.otpForm.valid) {
      const payload = {
        email: this.email,
        otp: this.otpForm.get('otp')?.value
      };

      this.companyService.validateOtp(payload).subscribe({
        next: () => {
          this.message = 'OTP verified successfully!';
          // ➡ نخزن الايميل والـ OTP في sessionStorage عشان SetPassword يستخدمهم
          sessionStorage.setItem('verifiedEmail', this.email);
          sessionStorage.setItem('verifiedOtp', this.otpForm.get('otp')?.value);
          this.router.navigate(['/set-password']); // ➡ بعد التحقق نروح SetPassword
        },
        error: (err) => this.message = 'Error: ' + err.message
      });
    }
  }
}
