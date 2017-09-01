<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnCustomerList.aspx.cs" Inherits="Member_OwnCustomerList" %>
<meta http-equiv="X-UA-Compatible" content="IE=10"> 
<%@ Register Src="../Member/LogedTopMenu.ascx" TagName="Loged" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=GetGlobalResourceObject("Member", "TitleCustomerInfo") %></title>
    <link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .UnderLineBlue {
            border-bottom-width: 2px;
            border-bottom-style: solid;
            border-bottom-color: #93A9B8;
        }

        .UnderLineDotted {
            border-bottom-style: dotted;
            border-bottom-color: #E8E8E8;
            border-bottom-width: 1px;
            padding-top: 4px;
            padding-bottom: 4px;
        }
    </style>
    <script type="text/javascript">
        window.onload = function () {
            $(".NavInformation").addClass("active");
        }
        function CompanyCustomerListDel(pk) { if (confirm("거래처정보를 삭제 하시겠습니까?")) { Member.OwnCustomerListTo99(pk, DelSucced); } }
        function DelSucced(result) { alert("삭제완료"); location.href = "OwnCustomerList.aspx"; }
        function GotoOwnCustomerListView(pk) { location.href = "OwnCustomerView.aspx?pk=" + pk; }
    </script>
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px;">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Member.asmx" />
            </Services>
        </asp:ScriptManager>
        <uc1:Loged ID="Loged1" runat="server" />
        <div class=" ContentsTopMenu">
            <p>
                <a href="OwnCustomerList.aspx"><strong><%=GetGlobalResourceObject("Member", "InfoCustomer")%></strong></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyCompanyView.aspx"><%=GetGlobalResourceObject("Member", "InfoCompany")%></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyAccountView.aspx"><%=GetGlobalResourceObject("Member", "InfoAccount")%></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyStaffView.aspx"><%=GetGlobalResourceObject("qjsdur", "djqandnjswjdqh") %></a>
            </p>
            <table border='0' cellpadding='0' cellspacing='0' style="width: 850px;">
                <tr>
                    <td class='UnderLineBlue' style='text-align: center; height: 30px;'>연번</td>
                    <td class='UnderLineBlue' style='text-align: center; width: 80px; height: 30px;'><%=GetGlobalResourceObject("RequestForm", "ListTitle_CompanyCode")%></td>
                    <td class='UnderLineBlue' style='text-align: center; width: 220px;'><%=GetGlobalResourceObject("Member", "CompanyName")%></td>
                    <td class='UnderLineBlue' style='text-align: center; width: 110px;'>TEL / FAX</td>
                    <td class='UnderLineBlue' style='text-align: center; width: 190px;'>E-Mail</td>
                    <td class='UnderLineBlue' style='text-align: center;'>Memo</td>
                    <td class='UnderLineBlue' style='text-align: center; width: 50px;'>Detail</td>
                    <td class='UnderLineBlue' style='text-align: center; width: 50px;'>Delete</td>
                </tr>
                <%=OwnCustomerListHTML %>
            </table>
        </div>
    </form>
</body>
</html>
