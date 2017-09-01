<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrepareTransport.aspx.cs" Inherits="Admin_PrepareTransport" %>

<%@ Register Src="LogedWithoutRecentRequest.ascx" TagName="LogedWithoutRecentRequest" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
    <script src="/Common/jquery-1.10.2.min.js"></script>
    <script src="/Lib/ForDialog.js"></script>
	<script type="text/javascript">
		var TotalStorageCount;
		var StorageVolume = new Array();
		var StorageWeight = new Array();
		var TOTALBOXCOUNT = new Array();
		var STORAGELEFT = new Array();
		var IsOpenPnPacked = new Array();
		var PACKEDCOUNT = new Array();

		window.onload = function () {
		    startest();
		}
		$(document).ready(function () {
		    ForDialog.initPopup();
		});
		function startest(value) {
			if (value == "7898") {
				form1.HBranchPk.value = "7898";
			}
			if (value == "2888") {
				form1.HBranchPk.value = "2888";
			}

			Admin.PrepareTransportOnloadOurStorage(form1.HBranchPk.value, function (result) {
				var OurStorage = "";
				var ArrivalName = "";
				var NowStorage = "";
				var TotalBoxCount = 0;
				var TotalWeight = 0;
				var TotalVolume = 0;
				var TotalRequest = 0;
				for (var q = 0; q < result.length; q++) {
					var Each = result[q].split("#@!");
					//alert(Each[16]);


					var TBClass = "TBody1";
					if (Each[23] == "8") {
						TBClass = "TBody1R";
					}

					if (Each[16] != NowStorage) {
						if (NowStorage != "") {
							OurStorage += "<tr><td colspan='8' style=\"background-color:#bbbbbb;\">Total Box : " + TotalBoxCount + " &nbsp;&nbsp;&nbsp;Total Weight : " + parseInt(TotalWeight + 0.00000001) / 100000 + "&nbsp;&nbsp;&nbsp;Total Volume : " + parseInt(TotalVolume + 0.00000001) / 100000 + "</td></tr>";
							TotalBoxCount = 0;
							TotalWeight = 0;
							TotalVolume = 0;
						}
						NowStorage = Each[16];
						ArrivalName = "";
						OurStorage += "<tr><td colspan='8' style=\"background-color:yellow;\"><strong><a onclick=\"OurStorageClick('" + Each[16] + "')\">" + Each[17] + "</a></strong></td></tr>";
					}
					if (Each[12] != ArrivalName) {
						ArrivalName = Each[12];
						OurStorage += "<tr><td colspan='8'><strong><a onclick=\"StorageArrivalClick('" + Each[12] + "', '" + Each[16] + "')\">" + Each[12] + "</a></strong></td></tr>";
					}

					StorageWeight[q] = Each[8];
					StorageVolume[q] = Each[9];
					TOTALBOXCOUNT[q] = Each[6];
					STORAGELEFT[q] = Each[10];
					var TotalCount = "0";
					if (Each[10] != "") {
						TotalCount = Each[10];
						TotalBoxCount += parseInt(Each[10]);
						if (Each[9] != "") { TotalVolume += parseInt(parseFloat(Each[9]) * 100000); }
						if (Each[8] != "") { TotalWeight += parseInt(parseFloat(Each[8]) * 100000); }
					}
					var OnlyReceiveNo = "";
					if (Each[2].length > 2) { OnlyReceiveNo = Each[2].substr(2); }

					var IsDefer = "";
					if (Each[19] == "52") {
						IsDefer = "style='background-color:#ADD8E6;'";
					}
					if (Each[6] == "") {
						OurStorage += "<tr class=\"" + TBClass + "\" >" +
						"<td colspan='3' style='padding-left:13px;'><input type=\"checkbox\" disabled=\"disabled\" id=\"OurStorage[" + q + "]\" value=\"" + Each[0] + "\" /><a href='./RequestView.aspx?g=s&pk=" + Each[0] + "'><span style='color:red; cursor:hand; '>박스수량을 확정지어주세요</span></a>" +
						"<input type=\"hidden\" id=\"OurStorage[" + q + "]AName\" value=\"N\" /> <input type=\"hidden\" id=\"OurStorage[" + q + "]BoxCount\" style='width:30px;text-align:center;' value=\"" + TotalCount + "\"  /> " +
						"</td>" +
						"<td " + IsDefer + "><a href='./RequestView.aspx?g=s&pk=" + Each[0] + "'>" + (Each[21] + "" == "" ? Each[1] : "<span style =\"color:red; font-weight:bold;\">" + Each[1] + "</span>") + " ~ " + (Each[22] + "" == "" ? Each[2] : "<span style =\"color:red; font-weight:bold;\">" + Each[2] + "</span>") + "</a></td>";
					}
					else {
						OurStorage += "<tr class=\"" + TBClass + "\" >" +
						"<td>&nbsp;&nbsp;&nbsp;<input type=\"checkbox\" id=\"OurStorage[" + q + "]\" value=\"" + Each[0] + "\" onclick=\"StorageCheck('" + q + "');\" />" +
						"<span style=\"font-weight:bold; font-size:15px; \">" + OnlyReceiveNo + "</span>" +
						"</td>" +
						"<td " + IsDefer + "><a href='./RequestView.aspx?g=s&pk=" + Each[0] + "'>" + (Each[21] + "" == "" ? Each[1] : "<span style =\"color:red; font-weight:bold;\">" + Each[1] + "</span>") + " ~ " + (Each[22] + "" == "" ? Each[2] : "<span style =\"color:red; font-weight:bold;\">" + Each[2] + "</span>") + "</a></td>" +
						"<td " + IsDefer + "><label for=\"OurStorage[" + q + "]\">" + NumberFormat(Each[9]) + "</label></td>" +
						"<td " + IsDefer + "><label for=\"OurStorage[" + q + "]\">" + NumberFormat(Each[8]) + "</label></td>";
					}


					if (Each[20] != "") {
						var DocumentRequestCL = Each[20].split("!");
						var HtmlDocumentRequest = "";
						for (var k = 0; k < DocumentRequestCL.length; k++) {
							if (DocumentRequestCL[k] == "31") {
								HtmlDocumentRequest += "&nbsp;화주원산지제공(客户提供产地证) ";
							}
							if (DocumentRequestCL[k] == "32") {
								HtmlDocumentRequest += "&nbsp;원산지신청(申请产地证) ";
							} if (DocumentRequestCL[k] == "33") {
								HtmlDocumentRequest += "&nbsp;단증보관(单证报关)";
							}
							if (DocumentRequestCL[k] == "34") {
								HtmlDocumentRequest += "&nbsp;FTA원산지대리신청(FTA代理申请产地证)";
							}
						}


						if (HtmlDocumentRequest != "") {
							OurStorage += "<td " + IsDefer + ">" + Each[11] + "</td>" +
					  "<td " + IsDefer + ">" + Each[18] + "</td>" +
					  "<td " + IsDefer + ">" + Each[3].substr(4, 4) + " ~ " + Each[4].substr(4, 4) + "</td>" +
					  "<td " + IsDefer + ">" + Each[5] + "</td></tr><tr><td>" +
						  "<input type=\"hidden\" id=\"OurStorage[" + q + "]AName\" value=" + Each[12] + " />  " +
						  "<input type=\"hidden\" id=\"OurStorage[" + q + "]NowStorage\" value=" + Each[16] + " />  " +
						  "<label for=\"OurStorage[" + q + "]\"><input type=\"text\" id=\"OurStorage[" + q + "]BoxCount\" style='width:30px;text-align:center;' value=\"" + TotalCount + "\" disabled=\"disabled\" /> / " + Each[6] + "</label>" +
					  "</td><td colspan='7' ><input type=\"text\" id=\"TBRequestComment[" + q + "]\"  onblur=\"BTNAddReqeustComment('" + q + "');\" style='width:600px; font-style:italic; border:0px; ' value=\"" + Each[14] + "\" /></td></tr>";
							OurStorage += "<tr><td class=\"" + TBClass + "\" ></td><td colspan='7' class=\"" + TBClass + "\" style=\"text-align:left;\">" + HtmlDocumentRequest + "</td></tr>";
						} else {
							OurStorage += "<td " + IsDefer + ">" + Each[11] + "</td>" +
				  "<td " + IsDefer + ">" + Each[18] + "</td>" +
				  "<td " + IsDefer + ">" + Each[3].substr(4, 4) + " ~ " + Each[4].substr(4, 4) + "</td>" +
				  "<td " + IsDefer + ">" + Each[5] + "</td></tr><tr><td class=\"" + TBClass + "\" >" +
					  "<input type=\"hidden\" id=\"OurStorage[" + q + "]AName\" value=" + Each[12] + " />  " +
					  "<input type=\"hidden\" id=\"OurStorage[" + q + "]NowStorage\" value=" + Each[16] + " />  " +
					  "<label for=\"OurStorage[" + q + "]\"><input type=\"text\" id=\"OurStorage[" + q + "]BoxCount\" style='width:30px;text-align:center;' value=\"" + TotalCount + "\" disabled=\"disabled\" /> / " + Each[6] + "</label>" +
				  "</td><td colspan='7' class=\"" + TBClass + "\"><input type=\"text\" id=\"TBRequestComment[" + q + "]\"  onblur=\"BTNAddReqeustComment('" + q + "');\" style='width:600px; font-style:italic; border:0px; ' value=\"" + Each[14] + "\" /></td></tr>";
						}
					}
					else {
						OurStorage += "<td " + IsDefer + ">" + Each[11] + "</td>" +
				  "<td " + IsDefer + ">" + Each[18] + "</td>" +
				  "<td " + IsDefer + ">" + Each[3].substr(4, 4) + " ~ " + Each[4].substr(4, 4) + "</td>" +
				  "<td " + IsDefer + ">" + Each[5] + "</td></tr><tr><td class=\"" + TBClass + "\">" +
					  "<input type=\"hidden\" id=\"OurStorage[" + q + "]AName\" value=" + Each[12] + " />  " +
					  "<input type=\"hidden\" id=\"OurStorage[" + q + "]NowStorage\" value=" + Each[16] + " />  " +
					  "<label for=\"OurStorage[" + q + "]\"><input type=\"text\" id=\"OurStorage[" + q + "]BoxCount\" style='width:30px;text-align:center;' value=\"" + TotalCount + "\" disabled=\"disabled\" /> / " + Each[6] + "</label>" +
				  "</td><td colspan='7' class=\"" + TBClass + "\"><input type=\"text\" id=\"TBRequestComment[" + q + "]\"  onblur=\"BTNAddReqeustComment('" + q + "');\" style='width:600px; font-style:italic; border:0px; ' value=\"" + Each[14] + "\" /></td></tr>";
					}
				}
				if (NowStorage != "") {
					OurStorage += "<tr><td colspan='8' style=\"background-color:#bbbbbb;\">Total Box : " + TotalBoxCount + " &nbsp;&nbsp;&nbsp;Total Weight : " + parseInt(TotalWeight + 0.00000001) / 100000 + "&nbsp;&nbsp;&nbsp;Total Volume : " + parseInt(TotalVolume + 0.00000001) / 100000 + "</td></tr>";
				}
				TotalStorageCount = q;
				document.getElementById("OurStorage").innerHTML = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style='width:850px;'><tr>" +
																											"<td style='width:115px; ' >CT</td>" +
																											"<td style='width:130px;'>고객번호</td>" +
																											"<td style='width:40px;' >CBM</td>" +
																											"<td style='width:40px;' >Kg</td>" +
																											"<td style='width:40px;'>" + form1.cnfqkfwl.value + "</td>" +
																											"<td style ='width:240px;'>Shipper</td>" +

																											"<td style='width:90px;'>" + form1.ehckrdlf.value + "</td>" +
																											"<td >운송방법</td>" +
																											"</tr>" + OurStorage + "</table>"; //<td>" + form1.dnsthdtlzhapsxm.value + "</td>
			}, function (result) { alert("ERROR : " + result); });
			Admin.LoadBranchStorage(form1.HBranchPk.value, function (result) {
				document.getElementById("PnStorageSelect").innerHTML = result + "<input type=\"button\" value=\"Change\" onclick=\"ChangeStorage();\" style=\"width:80px; \" />";
			}, function (result) { alert("ERROR : " + result); });
			Admin.PrepareTransportOnloadHead(form1.HBranchPk.value, function (result) {
				if (result[0] == "N") {
					return false;
				}
				//form1.DEBUG.value = result[0];
				//return false;
				var Html = "";
				for (var i = 0; i < result.length; i++) {
					//alert(result[i]);
					var EachGroup = result[i].split("&^%$%^");
					var HTMLFILE = "";
					if (EachGroup[2] != "") {
						var each = EachGroup[2].split("####");
						for (var j = 0; j < each.length; j++) {
							if (each[j] != "" && each[j] != "_") {
								HTMLFILE += "<a href='../UploadedFiles/FileDownload.aspx?S=" + each[j].substr(0, each[j].toString().indexOf("_")) + "'>" + each[j].substr(each[j].toString().indexOf("_") + 1) + "</a><br />";
							}
						}
						if (HTMLFILE != "") { HTMLFILE = "<div style=\"border-bottom:1px dotted #cccccc;\">" + HTMLFILE + "</div>"; }
					}


					//form1.DEBUG.value = each[6];
					var HTMLCOMMENT = "";
					//alert(EachGroup[1]);
					if (EachGroup[1] != "") {
						var each = EachGroup[1].split("####");
						for (var j = 0; j < each.length - 1; j++) {
							if (each[j] != "") {
								var tempEach = each[j].split("@@");
								HTMLCOMMENT += "<strong>" + tempEach[1] + "</strong> : " + tempEach[2] + "<br />";
							}
						}
					}

					var each = EachGroup[0].split("####");
					IsOpenPnPacked[i] = "N";
					var description = each[5].split("#@!");
					var Summary = "";
					if (each[8] != "" || each[9] != "") { Summary = "	<div style='padding-top:10px;'>총 " + each[9] + " 건&nbsp;&nbsp;" + each[8] + " CT&nbsp;&nbsp;" + each[10] + " kg&nbsp;&nbsp;" + each[11] + " CBM  <a onclick=\"PnPackedClick('" + i + "' ,'" + each[1] + "');\">" + form1.dufrhekerl.value + "</a><div id='PnPacked[" + each[1] + "]'></div></div>"; }
					switch (each[0]) {
						case "1":
							if (each[6] == "1") { Html += "<div id='TransportEach' style=\"background-color:#FFFACD; \" >"; }
							else { Html += "<div id='TransportEach' style=\"background-color:#FFA07A; \" >"; }
							Html += "	<input type=\"radio\" name=\"TransportBBHead\" id=\"TransportBBHead[" + each[1] + "]\" value=\"" + each[1] + "\" /><label for=\"TransportBBHead[" + each[1] + "]\"><strong> 항공 => " + each[7] + "</strong>" +
										"		<div style=\"padding-left:20px; width:270px; line-height:20px;  \">" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[0] + " : " + description[3] + " BL No. " + each[2] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + each[3] + " ~ " + each[4] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[1] + " (" + description[4] + ") ~ " + description[2] + " (" + description[5] + ")</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + HTMLFILE + "</div>" +
										"		</div></label>"
							break;
						case "2":
							//alert(each[5]);
							var btnSend = "";
							if (each[6] == "1") { Html += "<div id='TransportEach' style=\"background-color:#FFFACD; \" >"; }
							else { Html += "<div id='TransportEach' style=\"background-color:#98FB98; \" >"; }
							Html += "		<input type=\"radio\" name=\"TransportBBHead\" id=\"TransportBBHead[" + each[1] + "]\" value=\"" + each[1] + "\" /><label for=\"TransportBBHead[" + each[1] + "]\"><strong> 차량 => " + each[7] + "</strong>" +
										"		<div style=\"padding-left:20px; width:270px; line-height:20px;  \">" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[0] + " BL No. " + each[2] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[3] + " : " + description[4] + "  " + description[5] + "(" + description[6] + ")</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + each[3] + " ~ " + each[4] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[1] + " ~ " + description[2] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + HTMLFILE + "</div>" +
										"		</div></label>";
							break;
						case "3":
							if (each[6] == "1") {
								Html += "<div id='TransportEach' style=\"background-color:#FFFACD; \" >";
							}
							else {
								Html += "<div id='TransportEach' style=\"background-color:#c0d4eE; \" >";
							}
							Html += "		<input type=\"radio\" name=\"TransportBBHead\" id=\"TransportBBHead[" + each[1] + "]\" value=\"" + each[1] + "\" /><label for=\"TransportBBHead[" + each[1] + "]\"><strong> 선박 => " + each[7] + "</strong>" +
										"		<div style=\"padding-left:20px; width:270px; line-height:20px;  \">" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[0] + " : " + description[3] + " " + description[5] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[4] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[6] + "  " + description[7] + "  " + description[8] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + each[3] + " ~ " + each[4] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[1] + " ~ " + description[2] + "</div>" + HTMLFILE +
										"		</div></label>";
							break;
						case "4":
							if (each[6] == "1") {
								Html += "<div id='TransportEach' style=\"background-color:#FFFACD; \" >";
							}
							else {
								Html += "<div id='TransportEach' style=\"background-color:gray; \" >";
							}
							Html += "		<input type=\"radio\" name=\"TransportBBHead\" id=\"TransportBBHead[" + each[1] + "]\" value=\"" + each[1] + "\" /><label for=\"TransportBBHead[" + each[1] + "]\"><strong> hand carry => " + each[7] + "</strong>" +
										"		<div style=\"padding-left:20px; width:270px; line-height:20px;  \">" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[0] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + each[3] + " ~ " + each[4] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[1] + " (" + description[3] + ")" + " ~ " + description[2] + " (" + description[4] + ")" + "</div>" + HTMLFILE +
										"		</div></label>";
							break;
						case "5":
							if (each[6] == "1") {
								Html += "<div id='TransportEach' style=\"background-color:#FFFACD; \" >";
							}
							else {
								Html += "<div id='TransportEach' style=\"background-color:#c0d4eE; \" >";
							}
							Html += "		<input type=\"radio\" name=\"TransportBBHead\" id=\"TransportBBHead[" + each[1] + "]\" value=\"" + each[1] + "\" /><label for=\"TransportBBHead[" + each[1] + "]\"><strong> FCL => " + each[7] + "</strong>" +
										"		<div style=\"padding-left:20px; width:270px; line-height:20px;  \">" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[0] + " : " + description[3] + " " + description[5] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[4] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[6] + "  " + description[7] + "  " + description[8] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + each[3] + " ~ " + each[4] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[1] + " ~ " + description[2] + "</div>" + HTMLFILE +
										"		</div></label>";
							break;
						case "6":
							if (each[6] == "1") {
								Html += "<div id='TransportEach' style=\"background-color:#FFFACD; \" >";
							}
							else {
								Html += "<div id='TransportEach' style=\"background-color:#c0d4eE; \" >";
							}
							Html += "		<input type=\"radio\" name=\"TransportBBHead\" id=\"TransportBBHead[" + each[1] + "]\" value=\"" + each[1] + "\" /><label for=\"TransportBBHead[" + each[1] + "]\"><strong> LCL => " + each[7] + "</strong>" +
										"		<div style=\"padding-left:20px; width:270px; line-height:20px;  \">" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[0] + " : " + description[3] + " " + description[5] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[4] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[6] + "  " + description[7] + "  " + description[8] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + each[3] + " ~ " + each[4] + "</div>" +
										"			<div style=\"border-bottom:1px dotted #cccccc;\">" + description[1] + " ~ " + description[2] + "</div>" + HTMLFILE +
										"		</div></label>";
							break;

					}
					var btnSend = "";
					if (each[6] != "1") {
						btnSend = "<input type=\"button\" value=\"" + form1.dPdirqkfthd.value + "\" onclick=\"PackingSend('pre', '" + each[1] + "');\" />";
					}
					Html += "		<div style=\"width:300px; border:1px dotted #FFFFFF; padding:5px;  \"  >" + HTMLCOMMENT +
									"			<input type=\"text\" id=\"Comment[" + each[1] + "]\" style=\"width:230px;\" /><input type=\"button\" value=\"OK\" onclick=\"CommentAdd('" + each[1] + "');\" />" +
									"		</div>" +
									"		<div style='text-align:right;'><input type=\"button\" value=\"File Upload\" onclick=\"GoFileupload('" + each[1] + "');\" /><input type=\"button\" value=\"Cancel\" onclick=\"PackingCancle('" + each[1] + "');\" />" + btnSend + "</div>" +
										Summary + "	</div>";
					document.getElementById("TransportBB").innerHTML = Html;
					//alert(Html);
					//alert(result[i]);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function GoFileupload(pk) {
			window.open('./Dialog/FileUpload.aspx?G=0&S=' + pk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;');
		}

		function StorageCheck(q) {
			if (document.getElementById("OurStorage[" + q + "]").checked == true) {
				//alert(document.getElementById("OurStorage[" + q + "]").value);
				document.getElementById("OurStorage[" + q + "]BoxCount").disabled = "";
				document.getElementById("OurStorage[" + q + "]BoxCount").select();
			}
			else {
				document.getElementById("OurStorage[" + q + "]BoxCount").disabled = "disabled";
				//document.getElementById("OurStorage[" + q + "]BoxCount").select();
			}
			GetTotal();
		}
		function GetTotal() {
			//var RowLength = document.getElementById('ItemTable[0]').rows.length - 2;
			//var MaxCountUnder1 = 0;
			var TotalBoxCount = 0;
			var TotalVolume = 0;
			var TotalWeight = 0;
			var ischecked = false;
			for (q = 0; q < TotalStorageCount; q++) {
				if (document.getElementById("OurStorage[" + q + "]").checked == true) {
					TotalBoxCount += parseInt(parseFloat(document.getElementById("OurStorage[" + q + "]BoxCount").value) * 10000);
					TotalVolume += parseInt(parseFloat(StorageVolume[q]) * 10000);
					TotalWeight += parseInt(parseFloat(StorageWeight[q]) * 10000);
					if (!ischecked) { ischecked = true; }
				}
			}
			if (ischecked) {
				form1.SelectedBoxCount.value = parseInt(TotalBoxCount + 0.00000001) / 10000;
				form1.SelectedVolume.value = parseInt(TotalVolume + 0.00000001) / 10000;
				form1.SelectedWeight.value = parseInt(TotalWeight + 0.00000001) / 10000;
			}
			else {
				form1.SelectedBoxCount.value = 0;
				form1.SelectedVolume.value = 0;
				form1.SelectedWeight.value = 0;
			}
		}

		function StorageArrivalClick(thisvalue, nowStorage) {
			var checkedcount = 0;
			var TotalCount = 0;
			for (q = 0; q < TotalStorageCount; q++) {
				if (thisvalue == document.getElementById("OurStorage[" + q + "]AName").value && nowStorage == document.getElementById("OurStorage[" + q + "]NowStorage").value) {
					TotalCount++;
					if (document.getElementById("OurStorage[" + q + "]").checked == true) {
						checkedcount++;
					}
				}
			}
			if (TotalCount == checkedcount) {
				for (q = 0; q < TotalStorageCount; q++) {
					if (thisvalue == document.getElementById("OurStorage[" + q + "]AName").value && nowStorage == document.getElementById("OurStorage[" + q + "]NowStorage").value) {
						document.getElementById("OurStorage[" + q + "]").checked = false;
						document.getElementById("OurStorage[" + q + "]BoxCount").disabled = "disabled";
					}
				}
			}
			else {
				for (q = 0; q < TotalStorageCount; q++) {
					if (thisvalue == document.getElementById("OurStorage[" + q + "]AName").value && nowStorage == document.getElementById("OurStorage[" + q + "]NowStorage").value) {
						document.getElementById("OurStorage[" + q + "]").checked = true;
						document.getElementById("OurStorage[" + q + "]BoxCount").disabled = "";
					}
				}
			}
			GetTotal();
		}
		function OurStorageClick(thisvalue) {
			var checkedcount = 0;
			var TotalCount = 0;
			for (q = 0; q < TotalStorageCount; q++) {
				if (thisvalue == document.getElementById("OurStorage[" + q + "]NowStorage").value) {
					TotalCount++;
					if (document.getElementById("OurStorage[" + q + "]").checked == true) {
						checkedcount++;
					}
				}
			}
			if (TotalCount == checkedcount) {
				for (q = 0; q < TotalStorageCount; q++) {
					if (thisvalue == document.getElementById("OurStorage[" + q + "]NowStorage").value) {
						document.getElementById("OurStorage[" + q + "]").checked = false;
						document.getElementById("OurStorage[" + q + "]BoxCount").disabled = "disabled";
					}
				}
			}
			else {
				for (q = 0; q < TotalStorageCount; q++) {
					if (thisvalue == document.getElementById("OurStorage[" + q + "]NowStorage").value) {
						document.getElementById("OurStorage[" + q + "]").checked = true;
						document.getElementById("OurStorage[" + q + "]BoxCount").disabled = "";
					}
				}
			}
			GetTotal();
		}
		function SelectTransportWay(gubun) {
			window.open('./Dialog/SetTransportWay.aspx', '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=500px; width=600px;');
		}
		function CommentAdd(BBHeadPk) {
			if (document.getElementById("Comment[" + BBHeadPk + "]").value != "") {
				Admin.AddComment(BBHeadPk, form1.HAccountID.value, document.getElementById("Comment[" + BBHeadPk + "]").value, function (result) {
					alert("완료"); location.reload();
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function ToTransportBB() {
			form1.BTNToTransportBB.style.visibility = "Hidden";
			var TransportBBHeadPk = "";
			for (var i = 0; i < document.getElementsByName("TransportBBHead").length; i++) {
				if (document.getElementsByName("TransportBBHead")[i].checked == true) {
					TransportBBHeadPk = document.getElementsByName("TransportBBHead")[i].value;
					break;
				}
			}
			if (TransportBBHeadPk == "") { alert(form1.dnsthdqkdqjqdmftjsxorgowntpdy.value); return false; }
			var RequestFormSum = new Array();
			var BoxCountSum = new Array();
			var TotalBoxCountSum = new Array();
			var StorageLeft = new Array();
			var BeforeStorage = new Array();
			ValueSumCount = 0;
			for (q = 0; q < TotalStorageCount; q++) {
				if (document.getElementById("OurStorage[" + q + "]").checked == true) {
					if (document.getElementById("OurStorage[" + q + "]BoxCount").value == "0") {
						alert("0 박스는 등록할수 없습니다");
						form1.BTNToTransportBB.style.visibility = "visible";
						return false;
					}
					RequestFormSum[ValueSumCount] = document.getElementById("OurStorage[" + q + "]").value;
					BoxCountSum[ValueSumCount] = document.getElementById("OurStorage[" + q + "]BoxCount").value;
					TotalBoxCountSum[ValueSumCount] = TOTALBOXCOUNT[q];
					StorageLeft[ValueSumCount] = STORAGELEFT[q];
					BeforeStorage[ValueSumCount] = document.getElementById("OurStorage[" + q + "]NowStorage").value;
					ValueSumCount++;
				}
			}
			if (ValueSumCount == 0) {
				alert("선택된 화물이 없습니다.");
				form1.BTNToTransportBB.style.visibility = "visible";
				return false;
			}
			//alert(RequestFormSum[0] + " " + BoxCountSum[0] + " " + TotalBoxCountSum[0] + " " + StorageLeft[0]);
			Admin.TransportBBBodyInsert(form1.HBranchPk.value, BeforeStorage, TransportBBHeadPk, RequestFormSum, BoxCountSum, StorageLeft, form1.HAccountID.value, function (result) {
				//				form1.DEBUG.value = result;
				//				return false;
				alert("Success");
				location.reload();
			}, function (result) { alert("ERROR : " + result); });
		}
		function FixBoxCount() {
			form1.BTNFixCount.style.visibility = "hidden";

			var RequestFormSum = new Array();
			var BoxCountSum = new Array();
			var BeforeStorage = new Array();
			ValueSumCount = 0;
			for (q = 0; q < TotalStorageCount; q++) {
				if (document.getElementById("OurStorage[" + q + "]").checked == true) {
					RequestFormSum[ValueSumCount] = document.getElementById("OurStorage[" + q + "]").value;
					BoxCountSum[ValueSumCount] = document.getElementById("OurStorage[" + q + "]BoxCount").value;

					BeforeStorage[ValueSumCount] = document.getElementById("OurStorage[" + q + "]NowStorage").value;
					ValueSumCount++;
				}
			}
			if (ValueSumCount == 0) {
				alert("선택된 화물이 없습니다.");
				form1.BTNFixCount.style.visibility = "visible";
				return false;
			}
			//alert(RequestFormSum[0] + " " + BoxCountSum[0] + " " + TotalBoxCountSum[0] + " " + StorageLeft[0]);
			Admin.FixBoxCountOnPrepareTransport(form1.HAccountID.value, RequestFormSum, BoxCountSum, BeforeStorage, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
				else {
					alert("ERROR");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function OurBranchStorage_SetStatus(StatusCL) {
			form1.BTN_SetStatus_8.style.visibility = "Hidden";
			form1.BTN_SetStatus_0.style.visibility = "Hidden";


			var RequestFormSum = new Array();
			var BeforeStorage = new Array();
			ValueSumCount = 0;
			for (q = 0; q < TotalStorageCount; q++) {
				if (document.getElementById("OurStorage[" + q + "]").checked == true) {
					if (document.getElementById("OurStorage[" + q + "]BoxCount").value == "0") {
						alert("0 박스는 등록할수 없습니다");
						form1.BTN_SetStatus_8.style.visibility = "visible";
						form1.BTN_SetStatus_0.style.visibility = "visible";
						return false;
					}
					RequestFormSum[ValueSumCount] = document.getElementById("OurStorage[" + q + "]").value;
					BeforeStorage[ValueSumCount] = document.getElementById("OurStorage[" + q + "]NowStorage").value;
					ValueSumCount++;
				}
			}
			if (ValueSumCount == 0) {
				alert("선택된 화물이 없습니다.");
				form1.BTN_SetStatus_8.style.visibility = "visible";
				form1.BTN_SetStatus_0.style.visibility = "visible";
				return false;
			}
			//alert(RequestFormSum[0] + " " + BoxCountSum[0] + " " + TotalBoxCountSum[0] + " " + StorageLeft[0]);
			Admin.OurBranchStorage_SetStatus(BeforeStorage, RequestFormSum, StatusCL, function (result) {
				//				form1.DEBUG.value = result;
				//				return false;
				alert("Success");
				location.reload();
			}, function (result) { alert("ERROR : " + result); });
		}
		function ToStorage() {
			form1.BTNToStorage.style.visibility = "hidden";
			if (document.getElementById("STStorage").value == "0") {
				alert("Please Select WAREHOUSE");
				form1.BTNToStorage.style.visibility = "visible";
				return false;
			}
			var TransportBBHeadPk = "";
			for (var i = 0; i < document.getElementsByName("TransportBBHead").length; i++) {
				if (document.getElementsByName("TransportBBHead")[i].checked == true) {
					TransportBBHeadPk = document.getElementsByName("TransportBBHead")[i].value;
					break;
				}
			}
			var RequestFormSum = new Array();
			var BoxCountSum = new Array();
			//var StorageLeft = new Array();
			ValueSumCount = 0;
			for (q = 0; q < PACKEDCOUNT[i]; q++) {
				if (document.getElementById("Packed[" + TransportBBHeadPk + "][" + q + "]").checked == true) {
					if (document.getElementById("Packed[" + TransportBBHeadPk + "][" + q + "]BoxCount").value == "0") {
						alert("0 박스는 움직일수 없습니다.");
						form1.BTNToStorage.style.visibility = "visible";
						return false;
					}
					RequestFormSum[ValueSumCount] = document.getElementById("Packed[" + TransportBBHeadPk + "][" + q + "]").value;
					BoxCountSum[ValueSumCount] = document.getElementById("Packed[" + TransportBBHeadPk + "][" + q + "]BoxCount").value;
					//TotalBoxCountSum[ValueSumCount] = TOTALBOXCOUNT[q];
					//StorageLeft[ValueSumCount] = STORAGELEFT[q];
					ValueSumCount++;
				}
			}
			if (ValueSumCount == 0) { alert("선택된 화물이 없습니다."); return false; }
			Admin.ReStorcedInFromPacked(document.getElementById("STStorage").value, TransportBBHeadPk, RequestFormSum, BoxCountSum, form1.HAccountID.value, function (result) {
				alert("Success");
				location.reload();
			}, function (result) { alert("ERROR : " + result); });
		}
		function PnPackedClick(count, BBHPk) {
			if (IsOpenPnPacked[count] == "Y") {
				IsOpenPnPacked[count] = "N";
				document.getElementById("PnPacked[" + BBHPk + "]").innerHTML = "";
			}
			else {
				IsOpenPnPacked[count] = "Y";
				Admin.LoadPackedItemDetail(BBHPk, form1.HBranchPk.value, function (result) {
					var Html = "";
					var ArrivalName = "";
					for (var q = 0; q < result.length; q++) {
						var Each = result[q].split("#@!");
						if (Each[15] != ArrivalName) {
							ArrivalName = Each[15];
							Html += "<tr><td colspan='6'><strong>" + Each[15] + "</strong></td></tr>";
						}
						/*
						0		Storage.RequestFormPk, Storage.BoxCount, Storage.StatusCL , R.ShipperCode, R.ConsigneeCode, 
						5		R.DepartureDate, R.ArrivalDate, R.TransportWayCL, R.StepCL, R.StockedDate, 
						10	CH.TotalPackedCount, CH.PackingUnit, CH.TotalGrossWeight, CH.TotalVolume, DepratureR.Name 
						15	ArrivalR.Name */
						var TotalCount = 0;
						if (Each[1] != "") { TotalCount = parseInt(Each[1]); }
						Html += "<tr>" +
						"<td>&nbsp;&nbsp;&nbsp;" +
							"<input type=\"checkbox\" id=\"Packed[" + BBHPk + "][" + q + "]\" value=\"" + Each[0] + "\" onclick=\"PackedCheck('" + BBHPk + "',  '" + q + "');\" />" +
							"<input type=\"hidden\" id=\"Packed[" + BBHPk + "][" + q + "]AName\" value=" + Each[15] + " />  " +
							"<label for=\"Packed[" + BBHPk + "][" + q + "]\"><input type=\"text\" id=\"Packed[" + BBHPk + "][" + q + "]BoxCount\" style='width:30px;text-align:center;' value=\"" + TotalCount + "\" disabled=\"disabled\" /> / " + Each[10] + "</label></td>" +
						"<td><label for=\"Packed[" + BBHPk + "][" + q + "]\">" + NumberFormat(Each[12]) + "</label></td>" +
						"<td><label for=\"Packed[" + BBHPk + "][" + q + "]\">" + NumberFormat(Each[13]) + "</label></td>" +
						"<td>" + Each[3] + "~<br />" + Each[4] + "</td>" +
						"<td>" + Each[5].substr(4, 4) + "~<br />" + Each[6].substr(4, 4) + "</td></tr>";

					}
					PACKEDCOUNT[count] = q;
					document.getElementById("PnPacked[" + BBHPk + "]").innerHTML = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" style='width:330px;'><tr><td style='width:115px; ' >CT</td><td>Kg</td><td>CBM</td><td>고객번호</td><td>" + form1.ehckrdlf.value + "</td></tr>" + Html + "</table>";
					//form1.DEBUG.value = result[0];
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function PackedCheck(BBHPk, q) {
			if (document.getElementById("Packed[" + BBHPk + "][" + q + "]").checked == true) {
				document.getElementById("Packed[" + BBHPk + "][" + q + "]BoxCount").disabled = "";
				document.getElementById("Packed[" + BBHPk + "][" + q + "]BoxCount").select();
			}
			else {
				document.getElementById("Packed[" + BBHPk + "][" + q + "]BoxCount").disabled = "disabled";
			}
			document.getElementById("TransportBBHead[" + BBHPk + "]").checked = true;
		}
		function PackingSend(Gubun, BBHPk) {
			Admin.PackingSend(Gubun, BBHPk, form1.HAccountID.value, function (result) {
				if (result == "0") {
					alert("발송할수 없습니다.");
					return false;
				}
				else {
					alert(form1.dhksfy.value);
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); })
		}
		function PackingModify(BBHPk, TransportWayCL) {
			var dialogArgument = new Array();
			dialogArgument[0] = BBHPk;
			dialogArgument[1] = form1.HAccountID.value;
			dialogArgument[2] = form1.HBranchPk.value;
			dialogArgument[3] = TransportWayCL;
			var retVal = window.showModalDialog('./Dialog/SelectTransportWay.aspx?S=' + BBHPk, dialogArgument, 'dialogWidth=600px;dialogHeight=700px;resizable=1;status=0;scroll=0;help=0;');
			try {
				location.reload();
			}
			catch (ex) { return false; }
			//alert(BBHPk);
		}
		function PackingCancle(BBHPk) {
			Admin.PackingCancle(BBHPk, function (result) {
				//alert(result);
				if (result == "0") {
					alert("화물이 포함되어 있습니다. 해당화물을 모두 재고로 옮긴후 취소해주세요.");
				}
				else {
					alert(form1.wjdtkdcjflehldjTtmqslek.value);
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function BTNAddReqeustComment(value) {
			Admin.InsertRequestComment(document.getElementById("OurStorage[" + value + "]").value, document.getElementById("TBRequestComment[" + value + "]").value, form1.HAccountID.value, function (result) {
				return false;
			}, function (result) { alert("ERROR : " + result); });
		}
		function ChangeStorage() {
			if (document.getElementById("STStorage").value == "0") {
				alert("Please Select WAREHOUSE");
				return false;
			}
			var RequestFormSum = new Array();
			var BoxCountSum = new Array();
			var TotalBoxCountSum = new Array();
			var StorageLeft = new Array();
			var BeforeStorage = new Array();
			ValueSumCount = 0;
			for (q = 0; q < TotalStorageCount; q++) {
				if (document.getElementById("OurStorage[" + q + "]").checked == true && document.getElementById("STStorage").value != document.getElementById("OurStorage[" + q + "]NowStorage").value) {
					if (document.getElementById("OurStorage[" + q + "]BoxCount").value == "0") {
						alert("0 박스는 옮길수 없습니다.");
						return false;
					}
					RequestFormSum[ValueSumCount] = document.getElementById("OurStorage[" + q + "]").value;
					BoxCountSum[ValueSumCount] = document.getElementById("OurStorage[" + q + "]BoxCount").value;
					TotalBoxCountSum[ValueSumCount] = TOTALBOXCOUNT[q];
					StorageLeft[ValueSumCount] = STORAGELEFT[q];
					BeforeStorage[ValueSumCount] = document.getElementById("OurStorage[" + q + "]NowStorage").value;
					ValueSumCount++;
				}
			}
			if (ValueSumCount == 0) { alert("선택된 화물이 없습니다."); return false; }
			//alert(RequestFormSum[0] + " " + BoxCountSum[0] + " " + TotalBoxCountSum[0] + " " + StorageLeft[0]);
			Admin.StorageChange(document.getElementById("STStorage").value, RequestFormSum, BoxCountSum, StorageLeft, BeforeStorage, function (result) {
				alert("Success");
				location.reload();
			}, function (result) { alert("ERROR : " + result); });
		}
		function GoStorage(Value) {
			if (Value == "InBound") {
				location.href = "./PrepareDelivery.aspx";
			}
		}
		function GOCN(Value) {
			if (Value == "InBound") {
				location.href = "./PrepareDelivery.aspx";
			}
		}

	</script>
	<style type="text/css">
		#quick_menu input {
			width: 110px;
			height: 23px;
			margin: 5px;
		}
		div#TransportBB {
		}

		div#TransportEach {
			padding: 10px;
			width: 320px;
			float: left;
			margin-left: 10px;
			margin-right: 10px;
		}

		div#quick_menu {
			border: 1px solid #999999;
			background: #eeeeee;
			clear: both;
			text-align: center;
		}
	</style>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px;">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Admin.asmx" />
			</Services>
		</asp:ScriptManager>
		<uc1:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server" />
		<div style="background-color: White; width: 850px; height: 100%; padding: 25px;">
			<asp:Panel Height="40px" ID="Panel1" Visible="false" runat="server">
				<input type="button" value="CNYW" onclick="startest('2888');" style="width: 100px;" />
				<input type="button" value="CNSX" onclick="startest('7898');" style="width: 100px;" />
			</asp:Panel>
			<input type="hidden" id="HAccountID" value="<%=MEMBERINFO[2] %>" />
			<input type="hidden" id="HBranchPk" value="<%=MEMBERINFO[1] %>" />
			<input type="hidden" id="ehckrdlf" value="<%=GetGlobalResourceObject("qjsdur", "ehckrdlf") %>" />
			<div id="TransportBB"></div>
			<div id="quick_menu">
				<div>
					Box&nbsp;&nbsp;<input id="SelectedBoxCount" style="width: 50px; text-align: center;" />&nbsp;&nbsp;&nbsp;&nbsp;
				CBM&nbsp;&nbsp;<input id="SelectedVolume" style="width: 50px; text-align: center;" />&nbsp;&nbsp;&nbsp;&nbsp;
				Kg&nbsp;&nbsp;<input id="SelectedWeight" style="width: 50px; text-align: center;" />
					<input type="button" value="+ <%=GetGlobalResourceObject("qjsdur", "dnsthddPdircnrk") %> " onclick="SelectTransportWay('T');" style="width: 100px;" />
					<input type="button" value="↑ <%=GetGlobalResourceObject("qjsdur", "ghkanftlerl") %>" id="BTNToTransportBB" onclick="ToTransportBB();" style="width: 100px;" />
					<input type="button" value="↓ <%=GetGlobalResourceObject("qjsdur", "ektlckdrhfh") %>" id="BTNToStorage" onclick="ToStorage();" style="width: 100px;" />
					<input type="button" value="<%=GetGlobalResourceObject("qjsdur", "worhtnwjd") %>" id="BTNFixCount" onclick="FixBoxCount();" style="width: 100px;" />
					<br />
					<input type="button" value="보류" style="width:40px; padding-left:0px; padding-right:0px; text-align:center;" id="BTN_SetStatus_8" onclick="OurBranchStorage_SetStatus('8');"  />
					<input type="button" value="보류해제" style="width:70px; padding-left:0px; padding-right:0px; text-align:center;" id="BTN_SetStatus_0" onclick="OurBranchStorage_SetStatus('0');"  />
					<div id="PnStorageSelect"></div>
				</div>
			</div>
			<div id="OurStorage"></div>
			<input type="hidden" id="DEBUG" onclick="this.select();" />
			<input type="hidden" id="dnsthdtlzhapsxm" value="<%=GetGlobalResourceObject("qjsdur", "dnsthdtlzhapsxm") %>" />
			<input type="hidden" id="wjdtkdcjflehldjTtmqslek" value="<%=GetGlobalResourceObject("qjsdur", "wjdtkdcjflehldjTtmqslek") %>" />
			<input type="hidden" id="dnsthdqkdqjqdmftjsxorgowntpdy" value="<%=GetGlobalResourceObject("qjsdur", "dnsthdqkdqjqdmftjsxorgowntpdy") %>" />
			<input type="hidden" id="cnfqkfwl" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfwl") %>" />
			<input type="hidden" id="rhrorqjsgh" value="<%=GetGlobalResourceObject("qjsdur", "rhrorqjsgh") %>" />
			<input type="hidden" id="dhksfy" value="<%=GetGlobalResourceObject("qjsdur", "dhksfy") %>" />
			<input type="hidden" id="dufrhekerl" value="<%=GetGlobalResourceObject("qjsdur", "dufrhekerl") %>" />
			<input type="hidden" id="dPdirqkfthd" value="<%=GetGlobalResourceObject("qjsdur", "dPdirqkfthd") %>" />
		</div>
	</form>
</body>
</html>
