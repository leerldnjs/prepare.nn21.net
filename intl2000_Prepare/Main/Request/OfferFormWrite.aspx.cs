using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class RequestForm_OfferFormWrite : System.Web.UI.Page
{
	protected String[] MemberInfo;
	protected String[] SubInfo;
	protected StringBuilder OfferList;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] == null || Session["MemberInfo"] + "" == "")
		{
			Response.Redirect("~/Default.aspx");
		}
		else { 
			MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
			SubInfo = Session["SubInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		}

		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } }
		catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }
		LoadOfferList(MemberInfo[1], 1);
	}
	private void LoadOfferList(string CompanyPk, int PageNo)
	{
		DBConn DB = new DBConn();
		OfferList = new StringBuilder();
		OfferList.Append("<table style=\"width:848px;\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" >" +
										"<tr>" +
											"<td class=\"tdSubTGold\" style=\"border-bottom:solid 1px #DAA520; \" >"+GetGlobalResourceObject("qjsdur", "wkrtjddlf")+"</td>" +
											"<td class=\"tdSubTGold\" style=\"width:80px; border-bottom:solid 1px #DAA520; \">Style No</td>" +
											"<td class=\"tdSubTGold\" style=\"width:140px;border-bottom:solid 1px #DAA520;\">" + GetGlobalResourceObject("RequestForm", "Description") + "</td>" +
											"<td class=\"tdSubTGold\" style=\"width:140px;border-bottom:solid 1px #DAA520;\">" + GetGlobalResourceObject("RequestForm", "Material") + "</td>" +
											"<td class=\"tdSubTGold\" style=\"width:45px;border-bottom:solid 1px #DAA520;\">" + GetGlobalResourceObject("RequestForm", "Count") + "</td>" +
											"<td class=\"tdSubTGold\" style=\"width:35px;border-bottom:solid 1px #DAA520;\">" + GetGlobalResourceObject("RequestForm", "PackingCount") + "</td>" +
											"<td class=\"tdSubTGold\" style=\"width:60px;border-bottom:solid 1px #DAA520;\">" + GetGlobalResourceObject("RequestForm", "UnitCost") + "</td>" +
											"<td class=\"tdSubTGold\" style=\"width:90px;border-bottom:solid 1px #DAA520;\">" + GetGlobalResourceObject("Member", "rmador") + "</td>" +
											"<td class=\"tdSubTGold\" style=\"width:70px;border-bottom:solid 1px #DAA520;\">KG</td>" +
											"<td class=\"tdSubTGold\" style=\"width:60px;border-bottom:solid 1px #DAA520;\">CBM</td>" +
											"<td class=\"tdSubTGold\" style=\"width:15px;border-bottom:solid 1px #DAA520;\">&nbsp;</td>" +
										"</tr>");
		StringBuilder TempStringBuilder = new StringBuilder();
		string RequestFormPk = string.Empty; ;
		int RowCount = 0;
		decimal TotalQuantity = 0;
		decimal TotalAmount = 0;
		decimal TotalWeight = 0;
		decimal TotalVolume = 0;
		string MonetaryUnit = "";
		DB.SqlCmd.CommandText = @"SELECT RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.AccountID, RF.ConsigneeCCLPk, RF.CompanyInDocumentPk, RF.ConsigneeCode, RF.MonetaryUnitCL
														, RF.StepCL, RF.Memo, RF.RequestDate, RFI.RequestFormItemsPk, RFI.ItemCode, RFI.Description, RFI.Label, RFI.Material, RFI.Quantity, RFI.QuantityUnit
														, RFI.GrossWeight, RFI.Volume, RFI.UnitPrice, RFI.Amount, RFI.PackedCount, RFI.PackingUnit 
											FROM RequestForm as RF
														left join RequestFormItems as RFI on RF.RequestFormPk=RFI.RequestFormPk
											WHERE ShipperPk=" + CompanyPk + @" and StepCL=0
											ORDER BY RF.RequestDate DESC";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		/*	0		RF.RequestFormPk,		* 1		RF.ShipperPk,				* 2		RF.ConsigneePk,		* 3		RF.AccountID,		* 4		RF.ConsigneeCCLPk,		* 5		RF.CompanyInDocumentPk,		
		 * 6		RF.ConsigneeCode,		* 7		RF.MonetaryUnitCL,		* 8		RF.StepCL					* 9		RF.Memo,				* 10		RF.RequestDate			* 11		RFI.RequestFormItemsPk
		 * 12	RFI.ItemCode				* 13		RFI.Description				* 14		RFI.Label					* 15		RFI.Material			* 16		RFI.Quantity					* 17		RFI.QuantityUnit
		 * 18	RFI.GrossWeight			* 19		RFI.Volume					* 20		RFI.UnitPrice				* 21		RFI.Amount  22 포장수량 23 포장유닛*/

		if (RS.Read()) {
			switch (RS[7] + "") {
				case "18":
					MonetaryUnit = "￥";
					break;
				case "19":
					MonetaryUnit = "$";
					break;
				case "20":
					MonetaryUnit = "￦";
					break;
				case "21":
					MonetaryUnit = "Y";
					break;
				case "22":
					MonetaryUnit = "$";
					break;
				case "23":
					MonetaryUnit = "€";
					break;
				default:
					MonetaryUnit = "";
					break;
			}
			RequestFormPk = RS[0] + "";
			if (RS[16] + "" != "") {
				TotalQuantity += Decimal.Parse(RS[16] + "");
			}
			if (RS[21] + "" != "") {
				TotalAmount += Decimal.Parse(RS[21] + "");
			}
			if (RS[18] + "" != "") {
				TotalWeight += Decimal.Parse(RS[18] + "");
			}
			if (RS[19] + "" != "") {
				TotalVolume += Decimal.Parse(RS[19] + "");
			}
			TempStringBuilder.Append("<span onclick=\"LoadOfferSave('" + RS[0] + "')\" style=\"cursor:hand; color:#CD5C5C; \" >" + RS[10].ToString().Substring(0, 10) + "</span></td>" +
													"<td style=\"text-align:center;\">" + RS[14] + "</td>" +
													"<td style=\"text-align:center;\">" + RS[13] + "</td>" +
													"<td style=\"text-align:center;\">" + RS[15] + "</td>" +
													"<td style=\"text-align:center;\">" + Common.NumberFormat(RS[16] + "") + "</td>" +
													"<td style=\"text-align:center;\">" + Common.NumberFormat(RS[22] + "") + "</td>" +
													"<td style=\"text-align:center;\">" + MonetaryUnit + " " + Common.NumberFormat(RS[20] + "") + "</td>" +
													"<td style=\"text-align:center;\">" + MonetaryUnit + " " + Common.NumberFormat(RS[21] + "") + "</td>" +
													"<td style=\"text-align:center;\">" + Common.NumberFormat(RS[18] + "") + "</td>" +
													"<td style=\"text-align:center;\">" + Common.NumberFormat(RS[19] + "") + "</td>" +
													"<td style=\"text-align:center; \" >&nbsp;</td></tr>");
			RowCount++;
			while (RS.Read()) {
				if (RS[0] + "" != RequestFormPk) {
					OfferList.Append("<tr style=\"height:25px; \"><td rowspan=\"" + (RowCount + 1) + "\" style=\"text-align:center; border-bottom:solid 1px #DAA520; \">" +
													TempStringBuilder +
													"<tr style=\"height:30px; \"><td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520;\" colspan=\"3\" >TOTAL</td>" +
														"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520;\">" + Common.NumberFormat(TotalQuantity + "") + "</td>" +
														"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520;\">&nbsp;</td>" +
														"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520;\">&nbsp;</td>" +
														"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520;\">" + MonetaryUnit + " " + Common.NumberFormat(TotalAmount + "") + "</td>" +
														"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520;\">" + (TotalWeight == 0 ? "&nbsp;" : Common.NumberFormat(TotalWeight + "")) + "</td>" +
														"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520;\">" + (TotalVolume == 0 ? "&nbsp;" : Common.NumberFormat(TotalVolume + "")) + "</td>" +
														"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520; color:red; cursor:hand;\" title=\"Delete Document\" onclick=\"DeleteRequestForm('" + RequestFormPk + "')\">X</td></tr>");
					TempStringBuilder = new StringBuilder();
					RequestFormPk = RS[0] + "";
					RowCount = 0;
					TotalAmount = 0;
					TotalQuantity = 0;
					TotalVolume = 0;
					TotalWeight = 0;
					switch (RS[7] + "") {
						case "18":
							MonetaryUnit = "￥";
							break;
						case "19":
							MonetaryUnit = "$";
							break;
						case "20":
							MonetaryUnit = "￦";
							break;
						case "21":
							MonetaryUnit = "Y";
							break;
						case "22":
							MonetaryUnit = "$";
							break;
						case "23":
							MonetaryUnit = "€";
							break;
						default:
							MonetaryUnit = "";
							break;
					}
				} else {
					if (RS[16] + "" != "") {
						TotalQuantity += Decimal.Parse(RS[16] + "");
					}
					if (RS[21] + "" != "") {
						TotalAmount += Decimal.Parse(RS[21] + "");
					}
					if (RS[18] + "" != "") {
						TotalWeight += Decimal.Parse(RS[18] + "");
					}
					if (RS[19] + "" != "") {
						TotalVolume += Decimal.Parse(RS[19] + "");
					}
					TempStringBuilder.Append("<tr style=\"height:25px; \"><td style=\"text-align:center;\">" + RS[14] + "</td>" +
															"<td style=\"text-align:center;\">" + RS[13] + "</td>" +
															"<td style=\"text-align:center;\">" + RS[15] + "</td>" +
															"<td style=\"text-align:center;\">" + Common.NumberFormat(RS[16] + "") + "</td>" +
															"<td style=\"text-align:center;\">" + Common.NumberFormat(RS[22] + "") + "</td>" +
															"<td style=\"text-align:center;\">" + (RS[20] + "" == "&nbsp;" ? "" : MonetaryUnit) + " " + Common.NumberFormat(RS[20] + "") + "</td>" +
															"<td style=\"text-align:center;\">" + MonetaryUnit + " " + Common.NumberFormat(RS[21] + "") + "</td>" +
															"<td style=\"text-align:center;\">" + Common.NumberFormat(RS[18] + "") + "</td>" +
															"<td style=\"text-align:center;\">" + Common.NumberFormat(RS[19] + "") + "</td>" +
															"<td style=\"text-align:center; \" >&nbsp;</td></tr>");
				}
				RowCount++;
				if (RowCount == 1) {
					RequestFormPk = RS[0] + "";
					if (RS[16] + "" != "") {
						TotalQuantity += Decimal.Parse(RS[16] + "");
					}
					if (RS[21] + "" != "") {
						TotalAmount += Decimal.Parse(RS[21] + "");
					}
					if (RS[18] + "" != "") {
						TotalWeight += Decimal.Parse(RS[18] + "");
					}
					if (RS[19] + "" != "") {
						TotalVolume += Decimal.Parse(RS[19] + "");
					}

					TempStringBuilder.Append("<span onclick=\"LoadOfferSave('" + RS[0] + "')\" style=\"cursor:hand; color:#CD5C5C; \" >" + RS[10].ToString().Substring(0, 10) + "</span></td>" +
								  "<td style=\"text-align:center;\">" + RS[14] + "</td>" +
								  "<td style=\"text-align:center;\">" + RS[13] + "</td>" +
								  "<td style=\"text-align:center;\">" + RS[15] + "</td>" +
								  "<td style=\"text-align:center;\">" + Common.NumberFormat(RS[16] + "") + "</td>" +
								  "<td style=\"text-align:center;\">" + Common.NumberFormat(RS[22] + "") + "</td>" +
								  "<td style=\"text-align:center;\">" + (RS[19] + "" == "" ? "&nbsp;" : MonetaryUnit) + " " + Common.NumberFormat(RS[19] + "") + "</td>" +
								  "<td style=\"text-align:center;\">" + MonetaryUnit + " " + Common.NumberFormat(RS[21] + "") + "</td>" +
								  "<td style=\"text-align:center;\">" + Common.NumberFormat(RS[18] + "") + "</td>" +
								  "<td style=\"text-align:center;\">" + Common.NumberFormat(RS[19] + "") + "</td>" +
								  "<td style=\"text-align:center; \" >&nbsp;</td></tr>");
				}

			}
			OfferList.Append("<tr style=\"height:25px; \"><td rowspan=\"" + (RowCount + 1) + "\" style=\"text-align:center; border-bottom:solid 1px #DAA520; \">" +
											TempStringBuilder +
											"<tr style=\"height:30px; \"><td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520; \" colspan=\"3\" >TOTAL</td>" +
												"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520; \">" + Common.NumberFormat(TotalQuantity + "") + "</td>" +
												"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520; \">&nbsp;</td>" +
												"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520;\">&nbsp;</td>" +
												"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520; \">" + MonetaryUnit + " " + Common.NumberFormat(TotalAmount + "") + "</td>" +
												"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520; \">" + (TotalWeight == 0 ? "&nbsp;" : Common.NumberFormat(TotalWeight + "")) + "</td>" +
												"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520; \">" + (TotalVolume == 0 ? "&nbsp;" : Common.NumberFormat(TotalVolume + "")) + "</td>" +
												"<td class=\"tdSubMainGold\" style=\"text-align:center; border-bottom:solid 1px #DAA520; color:red; cursor:hand;\" title=\"Delete Document\" onclick=\"DeleteRequestForm('" + RequestFormPk + "')\">X</td></tr>");
		} else {
			OfferList.Append("<tr style=\"height:90px; \"><td style=\"text-align:center; border-bottom:solid 1px #DAA520; \" colspan=\"11\" >N O&nbsp;&nbsp;&nbsp;D A T A</td></tr>");
		}
		RS.Dispose();
		DB.DBCon.Close();
	}
}