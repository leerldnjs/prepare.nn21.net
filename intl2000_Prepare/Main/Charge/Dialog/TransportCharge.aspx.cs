using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Charge_Dialog_TransportCharge :System.Web.UI.Page
{
	protected string TransportPk;
	protected string Already;
	protected string ChargeNo;
	protected string FileCode;
	protected string Html_ChargeBody;
	protected string BranchPk_From;
	protected string BranchPk_To;
	protected string ConsigneePk;
	protected string Datetime_From;
	protected string[] MemberInformation;

	protected sRequestFormCalculateInfo Calculated;
	protected StringBuilder Calculated_Head = new StringBuilder();
	protected StringBuilder Calculated_Item = new StringBuilder();
	protected StringBuilder Calculated_Total = new StringBuilder();

	protected String SelectOurBranch = Common.SelectOurBranch;
	protected static String Monetary = ChargeC.Get_MonetaryUnit("0");

	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("/Default.aspx");
		}
		MemberInformation = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		TransportPk = Request.Params["S"] + "";
		Already = Request.Params["A"] + "";
		ChargeNo = Request.Params["N"] + "";
		FileCode = Request.Params["G"] + "";
		ChargeP CP = new ChargeP();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT [BRANCHPK_FROM], [BRANCHPK_TO], [DATETIME_FROM] FROM [dbo].[TRANSPORT_HEAD] WHERE [TRANSPORT_PK] = " + TransportPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			BranchPk_From = RS["BRANCHPK_FROM"] + "";
			BranchPk_To = RS["BRANCHPK_TO"] + "";
			Datetime_From = RS["DATETIME_FROM"] + "";
		}
		DB.DBCon.Close();
		Html_ChargeBody = CP.MakeHtml_ChargeBody("Transport", "", BranchPk_From);

		if (Already == "Y") {
			Load_Calculated();
		}
	}

	protected string Load_Calculated() {
		ChargeC CC = new ChargeC();
		Calculated = new sRequestFormCalculateInfo();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		Calculated = CC.Load_Calculated("TRANSPORT_HEAD", TransportPk, ChargeNo, ref DB);

		for (int i = 0; i < Calculated.arrHead.Count; i++) {
			Calculated_Head.Append("<th><table><tr><td style=\"display:none;\"><input type=\"hidden\" id=\"Branch_" + i + "\" value=\"" + Calculated.arrHead[i].Branch_Company_Pk + "\" /></td>");
			Calculated_Head.Append("<td style=\"display:none\"><input type=\"hidden\" id=\"BranchBank_" + i + "\" value=\"" + Calculated.arrHead[i].Branch_Bank_Pk + "\" /></td>");
			Calculated_Head.Append("<td><input type=\"button\" id=\"BtnBranch_" + i + "\" class=\"btn btn-default btn-xs\" value=\"" + Calculated.arrHead[i].Branch_Code + "\" onclick=\"ForModal(Type, this.id)\" /></td></tr>");
			Calculated_Head.Append("<tr><td style=\"display:none;\"><input type=\"hidden\" id=\"Customer_" + i + "\" value=\"" + Calculated.arrHead[i].Customer_Company_Pk + "\" /></td>");
			Calculated_Head.Append("<td><input type=\"button\" id=\"BtnCustomer_" + i + "\" class=\"btn btn-default btn-xs\" value=\"" + Calculated.arrHead[i].Customer_Code + "\" onclick=\"ForModal(Type, this.id)\" /></td></tr></table></th>");

			Calculated_Total.Append("<tr><td style=\"display:none;\"><input type=\"text\"  id=\"ChargeHeadPk_" + i + "\" value=\"" + Calculated.arrHead[i].RequestFormCalculate_Head_Pk + "\" /></td>");
			Calculated_Total.Append("<td id=\"TotalBranch_" + i + "\">" + Calculated.arrHead[i].Branch_Code + "</td>");
			Calculated_Total.Append("<td id=\"TotalCustomer_" + i + "\">" + Calculated.arrHead[i].Customer_Code + "</td>");
			Calculated_Total.Append("<td><select class=\"form-control\" id=\"TotalMonetary_" + i + "\">" + ChargeC.Get_MonetaryUnit(Calculated.arrHead[i].Monetary_Unit) + "</select></td>");
			Calculated_Total.Append("<td><input class=\"form-control\" id=\"TotalPrice_" + i + "\" type=\"text\" value=\"" + Common.NumberFormat(Calculated.arrHead[i].Total_Price) + "\" /></td></tr>");


			for (int j = 0; j < Calculated.arrHead[i].arrBody.Count; j++) {
				Calculated_Item.Append("<tr><td style=\"display:none;\"><input type=\"text\" id=\"ChargeBodyPk_" + j + "\" value=\"" + Calculated.arrHead[i].arrBody[j].RequestFormCalculate_Body_Pk + "\" /></td>");
				Calculated_Item.Append("<td style=\"display:none;\"><input type=\"hidden\" id=\"StandardPriceItem_" + j + "\" value=\"" + Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk + "\" /></td>");
				Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_OverWeightFlag_" + j + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk, "[OVERWEIGHT_FLAG]", ref DB) + "\"/></td>");
				Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_5_" + j + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk, "[EXW]", ref DB) + "\" /></td>");
				Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_6_" + j + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk, "[DDP]", ref DB) + "\" /></td>");
				Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_7_" + j + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk, "[CNF]", ref DB) + "\" /></td>");
				Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_8_" + j + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk, "[FOB]", ref DB) + "\" /></td>");
				Calculated_Item.Append("<td><select class=\"form-control\" id=\"SettlementBranch_" + j + "\">" + ChargeC.Get_OurBranch(Calculated.arrHead[i].arrBody[j].Settlement_Company_Pk) + "</select></td>");
				string[] Selected = { "", "", "" };
				switch (Calculated.arrHead[i].arrBody[j].Category) {
					case "해운비":
						Selected[0] = "selected=\"selected\"";
						break;
					case "대행비":
						Selected[1] = "selected=\"selected\"";
						break;
					case "제세금":
						Selected[2] = "selected=\"selected\"";
						break;
				}
				Calculated_Item.Append("<td><select class=\"form-control\" id=\"PriceCategory_" + j + "\"><option value=\"해운비\" " + Selected[0] + ">해운비 (영세)</option><option value=\"대행비\" " + Selected[1] + ">대행비 (과세)</option><option value=\"제세금\" " + Selected[2] + ">제세금 (관.부가세)</option></select></td>");
				Calculated_Item.Append("<td><input type=\"text\" class=\"form-control\" id=\"Title_" + j + "\" value=\"" + Calculated.arrHead[i].arrBody[j].Title + "\" /></td>");
				Calculated_Item.Append("<td><select class=\"form-control\" id=\"Item_MonetaryUnit_" + j + "\">" + ChargeC.Get_MonetaryUnit(Calculated.arrHead[i].arrBody[j].Original_Monetary_Unit) + "</select></td>");
				Calculated_Item.Append("<td><input type=\"text\" class=\"form-control\" id=\"PriceItemValue_" + j + "\" value=\"" + Common.NumberFormat(Calculated.arrHead[i].arrBody[j].Original_Price) + "\" /></td>");
				for (int x = 0; x < Calculated.arrHead.Count; x++) {
					var Checked = "";
					if (Calculated.arrHead[i].arrBody[j].RequestFormCalculate_Head_Pk == Calculated.arrHead[x].RequestFormCalculate_Head_Pk) {
						Checked = "checked=\"checked\"";
					}
					Calculated_Item.Append("<td style=\"text-align:center;\"><input type=\"radio\" id=\"RadioCharge_" + j + "_" + x + "\" name=\"Radio_" + Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk + "\" " + Checked + " /></td>");
				}
				Calculated_Item.Append("</tr>");
			}
		}

		DB.DBCon.Close();
		return "1";
	}

	/*protected string Load_Calculated() {
		ChargeC CC = new ChargeC();
		Calculated = new sRequestFormCalculateInfo();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		Calculated = CC.Load_Calculated("TRANSPORT_HEAD", TransportPk, ChargeNo, ref DB);

		for (int i = 0; i < Calculated.arrHead.Count; i++) {
			Calculated_Head.Append("<th><table><tr><td style=\"display:none;\"><input type=\"hidden\" id=\"Branch_" + i + "\" value=\"" + Calculated.arrHead[i].Branch_Company_Pk + "\" /></td>");
			Calculated_Head.Append("<td style=\"display:none\"><input type=\"hidden\" id=\"BranchBank_" + i + "\" value=\"" + Calculated.arrHead[i].Branch_Bank_Pk + "\" /></td>");
			Calculated_Head.Append("<td><input type=\"button\" id=\"BtnBranch_" + i + "\" class=\"btn btn-default btn-xs\" value=\"" + Calculated.arrHead[i].Branch_Code + "\" onclick=\"ForModal(Type, this.id)\" /></td></tr>");
			Calculated_Head.Append("<tr><td style=\"display:none;\"><input type=\"hidden\" id=\"Customer_" + i + "\" value=\"" + Calculated.arrHead[i].Customer_Company_Pk + "\" /></td>");
			Calculated_Head.Append("<td><input type=\"button\" id=\"BtnCustomer_" + i + "\" class=\"btn btn-default btn-xs\" value=\"" + Calculated.arrHead[i].Customer_Code + "\" onclick=\"ForModal(Type, this.id)\" /></td></tr></table></th>");

			Calculated_Total.Append("<tr><td style=\"display:none;\"><input type=\"text\"  id=\"ChargeHeadPk_" + i + "\" value=\"" + Calculated.arrHead[i].RequestFormCalculate_Head_Pk + "\" /></td>");
			Calculated_Total.Append("<td id=\"TotalBranch_" + i + "\">" + Calculated.arrHead[i].Branch_Code + "</td>");
			Calculated_Total.Append("<td id=\"TotalCustomer_" + i + "\">" + Calculated.arrHead[i].Customer_Code + "</td>");
			Calculated_Total.Append("<td><select class=\"form-control\" id=\"TotalMonetary_" + i + "\">" + ChargeC.Get_MonetaryUnit(Calculated.arrHead[i].Monetary_Unit) + "</select></td>");
			Calculated_Total.Append("<td><input class=\"form-control\" id=\"TotalPrice_" + i + "\" type=\"text\" value=\"" + Common.NumberFormat(Calculated.arrHead[i].Total_Price) + "\" /></td></tr>");
		}

		for (int i = 0; i < Calculated.arrBody.Count; i++) {
			Calculated_Item.Append("<tr><td style=\"display:none;\"><input type=\"text\" id=\"ChargeBodyPk_" + i + "\" value=\"" + Calculated.arrBody[i].RequestFormCalculate_Body_Pk + "\" /></td>");
			Calculated_Item.Append("<td style=\"display:none;\"><input type=\"hidden\" id=\"StandardPriceItem_" + i + "\" value=\"" + Calculated.arrBody[i].StandardPrice_Item_Pk + "\" /></td>");
			Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_OverWeightFlag_" + i + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrBody[i].StandardPrice_Item_Pk, "[OVERWEIGHT_FLAG]", ref DB) + "\"/></td>");
			Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_5_" + i + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrBody[i].StandardPrice_Item_Pk, "[EXW]", ref DB) + "\" /></td>");
			Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_6_" + i + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrBody[i].StandardPrice_Item_Pk, "[DDP]", ref DB) + "\" /></td>");
			Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_7_" + i + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrBody[i].StandardPrice_Item_Pk, "[CNF]", ref DB) + "\" /></td>");
			Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_8_" + i + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrBody[i].StandardPrice_Item_Pk, "[FOB]", ref DB) + "\" /></td>");
			Calculated_Item.Append("<td><select class=\"form-control\" id=\"SettlementBranch_" + i + "\">" + ChargeC.Get_OurBranch(Calculated.arrBody[i].Settlement_Company_Pk) + "</select></td>");
			string[] Selected = { "", "", "" };
			switch (Calculated.arrBody[i].Category) {
				case "해운비":
					Selected[0] = "selected=\"selected\"";
					break;
				case "대행비":
					Selected[1] = "selected=\"selected\"";
					break;
				case "제세금":
					Selected[2] = "selected=\"selected\"";
					break;
			}
			Calculated_Item.Append("<td><select class=\"form-control\" id=\"PriceCategory_" + i + "\"><option value=\"해운비\" " + Selected[0] + ">해운비 (영세)</option><option value=\"대행비\" " + Selected[1] + ">대행비 (과세)</option><option value=\"제세금\" " + Selected[2] + ">제세금 (관.부가세)</option></select></td>");
			Calculated_Item.Append("<td><input type=\"text\" class=\"form-control\" id=\"Title_" + i + "\" value=\"" + Calculated.arrBody[i].Title + "\" /></td>");
			Calculated_Item.Append("<td><select class=\"form-control\" id=\"Item_MonetaryUnit_" + i + "\">" + ChargeC.Get_MonetaryUnit(Calculated.arrBody[i].Original_Monetary_Unit) + "</select></td>");
			Calculated_Item.Append("<td><input type=\"text\" class=\"form-control\" id=\"PriceItemValue_" + i + "\" value=\"" + Common.NumberFormat(Calculated.arrBody[i].Original_Price) + "\" /></td>");
			for (int j = 0; j < Calculated.arrHead.Count; j++) {
				var Checked = "";
				if (Calculated.arrBody[i].RequestFormCalculate_Head_Pk == Calculated.arrHead[j].RequestFormCalculate_Head_Pk) {
					Checked = "checked=\"checked\"";
				}
				Calculated_Item.Append("<td style=\"text-align:center;\"><input type=\"radio\" id=\"RadioCharge_" + i + "_" + j + "\" name=\"Radio_" + i + "\" " + Checked + " /></td>");
			}
			Calculated_Item.Append("</tr>");
		}

		DB.DBCon.Close();
		return "1";
	}*/
}