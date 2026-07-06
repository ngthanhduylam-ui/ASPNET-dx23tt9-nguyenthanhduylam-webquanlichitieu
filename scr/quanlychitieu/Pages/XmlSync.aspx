<%@ Page Title="Sao lưu XML" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="XmlSync.aspx.cs" Inherits="XmlSync" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Sao lưu và Phục hồi XML - Quản lý chi tiêu
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="display: flex; gap: 30px;">
        
        <!-- Xuất dữ liệu (Export) -->
        <div class="card" style="flex: 1; text-align: center;">
            <h3 style="margin-bottom: 15px; color: #0056b3;">Backup</h3>
            <p style="margin-bottom: 20px; color: #666;">
                Backup toàn bộ lịch sử giao dịch
            </p>
            <asp:Button ID="btnExport" runat="server" Text="Tải file" CssClass="btn btn-primary" OnClick="btnExport_Click" />
        </div>

        <!-- Nhập dữ liệu (Import) -->
        <div class="card" style="flex: 1; text-align: center;">
            <h3 style="margin-bottom: 15px; color: #28a745;">Khôi phục</h3>
            <p style="margin-bottom: 20px; color: #666;">
                Khôi phục dữ liệu từ tệp XML đã lưu. Dữ liệu sẽ tự động được tạo nếu chưa tồn tại.
            </p>
            <div style="margin-bottom: 15px;">
                <asp:FileUpload ID="fileUploadXml" runat="server" />
            </div>
            <asp:Button ID="btnImport" runat="server" Text="Khôi phục" CssClass="btn btn-primary" style="background-color: #28a745;" OnClick="btnImport_Click" />
            
            <div style="margin-top: 15px;">
                <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
            </div>
        </div>

    </div>
</asp:Content>
