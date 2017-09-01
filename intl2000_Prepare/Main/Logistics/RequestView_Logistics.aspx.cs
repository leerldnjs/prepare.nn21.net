using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Logistics_RequestView_Logistics : System.Web.UI.Page
{
	protected StringBuilder HtmlItem;
	protected String[] MemberInformation;
	protected StringBuilder HtmlDelivery;
	protected String Gubun;
	private String RequestFormPk;
	private Int32 StepCL;
	private DBConn DB;
	protected String IsConsigneeConfirmedStyle;
	protected String TransportBetweenBranchPk;
	protected String ConsigneePk;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("../Default.aspx");
		}
		try {
			if (Request["Language"].Length == 2) {
				Session["Language"] = Request["Language"];
			}
		} catch (Exception) {
		}
		switch (Session["Language"] + "") {
			case "en":
				Page.UICulture = "en";
				break;
			case "ko":
				Page.UICulture = "ko";
				break;
			case "zh":
				Page.UICulture = "zh-cn";
				break;
		}

		MemberInformation = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		Gubun = Request.Params["g"];
		RequestFormPk = Request.Params["Pk"];

		LoadDeliveryLoad(Request.Params["Pk"], MemberInformation[2]);
		LoadItemTable(RequestFormPk);
		IsConsigneeConfirmedStyle = "style=\" border:2px solid black; background-color:#FFFACD;\"";
	}

	private void LoadItemTable(string RequestFormPk)
	{
		HtmlItem = new StringBuilder();

		HtmlItem.Append("	<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:580px; \" >" +
									"		<tr><td align=\"right\" colspan='8'></td></tr>" +
									"		<tr style=\"height:30px;\" >" +
									"			<td bgcolor=\"#F5F5F5\" height=\"20\" align=\"center\" width=\"45px\" >" + GetGlobalResourceObject("RequestForm", "BoxNo") + "</td>" +
									"			<td bgcolor=\"#F5F5F5\" align=\"center\" >" +
													GetGlobalResourceObject("RequestForm", "Description") + " / " +
													GetGlobalResourceObject("RequestForm", "Label") + " / " +
													GetGlobalResourceObject("RequestForm", "Material") + "</td>" +
									"			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"65\">" + GetGlobalResourceObject("RequestForm", "Count") + "</td>" +
									"			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"50\">" + GetGlobalResourceObject("RequestForm", "UnitCost") + "</td>" +
									"			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"75\">" + GetGlobalResourceObject("RequestForm", "Amount") + "</td>" +
									"			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"50\">" + GetGlobalResourceObject("RequestForm", "PackingCount") + "</td>" +
									"			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"45\">kg</td>" +
									"			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"30\">cbm</td>" +
									"		</tr>");

		DB.SqlCmd.CommandText = "EXEC SP_SelectItemWithRequestFormPk @RequestFormPk = " + RequestFormPk + ";";
		Decimal PriceSum = 0;
		Decimal weightSum = 0;
		Decimal volumeSum = 0;
		int CTSum = 0;
		int ROSum = 0;
		int PASum = 0;
		string monetaryunit = string.Empty;
		bool flagfirst = true;

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			if (flagfirst) {
				monetaryunit = Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "");
				flagfirst = false;
			}
			HtmlItem.Append("<tr style=\"height:25px; \"><td align='center' class='ItemTableIn'>" + RS["MarkNNumber"] + "</td>");

			if (RS["Label"] + "" != "" || RS["Material"] + "" != "") {
				HtmlItem.Append("<td align='left' style='padding-left:5px;' class='ItemTableIn'>" + RS["Description"] + " / " + RS["Label"] + " / " + RS["Material"] + "</td>");
			} else {
				HtmlItem.Append("<td align='left' style='padding-left:5px;' class='ItemTableIn'>" + RS["Description"] + "</td>");
			}

			if (RS["Quantity"] + "" != "") {
				switch (RS["QuantityUnit"] + "") {
					case "40":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " PCS" + "</td>");
						break;
					case "41":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " PRS" + "</td>");
						break;
					case "42":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " SET" + "</td>");
						break;
					case "43":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " S/F" + "</td>");
						break;
					case "44":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " YDS" + "</td>");
						break;
					case "45":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " M" + "</td>");
						break;
					case "46":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " KG" + "</td>");
						break;
					case "47":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " DZ" + "</td>");
						break;
					case "48":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " L" + "</td>");
						break;
					case "49":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " BOX" + "</td>");
						break;
					case "50":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " SQM" + "</td>");
						break;
					default:
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " " + "</td>");
						break;
				}
			} else {
				HtmlItem.Append("<td align='center' class='ItemTableIn'>&nbsp;</td>");
			}

			HtmlItem.Append("<td align='center' class='ItemTableIn'>" + monetaryunit + " " + Common.NumberFormat(RS["UnitPrice"] + "") + "</td>");


			HtmlItem.Append("<td align='center' class='ItemTableIn'>" + (RS["Amount"] + "" == "" ? "&nbsp;" : monetaryunit + " " + Common.NumberFormat(RS["Amount"] + "")) + "</td>");

			if (RS["PackedCount"] + "" != "") {
				switch (RS["PackingUnit"] + "") {
					case "15":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + RS["PackedCount"] + " CT" + "</td>");
						CTSum += int.Parse(RS["PackedCount"] + "");
						break;
					case "16":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + RS["PackedCount"] + " RO" + "</td>");
						ROSum += int.Parse(RS["PackedCount"] + "");
						break;
					case "17":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + RS["PackedCount"] + " PA" + "</td>");
						PASum += int.Parse(RS["PackedCount"] + "");
						break;
					case "18":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + RS["PackedCount"] + " M2" + "</td>");
						PASum += int.Parse(RS["PackedCount"] + "");
						break;
				}
			} else {
				HtmlItem.Append("<td align='center' class='ItemTableIn'>&nbsp;</td>");
			}

			HtmlItem.Append("<td align='center' class='ItemTableIn'>" + (RS["GrossWeight"] + "" == "" ? "&nbsp;" : Common.NumberFormat(RS["GrossWeight"] + "")) + "</td>");
			HtmlItem.Append("<td align='center' class='ItemTableIn'>" + (RS["Volume"] + "" == "" ? "&nbsp;" : Common.NumberFormat(RS["Volume"] + "")) + "</td></tr>");

			if (RS["Amount"] + "" != "") {
				PriceSum += decimal.Parse(RS["Amount"] + "");
			}
			if (RS["GrossWeight"] + "" != "") {
				weightSum += decimal.Parse(RS["GrossWeight"] + "");
			}
			if (RS["Volume"] + "" != "") {
				volumeSum += decimal.Parse(RS["Volume"] + "");
			}
			ConsigneePk = RS["ConsigneePk"] + "";
		}
		RS.Dispose();
		DB.DBCon.Close();

		string tempCT = CTSum != 0 ? CTSum + "CT " : string.Empty;
		string tempRO = ROSum != 0 ? ROSum + "RO " : string.Empty;
		string tempPA = PASum != 0 ? PASum + "PA " : string.Empty;

		if (StepCL < 55 || StepCL == 56) {
			HtmlItem.Append("<tr><td bgcolor=\"#F5F5F5\" height=\"30\" align=\"right\" colspan='8'>" + GetGlobalResourceObject("qjsdur", "chdqkrtm") + " : <strong>" + tempCT + tempRO + tempPA + "</strong> " + GetGlobalResourceObject("qjsdur", "chdwndfid") + " : <strong>" + Common.NumberFormat(weightSum + "") + "Kg</strong> " + GetGlobalResourceObject("qjsdur", "chdcpwjr") + " : <strong>" + Common.NumberFormat(volumeSum + "") + "CBM</strong></td></tr></table>");
		} else {
			HtmlItem.Append("<tr><td bgcolor=\"#F5F5F5\" height=\"30\" align=\"right\" colspan='8'>");
		}
	}
	private void LoadDeliveryLoad(string RequestFormPk,string AccountID)
	{
		HtmlDelivery = new StringBuilder();
		string DeliveryTable = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:580px;\">" +
			"<tr><td style=\"background-color:#E8E8E8;  width:150px; text-align:center; \">Arrival Time</td>" +
			"		<td style=\"background-color:#E8E8E8;  width:75px; text-align:center;\">Box Count</td>" +
			"		<td style=\"background-color:#E8E8E8;  width:95px; text-align:center;\">Type</td>" +
			"		<td style=\"background-color:#E8E8E8;  \">Deliverer</td>" +
			"		<td class='THead1' style=\"width:50px; background-color:#E8E8E8; \" rowspan=\"2\" >{0}</td>" +
			"		<td class='THead1' style=\"width:20px; background-color:#E8E8E8; \" rowspan=\"2\" >P</td></tr>" +
			"<tr><td colspan=\"2\" class='THead1'  style=\"text-align:center;\">Consignee Staff</td><td class='THead1'  colspan=\"2\" style=\"text-align:center;\">Address</td></tr>{1}</table>";
		string Row = "	<tr><td style=\"text-align:center;\" >{1}</td>" +
								"	<td style=\"text-align:center;\">{2} / {3}</td>" +
								"	<td style=\"text-align:center;\">{4}</td>" +
								"	<td style=\"text-align:center;\">{5}</td>" +
								"	<td style=\"text-align:center;\" rowspan=\"2\" class=\"TBody1\" >{8}</td>" +
								"	<td style=\"text-align:center;\" rowspan=\"2\" class=\"TBody1\" ><input type=\"button\" value=\"P\" onclick=\"DeliveryPrint('{9}')\" style=\"height:40px; width:15px; padding:0px;  \" /></td></tr>" +
							"	<tr><td class=\"TBody1\" colspan=\"2\" style=\"text-align:center;\">{6}</td>" +
								"	<td class=\"TBody1\" colspan=\"2\">{7}</td></tr>";
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT 
	OBSO.OurBranchStorageOutPk, OBSO.BoxCount, CONVERT(CHAR(8), OBSO.StockedDate, 10) AS StockedDate,  OBSO.StatusCL 
	, OBSC.StorageName
	, TBC.TransportBetweenCompanyPk, TBC.Type, TBC.Title, TBC.DriverName, TBC.DriverTEL, 
	TBC.FromDate, TBC.ToDate, TBC.WarehouseInfo, TBC.WarehouseMobile, TBC.PackedCount, 
	TBC.PackingUnit, TBC.Weight, TBC.Volume, TBC.DepositWhere, TBC.Price, TBC.Memo ,OBSO.TransportBetweenBranchPk
FROM OurBranchStorageOut AS OBSO 
	left join OurBranchStorageCode AS OBSC ON OBSO.StorageCode=OBSC.OurBranchStoragePk 
	left join TransportBC AS TBC ON OBSO.TransportBetweenCompanyPk=TBC.TransportBetweenCompanyPk 	
WHERE OBSO.RequestFormPk=" + RequestFormPk + " ORDER BY OBSO.StatusCL DESC ;";
		DB.DBCon.Open();
		StringBuilder TempRow = new StringBuilder();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		bool CheckIsReaded = false;

		while (RS.Read()) {
			if (!CheckIsReaded) {
				CheckIsReaded = true;
			}


			string[] RowValue = new string[10];
			RowValue[0] = RS["OurBranchStorageOutPk"] + "";
			RowValue[1] = RS["StorageName"] + "" == "" ? "미도착" : "<strong>" + RS["StorageName"] + "</strong> " + (RS["StockedDate"] + "" == "" ? "" : (RS["StockedDate"] + "").Substring(0, 5));
			RowValue[2] = RS["BoxCount"] + "";
			if (RS["Title"] + "" == "") {
				RowValue[4] = RS["Type"] + "";
			} else {
				RowValue[4] = RS["Title"] + "";
			}
			RowValue[5] = RS["DriverTEL"] + "" == "" ? RS["DriverName"] + "" : RS["DriverName"] + " (" + RS["DriverTEL"] + ")";

			string[] tempwarehouse = RS["WarehouseInfo"] + "" == "" ? new string[] { "", "", "" } : (RS["WarehouseInfo"] + "").Split(Common.Splite22, StringSplitOptions.None);
			RowValue[6] = RS["WarehouseMobile"] + "" == "" ? tempwarehouse[2] : tempwarehouse[2] + " (" + RS["WarehouseMobile"] + ")";
			if (RS["StatusCL"] + "" == "0" || RS["StatusCL"] + "" == "4") {
				RowValue[7] = "<input type=\"button\" value=\"출고지 / 배송기사 지정\" onclick=\"DeliverySet('" + RS["OurBranchStorageOutPk"] + "')\" />";
				RowValue[8] = "미지정";
			} else {
				RowValue[7] = "<span onclick=\"DeliverySet('" + RS["OurBranchStorageOutPk"] + "')\" style=\"cursor:hand;\"  >" + tempwarehouse[0] + "</span>";
				RowValue[8] = RS["StatusCL"] + "" == "6" ?
					(RS["FromDate"] + "" == "" ? "" : (RS["FromDate"] + "").Substring(4, 4)) + "<br /><span style=\"color:green;\">출고완료</span>" :
					"<input type=\"button\" value=\"수정\" onclick=\"DeliverySet('" + RS["OurBranchStorageOutPk"] + "')\" /><br /><input type=\"button\" value=\"취소\" onclick=\"DeliveryCancel('" + RS["OurBranchStorageOutPk"] + "','" + RequestFormPk + "','" + AccountID + "')\"  />";
			}
			RowValue[9] = RS["TransportBetweenCompanyPk"] + "";

			TempRow.Append(String.Format(Row, RowValue));
		}
		RS.Dispose();
		DB.DBCon.Close();

		if (CheckIsReaded) {
			HtmlDelivery.Append(String.Format(DeliveryTable, "&nbsp;", TempRow));
		} else {
			HtmlDelivery.Append(String.Format(DeliveryTable, "<input type=\"button\" value=\"Add\" onclick=\"DeliverySet('N');\" />", ""));
		}
	}
}