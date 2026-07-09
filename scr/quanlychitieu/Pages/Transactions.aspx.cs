using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
            LoadCategoryFilter();
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

    private void LoadCategoryFilter()
    {
        string query = "SELECT ma_danh_muc, CONCAT(ten_danh_muc, ' (', CASE WHEN loai_danh_muc = 'thu' THEN 'Thu' ELSE 'Chi' END, ')') AS ten_danh_muc FROM danh_muc WHERE ma_nguoi_dung = @UserId ORDER BY loai_danh_muc, ten_danh_muc";
        MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@UserId", GetUserId()) };
        DataTable dt = Database.GetData(query, parameters);

        ddlFilterCategory.DataSource = dt;
        ddlFilterCategory.DataBind();
        ddlFilterCategory.Items.Insert(0, new ListItem("Tất cả danh mục", ""));
    }

    private void LoadTransactions()
    {
        string query = @"
            SELECT gd.ma_giao_dich, gd.ngay_giao_dich, dm.ten_danh_muc, dm.loai_danh_muc, gd.so_tien, gd.ghi_chu
            FROM giao_dich gd
            INNER JOIN danh_muc dm ON gd.ma_danh_muc = dm.ma_danh_muc
            WHERE gd.ma_nguoi_dung = @UserId AND dm.ma_nguoi_dung = @UserId";

        List<MySqlParameter> parameters = new List<MySqlParameter>();
        parameters.Add(new MySqlParameter("@UserId", GetUserId()));

        string keyword = txtSearchKeyword.Text.Trim();
        string type = ddlFilterType.SelectedValue;
        string categoryId = ddlFilterCategory.SelectedValue;

        if (!string.IsNullOrEmpty(keyword))
        {
            query += " AND (LOWER(dm.ten_danh_muc) COLLATE utf8mb4_bin LIKE @Keyword OR LOWER(IFNULL(gd.ghi_chu, '')) COLLATE utf8mb4_bin LIKE @Keyword)";
            parameters.Add(new MySqlParameter("@Keyword", "%" + keyword.ToLowerInvariant() + "%"));
        }

        if (!string.IsNullOrEmpty(type))
        {
            query += " AND dm.loai_danh_muc = @Type";
            parameters.Add(new MySqlParameter("@Type", type));
        }

        if (!string.IsNullOrEmpty(categoryId))
        {
            query += " AND gd.ma_danh_muc = @CategoryId";
            parameters.Add(new MySqlParameter("@CategoryId", categoryId));
        }

        query += " ORDER BY gd.ngay_giao_dich DESC";

        DataTable dt = Database.GetData(query, parameters.ToArray());
        
        gvTransactions.DataSource = dt;
        gvTransactions.DataBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadTransactions();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtSearchKeyword.Text = "";
        ddlFilterType.SelectedValue = "";
        ddlFilterCategory.SelectedValue = "";
        LoadTransactions();
    }

    private string NormalizeAmountInput(string amountStr)
    {
        return (amountStr ?? "").Replace(".", "").Trim();
    }

    private string FormatAmountForInput(decimal amount)
    {
        return amount.ToString("N0", new CultureInfo("vi-VN"));
    }

    private bool ValidateTransactionInput(string categoryId, string amountStr, string dateStr, out decimal amount, out DateTime transactionDate)
    {
        amount = 0;
        transactionDate = DateTime.MinValue;
        string normalizedAmount = NormalizeAmountInput(amountStr);

        if (string.IsNullOrEmpty(categoryId))
        {
            ShowMessage("Vui lòng chọn danh mục.", false);
            return false;
        }

        if (string.IsNullOrEmpty(normalizedAmount))
        {
            ShowMessage("Vui lòng nhập số tiền.", false);
            return false;
        }

        if (!decimal.TryParse(normalizedAmount, NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
        {
            ShowMessage("Số tiền không hợp lệ.", false);
            return false;
        }

        if (amount <= 0)
        {
            ShowMessage("Số tiền phải lớn hơn 0.", false);
            return false;
        }

        if (string.IsNullOrEmpty(dateStr))
        {
            ShowMessage("Vui lòng chọn ngày giao dịch.", false);
            return false;
        }

        if (!DateTime.TryParse(dateStr, out transactionDate))
        {
            ShowMessage("Ngày giao dịch không hợp lệ.", false);
            return false;
        }

        return true;
    }

    protected void btnAddTransaction_Click(object sender, EventArgs e)
    {
        string categoryId = ddlCategories.SelectedValue;
        string amountStr = txtAmount.Text.Trim();
        string dateStr = txtDate.Text.Trim();
        string note = txtNote.Text.Trim();

        decimal amount;
        DateTime transactionDate;
        if (!ValidateTransactionInput(categoryId, amountStr, dateStr, out amount, out transactionDate))
        {
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
            new MySqlParameter("@Amount", amount),
            new MySqlParameter("@Date", transactionDate),
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
                txtAmount.Text = FormatAmountForInput(Convert.ToDecimal(row["so_tien"]));
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
