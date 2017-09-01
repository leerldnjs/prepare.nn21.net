using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_SettlementWithCustoms_Date : System.Web.UI.Page
{
	private DBConn DB;
	protected String TBList;
	protected String TBView;
	protected String TBRange;
	protected String[] MemberInfo;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../Default.aspx"); }
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		if (MemberInfo[0] == "Customs") {
			LogedWithoutRecentRequest111.Visible = false;
			Loged1.Visible = true;
		}

		string Range = Request.Params["Range"] + "";
		string CurrentDate = Request.Params["Date"] + "";
		TBRange=LoadRangeSelect(Range);
		TBList = LoadList(Range, ref CurrentDate);
		TBView = LoadView(CurrentDate);
	}
	private String LoadRangeSelect(string Range)
	{
		StringBuilder ReturnValue = new StringBuilder();
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT left(Original.[Date], 7) 
FROM 
	(
	SELECT [Date] FROM [dbo].[SettlementCustomerSend] GROUP BY [Date] 
	UNION 
	SELECT [Date] FROM [dbo].[SettlementWithCustoms] WHERE isnull([Description], '')<>'*ILBenefit*' GROUP BY [Date] 
	) AS Original
GROUP BY left(Original.[Date], 7) ORDER BY left(Original.[Date], 7)  DESC;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		ReturnValue.Append("<option value=''>전체</option>");
		while (RS.Read()) {
			if (Range == RS[0] + "") {
				ReturnValue.Append(string.Format("<option value='{0}' selected='selected'>{0}</option>", RS[0] + ""));
			} else {
				ReturnValue.Append(string.Format("<option value='{0}'>{0}</option>", RS[0] + ""));
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return "<select id=\"Range\" onchange=\"location.href='../Admin/SettlementWithCustoms_Date.aspx?Range='+this.value\">" + ReturnValue + "</select>";
	}
	private String LoadList(string Range,  ref string Date)
	{
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("	<table border='0' cellpadding='0' cellspacing='0' style='width:400px;' ><thead><tr style=\"height:35px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; \">날짜</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; \" >청구총액</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; \" >송금액</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; \" >차액</td>" +
										"		</tr></thead>");
		string BodyCommercial = "<tr style=\"height:25px;\">" +
											"		<td class='TBody1' ><a onclick=\"View('{0}');\"><span style='cursor:pointer;'>{1}</span></a></td>" +
											"		<td class='TBody1' ><a onclick=\"View('{0}');\"><span style='cursor:pointer;'>{2}</span></a></td>" +
											"		<td class='TBody1' ><a onclick=\"View('{0}');\"><span style='cursor:pointer;'>{3}</span></a></td>" +
											"		<td class='TBody1' ><a onclick=\"View('{0}');\"><span style='cursor:pointer;'>{4}</span></a></td>" +
											"		</tr>";
		DB = new DBConn();
		//Response.Write(Range + "<br />");
		DB.SqlCmd.CommandText = string.Format(@"
SELECT Original.Date, ISNULL(Swc.SumPrice, 0) AS SumPrice, ISNULL(SCS.SumSend, 0) AS SumSend, ISNULL(SCS.SumSend, 0)-ISNULL(Swc.SumPrice, 0) 
FROM 
	(		SELECT [Date] FROM [dbo].[SettlementCustomerSend] GROUP BY [Date] 
		UNION 
		SELECT [Date] FROM [dbo].[SettlementWithCustoms] WHERE isnull([Description], '')<>'*ILBenefit*' GROUP BY [Date] )
	AS Original
	left join 
	(
		SELECT [Date], SUM([Price]) AS SumPrice FROM [dbo].[SettlementWithCustoms] WHERE isnull([Description], '')<>'*ILBenefit*' GROUP BY [Date] 
	) AS SwC ON Original.Date=SwC.Date
	left join ( 
		SELECT [Date], sum([Price]) AS SumSend FROM [dbo].[SettlementCustomerSend] GROUP BY [Date] 
	) AS SCS ON Original.Date=SCS.[Date] 
WHERE 1=1 {0}
ORDER BY Original.Date DESC; ", (Range == "" ? "" : " and left(Original.Date, " + Range.Length + ")='" + Range + "' "));
		//Response.Write(DB.SqlCmd.CommandText);
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		decimal Total0 = 0;
		decimal Total1 = 0;
		decimal Total2 = 0;
		while (RS.Read()) {
			if (Date == "") {
				Date = RS[0] + "";
			}
			string[] StringFormatValue = new string[5];
			StringFormatValue[0] = RS[0] + "";
			if (Date == RS[0] + "") {
				StringFormatValue[1] = "<strong>" + RS[0] + "</stron>";
			} else {
				StringFormatValue[1] = RS[0] + "";
			}
			StringFormatValue[2] = Common.NumberFormat(RS[1] + "");
			StringFormatValue[3] = Common.NumberFormat(RS[2] + "");
			StringFormatValue[4] = Common.NumberFormat(RS[3] + "");
			Total0 += decimal.Parse(RS[1] + "");
			Total1 += decimal.Parse(RS[2] + "");
			Total2 += decimal.Parse(RS[3] + "");

			ReturnValue.Append(String.Format(BodyCommercial, StringFormatValue));
		}
		RS.Dispose();
		DB.DBCon.Close();
		ReturnValue.Append("<tr style=\"height:30px;\">" +
											"		<td class='THead1' >&nbsp;</td>" +
											"		<td class='THead1' >"+Common.NumberFormat(Total0.ToString())+"</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Total1.ToString()) + "</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Total2.ToString()) + "</td>" +
							"		</tr></table><br />");

		return ReturnValue + "</table>";
	}
	private String LoadView(string Date)
	{
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("	<table border='0' cellpadding='0' cellspacing='0' style='width:250px; padding-left:20px;  ' ><thead>" +
										"		<tr style=\"height:40px;\">" +
										"		<td class='THead1' >송금금액</td>" +
										"		<td class='THead1' style='width:30px; text-align:center; color:red;' >&nbsp;</td>" +
										"		</tr></thead>");


		string Format_EachRow = "<tr style=\"height:20px;\">" +
											"		<td class='TBody1' style=\"text-align:right; font-weight:bold; \" >{0}</td>" +
											"		<td class='TBody1' style='text-align:center;' >{1}</td>" +
											"		</tr>";
		DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT [SettlementCustomsSendPk], [Date], [Price], [Description] FROM [dbo].[SettlementCustomerSend] WHERE [Date]='" + Date + @"';";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		decimal TempTotal = 0;
		while (RS.Read()) {
			ReturnValue.Append(String.Format(Format_EachRow, Common.NumberFormat(RS[2] + "") + (RS["Description"] + "" == "" ? "" : " : "+RS["Description"] + ""), "<span style='color:red;' onclick=\"DelThis2('" + RS["SettlementCustomsSendPk"] + "');\">X</span>"));
			TempTotal += decimal.Parse(RS[2] + "");
		}
		RS.Dispose();

		ReturnValue.Append("<tr style=\"height:30px;\">" +
									"		<td class='THead1' style=\"text-align:center; \" colspan='2' >" + Common.NumberFormat(TempTotal.ToString()) + "</td>" +
									"		</tr></table><br />");


		ReturnValue.Append("	<table border='0' cellpadding='0' cellspacing='0' style='width:850px; margin:0 auto; ' ><thead>" +
										"		<tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:140px; \" >House BL</td>" +
										"		<td class='THead1' >Customer</td>" +
										"		<td class='THead1' style='width:50px;'>count</td>" +
										"		<td class='THead1' style='width:80px;' >합계</td>" +
										"		<td class='THead1' >Description</td>" +
										"		<td class='THead1' style='width:15px;' >D</td>" +
										"		</tr></thead>");
		Format_EachRow = "<tr style=\"height:20px;\">" +
											"		<td class='TBody1' style=\"text-align:left; font-weight:bold;\" ><a onclick=\"Goto('CheckDescription','{0}');\">{1}</a></td>" +
											"		<td class='TBody1' style='text-align:left;' ><a onclick=\"Goto('CheckDescription','{0}');\">{2}</a></td>" +
											"		<td class='TBody1' style='text-align:right;' >{3}</td>" +
											"		<td class='TBody1' style='text-align:right;' >{4}</td>" +
											"		<td class='TBody1' style='text-align:left;' >{5}</td>" +
											"		<td class='TBody1' style='text-align:left;' >{6}</td>" +
											"		</tr>";


		DB.SqlCmd.CommandText = @"
SELECT SwC.[SettlementWithCustomsPk]
      ,SwC.[BLNo]
      ,SwC.[Date]
      ,SwC.[Price]
      ,SwC.[Description]
      ,SwC.[AccountId]
	  , CD.CommercialDocumentHeadPk, CD.[Consignee] 
	  , RF.TotalPackedCount, RF.C,  RF.PackingUnit
  FROM [dbo].[SettlementWithCustoms] AS SwC
	left join [dbo].[CommercialDocument] CD ON SwC.BLNo=CD.BLNo 
	left join (
		SELECT 
			CCWR.CommercialDocumentPk, 
			SUM(RF.TotalPackedCount) AS TotalPackedCount, 
			COUNT(*) AS C, 
			SUM(RF.PackingUnit) AS PackingUnit 
		FROM  
			RequestForm AS RF	
			left join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk  
		 GROUP BY CCWR.CommercialDocumentPk) AS RF ON RF.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
  WHERE [Date]='" + Date + @"' 	and isnull([Description], '')<>'*ILBenefit*' 
  ORDER BY SwC.Registerd;";

		RS = DB.SqlCmd.ExecuteReader();
		int Count= 0;
		decimal Sum = 0;

		while (RS.Read()) {
			string[] StringFormatValue = new string[7];

			StringFormatValue[0] = RS["CommercialDocumentHeadPk"] + "";
			StringFormatValue[1] = RS["BLNo"] + "";
			StringFormatValue[2] = RS["Consignee"] + "";
			if (RS["C"] + "" == "1") {
				StringFormatValue[3] = RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
			} else {
				StringFormatValue[3] = RS["TotalPackedCount"] + "GT";			
			}
			StringFormatValue[4] = Common.NumberFormat(RS["Price"] + "");
			StringFormatValue[5] = RS["Description"] + "";
			StringFormatValue[6] = "<span style='color:red;' onclick=\"DelThis('" + RS["SettlementWithCustomsPk"] + "');\">X</span>";
			Sum += decimal.Parse(RS["Price"] + "");
			Count++;
			ReturnValue.Append(String.Format(Format_EachRow, StringFormatValue));
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "<tr style=\"height:30px;\">" +
											"		<td class='THead1' style=\"text-align:center; \" colspan='2' >&nbsp;</td>" +
											"		<td class='THead1' >" + Count + "건</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Sum.ToString()) + "</td>" +
											"		<td class='THead1' colspan='2' >&nbsp;</td>" +
											"		</tr></table>";
	}

}