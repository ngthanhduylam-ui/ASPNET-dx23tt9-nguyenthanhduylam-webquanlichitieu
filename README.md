# Website quản lý chi tiêu cá nhân

## Thông tin

* Môn học: Chuyên đề ASP.NET
* Đề tài: Xây dựng website quản lý chi tiêu cá nhân
* Sinh viên: Nguyễn Thanh Duy Lâm
* MSSV : 170123121
* Lớp: DX23TT9
* Thời gian làm chính thức: 06/07/2026 - 17/07/2026

## Giới thiệu

Đây là đồ án môn Chuyên đề ASP.NET. Website được làm để quản lý các khoản thu, chi cá nhân. Người dùng có thể đăng ký tài khoản, đăng nhập, thêm danh mục, thêm giao dịch, xem tổng thu, tổng chi, số dư và sao lưu dữ liệu XML.

Source code trước ngày 06/07/2026 được xem là bản thử nghiệm. Từ ngày 06/07/2026, project được đưa lên GitHub để quản lý chính thức trong quá trình làm đồ án.

## Công nghệ sử dụng

* ASP.NET Web Forms
* C#
* .NET Framework 4.8
* ADO.NET
* MySQL
* MySql.Data
* HTML, CSS
* GridView
* Forms Authentication
* XML

## Chức năng hiện có

* Đăng ký tài khoản
* Đăng nhập
* Đăng xuất
* Đổi mật khẩu
* Quản lý danh mục thu, chi
  * Thêm danh mục
  * Sửa danh mục
  * Xóa danh mục
* Quản lý giao dịch thu, chi
  * Thêm giao dịch
  * Sửa giao dịch
  * Xóa giao dịch
  * Tìm kiếm giao dịch theo danh mục hoặc ghi chú
  * Lọc giao dịch theo loại và danh mục
* Kiểm tra dữ liệu nhập khi thêm/sửa giao dịch
* Định dạng số tiền có dấu chấm hàng nghìn khi nhập
* Xem tổng thu, tổng chi và số dư
* Xem tổng số giao dịch
* Xem 5 giao dịch gần nhất
* Thống kê thu theo danh mục
* Thống kê chi theo danh mục
* Xuất dữ liệu giao dịch ra file XML
* Nhập dữ liệu giao dịch từ file XML

## Cấu trúc thư mục

```text
ASPNET-dx23tt9-nguyenthanhduylam-webquanlichitieu
│
├── README.md
├── progress-report
│   └── Tuan1.md
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
└── thesis
```

## Cách chạy project

### 1. Chuẩn bị

Cần cài:

* Visual Studio
* .NET Framework 4.8
* MySQL Server
* MySQL Connector/NET

### 2. Tạo database

Chạy file SQL trong thư mục:

```text
setup/database.sql
```

### 3. Sửa chuỗi kết nối

Mở file:

```text
scr/quanlychitieu/Web.config
```

Sau đó sửa thông tin kết nối database cho đúng với máy đang chạy.

### 4. Mở project

Project này là dạng ASP.NET Web Site, nên mở bằng Visual Studio như sau:

```text
File -> Open -> Web Site...
```

Chọn thư mục:

```text
scr/quanlychitieu
```

### 5. Chạy website

Nhấn:

```text
Ctrl + F5
```

hoặc chọn:

```text
Start Without Debugging
```

## Ghi chú

Project được hoàn thiện trong thời gian làm đồ án từ ngày 06/07/2026 đến 17/07/2026. Hiện tại website đã có các chức năng chính phục vụ quản lý thu chi cá nhân, gồm quản lý danh mục, quản lý giao dịch, tìm kiếm/lọc, thống kê và sao lưu XML.
