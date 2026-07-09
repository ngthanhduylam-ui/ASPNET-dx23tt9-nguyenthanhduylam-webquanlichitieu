<%@ Page Title="Quản lý danh mục" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Categories.aspx.cs" Inherits="Categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Danh mục Thu/Chi - Quản lý chi tiêu
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="display: flex; gap: 30px;">
        
        <!-- Form thêm/sửa danh mục -->
        <div class="card" style="flex: 1; height: fit-content;">
            <h3>Thêm danh mục mới</h3>
            <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
            
            <div class="form-group" style="margin-top: 15px;">
                <label>Tên danh mục</label>
                <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" placeholder="VD: Tiền nhà, Lương..."></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label>Loại danh mục</label>
                <asp:DropDownList ID="ddlCategoryType" runat="server" CssClass="form-control">
                    <asp:ListItem Value="chi" Text="Chi tiêu"></asp:ListItem>
                    <asp:ListItem Value="thu" Text="Thu nhập"></asp:ListItem>
                </asp:DropDownList>
            </div>
            
            <div class="form-group">
                <asp:Button ID="btnAddCategory" runat="server" Text="Thêm danh mục" CssClass="btn btn-primary" OnClick="btnAddCategory_Click" />
                <asp:Button ID="btnCancelEdit" runat="server" Text="Hủy" CssClass="btn btn-danger" Visible="false" CausesValidation="false" OnClick="btnCancelEdit_Click" />
            </div>
        </div>

        <!-- Danh sách danh mục -->
        <div class="card" style="flex: 2;">
            <h3>Danh sách các danh mục</h3>
            <asp:GridView ID="gvCategories" runat="server" CssClass="table-custom" AutoGenerateColumns="False" DataKeyNames="ma_danh_muc" OnRowDeleting="gvCategories_RowDeleting" OnRowCommand="gvCategories_RowCommand" EmptyDataText="Bạn chưa có danh mục nào. Hãy tạo mới bên trái.">
                <Columns>
                    <asp:BoundField DataField="ten_danh_muc" HeaderText="Tên danh mục" />
                    <asp:TemplateField HeaderText="Loại">
                        <ItemTemplate>
                            <span class='<%# Eval("loai_danh_muc").ToString() == "thu" ? "text-success" : "text-danger" %>'>
                                <%# Eval("loai_danh_muc").ToString() == "thu" ? "Thu nhập" : "Chi tiêu" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hành động">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditCategory" CommandArgument='<%# Eval("ma_danh_muc") %>' CssClass="btn btn-primary" style="padding: 5px 10px; font-size: 14px; margin-right: 5px; background-color: #ffc107; color: #333; border: none;">Sửa</asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger" style="padding: 5px 10px; font-size: 14px;" OnClientClick="return confirm('Bạn có chắc chắn muốn xóa danh mục này? Các giao dịch thuộc danh mục này cũng sẽ bị xóa theo!');">Xóa</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

    </div>
</asp:Content>
