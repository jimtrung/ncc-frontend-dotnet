[🇺🇸 English](README.md) | [🇻🇳 Tiếng Việt](README.vi.md)

# Hệ thống Quản lý Rạp chiếu phim - Frontend

Frontend cho Hệ thống Quản lý Rạp chiếu phim là một Ứng dụng Desktop WPF được xây dựng với .NET 8.0.

## 🛠 Yêu cầu tiên quyết

- **[.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)**
- **[Visual Studio 2022](https://visualstudio.microsoft.com/)** (Khuyên dùng) hoặc **[Visual Studio Code](https://code.visualstudio.com/)**

## 🚀 Cài đặt & Thiết lập

1.  Đảm bảo **Backend** đang chạy.
2.  Mở terminal tại thư mục này.
3.  Khôi phục các gói phụ thuộc:
    ```bash
    dotnet restore
    ```
4.  Chạy ứng dụng:
    ```bash
    dotnet run
    ```

## 🏗 Cấu trúc Dự án

- **Views**: Chứa các file XAML cho giao diện người dùng.
- **Controllers**: Xử lý logic và tương tác cho từng view.
- **Services**: Quản lý giao tiếp API với backend.
- **Models**: Các mô hình dữ liệu được sử dụng trong ứng dụng.
