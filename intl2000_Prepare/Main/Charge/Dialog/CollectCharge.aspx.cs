using Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Charge_Dialog_CollectCharge :System.Web.UI.Page
{
	protected string TableName = "";
	protected string TablePk = "";
	protected string CalcHeadPk = "";
	protected string Html_ChargePrice;
	protected string Html_ChargeTotal;
	protected string Html_ChargeBalance;
	protected string Html_CollectInfo;
	protected static string[] MemberInformation;
	protected sRequestFormCalculateHead Head;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("/Default.aspx");
		}
		MemberInformation = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		TableName = Request.Params["N"] + "";
		TablePk = Request.Params["P"] + "";
		CalcHeadPk = Request.Params["H"] + "";

		MakeHtml_ChargeData();
	}

	private string MakeHtml_ChargeData() {
		StringBuilder ChargePrice = new StringBuilder();
		StringBuilder ChargeTotal = new StringBuilder();
		StringBuilder ChargeBalance = new StringBuilder();
		StringBuilder CollectInfo = new StringBuilder();
		ChargeC CC = new ChargeC();
		Head = new sRequestFormCalculateHead();
		DBConn DB = new DBConn();

		DB.DBCon.Open();
		Head = CC.Load_RequestFormCalculateHead(TableName, TablePk, CalcHeadPk, ref DB);
		for (int i = 0; i < Head.arrBody.Count; i++) {
			ChargePrice.Append("<div class=\"col-xs-4 col-xs-offset-2\">");
			ChargePrice.Append(Head.arrBody[i].Title);
			ChargePrice.Append("</div>");
			ChargePrice.Append("<div class=\"col-xs-1\" style=\"text-align:center;\">");
			ChargePrice.Append(Common.GetMonetaryUnit(Head.arrBody[i].Original_Monetary_Unit));
			ChargePrice.Append("</div>");
			ChargePrice.Append("<div class=\"col-xs-4\">");
			ChargePrice.Append(Common.NumberFormat(Head.arrBody[i].Original_Price));
			ChargePrice.Append("</div>");
		}
		DB.DBCon.Close();
		Html_ChargePrice = ChargePrice + "";

		ChargeTotal.Append("<div class=\"col-xs-4 col-xs-offset-2\">합계: </div>");
		ChargeTotal.Append("<div class=\"col-xs-1\" style=\"text-align:center;\">" + Common.GetMonetaryUnit(Head.Monetary_Unit) + "</div>");
		ChargeTotal.Append("<div class=\"col-xs-4\">" + Common.NumberFormat(Head.Total_Price) + "</div>");
		Html_ChargeTotal = ChargeTotal + "";

		ChargeBalance.Append("<div class=\"col-xs-4 col-xs-offset-2\">잔액: </div>");
		ChargeBalance.Append("<div class=\"col-xs-1\" style=\"text-align:center;\">" + Common.GetMonetaryUnit(Head.Monetary_Unit) + "</div>");
		ChargeBalance.Append("<div class=\"col-xs-4\">" + Common.NumberFormat((float.Parse(Head.Total_Price) - float.Parse(Head.Deposited_Price)).ToString()) + "</div>");
		Html_ChargeBalance = ChargeBalance + "";

		
		CollectInfo.Append("<div class=\"col-xs-10 col-xs-offset-2\">" + Head.Bank_Branch_BankName + " : " + Head.Bank_Branch_BankNo + "</div>");
		CollectInfo.Append("<div class=\"col-xs-2 col-xs-offset-2\"><input type=\"text\" id=\"Dateymd\" class=\"form-control\" maxlength=\"8\" value=\"" + DateTime.Now.ToString("yyyyMMdd") + "\" /></div>");
		CollectInfo.Append("<div class=\"col-xs-1\"><input type=\"text\" id=\"Datehh\" class=\"form-control\" maxlength=\"2\" value=\"" + DateTime.Now.ToString("HH") + "\" /></div>");
		CollectInfo.Append("<div class=\"col-xs-1\" style=\"text-align:center; width:10px;\">:</div>");
		CollectInfo.Append("<div class=\"col-xs-1\"><input type=\"text\" id=\"Datemm\" class=\"form-control\" maxlength=\"2\" value=\"" + DateTime.Now.ToString("mm") + "\" /></div>");
		CollectInfo.Append("<div class=\"col-xs-1\" style=\"text-align:right; margin-top:3px;\" id=\"MonetaryUnit\">" + Common.GetMonetaryUnit(Head.Monetary_Unit) + "</div>");
		CollectInfo.Append("<div class=\"col-xs-2\"><input type=\"text\" id=\"Price\" class=\"form-control\" value=\"" + Common.NumberFormat((float.Parse(Head.Total_Price) - float.Parse(Head.Deposited_Price)).ToString()) + "\" /></div>");
		Html_CollectInfo = CollectInfo + "";

		return "0";
	}
}