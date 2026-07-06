using System;
using System.Data;
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
            WHERE g.ma_nguoi_dung = @UserId
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

                if (ds.Tables.Count > 0)
                {
                    int count = 0;
                    string userId = GetUserId();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string tenDanhMuc = row["ten_danh_muc"].ToString();
                        string loaiDanhMuc = row["loai_danh_muc"].ToString();
                        decimal soTien = Convert.ToDecimal(row["so_tien"]);
                        DateTime ngayGiaoDich = Convert.ToDateTime(row["ngay_giao_dich"]);
                        string ghiChu = row["ghi_chu"].ToString();

                        // 1. Kiểm tra xem danh mục đã tồn tại chưa, nếu chưa thì tạo mới
                        string checkCatQuery = "SELECT ma_danh_muc FROM danh_muc WHERE ten_danh_muc = @Ten AND loai_danh_muc = @Loai AND ma_nguoi_dung = @UserId";
                        MySqlParameter[] checkParams = {
                            new MySqlParameter("@Ten", tenDanhMuc),
                            new MySqlParameter("@Loai", loaiDanhMuc),
                            new MySqlParameter("@UserId", userId)
                        };
                        
                        object catResult = Database.ExecuteScalar(checkCatQuery, checkParams);
                        int categoryId;

                        if (catResult != null)
                        {
                            categoryId = Convert.ToInt32(catResult);
                        }
                        else
                        {
                            // Tạo danh mục mới
                            string insertCatQuery = "INSERT INTO danh_muc (ten_danh_muc, loai_danh_muc, ma_nguoi_dung) VALUES (@Ten, @Loai, @UserId); SELECT LAST_INSERT_ID();";
                            MySqlParameter[] insertCatParams = {
                                new MySqlParameter("@Ten", tenDanhMuc),
                                new MySqlParameter("@Loai", loaiDanhMuc),
                                new MySqlParameter("@UserId", userId)
                            };
                            categoryId = Convert.ToInt32(Database.ExecuteScalar(insertCatQuery, insertCatParams));
                        }

                        // 2. Kiểm tra xem giao dịch đã tồn tại chưa để tránh trùng lặp
                        string checkTransQuery = "SELECT COUNT(*) FROM giao_dich WHERE so_tien = @SoTien AND ngay_giao_dich = @Ngay AND ghi_chu = @GhiChu AND ma_danh_muc = @MaDanhMuc AND ma_nguoi_dung = @UserId";
                        MySqlParameter[] transParams = {
                            new MySqlParameter("@SoTien", soTien),
                            new MySqlParameter("@Ngay", ngayGiaoDich),
                            new MySqlParameter("@GhiChu", ghiChu),
                            new MySqlParameter("@MaDanhMuc", categoryId),
                            new MySqlParameter("@UserId", userId)
                        };

                        int isExist = Convert.ToInt32(Database.ExecuteScalar(checkTransQuery, transParams));

                        if (isExist == 0)
                        {
                            // 3. Chèn giao dịch
                            string insertTransQuery = "INSERT INTO giao_dich (so_tien, ngay_giao_dich, ghi_chu, ma_danh_muc, ma_nguoi_dung) VALUES (@SoTien, @Ngay, @GhiChu, @MaDanhMuc, @UserId)";
                            Database.ExecuteNonQuery(insertTransQuery, transParams);
                            count++;
                        }
                    }

                    ShowMessage("Đã phục hồi thành công " + count + " giao dịch!", true);
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

    private void ShowMessage(string msg, bool isSuccess)
    {
        lblMessage.Text = msg;
        lblMessage.CssClass = isSuccess ? "text-success" : "text-danger";
        lblMessage.Visible = true;
    }
}
