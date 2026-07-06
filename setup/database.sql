CREATE DATABASE IF NOT EXISTS quan_ly_chi_tieu CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE quan_ly_chi_tieu;

-- Bảng người dùng
CREATE TABLE IF NOT EXISTS nguoi_dung (
    ma_nguoi_dung INT AUTO_INCREMENT PRIMARY KEY,
    ten_dang_nhap VARCHAR(50) NOT NULL UNIQUE,
    mat_khau VARCHAR(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Bảng danh mục
CREATE TABLE IF NOT EXISTS danh_muc (
    ma_danh_muc INT AUTO_INCREMENT PRIMARY KEY,
    ten_danh_muc VARCHAR(100) NOT NULL,
    loai_danh_muc VARCHAR(20) NOT NULL, -- 'thu' hoặc 'chi'
    ma_nguoi_dung INT NOT NULL,
    FOREIGN KEY (ma_nguoi_dung) REFERENCES nguoi_dung(ma_nguoi_dung) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Bảng giao dịch
CREATE TABLE IF NOT EXISTS giao_dich (
    ma_giao_dich INT AUTO_INCREMENT PRIMARY KEY,
    so_tien DECIMAL(18,2) NOT NULL,
    ngay_giao_dich DATETIME NOT NULL,
    ghi_chu TEXT,
    ma_danh_muc INT NOT NULL,
    ma_nguoi_dung INT NOT NULL,
    FOREIGN KEY (ma_danh_muc) REFERENCES danh_muc(ma_danh_muc) ON DELETE CASCADE,
    FOREIGN KEY (ma_nguoi_dung) REFERENCES nguoi_dung(ma_nguoi_dung) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Dữ liệu mẫu (Tùy chọn)
-- INSERT INTO nguoi_dung (ten_dang_nhap, mat_khau) VALUES ('admin', 'admin123');
