<%@ Page Title="Đổi mật khẩu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Đổi mật khẩu - Quản lý chi tiêu
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card" style="max-width: 450px; margin: 50px auto;">
        <h2 style="text-align: center; margin-bottom: 20px;">Đổi mật khẩu</h2>
        <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
        
        <div class="form-group" style="margin-top: 15px;">
            <label for="txtOldPassword">Mật khẩu hiện tại</label>
            <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control" TextMode="Password" Required="true"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label for="txtNewPassword">Mật khẩu mới</label>
            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" Required="true"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label for="txtConfirmPassword">Xác nhận mật khẩu mới</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" Required="true"></asp:TextBox>
        </div>
        
        <div class="form-group" style="margin-top: 25px;">
            <asp:Button ID="btnChangePassword" runat="server" Text="Đổi mật khẩu" CssClass="btn btn-primary" style="width: 100%;" OnClick="btnChangePassword_Click" />
        </div>
    </div>
</asp:Content>
