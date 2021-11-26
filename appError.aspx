<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="appError.aspx.cs" Inherits="AssetManagement.appError" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>System Mulfuntion</title>
    <link href="Common/Styles/GeneralCSS.css?v=<%=ConfigurationManager.AppSettings["CSSandJSVersion"] %>" rel="stylesheet" type="text/css" />    
    <link href="Common/Styles/Text.css?v=<%=ConfigurationManager.AppSettings["CSSandJSVersion"] %>" rel="stylesheet" type="text/css" />   
    <link href="Common/Styles/buttons.css?v=<%=ConfigurationManager.AppSettings["CSSandJSVersion"] %>" rel="stylesheet" type="text/css" />  
    <script type="text/javascript" language="javascript" src="Common/Scripts/GeneralJS.js?v=<%=ConfigurationManager.AppSettings["CSSandJSVersion"] %>"></script>

</head>
<body>
        <form id="form_error" runat="server">

        <center>
            <div align="center" style="margin: 0 auto;">

                <table width="400" border="0" align="center" cellpadding="0" cellspacing="0">
	                <tr>
		                <td height="100" align="center"></td>
	                </tr>
	                <tr>
		                <td align="center" class="txt_blue_20">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
		                	    <tr>
		                		    <td align="right"><img src="Common/Graphics/logo_gaut.png" alt="DCS" /></td>

	                		    </tr>
	                	    </table>
                        </td>
	                </tr>
	                <tr>
		                <td height="10" align="center"></td>
	                </tr>
	                <tr>
		                <td height="1" align="center" bgcolor="#036" class="txt_red_12"></td>
	                </tr>
	                <tr>
		                <td height="10" align="center"></td>
	                </tr>
	                <tr>
	                	<td height="10" align="center" class="txt_red_12">SYSTEM MULFUNTION</td>
                	</tr>
	                <tr>
	                	<td height="10" align="center"></td>
                	</tr>
	                <tr>
	                	<td height="10" align="center" class="txt_black_12"><asp:Literal ID="ltlErrMsg" runat="server"></asp:Literal></td>
                	</tr>
	                <tr>
	                	<td height="10" align="center"></td>
                	</tr>

                    <asp:Panel ID="pnlButton" runat="server" Visible="false">
                        <tr>
	                	    <td height="10" align="center"><input id="btnButton" name="btnButton" type="button" class="btn_grey" value="BACK" onclick="history.back()" /></td>
                	    </tr>
                        <tr>
	                	    <td height="10" align="center">&nbsp;</td>
                	    </tr>
                    </asp:Panel>

	                <tr>
	                	<td height="10" align="center" class="txt_red_12">Call <strong>011 689 3813</strong> for support</td>
                	</tr>
	                <tr>
		                <td height="10" align="center"></td>
	                </tr>
	                <tr>
		                <td height="1" align="center" bgcolor="#036"></td>
	                </tr>
                </table>

            </div>
        </center>
        </form>
</body>
</html>
