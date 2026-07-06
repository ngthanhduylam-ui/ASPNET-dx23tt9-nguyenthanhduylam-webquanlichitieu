using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class Transactions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            LoadCategoriesDropdown();
            LoadTransactions();
        }
    }

    private string GetUserId()
    {
        FormsIdentity id = (FormsIdentity)User.Identity;
        return id.Ticket.UserData;
    }

    private void LoadCategoriesDropdown()
    {
        string query = "SELECT ma_danh_muc, CONCAT(ten_danh_muc, ' (', CASE WHEN loai_danh_muc = 'thu' THEN 'Thu' ELSE 'Chi' END, ')') AS ten_danh_muc FROM danh_muc WHERE ma_nguoi_dung = @UserId ORDER BY loai_danh_muc, ten_danh_muc";
        MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@UserId", GetUserId()) };
        DataTable dt = Database.GetData(query, parameters);
        
        ddlCategories.DataSource = dt;
        ddlCategories.DataBind();

        if (dt.Rows.Count == 0)
        {
            ShowMessage("Bạn cần tạo 'Danh mục' trước khi thêm giao dịch.", false);
            btnAddTransaction.Enabled = false;
        }
    }

    private void LoadTransactions()
    {
        string query = @"
            SELECT g.ma_giao_dich, g.ngay_giao_dich, d.ten_danh_muc, d.loai_danh_muc, g.so_tien, g.ghi_chu
            FROM giao_dich g
            INNER JOIN danh_muc d ON g.ma_danh_muc = d.ma_danh_muc
            WHERE g.ma_nguoi_dung = @UserId
            ORDER BY g.ngay_giao_dich DESC";
            
        MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@UserId", GetUserId()) };
        DataTable dt = Database.GetData(query, parameters);
        
        gvTransactions.DataSource = dt;
        gvTransactions.DataBind();
    }

    protected void btnAddTransaction_Click(object sender, EventArgs e)
    {
        string categoryId = ddlCategories.SelectedValue;
        string amountStr = txtAmount.Text.Trim();
        string dateStr = txtDate.Text.Trim();
        string note = txtNote.Text.Trim();

        if (string.IsNullOrEmpty(categoryId) || string.IsNullOrEmpty(amountStr) || string.IsNullOrEmpty(dateStr))
        {
            ShowMessage("Vui lòng điền đủ Số tiền, Ngày và chọn Danh mục.", false);
            return;
        }

        string transId = hfEditTransId.Value;
        bool isEdit = !string.IsNullOrEmpty(transId);

        string query = "";
        if (isEdit)
        {
            query = "UPDATE giao_dich SET so_tien=@Amount, ngay_giao_dich=@Date, ghi_chu=@Note, ma_danh_muc=@CategoryId WHERE ma_giao_dich=@TransId AND ma_nguoi_dung=@UserId";
        }
        else
        {
            query = "INSERT INTO giao_dich (so_tien, ngay_giao_dich, ghi_chu, ma_danh_muc, ma_nguoi_dung) VALUES (@Amount, @Date, @Note, @CategoryId, @UserId)";
        }

        MySqlParameter[] parameters = new MySqlParameter[]
        {
            new MySqlParameter("@Amount", amountStr),
            new MySqlParameter("@Date", dateStr),
            new MySqlParameter("@Note", note),
            new MySqlParameter("@CategoryId", categoryId),
            new MySqlParameter("@UserId", GetUserId()),
            new MySqlParameter("@TransId", isEdit ? transId : "0")
        };

        try
        {
            Database.ExecuteNonQuery(query, parameters);
            ResetForm();
            ShowMessage(isEdit ? "Đã cập nhật giao dịch!" : "Giao dịch đã được lưu!", true);
            LoadTransactions();
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi: " + ex.Message, false);
        }
    }

    protected void gvTransactions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditTrans")
        {
            string transId = e.CommandArgument.ToString();
            string query = "SELECT ma_giao_dich, so_tien, ngay_giao_dich, ghi_chu, ma_danh_muc FROM giao_dich WHERE ma_giao_dich = @TransId AND ma_nguoi_dung = @UserId";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@TransId", transId),
                new MySqlParameter("@UserId", GetUserId())
            };
            
            DataTable dt = Database.GetData(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                hfEditTransId.Value = row["ma_giao_dich"].ToString();
                txtAmount.Text = Convert.ToDecimal(row["so_tien"]).ToString("0.##");
                txtDate.Text = Convert.ToDateTime(row["ngay_giao_dich"]).ToString("yyyy-MM-dd");
                txtNote.Text = row["ghi_chu"].ToString();
                if (ddlCategories.Items.FindByValue(row["ma_danh_muc"].ToString()) != null)
                {
                    ddlCategories.SelectedValue = row["ma_danh_muc"].ToString();
                }
                
                btnAddTransaction.Text = "Cập nhật";
                btnCancelEdit.Visible = true;
            }
        }
    }

    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        ResetForm();
    }

    private void ResetForm()
    {
        hfEditTransId.Value = "";
        txtAmount.Text = "";
        txtNote.Text = "";
        txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        btnAddTransaction.Text = "Lưu giao dịch";
        btnCancelEdit.Visible = false;
        lblMessage.Visible = false;
    }

    protected void gvTransactions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string transId = gvTransactions.DataKeys[e.RowIndex].Value.ToString();
        string query = "DELETE FROM giao_dich WHERE ma_giao_dich = @TransId AND ma_nguoi_dung = @UserId";
        
        MySqlParameter[] parameters = new MySqlParameter[]
        {
            new MySqlParameter("@TransId", transId),
            new MySqlParameter("@UserId", GetUserId())
        };

        try
        {
            Database.ExecuteNonQuery(query, parameters);
            ShowMessage("Đã xóa giao dịch.", true);
            LoadTransactions();
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
