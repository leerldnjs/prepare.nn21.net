<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckDescription.aspx.cs" Inherits="Admin_CheckDescription" Debug="true" %>
<%@ Register src="LogedWithoutRecentRequest11.ascx" tagname="LogedWithoutRecentRequest11" tagprefix="uc1" %>
<%@ Register src="../CustomClearance/Loged.ascx" tagname="Loged" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../Common/jquery-1.10.2.min.js"></script>
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
	<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
	<style type="text/css">
		.Date{float:left; border:1px solid black; padding:5px; margin:3px; text-align:center; }
		.DateInnerDate{ font-weight:bold;}
		.Shipper{border:solid 1px black; height:90px; margin-top:-1px; padding:5px; }
	</style>
	<script type="text/javascript">
		var CommercialDocumentHeadPk;
		var RequestFormPk = new Array();
		var RequestFormPkComment;
		var GroupStyle = new Array();
		var SHIPPERPK;
		var CONSIGNEEPK;
		var ITEMMONETARYUNIT;
		var EachRequestItemCount = new Array();
		var STAMPIMG;
		var RAN = 0;
		var ISCO = false;
		var readonly = "readonly=\"readonly\"";
		GroupStyle[0] = "background-color:#BBBBBB;";
		GroupStyle[1] = "background-color:#FAEBD7;";
		function InvoiceExcelDown(CDPk) {
			location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?G=InvoiceItem&S=" + CDPk;
		}
		window.onload = function () {
			var URI = location.href;
			if (URI.indexOf("?") == -1) {
				alert("Please Access currect way");
				history.back();
				return false;
			}
			Admin.CheckDescriptionPageLoad(form1.HCommercialDocumentPk.value, function (result) {
				var Law_NmData = "";
				var InfoDocument = "";
				var InfoRequest = "";
				var InfoItem = "";
				var RequestFormCount = 0;
				var ibeforeRequestFormPk = "";
				var itemgruopCount = 0;
				for (var i = 0; i < result.length; i++) {
					var each = result[i].split("#@!");
					switch (each[0]) {
						case "CO":
							ISCO = true;
							document.getElementById("SPCO").innerHTML = "<fieldset><legend><strong>Cerificatie of Origin</strong></legend><ul>" +
								"<li>Name: <input type=\"text\" id=\"COIssueCompanyName\" value=\"" + each[1] + "\" style=\"width:300px; \" /></li>" +
								"<li>Address : <input type=\"text\" id=\"COIssueCompanyAddress\" value=\"" + each[2] + "\" style=\"width:250px; \" /><input type=\"hidden\" id=\"COIssueBranchPk\" value=\"" + each[3] + "\" /></li>" +
								"<li>Total Count : <input type=\"text\" id=\"COTotalCount\" value=\"" + each[4] + "\"  /></li>" +
								"<li>Issue Date : <input type=\"text\" id=\"COIssueDate\" value=\"" + each[5] + "\" /></li>" +
								"<li>File</li></ul></fieldset>";
							break;
						case "D":
							STAMPIMG = each[16];
							CommercialDocumentHeadPk = each[1];
							var FOBNCNF = "";
							var check18 = "";
							var check19 = "";
							var check20 = "";
							var check21 = "";
							var check22 = "";
							var check23 = "";
							switch (each[18]) {
								case "18": check18 = "selected=\"selected\""; break;
								case "19": check19 = "selected=\"selected\""; break;
								case "20": check20 = "selected=\"selected\""; break;
								case "21": check21 = "selected=\"selected\""; break;
								case "22": check22 = "selected=\"selected\""; break;
								case "23": check23 = "selected=\"selected\""; break;
							}
							FOBNCNF = "	<input type=\"text\" id=\"FORnCNF\" value=\"" + each[17] + "\" style=\"width:30px; text-align:center;\" />" +
											"	<select id=\"FOBNCNFMonetaryUnit\" style='width:70px; ' >" +
											"		<option value=\"19\"></option><option value=\"18\" " + check18 + " >RMB ￥</option>" +
											"		<option value=\"19\" " + check19 + " >USD $</option>" +
											"		<option value=\"20\" " + check20 + " >KRW ￦</option>" +
											"		<option value=\"21\" " + check21 + " >JPY Y</option>" +
											"		<option value=\"22\" " + check22 + " >HKD HK$</option>" +
											"		<option value=\"23\" " + check23 + " >EUR €</option>" +
											"	</select>" +
											"	<input type=\"hidden\" id=\"FORnCNFValue20\" value=\"" + each[27] + "\" />" +
											"	<input type=\"text\" id=\"FORnCNFValue\" value=\"" + each[19] + "\" style=\"width:50px;\" />";
							if (each[27] != "-1") {
								FOBNCNF += "&nbsp;&nbsp;&nbsp;￦ " + each[27];
							}

							var MarksNNo = "";
							if (each.length > 27) {
								MarksNNo = each[28];
							}

							var LCNo = "";
							if (each.length > 28) {
								LCNo = each[29];
							}

							InfoDocument = "<div style=\"padding-top:15px; \">" +
"	B/L No : <input type=\"text\" id=\"CDBLNo\" style=\"width:120px;\" value=\"" + each[2] + "\" /><input type=\"hidden\" id=\"HBLNo\" value=\"" + each[2] + "\" /><input type=\"button\" value=\"BLNo수정\" onclick=\"BLNoModify();\" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
"	Invoice No : <input type=\"text\" id=\"CDInvoiceNo\" style=\"width:120px;\" value=\"" + each[3] + "\" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
"	Payment Terms : <input type=\"text\" id=\"CDPaymentTerms\" style=\"width:40px; text-align:center;\"  value=\"" + each[14] + "\" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
"	FOB or CNF : " + FOBNCNF +
"</div>" +
"<div>" +
"	<div style=\"width:340px; float:left;\">Shipper<br />" +
"		<input type=\"hidden\" id=\"CDShipperNamePk\" />" +
"		<input type=\"text\" id=\"CDShipperName\" onclick=\"SelectCompanyInDocument('S');\"  style=\"width:330px;\" value=\"" + each[4] + "\"  /><br />" +
"		<textarea rows=\"3\" cols=\"55\" id=\"CDShipperAddress\" style=\"overflow:hidden;\" >" + each[5] + "</textarea>" +
"	</div>" +
"	<div style=\"width:340px; float:left;\">Consignee<br />" +
"		<input type=\"hidden\" id=\"CDConsigneeNamePk\" />" +
"		<input type=\"text\" id=\"CDConsigneeName\" onclick=\"SelectCompanyInDocument('C');\"  style=\"width:330px;\" value=\"" + each[6] + "\"  /><br />" +
"		<textarea rows=\"3\" cols=\"55\" id=\"CDConsigneeAddress\"  style=\"overflow:hidden;\"  >" + each[7] + "</textarea>" +
"	</div>" +
"	<div style=\"width:340px; float:left;\">Nortify Party<br />" +
"		<input type=\"text\" id=\"CDNortifyPartyName\" style=\"width:330px;\" value=\"" + each[8] + "\"  /><br />" +
"		<textarea rows=\"3\" cols=\"55\" id=\"CDNortifyPartyAddress\" style=\"overflow:hidden;\" >" + each[9] + "</textarea>" +
"	</div>" +
"</div>" +
"<div>" +
"	<div style=\"width:340px; float:left;\">Other Refferance<br /><textarea rows=\"2\" cols=\"53\" id=\"CDOtherRefferance\" style=\"overflow:hidden;\">" + each[15] + "</textarea><br />LC No <input type=\"text\" id=\"LCNo\" style=\"width:270px;\" value=\"" + LCNo + "\"  /><br />Marks and Numbers <input type=\"text\" id=\"MarksNNo\" style=\"width:180px;\" value=\"" + MarksNNo + "\"  /></div>" +
"	<div style=\"width:230px; float:left; padding-top:12px;\">" +
"		Port Of Landing&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"CDPortOfLanding\" style=\"width:100px;\" value=\"" + each[10] + "\"  /><br />" +
"		Final Destination <input type=\"text\" id=\"CDFinalDestination\" style=\"width:100px;\" value=\"" + each[11] + "\"  /><br />" +
"		Carrier <input type=\"text\" id=\"CDCarrier\" style=\"width:162px;\" value=\"" + each[12] + "\"  /><br />" +
"	</div>" +
"	<div style=\"width:230px; float:left; padding-top:12px;\">" +
"		Sailing On <input type=\"text\" id=\"CDSailingOn\" style=\"width:120px;\" value=\"" + each[13] + "\"  /><br />" +
"		Voyage <input type=\"text\" id=\"CDVoyage\" style=\"width:136px;\" value=\"" + each[21] + "\"  /><br />" +
"		VoyageNo <input type=\"text\" id=\"CDVoyageNo\" style=\"width:119px;\" value=\"" + each[20] + "\"  /><br />" +
"	</div>" +
"	<div style=\"width:230px; float:left; padding-top:12px;\">" +
"		ContainerNo <input type=\"text\" id=\"CDContainerNo\" style=\"width:110px;\" value=\"" + each[22] + "\"  /><br />" +
"		SealNo <input type=\"text\" id=\"CDSealNo\" style=\"width:145px;\" value=\"" + each[23] + "\"  /><br />" +
"		ContainerSize <input type=\"text\" id=\"CDContainerSize\" style=\"width:101px;\" value=\"" + each[24] + "\"  /><br />" +
"	</div>" +
"</div>";
							break;
						case "R":
							SHIPPERPK = each[2];
							CONSIGNEEPK = each[3];
							var clearanceDate = each[7];
							if (each[21] != "") {
								clearanceDate = each[21];
							}
							var tempString = "";

							if (form1.HGubun.value == "Customs") {
								if (RequestFormCount == 0) {
									tempString = "<strong>통관예정일</strong> <input id=\"TBClearanceDate\" size=\"10\" style=\"text-align:center;\" value=\"" + clearanceDate + "\" type=\"text\" readonly=\"readonly\" />";
								}
								InfoRequest += "	<div style=\"" + GroupStyle[RequestFormCount % 2] + "\"><strong>" + each[17] + "</strong>&nbsp;&nbsp;&nbsp;<a href=\"../Admin/RequestView.aspx?G=c&pk=" + each[1] + "\">" + each[6] + " ~ " + each[7] + "</a> ( " + each[8] + " )&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + tempString +
													"		<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + each[4] + " : " + each[11] +
													"		<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + each[5] + " : " + each[14] +
													"	</div>";
							}
							else {
								if (RequestFormCount == 0) {
									if (form1.HGubun.value == "Customs") {
										tempString = "<strong>통관예정일</strong> <input id=\"TBClearanceDate\" size=\"10\" style=\"text-align:center;\" value=\"" + clearanceDate + "\" type=\"text\" readonly=\"readonly\" />";
									}
									else {
										tempString = "<strong>통관예정일</strong> <input id=\"TBClearanceDate\" size=\"10\" style=\"text-align:center;\" value=\"" + clearanceDate + "\" type=\"text\" readonly=\"readonly\" />";
									}
								}
								else {
									if (form1.HGubun.value != "Customs") {
										tempString = "<input type=\"button\" value=\"BL분리\" onclick=\"BLSplitView('" + RequestFormCount + "');\" /><span id=\"spBLSplit" + RequestFormCount + "\"></span>";
									}
								}
								InfoRequest += "	<div style=\"" + GroupStyle[RequestFormCount % 2] + "\"><strong>" + each[17] + "</strong>&nbsp;&nbsp;&nbsp;<a href=\"../Admin/RequestView.aspx?G=c&pk=" + each[1] + "\">" + each[6] + " ~ " + each[7] + "</a> ( " + each[8] + " )&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + tempString +
													"		<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + each[4] + " : <a href=\"../Admin/CompanyInfo.aspx?M=View&S=" + each[2] + "\">" + each[11] + "</a> (TEL : " + each[12] + " / FAX : " + each[13] + ")" +
													"		<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + each[5] + " : <a href=\"../Admin/CompanyInfo.aspx?M=View&S=" + each[3] + "\">" + each[14] + "</a> (TEL : " + each[15] + " / FAX : " + each[16] + ")" +
													"	</div>";

							}
							RequestFormPk[RequestFormCount] = each[1];
							RequestFormPkComment = each[1];
							RequestFormCount++;
							break;
						case "I":

							if (each[16] == "") { continue; }
							if (ibeforeRequestFormPk == "") {//맨 처음
								ITEMMONETARYUNIT = each[17] + "";
								RequestFormCount = 0;
								ibeforeRequestFormPk = each[2];
								InfoItem += "	<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:1020px; margin-top:20px; border-top:solid 10px #BBBBBB;  \"><tr>" +
													"		<td style=\"text-align:center; width:60px; \">Box</td>" +
													"		<td style=\"text-align:center; width:95px; \">StyleNo<br />HsCode</td>" +
													"		<td style=\"text-align:center;\">Description</td>" +
													"		<td style=\"text-align:center; width:120px; \">Material</td>" +
													"		<td style=\"text-align:center; width:120px;\">Quantity <br />Box Count</td>" +
													"		<td style=\"text-align:center;width:110px;\" >Unit <br />Amount</td>" +
													"		<td style=\"text-align:center; width:85px; \">G-Weight<br />N-Weight</td>" +
													"		<td style=\"text-align:center; width:60px; \" >Volume</td>" +
													"		<td style=\"text-align:center; width:60px; \" >관 세<br />부가세</td>" +
													"		</tr></thead></table>" +
													"	<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:1020px; " + GroupStyle[RequestFormCount % 2] + " \" id=\"ItemGroup[" + RequestFormCount + "]\"  ><tr>" +
													"		<td style=\"text-align:center; width:60px; font-size:1px; height:1px; \">&nbsp;</td>" +
													"		<td style=\"text-align:center; width:75px; font-size:1px; height:1px; \">&nbsp;</td>" +
													"		<td style=\"text-align:center; width:25px; font-size:1px; height:1px; \">&nbsp;</td>" +
													"		<td style=\"text-align:center; font-size:1px; height:1px; \">&nbsp;</td>" +
													"		<td style=\"text-align:center; width:120px; font-size:1px; height:1px; \">&nbsp;</td>" +
													"		<td style=\"text-align:center; width:120px; font-size:1px; height:1px;\">&nbsp;</td>" +
													"		<td style=\"text-align:center;width:110px; font-size:1px; height:1px;\" >&nbsp;</td>" +
													"		<td style=\"text-align:center; width:85px; font-size:1px; height:1px; \">&nbsp;</td>" +
													"		<td style=\"text-align:center; width:60px; font-size:1px; height:1px; \" >&nbsp;</td>" +
													"		<td style=\"text-align:center; width:60px; font-size:1px; height:1px; \" >&nbsp;</td></tr>";
							}
							else if (ibeforeRequestFormPk != each[2]) {
								EachRequestItemCount[RequestFormCount] = itemgruopCount;
								ibeforeRequestFormPk = each[2];
								RequestFormCount++;
								itemgruopCount = 0;
								InfoItem += "	<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:1020px; " + GroupStyle[RequestFormCount % 2] + "\" id=\"ItemGroup[" + RequestFormCount + "]\" ><tr>" +
													"		<td style=\"text-align:center; width:60px; font-size:1px; height:1px; \">&nbsp;</td>" +
													"		<td style=\"text-align:center; width:95px; font-size:1px; height:1px; \">&nbsp;</td>" +
													"		<td style=\"text-align:center; width:25px; font-size:1px; height:1px; \">&nbsp;</td>" +
													"		<td style=\"text-align:center; font-size:1px; height:1px; \">&nbsp;</td>" +
													"		<td style=\"text-align:center; width:120px; font-size:1px; height:1px; \">&nbsp;</td>" +
													"		<td style=\"text-align:center; width:120px; font-size:1px; height:1px;\">&nbsp;</td>" +
													"		<td style=\"text-align:center;width:110px; font-size:1px; height:1px;\" >&nbsp;</td>" +
													"		<td style=\"text-align:center; width:85px; font-size:1px; height:1px; \">&nbsp;</td>" +
													"		<td style=\"text-align:center; width:60px; font-size:1px; height:1px; \" >&nbsp;</td>" +
													"		<td style=\"text-align:center; width:60px; font-size:1px; height:1px; \" >&nbsp;</td></tr>";
							}
							var temp = "";
							for (var j = 40; j < 52; j++) {
								if (j == each[9]) {
									switch (j) {
										case 40: temp += "<option value=\"40\" selected=\"selected\" >PCS</option>"; break;
										case 41: temp += "<option value=\"41\" selected=\"selected\" >PRS</option>"; break;
										case 42: temp += "<option value=\"42\" selected=\"selected\" >SET</option>"; break;
										case 43: temp += "<option value=\"43\" selected=\"selected\" >S/F</option>"; break;
										case 44: temp += "<option value=\"44\" selected=\"selected\" >YDS</option>"; break;
										case 45: temp += "<option value=\"45\" selected=\"selected\" >M</option>"; break;
										case 46: temp += "<option value=\"46\" selected=\"selected\" >Kg</option>"; break;
										case 47: temp += "<option value=\"47\" selected=\"selected\" >DZ</option>"; break;
										case 48: temp += "<option value=\"48\" selected=\"selected\" >L</option>"; break;
										case 49: temp += "<option value=\"49\" selected=\"selected\" >BOX</option>"; break;
										case 50: temp += "<option value=\"50\" selected=\"selected\" >SQM</option>"; break;
										case 51: temp += "<option value=\"51\" selected=\"selected\" >M2</option>"; break;
										case 52: temp += "<option value=\"52\" selected=\"selected\" >RO</option>"; break;
									}
								}
								else {
									switch (j) {
										case 40: temp += "<option value=\"40\" >PCS</option>"; break;
										case 41: temp += "<option value=\"41\" >PRS</option>"; break;
										case 42: temp += "<option value=\"42\" >SET</option>"; break;
										case 43: temp += "<option value=\"43\" >S/F</option>"; break;
										case 44: temp += "<option value=\"44\" >YDS</option>"; break;
										case 45: temp += "<option value=\"45\" >M</option>"; break;
										case 46: temp += "<option value=\"46\" >Kg</option>"; break;
										case 47: temp += "<option value=\"47\" >DZ</option>"; break;
										case 48: temp += "<option value=\"48\" >L</option>"; break;
										case 49: temp += "<option value=\"49\" >BOX</option>"; break;
										case 50: temp += "<option value=\"50\" >SQM</option>"; break;
										case 51: temp += "<option value=\"51\" >M2</option>"; break;
										case 52: temp += "<option value=\"52\" >RO</option>"; break;
									}
								}
							}
							var tempQuantityUnit = "<select id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][QuantityUnit]\">" + temp + "</select>";
							for (var k = 15; k < 18; k++) {
								if (k == each[11]) {
									switch (k) {
										case 15: temp += "<option value=\"15\" selected=\"selected\" >CT</option>"; break;
										case 16: temp += "<option value=\"16\" selected=\"selected\" >RO</option>"; break;
										case 17: temp += "<option value=\"17\" selected=\"selected\" >PA</option>"; break;
									}
								}
								else {
									switch (k) {
										case 15: temp += "<option value=\"15\"  >CT</option>"; break;
										case 16: temp += "<option value=\"16\"  >RO</option>"; break;
										case 17: temp += "<option value=\"17\"  >PA</option>"; break;
									}
								}
							}
							var tempPackingUnit = "<select id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][PackingUnit]\" size=\"1\" >" + temp + "</select>";
							/*
							0		1										2								3						4								5						6				7						8						9							10						11						12						
							13				14						15				16
							I		RequestFormItemsPk		RequestFormPk		ItemCode			MarkNNumber			Description		Label		Material			Quantity			QuantityUnit		PackedCount		PackingUnit			GrossWeight											Volume		monetaryUnit		UnitPrice		Amount
							RS["CDescription"] + "#@!" + RS["CMaterial"] + "#@!" + RS["TarriffRate"] + "#@!" + RS["AdditionalTaxRate"]);
							*/
							var tempguansedisable = "";
							var tempbugasedisable = "";

							if (each[26] != "") { Law_NmData += each[25] + ":" + each[26] + "!!!"; }
							if (each[20] != "") { tempguansedisable = "disabled = \"disabled\""; }
							if (each[21] != "") { tempbugasedisable = "disabled = \"disabled\""; }
							var CO1 = "&nbsp;";
							var CO2 = "&nbsp;";
							if (ISCO) {
								CO1 = "	<input type=\"checkbox\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][IsCo]\" />" +
									  "	<label for=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][IsCo]\">CO</label>";
								CO2 = " <input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][HSCODE]\" value=\"" + each[24] + "\" style=\"width:45px;\" />";
							} else {
								CO2 = " <input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][HsCcode]\" value=\"" + each[25] + "\" style=\"width:90px;\" />";
							}
							var onclick_ShowItemHistory_A = "";
							if (form1.HAccountID.value == "ilic03" || form1.HAccountID.value == "ilic30" || form1.HAccountID.value == "ilic06" || form1.HAccountID.value == "ilic01" || form1.HAccountID.value == "ilic07" || form1.HAccountID.value == "ilic08") {
								onclick_ShowItemHistory_A = "onclick=\"ShowItemHistory_A('" + RequestFormCount + "', '" + itemgruopCount + "');\"";
							}

							InfoItem += "	<tr>" +
"		<td align=\"center\" ><input type=\"hidden\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][RequestFormItemsPk]\" value=\"" + each[1] + "\" />" +
		"	<input type=\"hidden\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][ItemCode]\" value=\"" + each[3] + "\" />" +
		"	<input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][MarkNNumber]\" value=\"" + each[4] + "\" style=\"width:45px;\" /></td >" +
"		<td align=\"center\" ><input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][Brand]\" value=\"" + each[6] + "\" style=\"width:90px;\" /></td>" +
"		<td align=\"center\" rowspan=\"2\" ><input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][RAN]\" value=\"" + each[23] + "\" style=\"width:15px; text-align:center; font-weight:bold;\" onkeydown=\"CheckRAN('" + RequestFormCount + "', '" + itemgruopCount + "');\" /></td>" +
"		<td align=\"center\" ><input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][ItemName]\" onclick=\"ShowItemHistory('" + RequestFormCount + "', '" + itemgruopCount + "');\" value='" + each[5] + "' style=\"width:270px;\" /></td>" +
"		<td align=\"center\" ><input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][Matarial]\" " + onclick_ShowItemHistory_A + " value=\"" + each[7] + "\" style=\"width:110px;\" /></td >" +
"		<td align=\"center\" rowspan=\"2\" >" +
"			<input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][Quantity]\" value=\"" + each[8] + "\" style=\"text-align:right;\" size=\"5\" />" + tempQuantityUnit + "<br />" +
"			<input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][PackedCount]\" value=\"" + each[10] + "\"  style=\"width:60px; text-align:right;\" />" + tempPackingUnit +
"		</td>" +
"		<td align=\"center\" rowspan=\"2\" >" +
"			<input type=\"text\" style=\"border:0; width:10px; \" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][MonetaryUnit][0]\" value=\"" + each[14] + "\"  /><input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][UnitCost]\" value=\"" + each[15] + "\"  style=\"width:60px;\" /><br />" +
"			<input type=\"text\" style=\"border:0; width:10px; \" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][MonetaryUnit][1]\" value=\"" + each[14] + "\" /><input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][Price]\" value=\"" + each[16] + "\"  readonly=\"readonly\"  style=\"width:70px;\" />" +
"		</td>" +
"		<td align=\"center\" rowspan=\"2\" >" +
"			<input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][GWeight]\" value=\"" + each[12] + "\"  style=\"width:50px; text-align:right;\" /> Kg<br />" +
"			<input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][NWeight]\" value=\"" + each[22] + "\"  style=\"width:50px; text-align:right;\" /> Kg" +
"		</td>" +
"		<td align=\"center\" rowspan=\"2\" >" +
"			<input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][Volume]\" value=\"" + each[13] + "\"  style=\"width:50px; text-align:center;\" /><br />" +
"			CBM" +
"		</td>" +
"		<td><input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][Guanse]\" value=\"" + each[20] + "\" " + tempguansedisable + "   style=\"width:30px; text-align:center;\" /> %</td>" +
"	</tr><tr>" +
"		<td style=\"text-align:center;\">" + CO1 + "</td>" +
"		<td style=\"text-align:center;\">" + CO2 + "</td>" +
"		<td align=\"center\" ><input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][ItemNameByE]\" value=\"" + each[18] + "\"  onkeyup=\"SetItemCodeNew('" + RequestFormCount + "', '" + itemgruopCount + "');\" style=\"width:270px;\" /></td>" +
"		<td align=\"center\" ><input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][MatarialByE]\" value=\"" + each[19] + "\"  onkeyup=\"SetItemCodeNew('" + RequestFormCount + "', '" + itemgruopCount + "');\" style=\"width:110px;\" /></td >" +
"		<td><input type=\"text\" id=\"Item[" + RequestFormCount + "][" + itemgruopCount + "][Bugase]\" value=\"" + each[21] + "\" " + tempbugasedisable + "   style=\"width:30px; text-align:center;\" /> %</td>" +
"	</tr>";
							itemgruopCount++;
							EachRequestItemCount[RequestFormCount] = itemgruopCount;
							break;
						case "T":
							switch (each[1]) {
								case "관세": form1.TotalGuanse.value = each[3]; break;
								case "부가세": form1.TotalBugase.value = each[3]; break;
								default: form1.ClearanceFee.value = each[3]; break;
							}
							break;
					}
				}
				document.getElementById("SpMainContents").innerHTML = InfoRequest + InfoDocument + InfoItem;

				if (Law_NmData != "") {
					var arrLawData = Law_NmData.split("!!!");
					var html = "";
					for (var i = 0; i < arrLawData.length; i++) {
						if (html.indexOf(arrLawData[i]) < 0) {
							html += arrLawData[i] + "<br />";
						}
					}
					document.getElementById("SPLaw_NmData").innerHTML = "<fieldset><legend><strong>HSCODE Law</strong></legend>" + html + "<input type='hidden' id='HSCodeLaw' value='" + html + "' /></fieldset>";
				}

				var Temp = $("#TBClearanceDate").val();
				$("#TBClearanceDate").datepicker();
				$("#TBClearanceDate").datepicker("option", "dateFormat", "yymmdd");
				$("#TBClearanceDate").val(Temp);
			}, function (result) { alert("ERROR : " + result); });
		}
		function BLSplitView(count) {
			document.getElementById("spBLSplit" + count).innerHTML = "<input type=\"text\" id=\"TBNewBLNo[" + count + "]\" style=\"width:120px;\" /><input type=\"button\" value=\"OK\" onclick=\"BLSplit('" + CommercialDocumentHeadPk + "', '" + RequestFormPk[count] + "', '"+count+"');\" />";
		}
		function BLSplit(CommercialDocuPk, RequestFormPk, count) {
			Admin.BLSplit(CommercialDocumentHeadPk, RequestFormPk, document.getElementById("TBNewBLNo[" + count + "]").value, function (result) {
				alert("완료");
				location.reload();
			}, function (result) { alert("ERROR : " + result); });
		}
		function BLNoModify() {
			if (document.getElementById("CDBLNo").value == document.getElementById("HBLNo").value) {
				alert("기존BL번호와 새 BL이 같습니다. ");
				return false;
			}
			Admin.ModifyBLNo(CommercialDocumentHeadPk, document.getElementById("CDBLNo").value, function (result) {
				if (result == "") {
					alert("수정완료");
					location.reload();
				}
				else {
					if (RequestFormPk.length > 1) {
						alert("한개의 접수증일때만 합칠수 있습니다.");
					}
					else {
						if (confirm("이미 사용중인 BL번호입니다. 합치시겠습니까?")) {
							Admin.UnionBL(CommercialDocumentHeadPk, result, RequestFormPk[0], function (result) {
								location.href = "./CheckDescription.aspx?S=" + result;
							}, function (result) { alert("ERROR : " + result); });
						}
					}

				}
			}, function (result) { alert("ERROR : " + result); });
		}

		function SetItemCodeNew(RequestFormCount, itemgruopCount) {
			document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][ItemCode]").value = "";
			document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][Guanse]").disabled = "";
			document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][Bugase]").disabled = "";
		}
		function ShowItemHistory(RequestFormCount, itemgruopCount) {
			var DialogResult = new Array();
			DialogResult[0] = RequestFormCount;
			DialogResult[1] = itemgruopCount;
			DialogResult[2] = document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][ItemName]").value;
			DialogResult[3] = document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][Matarial]").value;
			var retVal = window.showModalDialog('./Dialog/CommercialItemHistory.aspx?S=' + CONSIGNEEPK, DialogResult, 'dialogWidth=1250px;dialogHeight=700px;resizable=0;status=0;scroll=1;help=0;');
			try {
				if (retVal == "N") {
					ShowItemHistory(RequestFormCount, itemgruopCount);
				}
				else {

					var Each = retVal.split("#@!");
					
					document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][ItemCode]").value = Each[0];
					document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][ItemNameByE]").value = Each[1];
					document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][MatarialByE]").value = Each[2];
					document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][Guanse]").value = Each[3];
					if (Each[3] != "") {
						document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][Guanse]").disabled = "disabled";
					}
					document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][Bugase]").value = Each[4];
					if (Each[4] != "") {
						document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][Bugase]").disabled = "disabled";
					}


					//0result
					//1TBHSCode
					//2TBDescription
					//3TBTradingDescription
					//4TBStandardModel
					//5TBMaterial
					//6TBGaunse
					//7TBBugase
					//8E1Checked

					//var Each = retVal.split("#@!");
					//document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][ItemCode]").value = Each[0];
					//document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][ItemNameByE]").value = Each[2];
					//document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][MatarialByE]").value = Each[6];
					//document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][Guanse]").value = Each[7];
					//if (Each[7] != "") {
					//	document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][Guanse]").disabled = "disabled";
					//}
					//document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][Bugase]").value = Each[8];
					//if (Each[8] != "") {
					//	document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][Bugase]").disabled = "disabled";
					//}
					var isFound = false;
					for (var i = 0; i < EachRequestItemCount.length; i++) {
						for (var j = 0; j < EachRequestItemCount[i]; j++) {

							if ($("#Item\\[" + i + "\\]\\[" + j + "\\]\\[HsCcode\\]").val() != "") {
								if ($("#Item\\[" + RequestFormCount + "\\]\\[" + itemgruopCount + "\\]\\[HsCcode\\]").val() == $("#Item\\[" + i + "\\]\\[" + j + "\\]\\[HsCcode\\]").val()) {
									document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][RAN]").value = document.getElementById("Item[" + i + "][" + j + "][RAN]").value;
									isFound = true;
									break;
								}
							}

							if (document.getElementById("Item[" + i + "][" + j + "][RAN]").value != "" && document.getElementById("Item[" + i + "][" + j + "][ItemCode]").value == Each[0]) {
								document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][RAN]").value = document.getElementById("Item[" + i + "][" + j + "][RAN]").value;
								isFound = true;
								break;
							}
						}
						if (isFound) {
							break;
						}
					}
					if (!isFound) {
						RAN++;
						document.getElementById("Item[" + RequestFormCount + "][" + itemgruopCount + "][RAN]").value = RAN;
					}
				}
			}
			catch (err) { return false; }
		}
		function ResultItemHistory_A() {
			var Each = form1.CommercialItemHistory_A.value.split("#@!");
			var H_RequestFormCount = document.getElementById("H_RequestFormCount").value;
			var H_itemgruopCount = document.getElementById("H_itemgruopCount").value;

			document.getElementById("Item[" + H_RequestFormCount + "][" + H_itemgruopCount + "][ItemCode]").value = Each[0];
			document.getElementById("Item[" + H_RequestFormCount + "][" + H_itemgruopCount + "][HsCcode]").value = Each[5];
			//document.getElementById("Item[" + H_RequestFormCount + "][" + H_itemgruopCount + "][ItemNameByE]").value = Each[1];
			document.getElementById("Item[" + H_RequestFormCount + "][" + H_itemgruopCount + "][MatarialByE]").value = Each[2];
			document.getElementById("Item[" + H_RequestFormCount + "][" + H_itemgruopCount + "][Guanse]").value = Each[3];
			if (Each[3] != "") {
				document.getElementById("Item[" + H_RequestFormCount + "][" + H_itemgruopCount + "][Guanse]").disabled = "disabled";
			}
			document.getElementById("Item[" + H_RequestFormCount + "][" + H_itemgruopCount + "][Bugase]").value = Each[4];
			if (Each[4] != "") {
				document.getElementById("Item[" + H_RequestFormCount + "][" + H_itemgruopCount + "][Bugase]").disabled = "disabled";
			}
			var isFound = false;
			for (var i = 0; i < EachRequestItemCount.length; i++) {
				for (var j = 0; j < EachRequestItemCount[i]; j++) {
					if (document.getElementById("Item[" + i + "][" + j + "][RAN]").value != "" && document.getElementById("Item[" + i + "][" + j + "][ItemCode]").value == Each[0]) {
						document.getElementById("Item[" + H_RequestFormCount + "][" + H_itemgruopCount + "][RAN]").value = document.getElementById("Item[" + i + "][" + j + "][RAN]").value;
						isFound = true;
						break;
					}
				}
				if (isFound) {
					break;
				}
			}
			if (!isFound) {
				RAN++;
				document.getElementById("Item[" + H_RequestFormCount + "][" + H_itemgruopCount + "][RAN]").value = RAN;
			}
		}
		function ShowItemHistory_A(RequestFormCount, itemgruopCount) {
			window.open('./Dialog/CommercialItemHistory_A.aspx?S=' + CONSIGNEEPK, 'Width=1250px;Height=700px;resizable=0;status=0;scroll=1;help=0;');
			document.getElementById("H_RequestFormCount").value = RequestFormCount;
			document.getElementById("H_itemgruopCount").value = itemgruopCount;
		}
		function ModalOpenSetSettlementWithCustoms() {
			window.open('../Admin/Dialog/SettlementWithCustoms.aspx?BLNo=' + $("#HBLNo").val() + '&AccountId=' + form1.HAccountID.value, '', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=400px,height=400px, left=500,top=300');
		}
		function goto_hscode() {
			location.href = "../CustomClearance/HSCode_Tariff.aspx?S=" + CONSIGNEEPK;
		}
		function SelectCompanyInDocument(SorC) {
			//var DialogResult = new Array();
			DialogResult1 = "Admin";
			DialogResult2 = SorC;
			DialogResult3 = form1.HBranchPk.value;
			DialogResult4 = form1.HAccountID.value;
			var CompanyPk;
			if (SorC == "S") {
				CompanyPk = SHIPPERPK; 
			}
			else {
				CompanyPk = CONSIGNEEPK;
			} 
			//var retVal = window.showModalDialog('../Request/Dialog/ShipperNameSelection.aspx?S=' + CompanyPk, DialogResult, 'dialogWidth=800px;dialogHeight=400px;resizable=0;status=0;scroll=1;help=0;');
			window.open('../Request/Dialog/ShipperNameSelection.aspx?S=' + CompanyPk + '&D1=' + DialogResult1 + '&D2=' + DialogResult2 + '&D3=' + DialogResult3 + '&D4=' + DialogResult4 + "&Shipper=" + SHIPPERPK + "&Consignee=" + CONSIGNEEPK, '', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=800px,height=400px');
			//try {
			//	if (retVal == 'D') { SelectCompanyInDocument(SorC); }
			//	else if (SorC == "S") {
			//		var ReturnArray = retVal.split('##');
			//		document.getElementById("CDShipperNamePk").value = ReturnArray[0];
			//		document.getElementById("CDShipperName").value = ReturnArray[1];
			//		document.getElementById("CDShipperAddress").value = ReturnArray[2];
			//		if (ReturnArray[4] != undefined) {
			//			STAMPIMG = "/3/" + ReturnArray[0] + "_" + ReturnArray[3] + "_" + ReturnArray[4];
			//		}
			//		else {
			//			STAMPIMG = "";
			//		}
			//	}
			//	else {
			//		var ReturnArray = retVal.split('##');
			//		document.getElementById("CDConsigneeName").value = ReturnArray[1];
			//		document.getElementById("CDConsigneeAddress").value = ReturnArray[2];
			//	}
			//}
			//catch (err) { return false; }
		}
		function SelectCompanyInDocumentOpen(retVal, SorC) {
		    try {
		        if (retVal == 'D') { SelectCompanyInDocument(SorC); }
		        else if (SorC == "S") {
		            var ReturnArray = retVal.split('##');
		            document.getElementById("CDShipperNamePk").value = ReturnArray[0];
		            document.getElementById("CDShipperName").value = ReturnArray[1];
		            document.getElementById("CDShipperAddress").value = ReturnArray[2];
		            if (ReturnArray[4] != undefined) {
		                STAMPIMG = "/3/" + ReturnArray[0] + "_" + ReturnArray[3] + "_" + ReturnArray[4];
		            }
		            else {
		                STAMPIMG = "";
		            }
		        }
		        else {
		        	var ReturnArray = retVal.split('##');
		        	document.getElementById("CDConsigneeNamePk").value = ReturnArray[0];
		            document.getElementById("CDConsigneeName").value = ReturnArray[1];
		            document.getElementById("CDConsigneeAddress").value = ReturnArray[2];
		        }
		    }
		    catch (err) { return false; }
		}
		

		function SetCalc() {
		
			//alert(ITEMMONETARYUNIT);
		   //public String CalcTarriffKOR(string ClearanceDate, string MonetaryUnitFreightCharge, string FreightCharge, string MonetaryUnitItemPrice, string ItemPriceSum, string ItemGuanseRate, string ItemBugaseRate)
		   
		      var SUMitemprice = "";
		      var SUMitemGuanseRate = "";
		      var SUMitemBugaseRate = "";
		      for (var i = 0; i < EachRequestItemCount.length; i++) {
		         for (var j = 0; j < EachRequestItemCount[i]; j++) {
		            if (document.getElementById("Item[" + i + "][" + j + "][Price]").value == "") {
		               alert("물품금액이 없습니다"); return false;
		            }
		            if (document.getElementById("Item[" + i + "][" + j + "][Guanse]").value == "") {
		               alert("관세가 없습니다."); return false;
		            }
		            if (document.getElementById("Item[" + i + "][" + j + "][Bugase]").value == "") {
		               alert("부가세가 없습니다."); return false;
		            }
		            SUMitemprice += document.getElementById("Item[" + i + "][" + j + "][Price]").value + "#@!";
		            SUMitemGuanseRate += document.getElementById("Item[" + i + "][" + j + "][Guanse]").value + "#@!";
		            SUMitemBugaseRate += document.getElementById("Item[" + i + "][" + j + "][Bugase]").value + "#@!";
		         }
		      }
		      SUMitemprice = SUMitemprice.substr(0, SUMitemprice.length - 3).replace(/,/g, "");
		      SUMitemGuanseRate = SUMitemGuanseRate.substr(0, SUMitemGuanseRate.length - 3);
		      SUMitemBugaseRate = SUMitemBugaseRate.substr(0, SUMitemBugaseRate.length - 3);
		      var freightcharge = "0";
		      var freightchargeMonetary = "";
		      if (document.getElementById("FORnCNFValue20").value != "-1") {
		         freightcharge = document.getElementById("FORnCNFValue20").value;
		         freightchargeMonetary = "20";
		      } else {
		         freightcharge = document.getElementById("FORnCNFValue").value;
		         freightchargeMonetary = document.getElementById("FOBNCNFMonetaryUnit").value;
		      }
		      Admin.ExchangeRateHistory_nullCheck("2", document.getElementById("TBClearanceDate").value, function (result) {
		         if (result < "2") {
		            alert("환율정보를 확인해주세요");
		         } else {
		            Admin.CalcTarriffKOR(document.getElementById("TBClearanceDate").value, freightchargeMonetary, freightcharge, ITEMMONETARYUNIT, SUMitemprice, SUMitemGuanseRate, SUMitemBugaseRate, function (result) {
		               form1.TotalGuanse.value = result[0];
		               form1.TotalBugase.value = result[1];
		               if (result[0] == "0" && result[1] == "0") {
		                  alert("세금합계가 만원이하입니다");
		               }
		            }, function (result) { alert("ERROR : " + result); });
		         }
		      }, function (result) { alert("ERROR : " + result); });
		}
		function SetSave() {
			var totalguanse = form1.TotalGuanse.value;
			var totalbugase = form1.TotalBugase.value;
			var clearancefee = form1.ClearanceFee.value;

			if (totalguanse == "") { totalguanse = "0"; }
			if (totalbugase == "") { totalbugase = "0"; }
			if (clearancefee == "") { clearancefee = "0"; }

			var TariffSUM = "관세#@!20#@!" + totalguanse + "%!$@#부가세#@!20#@!" + totalbugase + "%!$@#관세사비#@!20#@!" + clearancefee;
			var ItemSUM = "";
			for (var i = 0; i < EachRequestItemCount.length; i++) {
				for (var j = 0; j < EachRequestItemCount[i]; j++) {
					var tempguanse = ""; if (document.getElementById("Item[" + i + "][" + j + "][Guanse]").disabled == "") { tempguanse = document.getElementById("Item[" + i + "][" + j + "][Guanse]").value; }
					var tempbugase = ""; if (document.getElementById("Item[" + i + "][" + j + "][Bugase]").disabled == "") { tempbugase = document.getElementById("Item[" + i + "][" + j + "][Bugase]").value; }
					var co = "";
					if (ISCO == true && document.getElementById("Item[" + i + "][" + j + "][IsCo]").checked == true) {
						co = document.getElementById("Item[" + i + "][" + j + "][HSCODE]").value;
					}
					if (document.getElementById("Item[" + i + "][" + j + "][ItemCode]").value != "") {
						ItemSUM += document.getElementById("Item[" + i + "][" + j + "][ItemCode]").value + "#@!" +
										document.getElementById("Item[" + i + "][" + j + "][ItemNameByE]").value + "#@!" +
										document.getElementById("Item[" + i + "][" + j + "][MatarialByE]").value + "#@!" +
										tempguanse + "#@!" +
										tempbugase + "#@!" +
										document.getElementById("Item[" + i + "][" + j + "][RequestFormItemsPk]").value + "#@!" +
										document.getElementById("Item[" + i + "][" + j + "][NWeight]").value + "#@!" +
										document.getElementById("Item[" + i + "][" + j + "][RAN]").value + "#@!" +
										co + "%!$@#";
					}
				}
			}
			var COSUM = "N";
			//			if (ISCO) {
			//				COSUM = document.getElementById("COIssueCompanyName").value + "#@!" +
			//								document.getElementById("COIssueCompanyAddress").value + "#@!" +
			//								document.getElementById("COIssueBranchPk").value + "#@!" +
			//								document.getElementById("COTotalCount").value + "#@!" +
			//								document.getElementById("COIssueDate").value;
			//			}
			Admin.SetTarriffKor(CommercialDocumentHeadPk, RequestFormPk[0], CONSIGNEEPK, document.getElementById("CDInvoiceNo").value, document.getElementById("CDShipperName").value, document.getElementById("CDShipperAddress").value, document.getElementById("CDConsigneeName").value, document.getElementById("CDConsigneeAddress").value, document.getElementById("CDPaymentTerms").value, document.getElementById("FORnCNF").value + "#@!" + document.getElementById("FOBNCNFMonetaryUnit").value + "#@!" + document.getElementById("FORnCNFValue").value, document.getElementById("CDOtherRefferance").value, document.getElementById("CDNortifyPartyName").value, document.getElementById("CDNortifyPartyAddress").value, "1", document.getElementById("TBClearanceDate").value, STAMPIMG, ItemSUM, TariffSUM, form1.HAccountID.value, document.getElementById("CDPortOfLanding").value, document.getElementById("CDFinalDestination").value, document.getElementById("CDCarrier").value, document.getElementById("CDSailingOn").value, document.getElementById("CDVoyageNo").value, document.getElementById("CDVoyage").value, document.getElementById("CDContainerNo").value, document.getElementById("CDSealNo").value, document.getElementById("CDContainerSize").value, COSUM, document.getElementById("CDShipperNamePk").value, document.getElementById("CDConsigneeNamePk").value, $("#MarksNNo").val(), $("#LCNo").val(), function (result) {
				if (result == "1") {
					alert("완료");
					location.reload();
					return false;
				}
				form1.DEBUG.value = result;
			}, function (result) { alert("ERROR : " + result); });
		}

		function Print(value) {
			switch (value) {
				case "BL": location.href = "../CustomClearance/CommercialDocu_HouseBL.aspx?G=Print&S=" + CommercialDocumentHeadPk; break;
				case "Invoice": location.href = "../CustomClearance/CommercialDocu_Invoice.aspx?G=Print&S=" + CommercialDocumentHeadPk; break;
				case "Packing": location.href = "../CustomClearance/CommercialDocu_PackingList.aspx?G=Print&S=" + CommercialDocumentHeadPk; break;
				case "ConsigneeChargeFI":
					window.open('../Request/FreightChargeView.aspx?S=' + RequestFormPk[0] + "&G=C&T=F", '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=800px;');
					break;
				case "StampBL": location.href = "../CustomClearance/CommercialDocu_HouseBL.aspx?G=Stamp&S=" + CommercialDocumentHeadPk; break;
				case "StampInvoice": location.href = "../CustomClearance/CommercialDocu_Invoice.aspx?G=Stamp&S=" + CommercialDocumentHeadPk; break;
				case "StampPacking": location.href = "../CustomClearance/CommercialDocu_PackingList.aspx?G=Stamp&S=" + CommercialDocumentHeadPk; break;
				case "StampConsigneeChargeFI":
					window.open('../Request/FreightChargeView.aspx?S=' + RequestFormPk[0] + "&G=C&T=F&Stamp=Yes", '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=800px;');
					break;
			}
		}
		function CheckRAN(RequestCount, ItemCount) {
			if (window.event.keyCode == 13) {
				var isFound = false;
				for (var i = 0; i < EachRequestItemCount.length; i++) {
					for (var j = 0; j < EachRequestItemCount[i]; j++) {
						if (document.getElementById("Item[" + i + "][" + j + "][RAN]").value == document.getElementById("Item[" + RequestCount + "][" + ItemCount + "][RAN]").value) {
							document.getElementById("Item[" + RequestCount + "][" + ItemCount + "][ItemCode]").value = document.getElementById("Item[" + i + "][" + j + "][ItemCode]").value;
							document.getElementById("Item[" + RequestCount + "][" + ItemCount + "][ItemNameByE]").value = document.getElementById("Item[" + i + "][" + j + "][ItemNameByE]").value;
							document.getElementById("Item[" + RequestCount + "][" + ItemCount + "][MatarialByE]").value = document.getElementById("Item[" + i + "][" + j + "][MatarialByE]").value;
							document.getElementById("Item[" + RequestCount + "][" + ItemCount + "][Guanse]").value = document.getElementById("Item[" + i + "][" + j + "][Guanse]").value;
							if (document.getElementById("Item[" + RequestCount + "][" + ItemCount + "][Guanse]").value != "") {
								document.getElementById("Item[" + RequestCount + "][" + ItemCount + "][Guanse]").disabled = "disabled";
							}
							document.getElementById("Item[" + RequestCount + "][" + ItemCount + "][Bugase]").value = document.getElementById("Item[" + i + "][" + j + "][Bugase]").value;
							if (document.getElementById("Item[" + RequestCount + "][" + ItemCount + "][Bugase]").value != "") {
								document.getElementById("Item[" + RequestCount + "][" + ItemCount + "][Bugase]").disabled = "disabled";
							}
							isFound = true;
							break;
						}
					}
					if (isFound) { break; }
				}
				if (isFound) {
					if (ItemCount < EachRequestItemCount[RequestCount] - 1) {
						ItemCount++;
						document.getElementById("Item[" + RequestCount + "][" + ItemCount + "][RAN]").focus();
						document.getElementById("Item[" + RequestCount + "][" + ItemCount + "][RAN]").select();
					}
					else if (RequestCount < EachRequestItemCount.length - 1) {
						document.getElementById("Item[" + (RequestCount + 1) + "][0][RAN]").focus();
						document.getElementById("Item[" + (RequestCount + 1) + "][0][RAN]").select();
					}
				}
				else {
					alert("기존에 지정된 항목이 없습니다. ");
				}
			}
		}
		function Send(which) {
			SetSave();
			Admin.SetDocumentStepCLTo(CommercialDocumentHeadPk, which, form1.HAccountID.value, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function SendSungSim(which) {
			SetSave();
			Admin.SetDocumentStepCLTo_SungSim(CommercialDocumentHeadPk, which, form1.HAccountID.value, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}

		function InsertContents(CDPk, AccountID) {
			var data = {
				Table_Name: "RequestForm",
				Table_Pk: RequestFormPkComment,
				Category: "Request",
				Contents: form1.TBContents.value,
				Account_Id: AccountID
			}
			$.ajax({
				type: "POST",
				url: "/Process/HistoryP.asmx/Set_Comment",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});

			var data = {
				Table_Name: "CommercialDocument ",
				Table_Pk: CDPk,
				Category: "BL",
				Contents: form1.TBContents.value,
				Account_Id: AccountID
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

		function SetClearance_Ready() {
			Admin.SetClearance_Ready(form1.HBLNo.value, form1.HAccountID.value, function (result) {
				if (result == "0") {
					alert("이미 레디코리아에 등록 되어 있는 BLNo 입니다");
					location.reload();
				} else if (result == "2") {
					alert("통관예정일이 저장되어 있지 않습니다. 저장 하시고 진행해주세요");
					location.reload();
				} else if (result == "3") {
					alert("환율이 없습니다");
					location.reload();
				} else {
					var MSG = "";
					if (result.substr(0, 1) == "0") {
						MSG = "수출자Code를 찾지못했습니다. ";
					}
					if (result.substr(1, 1) == "0") {
						if (MSG != "") {
							MSG += "\r\n";
						}
						MSG = "수입자Code를 찾지못했습니다. ";
					}
					if (MSG != "") {
						alert(MSG);
					}
					alert("OK");
					Send("6");
					//관세사 전송);
					location.reload();
				}
			}, function (result) { return false; });
		}
		function DeleteComment(pk) {
			if (confirm("삭제하시겠습니까?")) {
				Admin.DeleteforRequestCommentsNCustom("2", pk, function (result) {
					if (result == "1") {
						alert("COMMENT DELETED");
						location.reload();
					} else {
						alert(result);
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function SetOrderRAN() {
			Admin.SetOrder_RAN(CommercialDocumentHeadPk, function (result) {
				var arrResult = result.split("@");
				for (var K = 0; K < arrResult.length; K++) {
					var arrEach = arrResult[K].split("!");
					for (var i = 0; i < EachRequestItemCount.length; i++) {
						var isFound = false;
						for (var j = 0; j < EachRequestItemCount[i]; j++) {
							if (document.getElementById("Item[" + i + "][" + j + "][RequestFormItemsPk]").value == arrEach[0]) {
								document.getElementById("Item[" + i + "][" + j + "][RAN]").value = arrEach[1];
								isFound = true;
								break;
							}
						}
						if (isFound) {
							break;
						}
					}
				}
				alert("SUCCESS");
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body style="background-color:#E4E4E4; width:1100px; margin:0 auto; padding-top:10px;"  >
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
	<uc2:Loged ID="Loged1" runat="server" Visible="false" />
	<uc1:LogedWithoutRecentRequest11 ID="LogedWithoutRecentRequest111" runat="server" />
	<div style="background-color:White; width:1050px; height:100%; padding:25px;">
		<input type="hidden" id="HCommercialDocumentPk" value="<%=CommercialDocumentPk %>" />
		<input type="hidden" id="HGubun" value="<%=MemberInfo[0] %>" />
		<input type="hidden" id="HBranchPk" value="<%=MemberInfo[1] %>" />
		<input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
		<div style="width:1050px; clear:both;"><%=PageDate %></div>
		<div style="clear:both; font-size:1px; ">&nbsp;</div>
		<div id="SpMainContents" style=" padding-left:10px;"></div>

		<div style="float:left; margin-top:20px; margin-left:50px; width:430px;">
			<span id="SPCO"></span>
			<span id="SPLaw_NmData"></span>
			<fieldset>
				<legend><strong>접수증 코멘트</strong></legend>
							<%=List_RequestComment %>
			</fieldset>
<br />
			<%=Comment %>
		</div>
		<div style="float:left; margin-top:10px; margin-left:50px;  ">
			<p><strong>관&nbsp;&nbsp;&nbsp;세</strong> : <input type="text" id="TotalGuanse" /></p>
			<p><strong>부가세</strong> : <input type="text" id="TotalBugase" /></p>
			<p><strong>관세사</strong> : <input type="text" id="ClearanceFee" value="33,000" /></p>
		</div>
		<div style="padding:20px; text-align:right; ">
			<p>
				<input type="button" value="HSCode 미확인" />
				<%=BTN_SetClearance_Ready %>
				<input type="button" value="란 정렬" onclick="SetOrderRAN();" />
			</p>
			<p>
            <%=Onlyilic66BTN %> 
				
				<input type="button" value="뒤로" onclick="history.back();" />
			</p>
			<p><%=InputBTN %></p>
			<p>
				<input type="button" value="Freight Invoice" onclick="Print('ConsigneeChargeFI');" />
				<input type="button" value="House BL" onclick="Print('BL');" />
				<input type="button" value="Invoice" onclick="Print('Invoice');" />
				<input type="button" value="Packing List" onclick="Print('Packing');" />
			</p>
			<p>
				<input type="button" value="Freight Invoice" onclick="Print('StampConsigneeChargeFI');" />
				<input type="button" value="House BL" onclick="Print('StampBL');" />
				<input type="button" value="Invoice" onclick="Print('StampInvoice');" />
				<input type="button" value="Packing List" onclick="Print('StampPacking');" />
			</p>

			<%=AttachedFiles %>
		</div>
		<div style="clear:both; font-size:1px; ">&nbsp;</div>		
		<input type="hidden" id="DEBUG" />
		<input type="hidden" id="CommercialItemHistory_A" name="CommercialItemHistory_A"/>
		<input type="hidden" id="H_RequestFormCount" name="H_RequestFormCount"/>
		<input type="hidden" id="H_itemgruopCount" name="H_itemgruopCount"/>

    </div>
    </form>
</body>
</html>