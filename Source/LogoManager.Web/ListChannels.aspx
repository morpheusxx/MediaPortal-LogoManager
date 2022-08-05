<%@ Page Title="Channels" Language="C#" MasterPageFile="~/ChannelManager.Master" AutoEventWireup="true" CodeBehind="ListChannels.aspx.cs" Inherits="ChannelManager.ListChannels" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Panel runat="server" GroupingText="List Channels">
        <table>
            <tr>
                <td style="padding:5px">Region: <asp:DropDownList runat="server" ID="ddRegion" /></td>
                <td style="padding:5px">Type: <asp:RadioButtonList ID="rblChannelType" runat="server" DataTextField="Key" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow" /></td>
                <td style="padding:5px"><asp:Button runat="server" ID="btnShowChannels" Text="Show Channels" OnClick="btnShowChannels_Click" /></td>
            </tr>
        </table>
    </asp:Panel>

    <asp:GridView ID="gvChannels" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Vertical" OnRowDataBound="gvChannels_RowDataBound" OnRowCommand="gvChannels_RowCommand" OnDataBinding="gvChannels_DataBinding">
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
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="/Images/delete.png" CommandName="DeleteChannel" CommandArgument='<%# Eval("Id") %>'
                        OnClientClick="return confirm('Are you sure you want to delete?');" />
                    <asp:HyperLink runat="server" ImageUrl="~/Images/edit.png" NavigateUrl='<%# "EditChannel.aspx?id=" + Eval("Id") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField DataNavigateUrlFields="Website" DataTextField="Name" HeaderText="Name" Target="_blank" />
            <asp:TemplateField HeaderText="Logo">
                <ItemTemplate>
                    <div style="float:left">
                        <asp:HyperLink runat="server" Width="48px" CssClass="logothumb" Target="_blank"
                                       ImageUrl='<%# Eval("ChannelLogoThumb") %>'
                                       NavigateUrl='<%# Eval("ChannelLogoUrl") %>' />
                    </div>
                    <div class="logoMetadata">
                        <%# Eval("Width") %> x <%# Eval("Height") %><br />
                        <%# Eval("SizeKb") %> kB
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Aliases">
                <ItemTemplate>
                    <asp:Repeater runat="server" ID="repeatAliases" DataMember="Aliases">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Name") %>'/>
                            <asp:Label runat="server" Text='<%# Eval("Providers") %>' Visible='<%# (int)Eval("ProvidersCount") > 0 %>' CssClass="provider"/>
                            <br />
                        </ItemTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ChannelDescription" HeaderText="Description" />
        </Columns>
    </asp:GridView>
</asp:Content>
