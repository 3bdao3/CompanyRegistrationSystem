Tech Stack
🔹 Backend

.NET 9 Web API (C#)

Entity Framework Core 9 (Code First with PostgreSQL)

Repository & Unit of Work Pattern

Service Layer + DTOs + AutoMapper

JWT Authentication

BCrypt Password Hashing

🔹 Frontend

Angular 16+

TypeScript

Angular Services & Interceptors (JWT handling)

Responsive UI with CSS / Angular bindings

🔹 Database

PostgreSQL

📂 Project Structure
CompanyRegistrationSystem/
│── API/                 # Presentation Layer (Controllers, Endpoints)
│── Application/          # DTOs, Services, Interfaces, Validators
│── Domain/               # Entities (Company, OTP, etc.)
│── Infrastructure/       # DbContext, Repositories, Configurations
│── Angular-Frontend/     # Angular 16+ client (Login, Signup, Home)

🔑 Features

✔ Company Signup with English & Arabic Name + Logo Upload
✔ OTP Verification for email confirmation
✔ Password Setup (BCrypt for hashing)
✔ JWT Authentication (Login + Logout)
✔ Protected Home Page showing:

✅ Company English Name (next to "Hello")

✅ Company Logo (top-left corner)
✔ Clean code with SOLID principles & layered architecture

⚡ API Endpoints
Endpoint	Method	Description
/api/company/signup	POST	Register a new company
/api/company/verify-otp	POST	Verify OTP for company email
/api/company/set-password	POST	Setup password after OTP
/api/company/login	POST	Login and receive JWT token
/api/company/home	GET	Get company details (Protected, requires JWT)
🖼 Home Page (After Login)

Top Left → Company Logo

Center → “Hello, [Company English Name]”

Logout Button → Top Right

▶ How to Run
🔹 Backend (.NET API)

Clone the repository

git clone https://github.com/your-username/company-registration-system.git


Navigate to API project folder and update appsettings.json with PostgreSQL connection string.

Run migrations & update database:

dotnet ef database update


Run the backend:

dotnet run

🔹 Frontend (Angular)

Navigate to Angular project folder:

cd Angular-Frontend


Install dependencies:

npm install


Run Angular app:

ng serve -o

📸 Demo Flow

Signup with company details + upload logo

Verify OTP (via email simulation or service)

Set Password

Login with JWT

Redirect to Home Page showing company logo + name

🏆 Professional Practices Used

Clean Architecture (Separation of Concerns)

Repository & Unit of Work Patterns

DTOs + AutoMapper for mapping

Secure Authentication (JWT + BCrypt)

Angular Services & State Management for token handling

Reusable components for UI
