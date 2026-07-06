# Xây dựng website quản lý chi tiêu cá nhân

## 1. Thông tin đồ án

* **Môn học:** Chuyên đề ASP.NET
* **Tên đề tài:** Xây dựng website quản lý chi tiêu cá nhân
* **Sinh viên thực hiện:** Nguyễn Thanh Duy Lâm
* **Lớp:** DX23TT9
* **Thời gian thực hiện chính thức:** 06/07/2026 - 17/07/2026

## 2. Mô tả đề tài

Website quản lý chi tiêu cá nhân được xây dựng nhằm hỗ trợ người dùng ghi nhận, quản lý và theo dõi các khoản thu, chi trong đời sống hằng ngày. Hệ thống giúp người dùng quản lý danh mục thu chi, thêm sửa xóa giao dịch, xem tổng thu, tổng chi, số dư và sao lưu dữ liệu bằng XML.

## 3. Công nghệ sử dụng

* ASP.NET Web Forms
* C#
* .NET Framework 4.8
* ADO.NET
* MySQL
* MySql.Data
* HTML, CSS
* ASP.NET Server Controls
* GridView
* Forms Authentication
* XML

## 4. Chức năng chính

* Đăng ký tài khoản
* Đăng nhập, đăng xuất
* Đổi mật khẩu
* Quản lý danh mục thu, chi
* Quản lý giao dịch thu, chi
* Xem bảng điều khiển tổng quan
* Thống kê tổng thu, tổng chi và số dư
* Xuất dữ liệu XML
* Nhập dữ liệu XML

## 5. Cấu trúc thư mục

```text
ASPNET-dx23tt9-nguyenthanhduylam-webquanlichitieu
│
├── README.md
│
├── scr
│   └── quanlychitieu
│       ├── Account
│       ├── App_Code
│       ├── App_Data
│       ├── Assets
│       ├── Bin
│       ├── Pages
│       ├── Default.aspx
│       ├── Site.Master
│       └── Web.config
│
├── setup
│   └── database.sql
│
├── progress-report
│
└── thesis
```

## 6. Hướng dẫn cài đặt và chạy chương trình

### Bước 1: Chuẩn bị môi trường

Cài đặt các phần mềm cần thiết:

* Visual Studio
* .NET Framework 4.8
* MySQL Server
* MySQL Connector/NET

### Bước 2: Tạo cơ sở dữ liệu

Mở MySQL và chạy file:

```text
setup/database.sql
```

### Bước 3: Cấu hình chuỗi kết nối

Mở file:

```text
scr/quanlychitieu/Web.config
```

Cập nhật chuỗi kết nối cơ sở dữ liệu cho phù hợp với máy đang chạy.

### Bước 4: Mở project trong Visual Studio

Vì project thuộc dạng ASP.NET Web Site, mở bằng:

```text
File -> Open -> Web Site...
```

Sau đó chọn thư mục:

```text
scr/quanlychitieu
```

### Bước 5: Chạy website

Nhấn:

```text
Ctrl + F5
```

hoặc chọn:

```text
Start Without Debugging
```

## 7. Ghi chú tiến độ

Từ ngày 06/07/2026, đồ án được bắt đầu thực hiện chính thức để phục vụ báo cáo môn học Chuyên đề ASP.NET. Source code trước đó được xem là bản thử nghiệm ban đầu và được dùng làm nền để phát triển phiên bản chính thức.

## 8. Link báo cáo và video demo

Nội dung này sẽ được cập nhật sau khi hoàn thiện báo cáo Word, PowerPoint và video demo.
