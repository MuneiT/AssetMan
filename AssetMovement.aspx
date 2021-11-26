<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetMovement.aspx.cs" Inherits="AssetManagement.AssetMovement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--[if lt IE 7]> <html class="lt-ie9 lt-ie8 lt-ie7" lang="en"> <![endif]-->
<!--[if IE 7]> <html class="lt-ie9 lt-ie8" lang="en"> <![endif]-->
<!--[if IE 8]> <html class="lt-ie9" lang="en"> <![endif]-->
<!--[if gt IE 8]><!--> <html lang="en"> <!--<![endif]-->

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" content="" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />	
    <title></title>
    <link href="Common/Styles/GeneralCSS.css?v=10" rel="stylesheet" type="text/css" />    
    <link href="Common/Styles/Text.css?v=10" rel="stylesheet" type="text/css" />   
    <link href="Common/Styles/buttons.css?v=10" rel="stylesheet" type="text/css" />  
    <script type="text/javascript" language="javascript" src="Common/Scripts/GeneralJS.js?v=10"></script>

<%--    <script type="text/javascript">
     <!--
        //must be global to be called by ExternalInterface
        function JSFunction() {
            __doPostBack('<%= btnMoveAsset.ClientID  %>', '');
        }
     -->
     </script>--%>
</head>
<body>
    <form id="Form1" method="post" enctype="multipart/form-data" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePageMethods="true" runat="server"></asp:ToolkitScriptManager>

        <div>

            <asp:Panel ID="pnlAll" runat="server">
            <asp:UpdatePanel ID="upAll" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="btnMoveAsset" EventName="Click" />--%>

                     <asp:postbacktrigger ControlID="btnMoveAsset" />
                </Triggers>

                <ContentTemplate>
             <asp:Panel ID="pnlInterface" runat="server">
                <div class="datagrid" style="height:415px;">
                     <table border="0" style="padding-top:5px; width:100%;">                                     
                        <tr>
                            <td colspan="2" align="center" class="txt_blue_cgothic_13"><strong>MOVING FROM</strong></td>
                            <td colspan="0" align="center" class="txt_blue_cgothic_13"><strong>MOVING TO</strong></td>
                           <%-- <asp:Panel ID = "panelddlMovingTo" runat="server" Visible="true">
                                <td align="left" class="txt_blue_cgothic_13">
                                    <div class="dropdown txt_blue_cgothic_13" style="border:1px solid #036; padding:2px 5px 5px 2px; height:20px">
                                    <asp:DropDownList ID="ddlMovingTo" Enabled="true" Width="200px" runat="server"  AutoPostBack="true" CssClass="dropdown-select" 
                                        onselectedindexchanged="ddlMovingTo_SelectedIndexChanged">
                                            <asp:ListItem Text="Please Select..." Selected="True" value="" />
                                            <asp:ListItem Text="New User" Value="0" />
                                            <asp:ListItem Text="Store Room (ICT)" Value="1" />
                                    </asp:DropDownList>
                                    </div>
                                   </td>
                                </asp:Panel>--%>
                        </tr>



                        <tr>
                            <asp:Panel ID = "panelFromPersal" runat="server" Visible="true">
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Persal No</strong></td>
                            <td><asp:Label ID="f_fromPersal" Width="150px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:Label></td>
                            </asp:Panel>

                        <asp:Panel ID = "panelTemp1" runat="server" Visible="false">
                        <td colspan="2" align="center" class="txt_blue_cgothic_13"><strong></strong></td>
                        </asp:Panel>

                            <asp:Panel ID = "panelReceiverEmail" runat="server" Visible="true">
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Persal No</strong></td>
                            <td>
                                <asp:TextBox ID="f_toPersal" Width="150px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13" OnTextChanged="f_toPersal_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                <asp:Label ID="userProfileError" Visible="true" CssClass="txt_red_cgothic_13" runat="server"></asp:Label>

                            </td>
                            </asp:Panel>


                        </tr>
                                                

                        
                        <tr>
                            <asp:Panel ID = "panelFromFullNames" runat="server" Visible="true">
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Full Name(s)</strong></td>
                            <td><asp:Label ID="f_fromFullName" Width="400px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:Label></td>
                            </asp:Panel>

                        <asp:Panel ID = "panelTemp2" runat="server" Visible="false">
                        <td colspan="2" align="center" class="txt_blue_cgothic_13"><strong></strong></td>
                        </asp:Panel>

                            <asp:Panel ID = "panelFullNames" runat="server" Visible="true">
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Full Name(s)</strong></td>
                            <td><asp:Label ID="f_toFullname" Width="400px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:Label></td>
                            </asp:Panel>
                        </tr>

                        <tr>
                            <asp:Panel ID = "panelFromTelNumber" runat="server" Visible="true">
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Telephone Number</strong></td>
                            <td><asp:Label ID="f_fromTel" Width="250px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:Label></td>
                            </asp:Panel>

                          <asp:Panel ID = "panelFromStoreRoom" runat="server" Visible="false">
                            <td width="250px" align="left" class="txt_blue_cgothic_13"><strong></strong></td>
                            <td><asp:Label ID="f_FromStoreRoom" Width="350px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13" Text="MOVING ASSET FROM STORE ROOM (ICT)"></asp:Label></td>
                          </asp:Panel>

                            <asp:Panel ID = "panelTelNumber" runat="server" Visible="true">
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Telephone Number</strong></td>
                            <td><asp:Label ID="f_toTel" Width="250px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:Label></td>
                            </asp:Panel>

                        
                        <asp:Panel ID = "panelToStoreRoom" runat="server" Visible="false">
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong></strong></td>
                            <td><asp:Label ID="f_ToStoreRoom" Width="400px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13" Text="MOVING ASSET TO STORE ROOM (ICT)"></asp:Label></td>
                            </asp:Panel>

                        </tr>

                       <tr>
                           <asp:Panel ID = "panelFromRoomNumber" runat="server" Visible="true">
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Room Number</strong></td>
                            <td><asp:Label ID="f_fromRoom" Width="250px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:Label></td>
                            </asp:Panel>

                        <asp:Panel ID = "panelTemp3" runat="server" Visible="false">
                        <td colspan="2" align="center" class="txt_blue_cgothic_13"><strong></strong></td>
                        </asp:Panel>

                            <asp:Panel ID = "panelRoomNumber" runat="server" Visible="true">
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Room Number</strong></td>
                            <td><asp:Label ID="f_toRoom" Width="250px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:Label></td>
                            </asp:Panel>
                        </tr>

                        <tr>
                            <asp:Panel ID = "panelFromDirectorate" runat="server" Visible="true">
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Directorate</strong></td>
                            <td><asp:Label ID="f_fromDirectorate" Width="350px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:Label></td>
                            </asp:Panel>

                            <asp:Panel ID = "pnlCondition" runat="server" Visible="true">
                             <td width= "50px" align="right" class="txt_blue_cgothic_13"><strong>Back for Repairs:</strong></td>
                         <td align="left" class="txt_blue_cgothic_13">
                            <label class="switch switch-green">
                                <input type="checkbox" id="rdbCondition" class="switch-input" runat="server">
                                <span class="switch-label" data-on="On" data-off="Off"></span>
                                <span class="switch-handle"></span>
                            </label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        </asp:Panel>

                        <asp:Panel ID = "panelTemp4" runat="server" Visible="false">
                        <td colspan="2" align="center" class="txt_blue_cgothic_13"><strong></strong></td>
                        </asp:Panel>

                            <asp:Panel ID = "panelDirectorate" runat="server" Visible="true">
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Directorate</strong></td>
                            <td><asp:Label ID="f_toDirectorate" Width="350px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:Label></td>
                            </asp:Panel>
                        </tr>

                        
                        <tr>
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Prepared By</strong></td>

                            <td align="left" class="txt_blue_cgothic_13">
                             <div class="dropdown txt_black_10" style="border:1px solid #036; margin:3px 3px 3px 3px; padding:0 0 3px 0; height:25px">
                                <asp:DropDownList ID="ddl_stafflist" Enabled="true" Width="280px" runat="server" CssClass="dropdown-select txt_blue_cgothic_13">
                                </asp:DropDownList>
                            </div>

                            
                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Movement Date</strong></td>
                            <td>
                                <asp:TextBox ID="f_movementDate" runat="server" CssClass="textarea textbox-input txt_blue_cgothic_13" style="width:150px; height:25px;"></asp:TextBox>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Common/Graphics/Calendar_scheduleHS.png" />
                                <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" EnableViewState="false" CssClass="MyCalendar" runat="server" TargetControlID="f_movementDate" PopupButtonID="Image1"></asp:CalendarExtender>
                            </td>
                        </tr>

                        <tr>

                            <td rowspan="2" align="right" class="txt_blue_cgothic_13" valign="top"><strong>Attach Movement Form(s)</strong></td>
                            <td rowspan="2" valign="top">
                                <asp:Panel ID="pnlUploadfile" runat="server" Visible="true">
                                    <div>
                                         <asp:FileUpload ID="moveFileUploder" Width="200" BackColor="#F2F2F2" BorderStyle="Solid" BorderColor="#CCCCCC" BorderWidth="1" runat="server"/>
                                     </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlDownloadQuoteFile" runat="server" Visible="false"><div><asp:Literal ID="lit_attachments" runat="server"></asp:Literal></div></asp:Panel>
                            </td>

                            <td width="250px" align="right" class="txt_blue_cgothic_13"><strong>Movements Reason</strong></td>
                            <td>
                                <div class="datagrid" style="width:98%;"><asp:TextBox ID="f_movementReason" runat="server" CssClass="textarea txt_blue_cgothic_13" Rows="2" Width="98%" TextMode="MultiLine"></asp:TextBox></div>
                            </td>
                        </tr>




                        <tr>
                            <td colspan="1"></td>
                            <td colspan="1" align="left">
                                <asp:Button ID="btnMoveAsset" Visible="true" runat="server" style="width:120px;" CssClass="button button-blue" Text="Move Asset" OnClick="btnProcessMovement_Click" />
                            </td>
                        </tr>


                        <tr>
                        <td colspan="4" style="padding-bottom:2px;">
                                <div class="datagrid" style="text-align:left;">
                                    <span  class="txt_blue_cgothic_13"><strong>Asset List</strong></span>
                                </div>
                                <asp:Literal ID="lit_assetlist" runat="server"></asp:Literal>
                            </td>
                        </tr>
                     </table>
                </div>
            </asp:Panel>


            <asp:Panel ID="pnlException" runat="server" Visible="false" style="overflow:hidden;">
                <div class="datagrid" style="height:415px;">
                    <div style="margin:20px 0 20px 0; text-align:center; overflow:hidden;">
                        <span class="txt_blue_cgothic_13">The following issues are preventing the process<br />from completing successfully:</span><br /><br />
                        <asp:Label ID="lblException" CssClass="txt_red_cgothic_13" runat="server"></asp:Label>
                    </div>
                    <div style="text-align:center"><asp:Button ID="btnCloseError"  Width="60px" runat="server" Text="Ok" CssClass="button button-red" OnClick="btnCloseError_Click" /></div>
                </div>
            </asp:Panel>

                            
                </ContentTemplate>
            </asp:UpdatePanel>

            </asp:Panel>

        </div>
    </form>
</body>
</html>
