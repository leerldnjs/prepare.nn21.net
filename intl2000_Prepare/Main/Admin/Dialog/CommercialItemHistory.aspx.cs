using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_Dialog_CommercialItemHistory : System.Web.UI.Page
{
	protected String ItemHistory;
	protected String CompanyPk;
	protected void Page_Load(object sender, EventArgs e)
	{
		CompanyPk = Request.Params["S"] + "";
		ItemHistory = LoadItemHistory(CompanyPk);

		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
	}
	private String LoadItemHistory(string CompanyPk)
	{
		DBConn DB = new DBConn();
		//DB.SqlCmd.CommandText = "SELECT [ItemCode], [Description], [Material], [TarriffRate], [AdditionalTaxRate] FROM ClearanceItemCodeKOR WHERE CompanyPk=" + CompanyPk + " ORDER BY Description ;";
		DB.SqlCmd.CommandText = @"
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
WHERE [CompanyPk]=" + CompanyPk + @" 
AND Deleted is null
--AND [HSCode] IS NOT NULL
AND [ItemCode] IN 
( SELECT MAX([ItemCode])
FROM [ClearanceItemCodeKOR]
GROUP BY [HSCode] 
	  ,[Description]
	  ,[RANDescription]
      ,[RANTradingDescription]
      ,[Material]
      ,[TarriffRate]
      ,[AdditionalTaxRate]
	  ,[FCN1]
      ,[E1]
	  ,[C]
	  ,[Law_Nm] )
ORDER BY HSCode desc,Description ;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder ReturnValue = new StringBuilder();
		int count = 1;
		while (RS.Read()) {
			//string SUM = "<input type=\"hidden\" id=\"SUM[" + count + "]\" value=\"" + RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "#@!" + RS[6] + "#@!" + RS[7] + "\" /><input type='button' value='선택' onclick=\"SelectThis('" + count + "');\" />";


			string SUM = "<input type=\"hidden\" id=\"SUM[" + count + "]\" value=\"" + RS["ItemCode"] + "#@!" + RS["Description"] + "#@!" + RS["Material"] + "#@!" + RS["TarriffRate"] + "#@!" + RS["AdditionalTaxRate"] + "\" /><input type='button' value='S' style=\"color:gray; width:30px; padding-left:0px; padding-right:0px; \" onclick=\"SelectThis('" + count + "','" + RS["HSCode"].ToString() + "','" + RS["ItemCode"].ToString() + "');\" />";
			string Copy = "<input type='button' value='C' style=\"color:blue; width:20px; padding-left:0px; padding-right:0px; \" onclick=\"Copy('" + RS["ItemCode"] + "','" + RS["HSCode"] + "','" + RS["Description"] + "','" + RS["RANDescription"] + "','" + RS["RANTradingDescription"] + "','" + RS["Material"] + "','" + RS["TarriffRate"] + "','" + RS["AdditionalTaxRate"] + "','" + RS["FCN1"] + "','" + RS["E1"] + "','" + RS["C"] + "','" + RS["Law_Nm"] + "');\" />";
			string Modify = "<input type='button' value='M' style=\"color:green; width:20px; padding-left:0px; padding-right:0px; \" onclick=\"Modify('" + RS["ItemCode"] + "','" + RS["HSCode"] + "','" + RS["Description"] + "','" + RS["RANDescription"] + "','" + RS["RANTradingDescription"] + "','" + RS["Material"] + "','" + RS["TarriffRate"] + "','" + RS["AdditionalTaxRate"] + "','" + RS["FCN1"] + "','" + RS["E1"] + "','" + RS["C"] + "','" + RS["Law_Nm"] + "');\" />";
			string DEL = "<input type='button' value='D' style=\"color:red; width:20px; padding-left:0px; padding-right:0px; \" id=\"BTNDelete[" + RS["ItemCode"] + "]\" onclick=\"Delete('" + RS["ItemCode"] + "');\" />";
			string FCN1Check = RS["FCN1"] + "" == "" ? "&nbsp;" : "FCN1적용";
			string E1Check = RS["E1"] + "" == "" ? "&nbsp;" : "E1적용";
			string CCheck = RS["C"] + "" == "" ? "&nbsp;" : "C적용";
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
										"<td class=\"td02\" style='text-align:center;' >" + SUM + " " + Copy + " " + Modify+" " + DEL + "</td></tr>");
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
            "			<td class='tdSubT' style=\"width:120px; \" align='center'><input type=\"button\" value=\"DeleteAll\" onclick=\"DeleteAll();\" /></td>" +
				"		</tr></thead>" +
					ReturnValue +
				"		<tr>" +
				"			<td class='tdSubT' align='center' colspan=\"2\" ><input type=\"text\" id=\"TBHSCode\" style=\"width:95px; \"  /></td>" +
				"			<td class='tdSubT' align='center'><input type=\"text\" id=\"TBDescription\" style=\"width:145px; \"  /></td>" +
				"			<td class='tdSubT' align='center'><input type=\"text\" id=\"TBRANDescription\" style=\"width:145px; \"  /></td>" +
				"			<td class='tdSubT' align='center'><input type=\"text\" id=\"TBRANTradingDescription\" style=\"width:145px; \"  /></td>" +
				"			<td class='tdSubT' align='center'><input type=\"text\" id=\"TBMaterial\" style=\"width:145px; \"  /></td>" +
				"			<td class='tdSubT' align='center'><input type=\"text\" id=\"TBGaunse\" style=\"width:40px; \" /></td>" +
				"			<td class='tdSubT' align='center'><input type=\"text\" id=\"TBBugase\" value=\"10\" style=\"width:40px; \" /></td>" +
				"			<td class='tdSubT' align='center'><input type=\"checkbox\" id=\"FCN1Check\" /> <label for=\"FCN1Check\">FCN1</label></td>" +
				"			<td class='tdSubT' align='center'><input type=\"checkbox\" id=\"E1Check\" /> <label for=\"E1Check\">E1</label></td>" +
				"			<td class='tdSubT' align='center'><input type=\"checkbox\" id=\"CCheck\" /> <label for=\"CCheck\">C</label></td>" +
				"			<td class='tdSubT' align='center'><input type=\"text\" id=\"TBLaw_Nm\" style=\"width:145px; \"  /></td>" +
				"			<td class='tdSubT' align='center'><input type=\"button\" value=\"입력\" onclick=\"Insert();\" /></td>" +
				"		</tr>" +
				"</table>";
	}
}