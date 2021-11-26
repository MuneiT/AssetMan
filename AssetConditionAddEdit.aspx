<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetConditionAddEdit.aspx.cs" Inherits="AssetManagement.AssetConditionAddEdit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="Common/Styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="Common/Styles/GeneralCSS.css" rel="stylesheet" type="text/css" />
    <link href="Common/Styles/Text.css" rel="stylesheet" type="text/css" />
    <script src="Common/Scripts/GeneralJS.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server" autocomplete="on" submitdisabledcontrols="true">
        <asp:ScriptManager ID="ScriptManager" runat = "server" />
    <div>
        
        <asp:Panel ID="pnlInterface" runat="server" style="overflow:hidden;">
                <table style="padding-top:5px;">
                                    <tr>
                        <td width= "45%" align="left" class="txt_blue_cgothic_13"><strong>ACTIVE:</strong></td>
                         <td align="left" class="txt_blue_cgothic_13">
                            <label class="switch switch-green">
                                <input type="checkbox" id="f_active" checked="checked" class="switch-input" runat="server">
                                <span class="switch-label" data-on="On" data-off="Off"></span>
                                <span class="switch-handle"></span>
                            </label>
                        &nbsp;&nbsp;&nbsp;</td>
                            <td align="left" class="txt_blue_11"></td>
                            <td align="left" class="txt_blue_11"></td>
                   </tr>

                  <tr>

                    <td align="left" class="txt_blue_cgothic_13"><strong>CONDITON:</strong></td>
                        <td align="left" class="txt_blue_cgothic_13">
                    <asp:TextBox ID="f_ConDesc" runat="server" 
                                 CssClass="textbox textbox-input txt_blue_cgothic_13" Height="25px" 
                                 style="width:200px; height:35px;"></asp:TextBox>

                 </tr>
                     <tr>
                    <td>

                    </td>
                        <td class="style2" >                 
                            <asp:Button ID="btnCommit" CssClass="button button-green" runat="server" 
                                Text="Save Details" onclick="btnCommit_Click" />
                        </td>
                     </tr>
               
             </table>
      </asp:Panel>


              <asp:Panel ID="pnlException" runat="server" Visible="false" style="overflow:hidden;">
                <div style="margin:20px 0 20px 0; text-align:center; overflow:hidden;">
                    <span class="txt_black_12">The following issues are preventing the process<br />from completing successfully:</span><br /><br />
                    <asp:Label ID="lblException" CssClass="txt_red_12" runat="server"></asp:Label>
                </div>

                  <div style="text-align:center"><asp:Button ID="btnCloseError" runat="server" Text="Ok" CssClass="button button-red" OnClick="btnCloseError_Click" /></div>
            </asp:Panel>
         </div>
    </form>
</body>
</html>
