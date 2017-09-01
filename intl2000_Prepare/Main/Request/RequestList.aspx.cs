using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Components;
using System.Data.SqlClient;

public partial class Request_RequestList : System.Web.UI.Page
{
	protected String[] MemberInfo;
	protected String[] SubInfo;
	protected StringBuilder HtmlList;
	protected String CompanyPk;
	protected String CompanyName;
	protected String CompanyRelation;
	private int PageNo;
	private DBConn DB;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("../Default.aspx");
		}

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

		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		SubInfo = Session["SubInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		try {
			PageNo = Int32.Parse(Request["PageNo"]);
		} catch (Exception) {
			PageNo = 1;
		}
		DB = new DBConn();
		string nowpk = Request.Params["S"] + "" == "" ? MemberInfo[1] : Request.Params["S"] + "";
		bool checkRight = LoadRelation(MemberInfo[1], nowpk);
		if (checkRight) {
			LoadRequestList(Request["G"] + "" == "" ? "A" : Request["G"] + "", nowpk);
		} else {
			HtmlList = new StringBuilder();
			HtmlList.Append("잘못된 경로입니다");
		}
	}
	private Boolean LoadRelation(string MainCompanyPk, string NowCompanyPk) {
		bool ReturnBool = false;
		string BTNFormat = "<input type=\"button\" onclick=\"GoToRelation('{0}')\" value=\"{1}\" {2} />&nbsp;";
		DB.SqlCmd.CommandText = @"
SELECT 
	[TargetCompanyPk], [TargetCompanyNick], C.CompanyCode, C.CompanyName 
FROM 
	[CompanyRelation] AS CR 
	left join Company AS C ON CR.TargetCompanyPk=C.CompanyPk
 WHERE CR.MainCompanyPk=" + MainCompanyPk + " and CR.GubunCL=0;";

		DB.DBCon.Open();
		StringBuilder ReturnValue = new StringBuilder();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();



		if (RS.Read()) {
			if (MainCompanyPk == NowCompanyPk) {
				ReturnBool = true;
				ReturnValue.Append(
					String.Format(BTNFormat, MainCompanyPk, SubInfo[0] + " " + SubInfo[1], " style=\"font-weight:bold;\" ") +
					String.Format(BTNFormat, RS[0], RS[2] + " " + (RS[1] + "" == "" ? RS[3] + "" : RS[1]), ""));
			} else {
				ReturnValue.Append(String.Format(BTNFormat, MainCompanyPk, SubInfo[0] + " " + SubInfo[1], ""));
				if (RS[0] + "" == NowCompanyPk) {
					ReturnBool = true;
					ReturnValue.Append(String.Format(BTNFormat, RS[0], RS[2] + " " + (RS[1] + "" == "" ? RS[3] + "" : RS[1]), " style=\"font-weight:bold;\" "));
				} else {
					ReturnValue.Append(String.Format(BTNFormat, RS[0], RS[2] + " " + (RS[1] + "" == "" ? RS[3] + "" : RS[1]), ""));
				}
			}
			while (RS.Read()) {
				if (RS[0] + "" == NowCompanyPk) {
					ReturnBool = true;
					ReturnValue.Append(String.Format(BTNFormat, RS[0], RS[2] + " " + (RS[1] + "" == "" ? RS[3] + "" : RS[1]), " style=\"font-weight:bold;\" "));
				} else {
					ReturnValue.Append(String.Format(BTNFormat, RS[0], RS[2] + " " + (RS[1] + "" == "" ? RS[3] + "" : RS[1]), ""));
				}
			}
		} else {
			if (MainCompanyPk == NowCompanyPk) {
				ReturnBool = true;
			}
			CompanyRelation = "";
		}
		RS.Dispose();
		DB.DBCon.Close();
		CompanyRelation = "<div>" + ReturnValue + "</div>";
		return ReturnBool;
	}
	private void LoadRequestList(string Gubun, string MemberPk) {
		CompanyPk = MemberPk;
		//Gubun
		//5051		접수완료 관리자 읽지않음 / 읽음
		//5253		접수보류 / 픽업예약
		//5455		입고됨 중량, 체적 입력안됨 / 중량, 체적 입력완료
		HtmlList = new StringBuilder();
		Int32 pageLength = 30;
		Int32 totlaRecord;

		HtmlList.Append("<table border='0' cellpadding='0' cellspacing='0' style='width:850px;' ><thead><tr height='30px'>" +
						//"<td class='THead1' style='width:160px;' >" + GetGlobalResourceObject("Member", "CompanyCode") + "</td>" +
						//"<td class='THead1' style='width:160px;' >Company Name</td>" +
						"<td class='THead1' style='width:60px;' >" + GetGlobalResourceObject("qjsdur", "ehckrdPwjd") + "</td>" +
						"<td class='THead1' style='width:200px;' >" + GetGlobalResourceObject("Member", "CompanyCode") + "</td>" +
						"<td class='THead1' style='width:270px;' >Company Name</td>" +
						//"<td class='THead1' style='width:130px;' >Booking Summary</td>" +
						"<td class='THead1' >Booking Summary</td>" +
						"<td class='THead1' style='width:45px;'>Box</td>" +
						"<td class='THead1' style='width:45px;'>Kg</td>" +
						"<td class='THead1' style='width:35px;'>CBM</td>" +
					//"<td class='THead1' style='width:85px;'>" + GetGlobalResourceObject("qjsdur", "cjdrnrmador") + "</td>" +
					//"<td class='THead1' style='width:85px;'>" + GetGlobalResourceObject("qjsdur", "dlqrmador") + "</td>" +
					//"<td class='THead1' >" + GetGlobalResourceObject("qjsdur", "ckdor") + "</td>" +
					"</tr></thead>" + RequestListLoad(MemberPk, Gubun, pageLength, PageNo, out totlaRecord) +
					"<tr height='10px'><td colspan='9' >&nbsp;</td></tr><TR Height='20px'><td colspan='9' style='background-color:#F5F5F5; text-align:center; padding:20px; '>" +
						new Common().SetPageListByNo(pageLength, PageNo, totlaRecord, "RequestList.aspx", "?G=" + Gubun + "&S=" + CompanyPk + "&") + "</TD></TR></Table>");
	}
	public String RequestListLoad(string CompanyPk, string Gubun, int pageLength, int pageNo, out int totlaRecord) {
		string TableInnerRow = "<tr><td class='{0}' style=\"text-align:left;\">{13}</td><td class='{0}'><a href=\"RequestFormView.aspx?pk={2} \">{4}</a></td>" +
			"<td class='{0}' style=\"text-align:left;\">{5}</td>" +
			"<td class='{0}' ><a href=\"RequestFormView.aspx?pk={2} \">{6}</a></td>" +
			"<td class='{0}'>{7}</td>" +
			"<td class='{0}'>{8}</td>" +
			"<td class='{0}'>{9}</td>" +
			//"<td class='{0}'\"><span>{10}</span></a></td>" +
			//"<td class='{0}'>{11}</td>" +
			//"<td class='{0}'><span>{12}</span></td>" +
			"</tr>";

		StringBuilder ReturnValue = new StringBuilder();
		GetQuery GQ = new GetQuery();

		switch (Gubun) {
			case "A":
				//DB.SqlCmd.CommandText = "select (select count(*) from RequestForm where ArrivalDate>'20110700' and ShipperPk=" + CompanyPk + " and StepCL>49 )+(select count(*) from RequestForm where ArrivalDate>'20110700' and ConsigneePk=" + CompanyPk + " and StepCL>49 )";
				DB.SqlCmd.CommandText = "select (select count(*) from RequestForm where ArrivalDate>dateadd(YEAR,-2,GETDATE()) and ShipperPk=" + CompanyPk + " and StepCL>49 )+(select count(*) from RequestForm where ArrivalDate>dateadd(YEAR,-2,GETDATE()) and ConsigneePk=" + CompanyPk + " and StepCL>49 )";
				break;
			case "I":
				//DB.SqlCmd.CommandText = "select count(*) from RequestForm where ArrivalDate>'20110700' and ConsigneePk=" + CompanyPk + " and StepCL>49 ";
				DB.SqlCmd.CommandText = "select count(*) from RequestForm where ArrivalDate>dateadd(YEAR,-2,GETDATE()) and ConsigneePk=" + CompanyPk + " and StepCL>49 ";
				break;
			default:
				DB.SqlCmd.CommandText = "select count(*) from RequestForm where ArrivalDate>dateadd(YEAR,-2,GETDATE()) and ShipperPk=" + CompanyPk + " and StepCL>49 ";
				break;  //O
		}
		//Response.Write(DB.SqlCmd.CommandText);

		DB.DBCon.Open();
		totlaRecord = (Int32)DB.SqlCmd.ExecuteScalar();
		if (totlaRecord == 0) {
			return "";
		}
		//Shipper가 예약한것은 Consignee가 보지못하게변경 SP_RequestListByEachCompanyOver77로 프로시저 수정 (12.03.15)
		//DB.SqlCmd.CommandText = "EXEC SP_RequestListByEachCompanyOver7 @CompanyPk=" + CompanyPk + ", @Gubun='" + Gubun + "';";

		//최근2년으로 변경 777
		DB.SqlCmd.CommandText = "EXEC SP_RequestListByEachCompanyOver777 @CompanyPk=" + CompanyPk + ", @Gubun='" + Gubun + "';";
		//DB.SqlCmd.CommandText = "EXEC SP_RequestListByEachCompanyOver77 @CompanyPk=" + CompanyPk + ", @Gubun='" + Gubun + "';";        
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		String StyleID;
		String TempRequestFormPk = string.Empty;
		StringBuilder Temp = new StringBuilder();
		bool IsFirst = true;
		int k = 0;
		while (k < (pageNo - 1) * pageLength) {
			if (RS.Read()) {
				if (TempRequestFormPk != RS[0] + "") {
					TempRequestFormPk = RS[0] + "";
					k++;
				}
			} else {
				break;
			}
		}
		for (int i = 0; i < pageLength; i++) {
			/*
             *		RF.RequestFormPk, RF.ShipperPk, ConsigneePk , RF.ShipperCode, RF.ConsigneeCode, 
             *		RF.DepartureDate, RF.ArrivalDate, RF.StepCL, RF.RequestDate, C.CompanyName, 
             *		CC.CompanyName as CCompanyName, CCL.TargetCompanyName, Departure.NameE, Arrival.NameE, RI.Description, 
             *		RCH.TotalPackedCount, RCH.PackingUnit, RCH.TotalGrossWeight, RCH.TotalVolume, RII.itemCount
             */
			if (RS.Read()) {
				if (TempRequestFormPk != RS[0] + "") {
					TempRequestFormPk = RS[0] + "";
					if (RS["StepCL"] + "" == "65") {
						StyleID = "TBody1";
					} else if (RS["DocumentStepCL"] + "" == "10" || RS["DocumentStepCL"] + "" == "11" || RS["DocumentStepCL"] + "" == "12") {
						StyleID = "TBody1B";
					} else {
						StyleID = "TBody1G";
					}

					string[] TableInnerData = new string[14];
					TableInnerData[0] = StyleID;
					TableInnerData[2] = TempRequestFormPk;
					if (RS["ShipperPk"] + "" == CompanyPk) {
						TableInnerData[1] = "s";
						TableInnerData[3] = RS["ConsigneePk"] + "";
						TableInnerData[5] = "<span style=\"color:red;\">To</span> <strong>" + RS["ConsigneeCode"] + "</strong> " +
						//((RS["CCompanyName"] + "").Length > 5 ? (RS["CCompanyName"] + "").Substring(0, 4) + "..." : RS["CCompanyName"] + "");
						((RS["CCompanyName"] + "").Length > 15 ? (RS["CCompanyName"] + "").Substring(0, 14) + "..." : RS["CCompanyName"] + "");
					} else {
						TableInnerData[1] = "c";
						TableInnerData[3] = RS["ShipperPk"] + "";
						TableInnerData[5] = "<span style=\"color:blue;\">From</span> <strong>" + RS["ShipperCode"] + "</strong> " +
						//((RS["CompanyName"] + "").Length > 5 ? (RS["CompanyName"] + "").Substring(0, 4) + "..." : RS["CompanyName"] + "");
						((RS["CompanyName"] + "").Length > 15 ? (RS["CompanyName"] + "").Substring(0, 14) + "..." : RS["CompanyName"] + "");
					}
					if (IsFirst) {
						CompanyName = TableInnerData[1] == "s" ? "<strong>" + RS["ShipperCode"] + "</strong> " + RS["CompanyName"] : "<strong>" + RS["ConsigneeCode"] + "</strong> " + RS["CCompanyName"];
						IsFirst = false;
					}
					TableInnerData[4] = RS["NameE"] + " ~ " + RS["ArrivalN"] + "<br />" + (RS["DepartureDate"] + "" == "" ? "" : (RS["DepartureDate"] + "").Substring(2)) + " ~ " + (RS["ArrivalDate"] + "" == "" ? "" : (RS["ArrivalDate"] + "").Substring(2));
					TableInnerData[6] = ((RS["Description"] + "").Length > 11 ? (RS["Description"] + "").Substring(0, 10) + "..." : RS["Description"] + "") + (RS["itemCount"] + "" == "" ? "" : " [" + RS["itemCount"] + "]");
					TableInnerData[6] = ((RS["Description"] + "").Length > 15 ? (RS["Description"] + "").Substring(0, 14) + "..." : RS["Description"] + "") + (RS["itemCount"] + "" == "" ? "" : " [" + RS["itemCount"] + "]");
					TableInnerData[7] = RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
					TableInnerData[8] = Common.NumberFormat(RS["TotalGrossWeight"] + "");
					TableInnerData[9] = Common.NumberFormat(RS["TotalVolume"] + "");

					string[] Description = (RS["TBBHeadDescription"] + "").Split(Common.Splite321, StringSplitOptions.None);
					string TempArrivalPort = "";
					if (Description[0].ToString() != "") {
						if (Description[2].Trim().Length > 5) {
							if (Description[2].ToString().Trim().Substring(0, 6) == "PYONGT") {
								TempArrivalPort = "PYEONGTAEK";
							} else if (Description[2].ToString().Trim().Substring(0, 4) == "INCH") {
								TempArrivalPort = "INCHEON";
							} else {
								TempArrivalPort = "";
							}
						}
					}

					TableInnerData[13] = TempArrivalPort;
					//decimal TotalCharge;
					//decimal Deposited = 0;
					//string MonetaryUnit;
					//if (TableInnerData[1] == "s") {
					//	TotalCharge = RS["ShipperCharge"] + "" != "" ? decimal.Parse(RS["ShipperCharge"] + "") : 0;

					//	if (RS["WillPayTariff"] + "" == "S" && RS["TSum"] + "" != "") {
					//		if (RS["ShipperMonetaryUnit"] + "" == "20") {
					//			TotalCharge += decimal.Parse(RS["TSum"] + "");
					//		} else {																						RCH
					//			string tempExchangedDate = "";																Tariff
					//			string ExchangeRate = RS["ExchangeRate"] + "";
					//			if (ExchangeRate != "") {
					//				string[] RowExchangeRate = ExchangeRate.Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
					//				for (int n = 0; n < RowExchangeRate.Length; n++) {
					//					if (tempExchangedDate == "") {
					//						tempExchangedDate = RowExchangeRate[n].Substring(6, 8);
					//						continue;
					//					}
					//					if (Int32.Parse(RowExchangeRate[n].Substring(6, 8)) > Int32.Parse(tempExchangedDate)) {
					//						tempExchangedDate = RowExchangeRate[n].Substring(6, 8);
					//					}
					//				}
					//			}
					//			string temp = "";
					//			TotalCharge += new Admin().GetExchangeRated("20", RS["ShipperMonetaryUnit"] + "", decimal.Parse(RS["TSum"] + ""), out temp, tempExchangedDate);
					//		}
					//	}

					//	if (RS["SDPrice"] + "" != "") {
					//		TotalCharge += decimal.Parse(RS["SDPrice"] + "");
					//	}
					//	if (RS["ShipperDeposited"] + "" != "") {
					//		Deposited = decimal.Parse(RS["ShipperDeposited"] + "");
					//	}
					//	MonetaryUnit = Common.GetMonetaryUnit(RS["ShipperMonetaryUnit"] + "");
					//} else {
					//	TotalCharge = RS["ConsigneeCharge"] + "" != "" ? decimal.Parse(RS["ConsigneeCharge"] + "") : 0;

					//	if (RS["WillPayTariff"] + "" == "C" && RS["TSum"] + "" != "") {
					//		if (RS["ConsigneeMonetaryUnit"] + "" == "20") {
					//			TotalCharge += decimal.Parse(RS["TSum"] + "");
					//		} else {
					//			string tempExchangedDate = "";
					//			string ExchangeRate = RS["ExchangeRate"] + "";
					//			if (ExchangeRate != "") {
					//				string[] RowExchangeRate = ExchangeRate.Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
					//				for (int n = 0; n < RowExchangeRate.Length; n++) {
					//					if (tempExchangedDate == "") {
					//						tempExchangedDate = RowExchangeRate[n].Substring(6, 8);
					//						continue;
					//					}
					//					if (Int32.Parse(RowExchangeRate[n].Substring(6, 8)) > Int32.Parse(tempExchangedDate)) {
					//						tempExchangedDate = RowExchangeRate[n].Substring(6, 8);
					//					}
					//				}
					//			}
					//			string temp = "";
					//			TotalCharge += new Admin().GetExchangeRated("20", RS["ShipperMonetaryUnit"] + "", decimal.Parse(RS["TSum"] + ""), out temp, tempExchangedDate);
					//		}
					//	}
					//	if (RS["CDPrice"] + "" != "") {
					//		TotalCharge += decimal.Parse(RS["CDPrice"] + "");
					//	}
					//	if (RS["ConsigneeDeposited"] + "" != "") {
					//		Deposited = decimal.Parse(RS["ConsigneeDeposited"] + "");
					//	}

					//	MonetaryUnit = Common.GetMonetaryUnit(RS["ConsigneeMonetaryUnit"] + "");
					//}

					//switch (MonetaryUnit) {
					//	case "￥":
					//		TotalCharge = Math.Round(TotalCharge, 1, MidpointRounding.AwayFromZero);
					//		break;
					//	case "$":
					//		TotalCharge = Math.Round(TotalCharge, 2, MidpointRounding.AwayFromZero);
					//		break;
					//	case "￦":
					//		TotalCharge = Math.Round(TotalCharge, 0, MidpointRounding.AwayFromZero);
					//		break;
					//}

					//TableInnerData[10] = TotalCharge == 0 ? "--" : MonetaryUnit + " " + Common.NumberFormat(TotalCharge + "");
					//TableInnerData[11] = Deposited == 0 ? "--" : MonetaryUnit + " " + Common.NumberFormat(Deposited + "");

					//if (TotalCharge > 0 && Deposited == 0) {
					//	TableInnerData[12] = "<span style=\"color:red;\">未</span>";
					//} else if (TotalCharge == 0) {
					//	TableInnerData[12] = "--";
					//} else {
					//	decimal tempminus = Deposited - TotalCharge;
					//	if (tempminus == 0) {
					//		TableInnerData[12] = "<span style=\"color:green;\">完</span>";
					//	} else if (tempminus > 0) {
					//		TableInnerData[12] = "<span style=\"color:blue;\">" + MonetaryUnit + " " + Common.NumberFormat(tempminus + "") + "</span>";
					//	} else {
					//		TableInnerData[12] = "<span style=\"color:red;\">" + MonetaryUnit + " " + Common.NumberFormat(tempminus + "") + "</span>";
					//	}
					//}
					ReturnValue.Append(string.Format(TableInnerRow, TableInnerData));
					continue;
				} else {
					i--;
					continue;
				}
			} else {
				break;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "";
	}
}