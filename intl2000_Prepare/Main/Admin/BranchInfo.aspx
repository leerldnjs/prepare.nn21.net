<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BranchInfo.aspx.cs" Inherits="Admin_BranchInfo" %>

<%@ Register Src="LogedWithoutRecentRequest.ascx" TagName="LogedWithoutRecentRequest" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Company Info</title>
	<style type="text/css">
		.tdSubT {
			border-bottom: 2px solid #93A9B8;
			padding-top: 30px;
			padding-bottom: 3px;
		}

		.td01 {
			background-color: #f5f5f5;
			text-align: center;
			height: 25px;
			width: 150px;
			padding-top: 4px;
			padding-bottom: 4px;
			border-bottom: 1px dotted #E8E8E8;
		}

		.td02 {
			width: 250px;
			padding-top: 4px;
			padding-bottom: 4px;
			padding-left: 10px;
			border-bottom: 1px dotted #E8E8E8;
			background-color: White;
		}

		.td023 {
			font-weight: bold;
			color: #2b2b2b;
			padding: 0 5px 0 5px;
		}

		.tdStaffBody {
			text-align: center;
			padding: 5px;
			border: dotted 1px #E8E8E8;
			background-color: White;
		}
	</style>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		var GUBUNCL;
		var COMPANYPK;
		var COMPANYCODE;
		var COMPANYNAME;
		var REGIONCODE;
		var COMPANYADDRESS;
		var COMPANYTEL;
		var COMPANYFAX;
		var PRESIDENTNAME;
		var PRESIDENTEMAIL;
		var COMPANYNO;
		var COMPANYADDITIONAL61;
		var COMPANYADDITIONAL62;
		var COMPANYADDITIONAL63;
		var COMPANYADDITIONAL64;
		var COMPANYADDITIONAL65;
		//var RESPONSIBLESTAFF;
		var MEMO;
		var STAFFSUM = new Array();
		var BranchStorageSum = new Array();
		var CompanyBankSum = new Array();
		var IsThirdRegion = false;

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
						if (result[0].substr(0, 1) == "7") {
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
			LoadRegionCodeCountry("RegionCode[0]");
			var Url = location.href;
			var GetValue = Url.substring(Url.indexOf("?") + 1);
			var EachValue = GetValue.split("&");
			var tempcount;

			if (EachValue[0].substr(0, 8) == "Language") { tempcount = 1; }
			else { tempcount = 0; }
			if (EachValue[tempcount] == "M=View") {
				COMPANYPK = EachValue[(tempcount + 1)].toString().substr(2);
				Admin.LoadBranchInfo(COMPANYPK, function (result) {
					if (result == "N") { return false; }
					var EachGroup = result.split("%!$@#");
					var Company = EachGroup[0].split("#@!");
					COMPANYCODE = Company[0]; form1.TB_CompanyCode.value = Company[0];
					COMPANYNAME = Company[1]; form1.TB_CompanyName.value = Company[1];
					document.getElementById("spRegionName").innerHTML = Company[2];
					COMPANYADDRESS = Company[3]; form1.TB_CompanyAddress.value = Company[3];
					COMPANYTEL = Company[4];
					document.getElementById("TB_CompanyTEL[0]").value = Company[4];

					COMPANYFAX = Company[5];
					document.getElementById("TB_CompanyFAX[0]").value = Company[5];

					PRESIDENTNAME = Company[6];
					form1.TB_PresidentName.value = Company[6];

					PRESIDENTEMAIL = Company[7]; form1.TB_CompanyEmail.value = Company[7];
					COMPANYNO = Company[8]; form1.TB_SaupjaNo.value = Company[8];
					GUBUNCL = Company[11];
					//RESPONSIBLESTAFF = Company[12]; form1.TB_ResponsibleStaff.value = Company[12];

					MEMO = Company[13];
					form1.TB_MEMO.value = Company[13];

					var StaffRowCount = 0;
					var BranchStorageCount = 0;
					var CompanyBankCount = 0;

					var filelist = "";
					for (var i = 1; i < EachGroup.length; i++) {
						var Each = EachGroup[i].split("#@!");
						switch (Each[0]) {
							case "A":
								if (Each[2] + "" != "") { InsertStaff('Company'); }
								else { InsertStaff(''); }
								STAFFSUM[StaffRowCount] = EachGroup[i].substr(4);
								document.getElementById("TBStaffGubun[" + StaffRowCount + "]").value = "A";
								document.getElementById("TBStaffPk[" + StaffRowCount + "]").value = Each[1];
								document.getElementById("TBStaffDuties[" + StaffRowCount + "]").value = Each[4];
								document.getElementById("TBStaffName[" + StaffRowCount + "]").value = Each[5];
								document.getElementById("TBStaffTEL[" + StaffRowCount + "]").value = Each[6];
								document.getElementById("TBStaffEmail[" + StaffRowCount + "]").value = Each[8];
								if (Each[2] + "" != "") {
									document.getElementById("TBStaffGubun[" + StaffRowCount + "]").value = "C";
									document.getElementById("TBStaffID[" + StaffRowCount + "]").value = Each[2];
									document.getElementById("TBStaffID[" + StaffRowCount + "]").disabled = "disabled";
									document.getElementById("TBStaffPassword[" + StaffRowCount + "]").value = Each[3];
								}
								document.getElementById("TBStaffMobile[" + StaffRowCount + "]").value = Each[7];
								document.getElementById("EmailNSMS[" + StaffRowCount + "]").value = Each[9];
								StaffRowCount++;
								break;
							case "W":
								BranchStorageSum[BranchStorageCount] = new Array();
								BranchStorageSum[BranchStorageCount][0] = Each[1];
								BranchStorageSum[BranchStorageCount][1] = Each[2];
								BranchStorageSum[BranchStorageCount][2] = Each[3];
								BranchStorageSum[BranchStorageCount][3] = Each[4];
								BranchStorageSum[BranchStorageCount][4] = Each[5];
								BranchStorageSum[BranchStorageCount][5] = Each[6];
								BranchStorageSum[BranchStorageCount][6] = Each[7];
								InsertBranchStorage();
								document.getElementById("TBBranchStorage[" + BranchStorageCount + "]Pk").value = Each[1];
								document.getElementById("TBBranchStorage[" + BranchStorageCount + "][0]").value = Each[2];
								document.getElementById("TBBranchStorage[" + BranchStorageCount + "][1]").value = Each[3];
								document.getElementById("TBBranchStorage[" + BranchStorageCount + "][2]").value = Each[4];
								document.getElementById("TBBranchStorage[" + BranchStorageCount + "][3]").value = Each[5];
								document.getElementById("TBBranchStorage[" + BranchStorageCount + "][4]").value = Each[6];
								document.getElementById("TBBranchStorage[" + BranchStorageCount + "][5]").value = Each[7];
								BranchStorageCount++;
								break;
							case "B":
								CompanyBankSum[CompanyBankCount] = new Array();
								CompanyBankSum[CompanyBankCount][0] = Each[1];
								CompanyBankSum[CompanyBankCount][1] = Each[2];
								CompanyBankSum[CompanyBankCount][2] = Each[3];
								CompanyBankSum[CompanyBankCount][3] = Each[4];
								CompanyBankSum[CompanyBankCount][4] = Each[5];
								CompanyBankSum[CompanyBankCount][5] = Each[6];
								InsertCompanyBank();
								document.getElementById("TBCompanyBank[" + CompanyBankCount + "]Pk").value = Each[1];
								document.getElementById("TBCompanyBank[" + CompanyBankCount + "][0]").value = Each[2];
								document.getElementById("TBCompanyBank[" + CompanyBankCount + "][1]").value = Each[3];
								document.getElementById("TBCompanyBank[" + CompanyBankCount + "][2]").value = Each[4];
								document.getElementById("TBCompanyBank[" + CompanyBankCount + "][3]").value = Each[5];
								document.getElementById("TBCompanyBank[" + CompanyBankCount + "][4]").value = Each[6];
								CompanyBankCount++;
								break;
							case "61":
								COMPANYADDITIONAL61 = Each[1];
								form1.BusinessType.value = Each[1];
								break;
							case "62":
								COMPANYADDITIONAL62 = Each[1];
								form1.TB_Homepage.value = Each[1];
								break;
								//case "63":
								//    COMPANYADDITIONAL63 = Each[1];
								//    var temp = Each[1].split("!");
								//    for (var j = 0; j < temp.length; j++) {
								//        switch (temp[j]) {
								//            case "57": form1.Upjong1.checked = true; break;
								//            case "58": form1.Upjong2.checked = true; break;
								//            case "59": form1.Upjong3.checked = true; break;
								//        }
								//    }
								//    break;
							case "64":
								COMPANYADDITIONAL64 = Each[1];
								var temp = Each[1].split("!");
								form1.TB_CategoryofBusiness.value = temp[0];
								form1.TB_BusinessConditions.value = temp[1];
								break;
							case "65":
								COMPANYADDITIONAL65 = Each[1];
								form1.TB_MajorItem.value = Each[1];
								break;
								//case "F":
								//    filelist += "<a href='../UploadedFiles/FileDownload.aspx?S=" + Each[1] + "' >ㆍ" + Each[2] + " : " + Each[3] + "</a>&nbsp;<span onclick=\"FileDelete('" + Each[1] + "');\" style='color:red;'>X</span><br />";
								//    break;
						}
					}
					//if (filelist != "") {
					//    document.getElementById("PNFileList").innerHTML = "<fieldset><legend><strong>Attached File</strong></legend>" + filelist + "</fieldset>";
					//}
				}, function (result) { alert("ERROR : " + result); });
			}
		}

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
			objCell6.align = "left";

			objCell1.innerHTML = "<input type=\"hidden\" id=\"TBBranchStorage[" + thisLineNo + "]Pk\" /><input type=\"text\" id=\"TBBranchStorage[" + thisLineNo + "][0]\" style=\"width:110px; text-align:center;\"/>";
			objCell2.innerHTML = "<input type=\"text\" id=\"TBBranchStorage[" + thisLineNo + "][1]\" style=\"width:250px; text-align:center;\"/>";
			objCell3.innerHTML = "<input type=\"text\" id=\"TBBranchStorage[" + thisLineNo + "][2]\" style=\"width:110px; text-align:center;\"/>";
			objCell4.innerHTML = "<input type=\"text\" id=\"TBBranchStorage[" + thisLineNo + "][3]\" style=\"width:110px; text-align:center;\"/>";
			objCell5.innerHTML = "<input type=\"text\" id=\"TBBranchStorage[" + thisLineNo + "][4]\" style=\"width:110px; text-align:center;\"/>";
			objCell6.innerHTML = "<input type=\"text\" id=\"TBBranchStorage[" + thisLineNo + "][5]\" style=\"width:120px; text-align:center;\"/>&nbsp;" +
            "<span onclick=\"DELETEBranchStorage('" + thisLineNo + "');\" style=\"color:Red; cursor:pointer;\" >X</span>";
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

			objCell1.innerHTML = "<input type=\"hidden\" id=\"TBCompanyBank[" + thisLineNo + "]Pk\" /><input type=\"text\" id=\"TBCompanyBank[" + thisLineNo + "][0]\" style=\"width:130px; text-align:center;\"/>";
			objCell2.innerHTML = "<input type=\"text\" id=\"TBCompanyBank[" + thisLineNo + "][1]\" style=\"width:120px; text-align:center;\"/>";
			objCell3.innerHTML = "<input type=\"text\" id=\"TBCompanyBank[" + thisLineNo + "][2]\" style=\"width:200px; text-align:center;\"/>";
			objCell4.innerHTML = "<input type=\"text\" id=\"TBCompanyBank[" + thisLineNo + "][3]\" style=\"width:200px; text-align:center;\"/>";
			objCell5.innerHTML = "<input type=\"text\" id=\"TBCompanyBank[" + thisLineNo + "][4]\" style=\"width:160px; text-align:center;\"/>&nbsp;" +
            "<span onclick=\"DELETECompanyBank('" + thisLineNo + "');\" style=\"color:Red; cursor:pointer;\" >X</span>";
		}

		function DeleteCompany() {
			if (confirm("삭제되면 복구할수 없습니다. 모든 거래내역도 같이 삭제됩니다. ")) {
				Admin.DeleteCompany(COMPANYPK, function (result) {
					if (result == "1") {
						alert("SUCCESS");
						location.reload();
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}

		function InsertStaff(gubun) {
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

			objCell1.align = "center";
			objCell2.align = "center";
			objCell3.align = "center";
			objCell4.align = "center";
			objCell5.align = "center";
			objCell6.align = "center";
			objCell7.align = "center";
			objCell8.align = "left";

			var rightIcon = "";
			rightIcon = "<span onclick=\"DELETESTAFF('" + thisLineNo + "');\" style=\"color:Red; cursor:pointer;\" >X</span>";

			objCell1.innerHTML = "<input type=\"hidden\" id=\"TBStaffGubun[" + thisLineNo + "]\" value=\"N\" /><input type=\"hidden\" id=\"TBStaffPk[" + thisLineNo + "]\" /><input type=\"text\" id=\"TBStaffDuties[" + thisLineNo + "]\" style=\"width:80px; text-align:center;\" />";
			objCell2.innerHTML = "<input type=\"text\" id=\"TBStaffName[" + thisLineNo + "]\" style=\"width:80px; text-align:center;\" />";
			objCell3.innerHTML = "<input type=\"text\" id=\"TBStaffTEL[" + thisLineNo + "]\" style=\"width:90px; text-align:center;\" />";
			objCell4.innerHTML = "<select id=\"EmailNSMS[" + thisLineNo + "]\" style=\"width: 50px;\"><option value=\"3\">ALL</option><option value=\"2\">Email</option><option value=\"1\">SMS</option><option value=\"0\">X</option> </select>";
			objCell5.innerHTML = "<input type=\"text\" id=\"TBStaffEmail[" + thisLineNo + "]\" style=\"width:130px; text-align:center;\" />";
			objCell6.innerHTML = "<input type=\"text\" id=\"TBStaffMobile[" + thisLineNo + "]\" style=\"width:90px; text-align:center;\" />";
			objCell7.innerHTML = "<input type=\"text\" id=\"TBStaffID[" + thisLineNo + "]\" style=\"width:80px; text-align:center;\" />";
			objCell8.innerHTML = "<input type=\"Password\" id=\"TBStaffPassword[" + thisLineNo + "]\" style=\"width:120px; text-align:center;\" />&nbsp;" + rightIcon;
		}

		function OnlyNum(obj) {
			val = obj.value;
			re = /[^0-9]/gi;
			if (re.test(val)) {
				alert("숫자만 입력가능합니다");
				obj.select();
				obj.focus();
			}
		}

		function moveNext(from, to, length) {
			if (from.value.length == length) {
				document.getElementById(to).select();
				document.getElementById(to).focus();
			}
		}

		function ONFAILED(result) {
			window.alert(result);
		}

		function AutoCompanyCodeSuccess(result) {
			form1.CompanyCode2.value = result;
			form1.CompanyCode2.select();
			form1.BTN_Submit.style.visibility = "visible";
		}

		function onlyEmail(obj) {
			if (obj.value.length > 0) {
				re = /^[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$/i;
				if (obj.value.length < 6 || !re.test(obj.value)) { alert("메일형식이 맞지 않습니다.\n"); obj.select(); obj.focus(); return false; }
			}
		}

		function CheckAutoNum(which, value) {
			if (value.length == 4) {
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

		function Goto(value) {
			switch (value) {
				case "back": history.back(); break;
				case "basic": location.href = "CompanyInfo.aspx?M=View&S=" + COMPANYPK; break;
				case "NewBasic": location.href = "CompanyView.aspx?S=" + COMPANYPK; break;
				case "customer": location.href = "CompanyInfoCustomer.aspx?S=" + COMPANYPK; break;
				case "request": location.href = "CompanyInfoRequestList.aspx?G=CI&S=" + COMPANYPK; break;
				case "talkBusiness": window.open('./Dialog/TalkBusiness.aspx?S=' + COMPANYPK, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=700px; width=600px;'); break;
				case "addcustomer": window.open('../Request/Dialog/OwnCustomerAdd.aspx?S=' + COMPANYPK + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	650px; width=900px;'); break;
				case "fileupload": window.open('./Dialog/FileUpload.aspx?G=1&S=' + COMPANYPK, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;'); break;
			}
		}

		function ModifyCompanyCode() {
			if (COMPANYCODE == form1.TB_CompanyCode.value) { alert("변동사항이 없습니다."); return false; }
			Admin.AskCompanyCodeUsed(form1.TB_CompanyCode.value, function (result) {
				if (result == "N") {
					if (confirm("해당고객번호를 사용중인 업체가 없습니다. \r\n 해당업체에 " + form1.TB_CompanyCode.value + "를 부여하겠습니까?")) {
						Admin.SetCompanyCustomerCodeManual(COMPANYPK, form1.TB_CompanyCode.value, function (result) {
							if (result == "0") { window.alert(form1.HUsedNumber.value); }
							else {
								alert("성공적으로 지정되었습니다.");
								COMPANYCODE = result;
							}
						}, ONFAILED);
					}
				}
				else {
					var resultSplit = result.split("!!");
					alert(form1.TB_CompanyCode.value + "는 이미 사용중인 번호입니다. \r\n기존 사용자 : " + resultSplit[1]);
				}
			}, function (result) { alert("ERROR : " + result); });
		}

		function CompanyModify() {
			var TOcompanyname = "*N!C$";
			var TOregioncode = "*N!C$";
			var TOcompanyaddress = "*N!C$";
			var TOcompanytel = "*N!C$";
			var TOcompanyfax = "*N!C$";
			var TOpresidentname = "*N!C$";
			var TOpresidentemail = "*N!C$";
			var TOcompanyno = "*N!C$";
			var TOstaffsum = "";
			var TOBranchStorageSum = "";
			var TOCompanyBankSum = "";
			var TO61 = "*N!C$";
			var TO62 = "*N!C$";
			var TO63 = "*N!C$";
			var TO64 = "*N!C$";
			var TO65 = "*N!C$";
			//var TOresponsiblestaff = "*N!C$";
			var TOMEMO = "*N!C$";
			if (COMPANYNAME != form1.TB_CompanyName.value) {
				TOcompanyname = form1.TB_CompanyName.value;
			}
			if (form1.RegionCode.value != "") {
				TOregioncode = form1.RegionCode.value;
			}
			if (COMPANYADDRESS != form1.TB_CompanyAddress.value) {
				TOcompanyaddress = form1.TB_CompanyAddress.value;
			}
			if (COMPANYTEL != document.getElementById("TB_CompanyTEL[0]").value) {
				TOcompanytel = document.getElementById("TB_CompanyTEL[0]").value;
			}
			if (COMPANYFAX != document.getElementById("TB_CompanyFAX[0]").value) {
				TOcompanyfax = document.getElementById("TB_CompanyFAX[0]").value;
			}
			if (PRESIDENTNAME != form1.TB_PresidentName.value) {
				TOpresidentname = form1.TB_PresidentName.value;
			}
			if (PRESIDENTEMAIL != form1.TB_CompanyEmail.value) {
				TOpresidentemail = form1.TB_CompanyEmail.value;
			}
			if (COMPANYNO != form1.TB_SaupjaNo.value) {
				TOcompanyno = form1.TB_SaupjaNo.value;
			}
			//if (RESPONSIBLESTAFF != form1.TB_ResponsibleStaff.value) {
			//    TOresponsiblestaff = form1.TB_ResponsibleStaff.value;
			//}

			if (MEMO != form1.TB_MEMO.value) {
				TOMEMO = form1.TB_MEMO.value;
			}
			var StaffRow = document.getElementById('TabStaff').rows.length - 2;
			for (var i = 0; i < StaffRow; i++) {
				if (STAFFSUM[i] != document.getElementById("TBStaffPk[" + i + "]").value + "#@!" +
	    				           document.getElementById("TBStaffID[" + i + "]").value + "#@!" +
                                   document.getElementById("TBStaffPassword[" + i + "]").value + "#@!" +
	    				           document.getElementById("TBStaffDuties[" + i + "]").value + "#@!" +
	    				           document.getElementById("TBStaffName[" + i + "]").value + "#@!" +
	    				           document.getElementById("TBStaffTEL[" + i + "]").value + "#@!" +
	    				           document.getElementById("TBStaffMobile[" + i + "]").value + "#@!" +
	    				           document.getElementById("TBStaffEmail[" + i + "]").value + "#@!" +
	    				           document.getElementById("EmailNSMS[" + i + "]").value) {
					if (document.getElementById("TBStaffDuties[" + i + "]").value == "" && document.getElementById("TBStaffName[" + i + "]").value == "") { }
					else if (document.getElementById("TBStaffID[" + i + "]").value + "" == "" || document.getElementById("TBStaffPassword[" + i + "]").value + "" == "") {
						alert("ID와 PW의 값은 필수 입니다");
						return false;
					}
					else {

						TOstaffsum += document.getElementById("TBStaffPk[" + i + "]").value + "#@!" +
                                      document.getElementById("TBStaffID[" + i + "]").value + "#@!" +
                                      document.getElementById("TBStaffPassword[" + i + "]").value + "#@!" +
	    							  document.getElementById("TBStaffDuties[" + i + "]").value + "#@!" +
	    							  document.getElementById("TBStaffName[" + i + "]").value + "#@!" +
	    							  document.getElementById("TBStaffTEL[" + i + "]").value + "#@!" +
	    							  document.getElementById("TBStaffMobile[" + i + "]").value + "#@!" +
	    							  document.getElementById("TBStaffEmail[" + i + "]").value + "#@!" +
	    							  document.getElementById("EmailNSMS[" + i + "]").value + "%!$@#";
					}
				}
			}
			var BranchStorageRow = document.getElementById("TabBranchStorage").rows.length - 2;
			for (var i = 0; i < BranchStorageRow; i++) {
				if (BranchStorageSum.length > i) {
					if (BranchStorageSum[i][1] != document.getElementById("TBBranchStorage[" + i + "][0]").value ||
						BranchStorageSum[i][2] != document.getElementById("TBBranchStorage[" + i + "][1]").value ||
						BranchStorageSum[i][3] != document.getElementById("TBBranchStorage[" + i + "][2]").value ||
						BranchStorageSum[i][4] != document.getElementById("TBBranchStorage[" + i + "][3]").value ||
						BranchStorageSum[i][5] != document.getElementById("TBBranchStorage[" + i + "][4]").value ||
						BranchStorageSum[i][6] != document.getElementById("TBBranchStorage[" + i + "][5]").value) {
						TOBranchStorageSum += document.getElementById("TBBranchStorage[" + i + "]Pk").value + "@@" +
									   document.getElementById("TBBranchStorage[" + i + "][0]").value + "@@" +
	    						       document.getElementById("TBBranchStorage[" + i + "][1]").value + "@@" +
	    							   document.getElementById("TBBranchStorage[" + i + "][2]").value + "@@" +
									   document.getElementById("TBBranchStorage[" + i + "][3]").value + "@@" +
	    							   document.getElementById("TBBranchStorage[" + i + "][4]").value + "@@" +
							   		   document.getElementById("TBBranchStorage[" + i + "][5]").value + "####";
					}
				}
				else {
					if ("" != document.getElementById("TBBranchStorage[" + i + "][0]").value ||
						"" != document.getElementById("TBBranchStorage[" + i + "][1]").value ||
						"" != document.getElementById("TBBranchStorage[" + i + "][2]").value ||
						"" != document.getElementById("TBBranchStorage[" + i + "][3]").value ||
						"" != document.getElementById("TBBranchStorage[" + i + "][4]").value ||
						"" != document.getElementById("TBBranchStorage[" + i + "][5]").value) {
						TOBranchStorageSum += document.getElementById("TBBranchStorage[" + i + "]Pk").value + "@@" +
									   document.getElementById("TBBranchStorage[" + i + "][0]").value + "@@" +
	    							   document.getElementById("TBBranchStorage[" + i + "][1]").value + "@@" +
	    							   document.getElementById("TBBranchStorage[" + i + "][2]").value + "@@" +
									   document.getElementById("TBBranchStorage[" + i + "][3]").value + "@@" +
	    							   document.getElementById("TBBranchStorage[" + i + "][4]").value + "@@" +
								       document.getElementById("TBBranchStorage[" + i + "][5]").value + "####";
					}
				}
			}
			var CompanyBankRow = document.getElementById("TabCompanyBank").rows.length - 2;
			for (var i = 0; i < CompanyBankRow; i++) {
				if (CompanyBankSum.length > i) {
					if (CompanyBankSum[i][1] != document.getElementById("TBCompanyBank[" + i + "][0]").value ||
						CompanyBankSum[i][2] != document.getElementById("TBCompanyBank[" + i + "][1]").value ||
						CompanyBankSum[i][3] != document.getElementById("TBCompanyBank[" + i + "][2]").value ||
						CompanyBankSum[i][4] != document.getElementById("TBCompanyBank[" + i + "][3]").value ||
						CompanyBankSum[i][5] != document.getElementById("TBCompanyBank[" + i + "][4]").value) {
						TOCompanyBankSum += document.getElementById("TBCompanyBank[" + i + "]Pk").value + "@@" +
									   document.getElementById("TBCompanyBank[" + i + "][0]").value + "@@" +
	    						       document.getElementById("TBCompanyBank[" + i + "][1]").value + "@@" +
	    							   document.getElementById("TBCompanyBank[" + i + "][2]").value + "@@" +
									   document.getElementById("TBCompanyBank[" + i + "][3]").value + "@@" +
							   		   document.getElementById("TBCompanyBank[" + i + "][4]").value + "####";
					}
				}
				else {
					if ("" != document.getElementById("TBCompanyBank[" + i + "][0]").value ||
						"" != document.getElementById("TBCompanyBank[" + i + "][1]").value ||
						"" != document.getElementById("TBCompanyBank[" + i + "][2]").value ||
						"" != document.getElementById("TBCompanyBank[" + i + "][3]").value ||
						"" != document.getElementById("TBCompanyBank[" + i + "][4]").value) {
						TOCompanyBankSum += document.getElementById("TBCompanyBank[" + i + "]Pk").value + "@@" +
									   document.getElementById("TBCompanyBank[" + i + "][0]").value + "@@" +
	    							   document.getElementById("TBCompanyBank[" + i + "][1]").value + "@@" +
	    							   document.getElementById("TBCompanyBank[" + i + "][2]").value + "@@" +
									   document.getElementById("TBCompanyBank[" + i + "][3]").value + "@@" +
								       document.getElementById("TBCompanyBank[" + i + "][4]").value + "####";
					}
				}
			}
			if (COMPANYADDITIONAL61 != form1.BusinessType.value) {
				TO61 = form1.BusinessType.value;
			}
			if (COMPANYADDITIONAL62 != form1.TB_Homepage.value) {
				TO62 = form1.TB_Homepage.value;
			}
			var Temp = "";

			if (Temp == "!") { Temp = ""; }
			if (COMPANYADDITIONAL64 != Temp) { TO64 = Temp; }

			if (TOcompanyname == "*N!C$" && TOregioncode == "*N!C$" && TOcompanyaddress == "*N!C$" && TOcompanytel == "*N!C$" && TOcompanyfax == "*N!C$" && TOpresidentemail == "*N!C$" &&
				TOcompanyno == "*N!C$" && TOstaffsum == "" && TOBranchStorageSum == "" && TOCompanyBankSum == "" && TO61 == "*N!C$" && TO62 == "*N!C$" && TO63 == "*N!C$" && TO64 == "*N!C$" && TO65 == "*N!C$" && TOMEMO == "*N!C$") {
				alert("변경된 내역이 없습니다. ");
				return false;
			}
			Admin.UpdateBranchInfo(GUBUNCL, COMPANYPK, TOcompanyname, TOregioncode, TOcompanyaddress, TOcompanytel, TOcompanyfax, TOpresidentname, TOpresidentemail, TOcompanyno, TO61, TO62, TO63, TO64, TO65, TOstaffsum, TOBranchStorageSum, TOCompanyBankSum, TOMEMO, function (result) {
				if (result == "1") { alert("수정완료"); location.reload(); }
				else { alert(result); }
			}, function (result) { alert("ERROR : " + result); });

		}

		function CheckStaff(StaffID) {
			if (StaffID != "") {
				Admin.CheckStaffID(StaffID, function (result) {
					if (result != "1") {
						alert("관리자 아이디가 아닙니다");
						form1.TB_ResponsibleStaff.value = RESPONSIBLESTAFF;
						form1.TB_ResponsibleStaff.focus();
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}

		function FileDelete(filepk) {
			Admin.FileDelete(filepk, "", function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}

		function DELETESTAFF(rowcount) {
			if (confirm("this row will be delete??")) {
				Admin.DelectAccount(document.getElementById("TBStaffPk[" + rowcount + "]").value, function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}

		function DELETEBranchStorage(rowcount) {
			if (confirm("this row will be delete?")) {
				Admin.DELETEBranchStorage(document.getElementById("TBBranchStorage[" + rowcount + "]Pk").value, function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}

		function DELETECompanyBank(rowcount) {
			if (confirm("this row will be delete?")) {
				Admin.DELETECompanyBank(document.getElementById("TBCompanyBank[" + rowcount + "]Pk").value, function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}

	</script>
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px;">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Admin.asmx" />
			</Services>
		</asp:ScriptManager>
		<uc2:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server" />
		<div style="background-color: White; width: 850px; height: 100%; padding: 25px;">
			<div style="background-color: #999999; padding: 7px;">
				<span style="color: White; font-weight: bold">
					<img src="../Images/ico_arrow.gif" alt="" />
					지사 정보 수정 </span>
			</div>
			<table border="0" cellpadding="0" cellspacing="0" style="width: 850px;">
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
					<td class="td01" style="height: 66px"><%=GetGlobalResourceObject("Member", "CompanyAddress") %></td>
					<td class="td023" colspan="3">
						<span id="spRegionName" style="padding-right: 20px;"></span>
						<input type="hidden" id="RegionCode" />
						<input type="button" value="지역수정" onclick="document.getElementById('spRegionCodeChange').style.visibility = 'visible';" />
						<span style="visibility: hidden;" id="spRegionCodeChange">
							<select id="RegionCode[0]" style="width: 60px; height: 23px;" name="country" onchange="LoadRegionCode('0');"></select>
							<select id="RegionCode[1]" style="width: 60px; height: 23px;" name="office" onchange="LoadRegionCode('1');"></select>
							<select id="RegionCode[2]" style="width: 60px; height: 23px;" name="area" onchange="LoadRegionCode('2');"></select>
						</span>
						<input type="hidden" id="HSelectionArea" value="<%=GetGlobalResourceObject("Member", "SelectionArea") %>" />
						<input type="hidden" id="HSelectionDetailArea" value="<%=GetGlobalResourceObject("Member", "SelectionDetailArea") %>" />
						<input type="text" id="TB_CompanyAddress" size="100" />
					</td>
				</tr>
				<tr>
					<td class="td01">TEL</td>
					<td class="td02">
						<input type="text" id="TB_CompanyTEL[0]" maxlength="20" />
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
						<input type="text" id="TB_CompanyFAX[0]" />
					</td>
					<td class="td01">지사 코드</td>
					<td class="td02">
						<input type="text" id="TB_CompanyCode" size="58" maxlength="20" style="width: 100px" />
						<input type="button" value="지사코드수정" onclick="ModifyCompanyCode();" />
					</td>
				</tr>
				<tr>
					<td class="td01">HomePage</td>
					<td class="td02">
						<input type="text" id="TB_Homepage" />
					</td>
					<td class="td01">E-mail</td>
					<td class="td02">
						<input type="text" id="TB_CompanyEmail" />
					</td>
				</tr>
				<tr>
					<td class="td01">Memo</td>
					<td class="td02" colspan="3">
						<textarea rows="4" cols="46" id="TB_MEMO" maxlength="500" style="width: 600px"></textarea>
					</td>
				</tr>
			</table>
			<div style="background-color: #777777; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #BBBBBB; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #CCCCCC; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #DDDDDD; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #EEEEEE; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #FFFFFF; height: 1px; font-size: 1px;"></div>
			<table id="TabStaff" style="background-color: White; width: 850px;" border="0" cellpadding="0" cellspacing="0">
				<thead>
					<tr>
						<td class="tdSubT" colspan="9">&nbsp;&nbsp;&nbsp;<strong>Staff</strong>
							<input type="button" value="add" onclick="InsertStaff('');" />
						</td>
					</tr>
					<tr style="height: 32px;">
						<td style="width: 100px; height: 20px; background-color: #F5F5F5; text-align: center;"><%= GetGlobalResourceObject("Member", "Duties") %></td>
						<td style="width: 100px; background-color: #F5F5F5; text-align: center;"><%=GetGlobalResourceObject("Member", "Name") %></td>
						<td style="width: 110px; background-color: #F5F5F5; text-align: center;">TEL</td>
						<td style="width: 60px; background-color: #F5F5F5; text-align: center;">MSG</td>
						<td style="width: 140px; background-color: #F5F5F5; text-align: center;">E-mail</td>
						<td style="width: 100px; background-color: #F5F5F5; text-align: center;">Mobile</td>
						<td style="width: 100px; background-color: #F5F5F5; text-align: center;">ID</td>
						<td style="background-color: #F5F5F5; text-align: center;">Password</td>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div style="background-color: #777777; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #BBBBBB; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #CCCCCC; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #DDDDDD; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #EEEEEE; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #FFFFFF; height: 1px; font-size: 1px;"></div>

			<table id="TabBranchStorage" style="background-color: White; width: 850px;" border="0" cellpadding="0" cellspacing="0">
				<thead>
					<tr>
						<td class="tdSubT" colspan="6">&nbsp;&nbsp;&nbsp;<strong>BranchStorage</strong>
							<input type="button" value="add" onclick="InsertBranchStorage();" />
						</td>
					</tr>
					<tr style="height: 32px;">
						<td style="width: 110px; height: 20px; background-color: #F5F5F5; text-align: center;">CompanyName</td>
						<td style="width: 250px; background-color: #F5F5F5; text-align: center;"><%=GetGlobalResourceObject("Member", "Address") %></td>
						<td style="width: 110px; background-color: #F5F5F5; text-align: center;">StaffName</td>
						<td style="width: 110px; background-color: #F5F5F5; text-align: center;">TEL</td>
						<td style="width: 110px; background-color: #F5F5F5; text-align: center;">Mobile</td>
						<td style="background-color: #F5F5F5; text-align: center;">Memo</td>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div style="background-color: #777777; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #BBBBBB; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #CCCCCC; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #DDDDDD; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #EEEEEE; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #FFFFFF; height: 1px; font-size: 1px;"></div>

			<table id="TabCompanyBank" style="background-color: White; width: 850px;" border="0" cellpadding="0" cellspacing="0">
				<thead>
					<tr>
						<td class="tdSubT" colspan="6">&nbsp;&nbsp;&nbsp;<strong>CompanyBank</strong>
							<input type="button" value="add" onclick="InsertCompanyBank();" />
						</td>
					</tr>
					<tr style="height: 32px;">
						<td style="width: 130px; height: 20px; background-color: #F5F5F5; text-align: center;">BankName</td>
						<td style="width: 120px; background-color: #F5F5F5; text-align: center;">OwnerName</td>
						<td style="width: 200px; background-color: #F5F5F5; text-align: center;">AccountNo</td>
						<td style="width: 200px; background-color: #F5F5F5; text-align: center;">Address</td>
						<td style="background-color: #F5F5F5; text-align: center;">BankMemo</td>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div style="background-color: #777777; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #BBBBBB; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #CCCCCC; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #DDDDDD; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #EEEEEE; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #FFFFFF; height: 1px; font-size: 1px;"></div>

			<div style="background-color: #FFFFFF; padding-top: 20px; padding-bottom: 15px; text-align: center;">
				<input type="button" id="BTN_Submit" style="width: 150px; height: 50px;" value="<%=GetGlobalResourceObject("Member", "Modify") %>" onclick="CompanyModify()" />
				<input type="button" value="Delete" onclick="DeleteCompany();" style="color: red;" />
				<div style="clear: both;">&nbsp;</div>
				<input type="hidden" id="HUsedNumber" value="<%=GetGlobalResourceObject("Alert", "CompanyCodeBeingUsed") %>" />
				<input type="hidden" id="HAccountID" value="<%=MEMBERINFO[2] %>" />
				<input type="hidden" id="DEBUG" />
			</div>
		</div>
	</form>
</body>
</html>
