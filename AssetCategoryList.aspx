<%@ Page Title="Asset(s) Category ListView" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeBehind="AssetCategoryList.aspx.cs" Inherits="AssetManagement.AssetCategoryList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType TypeName="AssetManagement.main" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cp_pageheading" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cp_pagedata" runat="server">

    <div id="divPopupDialog" class="PopUpBase">
        <table border="0" cellspacing="0" cellpadding="0" style="width:100%;">
            <tr>
                <td  valign="top" style="border-bottom:#000 dotted 1px; padding:0 0 5px 0; height:10px;">
                    <div style="margin:0 0 2px 0; padding:0 0 5px 0; float:right;" class="txt_black_11">
                         <input name="btnCloseDialog" id="btnCloseDialog" type="button" class="BtnClosePopOff" value="X" onclick="javascript:window.document.getElementById('ifOptions').src=''; ShowHideDiv('divPopupDialog', 'none');" onmouseover="ChangeClass(this.id, 'BtnClosePop')" onmouseout="ChangeClass(this.id, 'BtnClosePopOff')" />
                    </div>
                </td>
            </tr>

       </table>
	    <iframe id="ifOptions" allowtransparency="true" frameborder="0"></iframe>
    </div>

    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"/>--%>

    <div class="panel panel-primary">
        <div class="panel-heading" style="text-align:left;"><strong>Asset Category Management</strong></div>
        <div class="panel-body">

            <%= AssetManagement.Proc.Gen.makeSpace(08, "", true)%> 

             <asp:UpdatePanel ID="upnlListing" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="margin:5px 0 0 0;">
                        <div runat="server" id="dvRecords" visible="true" style="padding-top:10px;">
                            <div  style="padding-bottom:5px;">
                               <table align="center" border="0" width="100%" cellpadding="3" cellspacing="1" style="min-width:880px;" bgcolor="#CCCCCC">
                                   <tr>
				                        <td colspan="11" align="center">
                                            <asp:Panel ID="pnlInterface" runat="server">
                                                <table border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="padding:0 2px 0 0; width:30px;"><asp:LinkButton ID="btnFirst" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnFirst_Click"><div style="text-align:center; vertical-align:middle; padding-top:2px;">|<</div></asp:LinkButton></td>
                        	                            <td style="padding:0 2px 0 0; width:30px;"><asp:LinkButton ID="btnPrev" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnPrev_Click"><div style="text-align:center; vertical-align:middle; padding-top:2px;"><</div></asp:LinkButton></td>
                        	                            <td  class="txt_darkgrey_11" style="padding:0 5px 0 0; width:20px;">Page</td>
                        	                            <td style="padding:0 5px 0 0; width:20px;"><asp:TextBox ID="txtPage" runat="server" Width="20px" CssClass="txt_darkgrey_11"></asp:TextBox></td>
                        	                            <td class="txt_darkgrey_11" style="padding:0 5px 0 0; width:10px;">of</td>
                        	                            <td class="txt_black_11" style="padding:0 5px 0 0; width:20px;"><asp:Label ID="lblTotalPages" runat="server" CssClass="txt_darkgrey_11"></asp:Label></td>
                                                        <td style="padding:0 2px 0 0; width:30px;"><asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnNext_Click"><div style="text-align:center; vertical-align:middle; padding-top:2px;">></div></asp:LinkButton></td>
                                                        <td style="padding:0 2px 0 0; width:30px;"><asp:LinkButton ID="btnLast" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnLast_Click"><div style="text-align:center; vertical-align:middle; padding-top:2px;">>|</div></asp:LinkButton></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
			                        </tr>
                                </table>
                            </div>

                            <table class="table_settings" width="100%" style="padding-bottom:10px;" cellpadding="3" cellspacing="0"  bgcolor="#CCCCCC">
                                <thead style="border-bottom:3px solid;">
                                    <tr class="txt_blue_cgothic_12">                                    
                                        <th width="100" align="center" valign="middle"><strong><a href="javascript:void(0);" onclick="OpenPopupDialog('divPopupDialog', 'ifOptions', 150, 550, 'AssetCategoryAddEdit.aspx?new=true','PopUpBase PopPrimary');">CREATE NEW</a></strong>
                                        <th align="left" valign="middle"><span class="txt_blue_cgothic_12" style="white-space:nowrap;">CATEGORY DESCRIPTION</span></th>
                                        <th align="left" valign="middle"><span class="txt_blue_cgothic_12" style="white-space:nowrap;">CREATED</span></th>
                                        <th align="left" valign="middle"><span class="txt_blue_cgothic_12" style="white-space:nowrap;">MODIFIED</span></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound"></asp:Repeater>
                                </tbody>
                            </table>
                        </div>

                        <div id="dvNoRecords" runat="server" visible="false" align="center">
                            <table align="center" border="0" width="100%" cellpadding="3" cellspacing="1" style="min-width:880px;">
                            <tr>
                                <td class="txt_red_cgothic_13" style="padding-top:10px;text-decoration: none; text-align:center;">
                                    NO RECORDS AVAILABLE <br /><br />
                                    <a href="javascript:void(0);" onclick="OpenPopupDialog('divPopupDialog', 'ifOptions', 150, 550, 'AssetCategoryAddEdit.aspx?new=true','PopUpBase PopPrimary');">Click here to add new Category</a>                                             
                                </td>
                            </tr>
                            </table>
                        </div>

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
