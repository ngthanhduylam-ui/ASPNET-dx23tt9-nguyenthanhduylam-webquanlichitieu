<%@ Page Title="Đăng nhập" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Đăng nhập - Quản lý chi tiêu
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card" style="max-width: 400px; margin: 50px auto;">
        <h2 style="text-align: center; margin-bottom: 20px;">Đăng nhập</h2>
        <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
        
        <div class="form-group">
            <label for="txtUsername">Tên đăng nhập</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label for="txtPassword">Mật khẩu</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Required="true"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <asp:Button ID="btnLogin" runat="server" Text="Đăng nhập" CssClass="btn btn-primary" style="width: 100%;" OnClick="btnLogin_Click" />
        </div>
        
        <div style="text-align: center; margin-top: 15px;">
            Chưa có tài khoản? <a href="/Account/Register.aspx">Đăng ký ngay</a>
        </div>
    </div>
</asp:Content>
