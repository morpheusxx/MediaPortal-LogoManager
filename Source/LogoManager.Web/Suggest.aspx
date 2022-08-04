﻿<%@ Page Title="Suggest a Channel / Logo / Alias" Language="C#" MasterPageFile="~/ChannelManager.Master" AutoEventWireup="true" CodeBehind="Suggest.aspx.cs" Inherits="ChannelManager.Suggest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br/>
    <asp:RadioButton runat="server" GroupName="LogoOpts" ID="rdNewChannel" Text="Suggest new Channel" Checked="True" OnCheckedChanged="ShowFieldset" AutoPostBack="True" CssClass="radioCheck" />
    <asp:RadioButton runat="server" GroupName="LogoOpts" ID="rdNewLogo" Text="Suggest new Logo/Alias for existing Channel" OnCheckedChanged="ShowFieldset" AutoPostBack="True" CssClass="radioCheck" />

    <asp:Panel runat="server" ID="tabPanelNewChannel" GroupingText="Suggest new Channel">
        <table style="width: 100%">
            <tr>
                <td style="width: 100px">Name</td>
                <td>
                    <asp:TextBox ID="tbxChannelName" runat="server" Width="100%"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Website&nbsp;<asp:Image runat="server" ImageUrl="~/Images/help.png" ToolTip="Uri of the Channel Homepage (when available)" /></td>
                <td>
                    <asp:TextBox ID="tbxChannelWebsite" runat="server" Width="100%"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Region</td>
                <td>
                    <asp:DropDownList ID="ddlChannelRegion" runat="server" Width="100%">
                        <asp:ListItem Text="Afghanistan" Value="AF" />
                        <asp:ListItem Text="Åland Islands" Value="AX" />
                        <asp:ListItem Text="Albania" Value="AL" />
                        <asp:ListItem Text="Algeria" Value="DZ" />
                        <asp:ListItem Text="American Samoa" Value="AS" />
                        <asp:ListItem Text="Andorra" Value="AD" />
                        <asp:ListItem Text="Angola" Value="AO" />
                        <asp:ListItem Text="Anguilla" Value="AI" />
                        <asp:ListItem Text="Antarctica" Value="AQ" />
                        <asp:ListItem Text="Antigua and Barbuda" Value="AG" />
                        <asp:ListItem Text="Argentina" Value="AR" />
                        <asp:ListItem Text="Armenia" Value="AM" />
                        <asp:ListItem Text="Aruba" Value="AW" />
                        <asp:ListItem Text="Australia" Value="AU" />
                        <asp:ListItem Text="Austria" Value="AT" />
                        <asp:ListItem Text="Azerbaijan" Value="AZ" />
                        <asp:ListItem Text="Bahamas" Value="BS" />
                        <asp:ListItem Text="Bahrain" Value="BH" />
                        <asp:ListItem Text="Bangladesh" Value="BD" />
                        <asp:ListItem Text="Barbados" Value="BB" />
                        <asp:ListItem Text="Belarus" Value="BY" />
                        <asp:ListItem Text="Belgium" Value="BE" />
                        <asp:ListItem Text="Belize" Value="BZ" />
                        <asp:ListItem Text="Benin" Value="BJ" />
                        <asp:ListItem Text="Bermuda" Value="BM" />
                        <asp:ListItem Text="Bhutan" Value="BT" />
                        <asp:ListItem Text="Bolivia" Value="BO" />
                        <asp:ListItem Text="Bonaire" Value="BQ" />
                        <asp:ListItem Text="Bosnia and Herzegovina" Value="BA" />
                        <asp:ListItem Text="Botswana" Value="BW" />
                        <asp:ListItem Text="Bouvet Island" Value="BV" />
                        <asp:ListItem Text="Brazil" Value="BR" />
                        <asp:ListItem Text="British Indian Ocean Territory" Value="IO" />
                        <asp:ListItem Text="Brunei Darussalam" Value="BN" />
                        <asp:ListItem Text="Bulgaria" Value="BG" />
                        <asp:ListItem Text="Burkina Faso" Value="BF" />
                        <asp:ListItem Text="Burundi" Value="BI" />
                        <asp:ListItem Text="Cambodia" Value="KH" />
                        <asp:ListItem Text="Cameroon" Value="CM" />
                        <asp:ListItem Text="Canada" Value="CA" />
                        <asp:ListItem Text="Cape Verde" Value="CV" />
                        <asp:ListItem Text="Cayman Islands" Value="KY" />
                        <asp:ListItem Text="Central African Republic" Value="CF" />
                        <asp:ListItem Text="Chad" Value="TD" />
                        <asp:ListItem Text="Chile" Value="CL" />
                        <asp:ListItem Text="China" Value="CN" />
                        <asp:ListItem Text="Christmas Island" Value="CX" />
                        <asp:ListItem Text="Cocos (Keeling) Islands" Value="CC" />
                        <asp:ListItem Text="Colombia" Value="CO" />
                        <asp:ListItem Text="Comoros" Value="KM" />
                        <asp:ListItem Text="Congo" Value="CG" />
                        <asp:ListItem Text="Congo" Value="CD" />
                        <asp:ListItem Text="Cook Islands" Value="CK" />
                        <asp:ListItem Text="Costa Rica" Value="CR" />
                        <asp:ListItem Text="Côte d'Ivoire" Value="CI" />
                        <asp:ListItem Text="Croatia" Value="HR" />
                        <asp:ListItem Text="Cuba" Value="CU" />
                        <asp:ListItem Text="Curaçao" Value="CW" />
                        <asp:ListItem Text="Cyprus" Value="CY" />
                        <asp:ListItem Text="Czech Republic" Value="CZ" />
                        <asp:ListItem Text="Denmark" Value="DK" />
                        <asp:ListItem Text="Djibouti" Value="DJ" />
                        <asp:ListItem Text="Dominica" Value="DM" />
                        <asp:ListItem Text="Dominican Republic" Value="DO" />
                        <asp:ListItem Text="Ecuador" Value="EC" />
                        <asp:ListItem Text="Egypt" Value="EG" />
                        <asp:ListItem Text="El Salvador" Value="SV" />
                        <asp:ListItem Text="Equatorial Guinea" Value="GQ" />
                        <asp:ListItem Text="Eritrea" Value="ER" />
                        <asp:ListItem Text="Estonia" Value="EE" />
                        <asp:ListItem Text="Ethiopia" Value="ET" />
                        <asp:ListItem Text="Falkland Islands (Malvinas)" Value="FK" />
                        <asp:ListItem Text="Faroe Islands" Value="FO" />
                        <asp:ListItem Text="Fiji" Value="FJ" />
                        <asp:ListItem Text="Finland" Value="FI" />
                        <asp:ListItem Text="France" Value="FR" />
                        <asp:ListItem Text="French Guiana" Value="GF" />
                        <asp:ListItem Text="French Polynesia" Value="PF" />
                        <asp:ListItem Text="French Southern Territories" Value="TF" />
                        <asp:ListItem Text="Gabon" Value="GA" />
                        <asp:ListItem Text="Gambia" Value="GM" />
                        <asp:ListItem Text="Georgia" Value="GE" />
                        <asp:ListItem Text="Germany" Value="DE" />
                        <asp:ListItem Text="Ghana" Value="GH" />
                        <asp:ListItem Text="Gibraltar" Value="GI" />
                        <asp:ListItem Text="Greece" Value="GR" />
                        <asp:ListItem Text="Greenland" Value="GL" />
                        <asp:ListItem Text="Grenada" Value="GD" />
                        <asp:ListItem Text="Guadeloupe" Value="GP" />
                        <asp:ListItem Text="Guam" Value="GU" />
                        <asp:ListItem Text="Guatemala" Value="GT" />
                        <asp:ListItem Text="Guernsey" Value="GG" />
                        <asp:ListItem Text="Guinea" Value="GN" />
                        <asp:ListItem Text="Guinea-Bissau" Value="GW" />
                        <asp:ListItem Text="Guyana" Value="GY" />
                        <asp:ListItem Text="Haiti" Value="HT" />
                        <asp:ListItem Text="Heard Island and McDonald Mcdonald Islands" Value="HM" />
                        <asp:ListItem Text="Holy See (Vatican City State)" Value="VA" />
                        <asp:ListItem Text="Honduras" Value="HN" />
                        <asp:ListItem Text="Hong Kong" Value="HK" />
                        <asp:ListItem Text="Hungary" Value="HU" />
                        <asp:ListItem Text="Iceland" Value="IS" />
                        <asp:ListItem Text="India" Value="IN" />
                        <asp:ListItem Text="Indonesia" Value="ID" />
                        <asp:ListItem Text="Iran" Value="IR" />
                        <asp:ListItem Text="Iraq" Value="IQ" />
                        <asp:ListItem Text="Ireland" Value="IE" />
                        <asp:ListItem Text="Isle of Man" Value="IM" />
                        <asp:ListItem Text="Israel" Value="IL" />
                        <asp:ListItem Text="Italy" Value="IT" />
                        <asp:ListItem Text="Jamaica" Value="JM" />
                        <asp:ListItem Text="Japan" Value="JP" />
                        <asp:ListItem Text="Jersey" Value="JE" />
                        <asp:ListItem Text="Jordan" Value="JO" />
                        <asp:ListItem Text="Kazakhstan" Value="KZ" />
                        <asp:ListItem Text="Kenya" Value="KE" />
                        <asp:ListItem Text="Kiribati" Value="KI" />
                        <asp:ListItem Text="Democratic People's Republic of Korea" Value="KP" />
                        <asp:ListItem Text="Republic of Korea" Value="KR" />
                        <asp:ListItem Text="Kuwait" Value="KW" />
                        <asp:ListItem Text="Kyrgyzstan" Value="KG" />
                        <asp:ListItem Text="Lao People's Democratic Republic" Value="LA" />
                        <asp:ListItem Text="Latvia" Value="LV" />
                        <asp:ListItem Text="Lebanon" Value="LB" />
                        <asp:ListItem Text="Lesotho" Value="LS" />
                        <asp:ListItem Text="Liberia" Value="LR" />
                        <asp:ListItem Text="Libya" Value="LY" />
                        <asp:ListItem Text="Liechtenstein" Value="LI" />
                        <asp:ListItem Text="Lithuania" Value="LT" />
                        <asp:ListItem Text="Luxembourg" Value="LU" />
                        <asp:ListItem Text="Macao" Value="MO" />
                        <asp:ListItem Text="Macedonia" Value="MK" />
                        <asp:ListItem Text="Madagascar" Value="MG" />
                        <asp:ListItem Text="Malawi" Value="MW" />
                        <asp:ListItem Text="Malaysia" Value="MY" />
                        <asp:ListItem Text="Maldives" Value="MV" />
                        <asp:ListItem Text="Mali" Value="ML" />
                        <asp:ListItem Text="Malta" Value="MT" />
                        <asp:ListItem Text="Marshall Islands" Value="MH" />
                        <asp:ListItem Text="Martinique" Value="MQ" />
                        <asp:ListItem Text="Mauritania" Value="MR" />
                        <asp:ListItem Text="Mauritius" Value="MU" />
                        <asp:ListItem Text="Mayotte" Value="YT" />
                        <asp:ListItem Text="Mexico" Value="MX" />
                        <asp:ListItem Text="Micronesia" Value="FM" />
                        <asp:ListItem Text="Moldova" Value="MD" />
                        <asp:ListItem Text="Monaco" Value="MC" />
                        <asp:ListItem Text="Mongolia" Value="MN" />
                        <asp:ListItem Text="Montenegro" Value="ME" />
                        <asp:ListItem Text="Montserrat" Value="MS" />
                        <asp:ListItem Text="Morocco" Value="MA" />
                        <asp:ListItem Text="Mozambique" Value="MZ" />
                        <asp:ListItem Text="Myanmar" Value="MM" />
                        <asp:ListItem Text="Namibia" Value="NA" />
                        <asp:ListItem Text="Nauru" Value="NR" />
                        <asp:ListItem Text="Nepal" Value="NP" />
                        <asp:ListItem Text="Netherlands" Value="NL" />
                        <asp:ListItem Text="New Caledonia" Value="NC" />
                        <asp:ListItem Text="New Zealand" Value="NZ" />
                        <asp:ListItem Text="Nicaragua" Value="NI" />
                        <asp:ListItem Text="Niger" Value="NE" />
                        <asp:ListItem Text="Nigeria" Value="NG" />
                        <asp:ListItem Text="Niue" Value="NU" />
                        <asp:ListItem Text="Norfolk Island" Value="NF" />
                        <asp:ListItem Text="Northern Mariana Islands" Value="MP" />
                        <asp:ListItem Text="Norway" Value="NO" />
                        <asp:ListItem Text="Oman" Value="OM" />
                        <asp:ListItem Text="Pakistan" Value="PK" />
                        <asp:ListItem Text="Palau" Value="PW" />
                        <asp:ListItem Text="Palestine" Value="PS" />
                        <asp:ListItem Text="Panama" Value="PA" />
                        <asp:ListItem Text="Papua New Guinea" Value="PG" />
                        <asp:ListItem Text="Paraguay" Value="PY" />
                        <asp:ListItem Text="Peru" Value="PE" />
                        <asp:ListItem Text="Philippines" Value="PH" />
                        <asp:ListItem Text="Pitcairn" Value="PN" />
                        <asp:ListItem Text="Poland" Value="PL" />
                        <asp:ListItem Text="Portugal" Value="PT" />
                        <asp:ListItem Text="Puerto Rico" Value="PR" />
                        <asp:ListItem Text="Qatar" Value="QA" />
                        <asp:ListItem Text="Réunion" Value="RE" />
                        <asp:ListItem Text="Romania" Value="RO" />
                        <asp:ListItem Text="Russian Federation" Value="RU" />
                        <asp:ListItem Text="Rwanda" Value="RW" />
                        <asp:ListItem Text="Saint Barthélemy" Value="BL" />
                        <asp:ListItem Text="Saint Helena" Value="SH" />
                        <asp:ListItem Text="Saint Kitts and Nevis" Value="KN" />
                        <asp:ListItem Text="Saint Lucia" Value="LC" />
                        <asp:ListItem Text="Saint Martin (French part)" Value="MF" />
                        <asp:ListItem Text="Saint Pierre and Miquelon" Value="PM" />
                        <asp:ListItem Text="Saint Vincent and the Grenadines" Value="VC" />
                        <asp:ListItem Text="Samoa" Value="WS" />
                        <asp:ListItem Text="San Marino" Value="SM" />
                        <asp:ListItem Text="Sao Tome and Principe" Value="ST" />
                        <asp:ListItem Text="Saudi Arabia" Value="SA" />
                        <asp:ListItem Text="Senegal" Value="SN" />
                        <asp:ListItem Text="Serbia" Value="RS" />
                        <asp:ListItem Text="Seychelles" Value="SC" />
                        <asp:ListItem Text="Sierra Leone" Value="SL" />
                        <asp:ListItem Text="Singapore" Value="SG" />
                        <asp:ListItem Text="Sint Maarten (Dutch part)" Value="SX" />
                        <asp:ListItem Text="Slovakia" Value="SK" />
                        <asp:ListItem Text="Slovenia" Value="SI" />
                        <asp:ListItem Text="Solomon Islands" Value="SB" />
                        <asp:ListItem Text="Somalia" Value="SO" />
                        <asp:ListItem Text="South Africa" Value="ZA" />
                        <asp:ListItem Text="South Georgia and the South Sandwich Islands" Value="GS" />
                        <asp:ListItem Text="South Sudan" Value="SS" />
                        <asp:ListItem Text="Spain" Value="ES" />
                        <asp:ListItem Text="Sri Lanka" Value="LK" />
                        <asp:ListItem Text="Sudan" Value="SD" />
                        <asp:ListItem Text="Suriname" Value="SR" />
                        <asp:ListItem Text="Svalbard and Jan Mayen" Value="SJ" />
                        <asp:ListItem Text="Swaziland" Value="SZ" />
                        <asp:ListItem Text="Sweden" Value="SE" />
                        <asp:ListItem Text="Switzerland" Value="CH" />
                        <asp:ListItem Text="Syrian Arab Republic" Value="SY" />
                        <asp:ListItem Text="Taiwan" Value="TW" />
                        <asp:ListItem Text="Tajikistan" Value="TJ" />
                        <asp:ListItem Text="Tanzania" Value="TZ" />
                        <asp:ListItem Text="Thailand" Value="TH" />
                        <asp:ListItem Text="Timor-Leste" Value="TL" />
                        <asp:ListItem Text="Togo" Value="TG" />
                        <asp:ListItem Text="Tokelau" Value="TK" />
                        <asp:ListItem Text="Tonga" Value="TO" />
                        <asp:ListItem Text="Trinidad and Tobago" Value="TT" />
                        <asp:ListItem Text="Tunisia" Value="TN" />
                        <asp:ListItem Text="Turkey" Value="TR" />
                        <asp:ListItem Text="Turkmenistan" Value="TM" />
                        <asp:ListItem Text="Turks and Caicos Islands" Value="TC" />
                        <asp:ListItem Text="Tuvalu" Value="TV" />
                        <asp:ListItem Text="Uganda" Value="UG" />
                        <asp:ListItem Text="Ukraine" Value="UA" />
                        <asp:ListItem Text="United Arab Emirates" Value="AE" />
                        <asp:ListItem Text="United Kingdom" Value="GB" />
                        <asp:ListItem Text="United States" Value="US" />
                        <asp:ListItem Text="United States Minor Outlying Islands" Value="UM" />
                        <asp:ListItem Text="Uruguay" Value="UY" />
                        <asp:ListItem Text="Uzbekistan" Value="UZ" />
                        <asp:ListItem Text="Vanuatu" Value="VU" />
                        <asp:ListItem Text="Venezuela" Value="VE" />
                        <asp:ListItem Text="Viet Nam" Value="VN" />
                        <asp:ListItem Text="British Virgin Islands" Value="VG" />
                        <asp:ListItem Text="U.S. Virgin Islands" Value="VI" />
                        <asp:ListItem Text="Wallis and Futuna" Value="WF" />
                        <asp:ListItem Text="Western Sahara" Value="EH" />
                        <asp:ListItem Text="Yemen" Value="YE" />
                        <asp:ListItem Text="Zambia" Value="ZM" />
                        <asp:ListItem Text="Zimbabwe" Value="ZW" />
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Type</td>
                <td>
                    <asp:RadioButtonList ID="rblChannelType" runat="server" DataTextField="Key" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow" />
                </td>
            </tr>
            <tr>
                <td>Description</td>
                <td>
                    <asp:TextBox ID="tbxChannelDescription" runat="server" TextMode="MultiLine" Rows="5" Width="100%"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel runat="server" ID="tabPanelExistingChannel" GroupingText="Suggest new Logo/Alias for existing Channel" CssClass="hidden">

        <asp:UpdatePanel ID="UpdatePanelChannelSearch" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:TextBox ID="tbxChannelSearch" runat="server" Width="200px" /></td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
                        <td>
                            <asp:UpdateProgress ID="UpdateProgressChannelSearch" runat="server" AssociatedUpdatePanelID="UpdatePanelChannelSearch" DynamicLayout="true">
                                <ProgressTemplate>
                                    <img src="/Images/loading.gif">
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="listFoundChannels" runat="server" Rows="4" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="listFoundChannels_SelectedIndexChanged" /></td>
                        <td>
                            <asp:Panel ID="panelSelectedChannelInfo" runat="server" Visible="false">
                                <asp:Label ID="lblSelectedChannelType" runat="server" /><br />
                                <asp:Label ID="lblSelectedChannelRegion" runat="server" /><br />
                                <asp:HyperLink ID="linkSelectedChannel" runat="server" Target="_blank" /><br />
                                <asp:Label ID="lblSelectedChannelDescription" runat="server" />
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:ListBox ID="lbSelectedChannelAliases" runat="server" Visible="false" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:Panel runat="server" GroupingText="Logo">
        <table style="width: 100%">
            <tr>
                <td style="width: 100px">Name&nbsp;<asp:Image runat="server" ImageUrl="~/Images/help.png" ToolTip="A single logo might be assigned to different channels.&#013;Give this logo a name to identify what it represents." /></td>
                <td>
                    <asp:TextBox ID="tbxLogoName" runat="server" Width="100%" /></td>
            </tr>
            <tr>
                <td>Origin&nbsp;<asp:Image runat="server" ImageUrl="~/Images/help.png" ToolTip="Simply for documentation purposes, where does the image come from?&#013;E.g. Wikipedia, Channel Homepage, Self-Painted, ..." /></td>
                <td>
                    <asp:TextBox ID="tbxLogoOrigin" runat="server" TextMode="MultiLine" Width="100%" />
                </td>
            </tr>
            <tr>
                <td>File</td>
                <td>
                    <asp:FileUpload ID="uploadLogoFile" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel runat="server" GroupingText="Aliases">
        <asp:UpdatePanel ID="UpdatePanelAliases" runat="server">
            <ContentTemplate>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 200px">
                            <asp:ListBox ID="listNewAliases" runat="server" Rows="4" Width="100%" />
                        </td>
                        <td style="vertical-align: top; width: 36px">
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
    </asp:Panel>

    <asp:Panel runat="server" GroupingText="Message to Admin">
        <asp:TextBox ID="tbxSuggestionInfo" runat="server" TextMode="MultiLine" Rows="5" Width="100%"></asp:TextBox>
    </asp:Panel>
    <br />
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /><br />
    <asp:Label ID="lblReturnMessage" runat="server" Visible="false" ForeColor="Red" />
</asp:Content>
