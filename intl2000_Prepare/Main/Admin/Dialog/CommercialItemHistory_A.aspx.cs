using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_Dialog_CommercialItemHistory_A : System.Web.UI.Page
{
	protected String ItemHistory;
	protected String CompanyPk;
	protected void Page_Load(object sender, EventArgs e)
	{
		CompanyPk = Request.Params["S"] + "";
		string SearchValue = Request.Params["Search"] + "";
		//SearchValue = "96161000";
		if (SearchValue != "") {
			ItemHistory = LoadItemHistory(SearchValue);
		}

		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
	}
	
	private String LoadItemHistory(string SearchValue)
	{
		DBConn DB = new DBConn();
		
		DB.SqlCmd.CommandText = string.Format(@"
SELECT [ItemCode]
      ,[HSCode]
      ,[Description]
	  ,[RANDescription]
      ,[RANTradingDescription]
      ,[Material]
      ,[TarriffRate]
      ,[AdditionalTaxRate]
	  ,[FCN1]
      ,[E1]
	  ,[C]
	  ,[Law_Nm]
       FROM [ClearanceItemCodeKOR] 
WHERE Deleted is null 
and ([HSCode] like '%{0}%' or [Description] like '%{0}%')
ORDER BY HSCode desc,Description ;", SearchValue);
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder ReturnValue = new StringBuilder();
		int count = 1;
		while (RS.Read()) {
			string FCN1Check = RS["FCN1"] + "" == "" ? "&nbsp;" : "FCN1적용";
			string E1Check = RS["E1"] + "" == "" ? "&nbsp;" : "E1적용";
			string CCheck = RS["C"] + "" == "" ? "&nbsp;" : "C적용";
			string SUM = "<input type=\"hidden\" id=\"SUM[" + count + "]\" value=\"" + RS["ItemCode"] + "#@!" + RS["Description"] + "#@!" + RS["Material"] + "#@!" + RS["TarriffRate"] + "#@!" + RS["AdditionalTaxRate"] + "#@!" + RS["HSCode"] + "#@!" + RS["FCN1"] + "#@!" + RS["E1"] + "#@!" + RS["C"] + "#@!" + RS["RANDescription"] + "#@!" + RS["RANTradingDescription"] + "\" /><input type='button' value='S' style=\"color:gray; width:30px; padding-left:0px; padding-right:0px; \" onclick=\"SelectThis('" + count + "','" + RS["HSCode"].ToString() + "','" + RS["ItemCode"].ToString() + "');\" />";
			string DEL = "<input type='button' value='D' style=\"color:red; width:20px; padding-left:0px; padding-right:0px; \" onclick=\"Delete('" + RS["ItemCode"] + "');\" />";
			string Law_Nm = (RS["Law_Nm"].ToString()).Length > 17 ? RS["Law_Nm"].ToString().Substring(0, 17) + ".." : RS["Law_Nm"].ToString();
			ReturnValue.Append("<tr>" +
										"<td class=\"td02\" style=\"font-weight:bold; text-align:center;\">" + count + "</td>" +
										"<td class=\"td02\" style='text-align:left;'>" + RS["HSCode"] + "</td>" +
										"<td class=\"td02\" style='text-align:left;' >" + RS["Description"] + "</td>" +
										"<td class=\"td02\" style='text-align:left;' >" + RS["RANDescription"] + "</td>" +
										"<td class=\"td02\" style='text-align:left;' >" + RS["RANTradingDescription"] + "</td>" +
										"<td class=\"td02\" style='text-align:left;'>" + RS["Material"] + "</td>" +
										"<td class=\"td02\" style='text-align:center;' >" + RS["TarriffRate"] + "</td>" +
										"<td class=\"td02\" style='text-align:center;' >" + RS["AdditionalTaxRate"] + "</td>" +
										"<td class=\"td02\" style='text-align:center;' >" + FCN1Check + "</td>" +
										"<td class=\"td02\" style='text-align:center;' >" + E1Check + "</td>" +
										"<td class=\"td02\" style='text-align:center;' >" + CCheck + "</td>" +
										"<td class=\"td02\" style='text-align:left;' >" + Law_Nm + "</td>" +
										"<td class=\"td02\" style='text-align:center;' >" + SUM + " " + DEL + "</td></tr>");
			count++;
		}
		RS.Dispose();
		DB.DBCon.Close();

		return "	<table border='0' cellpadding='0' cellspacing='0' style=\"width:1200px; \">" +
				"		<thead><tr>" +
				"			<td class='tdSubT' style=\"width:10px; \" align='center'>No</td>" +
				"			<td class='tdSubT' style=\"width:100px; \" align='center'>HSCode</td>" +
				"			<td class='tdSubT' align='center'>품명(모델규격)</td>" +
				"			<td class='tdSubT' style=\"width:150px; \" align='center'>신고품명</td>" +
				"			<td class='tdSubT' style=\"width:150px; \" align='center'>신고거래품명</td>" +
				"			<td class='tdSubT' style=\"width:150px; \" align='center'>재질</td>" +
				"			<td class='tdSubT' style=\"width:50px; \" align='center'>관세</td>" +
				"			<td class='tdSubT' style=\"width:50px; \" align='center'>부가세</td>" +
				"			<td class='tdSubT' style=\"width:50px; \" align='center'>FCN1</td>" +
				"			<td class='tdSubT' style=\"width:50px; \" align='center'>E1</td>" +
				"			<td class='tdSubT' style=\"width:50px; \" align='center'>C</td>" +
				"			<td class='tdSubT' style=\"width:150px; \" align='center'>Law</td>" +
				"			<td class='tdSubT' style=\"width:120px; \" align='center'>&nbsp;</td>" +
				"		</tr></thead>" +
					ReturnValue +
				"</table>";
	}
}