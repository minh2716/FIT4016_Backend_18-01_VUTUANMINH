# Ứng dụng Quản lý Trường học và Học sinh

Đây là ứng dụng console được xây dựng bằng C# và Entity Framework Core
nhằm quản lý danh sách Trường học (Schools) và Học sinh (Students).

## Mô tả
Ứng dụng sử dụng Entity Framework theo mô hình Code First để tạo và quản lý
cơ sở dữ liệu SchoolManagement gồm hai bảng:
- schools (Trường học)
- students (Học sinh)

Hai bảng có quan hệ một – nhiều (một trường học có thể có nhiều học sinh).

## Chức năng
- Thêm mới học sinh (Create)
- Hiển thị danh sách học sinh có phân trang (Read)
- Cập nhật thông tin học sinh (Update)
- Xóa học sinh (Delete)
- Kiểm tra dữ liệu đầu vào (validation)
- Hiển thị thông báo lỗi thân thiện bằng Tiếng Anh
- Tạo dữ liệu mẫu cho hệ thống

## Cơ sở dữ liệu
- Tên CSDL: SchoolManagement

### Bảng schools
- Id (Primary Key)
- Name (Unique, Not Null)
- Principal (Not Null)
- Address (Not Null)
- CreatedAt
- UpdatedAt

### Bảng students
- Id (Primary Key)
- StudentId (Unique, Not Null)
- FullName (Not Null)
- Email (Unique, Not Null)
- Phone (Nullable)
- SchoolId (Foreign Key)
- CreatedAt
- UpdatedAt

## Công nghệ sử dụng
- Ngôn ngữ: C#
- Nền tảng: .NET 6
- ORM: Entity Framework Core
- Cơ sở dữ liệu: SQL Server

## Hướng dẫn chạy chương trình
1. Mở project bằng Visual Studio
2. Kiểm tra hoặc chỉnh sửa chuỗi kết nối trong file `SchoolDbContext.cs`
3. Chạy chương trình
4. Cơ sở dữ liệu và dữ liệu mẫu sẽ được tạo tự động

## Cấu trúc thư mục
