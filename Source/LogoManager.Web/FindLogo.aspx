<%@ Page Title="Find Logo" Language="C#" MasterPageFile="~/ChannelManager.Master" AutoEventWireup="true" CodeBehind="FindLogo.aspx.cs" Inherits="ChannelManager.FindLogo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <table>
        <tr>
            <td>Channel Name:</td>
            <td>
                <asp:TextBox ID="tbxName" runat="server" Width="200px"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbxName" ErrorMessage="*" ForeColor="Red" />
            </td>
        </tr>
        <tr>
            <td>Region Code:</td>
            <td>
                <asp:TextBox ID="tbxRegion" runat="server" Width="100px"/>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbxRegion" ErrorMessage="Must be two upper case letters (ISO-3166 ALPHA-2)" ForeColor="Red" ValidationExpression="[A-Z][A-Z]" />
            </td>
        </tr>
        <tr>
            <td>Type:</td>
            <td>
                <asp:RadioButtonList ID="rblChannelType" runat="server" DataTextField="Key" DataValueField="Value" RepeatDirection="Horizontal" />
            </td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
        </tr>    
    </table>
    <asp:GridView runat="server" ID="gvLogos" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Vertical">
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
            <asp:BoundField DataField="Name" HeaderText="Alias" />
            <asp:TemplateField HeaderText="Providers">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# string.Join(", ", ((IList<ChannelManager.EF.Provider>)Eval("Providers")).Select(a => a.Name)) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Channel.Name" HeaderText="Channel" />
            <asp:BoundField DataField="Channel.RegionCode" HeaderText="Region" />
            <asp:TemplateField HeaderText="Logo">
                <ItemTemplate>
                    <div style="float:left">
                        <asp:HyperLink runat="server" Width="48px" CssClass="logothumb" Target="_blank"
                            ImageUrl='<%# ChannelManager.Thumbnailer.GetThumbFileUrl(((IList<ChannelManager.EF.Logo>)Eval("Channel.Logos")).First(l => l.Suggestion == null).Id) %>' 
                            NavigateUrl='<%# "/Logos/" + ((IList<ChannelManager.EF.Logo>)Eval("Channel.Logos")).First(l => l.Suggestion == null).Id + ".png" %>'/>
                    </div>
                    <div class="logoMetadata">
                        <%# ((IList<ChannelManager.EF.Logo>)Eval("Channel.Logos")).First(l => l.Suggestion == null).Width %>x<%# ((IList<ChannelManager.EF.Logo>)Eval("Channel.Logos")).First(l => l.Suggestion == null).Height %><br /><%# (((IList<ChannelManager.EF.Logo>)Eval("Channel.Logos")).First(l => l.Suggestion == null).SizeInBytes / 1024.0).ToString("F1") %>KB
                    </div>
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:BoundField DataField="Channel.Description" HeaderText="Description" />
        </Columns>        
    </asp:GridView>
</asp:Content>
