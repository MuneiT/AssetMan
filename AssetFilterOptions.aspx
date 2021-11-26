<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetFilterOptions.aspx.cs" Inherits="AssetManagement.AssetFilterOptions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--[if lt IE 7]> <html class="lt-ie9 lt-ie8 lt-ie7" lang="en"> <![endif]-->
<!--[if IE 7]> <html class="lt-ie9 lt-ie8" lang="en"> <![endif]-->
<!--[if IE 8]> <html class="lt-ie9" lang="en"> <![endif]-->
<!--[if gt IE 8]><!--> <html lang="en"> <!--<![endif]-->

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">	
    <title></title>
    <link href="Common/Styles/GeneralCSS.css?v=10" rel="stylesheet" type="text/css" />    
    <link href="Common/Styles/Text.css?v=10" rel="stylesheet" type="text/css" />   
    <link href="Common/Styles/buttons.css?v=10" rel="stylesheet" type="text/css" />  
    <script type="text/javascript" language="javascript" src="Common/Scripts/GeneralJS.js?v=10"></script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePageMethods="true" runat="server"></asp:ToolkitScriptManager>
<%--     <asp:UpdatePanel ID="upnlListing" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true"> 
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnApplyFilters"/> 
                <asp:AsyncPostBackTrigger ControlID="btnClearFilter"/>
            </Triggers> 
            <ContentTemplate>--%>
                <div>
                   <table border="0" cellspacing="0" cellpadding="0" style="width:100%;">
                    <tr>
                        <td colspan="3" valign="top" style="height:40px; padding-top:10px; margin:0 0 4px 0;">

                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="right" valign="middle" style="padding:0 0 0 10px;" class="txt_blue_10">SEARCH BY</td>
                                        <td align="left" valign="middle" style="padding:0 0 0 10px;">  
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div class="dropdown txt_black_10" style="border:1px solid #036; margin:3px 3px 3px 3px; padding:0 0 3px 0;"> 
                                                            <asp:DropDownList ID="drpSearchBy" Width="170px" runat="server" CssClass="dropdown-select" onselectedindexchanged="f_sections_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Text="" Value="" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Asset Number" Value="asset_no"></asp:ListItem>
                                                                <asp:ListItem Text="Asset Value" Value="asset_v"></asp:ListItem>
                                                                <%--<asp:ListItem Text="Date Purchased" Value="date_pur"></asp:ListItem>--%>
                                                                <asp:ListItem Text="Supplier" Value="supplier"></asp:ListItem>
                                                                <asp:ListItem Text="Region" Value="region"></asp:ListItem>
                                                                <asp:ListItem Text="Persal Number" Value="persal"></asp:ListItem>
                                                                <asp:ListItem Text="Asset Category" Value="cat"></asp:ListItem>
                                                                <asp:ListItem Text="Asset Type" Value="type"></asp:ListItem>
                                                                <asp:ListItem Text="Division" Value="div"></asp:ListItem>
                                                            </asp:DropDownList>                                    
                                                        </div>
                                                    </td>
                                                    <td align="left" class="txt_blue_11" style="padding:0 0 0 3px;">
                                                         <asp:Panel ID="pnlSearchBy" runat="server" Visible="false">
                                                            <asp:TextBox ID="f_searchby" Width="350px" CssClass="textbox textbox-input" runat="server"></asp:TextBox>
                                                        </asp:Panel>

                                                        <asp:Panel ID="pnlRegions" runat="server" Visible="false">
                                                            <div class="dropdown txt_black_10" style="border:1px solid #036; margin:3px 3px 3px 3px; padding:0 0 3px 0;"> 
                                                                <asp:DropDownList ID="drpRegion" Width="350px" runat="server" CssClass="dropdown-select">
                                                                </asp:DropDownList>                                    
                                                            </div>
                                                        </asp:Panel>

                                                        <asp:Panel ID="pnlDivision" runat="server" Visible="false">
                                                            <div class="dropdown txt_black_10" style="border:1px solid #036; margin:3px 3px 3px 3px; padding:0 0 3px 0;"> 
                                                                <asp:DropDownList ID="drpDivision" Width="350px" runat="server" CssClass="dropdown-select">
                                                                </asp:DropDownList>                                    
                                                            </div>
                                                        </asp:Panel>

                                                        <%--<asp:Panel ID="pnlPurchaseDate" runat="server" Visible="false">
                                                            <asp:TextBox ID="f_PurchDate" Enabled="false" CssClass="textbox textbox-input" runat="server"></asp:TextBox>
                                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Common/Graphics/Calendar_scheduleHS.png" />
                                                            <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" EnableViewState="false" CssClass="MyCalendar" runat="server" TargetControlID="f_PurchDate" PopupButtonID="Image1">
                                                            </asp:CalendarExtender>
                                                        </asp:Panel>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>


                  


                                    <tr>
                                        <td align="right" valign="middle" style="padding:0 0 0 10px;" class="txt_blue_10">SORT/ORDER BY</td>
                                        <td align="left" valign="middle" style="padding:0 0 0 10px;">   
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div class="dropdown txt_black_10" style="border:1px solid #036; margin:3px 3px 3px 3px; padding:0 0 3px 0;">                                              
                                                            <asp:DropDownList ID="drpSortBy" Width="170px" runat="server" CssClass="dropdown-select">
                                                                <asp:ListItem Text="Asset Number" Value="asset_no" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Asset Value" Value="asset_v"></asp:ListItem>
                                                                <asp:ListItem Text="Date Purchased" Value="date_pur"></asp:ListItem>
                                                                <asp:ListItem Text="Supplier" Value="supplier"></asp:ListItem>
                                                                <asp:ListItem Text="Region" Value="region"></asp:ListItem>
                                                                <asp:ListItem Text="Persal Number" Value="persal"></asp:ListItem>
                                                                <asp:ListItem Text="Asset Category" Value="cat"></asp:ListItem>
                                                                <asp:ListItem Text="Asset Type" Value="type"></asp:ListItem>
                                                                <asp:ListItem Text="Division" Value="div"></asp:ListItem>
                                                             </asp:DropDownList>   
                                                          </div> 
                                                        </td> 
                                                        <td>
                                                        <div class="txt_blue_10" style="margin:3px 3px 3px 3px; padding:0 0 3px 0;">
                                                            <asp:RadioButtonList ID="rblOrderType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                                <asp:ListItem Text="ASC&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Value="0" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="DESC" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>
    	                                                </div>
                                                        </td>
                                                    </tr> 
                                                </table>                                        
                                        </td>
                                    </tr>


                                    <tr>
                                        <td  colspan="2" align="right" style="padding-top:10px;">
                                            <asp:Button ID="btnApplyFilters" runat="server" CssClass="button button-green" 
                                                Text="Apply Filter" onclick="btnApplyFilters_Click"/>

                                                &nbsp;
                                                <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>

                                            <asp:Button ID="btnClearFilter" runat="server" CssClass="button button-red" 
                                                Text="Clear Filter" onclick="btnClearFilters_Click"/>
                                        </td>
                                    </tr>
                             </table>

                        </td>
                    </tr>
                </table>
                </div>
    
<%--            </ContentTemplate>
        </asp:UpdatePanel>--%>
    </form>
</body>
</html>
