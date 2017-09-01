using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Finance_BankDeposit_inpyung : System.Web.UI.Page
{
	protected String ListHtml;
	protected String[] MEMBERINFO;
	private string AccountID;

	protected String Date;
	protected String BankPk;

	protected void Page_Load(object sender, EventArgs e) {
		Date = Request.Params["Date"] + "";
		if (Date == "") {
			Date = DateTime.Now.ToShortDateString();
		}
		BankPk = "1";
		try {
			MEMBERINFO = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}
		AccountID = MEMBERINFO[2] + "";

		/*
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		new Finance().Calc_BankRemain(BankPk, ref DB);
		DB.DBCon.Close();
		*/
		ListHtml = MakeHtml(Date, BankPk);
	}

	private string MakeHtml(string Date, string BankPk) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"
			SELECT [BANK_DEPOSIT_PK]
				  ,[CATEGORY]
				  ,[TYPE]
				  ,[TYPE_PK]
				  ,[DESCRIPTION]
				  ,[MONETARY_UNIT]
				  ,[PRICE]
				  ,[PRICE_REMAIN]
				  ,[DATETIME]
			  FROM [INTL2010].[dbo].[BANK_DEPOSIT] 
			  WHERE left([DATETIME], 10)='" + Date + @"' and BANK_PK=" + BankPk + @"
			  ORDER BY [DATETIME] ASC, REGISTERD ASC; ";

		DB.DBCon.Open();
		StringBuilder ReturnValue = new StringBuilder();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (RS["PRICE"].ToString().Substring(0, 1) == "-") {
				ReturnValue.Append("<tr>" +
					"<td class='TBody1' style='text-align:center; padding-top:10px; padding-bottom:10px; ' >" + RS["DATETIME"] + "</td>" +
					"<td class='TBody1' style='text-align:left; padding-left:20px; ' >" + RS["DESCRIPTION"] + "&nbsp;<span style='color:red; cursor:pointer;' onclick=\"Delete_BankDeposit('" + RS["BANK_DEPOSIT_PK"] + "');\">x</span></td>" +
					"<td class='TBody1' style='text-align:right;' >&nbsp;</td>" +
					"<td class='TBody1' style='text-align:right;' >" + Common.NumberFormat(RS["PRICE"] + "") + "</td>" +
					"<td class='TBody1' style='text-align:right;' >" + Common.NumberFormat(RS["PRICE_REMAIN"] + "") + "</td></tr>");
			} else {
				ReturnValue.Append("<tr>" +
					"<td class='TBody1' style='text-align:center; padding-top:10px; padding-bottom:10px; ' >" + RS["DATETIME"] + "</td>" +
					"<td class='TBody1' style='text-align:left; padding-left:20px; ' >" + RS["DESCRIPTION"] + "&nbsp;<span style='color:red; cursor:pointer;' onclick=\"Delete_BankDeposit('"+RS["BANK_DEPOSIT_PK"] +"');\">x</span></td>" +
					"<td class='TBody1' style='text-align:right;' >" + Common.NumberFormat(RS["PRICE"] + "") + "</td>" +
					"<td class='TBody1' style='text-align:right;' >&nbsp;</td>" +
					"<td class='TBody1' style='text-align:right;' >" + Common.NumberFormat(RS["PRICE_REMAIN"] + "") + "</td></tr>");
			}
		}
		RS.Close();
		DB.DBCon.Close();
		return "<table border='0' cellpadding='0' cellspacing='0' style='width:1050px;' >" +
		"<thead><tr style=\"height:40px;\">" +
		"	<td class='THead1' style=\"width:150px; \" >거래일시</td>" +
		"	<td class='THead1' >적요</td>" +
		"	<td class='THead1' style=\"width:120px; \"  >입금</td>" +
		"	<td class='THead1' style=\"width:120px; \"  >출금</td>" +
		"	<td class='THead1' style=\"width:120px; \"  >잔액</td>" +
		"	</tr></thead>" + ReturnValue +
		"	</table>";
	}
}