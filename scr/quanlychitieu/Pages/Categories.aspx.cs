using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class Categories : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCategories();
        }
    }

    private string GetUserId()
    {
        FormsIdentity id = (FormsIdentity)User.Identity;
        return id.Ticket.UserData;
    }

    private void LoadCategories()
    {
        string query = "SELECT ma_danh_muc, ten_danh_muc, loai_danh_muc FROM danh_muc WHERE ma_nguoi_dung = @UserId ORDER BY loai_danh_muc, ten_danh_muc";
        MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@UserId", GetUserId()) };
        DataTable dt = Database.GetData(query, parameters);
        
        gvCategories.DataSource = dt;
        gvCategories.DataBind();
    }

    protected void btnAddCategory_Click(object sender, EventArgs e)
    {
        string name = txtCategoryName.Text.Trim();
        string type = ddlCategoryType.SelectedValue;

        if (string.IsNullOrEmpty(name))
        {
            ShowMessage("Vui lòng nhập tên danh mục.", false);
            return;
        }

        string query = "INSERT INTO danh_muc (ten_danh_muc, loai_danh_muc, ma_nguoi_dung) VALUES (@Name, @Type, @UserId)";
        MySqlParameter[] parameters = new MySqlParameter[]
        {
            new MySqlParameter("@Name", name),
            new MySqlParameter("@Type", type),
            new MySqlParameter("@UserId", GetUserId())
        };

        try
        {
            Database.ExecuteNonQuery(query, parameters);
            txtCategoryName.Text = "";
            ShowMessage("Thêm danh mục thành công!", true);
            LoadCategories();
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi: " + ex.Message, false);
        }
    }

    protected void gvCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string categoryId = gvCategories.DataKeys[e.RowIndex].Value.ToString();
        string query = "DELETE FROM danh_muc WHERE ma_danh_muc = @CategoryId AND ma_nguoi_dung = @UserId";
        
        MySqlParameter[] parameters = new MySqlParameter[]
        {
            new MySqlParameter("@CategoryId", categoryId),
            new MySqlParameter("@UserId", GetUserId())
        };

        try
        {
            Database.ExecuteNonQuery(query, parameters);
            ShowMessage("Đã xóa danh mục.", true);
            LoadCategories();
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi khi xóa: " + ex.Message, false);
        }
    }

    private void ShowMessage(string msg, bool isSuccess)
    {
        lblMessage.Text = msg;
        lblMessage.CssClass = isSuccess ? "text-success" : "text-danger";
        lblMessage.Visible = true;
    }
}
