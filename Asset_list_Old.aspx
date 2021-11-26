<%@ Page Title="Asset Management" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeBehind="Asset_list_Old.aspx.cs" Inherits="AssetManagement.Asset_list" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ MasterType TypeName="AssetManagement.main" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cp_pageheading" runat="server">
<%--<%= AssetManagement.Proc.Gen.buildPageHeading("ASSET MANAGEMENT")%>--%>
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

    <asp:UpdateProgress ID="UPR" runat="server" AssociatedUpdatePanelID="upnlListing" DisplayAfter="0" DynamicLayout="false">
        <ProgressTemplate>
			<div id="divProgress" class="ProgressModal">
				<table border="0" align="center" cellpadding="0" cellspacing="0">
					<tr>
						<td width="32" valign="middle"><img src="Common/Graphics/loader_white.gif" width="32" height="32" /></td>
						<td valign="middle" class="txt_white_16" style="padding:0 0 0 10px;">Processing request, please wait...</td>
					</tr>
				</table>
			</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

        <asp:UpdatePanel ID="upnlListing" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="margin:2px 0 0 0;">                
                     <div>
                          <asp:Panel ID="pan_list" runat="server">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                          <div class="wrap_interface_top">
                                            <asp:Panel ID="pan_page" runat="server">
                                                <table border="0" cellspacing="0" cellpadding="0">
                                                    <tr>

                                                        <%--<asp:Panel ID="pnlDropDownList" runat="server" Visible="false">
                                                           <td><span  class="txt_blue_12">Select Category</span></td>
                                                           <td style="padding-right:50px;">
                                                                <div class="dropdown txt_black_10" style="border:1px solid #036; margin:3px 3px 3px 3px; padding:0 0 3px 0;">
                                                                    <asp:DropDownList ID="ddlCategory" Enabled="true" runat="server" CssClass="dropdown-select">
                                                                    </asp:DropDownList>
                                                                 </div>
                                                           </td>
                                                        </asp:Panel>--%>

                                                        <td style="padding:0 2px 0 0; width:30px;"><asp:LinkButton ID="btnFirst" runat="server" CssClass="NAGIGATION_ITEM txt_white_12" Width="20px" Height="18px" OnClick="btnFirst_Click"><div style="text-align:center; vertical-align:middle; padding-top:2px;">|<</div></asp:LinkButton></td>
                        	                            <td style="padding:0 2px 0 0; width:30px;"><asp:LinkButton ID="btnPrev" runat="server" CssClass="NAGIGATION_ITEM txt_white_12" Width="20px" Height="18px" OnClick="btnPrev_Click"><div style="text-align:center; vertical-align:middle; padding-top:2px;"><</div></asp:LinkButton></td>

                        	                            <td  class="txt_darkgrey_11" style="padding:0 5px 0 0; width:20px;">Page</td>
                        	                            <td style="padding:0 5px 0 0; width:20px;"><asp:TextBox ID="txtPage" runat="server" Width="20px" CssClass="txt_darkgrey_11"></asp:TextBox></td>
                        	                            <td class="txt_darkgrey_11" style="padding:0 5px 0 0; width:10px;">of</td>
                        	                            <td class="txt_black_11" style="padding:0 5px 0 0; width:20px;"><asp:Label ID="lblTotalPages" runat="server" CssClass="txt_darkgrey_11"></asp:Label></td>
                                                        <td style="padding:0 2px 0 0; width:30px;"><asp:LinkButton ID="btnNext" runat="server" CssClass="NAGIGATION_ITEM txt_white_12" Width="20px" Height="18px" OnClick="btnNext_Click"><div style="text-align:center; vertical-align:middle; padding-top:2px;">></div></asp:LinkButton></td>
                                                        <td style="padding:0 2px 0 0; width:30px;"><asp:LinkButton ID="btnLast" runat="server" CssClass="NAGIGATION_ITEM txt_white_12" Width="20px" Height="18px" OnClick="btnLast_Click"><div style="text-align:center; vertical-align:middle; padding-top:2px;">>|</div></asp:LinkButton></td>
                       	                
<%--                                                        <td>
                                                           <asp:LinkButton ID="btnFilterOptions" runat="server" CssClass="NAGIGATION_ITEM txt_white_12" Width="120px" Height="18px"><div style="text-align:center; vertical-align:middle; padding-top:2px;">FILTER OPTIONS</div></asp:LinkButton>
                                                        </td>--%>

                                                        <asp:Panel ID="pnlFilterOptions" Visible="true" runat="server">
                                                            <td style="padding:0 2px 0 5px; width:120px;" align="center" valign="middle" >
                                                                <input name="btnFilteredOptions" id="btnFilteredOptions" type="button" class="btnFilterOptions" style="width:120px; height:25px;" value="Filter Options" onclick="ShowHideDiv('divPopupFilterOptions', 'block'); AlignDivCenter('divPopupFilterOptions', 70, 50);"  />
                                                            </td>
                                                        </asp:Panel>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                         </asp:Panel>
                        </div>    
                    </div>



                 <%= AssetManagement.Proc.Gen.makeSpace(10, "", true)%> 

                  <div runat="server" id="dvRecords" visible="true" style="padding-bottom:10px;">
			           
                                    <div class="datagrid" style="text-align:right;">
                                        <span  class="txt_red_12">TOTAL ASSETS VALUE : <strong><asp:Label ID="lblBalance" runat="server"></asp:Label></strong></span>
                                    </div>

                                    <div class="datagrid">
                                        <table width="100%" border="0" cellpadding="1" cellspacing="0">
                                            <thead>
                                                    <tr>
                                                        <th width="180" align="center" valign="middle"><span class="txt_white_10" style="white-space:nowrap;"><a href="javascript:void(0);" onclick="OpenPopupDialog('divPopupDialog', 'ifOptions', 415, 900,'Asset_edit.aspx?new=true&page=Asset_list.aspx','PopUpBase PopPrimary');">ADD NEW</a></span></th>
                                                        <th width="100" align="left" bgcolor="#F2F2F2" class="txt_white_10" style="white-space:nowrap;"><strong>ASSET NO</strong></th>
                                                        <th align="left" bgcolor="#F2F2F2" class="txt_white_10" style="white-space:nowrap;"><strong>CATEGORY</strong></th>
                                                        <th align="left" bgcolor="#F2F2F2" class="txt_white_10" style="white-space:nowrap;"><strong>DESCRIPTION</strong></th>
                                                        <th align="left" bgcolor="#F2F2F2" class="txt_white_10" style="white-space:nowrap;"><strong>SERIAL NO</strong></th>
                                                        <th align="left" bgcolor="#F2F2F2" class="txt_white_10" style="white-space:nowrap;"><strong>CURRENT USERNAME</strong></th>
                                                        <th align="left" bgcolor="#F2F2F2" class="txt_white_10" style="white-space:nowrap;"><strong>VALUE</strong></th>
                                                    </tr>			        
                                            </thead>
                                            <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound"></asp:Repeater>
                                        </table>
                                    </div>
<%--                                </td>
                            </tr>--%>
                    </div>
                  </div>


                <div id="dvNoRecords" runat="server" visible="false" align="center">
                    <table align="center" border="0" width="100%" cellpadding="3" cellspacing="1" style="min-width:880px;">
                        <tr>
                            <td class="txt_red_cgothic_13" style="padding-top:10px;text-decoration: none; text-align:center; border-top:1px dotted grey;">
                                NO RESULTS FOUND.<br /><br />
                               <a href="Asset_list.aspx?DisposedAssets=false"> Back to asset list</a><br /><br />
                                 
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


        <!-- FILTER OPTIONS-->
        <div id="divPopupFilterOptions" style=" display:none; width:1200px; height:680px;" class="PopUpBase PopWhite">
            <table border="0" cellspacing="0" cellpadding="0" style="width:1200px; height:680px;">
                <tr>
                    <td colspan="3" valign="top" style="border-bottom:#000 solid 1px; padding:0 0 5px 0; height:10px;">
                        <div style="margin:0 0 2px 0; padding:0 0 5px 0; float:right;" class="txt_black_11">
                            <input name="btnCloseOptions" id="btnCloseOptions" type="button" class="BtnClosePopOff" value="X" onclick="ShowHideDiv('divPopupFilterOptions', 'none');" onmouseover="ChangeClass(this.id, 'BtnClosePop')" onmouseout="ChangeClass(this.id, 'BtnClosePopOff')" />
                        </div>
                    </td>
                </tr>


                <tr>
                    <th style="padding-top:10px;" class="headings">More Filter Options</th>
                    <th style="padding-top:10px; height:15px;" class="headings">Condition</th>
                    <th style="padding-top:10px;" class="headings">Category</th>
                </tr>

                <tr>
                    <td width="340px" valign="top" style="padding-top:12px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="float:left;">
                                    <asp:Label ID="Label1" CssClass="txt_green_cgothic_11" runat="server" Text="Specify filter bellow"></asp:Label>
                                    <%--<span class="txt_green_cgothic_11">Specify filter bellow</span>
                                    <asp:CheckBox ID="CheckBox1" runat="server" CssClass="txt_green_cgothic_11" Text="&nbsp;Select / Deselect All Condition" AutoPostBack="true" oncheckedchanged="chkSelectAllCondition_CheckedChanged"/>--%>
                                </div>
                                <div style="clear:both; height:550px; border:#006699 1px solid; white-space:nowrap;">
                                    <table>
                                        <tr>
                                            <td width="100px" align="left" valign="middle" style="margin:3px 3px 3px 3px;" class="txt_blue_cgothic_13"><strong>DCS ASSET NUMBER</strong></td>
                                            <td>
                                                <asp:TextBox ID="f_trackNo" Width="200px" CssClass="textbox textbox-input txt_blue_cgothic_13" runat="server"></asp:TextBox>
<%--                                                <asp:AutoCompleteExtender ServiceMethod="AssetNumbersAutoCompletionList"
                                                        MinimumPrefixLength="1" CompletionListItemCssClass="txt_blue_cgothic_13"    
                                                        CompletionInterval="10" 
                                                        EnableCaching="false"
                                                        CompletionSetCount="1" 
                                                        TargetControlID="f_trackNo"    
                                                        ID="AutoCompleteExtender1" 
                                                        runat="server" 
                                                        FirstRowSelected="false">
                                                </asp:AutoCompleteExtender>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100px" align="right" valign="middle" style="margin:3px 3px 3px 3px;" class="txt_blue_cgothic_13"><strong>PURCHASED DATE &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</strong><i>(Start)</i></td>
                                            <td><asp:TextBox ID="f_loadedStartDate" Width="150px" Enabled="false" CssClass="textbox textbox-input txt_blue_cgothic_13" runat="server"></asp:TextBox>
                                                <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Common/Graphics/Calendar_scheduleHS.png" />
                                                <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" EnableViewState="false" CssClass="MyCalendar" runat="server" TargetControlID="f_loadedStartDate" PopupButtonID="imgStartDate">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td align="right" class="txt_blue_cgothic_13"><i>(End)</i></td>
                                            <td><asp:TextBox ID="f_loadedEndDate" Width="150px" Enabled="false"  CssClass="textbox textbox-input txt_blue_cgothic_13" runat="server"></asp:TextBox>
                                                <asp:Image ID="imgEndDate" runat="server" ImageUrl="~/Common/Graphics/Calendar_scheduleHS.png" />
                                                <asp:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" EnableViewState="false" CssClass="MyCalendar" runat="server" TargetControlID="f_loadedEndDate" PopupButtonID="imgEndDate">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr><td colspan="2" style="border-top: 1px solid #006699;"></td></tr>


                                        <tr>
                                            <td width="100px" align="right" valign="middle" style="margin:3px 3px 3px 3px;" class="txt_blue_12"><strong>SORT/ORDER BY</strong></td>
                                            <td width="350px" align="left" valign="middle" style="margin:3px 3px 3px 3px;"> 
                                                <table border="0">
                                                    <tr>
                                                        <td width="100px" align="left" valign="middle" style="padding:0 0 0 10px;">  
                                                            <div class="dropdown txt_blue_12" style="border:1px solid #036; margin:3px 3px 3px 3px; padding:0 0 3px 0;">                                           
                                                                <asp:DropDownList ID="drpSoryBy" runat="server" CssClass="dropdown-select txt_blue_cgothic_13" Width="150px"> 
                                                                    <asp:ListItem Text="Asset Number" Value="an" Selected="True"></asp:ListItem>      
                                                                    <asp:ListItem Text="Purchased Date" Value="pd"></asp:ListItem>
                                                                    <asp:ListItem Text="Purchased Amount" Value="pd"></asp:ListItem>                                                           
                                                                    <asp:ListItem Text="Condition" Value="co"></asp:ListItem>
                                                                    <asp:ListItem Text="Category" Value="ca"></asp:ListItem>
                                                                    <asp:ListItem Text="Serial Number" Value="sn"></asp:ListItem>
                                                                    <asp:ListItem Text="Description" Value="de"></asp:ListItem>
                                                                </asp:DropDownList> 
                                                            </div>                                             
                                                        </td>

                                                        <td>
                                                            <div class="txt_blue_10" style="margin:3px 3px 3px 3px; padding:0 0 3px 0;">
                                                                <asp:RadioButtonList ID="rblOrderType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" CssClass="txt_blue_cgothic_13">
                                                                    <asp:ListItem Text="ASC&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Value="ASC" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="DESC" Value="DESC"></asp:ListItem>
                                                                </asp:RadioButtonList>
    	                                                    </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                        <tr><td colspan="2" style="border-top: 1px solid #006699;"></td></tr>

                                        <tr>
                                            <td colspan="2" align="right" valign="top" style="text-align:right;">
                                                <table>
                                                    <tr>
                                                            <td width="100px" align="left" valign="middle">  
                                                                <asp:Button ID="btnFetchData" runat="server" Text="Apply Filter" style="width:140px;" Height="25px" CssClass="btnNewBlue" OnClick="btnApplyFilter_Click"/>
                                                            </td>

                                                            <td align="left" valign="middle">  
                                                                <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" style="width:140px;" Height="25px" CssClass="btnFilterOptions" OnClick="btnClearFilter_Click"/>
                                                            </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>               
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>


                        <td width="180px" valign="top" style="padding-top:10px;">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="float:left;">
                                    <asp:CheckBox ID="chSelectAllConditions" runat="server" CssClass="txt_green_cgothic_11" Text="&nbsp;Select / Deselect All Condition" AutoPostBack="true" oncheckedchanged="chkSelectAllCondition_CheckedChanged"/>
                                </div>
                                <div style="clear:both; height:520px; overflow-y:scroll; border:#006699 1px solid; white-space:nowrap;">
                                    <asp:CheckBoxList ID="chkAssetCondition" runat="server" CssClass="txt_blue_cgothic_13" RepeatDirection="Vertical"></asp:CheckBoxList>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>

                    <td width="300px" valign="top" style="padding-top:10px;">
                        <asp:UpdatePanel ID="upnlOptions" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="float:left;">
                                    <asp:CheckBox ID="chkSelectAllCategories" runat="server" CssClass="txt_green_cgothic_11" Text="&nbsp;Select / Deselect All Categories" AutoPostBack="true" oncheckedchanged="chkSelectAllCategory_CheckedChanged"/>
                                </div>
                                <div style="clear:both; height:520px; overflow-y:scroll; border:#006699 1px solid; white-space:nowrap;">
                                    <asp:CheckBoxList ID="chkCategories" runat="server" CssClass="txt_blue_cgothic_13" RepeatDirection="Vertical"></asp:CheckBoxList>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        

   

       
       





</asp:Content>
