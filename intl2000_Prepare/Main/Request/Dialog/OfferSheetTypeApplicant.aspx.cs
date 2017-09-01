using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Request_Dialog_OfferSheetTypeApplicant : System.Web.UI.Page
{
	protected StringBuilder CCLInfo;
	protected void Page_Load(object sender, EventArgs e)
	{
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");

		try { switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; } }
		catch (Exception) { }

		LoadCompanyCustomerList(Request.Params["S"]);
	}
	private void LoadCompanyCustomerList(string CCLPk)
	{
		CCLInfo = new StringBuilder();
		DBConn DB = new DBConn();
		//보류....
		DB.SqlCmd.CommandText = @"SELECT CCL.TargetCompanyName, CCL.TargetCompanyTEL, CCL.TargetCompanyFAX, CCL.TargetPresidentName, 
																   CID.CompanyInDocumentPk, CID.Name, CID.Address 
													   FROM CompanyCustomerList AS CCL
																LEFT OUTER JOIN CompanyInDocument AS CID ON CCL.CompanyCustomerListPk=CID.GubunPk
													   WHERE CCL.CompanyCustomerListPk='"+CCLPk+"' ORDER BY CID.GubunCL ASC;";
		//보류
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read())
		{ 
			/*		0	CCL.TargetCompanyName, 
			 *		1	CCL.TargetCompanyTEL, 
			 *		2	CCL.TargetCompanyFAX, 
			 *		3	CCL.TargetPresidentName, 
			 *		4	CID.CompanyInDocumentPk, 
			 *		5	CID.Name, 
			 *		6	CID.Address			*/
			CCLInfo.Append("<p>" + RS[0] + "</p>" +
									  "<p>" + RS[1] + "</p>" +
									  "<p>" + RS[2] + "</p>" +
									  "<p>" + RS[3] + "</p>" +
									  "<p><input type=\"hidden\" id=\"CIDPk\" value=\"" + RS[4] + "\" />Company : <input type=\"text\" id=\"CIDName\" value=\"" + RS[5] + "\" /></p>" +
									  "<p><input type=\"hidden\" id=\"HBefore\" value=\"" + RS[5] + "!!!!" + RS[6] + "\" />Address : <input type=\"text\" id=\"CIDAddress\" value=\"" + RS[6] + "\" /></p>");
		}
		RS.Close();
		DB.DBCon.Close();
	}
}
