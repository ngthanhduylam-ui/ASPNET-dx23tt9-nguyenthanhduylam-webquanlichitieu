using System;
using System.Data;
using System.Globalization;
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
        string userId = GetUserId();
        string dateCondition = GetDateCondition();

        LoadSummary(userId, dateCondition);
        LoadRecentTransactions(userId, dateCondition);
        LoadCategoryStatistics(userId, dateCondition);
    }

    private string GetUserId()
    {
        FormsIdentity id = (FormsIdentity)User.Identity;
        return id.Ticket.UserData;
    }

    private string GetDateCondition()
    {
        string filter = ViewState["CurrentFilter"] != null ? ViewState["CurrentFilter"].ToString() : "month";

        if (filter == "day") return " AND DATE(gd.ngay_giao_dich) = CURDATE() ";
        if (filter == "week") return " AND YEARWEEK(gd.ngay_giao_dich, 1) = YEARWEEK(CURDATE(), 1) ";
        if (filter == "month") return " AND MONTH(gd.ngay_giao_dich) = MONTH(CURDATE()) AND YEAR(gd.ngay_giao_dich) = YEAR(CURDATE()) ";
        if (filter == "year") return " AND YEAR(gd.ngay_giao_dich) = YEAR(CURDATE()) ";

        return "";
    }

    private void LoadSummary(string userId, string dateCondition)
    {
        string querySummary = @"
            SELECT 
                COALESCE(SUM(CASE WHEN dm.loai_danh_muc = 'thu' THEN gd.so_tien ELSE 0 END), 0) AS TongThu,
                COALESCE(SUM(CASE WHEN dm.loai_danh_muc = 'chi' THEN gd.so_tien ELSE 0 END), 0) AS TongChi,
                COUNT(gd.ma_giao_dich) AS TongGiaoDich
            FROM giao_dich gd
            INNER JOIN danh_muc dm ON gd.ma_danh_muc = dm.ma_danh_muc
            WHERE gd.ma_nguoi_dung = @UserId AND dm.ma_nguoi_dung = @UserId " + dateCondition;

        MySqlParameter[] parameters = new MySqlParameter[]
        {
            new MySqlParameter("@UserId", userId)
        };

        DataTable dtSummary = Database.GetData(querySummary, parameters);
        decimal income = 0;
        decimal expense = 0;
        int transactionCount = 0;

        if (dtSummary.Rows.Count > 0)
        {
            income = Convert.ToDecimal(dtSummary.Rows[0]["TongThu"]);
            expense = Convert.ToDecimal(dtSummary.Rows[0]["TongChi"]);
            transactionCount = Convert.ToInt32(dtSummary.Rows[0]["TongGiaoDich"]);
        }

        lblIncome.Text = FormatMoney(income);
        lblExpense.Text = FormatMoney(expense);
        lblBalance.Text = FormatMoney(income - expense);
        lblTransactionCount.Text = transactionCount.ToString("N0", new CultureInfo("vi-VN"));
    }

    private void LoadRecentTransactions(string userId, string dateCondition)
    {
        string queryRecent = @"
            SELECT gd.ngay_giao_dich, dm.ten_danh_muc, dm.loai_danh_muc, gd.so_tien, gd.ghi_chu
            FROM giao_dich gd
            INNER JOIN danh_muc dm ON gd.ma_danh_muc = dm.ma_danh_muc
            WHERE gd.ma_nguoi_dung = @UserId AND dm.ma_nguoi_dung = @UserId " + dateCondition + @"
            ORDER BY gd.ngay_giao_dich DESC
            LIMIT 5";

        MySqlParameter[] recentParams = new MySqlParameter[] { new MySqlParameter("@UserId", userId) };
        DataTable dtRecent = Database.GetData(queryRecent, recentParams);
        
        gvRecentTransactions.DataSource = dtRecent;
        gvRecentTransactions.DataBind();
    }

    private void LoadCategoryStatistics(string userId, string dateCondition)
    {
        LoadCategoryStatisticsByType(userId, dateCondition, "chi", gvExpenseByCategory);
        LoadCategoryStatisticsByType(userId, dateCondition, "thu", gvIncomeByCategory);
    }

    private void LoadCategoryStatisticsByType(string userId, string dateCondition, string categoryType, GridView gridView)
    {
        string query = @"
            SELECT dm.ten_danh_muc, COALESCE(SUM(gd.so_tien), 0) AS tong_tien
            FROM giao_dich gd
            INNER JOIN danh_muc dm ON gd.ma_danh_muc = dm.ma_danh_muc
            WHERE gd.ma_nguoi_dung = @UserId
                AND dm.ma_nguoi_dung = @UserId
                AND dm.loai_danh_muc = @CategoryType " + dateCondition + @"
            GROUP BY dm.ma_danh_muc, dm.ten_danh_muc
            ORDER BY tong_tien DESC";

        MySqlParameter[] parameters = new MySqlParameter[]
        {
            new MySqlParameter("@UserId", userId),
            new MySqlParameter("@CategoryType", categoryType)
        };

        DataTable dt = Database.GetData(query, parameters);
        gridView.DataSource = dt;
        gridView.DataBind();
    }

    protected string FormatMoney(object value)
    {
        if (value == null || value == DBNull.Value)
        {
            return "0 đ";
        }

        decimal amount = Convert.ToDecimal(value);
        return amount.ToString("N0", new CultureInfo("vi-VN")) + " đ";
    }
}
