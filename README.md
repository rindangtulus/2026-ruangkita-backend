# ⚙️ RuangKita - Backend API

## 🚀 Fitur Backend
- **Smart Validation Logic**: Algoritma pengecekan ketersediaan ruangan secara real-time untuk mencegah bentrok jadwal.
- **Role-Based Authorization**: Pembedaan hak akses antara Admin (Approval) dan User (Request).
- **Status Tracking & History**: Pencatatan otomatis setiap perubahan status (Pending → Approved/Rejected) untuk audit log.
- **DTO Pattern**: Implementasi Data Transfer Object untuk pertukaran data yang aman antara React dan API.
- **Relational Database**: Manajemen hubungan antara data User, Room, Borrowing, dan StatusHistory.

## 🛠️ Tech Stack
- **Framework**: .NET 8 (ASP.NET Core Web API)
- **Language**: C#
- **ORM**: Entity Framework Core
- **Database**: SQL Server / PostgreSQL
- **Tools**: Postman (untuk testing API), dotnet-ef (untuk migrasi database)

## 🏗️ Struktur Database (Models)
- `User`: Identitas pengakses (Admin/User).
- `Room`: Data ruangan (Kapasitas & Fasilitas).
- `Borrowing`: Data transaksi peminjaman.
- `StatusHistory`: Catatan riwayat perubahan status peminjaman.

## 🚀 Cara Menjalankan Project

1. **Clone repositori**
   ```bash
   git clone [https://github.com/username/ruangkita-backend.git](https://github.com/username/ruangkita-backend.git)
   
2. **Update Database**
   ```bash
   dotnet ef database update
3. **Jalankan API**
   ```bash
   dotnet run

