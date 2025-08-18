import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CompanyService } from '../../services/company.service'; 

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule], // ✅ ضيف دول
  templateUrl: './signup.html',
  styleUrls: ['./signup.sass']
})
export class SignupComponent {
  signupForm: FormGroup;
  selectedFile: File | null = null;
  message: string = '';

  constructor(private fb: FormBuilder, private companyService: CompanyService) {
    this.signupForm = this.fb.group({
      companyArabicName: ['', Validators.required],
      companyEnglishName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [''],
      websiteUrl: [''],
      logo: [null]
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
    this.signupForm.patchValue({ logo: this.selectedFile });
  }

  onSubmit() {
    if (this.signupForm.valid) {
      const formData = new FormData();
      formData.append('companyArabicName', this.signupForm.get('companyArabicName')?.value);
      formData.append('companyEnglishName', this.signupForm.get('companyEnglishName')?.value);
      formData.append('email', this.signupForm.get('email')?.value);
      formData.append('phoneNumber', this.signupForm.get('phoneNumber')?.value);
      formData.append('websiteUrl', this.signupForm.get('websiteUrl')?.value);

      if (this.selectedFile) {
        formData.append('logo', this.selectedFile);
      }

      this.companyService.signUpCompany(formData).subscribe({
        next: (res) => this.message = 'Company registered successfully!',
        error: (err) => this.message = 'Error: ' + err.message
      });
    }
  }
}
