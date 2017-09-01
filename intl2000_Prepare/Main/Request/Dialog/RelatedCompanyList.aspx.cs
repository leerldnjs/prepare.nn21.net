using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Request_Dialog_RelatedCompanyList : System.Web.UI.Page
{
	protected String CustomerList = string.Empty;
	private DBConn DB;
	protected void Page_Load(object sender, EventArgs e) {
		switch (Session["Language"] + "") {
			case "en":
				Page.UICulture = "en";
				break;
			case "ko":
				Page.UICulture = "ko";
				break;
			case "zh":
				Page.UICulture = "zh-cn";
				break;
			default:
				Page.UICulture = "ko";
				break;
		}
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
		CustomerList = LoadRelatedCompanyList(Request.Params["S"] + "");
	}
	private String LoadRelatedCompanyList(string MainCompanyPk) {
		StringBuilder ReturnValue = new StringBuilder();
		string RowFormat = "<tr><td class=\"td02\" style=\"text-align:center;\">{0}</td>" +
												"<td class=\"td02\" style='text-align:center; font-weight:bold;' >{1}</td>" +
												"<td class=\"td02\" style='text-align:left;' >&nbsp;&nbsp;&nbsp;{2}</td>" +
												"<td class=\"td02\" style='text-align:center;' >{3}</td></tr>";
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT C.[CompanyPk], C.[CompanyName], C.[CompanyNameE], C.[CompanyCode], C.[RegionCode], C.[CompanyAddress]
	, RC.[RegionCodePk], RC.[Name], RC.OurBranchCode
	, CompanyAdd.Value
FROM [Company] AS C 
	left join [RegionCode] AS RC ON C.[RegionCode]=RC.[RegionCode]  
	left join (select * from CompanyAdditionalInfomation where Title=80) CompanyAdd on C.CompanyPk=CompanyAdd.CompanyPk
WHERE C.CompanyPk=" + MainCompanyPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Append("<tr><td class='tdSubT' colspan='2' style=\"font-weight:bold; text-align:center;\" >" + GetGlobalResourceObject("qjsdur", "rhksfldjqcp") + "</td><td class='tdSubT' colspan='2'>&nbsp;</td></tr>");
			string button = "<input type='button' value='select'  onclick='BTNSelectClick(\"" +
				"0\", \"" +
				RS["CompanyPk"] + "\", \"" +
				Server.UrlEncode("" + RS["CompanyName"]) + "\", \"" +
				RS["CompanyCode"] + "\", \"" +
				RS["RegionCode"] + "|" + RS["RegionCodePk"] + "\", \"" +
				RS["OurBranchCode"] + "\")'  />";

			ReturnValue.Append(string.Format(RowFormat, RS["Name"] + "", RS["Value"] + "" != "" ? "<span style =\"color:red; font-weight:bold;\">" + RS["CompanyCode"] + "</span>" : RS["CompanyCode"] + "", RS["Value"] + "" != "" ? "<span style =\"color:red; font-weight:bold;\">" + "*" + RS["CompanyName"] + " " + RS["CompanyNameE"] + "" + "</span>" : "*" + RS["CompanyName"] + " " + RS["CompanyNameE"] + "", button));
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"
	SELECT CR.[CompanyRelationPk], CR.[TargetCompanyPk], CR.[GubunCL], CR.[TargetCompanyNick], CR.[Memo] 
		, C.[CompanyCode], C.CompanyName, C.[CompanyNameE], C.[RegionCode], C.[CompanyAddress]
		, RC.[RegionCodePk], RC.[Name], RC.OurBranchCode
		, CompanyAdd.Value
	FROM [CompanyRelation] AS CR 
		left join [Company] AS C ON CR.[TargetCompanyPk]=C.[CompanyPk]
		left join [RegionCode] AS RC ON C.[RegionCode]=RC.[RegionCode]
		left join (select * from CompanyAdditionalInfomation where Title=80) CompanyAdd on C.CompanyPk=CompanyAdd.CompanyPk
	WHERE CR.[MainCompanyPk]=" + MainCompanyPk + @"
	ORDER BY CR.[GubunCL];";
		RS = DB.SqlCmd.ExecuteReader();
		String Gubun = "-1";
		while (RS.Read()) {
			if (Gubun == "-1" || Gubun != RS["GubunCL"] + "") {
				Gubun = RS["GubunCL"] + "";
				if (Gubun == "0") {
					//ReturnValue.Append("<tr><td class='tdSubT' colspan='2' style=\"font-weight:bold; text-align:center;\" >관리업체</td><td class='tdSubT' colspan='2'>&nbsp;</td></tr>");
				} else {
					ReturnValue.Append("<tr><td class='tdSubT' colspan='2' style=\"font-weight:bold; text-align:center; padding-top:40px; \" >" + GetGlobalResourceObject("qjsdur", "rjfocj") + "</td><td class='tdSubT' colspan='2'>&nbsp;</td></tr>");
				}
			}

			string button = "<input type='button' value='select'  onclick='BTNSelectClick(\"" +
				RS["CompanyRelationPk"] + "\", \"" +
				RS["TargetCompanyPk"] + "\", \"" +
				(RS["TargetCompanyNick"] + "" == "" ? RS["CompanyName"] + "" : RS["TargetCompanyNick"] + "").Replace("'", "") + "\", \"" +
				RS["CompanyCode"] + "\", \"" +
				RS["RegionCode"] + "|" + RS["RegionCodePk"] + "\", \"" +
				RS["OurBranchCode"] + "\" )'  />";
			ReturnValue.Append(string.Format(RowFormat, RS["Name"] + "", RS["Value"] + "" != "" ? "<span style =\"color:red; font-weight:bold;\">" + RS["CompanyCode"] + "</span>" : RS["CompanyCode"] + "", RS["Value"] + "" != "" ? "<span style =\"color:red; font-weight:bold;\">" + (RS["TargetCompanyNick"] + "" == "" ? RS["CompanyName"] + "/" + RS["CompanyNameE"] + "" : RS["TargetCompanyNick"] + "/" + RS["CompanyNameE"] + "") + "</span>" : (RS["TargetCompanyNick"] + "" == "" ? RS["CompanyName"] + "/" + RS["CompanyNameE"] + "" : RS["TargetCompanyNick"] + "/" + RS["CompanyNameE"] + ""), button));
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "";
	}
}