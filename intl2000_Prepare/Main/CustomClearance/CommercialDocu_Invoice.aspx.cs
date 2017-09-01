using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Components;
using System.Data.SqlClient;

public partial class CustomClearance_CommercialDocu_Invoice : System.Web.UI.Page
{
	protected String Shipper;
	protected String ShipperAddress;
	protected String Consignee;
	protected String ConsigneeAddress;
	protected String NotifyParty;
	protected String NotifyPartyAddress;
	protected String PortOfLanding;
	protected String Carrier;
	protected String FinalDestination;
	protected String SailingOnOrAbout;
	protected String PaymentTerms;
	protected StringBuilder ItemList;
	protected String TotalQuantity;
	protected String TotalAmount;
	protected String[] MemberInfo;
	protected String InvoiceNo;
	protected String Buyer;
	protected String Memo;
	protected String StampImg;
	protected String FOBNCNF;
    protected String aabb;
	protected String gubun;
	//protected String CommercialDocumentPk;
    protected void Page_Load(object sender, EventArgs e)
    {
		try { MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None); }
		catch (Exception) { Response.Redirect("../Default.aspx"); }
		//자기문서아니면 안보이게 만들기!!!!!!

		gubun = Request.Params["G"] + "" == "" ? "View" : Request.Params["G"] + "";
		if (Request.Params["S"] + "" != "")
		{
			LoadCommercialInvoice(Request.Params["S"]);
			LoadCommercialInvoiceItems(Request.Params["S"]);
		}
    }
	private void LoadCommercialInvoice(string CommercialDocumentHeadPk)
	{
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "	SELECT	"+
			"	[BLNo], [InvoiceNo], [Shipper], [ShipperAddress], [Consignee], [ConsigneeAddress], [NotifyParty], [NotifyPartyAddress], [PortOfLoading], [FinalDestination], [Carrier], [SailingOn], [PaymentTerms] " +
			"	, [OtherReferences], [StampImg], [FOBorCNF], [Registerd], [ClearanceDate], [StepCL] " +
		"	FROM CommercialDocument " +
		"	WHERE CommercialDocumentHeadPk=" + CommercialDocumentHeadPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		
		if (RS.Read())
		{
			InvoiceNo = RS["InvoiceNo"] + "";
			Shipper = ""+RS["Shipper"];
			ShipperAddress = ""+RS["ShipperAddress"];
			Consignee = ""+RS["Consignee"];
			ConsigneeAddress = ""+RS["ConsigneeAddress"];
			NotifyParty = ""+RS["NotifyParty"];
			NotifyPartyAddress = ""+RS["NotifyPartyAddress"];
			PortOfLanding = ""+RS["PortOfLoading"];
			FinalDestination = ""+RS["FinalDestination"];
			Carrier = ""+RS["Carrier"];
			SailingOnOrAbout = ""+RS["SailingOn"];
			PaymentTerms = ""+RS["PaymentTerms"];
			if (RS["StampImg"] + "" != "" && (RS["StampImg"] + "").IndexOf(".") > 0)
			{
				StampImg = "<div style=\"margin-top:-125px; margin-left:430px; \"><img alt=\"\" src=\"../UploadedFiles" + RS["StampImg"] + "\" /></div>";
			}
			else
			{
				//StampImg = "<div style=\"margin-top:-120px; margin-left:430px; \"><img alt=\"\" style=\"width:180px; height:120px;  \" src=\"../UploadedFiles/3/GOOYIN.png\" /></div>";
				StampImg = "";
			}

			Memo = ""+RS["OtherReferences"];
			if (RS["FOBorCNF"] + "" == "") {
				FOBNCNF = "";
			} else {
				string[] tempFOBNCNF = (RS["FOBorCNF"] + "").Split(Common.Splite321, StringSplitOptions.None);
				if (tempFOBNCNF[0] == "CNF") {
					FOBNCNF = tempFOBNCNF[0];
				} else {
					FOBNCNF = tempFOBNCNF[0]; //+ " " + (tempFOBNCNF[2] == "" ? "" : Common.GetMonetaryUnit(tempFOBNCNF[1]) + Common.NumberFormat(tempFOBNCNF[2]));
				}
			}
			
		}
		RS.Dispose();
		DB.DBCon.Close();
	}
	private void LoadCommercialInvoiceItems(string CommercialDocumentHeadPk)
	{
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"	
	SELECT RF.MonetaryUnitCL, RFI.MarkNNumber, RFI.Label, RFI.Quantity, RFI.QuantityUnit, RFI.UnitPrice, RFI.Amount, CICK.Description, CICK.Material, Row.C
	FROM CommerdialConnectionWithRequest as CCWR 
		left join RequestForm AS RF ON CCWR.RequestFormPk=RF.RequestFormPk 
		left join RequestFormItems AS RFI ON CCWR.RequestFormPk =RFI.RequestFormPk  
		left join ClearanceItemCodeKOR AS CICK ON RFI.ItemCode=CICK.ItemCode  
		left join (select count(*) AS C, RequestFormPk FROM RequestFormItems GROUP BY RequestFormPk ) AS Row ON CCWR.RequestFormPk =Row.RequestFormPk  
	WHERE CCWR.CommercialDocumentPk=" + CommercialDocumentHeadPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string Quantity;
		string UnitPrice;
		string Amount;
		int RowLength = 0;
		Decimal TempTotalAmount = 0;
		ItemList = new StringBuilder();
		string TempQuantityUnit = string.Empty;
		string TempQuantityUnitCL = string.Empty;
		Decimal[] TempTotalQuantity = new Decimal[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		TotalQuantity = string.Empty;
		string TempMonetaryUnit="";
		bool isFirstRow = true;
		string TRHeight = "";
		while (RS.Read())
		{
			if (isFirstRow)
			{
				int temprowcount=Int32.Parse(RS["C"] + "");
				if (temprowcount > 20 && temprowcount < 30)
				{
					TRHeight = "30px";
				}
				else
				{
					TRHeight = "20px";
				}
				isFirstRow = false;
			}

			if (RowLength == 0) { TempMonetaryUnit = Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + ""); }

			RowLength++;
			if (RS["Quantity"] + "" != "") {
				Quantity = Common.NumberFormat(RS["Quantity"] + "") + " " + Common.GetQuantityUnit(RS["QuantityUnit"] + "");
				if (TempTotalQuantity[(Int32.Parse(RS["QuantityUnit"] + "") - 40)] == 0) {
					TempTotalQuantity[(Int32.Parse(RS["QuantityUnit"] + "") - 40)] = decimal.Parse(RS["Quantity"] + "");
				} else {
					TempTotalQuantity[(Int32.Parse(RS["QuantityUnit"] + "") - 40)] += decimal.Parse(RS["Quantity"] + "");
				}
			} else { Quantity = ""; }
			UnitPrice = RS["UnitPrice"] + "" != "" ?"<span style=\"font-size:8px;\">"+ TempMonetaryUnit + "</span> " + Common.NumberFormat(RS["UnitPrice"] + "") : "";
			if (RS["Amount"] + "" != "")
			{
				Amount = "<span style=\"font-size:8px;\">"+TempMonetaryUnit + "</span> " + Common.NumberFormat(RS["Amount"] + "");
				TempTotalAmount += decimal.Parse(RS["Amount"] + "");
			}
			else { Amount = ""; }
			ItemList.Append("<tr style=\"height:" + TRHeight + ";\"><td class=\"BL\" style=\"text-align:center; font-size:12px; \" >" + RS["MarkNNumber"] + "&nbsp;</td>" +
				"<td style=\"padding-left:20px; font-size:12px; \" >" + (RS["Label"] + "" == "" ? "" : RS["Label"] + " : ") + RS["Description"] + (RS["Material"] + "" == "" ? "" : " (" + RS["Material"] + ") ") + "</td>" +
																				"<td style=\"text-align:right; padding-right:10px; font-size:12px;  \" >" + Quantity + "</td>" +
																				"<td style=\"text-align:right; padding-right:20px; font-size:12px;  \" >" + UnitPrice + "</td>" +
																				"<td class=\"BR\" style=\"text-align:right; padding-right:30px; font-size:12px;  \" >" + Amount + "</td></tr>");
		}
		string monetaryToAlpha;
		switch (TempMonetaryUnit)
		{
			case "￥": monetaryToAlpha = "RMB "; break;
			case "$" : monetaryToAlpha="USD "; break;
			case "￦": monetaryToAlpha = "KRW ";break;
			case "Y": monetaryToAlpha = "JPY ";break;
			case "HK$": monetaryToAlpha = "HKD ";break;
			case "€": monetaryToAlpha = "EUR ";break;
			default: monetaryToAlpha = " "; break;
		}
        

		TotalAmount = TempTotalAmount == 0 ? "" : monetaryToAlpha + " " + Common.NumberFormat(TempTotalAmount + "");
		for (int i = 0; i < TempTotalQuantity.Length; i++) {
			if (TempTotalQuantity[i] != 0) {
				TotalQuantity += Common.NumberFormat(TempTotalQuantity[i] + "") + " " + Common.GetQuantityUnit((i + 40) + "") + "<br />";
			}
		}

		for (; RowLength < 17; RowLength++)
		{
			ItemList.Append("<tr style=\"height:23px;\"><td class=\"BL\" >&nbsp;</td><td >&nbsp;</td><td >&nbsp;</td><td >&nbsp;</td><td class=\"BR\" >&nbsp;</td></tr>");
		}
	}
}