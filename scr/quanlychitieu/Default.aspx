<%@ Page Title="Tổng quan" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tổng quan - Quản lý chi tiêu
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px;">
        <h2>Xin chào, <asp:Label ID="lblUsername" runat="server" Font-Bold="true" ForeColor="#0056b3"></asp:Label>!</h2>
        <div style="display: flex; align-items: center;">
            <div class="tabs">
                <asp:LinkButton ID="btnTabDay" runat="server" CssClass="tab" CommandArgument="day" OnClick="btnTabFilter_Click">Hôm nay</asp:LinkButton>
                <asp:LinkButton ID="btnTabWeek" runat="server" CssClass="tab" CommandArgument="week" OnClick="btnTabFilter_Click">Tuần này</asp:LinkButton>
                <asp:LinkButton ID="btnTabMonth" runat="server" CssClass="tab active" CommandArgument="month" OnClick="btnTabFilter_Click">Tháng này</asp:LinkButton>
                <asp:LinkButton ID="btnTabYear" runat="server" CssClass="tab" CommandArgument="year" OnClick="btnTabFilter_Click">Năm nay</asp:LinkButton>
                <asp:LinkButton ID="btnTabAll" runat="server" CssClass="tab" CommandArgument="all" OnClick="btnTabFilter_Click">Tất cả</asp:LinkButton>
            </div>
            <a href="~/Pages/Transactions.aspx" runat="server" class="btn btn-primary" style="margin-left: 30px;">+ Giao dịch</a>
        </div>
    </div>

    <!-- Hàng thẻ tóm tắt -->
    <div style="display: flex; gap: 20px; margin-bottom: 30px; flex-wrap: wrap;">
        <!-- Tổng thu -->
        <div class="card" style="flex: 1; min-width: 200px; text-align: center; border-top: 4px solid #28a745;">
            <h3 style="color: #666; font-size: 16px;">TỔNG THU</h3>
            <p style="font-size: 28px; font-weight: bold; color: #28a745; margin-top: 10px;">
                <asp:Label ID="lblIncome" runat="server" Text="0 đ"></asp:Label>
            </p>
        </div>

        <!-- Tổng chi -->
        <div class="card" style="flex: 1; min-width: 200px; text-align: center; border-top: 4px solid #dc3545;">
            <h3 style="color: #666; font-size: 16px;">TỔNG CHI</h3>
            <p style="font-size: 28px; font-weight: bold; color: #dc3545; margin-top: 10px;">
                <asp:Label ID="lblExpense" runat="server" Text="0 đ"></asp:Label>
            </p>
        </div>

        <!-- Số dư -->
        <div class="card" style="flex: 1; min-width: 200px; text-align: center; border-top: 4px solid #0056b3;">
            <h3 style="color: #666; font-size: 16px;">SỐ DƯ</h3>
            <p style="font-size: 28px; font-weight: bold; color: #0056b3; margin-top: 10px;">
                <asp:Label ID="lblBalance" runat="server" Text="0 đ"></asp:Label>
            </p>
        </div>

        <!-- Tổng số giao dịch -->
        <div class="card" style="flex: 1; min-width: 200px; text-align: center; border-top: 4px solid #6c757d;">
            <h3 style="color: #666; font-size: 16px;">TỔNG SỐ GIAO DỊCH</h3>
            <p style="font-size: 28px; font-weight: bold; color: #333; margin-top: 10px;">
                <asp:Label ID="lblTransactionCount" runat="server" Text="0"></asp:Label>
            </p>
        </div>
    </div>
    
    <!-- 5 giao dịch gần đây (Ví dụ) -->
    <div class="card">
        <h3>5 Giao dịch gần nhất</h3>
        <asp:GridView ID="gvRecentTransactions" runat="server" CssClass="table-custom" AutoGenerateColumns="False" EmptyDataText="Chưa có giao dịch nào gần đây.">
            <Columns>
                <asp:BoundField DataField="ngay_giao_dich" HeaderText="Ngày" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="ten_danh_muc" HeaderText="Danh mục" />
                <asp:TemplateField HeaderText="Số tiền">
                    <ItemTemplate>
                        <span class='<%# Eval("loai_danh_muc").ToString() == "thu" ? "text-success" : "text-danger" %>'>
                            <%# Eval("loai_danh_muc").ToString() == "thu" ? "+" : "-" %> <%# FormatMoney(Eval("so_tien")) %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ghi_chu" HeaderText="Ghi chú" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Thống kê theo danh mục -->
    <div style="display: flex; gap: 20px; flex-wrap: wrap;">
        <div class="card" style="flex: 1; min-width: 300px;">
            <h3>Thống kê chi theo danh mục</h3>
            <asp:GridView ID="gvExpenseByCategory" runat="server" CssClass="table-custom" AutoGenerateColumns="False" EmptyDataText="Chưa có dữ liệu chi theo danh mục.">
                <Columns>
                    <asp:BoundField DataField="ten_danh_muc" HeaderText="Tên danh mục" />
                    <asp:TemplateField HeaderText="Tổng tiền">
                        <ItemTemplate>
                            <span class="text-danger"><%# FormatMoney(Eval("tong_tien")) %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="card" style="flex: 1; min-width: 300px;">
            <h3>Thống kê thu theo danh mục</h3>
            <asp:GridView ID="gvIncomeByCategory" runat="server" CssClass="table-custom" AutoGenerateColumns="False" EmptyDataText="Chưa có dữ liệu thu theo danh mục.">
                <Columns>
                    <asp:BoundField DataField="ten_danh_muc" HeaderText="Tên danh mục" />
                    <asp:TemplateField HeaderText="Tổng tiền">
                        <ItemTemplate>
                            <span class="text-success"><%# FormatMoney(Eval("tong_tien")) %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
