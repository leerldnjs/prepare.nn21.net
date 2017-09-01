using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

/// <summary>
/// ChargeP의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class ChargeP :System.Web.Services.WebService
{

	public ChargeP() {
		//디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
		//InitializeComponent(); 
	}

	[WebMethod]
	public string Load_StrandardPriceValue(string StandardPricePk, string Criterion, string OverWeight) {
		string ReturnValue = "";
		string CriterionPrice = "";
		string OverWeightPrice = "";
		List<string> Temp = new List<string>();
		ChargeC CC = new ChargeC();
		List<sStandardPriceValue> Criterion_Value = new List<sStandardPriceValue>();
		List<sStandardPriceValue> OverWeight_Value = new List<sStandardPriceValue>();

		DBConn DB = new DBConn();
		DB.DBCon.Open();

		Criterion_Value = CC.Load_StandardPriceValue(StandardPricePk, Criterion, ref DB);
		for (int i = 0; i < Criterion_Value.Count; i++) {
			Temp.Add(Criterion_Value[i].StandardPrice_Item_Pk + ":" + Common.NumberFormat(Criterion_Value[i].Price));
		}
		CriterionPrice = string.Join(",!", Temp);
		Temp.Clear();

		OverWeight_Value = CC.Load_StandardPriceValue(StandardPricePk, OverWeight, ref DB);
		for (int i = 0; i < OverWeight_Value.Count; i++) {
			Temp.Add(OverWeight_Value[i].StandardPrice_Item_Pk + ":" + Common.NumberFormat(OverWeight_Value[i].Price));
		}
		OverWeightPrice = string.Join(",!", Temp);

		ReturnValue = CriterionPrice + "@@" + OverWeightPrice;
		DB.DBCon.Close();
		return ReturnValue;
	}

	[WebMethod]
	public string Load_CompanyBank(string BranchPk) {
		StringBuilder ReturnValue = new StringBuilder();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"SELECT [CompanyBankPk], [BankMemo], [BankName], [AccountNo]
		FROM [dbo].[CompanyBank]
		WHERE [IsDel] = 0
		AND [GubunPk] = " + BranchPk;

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("<option value=\"" + RS["CompanyBankPk"] + "\">" + RS["BankMemo"] + " " + RS["BankName"] + " " + RS["AccountNo"] + "</option>");
		}

		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public string LoadList_StPriceItem(string PriceListPk) {
		StringBuilder ReturnValue = new StringBuilder();
		ChargeC CC = new ChargeC();
		sStandardPriceList PriceItem = new sStandardPriceList();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		PriceItem = CC.Load_StandardPriceItem(PriceListPk, ref DB);
		ReturnValue.Append("<option value=\"0\">선택</option>");
		for (int i = 0; i < PriceItem.arrItem.Count; i++) {
			ReturnValue.Append("<option value=\"" + PriceItem.arrItem[i].StandardPrice_Item_Pk + "\">" + PriceItem.arrItem[i].Title + "</option>");
		}

		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public string LoadList_StPriceValue(string PriceItemPk) {
		StringBuilder ReturnValue = new StringBuilder();
		ChargeC CC = new ChargeC();
		List<sStandardPriceValue> PriceValue = new List<sStandardPriceValue>();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		PriceValue = CC.LoadList_StandardPriceValue(PriceItemPk, ref DB);
		ReturnValue.Append("<option value=\"0\">선택</option>");
		for (int i = 0; i < PriceValue.Count; i++) {
			ReturnValue.Append("<option value=\"" + PriceValue[i].StandardPrice_Value_Pk + "\">" + PriceValue[i].Criterion_Text + "</option>");
		}

		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public string Set_FixCharge(string Table_Name, string Table_Pk, string Type, string Charge_No, string AlreadyCalc, string StandardPricePk, string CriterionValue, string OverWeightValue, string PaymentWay, string ExchangeDate, string[] ChargeBodyPk, string[] SettlementBranch, string[] Category, string[] StandardPriceItem, string[] Title, string[] Item_MonetaryUnit, string[] Price, string[] ChargeHeadPk, string[] ChargeBranch, string[] ChargeBranchBank, string[] ChargeCustomer, string[] Charge_MonetaryUnit, string[] ChargeTotalPrice, string[] ExchangeRate, string[][] ChargeCol, string AccountID) {
		sRequestFormCalculateInfo Request = new sRequestFormCalculateInfo();
		sRequestFormCalculateHead Head;
		sRequestFormCalculateBody Body;
		string CalculateHeadPk = "";
		ChargeC CC = new ChargeC();

		int ItemCount = SettlementBranch.Length; //세로수(항목수)
		int ChargeCount = ChargeCol[0].Length; //가로수(청구수)

		DBConn DB = new DBConn();
		DB.DBCon.Open();

		if (Table_Name == "RequestForm") {
			Request.RequestPk = Table_Pk;
			Request.StandardPricePk = StandardPricePk;
			Request.CriterionValue = CriterionValue;
			Request.OverWeightValue = OverWeightValue;
			Request.PaymentWay = PaymentWay;
			Request.ExchangeDate = ExchangeDate;
			Request.StepCL = "58";
			CC.Set_CalculatedInfo(Request, ref DB);

			string Code = "58";
			if (AlreadyCalc == "Y") {
				Code = "59";
			}

			HistoryC HisC = new HistoryC();
			sHistory His = new sHistory();
			His.Table_Name = Table_Name;
			His.Table_Pk = Table_Pk;
			His.Code = Code;
			His.Account_Id = AccountID;
			HisC.Set_History(His, ref DB);
		}

		for (int i = 0; i < ChargeCount; i++) {
			Head = new sRequestFormCalculateHead();
			Head.RequestFormCalculate_Head_Pk = ChargeHeadPk[i];
			Head.Table_Name = Table_Name;
			Head.Table_Pk = Table_Pk;
			Head.Type = Type;
			Head.Charge_No = Charge_No;
			Head.Branch_Company_Pk = ChargeBranch[i];
			Head.Branch_Bank_Pk = ChargeBranchBank[i];
			Head.Customer_Company_Pk = ChargeCustomer[i];
			Head.Monetary_Unit = Charge_MonetaryUnit[i];
			Head.Total_Price = ChargeTotalPrice[i];

			CalculateHeadPk = CC.Set_CalculatedHead(Head, ref DB);
			for (int j = 0; j < ItemCount; j++) {
				if (ChargeCol[j][i] == "Y") {
					Body = new sRequestFormCalculateBody();
					Body.RequestFormCalculate_Body_Pk = ChargeBodyPk[j];
					Body.RequestFormCalculate_Head_Pk = CalculateHeadPk;
					Body.Settlement_Company_Pk = SettlementBranch[j];
					Body.Category = Category[j];
					Body.StandardPrice_Item_Pk = StandardPriceItem[j];
					Body.Title = Title[j];
					Body.Original_Monetary_Unit = Item_MonetaryUnit[j];
					Body.Exchanged_Monetary_Unit = Charge_MonetaryUnit[i];
					Body.To_Exchange_Rate = ExchangeRate[j];
					Body.Original_Price = Price[j];

					CC.Set_CalculatedBody(Body, ref DB);
				}
			}
		}

		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string Set_Deposited(string Table_Name, string Table_Pk, string CalculateHeadPk, string BankPk, string MonetaryUnit, string Price, string AccountID, string DepositedDate, string Description, string Comment) {
		Dictionary<string, string> Data = new Dictionary<string, string>();
		Utility UtilityC = new Utility();
		Finance FinanceP = new Finance();
		HistoryC HisC = new HistoryC();
		sHistory History = new sHistory();
		DBConn DB = new DBConn();
		Data.Add("BANK_DEPOSIT_PK", "");
		Data.Add("BANK_PK", BankPk);
		Data.Add("CATEGORY", "Deposited");
		Data.Add("TYPE", Table_Name);
		Data.Add("TYPE_PK", Table_Pk);
		Data.Add("DESCRIPTION", CalculateHeadPk + " " + Description);
		Data.Add("MONETARY_UNIT", MonetaryUnit);
		Data.Add("PRICE", Price);
		Data.Add("PRICE_REMAIN", "");
		Data.Add("DATETIME", DepositedDate);
		string Query = UtilityC.GetQuery("BANK_DEPOSIT", Data);

		DB.DBCon.Open();
		DB.SqlCmd.CommandText = Query;
		DB.SqlCmd.ExecuteNonQuery();
		FinanceP.Calc_BankRemain(BankPk, ref DB);

		DB.SqlCmd.CommandText = @"UPDATE [dbo].[REQUESTFORMCALCULATE_HEAD] SET [DEPOSITED_PRICE] = ISNULL([DEPOSITED_PRICE], 0) + " + Price + @", [LAST_DEPOSITED_DATE] = getdate() WHERE [REQUESTFORMCALCULATE_HEAD_PK] = " + CalculateHeadPk;
		DB.SqlCmd.ExecuteNonQuery();

		if (Table_Name == "RequestForm") {
			History.Table_Name = Table_Name;
			History.Table_Pk = Table_Pk;
			History.Code = "73";
			History.Description = Price + " " + Comment;
			History.Account_Id = AccountID;
			HisC.Set_History(History, ref DB);
		}

		DB.DBCon.Close();
		return "1";
	}


	[WebMethod]
	public string MakeHtml_ChargeBody(string Type, string PaymentWho, string ShipperPk) {
		StringBuilder ReturnValue = new StringBuilder();
		ChargeC CC = new ChargeC();
		DBConn DB = new DBConn();
		List<sStandardPriceList> SubPriceList = new List<sStandardPriceList>();
		DB.DBCon.Open();
		SubPriceList = CC.LoadList_StandardPriceList(ShipperPk, "", "S", ref DB);
		DB.DBCon.Close();
		if (Type == "Request") {
			string[] Selected = { "", "", "", "", "" };
			switch (PaymentWho) {
				case "5":
					Selected[0] = "checked=\"checked\"";
					break;
				case "6":
					Selected[1] = "checked=\"checked\"";
					break;
				case "7":
					Selected[2] = "checked=\"checked\"";
					break;
				case "8":
					Selected[3] = "checked=\"checked\"";
					break;
				case "9":
				default:
					Selected[4] = "checked=\"checked\"";
					break;
			}
			ReturnValue.Append("<div class=\"row\" style=\"margin-bottom:10px;\"><div class=\"col-xs-offset-1 col-xs-2\" style=\"text-align:center;\"><input type=\"radio\" name=\"Payment\" value=\"5\" " + Selected[0] + " onclick=\"PaymentWay(this.value)\" /> EXW (A)</div>");
			ReturnValue.Append("<div class=\"col-xs-2\" style=\"text-align:center;\" ><input type=\"radio\" name=\"Payment\" value=\"6\" onclick=\"PaymentWay(this.value)\" " + Selected[1] + "/> DDP (B)</div>");
			ReturnValue.Append("<div class=\"col-xs-2\" style=\"text-align:center;\" ><input type=\"radio\" name=\"Payment\" value=\"7\" onclick=\"PaymentWay(this.value)\" " + Selected[2] + "/> CNF (C)</div>");
			ReturnValue.Append("<div class=\"col-xs-2\" style=\"text-align:center;\" ><input type=\"radio\" name=\"Payment\" value=\"8\" onclick=\"PaymentWay(this.value)\" " + Selected[3] + "/> FOB (D)</div>");
			ReturnValue.Append("<div class=\"col-xs-2\" style=\"text-align:center;\" ><input type=\"radio\" name=\"Payment\" value=\"9\" onclick=\"PaymentWay(this.value)\" " + Selected[4] + "/> ETC    </div></div>");
		}
		
		ReturnValue.Append("<div class=\"well m-t\">");
		ReturnValue.Append("<div class=\"row\"><label class=\"control-label col-xs-2\" style=\"text-align:right; margin-top:9px;\">부가 단가표: </label>");
		ReturnValue.Append("<div class=\"col-xs-2\"><select id=\"st_StandardPriceSub\" class=\"form-control\" onchange=\"Changed_StandardPriceSub('List')\"><option value=\"0\">선택</option>");
		for (int i = 0; i < SubPriceList.Count; i++) {
			ReturnValue.Append("<option value=\"" + SubPriceList[i].StandardPrice_Pk + "\">" + SubPriceList[i].Title + "</option>");
		}
		ReturnValue.Append("</select></div></div>");
		ReturnValue.Append("<div class=\"row\"><label class=\"control-label col-xs-2\" style=\"text-align:right; margin-top:9px;\">단가기준1: </label>");
		ReturnValue.Append("<div class=\"col-xs-2\"><select class=\"form-control\" id=\"st_StandardItemSub\" onchange=\"Changed_StandardPriceSub('Item')\"><option value=\"0\">선택</option>");
		ReturnValue.Append("</select></div>");
		ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"button\" class=\"btn btn-default btn-xs\" style=\"margin-top:6px;\" value=\"Load\" onclick=\"LoadStandardPriceItemSub()\" /></div>");
		ReturnValue.Append("<label class=\"control-label col-xs-2\" style=\"text-align:right; margin-top:9px;\">단가기준2: </label>");
		ReturnValue.Append("<div class=\"col-xs-2\"><select class=\"form-control\" id=\"st_StandardValueSub\"><option value=\"0\">선택</option>");
		ReturnValue.Append("</select></div>");
		ReturnValue.Append("<div class=\"col-xs-2\"><input type=\"button\" class=\"btn btn-default btn-xs\" value=\"Confirm\" style=\"margin-top:6px;\" onclick=\"Changed_StandardPriceSub('Value')\" /></div></div>");
		ReturnValue.Append("</div>");

		ReturnValue.Append("<div class=\"col-xs-2\"><input type=\"button\" class=\"btn btn-default btn-xs\" value=\"항목추가\" onclick=\"AddChargeBody();\" /></div>");
		ReturnValue.Append("<div class=\"col-xs-offset-8 col-xs-2\" style=\"text-align:right;\"><input type=\"button\" class=\"btn btn-default btn-xs\" value=\"청구추가\" onclick=\"AddChargeCol()\"/></div>");
		ReturnValue.Append("<div class=\"col-xs-12\"><table class=\"table\" id=\"ChargeTable\"><thead><tr><th>정산지사</th><th>구분</th><th>항목</th><th>단위</th><th>금액</th>" + AddChargeTable_THead(Type, "0") + AddChargeTable_THead(Type, "1") + "</tr></thead>");
		ReturnValue.Append("<tbody id=\"ChargeBody\">");
		ReturnValue.Append("</tbody></table></div>");
		ReturnValue.Append("<div class=\"col-xs-offset-5 col-xs-2\" style=\"margin-top:10px; font-weight:bold;\">Total <input type=\"button\" class=\"btn btn-default btn-xs\" onclick=\"TotalCalc()\" value=\"Calc\" style=\"margin-bottom:6px;\"/></div>");
		ReturnValue.Append("<table class=\"table\"><thead><tr><th>청구회사</th><th>지불회사</th><th>단위</th><th>금액</th></tr></thead>");
		ReturnValue.Append("<tbody id=\"ChargeTotal\">" + AddChargeTotal("Request", "0") + AddChargeTotal("Request", "1") + "</tbody></table>");

		return ReturnValue + "";
	}


	[WebMethod]
	public string MakeHtml_StandardPriceItem(string StandardPricePk) {
		ChargeC CC = new ChargeC();
		StringBuilder ReturnValue = new StringBuilder();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		sStandardPriceList StandardPrice = CC.Load_StandardPriceItem(StandardPricePk, ref DB);

		for (int i = 0; i < StandardPrice.arrItem.Count; i++) {
			ReturnValue.Append("<tr><td style=\"display:none;\"><input type=\"hidden\" id=\"StandardPriceItem_" + i + "\" value=\"" + StandardPrice.arrItem[i].StandardPrice_Item_Pk + "\" /></td>");
			ReturnValue.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_OverWeightFlag_" + i + "\" value=\"" + StandardPrice.arrItem[i].OverWeight_Flag + "\"/></td>");
			ReturnValue.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_5_" + i + "\" value=\"" + StandardPrice.arrItem[i].EXW + "\" /></td>");
			ReturnValue.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_6_" + i + "\" value=\"" + StandardPrice.arrItem[i].DDP + "\" /></td>");
			ReturnValue.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_7_" + i + "\" value=\"" + StandardPrice.arrItem[i].CNF + "\" /></td>");
			ReturnValue.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_8_" + i + "\" value=\"" + StandardPrice.arrItem[i].FOB + "\" /></td>");
			ReturnValue.Append("<td><select class=\"form-control\" id=\"SettlementBranch_" + i + "\">" + ChargeC.Get_OurBranch(StandardPrice.arrItem[i].Settlement_Branch_Pk) + "</select></td>");
			ReturnValue.Append("<td><select class=\"form-control\" id=\"PriceCategory_" + i + "\"><option value=\"해운비\" selected=\"selected\">해운비 (영세)</option><option value=\"대행비\">대행비 (과세)</option><option value=\"제세금\">제세금 (관.부가세)</option></select></td>");
			ReturnValue.Append("<td><input type=\"text\" class=\"form-control\" id=\"Title_" + i + "\" value=\"" + StandardPrice.arrItem[i].Title + "\" /></td>");
			ReturnValue.Append("<td><select class=\"form-control\" id=\"Item_MonetaryUnit_" + i + "\">" + ChargeC.Get_MonetaryUnit(StandardPrice.arrItem[i].Monetary_Unit) + "</select></td>");
			ReturnValue.Append("<td><input type=\"text\" class=\"form-control\" id=\"PriceItemValue_" + i + "\" /></td>");
			ReturnValue.Append(AddChargeTable_TBody("Request", i.ToString(), "0", StandardPrice.arrItem[i].StandardPrice_Item_Pk) + AddChargeTable_TBody("Request", i.ToString(), "1", StandardPrice.arrItem[i].StandardPrice_Item_Pk) + "</tr>");
		}

		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public string MakeHtml_StandardPriceItemSub(string StandardPricePk, string StandardItemPk, string Col, string Row) {
		ChargeC CC = new ChargeC();
		StringBuilder ReturnValue = new StringBuilder();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		sStandardPriceList StandardPrice = CC.Load_StandardPriceItem(StandardPricePk, ref DB);

		for (int i = 0; i < StandardPrice.arrItem.Count; i++) {
			if (StandardPrice.arrItem[i].StandardPrice_Item_Pk == StandardItemPk) {
				ReturnValue.Append("<tr><td style=\"display:none;\"><input type=\"hidden\" id=\"StandardPriceItem_" + Row + "\" value=\"" + StandardPrice.arrItem[i].StandardPrice_Item_Pk + "\" /></td>");
				ReturnValue.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_OverWeightFlag_" + Row + "\" value=\"" + StandardPrice.arrItem[i].OverWeight_Flag + "\"/></td>");
				ReturnValue.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_5_" + Row + "\" value=\"" + StandardPrice.arrItem[i].EXW + "\" /></td>");
				ReturnValue.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_6_" + Row + "\" value=\"" + StandardPrice.arrItem[i].DDP + "\" /></td>");
				ReturnValue.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_7_" + Row + "\" value=\"" + StandardPrice.arrItem[i].CNF + "\" /></td>");
				ReturnValue.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_8_" + Row + "\" value=\"" + StandardPrice.arrItem[i].FOB + "\" /></td>");
				ReturnValue.Append("<td><select class=\"form-control\" id=\"SettlementBranch_" + Row + "\">" + ChargeC.Get_OurBranch(StandardPrice.arrItem[i].Settlement_Branch_Pk) + "</select></td>");
				ReturnValue.Append("<td><select class=\"form-control\" id=\"PriceCategory_" + Row + "\"><option value=\"해운비\">해운비 (영세)</option><option value=\"대행비\" selected=\"selected\">대행비 (과세)</option><option value=\"제세금\">제세금 (관.부가세)</option></select></td>");
				ReturnValue.Append("<td><input type=\"text\" class=\"form-control\" id=\"Title_" + Row + "\" value=\"" + StandardPrice.arrItem[i].Title + "\" /></td>");
				ReturnValue.Append("<td><select class=\"form-control\" id=\"Item_MonetaryUnit_" + Row + "\">" + ChargeC.Get_MonetaryUnit(StandardPrice.arrItem[i].Monetary_Unit) + "</select></td>");
				ReturnValue.Append("<td><input type=\"text\" class=\"form-control\" id=\"PriceItemValue_" + Row + "\" /></td>");
				for (int j = 0; j <= Int32.Parse(Col); j++) {
					ReturnValue.Append(AddChargeTable_TBody("Request", Row, j.ToString(), StandardPrice.arrItem[i].StandardPrice_Item_Pk));
				}
				ReturnValue.Append("</tr>");
			}
		}

		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public string MakeHtml_Modal(string Type, string ShipperPk, string ConsigneePk) {
		StringBuilder ReturnValue = new StringBuilder();
		DBConn DB = new DBConn();
		SqlDataReader RS;
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"
			SELECT 'Shipper' AS WHO, [CompanyPk], [CompanyCode], [CompanyName], [PresidentName] 
			FROM [dbo].[Company]
			WHERE [CompanyPk] = " + ShipperPk + @" 
			UNION ALL
			SELECT 'Consignee' AS WHO, [CompanyPk], [CompanyCode], [CompanyName], [PresidentName] 
			FROM [dbo].[Company]
			WHERE [CompanyPk] = " + ConsigneePk;
		RS = DB.SqlCmd.ExecuteReader();

		ReturnValue.Append("<table class=\"table\">");
		ReturnValue.Append("<thead><tr><th>청구지사</th></tr></thead>");
		ReturnValue.Append("<tr><td><select id=\"St_Branch\" class=\"form-control\" onchange=\"RetrieveBank(this.value)\"><option value=0>지사를 선택하세요.</option>" + ChargeC.Get_OurBranch("0") + "</select></td>");
		ReturnValue.Append("<td><select id=\"St_BranchBank\" class=\"form-control\"><option value=\"0\">지사를 선택하세요.</option></select></td></tr>");
		ReturnValue.Append("</table>");

		ReturnValue.Append("<table class=\"table\"><thead><tr><th>지불회사</th></tr></thead>");
		while (RS.Read()) {
			ReturnValue.Append("<tr onclick=\"ChoosedCustomer('" + RS["CompanyPk"] + "', '" + RS["CompanyCode"] + "')\"><td class=\"text-danger\">" + RS["WHO"] + "</td><td>" + RS["CompanyCode"] + "</td><td>" + RS["CompanyName"] + "</td><td>" + RS["PresidentName"] + "</td></tr>");
		}
		ReturnValue.Append("</table>");
		RS.Close();

		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public string Get_PriceValueSub(string PriceValuePk) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT [PRICE] FROM [dbo].[STANDARDPRICE_VALUE] WHERE [STANDARDPRICE_VALUE_PK] = " + PriceValuePk;
		string ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		return ReturnValue;
	}

	[WebMethod]
	public string AddChargeTable_THead(string Type, string Count) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<th><table><tr><td style=\"display:none;\"><input type=\"hidden\" id=\"Branch_" + Count + "\" /></td>");
		ReturnValue.Append("<td style=\"display:none\"><input type=\"hidden\" id=\"BranchBank_" + Count + "\" /></td>");
		ReturnValue.Append("<td><input type=\"button\" id=\"BtnBranch_" + Count + "\" class=\"btn btn-default btn-xs\" value=\"청구회사\" onclick=\"ForModal('" + Type + "', this.id)\" /></td></tr>");
		ReturnValue.Append("<tr><td style=\"display:none;\"><input type=\"hidden\" id=\"Customer_" + Count + "\" /></td>");
		ReturnValue.Append("<td><input type=\"button\" id=\"BtnCustomer_" + Count + "\" class=\"btn btn-default btn-xs\" value=\"지불회사\" onclick=\"ForModal('" + Type + "', this.id)\" /></td></tr></table></th>");

		return ReturnValue + "";
	}

	[WebMethod]
	public string AddChargeTable_TBody(string Type, string Row, string Col, string Group) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<td style=\"text-align:center;\"><input type=\"radio\" id=\"RadioCharge_" + Row + "_" + Col + "\" name=\"Radio_" + Group + "\" /></td>");

		return ReturnValue + "";
	}

	[WebMethod]
	public string AddChargeTotal(string Type, string Count) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<tr><td id=\"TotalBranch_" + Count + "\"></td>");
		ReturnValue.Append("<td id=\"TotalCustomer_" + Count + "\"></td>");
		ReturnValue.Append("<td><select class=\"form-control\" id=\"TotalMonetary_" + Count + "\">" + ChargeC.Get_MonetaryUnit("0") + "</select></td>");
		ReturnValue.Append("<td><input class=\"form-control\" id=\"TotalPrice_" + Count + "\" type=\"text\" /></td></tr>");

		return ReturnValue + "";
	}

	[WebMethod]
	public string GetChargeNo(string TableName, string TablePk, string Type) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"SELECT MAX([CHARGE_NO]) FROM [dbo].[REQUESTFORMCALCULATE_HEAD] WHERE [TABLE_NAME] = '" + TableName + @"' AND [TABLE_PK] = " + TablePk + @" AND [TYPE] = '" + Type + @"'";
		string MaxChargeNo = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();

		return MaxChargeNo;
	}


}
