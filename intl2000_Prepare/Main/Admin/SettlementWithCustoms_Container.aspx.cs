using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_SettlementWithCustoms_Container : System.Web.UI.Page
{
	private DBConn DB;
	protected String TBList;
	protected String TBView;
	protected String TBRange;
	protected String[] MemberInfo;

	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../Default.aspx"); }
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		if (MemberInfo[0] == "Customs") {
			LogedWithoutRecentRequest111.Visible = false;
			Loged1.Visible = true;
		}

		string Range = Request.Params["Range"] + "";
		string CurrentDate = Request.Params["Date"] + "";
		TBRange = LoadRangeSelect(ref Range);
		TBList = LoadList(Range, ref CurrentDate);
		TBView = LoadView(Range, CurrentDate);
	}
	private String LoadRangeSelect(ref string Range) {
		StringBuilder ReturnValue = new StringBuilder();
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
	SELECT
		left(TBBH.[DATETIME_TO], 8)
	FROM 
		[dbo].[TRANSPORT_HEAD] AS TBBH 
		left join TransportBBHistory AS Storage ON TBBH.[TRANSPORT_PK]=Storage.TransportBetweenBranchPk 
		left join RequestForm AS R ON R.RequestFormPk=Storage.RequestFormPk 
	WHERE TBBH.[BRANCHPK_TO]=3157 and Storage.RequestFormPk is not null and TBBH.[DATETIME_TO]>'2012' and R.[DocumentStepCL] not in (3, 4, 0)  
	GROUP BY left(TBBH.[DATETIME_TO], 8)
	ORDER BY left(TBBH.[DATETIME_TO], 8) DESC;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (Range == "") {
				Range = RS[0] + "";
			}
			if (Range == RS[0] + "") {
				ReturnValue.Append(string.Format("<option value='{0}' selected='selected'>{0}</option>", RS[0] + ""));
			} else {
				ReturnValue.Append(string.Format("<option value='{0}'>{0}</option>", RS[0] + ""));
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return "<select id=\"Range\" onchange=\"location.href='../Admin/SettlementWithCustoms_Container.aspx?Range='+this.value\">" + ReturnValue + "</select>";
	}

	private String LoadList(string Range, ref string Date) {
		StringBuilder ReturnValue = new StringBuilder();
		string BodyCommercial = "<tr style=\"height:25px;\">" +
											"		<td class='TBody1' ><a onclick=\"View('{0}');\"><span style='cursor:pointer;'>{1}</span></a></td>" +
											"		<td class='TBody1' ><a onclick=\"View('{0}');\"><span style='cursor:pointer;'>{2}</span></a></td>" +
											"		</tr>";
		DB = new DBConn();
		DB.SqlCmd.CommandText = string.Format(@"
			SELECT 
				left([DATETIME_TO], 10) AS ToDateTime, 
				sum(isnull(Tariff.T1V, 0)+isnull(Tariff.T2V , 0)+isnull(Tariff.T3V , 0)+isnull(Tariff.C1V, 0)-ISNULL(SwC.SettlementPrice, 0) )AS IsClear
				, COUNT(*) AS EachCount
			FROM 
			[dbo].[TRANSPORT_HEAD] AS TBBH 
			left join TransportBBHistory AS Storage ON TBBH.[TRANSPORT_PK]=Storage.TransportBetweenBranchPk 
			left join CommerdialConnectionWithRequest AS CCWR On Storage.RequestFormPk=CCWR.RequestFormPk 
			left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
			left join (
				SELECT CCWR.CommercialDocumentPk, Sum(T1.[ORIGINAL_PRICE]) as T1V, Sum(T2.[ORIGINAL_PRICE]) as T2V, Sum(T3.[ORIGINAL_PRICE]) as T3V , Sum(C1.Price) AS C1V 
				FROM [RequestForm] AS R 
						Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RCH on R.RequestFormPk=RCH.[TABLE_PK] 
						left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세' 
									) AS T1 ON T1.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK] 
						left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='부가세'
									) AS T2 ON T2.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK]
						left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세사비'
									) AS T3 ON T3.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK]   
						left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) AS Price 
									FROM [dbo].[REQUESTFORMCALCULATE_BODY]
									WHERE (Title like '%(VAT포함)%'  OR Title='보수작업비' OR Title='보수작업료' OR Title='인증료' OR Title='임시개청료') and [ORIGINAL_MONETARY_UNIT]=20 
									GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
									) AS C1 ON RCH.[REQUESTFORMCALCULATE_HEAD_PK]=C1.[REQUESTFORMCALCULATE_HEAD_PK] 
						left join CommerdialConnectionWithRequest AS CCWR On R.RequestFormPk=CCWR.RequestFormPk 
						WHERE ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
						GROUP BY CCWR.CommercialDocumentPk		
						) AS Tariff ON Tariff.CommercialDocumentPk=CCWR.CommercialDocumentPk  
			left join (
				SELECT [BLNo], Sum([Price]) AS SettlementPrice
				  FROM [dbo].[SettlementWithCustoms] 
					GROUP BY BLNo		
			) AS SwC ON CD.BLNo=SwC.BLNo 
			left join RequestForm AS R on Storage.RequestFormPk=R.RequestFormPk 
			Left join Company AS CC on R.ConsigneePk=CC.CompanyPk 
			WHERE TBBH.[BRANCHPK_TO]=3157 and Storage.RequestFormPk is not null and R.[DocumentStepCL] not in (3, 4, 0) {0}  
			GROUP BY left([DATETIME_TO], 10)
			ORDER BY ToDateTime DESC
", ( Range == "" ? "" : " and left(TBBH.[DATETIME_TO], " + Range.Length + ")='" + Range + "' " ));
		//Response.Write(DB.SqlCmd.CommandText);
		DB.DBCon.Open();
		int TotalC = 0;
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string[] StringFormatValue = new string[3];
			StringFormatValue[0] = RS[0] + "";
			if (Date == RS[0] + "") {
				StringFormatValue[1] = "<strong>" + RS[0] + "</stron>";
			} else {
				if (RS[1] + "" == "0.0000") {
					StringFormatValue[1] = "<span style='color:blue;'>" + RS[0] + "</span>";
				} else {
					StringFormatValue[1] = RS[0] + "";
				}
			}
			StringFormatValue[2] = RS["EachCount"] + "";
			TotalC += Int32.Parse(RS["EachCount"] + "");
			ReturnValue.Append(String.Format(BodyCommercial, StringFormatValue));
		}
		RS.Dispose();
		DB.DBCon.Close();
		if (Date == "All" || Date == "") {
			Date = "All";
			return "	<table border='0' cellpadding='0' cellspacing='0' style='width:190px;' ><thead><tr style=\"height:35px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; \">날짜</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; \">갯수</td>" +
										"		</tr></thead>" + String.Format(BodyCommercial, "All", "<strong>전체</strong>", Common.NumberFormat(TotalC + "")) + ReturnValue + "</table>";
		} else {
			return "	<table border='0' cellpadding='0' cellspacing='0' style='width:190px;' ><thead><tr style=\"height:35px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; \">날짜</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; \">갯수</td>" +
										"		</tr></thead>" + String.Format(BodyCommercial, "All", "전체", Common.NumberFormat(TotalC + "")) + ReturnValue + "</table>";
		}


	}
	private String LoadView(string Range, string Date) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("	<table border='0' cellpadding='0' cellspacing='0' style='width:1100px; margin:0 auto; ' ><thead>" +
										"		<tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:140px; \" >House BL</td>" +
										"		<td class='THead1' style='width:140px;'>Consignee</td>" +
										"		<td class='THead1' style='width:50px;' >Count</td>" +
										"		<td class='THead1' >관세</td>" +
										"		<td class='THead1' >부가세</td>" +
										"		<td class='THead1' >통관수수료</td>" +
										"		<td class='THead1' >기타비용</td>" +
										"		<td class='THead1' style='width:15px;' >화주 청구금액</td>" +
										"		<td class='THead1' >S</td>" +
										"		<td class='THead1' >C</td>" +
										"		<td class='THead1' >관세사 정산금액</td>" +
										"		<td class='THead1' >차액</td>" +
										"		<td class='THead1' ></td>" +
										"		<td class='THead1' >필증</td>" +
										"		</tr></thead>");
		string Format_EachRow = "<tr style=\"height:20px;\">" +
											"		<td class='{0}' style=\"text-align:center; font-weight:bold;\" >{1}</td>" +
											"		<td class='{0}' style='text-align:left;' >{2}</td>" +
											"		<td class='{0}' style='text-align:right;' >{3}</td>" +
											"		<td class='{0}' style='text-align:right;' >{4}</td>" +
											"		<td class='{0}' style='text-align:right;' >{5}</td>" +
											"		<td class='{0}' style='text-align:right;' >{6}</td>" +
											"		<td class='{0}' style='text-align:right;' >{7}</td>" +
											"		<td class='{0}' style='text-align:right;' >{11}</td>" +
											"		<td class='{0}' style='text-align:right; color:red; ' >{14}</td>" +
											"		<td class='{0}' style='text-align:right; color:blue' >{15}</td>" +
											"		<td class='{0}' style='text-align:right;' >{12}</td>" +
											"		<td class='{0}' style='text-align:right;' >{13}</td>" +
											"		<td class='{0}' style='text-align:right;' >{8}</td>" +
											"		<td class='{0}' style='text-align:right;' >{9}</td>" +
											"		</tr>";
		DB = new DBConn();


		DB.SqlCmd.CommandText = @"
		SELECT 
		TBBH.[DATETIME_TO], 
		Storage.RequestFormPk, Storage.BoxCount 
		, CC.CompanyName , CC.CompanyCode
		, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, CalcH.[DEPOSITED_PRICE]
		, CD.CommercialDocumentHeadPk, CD.BLNo 
		, isnull(Tariff.T1V, 0) as T1V, isnull(Tariff.T2V, 0) as T2V, isnull(Tariff.T3V, 0) as T3V 
		, isnull(Tariff.C1V, 0) AS C1V 
		,  isnull(Tariff.T1V, 0)+isnull(Tariff.T2V, 0)+isnull(Tariff.T3V, 0)+isnull(Tariff.C1V, 0) AS WithCustomerTotal 
		, isnull( SwC.SettlementPrice , 0) AS SettlementPrice
		, R.DocumentStepCL 
		, CF.ClearancedFilePk
	FROM 
		[dbo].[TRANSPORT_HEAD] AS TBBH 
		left join TransportBBHistory AS Storage ON TBBH.[TRANSPORT_PK]=Storage.TransportBetweenBranchPk 
		left join CommerdialConnectionWithRequest AS CCWR On Storage.RequestFormPk=CCWR.RequestFormPk 
		left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
		left join (
			SELECT CCWR.CommercialDocumentPk, Sum(T1.[ORIGINAL_PRICE]) as T1V, Sum(T2.[ORIGINAL_PRICE]) as T2V, Sum(T3.[ORIGINAL_PRICE]) as T3V , Sum(C1.Price) AS C1V 
			FROM [RequestForm] AS R 
					Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RCH on R.RequestFormPk=RCH.[TABLE_PK] 
						left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세' 
									) AS T1 ON T1.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK] 
						left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='부가세'
									) AS T2 ON T2.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK]
						left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세사비'
									) AS T3 ON T3.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK]   
						left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) AS Price 
									FROM [dbo].[REQUESTFORMCALCULATE_BODY]
									WHERE (Title like '%(VAT포함)%'  OR Title='보수작업비' OR Title='보수작업료' OR Title='인증료' OR Title='임시개청료') and [ORIGINAL_MONETARY_UNIT]=20 
									GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
									) AS C1 ON RCH.[REQUESTFORMCALCULATE_HEAD_PK]=C1.[REQUESTFORMCALCULATE_HEAD_PK] 
						left join CommerdialConnectionWithRequest AS CCWR On R.RequestFormPk=CCWR.RequestFormPk 
						WHERE ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
			 GROUP BY CCWR.CommercialDocumentPk		
		) AS Tariff ON Tariff.CommercialDocumentPk=CCWR.CommercialDocumentPk  
		left join (
			SELECT [BLNo], Sum([Price]) AS SettlementPrice
			  FROM [dbo].[SettlementWithCustoms] 
				GROUP BY BLNo		
		) AS SwC ON CD.BLNo=SwC.BLNo 
		left join RequestForm AS R on Storage.RequestFormPk=R.RequestFormPk 
		Left join Company AS CC on R.ConsigneePk=CC.CompanyPk 
		Left join [dbo].[REQUESTFORMCALCULATE_HEAD] AS CalcH on R.RequestFormPk=CalcH.[TABLE_PK] 
		left join [ClearancedFile] AS CF ON CF.RequestFormPk=Storage.RequestFormPk 
	WHERE (TBBH.[BRANCHPK_TO]=3157 OR (TBBH.[BRANCHPK_FROM]=3157 and SwC.SettlementPrice is not null)) 
	AND ISNULL(CalcH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
	AND Storage.RequestFormPk is not null and R.[DocumentStepCL] not in (3, 4, 0) and TBBH.[DATETIME_TO]>'" + Range + "' " + ( Date == "All" ? @" 
	AND isnull(Tariff.T1V, 0)+isnull(Tariff.T2V, 0)+isnull(Tariff.T3V, 0)+isnull(Tariff.C1V, 0)-isnull( SwC.SettlementPrice , 0)<>0 " : " and TBBH.ToDateTime>'" + Range + "'" ) + @"  
	ORDER BY TBBH.[DATETIME_TO],  TBBH.[TRANSPORT_PK],  R.ConsigneeCode, CD.BLNo,  Storage.RequestFormPk;";
		//Response.Write(DB.SqlCmd.CommandText);
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int Count = 0;
		decimal[] Sum = new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		string CheckBLNo = "";
		while (RS.Read()) {
			if (Date == "All") {
				if (( RS["DATETIME_TO"] + "" ).Substring(0, Range.Length) != Range) {
					continue;
				}
			} else {
				if (( RS["DATETIME_TO"] + "" ).Substring(0, 10) != Date) {
					continue;
				}
			}
			string[] StringFormatValue = new string[18];
			if (CheckBLNo == RS["BLNo"] + "") {
				continue;
			} else {
				CheckBLNo = RS["BLNo"] + "";
			}
			StringFormatValue[0] = "TBody1G";
			if (RS["DocumentStepCL"] + "" == "13" || RS["DocumentStepCL"] + "" == "14" || RS["DocumentStepCL"] + "" == "15") {
				StringFormatValue[0] = "TBody1";
			}
			StringFormatValue[1] = "<a onclick=\"Goto('CheckDescription','" + RS["CommercialDocumentHeadPk"] + "');\">" + RS["BLNo"] + "</a>";
			StringFormatValue[2] = "<strong><a onclick=\"Goto('RequestForm','" + RS["RequestFormPk"] + "');\">" + RS["CompanyCode"] + "</a></strong> <a onclick=\"Goto('RequestForm','" + RS["RequestFormPk"] + "');\">" + RS["CompanyName"] + "</a>";
			StringFormatValue[3] = RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");

			StringFormatValue[4] = Common.NumberFormat(RS["T1V"] + "");
			StringFormatValue[5] = Common.NumberFormat(RS["T2V"] + "");
			StringFormatValue[6] = Common.NumberFormat(RS["T3V"] + "");
			StringFormatValue[7] = Common.NumberFormat(RS["C1V"] + "");

			StringFormatValue[11] = "<span style='color:black;' >" + Common.NumberFormat(RS["WithCustomerTotal"] + "") + "</span>";
			StringFormatValue[12] = Common.NumberFormat(RS["SettlementPrice"] + "");

			decimal left = Decimal.Parse(RS["SettlementPrice"] + "") - Decimal.Parse(RS["WithCustomerTotal"] + "");
			StringFormatValue[8] = "&nbsp;";
			if (left < 0) {
				StringFormatValue[8] = "<span style='color:blue;' onclick=\"SendToBenefit('" + RS["BLNo"] + "', '" + ( left * -1 ) + "')\">이익</span>";
			} else if (left > 0) {
				StringFormatValue[8] = "<span style='color:red;' onclick=\"SendToBenefit('" + RS["BLNo"] + "', '" + ( left * -1 ) + "')\">손실</span>";
			}
			if (RS["ClearancedFilePk"] + "" != "") {
				StringFormatValue[9] = "<a href='../UploadedFiles/FileDownload.aspx?S=" + RS["ClearancedFilePk"] + "&T=ClearancedFile' >Down</a></td>";
			} else {
				StringFormatValue[9] = "&nbsp;";
			}
			StringFormatValue[13] = Common.NumberFormat(left.ToString());


			StringFormatValue[14] = Common.NumberFormat(RS["DEPOSITED_PRICE"] + "");
			StringFormatValue[15] = Common.NumberFormat(RS["DEPOSITED_PRICE"] + "");
			Sum[0] += decimal.Parse(RS["T1V"] + "");
			Sum[1] += decimal.Parse(RS["T2V"] + "");
			Sum[2] += decimal.Parse(RS["T3V"] + "");
			Sum[3] += decimal.Parse(RS["C1V"] + "");
			Sum[7] += decimal.Parse(RS["WithCustomerTotal"] + "");
			Sum[8] += decimal.Parse(RS["SettlementPrice"] + "");
			Sum[9] += left;

			Count++;
			ReturnValue.Append(String.Format(Format_EachRow, StringFormatValue));
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "<tr style=\"height:30px;\">" +
											"		<td class='THead1' style=\"text-align:center; \" colspan='2' >&nbsp;</td>" +
											"		<td class='THead1' >" + Count + "건</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Sum[0].ToString()) + "</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Sum[1].ToString()) + "</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Sum[2].ToString()) + "</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Sum[3].ToString()) + "</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Sum[7].ToString()) + "</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Sum[8].ToString()) + "</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Sum[9].ToString()) + "</td>" +
											"		<td class='THead1' colspan='2'>&nbsp;</td>" +
											"		</tr></table>";
	}
}