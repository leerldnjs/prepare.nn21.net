<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyInfo.aspx.cs" Inherits="Admin_CompanyInfo" %>

<%@ Register Src="LogedWithoutRecentRequest.ascx" TagName="Loged" TagPrefix="uc1" %>
<%@ Register Src="../CustomClearance/Loged.ascx" TagName="Loged" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
			padding-top: 4px;
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
		var COMPANYADDITIONAL62="";
		var COMPANYADDITIONAL63 = "";
		var COMPANYADDITIONAL64 = "";
		var COMPANYADDITIONAL65 = "";
		var COMPANYADDITIONAL80="";
		var RESPONSIBLESTAFF;
		var STAFFSUM = new Array();
		var WAREHOUSESUM = new Array();
		window.onload = function () {
			LoadRegionCodeCountry("regionCodeI1");

			var Url = location.href;
			var GetValue = Url.substring(Url.indexOf("?") + 1);
			var EachValue = GetValue.split("&");
			var tempcount;
			document.getElementById("SPDELETE").innerHTML = "<input type=\"button\" id=\"BTN_DELETE\"  value=\"delete\" onclick=\"DeleteCompany();\" />";

			if (form1.HAccountID.value == "ilic30" || form1.HAccountID.value == "ilic03" || form1.HAccountID.value == "ilic01" || form1.HAccountID.value == "ilic06") {
				document.getElementById("SPHardDELETE").innerHTML = "<input type=\"button\" id=\"BTN_HardDELETE\" style=\"color:Red;\" value=\"delete\" onclick=\"HardDeleteCompany();\" />";
			}

			if (EachValue[0].substr(0, 8) == "Language") { tempcount = 1; }
			else { tempcount = 0; }
			if (EachValue[tempcount] == "M=View") {
				COMPANYPK = EachValue[(tempcount + 1)].toString().substr(2);
				Admin.LoadCompanyInfo(COMPANYPK, function (result) {
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
					COMPANYNAMEE = Company[9]; form1.TB_CompanyNamee.value = Company[9];
					//COMPANYADDRESSE = Company[10]; form1.TB_CompanyAddressByEng.value = Company[10];
					GUBUNCL = Company[11];
					RESPONSIBLESTAFF = Company[12]; form1.TB_ResponsibleStaff.value = Company[12];
					var StaffRowCount = 0;
					var WarehouseCount = 0;
					var filelist = "";
					for (var i = 1; i < EachGroup.length; i++) {
						var Each = EachGroup[i].split("#@!");
						switch (Each[0]) {
							case "A":
								if (Each[2] + "" != "" && Each[2] + "" != "undefined") { InsertStaff('Company'); }
								else { InsertStaff(''); }
								STAFFSUM[StaffRowCount] = EachGroup[i].substr(4);
								document.getElementById("TBStaffGubun[" + StaffRowCount + "]").value = "A";
								document.getElementById("TBStaffPk[" + StaffRowCount + "]").value = Each[1];
								document.getElementById("TBStaffDuties[" + StaffRowCount + "]").value = Each[3];
								document.getElementById("TBStaffName[" + StaffRowCount + "]").value = Each[4];
								document.getElementById("TBStaffTEL[" + StaffRowCount + "]").value = Each[5];
								document.getElementById("TBStaffEmail[" + StaffRowCount + "]").value = Each[7];
								if (Each[2] + "" != "") {
									document.getElementById("TBStaffGubun[" + StaffRowCount + "]").value = "C";
									document.getElementById("TBStaffID[" + StaffRowCount + "]").value = Each[2];
									document.getElementById("TBStaffDuties[" + StaffRowCount + "]").disabled = "disabled";
									document.getElementById("TBStaffName[" + StaffRowCount + "]").disabled = "disabled";
								}
								document.getElementById("TBStaffMobile0[" + StaffRowCount + "]").value = Each[6];
								document.getElementById("IsEmailSMS[" + StaffRowCount + "]").value = Each[8];
								document.getElementById("TBStaffMemo[" + StaffRowCount + "]").value = Each[9] + "";
								StaffRowCount++;
								break;
							case "W":
								WAREHOUSESUM[WarehouseCount] = new Array();
								WAREHOUSESUM[WarehouseCount][0] = Each[1];
								WAREHOUSESUM[WarehouseCount][1] = Each[2];
								WAREHOUSESUM[WarehouseCount][2] = Each[3];
								WAREHOUSESUM[WarehouseCount][3] = Each[4];
								WAREHOUSESUM[WarehouseCount][4] = Each[5];
								WAREHOUSESUM[WarehouseCount][5] = Each[6];
								WAREHOUSESUM[WarehouseCount][6] = Each[7];
								InsertWarehouse();
								document.getElementById("TBWarehouse[" + WarehouseCount + "]Pk").value = Each[1];
								document.getElementById("TBWarehouse[" + WarehouseCount + "][0]").value = Each[2];
								document.getElementById("TBWarehouse[" + WarehouseCount + "][1]").value = Each[3];
								document.getElementById("TBWarehouse[" + WarehouseCount + "][2]").value = Each[4];
								document.getElementById("TBWarehouse[" + WarehouseCount + "][3]").value = Each[5];
								document.getElementById("TBWarehouse[" + WarehouseCount + "][4]").value = Each[6];
								document.getElementById("TBWarehouse[" + WarehouseCount + "][5]").value = Each[7];
								WarehouseCount++;
								break;
							case "61":
								COMPANYADDITIONAL61 = Each[1];
								form1.BusinessType.value = Each[1];
								break;
							case "62":
								COMPANYADDITIONAL62 = Each[1];
								form1.TB_Homepage.value = Each[1];
								break;
							case "63":
								COMPANYADDITIONAL63 = Each[1];
								var temp = Each[1].split("!");
								for (var j = 0; j < temp.length; j++) {
									switch (temp[j]) {
										case "57": form1.Upjong1.checked = true; break;
										case "58": form1.Upjong2.checked = true; break;
										case "59": form1.Upjong3.checked = true; break;
									}
								}
								break;
							case "64":
								COMPANYADDITIONAL64 = Each[1];
								var temp = Each[1].split("!");
								form1.TB_CategoryofBusiness.value = temp[0];
								form1.TB_BusinessConditions.value = temp[1];
								break;
							case "65":
								COMPANYADDITIONAL65 = Each[1];
								form1.TBMajorItem.value = Each[1];
								break;
							case "80":
								if (Each[1] + "" != "") {
									COMPANYADDITIONAL80 = "true";
									form1.Pyeongtaek.checked = true;
								}
								break;
							case "F":
								filelist += "<a href='../UploadedFiles/FileDownload.aspx?S=" + Each[1] + "' >ㆍ" + Each[2] + " : " + Each[3] + "</a>&nbsp;<span onclick=\"FileDelete('" + Each[1] + "');\" style='color:red;'>X</span><br />";
								break;
						}
					}
					if (filelist != "") {
						document.getElementById("PNFileList").innerHTML = "<fieldset><legend><strong>Attached File</strong></legend>" + filelist + "</fieldset>";
					}
				}, function (result) { alert("ERROR : " + result); });
			}
			if (form1.HAccountID.value != "ilic32" && form1.HAccountID.value != "ilman" && form1.HAccountID.value != "ilic01" && form1.HAccountID.value != "ilic03" && form1.HAccountID.value != "ilic66" && form1.HAccountID.value != "ilic77" && form1.HAccountID.value != "ilic55" && form1.HAccountID.value != "ilic06" && form1.HAccountID.value != "ilic07" && form1.HAccountID.value != "ilic08" && form1.HAccountID.value != "ilyw0" && form1.HAccountID.value != "ilgz0" && form1.HAccountID.value != "ilyt0" && form1.HAccountID.value != "ilgz1" && form1.HAccountID.value != "ilgz2" && form1.HAccountID.value != "ilgz3" && form1.HAccountID.value != "ilgz4" && form1.HAccountID.value != "ilgz5" && form1.HAccountID.value != "ilgz6" && form1.HAccountID.value != "ilgz7" && form1.HAccountID.value != "ilgz8" && form1.HAccountID.value != "ilgz9" && form1.HAccountID.value != "ilqd1" && form1.HAccountID.value != "ilqd2") {
				document.getElementById("Pn_Modify").style.display = "none";
			}

		}
		function delopt(selectCtrl, mTxt) {
			if (selectCtrl.options) {
				for (i = selectCtrl.options.length; i >= 0; i--) { selectCtrl.options[i] = null; }
			}
			selectCtrl.options[0] = new Option(mTxt, "")
			selectCtrl.selectedIndex = 0;
		}
		function LoadRegionCodeCountry(selectID) {
			delopt(document.getElementById(selectID), ":: Country ::");
			Admin.LoadReginoCodeCountry_ReturnWithPk(function (result) {
				for (var i = 0; i < result.length; i++) {
					var thisR = result[i].split("$");
					document.getElementById(selectID).options[i + 1] = new Option(thisR[1], thisR[0]);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function cate1change(first, second, third, idx, TB_RegionPk) {
			delopt(document.getElementById(second), ":: Region ::");
			delopt(document.getElementById(third), ":: Local ::");

			var FirstValue = document.getElementById(first).options[idx].value.split("|");
			Admin.LoadRegionCode_ReturnWithPk(FirstValue[0], function (result) {
				for (var i = 1; i < result.length; i++) {
					var thisR = result[i].split("$");
					document.getElementById(second).options[i] = new Option(thisR[1], thisR[0]);
				}
			});

			document.getElementById(TB_RegionPk).value = FirstValue[1];
		}
		function cate2change(first, second, third, idx, TB_RegionPk) {
			delopt(document.getElementById(third), ":: Local ::");
			cate1 = document.getElementById(first).selectedIndex;

			var SecondValue = document.getElementById(second).options[idx].value.split("|");
			Admin.LoadRegionCode_ReturnWithPk(SecondValue[0], function (result) {
				for (var i = 1; i < result.length; i++) {
					var thisR = result[i].split("$");
					document.getElementById(third).options[i] = new Option(thisR[1], thisR[0]);
				}
			});

			document.getElementById(TB_RegionPk).value = SecondValue[1];
		}
		function cate3change(first, second, third, idx, TB_RegionPk) {
			var ThirdValue = document.getElementById(third).options[idx].value.split("|");
			document.getElementById(TB_RegionPk).value = ThirdValue[1];
		}
		function InsertWarehouse() {		// 상품추가
			var objTable = document.getElementById("TabWarehouse");
			objTable.appendChild(document.createElement("TBODY"));
			var lastRow = objTable.rows.length;
			var thisLineNo = lastRow - 2;
			var objRow1 = objTable.insertRow(lastRow);
			var objCell1 = objRow1.insertCell(); var objCell2 = objRow1.insertCell(); var objCell3 = objRow1.insertCell(); var objCell4 = objRow1.insertCell(); var objCell5 = objRow1.insertCell(); var objCell6 = objRow1.insertCell();
			objCell1.align = "center"; objCell2.align = "center"; objCell3.align = "center"; objCell4.align = "center"; objCell5.align = "center"; objCell6.align = "center";
			objCell1.innerHTML = "<input type=\"hidden\" id=\"TBWarehouse[" + thisLineNo + "]Pk\" /><input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][0]\" style=\"width:90px;\" />";
			objCell2.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][1]\" style=\"width:315px;\" />";
			objCell3.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][2]\" style=\"width:90px;\" />";
			objCell4.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][3]\" style=\"width:60px;\" />";
			objCell5.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][4]\" style=\"width:110px;\" />";
			objCell6.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][5]\" style=\"width:100px;\" />&nbsp;" +
												"<span onclick=\"DELETEWAREHOUSE('" + thisLineNo + "');\" style=\"color:Red; cursor:hand;\" >X</span>";
		}

		function InsertWarehouse__() {		// 상품추가
			var objTable = document.getElementById("TabWarehouse");
			objTable.appendChild(document.createElement("TBODY"));
			var lastRow = objTable.rows.length;
			var thisLineNo = (lastRow - 2) / 2;
			var objRow1 = objTable.insertRow(lastRow);
			var objCell1 = objRow1.insertCell(); var objCell2 = objRow1.insertCell(); var objCell3 = objRow1.insertCell(); var objCell4 = objRow1.insertCell(); var objCell5 = objRow1.insertCell();
			objCell1.align = "center"; objCell2.align = "center"; objCell3.align = "center"; objCell4.align = "center"; objCell5.align = "center";
			objCell1.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][0]\" style=\"width:80px;\" />";
			objCell2.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][1]\" style=\"width:275px;\" />";
			objCell3.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][2]\" style=\"width:110px;\" />";
			objCell4.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][3]\" style=\"width:60px;\" />";
			objCell5.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][4]\" style=\"width:110px;\" /><span onclick=\"DELETEWAREHOUSE('" + thisLineNo + "');\" style=\"color:Red; cursor:hand;\" >X</span>";

			var lastRow = objTable.rows.length;
			var objRow2 = objTable.insertRow(lastRow);
			var objCell21 = objRow2.insertCell(); var objCell22 = objRow2.insertCell();
			var objCell23 = objRow2.insertCell();
			var objCell24 = objRow2.insertCell();
			var objCell25 = objRow2.insertCell();

			objCell21.align = "center"; objCell22.align = "center";
			objCell23.align = "center";
			objCell24.align = "center";
			objCell25.align = "center";

			objCell21.innerHTML = "&nbsp;";
			objCell22.innerHTML = "&nbsp;<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][5]\" style=\"width:275px;\"  />";
			objCell23.innerHTML = "&nbsp;";
			objCell24.innerHTML = "&nbsp;";
			objCell25.innerHTML = "&nbsp;";
		}
		function DeleteCompany() {
			if (confirm("삭제 하시겠습니까? ")) {
				Admin.DeleteCompany(COMPANYPK, function (result) {
					if (result == "1") {
						alert("SUCCESS");
						window.location = 'CompanyList.aspx';
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function HardDeleteCompany() {
			if (confirm("삭제되면 절대로 복구할수 없습니다. 모든 거래내역과 거래처연결내역이 사라집니다 ")) {
				Admin.HardDeleteCompany(COMPANYPK, function (result) {
					if (result == "1") {
						alert("SUCCESS");
						window.location = 'CompanyList.aspx';
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function RestoreCompany() {
			if (confirm("복원하셔도 아이디의 정보는 복원 되지 않습니다")) {
				Admin.RestoreCompany(COMPANYPK, function (result) {
					if (result == "1") {
						alert("SUCCESS");
						window.location = 'CompanyList.aspx';
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

			objCell1.align = "center";
			objCell2.align = "center";
			objCell3.align = "center";
			objCell4.align = "center";
			objCell5.align = "center";
			objCell6.align = "center";
			objCell7.align = "left";

			var rightIcon = "";

			if (gubun == "Company") {
				if (form1.HAccountID.value == "ilic00" || form1.HAccountID.value == "ilic30" || form1.HAccountID.value == "ilogistics" || form1.HAccountID.value == "ilman" || form1.HAccountID.value == "ilyw0" || form1.HAccountID.value == "ilic01") {
					rightIcon = "<span onclick=\"SendSMSAccountInfo('" + thisLineNo + "');\" style=\"color:Blue; cursor:pointer;\" >id/pw</span>" +
                        "<span onclick=\"DELETESTAFF('" + thisLineNo + "');\" style=\"color:Red; cursor:pointer;\" >X</span>";
				} else {
					rightIcon = "<span onclick=\"SendSMSAccountInfo('" + thisLineNo + "');\" style=\"color:Blue; cursor:pointer;\" >id/pw</span>";

				}

			}
			else {
				rightIcon = "<span onclick=\"DELETESTAFF('" + thisLineNo + "');\" style=\"color:Red; cursor:pointer;\" >X</span>";
			}

			objCell1.innerHTML = "<input type=\"hidden\" id=\"TBStaffGubun[" + thisLineNo + "]\" value=\"N\" /><input type=\"hidden\" id=\"TBStaffPk[" + thisLineNo + "]\" /><input type=\"hidden\" id=\"TBStaffID[" + thisLineNo + "]\" /><input type=\"text\" id=\"TBStaffDuties[" + thisLineNo + "]\" style=\"width:60px; text-align:center;\" />";
			objCell2.innerHTML = "<input type=\"text\" id=\"TBStaffName[" + thisLineNo + "]\" style=\"width:60px;\" />";
			objCell3.innerHTML = "<input type=\"text\" id=\"TBStaffTEL[" + thisLineNo + "]\" style=\"width:90px;\" />";
			objCell4.innerHTML = "<select id=\"IsEmailSMS[" + thisLineNo + "]\" ><option value=\"3\">ALL</option><option value=\"2\">Email</option><option value=\"1\">SMS</option><option value=\"0\">X</option> </select>";
			objCell5.innerHTML = "<input type=\"text\" id=\"TBStaffEmail[" + thisLineNo + "]\" style=\"width:145px;\" />";
			objCell6.innerHTML = "<input type=\"text\" id=\"TBStaffMobile0[" + thisLineNo + "]\" style=\"width:105px;\" />";
			objCell7.innerHTML = "<input type=\"text\" id=\"TBStaffMemo[" + thisLineNo + "]\" style=\"width:200px;\" /> " + rightIcon;
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
		function ONFAILED(result) { window.alert(result); }
		function AutoCompanyCodeSuccess(result) {
			form1.CompanyCode2.value = result;
			form1.CompanyCode2.select();
			form1.BTN_Submit.style.visibility = "visible";
			//form1.CompanyCode2.focus();
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
					//alert(result);
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
					if (form1.HAccountID.value == "ilgz0" || form1.HAccountID.value == "ilyw0" || form1.HAccountID.value == "ilic01" || form1.HAccountID.value == "ilman" || form1.HAccountID.value == "ilogistics" || form1.HAccountID.value == "ilsy0" || form1.HAccountID.value == "ilyt0" || form1.HAccountID.value == "ilic06" || form1.HAccountID.value == "ilic03" || form1.HAccountID.value == "ilic08") {
						if (confirm(resultSplit[1] + "가 이미 사용중인 고객번호 입니다. \r\n 기존고객정보에 로그인정보를 덮어쓸까요?")) {
							Admin.UnionTwoCompany(resultSplit[0], COMPANYPK, function (result) {
								if (result == "1") {
									alert("SUCCESS");
								}
								else {
									alert("FAIL");
								}
							}, function (result) { alert("ERROR : " + result); });
						}
					} else {
						alert(form1.TB_CompanyCode.value + "는 이미 사용중인 번호입니다. \r\n기존 사용자 : " + resultSplit[1]);
					}
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function CompanyModify() {
			var TOcompanyname = "*N!C$";
			var TOcompanynamee = "*N!C$";
			var TOregioncode = "*N!C$";
			var TOcompanyaddress = "*N!C$";
			var TOcompanytel = "*N!C$";
			var TOcompanyfax = "*N!C$";
			var TOpresidentname = "*N!C$";
			var TOpresidentemail = "*N!C$";
			var TOcompanyno = "*N!C$";
			var TOstaffsum = "";
			var TOwarehousesum = "";
			var TO61 = "*N!C$";
			var TO62 = "*N!C$";
			var TO63 = "*N!C$";
			var TO64 = "*N!C$";
			var TO65 = "*N!C$";
			var TO80 = "*N!C$";
			var TOresponsiblestaff = "*N!C$";
			if (COMPANYNAME != form1.TB_CompanyName.value) {
				TOcompanyname = form1.TB_CompanyName.value;
			}
			if (COMPANYNAMEE != form1.TB_CompanyNamee.value) {
				TOcompanynamee = form1.TB_CompanyNamee.value;
			}
			if (form1.gcodeExport.value != "") {
				TOregioncode = form1.gcodeExport.value;
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
			if (RESPONSIBLESTAFF != form1.TB_ResponsibleStaff.value) {
				TOresponsiblestaff = form1.TB_ResponsibleStaff.value;
			}
			var StaffRow = document.getElementById('TabStaff').rows.length - 2;
			for (var i = 0; i < StaffRow; i++) {
				//	    		alert(STAFFSUM[i] + "");
				//	    		alert(document.getElementById("TBStaffPk[" + i + "]").value + "#@!" +
				//	    				document.getElementById("TBStaffID[" + i + "]").value + "#@!" +
				//	    				document.getElementById("TBStaffDuties[" + i + "]").value + "#@!" +
				//	    				document.getElementById("TBStaffName[" + i + "]").value + "#@!" +
				//	    				document.getElementById("TBStaffTEL[" + i + "]").value + "#@!" +
				//	    				document.getElementById("TBStaffMobile0[" + i + "]").value + "#@!" +
				//	    				document.getElementById("TBStaffEmail[" + i + "]").value + "#@!" +
				//	    				document.getElementById("IsEmailSMS[" + i + "]").value + "#@!" +
				//	    				document.getElementById("TBStaffMemo[" + i + "]").value);
				if (STAFFSUM[i] != document.getElementById("TBStaffPk[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffID[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffDuties[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffName[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffTEL[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffMobile0[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffEmail[" + i + "]").value + "#@!" +
	    				document.getElementById("IsEmailSMS[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffMemo[" + i + "]").value) {
					if (document.getElementById("TBStaffDuties[" + i + "]").value == "" && document.getElementById("TBStaffName[" + i + "]").value == "") { }
					else {

						TOstaffsum += document.getElementById("TBStaffPk[" + i + "]").value + "#@!" +
	    										document.getElementById("TBStaffDuties[" + i + "]").value + "#@!" +
	    										document.getElementById("TBStaffName[" + i + "]").value + "#@!" +
	    										document.getElementById("TBStaffTEL[" + i + "]").value + "#@!" +
	    										document.getElementById("TBStaffMobile0[" + i + "]").value + "#@!" +
	    										document.getElementById("TBStaffEmail[" + i + "]").value + "#@!" +
	    										document.getElementById("IsEmailSMS[" + i + "]").value + "#@!" +
	    										document.getElementById("TBStaffMemo[" + i + "]").value + "%!$@#";
					}
				}
			}
			var WarehouseRow = document.getElementById("TabWarehouse").rows.length - 2;
			for (var i = 0; i < WarehouseRow; i++) {
				if (WAREHOUSESUM.length > i) {
					if (WAREHOUSESUM[i][1] != document.getElementById("TBWarehouse[" + i + "][0]").value ||
						WAREHOUSESUM[i][2] != document.getElementById("TBWarehouse[" + i + "][1]").value ||
						WAREHOUSESUM[i][3] != document.getElementById("TBWarehouse[" + i + "][2]").value ||
						WAREHOUSESUM[i][4] != document.getElementById("TBWarehouse[" + i + "][3]").value ||
						WAREHOUSESUM[i][5] != document.getElementById("TBWarehouse[" + i + "][4]").value ||
						WAREHOUSESUM[i][6] != document.getElementById("TBWarehouse[" + i + "][5]").value) {
						TOwarehousesum += document.getElementById("TBWarehouse[" + i + "]Pk").value + "@@" +
													document.getElementById("TBWarehouse[" + i + "][0]").value + "@@" +
	    											 document.getElementById("TBWarehouse[" + i + "][1]").value + "@@" +
	    											 document.getElementById("TBWarehouse[" + i + "][2]").value + "@@" +
													 document.getElementById("TBWarehouse[" + i + "][3]").value + "@@" +
	    											 document.getElementById("TBWarehouse[" + i + "][4]").value + "@@" +
													 document.getElementById("TBWarehouse[" + i + "][5]").value + "####";
					}
				}
				else {
					if ("" != document.getElementById("TBWarehouse[" + i + "][0]").value ||
						"" != document.getElementById("TBWarehouse[" + i + "][1]").value ||
						"" != document.getElementById("TBWarehouse[" + i + "][2]").value ||
						"" != document.getElementById("TBWarehouse[" + i + "][3]").value ||
						"" != document.getElementById("TBWarehouse[" + i + "][4]").value ||
						"" != document.getElementById("TBWarehouse[" + i + "][5]").value) {
						TOwarehousesum += document.getElementById("TBWarehouse[" + i + "]Pk").value + "@@" +
													document.getElementById("TBWarehouse[" + i + "][0]").value + "@@" +
	    											 document.getElementById("TBWarehouse[" + i + "][1]").value + "@@" +
	    											 document.getElementById("TBWarehouse[" + i + "][2]").value + "@@" +
													 document.getElementById("TBWarehouse[" + i + "][3]").value + "@@" +
	    											 document.getElementById("TBWarehouse[" + i + "][4]").value + "@@" +
													 document.getElementById("TBWarehouse[" + i + "][5]").value + "####";
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
			if (form1.Upjong1.checked == true) { Temp += "57"; }
			if (form1.Upjong2.checked == true) { Temp += "!58"; }
			if (form1.Upjong3.checked == true) { Temp += "!59"; }
			if (COMPANYADDITIONAL63 != Temp) { TO63 = Temp; }

			Temp = form1.TB_CategoryofBusiness.value + "!" + form1.TB_BusinessConditions.value;
			if (Temp == "!") { Temp = ""; }
			if (COMPANYADDITIONAL64 != Temp) { TO64 = Temp; }

			if (COMPANYADDITIONAL65 != form1.TBMajorItem.value) {
				TO65 = form1.TBMajorItem.value;
			}

			var TempCheck = "";
			if (form1.Pyeongtaek.checked == true) { TempCheck = "true"; }
			if (COMPANYADDITIONAL80 != TempCheck) {
				if (form1.Pyeongtaek.checked == true) {
					TO80 = "true";
				} else {
					TO80 = "";
				}
			}


			if (TOcompanyname == "*N!C$" && TOregioncode == "*N!C$" && TOcompanyaddress == "*N!C$" && TOcompanytel == "*N!C$" && TOcompanyfax == "*N!C$" && TOpresidentemail == "*N!C$" &&
				TOcompanyno == "*N!C$" && TOstaffsum == "" && TOwarehousesum == "" && TO61 == "*N!C$" && TO62 == "*N!C$" && TO63 == "*N!C$" && TO64 == "*N!C$" && TO65 == "*N!C$" && TO80 == "*N!C$" && TOresponsiblestaff == "*N!C$" && TOcompanynamee == "*N!C$") {
				alert("변경된 내역이 없습니다. ");
				return false;
			}
			Admin.UpdateCompanyInfo(GUBUNCL, COMPANYPK, TOcompanyname, TOcompanynamee, TOregioncode, TOcompanyaddress, TOcompanytel, TOcompanyfax, TOpresidentname, TOpresidentemail, TOcompanyno, TO61, TO62, TO63, TO64, TO65, TO80, TOstaffsum, TOwarehousesum, TOresponsiblestaff, function (result) {
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
			//if (document.getElementById("TBStaffGubun[" + rowcount + "]").value == "A") {
			if (confirm("삭제하시겠습니까?")) {
				Admin.DelectAccount(document.getElementById("TBStaffPk[" + rowcount + "]").value, function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					}
				}, function (result) { alert("ERROR : " + result); });
			}
			//}
			//else { alert("삭제할수 없습니다."); }
		}
		function DELETEWAREHOUSE(rowcount) {
			if (confirm("this row will be delete")) {
				Admin.DeleteWarehouse(document.getElementById("TBWarehouse[" + rowcount + "]Pk").value, function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function SendSMSAccountInfo(rowcount) {
			var dialogArgument = new Array();
			dialogArgument[0] = COMPANYNAME;
			dialogArgument[1] = document.getElementById("TBStaffName[" + rowcount + "]").value;
			dialogArgument[2] = document.getElementById("TBStaffMobile0[" + rowcount + "]").value;
			dialogArgument[3] = document.getElementById("TBStaffID[" + rowcount + "]").value;
			dialogArgument[4] = document.getElementById("TBStaffPk[" + rowcount + "]").value;

			var retVal = window.showModalDialog('./Dialog/SendAccountInfoBySMS.aspx', dialogArgument, "dialogHeight:500px; dialogWidth:480px; resizable:1; status:0; scroll:1; help:0; ");
			if (retVal == "Y") { window.document.location.reload(); }
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
		<uc1:Loged ID="Loged1" runat="server" />
		<uc2:Loged ID="Loged2" runat="server" Visible="false" />
		<div style="background-color: White; width: 850px; height: 100%; padding: 25px;">
			<div style="float: right; padding-bottom: 10px;">
				<%=GetGlobalResourceObject("Member", "Staff") %> ID : <%--<input type="text" id="TB_ResponsibleStaff" disabled="disabled"  onfocus ="this.select();" onblur="CheckStaff(this.value);" style="width:85px; text-align:center; font-size:20px;"/>--%>
				<%=TB_ResponsibleStaff %>
			</div>
			<div style="padding-bottom: 10px; width: 500px;">
				<span onclick="Goto('back');" style="cursor: pointer;"><%=GetGlobalResourceObject("qjsdur", "enlfh") %></span>&nbsp;&nbsp;||&nbsp;&nbsp;
		<span onclick="Goto('NewBasic');" style="cursor: pointer;">Total</span>&nbsp;&nbsp;||&nbsp;&nbsp;
		<strong><%=GetGlobalResourceObject("qjsdur", "rlqhswjdqh") %></strong>&nbsp;&nbsp;||&nbsp;&nbsp;
		<span onclick="Goto('request');" style="cursor: pointer;"><%=GetGlobalResourceObject("qjsdur", "rjfosodurqhrl") %></span>
				<input type="hidden" id="HUsedNumber" value="<%=GetGlobalResourceObject("Alert", "CompanyCodeBeingUsed") %>" />
				<input type="hidden" id="HAccountID" value="<%=MEMBERINFO[2] %>" />
				<input type="hidden" id="DEBUG" />
			</div>
			<div style="padding-bottom: 10px; padding-top: 20px; text-align: right;">
				<div id="PNFileList" style="width: 400px; float: left; text-align: left; margin-left: 20px; line-height: 20px;"></div>
			</div>
			<table border="0" cellpadding="0" cellspacing="0" style="width: 850px;">
				<tr>
					<td class="td01" style="width: 100px;"><strong><%= GetGlobalResourceObject("Member", "CompanyName")%></strong></td>
					<td class="td023" colspan="2">
						<input type="text" id="TB_CompanyName" style="width: 250px;" />&nbsp;&nbsp;<input type="text" id="TB_PresidentName" style="width: 100px;" /></td>
					<td class="td01">
						<input type="text" id="TB_CompanyCode" style="width: 100px; font-size: 20px; text-align: center;" />
						&nbsp;
                <%= BTN_ModifyCompanyCode%>
					</td>
				</tr>
				<tr>
					<td class="td01" style="width: 100px;"><strong><%= GetGlobalResourceObject("Member", "CompanyNameE")%></strong></td>
					<td class="td023" colspan="3">
						<input type="text" id="TB_CompanyNamee" name="TB_CompanyNamee" style="width: 250px;" /></td>

				</tr>
				<tr>
					<td class="td01"><strong><%=GetGlobalResourceObject("Member", "CompanyAddress") %></strong></td>
					<td colspan="3" class="td023">
						<span id="spRegionName" style="padding-right: 20px;"></span>
						<input type="hidden" id="gcodeExport" value="" />
						<%=BTN_wldurtnwjd %>
						<span style="visibility: hidden;" id="spRegionCodeChange">
							<select id="regionCodeI1" style="width: 80px;" name="country" size="1" onchange="cate1change('regionCodeI1','regionCodeI2','regionCodeI3', this.selectedIndex,'gcodeExport')">
								<option value="">:: <%=GetGlobalResourceObject("Member", "SelectionCountry2") %> ::</option>
								<option value="1">KOREA&nbsp;한국</option>
								<option value="3">CHINA&nbsp;中國</option>
								<option value="514">Japan&nbsp;日本</option>
								<option value="590">United States</option>
								<option value="594">Mexico</option>
								<option value="586">Other Location</option>
							</select>
							<select id="regionCodeI2" style="width: 120px;" name="office" onchange="cate2change('regionCodeI1','regionCodeI2','regionCodeI3',this.selectedIndex,'gcodeExport')">
								<option value="">:: <%=GetGlobalResourceObject("Member", "SelectionArea") %> ::</option>
							</select>
							<select id="regionCodeI3" style="width: 120px;" name="area" onchange="cate3change('regionCodeI1','regionCodeI2','regionCodeI3',this.selectedIndex,'gcodeExport')">
								<option value="">:: <%=GetGlobalResourceObject("Member", "SelectionDetailArea") %> ::</option>
							</select>
						</span>
						<input type="hidden" id="HSelectionArea" value=":: <%=GetGlobalResourceObject("Member", "SelectionArea") %> ::" />
						<input type="hidden" id="HSelectionDetailArea" value=":: <%=GetGlobalResourceObject("Member", "SelectionDetailArea") %> ::" />
						<div style="padding-top: 4px;">
							<input type="text" id="TB_CompanyAddress" style="width: 480px;" /></div>
					</td>
				</tr>
				<tr>
					<td class="td01">TEL</td>
					<td class="td02">
						<input type="text" id="TB_CompanyTEL[0]" /></td>
					<td class="td01" style="width: 100px;"><%=GetGlobalResourceObject("Member", "SaupjaNo") %></td>
					<td class="td02">
						<select id="BusinessType">
							<option value="54"><%=GetGlobalResourceObject("qjsdur", "rlxk")%></option>
							<option value="55"><%=GetGlobalResourceObject("qjsdur", "qjqdls") %></option>
							<option value="56"><%=GetGlobalResourceObject("qjsdur", "rodlstkdjqwk")%></option>
						</select>
						<input type="text" id="TB_SaupjaNo" maxlength="15" size="25" /></td>
				</tr>
				<tr>
					<td class="td01">FAX</td>
					<td class="td02">
						<input type="text" id="TB_CompanyFAX[0]" /></td>
					<td class="td01">Homepage</td>
					<td class="td02">
						<input type="text" id="TB_Homepage" size="30" /></td>
				</tr>
				<tr>
					<td class="td01">E-mail</td>
					<td class="td02">
						<input type="text" id="TB_CompanyEmail" size="30" /></td>
					<td class="td01"><%=GetGlobalResourceObject("Member", "Upjong") %> / <%=GetGlobalResourceObject("Member", "Uptae") %></td>
					<td class="td02">
						<input type="text" id="TB_CategoryofBusiness" size="8" value="" style="text-align: center;" />
						/ 
				<input type="text" id="TB_BusinessConditions" size="8" value="" style="text-align: center;" /></td>
				</tr>
				<tr>
					<td class="td01">Major Item</td>
					<td class="td02">
						<input type="text" id="TBMajorItem" size="30" /></td>
					<td class="td01"><%=GetGlobalResourceObject("Member", "Upmu") %></td>
					<td class="td02">
						<input type="checkbox" id="Upjong1" />
						<label for="Upjong1"><%=GetGlobalResourceObject("Member", "Production") %></label>
						&nbsp;&nbsp;
				<input type="checkbox" id="Upjong2" />
						<label for="upjong2"><%=GetGlobalResourceObject("Member", "Distribution") %></label>
						&nbsp;&nbsp;
				<input type="checkbox" id="Upjong3" />
						<label for="upjong3"><%=GetGlobalResourceObject("Member", "Saler") %></label>
					</td>
				</tr>
				<tr>
					<td class="td01">평택금지</td>
					<td class="td02">
						<input type="checkbox" id="Pyeongtaek" />
						<label for="Pyeongtaek">평택금지</label>
						</td>
					<td class="td01">&nbsp;</td>
					<td class="td02">&nbsp;</td>
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
						<td class="tdSubT" colspan="7">&nbsp;&nbsp;&nbsp;<strong>staff</strong>
							<input type="button" value="add" onclick="InsertStaff('');" /></td>
					</tr>
					<tr style="height: 30px;">
						<td bgcolor="#F5F5F5" height="20" align="center" style="width: 70px;"><%=GetGlobalResourceObject("Member", "Duties") %></td>
						<td bgcolor="#F5F5F5" align="center" style="width: 70px;"><%=GetGlobalResourceObject("Member", "Name") %></td>
						<td bgcolor="#F5F5F5" align="center" style="width: 120px;">TEL</td>
						<td bgcolor="#F5F5F5" align="center" style="width: 70px;">MSG</td>
						<td bgcolor="#F5F5F5" align="center" style="width: 155px;">E-mail</td>
						<td bgcolor="#F5F5F5" align="center" style="width: 125px;">Mobile</td>
						<td bgcolor="#F5F5F5" align="center">Working hours / Memo</td>
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
			<table id="TabWarehouse" style="background-color: White; width: 850px;" border="0" cellpadding="0" cellspacing="0">
				<thead>
					<tr>
						<td class="tdSubT" colspan="6">&nbsp;&nbsp;&nbsp;<strong>Warehouse</strong>
							<input type="button" value="add" onclick="InsertWarehouse();" /></td>
					</tr>
					<tr style="height: 30px;">
						<td bgcolor="#F5F5F5" height="20" align="center" style="width: 95px;"><%=GetGlobalResourceObject("qjsdur", "tkdgh") %></td>
						<td bgcolor="#F5F5F5" align="center"><%=GetGlobalResourceObject("Member", "Address") %></td>
						<td bgcolor="#F5F5F5" align="center" style="width: 95px;">TEL</td>
						<td bgcolor="#F5F5F5" align="center" style="width: 75px;"><%=GetGlobalResourceObject("qjsdur", "ekaekdwkaud") %></td>
						<td bgcolor="#F5F5F5" align="center" style="width: 110px;">Mobile</td>
						<td bgcolor="#F5F5F5" align="center" style="width: 125px;">Memo</td>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
			<div style="background-color: #FFFFFF; height: 10px; font-size: 10px;"></div>
			<div style="background-color: #777777; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #BBBBBB; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #CCCCCC; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #DDDDDD; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #EEEEEE; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #FFFFFF; height: 1px; font-size: 1px;"></div>
			<div style="background-color: #FFFFFF; padding-top: 20px; padding-bottom: 15px; text-align: center;">
				<%--<span style="font-size:20px; font-weight:bold;"> <%=GetGlobalResourceObject("qjsdur", "ekaekdwk") %> </span>--%>
				<div id="Pn_Modify">
					<input type="button" id="BTN_Submit" style="width: 150px; height: 50px;" value="<%=GetGlobalResourceObject("Member", "Modify") %>" onclick="CompanyModify()" />
					<span id="SPDELETE"></span>
					<span id="SPRestore"></span>
					<span id="SPHardDELETE"></span>
				</div>
				<div style="clear: both;">&nbsp;</div>
			</div>
		</div>
	</form>
</body>
</html>