<%@ Page Title="Suggestions" Language="C#" MasterPageFile="~/ChannelManager.Master" AutoEventWireup="true" CodeBehind="ListSuggestions.aspx.cs" Inherits="ChannelManager.ListSuggestions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:GridView ID="gvSuggestions" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Vertical" AutoGenerateColumns="false">
        <AlternatingRowStyle BackColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
        <Columns>
            <asp:TemplateField HeaderText="Creation">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("Creation")%>' /><br />
                    <asp:Label runat="server" Text='<%# Eval("UserName")%>' Font-Italic="true" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="SuggestionDetail.aspx?id={0}" DataTextField="Type" HeaderText="Type" />
            <asp:BoundField DataField="Channel" HeaderText="Channel" />
            <asp:BoundField DataField="Region" HeaderText="Region" />
            <asp:TemplateField HeaderText="Logo">
                <ItemTemplate>
                    <asp:Image ImageUrl='<%# "/Logos/" + Eval("Logo") + ".png" %>' runat="server" Width="48px" Visible='<%# Eval("Logo") != null %>'/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Aliases" HeaderText="Aliases" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
        </Columns>
    </asp:GridView>
</asp:Content>
