import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CompanyService } from '../../services/company.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-set-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './set-password.html',
  styleUrls: ['./set-password.sass']
})
export class SetPasswordComponent {
  passwordForm: FormGroup;
  message: string = '';
  email: string = '';
  otp: string = '';

  constructor(private fb: FormBuilder, private companyService: CompanyService, private router: Router) {
    // ➡ ناخد الايميل والـ OTP من sessionStorage بعد Validate OTP
    this.email = sessionStorage.getItem('verifiedEmail') || '';
    this.otp = sessionStorage.getItem('verifiedOtp') || '';

    this.passwordForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordsMatch });
  }

  // تحقق من تطابق الباسوردات
  passwordsMatch(group: FormGroup) {
    const pass = group.get('newPassword')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return pass === confirm ? null : { mismatch: true };
  }

  onSubmit() {
    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();
      return;
    }

    const payload = {
      email: this.email, 
      otp: this.otp, // ➡ مهم جداً للـ API
      newPassword: this.passwordForm.get('newPassword')?.value,
      confirmPassword: this.passwordForm.get('confirmPassword')?.value
    };

    this.companyService.setPassword(payload).subscribe({
      next: () => {
        this.message = 'Password set successfully!';
        sessionStorage.removeItem('verifiedEmail');
        sessionStorage.removeItem('verifiedOtp');
        this.router.navigate(['/login']); 
      },
      error: (err) => this.message = 'Error: ' + err.message
    });
  }
}
