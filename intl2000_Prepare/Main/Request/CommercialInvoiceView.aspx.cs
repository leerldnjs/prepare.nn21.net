using System;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Request_CommercialInvoiceView : System.Web.UI.Page
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
	protected String MonetaryUnit;
	private String MonetaryMark;
	protected String TotalQuantity;
	protected String TotalAmount;
	protected String[] MemberInfo;
	protected String InvoiceNo;
	protected String Buyer;
	protected String Memo;
	protected void Page_Load(object sender, EventArgs e)
	{
		try {
			MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		} catch (Exception) {
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


		//자기문서아니면 안보이게 만들기!!!!!!
		LoadCommercialInvoice(Request.Params["S"]);
		LoadCommercialInvoiceItems(Request.Params["S"]);
	}
	private void LoadCommercialInvoice(string RequestFormPk)
	{
		/*		0		RF.ShipperPk,				RF.ConsigneePk,		RF.AccountID,						RF.DepartureDate,				RF.PaymentWhoCL, 
		 *		5		RF.MonetaryUnitCL,		RF.StepCL,				RF.NotifyPartyName,			RF.NotifyPartyAddress,		RF.Memo, 
		 *		10	RF.RequestDate,			CID.Name,				CID.Address,						CID2.Name,							CID2.Address, 
		 *		15	DR.Name,						AR.Name 
		 */
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT 
	R.[ShipperPk], R.[DepartureDate], R.[ShipperStaffName], R.[ShipperStaffTEL], R.[ShipperStaffMobile]
	, R.[PaymentWayCL], R.[MonetaryUnitCL], R.[NotifyPartyName], R.[NotifyPartyAddress], R.[Memo]
	, DRC.Name AS DRCN, ARC.Name AS ARCN 
FROM 
	[RequestForm] AS R 
	left join RegionCode AS DRC ON DRC.RegionCode=R.[DepartureRegionCode]
	left join RegionCode AS ARC ON ARC.RegionCode=R.[ArrivalRegionCode]
WHERE 
	R.RequestFormPk=" + RequestFormPk + ";";
		//DB.SqlCmd.CommandText = "EXECUTE SP_SelectCommercialInvoice @RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		if (RS.Read()) {
			if (RS["ShipperPk"] + "" == MemberInfo[1]) {
				switch (RS["MonetaryUnitCL"] + "") {
					case "18":
						MonetaryUnit = "RMB";
						MonetaryMark = "￥";
						break;
					case "19":
						MonetaryUnit = "USD";
						MonetaryMark = "$";
						break;
					case "20":
						MonetaryUnit = "KRW";
						MonetaryMark = "￦";
						break;
					case "21":
						MonetaryUnit = "JPY";
						MonetaryMark = "Y";
						break;
					case "22":
						MonetaryUnit = "HKD";
						MonetaryMark = "$";
						break;
					case "23":
						MonetaryUnit = "EUR";
						MonetaryMark = "€";
						break;
				}
				Shipper = RS["ShipperStaffTEL"] + "";
				string[] address = (RS["Memo"] + "").Split(Common.Splite321, StringSplitOptions.None);
				ShipperAddress = address[0];
				Consignee = RS["ShipperStaffMobile"] + "";
				ConsigneeAddress = address[1];
				if (RS["NotifyPartyName"] + "" == "#@!") {
					NotifyParty = "Same as Above";
					NotifyPartyAddress = "";
				} else {
					string[] notify = (RS["NotifyPartyName"] + "").Split(Common.Splite321, StringSplitOptions.None);
					NotifyParty = notify[0];
					NotifyPartyAddress = notify[1];
				}
				PortOfLanding = RS["DRCN"] + "";
				FinalDestination = RS["ARCN"] + "";
				if (RS["DepartureDate"] + "" != "") {
					SailingOnOrAbout = RS["DepartureDate"].ToString().Substring(0, 4) + ". " + RS["DepartureDate"].ToString().Substring(4, 2) + ". " + RS["DepartureDate"].ToString().Substring(6, 2);
				}
				switch (RS["PaymentWayCL"] + "") {
					case "3":
						PaymentTerms = "T / T";
						break;
					case "4":
						PaymentTerms = "L / C";
						break;
					default:
						PaymentTerms = "";
						break;
				}
				InvoiceNo = RS["ShipperStaffName"] + "";
				string[] temp = (RS["NotifyPartyAddress"] + "").Split(new string[] { "@@@@" }, StringSplitOptions.None);
				Buyer = temp[0];
				Memo = temp[1];
				Carrier = temp[2];
			} else {
				RS.Dispose();
				DB.DBCon.Close();
				Response.Redirect("./OfferFormList.aspx");
			}
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			Response.Redirect("./OfferFormList.aspx");
		}
		RS.Dispose();
		DB.DBCon.Close();
	}
	private void LoadCommercialInvoiceItems(string RequestFormPk)
	{
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT RequestFormItemsPk, ItemCode, MarkNNumber, Description, Label, Quantity, QuantityUnit, UnitPrice, Amount " +
								" FROM RequestFormItems " +
								" where RequestFormPk=" + RequestFormPk + ";";
		/*		0		RequestFormItemsPk, 
		 *		1		ItemCode, 
		 *		2		MarkNNumber, 
		 *		3		Description, 
		 *		4		Label, 
		 *		5		Quantity, 
		 *		6		QuantityUnit, 
		 *		7		UnitPrice, 
		 *		8		Amount	*/

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string Quantity;
		string UnitPrice;
		string Amount;
		int RowLength = 0;
		Decimal TempTotalAmount = 0;
		Decimal TempTotalQuantity = 0;
		ItemList = new StringBuilder();
		string TempQuantityUnit = string.Empty;
		string TempQuantityUnitCL = string.Empty;
		TotalQuantity = string.Empty;
		while (RS.Read()) {
			RowLength++;
			if (RS[5] + "" != "") {
				switch (RS[6] + "") {
					case "40":
						TempQuantityUnit = " PCS";
						break;
					case "41":
						TempQuantityUnit = " PRS";
						break;
					case "42":
						TempQuantityUnit = " SET";
						break;
					case "43":
						TempQuantityUnit = " S/F";
						break;
					case "44":
						TempQuantityUnit = " YDS";
						break;
					case "45":
						TempQuantityUnit = " M";
						break;
					case "46":
						TempQuantityUnit = " KG";
						break;
					case "47":
						TempQuantityUnit = " DZ";
						break;
					case "48":
						TempQuantityUnit = " L";
						break;
					case "49":
						TempQuantityUnit = " BOX";
						break;
					case "50":
						TempQuantityUnit = " SQM";
						break;
					case "51":
						TempQuantityUnit = " M2";
						break;
					case "52":
						TempQuantityUnit = " RO";
						break;
					default:
						TempQuantityUnit = "";
						break;
				}
				Quantity = Common.NumberFormat(RS[5] + "") + TempQuantityUnit;

				if (TempQuantityUnitCL + "" == "") {
					TempQuantityUnitCL = RS[6] + "";
				} else if (TempQuantityUnitCL != RS[6] + "") {
					TotalQuantity += Common.NumberFormat(TempTotalQuantity + "") + " " + TempQuantityUnit + "<br />";
					TempQuantityUnit = RS[6] + "";
				}
				TempTotalQuantity += decimal.Parse(RS[5] + "");
			} else {
				Quantity = "";
			}
			UnitPrice = RS[7] + "" != "" ? UnitPrice = MonetaryMark + " " + Common.NumberFormat(RS[7] + "") : "";
			if (RS[8] + "" != "") {
				Amount = MonetaryMark + " " + Common.NumberFormat(RS[8] + "");
				TempTotalAmount += decimal.Parse(RS[8] + "");
			} else {
				Amount = "";
			}
			ItemList.Append("<tr style=\"height:30px;\"><td class=\"BL\" style=\"text-align:center;\" >" + RS[2] + "</td>" +
																				"<td style=\"padding-left:20px;\" >" + RS[3] + (RS[4] + "" == "" ? "" : " (" + RS[4] + ") ") + "</td>" +
																				"<td style=\"text-align:right; padding-right:10px; \" >" + Quantity + "</td>" +
																				"<td style=\"text-align:right; padding-right:20px; \" >" + UnitPrice + "</td>" +
																				"<td class=\"BR\" style=\"text-align:right; padding-right:30px; \" >" + Amount + "</td></tr>");
		}

		TotalAmount = TempTotalAmount == 0 ? "" : MonetaryMark + " " + Common.NumberFormat(TempTotalAmount + "");
		if (TempTotalQuantity != 0) {
			TotalQuantity += Common.NumberFormat(TempTotalQuantity + "") + " " + TempQuantityUnit;
		}

		for (; RowLength < 9; RowLength++) {
			ItemList.Append("<tr style=\"height:30px;\"><td class=\"BL\" >&nbsp;</td><td >&nbsp;</td><td >&nbsp;</td><td >&nbsp;</td><td class=\"BR\" >&nbsp;</td></tr>");
		}
	}
}