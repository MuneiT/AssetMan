<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeBehind="Asset_List.aspx.cs" Inherits="AssetManagement.Asset_List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register src="Common/UC/MessageBox.ascx" tagname="MessageBox" tagprefix="ucMB" %>

<%@ MasterType TypeName="AssetManagement.main" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cp_pageheading" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cp_pagedata" runat="server">

     <script type="text/javascript">
         Sys.Net.WebRequestManager.add_invokingRequest(onInvoke);
         Sys.Net.WebRequestManager.add_completedRequest(onComplete);

         function onInvoke(sender, args) {
             $find('<%= mpeProgress.ClientID %>').show();
         }

         function onComplete(sender, args) {
             $find('<%= mpeProgress.ClientID %>').hide();
         }


      </script>

 

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

     <asp:Panel ID="pnlProgress" runat="server" Style="display: none; width: 100%; height: 100%; z-index: 2001;">
        <div style="position: fixed; top: 0px; bottom: 0px; left: 0px; right: 0px; overflow: hidden; padding: 0; margin: 0; z-index: 100000;"></div>

        <div style="position: fixed; top: 50%; left: 50%; margin-top: -50px; margin-left: -50px; height:50px; width:50px; z-index: 100001;  background-color: #FFFFFF; border:1px solid #000000; background-image: url('Common/Graphics/loader_white.gif'); background-repeat: no-repeat; background-position:center;
         -webkit-border-radius:4px; -moz-border-radius: 4px; border-radius: 4px; -webkit-box-shadow: #000 0px 0px 30px; -moz-box-shadow: #000 0px 0px 30px; box-shadow: #000 0px 0px 30px; behavior: url(Common/Styles/PIE/PIE.htc); vertical-align:bottom; text-align:center;"></div>
    </asp:Panel>

     <ajaxToolKit:ModalPopupExtender 
        BackgroundCssClass="modalBackground"
        ID="mpeProgress"
        runat="server"
        TargetControlID="pnlProgress"
        PopupControlID="pnlProgress">
     </ajaxToolKit:ModalPopupExtender>

     <%--<%= AssetManagement.Proc.Gen.makeSpace(10, "", true)%>--%>

    

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

    
           <%--<asp:ScriptManager ID="ScriptManager1" runat="server"/>--%>

           <asp:UpdatePanel ID="upnlListing" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
              <div class="panel panel-primary">
                <div class="panel-heading" style="text-align:left;"><strong><asp:Literal ID="lit_Title" Text="ASSETS LIST" runat="server"></asp:Literal></strong></div>
                  <div class="panel-body"> 
                    <div runat="server"  visible="true" style="padding-top:10px;">
                      <div  style="padding-bottom:5px;">
                        <asp:Panel ID="pan_list" runat="server">
                       <table align="center" border="0" width="100%" cellpadding="3" cellspacing="1" style="min-width:880px;" bgcolor="#CCCCCC">
                           
                           
                           <tr>
                               
                                <td align="left">
                                        <table border="0" cellspacing="0" cellpadding="0">
                                         <tr>

                                         <asp:Panel ID="pnlExports" runat="server" Visible="false">
                                           <td style="padding:0 5px 0 0;" >
                                                <input name="btnFilterOptions" id="btnFilterOptions" type="button" class="btn btn-primary" style="width:120px; height:35px;" value="Filter Options" onclick="ShowHideDiv('divPopupFilterOptions', 'block'); AlignDivCenter('divPopupFilterOptions', 70, 50);"/>
                                                <%-- <button type="btnFilterOptions" class="btn btn-primary" onClick="javascript:ReverseDisplay('divPopupFilterOptions'); return false;"><span class="glyphicon glyphicon-filter"></span>&nbsp Filter Options</button>--%>
                                           </td>
                                            
                                                <td style="padding:0 5px 0 0;"><asp:Button ID="btnExportTOPdf" OnClick="btnExportToPDF_Click" runat="server" Text="Export To PDF"  CssClass="btn btn-warning"/></td>
                                                <td style="padding:0 5px 0 0;"><asp:Button ID="btnExportToExcel" OnClick="btnExportToExcel_Click" runat="server" Text="Export To MS Excel"  CssClass="btn btn-success"/></td>
                                                </asp:Panel>

                                         </tr>
                                        </table>

                                    </td>
                                    
                             <asp:Panel ID="ofNoRec" runat="server" Visible="false">
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

                                <td align="right">
                                    <table border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                          <td  class="txt_red_16">TOTAL ASSETS VALUE : <strong><asp:Label ID="lblBalance" runat="server"></asp:Label></strong></td>
                                        </tr>
                                    </table>
                                </td>

                                <tr>
                                    <td colspan="15" style="border-bottom:1px solid CornflowerBlue;"></td>
                                </tr>
                             </asp:Panel>


                            </tr>
                            
                                
                        </table>

                       <%= AssetManagement.Proc.Gen.makeSpace(10, "", true)%> 
                      </asp:Panel>
                      </div>
                    </div>
                  </div>

                    <div runat="server" id="dvRecords" visible="true" style="padding-top:10px;">
                        <table class="table_settings" width="100%" style="padding-bottom:10px;" cellpadding="3" cellspacing="0"  bgcolor="#CCCCCC">
                           <thead style="border-bottom:3px solid;">
                               <tr class="txt_blue_cgothic_12">
                                        <th width="90" align="center" valign="middle"><strong><a href="javascript:void(0);" onclick="OpenPopupDialog('divPopupDialog', 'ifOptions', 550, 950,'Asset_edit.aspx?new=true&page=Asset_list.aspx','PopUpBase PopPrimary');">ADD NEW</a></strong></th>
                                        <th width="100" align="left" valign="middle"><span class="txt_blue_cgothic_12" style="white-space:nowrap;">ASSET NO</span></th>
                                        <th width="left" align="left" valign="middle"><span class="txt_blue_cgothic_12" style="white-space:nowrap;">ASSET CATEGORY</span></th>
                                        <th width="left" align="left" valign="middle"><span class="txt_blue_cgothic_12" style="white-space:nowrap;">ASSET DESCRIPTION</span></th> 
                                        <th width="left" align="left" valign="middle"><span class="txt_blue_cgothic_12" style="white-space:nowrap;">ASSET CONDITION</span></th> 
                                        <th width="left" align="left" valign="middle"><span class="txt_blue_cgothic_12" style="white-space:nowrap;">ASSET SERIAL NO</span></th> 
                                        <th width="left" align="left" valign="middle"><span class="txt_blue_cgothic_12" style="white-space:nowrap;">CURRENT ASSET OWNER</span></th> 
                                        <th width="left" align="left" valign="middle"><span class="txt_blue_cgothic_12" style="white-space:nowrap;">ASSET VALUE</span></th> 
                                        <th width="left" align="left" valign="middle"><span class="txt_blue_cgothic_12" style="white-space:nowrap;">ASSET DURATION</span></th> 
                                 </tr>
                            </thead>
                         <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound"></asp:Repeater>
                       </table>
                   </div>

                    <div id="dvNoRecords" runat="server" visible="false" align="center">
                    <table align="center" border="0" width="100%" cellpadding="3" cellspacing="1" style="min-width:880px;">
                    <tr>
                        <td class="txt_red_cgothic_13" style="padding-top:10px;text-decoration: none; text-align:center;">
                            NO ASSETS CONFIGURED<br /><br />
                            <a href="javascript:void(0);" onclick="OpenPopupDialog('divPopupDialog', 'ifOptions', 465, 900,'Asset_edit.aspx?new=true&page=Asset_list.aspx','PopUpBase PopPrimary');">Click here to add New Asset</a>
                        </td>
                    </tr>
                    </table>
                </div>
               
               
               </div>
        </ContentTemplate>
              
    </asp:UpdatePanel>


                   <!-- FILTER OPTIONS-->

      <div id="divPopupFilterOptions" style=" display:none; width:900px; height:650px;" class="PopUpBase PopWhite panel panel-primary ">
      <%--<div id="div1" style=" display:none; width:1110px; height:650px;" class="PopUpBase PopWhite panel panel-primary ">--%>
             <div class="panel-heading"><strong><asp:Literal ID="Literal1" Text="ICT Asset Filter Options" runat="server"></asp:Literal></strong></div>
                <div class="panel-body">
                 <table border="0" cellspacing="0" cellpadding="0" style="width:800px; height:575px;">
                <%-- <table border="0" cellspacing="0" cellpadding="0" style="width:1060px; height:575px;">--%>
                <tr>
                    <td colspan="4" valign="top" style="height:40px; padding-top:10px; margin:0 0 4px 0;">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                           <%--<td>
                                             <div class="txt_blue_10" style="margin:3px 3px 3px 3px; padding:0 0 3px 0;">
                                                    <asp:RadioButtonList ID="rblDateOrMove" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" CssClass="txt_blue_cgothic_13">
                                                        <asp:ListItem Text="PURCHASED DATE&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Value="Pur" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="ASSIGNED DATE" Value="Mov"></asp:ListItem>
                                                    </asp:RadioButtonList>
    	                                        </div>
                                            </td>--%>
                                            

                                             <td align="right" class="txt_blue_cgothic_13" style="margin:3px 3px 3px 3px;" 
                                                valign="middle" width="200px"><strong>SEARCH ASSET BY </strong></td>
                                            <td align="left" style="padding:0 0 0 10px;" valign="middle">
                                                <table>
                                                    <tr>
                                                        <td width="120px" valign="middle">
                                                            <div class="dropdown txt_blue_12" 
                                                                style="border:1px solid #036; margin:3px 3px 3px 3px; padding:0 0 3px 0;">
                                                                <asp:DropDownList ID="drpSearchBy" runat="server" AutoPostBack="true" 
                                                                    CssClass="dropdown-select" 
                                                                    onselectedindexchanged="f_sections_SelectedIndexChanged" Width="150px">
                                                                    <asp:ListItem Selected="True" Text="Select Search Text" Value=""></asp:ListItem>
                                                                    <asp:ListItem Text="Persal Number" Value="persal"></asp:ListItem>
                                                                    <asp:ListItem Text="Name or Surname" Value="employee"></asp:ListItem>
                                                                    <asp:ListItem Text="Asset Number" Value="asset_no"></asp:ListItem>
                                                                    <asp:ListItem Text="Serial Number" Value="serial"></asp:ListItem>
                                                                    <asp:ListItem Text="Purchased Date" Value="pur_date"></asp:ListItem>
                                                                    <asp:ListItem Text="Assigned Date" Value="mov_date"></asp:ListItem>
                                                                 </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td align="left" class="txt_blue_11" style="padding:0 0 0 3px;">
                                                            <asp:Panel ID="pnlSearchBy" runat="server" Visible="false">
                                                                <asp:TextBox ID="f_searchby" runat="server" CssClass="textbox textbox-input" 
                                                                    Width="200px"></asp:TextBox>
                                                            </asp:Panel>

                                                            <asp:Panel ID="pnlSearchText" runat="server" Visible="false">
                                                                <asp:TextBox ID="f_SearchText" runat="server" CssClass="textbox textbox-input" 
                                                                    Width="200px"></asp:TextBox>
                                                            </asp:Panel>
                                                            <%--<asp:Panel ID="pnlStoreRoom" runat="server" Visible="false">
                                                            <div class="dropdown txt_black_10" style="border:1px solid #036; margin:3px 3px 3px 3px; padding:0 0 3px 0;"> 
                                                                <asp:DropDownList ID="drpStore" Width="150px" runat="server" CssClass="dropdown-select">
                                                                </asp:DropDownList>                                    
                                                            </div>
                                                            </asp:Panel>--%>


                                                            <asp:Panel ID="pnlPurchased" runat="server" Visible="false">
                                                                <td width="50px" valign="middle" align="right" class="txt_blue_cgothic_13">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>(From)</i>&nbsp;&nbsp;&nbsp;</td>
                                                                <td width="120px" valign="middle" ><asp:TextBox ID="f_fromDate" Enabled="true" Width="100px" placeholder="yyyy-mm-dd" CssClass="textbox textbox-input txt_blue_cgothic_13" runat="server"></asp:TextBox>

                                             
                                                                <asp:Image ID="cldFromDate" runat="server" ImageUrl="~/Common/Graphics/Calendar_scheduleHS.png" Height="20px" />
                                                                         <ajaxToolKit:CalendarExtender ID="CalendarExtender1" CssClass="MyCalendar" Format="yyyy-MM-dd" runat="server" TargetControlID="f_fromDate" PopupButtonID="cldFromDate">
                                                                         </ajaxToolKit:CalendarExtender>
                                                                </td>

                                                                <td width="50px" valign="middle" align="right" class="txt_blue_cgothic_13">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>(To)</i>&nbsp;&nbsp;&nbsp;</td>
                                                                    <td valign="middle"><asp:TextBox ID="f_ToDate" Enabled="true" Width="100px" placeholder="yyyy-mm-dd" CssClass="textbox textbox-input txt_blue_cgothic_13" runat="server"></asp:TextBox>

                                              
                                                                    <asp:Image ID="cldToDate" runat="server" ImageUrl="~/Common/Graphics/Calendar_scheduleHS.png" Height="20px" />
                                                                    <ajaxToolKit:CalendarExtender ID="CalendarExtender3" CssClass="MyCalendar" Format="yyyy-MM-dd" runat="server" TargetControlID="f_ToDate" PopupButtonID="cldToDate">
                                                                    </ajaxToolKit:CalendarExtender>                                       
                                                                </td>
                                                            </asp:Panel>

                                                            <asp:Panel ID="pnlMoveDate" runat="server" Visible="false">
                                                                <td width="50px" valign="middle" align="right" class="txt_blue_cgothic_13">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>(From)</i>&nbsp;&nbsp;&nbsp;</td>
                                                                <td width="120px" valign="middle" ><asp:TextBox ID="f_mov_from" Enabled="true" Width="100px" placeholder="yyyy-mm-dd" CssClass="textbox textbox-input txt_blue_cgothic_13" runat="server"></asp:TextBox>

                                             
                                                                <asp:Image ID="cldMoveFromDate" runat="server" ImageUrl="~/Common/Graphics/Calendar_scheduleHS.png" Height="20px" />
                                                                         <ajaxToolKit:CalendarExtender ID="CalendarExtender2" CssClass="MyCalendar" Format="yyyy-MM-dd" runat="server" TargetControlID="f_mov_from" PopupButtonID="cldMoveFromDate">
                                                                         </ajaxToolKit:CalendarExtender>
                                                                </td>

                                                                <td width="50px" valign="middle" align="right" class="txt_blue_cgothic_13">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>(To)</i>&nbsp;&nbsp;&nbsp;</td>
                                                                    <td valign="middle"><asp:TextBox ID="f_mov_To" Enabled="true" Width="100px" placeholder="yyyy-mm-dd" CssClass="textbox textbox-input txt_blue_cgothic_13" runat="server"></asp:TextBox>

                                              
                                                                    <asp:Image ID="cldMoveToDate" runat="server" ImageUrl="~/Common/Graphics/Calendar_scheduleHS.png" Height="20px" />
                                                                    <ajaxToolKit:CalendarExtender ID="CalendarExtender4" CssClass="MyCalendar" Format="yyyy-MM-dd" runat="server" TargetControlID="f_mov_To" PopupButtonID="cldMoveToDate">
                                                                    </ajaxToolKit:CalendarExtender>                                       
                                                                </td>
                                                            </asp:Panel>
                                                                                                                       
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>

                                             
                                        </tr>

                                        <tr>
                                            

                                            <td width="100px" align="right" valign="middle" style="margin:3px 3px 3px 3px;" class="txt_blue_cgothic_13"><strong>SORT/ORDER BY</strong></td>
                                            <td width="350px" align="left" valign="middle" style="margin:3px 3px 3px 3px;"> 
                                                <table border="0">
                                                    <tr>
                                                        <td width="100px" align="left" valign="middle" style="padding:0 0 0 10px;">  
                                                            <div class="dropdown txt_blue_12" style="border:1px solid #036; margin:3px 3px 3px 3px; padding:0 0 3px 0;">                                           
                                                                <asp:DropDownList ID="drpSortBy" runat="server" CssClass="dropdown-select txt_blue_cgothic_13" Width="150px"> 
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
                                       

                                     </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExportToPDF"/>
                                    <asp:PostBackTrigger ControlID="btnExportToExcel"/>
                                </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>


                <tr>
                    <td style="padding-top:20px;" valign="top" >
                        <ajaxToolKit:accordion id="feedbackAccordion" runat="server" selectedindex="0"  headercssclass="accordionHeader"
                        headerselectedcssclass="accordionHeaderSelected" contentcssclass="accordionContent"
                        fadetransitions="false" framespersecond="40" transitionduration="250" autosize="None"
                        requireopenedpane="false" SuppressHeaderPostbacks="false">
                           
                            <Panes>                              
                                <ajaxToolKit:AccordionPane ID="apCondition" runat="server">
                                    <Header>ASSET CONDITION</Header>
                                    <Content>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div style="float:left;">
                                                    <span class="txt_green_cgothic_11" Text="">&nbsp;Select Condition</span>
                                                </div>
                                                <div style="clear:both; height:250px; overflow-y:scroll; border:#006699 1px solid; white-space:nowrap;">
                                                    <asp:CheckBoxList ID="chkAssetCondition" runat="server" CssClass="txt_blue_cgothic_13" RepeatDirection="Vertical" CellPadding="5" CellSpacing="5"></asp:CheckBoxList>
                                                </div>

                         
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </Content>
                                </ajaxToolKit:AccordionPane>


                                   <ajaxToolKit:AccordionPane ID="apCategory" runat="server">
                                    <Header> ASSET CATEGORY</Header>
                                    <Content>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div style="float:left;">
                                                    <span class="txt_green_cgothic_11" Text="">&nbsp;Select Category</span>
                                                </div>
                                                <div style="clear:both; height:250px; overflow-y:scroll; border:#006699 1px solid; white-space:nowrap;">
                                                    <asp:CheckBoxList ID="chkCategories" runat="server" CssClass="txt_blue_cgothic_13" RepeatDirection="Vertical" CellPadding="5" CellSpacing="5"></asp:CheckBoxList>
                                                </div>
                                            </ContentTemplate>

                                        </asp:UpdatePanel>
                                    </Content>
                                </ajaxToolKit:AccordionPane>
                                
                                        <ajaxToolKit:AccordionPane ID="AccordionPane1" runat="server">
                                    <Header> ASSET STATUS</Header>
                                    <Content>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div style="float:left;">
                                                    <span class="txt_green_cgothic_11" Text="">&nbsp;Select Status</span>
                                                </div>
                                                <div style="clear:both; height:250px; overflow-y:scroll; border:#006699 1px solid; white-space:nowrap;">
                                                    <asp:CheckBoxList ID="chkStatus" runat="server" CssClass="txt_blue_cgothic_13" RepeatDirection="Vertical" CellPadding="5" CellSpacing="5"></asp:CheckBoxList>
                                                </div>
                                            </ContentTemplate>

                                        </asp:UpdatePanel>
                                    </Content>
                                </ajaxToolKit:AccordionPane>
                            </Panes>
                          </ajaxToolKit:accordion>
                     
                    </td>
                </tr>

                <tr>
                    <td colspan="3" valign="top" style="padding:2px 0 0 0; height:10px;">
                        <div style="margin:0 0 2px 0; padding:0 0 5px 0; float:right;" class="txt_black_11">
                            <asp:Button ID="btnApplyFilter" Width="120px" Height="35px" runat="server" Text="Apply Filter" OnClick="btnApplyFilter_Click"  CssClass="btn btn-success"/>&nbsp;&nbsp;
                            <asp:Button ID="btnClearFilter" Width="120px" Height="35px" runat="server" Text="Clear Filter" OnClick="btnClearFilter_Click"  CssClass="btn btn-primary"/>&nbsp;&nbsp;
                            <input name="btnCloseOptions" id="btnCloseOptions" type="button" class="btn btn-danger" style="width:120px; height:35px;" value="Close" onclick="ShowHideDiv('divPopupFilterOptions', 'none');"/>

                        </div>
                    </td>
                </tr>

            </table>
            </div>
        </div>   

         

 
</asp:Content>
