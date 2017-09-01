using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Finance_ClearanceEndList : System.Web.UI.Page
{
	protected string Html;
	protected string Date;
	protected string MSG;
	protected void Page_Load(object sender, EventArgs e) {
		Date = Request.Params["D"] + "";
		if (Date == "") {
			Date = DateTime.Now.ToString("yyyyMMdd");
		}
		string ReturnValue = LoadClearanceEndData(Date, out MSG);
		if (ReturnValue == "1") {
			Html = MakeHtmlClearanceEnd(Date);
		} else {
			Html = ReturnValue;
		}
	}

	private string MakeHtmlClearanceEnd(string Date) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<table border='0' cellpadding='0' cellspacing='0' style='width:1050px; margin:0 auto; ' ><thead><tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; width:130px;  \">신고번호</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; \"  >수입자</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; \">BL번호</td>" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:80px;'>관세</td>" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:80px;'>부가세</td>" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:90px;' >합계</td>" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:200px;' >서류</td>" +
										"		</tr></thead>");

		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"
SELECT 
	RF.RequestFormPk, RF.ConsigneeCode, RF.ArrivalDate
	, CF.[ClearancedFilePk], CF.[GubunCL], CF.[PhysicalPath]
    , CD.[BLNo]
FROM RequestForm AS RF 
	inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk 
	left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk 
	left join ClearancedFile AS CF ON CF.RequestFormPk=RF.RequestFormPk 
WHERE CD.ClearanceEndDate='" + Date + @"'  
ORDER BY CD.BLNo, CF.[ClearancedFilePk];
";
		List<string[]> AttachedFiles = new List<string[]>();
		string BLNo = "";
		string EachBLFile = "";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (RS["ClearancedFilePk"] + "" == "") {
				continue;
			}
			if (EachBLFile != "" && BLNo != RS["BLNo"] + "") {
				AttachedFiles.Add(new string[] { BLNo, EachBLFile });
				BLNo = RS["BLNo"] + "";
				EachBLFile = "";
			}
			string tempstring;
			switch (RS["GubunCL"] + "") {
				case "0":
					tempstring = "모음";
					break;
				case "1":
					tempstring = "수입신고필증";
					break;
				case "2":
					tempstring = "관부가세 납부영수증";
					break;
				case "3":
					tempstring = "관세사비 세금계산서";
					break;
				case "4":
					tempstring = "수입 세금계산서";
					break;
				case "100":
					tempstring = "모음";
					break;
				case "101":
					tempstring = "수입신고필증";
					break;
				case "102":
					tempstring = "관부가세 납부영수증";
					break;
				case "103":
					tempstring = "관세사비 세금계산서";
					break;
				default:
					tempstring = "기타";
					break;
			}
			EachBLFile += "<br /><a href='../UploadedFiles/FileDownload.aspx?S=" + RS["ClearancedFilePk"] + "&T=ClearancedFile' >" + tempstring + "</a>";
		}
		if (EachBLFile != "") {
			AttachedFiles.Add(new string[] { BLNo, EachBLFile });
		}

		RS.Dispose();

		DB.SqlCmd.CommandText = @"
SELECT 
	RF.RequestFormPk, RF.ConsigneeCode, RF.ArrivalDate
	, CD.CommercialDocumentHeadPk
    , CD.[BLNo]
	, CD.[ClearanceDate]
    , CD.[ClearanceNo]
	, CC.CompanyName AS ConsigneeCompanyName 
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume 
    , T1.Value as T1V, T2.Value as T2V 
	, T1.Value+T2.Value AS TariffSum
FROM RequestForm AS RF 
	inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk 
	left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk 
	left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RCH on RF.RequestFormPk=RCH.[TABLE_PK] 
	left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] AS Value FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세' 
				) AS T1 ON T1.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK] 
	left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] AS Value FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='부가세'
				) AS T2 ON T2.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK]
WHERE CD.ClearanceEndDate='" + Date + @"' 
AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
ORDER BY CD.[ClearanceDate], CD.[BLNo];";

		RS = DB.SqlCmd.ExecuteReader();
		BLNo = "";
		while (RS.Read()) {
			if (RS["BLNo"] + "" == BLNo) {
				continue;
			}
			BLNo = RS["BLNo"] + "";
			string temp_filesum = "";
			foreach (string[] row in AttachedFiles) {
				if (row[0] == BLNo) {
					temp_filesum = row[1].Substring(6);
				}
			}
			ReturnValue.Append("<tr>" +
				"<td class='TBody1' >" + RS["ClearanceNo"] + "</td>" +
				"<td class='TBody1' onclick=\"Goto(" + RS["RequestFormPk"] + ")\"><strong>" + RS["ConsigneeCompanyName"] + "</strong></td>" +
				"<td class='TBody1' >" + RS["BLNo"] + "</td>" +
				"<td class='TBody1' style='text-align:right; '>" + Common.NumberFormat(RS["T1V"] + "") + "</td>" +
				"<td class='TBody1' style='text-align:right; '>" + Common.NumberFormat(RS["T2V"] + "") + "</td>" +
				"<td class='TBody1' style='text-align:right; ' >" + Common.NumberFormat(RS["TariffSum"] + "") + "</td>" +
				"<td class='TBody1' style='text-align:left; padding-left:20px; ' >" + temp_filesum + "</td></tr>");
		}
		RS.Dispose();
		DB.DBCon.Close();

		return ReturnValue + "";
	}
	private string LoadClearanceEndData(string Date, out string MSG) {
		DBConn DB = new DBConn("ReadyKorea");
		MSG = "";
		DB.SqlCmd.CommandText = @"
			SELECT 
				[Rpt_No],[Rpt_Day],[Lis_Day],[BlNo],[Hsn]
				,[Tot_Wt],[Tot_Ut],[Tot_Pack_Cnt],[Tot_Pack_Ut]
				,[Tot_Gs],[Tot_Vat] 
			FROM [dbo].[CUSDEC929A1] 
			WHERE [Rece] in ('수리', '결제', '생성') and [Lis_Day]='" + Date + "';";

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		List<string[]> Data_ReadyKorea = new List<string[]>();
		while (RS.Read()) {
			Data_ReadyKorea.Add(new string[] { RS[0] + "", RS[1] + "", RS[2] + "", RS[3] + "", RS[4] + "", RS[5] + "", RS[6] + "", RS[7] + "", RS[8] + "", RS[9] + "", RS[10] + "" });
		}
		RS.Dispose();
		DB.DBCon.Close();

		StringBuilder BLSum = new StringBuilder();
		foreach (string[] each in Data_ReadyKorea) {
			BLSum.Append(", '" + each[3] + "' ");
		}
		if (BLSum.ToString() == "") {
			return "없습니다.";
		}

		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT 
	CD.[CommercialDocumentHeadPk], CD.[BLNo], CD.[ClearanceEndDate], CD.[ClearanceNo]
	, isnull(Tariff0.Value, 0) AS T1
	, isnull(Tariff1.Value, 0) AS T2 
FROM [dbo].[CommercialDocument] AS CD 
LEFT JOIN [dbo].[CommerdialConnectionWithRequest] AS CCWR ON CD.[CommercialDocumentHeadPk] = CCWR.[CommercialDocumentPk]
LEFT JOIN [dbo].[RequestForm] AS RF ON CCWR.[RequestFormPk] = RF.[RequestFormPk]
Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RCH on RF.RequestFormPk=RCH.[TABLE_PK] 
left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] AS Value FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세' 
			) AS Tariff0 ON Tariff0.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK] 
left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] AS Value FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='부가세'
			) AS Tariff1 ON Tariff1.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK]
WHERE BLNo in (" + BLSum.ToString().Substring(1) + @")
AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm';";

		StringBuilder Query = new StringBuilder();
		DB.DBCon.Open();
		RS = DB.SqlCmd.ExecuteReader();
		string ReturnValue = "";
		while (RS.Read()) {
			int Cursor = -1;
			for (var i = 0; i < Data_ReadyKorea.Count; i++) {
				if (Data_ReadyKorea[i][3].Trim() == (RS["BLNo"] + "").Trim()) {
					Cursor = i;
					break;
				}
			}

			if (Cursor == -1) {
				continue;
			}

			if (decimal.Parse(Data_ReadyKorea[Cursor][9]) != decimal.Parse(RS["T1"] + "") || decimal.Parse(Data_ReadyKorea[Cursor][10]) != decimal.Parse(RS["T2"] + "")) {
				if (decimal.Parse(RS["T1"] + "") == 0 && decimal.Parse(RS["T2"] + "") == 0) {

				} else {
					MSG += "해당비엘의 관부가세 잘못입력되었습니다 : " + RS["BLNo"];
					continue;
				}
			}

			Query.Append(@"
				UPDATE [dbo].[CommercialDocument] SET 
					[ClearanceEndDate] = '" + Data_ReadyKorea[Cursor][2] + @"'
					,[ClearanceNo] = '" + Data_ReadyKorea[Cursor][0] + @"'
				 WHERE [CommercialDocumentHeadPk]=" + RS["CommercialDocumentHeadPk"]);
			ReturnValue = "1";
		}
		RS.Dispose();
		if (ReturnValue == "1") {
			DB.SqlCmd.CommandText = Query + "";
			DB.SqlCmd.ExecuteNonQuery();
		}
		DB.DBCon.Close();
		return ReturnValue;
	}
}