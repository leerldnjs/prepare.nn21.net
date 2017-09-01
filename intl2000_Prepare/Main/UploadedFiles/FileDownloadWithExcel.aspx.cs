using Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

public partial class UploadedFiles_FileDownloadWithExcel : System.Web.UI.Page
{
	private DBConn DB;
	private String stepCL;

	protected void Page_Load(object sender, EventArgs e) {
		try {
			if (Request["Language"].Length == 2) {
				Session["Language"] = Request["Language"];
			}
		} catch (Exception) {
		}
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
		}

		DB = new DBConn();
		DB.DBCon.Open();
		string Title = "";
		string result = "";
		switch (Request.Params["G"] + "") {
			case "SimpleMisu":
				result = LoadSimpleMisu(Request.Params["D"] + "", Request.Params["T"] + "", ref Title);
				break;
			case "SimpleMisu_2014Before":
				result = LoadSimpleMisu(Request.Params["D"] + "", Request.Params["T"] + "", ref Title);
				break;
			case "TodayAbji":
				string branchPk = Request.Params["S"] + "";
				string arrivalDate = Request.Params["D"] + "";
				string InorOut = Request.Params["G2"] + "";
				string InorOutDate;
				string InorOutBranch;

				List<string> TBBPk = new List<string>();
				InorOutDate = InorOut == "in" ? "[DATETIME_TO]" : "[DATETIME_FROM]";
				InorOutBranch = InorOut == "in" ? "[BRANCHPK_TO]" : "[BRANCHPK_FROM]";

				DB.SqlCmd.CommandText = "SELECT [TRANSPORT_PK] FROM [dbo].[TRANSPORT_HEAD] WHERE " + InorOutBranch + "=" + branchPk + " and left(" + InorOutDate + ", 8)='" + arrivalDate + "';";
				SqlDataReader RS = DB.SqlCmd.ExecuteReader();
				while (RS.Read()) {
					TBBPk.Add(RS[0] + "");
				}
				RS.Dispose();

				StringBuilder TempResult = new StringBuilder();
				foreach (string pk in TBBPk) {
					TempResult.Append(LoadDescription_TodayAbji(pk, ref Title) + LoadAbjiList_TodayAbji(pk) + "<br />");
					//TempResult.Append(LoadDescription(pk, ref Title) + LoadAbjiList(pk) + "<br />");
				}
				Title = arrivalDate;
				result = TempResult + "";
				break;

			case "TodayAbjiForClearance":
				branchPk = Request.Params["S"] + "";
				arrivalDate = Request.Params["D"] + "";
				InorOut = Request.Params["G2"] + "";
				TBBPk = new List<string>();
				InorOutDate = InorOut == "in" ? "[DATETIME_TO]" : "[DATETIME_FROM]";
				InorOutBranch = InorOut == "in" ? "[BRANCHPK_TO]" : "[BRANCHPK_FROM]";

				DB.SqlCmd.CommandText = "SELECT [TRANSPORT_PK] FROM [dbo].[TRANSPORT_HEAD] WHERE " + InorOutBranch + "=" + branchPk + " and left(" + InorOutDate + ", 8)='" + arrivalDate + "';";

				RS = DB.SqlCmd.ExecuteReader();
				while (RS.Read()) {
					TBBPk.Add(RS[0] + "");
				}
				RS.Dispose();

				TempResult = new StringBuilder();
				TempResult.Append(@"<table border='1'><tr><td>입항일</td><td>BLNo</td><td>고객번호</td><td>상호</td><td>포장수량</td><td>관세</td><td>부가세</td><td>통관료</td><td>기타</td><td>기타</td><td>계</td></tr>");
				foreach (string pk in TBBPk) {
					TempResult.Append(LoadAbjiListForClearance(pk));
				}
				TempResult.Append("</table>");
				Title = arrivalDate;
				result = TempResult + "";
				break;

			case "TodayAbjiForJaemu":
				branchPk = Request.Params["S"] + "";
				arrivalDate = Request.Params["D"] + "";
				InorOut = Request.Params["G2"] + "";

				TBBPk = new List<string>();
				InorOutDate = InorOut == "in" ? "[DATETIME_TO]" : "[DATETIME_FROM]";
				InorOutBranch = InorOut == "in" ? "[BRANCHPK_TO]" : "[BRANCHPK_FROM]";

				DB.SqlCmd.CommandText = "SELECT [TRANSPORT_PK] FROM [dbo].[TRANSPORT_HEAD] WHERE " + InorOutBranch + "=" + branchPk + " and left(" + InorOutDate + ", " + arrivalDate.Length + ")='" + arrivalDate + "';";

				RS = DB.SqlCmd.ExecuteReader();
				while (RS.Read()) {
					TBBPk.Add(RS[0] + "");
				}
				RS.Dispose();

				TempResult = new StringBuilder();
				TempResult.Append(@"<table border='1'><tr><td>년도</td><td>월</td><td>일</td><td>매입매출구분(1-매출/2-매입)</td><td>과세유형</td><td>불공제사유</td><td>신용카드거래처코드</td><td>신용카드사명</td><td>신용카드(가맹점)번호</td><td>거래처명</td><td>사업자(주민)등록번호</td><td> 공급가액 </td><td> 부가세 </td><td>품명</td><td>전자세금(1.전자)</td><td>기본계정</td><td>상대계정</td><td>현금영수증 승인번호</td>
<!--td>발화인 입금일</td>
<td>발화인 입금액</td>
<td>발화인 차액</td>
<td>수하인 입금일</td>
<td>수하인 입금액</td>
<td>수하인 차액</td>
<td>出</td-->
</tr>");
				foreach (string pk in TBBPk) {
					if (arrivalDate.Length > 3 && Int32.Parse(arrivalDate.Substring(0, 4)) > 2014) {
						TempResult.Append(LoadAbjiListForJaemu2015(pk, Request.Params["G3"] + ""));
					} else {
						TempResult.Append(LoadAbjiListForJaemu(pk, Request.Params["G3"] + ""));
						//TempResult.Append(LoadAbjiListForJaemu2015(pk, Request.Params["G3"] + ""));
					}
				}
				TempResult.Append("</table>");
				Title = arrivalDate;
				result = TempResult + "";

				break;

			case "Abji":
				result = LoadDescription(Request.Params["S"], ref Title) + LoadAbjiList(Request.Params["S"]);
				break;

			case "JukWha":
				result = LoadDescription(Request.Params["S"], ref Title) + LoadJukhwaList(Request.Params["S"]);
				break;

			case "JukWhaSend":
				result = LoadDescription(Request.Params["S"], ref Title) + LoadJukhwaList2(Request.Params["S"]);
				break;

			case "JukWha2":
				result = LoadDescriptionJukhwa(Request.Params["S"]);
				Title = "JukWha";
				break;
			case "StorcedList":
				result = LoadStorcedList(Request.Params["S"]);
				Title = "StorageList";
				break;

			case "InvoiceItem":
				result = LoadInvoiceItem(Request.Params["S"]);
				Title = Request.Params["G"];
				break;

			case "CompanyInfo":
				Title = "aaaaaaaaaaaaaaaaaaaaaaa";
				result = LoadCompanyInformation();
				break;

			case "Chulgo":
				Title = "ListDown";
				result = LoadChulgo(Request.Params["S"], Request.Params["P"]);
				break;

			case "OKChulgo":
				Title = "ListDown";
				result = LoadOKChulgo(Request.Params["S"], Request.Params["P"]);
				break;

			case "TariffList":
				result = LoadTariffList(Request.Params["S"], ref Title);
				break;

			case "CompanyRequestList":
				Title = "RequestList";
				result = LoadCustomerRequestList(Request.Params["S"]);
				break;

			case "MemberRequestList":
				Title = "RequestList";
				result = LoadMemberRequestList(Request.Params["S"]);
				break;

			case "YT_Clearance":
				//string BBHPk = Request.Params["S"];
				result = LoadDescription_YTClearance(Request.Params["S"], ref Title);
				//result = LoadDescription(Request.Params["S"], ref Title)+ LoadPackedList(Request.Params["S"]);
				break;

			case "ClearanceEndList":
				string date = Request.Params["D"] + "";
				result = MakeHtmlClearanceEnd(date);
				Title = "ClearanceList " + date;
				break;

			default:
				//string BBHPk = Request.Params["S"];
				result = LoadDescription(Request.Params["S"], ref Title) + LoadPackedList(Request.Params["S"]);
				//result = LoadDescription(Request.Params["S"], ref Title)+ LoadPackedList(Request.Params["S"]);
				break;
		}
		DB.DBCon.Close();

		System.Web.HttpResponse objResponse = System.Web.HttpContext.Current.Response;

		objResponse.ClearContent();
		objResponse.ClearHeaders();

		objResponse.Write("<meta http-equiv=Content-Type content=''text/html; charset=utf-8''>");
		objResponse.Charset = "UTF-8";
		objResponse.ContentType = "application/xls";
		objResponse.AddHeader("content-disposition", "attachment;filename=" + Title.Trim() + "_.xls");


		objResponse.Write(result + "");
		objResponse.Flush();
		objResponse.Close();
		objResponse.End();

	}

	private string MakeHtmlClearanceEnd(string Date) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<table border='0' cellpadding='0' cellspacing='0' style='width:850px; margin:0 auto; ' ><thead><tr style=\"height:30px;\">" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:50px;' >No</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; width:130px;  \">신고번호</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; \" >수입자</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; \">BL번호</td>" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:80px;'>관세</td>" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:80px;'>부가세</td>" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:90px;' >합계</td>" +
										"		</tr></thead>");

		DBConn DB = new DBConn();
		DB.DBCon.Open();
		string BLNo = "";

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

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		BLNo = "";
		int RowNo = 0;
		decimal totalT1 = 0;
		decimal totalT2 = 0;
		decimal totalT3 = 0;
		while (RS.Read()) {
			if (RS["BLNo"] + "" == BLNo) {
				continue;
			}
			RowNo++;
			BLNo = RS["BLNo"] + "";
			ReturnValue.Append("<tr style='height:30px; '>" +
				"<td class='TBody1' >" + RowNo + "</td>" +
				"<td class='TBody1' >" + RS["ClearanceNo"] + "</td>" +
				"<td class='TBody1' ><strong>" + RS["ConsigneeCompanyName"] + "</strong></td>" +
				"<td class='TBody1' >" + RS["BLNo"] + "</td>" +
				"<td class='TBody1' style='text-align:right; '>" + Common.NumberFormat(RS["T1V"] + "") + "</td>" +
				"<td class='TBody1' style='text-align:right; '>" + Common.NumberFormat(RS["T2V"] + "") + "</td>" +
				"<td class='TBody1' style='text-align:right; ' >" + Common.NumberFormat(RS["TariffSum"] + "") + "</td></tr>");
			if (RS["T1V"] + "" != "") {
				totalT1 += decimal.Parse(RS["T1V"] + "");
			}
			if (RS["T2V"] + "" != "") {
				totalT2 += decimal.Parse(RS["T2V"] + "");
			}
			if (RS["TariffSum"] + "" != "") {
				totalT3 += decimal.Parse(RS["TariffSum"] + "");
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		ReturnValue.Append("<tr style='height:30px; '>" +
			"<td class='TBody1' >&nbsp;</td>" +
			"<td class='TBody1' >&nbsp;</td>" +
			"<td class='TBody1' >&nbsp;</td>" +
			"<td class='TBody1' >&nbsp;</td>" +
			"<td class='TBody1' style='text-align:right; font-weight:bold;'>" + Common.NumberFormat(totalT1.ToString()) + "</td>" +
			"<td class='TBody1' style='text-align:right; font-weight:bold;'>" + Common.NumberFormat(totalT2.ToString()) + "</td>" +
			"<td class='TBody1' style='text-align:right; font-weight:bold;' >" + Common.NumberFormat(totalT3.ToString()) + "</td></tr></table>");

		return ReturnValue + "";
	}

	private String LoadTariffList(string listpk, ref string Title) {
		Title = listpk;
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<div style='text-align:right; padding-right:10px; '><input type='button' value='Excel 다운로드' onclick='DownExcel();' /></div>	<table border='0' cellpadding='0' cellspacing='0' style='width:850px; margin:0 auto; ' ><thead><tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; width:60px;  \">Status</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:140px; \" >House BL</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; \">Customer</td>" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:110px;'>count</td>" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:70px;'>관세</td>" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:70px;'>부가세</td>" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:70px;'>관세사비</td>" +
										"		<td class='THead1' style='background-color:#E8E8E8; width:80px;' >합계</td>" +
										"		</tr></thead>");
		string BodyCommercial = "<tr style=\"height:20px;\">" +
											"		<td class='TBody1' style=\"text-align:center; \">{2}</td>" +
											"		<td class='TBody1' style=\"text-align:center; font-weight:bold;\" ><a onclick=\"Goto('CheckDescription','{0}');\">{3}</a></td>" +
											"		<td class='TBody1' ><a onclick=\"Goto('Company', '{1}')\" ><strong>{4}</strong> {5}</a></td>" +
											"		<td class='TBody1' >{6} ({7})</td>" +
											"		<td class='TBody1' style='text-align:right;' >{8}</td>" +
											"		<td class='TBody1' style='text-align:right;' >{9}</td>" +
											"		<td class='TBody1' style='text-align:right;' >{10}</td>" +
											"		<td class='TBody1' style='text-align:right;' >{11}</td>" +
											"		</tr>";
		string BodyRequest = "<tr style=\"height:20px;\">" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >└---------></td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' ><a onclick=\"Goto('Request', '{5}')\" >{1} ({3})</a></td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"	</tr>";
		DB.SqlCmd.CommandText = @"
SELECT
	RF.RequestFormPk, RF.ConsigneePk, RF.ConsigneeCode, RF.ArrivalDate, RF.DocumentStepCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.ContainerNo
	, CC.CompanyName AS ConsigneeCompanyName
	, Departure.NameE
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
	, Tariff.TSum
    , T1.Value as T1V, T2.Value as T2V, T3.Value as T3V
FROM RequestForm AS RF
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RFCH on RF.RequestFormPk=RFCH.[TABLE_PK] 
	inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk
	left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
	left join Company AS CC on RF.ConsigneePk=CC.CompanyPk
	Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode
	Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode
	left join (
			SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='제세금' group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff.[REQUESTFORMCALCULATE_HEAD_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As Value FROM [dbo].[REQUESTFORMCALCULATE_BODY] where Title in ('관세') group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS T1 ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=T1.[REQUESTFORMCALCULATE_HEAD_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As Value FROM [dbo].[REQUESTFORMCALCULATE_BODY] where Title in ('부가세') group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS T2 ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=T2.[REQUESTFORMCALCULATE_HEAD_PK]
    left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As Value FROM [dbo].[REQUESTFORMCALCULATE_BODY] where Title in ('관세사비') group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS T3 ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=T3.[REQUESTFORMCALCULATE_HEAD_PK]
WHERE CCWR.TariffCharge=" + listpk + @"
AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
Order by CD.ContainerNo, CD.BLNo ASC, CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC; ";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string TempCommercialDocumentHeadPk = "";
		string TempRequestFormPk = "";
		int InputTVCount = 0;
		decimal[] Sum = new decimal[] { 0, 0, 0 };

		while (RS.Read()) {
			if (TempCommercialDocumentHeadPk != RS["CommercialDocumentHeadPk"] + "") {
				string tempDocumentStep;
				switch (RS["DocumentStepCL"] + "") {
					case "1":
						tempDocumentStep = "미전송";
						break;

					case "2":
						tempDocumentStep = "<span style=\"color:blue; \">전송완</span>";
						break;

					case "3":
						tempDocumentStep = "자통";
						break;

					case "4":
						tempDocumentStep = "샘플";
						break;

					case "5":
						tempDocumentStep = "<span style=\"color:green; \">정산완료</span>";
						break;

					case "6":
						tempDocumentStep = "통관지시";
						break;

					case "7":
						tempDocumentStep = "생략";
						break;

					case "8":
						tempDocumentStep = "서류제출";
						break;

					case "9":
						tempDocumentStep = "검사";
						break;

					case "10":
						tempDocumentStep = "세금납부";
						break;

					case "11":
						tempDocumentStep = "세금납부";
						break;

					case "12":
						tempDocumentStep = "세금납부";
						break;

					case "13":
						tempDocumentStep = "면허완료";
						break;

					case "14":
						tempDocumentStep = "면허완료";
						break;

					case "15":
						tempDocumentStep = "면허완료";
						break;

					default:
						tempDocumentStep = "?";
						break;
				}
				TempCommercialDocumentHeadPk = RS["CommercialDocumentHeadPk"] + "";
				TempRequestFormPk = RS["RequestFormPk"] + "";
				string[] StringFormatValue = new string[12];
				StringFormatValue[0] = RS["CommercialDocumentHeadPk"] + "";
				StringFormatValue[1] = RS["ConsigneePk"] + "";
				StringFormatValue[2] = tempDocumentStep;
				//StringFormatValue[3] = RS["ContainerNo"] + "" == "" ? "&nbsp;" : RS["ContainerNo"] + "";
				StringFormatValue[3] = RS["BLNo"] + "";
				StringFormatValue[4] = RS["ConsigneeCode"] + "";
				StringFormatValue[5] = RS["ConsigneeCompanyName"] + "";
				StringFormatValue[6] = (RS["ArrivalDate"] + "").Substring(2);
				StringFormatValue[7] = RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
				StringFormatValue[8] = Common.NumberFormat(RS["T1V"] + "");
				StringFormatValue[9] = Common.NumberFormat(RS["T2V"] + "");
				StringFormatValue[10] = Common.NumberFormat(RS["T3V"] + "");

				Decimal TempDecimal = 0;

				InputTVCount++;
				if (RS["T1V"] + "" != "") {
					Sum[0] += decimal.Parse(RS["T1V"] + "");
					TempDecimal += decimal.Parse(RS["T1V"] + "");
				}
				if (RS["T2V"] + "" != "") {
					Sum[1] += decimal.Parse(RS["T2V"] + "");
					TempDecimal += decimal.Parse(RS["T2V"] + "");
				}
				if (RS["T3V"] + "" != "") {
					Sum[2] += decimal.Parse(RS["T3V"] + "");
					TempDecimal += decimal.Parse(RS["T3V"] + "");
				}
				StringFormatValue[11] = Common.NumberFormat(TempDecimal + "");

				ReturnValue.Append(String.Format(BodyCommercial, StringFormatValue));
			} else {
				if (TempRequestFormPk == RS["RequestFormPk"] + "") {
					continue;
				}
				ReturnValue.Append(String.Format(BodyRequest,
													RS["NameE"] + "",
													(RS["ArrivalDate"] + "").Substring(2),
													RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + ""),
													Common.NumberFormat(RS["TotalGrossWeight"] + ""),
													Common.NumberFormat(RS["TotalVolume"] + ""),
													RS["RequestFormPk"] + ""));
			}
		}
		RS.Dispose();
		return ReturnValue + "<tr style=\"height:30px;\">" +
											"		<td class='THead1' style=\"text-align:center; \" colspan='3' >&nbsp;</td>" +
											"		<td class='THead1' >" + InputTVCount + "건</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Sum[0].ToString()) + "</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Sum[1].ToString()) + "</td>" +
											"		<td class='THead1' >" + Common.NumberFormat(Sum[2].ToString()) + "</td>" +
											"		<td class='THead1' >" + Common.NumberFormat((Sum[0] + Sum[1] + Sum[2]).ToString()) + "</td>" +
											"		</tr></table>";
	}

	private String LoadOKChulgo(string Type, string BranchPk) {
		DB.SqlCmd.CommandText = @"
DECLARE @BranchCode int;
SET @BranchCode=" + BranchPk + @";
SELECT
	OBS.BoxCount, OBSC.StorageName
	, Consignee.CompanyName AS ConsigneeName
	, R.ConsigneeCode
	, TBC.Type, TBC.Title, TBC.DriverName, TBC.DriverTEL, TBC.[TEL], TBC.[CarSize], TBC.FromDate, TBC.[WarehouseInfo]
	, TBC.[PackedCount], TBC.[PackingUnit], TBC.[DepositWhere], TBC.[Price], TBC.[Memo], TBC.DeliveryPrice TBCDeliveryPrice
    , TBCHistory.Name TBCHistoryName
    ,R.TotalGrossWeight,R.TotalVolume
FROM OurBranchStorageOut AS OBS
	left join OurBranchStorageCode AS OBSC ON OBS.StorageCode=OBSC.OurBranchStoragePk
	left join RequestForm AS R ON OBS.RequestFormPk=R.RequestFormPk
	left join Company AS Consignee ON Consignee.CompanyPk=R.ConsigneePk
	left join TransportBC AS TBC ON TBC.TransportBetweenCompanyPk=OBS.TransportBetweenCompanyPk
    left join (SELECT t.TransportBetweenCompanyPk,a.Name,t.Registerd FROM [dbo].[TransportBCHistory] t left join Account_ a on t.ActID=a.AccountID where t.GubunCL=0) TBCHistory on OBS.TransportBetweenCompanyPk =TBCHistory.TransportBetweenCompanyPk
WHERE OBSC.OurBranchCode=@BranchCode and isnull(TBC.Type, '')='" + Type + @"'
  and OBS.BoxCount>0
  and (R.DocumentStepCL in (5, 6, 7, 8, 9, 10, 11, 12)
  or isnull(OBS.StatusCL, 0)<6 )
ORDER BY  TBCHistory.Registerd desc, StaTusCL ASC; ";
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" >" +
			"<tr>" +
				"<td>고객코드</td>" +
				"<td>업체명</td>" +
				"<td>주소</td>" +
				"<td>담당자</td>" +
				"<td>담당자 전화번호</td>" +
				"<td>창고</td>" +
				"<td>수량</td>" +
				"<td>중량</td>" +
				"<td>체적</td>" +
				"<td>출고일</td>" +
				"<td>구분</td>" +
				"<td>규격</td>" +
				"<td>기사명</td>" +
				"<td>기사전화번호</td>" +
				"<td>화주지불</td>" +
				"<td>화주금액</td>" +
				"<td>IL금액</td>" +
				"<td>IL담당자</td>" +
				"<td>메모</td>" +
			"</tr>");
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string[] consigneeData = RS["WarehouseInfo"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
			ReturnValue.Append("<tr>" +
				"<td>" + RS["ConsigneeCode"] + "</td>" +
				"<td>" + RS["ConsigneeName"] + "</td>" +
				"<td>" + consigneeData[0] + "</td>" +
				"<td>" + consigneeData[2] + "</td>" +
				"<td>" + consigneeData[1] + "</td>" +
				"<td>" + RS["StorageName"] + "</td>" +
				"<td>" + RS["PackedCount"] + " / " + RS["BoxCount"] + " " + Common.GetPackingUnit(RS["PackingUnit"].ToString()) + "</td>" +
				"<td>" + Common.NumberFormat(RS["TotalGrossWeight"].ToString()) + "KG" + "</td>" +
				"<td>" + Common.NumberFormat(RS["TotalVolume"].ToString()) + "CBM" + "</td>" +
				"<td>" + RS["FromDate"] + "</td>" +
				"<td>" + RS["Title"] + "</td>" +
				"<td>" + RS["CarSize"] + "</td>" +
				"<td>" + RS["DriverName"] + "</td>" +
				"<td>" + RS["DriverTEL"] + "</td>" +
				"<td>" + (RS["DepositWhere"] + "" == "1" ? "착불" : "현불") + "</td>" +
				"<td>" + RS["Price"] + "</td>" +
				"<td>" + RS["TBCDeliveryPrice"] + "</td>" +
				"<td>" + RS["TBCHistoryName"] + "</td>" +
				"<td>" + RS["Memo"] + "</td>" +
			"</tr>");
		}
		RS.Dispose();
		return ReturnValue + "";
	}

	private String LoadChulgo(string Type, string BranchPk) {
		DB.SqlCmd.CommandText = @"
DECLARE @BranchCode int;
SET @BranchCode=" + BranchPk + @";
SELECT
	OBS.BoxCount, OBSC.StorageName
	, Consignee.CompanyName AS ConsigneeName
	, R.ConsigneeCode
	, TBC.Type, TBC.Title, TBC.DriverName, TBC.DriverTEL, TBC.[TEL], TBC.[CarSize], TBC.FromDate, TBC.[WarehouseInfo]
	, TBC.[PackedCount], TBC.[PackingUnit], TBC.[DepositWhere], TBC.[Price], TBC.[Memo]
    ,R.TotalGrossWeight,R.TotalVolume
FROM OurBranchStorageOut AS OBS
	left join OurBranchStorageCode AS OBSC ON OBS.StorageCode=OBSC.OurBranchStoragePk
	left join RequestForm AS R ON OBS.RequestFormPk=R.RequestFormPk
	left join Company AS Consignee ON Consignee.CompanyPk=R.ConsigneePk
	left join TransportBC AS TBC ON TBC.TransportBetweenCompanyPk=OBS.TransportBetweenCompanyPk
WHERE OBSC.OurBranchCode=@BranchCode and isnull(TBC.Type, '')='" + Type + @"' and OBS.BoxCount>0 and ( R.DocumentStepCL in (5, 6, 7, 8, 9, 10, 11, 12) or isnull(OBS.StatusCL, 0)<6 )
ORDER BY TBC.Type ASC, TBC.Title ASC, Consignee.CompanyPk, StaTusCL ASC; ";
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" >" +
			"<tr>" +
				"<td>고객코드</td>" +
				"<td>업체명</td>" +
				"<td>주소</td>" +
				"<td>담당자</td>" +
				"<td>담당자 전화번호</td>" +
				"<td>창고</td>" +
				"<td>수량</td>" +
				"<td>중량</td>" +
				"<td>체적</td>" +
				"<td>출고일</td>" +
				"<td>구분</td>" +
				"<td>규격</td>" +
				"<td>기사명</td>" +
				"<td>기사전화번호</td>" +
				"<td>지불</td>" +
				"<td>금액</td>" +
				"<td>메모</td>" +
			"</tr>");
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string[] consigneeData = RS["WarehouseInfo"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
			ReturnValue.Append("<tr>" +
				"<td>" + RS["ConsigneeCode"] + "</td>" +
				"<td>" + RS["ConsigneeName"] + "</td>" +
				"<td>" + consigneeData[0] + "</td>" +
				"<td>" + consigneeData[2] + "</td>" +
				"<td>" + consigneeData[1] + "</td>" +
				"<td>" + RS["StorageName"] + "</td>" +
				"<td>" + RS["PackedCount"] + " / " + RS["BoxCount"] + " " + Common.GetPackingUnit(RS["PackingUnit"].ToString()) + "</td>" +
				"<td>" + Common.NumberFormat(RS["TotalGrossWeight"].ToString()) + "KG" + "</td>" +
				"<td>" + Common.NumberFormat(RS["TotalVolume"].ToString()) + "CBM" + "</td>" +
				"<td>" + RS["FromDate"] + "</td>" +
				"<td>" + RS["Title"] + "</td>" +
				"<td>" + RS["CarSize"] + "</td>" +
				"<td>" + RS["DriverName"] + "</td>" +
				"<td>" + RS["DriverTEL"] + "</td>" +
				"<td>" + (RS["DepositWhere"] + "" == "1" ? "착불" : "현불") + "</td>" +
				"<td>" + RS["Price"] + "</td>" +
				"<td>" + RS["Memo"] + "</td>" +
			"</tr>");
		}
		RS.Dispose();
		return ReturnValue + "";
	}

	private String LoadSimpleMisu(string Date, string Type, ref string Title) {

		string SQueryWhere = "";
		string CQueryWhere = "";
		string DQueryWhere = "";
		string DQueryWhere1 = "";
		if (Date == "Now") {
			DQueryWhere = " and R.ArrivalDate>'20141231'";
			DQueryWhere1 = " and R.ArrivalDate>'20141231'";
		} else {
			DQueryWhere = " and R.ArrivalDate>'20130000' and R.ArrivalDate<'20141232'";
			DQueryWhere1 = " and R.ArrivalDate>'20110700' and R.ArrivalDate<'20141232'";
		}
		switch (Type) {
			case "03":
				Title = "03MISU";
				SQueryWhere = "and (right(R.Shippercode,1) in ('0','1','2') OR  right(R.Shippercode,2) in ('0A','0B','1C','0D','0E','0F','0G','0H','0I','0J','1A','1B','1C','1D','1E','1F','1G','1H','1I','1J', '2A','2B','2C','2D','2E','2F','2G','2H','2I','2J')  )";
				CQueryWhere = "and (right(R.Consigneecode,1) in ('0','1','2') OR  right(R.Consigneecode,2) in ('0A','0B','1C','0D','0E','0F','0G','0H','0I','0J','1A','1B','1C','1D','1E','1F','1G','1H','1I','1J', '2A','2B','2C','2D','2E','2F','2G','2H','2I','2J')  )";
				break;
			case "46":
				Title = "46MISU";
				SQueryWhere = "and (right(R.Shippercode,1) in ('4','5','6', '3') OR  right(R.Shippercode,2) in ('4A','4B','4C','4D','4E','4F','4G','4H','4I','4J','5A','5B','5C','5D','5E','5F','5G','5H','5I','5J', '6A','6B','6C','6D','6E','6F','6G','6H','6I','6J', '3A','3B','3C','3D','3E','3F','3G','3H','3I','3J')  )";
				CQueryWhere = "and (right(R.Consigneecode,1) in ('4','5','6', '3') OR  right(R.Consigneecode,2) in ('4A','4B','4C','4D','4E','4F','4G','4H','4I','4J','5A','5B','5C','5D','5E','5F','5G','5H','5I','5J', '6A','6B','6C','6D','6E','6F','6G','6H','6I','6J', '3A','3B','3C','3D','3E','3F','3G','3H','3I','3J')  )";
				break;
			case "79":
				Title = "79MISU";
				SQueryWhere = "and (right(R.Shippercode,1) in ('7','8','9') OR  right(R.Shippercode,2) in ('7A','7B','7C','7D','7E','7F','7G','7H','7I','7J','8A','8B','8C','8D','8E','8F','8G','8H','8I','8J', '9A','9B','9C','9D','9E','9F','9G','9H','9I','9J')  )";
				CQueryWhere = "and (right(R.Consigneecode,1) in ('7','8','9') OR  right(R.Consigneecode,2) in ('7A','7B','7C','7D','7E','7F','7G','7H','7I','7J','8A','8B','8C','8D','8E','8F','8G','8H','8I','8J', '9A','9B','9C','9D','9E','9F','9G','9H','9I','9J')  )";
				break;
		}
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<table border='0' cellpadding='0' cellspacing='0' style='width:650px;' >" +
		  "<tr>" +
		  "	<td>Arrival & Box</td>" +
		  "	<td>Company</td>" +

		  "	<td>물류비</td>" +
		  "	<td>관세</td>" +
		  "	<td>부가세</td>" +
		"	<td>관세사비</td>" +
		"	<td>배송비</td>" +

		"	<td>&nbsp;</td>" +
		"	<td>Charge</td>" +
		"	<td>미수금</td>" +
		  "	</tr>");

		string Row = "<tr>" +
			  "<td>{2}</td>" +
			  "<td>{3}</td>" +

			  "<td>{7}</td>" +
			  "<td>{8}</td>" +
			  "<td>{9}</td>" +
			  "<td>{10}</td>" +
			  "<td>{11}</td>" +

			  "<td>{4}</td>" +
			  "<td>{5}</td>" +
			  "<td>{6}</td>" +
			  "</tr>";

		DB.SqlCmd.CommandText = @"
SELECT
	'Out' AS Type,
	R.RequestFormPk, R.ShipperPk AS CompanyPk, R.ShipperCode AS CompanyCode, R.DepartureDate, R.ArrivalDate
	, C.CompanyName AS CompanyName
	, R.TotalPackedCount, R.PackingUnit
	, R.ExchangeDate
	, RFCH.[MONETARY_UNIT] AS MonetaryUnit
	, ISNULL(RFCH.[TOTAL_PRICE], 0) AS Charge
	, RFCH.[DEPOSITED_PRICE] AS Deposited
   ,ISNULL(RFCH.[TOTAL_PRICE], 0) - ISNULL(CDelivery.Price, 0) - ISNULL(Tariff.TSum, 0) AS Foward
   ,ISNULL(CDelivery.Price, 0) AS DeliveryPrice
   ,Tariff.TSum AS TariffSum
   ,Tariff8.TSum8
   ,Tariff9.TSum9
   ,Tariff10.TSum10
FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH
	left join RequestForm AS R ON RFCH.[TABLE_PK] = R.RequestFormPk
	left join Company AS C ON R.ShipperPk=C.CompanyPk
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='제세금' group by [REQUESTFORMCALCULATE_HEAD_PK]
	) AS Tariff ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff.[REQUESTFORMCALCULATE_HEAD_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) AS Price 
		FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='대행비' group by [REQUESTFORMCALCULATE_HEAD_PK]
	) AS CDelivery ON CDelivery.[REQUESTFORMCALCULATE_HEAD_PK]=RFCH.[REQUESTFORMCALCULATE_HEAD_PK]
	left join RegionCode AS DRC ON R.DepartureRegionCode=DRC.RegionCode
left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum8 FROM [dbo].[REQUESTFORMCALCULATE_BODY] where Title in ('관세') group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff8 ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff8.[REQUESTFORMCALCULATE_HEAD_PK]
left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum9 FROM [dbo].[REQUESTFORMCALCULATE_BODY] where Title in ('부가세') group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff9 ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff9.[REQUESTFORMCALCULATE_HEAD_PK]
    left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum10 FROM [dbo].[REQUESTFORMCALCULATE_BODY] where Title in ('관세사비') group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff10 ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff10.[REQUESTFORMCALCULATE_HEAD_PK]
WHERE 1=1 
	AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
   and DRC.OurBranchCode=3157
   and ISNULL([TOTAL_PRICE], 0)<>isnull([DEPOSITED_PRICE], 0)
   and R.StepCL not in (33) " + SQueryWhere + DQueryWhere1 + @"
order by ArrivalDate desc";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			String[] RowData = new String[12];
			RowData[0] = RS["RequestFormPk"] + "";
			RowData[1] = RS["CompanyPk"] + "";
			RowData[2] = (RS["ArrivalDate"] + "" == "" ? "" : (RS["ArrivalDate"] + "").Substring(2)) + " : " + RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
			if (RS["Type"] + "" == "Out") {
				RowData[3] = "<span style=\"color:red; font-weight:bold;\">出</span> <strong>" + RS["CompanyCode"] + "</strong> " + RS["CompanyName"];
			} else {
				RowData[3] = "<span style=\"color:blue; font-weight:bold;\">入</span> <strong>" + RS["CompanyCode"] + "</strong> " + RS["CompanyName"];
			}
			decimal tempTariffS = 0;
			if (RS["MonetaryUnit"] + "" != "20") {
				string tempExchangedDate = "";
				string ExchangeRate = RS["ExchangeDate"] + "";
				if (ExchangeRate != "") {
					tempExchangedDate = ExchangeRate;
				}
				string temp = "";
				tempTariffS = new Admin().GetExchangeRated("20", RS["MonetaryUnit"] + "", decimal.Parse(RS["TariffSum"] + ""), out temp, tempExchangedDate);            //Response.Write(tempTariffS);

				switch (RS["MonetaryUnit"] + "") {
					case "18":
						tempTariffS = Math.Round(tempTariffS, 1, MidpointRounding.AwayFromZero);
						break;

					case "19":
						tempTariffS = Math.Round(tempTariffS, 2, MidpointRounding.AwayFromZero);
						break;

					case "20":
						tempTariffS = Math.Round(tempTariffS, 0, MidpointRounding.AwayFromZero);
						break;
				}
			}

			RowData[4] = Common.GetMonetaryUnit(RS["MonetaryUnit"] + "");
			RowData[5] = Common.NumberFormat((decimal.Parse(RS["Charge"] + "") + tempTariffS).ToString());
			decimal d_Charge = decimal.Parse(RS["Charge"] + "" == "" ? "0" : RS["Charge"] + "") + tempTariffS;

			string Deposited = RS["Deposited"] + "";
			if (Deposited == "") {
				Deposited = "0";
			}
			decimal d_Deposited = decimal.Parse(Deposited);

			if (Deposited == "") {
				RowData[6] = "<span style='color:green;'>--</span>";
			} else {
				decimal calc = d_Deposited - d_Charge;
				if (calc == 0) {
					continue;
				}
				if (calc > 0) {
					RowData[6] = "<span style='color:blue;'>" + Common.NumberFormat(calc.ToString()) + "</span>";
				} else {
					RowData[6] = "<span style='color:red;'>" + Common.NumberFormat(calc.ToString()) + "</span>";
				}
			}
			RowData[7] = Common.NumberFormat(RS["Foward"].ToString() == "" ? "0" : RS["Foward"].ToString());
			RowData[8] = Common.NumberFormat(RS["TSum8"].ToString() == "" ? "0" : RS["TSum8"].ToString());
			RowData[9] = Common.NumberFormat(RS["TSum9"].ToString() == "" ? "0" : RS["TSum9"].ToString());
			RowData[10] = Common.NumberFormat(RS["TSum10"].ToString() == "" ? "0" : RS["TSum10"].ToString());
			RowData[11] = Common.NumberFormat(RS["DeliveryPrice"].ToString() == "" ? "0" : RS["DeliveryPrice"].ToString());

			ReturnValue.Append(String.Format(Row, RowData));
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"
SELECT
	'Out' AS Type,
	R.RequestFormPk, R.ConsigneePk AS CompanyPk, R.ConsigneeCode AS CompanyCode, R.DepartureDate, R.ArrivalDate
	, C.CompanyName AS CompanyName
	, R.TotalPackedCount, R.PackingUnit
	, RFCH.[MONETARY_UNIT] AS MonetaryUnit
	, ISNULL(RFCH.[TOTAL_PRICE], 0) AS Charge
	, RFCH.[DEPOSITED_PRICE] AS Deposited
   ,ISNULL(RFCH.[TOTAL_PRICE], 0) - ISNULL(CDelivery.Price, 0) - ISNULL(Tariff.TSum, 0) AS Foward
   ,ISNULL(CDelivery.Price, 0) AS DeliveryPrice
   ,Tariff8.TSum8
   ,Tariff9.TSum9
   ,Tariff10.TSum10
FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH
	left join RequestForm AS R ON RFCH.[TABLE_PK]=R.RequestFormPk
	left join Company AS C ON R.ShipperPk=C.CompanyPk
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='제세금' group by [REQUESTFORMCALCULATE_HEAD_PK]
	) AS Tariff ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff.[REQUESTFORMCALCULATE_HEAD_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) AS Price 
		FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='대행비' group by [REQUESTFORMCALCULATE_HEAD_PK]
	) AS CDelivery ON CDelivery.[REQUESTFORMCALCULATE_HEAD_PK]=RFCH.[REQUESTFORMCALCULATE_HEAD_PK]
	left join RegionCode AS DRC ON R.DepartureRegionCode=DRC.RegionCode
left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum8 FROM [dbo].[REQUESTFORMCALCULATE_BODY] where Title in ('관세') group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff8 ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff8.[REQUESTFORMCALCULATE_HEAD_PK]
left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum9 FROM [dbo].[REQUESTFORMCALCULATE_BODY] where Title in ('부가세') group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff9 ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff9.[REQUESTFORMCALCULATE_HEAD_PK]
    left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum10 FROM [dbo].[REQUESTFORMCALCULATE_BODY] where Title in ('관세사비') group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff10 ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff10.[REQUESTFORMCALCULATE_HEAD_PK]
WHERE
	1=1 
	and DRC.OurBranchCode=3157
	AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
   and ISNULL([TOTAL_PRICE], 0)<>isnull([DEPOSITED_PRICE], 0)
	and R.StepCL not in (33)
	and R.DocumentStepCL in (0,3,4, 10, 11, 12, 13, 14, 15) " + SQueryWhere + DQueryWhere + @"
UNION ALL

SELECT
	'In' AS Type,
	R.RequestFormPk, R.ConsigneePk AS CompanyPk, R.ConsigneeCode AS CompanyCode, R.DepartureDate, R.ArrivalDate
	, C.CompanyName AS CompanyName
	, R.TotalPackedCount, R.PackingUnit
	, RFCH.[MONETARY_UNIT] AS MonetaryUnit
	, ISNULL(RFCH.[TOTAL_PRICE], 0) AS Charge
	, RFCH.[DEPOSITED_PRICE] AS Deposited
   ,ISNULL(RFCH.[TOTAL_PRICE], 0) - ISNULL(CDelivery.Price, 0) - ISNULL(Tariff.TSum, 0) AS Foward
   ,ISNULL(CDelivery.Price, 0) AS DeliveryPrice
   ,Tariff8.TSum8
   ,Tariff9.TSum9
   ,Tariff10.TSum10
FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH
	left join RequestForm AS R ON RFCH.[TABLE_PK]=R.RequestFormPk
	left join Company AS C ON R.ConsigneePk=C.CompanyPk
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='제세금' group by [REQUESTFORMCALCULATE_HEAD_PK]
	) AS Tariff ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff.[REQUESTFORMCALCULATE_HEAD_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) AS Price 
		FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='대행비' group by [REQUESTFORMCALCULATE_HEAD_PK]
	) AS CDelivery ON CDelivery.[REQUESTFORMCALCULATE_HEAD_PK]=RFCH.[REQUESTFORMCALCULATE_HEAD_PK]
	left join RegionCode AS ARC ON R.ArrivalRegionCode=ARC.RegionCode
left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum8 FROM [dbo].[REQUESTFORMCALCULATE_BODY] where Title in ('관세') group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff8 ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff8.[REQUESTFORMCALCULATE_HEAD_PK]
left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum9 FROM [dbo].[REQUESTFORMCALCULATE_BODY] where Title in ('부가세') group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff9 ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff9.[REQUESTFORMCALCULATE_HEAD_PK]
    left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TSum10 FROM [dbo].[REQUESTFORMCALCULATE_BODY] where Title in ('관세사비') group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff10 ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff10.[REQUESTFORMCALCULATE_HEAD_PK]
WHERE 1=1
   and ARC.OurBranchCode=3157
	AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
   and ISNULL([TOTAL_PRICE], 0)<>isnull([DEPOSITED_PRICE], 0)
   and R.StepCL not in (33)
	and R.DocumentStepCL in (0,3,4, 10, 11, 12, 13, 14, 15) " + CQueryWhere + DQueryWhere;
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			String[] RowData = new String[12];
			RowData[0] = RS["RequestFormPk"] + "";
			RowData[1] = RS["CompanyPk"] + "";
			RowData[2] = (RS["ArrivalDate"] + "" == "" ? "" : (RS["ArrivalDate"] + "").Substring(2)) + " : " + RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
			if (RS["Type"] + "" == "Out") {
				RowData[3] = "<span style=\"color:red; font-weight:bold;\">出</span> <strong>" + RS["CompanyCode"] + "</strong> " + RS["CompanyName"];
			} else {
				RowData[3] = "<span style=\"color:blue; font-weight:bold;\">入</span> <strong>" + RS["CompanyCode"] + "</strong> " + RS["CompanyName"];
			}

			RowData[4] = Common.GetMonetaryUnit(RS["MonetaryUnit"] + "");
			RowData[5] = Common.NumberFormat(RS["Charge"] + "");
			decimal d_Charge = decimal.Parse(RS["Charge"] + "" == "" ? "0" : RS["Charge"] + "");

			string Deposited = RS["Deposited"] + "";
			if (Deposited == "") {
				Deposited = "0";
			}
			decimal d_Deposited = decimal.Parse(Deposited);

			if (Deposited == "") {
				RowData[6] = "<span style='color:green;'>--</span>";
			} else {
				decimal calc = d_Deposited - d_Charge;
				if (calc > 0) {
					RowData[6] = "<span style='color:blue;'>" + Common.NumberFormat(calc.ToString()) + "</span>";
				} else {
					RowData[6] = "<span style='color:red;'>" + Common.NumberFormat(calc.ToString()) + "</span>";
				}
			}
			RowData[7] = Common.NumberFormat(RS["Foward"].ToString() == "" ? "0" : RS["Foward"].ToString());
			RowData[8] = Common.NumberFormat(RS["TSum8"].ToString() == "" ? "0" : RS["TSum8"].ToString());
			RowData[9] = Common.NumberFormat(RS["TSum9"].ToString() == "" ? "0" : RS["TSum9"].ToString());
			RowData[10] = Common.NumberFormat(RS["TSum10"].ToString() == "" ? "0" : RS["TSum10"].ToString());
			RowData[11] = Common.NumberFormat(RS["DeliveryPrice"].ToString() == "" ? "0" : RS["DeliveryPrice"].ToString());
			ReturnValue.Append(String.Format(Row, RowData));
		}
		ReturnValue.Append("	</table>");
		return ReturnValue + "";
	}

	private String LoadCompanyInformation() {
		DB.SqlCmd.CommandText = @"
SELECT
	C.CompanyPk, 	C.[CompanyNo], C.[CompanyCode], C.[CompanyName], C.[CompanyTEL], C.[CompanyFAX], C.[PresidentEmail]
	, A.[Duties], A.[Name], A.[TEL], A.[Mobile], A.[Email]
FROM
	[Company] AS C
	left join Account_ AS A ON C.CompanyPk=A.CompanyPk
WHERE
	C.GubunCL<>9 and left(C.RegionCode, 1)='1'
ORDER BY C.GubunCL ";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string tempCompanyPk = "";
		int maxAccount = 0;
		int tempAccount = 0;
		StringBuilder ReturnValue = new StringBuilder();
		while (RS.Read()) {
			if (tempCompanyPk != RS["CompanyPk"] + "") {
				if (tempAccount > maxAccount) {
					maxAccount = tempAccount;
				}
				tempCompanyPk = RS["CompanyPk"] + "";
				tempAccount = 1;
				ReturnValue.Append("</tr></tr><td>" + RS["CompanyNo"] + "</td><td>" + RS["CompanyCode"] + "</td><td>" + RS["CompanyName"] + "</td><td>" + RS["CompanyTEL"] + "</td><td>" + RS["CompanyFAX"] + "</td><td>" + RS["PresidentEmail"] + "</td><td>" + RS["Duties"] + "</td><td>" + RS["Name"] + "</td><td>" + RS["TEL"] + "</td><td>" + RS["Mobile"] + "</td><td>" + RS["Email"] + "</td>");
			} else {
				ReturnValue.Append("<td>" + RS["Duties"] + "</td><td>" + RS["Name"] + "</td><td>&nbsp;" + RS["TEL"] + "</td><td>&nbsp;" + RS["Mobile"] + "</td><td>" + RS["Email"] + "</td>");
				tempAccount++;
			}
		}
		RS.Dispose();
		StringBuilder TableHeader = new StringBuilder();
		TableHeader.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\"><tr>" +
			"<td>사업자번호</td><td>고객번호</td><td>상호</td><td>전화번호</td><td>팩스</td><td>이메일</td>");
		for (int i = 0; i < maxAccount; i++) {
			TableHeader.Append("<td>직책(" + (i + 1) + ")</td><td>이름(" + (i + 1) + ")</td><td>전화번호</td><td>휴대전화</td><td>이메일</td>");
		}
		return TableHeader + "" + ReturnValue + "</table>";
	}

	private String LoadInvoiceItem(string CDPk) {
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"
	SELECT
		RF.MonetaryUnitCL, RFI.MarkNNumber, RFI.Label, RFI.Quantity, RFI.QuantityUnit, RFI.UnitPrice, RFI.Amount, CICK.Description, CICK.Material
	FROM
		CommerdialConnectionWithRequest as CCWR
		left join RequestForm AS RF ON CCWR.RequestFormPk=RF.RequestFormPk
		left join RequestFormItems AS RFI ON CCWR.RequestFormPk =RFI.RequestFormPk
		left join ClearanceItemCodeKOR AS CICK ON RFI.ItemCode=CICK.ItemCode
	WHERE
		CCWR.CommercialDocumentPk=" + CDPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string Row = "<tr><td>{0}</td><td>{1} {2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td></tr>";
		ReturnValue.Append("<tr><td>BoxNo</td><td>Description / StyleNo</td><td>Material</td><td colspan='2' >Quantity</td><td colspan='2' >UnitPrice</td><td colspan='2' >Amount</td></tr>");

		while (RS.Read()) {
			string strR = RS["Description"] + "";
			if (RS["Label"] + "" != "") {
				strR += " (" + RS["Label"] + ")";
			}

			byte[] temp;
			temp = Encoding.GetEncoding("ISO-8859-6").GetBytes(strR);
			temp = Encoding.Convert(Encoding.GetEncoding("ISO-8859-6"), Encoding.UTF8, temp);
			strR = Encoding.UTF8.GetString(temp);

			String[] Data = new String[] {
				RS["MarkNNumber"] + "&nbsp;",
				strR,
				"",
				RS["Material"] + "",
				Common.NumberFormat(RS["Quantity"] + ""),
				Common.GetQuantityUnit(RS["QuantityUnit"] + ""),
				Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + ""),
				Common.NumberFormat(RS["UnitPrice"] + ""),
				Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + ""),
				Common.NumberFormat(RS["Amount"] + "") };
			ReturnValue.Append(String.Format(Row, Data));
		}
		RS.Dispose();
		return "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" + ReturnValue + "</table>";
	}

	private String LoadDescription(string BBPk, ref String Title) {
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"	SELECT BBHead.[TRANSPORT_WAY], BBHead.[VALUE_STRING_0], BBHead.[DATETIME_FROM], BBHead.[DATETIME_TO]
																, BBHead.[VESSELNAME], BBHead.[VOYAGE_NO], BBHead.[VALUE_STRING_1], BBHead.[VALUE_STRING_2], BBHead.[VALUE_STRING_3]
																, BBHead.[AREA_FROM], BBHead.[AREA_TO], BBHead.[TRANSPORT_STATUS], TP.[NO], TP.[SIZE], TP.[SEAL_NO]
																, DC.CompanyName AS FromCompany, AC.CompanyName AS ToCompany
															FROM [dbo].[TRANSPORT_HEAD] AS BBHead
																left join Company AS DC on DC.CompanyPk=BBHead.[BRANCHPK_FROM]
																left join Company AS AC on AC.CompanyPk=BBHead.[BRANCHPK_TO]
																left join [dbo].[TRANSPORT_PACKED] AS TP ON BBHead.[TRANSPORT_PK] = TP.[TRANSPORT_PACKED_PK]
															WHERE BBHead.[TRANSPORT_PK]=" + BBPk + ";";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			string FromDate = "";
			string FromHour = "";
			string FromMin = "";
			string ToDate = "";
			string ToHour = "";
			string ToMin = "";
			stepCL = RS["TRANSPORT_STATUS"] + "";
			if (RS["DATETIME_FROM"] + "" != "... :" && RS["DATETIME_FROM"] + "" != "") {
				string temp = RS["DATETIME_FROM"] + "";
				FromDate = temp.Substring(0, temp.IndexOf(" ")).Replace(".", "");
				FromHour = temp.Substring(temp.IndexOf(" "), temp.IndexOf(":") - temp.IndexOf(" "));
				FromMin = temp.Substring(temp.IndexOf(":") + 1);
			}
			if (RS["DATETIME_TO"] + "" != "... :" && RS["DATETIME_TO"] + "" != "") {
				string temp = RS["DATETIME_TO"] + "";
				ToDate = temp.Substring(0, temp.IndexOf(" ") - 1).Replace(".", "");
				ToHour = temp.Substring(temp.IndexOf(" "), temp.IndexOf(":") - temp.IndexOf(" "));
				ToMin = temp.Substring(temp.IndexOf(":") + 1);
			}
			Title = FromDate.Substring(2) + "~" + ToDate.Substring(2) + " " + RS["VALUE_STRING_0"];
			switch (RS["TRANSPORT_WAY"] + "") {
				case "Air":
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
						"<tr><td>" + GetGlobalResourceObject("qjsdur", "gkdrhdtk") + "</td><td>" + RS["VESSELNAME"] + "</td>" +
						"		<td>MASTER B/L No</td><td>" + RS["VALUE_STRING_0"] + "</td></tr>" +
						"<tr><td>" + GetGlobalResourceObject("qjsdur", "cnfqkfrhdgkd") + "</td><td>" + RS["AREA_FROM"] + "</td>" +
						"		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td>" + FromDate + " " + FromHour + ":" + FromMin + "</td></tr>" +
						"<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrwrhdgkd") + "</td><td>" + RS["AREA_TO"] + "</td>" +
						"		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td>" + ToDate + " " + ToHour + ":" + ToMin + "</td></tr>" +
						"<tr><td>" + GetGlobalResourceObject("qjsdur", "gkdrhdaud") + "</td><td>" + RS["VOYAGE_NO"] + "</td><td></td><td></td></tr>" +
						"<tr><td>" + GetGlobalResourceObject("qjsdur", "cnfqkfgkdrhdtkdusfkrcj") + "</td><td>" + RS["VALUE_STRING_1"] + "</td>" +
						"		<td>" + GetGlobalResourceObject("qjsdur", "ehckrgkdrhdtkdusfkrcj") + "</td><td>" + RS["VALUE_STRING_2"] + "</td></tr></table>");
					break;

				case "Car":
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
						"<tr><td>" + GetGlobalResourceObject("qjsdur", "ckfidghltkaud") + "&nbsp;</td><td>" + RS["VESSELNAME"] + "&nbsp;</td>" +
						"		<td>" + GetGlobalResourceObject("qjsdur", "ghkanfthdwkdqjsgh") + "</td><td>" + RS["VALUE_STRING_0"] + "</td></tr>" +
						"<tr><td>" + GetGlobalResourceObject("qjsdur", "cnfqkfwl") + "</td><td>" + RS["AREA_FROM"] + "&nbsp;</td>" +
						"		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td>" + FromDate + " " + FromHour + ":" + FromMin + "</td></tr>" +
						"<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrwl") + "</td><td>" + RS["AREA_TO"] + "</td>" +
						"		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td>" + ToDate + " " + ToHour + ":" + ToMin + "</td></tr>" +
						"<tr><td>" + GetGlobalResourceObject("qjsdur", "ckfidqjsgh") + "</td><td>" + RS["VOYAGE_NO"] + "</td>" +
						"		<td>" + GetGlobalResourceObject("qjsdur", "ckfidrbrur") + "</td><td>" + RS["VALUE_STRING_3"] + "</td></tr>" +
						"<tr><td>" + GetGlobalResourceObject("qjsdur", "ckfiddusfkrcj") + "</td><td>" + RS["VALUE_STRING_1"] + "</td>" +
						"		<td>" + GetGlobalResourceObject("qjsdur", "rltktjdaud") + "</td><td>" + RS["VALUE_STRING_2"] + "</td></tr></table>");
					break;

				case "Ship":
					// Response.Write(description[0] + "," + description[1] + "," + description[2]);
						ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
							"<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkrghltkaud") + "</td><td>" + RS["VESSELNAME"] + "</td>" +
							"		<td>MASTER B/L</td><td>" + RS["VALUE_STRING_0"] + "</td></tr>" +
							"<tr><td>" + GetGlobalResourceObject("qjsdur", "tjswjrgkd") + "</td><td>" + RS["AREA_FROM"] + "</td>" +
							"		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td>" + FromDate + "&nbsp;&nbsp;" + FromHour + ":" + FromMin + "</td></tr>" +
							"<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrgkd") + "</td><td>" + RS["AREA_TO"] + "</td>" +
							"		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td>" + ToDate + "&nbsp;&nbsp;" + ToHour + ":" + ToMin + "</td></tr>" +
							"<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkraud") + "</td><td>" + RS["VOYAGE_NO"] + "</td>" +
							"		<td>&nbsp;</td><td>&nbsp;</td></tr>" +
							"<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjrbrur") + "</td><td >" + RS["SIZE"] + "</td>" +
							"<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjqjsgh") + "</td><td>" + RS["NO"] + "&nbsp;</td>" +
							"		<td>" + GetGlobalResourceObject("qjsdur", "Tlfqjsgh") + "</td><td>" + RS["SEAL_NO"] + "&nbsp;</td></tr></table>");
					
					/*					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
                                            "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkrghltkaud") + "</td><td>" + description[0] + "</td>" +
                                            "		<td>MASTER B/L</td><td>" + RS["BLNo"] + "</td></tr>" +
                                            "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjswjrgkd") + "</td><td>" + description[1] + "</td>" +
                                            "		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td>" + FromDate + " " + FromHour + ":" + FromMin + "</td></tr>" +
                                            "<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrgkd") + "</td><td>" + description[2] + "</td>" +
                                            "		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td>" + ToDate + " " + ToHour + ":" + ToMin + "</td></tr>" +
                                            "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkraud") + "</td><td>" + description[3] + "</td>" +
                                            "		<td>" + GetGlobalResourceObject("qjsdur", "tncnfwktkdgh") + "</td><td>" + description[4] + "</td></tr>" +
                                            "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjrbrur") + "</td><td>" + description[5] + "</td>" +
                                            "		<td>" + GetGlobalResourceObject("qjsdur", "gkdck") + "</td><td>" + description[6] + "</td></tr>" +
                                            "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjqjsgh") + "</td><td>" + description[7] + "</td>" +
                                            "		<td>" + GetGlobalResourceObject("qjsdur", "Tlfqjsgh") + "</td><td>" + description[8] + "</td></tr></table>"); */
					break;

				case "OtherCompany":
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
						"<tr><td>Hand carry Name</td><td>" + RS["VESSELNAME"] + "</td>" +
						"		<td>MASTER B/L</td><td>" + RS["VALUE_STRING_0"] + "</td></tr>" +
						"<tr><td>" + GetGlobalResourceObject("qjsdur", "tjswjrgkd") + "</td><td>" + RS["AREA_FROM"] + "</td>" +
						"		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td>" + FromDate + " " + FromHour + ":" + FromMin + "</td></tr>" +
						"<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrgkd") + "</td><td>" + RS["AREA_TO"] + "</td>" +
						"		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td>" + ToDate + " " + ToHour + ":" + ToMin + "</td></tr>" +
						"<tr><td>Departure Staff TEL</td><td>" + RS["VALUE_STRING_1"] + "</td>" +
						"		<td>Arrival Staff TEL</td><td>" + RS["VALUE_STRING_2"] + "</td></tr></table>");
					break;
			}
		}
		RS.Dispose();
		return ReturnValue + "<p>" + LoadCommentList(BBPk) + "</p>";
	}

	private String LoadDescription_TodayAbji(string BBPk, ref String Title) {
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"	SELECT BBHead.[TRANSPORT_WAY], BBHead.[VALUE_STRING_0], BBHead.[DATETIME_FROM], BBHead.[DATETIME_TO]
																, BBHead.[VESSELNAME], BBHead.[VOYAGE_NO], BBHead.[VALUE_STRING_1], BBHead.[VALUE_STRING_2], BBHead.[VALUE_STRING_3]
																, BBHead.[AREA_FROM], BBHead.[AREA_TO], BBHead.[TRANSPORT_STATUS], TP.[NO], TP.[SIZE], TP.[SEAL_NO]
																, DC.CompanyName AS FromCompany, AC.CompanyName AS ToCompany
															FROM [dbo].[TRANSPORT_HEAD] AS BBHead
																left join Company AS DC on DC.CompanyPk=BBHead.[BRANCHPK_FROM]
																left join Company AS AC on AC.CompanyPk=BBHead.[BRANCHPK_TO]
																left join [dbo].[TRANSPORT_PACKED] AS TP ON BBHead.[TRANSPORT_PK] = TP.[TRANSPORT_PACKED_PK]
															WHERE BBHead.[TRANSPORT_PK]=" + BBPk + ";";

		//Response.Write(DB.SqlCmd.CommandText);

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			string FromDate = "";
			string ToDate = "";
			stepCL = RS["TRANSPORT_STATUS"] + "";
			if (RS["DATETIME_FROM"] + "" != "... :" && RS["DATETIME_FROM"] + "" != "") {
				string temp = RS["DATETIME_FROM"] + "";
				FromDate = temp.Substring(0, temp.IndexOf(" ") - 1).Replace(".", "");
			}
			if (RS["DATETIME_TO"] + "" != "... :" && RS["DATETIME_TO"] + "" != "") {
				string temp = RS["DATETIME_TO"] + "";
				ToDate = temp.Substring(0, temp.IndexOf(" ") - 1).Replace(".", "");
			}
			Title = FromDate.Substring(2) + "~" + ToDate.Substring(2) + " " + RS["VALUE_STRING_0"];

			switch (RS["TRANSPORT_WAY"] + "") {
				case "Air":
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
						"<tr><td>AIR" + RS["VESSELNAME"] + "&nbsp;&nbsp;" + RS["VALUE_STRING_0"] + "&nbsp;&nbsp;" + RS["AREA_FROM"] + "&nbsp;&nbsp;" + RS["AREA_TO"] + "&nbsp;&nbsp;" + RS["VOYAGE_NO"] + "&nbsp;&nbsp;" + RS["VALUE_STRING_1"] + "&nbsp;&nbsp;" + RS["VALUE_STRING_2"] + "</td></tr></table>");
					break;

				case "Car":
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
						"<tr><td>CAR" + RS["VESSELNAME"] + "&nbsp;&nbsp;" + RS["VALUE_STRING_0"] + "&nbsp;&nbsp;" + RS["AREA_FROM"] + "&nbsp;&nbsp;" + RS["AREA_TO"] + "&nbsp;&nbsp;" + RS["VOYAGE_NO"] + "&nbsp;&nbsp;" + RS["VALUE_STRING_3"] + "&nbsp;&nbsp;" + RS["VALUE_STRING_1"] + "&nbsp;&nbsp;" + RS["VALUE_STRING_2"] + "</td></tr></table>");
					break;

				case "Ship":
					// Response.Write(description[0] + "," + description[1] + "," + description[2]);
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
						"<tr><td>SHIP" + RS["VESSELNAME"] + "&nbsp;&nbsp;" + RS["VALUE_STRING_0"] + "&nbsp;&nbsp;" + RS["AREA_FROM"] + "&nbsp;&nbsp;" + RS["AREA_TO"] + "&nbsp;&nbsp;" + RS["VOYAGE_NO"] + "&nbsp;&nbsp;" + RS["SIZE"] + "&nbsp;&nbsp;" + RS["NO"] + "&nbsp;&nbsp;" + RS["SEAL_NO"] + "</td></tr></table>");
					break;

				case "OtherCompany":
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
						"<tr><td>OtherCompany" + RS["VESSELNAME"] + "&nbsp;&nbsp;" + RS["VALUE_STRING_0"] + "&nbsp;&nbsp;" + RS["AREA_FROM"] + "&nbsp;&nbsp;" + RS["AREA_TO"] + "&nbsp;&nbsp;" + RS["VALUE_STRING_1"] + "&nbsp;&nbsp;" + RS["VALUE_STRING_2"] + "</td></tr></table>");
					break;
			}
		}
		RS.Dispose();
		return ReturnValue + "";
	}

	private String LoadDescription_YTClearance(string BBPk, ref String Title) {
		StringBuilder ReturnValue = new StringBuilder();
		StringBuilder Bottom = new StringBuilder();
		DB.SqlCmd.CommandText = @"	SELECT BBHead.[TRANSPORT_WAY], BBHead.[VALUE_STRING_0], BBHead.[DATETIME_FROM], BBHead.[DATETIME_TO]
																, BBHead.[VESSELNAME], BBHead.[VOYAGE_NO], BBHead.[VALUE_STRING_1], BBHead.[VALUE_STRING_2], BBHead.[VALUE_STRING_3]
																, BBHead.[AREA_FROM], BBHead.[AREA_TO], BBHead.[TRANSPORT_STATUS], TP.[NO], TP.[SIZE], TP.[SEAL_NO]
																, DC.CompanyName AS FromCompany, AC.CompanyName AS ToCompany
															FROM [dbo].[TRANSPORT_HEAD] AS BBHead
																left join Company AS DC on DC.CompanyPk=BBHead.[BRANCHPK_FROM]
																left join Company AS AC on AC.CompanyPk=BBHead.[BRANCHPK_TO]
																left join [dbo].[TRANSPORT_PACKED] AS TP ON BBHead.[TRANSPORT_PK] = TP.[TRANSPORT_PACKED_PK]
															WHERE BBHead.[TRANSPORT_PK]=" + BBPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string BLNo = "";
		string stepCL = "";
		string ContainerNo = "";
		if (RS.Read()) {
			BLNo = RS["VALUE_STRING_0"] + "";
			ContainerNo = RS["NO"].ToString().Length > 6 ? RS["NO"].ToString() : "";
			Title = ContainerNo + " YT";
			stepCL = RS["TRANSPORT_STATUS"] + "";
			ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
				"<tr><td colspan='10' style='text-align:center; font-weight:bold; font-size:30px; '>爱尔国际物流分票清单</td></tr>" +
				"<tr><td colspan='10' style='text-align:center; padding:20px;  font-weight:bold; font-size:18px; '>提单号 : " + RS["VALUE_STRING_0"] + "<br />船名 : " + RS["VOYAGE_NO"] + "</td></tr>");
		}
		RS.Dispose();
		Bottom.Append(@"
<tr>
	<td colspan='1'>&nbsp;</td>
	<td colspan='2' style='text-align:center; padding:20px;  font-weight:bold; font-size:18px; '>SHIPPER : </td>
	<td colspan='5' style='text-align:center; padding:20px;  font-size:14px; '>SHENZHEN RUIJIE ANFENG <br /> IMPORT&EXPORT CO.,LTD.<br />
ROOM B0704 , NO1, YI DA BUILDING , <br />CHUANGYE RD,BAOAN,SHENZHEN</td>
	<td colspan='1'>备注：（单位）</td>
	<td colspan='1'>&nbsp;</td>
</tr>
<tr>
	<td colspan='1'>&nbsp;</td>
	<td colspan='2' style='text-align:center; padding:20px;  font-weight:bold; font-size:18px; '>CNEE : </td>
	<td colspan='5' style='text-align:center; padding:20px;  font-size:14px; '>
		INTERNATIONAL LOGISTICS CO., LTD. <br />
		IL BLDG., 31-1,SHINHEUNG-DONG 1GA,JUNG-GU,<br /> INCHEON, KOREA<br />
		TEL:032-772-8481 FAX:032-765-8688/9</td>
	<td colspan='1'>备注：（单位）</td>
	<td colspan='1'>&nbsp;</td>
</tr>
<tr><td colspan='10' style='text-align:center; font-weight:bold; font-size:14px; '>预付OF BAF CAF EBS</td></tr>
");

		string Table;
		string TempQ;
		if (stepCL == "" || stepCL == "3") {
			Table = "TransportBBHistory";
			TempQ = "";
		} else {
			Table = "[dbo].[TRANSPORT_BODY]";
			TempQ = " ";
		}

		DB.SqlCmd.CommandText = @"
			SELECT
				R.RequestFormPk AS RequestFormPk, RFI.[Description]
			FROM
				" + Table + @" AS Storage
				left join RequestForm AS R on Storage.[REQUEST_PK]=R.RequestFormPk
				--left join [CommerdialConnectionWithRequest] AS CCWR ON R.RequestFormPk=CCWR.RequestFormPk
				--left join [CommercialDocument] AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
				left join [INTL2010].[dbo].[RequestFormItems] AS RFI ON R.RequestFormPk=RFI.RequestFormPk
			WHERE
				ISNULL(R.RequestFormPk, 0)<>0
                and R.DocumentStepCL<>4
				and Storage.[TRANSPORT_HEAD_PK]=" + BBPk + TempQ + @"
			ORDER BY RequestFormPk;";

		RS = DB.SqlCmd.ExecuteReader();

		List<string> RequestFormPk = new List<string>();
		List<List<string>> RequestItem = new List<List<string>>();
		while (RS.Read()) {
			var Cursor = -1;
			for (var i = 0; i < RequestFormPk.Count; i++) {
				if (RequestFormPk[i] == RS["RequestFormPk"] + "") {
					Cursor = i;
					break;
				}
			}
			if (Cursor == -1) {
				RequestFormPk.Add(RS["RequestFormPk"] + "");
				RequestItem.Add(new List<string>());
				Cursor = RequestItem.Count - 1;
			}

			bool CheckIsIn = false;
			for (var i = 0; i < RequestItem[Cursor].Count; i++) {
				if (RequestItem[Cursor][i] == RS["Description"] + "") {
					CheckIsIn = true;
					break;
				}
			}
			if (!CheckIsIn) {
				RequestItem[Cursor].Add(RS["Description"] + "");
			}
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"
			SELECT
				R.RequestFormPk AS RequestFormPk,R.DocumentRequestCL
				, Sum(Storage.[PACKED_COUNT]) AS BoxCount
				, SUM(R.TotalGrossWeight) AS Weight
				, SUM(R.TotalVolume) AS Volume
			FROM
				" + Table + @" AS Storage
				left join RequestForm AS R on Storage.[REQUEST_PK]=R.RequestFormPk
    --          left join [CommerdialConnectionWithRequest] AS CCWR ON R.RequestFormPk=CCWR.RequestFormPk
	--			left join [CommercialDocument] AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
			WHERE
				ISNULL(R.RequestFormPk, 0)<>0
				and R.DocumentStepCL<>4
				and Storage.[TRANSPORT_HEAD_PK]=" + BBPk + TempQ + @"
			 GROUP BY R.ConsigneeCode, R.RequestFormPk, R.ArrivalRegionCode,R.DocumentRequestCL
			ORDER BY R.ConsigneeCode, R.RequestFormPk, R.ArrivalRegionCode,R.DocumentRequestCL;";
		RS = DB.SqlCmd.ExecuteReader();
		decimal TotalBoxCount = 0;
		decimal TotalWeight = 0;
		decimal TotalVolume = 0;
		List<string[]> ItemSum = new List<string[]>();

		while (RS.Read()) {
			TotalBoxCount += decimal.Parse(RS["PACKED_COUNT"] + "");
			TotalWeight += decimal.Parse(RS["Weight"] + "");
			TotalVolume += decimal.Parse(RS["Volume"] + "");

			int Cursor = -1;
			for (var i = 0; i < RequestFormPk.Count; i++) {
				if (RS["RequestFormPk"] + "" == RequestFormPk[i]) {
					Cursor = i;
					break;
				}
			}
			StringBuilder Item = new StringBuilder();
			if (Cursor > -1) {
				List<string> thisRow = RequestItem[Cursor];
				foreach (string each in thisRow) {
					Item.Append(", " + each);
				}
			}
			ItemSum.Add(new string[] { RS["RequestFormPk"].ToString().Trim(), Common.NumberFormat(RS["PACKED_COUNT"].ToString()), Common.NumberFormat(RS["Weight"].ToString()), Common.NumberFormat(RS["Volume"].ToString()), Item.ToString().Substring(1), RS["DocumentRequestCL"] + "" });
		}
		RS.Dispose();

		List<string[]> temp = new List<string[]>();

		string Tempeach0 = "";
		decimal Tempeach1 = 0;
		decimal Tempeach2 = 0;
		decimal Tempeach3 = 0;
		StringBuilder Tempeach4 = new StringBuilder();
		string Tempeach5 = "";
		string Tempeach4_sub;
		foreach (string[] eachrow in ItemSum) {
			if (eachrow[5].IndexOf("33!") > -1) {
				temp.Add(eachrow);
			} else {
				Tempeach1 += decimal.Parse(eachrow[1]);
				Tempeach2 += decimal.Parse(eachrow[2]);
				Tempeach3 += decimal.Parse(eachrow[3]);
				Tempeach4.Append(", " + eachrow[4]);
			}
		}
		Tempeach4_sub = Tempeach4.ToString().Length > 0 ? Tempeach4.ToString().Substring(1) : Tempeach4.ToString();
		string[] Our = new string[] {
		Tempeach0.ToString().Trim(),
		Common.NumberFormat(Tempeach1.ToString()),
		Common.NumberFormat(Tempeach2.ToString()),
		Common.NumberFormat(Tempeach3.ToString()),
		Tempeach4_sub,
		Tempeach5
	};

		if (Our[1] != "0" || Our[2] != "0" || Our[3] != "0" || Our[4] != "") {
			temp.Add(Our);
		}
		ItemSum = temp;

		string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		StringBuilder InnerItem = new StringBuilder();
		for (int i = 0; i < ItemSum.Count; i++) {
			string[] innerData = ItemSum[i];
			if (i == 0) {
				InnerItem.Append("<tr><td>序号</td><td>提单号</td><td>件  数</td><td>重  量</td><td>体  积</td><td>备  注</td><td>封号</td><td>品名</td><td>&nbsp;</td><td>&nbsp;</td></tr>" +
					"<tr style='height:30px; '><td>" + (i + 1) + "</td><td>" + Alphabet.Substring(i, 1) + "</td><td>" + innerData[1] + "</td><td>" + innerData[2] + "</td><td>" + innerData[3] + "</td><td rowspan='" + (ItemSum.Count + 1) + "'>" + ContainerNo + "</td><td>&nbsp;</td><td>" + innerData[4] + "</td><td></td><td>无纸</td></tr>");
			} else {
				InnerItem.Append("<tr style='height:30px; '><td>" + (i + 1) + "</td><td>" + Alphabet.Substring(i, 1) + "</td><td>" + innerData[1] + "</td><td>" + innerData[2] + "</td><td>" + innerData[3] + "</td><td>&nbsp;</td><td>" + innerData[4] + "</td><td></td><td>无纸</td></tr>");
			}
		}
		InnerItem.Append("<tr style='height:30px; '><td>" + (ItemSum.Count + 1).ToString() + "</td><td>TOTAL</td><td>" + Common.NumberFormat(TotalBoxCount.ToString()) + "</td><td>" + Common.NumberFormat(TotalWeight.ToString()) + "</td><td>" + Common.NumberFormat(TotalVolume.ToString()) + "</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");

		return ReturnValue + "" + InnerItem + Bottom + "</table>";
	}

	//상목씨 요청사항 적용 20130402
	private String LoadDescriptionJukhwa(string BBPk) {
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"	SELECT BBHead.[TRANSPORT_WAY], BBHead.[VALUE_STRING_0], BBHead.[DATETIME_FROM], BBHead.[DATETIME_TO]
																, BBHead.[VESSELNAME], BBHead.[VOYAGE_NO], BBHead.[VALUE_STRING_1], BBHead.[VALUE_STRING_2], BBHead.[VALUE_STRING_3]
																, BBHead.[AREA_FROM], BBHead.[AREA_TO], BBHead.[TRANSPORT_STATUS], TP.[NO], TP.[SIZE], TP.[SEAL_NO]
																, DC.CompanyName AS FromCompany, AC.CompanyName AS ToCompany
															FROM [dbo].[TRANSPORT_HEAD] AS BBHead
																left join Company AS DC on DC.CompanyPk=BBHead.[BRANCHPK_FROM]
																left join Company AS AC on AC.CompanyPk=BBHead.[BRANCHPK_TO]
																left join [dbo].[TRANSPORT_PACKED] AS TP ON BBHead.[TRANSPORT_PK] = TP.[TRANSPORT_PACKED_PK]
															WHERE BBHead.[TRANSPORT_PK]=" + BBPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			stepCL = RS["TRANSPORT_STATUS"] + "";
			string FromDate = "";
			if (RS["DATETIME_FROM"] + "" != "... :" && RS["DATETIME_FROM"] + "" != "") {
				string temp = RS["DATETIME_FROM"] + "";
				FromDate = temp.Substring(0, temp.IndexOf(" ")).Replace(".", "").Substring(4, 4);
			}
			string ToDate = "";
			if (RS["DATETIME_TO"] + "" != "... :" && RS["DATETIME_TO"] + "" != "") {
				string temp = RS["DATETIME_TO"] + "";
				ToDate = temp.Substring(0, temp.IndexOf(" ")).Replace(".", "").Substring(4, 4);
			}

			switch (RS["TRANSPORT_WAY"] + "") {
				case "Air":
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:700px; \">" +
										"<tr><td colspan='11' style=\"font-weight:bold; font-size:30px; text-align:center;  \">적화물 명세서</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-size:15px; padding-top:4px; padding-bottom:4px; font-weight:bold; \" >" + ToDate.Substring(0, 2) + "월 " + ToDate.Substring(2, 2) + "일</td>" +
											"<td colspan='2' style=\"font-weight:bold; text-align:center; font-size:14px; \">주식회사 아이엘</td>" +
											"<td colspan='4' style=\"font-weight:bold; text-align:center; font-size:14px; \">" + RS["VESSELNAME"] + "</td>" +
											"<td colspan='3' style=\"font-weight:bold; text-align:center; font-size:10px; \">" + RS["VOYAGE_NO"] + "</td></tr>" +
										"<tr><td colspan='4' style=\"font-size:14px; text-align:center; font-weight:bold; \">" + RS["AREA_FROM"] + " " + FromDate + " ~ " + RS["AREA_TO"] + " " + ToDate + "</td><td colspan='3' style=\"text-align:center;  font-weight:bold;\">접안시간</td><td colspan='4' style=\"font-weight:bold; text-align:center; font-size:14px; \">&nbsp;</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-weight:bold; \">B/L #</td><td colspan='2' style=\"width:100px; font-size:20px; text-align:center;  font-weight:bold; \">" + RS["VALUE_STRING_0"] + "</td><td colspan='3' style=\"text-align:center;  font-weight:bold;\">입고시간</td><td colspan='4' style=\"font-weight:bold; text-align:center; font-size:14px; \">&nbsp;</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-weight:bold; \">입하창고</td><td colspan='2' style=\"font-size:18px; text-align:center; font-weight:bold; \">조 양 창 고</td><td colspan='3' style=\"text-align:center;  font-weight:bold;\">완료시간</td><td colspan='4' style=\"font-weight:bold; text-align:center; font-size:14px; \">&nbsp;</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-weight:bold; \">컨테이너No</td><td style=\"font-size:20px; text-align:center; width:180px; font-weight:bold; \">" + "&nbsp;" + "</td><td style=\"font-size:14px; width:60px; text-align:center; font-weight:bold; \">" + (RS["VALUE_STRING_2"] + "" == "" ? "&nbsp;" : RS["VALUE_STRING_2"] + "") + "</td><td style=\"text-align:center; font-size:10px;  font-weight:bold;\">씰 No</td><td colspan='2' style=\"font-size:10px; text-align:center; \">" + "&nbsp;" + "</td><td colspan='2' style=\"text-align:center; font-weight:bold;\">관리대상 여부</td><td colspan='2'  style=\"text-align:center; font-weight:bold; font-size:18px; \">&nbsp;</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-weight:bold; \">CY</td><td style=\"font-size:20px; text-align:center; width:180px; font-weight:bold; \">&nbsp;</td><td style=\"font-size:14px; width:60px; text-align:center; font-weight:bold; \">&nbsp;</td><td colspan='3' style=\"text-align:center; font-size:10px;  font-weight:bold;\">소요시간</td><td colspan='4' style=\"text-align:center; font-weight:bold;\">&nbsp;</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-weight:bold; \">보세운송</td><td style=\"font-size:20px; text-align:center; width:180px; font-weight:bold; \">&nbsp;</td><td style=\"font-size:14px; width:60px; text-align:center; font-weight:bold; \">&nbsp;</td><td colspan='3' style=\"text-align:center; font-size:10px;  font-weight:bold;\">소요시간</td><td colspan='4' style=\"text-align:center; font-weight:bold;\">&nbsp;</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-weight:bold; \">총 BL건수</td><td colspan='2' style=\"font-size:20px; text-align:center; width:180px; font-weight:bold; \"><td colspan='3' style=\"text-align:center; font-size:10px;  font-weight:bold;\">PA/RO</td><td colspan='4' style=\"text-align:center; font-weight:bold;\">&nbsp;</td></tr>");

					break;

				default:
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:700px; \">" +
										"<tr><td colspan='11' style=\"font-weight:bold; font-size:30px; text-align:center;  \">적화물 명세서</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-size:15px; padding-top:4px; padding-bottom:4px; font-weight:bold; \" >" + ToDate.Substring(0, 2) + "월 " + ToDate.Substring(2, 2) + "일</td>" +
											"<td colspan='2' style=\"font-weight:bold; text-align:center; font-size:14px; \">주식회사 아이엘</td>" +
											"<td colspan='4' style=\"font-weight:bold; text-align:center; font-size:14px; \">" + RS["VESSELNAME"] + "</td>" +
											"<td colspan='3' style=\"font-weight:bold; text-align:center; font-size:10px; \">" + RS["VOYAGE_NO"] + "</td></tr>" +
										"<tr><td colspan='4' style=\"font-size:14px; text-align:center; font-weight:bold; \">" + RS["AREA_FROM"] + " " + FromDate + " ~ " + RS["AREA_TO"] + " " + ToDate + "</td><td colspan='3' style=\"text-align:center;  font-weight:bold;\">접안시간</td><td colspan='4' style=\"font-weight:bold; text-align:center; font-size:14px; \">&nbsp;</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-weight:bold; \">B/L #</td><td colspan='2' style=\"width:100px; font-size:20px; text-align:center;  font-weight:bold; \">" + RS["VALUE_STRING_0"] + "</td><td colspan='3' style=\"text-align:center;  font-weight:bold;\">입고시간</td><td colspan='4' style=\"font-weight:bold; text-align:center; font-size:14px; \">&nbsp;</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-weight:bold; \">입하창고</td><td colspan='2' style=\"font-size:18px; text-align:center; font-weight:bold; \">조 양 창 고</td><td colspan='3' style=\"text-align:center;  font-weight:bold;\">완료시간</td><td colspan='4' style=\"font-weight:bold; text-align:center; font-size:14px; \">&nbsp;</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-weight:bold; \">컨테이너No</td><td style=\"font-size:20px; text-align:center; width:180px; font-weight:bold; \">" + (RS["NO"] + "" == "" ? "&nbsp;" : RS["NO"] + "") + "</td><td style=\"font-size:14px; width:60px; text-align:center; font-weight:bold; \">" + (RS["VOYAGE_NO"] + "" == "" ? "&nbsp;" : RS["VOYAGE_NO"] + "") + "</td><td style=\"text-align:center; font-size:10px;  font-weight:bold;\">씰 No</td><td colspan='2' style=\"font-size:10px; text-align:center; \">" + (RS["SEAL_NO"] + "" == "" ? "&nbsp;" : RS["SEAL_NO"] + "") + "</td><td colspan='2' style=\"text-align:center; font-weight:bold;\">관리대상 여부</td><td colspan='2'  style=\"text-align:center; font-weight:bold; font-size:18px; \">&nbsp;</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-weight:bold; \">CY</td><td style=\"font-size:20px; text-align:center; width:180px; font-weight:bold; \">&nbsp;</td><td style=\"font-size:14px; width:60px; text-align:center; font-weight:bold; \">&nbsp;</td><td colspan='3' style=\"text-align:center; font-size:10px;  font-weight:bold;\">소요시간</td><td colspan='4' style=\"text-align:center; font-weight:bold;\">&nbsp;</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-weight:bold; \">보세운송</td><td style=\"font-size:20px; text-align:center; width:180px; font-weight:bold; \">&nbsp;</td><td style=\"font-size:14px; width:60px; text-align:center; font-weight:bold; \">&nbsp;</td><td colspan='3' style=\"text-align:center; font-size:10px;  font-weight:bold;\">소요시간</td><td colspan='4' style=\"text-align:center; font-weight:bold;\">&nbsp;</td></tr>" +
										"<tr><td colspan='2' style=\"text-align:center; font-weight:bold; \">총 BL건수</td><td colspan='2' style=\"font-size:20px; text-align:center; width:180px; font-weight:bold; \"><td colspan='3' style=\"text-align:center; font-size:10px;  font-weight:bold;\">PA/RO</td><td colspan='4' style=\"text-align:center; font-weight:bold;\">&nbsp;</td></tr>");

					break;
			}
		}
		RS.Dispose();

		ReturnValue.Append("<tr><td colspan='11'>&nbsp;</td></tr><tr>" +
						"	<td style=\"width:25px; font-size:8px; font-weight:bold;\">순번</td>" +
						"	<td style=\"width:75px; text-align:center; font-weight:bold; \" >MARK</td>" +
						"	<td colspan='2' style=\"text-align:center; width:210px; font-weight:bold;  \"  >화물 명세</td>" +
						"	<td colspan='2' style=\"text-align:center; width:50px; font-weight:bold; background-color:#DFE0CF;\">수량</td>" +
						"	<td style=\"width:40px; text-align:center; font-weight:bold;\" >도착</td>" +
						"	<td style=\"width:120px; text-align:center; font-size:10px; font-weight:bold;\" >수입자</td>" +
						"	<td style=\"width:30px; text-align:center;  font-size:10px; font-weight:bold; \" >CBM</td>" +
						"	<td style=\"width:100px; text-align:center;  font-size:10px; font-weight:bold; \" >BLNo</td>" +
						"	<td style=\"width:100px; text-align:center; font-size:10px; font-weight:bold;\" >기타사항</td></tr>");
		string Table;
		string TempQ;
		if (stepCL == "" || stepCL == "3") {
			Table = "TransportBBHistory";
			TempQ = "";
		} else {
			Table = "[dbo].[TRANSPORT_BODY]";
			TempQ = " ";
		}
		DB.SqlCmd.CommandText = @"
SELECT
	Storage.[PACKED_COUNT]
	, R.RequestFormPk, R.ConsigneeCode , RFI.description
	, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume
	, CICK.CICKD , CC.CompanyName
	, CD.BLNo, CD.Consignee ,R.ArrivalDate, R.TransportWayCL, R.ArrivalDate
FROM
	" + Table + @" AS Storage left join
	RequestForm AS R on Storage.[REQUEST_PK]=R.RequestFormPk
	Left join Company AS CC on R.ConsigneePk=CC.CompanyPk
	Left join RegionCode AS Departure on R.DepartureRegionCode=Departure.RegionCode
	Left join RegionCode AS Arrival on R.ArrivalRegionCode=Arrival.RegionCode
	left join RequestFormItems AS RFI ON R.RequestFormPk=RFI.RequestFormPk
	left join (
		SELECT [RequestFormItemsPk], CICK.ItemCode AS CICKC,CICK.Description AS CICKD
		FROM RequestFormItems AS RFI
			left join ClearanceItemCodeKOR AS CICK ON RFI.ItemCode=CICK.ItemCode
		) AS CICK ON CICK.RequestFormItemsPk=RFI.RequestFormItemsPk
	left join (SELECT [TABLE_PK], [ACCOUNT_ID], [DESCRIPTION], [REGISTERD] FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [CODE]='0') AS RFAI ON R.RequestFormPk=RFAI.[TABLE_PK]
	left join CommerdialConnectionWithRequest AS CCWR On R.RequestFormPk=CCWR.RequestFormPk
	left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
WHERE Storage.[TRANSPORT_HEAD_PK]=" + BBPk + TempQ + @"
ORDER BY   R.ConsigneeCode, R.RequestFormPk, CD.BLNo, R.ArrivalRegionCode;";
		RS = DB.SqlCmd.ExecuteReader();
		string BeforeRequest = "";
		List<string> ItemSum = new List<string>();
		StringBuilder itemTemp;
		int count = 0;
		string InnerTable = "<tr><td style=\"text-align:center; font-size:10px; font-weight:bold; \">{0}</td>" +
							"<td style=\"font-size:18px; font-weight:bold; \">{1}</td>" +
							"<td colspan='2'  style=\"width:200px;\">{2}</td>" +
							"<td style=\"text-align:right; width:40px; font-size:16px;  font-weight:bold; background-color:#DFE0CF;\">{3}</td>" +
							"<td style=\"text-align:center;width:20px; font-weight:bold; background-color:#DFE0CF; font-size:12px; \">{4}</td>" +
							"<td style=\"text-align:center;  \">{5}</td>" +
							"<td style=\"text-align:center;  font-size:10px; \" >{6}</td>" +
							"<td style=\"text-align:right; padding-right:4px; font-size:10px;  \">{7}</td>" +
							"<td style=\"text-align:right; padding-right:4px; font-size:10px;  \">{8}</td>" +
							"<td style=\"text-align:center; padding-right:4px; font-size:10px; font-weight:bold;  \">&nbsp;</td></tr>";
		string[] InnerTableData = new string[9];
		int TotalPackingCount = 0;
		decimal TotalVolume = 0;
		while (RS.Read()) {
			if (BeforeRequest == RS["RequestFormPk"] + "") {
				bool isinitemsum = false;
				if (RS["description"] + "" != "") {
					foreach (string each in ItemSum) {
						if (each == RS["description"] + "") {
							isinitemsum = true;
							break;
						}
					}
				}
				if (!isinitemsum) {
					if (RS["description"] + "" != "")
						ItemSum.Add(RS["description"] + "");
				}
				continue;
			} else {
				if (BeforeRequest != "") {
					if (ItemSum.Count < 1) {
						InnerTableData[2] = "";
					} else {
						InnerTableData[2] = ItemSum[0] + (ItemSum.Count > 1 ? "외 " + (Int32.Parse(ItemSum.Count + "") - 1) + "건" : "");
					}

					ReturnValue.Append(string.Format(InnerTable, InnerTableData));
					InnerTableData = new string[9];
				}
				BeforeRequest = RS["RequestFormPk"] + "";
				count++;
				ItemSum = new List<string>();
				if (RS["description"] + "" != "") {
					ItemSum.Add(RS["description"] + "");
				}
				InnerTableData[0] = count + "";
				InnerTableData[1] = RS["ConsigneeCode"] + "";
				InnerTableData[3] = RS["PACKED_COUNT"] + " ";
				InnerTableData[4] = Common.GetPackingUnit(RS["PackingUnit"] + "");
				InnerTableData[5] = RS["ArrivalDate"] + "" != "" ? (RS["ArrivalDate"] + "").Substring(4, 4) : "&nbsp;";
				InnerTableData[6] = RS["CompanyName"] + "";
				InnerTableData[7] = Common.NumberFormat(RS["TotalVolume"] + "");
				InnerTableData[8] = RS["BLNo"] + "";
				try {
					TotalPackingCount += Int32.Parse(RS["BoxCount"] + "");
				} catch (Exception) {
				}
				try {
					TotalVolume += decimal.Parse(RS["TotalVolume"] + "");
				} catch (Exception) {
				}
				continue;
			}
		}
		RS.Dispose();
		itemTemp = new StringBuilder();
		for (int i = 0; i < ItemSum.Count; i++) {
			if (i == 0) {
				itemTemp.Append(ItemSum[i]);
			} else {
				itemTemp.Append(", " + ItemSum[i]);
			}
		}
		InnerTableData[2] = itemTemp + "";
		ReturnValue.Append(string.Format(InnerTable, InnerTableData));

		return ReturnValue + "<tr><td colspan='4' style=\"text-align:center; font-size:10px; font-weight:bold; \">&nbsp;</td>" +
												"<td style=\"text-align:right;  font-size:16px; font-weight:bold; background-color:#A5E3EB;\">" + TotalPackingCount + "</td>" +
												"<td colspan='2' style=\"text-align:left; font-weight:bold; font-size:14px; background-color:#A5E3EB; \">PKS</td><td style=\"text-align:center; font-size:10px;  \" >&nbsp;</td><td style=\"text-align:right; padding-right:4px; font-size:10px;  \">" + Common.NumberFormat(TotalVolume + "") + "</td><td>&nbsp;</td></tr>" +
												"<tr><td colspan='11' style=\"text-align:right; \"><br><font style=\"font-size:10px; \">注 &#62; &#62; 단, 관리대상일 경우엔 세관 지정 (구내) 창고로 변경됨.&nbsp;&nbsp;<br>" +
												"<font style=\"font-size:12px; \"><b>TEL: 032-772-8481 FAX: 032-765-8688 &nbsp;&nbsp;</b></font><br><br></td></tr></table>";
	}

	private String LoadCommentList(string BBPk) {
		StringBuilder CommentTemp = new StringBuilder();

		HistoryC HisC = new HistoryC();
		List<sComment> Comment = new List<sComment>();
		Comment = HisC.LoadList_Comment("TRANSPORT_HEAD", BBPk, "'Transport_Head'", ref DB);

		for (int i = 0; i < Comment.Count; i++) {
			CommentTemp.Append(Comment[i].Contents + " : <strong>" + Comment[i].Account_Id + "</strong> " + (Comment[i].Registerd).Substring(5, (Comment[i].Registerd).Length - 8) + "<br />");
		}

		return CommentTemp + "" == "" ? "" : "<fieldset style=\"width:420px; padding:10px;  \"><legend><strong>Comment</strong></legend>" + CommentTemp + "</fieldset>";
	}

	private String LoadPackedList(string BBPk) {
		StringBuilder ReturnValue = new StringBuilder();
		string Table;
		string TempQ;
		if (stepCL == "" || stepCL == "3") {
			Table = "TransportBBHistory";
			TempQ = "";
		} else {
			Table = "[dbo].[TRANSPORT_BODY]";
			TempQ = " ";
		}
		DB.SqlCmd.CommandText = @"		SELECT
																	Storage.[REQUEST_PK], Storage.[PACKED_COUNT] , R.RequestFormPk, R.ShipperPk, R.ConsigneePk
																	, R.ShipperCode, R.ConsigneeCode , R.DepartureDate, R.ArrivalDate, R.TransportWayCL, R.DocumentRequestCL
																	, CC.CompanyName
																	, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, RFI.Description
																	, RFAI.[ACCOUNT_ID], RFAI.[DESCRIPTION], RFAI.[REGISTERD] , Departure.OurBranchCode AS DOBC, Arrival.OurBranchCode AS AOBC
																	, Initial.Initial
																FROM
																	" + Table + @" AS Storage left join
																	RequestForm AS R on Storage.[REQUEST_PK]=R.RequestFormPk Left join
																	Company AS CC on R.ConsigneePk=CC.CompanyPk Left join
																	RegionCode AS Departure on R.DepartureRegionCode=Departure.RegionCode Left join
																	RegionCode AS Arrival on R.ArrivalRegionCode=Arrival.RegionCode left join
																	RequestFormItems AS RFI ON R.RequestFormPk=RFI.RequestFormPk left join
																	(SELECT [TABLE_PK], [ACCOUNT_ID], [DESCRIPTION], [REGISTERD] FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [CODE]='0') AS RFAI ON R.RequestFormPk=RFAI.[TABLE_PK]
																	left join TransportWayCLDescription AS Initial ON Initial.TransportWayCL=R.TransportWayCL
																WHERE ISNULL(R.RequestFormPk, 0)<>0 and Storage.[TRANSPORT_HEAD_PK]=" + BBPk + TempQ + "  ORDER BY R.ConsigneeCode, R.ArrivalRegionCode;";
		//Response.Write(DB.SqlCmd.CommandText);
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		bool IsFirst = true;
		string BeforeRequest = "";
		string HtmlDocumentRequest = "";
		List<string> ItemSum = new List<string>();
		StringBuilder itemTemp;
		int count = 0;
		while (RS.Read()) {
			if (IsFirst) {
				count = 1;
				IsFirst = false;
				BeforeRequest = RS["RequestFormPk"] + "";
				ItemSum = new List<string>();
				ItemSum.Add(RS["Description"] + "");
				itemTemp = new StringBuilder();
				//20140714 한경리님
				if (RS["DocumentRequestCL"] + "" != "") {
					string DocumentRequestCL = RS["DocumentRequestCL"] + "";
					string[] Documents = DocumentRequestCL.Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
					foreach (string T in Documents) {
						switch (T) {
							case "31":
								HtmlDocumentRequest += "화주원산지제공(客户提供产地证) ";
								break;

							case "32":
								if (HtmlDocumentRequest != "") {
									HtmlDocumentRequest += "<br>";
								}
								HtmlDocumentRequest += "원산지신청(申请产地证) ";
								break;

							case "33":
								if (HtmlDocumentRequest != "") {
									HtmlDocumentRequest += "<br>";
								}
								HtmlDocumentRequest += "단증보관(单证报关) ";
								break;
							case "34":
								if (HtmlDocumentRequest != "") {
									HtmlDocumentRequest += "<br>";
								}
								HtmlDocumentRequest += "FTA";
								break;
						}
					}
				} else {
					HtmlDocumentRequest = "";
				}
				ReturnValue.Append("<tr>" +
												"	<td>" + count + "</td>" +
												"	<td>" + Common.DBToHTML(RS["ShipperCode"]) + "</td>" +
												"	<td>" + Common.DBToHTML(RS["ConsigneeCode"]) + "</td>" +
												"	<td>" + Common.DBToHTML(RS["CompanyName"]) + "</td>" +
												"	<td>" + Common.DBToHTML(RS["DepartureDate"]) + "</td>" +
												"	<td>" + Common.DBToHTML(RS["Initial"]) + "</td>" +
												"	<td>" + Common.DBToHTML(RS["ArrivalDate"]) + "</td>" +
												"	<td>" + Common.DBToHTML(RS["PACKED_COUNT"]) + "</td>" +
												"	<td>" + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</td>" +
												"	<td>" + Common.DBToHTML(RS["TotalPackedCount"]) + "</td>" +
												"	<td>" + Common.NumberFormat(RS["TotalVolume"] + "") + "</td>" +
												"	<td>" + Common.NumberFormat(RS["TotalGrossWeight"] + "") + "</td>" +
												"	<td>" + Common.DBToHTML(RS["DESCRIPTION"]) + "</td>" +
												"	<td>" + Common.DBToHTML(HtmlDocumentRequest) + "</td>");
				continue;
			}
			if (BeforeRequest == RS["RequestFormPk"] + "") {
				bool isinitemsum = false;
				foreach (string each in ItemSum) {
					if (each == RS["Description"] + "") {
						isinitemsum = true;
						break;
					}
				}
				if (!isinitemsum) {
					ItemSum.Add(RS["Description"] + "");
				}
			} else {
				itemTemp = new StringBuilder();
				for (int i = 0; i < ItemSum.Count; i++) {
					if (i == 0) {
						itemTemp.Append(ItemSum[i]);
					} else {
						itemTemp.Append(", " + ItemSum[i]);
					}
				}
				ReturnValue.Append("	<td>" + Common.DBToHTML(itemTemp.ToString()) + "</td></tr>");
				BeforeRequest = RS["RequestFormPk"] + "";
				HtmlDocumentRequest = "";
				if (RS["DocumentRequestCL"] + "" != "") {
					string DocumentRequestCL = RS["DocumentRequestCL"] + "";
					string[] Documents = DocumentRequestCL.Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
					foreach (string T in Documents) {
						switch (T) {
							case "31":
								HtmlDocumentRequest += "화주원산지제공(客户提供产地证) ";
								break;

							case "32":
								if (HtmlDocumentRequest != "") {
									HtmlDocumentRequest += "<br>";
								}
								HtmlDocumentRequest += "원산지신청(申请产地证) ";
								break;

							case "33":
								if (HtmlDocumentRequest != "") {
									HtmlDocumentRequest += "<br>";
								}
								HtmlDocumentRequest += "단증보관(单证报关) ";
								break;
							case "34":
								if (HtmlDocumentRequest != "") {
									HtmlDocumentRequest += "<br>";
								}
								HtmlDocumentRequest += "FTA ";
								break;
						}
					}
				}

				ItemSum = new List<string>();
				ItemSum.Add(RS["Description"] + "");
				itemTemp = new StringBuilder();
				count++;
				ReturnValue.Append("<tr>" +
												"	<td>" + count + "</td>" +
												"	<td>" + Common.DBToHTML(RS["ShipperCode"]) + "</td>" +
												"	<td>" + Common.DBToHTML(RS["ConsigneeCode"]) + "</td>" +
												"	<td>" + Common.DBToHTML(RS["CompanyName"]) + "</td>" +
												"	<td>" + Common.DBToHTML(RS["DepartureDate"]) + "</td>" +
												"	<td>" + Common.DBToHTML(RS["Initial"]) + "</td>" +
												"	<td>" + Common.DBToHTML(RS["ArrivalDate"]) + "</td>" +
												"	<td>" + Common.DBToHTML(RS["PACKED_COUNT"]) + "</td>" +
												"	<td>" + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</td>" +
												"	<td>" + Common.DBToHTML(RS["TotalPackedCount"]) + "</td>" +
												"	<td>" + Common.NumberFormat(RS["TotalVolume"] + "") + "</td>" +
												"	<td>" + Common.NumberFormat(RS["TotalGrossWeight"] + "") + "</td>" +
												"	<td>" + Common.DBToHTML(RS["DESCRIPTION"]) + "</td>" +
												"	<td>" + Common.DBToHTML(HtmlDocumentRequest) + "</td>");
			}
		}
		RS.Dispose();
		itemTemp = new StringBuilder();
		for (int i = 0; i < ItemSum.Count; i++) {
			if (i == 0) {
				itemTemp.Append(ItemSum[i]);
			} else {
				itemTemp.Append(", " + ItemSum[i]);
			}
		}
		ReturnValue.Append("	<td>" + Common.DBToHTML(itemTemp.ToString()) + "</td></tr>");

		return "<br /><table border='1' cellpadding='0' cellspacing='0' style=\"width:850px;\"><tr height='30px;'>" +
						"	<td class='THead1' style=\"width:55px;\" >No</td>" +
						"	<td class='THead1' style=\"width:55px;\" >Shipper</td>" +
						"	<td class='THead1' style=\"width:55px;\" >Consignee</td>" +
						"	<td class='THead1' style=\"width:55px;\" >Consignee CompanyName</td>" +
						"	<td class='THead1' style=\"width:75px;\" >Start</td>" +
						"	<td class='THead1' style=\"width:90px;\" >Initial</td>" +
						"	<td class='THead1' style=\"width:75px;\" >" + GetGlobalResourceObject("qjsdur", "ehckrdlf") + "</td>" +
						"	<td class='THead1' style=\"width:70px;\" >" + GetGlobalResourceObject("qjsdur", "vhwkdtnfid") + "</td>" +
						"	<td class='THead1' style=\"width:70px;\" >&nbsp;</td>" +
						"	<td class='THead1' style=\"width:70px;\" >Total</td>" +
						"	<td class='THead1' style=\"width:70px;\" >" + GetGlobalResourceObject("qjsdur", "cpwjr") + "CBM</td>" +
						"	<td class='THead1' style=\"width:70px;\" >" + GetGlobalResourceObject("qjsdur", "wndfid") + "Kg</td>" +
						"	<td class='THead1' >Comment</td>" +
						"	<td class='THead1' >무역서류</td>" +
						"	<td class='THead1' >Description</td></tr>" + ReturnValue + "</table>";
	}

	private String LoadCustomerRequestList(string CompanyPk) {
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"
SELECT
	R.RequestFormPk, R.ShipperPk, R.ConsigneePk , R.ShipperCode, R.ConsigneeCode , R.DepartureDate, R.ArrivalDate, R.TransportWayCL ,
	SC.CompanyName AS SCompanyName, CC.CompanyName AS CCompanyName,
	R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, CalcH.[TOTAL_PRICE], CalcH.[DEPOSITED_PRICE], CalcH.[LAST_DEPOSITED_DATE],
	DeliveryCharge.Price , TariffCharge.TSum, FreightCharge.FSum,
	CD.BLNo
FROM
	RequestForm AS R
	Left join Company AS SC on R.ShipperPk=SC.CompanyPk
	Left join Company AS CC on R.ConsigneePk=CC.CompanyPk
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as CalcH on R.RequestFormPk=CalcH.[TABLE_PK] 
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) AS FSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='해운비' GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		)	AS FreightCharge ON CalcH.[REQUESTFORMCALCULATE_HEAD_PK]=FreightCharge.[REQUESTFORMCALCULATE_HEAD_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) AS Price FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='대행비' GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		)	AS DeliveryCharge ON CalcH.[REQUESTFORMCALCULATE_HEAD_PK]=DeliveryCharge.[REQUESTFORMCALCULATE_HEAD_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) AS TSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='제세금' GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		)	AS TariffCharge ON CalcH.[REQUESTFORMCALCULATE_HEAD_PK]=TariffCharge.[REQUESTFORMCALCULATE_HEAD_PK]
	left join CommerdialConnectionWithRequest AS CCWR On R.RequestFormPk=CCWR.RequestFormPk
	left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
WHERE R.ShipperPk=" + CompanyPk + @" OR R.ConsigneePk=" + CompanyPk + @"
AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
      Order by R.ArrivalDate DESC, R.RequestFormPk DESC;";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			decimal TotalCharge, FreightCharge, DeliveryCharge;
			string Gubun;
			string TargetCode, TargetCompany;

			if (RS["ShipperPk"] + "" == CompanyPk) {
				Gubun = "수출";
				TargetCode = RS["ConsigneeCode"] + "";
				TargetCompany = RS["CCompanyName"] + "";
			} else {
				Gubun = "수입";
				TargetCode = RS["ShipperCode"] + "";
				TargetCompany = RS["SCompanyName"] + "";
			}

			TotalCharge = RS["TOTAL_PRICE"] + "" == "" ? 0 : decimal.Parse(RS["TOTAL_PRICE"] + "");
			FreightCharge = decimal.Parse(RS["FSum"] + "");
			DeliveryCharge = decimal.Parse(RS["Price"] + "");

			ReturnValue.Append("<tr><td>" + Gubun + "</td>" +
									   "	<td>" + (RS["ArrivalDate"] + "" == "" ? "&nbsp;" : RS["ArrivalDate"] + "") + "</td>" +
													"	<td>" + RS["BLNo"] + "</td>" +
													"	<td>" + TargetCode + "</td>" +
													"	<td>" + TargetCompany + "</td>" +
													"	<td>" + RS["TotalPackedCount"] + "</td>" +
													"	<td>" + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</td>" +
													"	<td>" + Common.NumberFormat(RS["TotalVolume"] + "") + "</td>" +
													"	<td>" + Common.NumberFormat(RS["TotalGrossWeight"] + "") + "</td>" +
													"	<td>" + Common.NumberFormat(FreightCharge.ToString()) + "</td>" +
													"	<td>" + Common.NumberFormat(RS["TSum"] + "") + "</td>" +
													"	<td>" + Common.NumberFormat(DeliveryCharge.ToString()) + "</td>" +
													"	<td>" + Common.NumberFormat(TotalCharge + "") + "</td>" +
													"</tr>");
		}
		RS.Dispose();
		return "<br /><table border='1' cellpadding='0' cellspacing='0' style=\"width:850px;\"><tr height='30px;'>" +
						"	<td class='THead1' style=\"width:55px;\" >구분</td>" +
				  "	<td class='THead1' style=\"width:70px;\" >도착일</td>" +
						"	<td class='THead1' style=\"width:55px;\" >BLNo</td>" +
						"	<td class='THead1' style=\"width:55px;\" >거래처 고객번호</td>" +
						"	<td class='THead1' style=\"width:55px;\" >거래처 상호</td>" +
						"	<td class='THead1' style=\"width:75px;\" >포장수량</td>" +
						"	<td class='THead1' style=\"width:90px;\" >&nbsp;</td>" +
						"	<td class='THead1' style=\"width:70px;\" >체적</td>" +
						"	<td class='THead1' style=\"width:70px;\" >중량</td>" +
						"	<td class='THead1' style=\"width:70px;\" >운송비</td>" +
						"	<td class='THead1' style=\"width:70px;\" >Tariff</td>" +
						"	<td class='THead1' style=\"width:70px;\" >DeliveryCharge</td>" +
						"	<td class='THead1' style=\"width:70px;\" >비용합계</td>" +
						"	</tr>" + ReturnValue + "</table>";
	}

	private String LoadMemberRequestList(string CompanyPk) {
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"


SELECT
	R.RequestFormPk, R.ShipperPk, R.ConsigneePk , R.ShipperCode, R.ConsigneeCode , R.DepartureDate, R.ArrivalDate, R.TransportWayCL ,
	SC.CompanyName AS SCompanyName, CC.CompanyName AS CCompanyName,
	R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, CalcH.[TOTAL_PRICE], CalcH.[DEPOSITED_PRICE], CalcH.[LAST_DEPOSITED_DATE],
	DeliveryCharge.Price , TariffCharge.TSum, FreightCharge.FSum,
	CD.BLNo
FROM
	RequestForm AS R
	Left join Company AS SC on R.ShipperPk=SC.CompanyPk
	Left join Company AS CC on R.ConsigneePk=CC.CompanyPk
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as CalcH on R.RequestFormPk=CalcH.[TABLE_PK] 
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) AS FSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='해운비' GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		)	AS FreightCharge ON CalcH.[REQUESTFORMCALCULATE_HEAD_PK]=FreightCharge.[REQUESTFORMCALCULATE_HEAD_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) AS Price FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='대행비' GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		)	AS DeliveryCharge ON CalcH.[REQUESTFORMCALCULATE_HEAD_PK]=DeliveryCharge.[REQUESTFORMCALCULATE_HEAD_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) AS TSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='제세금' GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		)	AS TariffCharge ON CalcH.[REQUESTFORMCALCULATE_HEAD_PK]=TariffCharge.[REQUESTFORMCALCULATE_HEAD_PK]
	left join CommerdialConnectionWithRequest AS CCWR On R.RequestFormPk=CCWR.RequestFormPk
	left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
WHERE (R.ShipperPk=" + CompanyPk + @" OR R.ConsigneePk=" + CompanyPk + @") and R.ArrivalDate>dateadd(YEAR,-2,GETDATE())
Order by R.ArrivalDate DESC, R.RequestFormPk DESC;";


		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			decimal TotalCharge, FreightCharge, DeliveryCharge;
			string Gubun;
			string TargetCode, TargetCompany;


			if (RS["ShipperPk"] + "" == CompanyPk) {
				Gubun = "수출";
				TargetCode = RS["ConsigneeCode"] + "";
				TargetCompany = RS["CCompanyName"] + "";
			}
			else {
				Gubun = "수입";
				TargetCode = RS["ShipperCode"] + "";
				TargetCompany = RS["SCompanyName"] + "";
			}

			TotalCharge = RS["TOTAL_PRICE"] + "" == "" ? 0 : decimal.Parse(RS["TOTAL_PRICE"] + "");
			FreightCharge = decimal.Parse(RS["FSum"] + "");
			DeliveryCharge = decimal.Parse(RS["Price"] + "");

			ReturnValue.Append("<tr><td>" + Gubun + "</td>" +
			"	<td>" + (RS["ArrivalDate"] + "" == "" ? "&nbsp;" : RS["ArrivalDate"] + "") + "</td>" +
													"	<td>" + RS["BLNo"] + "</td>" +

													"	<td>" + TargetCode + "</td>" +
													"	<td>" + TargetCompany + "</td>" +
													"	<td>" + RS["TotalPackedCount"] + "</td>" +
													"	<td>" + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</td>" +
													"	<td>" + Common.NumberFormat(RS["TotalVolume"] + "") + "</td>" +
													"	<td>" + Common.NumberFormat(RS["TotalGrossWeight"] + "") + "</td>" +
													//"	<td>" + Common.NumberFormat(FreightCharge.ToString()) + "</td>" +
													//"	<td>" + Common.NumberFormat(RS["TSum"] + "") + "</td>" +
													//"	<td>" + Common.NumberFormat(DeliveryCharge.ToString()) + "</td>" +
													//"	<td>" + Common.NumberFormat(TotalCharge + "") + "</td>" +
													"</tr>");
		}
		RS.Dispose();
		return "<br /><table border='1' cellpadding='0' cellspacing='0' style=\"width:850px;\"><tr height='30px;'>" +
						"	<td class='THead1' style=\"width:55px;\" >구분</td>" +
				  "	<td class='THead1' style=\"width:70px;\" >출고일</td>" +
						"	<td class='THead1' style=\"width:55px;\" >BLNo</td>" +
						"	<td class='THead1' style=\"width:55px;\" >거래처 고객번호</td>" +
						"	<td class='THead1' style=\"width:55px;\" >거래처 상호</td>" +
						"	<td class='THead1' style=\"width:75px;\" >포장수량</td>" +
						"	<td class='THead1' style=\"width:90px;\" >&nbsp;</td>" +
						"	<td class='THead1' style=\"width:70px;\" >체적</td>" +
						"	<td class='THead1' style=\"width:70px;\" >중량</td>" +
						//"	<td class='THead1' style=\"width:70px;\" >운송비</td>" +
						//"	<td class='THead1' style=\"width:70px;\" >Tariff</td>" +
						//"	<td class='THead1' style=\"width:70px;\" >DeliveryCharge</td>" +
						//"	<td class='THead1' style=\"width:70px;\" >비용합계</td>" +
						"	</tr>" + ReturnValue + "</table>";
	}

	private String LoadAbjiListForClearance(string BBPk) {
		string HtmlFormat_EachRow = "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td></tr>";
		string HtmlFormat_EachRowSub = "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>{8}</td><td>{9}</td><td>&nbsp;</td></tr>";

		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"
DECLARE @TBBPk int;
DECLARE @TBStep tinyint;
DECLARE @ToDateTime varchar(50);

SET @TBBPk=" + BBPk + @";

SELECT @TBStep=isnull(BBHead.[TRANSPORT_STATUS], ''), @ToDateTime=BBHead.ToDateTime
FROM [dbo].[TRANSPORT_HEAD] AS BBHead
WHERE BBHead.[TRANSPORT_PK]=@TBBPk;

if (@TBStep='3' or @TBStep='')
BEGIN
	SELECT
		@ToDateTime AS ToDateTime
		, Storage.RequestFormPk, Storage.BoxCount
		, R.RequestFormPk, R.ShipperPk, R.ConsigneePk , R.ShipperCode, R.ConsigneeCode , R.DepartureDate, R.ArrivalDate, R.TransportWayCL , R.StepCL
		, CC.CompanyName , CC.CompanyNo
		, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, CalcH.[TOTAL_PRICE], CalcH.[DEPOSITED_PRICE], CalcH.[LAST_DEPOSITED_DATE]
		, CD.BLNo
		, OBS.StockedDate
		, OBS.StatusCL
		, T1.[ORIGINAL_PRICE] as T1V, T2.[ORIGINAL_PRICE] as T2V, T3.[ORIGINAL_PRICE] as T3V
		, C1.[ORIGINAL_PRICE] AS C1V, C2.[ORIGINAL_PRICE] AS C2V, C3.[ORIGINAL_PRICE] AS C3V, C4.[ORIGINAL_PRICE] AS C4V
	FROM
		TransportBBHistory AS Storage
		left join RequestForm AS R on Storage.RequestFormPk=R.RequestFormPk
		Left join Company AS CC on R.ConsigneePk=CC.CompanyPk
		Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as CalcH on Storage.RequestFormPk=CalcH.[TABLE_PK] 
		left join CommerdialConnectionWithRequest AS CCWR On Storage.RequestFormPk=CCWR.RequestFormPk
		left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
		left join (select StatusCL,StockedDate,RequestFormPk from OurBranchStorageOut group by StatusCL,StockedDate,RequestFormPk) AS OBS On Storage.RequestFormPk=OBS.RequestFormPk
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세' 
					) AS T1 ON T1.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK] 
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='부가세'
					) AS T2 ON T2.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK]
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세사비'
					) AS T3 ON T3.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK]   
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='보수작업료' AND [ORIGINAL_MONETARY_UNIT] = 20
					) AS C1 ON C1.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK] 
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='인증료' AND [ORIGINAL_MONETARY_UNIT] = 20
					) AS C2 ON C2.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK] 
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='창고료' AND [ORIGINAL_MONETARY_UNIT] = 20
					) AS C3 ON C3.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK] 
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='임시개청료' AND [ORIGINAL_MONETARY_UNIT] = 20
					) AS C4 ON C4.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK] 
	WHERE Storage.TransportBetweenBranchPk=@TBBPk
	AND ISNULL(CalcH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
	ORDER BY R.ConsigneeCode, Storage.RequestFormPk;
END
else
BEGIN
	SELECT
		@ToDateTime AS ToDateTime
		, Storage.[REQUEST_PK], Storage.[PACKED_COUNT]
		, R.RequestFormPk, R.ShipperPk, R.ConsigneePk , R.ShipperCode, R.ConsigneeCode , R.DepartureDate, R.ArrivalDate, R.TransportWayCL , R.StepCL
		, CC.CompanyName , CC.CompanyNo
		, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, CalcH.[TOTAL_PRICE], CalcH.[DEPOSITED_PRICE], CalcH.[LAST_DEPOSITED_DATE]
		, CD.BLNo
		, OBS.StockedDate
		, OBS.StatusCL
		, T1.[ORIGINAL_PRICE] as T1V, T2.[ORIGINAL_PRICE] as T2V, T3.[ORIGINAL_PRICE] as T3V
		, C1.[ORIGINAL_PRICE] AS C1V, C2.[ORIGINAL_PRICE] AS C2V, C3.[ORIGINAL_PRICE] AS C3V, C4.[ORIGINAL_PRICE] AS C4V
	FROM
		[dbo].[TRANSPORT_BODY] AS Storage
		left join RequestForm AS R on Storage.[REQUEST_PK]=R.RequestFormPk
		Left join Company AS CC on R.ConsigneePk=CC.CompanyPk
		Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as CalcH on Storage.[REQUEST_PK]=CalcH.[TABLE_PK] 
		left join CommerdialConnectionWithRequest AS CCWR On Storage.[REQUEST_PK]=CCWR.RequestFormPk
		left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
		left join (select StatusCL,StockedDate,RequestFormPk from OurBranchStorageOut group by StatusCL,StockedDate,RequestFormPk) AS OBS On Storage.[REQUEST_PK]=OBS.RequestFormPk
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세' 
					) AS T1 ON T1.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK] 
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='부가세'
					) AS T2 ON T2.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK]
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세사비'
					) AS T3 ON T3.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK]   
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='보수작업료' AND [ORIGINAL_MONETARY_UNIT] = 20
					) AS C1 ON C1.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK] 
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='인증료' AND [ORIGINAL_MONETARY_UNIT] = 20
					) AS C2 ON C2.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK] 
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='검역료' AND [ORIGINAL_MONETARY_UNIT] = 20
					) AS C3 ON C3.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK] 
		left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='임시개청료' AND [ORIGINAL_MONETARY_UNIT] = 20
					) AS C4 ON C4.[REQUESTFORMCALCULATE_HEAD_PK]=CalcH.[REQUESTFORMCALCULATE_HEAD_PK] 
	WHERE Storage.[TRANSPORT_HEAD_PK]=@TBBPk 
	AND ISNULL(CalcH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
	ORDER BY R.ConsigneeCode, Storage.[REQUEST_PK];
END;";
		//Response.Write(DB.SqlCmd.CommandText + "<br /><br />");
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string[] TRData = new string[18];

			TRData[0] = (RS["ToDateTime"] + "").Substring(0, 11);
			TRData[1] = RS["BLNo"] + "";
			TRData[2] = RS["ConsigneeCode"] + "";
			TRData[3] = RS["CompanyName"] + "";
			TRData[4] = RS["PACKED_COUNT"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
			TRData[5] = Common.NumberFormat(RS["T1V"] + "");
			TRData[6] = Common.NumberFormat(RS["T2V"] + "");
			TRData[7] = Common.NumberFormat(RS["T3V"] + "");
			TRData[8] = "";
			TRData[9] = "";
			bool isFirst = true;
			if (RS["C1V"] + "" != "") {
				if (isFirst) {
					TRData[8] = "보수작업료";
					TRData[9] = Common.NumberFormat(RS["C1V"] + "");
					isFirst = false;
					ReturnValue.Append(string.Format(HtmlFormat_EachRow, TRData));
				} else {
					TRData[8] = "보수작업료";
					TRData[9] = Common.NumberFormat(RS["C1V"] + "");
					ReturnValue.Append(string.Format(HtmlFormat_EachRowSub, TRData));
				}
			}

			if (RS["C2V"] + "" != "") {
				if (isFirst) {
					TRData[8] = "인증료";
					TRData[9] = Common.NumberFormat(RS["C2V"] + "");
					isFirst = false;
					ReturnValue.Append(string.Format(HtmlFormat_EachRow, TRData));
				} else {
					TRData[8] = "인증료";
					TRData[9] = Common.NumberFormat(RS["C2V"] + "");
					ReturnValue.Append(string.Format(HtmlFormat_EachRowSub, TRData));
				}
			}
			if (RS["C3V"] + "" != "") {
				if (isFirst) {
					TRData[8] = "검역료";
					TRData[9] = Common.NumberFormat(RS["C3V"] + "");
					isFirst = false;
					ReturnValue.Append(string.Format(HtmlFormat_EachRow, TRData));
				} else {
					TRData[8] = "검역료";
					TRData[9] = Common.NumberFormat(RS["C3V"] + "");
					ReturnValue.Append(string.Format(HtmlFormat_EachRowSub, TRData));
				}
			}
			if (RS["C4V"] + "" != "") {
				if (isFirst) {
					TRData[8] = "임시개청료";
					TRData[9] = Common.NumberFormat(RS["C4V"] + "");
					isFirst = false;
					ReturnValue.Append(string.Format(HtmlFormat_EachRow, TRData));
				} else {
					TRData[8] = "임시개청료";
					TRData[9] = Common.NumberFormat(RS["C4V"] + "");
					ReturnValue.Append(string.Format(HtmlFormat_EachRowSub, TRData));
				}
			}

			if (isFirst) {
				ReturnValue.Append(string.Format(HtmlFormat_EachRow, TRData));
			}
		}
		RS.Dispose();
		return ReturnValue + "";
	}

	private String LoadAbjiListForJaemu2015(string BBPk, string TYPE) {
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"	SELECT BBHead.[TRANSPORT_STATUS]
															FROM [dbo].[TRANSPORT_HEAD] AS BBHead
															WHERE BBHead.[TRANSPORT_PK]=" + BBPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			stepCL = RS["TRANSPORT_STATUS"] + "";
		}
		RS.Dispose();

		string HtmlFormat_EachRow = "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{15}</td><td>{16}</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{17}</td><td>{6}</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>{7}</td>";//<td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td><td>{14}</td></tr>
		ReturnValue = new StringBuilder();
		string Table;
		string HeadColumn;
		string RequestColumn;
		string CountColumn;
		string TempQ;
		if (stepCL == "" || stepCL == "3") {
			Table = "TransportBBHistory";
			HeadColumn = "[TransportBetweenBranchPk]";
			RequestColumn = "[RequestFormPk]";
			CountColumn = "[BoxCount]";
			TempQ = "";
		} else {
			Table = "[dbo].[TRANSPORT_BODY]";
			HeadColumn = "[TRANSPORT_HEAD_PK]";
			RequestColumn = "[REQUEST_PK]";
			CountColumn = "[PACKED_COUNT]";
			TempQ = " ";
		}
		string QueryWhere = "";
		switch (TYPE) {
			case "ALL":
				break;

			case "12":
				QueryWhere = "and (right(R.Consigneecode,1) in ('1', '2') OR  right(R.Consigneecode,2) in ('1A','1B','1C','1D','1E','1F','1G','1H','1I','1J', '2A','2B','2C','2D','2E','2F','2G','2H','2I','2J')  )";
				break;

			case "34":
				QueryWhere = "and (right(R.Consigneecode,1) in ('3', '4') OR  right(R.Consigneecode,2) in ('3A','3B','3C','3D','3E','3F','3G','3H','3I','3J', '4A','4B','4C','4D','4E','4F','4G','4H','4I','4J')  )";
				break;

			case "5":
				QueryWhere = "and (right(R.Consigneecode,1) in ('5') OR  right(R.Consigneecode,2) in ('5A','5B','5C','5D','5E','5F','5G','5H','5I','5J')  )";
				break;

			case "67":
				QueryWhere = " and (right(R.Consigneecode,1) in ('6','7') OR  right(R.Consigneecode,2) in ('6A','6B','6C','6D','6E','6F','6G','6H','6I','6J','7A','7B','7C','7D','7E','7F','7G','7H','7I','7J')  )";
				break;

			case "89":
				QueryWhere = "and (right(R.Consigneecode,1) in ('8','9') OR  right(R.Consigneecode,2) in ('8A','8B','8C','8D','8E','8F','8G','8H','8I','8J', '9A','9B','9C','9D','9E','9F','9G','9H','9I','9J')  )";
				break;

			case "0":
				QueryWhere = "and (right(R.Consigneecode,1) in ('0') OR  right(R.Consigneecode,2) in ('0A','0B','0C','0D','0E','0F','0G','0H','0I','0J')  )";
				break;
		}
		DB.SqlCmd.CommandText = @"
SELECT
	Storage." + RequestColumn + @", Storage." + CountColumn + @" ,
	R.RequestFormPk, R.ShipperPk, R.ConsigneePk , R.ShipperCode, R.ConsigneeCode , R.DepartureDate, R.ArrivalDate, R.TransportWayCL , R.StepCL,
	CC.CompanyName , CC.CompanyNo,
	R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, CalcH.[TOTAL_PRICE], CalcH.[DEPOSITED_PRICE], CalcH.[LAST_DEPOSITED_DATE],
	CD.BLNo ,
	OBS.StockedDate ,
	OBS.StatusCL ,
	TBC.Title ,
	isnull(altnrma.P, 0) AS altnrmaSum,
	isnull(qnrktp.P, 0) AS qnrktpSum
FROM
	" + Table + @" AS Storage
	left join RequestForm AS R on Storage." + RequestColumn + @"=R.RequestFormPk
	Left join Company AS CC on R.ConsigneePk=CC.CompanyPk
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as CalcH ON Storage." + RequestColumn + @"=CalcH.[TABLE_PK] 
	left join CommerdialConnectionWithRequest AS CCWR On Storage." + RequestColumn + @"=CCWR.RequestFormPk
	left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK]
			  ,SUM([ORIGINAL_PRICE]) AS P
		  FROM [dbo].[REQUESTFORMCALCULATE_BODY]
		  WHERE (Title='과입금' or Title='미수금' or Title='임시개청비' or Title='인증료' or Title='보수작업비')
		  AND [ORIGINAL_MONETARY_UNIT]=20
		  GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		) AS altnrma ON CalcH.[REQUESTFORMCALCULATE_HEAD_PK]=altnrma.[REQUESTFORMCALCULATE_HEAD_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK]
			  ,SUM([ORIGINAL_PRICE]) AS P
		  FROM [dbo].[REQUESTFORMCALCULATE_BODY]
		  WHERE (Title like('%(VAT포함)%') OR Title like('%(부가세포함)%')) 
		  AND [ORIGINAL_MONETARY_UNIT]=20
		  GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
	) AS qnrktp CalcH.[REQUESTFORMCALCULATE_HEAD_PK]=qnrktp.[REQUESTFORMCALCULATE_HEAD_PK]
WHERE Storage." + HeadColumn + @"=" + BBPk + TempQ + QueryWhere + @"
AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
ORDER BY R.ConsigneeCode, Storage." + RequestColumn + @";";
		//Response.Write(DB.SqlCmd.CommandText+"<br /><br />");

		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string[] TRData = new string[18];

			TRData[0] = RS["ArrivalDate"] + "" == "" ? "" : (RS["ArrivalDate"] + "").Substring(0, 4);
			TRData[1] = RS["ArrivalDate"] + "" == "" ? "" : Int32.Parse((RS["ArrivalDate"] + "").Substring(4, 2)).ToString();
			TRData[2] = RS["ArrivalDate"] + "" == "" ? "" : Int32.Parse((RS["ArrivalDate"] + "").Substring(6)).ToString();
			TRData[3] = RS["CompanyName"] + "";
			string tempConsignee = RS["Consignee"] + "";
			if (tempConsignee.LastIndexOf("(") < tempConsignee.LastIndexOf(")")) {
				string tempCompanyNo = tempConsignee.Substring(tempConsignee.LastIndexOf("(") + 1, tempConsignee.LastIndexOf(")") - tempConsignee.LastIndexOf("(") - 1);
				if (tempCompanyNo.Trim().Length == 12) {
					TRData[4] = tempCompanyNo;
				} else {
					TRData[4] = RS["CompanyNo"] + "";
				}
			} else {
				TRData[4] = RS["CompanyNo"] + "";
			}

			decimal TotalCharge = RS["TOTAL_PRICE"] + "" == "" ? 0 : decimal.Parse(RS["TOTAL_PRICE"] + "");

			decimal OF_Tariff = RS["TOTAL_PRICE"] + "" == "" ? 0 : decimal.Parse(RS["TOTAL_PRICE"] + "");
			if (RS["altnrmaSum"] + "" != "") {
				OF_Tariff -= decimal.Parse(RS["altnrmaSum"] + "");
			}
			if (RS["qnrktpSum"] + "" != "0.0000") {
				OF_Tariff -= decimal.Parse(RS["qnrktpSum"] + "");
			}

			TRData[5] = Common.NumberFormat(OF_Tariff + "");
			TRData[6] = "해운비 " + RS["BLNo"] + " - " + RS["PACKED_COUNT"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
			TRData[7] = RS["BLNo"] + "";
			TRData[15] = "1";
			TRData[16] = "12";
			TRData[17] = "";
			ReturnValue.Append(string.Format(HtmlFormat_EachRow, TRData));

			decimal qnrktpSum = 0;
			if (RS["qnrktpSum"] + "" != "0.0000") {
				qnrktpSum += decimal.Parse(RS["qnrktpSum"] + "");
			}

			if (qnrktpSum > 0) {
				TRData[5] = Common.NumberFormat((qnrktpSum * 10 / 11).ToString());
				TRData[6] = "대행비 " + RS["BLNo"] + " - " + RS["PACKED_COUNT"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
				TRData[8] = "";
				TRData[9] = "";
				TRData[10] = "";
				TRData[11] = "";
				TRData[12] = "";
				TRData[13] = "";
				TRData[14] = "";
				TRData[16] = "11";
				TRData[17] = Common.NumberFormat((qnrktpSum / 11).ToString());
				ReturnValue.Append(string.Format(HtmlFormat_EachRow, TRData));
			}
			if (RS["altnrmaSum"] + "" != "0.0000") {
				TRData[5] = Common.NumberFormat(decimal.Parse(RS["altnrmaSum"] + "") + "");
				TRData[6] = "미수금 " + RS["BLNo"] + " - " + RS["PACKED_COUNT"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
				TRData[8] = "";
				TRData[9] = "";
				TRData[10] = "";
				TRData[11] = "";
				TRData[12] = "";
				TRData[13] = "";
				TRData[14] = "";
				TRData[15] = "KR";
				TRData[16] = "";
				TRData[17] = "";

				ReturnValue.Append(string.Format(HtmlFormat_EachRow, TRData));
			}
		}
		RS.Dispose();
		return ReturnValue + "";
	}

	private String LoadAbjiListForJaemu(string BBPk, string TYPE) {
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"	SELECT BBHead.[TRANSPORT_STATUS]
															FROM [dbo].[TRANSPORT_HEAD] AS BBHead
															WHERE BBHead.[TRANSPORT_PK]=" + BBPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			stepCL = RS["TRANSPORT_STATUS"] + "";
		}
		RS.Dispose();

		string HtmlFormat_EachRow = "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{15}</td><td>{16}</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{17}</td><td>{6}</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td><td>{14}</td></tr>";
		ReturnValue = new StringBuilder();
		string Table;
		string HeadColumn;
		string RequestColumn;
		string CountColumn;
		string TempQ;
		if (stepCL == "" || stepCL == "3") {
			Table = "TransportBBHistory";
			HeadColumn = "[TransportBetweenBranchPk]";
			RequestColumn = "[RequestFormPk]";
			CountColumn = "[BoxCount]";
			TempQ = "";
		}
		else {
			Table = "[dbo].[TRANSPORT_BODY]";
			HeadColumn = "[TRANSPORT_HEAD_PK]";
			RequestColumn = "[REQUEST_PK]";
			CountColumn = "[PACKED_COUNT]";
			TempQ = " ";
		}
		string QueryWhere = "";
		switch (TYPE) {
			case "ALL":

				break;

			case "14":
				QueryWhere = "and right(R.Consigneecode,1) in ('1','2','3','4')";
				break;

			case "57":
				QueryWhere = "and right(R.Consigneecode,1) in ('5','6','7')";
				break;

			case "80":
				QueryWhere = "and right(R.Consigneecode,1) in ('8','9','0')";
				break;
		}
		DB.SqlCmd.CommandText = @"
SELECT
	Storage." + RequestColumn + @", Storage." + CountColumn + @" ,
	R.RequestFormPk, R.ShipperPk, R.ConsigneePk , R.ShipperCode, R.ConsigneeCode , R.DepartureDate, R.ArrivalDate, R.TransportWayCL , R.StepCL,
	CC.CompanyName , CC.CompanyNo,
	R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, CalcH.[TOTAL_PRICE], CalcH.[DEPOSITED_PRICE], CalcH.[LAST_DEPOSITED_DATE],
	CD.BLNo ,
	OBS.StockedDate ,
	OBS.StatusCL ,
	TBC.Title ,
	isnull(altnrma.P, 0) AS altnrmaSum,
	isnull(qnrktp.P, 0) AS qnrktpSum
FROM
	" + Table + @" AS Storage
	left join RequestForm AS R on Storage." + RequestColumn + @"=R.RequestFormPk
	Left join Company AS CC on R.ConsigneePk=CC.CompanyPk
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as CalcH ON Storage." + RequestColumn + @"=CalcH.[TABLE_PK] 
	left join CommerdialConnectionWithRequest AS CCWR On Storage." + RequestColumn + @"=CCWR.RequestFormPk
	left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
	left join (select Title,RequestFormPk from TransportBC group by Title,RequestFormPk) AS TBC On Storage." + RequestColumn + @"=TBC.RequestFormPk
	left join (select StatusCL,StockedDate,RequestFormPk from OurBranchStorageOut group by StatusCL,StockedDate,RequestFormPk) AS OBS On Storage." + RequestColumn + @"=OBS.RequestFormPk
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK]
			  ,SUM([ORIGINAL_PRICE]) AS P
		  FROM [dbo].[REQUESTFORMCALCULATE_BODY]
		  WHERE (Title='과입금' or Title='미수금' or Title='임시개청비' or Title='인증료' or Title='보수작업비')
		  AND [ORIGINAL_MONETARY_UNIT]=20
		  GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		) AS altnrma ON CalcH.[REQUESTFORMCALCULATE_HEAD_PK]=altnrma.[REQUESTFORMCALCULATE_HEAD_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK]
			  ,SUM([ORIGINAL_PRICE]) AS P
		  FROM [dbo].[REQUESTFORMCALCULATE_BODY]
		  WHERE (Title like('%(VAT포함)%') OR Title like('%(부가세포함)%')) 
		  AND [ORIGINAL_MONETARY_UNIT]=20
		  GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
	) AS qnrktp CalcH.[REQUESTFORMCALCULATE_HEAD_PK]=qnrktp.[REQUESTFORMCALCULATE_HEAD_PK]
WHERE Storage." + HeadColumn + @"=" + BBPk + TempQ + QueryWhere + @"
AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
ORDER BY R.ConsigneeCode, Storage." + RequestColumn + @";";

		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string[] TRData = new string[18];

			TRData[0] = (RS["ArrivalDate"] + "").Substring(0, 4);
			TRData[1] = Int32.Parse((RS["ArrivalDate"] + "").Substring(4, 2)).ToString();
			TRData[2] = Int32.Parse((RS["ArrivalDate"] + "").Substring(6)).ToString();
			TRData[3] = RS["CompanyName"] + "";
			TRData[4] = RS["CompanyNo"] + "";

			decimal TotalCharge = RS["TOTAL_PRICE"] + "" == "" ? 0 : decimal.Parse(RS["TOTAL_PRICE"] + "");

			string ChargeLeft = "";

			if (RS["DEPOSITED_PRICE"] + "" != "" && RS["TOTAL_PRICE"] + "" != "") {
				ChargeLeft = Common.NumberFormat(TotalCharge - Decimal.Parse(RS["DEPOSITED_PRICE"] + "") + "");
			}
			string StatusCL;
			string FromDate;
			if (RS["StatusCL"] + "" == "5" || RS["StatusCL"] + "" == "6") {
				StatusCL = "出";
				FromDate = (RS["StockedDate"] + "" == "" ? "" : (RS["StockedDate"] + "").Substring(5, 5));
			} else {
				StatusCL = "";
				FromDate = "";
			}

			string DeliveryTitle = "";
			if (RS["title"] + "" == "직출") {
				DeliveryTitle = "직출";
			} else if (RS["title"] + "" == "화주님 직출") {
				DeliveryTitle = "화주님 직출";
			} else if (RS["title"] + "" == "도착지 화주직출") {
				DeliveryTitle = "도착지 화주직출";
			} else if (RS["title"] + "" == "자체통관출고" || RS["title"] + "" == "자체통관 출고") {
				DeliveryTitle = "자체통관출고";
			}

			decimal OF_Tariff = RS["TOTAL_PRICE"] + "" == "" ? 0 : decimal.Parse(RS["TOTAL_PRICE"] + "");
			if (RS["altnrmaSum"] + "" != "") {
				OF_Tariff -= decimal.Parse(RS["altnrmaSum"] + "");
			}
			if (RS["qnrktpSum"] + "" != "0.0000") {
				OF_Tariff -= decimal.Parse(RS["qnrktpSum"] + "");
			}

			TRData[5] = Common.NumberFormat(OF_Tariff + "");
			TRData[6] = "해운비 " + Common.NumberFormat(TotalCharge + "") + "" + " " + RS["BoxCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "") + " - " + RS["BLNo"];
			TRData[7] = RS["BLNo"] + "";

			TRData[8] = (RS["LAST_DEPOSITED_DATE"] + "" == "" ? "" : (RS["LAST_DEPOSITED_DATE"] + "").Substring(0, 10));
			TRData[9] = Common.NumberFormat(RS["DEPOSITED_PRICE"] + "");
			TRData[10] = ChargeLeft;
			TRData[11] = (RS["LAST_DEPOSITED_DATE"] + "" == "" ? "" : (RS["LAST_DEPOSITED_DATE"] + "").Substring(0, 10));
			TRData[12] = Common.NumberFormat(RS["DEPOSITED_PRICE"] + "");
			TRData[13] = ChargeLeft;
			TRData[14] = StatusCL + FromDate + DeliveryTitle;
			TRData[15] = "1";
			TRData[16] = "12";
			TRData[17] = "";
			ReturnValue.Append(string.Format(HtmlFormat_EachRow, TRData));

			if (RS["qnrktpSum"] + "" != "0.0000") {
				TRData[5] = Common.NumberFormat((decimal.Parse(RS["qnrktpSum"] + "") * 10 / 11).ToString());
				TRData[6] = "과세 " + Common.NumberFormat(TotalCharge + "") + "" + " " + RS["BoxCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "") + " - " + RS["BLNo"];
				TRData[8] = "";
				TRData[9] = "";
				TRData[10] = "";
				TRData[11] = "";
				TRData[12] = "";
				TRData[13] = "";
				TRData[14] = "";
				TRData[16] = "11";
				TRData[17] = Common.NumberFormat((decimal.Parse(RS["qnrktpSum"] + "") / 11).ToString());

				ReturnValue.Append(string.Format(HtmlFormat_EachRow, TRData));
			}
			if (RS["altnrmaSum"] + "" != "") {
				TRData[5] = Common.NumberFormat(decimal.Parse(RS["qnrktpSum"] + "") + "");
				TRData[6] = "미수금 " + Common.NumberFormat(TotalCharge + "") + "" + " " + RS["BoxCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "") + " - " + RS["BLNo"];
				TRData[8] = "";
				TRData[9] = "";
				TRData[10] = "";
				TRData[11] = "";
				TRData[12] = "";
				TRData[13] = "";
				TRData[14] = "";
				TRData[15] = "KR";
				TRData[16] = "";
				TRData[17] = "";

				ReturnValue.Append(string.Format(HtmlFormat_EachRow, TRData));
			}
		}
		RS.Dispose();
		return ReturnValue + "";
	}

	private String LoadAbjiList(string BBPk) {
		StringBuilder ReturnValue = new StringBuilder();
		string Table;
		string HeadColumn;
		string RequestColumn;
		string CountColumn;
		string TempQ;
		if (stepCL == "" || stepCL == "3") {
			Table = "TransportBBHistory";
			HeadColumn = "[TransportBetweenBranchPk]";
			RequestColumn = "[RequestFormPk]";
			CountColumn = "[BoxCount]";
			TempQ = "";
		} else {
			Table = "[dbo].[TRANSPORT_BODY]";
			HeadColumn = "[TRANSPORT_HEAD_PK]";
			RequestColumn = "[REQUEST_PK]";
			CountColumn = "[PACKED_COUNT]";
			TempQ = " ";
		}
		DB.SqlCmd.CommandText = @"
SELECT
	Storage." + RequestColumn + @", Storage." + CountColumn + @" ,
	R.RequestFormPk, R.ShipperPk, R.ConsigneePk , R.ShipperCode, R.ConsigneeCode , R.DepartureDate, R.ArrivalDate, R.TransportWayCL , R.StepCL, R.DocumentRequestCL,
	CC.CompanyName , CC.CompanyNo,
	R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, 
	CD.BLNo ,
	OBS.StockedDate ,
	OBS.StatusCL ,
	TBC.Title,
	TB.Contents,
	SA.Value SAValue, CA.Value CAValue
FROM
	" + Table + @" AS Storage
	left join RequestForm AS R on Storage." + RequestColumn + @"=R.RequestFormPk
    left join (select [CONTENTS],[TABLE_PK] from [dbo].[COMMENT] WHERE [CATEGORY] in ('Basic','Request_Confirm ')) AS TB on Storage." + RequestColumn + @" = TB.[TABLE_PK]
	Left join Company AS CC on R.ConsigneePk=CC.CompanyPk
	Left join (SELECT * FROM CompanyAdditionalInfomation where TITLE=80) SA on R.ShipperPk = SA.CompanyPk
	Left join (SELECT * FROM CompanyAdditionalInfomation where TITLE=80) CA on R.ConsigneePk = CA.CompanyPk
	left join CommerdialConnectionWithRequest AS CCWR On Storage." + RequestColumn + @"=CCWR.RequestFormPk
	left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
	left join (select Title,RequestFormPk from TransportBC group by Title,RequestFormPk) AS TBC On Storage." + RequestColumn + @"=TBC.RequestFormPk
	left join (select StatusCL,StockedDate,RequestFormPk from OurBranchStorageOut group by StatusCL,StockedDate,RequestFormPk) AS OBS On Storage." + RequestColumn + @"=OBS.RequestFormPk
WHERE Storage." + HeadColumn + @"=" + BBPk + TempQ + @"
ORDER BY R.ConsigneeCode, Storage." + RequestColumn + @";";

		DataTable ReturnValue1 = new DataTable();
		SqlDataAdapter DA = new SqlDataAdapter(DB.SqlCmd);
		DA.Fill(ReturnValue1);
		DataTable RS = ReturnValue1;

		StringBuilder ReturnValueS = new StringBuilder();
		bool isFirstRow = true;
		string beforeRequestFormPk = "";
		string RowSpanStyle = "";
		for (var i = 0; i < RS.Rows.Count; i++) {
			DataRow row = RS.Rows[i];
			string StatusCL;
			string FromDate;
			if (row["StatusCL"] + "" == "5" || row["StatusCL"] + "" == "6") {
				StatusCL = "出";
				FromDate = (row["StockedDate"] + "" == "" ? "" : (row["StockedDate"] + "").Substring(5, 5));
			} else {
				StatusCL = "";
				FromDate = "";
			}
			string DeliveryTitle;
			switch (row["title"] + "") {
				//	case "직출":
				//DeliveryTitle = "직출";
				//		break;
				//	case"화주님 직출":
				//	DeliveryTitle = "직출";
				//			break;
				//	case"도착지 화주직출":
				//				DeliveryTitle = "화주직출";
				//				break;
				case "자체통관 출고":
					DeliveryTitle = "자통출고";
					break;

				case "자체통관출고":
					DeliveryTitle = "자통출고";
					break;

				default:
					DeliveryTitle = "";
					break;
			}
			string CO = row["DocumentRequestCL"] + "";
			string COCO = "";
			string[] DocumentRequestCL = CO.Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
			foreach (string T in DocumentRequestCL) {
				switch (T) {
					case "31":
						COCO += "화주원산지제공";
						break;

					case "32":
						COCO += "원산지신청";
						break;

					case "34":
						COCO += "FTA";
						break;
				}
			}
			string Pyeongtaek = "";
			Pyeongtaek += row["SAValue"] + "" == "" ? "" : "S 금지(禁止平仄)";
			Pyeongtaek += row["CAValue"] + "" == "" ? "" : "C 금지(禁止平仄)";
			string Contents = row["Contents"] + "";
			//엑셀다운시 string 데이터크기로 인한 오류발생시 (RS["ArrivalDate"] + "").Substring(4) 의.Substring(4)를 지운다.

			isFirstRow = beforeRequestFormPk == row["RequestFormPk"] + "" ? false : true;
			if (isFirstRow) {
				beforeRequestFormPk = RS.Rows[i]["RequestFormPk"] + "";
				var countRowspan = 1;
				while (true) {
					if (i + countRowspan < RS.Rows.Count && beforeRequestFormPk == RS.Rows[i + countRowspan]["RequestFormPk"] + "") {
						countRowspan++;
						continue;
					}
					break;
				}
				if (countRowspan > 1) {
					RowSpanStyle = countRowspan.ToString();
				} else {
					RowSpanStyle = countRowspan.ToString();
				}
			}
			string TableRow_Rowspan = "<tr><td rowspan=" + RowSpanStyle + ">" + row["BLNo"] + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + row["CompanyNo"] + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + row["ConsigneeCode"] + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + row["CompanyName"] + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + row["PACKED_COUNT"] + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + Common.GetPackingUnit(row["PackingUnit"] + "") + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + Common.NumberFormat(row["TotalVolume"] + "") + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + Common.NumberFormat(row["TotalGrossWeight"] + "") + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + ((row["ArrivalDate"] + "").Length < 4 ? "&nbsp;" : (row["ArrivalDate"] + "").Substring(4)) + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + COCO + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + Pyeongtaek + "</td>" +
													"	<td>" + Contents + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + DeliveryTitle + "</td>" +
													"</tr>";
			string TableRow_ElseRowspan = "<tr><td>" + Contents + "</td></tr>";
			if (isFirstRow) {
				ReturnValue.Append(TableRow_Rowspan);
			} else {
				ReturnValue.Append(TableRow_ElseRowspan);
			}
		}
		RS.Dispose();
		return "<br /><table border='1' cellpadding='0' cellspacing='0' style=\"width:850px;\"><tr height='30px;'>" +
						"	<td class='THead1' style=\"width:55px;\" >BLNo</td>" +
						"	<td class='THead1' style=\"width:55px;\" >사업자</td>" +
						"	<td class='THead1' style=\"width:55px;\" >고객번호</td>" +
						"	<td class='THead1' style=\"width:55px;\" >상호</td>" +
						"	<td class='THead1' style=\"width:75px;\" >포장수량</td>" +
						"	<td class='THead1' style=\"width:90px;\" >&nbsp;</td>" +
						"	<td class='THead1' style=\"width:70px;\" >체적</td>" +
						"	<td class='THead1' style=\"width:70px;\" >중량</td>" +
						"	<td class='THead1' style=\"width:70px;\" >출고일</td>" +
						/*
						"	<td class='THead1' style=\"width:70px;\" >Shipper</td>" +
						"	<td class='THead1' style=\"width:70px;\" >Consignee</td>" +
						"	<td class='THead1' style=\"width:70px;\" >Tariff</td>" +
						"	<td class='THead1' style=\"width:70px;\" >DeliveryCharge</td>" +
						"	<td class='THead1' style=\"width:70px;\" >도착지 비용합계</td>" +
						"	<td class='THead1' style=\"width:70px;\" >발화인 입금일</td>" +
						"	<td class='THead1' style=\"width:70px;\" >발화인 입금액</td>" +
						"	<td class='THead1' style=\"width:70px;\" >발화인 차액</td>" +
						"	<td class='THead1' style=\"width:70px;\" >수하인 입금일</td>" +
						"	<td class='THead1' style=\"width:70px;\" >수하인 입금액</td>" +
						"	<td class='THead1' style=\"width:70px;\" >수하인 차액</td>" +
						"	<td class='THead1' style=\"width:70px;\" >出</td>" +
						 */
						"	<td class='THead1' style=\"width:70px;\" >CO</td>" +
						"	<td class='THead1' style=\"width:70px;\" >Pyeongtaek</td>" +
						"	<td class='THead1' style=\"width:70px;\" >Comment</td>" +
						"	<td class='THead1' style=\"width:70px;\" >出</td>" +
						"	<td class='THead1' style=\"width:70px;\" >비고</td>" +
						"	</tr>" + ReturnValue + "</table>";
	}

	private String LoadAbjiList_TodayAbji(string BBPk) {
		StringBuilder ReturnValue = new StringBuilder();
		string Table;
		string HeadColumn;
		string RequestCloumn;
		string TempQ;
		if (stepCL == "" || stepCL == "3") {
			Table = "TransportBBHistory";
			HeadColumn = "[TransportBetweenBranchPk]";
			RequestCloumn = "[RequestFormPk]";
			TempQ = "";
		} else {
			Table = "[dbo].[TRANSPORT_BODY]";
			HeadColumn = "[TRANSPORT_HEAD_PK]";
			RequestCloumn = "[REQUEST_PK]";
			TempQ = " ";
		}
		DB.SqlCmd.CommandText = @"
SELECT
	Storage.[REQUEST_PK], Storage.[PACKED_COUNT] ,
	R.RequestFormPk, R.ShipperPk, R.ConsigneePk , R.ShipperCode, R.ConsigneeCode , R.DepartureDate, R.ArrivalDate, R.TransportWayCL , R.StepCL, R.DocumentRequestCL,
	CC.CompanyName , CC.CompanyNo,
	R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, 
	CD.BLNo ,
	OBS.StockedDate ,
	OBS.StatusCL ,
	TBC.Title,
	TB.Contents
FROM
	" + Table + @" AS Storage
	left join RequestForm AS R on Storage." + RequestCloumn + @"=R.RequestFormPk
    left join (select [CONTENTS],[TABLE_PK] from [dbo].[COMMENT] WHERE [CATEGORY] in ('Basic','Request_Confirm ')) AS TB on Storage." + RequestCloumn + @" = TB.[TABLE_PK]
	Left join Company AS CC on R.ConsigneePk=CC.CompanyPk
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as CalcH on Storage." + RequestCloumn + @"=CalcH.[TABLE_PK] 
	left join CommerdialConnectionWithRequest AS CCWR On Storage." + RequestCloumn + @"=CCWR.RequestFormPk
	left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
	left join (select Title,RequestFormPk from TransportBC group by Title,RequestFormPk) AS TBC On Storage." + RequestCloumn + @"=TBC.RequestFormPk
	left join (select StatusCL,StockedDate,RequestFormPk from OurBranchStorageOut group by StatusCL,StockedDate,RequestFormPk) AS OBS On Storage." + RequestCloumn + @"=OBS.RequestFormPk
WHERE Storage." + HeadColumn + @"=" + BBPk + TempQ + @"
AND ISNULL(CalcH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
ORDER BY R.ConsigneeCode, Storage." + RequestCloumn + @";";

		DataTable ReturnValue1 = new DataTable();
		SqlDataAdapter DA = new SqlDataAdapter(DB.SqlCmd);
		DA.Fill(ReturnValue1);
		DataTable RS = ReturnValue1;

		StringBuilder ReturnValueS = new StringBuilder();
		bool isFirstRow = true;
		string beforeRequestFormPk = "";
		string RowSpanStyle = "";
		for (var i = 0; i < RS.Rows.Count; i++) {
			DataRow row = RS.Rows[i];
			string StatusCL;
			string FromDate;
			if (row["StatusCL"] + "" == "5" || row["StatusCL"] + "" == "6") {
				StatusCL = "出";
				FromDate = (row["StockedDate"] + "" == "" ? "" : (row["StockedDate"] + "").Substring(5, 5));
			} else {
				StatusCL = "";
				FromDate = "";
			}
			string DeliveryTitle;
			switch (row["title"] + "") {
				case "직출":
					DeliveryTitle = "직출";
					break;

				case "화주님 직출":
					DeliveryTitle = "직출";
					break;

				case "도착지 화주직출":
					DeliveryTitle = "직출";
					break;

				case "자체통관 출고":
					DeliveryTitle = "자통출고";
					break;

				case "자체통관출고":
					DeliveryTitle = "자통출고";
					break;

				default:
					DeliveryTitle = "";
					break;
			}
			string CO = row["DocumentRequestCL"] + "";
			string COCO = "";
			string[] DocumentRequestCL = CO.Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
			foreach (string T in DocumentRequestCL) {
				switch (T) {
					case "31":
						COCO += "화주원산지제공";
						break;

					case "32":
						COCO += "원산지신청";
						break;
					case "34":
						COCO += "FTA";
						break;
				}
			}

			string Contents = row["Contents"] + "";

			isFirstRow = beforeRequestFormPk == row["RequestFormPk"] + "" ? false : true;
			if (isFirstRow) {
				beforeRequestFormPk = RS.Rows[i]["RequestFormPk"] + "";
				var countRowspan = 1;
				while (true) {
					if (i + countRowspan < RS.Rows.Count && beforeRequestFormPk == RS.Rows[i + countRowspan]["RequestFormPk"] + "") {
						countRowspan++;
						continue;
					}
					break;
				}
				if (countRowspan > 1) {
					RowSpanStyle = countRowspan.ToString();
				} else {
					RowSpanStyle = countRowspan.ToString();
				}
			}
			string TableRow_Rowspan = "<tr><td rowspan=" + RowSpanStyle + ">" + row["BLNo"] + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + row["ConsigneeCode"] + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + row["CompanyName"] + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + row["PACKED_COUNT"] + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + Common.GetPackingUnit(row["PackingUnit"] + "") + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + Common.NumberFormat(row["TotalVolume"] + "") + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + Common.NumberFormat(row["TotalGrossWeight"] + "") + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + ((row["ArrivalDate"] + "").Length < 4 ? "&nbsp;" : (row["ArrivalDate"] + "").Substring(4)) + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + COCO + "</td>" +
													"	<td>" + Contents + "</td>" +
													"	<td rowspan=" + RowSpanStyle + ">" + DeliveryTitle + "</td>" +
													"</tr>";
			string TableRow_ElseRowspan = "<tr><td>" + Contents + "</td></tr>";
			if (isFirstRow) {
				ReturnValue.Append(TableRow_Rowspan);
			} else {
				ReturnValue.Append(TableRow_ElseRowspan);
			}
		}
		RS.Dispose();
		return "<br /><table border='1' cellpadding='0' cellspacing='0' style=\"width:850px;\"><tr height='30px;'>" +
						"	<td class='THead1' style=\"width:55px;\" >BLNo</td>" +
						"	<td class='THead1' style=\"width:55px;\" >고객번호</td>" +
						"	<td class='THead1' style=\"width:55px;\" >상호</td>" +
						"	<td class='THead1' style=\"width:75px;\" >포장수량</td>" +
						"	<td class='THead1' style=\"width:90px;\" >&nbsp;</td>" +
						"	<td class='THead1' style=\"width:70px;\" >체적</td>" +
						"	<td class='THead1' style=\"width:70px;\" >중량</td>" +
						"	<td class='THead1' style=\"width:70px;\" >출고일</td>" +
						"	<td class='THead1' style=\"width:70px;\" >CO</td>" +
						"	<td class='THead1' style=\"width:70px;\" >Comment</td>" +
						"	<td class='THead1' style=\"width:70px;\" >出</td>" +
						"	<td class='THead1' style=\"width:70px;\" >비고</td>" +
						"	</tr>" + ReturnValue + "</table>";
	}

	private String LoadJukhwaList(string BBPk) {
		string Table;
		string TempQ;

		if (stepCL == "" || stepCL == "3") {
			Table = "TransportBBHistory";
			TempQ = "";
		} else {
			Table = "[dbo].[TRANSPORT_BODY]";
			TempQ = " ";
		}

		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"
SELECT Storage.[REQUEST_PK], Storage.[PACKED_COUNT] , R.RequestFormPk, R.ShipperPk, R.ConsigneePk
	, R.ShipperCode, R.ConsigneeCode , R.DepartureDate, R.ArrivalDate, R.TransportWayCL
	, CC.CompanyName
	, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, RFI.Description, CICK.CICKD
	, RFAI.[ACCOUNT_ID], RFAI.[CONTENTS], RFAI.[REGISTERD] , Departure.OurBranchCode AS DOBC, Arrival.OurBranchCode AS AOBC
	, Initial.Initial, CD.BLNo, CD.Shipper, CD.Consignee
FROM
	" + Table + @" AS Storage left join
	RequestForm AS R on Storage.[REQUEST_PK]=R.RequestFormPk Left join
	Company AS CC on R.ConsigneePk=CC.CompanyPk Left join
	RegionCode AS Departure on R.DepartureRegionCode=Departure.RegionCode Left join
	RegionCode AS Arrival on R.ArrivalRegionCode=Arrival.RegionCode left join
	RequestFormItems AS RFI ON R.RequestFormPk=RFI.RequestFormPk left join
	(
		SELECT [RequestFormItemsPk], CICK.ItemCode AS CICKC,CICK.Description AS CICKD
		FROM RequestFormItems AS RFI
			left join  ClearanceItemCodeKOR AS CICK ON RFI.ItemCode=CICK.ItemCode
	)
	AS CICK ON CICK.RequestFormItemsPk=RFI.RequestFormItemsPk left join
	(SELECT [TABLE_PK], [ACCOUNT_ID], [CONTENTS], [REGISTERD] FROM [dbo].[COMMENT] WHERE [TABLE_NAME] = 'RequestForm' AND [CATEGORY]='Basic') AS RFAI ON R.RequestFormPk=RFAI.[TABLE_PK]
	left join TransportWayCLDescription AS Initial ON Initial.TransportWayCL=R.TransportWayCL
	left join CommerdialConnectionWithRequest AS CCWR On R.RequestFormPk=CCWR.RequestFormPk
	left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
WHERE Storage.[TRANSPORT_HEAD_PK]=" + BBPk + TempQ + @"
ORDER BY BLNo ASC, CICK.CICKC ASC;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string BeforeRequest = "";
		List<string> ItemSum = new List<string>();
		StringBuilder itemTemp;
		int count = 0;
		while (RS.Read()) {
			if (BeforeRequest == RS["REQUEST_PK"] + "") {
				bool isinitemsum = false;
				if (RS["CICKD"] + "" != "") {
					foreach (string each in ItemSum) {
						if (each == RS["CICKD"] + "") {
							isinitemsum = true;
							break;
						}
					}
				}
				if (!isinitemsum) {
					if (RS["CICKD"] + "" != "")
						ItemSum.Add(RS["CICKD"] + "");
				}
				continue;
			} else {
				if (BeforeRequest != "") {
					itemTemp = new StringBuilder();
					for (int i = 0; i < ItemSum.Count; i++) {
						if (i == 0)
							itemTemp.Append(ItemSum[i]);
						else
							itemTemp.Append(", " + ItemSum[i]);
					}
					ReturnValue.Append("	<td>" + (itemTemp + "" == "" ? "&nbsp;" : itemTemp + "") + "</td></tr>");
				}
				BeforeRequest = RS["REQUEST_PK"] + "";
				count++;
				ItemSum = new List<string>();
				if (RS["CICKD"] + "" != "") {
					ItemSum.Add(RS["CICKD"] + "");
				}

				ReturnValue.Append("<tr>" +
													"	<td>" + count + "</td>" +
													"	<td>" + RS["BLNo"] + "</td>" +
													"	<td>" + RS["Shipper"] + "</td>" +
													"	<td>" + RS["ConsigneeCode"] + "</td>" +
													"	<td>" + RS["Consignee"] + "</td>" +
													"	<td>" + RS["DepartureDate"] + "</td>" +
													"	<td>" + RS["Initial"] + "</td>" +
													"	<td>" + RS["ArrivalDate"] + "</td>" +
													"	<td>" + RS["PACKED_COUNT"] + "</td>" +
													"	<td>" + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</td>" +
													"	<td>" + RS["TotalPackedCount"] + "</td>" +
													"	<td>" + Common.NumberFormat(RS["TotalVolume"] + "") + "</td>" +
													"	<td>" + Common.NumberFormat(RS["TotalGrossWeight"] + "") + "</td>");
				continue;
			}
		}
		RS.Dispose();
		itemTemp = new StringBuilder();
		for (int i = 0; i < ItemSum.Count; i++) {
			if (i == 0) {
				itemTemp.Append(ItemSum[i]);
			} else {
				itemTemp.Append(", " + ItemSum[i]);
			}
		}
		ReturnValue.Append("	<td>" + itemTemp + "</td></tr>");
		string a = "<table border='1' cellpadding='0' cellspacing='0' ><tr>" +
						"	<td class='THead1' >No</td>" +
						"	<td class='THead1' >BL</td>" +
						"	<td class='THead1' >Shipper</td>" +
						"	<td class='THead1' >Consignee</td>" +
						"	<td class='THead1' >Consignee CompanyName</td>" +
						"	<td class='THead1' >Start</td>" +
						"	<td class='THead1' >Initial</td>" +
						"	<td class='THead1' >Arrival</td>" +
						"	<td class='THead1' >BoxCount</td>" +
						"	<td class='THead1' > </td>" +
						"	<td class='THead1' >TotalCount</td>" +
						"	<td class='THead1' >CBM</td>" +
						"	<td class='THead1' >Kg</td>" +
						"	<td class='THead1' >Description</td></tr>" + ReturnValue + "</table>";
		return a;
	}

	private String LoadJukhwaList2(string BBPk) {
		string Table;
		string TempQ;

		if (stepCL == "" || stepCL == "3") {
			Table = "TransportBBHistory";
			TempQ = "";
		} else {
			Table = "[dbo].[TRANSPORT_BODY]";
			TempQ = " ";
		}

		DB.SqlCmd.CommandText = @"
SELECT
	Storage.[PACKED_COUNT]
	, R.RequestFormPk, R.ConsigneeCode
	, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume
	, CICK.CICKD
	, CD.BLNo, CD.Shipper, CD.ShipperAddress, CD.Consignee, CD.ConsigneeAddress
FROM
	" + Table + @" AS Storage left join
	RequestForm AS R on Storage.[REQUEST_PK]=R.RequestFormPk
	Left join Company AS CC on R.ConsigneePk=CC.CompanyPk
	Left join RegionCode AS Departure on R.DepartureRegionCode=Departure.RegionCode
	Left join RegionCode AS Arrival on R.ArrivalRegionCode=Arrival.RegionCode
	left join RequestFormItems AS RFI ON R.RequestFormPk=RFI.RequestFormPk
	left join (
		SELECT [RequestFormItemsPk], CICK.ItemCode AS CICKC,CICK.Description AS CICKD
		FROM RequestFormItems AS RFI
			left join ClearanceItemCodeKOR AS CICK ON RFI.ItemCode=CICK.ItemCode
		) AS CICK ON CICK.RequestFormItemsPk=RFI.RequestFormItemsPk
	left join (SELECT [TABLE_PK], [ACCOUNT_ID], [CONTENTS], [REGISTERD] FROM [dbo].[COMMENT] WHERE [TABLE_NAME] = 'RequestForm' AND [CATEGORY]='Basic') AS RFAI ON R.RequestFormPk=RFAI.TABLE_PK
	left join CommerdialConnectionWithRequest AS CCWR On R.RequestFormPk=CCWR.RequestFormPk
	left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
WHERE Storage.[TRANSPORT_HEAD_PK]=" + BBPk + TempQ + @"
ORDER BY BLNo ASC, CICK.CICKC ASC;";

		//return DB.SqlCmd.CommandText;
		//Response.Write(DB.SqlCmd.CommandText);
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string TempBLNo = "_";
		string TempRequestFormPk = "";
		string[] RowValue = new string[] { };
		StringBuilder ReturnValue = new StringBuilder();
		List<string> ItemSum = new List<string>();
		StringBuilder itemTemp;
		string rowFormat = "<tr><td>{0}</td>" +
						"	<td>{1}</td>" +
						"	<td>{2}</td>" +
						"	<td>{3}</td>" +
						"	<td>{4}</td>" +
						"	<td>{5}</td>" +
						"	<td>{6}</td>" +
						"	<td>{7}</td>" +
						"	<td>{8}</td>" +
						"	<td>{9}</td>" +
						"	<td>{10}</td>" +
						"	<td>{11}</td></tr>";
		int count = 0;
		int TotalPackedCount = 0;
		decimal TotalGrossWeight = 0;
		decimal TotalVolume = 0;

		while (RS.Read()) {
			if (TempBLNo == "_") {
				count++;
				TempBLNo = RS["BLNo"] + "";
				TempRequestFormPk = RS["RequestFormPk"] + "";
				if (RS["PACKED_COUNT"] + "" != "") {
					TotalPackedCount += Int32.Parse(RS["PACKED_COUNT"] + "");
				}
				if (RS["TotalGrossWeight"] + "" != "") {
					TotalGrossWeight += decimal.Parse(RS["TotalGrossWeight"] + "");
				}
				if (RS["TotalVolume"] + "" != "") {
					TotalVolume += decimal.Parse(RS["TotalVolume"] + "");
				}

				RowValue = new string[] {
					count.ToString(),
					TempBLNo,
					RS["Shipper"]+"",
					RS["ShipperAddress"]+"",
					RS["ConsigneeCode"]+"",
					RS["Consignee"]+"",
					RS["ConsigneeAddress"]+"",
					"",
					Common.GetPackingUnit(RS["PackingUnit"]+""),
					"",
					"",
					""};

				bool isinitemsum = false;
				if (RS["CICKD"] + "" != "") {
					foreach (string each in ItemSum) {
						if (each == RS["CICKD"] + "") {
							isinitemsum = true;
							break;
						}
					}
				}
				if (!isinitemsum) {
					if (RS["CICKD"] + "" != "")
						ItemSum.Add(RS["CICKD"] + "");
				}
				continue;
			}

			if (TempBLNo == RS["BLNo"] + "") {
				if (TempRequestFormPk != RS["RequestFormPk"] + "") {
					TempRequestFormPk = RS["RequestFormPk"] + "";
					if (RS["PACKED_COUNT"] + "" != "") {
						TotalPackedCount += Int32.Parse(RS["PACKED_COUNT"] + "");
					}
					if (RS["TotalGrossWeight"] + "" != "") {
						TotalGrossWeight += decimal.Parse(RS["TotalGrossWeight"] + "");
					}
					if (RS["TotalVolume"] + "" != "") {
						TotalVolume += decimal.Parse(RS["TotalVolume"] + "");
					}
				}
				bool isinitemsum = false;
				if (RS["CICKD"] + "" != "") {
					foreach (string each in ItemSum) {
						if (each == RS["CICKD"] + "") {
							isinitemsum = true;
							break;
						}
					}
				}
				if (!isinitemsum) {
					if (RS["CICKD"] + "" != "")
						ItemSum.Add(RS["CICKD"] + "");
				}
				continue;
			} else {
				RowValue[7] = TotalPackedCount + "";
				RowValue[9] = TotalGrossWeight + "";
				RowValue[10] = TotalVolume + "";

				itemTemp = new StringBuilder();
				for (int i = 0; i < ItemSum.Count; i++) {
					if (i == 0)
						itemTemp.Append(ItemSum[i]);
					else
						itemTemp.Append(", " + ItemSum[i]);
				}
				RowValue[11] = Common.DBToHTML(itemTemp + "");
				ReturnValue.Append(String.Format(rowFormat, RowValue));

				RowValue = new string[] { };
				ItemSum = new List<string>();
				TotalPackedCount = 0;
				TotalGrossWeight = 0;
				TotalVolume = 0;
				count++;

				TempBLNo = RS["BLNo"] + "";
				TempRequestFormPk = RS["RequestFormPk"] + "";
				if (RS["PACKED_COUNT"] + "" != "") {
					TotalPackedCount += Int32.Parse(RS["PACKED_COUNT"] + "");
				}
				if (RS["TotalGrossWeight"] + "" != "") {
					TotalGrossWeight += decimal.Parse(RS["TotalGrossWeight"] + "");
				}
				if (RS["TotalVolume"] + "" != "") {
					TotalVolume += decimal.Parse(RS["TotalVolume"] + "");
				}

				RowValue = new string[] {
					count.ToString(),
					TempBLNo,
					RS["Shipper"]+"",
					RS["ShipperAddress"]+"",
					RS["ConsigneeCode"]+"",
					RS["Consignee"]+"",
					RS["ConsigneeAddress"]+"",
					"",
					Common.GetPackingUnit(RS["PackingUnit"]+""),
					"",
					"",
					""};

				bool isinitemsum = false;
				if (RS["CICKD"] + "" != "") {
					foreach (string each in ItemSum) {
						if (each == RS["CICKD"] + "") {
							isinitemsum = true;
							break;
						}
					}
				}
				if (!isinitemsum) {
					if (RS["CICKD"] + "" != "")
						ItemSum.Add(RS["CICKD"] + "");
				}
			}
			continue;
		}
		RS.Dispose();
		RowValue[7] = TotalPackedCount + "";
		RowValue[9] = TotalGrossWeight + "";
		RowValue[10] = TotalVolume + "";
		itemTemp = new StringBuilder();
		for (int i = 0; i < ItemSum.Count; i++) {
			if (i == 0) {
				itemTemp.Append(ItemSum[i]);
			} else {
				itemTemp.Append(", " + ItemSum[i]);
			}
		}
		RowValue[11] = Common.DBToHTML(itemTemp + "");
		ReturnValue.Append(String.Format(rowFormat, RowValue));

		string a = "<table border='1' cellpadding='0' cellspacing='0' ><tr>" +
						"	<td class='THead1' >HSN</td>" +
						"	<td class='THead1' >House BL</td>" +
						"	<td class='THead1' >Shipper</td>" +
						"	<td class='THead1' >ShipperAddress</td>" +
						"	<td class='THead1' >ConsigneeCode</td>" +
						"	<td class='THead1' >Consignee</td>" +
						"	<td class='THead1' >ConsigneeAddress</td>" +
						"	<td class='THead1' >BoxCount</td>" +
						"	<td class='THead1' >&nbsp;</td>" +
						"	<td class='THead1' >CBM</td>" +
						"	<td class='THead1' >Kg</td>" +
						"	<td class='THead1' >Description</td></tr>" + ReturnValue + "</table>";
		return a;
	}

	
	private string LoadStorcedList(string BranchPk) {
		DB.SqlCmd.CommandText = @"
SELECT
	R.ShipperCode, R.ConsigneeCode, R.DepartureDate, R.ArrivalDate,
	R.TransportWayCL, R.StepCL,
	TWCD.Initial,
	R.TotalPackedCount,
	R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, Storage.[PACKED_COUNT], DepratureR.Name AS DName ,
	ArrivalR.Name AS AName , RFAI.[ACCOUNT_ID], RFAI.[DESCRIPTION], RFAI.[REGISTERD] ,
	Storage.[WAREHOUSE_PK], OSC.StorageName, C.CompanyName ,
	TBBH.TransportBetweenBranchPk, TBBH.[TRANSPORT_WAY], TBBH.[DATETIME_FROM], TBBH.[DATETIME_FROM], TBBH.CompanyName
FROM [dbo].[STORAGE] AS Storage
	left join RequestForm AS R on R.RequestFormPk=Storage.[REQUEST_PK]
	left join OurBranchStorageCode AS OSC On Storage.[WAREHOUSE_PK]=OSC.OurBranchStoragePk
	left join TransportWayCLDescription AS TWCD ON TWCD.TransportWayCL=R.TransportWayCL
	left join RegionCode AS DepratureR on R.DepartureRegionCode=DepratureR.RegionCode
	left join RegionCode AS ArrivalR on R.ArrivalRegionCode=ArrivalR.RegionCode
	left join (SELECT [TABLE_PK], [ACCOUNT_ID], [DESCRIPTION], [REGISTERD] FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [CODE]='0') AS RFAI on R.RequestFormPk=RFAI.[TABLE_PK]
	left join Company AS C on R.ShipperPk=C.CompanyPk
	left join (
		SELECT TBBH.StorageCode, TBBH.RequestFormPk, TBBH.BoxCount, TBBH.TransportBetweenBranchPk
			, TBBHead.[TRANSPORT_WAY], TBBHead.[VALUE_STRING_0], TBBHead.[BRANCHPK_FROM], TBBHead.[BRANCHPK_TO], TBBHead.[DATETIME_FROM], TBBHead.[DATETIME_TO] 
			, C.CompanyName
		FROM TransportBBHistory AS TBBH
			left join OurBranchStorageCode AS OBSC ON TBBH.StorageCode=OBSC.OurBranchStoragePk
			left join [dbo].[TRANSPORT_HEAD] AS TBBHead ON TBBHead.[TRANSPORT_PK]=TBBH.TransportBetweenBranchPk
			left join Company AS C ON C.CompanyPk=TBBHead.[BRANCHPK_FROM]
		WHERE OBSC.OurBranchCode=" + BranchPk + @") AS TBBH ON TBBH.RequestFormPk=R.RequestFormPk
WHERE
	OSC.OurBranchCode=" + BranchPk + @"
	and (isnull(CH.TotalPackedCount, 0)=0 or Storage.[PACKED_COUNT]<>0)
ORDER BY
	left(ArrivalRegionCode, 1) ASC,
	[WAREHOUSE_PK] ASC,
	TBBH.TransportBetweenBranchPk ASC,
	ArrivalR.RegionCode ASC, CAST(right(isnull(R.ConsigneeCode, '00'), len(isnull(R.ConsigneeCode, '00'))-2) AS int) ASC ;";
		//Response.Write(DB.SqlCmd.CommandText);
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		StringBuilder RowValue = new StringBuilder();

		string Table = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:580px;\">" +
								"	<tr><td>&nbsp;</td>" +
								"		<td>창고</td>" +
								"		<td>지사간 운송방법</td>" +
								"		<td>지사간 운송일정</td>" +
								"		<td>Shipper</td>" +
								"		<td>Consignee</td>" +
								"		<td>CompanyName</td>" +
								"		<td>DepartureDate</td>" +
								"		<td>ArrivalDate</td>" +
								"		<td>&nbsp;</td>" +
								"		<td>Now</td>" +
								"		<td>Total</td>" +
								"		<td>Kg</td>" +
								"		<td>CBM</td>" +
								"		<td>Comment</td></tr>{0}</table>";
		string Row = "<tr><td>{0}</td>" +
								"<td>{1}</td>" +
								"<td>{2}</td>" +
								"		<td>{3}</td>" +
								"		<td>{4}</td>" +
								"		<td>{5}</td>" +
								"		<td>{6}</td>" +
								"		<td>{7}</td>" +
								"		<td>{8}</td>" +
								"		<td>{9}</td>" +
								"		<td style=\"{0} \">{10}</td>" +
								"		<td style=\"{0} \">{11}</td>" +
								"		<td style=\"{0} \">{12}</td>" +
								"		<td style=\"{0} \">{13}</td>" +
								"		<td style=\"{0} \">{14}</td></tr>";
		while (RS.Read()) {
			string[] TempRow = new string[15];
			//TempRow[0] = RS["StepCL"] + "" == "52" ? "style=\"background=color:blue;\"" : "";
			TempRow[0] = RS["StepCL"] + "" == "52" ? "보류" : "&nbsp;";
			TempRow[1] = RS["StorageName"] + "";
			TempRow[2] = Common.GetTransportWay(RS["TRANSPORT_WAY"] + "") + " FROM " + RS["CompanyName"];
			TempRow[3] = RS["DATETIME_FROM"] + "" != "" ? (RS["DATETIME_FROM"] + "").Substring(5, 5) + "-" + (RS["DATETIME_TO"] + "").Substring(5, 5) : "";
			TempRow[4] = RS["ShipperCode"] + "";
			TempRow[5] = RS["ConsigneeCode"] + "";
			TempRow[6] = RS["CompanyName"] + "";
			TempRow[7] = RS["DepartureDate"] + "" == "" ? "" : RS["DepartureDate"] + "";
			TempRow[8] = RS["ArrivalDate"] + "" == "" ? "" : RS["ArrivalDate"] + "";
			TempRow[9] = RS["Initial"] + "" == "" ? "" : RS["Initial"] + "";
			TempRow[10] = RS["PACKED_COUNT"] + "";
			TempRow[11] = RS["TotalPackedCount"] + "";
			TempRow[12] = RS["TotalGrossWeight"] + "";
			TempRow[13] = RS["TotalVolume"] + "";
			TempRow[14] = RS["Value"] + "";

			RowValue.Append(string.Format(Row, TempRow));
		}
		RS.Dispose();
		return string.Format(Table, RowValue + "");
	}
}