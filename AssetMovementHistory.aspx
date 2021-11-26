<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetMovementHistory.aspx.cs" Inherits="AssetManagement.AssetMovementHistory" %>

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
      <link href="Common/Styles/GeneralCSS.css?v=12" rel="stylesheet" type="text/css" />    
      <link href="Common/Styles/Text.css?v=12" rel="stylesheet" type="text/css" />   
      <link href="Common/Styles/buttons.css?v=12" rel="stylesheet" type="text/css" />  
      <script type="text/javascript" language="javascript" src="Common/Scripts/GeneralJS.js?v=12"></script>
</head>
<body>
     <form id="form1" runat="server" autocomplete="off" submitdisabledcontrols="true">
    <%--<div class="txt_blue_12" style="padding-top:5px;"  onclick=""><strong>Asset Movement History</strong></div>
     <asp:ScriptManager ID="ScriptManager1" runat="server"/>--%>


                <div class="txt_blue_cgothic_13" style="padding-top:5px; padding-bottom:3px;" onclick="">
                    <strong>
                        <asp:Literal ID="lit_asset_information" runat="server"></asp:Literal>
                    </strong>
                 </div>

                <div>
           
                
                     <asp:Panel ID="pnlAssetMovement" runat="server" Visible="false" style="overflow:scroll; padding-top:5px;">
                        <div class="datagrid" style="height:370px;">
                        <div class="datagrid">
                            <table border="1" style="100%;">
                                <thead>
                                    <tr>
                                         <th width="200" align="left" valign="top" bgcolor="#F2F2F2"><span class="txt_white_10" style="white-space:nowrap;">PREVIOUSLY ASSIGNED</span></th>
                                         <th width="200" align="left" valign="top" bgcolor="#F2F2F2"><span class="txt_white_10" style="white-space:nowrap;">CURRENTLY ASSIGNED</span></th>
                                         <th align="left" valign="top" bgcolor="#F2F2F2"><span class="txt_white_10" style="white-space:nowrap;">REASON</span></th>
				                         <th width="120" align="left" valign="top" bgcolor="#F2F2F2"><span class="txt_white_10" style="white-space:nowrap;">MOVED BY</span></th>
                                         <th width="80" align="left" valign="top" bgcolor="#F2F2F2"><span class="txt_white_10" style="white-space:nowrap;">MOVED DATE</span></th>
                                         <th width="120" align="left" valign="top" bgcolor="#F2F2F2"><span class="txt_white_10" style="white-space:nowrap;">PREPARED BY</span></th>
                                         <th width="80" align="left" valign="top" bgcolor="#F2F2F2"><span class="txt_white_10" style="white-space:nowrap;">MOVEMENT FORM</span></th>
                                    </tr>
                                </thead>                                              
                                <asp:Literal ID="lit_Movement_History" runat="server"></asp:Literal>
                            </table>
                        </div> 
                        </div>              
                     </asp:Panel>
                  

<%--                 <asp:Panel ID="pnlNoMonitoring" runat="server" Visible="false" style="overflow:hidden; padding-top:5px;">
                    <div class="datagrid">
                         <table style="padding-top:5px; width:100%;">
                            <thead>
                                <tr><th>Notification</th></tr>
                            </thead>
                                                       
                        </table>
                    </div>
                 </asp:Panel>--%>
                 
                 

         
            <asp:Panel ID="pnlException" runat="server" Visible="false" style="overflow:hidden;">
             <div class="datagrid" style="height:415px;">
                <div style="margin:20px 0 20px 0; text-align:center; overflow:hidden;">
                    <span class="txt_green_12">The following issues are preventing the process from completing successfully:</span><br /><br />
                    <asp:Label ID="lblException" CssClass="txt_red_12" runat="server"></asp:Label>
                </div>
               <%-- <div style="text-align:center"><asp:Button ID="btnCloseError"  Width="60px" runat="server" Text="Ok" OnClick="btnCloseError_Click" CssClass="button button-red"/></div>--%>
             </div>
            </asp:Panel> 
         
       </div>

      </form>
</body>
</html>
