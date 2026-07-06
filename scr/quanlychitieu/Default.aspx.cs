using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblUsername.Text = User.Identity.Name;
            ViewState["CurrentFilter"] = "month";
            LoadDashboardData();
        }
    }

    protected void btnTabFilter_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        ViewState["CurrentFilter"] = btn.CommandArgument;
        
        // Reset tất cả các tab
        btnTabDay.CssClass = "tab";
        btnTabWeek.CssClass = "tab";
        btnTabMonth.CssClass = "tab";
        btnTabYear.CssClass = "tab";
        btnTabAll.CssClass = "tab";
        
        // Highlight tab đang chọn
        btn.CssClass = "tab active";

        LoadDashboardData();
    }

    private void LoadDashboardData()
    {
        // Lấy UserID từ Ticket
        FormsIdentity id = (FormsIdentity)User.Identity;
        string userId = id.Ticket.UserData;

        string filter = ViewState["CurrentFilter"] != null ? ViewState["CurrentFilter"].ToString() : "month";
        string dateCondition = "";

        if (filter == "day") dateCondition = " AND DATE(g.ngay_giao_dich) = CURDATE() ";
        else if (filter == "week") dateCondition = " AND YEARWEEK(g.ngay_giao_dich, 1) = YEARWEEK(CURDATE(), 1) ";
        else if (filter == "month") dateCondition = " AND MONTH(g.ngay_giao_dich) = MONTH(CURDATE()) AND YEAR(g.ngay_giao_dich) = YEAR(CURDATE()) ";
        else if (filter == "year") dateCondition = " AND YEAR(g.ngay_giao_dich) = YEAR(CURDATE()) ";

        // Truy vấn tổng thu, tổng chi
        string queryTotal = @"
            SELECT 
                SUM(CASE WHEN d.loai_danh_muc = 'thu' THEN g.so_tien ELSE 0 END) AS TongThu,
                SUM(CASE WHEN d.loai_danh_muc = 'chi' THEN g.so_tien ELSE 0 END) AS TongChi
            FROM giao_dich g
            INNER JOIN danh_muc d ON g.ma_danh_muc = d.ma_danh_muc
            WHERE g.ma_nguoi_dung = @UserId " + dateCondition;

        MySqlParameter[] parameters = new MySqlParameter[]
        {
            new MySqlParameter("@UserId", userId)
        };

        DataTable dtTotals = Database.GetData(queryTotal, parameters);

        decimal income = 0;
        decimal expense = 0;

        if (dtTotals.Rows.Count > 0)
        {
            if (dtTotals.Rows[0]["TongThu"] != DBNull.Value)
                income = Convert.ToDecimal(dtTotals.Rows[0]["TongThu"]);
                
            if (dtTotals.Rows[0]["TongChi"] != DBNull.Value)
                expense = Convert.ToDecimal(dtTotals.Rows[0]["TongChi"]);
        }

        lblIncome.Text = income.ToString("N0") + " đ";
        lblExpense.Text = expense.ToString("N0") + " đ";
        lblBalance.Text = (income - expense).ToString("N0") + " đ";

        // Load 5 giao dịch gần nhất
        string queryRecent = @"
            SELECT g.ngay_giao_dich, d.ten_danh_muc, d.loai_danh_muc, g.so_tien, g.ghi_chu
            FROM giao_dich g
            INNER JOIN danh_muc d ON g.ma_danh_muc = d.ma_danh_muc
            WHERE g.ma_nguoi_dung = @UserId " + dateCondition + @"
            ORDER BY g.ngay_giao_dich DESC
            LIMIT 5";

        MySqlParameter[] recentParams = new MySqlParameter[] { new MySqlParameter("@UserId", userId) };
        DataTable dtRecent = Database.GetData(queryRecent, recentParams);
        
        gvRecentTransactions.DataSource = dtRecent;
        gvRecentTransactions.DataBind();
    }
}
