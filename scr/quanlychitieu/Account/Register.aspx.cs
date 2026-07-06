using System;
using MySql.Data.MySqlClient;

public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/Default.aspx");
        }
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();
        string confirmPassword = txtConfirmPassword.Text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowMessage("Vui lòng nhập đầy đủ thông tin.", false);
            return;
        }

        if (password != confirmPassword)
        {
            ShowMessage("Mật khẩu xác nhận không khớp.", false);
            return;
        }

        // Kiểm tra xem tên đăng nhập đã tồn tại chưa
        string checkQuery = "SELECT COUNT(*) FROM nguoi_dung WHERE ten_dang_nhap = @Username";
        MySqlParameter[] checkParams = new MySqlParameter[] { new MySqlParameter("@Username", username) };
        
        int count = Convert.ToInt32(Database.ExecuteScalar(checkQuery, checkParams));
        if (count > 0)
        {
            ShowMessage("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.", false);
            return;
        }

        // Thực hiện đăng ký
        string hashedPassword = SecurityHelper.HashPassword(password);
        string insertQuery = "INSERT INTO nguoi_dung (ten_dang_nhap, mat_khau) VALUES (@Username, @Password)";
        MySqlParameter[] insertParams = new MySqlParameter[]
        {
            new MySqlParameter("@Username", username),
            new MySqlParameter("@Password", hashedPassword)
        };

        try
        {
            Database.ExecuteNonQuery(insertQuery, insertParams);
            ShowMessage("Đăng ký thành công! Đang chuyển hướng đến trang đăng nhập...", true);
            // Có thể dùng một timer bằng javascript để chuyển hướng sau 2 giây
            ClientScript.RegisterStartupScript(this.GetType(), "redirect", "setTimeout(function(){ window.location.href = 'Login.aspx'; }, 2000);", true);
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi hệ thống: " + ex.Message, false);
        }
    }

    private void ShowMessage(string msg, bool isSuccess)
    {
        lblMessage.Text = msg;
        lblMessage.CssClass = isSuccess ? "text-success" : "text-danger";
        lblMessage.Visible = true;
    }
}
