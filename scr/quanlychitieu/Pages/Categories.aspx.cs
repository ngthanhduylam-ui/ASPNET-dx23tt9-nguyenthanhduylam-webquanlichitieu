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
        string categoryId = ViewState["EditCategoryId"] == null ? "" : ViewState["EditCategoryId"].ToString();
        bool isEdit = !string.IsNullOrEmpty(categoryId);

        if (string.IsNullOrEmpty(name))
        {
            ShowMessage("Vui lòng nhập tên danh mục.", false);
            return;
        }

        string query = "";
        if (isEdit)
        {
            query = "UPDATE danh_muc SET ten_danh_muc = @Name, loai_danh_muc = @Type WHERE ma_danh_muc = @CategoryId AND ma_nguoi_dung = @UserId";
        }
        else
        {
            query = "INSERT INTO danh_muc (ten_danh_muc, loai_danh_muc, ma_nguoi_dung) VALUES (@Name, @Type, @UserId)";
        }

        MySqlParameter[] parameters = new MySqlParameter[]
        {
            new MySqlParameter("@Name", name),
            new MySqlParameter("@Type", type),
            new MySqlParameter("@UserId", GetUserId()),
            new MySqlParameter("@CategoryId", categoryId)
        };

        try
        {
            int result = Database.ExecuteNonQuery(query, parameters);
            ResetForm();

            if (isEdit && result == 0)
            {
                ShowMessage("Không tìm thấy danh mục cần cập nhật.", false);
            }
            else
            {
                ShowMessage(isEdit ? "Cập nhật danh mục thành công!" : "Thêm danh mục thành công!", true);
            }

            LoadCategories();
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi: " + ex.Message, false);
        }
    }

    protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditCategory")
        {
            string categoryId = e.CommandArgument.ToString();
            string query = "SELECT ma_danh_muc, ten_danh_muc, loai_danh_muc FROM danh_muc WHERE ma_danh_muc = @CategoryId AND ma_nguoi_dung = @UserId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@CategoryId", categoryId),
                new MySqlParameter("@UserId", GetUserId())
            };

            DataTable dt = Database.GetData(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                ViewState["EditCategoryId"] = row["ma_danh_muc"].ToString();
                txtCategoryName.Text = row["ten_danh_muc"].ToString();
                ddlCategoryType.SelectedValue = row["loai_danh_muc"].ToString();
                btnAddCategory.Text = "Cập nhật danh mục";
                btnCancelEdit.Visible = true;
                lblMessage.Visible = false;
            }
        }
    }

    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        ResetForm();
        lblMessage.Visible = false;
    }

    private void ResetForm()
    {
        ViewState["EditCategoryId"] = null;
        txtCategoryName.Text = "";
        ddlCategoryType.SelectedValue = "chi";
        btnAddCategory.Text = "Thêm danh mục";
        btnCancelEdit.Visible = false;
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
            if (ViewState["EditCategoryId"] != null && ViewState["EditCategoryId"].ToString() == categoryId)
            {
                ResetForm();
            }

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
