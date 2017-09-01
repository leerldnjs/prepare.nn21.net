using Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Charge_CollectList :System.Web.UI.Page
{
	protected String[] MEMBERINFO;

	public static object StringBuiler { get; private set; }

	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("/Default.aspx");
		}
		else {
			MEMBERINFO = (Session["MemberInfo"] + "").Split(Common.Splite11, StringSplitOptions.None);
		}
	}

	[WebMethod]
	public static string MakeHtml_DepositHis(string BankPk, string RetrieveDate) {
		ChargeC CC = new ChargeC();
		StringBuilder ReturnValue = new StringBuilder();
		List<sBankDeposit> BankDeposit = new List<sBankDeposit>();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		BankDeposit = CC.Load_BankDeposit(BankPk, RetrieveDate, ref DB);
		for (int i = 0; i < BankDeposit.Count; i++) {
			ReturnValue.Append("<tr><td>" + BankDeposit[i].DateTime + "</td>");
			ReturnValue.Append("<td>" + BankDeposit[i].Description + "&nbsp;&nbsp;&nbsp;<span class=\"bg-danger\" style=\"cursor:pointer;\" onclick=\"DelDeposit('" + BankDeposit[i].Bank_Deposit_Pk + "')\">X<span></td>");
			if (BankDeposit[i].Price.Substring(0, 1) == "-") {
				ReturnValue.Append("<td> </td>");
				ReturnValue.Append("<td class=\"text-danger\">" + Common.NumberFormat(BankDeposit[i].Price) + "</td>");
			}
			else {
				ReturnValue.Append("<td class=\"text-primary\">" + Common.NumberFormat(BankDeposit[i].Price) + "</td>");
				ReturnValue.Append("<td> </td>");
			}
			ReturnValue.Append("<td>" + Common.NumberFormat(BankDeposit[i].Price_Remain) + "</td></tr>");
		}

		DB.DBCon.Close();
		return ReturnValue + "";
	}
}