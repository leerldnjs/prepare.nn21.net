using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Components;
using System.Data.SqlClient;

public partial class Request_Dialog_OfferSheetTypeBankInfo : System.Web.UI.Page
{
	protected String[] BankInfo = new String[5] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
	protected void Page_Load(object sender, EventArgs e)
	{
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");

		try { switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; } }
		catch (Exception) { }

		LoadBankInfo(Request.Params["S"]);
	}
	private void LoadBankInfo(string CompanyInDocumentPk)
	{
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"SELECT [CompanyInDocumentBankPk], [Name], [Address], [SwiftCode], [AccountNo]
														 FROM CompanyInDocumentBank
														 WHERE [CompanyInDocumentPk]=" + CompanyInDocumentPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) { BankInfo[0] = RS[0] + ""; BankInfo[1] = RS[1] + ""; BankInfo[2] = RS[2] + ""; BankInfo[3] = RS[3] + ""; BankInfo[4] = RS[4] + ""; }
		RS.Close();
		DB.DBCon.Close();
	}
}
