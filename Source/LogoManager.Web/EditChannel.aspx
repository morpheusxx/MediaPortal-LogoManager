<%@ Page Title="" Language="C#" MasterPageFile="~/ChannelManager.Master" AutoEventWireup="true" CodeBehind="EditChannel.aspx.cs" Inherits="ChannelManager.EditChannel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        a img { width: 72px; }
    </style>
    <h3><asp:Label runat="server" ID="lblChannelName" /></h3>
    <div style="width:500px">
        <table style="width:100%">
            <tr>
                <td>Logo</td>
                <td style="display: flex;align-items: center;">
                    <asp:HyperLink ID="imgChannelLogo" runat="server" Target="_blank" CssClass="logoBig" />
                    <table style="padding:2px;border: 1px solid black;float:right;margin-left:5px;text-align:end;">
                        <tr>
                            <td>New Logo</td>
                            <td style="text-align:right"><asp:FileUpload runat="server" ID="uploadLogoFile" /></td>
                        </tr>
                        <tr>
                            <td>Name&nbsp;<asp:Image runat="server" ImageUrl="~/Images/help.png" ToolTip="A single logo might be assigned to different channels.&#013;Give this logo a name to identify what it represents." /></td>
                            <td><asp:TextBox ID="tbxLogoName" runat="server" Width="100%" /></td>
                        </tr>
                        <tr>
                            <td>Origin&nbsp;<asp:Image runat="server" ImageUrl="~/Images/help.png" ToolTip="Simply for documentation purposes, where does the image come from?&#013;E.g. Wikipedia, Channel Homepage, Self-Painted, ..." /></td>
                            <td><asp:TextBox ID="tbxLogoOrigin" runat="server" Width="100%" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>Website&nbsp;<asp:Image runat="server" ImageUrl="~/Images/help.png" ToolTip="Uri of the Channel Homepage (when available)" /></td>
                <td><asp:TextBox ID="tbxChannelWebsite" runat="server" Width="100%" /></td>
            </tr>
            <tr>
                <td>Description</td>
                <td><asp:TextBox ID="tbxChannelDescription" runat="server" TextMode="MultiLine" Rows="5" Width="100%" /></td>
            </tr>
            <tr>
                <td>Aliases</td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelAliases" runat="server">
                        <ContentTemplate>
                            <table style="width:100%">
                                <tr>
                                    <td style="width:200px">
                                        <asp:ListBox ID="listNewAliases" runat="server" Rows="4" Width="100%" />
                                    </td>
                                    <td style="vertical-align: top;width:36px">
                                        <asp:ImageButton ID="btnRemoveAlias" runat="server" ImageUrl="~/Images/delete.png" Visible="false" OnClick="btnRemoveAlias_Click" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAddNewAlias" runat="server" Text="New Alias:" /><br />
                                        <asp:TextBox ID="tbxChannelAliases" runat="server" Width="100%" />
                                        <asp:Button ID="btnAddAlias" runat="server" Text="Add" OnClick="btnAddAlias_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:right;padding-top:10px">
                    <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td><asp:Label ID="lblReturnMessage" runat="server" Visible="false" ForeColor="Red" /></td>
            </tr>
        </table>
    </div>
</asp:Content>
