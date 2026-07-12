# Báo cáo tiến độ tuần 01

## Thời gian thực hiện
Từ ngày 06/07/2026 đến ngày 09/07/2026.

## Nội dung đã thực hiện
Trong tuần đầu tiên, em bắt đầu chuẩn bị đồ án chính thức cho môn Chuyên đề ASP.NET với đề tài “Xây dựng website quản lý chi tiêu cá nhân”.

Các công việc đã thực hiện gồm:
- Tạo repository GitHub cho đồ án.
- Sắp xếp lại cấu trúc thư mục theo yêu cầu của học phần.
- Đưa source code bản thử nghiệm vào thư mục `scr`.
- Đưa file cơ sở dữ liệu vào thư mục `setup`.
- Tạo file `README.md` ban đầu để mô tả đề tài và hướng dẫn chạy chương trình.
- Xác định các chức năng chính của website như đăng ký, đăng nhập, quản lý danh mục, quản lý giao dịch và sao lưu dữ liệu 
XML.
- Đã mời giảng viên tham gia repository GitHub để theo dõi tiến độ đồ án.
- Đã mở project bằng Visual Studio và chạy thử trên localhost.
- Đã tạo cơ sở dữ liệu `quan_ly_chi_tieu` bằng MySQL.
- Đã kiểm tra các bảng chính gồm `nguoi_dung`, `danh_muc`, `giao_dich`.
- Đã đăng ký tài khoản thử nghiệm và đăng nhập thành công.
- Đã thêm danh mục mẫu và giao dịch mẫu.
- Đã kiểm tra bảng điều khiển hiển thị đúng tổng thu, tổng chi và số dư.
- Rà soát trang quản lý danh mục và trang quản lý giao dịch.
- Bổ sung chức năng tìm kiếm tương đối ở trang Giao dịch.
- Bổ sung lọc giao dịch theo loại giao dịch và danh mục.
- Sửa lỗi nút Tìm kiếm bị dính kiểm tra dữ liệu của form thêm giao dịch.
- Kiểm tra lại chức năng thêm, sửa, xóa giao dịch sau khi bổ sung bộ lọc.
### Ngày 09/07/2026
- Rà soát trang quản lý danh mục.
- Bổ sung chức năng sửa danh mục.
- Thêm nút “Sửa” bên cạnh nút “Xóa” trong danh sách danh mục.
- Khi bấm “Sửa”, thông tin danh mục được đưa lên form để cập nhật.
- Bổ sung nút “Hủy” để quay lại chế độ thêm mới.
- Kiểm tra lại chức năng thêm, sửa, xóa danh mục.
- Rà soát trang quản lý giao dịch.
- Hoàn thiện kiểm tra dữ liệu nhập khi thêm/sửa giao dịch.
- Kiểm tra số tiền không được để trống, phải là số và phải lớn hơn 0.
- Kiểm tra ngày giao dịch không được để trống và phải hợp lệ.
- Bổ sung định dạng số tiền tự động có dấu chấm hàng nghìn khi nhập.
- Kiểm tra lại chức năng tìm kiếm, lọc, thêm, sửa, xóa giao dịch sau khi cập nhật.
### Ngày 10/07/2026
- Rà soát trang chủ/Dashboard.
- Bổ sung ô thống kê tổng số giao dịch.
- Hoàn thiện phần hiển thị tổng thu, tổng chi và số dư.
- Bổ sung danh sách 5 giao dịch gần nhất.
- Bổ sung bảng thống kê chi theo danh mục.
- Bổ sung bảng thống kê thu theo danh mục.
- Định dạng số tiền theo dạng Việt Nam để dễ theo dõi.
- Kiểm tra lại Dashboard với dữ liệu thu, chi và các danh mục khác.
### Ngày 11/07/2026
- Rà soát chức năng Sao lưu XML của website.
- Kiểm tra chức năng xuất danh sách giao dịch ra file XML.
- Kiểm tra file XML có các thông tin như danh mục, loại danh mục, số tiền, ngày giao dịch và ghi chú.
- Hoàn thiện phần khôi phục dữ liệu từ file XML.
- Bổ sung kiểm tra dữ liệu khi nhập XML để tránh lỗi dữ liệu.
- Kiểm tra trường hợp nhập lại file XML cũ để tránh bị trùng giao dịch.
- Kiểm tra trường hợp file XML sai số tiền và hệ thống báo lỗi theo dòng.
### Ngày 12/07/2026
- Rà soát tổng thể các trang chính của website.
- Kiểm tra trang đăng nhập, đăng ký và đổi mật khẩu.
- Kiểm tra lại Dashboard, danh mục, giao dịch và sao lưu XML.
- Rà soát đường dẫn menu và các liên kết giữa các trang.
- Chỉnh lại một số đường dẫn để tránh lỗi khi truy cập từ trang con.
- Cập nhật README theo các chức năng hiện tại của website.
- Chạy kiểm tra toàn bộ project và không phát hiện lỗi.

Kết quả đạt được: Website đã ổn định hơn trước khi chuyển sang giai đoạn viết báo cáo và làm slide. Các chức năng chính đã được rà soát lại, README cũng được cập nhật để phản ánh đúng chức năng hiện có của project.
## Kết quả đạt được
- Đã có cấu trúc thư mục ban đầu cho đồ án.
- Đã có source code nền để tiếp tục phát triển.
- Đã có file cơ sở dữ liệu và README ban đầu.
- Đã xác định được hướng phát triển chính thức từ ngày 06/07/2026.
- Website đã chạy được trên máy cá nhân. Các chức năng nền như đăng nhập, thêm danh mục, thêm giao dịch và xem tổng quan đã hoạt động.
- Trang Giao dịch đã có thêm chức năng tìm kiếm và lọc dữ liệu. Người dùng có thể tìm giao dịch theo tên danh mục hoặc ghi chú, đồng thời lọc theo loại giao dịch và danh mục.
- Trang Danh mục đã có đủ thao tác thêm, sửa, xóa. Ô nhập số tiền dễ nhìn hơn khi nhập các khoản tiền lớn.
- Trang Dashboard hiển thị rõ tình hình thu chi của người dùng, gồm tổng thu, tổng chi, số dư, tổng số giao dịch, giao dịch gần đây và thống kê theo danh mục.
- Chức năng Sao lưu XML đã hoạt động ổn hơn. Người dùng có thể xuất dữ liệu giao dịch ra file XML và khôi phục lại khi cần. Khi nhập dữ liệu, hệ thống có kiểm tra lỗi cơ bản và bỏ qua các giao dịch đã tồn tại để tránh bị trùng dữ liệu.
- Website đã ổn định hơn trước khi chuyển sang giai đoạn viết báo cáo và làm slide. Các chức năng chính đã được rà soát lại, README cũng được cập nhật để phản ánh đúng chức năng hiện có của project.
## Công việc dự kiến tiếp theo
- Kiểm tra lại việc chạy project trên Visual Studio.
- Kiểm tra kết nối cơ sở dữ liệu.
- Rà soát chức năng đăng ký, đăng nhập.
- Hoàn thiện chức năng quản lý danh mục và giao dịch.
- Bổ sung tìm kiếm, lọc dữ liệu và thống kê nếu còn thiếu.

## Ghi chú
Các phần code trước ngày 06/07/2026 được xem là bản thử nghiệm. Từ ngày 06/07/2026, đồ án bắt đầu được quản lý chính thức trên GitHub để phục vụ báo cáo môn học.