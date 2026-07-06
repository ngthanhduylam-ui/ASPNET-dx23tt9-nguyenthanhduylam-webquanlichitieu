<%@ Page Title="Đăng ký" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Đăng ký tài khoản - Quản lý chi tiêu
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card" style="max-width: 400px; margin: 50px auto;">
        <h2 style="text-align: center; margin-bottom: 20px;">Đăng ký tài khoản</h2>
        <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
        
        <div class="form-group">
            <label for="txtUsername">Tên đăng nhập</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label for="txtPassword">Mật khẩu</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Required="true"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label for="txtConfirmPassword">Xác nhận mật khẩu</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" Required="true"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <asp:Button ID="btnRegister" runat="server" Text="Đăng ký" CssClass="btn btn-primary" style="width: 100%;" OnClick="btnRegister_Click" />
        </div>
        
        <div style="text-align: center; margin-top: 15px;">
            Đã có tài khoản? <a href="/Account/Login.aspx">Đăng nhập</a>
        </div>
    </div>
</asp:Content>
