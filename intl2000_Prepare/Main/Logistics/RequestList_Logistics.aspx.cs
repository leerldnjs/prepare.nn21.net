using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Logistics_RequestList_Logistics : System.Web.UI.Page
{
	protected String[] MemberInfo;
	protected StringBuilder HtmlList;
	private int PageNo;
	private DBConn DB;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../Default.aspx"); }
		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } } catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }

		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		try { PageNo = Int32.Parse(Request["PageNo"]); } catch (Exception) { PageNo = 1; }

		LoadArrival(MemberInfo[1]);
	}

	private Boolean LoadArrival(string BranchPk)
	{
		HtmlList = new StringBuilder();
		Int32 pageLength = 30;
		Int32 totlaRecord;
		String TableFormat = "<table border='0' cellpadding='0' cellspacing='0' style='width:850px;' ><thead><tr height='30px'>" +
						"<td class='THead1' style='width:55px;' >Start</td>" +
						"<td class='THead1' style='width:55px;'>Arrival</td>" +
						"<td class='THead1' style='width:100px;'>CompanyCode</td>" + 
						"<td class='THead1' >Customer</td>" +
						"<td class='THead1' style='width:55px;'>count</td>" +
						"<td class='THead1' style='width:55px;'>Kg</td>" +
						"<td class='THead1' style='width:55px;'>CBM</td>" +
					"</tr></thead>{0}<tr><td colspan='11' style='background-color:#F5F5F5; text-align:center; padding:20px; '>{1}</td></tr></table>";
		String TableRowFormat = "<tr height='30px'>" +
								"<td class='{0}'><a onclick=\"Goto('RequestForm', '{5}');\">{1}</a></td>" +
								"<td class='{0}'><a onclick=\"Goto('RequestForm', '{5}');\">{3}</a></td>" +
								"<td class='{0}'><a onclick=\"Goto('CompanyInfo', '{15}');\">{9}</a></td>" +
								"<td class='{0}'><a onclick=\"Goto('CompanyInfo', '{15}');\">{8}</a></td>" +
								"<td class='{0}'>{10}</td>" +
								"<td class='{0}'>{11}</td>" +
								"<td class='{0}'>{12}</td></tr>";
		//수입자 pk {15}
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"	
		SELECT count(*)  
		FROM RequestForm AS RF 
			LEFT JOIN RegionCode AS R ON RF.ArrivalRegionCode=R.RegionCode
		    left join OurBranchStorageOut as OBSO on RF.RequestFormPk=OBSO.RequestFormPk	
		WHERE R.OurBranchCode=" + BranchPk + @"
		and StepCL in (54, 55, 56, 57, 58, 59, 60, 61) 
		--and isnull(DocumentStepCL, 0)<13 
		--and isnull(DocumentStepCL, 0)<>3 
		--and DocumentRequestCL is null
		and OBSO.StorageCode is null;";
		DB.DBCon.Open();

		totlaRecord = (Int32)DB.SqlCmd.ExecuteScalar();
		if (totlaRecord == 0) {
			DB.DBCon.Close();
			return false;
		}
		DB.SqlCmd.CommandText = @" SELECT RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
												  RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentRequestCL, RF.StepCL, RF.DocumentStepCL, 
												  RF.StockedDate, 
												  C.CompanyName as SCompanyName, C.CompanyNamee as SCompanyNamee, 
												  CC.CompanyName as CCompanyName, CC.CompanyNamee as CCompanyNamee, 
												  Departure.NameE AS DepartureArea, Arrival.NameE AS ArrivalArea, 
												  RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume, Msg.MsgCount 
										FROM RequestForm AS RF 
											Left join Company AS C on RF.ShipperPk=C.CompanyPk 
											Left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
											Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
											Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
											left join (SELECT Count(*) as MsgCount, GubunPk FROM MsgSendedHistory Group by GubunPk) AS Msg ON RF.RequestFormPk=Msg.GubunPk 
											left join OurBranchStorageOut as OBSO on RF.RequestFormPk=OBSO.RequestFormPk
										WHERE Arrival.OurBranchCode=" + BranchPk + @" and StepCL in (54, 55, 56, 57, 58, 59, 60, 61) 
										  --and isnull(DocumentStepCL, 0)<13
										  --and isnull(DocumentStepCL, 0)<>3
										  --and DocumentRequestCL is null
											and OBSO.StorageCode is null
										ORDER BY RF.ArrivalDate DESC, RF.StockedDate DESC,  RF.DepartureDate ASC, RF.StepCL ASC, RF.ShipperCode asc;";

		StringBuilder RowValue = new StringBuilder();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		String StyleID;
		for (int i = 0; i < (PageNo - 1) * pageLength; i++) {
			if (RS.Read()) {
				continue;
			} else {
				break;
			}
		}
		for (int i = 0; i < pageLength; i++) {
			if (RS.Read()) {
				string IsCostCharged = RS["StepCL"] + "";
				if (IsCostCharged == "54" || IsCostCharged == "55" || IsCostCharged == "56" || IsCostCharged == "57") {
					IsCostCharged = "False";
				} else {
					IsCostCharged = "True";
				}

				if (RS["TransportWayCL"] + "" == "28" || RS["TransportWayCL"] + "" == "29") {
					//항공일경우 보라색
					StyleID = "TBody1R";
				} else if (RS["DocumentStepCL"] + "" == "") {
					//노란색
					StyleID = "TBody1G";
				} else {
					//흰색// 화주 도큐먼트 오케이 된것 
					StyleID = "TBody1";
				}

				RowValue.Append(String.Format(TableRowFormat,
					StyleID,
					(RS["DepartureDate"] + "" == "" ? "&nbsp" : (RS["DepartureDate"] + "").Substring(2)),
					RS["DepartureArea"] + "",
					(RS["ArrivalDate"] + "" == "" ? "&nbsp" : (RS["ArrivalDate"] + "").Substring(2)),
					Common.GetTransportWay(RS["TransportWayCL"] + ""),
					RS["RequestFormPk"] + "",
					RS["SCompanyName"] + (RS["SCompanyNamee"] + "" == "" ? "&nbsp;" : "/" + RS["SCompanyNamee"]),
					RS["ShipperCode"] + "",
					RS["CCompanyName"] + (RS["CCompanyNamee"] + "" == "" ? "&nbsp;" : "/" + RS["CCompanyNamee"]),
					RS["ConsigneeCode"] + "" == "" ? "<span style=\"color:red;\">NEW</span>" : RS["ConsigneeCode"],
					(RS["TotalPackedCount"] + "" == "" ? "" : Common.NumberFormat(RS["TotalPackedCount"] + "") + Common.GetPackingUnit(RS["PackingUnit"] + "")),
					Common.NumberFormat(RS["TotalGrossWeight"] + ""),
					Common.NumberFormat(RS["TotalVolume"] + ""),
					(RS["MsgCount"] + "") == "" ? "<img src=\"../Images/CheckFalse.jpg\" style=\"width:20px;\" alt=\"\" />" : "<a onclick=\"PopMsgSend('" + RS["RequestFormPk"] + "');\">" + RS["MsgCount"] + "</a>",
					IsCostCharged,
					RS["ConsigneePk"] + ""
					));
			} else {
				break;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		HtmlList.Append(string.Format(TableFormat, RowValue + "", new Common().SetPageListByNo(pageLength, PageNo, totlaRecord, "RequestList_Logistics.aspx", "?")));
		return true;

	}
}