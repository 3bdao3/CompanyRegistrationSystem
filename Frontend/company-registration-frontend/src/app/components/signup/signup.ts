import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CompanyService } from '../../services/company.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './signup.html',
  styleUrls: ['./signup.sass']
})
export class SignupComponent {
  signupForm: FormGroup;
  selectedFile: File | null = null;
  previewUrl: string | ArrayBuffer | null = null;
  message: string = '';

  constructor(
    private fb: FormBuilder,
    private companyService: CompanyService,
    private router: Router
  ) {
    this.signupForm = this.fb.group({
      companyArabicName: ['', Validators.required],
      companyEnglishName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [''],
      websiteUrl: ['']
    });
  }

  onFileSelected(event: any) {
    const file = event.target.files?.[0];
    if (!file) return;

    this.selectedFile = file;

    const reader = new FileReader();
    reader.onload = () => this.previewUrl = reader.result;
    reader.readAsDataURL(file);
  }

  onSubmit() {
    if (this.signupForm.invalid) {
      this.signupForm.markAllAsTouched();
      return;
    }

    if (this.selectedFile) {
      // رفع الصورة أولًا
      this.companyService.uploadLogo(this.selectedFile).subscribe({
        next: (res: any) => {
          const logoPath = res.path || res.Path; // خد أي واحد موجود
          this.sendSignup(logoPath);
        },
        error: (err) => this.message = 'Error uploading logo: ' + (err.error?.message || err.message)
      });
    } else {
      this.sendSignup();
    }
  }

  private sendSignup(logoPath?: string) {
    const payload: any = {
      ArabicName: this.signupForm.get('companyArabicName')?.value,
      EnglishName: this.signupForm.get('companyEnglishName')?.value,
      Email: this.signupForm.get('email')?.value,
      PhoneNumber: this.signupForm.get('phoneNumber')?.value,
      WebsiteUrl: this.signupForm.get('websiteUrl')?.value
    };

    if (logoPath) payload.LogoPath = logoPath;

    this.companyService.signUpCompany(payload).subscribe({
      next: () => {
        this.message = 'Company registered successfully!';

        // تخزين بيانات الشركة بعد نجاح التسجيل
        this.companyService.setCompanyData({
          EnglishName: payload.EnglishName,
          LogoPath: logoPath || null
        });

        this.router.navigate(['/validate-otp'], { state: { email: payload.Email } });
      },
      error: (err) => this.message = 'Error: ' + (err.error?.message || err.message)
    });
  }
}
