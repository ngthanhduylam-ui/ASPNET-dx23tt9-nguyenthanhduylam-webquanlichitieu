using System;
using System.Data;
using System.Globalization;
using System.Web.Security;
using MySql.Data.MySqlClient;

public partial class XmlSync : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private string GetUserId()
    {
        FormsIdentity id = (FormsIdentity)User.Identity;
        return id.Ticket.UserData;
    }

    // TÍNH NĂNG XUẤT XML (Chuyển Data từ ADO.NET sang XML)
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string query = @"
            SELECT d.ten_danh_muc, d.loai_danh_muc, g.so_tien, g.ngay_giao_dich, g.ghi_chu
            FROM giao_dich g
            INNER JOIN danh_muc d ON g.ma_danh_muc = d.ma_danh_muc
            WHERE g.ma_nguoi_dung = @UserId AND d.ma_nguoi_dung = @UserId
            ORDER BY g.ngay_giao_dich ASC";

        MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@UserId", GetUserId()) };
        DataSet ds = Database.GetDataSet(query, parameters);

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/xml";
            Response.AddHeader("content-disposition", "attachment;filename=LichSuGiaoDich.xml");
            Response.Charset = "utf-8";
            
            // Lệnh WriteXml chuyển trực tiếp Dataset thành file XML
            ds.WriteXml(Response.OutputStream);
            Response.End();
        }
        else
        {
            ShowMessage("Bạn chưa có giao dịch nào để xuất.", false);
        }
    }

    // TÍNH NĂNG NHẬP XML (Chuyển XML về DataSet và chèn vào Database qua ADO.NET)
    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (fileUploadXml.HasFile)
        {
            try
            {
                if (System.IO.Path.GetExtension(fileUploadXml.FileName).ToLower() != ".xml")
                {
                    ShowMessage("Vui lòng chọn file định dạng .xml!", false);
                    return;
                }

                DataSet ds = new DataSet();
                // Đọc file XML được upload lên vào DataSet
                ds.ReadXml(fileUploadXml.PostedFile.InputStream);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int count = 0;
                    int duplicateCount = 0;
                    string userId = GetUserId();
                    DataTable table = ds.Tables[0];

                    string missingColumn = GetMissingColumnName(table);
                    if (!string.IsNullOrEmpty(missingColumn))
                    {
                        ShowMessage("File XML thiếu cột: " + missingColumn + ".", false);
                        return;
                    }

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow row = table.Rows[i];
                        string tenDanhMuc;
                        string loaiDanhMuc;
                        decimal soTien;
                        DateTime ngayGiaoDich;
                        string ghiChu;
                        string errorMessage;

                        if (!TryReadTransactionRow(row, i + 1, out tenDanhMuc, out loaiDanhMuc, out soTien, out ngayGiaoDich, out ghiChu, out errorMessage))
                        {
                            ShowMessage(errorMessage, false);
                            return;
                        }

                        // 1. Kiểm tra xem danh mục đã tồn tại chưa, nếu chưa thì tạo mới cho user hiện tại
                        int categoryId = GetOrCreateCategoryId(tenDanhMuc, loaiDanhMuc, userId);

                        // 2. Kiểm tra xem giao dịch đã tồn tại chưa để tránh trùng lặp
                        if (!IsTransactionExists(soTien, ngayGiaoDich, ghiChu, categoryId, userId))
                        {
                            // 3. Chèn giao dịch
                            InsertTransaction(soTien, ngayGiaoDich, ghiChu, categoryId, userId);
                            count++;
                        }
                        else
                        {
                            duplicateCount++;
                        }
                    }

                    ShowMessage("Đã phục hồi thành công " + count + " giao dịch. Bỏ qua " + duplicateCount + " giao dịch đã tồn tại.", true);
                }
                else
                {
                    ShowMessage("File XML không có dữ liệu hợp lệ.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi đọc file XML: " + ex.Message, false);
            }
        }
        else
        {
            ShowMessage("Vui lòng chọn một file XML để tải lên.", false);
        }
    }

    private string GetMissingColumnName(DataTable table)
    {
        string[] requiredColumns = { "ten_danh_muc", "loai_danh_muc", "so_tien", "ngay_giao_dich", "ghi_chu" };

        foreach (string columnName in requiredColumns)
        {
            if (!table.Columns.Contains(columnName))
            {
                return columnName;
            }
        }

        return "";
    }

    private bool TryReadTransactionRow(DataRow row, int rowNumber, out string tenDanhMuc, out string loaiDanhMuc, out decimal soTien, out DateTime ngayGiaoDich, out string ghiChu, out string errorMessage)
    {
        tenDanhMuc = row["ten_danh_muc"].ToString().Trim();
        loaiDanhMuc = NormalizeCategoryType(row["loai_danh_muc"].ToString());
        ghiChu = row["ghi_chu"].ToString().Trim();
        soTien = 0;
        ngayGiaoDich = DateTime.MinValue;
        errorMessage = "";

        if (string.IsNullOrEmpty(tenDanhMuc))
        {
            errorMessage = "Dòng " + rowNumber + ": Tên danh mục không được để trống.";
            return false;
        }

        if (string.IsNullOrEmpty(loaiDanhMuc))
        {
            errorMessage = "Dòng " + rowNumber + ": Loại danh mục không hợp lệ. Chỉ nhận 'thu' hoặc 'chi'.";
            return false;
        }

        if (!TryParseAmount(row["so_tien"].ToString(), out soTien))
        {
            errorMessage = "Dòng " + rowNumber + ": Số tiền không hợp lệ.";
            return false;
        }

        if (soTien <= 0)
        {
            errorMessage = "Dòng " + rowNumber + ": Số tiền phải lớn hơn 0.";
            return false;
        }

        if (!DateTime.TryParse(row["ngay_giao_dich"].ToString(), out ngayGiaoDich))
        {
            errorMessage = "Dòng " + rowNumber + ": Ngày giao dịch không hợp lệ.";
            return false;
        }

        return true;
    }

    private bool TryParseAmount(string value, out decimal amount)
    {
        if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
        {
            return true;
        }

        return decimal.TryParse(value, out amount);
    }

    private string NormalizeCategoryType(string type)
    {
        string value = type.Trim().ToLower();

        if (value == "thu" || value == "thu nhập" || value == "thu nhap")
        {
            return "thu";
        }

        if (value == "chi" || value == "chi tiêu" || value == "chi tieu")
        {
            return "chi";
        }

        return "";
    }

    private int GetOrCreateCategoryId(string tenDanhMuc, string loaiDanhMuc, string userId)
    {
        string checkCatQuery = "SELECT ma_danh_muc FROM danh_muc WHERE ten_danh_muc = @Ten AND loai_danh_muc = @Loai AND ma_nguoi_dung = @UserId";
        MySqlParameter[] checkParams = {
            new MySqlParameter("@Ten", tenDanhMuc),
            new MySqlParameter("@Loai", loaiDanhMuc),
            new MySqlParameter("@UserId", userId)
        };

        object catResult = Database.ExecuteScalar(checkCatQuery, checkParams);

        if (catResult != null)
        {
            return Convert.ToInt32(catResult);
        }

        string insertCatQuery = "INSERT INTO danh_muc (ten_danh_muc, loai_danh_muc, ma_nguoi_dung) VALUES (@Ten, @Loai, @UserId); SELECT LAST_INSERT_ID();";
        MySqlParameter[] insertCatParams = {
            new MySqlParameter("@Ten", tenDanhMuc),
            new MySqlParameter("@Loai", loaiDanhMuc),
            new MySqlParameter("@UserId", userId)
        };

        return Convert.ToInt32(Database.ExecuteScalar(insertCatQuery, insertCatParams));
    }

    private bool IsTransactionExists(decimal soTien, DateTime ngayGiaoDich, string ghiChu, int categoryId, string userId)
    {
        string checkTransQuery = "SELECT COUNT(*) FROM giao_dich WHERE so_tien = @SoTien AND ngay_giao_dich = @Ngay AND ghi_chu = @GhiChu AND ma_danh_muc = @MaDanhMuc AND ma_nguoi_dung = @UserId";
        MySqlParameter[] parameters = {
            new MySqlParameter("@SoTien", soTien),
            new MySqlParameter("@Ngay", ngayGiaoDich),
            new MySqlParameter("@GhiChu", ghiChu),
            new MySqlParameter("@MaDanhMuc", categoryId),
            new MySqlParameter("@UserId", userId)
        };

        int count = Convert.ToInt32(Database.ExecuteScalar(checkTransQuery, parameters));
        return count > 0;
    }

    private void InsertTransaction(decimal soTien, DateTime ngayGiaoDich, string ghiChu, int categoryId, string userId)
    {
        string insertTransQuery = "INSERT INTO giao_dich (so_tien, ngay_giao_dich, ghi_chu, ma_danh_muc, ma_nguoi_dung) VALUES (@SoTien, @Ngay, @GhiChu, @MaDanhMuc, @UserId)";
        MySqlParameter[] parameters = {
            new MySqlParameter("@SoTien", soTien),
            new MySqlParameter("@Ngay", ngayGiaoDich),
            new MySqlParameter("@GhiChu", ghiChu),
            new MySqlParameter("@MaDanhMuc", categoryId),
            new MySqlParameter("@UserId", userId)
        };

        Database.ExecuteNonQuery(insertTransQuery, parameters);
    }

    private void ShowMessage(string msg, bool isSuccess)
    {
        lblMessage.Text = msg;
        lblMessage.CssClass = isSuccess ? "text-success" : "text-danger";
        lblMessage.Visible = true;
    }
}
