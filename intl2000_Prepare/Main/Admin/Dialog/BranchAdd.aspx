<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="BranchAdd.aspx.cs" Inherits="Admin_Dialog_BranchAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=GetGlobalResourceObject("Admin", "BranchAddTitle")%></title>

    <style type="text/css">
        .tdSubT
        {
            border-bottom-width: 2px;
            border-bottom-style: solid;
            border-bottom-color: #93A9B8;
            padding-top: 30px;
            padding-bottom: 3px;
        }

        .td01
        {
            background-color: #f5f5f5;
            text-align: center;
            height: 25px;
            width: 150px;
            padding-top: 4px;
            padding-bottom: 4px;
            border-bottom: 1px dotted #E8E8E8;
        }

        .td02
        {
            width: 250px;
            padding-top: 4px;
            padding-bottom: 4px;
            padding-left: 10px;
            border-bottom: 1px dotted #E8E8E8;
            background-color: White;
        }

        .td023
        {
            width: 740px;
            padding-top: 4px;
            padding-bottom: 4px;
            padding-left: 10px;
            border-bottom: 1px dotted #E8E8E8;
            background-color: White;
        }

        .tdStaffBody
        {
            text-align: center;
            padding: 5px;
            border: dotted 1px #E8E8E8;
            background-color: White;
        }
    </style>
    <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var RESPONSIBLESTAFF;
        var IsThirdRegion = false;
        var GUBUNCL = "9";
        //0	본인이 가입 
        //1	관리자 임의등록
        //2	SUB
        //9 지사등록

        function delopt(selectCtrl, mTxt) {
            if (selectCtrl.options) {
                for (i = selectCtrl.options.length; i >= 0; i--) {
                    selectCtrl.options[i] = null;
                }
            }
            selectCtrl.options[0] = new Option(mTxt, "")
            selectCtrl.selectedIndex = 0;
        }

        function LoadRegionCodeCountry(selectID) {
            delopt(document.getElementById("RegionCode[0]"), "Country");
            delopt(document.getElementById("RegionCode[1]"), "Area");
            delopt(document.getElementById("RegionCode[2]"), "Detail");
            Admin.LoadReginoCodeCountry(function (result) {
                for (var i = 0; i < result.length; i++) {
                    var thisR = result[i].split("$");
                    document.getElementById(selectID).options[i + 1] = new Option(thisR[1], thisR[0]);
                }
            }, function (result) { alert("ERROR : " + result); });
        }

        function LoadRegionCode(thisC) {
            if (thisC == "0") {
                delopt(document.getElementById("RegionCode[1]"), "Area");
                delopt(document.getElementById("RegionCode[2]"), "Detail");
            }
            else if (thisC == "1" && IsThirdRegion == true) {
                delopt(document.getElementById("RegionCode[2]"), "Detail");
            }

            if (thisC == "0" || IsThirdRegion == true) {
                Admin.LoadRegionCode(document.getElementById("RegionCode[" + thisC + "]").value, function (result) {
                    if (thisC == "0") {
                        if (result[0].substr(0, 1) == "7" ) {
                            document.getElementById("RegionCode[1]").style.visibility = "visible";
                            document.getElementById("RegionCode[2]").style.visibility = "visible";
                            IsThirdRegion = true;
                        }
                        else {
                            document.getElementById("RegionCode[1]").style.visibility = "visible";
                            document.getElementById("RegionCode[2]").style.visibility = "hidden";
                            IsThirdRegion = false;
                        }
                    }
                    for (var i = 1; i < result.length; i++) {
                        var thisR = result[i].split("$");
                        document.getElementById("RegionCode[" + (parseInt(thisC) + 1) + "]").options[i] = new Option(thisR[1], thisR[0]);
                    }
                }, function (result) { alert("ERROR : " + result); });
            }
            form1.RegionCode.value = document.getElementById("RegionCode[" + thisC + "]").value;
        }

        window.onload = function () {
            var DA = dialogArguments.toString().split("!");
            form1.HAccountID.value = DA[0];
            RESPONSIBLESTAFF = DA[0];
            LoadRegionCodeCountry("RegionCode[0]");
        }

        // 지사창고추가
        function InsertBranchStorage() {
            var objTable = document.getElementById("TabBranchStorage");
            objTable.appendChild(document.createElement("TBODY"));
            var lastRow = objTable.rows.length;
            var thisLineNo = lastRow - 2;
            var objRow1 = objTable.insertRow(lastRow);
            var objCell1 = objRow1.insertCell();
            var objCell2 = objRow1.insertCell();
            var objCell3 = objRow1.insertCell();
            var objCell4 = objRow1.insertCell();
            var objCell5 = objRow1.insertCell();
            var objCell6 = objRow1.insertCell();

            objCell1.align = "center";
            objCell2.align = "center";
            objCell3.align = "center";
            objCell4.align = "center";
            objCell5.align = "center";
            objCell6.align = "center";

            objCell1.innerHTML = "<input type=\"text\" id=\"TBBranchStorage[" + thisLineNo + "][0]\" style=\"width:120px; text-align: center;\" />";
            objCell2.innerHTML = "<input type=\"text\" id=\"TBBranchStorage[" + thisLineNo + "][1]\" style=\"width:250px; text-align: center;\" />";
            objCell3.innerHTML = "<input type=\"text\" id=\"TBBranchStorage[" + thisLineNo + "][2]\" style=\"width:120px; text-align: center;\" />";
            objCell4.innerHTML = "<input type=\"text\" id=\"TBBranchStorage[" + thisLineNo + "][3]\" style=\"width:120px; text-align: center;\" />";
            objCell5.innerHTML = "<input type=\"text\" id=\"TBBranchStorage[" + thisLineNo + "][4]\" style=\"width:120px; text-align: center;\" />";
            objCell6.innerHTML = "<input type=\"text\" id=\"TBBranchStorage[" + thisLineNo + "][5]\" style=\"width:250px; text-align: center;\" />";
        }

        // 계좌정보
        function InsertCompanyBank() {
            var objTable = document.getElementById("TabCompanyBank");
            objTable.appendChild(document.createElement("TBODY"));
            var lastRow = objTable.rows.length;
            var thisLineNo = lastRow - 2;
            var objRow1 = objTable.insertRow(lastRow);
            var objCell1 = objRow1.insertCell();
            var objCell2 = objRow1.insertCell();
            var objCell3 = objRow1.insertCell();
            var objCell4 = objRow1.insertCell();
            var objCell5 = objRow1.insertCell();

            objCell1.align = "center";
            objCell2.align = "center";
            objCell3.align = "center";
            objCell4.align = "center";
            objCell5.align = "center";

            objCell1.innerHTML = "<input type=\"text\" id=\"TBCompanyBank[" + thisLineNo + "][0]\" style=\"width:120px; text-align: center;\" />";
            objCell2.innerHTML = "<input type=\"text\" id=\"TBCompanyBank[" + thisLineNo + "][1]\" style=\"width:120px; text-align: center;\" />";
            objCell3.innerHTML = "<input type=\"text\" id=\"TBCompanyBank[" + thisLineNo + "][2]\" style=\"width:250px; text-align: center;\" />";
            objCell4.innerHTML = "<input type=\"text\" id=\"TBCompanyBank[" + thisLineNo + "][3]\" style=\"width:250px; text-align: center;\" />";
            objCell5.innerHTML = "<input type=\"text\" id=\"TBCompanyBank[" + thisLineNo + "][4]\" style=\"width:240px; text-align: center;\" />";
        }

        //Staff 추가
        function InsertStaff() {
            var objTable = document.getElementById("TabStaff");
            objTable.appendChild(document.createElement("TBODY"));
            var lastRow = objTable.rows.length;
            var thisLineNo = lastRow - 2;
            var objRow1 = objTable.insertRow(lastRow);
            var objCell1 = objRow1.insertCell();
            var objCell2 = objRow1.insertCell();
            var objCell3 = objRow1.insertCell();
            var objCell4 = objRow1.insertCell();
            var objCell5 = objRow1.insertCell();
            var objCell6 = objRow1.insertCell();
            var objCell7 = objRow1.insertCell();
            var objCell8 = objRow1.insertCell();
            var objCell9 = objRow1.insertCell();

            objCell1.align = "center";
            objCell2.align = "center";
            objCell3.align = "center";
            objCell4.align = "center";
            objCell5.align = "center";
            objCell6.align = "center";
            objCell7.align = "center";
            objCell8.align = "center";
            objCell9.align = "center";

            objCell1.innerHTML = "<td align=\"center\"><input type=\"text\" id=\"TBStaffDuties[" + thisLineNo + "]\" style=\"width:90px; text-align:center;\" /></td>";
            objCell2.innerHTML = "<td align=\"center\"><input type=\"text\" id=\"TBStaffName[" + thisLineNo + "]\" style=\"width:90px; text-align: center;\" /></td>";
            objCell3.innerHTML = "<td align=\"center\"><input type=\"text\" id=\"TBStaffTEL[" + thisLineNo + "]\" style=\"width:90px; text-align: center;\"/></td>";
            objCell4.innerHTML = "<td align=\"center\">" +
				"<select id=\"EmailNSMS[" + thisLineNo + "]\" style=\"width:50px;\"><option value=\"3\">ALL</option><option value=\"2\">Email</option><option value=\"1\">SMS</option><option value=\"0\">X</option></select></td>";
            objCell5.innerHTML = "<td align=\"center\"><input type=\"text\" id=\"TBStaffEmail[" + thisLineNo + "]\" style=\"width:130px; text-align: center;\" onblur=\"onlyEmail(this)\" /></td>";
            objCell6.innerHTML = "<td align=\"center\"><input type=\"text\" id=\"TBStaffMobile[" + thisLineNo + "]\" style=\"width:90px; text-align: center;\"/></td>";
            objCell7.innerHTML = "<td align=\"center\"><input type=\"text\" id=\"TBStaffID[" + thisLineNo + "]\" style=\"width:90px; text-align: center;\"/></td>";
            objCell8.innerHTML = "<td align=\"center\"><input type=\"Password\" id=\"TBStaffPassword[" + thisLineNo + "]\" style=\"width:90px; text-align: center;\"/></td>";
            objCell9.innerHTML = "<td align=\"center\"><input type=\"text\" id=\"TBStaffMemo[" + thisLineNo + "]\" style=\"width:250px; text-align: center;\"/></td>";
        }

        function BranchAddSubmit() {
            if (form1.RegionCode.value == "") {
                alert("주소의 지역을 선택해주세요");
                return false;
            }
            var CompanyCode = form1.CompanyCode1.value + form1.CompanyCode2.value;
            Admin.AskCompanyCodeUsed(CompanyCode, function (result) {
                if (result != "N") {
                    alert("이미 사용중인 고객번호입니다.");
                    return false;
                } else {
                    AddBranch();
                }
            }, function (result) { alert("ERROR : " + result); return false; });
        }

        function AddBranch() {
            var CompanyCode = form1.CompanyCode1.value + form1.CompanyCode2.value;
            var CompanyValue = new Array();
            CompanyValue[0] = GUBUNCL;
            CompanyValue[1] = CompanyCode;
            CompanyValue[2] = form1.TB_CompanyName.value;
            CompanyValue[3] = form1.RegionCode.value;
            CompanyValue[4] = form1.TB_CompanyAddress.value;
            CompanyValue[5] = form1.TB_CompanyTEL.value;
            CompanyValue[6] = form1.TB_CompanyFAX.value;
            CompanyValue[7] = form1.TB_PresidentName.value;
            CompanyValue[8] = form1.TB_CompanyEmail.value;
            CompanyValue[9] = form1.TB_SaupjaNo.value;
            CompanyValue[10] = form1.TB_MEMO.value;

            var businessType = form1.BusinessType.value;
            var AdditionalInformationSum = "";
            if (document.getElementById("TB_CompanyHomepage").value != "") { AdditionalInformationSum += "%!$@#62#@!" + document.getElementById("TB_CompanyHomepage").value; }
            var CompanyUpjong = "";
            var TabStaffRowLength = document.getElementById("TabStaff").rows.length - 2;
            var StaffSum = "";
            for (var i = 0; i < TabStaffRowLength; i++) {
                if (document.getElementById("TBStaffID[" + i + "]").value == "" && document.getElementById("TBStaffPassword[" + i + "]").value == "" && document.getElementById("TBStaffDuties[" + i + "]").value == "" && document.getElementById("TBStaffName[" + i + "]").value == "" && document.getElementById("TBStaffTEL[" + i + "]").value == "" && document.getElementById("TBStaffEmail[" + i + "]").value == "" && document.getElementById("TBStaffMemo[" + i + "]").value) {
                }
                else {
                    StaffSum += document.getElementById("TBStaffID[" + i + "]").value + "#@!" +
                                document.getElementById("TBStaffPassword[" + i + "]").value + "#@!" +
                                document.getElementById("TBStaffDuties[" + i + "]").value + "#@!" +
                                document.getElementById("TBStaffName[" + i + "]").value + "#@!" +
                                document.getElementById("TBStaffTEL[" + i + "]").value + "#@!" +
                                document.getElementById("TBStaffMobile[" + i + "]").value + "#@!" +
                                document.getElementById("TBStaffEmail[" + i + "]").value + "#@!" +
                                document.getElementById("EmailNSMS[" + i + "]").value + "#@!" +
                                document.getElementById("TBStaffMemo[" + i + "]").value + "%!$@#";
                }
            }

            var TabBranchStorageRowLength = document.getElementById('TabBranchStorage').rows.length - 2;
            var BranchStorageSum = "";
            for (var i = 0; i < TabBranchStorageRowLength; i++) {
                if (document.getElementById("TBBranchStorage[" + i + "][0]").value == "" && document.getElementById("TBBranchStorage[" + i + "][1]").value == "" && document.getElementById("TBBranchStorage[" + i + "][3]").value == "" && document.getElementById("TBBranchStorage[" + i + "][4]").value == "" && document.getElementById("TBBranchStorage[" + i + "][5]").value == "") { }
                else {
                    BranchStorageSum += document.getElementById("TBBranchStorage[" + i + "][0]").value + "@@" +
                                        document.getElementById("TBBranchStorage[" + i + "][1]").value + "@@" +
                                        document.getElementById("TBBranchStorage[" + i + "][2]").value + "@@" +
                                        document.getElementById("TBBranchStorage[" + i + "][3]").value + "@@" +
                                        document.getElementById("TBBranchStorage[" + i + "][4]").value + "@@" +
                                        document.getElementById("TBBranchStorage[" + i + "][5]").value + "####";
                }
            }

            var TabCompanyBankRowLength = document.getElementById('TabCompanyBank').rows.length - 2;
            var CompanyBankSum = "";
            for (var i = 0; i < TabCompanyBankRowLength; i++) {
                if (document.getElementById("TBCompanyBank[" + i + "][0]").value == "" && document.getElementById("TBCompanyBank[" + i + "][1]").value == "" && document.getElementById("TBCompanyBank[" + i + "][3]").value == "" && document.getElementById("TBCompanyBank[" + i + "][4]").value == "") { }
                else {
                    CompanyBankSum += document.getElementById("TBCompanyBank[" + i + "][0]").value + "@@" +
								 document.getElementById("TBCompanyBank[" + i + "][1]").value + "@@" +
								 document.getElementById("TBCompanyBank[" + i + "][2]").value + "@@" +
							 	 document.getElementById("TBCompanyBank[" + i + "][3]").value + "@@" +
                                 document.getElementById("TBCompanyBank[" + i + "][4]").value + "####";
                }
            }
            Admin.InsertCompanyBranchByAdmin(form1.HAccountID.value, CompanyValue, StaffSum, BranchStorageSum, CompanyBankSum, AdditionalInformationSum, businessType, function (result) {
                window.returnValue = true;
                returnValue = result;
                self.close();
                return false;
            }, ONFAILED);
        }

        function ONFAILED(result) { window.alert(result); }

        function onlyEmail(obj) {
            if (obj.value.length > 0) {
                re = /^[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$/i;
                if (obj.value.length < 6 || !re.test(obj.value)) {
                    alert("메일형식이 맞지 않습니다.\n");
                    obj.select();
                    obj.focus();
                    return false;
                }
            }
        }

        function CheckAutoNum(which, value) {
            if (value.length == 2) {
                Admin.AutoCompanyCode(value, function (result) {
                    document.getElementById(which).value = result;
                    document.getElementById(which).select();
                    document.getElementById(which).focus();
                }, function (result) {
                    alert(result);
                });
            }
            else { return false; }
        }
    </script>
</head>
<body style="background-color: #E4E4E4; width: 1000px; margin: 0 auto; padding-top: 20px; padding-bottom: 20px;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="SM" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Admin.asmx" />
            </Services>
        </asp:ScriptManager>
        <div style="background-color:#999999; padding:7px;">
			<span style="color:White; font-weight:bold"><img src="../../Images/ico_arrow.gif" alt="" /> 지사 정보 등록 </span> 
            </div>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 1000px;">
            <tr>
                <td class="td01"><%= GetGlobalResourceObject("Member", "CompanyName")%></td>
                <td class="td02">
                    <input type="text" id="TB_CompanyName" maxlength="20" />
                </td>
                <td class="td01">PresidentName</td>
                <td class="td02">
                    <input type="text" id="TB_PresidentName" maxlength="20" />
                </td>
            </tr>
            <tr>
                <td style="background-color: #f5f5f5; text-align: center; height: 25px; width: 150px; padding-top: 4px; padding-bottom: 4px;"><%=GetGlobalResourceObject("Member", "CompanyAddress") %></td>
                <td class="td023" colspan="3">
                    <input type="hidden" id="RegionCode" />
                    <input type="hidden" id="ResponsibleBranch" />
                    <select id="RegionCode[0]" style="width: 100px; height: 23px;" onchange="LoadRegionCode('0');"></select>
                    <select id="RegionCode[1]" style="width: 140px; height: 23px;" name="office" onchange="LoadRegionCode('1');"></select>
                    <select id="RegionCode[2]" style="width: 140px; height: 23px;" name="area" onchange="LoadRegionCode('2');"></select>
                    <div style="padding-top: 4px;">
                        <input type="text" id="TB_CompanyAddress" maxlength="100"  style="width:600px"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="td01">TEL</td>
                <td class="td02">
                    <input type="text" id="TB_CompanyTEL" maxlength="20" />
                </td>
                <td class="td01"><%=GetGlobalResourceObject("Member", "SaupjaNo") %></td>
                <td class="td02">
                    <select id="BusinessType">
                        <option value="54"><%=GetGlobalResourceObject("qjsdur", "rlxk")%></option>
                        <option value="55"><%=GetGlobalResourceObject("qjsdur", "qjqdls") %></option>
                        <option value="56"><%=GetGlobalResourceObject("qjsdur", "rodlstkdjqwk")%></option>
                    </select>
                    <input type="text" id="TB_SaupjaNo" maxlength="20" />
                </td>
            </tr>
            <tr>
                <td class="td01">FAX</td>
                <td class="td02">
                    <input type="text" id="TB_CompanyFAX" maxlength="20" style="width: 390px" />
                </td>
                <td class="td01">지사 코드</td>
                <td class="td02">
                    <input type="text" id="CompanyCode1" onkeyup="CheckAutoNum('CompanyCode2', this.value)" maxlength="4" style="width: 60px; text-align: center; font-size: 20px;" />
                    <input type="text" id="CompanyCode2" style="width: 80px; font-size: 20px;" />
                </td>
            </tr>
            <tr>
                <td class="td01">HomePage</td>
                <td class="td02">
                    <input type="text" id="TB_CompanyHomepage" maxlength="20" style="width: 390px" />
                </td>
                <td class="td01">E-mail</td>
                <td class="td02">
                    <input type="text" id="TB_CompanyEmail" onblur="onlyEmail(this)" maxlength="20" style="width: 390px" />
                </td>
            </tr>
            <tr>
                <td class="td01">Memo</td>
                <td class="td02" colspan="3">
                    <textarea rows="4" cols="46" id="TB_MEMO" maxlength="500" style="width:600px"></textarea>
                </td>
            </tr>
        </table>
        <div style="background-color: #777777; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #BBBBBB; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #CCCCCC; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #DDDDDD; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #EEEEEE; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #FFFFFF; height: 1px; font-size: 1px;"></div>

        <table id="TabStaff" style="background-color: White; width: 1000px;" border="0" cellpadding="0" cellspacing="0">
            <thead>
                <tr>
                    <td class="tdSubT" colspan="9">&nbsp;&nbsp;&nbsp;<strong>Staff</strong>
                        <input type="button" value="add" onclick="InsertStaff();" />
                    </td>
                </tr>
                <tr>
                    <td style="background-color:#F5F5F5; text-align:center; width:90px; height:20px;" ><%= GetGlobalResourceObject("Member", "Duties") %></td>
                    <td  style="width: 90px; background-color:#F5F5F5; text-align:center;"><%=GetGlobalResourceObject("Member", "Name") %></td>
                    <td  style="width: 90px; background-color:#F5F5F5; text-align:center;">TEL</td>
                    <td  style="width: 50px; background-color:#F5F5F5; text-align:center;">MSG</td>
                    <td  style="width: 130px; background-color:#F5F5F5; text-align:center;">E-mail</td>
                    <td  style="width: 90px; background-color:#F5F5F5; text-align:center;">Mobile</td>
                    <td  style="width: 90px; background-color:#F5F5F5; text-align:center;">ID</td>
                    <td  style="width: 90px; background-color:#F5F5F5; text-align:center;">Password</td>
                    <td  style=" background-color:#F5F5F5; text-align:center;">Memo</td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td align="center">
                        <input type="text" id="TBStaffDuties[0]" style="width: 90px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBStaffName[0]" style="width: 90px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBStaffTEL[0]" style="width: 90px; text-align: center;" /></td>
                    <td align="center">
                        <select id="EmailNSMS[0]" style="width: 50px; text-align: center;">
                            <option value="3">ALL</option>
                            <option value="2">Email</option>
                            <option value="1">SMS</option>
                            <option value="0">X</option>
                        </select>
                    </td>
                    <td align="center">
                        <input type="text" id="TBStaffEmail[0]" style="width: 130px; text-align: center;" onblur="onlyEmail(this)" /></td>
                    <td align="center">
                        <input type="text" id="TBStaffMobile[0]" style="width: 90px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBStaffID[0]" style="width: 90px; text-align: center;" /></td>
                    <td align="center">
                        <input type="Password" id="TBStaffPassword[0]" style="width: 90px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBStaffMemo[0]" style="text-align: center;width: 250px;" /></td>
                </tr>
            </tbody>
        </table>
        <div style="background-color: #777777; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #BBBBBB; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #CCCCCC; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #DDDDDD; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #EEEEEE; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #FFFFFF; height: 1px; font-size: 1px;"></div>

        <table id="TabBranchStorage" style="background-color: White; width: 1000px;" border="0" cellpadding="0" cellspacing="0">
            <thead>
                <tr>
                    <td class="tdSubT" colspan="6">&nbsp;&nbsp;&nbsp;<strong>BranchStorage</strong>
                        <input type="button" value="add" onclick="InsertBranchStorage();" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;  height:20px; background-color:#F5F5F5; text-align:center;">CompanyName</td>
                    <td style="width: 250px; background-color:#F5F5F5; text-align:center;" ><%=GetGlobalResourceObject("Member", "Address") %></td>
                    <td style="width: 120px; background-color:#F5F5F5; text-align:center;" >StaffName</td>
                    <td style="width: 120px; background-color:#F5F5F5; text-align:center;" >TEL</td>
                    <td style="width: 120px; background-color:#F5F5F5; text-align:center;" >Mobile</td>
                    <td style="background-color:#F5F5F5; text-align:center;">Memo</td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td align="center">
                        <input type="text" id="TBBranchStorage[0][0]" style="width: 120px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBBranchStorage[0][1]" style="width: 250px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBBranchStorage[0][2]" style="width: 120px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBBranchStorage[0][3]" style="width: 120px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBBranchStorage[0][4]" style="width: 120px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBBranchStorage[0][5]" style="width: 250px; text-align: center;" /></td>
                </tr>
            </tbody>
        </table>
        <div style="background-color: #777777; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #BBBBBB; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #CCCCCC; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #DDDDDD; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #EEEEEE; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #FFFFFF; height: 1px; font-size: 1px;"></div>

        <table id="TabCompanyBank" style="background-color: White; width: 1000px;" border="0" cellpadding="0" cellspacing="0">
            <thead>
                <tr>
                    <td class="tdSubT" colspan="6">&nbsp;&nbsp;&nbsp;<strong>CompanyBank</strong>
                        <input type="button" value="add" onclick="InsertCompanyBank();" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px; height:20px; background-color:#F5F5F5; text-align:center;" >BankName</td>
                    <td  style="width: 120px; background-color:#F5F5F5; text-align:center;" >OwnerName</td>
                    <td  style="width: 250px; background-color:#F5F5F5; text-align:center;" >AccountNo</td>
                    <td  style="width: 250px; background-color:#F5F5F5; text-align:center;" >Address</td>
                    <td  style="background-color:#F5F5F5; text-align:center;">BankMemo</td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td align="center">
                        <input type="text" id="TBCompanyBank[0][0]" style="width: 120px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBCompanyBank[0][1]" style="width: 120px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBCompanyBank[0][2]" style="width: 250px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBCompanyBank[0][3]" style="width: 250px; text-align: center;" /></td>
                    <td align="center">
                        <input type="text" id="TBCompanyBank[0][4]" style="width: 240px; text-align: center;" /></td>
                </tr>
            </tbody>
        </table>
        <div style="background-color: #777777; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #BBBBBB; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #CCCCCC; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #DDDDDD; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #EEEEEE; height: 1px; font-size: 1px;"></div>
        <div style="background-color: #FFFFFF; height: 1px; font-size: 1px;"></div>

        <div style="width: 1000px; background-color: #FFFFFF; padding-top: 20px; padding-bottom: 15px; text-align: center;">
            <input type="button" value="submit" onclick="BranchAddSubmit();" />
            <input type="hidden" id="dhksfy" value="<%=GetGlobalResourceObject("qjsdur", "dhksfy") %>" />
        </div>
        <input type="hidden" id="HCCLPK" value="<%=Request.Params["CCLPK"]+"" %>" />
        <input type="hidden" id="HAccountID" />
    </form>
</body>
</html>
