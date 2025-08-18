import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompanyService } from '../../services/company.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.html',
  styleUrls: ['./home.sass']
})
export class HomeComponent implements OnInit {
  companyData: any = null;

  constructor(private companyService: CompanyService, private router: Router) {}

  ngOnInit(): void {
    // ➡ نجيب البيانات من الـ service مباشرة
    this.companyData = this.companyService.getCompanyData();

    // لو مفيش بيانات، ارجع للصفحة الرئيسية
    if (!this.companyData) {
      this.router.navigate(['/signup']);
    }
  }

  logout() {
    // ➡ مسح بيانات الشركة و token
    this.companyService.logout();
    sessionStorage.clear();
    this.router.navigate(['/signup']);
  }
}
