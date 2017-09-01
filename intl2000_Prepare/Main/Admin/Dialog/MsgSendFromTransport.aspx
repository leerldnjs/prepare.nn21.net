<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MsgSendFromTransport.aspx.cs" Inherits="Admin_Dialog_MsgSendFromTransport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
   <title></title>
   <script type="text/javascript">
      var BBHPk;
      var ACCOUNTID;
      var OURBRANCHPK;
      var GUBUN;
      var STEPCL;
      var MasterBLNAirCompanyName = "";
      var WHO = "1";
      var RequestFormPkSum = "";
      var CountCheck = 0;

      window.onload = function () {
         BBHPk = dialogArguments[0];
         ACCOUNTID = dialogArguments[1];
         OURBRANCHPK = dialogArguments[2];         
         STEPCL = dialogArguments[3];         
         EachInfo = dialogArguments[4].split("#@!");         
         document.getElementById("PnMsgMain").innerHTML = "#Name : " + EachInfo[0] + "<br />#MASTER B/L : " + EachInfo[1] + "<br/><br/>";
         MakeCompanyS();
      }
      function MakeCompanyS() {
         if (form1.Shipper.checked == true) { WHO = "0"; }
         Admin.MakeCompanyS_MsgSendFromTransport(BBHPk, WHO,STEPCL, function (result) {
            var Temp="";
            for (var i = 0; i < result.length; i++) {
               var Row = result[i].split("#@!");
               Temp += "<tr><td><input type=\"checkbox\" checked = \"checked\" id=\"Company[" + i + "]\" value=\"" + Row[0] + "\" /><label for=\"Company[" + i + "]\">" + Row[1] + "</label></td><td>" + Row[2] + "</td><td>" + Row[3] + "</td><td>" + Row[4] + "</td></tr>";
               CountCheck++;
            }
            document.getElementById("CompanyS").innerHTML = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td class='THead1' style=\"width:75px;\" >고객번호</td><td class='THead1' style=\"width:75px;\" >DepartureDate</td><td class='THead1' style=\"width:75px;\" >ArrivalDate</td><td class='THead1' style=\"width:75px;\" >BoxCount</td></tr>" + Temp + "</table>";
         }, function (result) { alert("ERROR : " + result); });
      }
      function BTNSubmitClick() {
         if (confirm("단체메세지가 발송됩니다.")) {
            if (form1.Shipper.checked == true) { WHO = "0"; }
            for (var i = 0; i < CountCheck; i++) {
               if (document.getElementById("Company[" + i + "]").checked) {
                  RequestFormPkSum += document.getElementById("Company[" + i + "]").value + "#@!";
               }
            }            
            Admin.MsgSendFromTransport(RequestFormPkSum, ACCOUNTID, OURBRANCHPK, form1.TBSMS.value, WHO, STEPCL, function (result) {
               if (result != "1") {
                  alert(result);
                  return false;
               }
               alert(form1.dhksfy.value); window.returnValue = true; returnValue = "Y"; self.close();
            }, function (result) { alert("ERROR : " + result); });
         } else {
            RequestFormPkSum = "";
         }
         
      }

   </script>
   <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #999999; padding-left: 10px; padding-right: 10px; width: 480px; overflow-x: hidden;">
   <form id="form1" runat="server">
      <asp:ScriptManager ID="SM" runat="server">
         <Services>
            <asp:ServiceReference Path="~/WebService/Admin.asmx" />
         </Services>
      </asp:ScriptManager>
      <div style="width: 420px; padding-left: 10px; padding-right: 10px; background-color: white;">
         <div id="PnMsgMain"></div>
         SMS<br />
         <textarea id="TBSMS" cols="50" rows="7"></textarea>
         <p style="padding: 10px;">
            <input type="radio" name="WHO" id="Shipper" value="0" onclick="MakeCompanyS();"/><label for="Shipper">Shipper</label>
            <input type="radio" name="WHO" id="Consignee" value="1" checked="checked" onclick="MakeCompanyS();"/><label for="Consignee">Consignee</label>
         </p>
         <div id="CompanyS"></div>
         <p style="text-align: center; padding: 10px;">
            <input type="button" value="확인" style="width: 100px; height: 50px;" onclick="BTNSubmitClick();" />
         </p>
         <input type="hidden" id="dhksfy" value="<%=GetGlobalResourceObject("qjsdur", "dhksfy") %>" />
         <input type="hidden" id="K1Check" />

      </div>
   </form>
</body>
</html>
