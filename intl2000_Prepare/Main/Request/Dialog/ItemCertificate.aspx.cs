using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Request_Dialog_ItemCertificate : System.Web.UI.Page
{
	protected StringBuilder HtmlItem;
	protected String Html_Summary;
	private DBConn DB;
	protected void Page_Load(object sender, EventArgs e) {
		DB = new DBConn();
		string RequestFormPk = Request.Params["S"];
		Init_RequestItemTrim(RequestFormPk);
		LoadItemTable(RequestFormPk, ref Html_Summary);
	}
	private bool Init_RequestItemTrim(string RequestFormPk) {
		DB.SqlCmd.CommandText = @"
SELECT [RequestFormItemsPk]
      ,[Label]
  FROM [INTL2010].[dbo].[RequestFormItems] 
  WHERE RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder query = new StringBuilder();
		while (RS.Read()) {
			if (RS["Label"].ToString().IndexOf(" ") > -1) {
				query.Append("UPDATE [INTL2010].[dbo].[RequestFormItems] SET [Label]='" + RS["Label"].ToString().Replace(" ", "") + "' WHERE [RequestFormItemsPk]=" + RS["RequestFormItemsPk"] + ";");
			}
		}
		RS.Dispose();
		if (query.ToString() != "") {
			DB.SqlCmd.CommandText = query + "";
			DB.SqlCmd.ExecuteScalar();
		}
		DB.DBCon.Close();
		return true;
	}
	private void LoadItemTable(string RequestFormPk, ref string Summary) {
		StringBuilder Html_Summary = new StringBuilder();
		HtmlItem = new StringBuilder();
		HtmlItem.Append("	<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:700px; \" >" +
									"		<tr style=\"height:30px;\" >" +
									"			<td bgcolor=\"#F5F5F5\" height=\"20\" align=\"center\" width=\"45px\" >" + GetGlobalResourceObject("RequestForm", "BoxNo") + "</td>" +
									"			<td bgcolor=\"#F5F5F5\" align=\"center\" width='120px; ' >인증번호</td>" +
									"			<td bgcolor=\"#F5F5F5\" align=\"center\" >" +
													GetGlobalResourceObject("RequestForm", "Description") + " / " +
													GetGlobalResourceObject("RequestForm", "Label") + " / " +
													GetGlobalResourceObject("RequestForm", "Material") + "</td>" +
									"			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"65\">" + GetGlobalResourceObject("RequestForm", "Count") + "</td>" +
									"			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"50\">" + GetGlobalResourceObject("RequestForm", "UnitCost") + "</td>" +
									"			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"75\">" + GetGlobalResourceObject("RequestForm", "Amount") + "</td>" +
									"		</tr>");
		DB.SqlCmd.CommandText = "EXEC SP_SelectItemWithRequestFormPk @RequestFormPk = " + RequestFormPk + ";";
		DB.SqlCmd.CommandText = @"
	SELECT 
		IC.CertificateNo,  
		RFI.RequestFormItemsPk, RFI.ItemCode, RFI.MarkNNumber, RFI.Description, RFI.Label, 
		RFI.Material, RFI.Quantity, RFI.QuantityUnit, RFI.PackedCount, RFI.PackingUnit, 
		RFI.GrossWeight, RFI.Volume, RFI.UnitPrice, RFI.Amount, RFI.LastModify, 
		RF.MonetaryUnitCL ,RF.ConsigneePk 
	FROM 
		RequestForm AS RF
		left join RequestFormItems AS RFI On RF.RequestFormPk=RFI.RequestFormPk 
		left join ItemCertificate AS IC ON IC.StyleNo=RFI.Label 
	WHERE RF.RequestFormPk=" + RequestFormPk + @" 
		and ISNULL(IC.CompanyPk, RF.ConsigneePk)=RF.ConsigneePk 
	ORDER BY RFI.RAN; ";

		string monetaryunit = string.Empty;
		bool flagfirst = true;
		/*
	SELECT 
		RFI.RequestFormItemsPk, RFI.ItemCode, RFI.MarkNNumber, RFI.Description, RFI.Label, 
		RFI.Material, RFI.Quantity, RFI.QuantityUnit, RFI.PackedCount, RFI.PackingUnit, 
		RFI.GrossWeight, RFI.Volume, RFI.UnitPrice, RFI.Amount, RFI.LastModify, 
		RF.MonetaryUnitCL 
	FROM 
		RequestForm AS RF
		left join RequestFormItems AS RFI On RF.RequestFormPk=RFI.RequestFormPk 
	WHERE RF.RequestFormPk=@RequestFormPk
		 */
		DB.DBCon.Open();
		List<string[]> CertificateSummary = new List<string[]>();

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (RS["CertificateNo"] + "" != "") {
				string CertificateNo = RS["CertificateNo"] + "";
				int Cursor = -1;
				for (var i = 0; i < CertificateSummary.Count; i++) {
					if (CertificateNo == CertificateSummary[i][0]) {
						Cursor = i;
						break;
					}
				}
				if (Cursor == -1) {
					CertificateSummary.Add(new string[] { CertificateNo, RS["Quantity"] + "" });
				} else {
					CertificateSummary[Cursor][1] = ( decimal.Parse(RS["Quantity"] + "") + decimal.Parse(CertificateSummary[Cursor][1]) ).ToString();
				}
			}
			if (flagfirst) {
				monetaryunit = Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "");
				flagfirst = false;
			}
			HtmlItem.Append("<tr style=\"height:25px; \"><td align='center' class='ItemTableIn'>" + RS["MarkNNumber"] + "</td>");
			HtmlItem.Append("<td align='left' style='padding-left:5px;' class='ItemTableIn'>" + RS["CertificateNo"] + "</td>");

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
					case "51":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " M2" + "</td>");
						break;
					case "52":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " RO" + "</td>");
						break;
					default:
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " " + "</td>");
						break;
				}
			} else {
				HtmlItem.Append("<td align='center' class='ItemTableIn'>&nbsp;</td>");
			}

			HtmlItem.Append("<td align='center' class='ItemTableIn'>" + monetaryunit + " " + Common.NumberFormat(RS["UnitPrice"] + "") + "</td>");


			HtmlItem.Append("<td align='center' class='ItemTableIn'>" + ( RS["Amount"] + "" == "" ? "&nbsp;" : monetaryunit + " " + Common.NumberFormat(RS["Amount"] + "") ) + "</td>");
		}
		RS.Dispose();
		DB.DBCon.Close();

		for (var i = 0; i < CertificateSummary.Count; i++) {
			Html_Summary.Append("<p><strong>" + CertificateSummary[i][0] + "</strong> : " +Common.NumberFormat(CertificateSummary[i][1])+ "</p>");
		}
		Summary = Html_Summary + "";
		//Response.Write(StepCL + "");
		HtmlItem.Append("<tr><td bgcolor=\"#F5F5F5\" height=\"30\" align=\"right\" colspan='8'>&nbsp;</td></tr></table>");
	}
}