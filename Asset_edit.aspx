<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Asset_edit.aspx.cs" Inherits="AssetManagement.Asset_edit" %>

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
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.css?parameter=1"" rel="stylesheet" />
   <%-- <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css?parameter=1" rel="stylesheet">--%>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off" submitdisabledcontrols="true">
      <asp:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePageMethods="true" runat="server"></asp:ToolkitScriptManager>
        <div>
            <asp:Panel ID="pnlInterface" runat="server">
                <div class="datagrid" style="height:490px;">
                <table style="padding-top:5px; width:98%;">                                     
                 
                  <%--<tr>

                    <td width="120" align="left"  class="txt_blue_cgothic_13">ACTIVE</td>
                    <td align="left" class="txt_blue_11">
                        <label class="switch switch-green">
                          <input type="checkbox" id="f_active" class="switch-input" runat="server">
                          <span class="switch-label" data-on="On" data-off="Off"></span>
                          <span class="switch-handle"></span>
                        </label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     </td>
                    </tr>--%>
                <tr>
                        <td align="left" class="txt_blue_cgothic_13"><strong>ASSET DURATION</strong></td>
                    <td bgcolor="#FFFFFF" class="txt_red_16">
                        <strong>
                        <asp:Label ID="lblDuration" runat="server" Text=""></asp:Label>
                        </strong>
                    </td>
                </tr>
                    
                     <tr>
                        <td width="1" align="left" class="txt_blue_cgothic_13"><strong>ASSET DCS NO</strong></td>
                        <td align="left" class="txt_blue_12">
                            <asp:TextBox ID="f_assetNo" Width="150px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td align="left" class="txt_blue_cgothic_13"><strong>DATE PURCHASED</strong></td>
                        <td align="left" style="width:130px; height:25px;" class="txt_blue_11">
                            <asp:TextBox ID="f_date" style="width:100px; height:35px;" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13" Height="25px" ></asp:TextBox>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Common/Graphics/Calendar_scheduleHS.png" />
                            <asp:CalendarExtender ID="cldPurchaseDate" Format="yyyy-MM-dd" EnableViewState="false" CssClass="MyCalendar" runat="server" TargetControlID="f_date" PopupButtonID="Image1">
                            </asp:CalendarExtender>
                        </td>
                                              
                                                  
                       
                    </tr>


                    <tr>
                        <td align="left" class="txt_blue_cgothic_13"><strong>PURCHASED ORDER NO</strong></td>
                        <td align="left" class="txt_blue_12">
                            <asp:TextBox ID="f_purchasedOrderNo" Width="300px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:TextBox>
                        </td>

                        
                    </tr>

                    <tr>
                        <td align="left" class="txt_blue_cgothic_13"><strong>ASSET DESCRIPTION</strong></td>
                        <td align="left" class="txt_blue_12">
                            <div class="datagrid" style="width:98%;"><asp:TextBox ID="f_description" runat="server" CssClass="textarea txt_blue_cgothic_13" Rows="3" Width="99%" TextMode="MultiLine"></asp:TextBox></div>
                        </td>
                    </tr>

                   <tr>
                        <td  align="left" class="txt_blue_cgothic_13"><strong>ASSET SERIAL NO</strong></td>
                        <td align="left" class="txt_blue_cgothic_13">
                            <asp:TextBox ID="f_assetSerial" Width="300px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:TextBox>
                        </td>
                    </tr>




                    <tr>
                        <td align="left" class="txt_blue_cgothic_13"><strong>CATEGORY</strong></td>
                        <td align="left" class="txt_blue_cgothic_13">
                             <div class="dropdown txt_black_10" style="border:1px solid #036; margin:3px 3px 3px 3px; padding:0 0 3px 0;">
                                <asp:DropDownList ID="f_category" Enabled="true" Width="280px" runat="server" CssClass="dropdown-select txt_blue_cgothic_13">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
            

                     <tr>
                        <td align="left" class="txt_blue_cgothic_13"><strong>CONDITION</strong></td>
                        <td align="left" class="txt_blue_cgothic_13">
                            <div class="dropdown txt_black_10" style="border:1px solid #036; margin:3px 3px 3px 3px; padding:0 0 3px 0;">
                              <asp:DropDownList ID="ddl_Condition" runat="server" AutoPostBack="True" 
                                    CssClass="dropdown-select txt_blue_cgothic_13" Enabled="true" 
                                    Width="150px" onselectedindexchanged="ddl_Condition_SelectedIndexChanged">
                               </asp:DropDownList>
                            </div>
                        </td>
                     </tr> 

                        
                     
                     <tr>
                        <td align="left" class="txt_blue_cgothic_13"><strong>VALUE IN RANDS</strong></td>
                            <td align="left" class="txt_blue_13">
                            <div class="input-group" style="width:250px;">
                            <span class="input-group-addon">R</span>
                            <asp:TextBox ID="f_value" runat="server" CssClass="form-control xt_blue_cgothic_14"></asp:TextBox>
                            <span class="input-group-addon"></span>
                            </div>
                        </td>

                        
                    </tr>
                   
                  
                     
                     
                      


                     <asp:Panel ID="pnlLost" runat="server" Visible="false">
                      <tr>
                         <td width="150px" align="left" class="txt_blue_cgothic_14" style="padding-top:5px; margin-top:30px; padding-left:5px"><strong>ASSET LOST DATE</strong></td>
                            <td align="left" class="txt_blue_cgothic_13" style="height:25px;" valign="middle">
                             <asp:TextBox ID="f_lostDate" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13" Height="25px"  
                                 style="width:100px; height:35px;"></asp:TextBox>
                             <asp:Image ID="cldLost" runat="server" ImageUrl="~/Common/Graphics/Calendar_scheduleHS.png" Height="19px" />
                                            <asp:CalendarExtender ID="CalendarExtender2" CssClass="MyCalendar" Format="yyyy-MM-dd" runat="server" TargetControlID="f_lostDate" PopupButtonID="cldLost">
                                            </asp:CalendarExtender>
                         </td>
                       </tr>
                      </asp:Panel>

                      <asp:Panel ID="pnlDisposal" runat="server" Visible="false">
                      <tr>
                         <td width="150px" align="left" class="txt_blue_cgothic_14" style="padding-top:5px; margin-top:30px; padding-left:5px"><strong>DISPOSE DATE</strong></td>
                            <td align="left" class="txt_blue_cgothic_13" style="height:25px;" valign="middle">
                             <asp:TextBox ID="f_Disposal" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13" Height="25px"  
                                 style="width:100px; height:35px;"></asp:TextBox>
                             <asp:Image ID="cldDispose" runat="server" ImageUrl="~/Common/Graphics/Calendar_scheduleHS.png" Height="19px" />
                                            <asp:CalendarExtender ID="CalendarExtender3" CssClass="MyCalendar" Format="yyyy-MM-dd" runat="server" TargetControlID="f_Disposal" PopupButtonID="cldDispose">
                                            </asp:CalendarExtender>
                         </td>
                       </tr>
                      </asp:Panel>
                          
                    <asp:Panel ID="pnlReason" runat="server" Visible="false">
                      <tr>
                         <td width="150px" align="left" class="txt_blue_cgothic_14" style="padding-top:5px; margin-top:30px; padding-left:5px"><strong>REASON</strong></td>
                        <td align="left" class="txt_blue_cgothic_13">
                            <%--<asp:TextBox ID="f_Reason" Width="400px" runat="server" CssClass="textbox textbox-input txt_blue_cgothic_13"></asp:TextBox>--%>
                             <div class="datagrid" style="width:98%;"><asp:TextBox ID="f_Reason" runat="server" CssClass="textarea txt_blue_cgothic_13" Rows="2" Width="99%" TextMode="MultiLine"></asp:TextBox></div>
                        </td>
                       </tr>
                      </asp:Panel>         
                                      
                    <tr><td colspan="2" style="border-bottom:#CCCCCC 1px solid"></td></tr>

                    <asp:Panel ID="pnlMessageBox" runat="server" Visible="false">
                        <tr>
                            <td align="center" colspan="4" style="border-bottom:#CCCCCC 1px solid" class="txt_red_14">
                                <asp:Literal ID="lit_Message" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </asp:Panel>


                    

                </table>
                    <div id="divYesNo" runat="server" visible="false" align="center">
                       <table align="center" border="0" width="100%" cellpadding="3" cellspacing="1" style="min-width:880px;">
                         <tr>
                            <td class="txt_red_cgothic_13" style="padding-top:10px;text-decoration: none; text-align:center;">
                                MOVE THIS ASSET TO DISPOSSED ASSETS ?
                            </td>
                         </tr>

                        

                       </table>
                    </div>

                      
                    
                    <div id="Buttons" runat="server" visible="true" align="center">
                       <tr>
                          <td colspan="2" align="center" > 
                          <br />
                            <asp:Button ID="btnCommit" Visible="false" runat="server" style="width:120px;" CssClass="button button-green" Text="Save Details" OnClick="btnCommit_Click" />
                            <asp:Button ID="btnDisposeYes" Visible="false" runat="server" style="width:120px;" CssClass="button button-red" Text="Yes" OnClick="btnDisposeYes_Click" />
                            <asp:Button ID="btnDisposeNo" Visible="false" runat="server" style="width:120px;" CssClass="button button-light-blue" Text="No" OnClick="btnDisposeNo_Click" />
                          </td>
                        </tr>
                    </div>

                    <div id="divLostYesNo" runat="server" visible="false" align="center">
                       <table align="center" border="0" width="100%" cellpadding="3" cellspacing="1" style="min-width:880px;">
                         <tr>
                            <td class="txt_red_cgothic_13" style="padding-top:10px;text-decoration: none; text-align:center;">
                                 MOVE THIS ASSET TO LOST ASSETS ?
                            </td>
                         </tr>
                        </table>
                       
                    </div>

                    <div id="divUnlose" runat="server" visible="false" align="center">
                       <table align="center" border="0" width="100%" cellpadding="3" cellspacing="1" style="min-width:880px;">
                         <tr>
                            <td class="txt_red_cgothic_13" style="padding-top:10px;text-decoration: none; text-align:center;">
                                 MOVE THIS ASSET TO STOREROOM ?
                            </td>
                         </tr>
                        </table>
                       
                    </div>
                    
                    <div id="Div2" runat="server" visible="true" align="center">
                       <tr>
                          <td colspan="2" align="center" > 
                          <br />
                            <asp:Button ID="btnLoseYes" Visible="false" runat="server" style="width:120px;" CssClass="button button-red" Text="Yes" OnClick="btnLoseYes_Click" />
                            <asp:Button ID="btnLoseNo" Visible="false" runat="server" style="width:120px;" CssClass="button button-light-blue" Text="No" OnClick="btnLoseNo_Click" />
                          </td>
                        </tr>
                    </div>
                       

                </div>
                

            </asp:Panel>

            <asp:Panel ID="pnlException" runat="server" Visible="false" style="overflow:hidden;">
                <div class="datagrid" style="height:410px;">
                <div style="margin:20px 0 20px 0; text-align:center; overflow:hidden;">
                    <span class="txt_blue_cgothic_13">The following issues are preventing the process<br />from completing successfully:</span><br /><br />
                    <asp:Label ID="lblException" CssClass="txt_red_cgothic_13" runat="server"></asp:Label>
                </div>
                <div style="text-align:center"><asp:Button ID="btnCloseError"  Width="60px" runat="server" Text="Ok" CssClass="button button-red" OnClick="btnCloseError_Click" /></div>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
