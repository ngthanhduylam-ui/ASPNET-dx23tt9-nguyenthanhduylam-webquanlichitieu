using System;
using System.Web.Security;
using MySql.Data.MySqlClient;

public partial class ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private string GetUserId()
    {
        FormsIdentity id = (FormsIdentity)User.Identity;
        return id.Ticket.UserData;
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        string oldPass = txtOldPassword.Text.Trim();
        string newPass = txtNewPassword.Text.Trim();
        string confirmPass = txtConfirmPassword.Text.Trim();

        if (string.IsNullOrEmpty(oldPass) || string.IsNullOrEmpty(newPass))
        {
            ShowMessage("Vui lòng nhập đầy đủ thông tin.", false);
            return;
        }

        if (newPass != confirmPass)
        {
            ShowMessage("Mật khẩu mới và xác nhận không khớp.", false);
            return;
        }

        string userId = GetUserId();
        string hashedOldPass = SecurityHelper.HashPassword(oldPass);

        // Kiểm tra mật khẩu cũ có đúng không
        string checkQuery = "SELECT COUNT(*) FROM nguoi_dung WHERE ma_nguoi_dung = @UserId AND mat_khau = @OldPassword";
        MySqlParameter[] checkParams = {
            new MySqlParameter("@UserId", userId),
            new MySqlParameter("@OldPassword", hashedOldPass)
        };

        int count = Convert.ToInt32(Database.ExecuteScalar(checkQuery, checkParams));
        
        if (count == 0)
        {
            ShowMessage("Mật khẩu hiện tại không chính xác.", false);
            return;
        }

        // Cập nhật mật khẩu mới
        string hashedNewPass = SecurityHelper.HashPassword(newPass);
        string updateQuery = "UPDATE nguoi_dung SET mat_khau = @NewPassword WHERE ma_nguoi_dung = @UserId";
        MySqlParameter[] updateParams = {
            new MySqlParameter("@NewPassword", hashedNewPass),
            new MySqlParameter("@UserId", userId)
        };

        try
        {
            Database.ExecuteNonQuery(updateQuery, updateParams);
            ShowMessage("Đổi mật khẩu thành công!", true);
            
            // Xóa form
            txtOldPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
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
