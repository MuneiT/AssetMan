<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetDetails.aspx.cs" Inherits="AssetManagement.AssetDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">	
    <title></title>
      <link href="Common/Styles/GeneralCSS.css?v=<%=ConfigurationManager.AppSettings["CSSandJSVersion"] %>" rel="stylesheet" type="text/css" />    
    <link href="Common/Styles/Text.css?v=<%=ConfigurationManager.AppSettings["CSSandJSVersion"] %>" rel="stylesheet" type="text/css" />   
    <link href="Common/Styles/buttons.css?v=<%=ConfigurationManager.AppSettings["CSSandJSVersion"] %>" rel="stylesheet" type="text/css" />    
    <script type="text/javascript" language="javascript" src="Common/Scripts/GeneralJS.js?v=<%=ConfigurationManager.AppSettings["CSSandJSVersion"] %>"></script>
</head>
<body>
     <form id="form1" runat="server" autocomplete="off" submitdisabledcontrols="true">
    <div class="txt_lightblue_12" style="border-bottom:#000 dotted 1px; margin:0 0 10px 0; padding:0 0 5px 0;" onclick=""><strong>ASSET INFORMATION</strong></div>
            <div style="padding:0 10px 0 10px;">
	            <table width="100%" border="0" cellspacing="0" cellpadding="0">

                    <tr>
			            <td align="left" valign="top" class="txt_blue_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid">EMPLOYEE</td>
			            <td align="left" valign="top" class="txt_green_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid"><asp:Label ID="lblEmployee" runat="server"></asp:Label></td>
		            </tr>
		            <tr>
			            <td width="250" align="left" valign="top" class="txt_blue_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid">ASSSET NUMBER</td>
			            <td align="left" valign="top" class="txt_green_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid"><asp:Label ID="lblAssetNumber" runat="server"></asp:Label></td>
		            </tr>
                     <tr>
			            <td align="left" valign="top" class="txt_blue_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid">PURCHASED DATE</td>
			            <td align="left" valign="top" class="txt_green_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid"><asp:Label ID="lblDatePurcased" runat="server"></asp:Label></td>
		            </tr>
                   
                    <tr>
			            <td align="left" valign="top" class="txt_blue_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid">ORDER NUMBER</td>
			            <td align="left" valign="top" class="txt_green_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid"><asp:Label ID="lblOrderNumber" runat="server"></asp:Label></td>
		            </tr>
                    
                     <tr>
			            <td align="left" valign="top" class="txt_blue_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid">ASSET DESCRIPTION</td>
			            <td align="left" valign="top" class="txt_green_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid"><asp:Label ID="lblAssetDescription" runat="server"></asp:Label></td>
		            </tr>

                    <tr>
			            <td align="left" valign="top" class="txt_blue_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid">SERIAL NUMBER</td>
			            <td align="left" valign="top" class="txt_green_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid"><asp:Label ID="lblSerialNumber" runat="server"></asp:Label></td>
		            </tr>

                      <tr>
			            <td align="left" valign="top" class="txt_blue_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid">ASSET CATEGORY</td>
			            <td align="left" valign="top" class="txt_green_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid"><asp:Label ID="lblAssetCategory" runat="server"></asp:Label></td>
		            </tr>

                    <tr>
			            <td align="left" valign="top" class="txt_blue_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid">CONDITION</td>
			            <td align="left" valign="top" class="txt_green_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid"><asp:Label ID="lblCondition" runat="server"></asp:Label></td>
		            </tr>
		                                
                    <tr>
			            <td align="left" valign="top" class="txt_blue_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid">VALUE IN RANDS</td>
			            <td align="left" valign="top" class="txt_green_11" style="padding:5px 0 5px 0; border-bottom:#CCCCCC 1px solid"><asp:Label ID="lblValue" runat="server"></asp:Label></td>
		            </tr>
		                                                                                     
                                 
                </table>
    
    </div>
    </form>
</body>
</html>
