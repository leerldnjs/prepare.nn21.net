using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Admin_RequestList : System.Web.UI.Page
{
	protected String[] MemberInfo;
	protected StringBuilder HtmlList;
	private int PageNo;
	private DBConn DB;
	protected void Page_Load(object sender, EventArgs e) {
		/*
		Session["MemberInfo"] = "ShippingBranch!11773!ilbj0!!총괄";
		Session["Type"] = "ShippingBranch";
		Session["SubInfo"] = "CNBJ!Beijing";
		Session["ID"] = "ilbj0";
		Session["CompanyPk"] = "11773";
		*/
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../Default.aspx"); }
		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } } catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }

		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		try { PageNo = Int32.Parse(Request["PageNo"]); } catch (Exception) { PageNo = 1; }

		string Gubun = Request["G"] + "";
		switch (Gubun) {
			case "5051":
				Load5051(MemberInfo[1]);
				break;
			case "5455":
				Load5455(MemberInfo[1]);
				break;
			case "33":
				Load33();
				break;
			default:
				LoadArrival(MemberInfo[1]);
				break;
		}

		if (MemberInfo[0] == "Customs") {
			LogedWithoutRecentRequest1.Visible = false;
			Loged1.Visible = true;
		} else if (MemberInfo[0] == "OurBranchOut") {
			LogedWithoutRecentRequest1.Visible = false;
			Loged2.Visible = true;
		}
	}
	private Boolean Load33() {
		Boolean ReturnValue = false;
		Int32 pageLength = 30;
		Int32 totlaRecord;
		String TableFormat = "<table border='0' cellpadding='0' cellspacing='0' style='width:850px;' ><thead><tr height='30px'>" +
						"<td class='THead1' style='width:60px;' >" + GetGlobalResourceObject("Member", "CompanyCode") + "</td>" +
						"<td class='THead1' style='width:200px;' >Company Name</td>" +
						"<td class='THead1'>Booking Summary</td>" +
						"<td class='THead1' style='width:60px;'>Date of shipment</td>" +
						"<td class='THead1' style='width:150px;'>Items</td>" +
						"<td class='THead1' style='width:40px;'>CT</td>" +
						"<td class='THead1' style='width:50px;'>Kg</td>" +
						"<td class='THead1' style='width:50px;'>CBM</td>" +
						"<td class='THead1' style='width:60px;'>" + GetGlobalResourceObject("Member", "RequestDate") + "</td>" +
					"</tr></thead>" +
					"{0}" +
					"<TR><td colspan='9' style='background-color:#F5F5F5; text-align:center; padding:20px; '>{1}</TD></TR></Table>";
		String TableRowFormat = "	<tr height='30px; '><td class='{0}' >{2}</td>" +
												"		<td class='{0}' >{3}</td>" +
												"		<td class='{0}' ><a href=\"RequestViewEstimation.aspx?g=s&pk={1}\">{4}</a></td>" +
												"		<td class='{0}' >{10}</td>" +
												"		<td class='{0}' >{5}</td>" +
												"		<td class='{0}' >{6}</td>" +
												"		<td class='{0}' >{7}</td>" +
												"		<td class='{0}' >{8}</td>" +
												"		<td class='{0}' >{9}</td></tr>";
		DB = new DBConn();
		DB.SqlCmd.CommandText = @" 
SELECT count(*) 
FROM RequestForm AS RF 
	LEFT JOIN RegionCode AS R ON RF.DepartureRegionCode=R.RegionCode  
WHERE StepCL=33;";
		DB.DBCon.Open();
		totlaRecord = (Int32)DB.SqlCmd.ExecuteScalar();
		if (totlaRecord == 0) {
			DB.DBCon.Close();
			return false;
		}
		StringBuilder RowValue = new StringBuilder();
		DB.SqlCmd.CommandText = @" 
SELECT 
	RF.RequestFormPk, RF.ShipperPk, RF.ShipperCode, RF.DepartureDate, RF.ArrivalDate, RF.ShipmentDate, RF.StepCL, RF.RequestDate
	  , C.CompanyName, C.CompanyNameE
	  , Departure.Name as DName
	  , Arrival.Name as AName
	  , RI.Description
	  , RII.itemCount, RII.packedCount, RII.GrossWeight, RII.Volume 
	 FROM RequestForm AS RF 
		 Left join Company AS C on RF.ShipperPk=C.CompanyPk 
		 Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
		 Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
		 Left join RequestFormItems as RI on RF.RequestFormPk=RI.RequestFormPk 
		 Left join ( SELECT S.RequestFormPk, Count(*) as itemCount, sum(PackedCount) as packedCount, sum(GrossWeight) as GrossWeight, sum(Volume) as Volume FROM RequestFormItems as S group by RequestFormPk )as RII on RF.RequestFormPk=RII.RequestFormPk 
	 WHERE StepCL =33 
	 Order by RequestDate DESC;";
		string Style = "";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		String TempRequestFormPk = string.Empty;
		int k = 0;
		while (k < (PageNo - 1) * pageLength) {
			if (RS.Read()) {
				if (TempRequestFormPk != RS["RequestFormPk"] + "") {
					TempRequestFormPk = RS["RequestFormPk"] + "";
					k++;
				}
			} else {
				break;
			}
		}
		DateTime Registerd;
		for (int i = 0; i < pageLength; i++) {
			if (RS.Read()) {
				if (TempRequestFormPk != RS["RequestFormPk"] + "") {
					TempRequestFormPk = RS["RequestFormPk"] + "";
					Registerd = Convert.ToDateTime(RS["RequestDate"]);
					string RegisterdString = "";
					if (Registerd.Date == DateTime.Now.Date) {
						RegisterdString = "<span style=\"color:blue;\">" + Registerd.TimeOfDay.ToString().Substring(0, 5) + "</span>";
					} else {
						RegisterdString = "<span style=\"color:black;\">" + Registerd.Date.ToString().Substring(2, 9) + "</span>";
					}
					if (DateTime.Now.AddDays(-30) < Registerd) {
						Style = "Check";
					}
					String[] rowdata = new string[]{
						Style + "" == "" ? "TBody1G" : "TBody1",
						RS["RequestFormPk"]+"",
						RS["ShipperCode"]+"",
						RS["CompanyName"] + (RS["CompanyNamee"] + "" == "" ? "&nbsp;" : "/" + RS["CompanyNamee"]) ,
						(RS["DepartureDate"] + "" == ""?"":RS["DepartureDate"].ToString().Substring(4, 2)+"/" +RS["DepartureDate"].ToString().Substring(6, 2))+ " (" + RS["DName"] + ") ~ " +
						(RS["ArrivalDate"] + "" == ""?"":RS["ArrivalDate"].ToString().Substring(4, 2)+"/" +RS["ArrivalDate"].ToString().Substring(6, 2))+  " (" + RS["AName"] + ")",
						RS["Description"] + (RS["itemCount"] + "" == "" ? "&nbsp;" : " (" + RS["itemCount"] + ")") ,
						RS["packedCount"]+""==""?"&nbsp;":RS["packedCount"]+"",
						Common.NumberFormat(RS["GrossWeight"] + ""),
						Common.NumberFormat(RS["Volume"] + ""),
						RegisterdString,
						RS["ShipmentDate"]+""==""?"":RS["ShipmentDate"].ToString().Substring(4, 2)+"/" +RS["ShipmentDate"].ToString().Substring(6, 2)
					};
					RowValue.Append(String.Format(TableRowFormat, rowdata));
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
		HtmlList = new StringBuilder();
		if (Session["Type"] + "" != "ShippingBranch") {
			HtmlList.Append("<p><a href=\"RequestList.aspx?G=5051\">발송예약</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"RequestList.aspx?G=33\"><strong>견적</strong></a>");
		}
		HtmlList.Append(String.Format(TableFormat,
			RowValue + "",
			new Common().SetPageListByNo(pageLength, PageNo, totlaRecord, "RequestList.aspx", "?G=33&")));
		return true;
	}

	private Boolean Load5051(string BranchPk) {
		Int32 pageLength = 30;
		Int32 totlaRecord;
		String TableFormat = "<table border='0' cellpadding='0' cellspacing='0' style='width:850px;' ><thead><tr height='30px'>" +
						"<td class='THead1' style='width:60px;' >" + GetGlobalResourceObject("Member", "CompanyCode") + "</td>" +
						"<td class='THead1' style='width:200px;' >Company Name</td>" +
						"<td class='THead1'>Booking Summary</td>" +
						"<td class='THead1' style='width:60px;'>Date of shipment</td>" +
						"<td class='THead1' style='width:150px;'>Items</td>" +
						"<td class='THead1' style='width:40px;'>CT</td>" +
						"<td class='THead1' style='width:50px;'>Kg</td>" +
						"<td class='THead1' style='width:50px;'>CBM</td>" +
						"<td class='THead1' style='width:60px;'>" + GetGlobalResourceObject("Member", "RequestDate") + "</td>" +
					"</tr></thead>" +
					"{0}" +
					"<TR><td colspan='9' style='background-color:#F5F5F5; text-align:center; padding:20px; '>{1}</TD></TR></Table>";
		String TableRowFormat = "	<tr height='30px; '><td class='{0}' >{2}</td>" +
												"		<td class='{0}' >{3}</td>" +
												"		<td class='{0}' ><a href=\"RequestView.aspx?g=s&pk={1}\">{4}</a></td>" +
												"		<td class='{0}' >{10}</td>" +
												"		<td class='{0}' >{5}</td>" +
												"		<td class='{0}' >{6}</td>" +
												"		<td class='{0}' >{7}</td>" +
												"		<td class='{0}' >{8}</td>" +
												"		<td class='{0}' >{9}</td></tr>";
		DB = new DBConn();
		DB.SqlCmd.CommandText = @" 
SELECT count(*) 
FROM RequestForm AS RF 
	LEFT JOIN RegionCode AS R ON RF.DepartureRegionCode=R.RegionCode  
WHERE R.OurBranchCode=" + BranchPk + " and StepCL in (50, 51);";
		DB.DBCon.Open();
		totlaRecord = (Int32)DB.SqlCmd.ExecuteScalar();
		if (totlaRecord == 0) {
			DB.DBCon.Close();
			HtmlList = new StringBuilder();
			if (Session["Type"] + "" != "ShippingBranch") {
				HtmlList.Append("<p><a href=\"RequestList.aspx?G=5051\">발송예약</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"RequestList.aspx?G=33\"><strong>견적</strong></a>");
			}
			return false;
		}
		StringBuilder RowValue = new StringBuilder();
		DB.SqlCmd.CommandText = " EXECUTE SP_SelectRequestList5051 @BranchPk=" + BranchPk + ";";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		String TempRequestFormPk = string.Empty;
		int k = 0;
		while (k < (PageNo - 1) * pageLength) {
			if (RS.Read()) {
				if (TempRequestFormPk != RS["RequestFormPk"] + "") {
					TempRequestFormPk = RS["RequestFormPk"] + "";
					k++;
				}
			} else {
				break;
			}
		}
		DateTime Registerd;
		for (int i = 0; i < pageLength; i++) {
			if (RS.Read()) {
				if (TempRequestFormPk != RS["RequestFormPk"] + "") {
					TempRequestFormPk = RS["RequestFormPk"] + "";
					Registerd = Convert.ToDateTime(RS["RequestDate"]);
					string RegisterdString = "";
					if (Registerd.Date == DateTime.Now.Date) {
						RegisterdString = "<span style=\"color:blue;\">" + Registerd.TimeOfDay.ToString().Substring(0, 5) + "</span>";
					} else {
						RegisterdString = "<span style=\"color:black;\">" + Registerd.Date.ToString().Substring(2, 9) + "</span>";
					}
					String[] rowdata = new string[]{
						RS["StepCL"] + "" == "50" ? "TBody1G" : "TBody1",
						RS["RequestFormPk"]+"",
						RS["ShipperCode"]+"",
						RS["CompanyName"] + (RS["CompanyNamee"] + "" == "" ? "&nbsp;" : "/" + RS["CompanyNamee"]) ,
						(RS["DepartureDate"] + "" == ""?"":RS["DepartureDate"].ToString().Substring(4, 2)+"/" +RS["DepartureDate"].ToString().Substring(6, 2))+ " (" + RS["DName"] + ") ~ " +
						(RS["ArrivalDate"] + "" == ""?"":RS["ArrivalDate"].ToString().Substring(4, 2)+"/" +RS["ArrivalDate"].ToString().Substring(6, 2))+  " (" + RS["AName"] + ")",
						RS["Description"] + (RS["itemCount"] + "" == "" ? "&nbsp;" : " (" + RS["itemCount"] + ")") ,
						RS["packedCount"]+""==""?"&nbsp;":RS["packedCount"]+"",
						Common.NumberFormat(RS["GrossWeight"] + ""),
						Common.NumberFormat(RS["Volume"] + ""),
						RegisterdString,
						RS["ShipmentDate"]+""==""?"":RS["ShipmentDate"].ToString().Substring(4, 2)+"/" +RS["ShipmentDate"].ToString().Substring(6, 2)
					};
					RowValue.Append(String.Format(TableRowFormat, rowdata));
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
		HtmlList = new StringBuilder();
		if (BranchPk == "2888") {
			P5051.Visible = true;
		}
		HtmlList.Append("<p><a href=\"RequestList.aspx?G=5051\"><strong>발송예약</strong></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"RequestList.aspx?G=33\">견적</a>");
		HtmlList.Append(String.Format(TableFormat,
			RowValue + "",
			new Common().SetPageListByNo(pageLength, PageNo, totlaRecord, "RequestList.aspx", "?G=5051&")));

		return true;
	}

	private Boolean Load5455(string BranchPk) {
		Int32 pageLength = 30;
		Int32 totlaRecord;
		String TableFormat = "<table border='0' cellpadding='0' cellspacing='0' style='width:850px;' ><thead><tr height='30px'>" +
								"<td class='THead1' style='width:45px;' >Storced</td>" +
								"<td class='THead1' style='width:80px;' >Destination</td>" +
								"<td class='THead1' style='width:45px;' >Start</td>" +
								"<td class='THead1' style='width:45px;'>Arrival</td>" +
								"<td class='THead1' style='width:45px;'>Date of shipment</td>" +
								"<td class='THead1' style='width:75px;'>Transport</td>" +
								"<td class='THead1' colspan=\"2\">Customer</td>" +
								"<td class='THead1' style='width:50px;'>count</td>" +
								"<td class='THead1' style='width:45px;'>Kg</td>" +
								"<td class='THead1' style='width:45px;'>CBM</td>" +
								"<td class='THead1' style='width:20px;'>p</td>" +
								"<td class='THead1' style='width:20px;'>mc</td>" +
							"</tr></thead>{0}<TR><td colspan='12' style='background-color:#F5F5F5; text-align:center; padding:20px; '>{1}</TD></TR></Table>";
		String TableRowFormat = "	<tr>" +
						"<td class='{0}'>{2}</td>" +
						"<td class='{0}'>{3}</td>" +
						"<td class='{0}'>{4}</td>" +
						"<td class='{0}'>{5}</td>" +
						"<td class='{0}'>{14}</td>" +
						"<td class='{0}'>{6}</td>" +
						"<td class='{0}' style='padding-left:5px; '><a href=\"RequestView.aspx?g=s&pk={1} \">{7}</a></td>" +
						"<td class='{0}' style='text-align:left; padding-left:5px; '><a href=\"RequestView.aspx?g=s&pk={1} \">{8}</a></td>" +
						"<td class='{0}'>{9}</td>" +
						"<td class='{0}'>{10}</td>" +
						"<td class='{0}'>{11}</td>" +
						"<td class='{0}'><img src=\"../Images/Check{12}.jpg\" style=\"width:20px;\" alt=\"\" /></td>" +
						"<td class='{0}' >{13}</a></td></tr>";
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT count(*)  
FROM RequestForm AS RF 
	LEFT JOIN RegionCode AS R ON RF.DepartureRegionCode=R.RegionCode  
	left join (
		SELECT [REQUEST_PK], sum([PACKED_COUNT]) AS TotalBox 
		FROM [dbo].[STORAGE] AS OBS 
			left join OurBranchStorageCode AS OBSC on OBS.[WAREHOUSE_PK]=OBSC.OurBranchStoragePk 
		WHERE OBSC.StorageCode=" + BranchPk + @"
		group by [REQUEST_PK]
		) AS StorageC on StorageC.[REQUEST_PK]=RF.RequestFormPk 
WHERE R.OurBranchCode=" + BranchPk + " and StepCL in (52, 54, 55, 56, 57, 58) and (StorageC.TotalBox>0 or isnull(RF.DocumentStepCL, -1)=-1) ;";
		DB.DBCon.Open();
		totlaRecord = (Int32)DB.SqlCmd.ExecuteScalar();
		if (totlaRecord == 0) {
			DB.DBCon.Close();
			return false;
		}

		DB.SqlCmd.CommandText = @"
SELECT RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
	RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentRequestCL, RF.StepCL, RF.DocumentStepCL , RF.StockedDate, RF.ShipmentDate,
	C.CompanyName as SCompanyName, C.CompanyNameE as SCompanyNamee,
	CC.CompanyName as CCompanyName, CC.CompanyNameE as CCompanyNamee, 
	CCL.TargetCompanyName, Departure.NameE AS DepartureArea, Arrival.NameE AS ArrivalArea, 
	RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume , Msg.MsgCount
FROM RequestForm AS RF
	Left join Company AS C on RF.ShipperPk = C.CompanyPk
	Left join Company AS CC on RF.ConsigneePk = CC.CompanyPk
	Left join CompanyCustomerList AS CCL on RF.ConsigneeCCLPk = CCL.CompanyCustomerListPk
	Left join RegionCode AS Departure on RF.DepartureRegionCode = Departure.RegionCode
	Left join RegionCode AS Arrival on RF.ArrivalRegionCode = Arrival.RegionCode
	left join
	(
		SELECT [REQUEST_PK] , sum([PACKED_COUNT]) AS TotalBox
		FROM [dbo].[STORAGE] AS OBS
			left join OurBranchStorageCode AS OBSC on OBS.[WAREHOUSE_PK]= OBSC.OurBranchStoragePk
		WHERE OBSC.OurBranchCode= " + BranchPk + @"--and OBS.StatusCL<2
		group by REQUEST_PK
	) AS StorageC on StorageC.[REQUEST_PK] = RF.RequestFormPk
	left join(SELECT Count(*) as MsgCount, GubunPk FROM MsgSendedHistory Group by GubunPk) AS Msg ON RF.RequestFormPk = Msg.GubunPk
WHERE Departure.OurBranchCode = " + BranchPk + @"and StepCL in (52, 54, 55, 56, 57, 58) and(StorageC.TotalBox > 0 or isnull(RF.DocumentStepCL, -1) = -1)
Order by RF.StockedDate DESC, RF.DepartureDate ASC, RF.ArrivalDate ASC, RF.StepCL ASC, RF.ShipperCode asc";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int j = 0;
		while (j < (PageNo - 1) * pageLength) {
			if (RS.Read()) {
				j++;
				continue;
			} else {
				break;
			}
		}
		StringBuilder RowValue = new StringBuilder();
		for (int i = 0; i < pageLength; i++) {
			if (RS.Read()) {
				string[] rowdata = new string[15];

				if (RS["StepCL"] + "" == "52") {
					rowdata[0] = "TBody1B";
				} else if (RS["TransportWayCL"] + "" == "28" || RS["TransportWayCL"] + "" == "29") {
					rowdata[0] = "TBody1R";
				} else {
					if (RS["DocumentStepCL"] + "" == "") {
						rowdata[0] = "TBody1G";
					} else {
						rowdata[0] = "TBody1";
					}
				}
				rowdata[1] = RS["RequestFormPk"] + "";
				rowdata[2] = (RS["StockedDate"] + "").Length > 7 ? String.Format("{0}/{1}", (RS["StockedDate"] + "").Substring(4, 2), (RS["StockedDate"] + "").Substring(6, 2)) : "";
				rowdata[3] = RS["ArrivalArea"] + "";
				rowdata[4] = RS["DepartureDate"] + "" == "" ? "" : String.Format("{0}/{1}", RS["DepartureDate"].ToString().Substring(4, 2), RS["DepartureDate"].ToString().Substring(6, 2));
				rowdata[5] = RS["ArrivalDate"] + "" == "" ? "" : String.Format("{0}/{1}", RS["ArrivalDate"].ToString().Substring(4, 2), RS["ArrivalDate"].ToString().Substring(6, 2));
				rowdata[6] = Common.GetTransportWay(RS["TransportWayCL"] + "");
				rowdata[7] = (RS["ShipperCode"] + "" == "" ? "" : RS["ShipperCode"] + "") + " ~ " + RS["ConsigneeCode"];
				rowdata[8] = RS["SCompanyName"] + (RS["SCompanyNamee"] + "" == "" ? "&nbsp;" : "/" + RS["SCompanyNamee"]) + " ~ " + RS["CCompanyName"] + (RS["CCompanyNamee"] + "" == "" ? "&nbsp;" : "/" + RS["CCompanyNamee"]);
				rowdata[9] = (RS["TotalPackedCount"] + "" == "" ? "" : Common.NumberFormat(RS["TotalPackedCount"] + "") + Common.GetPackingUnit(RS["PackingUnit"] + ""));
				rowdata[10] = Common.NumberFormat(RS["TotalGrossWeight"] + "");
				rowdata[11] = Common.NumberFormat(RS["TotalVolume"] + "");
				rowdata[12] = RS["StepCL"] + "" == "58" ? "True" : "False";
				rowdata[13] = (RS["MsgCount"] + "") == "" ? "<img src=\"../Images/CheckFalse.jpg\" style=\"width:20px;\" alt=\"\" />" : "<a onclick=\"PopMsgSend('" + RS["RequestFormPk"] + "');\">" + RS["MsgCount"] + "</a>";
				rowdata[14] = RS["ShipmentDate"] + "" == "" ? "" : String.Format("{0}/{1}", RS["ShipmentDate"].ToString().Substring(4, 2), RS["ShipmentDate"].ToString().Substring(6, 2));
				RowValue.Append(string.Format(TableRowFormat, rowdata));
				continue;
			} else {
				break;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		HtmlList = new StringBuilder();
		if (BranchPk == "2888") {
			P5455.Visible = true;
		}
		HtmlList.Append(String.Format(TableFormat,
			RowValue + "",
			new Common().SetPageListByNo(pageLength, PageNo, totlaRecord, "RequestList.aspx", "?G=5455&")));
		//return true;

		return true;
	}
	private Boolean LoadArrival(string BranchPk) {
		//if (MemberInfo[0] == "OurBranch") {
		HtmlList = new StringBuilder();
		Int32 pageLength = 30;
		Int32 totlaRecord;
		String TableFormat = "<table border='0' cellpadding='0' cellspacing='0' style='width:850px;' ><thead><tr height='30px'>" +
						"<td class='THead1' style='width:55px;' >Start</td>" +
						"<td class='THead1' style='width:80px;' >Departure</td>" +
						"<td class='THead1' style='width:55px;'>Arrival</td>" +
						"<td class='THead1' style='width:55px;'>Date of Shipment</td>" +
						"<td class='THead1' style='width:80px;'>Transport</td>" +
						"<td class='THead1' colspan=\"2\" >Customer</td>" +
						"<td class='THead1' style='width:50px;'>count</td>" +
						"<td class='THead1' style='width:45px;'>Kg</td>" +
						"<td class='THead1' style='width:45px;'>CBM</td>" +
						"<td class='THead1' style='width:20px;'>p</td>" +
						"<td class='THead1' style='width:20px;'>mc</td>" +
						"<td class='THead1' style='width:20px;'>F</td>" +
					"</tr></thead>{0}<TR><td colspan='11' style='background-color:#F5F5F5; text-align:center; padding:20px; '>{1}</TD></TR></Table>";
		String TableRowFormat = "<tr height='30px'>" +
								"<td class='{0}'>{1}</td>" +
								"<td class='{0}'>{2}</td>" +
								"<td class='{0}'>{3}</td>" +
								"<td class='{0}'>{15}</td>" +
								"<td class='{0}'>{4}</td>" +
								"<td class='{0}' style='text-align:center;'><a href=\"RequestView.aspx?g=c&pk={5} \"><strong>{7} ~ {9}</strong> </a></td>" +
								"<td class='{0}' style='text-align:left;'>&nbsp;&nbsp;&nbsp;<a href=\"RequestView.aspx?g=c&pk={5} \">{6} ~ {8}</a></td>" +
								"<td class='{0}'>{10}</td>" +
								"<td class='{0}'>{11}</td>" +
								"<td class='{0}'>{12}</td>" +
								"<td class='{0}'><img src=\"../Images/Check{14}.jpg\" style=\"width:20px;\" alt=\"\" /></td>" +
								"<td class='{0}'>{13}</td>" +
								"<td class='{0}'>{16}</td></tr>";
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"	
SELECT count(*)  
FROM RequestForm AS RF 
	LEFT JOIN RegionCode AS R ON RF.ArrivalRegionCode=R.RegionCode 
WHERE R.OurBranchCode=" + BranchPk + " and StepCL in (54, 55, 56, 57, 58, 59, 60, 61) and isnull(DocumentStepCL, 0)=0;";

		DB.DBCon.Open();

		totlaRecord = (Int32)DB.SqlCmd.ExecuteScalar();
		if (totlaRecord == 0) {
			DB.DBCon.Close();
			if (MemberInfo[0] == "OurBranch") {
				HtmlList.Append("<p><a href=\"RequestList.aspx?G=Arrival\"><strong>출발지 입고완료</strong></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"CheckDescriptionList.aspx\">BL List</a>");
				HtmlList.Append("&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"../Request/RequestModifyHistory.aspx\">Modify History</a></p>");

			}

			return false;
		}
		DB.SqlCmd.CommandText = @" SELECT RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
										  RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentRequestCL, RF.StepCL, RF.DocumentStepCL, 
										  RF.StockedDate, RF.ShipmentDate,
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
								WHERE Arrival.OurBranchCode=" + BranchPk + @" and StepCL in (54, 55, 56, 57, 58, 59, 60, 61) and isnull(DocumentStepCL, 0)=0
								ORDER BY RF.ArrivalDate ASC, RF.StockedDate DESC,  RF.DepartureDate ASC, RF.StepCL ASC, RF.ShipperCode asc;";


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
				string IsFTA = "";
				string[] Documents = RS["DocumentRequestCL"].ToString().Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
				foreach (string T in Documents) {
					if (T == "34") {
						IsFTA = "True";
					}
				}
				if (RS["TransportWayCL"] + "" == "28" || RS["TransportWayCL"] + "" == "29") {
					StyleID = "TBody1R";
				} else if (RS["DocumentStepCL"] + "" == "") {
					StyleID = "TBody1G";
				} else {
					StyleID = "TBody1";
				}
				//Response.Write(RS["PackingUnit"] + " / ");
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
					(RS["ShipmentDate"] + "" == "" ? "&nbsp" : (RS["ShipmentDate"] + "").Substring(2)),
					IsFTA == "True" ? "<img src=\"../Images/CheckTrue.jpg\" style=\"width:20px;\" alt=\"\" />" : "&nbsp"
					));
			} else {
				break;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		if (MemberInfo[0] == "OurBranch") {
			HtmlList.Append("<p><a href=\"RequestList.aspx?G=Arrival\"><strong>출발지 입고완료</strong></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"CheckDescriptionList.aspx\">BL List</a>");
			HtmlList.Append("&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"../Request/RequestModifyHistory.aspx\">Modify History</a></p>");
		}
		HtmlList.Append(string.Format(TableFormat, RowValue + "", new Common().SetPageListByNo(pageLength, PageNo, totlaRecord, "RequestList.aspx", "?G=Arrival&")));
		return true;
	}

	protected void Button1_Click(object sender, EventArgs e) {
		Load5051("2888");
	}
	protected void Button2_Click(object sender, EventArgs e) {
		Load5051("7898");
	}
	protected void Button3_Click(object sender, EventArgs e) {
		Load5455("2888");
	}
	protected void Button4_Click(object sender, EventArgs e) {
		Load5455("7898");
	}
}