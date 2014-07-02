<%@ Page Title="Channels" Language="C#" MasterPageFile="~/ChannelManager.Master" AutoEventWireup="true" CodeBehind="ListChannels.aspx.cs" Inherits="ChannelManager.ListChannels" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    Region: <asp:DropDownList runat="server" ID="ddRegion" />
    <asp:Button runat="server" ID="btnShowChannels" Text="Show Channels" OnClick="btnShowChannels_Click" />
    <br />
    <asp:GridView ID="gvChannels" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Vertical" OnRowDataBound="gvChannels_RowDataBound">
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
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="Website" DataTextField="Name" HeaderText="Name" Target="_blank" />
            <asp:TemplateField HeaderText="Type">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# (byte)Eval("Type") == 0 ? "TV" : "Radio" %>'/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="RegionCode" HeaderText="Region" />
            <asp:TemplateField HeaderText="Logo">
                <ItemTemplate>
                    <div style="float:left">
                        <asp:Image ImageUrl='<%# "/Logos/" + ((IList<ChannelManager.EF.Logo>)Eval("Logos")).First(l => l.Suggestion == null).Id + ".png" %>' runat="server" Width="48px" CssClass="logothumb" />
                    </div>
                    <div style="float:right;font-size:smaller;margin-top:10px;margin-left:5px">
                        <%# ((IList<ChannelManager.EF.Logo>)Eval("Logos")).First(l => l.Suggestion == null).Width %>x<%# ((IList<ChannelManager.EF.Logo>)Eval("Logos")).First(l => l.Suggestion == null).Height %><br />
                        <%# (((IList<ChannelManager.EF.Logo>)Eval("Logos")).First(l => l.Suggestion == null).SizeInBytes / 1024.0).ToString("F1") %> KB
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Aliases">
                <ItemTemplate>
                    <asp:Repeater runat="server" ID="repeatAliases">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Name") %>'/>
                            <asp:Label runat="server" Text='<%# string.Format("({0})", string.Join(", ", ((IList<ChannelManager.EF.Provider>)Eval("Providers")).Select(a => a.Name))) %>' Visible='<%# (int)Eval("Providers.Count") > 0 %>' CssClass="provider"/>
                            <br />
                        </ItemTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Description" HeaderText="Description" />
        </Columns>
    </asp:GridView>
</asp:Content>
