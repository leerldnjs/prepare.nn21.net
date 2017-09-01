using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Components;
using System.Data.SqlClient;

public partial class Request_PackingListView : System.Web.UI.Page
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
	protected String InvoiceNo;
	protected String PaymentTerms;
	protected String OtherReferences;
	protected StringBuilder ItemList;
	protected String MonetaryUnit;
	//private String MonetaryMark;
	protected String TotalQuantity;
	protected String TotalGrossWeight;
	protected String TotalVolume;
	protected String[] MemberInfo;
	protected String Buyer;
	protected String Memo;
	protected void Page_Load(object sender, EventArgs e)
	{
		try { MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None); }
		catch (Exception) { Response.Redirect("../Default.aspx"); }
		//자기문서아니면 안보이게 만들기!!!!!!
		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } }
		catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }


		LoadCommercialInvoice(Request.Params["S"]);
		LoadCommercialInvoiceItems(Request.Params["S"]);
	}
	private void LoadCommercialInvoice(string RequestFormPk)
	{
		/*		0		RF.ShipperPk,				RF.ConsigneePk,		RF.AccountID,						RF.DepartureDate,				RF.PaymentWhoCL, 
		 *		5		RF.MonetaryUnitCL,		RF.StepCL,				RF.NotifyPartyName,			RF.NotifyPartyAddress,		RF.Memo, 
		 *		10	RF.RequestDate,			CID.Name,				CID.Address,						CID2.Name,							CID2.Address, 
		 *		15	DR.Name,						AR.Name			*/
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "EXECUTE SP_SelectCommercialInvoice @RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read())
		{
			if (RS[0] + "" == MemberInfo[1] || RS[1] + "" == MemberInfo[1])
			{
				Shipper = RS[13] + "";
				ShipperAddress = RS[14] + "";
				Consignee = RS[15] + "";
				ConsigneeAddress = RS[16] + "";
				if ( RS[11] + "" == "" && RS[12] + "" == "" )
				{
					NotifyParty = "Same as Above";
					NotifyPartyAddress = "";
				}
				else
				{
					NotifyParty = RS[11] + "";
					NotifyPartyAddress = RS[12] + "";
				}
				PortOfLanding = RS[18] + "";
				FinalDestination = RS[19] + "";
				if ( RS[5] + "" != "" )
				{
					SailingOnOrAbout = RS[5].ToString().Substring(0, 4) + ". " + RS[5].ToString().Substring(4, 2) + ". " + RS[5].ToString().Substring(6, 2);
				}
				switch ( RS[8] + "" )
				{
					case "3": PaymentTerms = "T / T"; break;
					case "4": PaymentTerms = "L / C"; break;
					default: PaymentTerms = ""; break;
				}
				InvoiceNo = RS[20] + "";
				try { Buyer = (RS[21] + "").Substring(0, (RS[21] + "").IndexOf("@@@@")); }
				catch ( Exception ) { Buyer = ""; }
				try { Memo = (RS[21] + "").Substring((RS[21] + "").IndexOf("@@@@") + 4, (RS[21] + "").LastIndexOf("@@@@") - (RS[21] + "").IndexOf("@@@@") - 4); }
				catch ( Exception ) { Memo = ""; }
				try { Carrier = (RS[21] + "").Substring((RS[21] + "").LastIndexOf("@@@@") + 4); }
				catch ( Exception ) { Carrier = ""; }
			}
			else
			{
				RS.Dispose();
				DB.DBCon.Close();
				Response.Redirect("./OfferFormList.aspx");
			}
		}
		else
		{
			RS.Dispose();
			DB.DBCon.Close();
			Response.Redirect("./OfferFormList.aspx");
		}

		RS.Dispose();
		DB.DBCon.Close();
		/*
		protected	String InvoiceNo;
		protected	String OtherReferences;
		protected String ItemList;
		 */
	}
	private void LoadCommercialInvoiceItems(string RequestFormPk)
	{
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT * FROM RequestFormItems WHERE RequestFormPk=" + RequestFormPk + ";";
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
		
		int RowLength = 0;
		Decimal TempTotalQuantity = 0;
		Decimal TempTotalGrossWeight = 0;
		Decimal TempTotalVolume = 0;
		ItemList = new StringBuilder();
		string TempQuantityUnit = string.Empty;
		string TempQuantityUnitCL = string.Empty;
		TotalQuantity = string.Empty;
		while (RS.Read())
		{
			RowLength++;
			if (RS[7] + "" != "")
			{
				switch (RS[8] + "")
				{
					case "40": TempQuantityUnit = " PCS"; break;
					case "41": TempQuantityUnit = " PRS"; break;
					case "42": TempQuantityUnit = " SET"; break;
					case "43": TempQuantityUnit = " S/F"; break;
					case "44": TempQuantityUnit = " YDS"; break;
					case "45": TempQuantityUnit = " M"; break;
					case "46": TempQuantityUnit = " KG"; break;
					case "47": TempQuantityUnit = " DZ"; break;
					case "48": TempQuantityUnit = " L"; break;
					case "49": TempQuantityUnit = " BOX"; break;
					case "50": TempQuantityUnit = " SQM"; break;
					case "51": TempQuantityUnit = " M2"; break;
					case "52": TempQuantityUnit = " RO"; break;
					default: TempQuantityUnit = ""; break;
				}
				Quantity = Common.NumberFormat(RS[7] + "") + TempQuantityUnit;

				if ( TempQuantityUnitCL + "" == "" )
				{
					TempQuantityUnitCL = RS[8] + "";
					try { TempTotalQuantity = decimal.Parse(RS[7] + ""); }
					catch ( Exception ) { }
				}
				else if ( TempQuantityUnitCL == RS[8] + "" )
				{
					try { TempTotalQuantity += decimal.Parse(RS[7] + ""); }
					catch ( Exception ) { }
				}
				else
				{
					TotalQuantity += Common.NumberFormat(TempTotalQuantity + "") + " " + TempQuantityUnit + "<br />";
					TempQuantityUnit = RS[7] + "";
					try { TempTotalQuantity = decimal.Parse(RS[7] + ""); }
					catch ( Exception )
					{
						TempTotalQuantity = 0;
					}
				}

				try { TempTotalGrossWeight += Decimal.Parse(RS[11] + ""); }
				catch ( Exception ) { }

				try { TempTotalVolume += Decimal.Parse(RS[12] + ""); }
				catch ( Exception ) { }
			}
			else { Quantity = ""; }

			ItemList.Append("<tr style=\"height:30px;\"><td class=\"BL\" style=\"text-align:center;\" >" + RS[3] + "</td>" +
																				"<td style=\"padding-left:20px;\" >" + RS[4] + "</td>" +
																				"<td style=\"text-align:right; padding-right:10px; \" >" + Quantity + "</td>" +
																				"<td style=\"text-align:right; padding-right:10px; \" >" + Common.NumberFormat(RS[11] + "") + " Kg</td>" +
																				"<td style=\"text-align:right; padding-right:10px; \" ></td>" +
																				"<td class=\"BR\" style=\"text-align:right; padding-right:10px; \" >" + Common.NumberFormat(RS[12] + "") + " CBM</td>" +
																				"</tr>");
		}
		TotalQuantity += Common.NumberFormat(TempTotalQuantity + "") + " " + TempQuantityUnit;
		TotalGrossWeight = Common.NumberFormat(TempTotalGrossWeight + "") + " Kg";
		TotalVolume=Common.NumberFormat(TempTotalVolume+"")+" CBM";
		
		//TotalAmount = TempTotalAmount == 0 ? "" : MonetaryMark + " " + Common.NumberFormat(TempTotalAmount + "");
		//if (TempTotalQuantity != 0) { TotalQuantity += Common.NumberFormat(TempTotalQuantity + "") + " " + TempQuantityUnit; }

		for (; RowLength < 9; RowLength++)
		{
			ItemList.Append("<tr style=\"height:30px;\"><td class=\"BL\" >&nbsp;</td><td >&nbsp;</td><td >&nbsp;</td><td >&nbsp;</td><td >&nbsp;</td><td class=\"BR\" >&nbsp;</td></tr>");
		}
	}
}