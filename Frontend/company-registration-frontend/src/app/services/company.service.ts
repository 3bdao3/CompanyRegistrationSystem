import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private apiUrl = environment.apiUrl + '/Company';
  private tokenKey = 'token';
  private companyDataKey = 'companyData'; // ➡ مفتاح تخزين بيانات الشركة

  constructor(private http: HttpClient) {}

  // ➡ Headers مع التوكن
  private getAuthHeaders(): HttpHeaders {
    const token = this.getToken();
    return token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
  }

  // ✅ Upload Logo
  uploadLogo(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.apiUrl}/upload-logo`, formData);
  }

  // ✅ SignUp
  signUpCompany(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/signup`, data);
  }

  // ✅ Validate OTP
  validateOtp(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/validate-otp`, data);
  }

  // ✅ Set Password
  setPassword(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/set-password`, data);
  }

  // ✅ Login
  login(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, data);
  }

  // ✅ Home (Protected endpoint)
  home(): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.get(`${this.apiUrl}/home`, { headers });
  }

  // ✅ Token Handling
  setToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  setCompanyData(data: any) {
    localStorage.setItem(this.companyDataKey, JSON.stringify(data));
  }

  getCompanyData(): any {
    const data = localStorage.getItem(this.companyDataKey);
    return data ? JSON.parse(data) : null;
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.companyDataKey);
  }
}
