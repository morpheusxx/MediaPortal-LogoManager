<%@ Page Title="Providers" Language="C#" MasterPageFile="~/ChannelManager.Master" AutoEventWireup="true" CodeBehind="ListProviders.aspx.cs" Inherits="ChannelManager.ListProviders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Panel runat="server" GroupingText="List Providers">
        Provider: <asp:DropDownList runat="server" ID="ddProviders" DataTextField="Name" DataValueField="Id" />
        <asp:Button runat="server" ID="btnShowAliases" Text="Show Aliases" OnClick="btnShowAliases_Click" />
    </asp:Panel>

    <asp:GridView runat="server" ID="gvAliases" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Both">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Alias" />
            <asp:TemplateField HeaderText="Type">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("Channel.Type") %>'/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
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
    </asp:GridView>
</asp:Content>
