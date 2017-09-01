using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_Dialog_SetTariff : System.Web.UI.Page
{
	private DBConn DB;
	protected String Header;
	protected String TBList;
	protected String[] MemberInfo;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../Default.aspx"); }
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		if (MemberInfo[0] == "Customs") {
			LogedWithoutRecentRequest111.Visible = false;
			Loged1.Visible = true;
			Header = "";
		} else {
			Header = "<p><a href=\"RequestList.aspx?G=Arrival\">출발지 입고완료</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"CheckDescriptionList.aspx\">BL List</a>" +
				"&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"SetTariff.aspx\"><strong>세금맞추기</strong></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"AccountTariffList.aspx\">세금 송금내역</a></p>";
		}

		if (IsPostBack) {
			TBList = LoadList(MemberInfo[1], Request.Form["HClipBoard"].ToString());
		} else {
			TBList = LoadList(MemberInfo[1], null);
		}
	}
	
	private String LoadList(string OurBranchPk, string ClipBoard) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("	<table border='0' cellpadding='0' cellspacing='0' style='width:1100px;' ><thead><tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; width:60px;  \">Status</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:140px; \" >House BL</td>" +
										"		<td class='THead1' >Customer</td>" +
										"		<td class='THead1' style='width:110px;'>count</td>" +
										"		<td class='THead1' style='width:70px;'>관세</td>" +
										"		<td class='THead1' style='width:70px;'>부가세</td>" +
										"		<td class='THead1' style='width:60px;'>관세사비</td>" +
										"		<td class='THead1' style='width:80px;'>관세 차</td>" +
										"		<td class='THead1' style='width:80px;'>부가세 차</td>" +
										"		<td class='THead1' style='width:70px;'>관세사비 차</td>" +
										"		<td class='THead1' style='width:90px;'>관부가세 정산</td>" +
										"		<td class='THead1' style='width:90px;'>입금 차액</td>" +
										"		<td class='THead1' style='width:90px;'>C</td>" +
										"		</tr></thead>");
		string BodyCommercial = "<tr style=\"height:20px;\">" +
											"		<td class='TBody1' style=\"text-align:center; \">{2}</td>" +
											"		<td class='TBody1' style=\"text-align:center; font-weight:bold;\" ><a onclick=\"Goto('CheckDescription','{0}');\">{4}</a></td>" +
											"		<td class='TBody1' ><a onclick=\"Goto('Company', '{1}')\" ><strong>{5}</strong> {6}</a></td>" +
											"		<td class='TBody1' >{7} ({8})</td>" +
											"		<td class='TBody1' >{9}</td>" +
											"		<td class='TBody1' >{10}</td>" +
											"		<td class='TBody1' >{11}</td>" +
											"		<td class='TBody1' >{12}</td>" +
											"		<td class='TBody1' >{13}</td>" +
											"		<td class='TBody1' >{14}</td>" +
											"		<td class='TBody1' >{15}</td>" +
											"		<td class='TBody1' >{16}</td>" +
											"		<td class='TBody1' >{17}</td>" +
											"		</tr>";
		string BodyRequest = "<tr style=\"height:20px;\">" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >└---------></td>" +
										"		<td class='TBody1' ><a onclick=\"Goto('Request', '{5}')\" >{1} ({2})</a></td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"	</tr>";
		DB = new DBConn();
		string[][] Pasted = new string[1][];
		if (ClipBoard == null) {
			DB.SqlCmd.CommandText = @"
SELECT TOP 1000
	RF.RequestFormPk, RF.ConsigneePk, RF.ConsigneeCode, RF.ArrivalDate, RF.DocumentStepCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.ContainerNo 
	, CC.CompanyName AS ConsigneeCompanyName 
	, Departure.NameE 
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume 
	, T1.Value as T1V, T2.Value as T2V, T3.Value as T3V 
FROM RequestForm AS RF 
	inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk 
	left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk 
	left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
	Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
	Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode 
	left join ( 
		SELECT [CommercialDocumentHeadPk], [MonetaryUnitCL], [Value] FROM [CommercialDocumentTariff] WHERE Title='관세'
	) AS T1 ON T1.[CommercialDocumentHeadPk]=CD.CommercialDocumentHeadPk 
	left join ( SELECT [CommercialDocumentHeadPk], [MonetaryUnitCL], [Value] FROM [CommercialDocumentTariff] WHERE Title='부가세') AS T2 ON T2.[CommercialDocumentHeadPk]=CD.CommercialDocumentHeadPk 
	left join ( SELECT [CommercialDocumentHeadPk], [MonetaryUnitCL], [Value] FROM [CommercialDocumentTariff] WHERE Title='관세사비') AS T3 ON T3.[CommercialDocumentHeadPk]=CD.CommercialDocumentHeadPk 
WHERE Arrival.OurBranchCode=3157 and RF.DocumentStepCL not in (1, 3, 4, 11, 12, 14, 15) and CD.StepCL=1
Order by 
	CD.ContainerNo, CD.BLNo ASC, CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC;";
		} else {
			string[] EachRow = ClipBoard.Split(new string[] { "%!$@#" }, StringSplitOptions.RemoveEmptyEntries);
			StringBuilder queryWhereBLNumber = new StringBuilder();
			Pasted = new string[ClipBoard.Length][];
			for (var i = 0; i < EachRow.Length; i++) {
				Pasted[i] = EachRow[i].Split(new string[] { "@#$" }, StringSplitOptions.None);
				if (queryWhereBLNumber + "" != "") {
					queryWhereBLNumber.Append(", ");
				}
				queryWhereBLNumber.Append("'" + Pasted[i][0] + "'");
			}
			DB.SqlCmd.CommandText = @"
SELECT 
	RF.RequestFormPk, RF.ConsigneePk, RF.ConsigneeCode, RF.ArrivalDate, RF.DocumentStepCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.ContainerNo, CD.StepCL 
	, CC.CompanyName AS ConsigneeCompanyName 
	, Departure.NameE 
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume 
	, isnull(T1.Value, 0) as T1V, isnull(T2.Value, 0) as T2V, isnull(T3.Value, 0) as T3V 
	, Deposited.ConsigneeMonetaryUnit, isnull(Deposited.ConsigneeCharge, 0) AS ConsigneeCharge, isnull(Deposited.ConsigneeDeposited, 0) AS ConsigneeDeposited, Deposited.WillPayTariff, ISNULL(Deposited.TSum, 0) AS TSum, ISNULL(Deposited.CDPrice, 0) AS CDPrice 
FROM RequestForm AS RF 
	inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk 
	left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk 
	left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
	Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
	Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode 
	left join ( 
		SELECT [CommercialDocumentHeadPk], [MonetaryUnitCL], [Value] FROM [CommercialDocumentTariff] WHERE Title='관세'
	) AS T1 ON T1.[CommercialDocumentHeadPk]=CD.CommercialDocumentHeadPk 
	left join ( 
		SELECT [CommercialDocumentHeadPk], [MonetaryUnitCL], [Value] FROM [CommercialDocumentTariff] WHERE Title='부가세'
	) AS T2 ON T2.[CommercialDocumentHeadPk]=CD.CommercialDocumentHeadPk 
	left join ( 
		SELECT [CommercialDocumentHeadPk], [MonetaryUnitCL], [Value] FROM [CommercialDocumentTariff] WHERE Title='관세사비'
	) AS T3 ON T3.[CommercialDocumentHeadPk]=CD.CommercialDocumentHeadPk 
	inner join (	
				SELECT 
					RF.RequestFormPk
					, RCH.ConsigneeMonetaryUnit, RCH.ConsigneeCharge, RCH.ConsigneeDeposited, RCH.WillPayTariff 
					, Tariff.TSum
					, CDelivery.Price AS CDPrice
				FROM RequestForm AS RF 
					Left join Company AS C on RF.ShipperPk=C.CompanyPk 
					Left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
					Left join RequestFormCalculateHead as RCH on RF.RequestFormPk=RCH.RequestFormPk 
					left join (
						SELECT GubunPk, sum([Value]) As TSum FROM CommercialDocumentTariff group by GubunPk
						) AS Tariff ON RF.RequestFormPk=Tariff.GubunPk 
					left join (
						SELECT RequestFormPk, Price, MonetaryUnit FROM RequestFormCalculateBody WHERE GubunCL=300 and StandardPriceHeadPkNColumn='D'
						) AS CDelivery ON CDelivery.RequestFormPk=RF.RequestFormPk
				WHERE Tariff.TSum>0
		) AS Deposited ON RF.RequestFormPk=Deposited.RequestFormPk 

WHERE CD.BLNo in (" + queryWhereBLNumber + @") 
Order by 
	CD.ContainerNo, CD.BLNo ASC, CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC;";
		}
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string TempCommercialDocumentHeadPk = "";
		string TempRequestFormPk = "";
		string TempCDStepCL = "";
		int InputTVCount = 0;
		while (RS.Read()) {
			if (TempCommercialDocumentHeadPk != RS["CommercialDocumentHeadPk"] + "") {
				string tempDocumentStep;
				switch (RS["DocumentStepCL"] + "") {
					case "1": tempDocumentStep = "미전송"; break;
					case "2": tempDocumentStep = "<span style=\"color:blue; \">전송완</span>"; break;
					case "3": tempDocumentStep = "자통"; break;
					case "4": tempDocumentStep = "샘플"; break;
					case "5": tempDocumentStep = "<span style=\"color:green; \">정산완료</span>"; break;
					case "6": tempDocumentStep = "통관지시"; break;
					case "7": tempDocumentStep = "생략"; break;
					case "8": tempDocumentStep = "서류제출"; break;
					case "9": tempDocumentStep = "검사"; break;
					case "10": tempDocumentStep = "세금납부"; break;
					case "11": tempDocumentStep = "세금납부"; break;
					case "12": tempDocumentStep = "세금납부"; break;
					case "13": tempDocumentStep = "면허완료"; break;
					case "14": tempDocumentStep = "면허완료"; break;
					case "15": tempDocumentStep = "면허완료"; break;
					default: tempDocumentStep = "?"; break;
				}
				TempCommercialDocumentHeadPk = RS["CommercialDocumentHeadPk"] + "";
				TempRequestFormPk = RS["RequestFormPk"] + "";
				string[] StringFormatValue = new string[18];
				StringFormatValue[0] = RS["CommercialDocumentHeadPk"] + "";
				StringFormatValue[1] = RS["ConsigneePk"] + "";
				StringFormatValue[2] = tempDocumentStep;
				StringFormatValue[3] = RS["ContainerNo"] + "" == "" ? "&nbsp;" : RS["ContainerNo"] + "";
				StringFormatValue[4] = RS["BLNo"] + "";
				StringFormatValue[5] = RS["ConsigneeCode"] + "";
				StringFormatValue[6] = RS["ConsigneeCompanyName"] + "";
				StringFormatValue[7] = RS["ArrivalDate"] + "";
				StringFormatValue[8] = RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
				StringFormatValue[9] = Common.NumberFormat(RS["T1V"] + "");
				StringFormatValue[10] = Common.NumberFormat(RS["T2V"] + "");
				StringFormatValue[11] = Common.NumberFormat(RS["T3V"] + "");

				if (ClipBoard == null) {
					StringFormatValue[12] = "&nbsp;";
					StringFormatValue[13] = "&nbsp;";
					StringFormatValue[14] = "&nbsp;";
					StringFormatValue[15] = "&nbsp;";
				} else {
					Decimal TempPasted1 = 0;
					Decimal TempPasted2 = 0;
					Decimal TempPasted3 = 0;
					Decimal TempSaved1 = decimal.Parse(RS["T1V"] + "");
					Decimal TempSaved2 = decimal.Parse(RS["T2V"] + "");
					Decimal TempSaved3 = decimal.Parse(RS["T3V"] + "");

					for (var i = 0; i < Pasted.Length; i++) {
						if (Pasted[i] == null) {
							continue;
						}

						if (RS["BLNo"] + "" == Pasted[i][0]) {
							TempPasted1 = decimal.Parse(((Pasted[i][1] + "").Replace("\r\n", "") == "" ? "0" : Pasted[i][1]));
							TempPasted2 = decimal.Parse(((Pasted[i][2] + "").Replace("\r\n", "") == "" ? "0" : Pasted[i][2]));
							TempPasted3 = decimal.Parse(((Pasted[i][3] + "").Replace("\r\n", "") == "" ? "0" : Pasted[i][3]));
							break;
						}
					}
					Decimal Difference1 = TempPasted1 - TempSaved1;
					Decimal Difference2 = TempPasted2 - TempSaved2;
					Decimal Difference3 = TempPasted3 - TempSaved3;
					StringFormatValue[12] = Common.NumberFormat((Difference1).ToString());
					StringFormatValue[13] = Common.NumberFormat((Difference2).ToString());
					StringFormatValue[14] = Common.NumberFormat((Difference3).ToString());

					StringFormatValue[15] = TempCDStepCL;
					if (Difference1 == 0 && Difference2 == 0 && Difference3 == 0) {
						StringFormatValue[15] += "--";
					} else {
						StringFormatValue[15] += "<input type='button'  value='수정' onclick=\"TariffChange('" + TempCommercialDocumentHeadPk + "', '" + TempPasted1 + "', '" + TempPasted2 + "', '" + TempPasted3 + "');\" />";
					}

					Decimal Left = 0;
					if (RS["WillPayTariff"] + "" == "C") {
						Left = decimal.Parse(RS["ConsigneeCharge"] + "") + decimal.Parse(RS["TSum"] + "") + decimal.Parse(RS["CDPrice"] + "");
					} else {
						Left = decimal.Parse(RS["ConsigneeCharge"] + "") + decimal.Parse(RS["CDPrice"] + "");
					}
					Left += decimal.Parse(RS["ConsigneeDeposited"] + "") * -1;


					if (Left == 0) {
						StringFormatValue[16] = "<span style='color:green;'>--</span>";
					} else if (Left > 0) {
						StringFormatValue[16] = "<span style='color:blue;'>" + Common.NumberFormat(Left.ToString()) + "</span>";
					} else {
						StringFormatValue[16] = "<span style='color:red;'>" + Common.NumberFormat(Left.ToString()) + "</span>";
					}
				}
				StringFormatValue[17] += "<input id=\"chk_CDPk[" + InputTVCount + "]\" type='checkbox' value='" + TempCommercialDocumentHeadPk + "' checked='checked' />";
				InputTVCount++;

				ReturnValue.Append(String.Format(BodyCommercial, StringFormatValue));
			} else {
				if (TempRequestFormPk == RS["RequestFormPk"] + "") {
					continue;
				}
				ReturnValue.Append(String.Format(BodyRequest,
													RS["NameE"] + "",
													(RS["ArrivalDate"] + ""),
													RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + ""),
													Common.NumberFormat(RS["TotalGrossWeight"] + ""),
													Common.NumberFormat(RS["TotalVolume"] + ""),
													RS["RequestFormPk"] + ""));
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "</table><input type=\"hidden\" id=\"TariffInputCount\" value=\"" + InputTVCount + "\" /> ";
	}
}