using System;
using System.Web.Security;
using MySql.Data.MySqlClient;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/Default.aspx");
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            lblError.Text = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.";
            lblError.Visible = true;
            return;
        }

        string hashedPassword = SecurityHelper.HashPassword(password);
        string query = "SELECT ma_nguoi_dung FROM nguoi_dung WHERE ten_dang_nhap = @Username AND mat_khau = @Password";
        
        MySqlParameter[] parameters = new MySqlParameter[]
        {
            new MySqlParameter("@Username", username),
            new MySqlParameter("@Password", hashedPassword)
        };

        object result = Database.ExecuteScalar(query, parameters);

        if (result != null)
        {
            string userId = result.ToString();
            // Thiết lập ticket chứng thực, lưu ma_nguoi_dung vào userData
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddMinutes(2880), false, userId);
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(authCookie);
            
            Response.Redirect("~/Default.aspx");
        }
        else
        {
            lblError.Text = "Tên đăng nhập hoặc mật khẩu không chính xác.";
            lblError.Visible = true;
        }
    }
}
