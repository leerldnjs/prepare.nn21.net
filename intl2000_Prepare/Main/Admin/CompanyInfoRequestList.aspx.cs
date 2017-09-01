using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Admin_CompanyInfoRequestList : System.Web.UI.Page
{
	protected String[] MemberInfo;
	protected StringBuilder HtmlList;
	protected String CompanyPk;
	protected String CompanyName;
	private int PageNo;
	protected void Page_Load(object sender, EventArgs e)
	{
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

		try {
			PageNo = Int32.Parse(Request["PageNo"]);
		} catch (Exception) {
			PageNo = 1;
		}

		if (string.IsNullOrEmpty(Request["S"])) {
			LoadRequestList(Request["G"], MemberInfo[1]);
		} else {
			LoadRequestList(Request["G"], Request["S"]);
		}

		if (MemberInfo[0] == "Customs") {
			LogedWithoutRecentRequest1.Visible = false;
			Loged2.Visible = true;
		}
	}
	private void LoadRequestList(string Gubun, string MemberPk)
	{
		CompanyPk = MemberPk;
		//Gubun
		//5051		접수완료 관리자 읽지않음 / 읽음
		//5253		접수보류 / 픽업예약
		//5455		입고됨 중량, 체적 입력안됨 / 중량, 체적 입력완료
		HtmlList = new StringBuilder();
		Int32 pageLength = 30;
		Int32 totlaRecord;

		HtmlList.Append("<table border='0' cellpadding='0' cellspacing='0' style='width:850px;' ><thead><tr height='30px'>" +
						"<td class='THead1' style='width:160px;' >" + GetGlobalResourceObject("Member", "CompanyCode") + "</td>" +
						"<td class='THead1' style='width:160px;' >Company Name</td>" +
						"<td class='THead1' style='width:130px;' >Booking Summary</td>" +
						"<td class='THead1' style='width:45px;'>Box</td>" +
						"<td class='THead1' style='width:45px;'>Kg</td>" +
						"<td class='THead1' style='width:35px;'>CBM</td>" +
						"<td class='THead1' style='width:85px;'>청구금액</td>" +
						"<td class='THead1' style='width:85px;'>입금액</td>" +
						"<td class='THead1' >차액</td>" +
					"</tr></thead>" + RequestListLoadByAdmin(MemberPk, pageLength, PageNo, out totlaRecord));
		HtmlList.Append("<tr height='10px'><td colspan='9' >&nbsp;</td></tr><TR Height='20px'><td colspan='9' style='background-color:#F5F5F5; text-align:center; padding:20px; '>" + new Common().SetPageListByNo(pageLength, PageNo, totlaRecord, "CompanyInfoRequestList.aspx", "?G=CI&S=" + MemberPk + "&") + "</TD></TR></Table>");
	}
	public String RequestListLoadByAdmin(string CompanyPk, int pageLength, int pageNo, out int totlaRecord)
	{
		string TableInnerRow = "<tr><td class='{0}'><a href=\"RequestView.aspx?g={1}&pk={2} \">{4}</a></td>" +
"<td class='{0}' style=\"text-align:left;\"><a href=\"CompanyInfo.aspx?M=View&S={3} \">{5}</td>" +
"<td class='{0}' ><a href=\"RequestView.aspx?g={1}&pk={2} \">{6}</a></td>" +
"<td class='{0}'>{7}</td>" +
"<td class='{0}'>{8}</td>" +
"<td class='{0}'>{9}</td>" +
"<td class='{0}'\"><span  onclick=\"ShowFreightChargeView('{2}', '{1}');\"  style=\"cursor:hand;\">{10}</span></a></td>" +
"<td class='{0}'>{11}</td>" +
"<td class='{0}'><span onclick=\"ShowDeposited('{2}', '{1}', '{3}'); \" style=\"cursor:hand;\">{12}</span></td>" +
"</tr>";
		DBConn DB = new DBConn();
		StringBuilder ReturnValue = new StringBuilder();
		GetQuery GQ = new GetQuery();
		//페이지네비게이션
		if (MemberInfo[1] == "7898") {
			DB.SqlCmd.CommandText = "	select	(select count(*) from RequestForm where ShipperPk=" + CompanyPk + " and StepCL>49 and (ArrivalRegionCode in('2!34!6','2!34!10')  or DepartureRegionCode in('2!34!6','2!34!10') ) )+" +
														  "		(select count(*) from RequestForm where ConsigneePk=" + CompanyPk + " and StepCL>49 and (ArrivalRegionCode in('2!34!6','2!34!10')  or DepartureRegionCode in('2!34!6','2!34!10') ))";
		} else {
			DB.SqlCmd.CommandText = "	select	(select count(*) from RequestForm where ShipperPk=" + CompanyPk + " and StepCL>49 )+" +
															  "		(select count(*) from RequestForm where ConsigneePk=" + CompanyPk + " and StepCL>49)";
		}
		DB.DBCon.Open();
		totlaRecord = (Int32)DB.SqlCmd.ExecuteScalar();
		if (totlaRecord == 0) {
			return "";
		}
		//목록가져오기
		if (MemberInfo[1] == "7898") {
			DB.SqlCmd.CommandText = @"	
SELECT 
	RF.RequestFormPk, RF.ShipperPk, ConsigneePk , RF.ShipperCode, RF.ConsigneeCode, RF.DepartureDate, RF.ArrivalDate, RF.StepCL, RF.DocumentStepCL, RF.RequestDate
	, C.CompanyName
	, CC.CompanyName as CCompanyName
	, Departure.NameE, Arrival.NameE AS ArrivalN
	, RI.Description
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume, RF.ExchangeDate, RCH.[MONETARY_UNIT], RCH.[TOTAL_PRICE], RCH.[DEPOSITED_PRICE], RCH.[LAST_DEPOSITED_DATE]
	, RII.itemCount
FROM RequestForm AS RF 
	Left join Company AS C on RF.ShipperPk=C.CompanyPk 
	Left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
	Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
	Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
	Left join RequestFormItems as RI on RF.RequestFormPk=RI.RequestFormPk 
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RCH on RF.RequestFormPk=RCH.[TABLE_PK] 
	Left join ( SELECT S.RequestFormPk, Count(*) as itemCount FROM RequestFormItems as S group by RequestFormPk )as RII on RF.RequestFormPk=RII.RequestFormPk 
WHERE StepCL>49 
AND ( ShipperPk=" + CompanyPk + " or ConsigneePk=" + CompanyPk + @" ) 
AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
AND  (RF.ArrivalRegionCode in('2!34!6','2!34!10')  or RF.DepartureRegionCode in('2!34!6','2!34!10') )
Order by RF.ArrivalDate DESC, RF.RequestFormPk DESC;";

		} else {
			//TOP " + (80 * pageNo).ToString() + @"
			DB.SqlCmd.CommandText = @"	
SELECT 
	RF.RequestFormPk, RF.ShipperPk, ConsigneePk , RF.ShipperCode, RF.ConsigneeCode, RF.DepartureDate, RF.ArrivalDate, RF.StepCL, RF.DocumentStepCL, RF.RequestDate
	, C.CompanyName
	, CC.CompanyName as CCompanyName
	, Departure.NameE, Arrival.NameE AS ArrivalN
	, RI.Description
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume, RF.ExchangeDate, RCH.[MONETARY_UNIT], RCH.[TOTAL_PRICE], RCH.[DEPOSITED_PRICE], RCH.[LAST_DEPOSITED_DATE]
	, RII.itemCount
FROM RequestForm AS RF 
	Left join Company AS C on RF.ShipperPk=C.CompanyPk 
	Left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
	Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
	Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
	Left join RequestFormItems as RI on RF.RequestFormPk=RI.RequestFormPk 
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RCH on RF.RequestFormPk=RCH.[TABLE_PK] 
	Left join ( SELECT S.RequestFormPk, Count(*) as itemCount FROM RequestFormItems as S group by RequestFormPk )as RII on RF.RequestFormPk=RII.RequestFormPk 
WHERE StepCL>49 
AND ( ShipperPk=" + CompanyPk + " or ConsigneePk=" + CompanyPk + @" ) 
AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
			Order by RF.ArrivalDate DESC, RF.RequestFormPk DESC;";
		}
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
			 *				RF.RequestFormPk, RF.ShipperPk, ConsigneePk , RF.ShipperCode, RF.ConsigneeCode, 
			 *				RF.DepartureDate, RF.ArrivalDate, RF.StepCL, RF.RequestDate, C.CompanyName, 
			 *				CC.CompanyName as CCompanyName, CCL.TargetCompanyName, Departure.NameE, Arrival.NameE, RI.Description, 
			 *				RCH.TotalPackedCount, RCH.PackingUnit, RCH.TotalGrossWeight, RCH.TotalVolume, RII.itemCount
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
					string[] TableInnerData = new string[13];
					TableInnerData[0] = StyleID;
					TableInnerData[2] = TempRequestFormPk;
					if (RS["ShipperPk"] + "" == CompanyPk) {
						TableInnerData[1] = "s";
						TableInnerData[3] = RS["ConsigneePk"] + "";
						TableInnerData[5] = "<span style=\"color:red;\">To</span> <strong>" + RS["ConsigneeCode"] + "</strong> " +
							((RS["CCompanyName"] + "").Length > 5 ? (RS["CCompanyName"] + "").Substring(0, 4) + "..." : RS["CCompanyName"] + "");
					} else {
						TableInnerData[1] = "c";
						TableInnerData[3] = RS["ShipperPk"] + "";
						TableInnerData[5] = "<span style=\"color:blue;\">From</span> <strong>" + RS["ShipperCode"] + "</strong> " +
							((RS["CompanyName"] + "").Length > 5 ? (RS["CompanyName"] + "").Substring(0, 4) + "..." : RS["CompanyName"] + "");
					}
					if (IsFirst) {
						CompanyName = TableInnerData[1] == "s" ? "<strong>" + RS["ShipperCode"] + "</strong> " + RS["CompanyName"] : "<strong>" + RS["ConsigneeCode"] + "</strong> " + RS["CCompanyName"];
						IsFirst = false;
					}
					TableInnerData[4] = RS["NameE"] + " ~ " + RS["ArrivalN"] + "<br />" + (RS["DepartureDate"] + "" == "" ? "" : (RS["DepartureDate"] + "").Substring(2)) + " ~ " + (RS["ArrivalDate"] + "" == "" ? "" : (RS["ArrivalDate"] + "").Substring(2));
					TableInnerData[6] = ((RS["Description"] + "").Length > 11 ? (RS["Description"] + "").Substring(0, 10) + "..." : RS["Description"] + "") + (RS["itemCount"] + "" == "" ? "" : " [" + RS["itemCount"] + "]");
					TableInnerData[7] = RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
					TableInnerData[8] = Common.NumberFormat(RS["TotalGrossWeight"] + "");
					TableInnerData[9] = Common.NumberFormat(RS["TotalVolume"] + "");
					decimal TotalCharge;
					decimal Deposited = 0;
					string MonetaryUnit;

					TotalCharge = RS["TOTAL_PRICE"] + "" != "" ? decimal.Parse(RS["TOTAL_PRICE"] + "") : 0;
					if (RS["DEPOSITED_PRICE"] + "" != "") {
						Deposited = decimal.Parse(RS["DEPOSITED_PRICE"] + "");
					}
					MonetaryUnit = Common.GetMonetaryUnit(RS["MONETARY_UNIT"] + "");

					switch (MonetaryUnit) {
						case "￥":
							TotalCharge = Math.Round(TotalCharge, 1, MidpointRounding.AwayFromZero);
							break;
						case "$":
							TotalCharge = Math.Round(TotalCharge, 2, MidpointRounding.AwayFromZero);
							break;
						case "￦":
							TotalCharge = Math.Round(TotalCharge, 0, MidpointRounding.AwayFromZero);
							break;
					}

					TableInnerData[10] = TotalCharge == 0 ? "--" : MonetaryUnit + " " + Common.NumberFormat(TotalCharge + "");
					TableInnerData[11] = Deposited == 0 ? "--" : MonetaryUnit + " " + Common.NumberFormat(Deposited + "");

					if (TotalCharge > 0 && Deposited == 0) {
						TableInnerData[12] = "<span style=\"color:red;\">未</span>";
					} else if (TotalCharge == 0) {
						TableInnerData[12] = "--";
					} else {
						decimal tempminus = Deposited - TotalCharge;
						if (tempminus == 0) {
							TableInnerData[12] = "<span style=\"color:green;\">完</span>";
						} else if (tempminus > 0) {
							TableInnerData[12] = "<span style=\"color:blue;\">" + MonetaryUnit + " " + Common.NumberFormat(tempminus + "") + "</span>";
						} else {
							TableInnerData[12] = "<span style=\"color:red;\">" + MonetaryUnit + " " + Common.NumberFormat(tempminus + "") + "</span>";
						}
					}
					if (RS["ShipperPk"] + "" != CompanyPk) {
						if (RS["StepCL"] + "" == "50" || RS["StepCL"] + "" == "51") {
							TableInnerData[0] = "";
							TableInnerData[1] = "";
							TableInnerData[2] = "";
							TableInnerData[3] = "";
							TableInnerData[4] = "";
							TableInnerData[5] = "";
							TableInnerData[6] = "";
							TableInnerData[7] = "";
							TableInnerData[8] = "";
							TableInnerData[9] = "";
							TableInnerData[10] = "";
							TableInnerData[11] = "";
							TableInnerData[12] = "";
						}
					}

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