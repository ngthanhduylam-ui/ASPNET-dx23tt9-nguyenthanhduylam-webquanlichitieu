<%@ Page Title="Quản lý giao dịch" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Transactions.aspx.cs" Inherits="Transactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Giao dịch - Quản lý chi tiêu
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="display: flex; gap: 30px;">
        
        <!-- Form nhập giao dịch -->
        <div class="card" style="flex: 1; height: fit-content;">
            <h3>Thêm giao dịch mới</h3>
            <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
            
            <div class="form-group" style="margin-top: 15px;">
                <label>Danh mục</label>
                <asp:DropDownList ID="ddlCategories" runat="server" CssClass="form-control" DataTextField="ten_danh_muc" DataValueField="ma_danh_muc">
                </asp:DropDownList>
            </div>
            
            <div class="form-group">
                <label>Số tiền (VNĐ)</label>
                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" inputmode="numeric"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Ngày giao dịch</label>
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="yyyy-MM-dd"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label>Ghi chú (Tùy chọn)</label>
                <asp:TextBox ID="txtNote" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:HiddenField ID="hfEditTransId" runat="server" />
                <asp:Button ID="btnAddTransaction" runat="server" Text="Lưu giao dịch" CssClass="btn btn-primary" OnClick="btnAddTransaction_Click" />
                <asp:Button ID="btnCancelEdit" runat="server" Text="Hủy sửa" CssClass="btn btn-danger" Visible="false" OnClick="btnCancelEdit_Click" />
            </div>
        </div>

        <!-- Danh sách giao dịch -->
        <div class="card" style="flex: 2;">
            <h3>Lịch sử giao dịch</h3>
            <div style="display: flex; gap: 10px; flex-wrap: wrap; align-items: end; margin-top: 15px;">
                <div class="form-group" style="flex: 2; min-width: 180px; margin-bottom: 0;">
                    <label>Từ khóa</label>
                    <asp:TextBox ID="txtSearchKeyword" runat="server" CssClass="form-control" placeholder="Nhập tên danh mục hoặc ghi chú"></asp:TextBox>
                </div>
                <div class="form-group" style="flex: 1; min-width: 130px; margin-bottom: 0;">
                    <label>Loại giao dịch</label>
                    <asp:DropDownList ID="ddlFilterType" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Tất cả" Value=""></asp:ListItem>
                        <asp:ListItem Text="Thu nhập" Value="thu"></asp:ListItem>
                        <asp:ListItem Text="Chi tiêu" Value="chi"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group" style="flex: 1.5; min-width: 160px; margin-bottom: 0;">
                    <label>Danh mục</label>
                    <asp:DropDownList ID="ddlFilterCategory" runat="server" CssClass="form-control" DataTextField="ten_danh_muc" DataValueField="ma_danh_muc">
                    </asp:DropDownList>
                </div>
                <div class="form-group" style="margin-bottom: 0;">
                    <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" CssClass="btn btn-primary" CausesValidation="false" UseSubmitBehavior="false" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnClearFilter" runat="server" Text="Xóa lọc" CssClass="btn btn-danger" CausesValidation="false" UseSubmitBehavior="false" OnClick="btnClearFilter_Click" />
                </div>
            </div>
            <asp:GridView ID="gvTransactions" runat="server" CssClass="table-custom" AutoGenerateColumns="False" DataKeyNames="ma_giao_dich" OnRowDeleting="gvTransactions_RowDeleting" OnRowCommand="gvTransactions_RowCommand" EmptyDataText="Chưa có giao dịch nào.">
                <Columns>
                    <asp:BoundField DataField="ngay_giao_dich" HeaderText="Ngày" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="ten_danh_muc" HeaderText="Danh mục" />
                    <asp:TemplateField HeaderText="Số tiền">
                        <ItemTemplate>
                            <span class='<%# Eval("loai_danh_muc").ToString() == "thu" ? "text-success" : "text-danger" %>'>
                                <%# Eval("loai_danh_muc").ToString() == "thu" ? "+" : "-" %> <%# Eval("so_tien", "{0:N0}") %> đ
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ghi_chu" HeaderText="Ghi chú" />
                    <asp:TemplateField HeaderText="Hành động">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditTrans" CommandArgument='<%# Eval("ma_giao_dich") %>' CssClass="btn btn-primary" style="padding: 5px 10px; font-size: 14px; margin-right: 5px; background-color: #ffc107; color: #333; border: none;">Sửa</asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger" style="padding: 5px 10px; font-size: 14px;" OnClientClick="return confirm('Xóa giao dịch này?');">Xóa</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

    </div>

    <script type="text/javascript">
        function formatMoney(value) {
            var digits = value.replace(/\D/g, "");
            return digits.replace(/\B(?=(\d{3})+(?!\d))/g, ".");
        }

        document.addEventListener("DOMContentLoaded", function () {
            var amountInput = document.getElementById("<%= txtAmount.ClientID %>");
            if (amountInput == null) {
                return;
            }

            amountInput.value = formatMoney(amountInput.value);

            amountInput.addEventListener("input", function () {
                amountInput.value = formatMoney(amountInput.value);
            });
        });
    </script>
</asp:Content>
