<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestView.aspx.cs" Inherits="Admin_RequestView" Debug="true" %>

<%@ Register Src="LogedWithoutRecentRequest11.ascx" TagName="LogedWithoutRecentRequest11" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title></title>
	<script src="../Common/jquery-1.10.2.min.js" type="text/javascript"></script>
	<style type="text/css">
		.DivEachGroup {
			margin-top: 7px;
			margin-bottom: 7px;
			padding-top: 3px;
			padding-bottom: 3px;
		}
	</style>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		var IsMsgSend = 0;
		function SelectDiv(ID) {
			if (IsMsgSend == 1) {
				if (document.getElementById(ID).style.borderWidth == "2px") {
					document.getElementById(ID).style.borderColor = "blue";
					document.getElementById(ID).style.borderWidth = "0px";
					document.getElementById(ID).style.borderStyle = "solid";
				}
				else {
					document.getElementById(ID).style.borderColor = "blue";
					document.getElementById(ID).style.borderWidth = "2px";
					document.getElementById(ID).style.borderStyle = "solid";
				}
			}
			else {
				return false;
			}
		}
		function CompanyViewLight(Gubun) {
			Admin.LoadCompanyInfoLight(form1.HRequestFormPk.value, Gubun, form1.HDetailView.value, form1.HSangdamList.value, form1.HAccountID.value, function (result) {
				if (result != "0") {
					document.getElementById("PnLeft").innerHTML = result[0];
					document.getElementById("PnRight").innerHTML = result[1];
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function BodyOnload() {
			var Url = location.href;
			var Cut = Url.indexOf("?");
			var PostValue = Url.substring(Cut + 1);
			var EachPostValue = PostValue.split("&");
			if (EachPostValue.length == 2) { CompanyViewLight(EachPostValue[0].split("=")[1]); }
		}
		function ONSUCCESS0(result) {
			if (result != "0") {
				document.getElementById("TB_ShipperCode1").value = result.toString().substr(0, 3);
				document.getElementById("TB_ShipperCode2").value = result.toString().substr(3, result.toString().length - 3);
				document.getElementById("TB_ShipperCode2").readOnly = "readonly";
				document.getElementById("CompanyCodeSave").style.visibility = 'hidden';
				document.getElementById("TB_ShipperCode2").style.border = 0;
			}
			else { window.alert(form1.HError.value); }
		}
		function ONFAILED(result) {
			window.alert(result);
		}
		function SetShipperCompanyCodeManual() {
			var pk = document.getElementById('ShipperPk').value;
			var value = document.getElementById('TB_ShipperCode1').value + document.getElementById('TB_ShipperCode2').value;
			Admin.SetCompanyCustomerCodeManual(pk, value, SuccessSetShipperCompanyCodeManual, ONFAILED);
		}
		function SuccessSetShipperCompanyCodeManual(result) {
			if (result == "0") { window.alert(form1.HUsedNumber.value); }
			else {
				document.getElementById("TB_ShipperCode1").value = result.toString().substr(0, 3);
				document.getElementById("TB_ShipperCode2").value = result.toString().substr(3, result.toString().length - 3);
				document.getElementById("TB_ShipperCode2").readOnly = "readonly";
				document.getElementById("TB_ShipperCode2").style.border = "0";
				document.getElementById("CompanyCodeSave").style.visibility = 'hidden';
			}
		}
		function ModifyCompanyCode(companypk) {
			Admin.AskCompanyCodeUsed(document.getElementById("PnRightCompanyCode").value, function (result) {
				if (result == "N") {
					if (confirm("해당고객번호를 사용중인 업체가 없습니다. \r\n 해당업체에 " + document.getElementById("PnRightCompanyCode").value + "를 부여하겠습니까?")) {
						Admin.SetCompanyCustomerCodeManual(companypk, document.getElementById("PnRightCompanyCode").value, function (result) {
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
					alert(resultSplit[1] + "가 이미 사용중인 고객번호 입니다.");
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function InsertContents(companypk) {
			if (document.getElementById("PnRightTalkBusiness").value.trim() != "") {

				var data = {
					Table_Name: "Company",
					Table_Pk: companypk,
					Category: "Basic",
					Contents: document.getElementById("PnRightTalkBusiness").value,
					Account_Id: form1.HAccountID.value
				}
				$.ajax({
					type: "POST",
					url: "/Process/HistoryP.asmx/Set_Comment",
					data: JSON.stringify(data),
					dataType: "json",
					contentType: "application/json; charset=utf-8",
					success: function (result) {
						alert("SUCCESS");
						location.reload();
					},
					error: function (result) {
						alert('failure : ' + result);
					}
				});
			}
			else {
				alert("내용이 없습니다.");
			}
		}
		function PopupItemModify(pk, AccountID) {	//화물 수정
			var SendValue = new Array();
			SendValue[0] = form1.HAccountID.value;
			SendValue[1] = "A";
			SendValue[2] = form1.HOurBranchPk.value;
			var retVal = window.showModalDialog('../Request/ItemModify.aspx?S=' + pk + '&CL=71', SendValue, "dialogHeight:600px; dialogWidth:900px; resizable:1; status:0; scroll:1; help:0; ");
			if (retVal == "Y") { window.document.location.reload(); }
		}
		function PopupItemModifyList(pk) { //수정내역 보기
			window.open('../Request/ItemModifyList.aspx?S=' + pk + '&CL=71', '', 'location=no, directories=no, resizable=yes, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=600px;');
		}
		function PopupCertificateList(pk) { //수정내역 보기
			window.open('../Request/Dialog/ItemCertificate.aspx?S=' + pk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=100px; left=200px; height=700px; width=800px;');
		}

		function CheckStaff(which) {
			if (document.getElementById("Staff[" + which + "][SendMsg]").checked) {
				if (document.getElementById("Staff[" + which + "][IsSMS]")) {
					document.getElementById("Staff[" + which + "][IsSMS]").checked = true;
				}
				if (document.getElementById("Staff[" + which + "][IsEmail]")) {
					document.getElementById("Staff[" + which + "][IsEmail]").checked = true;
				}
			}
			else {
				if (document.getElementById("Staff[" + which + "][IsSMS]")) {
					document.getElementById("Staff[" + which + "][IsSMS]").checked = false;
				}
				if (document.getElementById("Staff[" + which + "][IsEmail]")) {
					document.getElementById("Staff[" + which + "][IsEmail]").checked = false;
				}
			}
		}
		function ChangeBLFourCode(FourCode) {
			if (document.getElementById("TBBLNo").value.length > 4) {
				document.getElementById("TBBLNo").value = FourCode + document.getElementById("TBBLNo").value.substr(4);
			}
		}
		var MSGSENDSTAFF = new Array();
		function SetRequestFormStep(how) {
			var which = form1.HRequestFormPk.value;
			var CalcHeadPk = document.getElementById("HCalcHeadPk").value;

			switch (how) {
				case "ChargeHistory":
					window.open('../Admin/Dialog/ChargeModifyList.aspx?S=' + form1.HRequestFormPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=900px;');
					break;
				case "Modify":
					var dialogArgument = new Array();
					dialogArgument[0] = form1.HAccountID.value;
					dialogArgument[1] = form1.HOurBranchPk.value;
					var retVal = window.showModalDialog('../Request/RequestWrite.aspx?G=Admin&M=Modify&P=' + which, dialogArgument, "dialogHeight:700px; dialogWidth:1000px; resizable:1; status:0; scroll:1; help:0; ");
					if (retVal == "Y") { window.document.location.reload(); }
					break;
					////////////140125 김상수
				case 'HistoryAppend2886FixCharge':
					if (confirm("Do you really want to operate?")) {
						Admin.AppendRequestFormHistory(form1.HRequestFormPk.value, "82", form1.HAccountID.value, function (result) {
							if (result == "1") {
								alert("SUCCESS");
								location.reload();
							}
						}, function (result) { alert("ERROR : " + result); });
					}
					break;
					////////////140125 김상수
					////////////140213 김상수
				case 'receipt_confirm16':
					if (confirm("계산서 발행합니다.")) {
						Admin.AppendRequestFormHistory(form1.HRequestFormPk.value, "16", form1.HAccountID.value, function (result) {
							if (result == "1") {
								alert("SUCCESS");
								location.reload();
							}
						}, function (result) { alert("ERROR : " + result); });
					}
					break;
				case 'receipt_confirm17':
					if (confirm("계산서 발행을 취소합니다.")) {
						Admin.AppendRequestFormHistory(form1.HRequestFormPk.value, "17", form1.HAccountID.value, function (result) {
							if (result == "1") {
								alert("SUCCESS");
								location.reload();
							}
						}, function (result) { alert("ERROR : " + result); });
					}
					break;
					////////////140213 김상수

				case 'FixCharge':
					/*
					var dialogArgument = form1.HAccountID.value;
					var retVal = window.showModalDialog('./Dialog/FixCharge.aspx?S=' + form1.HRequestFormPk.value, dialogArgument, "dialogHeight:700px; dialogWidth:800px; resizable:0; status:0; scroll:1; help:0; ");
					if (retVal == "Y") { window.document.location.reload(); }
					*/
					window.open('/Charge/Dialog/RequestCharge.aspx?S=' + form1.HRequestFormPk.value + "&A=N", '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, height=650px; width=800px;');
					break;
				case "ModifyCharge":
					/*
					var dialogArgument = "ModifyCharge" + form1.HAccountID.value;
					var retVal = window.showModalDialog('./Dialog/FixCharge.aspx?S=' + form1.HRequestFormPk.value, dialogArgument, "dialogHeight:700px; dialogWidth:800px; resizable:0; status:0; scroll:1; help:0; ");
					if (retVal == "Y") { window.document.location.reload(); }
					*/
					window.open('/Charge/Dialog/RequestCharge.aspx?S=' + form1.HRequestFormPk.value + "&A=Y", '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, height=650px; width=800px;');
					break;
				case "DeliveryCharge":
					var dialogArgument = "DeliveryCharge" + form1.HAccountID.value;
					var retVal = window.showModalDialog('./Dialog/DeliveryCharge.aspx?S=' + form1.HRequestFormPk.value, dialogArgument, "dialogHeight:150px; dialogWidth:390px; resizable:0; status:0; scroll:1; help:0; ");
					if (retVal == "Y") { window.document.location.reload(); }
					break;
				case 'Stoced':
					var dialogArgument = new Array();
					dialogArgument[0] = form1.HAccountID.value;
					dialogArgument[1] = form1.HOurBranchPk.value;
					var retVal = window.showModalDialog('./Dialog/RequestStorcedIn.aspx?S=' + which, dialogArgument, "dialogHeight:250px; dialogWidth:450px; resizable:1; status:0; scroll:1; help:0; ");
					if (retVal == "Y") { window.document.location.reload(); }
					break;
				case 'defer': 	//보류
					var retVal = window.showModalDialog('./Dialog/SetRequestDefer.aspx?S=' + which, form1.HAccountID.value, "dialogHeight:300px; dialogWidth:300px; resizable:1; status:0; scroll:0; help:0; ");
					if (retVal == "Y") { window.document.location.reload(); }
					break;
				case 'Send2':
					var dialogArgument = new Array();
					dialogArgument[0] = form1.HRequestFormPk.value;
					dialogArgument[1] = form1.HAccountID.value;
					dialogArgument[2] = form1.HOurBranchPk.value;
					dialogArgument[3] = form1.HGubun.value;
					var retVal = window.showModalDialog('./Dialog/MsgSendFromRequest.aspx', dialogArgument, "dialogHeight:700px; dialogWidth:480px; resizable:1; status:0; scroll:1; help:0; ");
					if (retVal == "Y") { window.document.location.reload(); }
					break;
					//				case "SendAuto":  
					//					if (confirm("발화인에게 접수메세지 발송하시겠습니까?")) {  
					//						Admin.SendMSGAuto(form1.HRequestFormPk.value, "ShipperOnly", function (result) {  
					//							var msg = result.split('####');  
					//							alert("발송완료 : " + msg[0].replace(/@@/g, '\r\n') + "발송실패 : " + msg[1].replace(/@@/g, '\r\n'));  
					//							//alert("메세지 발송 완료");  
					//						}, function (result) { alert("ERROR : " + result); });  
					//					}  
					//					break; 
				case "deferRestore":
					if (confirm("보류해제하시겠습니까?")) {
						Admin.DeferRestore(form1.HRequestFormPk.value, form1.HAccountID.value, function (result) {
							if (result == "1") {
								alert("SUCCESS");
								location.reload();
							}
						}, function (result) { alert("ERROR : " + result); });
					}
					break;
				case "ShipperCharge":
					window.open('/Charge/Dialog/ChargeView.aspx?S=' + form1.HRequestFormPk.value + "&G=" + CalcHeadPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=800px;');
					break;
				case "ConsigneeCharge":
					window.open('/Charge/Dialog/ChargeView.aspx?S=' + form1.HRequestFormPk.value + "&G=" + CalcHeadPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=800px;');
					break;
				case "ConsigneeCharge_ASECO":
					window.open('/Charge/Dialog/ChargeView.aspx?S=' + form1.HRequestFormPk.value + "&G=" + CalcHeadPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=800px;');
					break;
				case "ConsigneeChargeFI":
					window.open('../Request/FreightChargeView.aspx?S=' + form1.HRequestFormPk.value + "&G=C&T=F", '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=800px;');
					break;
				case "MsgHistory":
					window.open('../Request/Dialog/MsgSendedList.aspx?S=' + form1.HRequestFormPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=900px;');
					break;
				case "DepartureOK":
					if (confirm("OK?")) {
						Admin.UpdateRequestFormDocumentStepCL(form1.HRequestFormPk.value, "0", form1.HAccountID.value, function (result) {
							if (result == "1") {
								alert("SUCCESS");
								location.reload();
							}
						}, function (result) { alert("ERROR : " + result); });
					} else {
						return false;
					}
					break;
				case "OtherBL_Upload":
					window.open('./Dialog/OtherBL_Upload.aspx?G=5&S=' + form1.HRequestFormPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=200px; width=500px;');
					break;
				case "LoadBLNo":
					Admin.LoadBLNo(form1.HRequestFormPk.value, function (result) {
						document.getElementById("DVMakeBL").innerHTML = "<select id='st_BLFourCode' onchange=\"ChangeBLFourCode(this.value);\"><option value='INTL' >INTL</option><option value='AACO' >AACO</option></select> <input type=\"text\" id=\"TBBLNo\" value=\"" + result + "\" /><input type=\"button\" id=\"BTNMakeBLOK\" value=\"OK\" onclick=\"SetRequestFormStep('MakeBL');\" />";
						if (result.length > 4 && result.substr(0, 4) == "AACO") {
							document.getElementById("st_BLFourCode").value = "AACO";
						}

					}, function (result) { alert("ERROR : " + result); });
					break;
				case "MakeBL":
					document.getElementById("BTNMakeBL").style.visibility = "hidden";
					document.getElementById("BTNMakeBLOK").style.visibility = "hidden";
					Admin.MakeBL(form1.HRequestFormPk.value, document.getElementById("st_BLFourCode").value, document.getElementById("TBBLNo").value, form1.HAccountID.value, function (result) {
						if (result[0] == "1") {
							alert("OK");
							document.getElementById("BTNMakeBLOK").style.visibility = "visible";
							document.getElementById("BTNMakeBL").style.visibility = "visible";
							location.reload();
						}
						else if (result[0] == "-1") {
							alert("화폐단위가 서로 달라서 합칠수 없습니다.");
							return false;
						}
						else if (result[0] == "0") {
							if (confirm("이미 생성된 BL번호입니다. 기존비엘과 합치겠습니까?")) {
								Admin.AddCommercialDocument(form1.HRequestFormPk.value, result[1], form1.HAccountID.value, function (result) {
									if (result == "1") {
										alert("OK");
									}
									else {
										alert("FAIL");
									}
								}, function (result) { alert("ERROR : " + result); });
							}
						}
					}, function (result) { alert("ERROR : " + result); });
					document.getElementById("BTNMakeBL").visibility = "visible";
					break;
				case "TempEnd":
					if (confirm("강제 거래완료 됩니다!")) {
						Admin.TempEnd(form1.HRequestFormPk.value, function (result) {
							if (result == "1") {
								alert("완료");
							}
						}, function (result) { alert("ERROR : " + result); });
					}
					break;
				case "CollectByS":
					var dialogArgument = new Array();
					dialogArgument[0] = form1.HRequestFormPk.value;
					dialogArgument[1] = form1.HAccountID.value;
					dialogArgument[2] = "S";
					dialogArgument[3] = form1.HShipperPk.value;
					var retVal = window.showModalDialog('./Dialog/CollectPayment.aspx', dialogArgument, "dialogHeight:500px; dialogWidth:480px; resizable:1; status:0; scroll:1; help:0; ");
					window.document.location.reload();
					break;
				case "CollectByC":
					var dialogArgument = new Array();
					dialogArgument[0] = form1.HRequestFormPk.value;
					dialogArgument[1] = form1.HAccountID.value;
					dialogArgument[2] = "C";
					dialogArgument[3] = form1.HConsigneePk.value;
					var retVal = window.showModalDialog('./Dialog/CollectPayment.aspx', dialogArgument, "dialogHeight:500px; dialogWidth:480px; resizable:1; status:0; scroll:1; help:0; ");
					window.document.location.reload();
					break;
			}
		}
		function CheckAutoNum(which, value) {
			if (value.length == 4 && which == "S") {
				Admin.AutoCompanyCode(value, function (result) {
					//form1.Debug.value = result;
					document.getElementById("ShipperCodeLastNum").value = result;
					document.getElementById("ShipperCodeLastNum").select();
					document.getElementById("ShipperCodeLastNum").focus();
					//alert(result);
				}, function (result) {
					alert(result);
				});
			}
			else if (value.length == 4 && which == "C") {
				document.getElementById("ConsigneeCodeLastNum").focus();
			}
		}
		function SetCompanyCode(Which, CompanyPk) {
			var Value;
			if (Which == "S") {
				Value = document.getElementById("ShipperCodeFirst4").value + document.getElementById("ShipperCodeLastNum").value;
			}
			else if (Which == "C") {
				Value = document.getElementById("ConsigneeCodeFirst4").value + document.getElementById("ConsigneeCodeLastNum").value;
			}
			Admin.SetCompanyCustomerCodeManual(CompanyPk, Value, function (result) {
				if (result == "0") {
					alert("이미 사용중인 고객번호입니다. 수정할수 없습니다. ");
					return false;
				}
				alert(result + "로 고객번호 지정 완료");
				location.reload();
			}, function (result) {
				alert(result);
			});
		}

		function ConnectCCLPKnConsigneeCode() {
			var Value = document.getElementById("ConsigneeCodeFirst4").value + document.getElementById("ConsigneeCodeLastNum").value;
			Admin.SetCompanyCustomerCodeManual(CompanyPk, Value, function (result) {
				if (result == "0") {
					alert("이미 등록된 고객번호입니다.");
					return false;
				}
				alert(result + "로 고객번호 지정 완료");
				location.reload();
			}, function (result) {
				alert(result);
			});
		}
		function ForSetCompanyCode(which) {
			if (which == "S") {
				document.getElementById("PnShipper").style.visibility = "visible";
				document.getElementById("ShipperCodeFirst4").focus();
			}
			else if (which == "C") {
				document.getElementById("PnConsignee").style.visibility = "visible";
				document.getElementById("ConsigneeCodeFirst4").focus();
			}
		}
		function DeleteRequest(requestformpk) {
			if (confirm("삭제하시겠습니까? 복구할수 없습니다.")) {
				Admin.RequestDelete(requestformpk, function (result) {
					if (result == "1") {
						alert("삭제완료");
						history.back();
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function FileUpload(requestFormPk, Gubun) {
			window.open('./Dialog/FileUpload.aspx?G=' + Gubun + '&S=' + requestFormPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;');
		}
		function ShowOtherBL(requestFormPk) {
			location.href = "../UploadedFiles/FileDownload.aspx?S=" + requestFormPk;
		}
		function FileDelete(filepk, table) {
			Admin.FileDelete(filepk, table, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
				else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}

		function ContentsAdd(Category) {
			if (document.getElementById("PnRightTalkRequest").value != "") {
				var data = {
					Table_Name: "RequestForm",
					Table_Pk: form1.HRequestFormPk.value,
					Category: Category,
					Contents: document.getElementById("PnRightTalkRequest").value,
					Account_Id: form1.HAccountID.value
				}
				$.ajax({
					type: "POST",
					url: "/Process/HistoryP.asmx/Set_Comment",
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
			else {
				alert("등록시킬 내용이 없습니다.");
			}
		}

		function GoClearance(Gubun, Value) {
			switch (Gubun) {
				case "Clearance": location.href = "../Admin/CheckDescription.aspx?S=" + Value; break;
				case "ShowBL": location.href = "../CustomClearance/CommercialDocu_HouseBL.aspx?S=" + Value; break;
				case "ShowInvoice": location.href = "../CustomClearance/CommercialDocu_Invoice.aspx?S=" + Value; break;
				case "ShowPacking": location.href = "../CustomClearance/CommercialDocu_PackingList.aspx?S=" + Value; break;
			    case "DO": location.href = "../CustomClearance/CommercialDocu_DeliveryOrder.aspx?S=" + Value + "&B=" + form1.HTransportBetweenBranchPk.value + ""; break;
			}
		}
		function popTalkBusiness(companyPk) {
			window.open('./Dialog/TalkBusiness.aspx?S=' + companyPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=700px; width=600px;');
		}
		function DeliverySet(StoragePk, TransportPk) {
			/*
			window.open('./Dialog/DeliverySet.aspx?P=' + pk + '&S=' + form1.HRequestFormPk.value + "&C=" + form1.HConsigneePk.value + "&O=" + form1.HOurBranchPk.value + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=600px; width=800px;');*/
			window.open('/Transport/Dialog/TransportDelivery.aspx?SB=' + StoragePk + '&R=' + form1.HRequestFormPk.value + "&C=" + form1.HConsigneePk.value + "&T=" + TransportPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=100px; left=200px; height=600px; width=1000px;');
		}
		function DeliveryCancel(tbcpk, RequestFormPk, AccountID) {
			Admin.CancelDeliveryOrder(tbcpk, RequestFormPk, AccountID, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}

		function DeliveryPrint(tbcpk) {
			window.open('./Dialog/DeliveryReceipt2.aspx?G=Print&S=' + tbcpk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=500px; width=900px;');
		}

		function CommentDelete(CommentPk) {
			if (confirm("Will be Deleted")) {
				var data = {
					Comment_Pk: CommentPk
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

		function SetFileGubunCL(filepk, setvalue) {
			Admin.FileGubunCLChange("", filepk, setvalue, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function SetFileGubunCL2(filepk, setvalue) {
			Admin.FileGubunCLChange("ClearancedFile", filepk, setvalue, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					location.reload();
				}
				else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function SetDaeNap(type) {
			var ConfirmComment;
			if (type == "Hold") {
				ConfirmComment = "대납지시 하시겠습니까?";
			}
			else {
				ConfirmComment = "대납완료지시 하시겠습니까?";
			}
			if (confirm(ConfirmComment)) {
				Admin.DaeNapP(type, form1.HRequestFormPk.value, form1.HAccountID.value, function (result) {
					if (result == "1") {
						alert("SUCCESS");
						location.reload();
					}
					else {
						alert(result);
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function SetCarryover(Method, AttachedRequestFormPk, OriginalRequestFormPk, SorC, Amount) {
			if (Method == "reset") {
				if (!confirm("기존 이월자료를 삭제하고 이접수증에 이월시킵니다.")) {
					return false;
				}
			}
			Admin.SetCarryover(Method, AttachedRequestFormPk, OriginalRequestFormPk, SorC, Amount, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					document.getElementById("BTN_CarryOver" + OriginalRequestFormPk).disabled = true;
					//location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		//20131224 관세사작업으로 인한 통관지시
		function DocumentStepCLTo() {
			if (confirm("통관지시하시겠습니까?")) {
				Admin.SetDocumentStepCLToRequestView(form1.HRequestFormPk.value, form1.HAccountID.value, function (result) {
					if (result == "1") {
						alert("SUCCESS");
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
	</script>
</head>
<body style="background-color: #E4E4E4; width: 1100px; margin: 0 auto; padding-top: 10px;" onload="BodyOnload()">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Admin.asmx" />
			</Services>
		</asp:ScriptManager>
		<uc1:LogedWithoutRecentRequest11 ID="LogedWithoutRecentRequest111" runat="server" />

		<div id="google_translate_element"></div><script type="text/javascript">
													 function googleTranslateElementInit() {
														 new google.translate.TranslateElement({ pageLanguage: 'ko', layout: google.translate.TranslateElement.InlineLayout.SIMPLE, multilanguagePage: true }, 'google_translate_element');
													 }
</script><script type="text/javascript" src="//translate.google.com/translate_a/element.js?cb=googleTranslateElementInit"></script>

		<div style="background-color: White; width: 1050px; height: 100%; padding: 25px;">
			<div style="float: right; width: 590px;">
				<div style="width: 580px;"><%=HtmlFileList %></div>
				<fieldset>
					<legend style="font-weight: bold;">Request Comment</legend>
					<%=HtmlCommentList %>
					<%--<input type="text" id="PnRightTalkRequest" style="width: 360px;" />--%>
					<textarea rows="2" cols="70" id="PnRightTalkRequest"></textarea>

					<input type="button" value="OK" onclick="ContentsAdd('Request');" /> 
				</fieldset>
				<%=HtmlMemo %>
				<div style="background-color: #f5f5f5; border: 1px solid black; margin-top: 10px;"><%=RecentClearanceHtml %></div>
				<div id="DVItem" class="DivEachGroup" onclick="SelectDiv(this.id)" <%=IsConsigneeConfirmedStyle %>><%=HtmlItem %></div>
				<div id="DVJubsuWay" class="DivEachGroup"><%=HtmlJubsuWayCL %></div>
				<div id="DVDocumentRequest" class="DivEachGroup" onclick="SelectDiv(this.id)"><%=HtmlDocumentRequest %></div>
				<div><%=HtmlDelivery %></div>
			</div>
			<div style="width: 450px;">
				<div id="DVSchedule" class="DivEachGroup" onclick="SelectDiv(this.id)"><%=HtmlSchedule %></div>
				<div id="DVOwner" class="DivEachGroup" onclick="SelectDiv(this.id)">
					<%=HtmlOwnerOfGoods %>

					<div id="PnLeft"></div>
					<%=HtmlCommentListColored %>

					<div id="PnRight"></div>
					<div style="width: 450px;">
						<fieldset>
							<legend><strong>Clearance File</strong></legend>
							<%=HtmlFileList_Ever %>
						</fieldset>
					</div>
				</div>
				<div>
					<span id="DVMakeBL"></span>
					<fieldset style="padding-top: 5px; padding-bottom: 5px;">
						<legend><strong>Button</strong></legend>
						<%=HtmlButton %><br />
						<%=HtmlButtonPayment %>
						<%=DaeNap %><%=BTN_Onlyilic66 %>
					</fieldset>
				</div>
				<input type="hidden" id="HRequestFormPk" value="<%=Request.Params["pk"] %>" />
				<input type="hidden" id="HOurBranchPk" value="<%=MemberInformation[1] %>" />
				<input type="hidden" id="HAccountID" value="<%=MemberInformation[2] %>" />
				<%=ConsigeeInDocument %>
				<input type="hidden" id="Debug" />
			</div>
			<div style="width: 1050px; margin-top: 10px; border-top: 4px solid #bbbbbb; clear: both;">
				<div style="float: left;" id="DVPayment" class="DivEachGroup" onclick="SelectDiv(this.id)"><%=HtmlPayment %></div>
				<div style="float: left; margin-top: 38px; margin-left: 10px; width: 600px; padding-bottom: 80px;">
					<div style="float: left;"><%=HtmlDeposited %></div>
					<div style="float: left; margin-left: 20px;"><%=Html_SettlementWithCustoms %></div>
					<div style="clear: both;">&nbsp;</div>
					<br />
					<%--<%=HtmlMemo %>--%>
					<fieldset>
						<legend><strong>HISTORY</strong></legend>
						<%=HtmlRequestHistory %>
					</fieldset>
					<div>
						<%=Html_HistoryShipper %>
					</div>
					<div>
						<%=Html_HistoryConsignee %>
					</div>

				</div>
			</div>
			<div style="width: 1050px; clear: both;">
				<input type="hidden" id="HNumberAuto1" value="<%=GetGlobalResourceObject("Alert", "SetCompanyCodeAuto1") %>" />
				<input type="hidden" id="HNumberAuto2" value="<%=GetGlobalResourceObject("Alert", "SetCompanyCodeAuto2") %>" />
				<input type="hidden" id="HUsedNumber" value="<%=GetGlobalResourceObject("Alert", "CompanyCodeBeingUsed") %>" />
				<input type="hidden" id="wlwjddhksfy" value="<%=GetGlobalResourceObject("Alert", "wlwjddhksfy") %>" />
				<input type="hidden" id="HGubun" value="<%=Gubun %>" />
				<input type="hidden" id="PaymentSipperMonetary" value="<%=PaymentShipperMonetary %>" />
				<input type="hidden" id="PaymentConsigneeMonetary" value="<%=PaymentConsigneeMonetary %>" />
				<input type="hidden" id="HShipperPk" value="<%=ShipperPk %>" />
				<input type="hidden" id="HConsigneePk" value="<%=ConsigneePk %>" />
				<input type="hidden" id="HDetailView" value="<%=GetGlobalResourceObject("qjsdur", "tkdtpqhrl") %>" />
				<input type="hidden" id="HSangdamList" value="<%=GetGlobalResourceObject("qjsdur", "tkdekasodur") %>" />

				<input type="hidden" id="Hqhfb" value="<%=GetGlobalResourceObject("qjsdur", "qhfb") %>" />
				<input type="hidden" id="Hwjqtn" value="<%=GetGlobalResourceObject("qjsdur", "wjqtn") %>" />
				<input type="hidden" id="Hghkrdls" value="<%=GetGlobalResourceObject("qjsdur", "ghkrdls") %>" />

				<input type="hidden" id="HvlrdjqdPdir" value="<%=GetGlobalResourceObject("qjsdur", "vlrdjqdPdir") %>" />
				<input type="hidden" id="Hdlqrhdhksfy" value="<%=GetGlobalResourceObject("qjsdur", "dlqrhdhksfy") %>" />
				<input type="hidden" id="Hdnsdlacorwjddhksfy" value="<%=GetGlobalResourceObject("qjsdur", "dnsdlacorwjddhksfy") %>" />
				<input type="hidden" id="Hrhksqnrktpghkrwjd" value="<%=GetGlobalResourceObject("qjsdur", "rhksqnrktpghkrwjd") %>" />
				<input type="hidden" id="Hdnsdlatnwjd" value="<%=GetGlobalResourceObject("qjsdur", "dnsdlatnwjd") %>" />
				<input type="hidden" id="Hqhfbgowp" value="<%=GetGlobalResourceObject("qjsdur", "qhfbgowp") %>" />
				<input type="hidden" id="HTransportBetweenBranchPk" value="<%=BBHPk %>" />
				<input type="hidden" id="HCalcHeadPk" value="<%=CalcHeadPk %>" />
			</div>
		</div>
	</form>
</body>
</html>
