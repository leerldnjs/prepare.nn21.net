<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="CompanyView.aspx.cs" Inherits="Admin_CompanyView" %>

<%@ Register Src="LogedWithoutRecentRequest.ascx" TagName="LogedWithoutRecentRequest" TagPrefix="uc1" %>
<%@ Register Src="../CustomClearance/Loged.ascx" TagName="Loged" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title></title>
   <style type="text/css">
      .tdSubT {
         border-bottom: 2px solid #93A9B8;
         padding-top: 30px;
         padding-bottom: 3px;
      }

      .td01 {
         width: 90px;
         background-color: #f5f5f5;
         text-align: center;
         height: 25px;
         border-bottom: 1px dotted #E8E8E8;
      }

      .td02 {
         width: 200px;
         padding-left: 10px;
         border-bottom: 1px dotted #E8E8E8;
         background-color: White;
      }

      .td03 {
         width: 110px;
         padding-left: 10px;
         border-bottom: 1px dotted #E8E8E8;
         background-color: White;
      }

      .td023 {
         width: 410px;
         padding-bottom: 4px;
         padding-left: 10px;
         border-bottom: 1px dotted #E8E8E8;
         background-color: White;
      }

      .tdStaffBody {
         text-align: center;
         padding: 5px;
         border: dotted 1px #E8E8E8;
         background-color: White;
      }

      .BB {
         border-bottom: 1px solid #93A9B8;
      }

      .BR {
         border-right: 1px solid #93A9B8;
      }
   </style>
   <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
   <script src="../Common/RegionCode.js?version=20131029" type="text/javascript"></script>
   <script type="text/javascript">
      function SendSMSAccountInfo(companyname, name, mobile, id, pk) {
         var dialogArgument = new Array();
         dialogArgument[0] = companyname;
         dialogArgument[1] = name;
         dialogArgument[2] = mobile;
         dialogArgument[3] = id;
         dialogArgument[4] = pk;
         var retVal = window.showModalDialog('./Dialog/SendAccountInfoBySMS.aspx', dialogArgument, "dialogHeight:500px; dialogWidth:480px; resizable:1; status:0; scroll:1; help:0; ");
         if (retVal == "Y") { window.document.location.reload(); }
      }
      function FileDelete(filepk) {
         if (confirm("Will be Deleted. Right?")) {
            Admin.FileDelete(filepk, "", function (result) {
               if (result == "1") {
                  alert("Success");
                  location.reload();
               }
            }, function (result) { alert("ERROR : " + result); });
         }
      }

      function DeleteRelatedCompany(pk, gubun) {
         var msg;
         if (gubun == "0") {
            msg = "관리업체를 삭제해도 해당 업체의 정보는 사라지지 않습니다.";
         }
         else {
            msg = "해당 거래처와의 연결만 삭제됩니다. ";
         }

         if (confirm(msg)) {
            Admin.DeleteCompanyRelation(pk, function (result) {
               if (result == "1") {
                  alert("Success");
                  location.reload();
               }
            }, function (result) { alert("ERROR : " + result); });
         }
      }
	   function ShowFreightChargeView(RequestFormPk, SorC) {
		   window.open('../Request/FreightChargeView.aspx?S=' + RequestFormPk + "&G=" + SorC.toUpperCase(), '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=800px;');
	   }
      function DELETESTAFF(pk) {
         if (confirm("Will be Deleted. Right?")) {
            Admin.DelectAccount(pk, function (result) {
               if (result == "1") {
                  alert("Success");
                  location.reload();
               }
            }, function (result) { alert("ERROR : " + result); });
         }
      }
      function Goto(value) {
      	switch (value) {
      		case "back": history.back(); break;
      		case "basic": location.href = "CompanyInfo.aspx?M=View&S=" + form1.HCompanyPk.value; break;
      		case "test": location.href = "CompanyInfotest.aspx?M=View&S=" + form1.HCompanyPk.value; break;
      		case "customer": location.href = "CompanyInfoCustomer.aspx?S=" + form1.HCompanyPk.value; break;
      		case "request": location.href = "CompanyInfoRequestList.aspx?G=CI&S=" + form1.HCompanyPk.value; break;
			case "SelectAACO": window.open('./Dialog/SelectAACO.aspx?S=' + form1.HCompanyPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=700px; width=600px;'); break;
      		case "talkBusiness": window.open('./Dialog/TalkBusiness.aspx?S=' + form1.HCompanyPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=700px; width=600px;'); break;
      		case "addcustomer": window.open('../Request/Dialog/OwnCustomerAdd.aspx?S=' + form1.HCompanyPk.value + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	650px; width=900px;'); break;
				case "fileupload": window.open('./Dialog/FileUpload.aspx?G=1&S=' + form1.HCompanyPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;'); break;
				case "Clearance": window.open('./Dialog/FileUpload.aspx?G=99&S=' + form1.HCompanyPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;'); break;
      		case "Tradingtransfer": window.open('./Dialog/Tradingtransfer.aspx?&S=' + form1.HCompanyPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;'); break;
      		case "RequestWrite": location.href = "../Request/RequestWrite.aspx?G=Admin&P=" + form1.HCompanyPk.value; break;
      		case "TalkBusinessforBusiness": window.open('./Dialog/TalkBusinessforBusiness.aspx?S=' + form1.HCompanyPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=700px; width=600px;'); break;
      		case "TalkBusinessforRelatedCompany": window.open('./Dialog/TalkBusinessforRelatedCompany.aspx?S=' + form1.HCompanyPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=700px; width=600px;'); break;
      	}
      }

	   function CommentDelete(TalkBusinessPk) {
		   if (confirm("this comment will be deleted")) {
			   var data = {
				   Comment_Pk: TalkBusinessPk
			   }
			   $.ajax({
				   type: "POST",
				   url: "/Process/HistoryP.asmx/Delete_Comment",
				   data: JSON.stringify(data),
				   dataType: "json",
				   contentType: "application/json; charset=utf-8",
				   success: function (result) {
					   alert("성공");
					   location.reload();
				   },
				   error: function (result) {
					   alert('failure : ' + result);
				   }
			   });
		   }
	   }
	   function DELETEWAREHOUSE(pk) {
		   if (confirm("this row will be delete")) {
			   Admin.DeleteWarehouse(pk, function (result) {
				   if (result == "1") {
					   alert("Success");
					   location.reload();
				   }
			   }, function (result) { alert("ERROR : " + result); });
		   }
	   }
	   function SetCompanyAddInfo(Title, Value) {
		   Admin.SetCompanyAddInfo(document.getElementById("HCompanyPk").value, Title, Value, function (result) {
			   if (result == "1") {
				   alert("Success");
				   location.reload();
			   }
		   }, function (result) { alert("ERROR : " + result); });
	   }

	   function goto_hscode() {
		   location.href = "../CustomClearance/HSCode_Tariff.aspx?S=" + form1.HCompanyPk.value;
	   }
   </script>
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px;">
   <form id="form1" runat="server">
      <uc1:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server" />
      <uc2:Loged ID="Loged1" runat="server" Visible="false" />
      <asp:ScriptManager ID="SM" runat="server">
         <Services>
            <asp:ServiceReference Path="~/WebService/Admin.asmx" />
         </Services>
      </asp:ScriptManager>
      <div style="background-color: White; min-height: 500px;">
         <div style="padding-right: 20px; padding-top: 20px; text-align: right;">
            <%=HtmlButtonforBusiness %>
            <%=HtmlButton %>
         </div>
         <div style="width: 520px; float: right;">
            <%=CompanyInfo %>
            <%=StaffInfo %>
            <%=WarehouseInfo %>
         </div>
         <div style="width: 360px; padding-top: 10px; padding-left: 10px;">
            <%=TalkBusiness %>
            <%=RecentRequest %>
            <%=FileList %>
			<%=ClearanceList %>
            <%=RelatedCompany %>
         </div>
         <div style="width: 840px; padding: 20px; clear: both;">
            <table border="0" cellpadding="0" cellspacing="0">
               <tr style="height: 1px;">
                  <td style="width: 180px; font-size: 1px;">&nbsp;</td>
                  <td style="width: 25px; font-size: 1px;">&nbsp;</td>
                  <td style="width: 25px; font-size: 1px;">&nbsp;</td>
                  <td style="width: 180px; font-size: 1px;">&nbsp;</td>
                  <td style="width: 90px; font-size: 1px;">&nbsp;</td>
                  <td style="width: 90px; font-size: 1px;">&nbsp;</td>
                  <td style="width: 25px; font-size: 1px;">&nbsp;</td>
                  <td style="width: 25px; font-size: 1px;">&nbsp;</td>
                  <td style="width: 180px; font-size: 1px;">&nbsp;</td>
               </tr>
               <%--<%=CompanyRelatedInfo%>--%>
            </table>
         </div>
         <input type="hidden" id="HCompanyPk" value="<%=CompanyPk %>" />
         <input type="hidden" id="HAccountID" value="<%=Session["ID"]  %>" />
      </div>
   </form>
</body>
</html>