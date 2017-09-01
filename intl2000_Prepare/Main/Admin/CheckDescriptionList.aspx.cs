using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_CheckDescriptionList : System.Web.UI.Page
{

	protected String[] MemberInfo;
	protected String Gubun;
	protected String Header;
	protected String PageDate;

	protected String RequestList;
	protected Int32 Count;
	private DBConn DB;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../Default.aspx"); }
		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } } catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		if (MemberInfo[0] == "Customs") {
			LogedWithoutRecentRequest111.Visible = false;
			Loged1.Visible = true;
			Header = "";
		} else {
			Header = "<p><a href=\"RequestList.aspx?G=Arrival\">출발지 입고완료</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"CheckDescriptionList.aspx\"><strong>BL List</strong></a>" +
				"&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"SetTariff.aspx\">세금맞추기</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"AccountTariff.aspx\">관세사로 세금 송금하기</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"AccountTariffList.aspx\">세금 송금내역</a></p>";
		}

		DB = new DBConn();
		if (Request.Params["Type"] + "" == "SerchBL") {
			RequestList = LoadList_SerchBL(Request.Params["Value"]);
		} else {
			PageDate = MemberInfo[0] == "Customs" ? LoadPageDateForCustoms(MemberInfo[1]) : LoadPageDate(MemberInfo[1]);
			string gubun = Request.Params["G"] + "" == "" ? "O" : Request.Params["G"] + "";
			string date = Request.Params["D"] + "" == "" ? "A" : Request.Params["D"] + "";
			RequestList = MemberInfo[0] == "Customs" ? LoadListForCustoms(MemberInfo[1], gubun, date) : LoadList(MemberInfo[1], gubun, date);
		}
	}
	private String LoadPageDateForCustoms(string OurBranchPk) {
		List<int> NotCalculatedList = new List<int>();
		List<string> NotCalculatedCount = new List<string>();
		Int32 NotCalculatedTotal = 0;
		List<int> CalculatedList = new List<int>();
		List<string> CalculatedCount = new List<string>();
		Int32 CalculatedTotal = 0;

		DB.SqlCmd.CommandText = @"	SELECT RF.ArrivalDate, Count(*) AS DocumentCount 
															FROM RequestForm AS RF 
																Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
																inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk 
															WHERE Arrival.OurBranchCode=" + OurBranchPk + @" and  RF.DocumentStepCL=2 and RF.StepCL<64 
															Group By RF.ArrivalDate 
															Order by RF.ArrivalDate ASC;";

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			NotCalculatedList.Add(Int32.Parse("" + RS[0]));
			NotCalculatedCount.Add("" + RS[1]);
			NotCalculatedTotal += Int32.Parse("" + RS[1]);
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"	SELECT RF.ArrivalDate, Count(*) AS DocumentCount 
															FROM RequestForm AS RF 
																Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
																inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk 
															WHERE Arrival.OurBranchCode=" + OurBranchPk + @" and RF.DocumentStepCL=5 and RF.StepCL<64 
															Group By RF.ArrivalDate 
															Order by RF.ArrivalDate ASC;";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			CalculatedList.Add(Int32.Parse("" + RS[0]));
			CalculatedCount.Add("" + RS[1]);
			CalculatedTotal += Int32.Parse("" + RS[1]);
		}
		RS.Dispose();
		StringBuilder ReturnValue = new StringBuilder();
		int tempJ = 0;
		string EachBox = "<div class=\"Date\"><div class=\"DateInnerDate\">{0}-{1}</div><a href='./CheckDescriptionList.aspx?D={4}&G=O'>{2}</a> / <a href='./CheckDescriptionList.aspx?D={4}&G=T'>{3}</a></div>";
		//15 23

		if (NotCalculatedTotal > 0 && CalculatedTotal > 0) {
			for (int i = 0; i < NotCalculatedList.Count; i++) {
				if (CalculatedList.Count == tempJ) {
					ReturnValue.Append(String.Format(EachBox, (NotCalculatedList[i] + "").Substring(4, 2), (NotCalculatedList[i] + "").Substring(6, 2), NotCalculatedCount[i], "0", NotCalculatedList[i]));
					continue;
				}
				if (NotCalculatedList[i] == CalculatedList[tempJ]) {
					ReturnValue.Append(String.Format(EachBox, (NotCalculatedList[i] + "").Substring(4, 2), (NotCalculatedList[i] + "").Substring(6, 2), NotCalculatedCount[i], CalculatedCount[tempJ], NotCalculatedList[i]));
					tempJ++;
					continue;
				}
				if (NotCalculatedList[i] < CalculatedList[tempJ]) {
					ReturnValue.Append(String.Format(EachBox, (NotCalculatedList[i] + "").Substring(4, 2), (NotCalculatedList[i] + "").Substring(6, 2), NotCalculatedCount[i], "0", NotCalculatedList[i]));
					continue;
				}
				bool iinc = false;
				while (CalculatedList.Count > tempJ && NotCalculatedList[i] > CalculatedList[tempJ]) {
					ReturnValue.Append(String.Format(EachBox, (CalculatedList[tempJ] + "").Substring(4, 2), (CalculatedList[tempJ] + "").Substring(6, 2), "0", CalculatedCount[tempJ], CalculatedList[tempJ]));
					tempJ++;
					iinc = true;
				}
				if (iinc) { i--; }
			}
			if (CalculatedList.Count > tempJ) {
				while (CalculatedList.Count == tempJ) {
					ReturnValue.Append(String.Format(EachBox, (CalculatedList[tempJ] + "").Substring(4, 2), (CalculatedList[tempJ] + "").Substring(6, 2), "0", CalculatedCount[tempJ], CalculatedList[tempJ]));
					tempJ++;
				}
			}
		} else if (NotCalculatedTotal == 0) {
			for (int i = 0; i < CalculatedList.Count; i++) {
				ReturnValue.Append(String.Format(EachBox, (CalculatedList[i] + "").Substring(4, 2), (CalculatedList[i] + "").Substring(6, 2), "0", CalculatedCount[i], CalculatedList[i]));
			}

		} else if (CalculatedTotal == 0) {
			for (int i = 0; i < NotCalculatedList.Count; i++) {
				ReturnValue.Append(String.Format(EachBox, (NotCalculatedList[i] + "").Substring(4, 2), (NotCalculatedList[i] + "").Substring(6, 2), NotCalculatedCount[i], "0", NotCalculatedList[i]));

			}
		}
		return String.Format("<div class=\"Date\"><div class=\"DateInnerDate\">TOTAL</div><a href='./CheckDescriptionList.aspx?D=A&G=O'>{0}</a> / <a href='./CheckDescriptionList.aspx?D=A&G=T'>{1}</a></div>", NotCalculatedTotal, CalculatedTotal) + ReturnValue;
	}
	private String LoadPageDate(string OurBranchPk) {
		List<string> OwnDateList = new List<string>();
		List<string> OwnDateCount = new List<string>();
		DB.SqlCmd.CommandText = @"	SELECT CD.ClearanceDate, Count(*) AS DocumentCount 
															FROM RequestForm AS RF 
																Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
																inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk 
																left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
															WHERE Arrival.OurBranchCode=" + OurBranchPk + @" and RF.StepCL<64 and RF.DocumentStepCL<2
															Group By CD.ClearanceDate 
															Order by CD.ClearanceDate ASC;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {

			OwnDateList.Add("" + RS[0]);
			OwnDateCount.Add("" + RS[1]);
		}
		RS.Dispose();
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"	SELECT CD.ClearanceDate, Count(*) AS DocumentCount 
															FROM RequestForm AS RF 
																Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
																inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk 
																left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
															WHERE Arrival.OurBranchCode=" + OurBranchPk + @" and RF.StepCL<64 and RF.DocumentStepCL in (1, 2, 5, 6)
															Group By CD.ClearanceDate 
															Order by CD.ClearanceDate ASC;";
		RS = DB.SqlCmd.ExecuteReader();
		int k = OwnDateList.Count == 0 ? -1 : 0;
		int OwnTotalCount = 0;
		int OurBranchTotalCount = 0;
		while (RS.Read()) {
			string tempOwnCount = "0";
			if (k != -1 && RS[0] + "" == OwnDateList[k]) {
				tempOwnCount = OwnDateCount[k];
				OwnTotalCount += Int32.Parse(OwnDateCount[k]);
				k++;
				if (k == OwnDateCount.Count) { k = -1; }
			}
			//Response.Write(" " + k + " " + OwnDateCount.Count + "<br />");
			OurBranchTotalCount += Int32.Parse(RS[1] + "");
			ReturnValue.Append(String.Format("<div class=\"Date\"><div class=\"DateInnerDate\">{0}-{1}</div><a href='./CheckDescriptionList.aspx?D=" + (RS[0] + "" == "" ? "N" : RS[0]) + "&G=O'>{2}</a> / <a href='./CheckDescriptionList.aspx?D=" + (RS[0] + "" == "" ? "N" : RS[0]) + "&G=T'>{3}</a></div>", RS[0] + "" == "" ? "--" : (RS[0] + "").Substring(4, 2), RS[0] + "" == "" ? "--" : (RS[0] + "").Substring(6, 2), tempOwnCount, RS[1] + ""));
		}
		RS.Dispose();
		return String.Format("<div class=\"Date\"><div class=\"DateInnerDate\">TOTAL</div><a href='./CheckDescriptionList.aspx?D=A&G=O'>{0}</a> / <a href='./CheckDescriptionList.aspx?D=A&G=T'>{1}</a></div>", OwnTotalCount, OurBranchTotalCount) + ReturnValue;
	}
	private String LoadListForCustoms(string OurBranchPk, string Gubun, string Date) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("	<table border='0' cellpadding='0' cellspacing='0' style='width:1050px;' ><thead><tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; width:60px;  \">STAFF</td>" +
										"		<td class='THead1' style='width:100px;' >ContainerNo</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:150px; \" >Master BL</td>" +
							  "		<td class='THead1' style='width:80px;' >Departure</td>" +
							  "		<td class='THead1' style='width:80px;' >Arrival</td>" +
							  "		<td class='THead1' style='width:90px;'>StartArea</td>" +
										"		<td class='THead1' style='width:90px;'>Transport</td>" +
										"		<td class='THead1'>Customer</td>" +
										"		<td class='THead1' style='width:50px;'>count</td>" +
										"		<td class='THead1' style='width:45px;'>Kg</td>" +
										"		<td class='THead1' style='width:45px;'>CBM</td>" +
										"		<td class='THead1' style='width:45px;'>운임</td>" +
										"		<td class='THead1' style='width:45px;'>세금</td>" +
										"		<td class='THead1' style='width:20px;'>F</td>" +
										"		</tr></thead>");
		string BodyCommercial = "<tr style=\"height:20px;\">" +
											"		<td class='TBody1' style=\"text-align:center; color:green; \">{0}</td>" +
											"		<td class='TBody1' >{17}</td>" +
											"		<td class='TBody1' style=\"text-align:center; font-weight:bold;\" ><a onclick=\"Goto('CheckDescription','{1}');\"><span style=\"cursor:pointer;\">{2}</span></a></td>" +
											"		<td class='TBody1' >{3}</td>" +
											"		<td class='TBody1' >{4}</td>" +
											"		<td class='TBody1' >{5}</td>" +
											"		<td class='TBody1' >{6}</td>" +
											"		<td class='TBody1' >{9} ~ {12}</td>" +
											"		<td class='TBody1' >{13}</td>" +
											"		<td class='TBody1' >{14}</td>" +
											"		<td class='TBody1' >{15}</td>" +
											"		<td class='TBody1' >&nbsp;</td>" +
											"		<td class='TBody1' >&nbsp;</td>" +											
											"		<td class='TBody1' >{18}</td>" +
											"		</tr>";
		string BodyRequest = "<tr style=\"height:20px;\">" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >└---------></td>" +
										"		<td class='TBody1' >{0}</td>" +
										"		<td class='TBody1' >{1}</td>" +
										"		<td class='TBody1' >{2}</td>" +
										"		<td class='TBody1' >{3}</td>" +
										"		<td class='TBody1'>{6} ~ {9}</td>" +
										"		<td class='TBody1' >{10}</td>" +
										"		<td class='TBody1' >{11}</td>" +
										"		<td class='TBody1' >{12}</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"	</tr>";
		if (Gubun == "T" && Date == "A") {
			DB.SqlCmd.CommandText = @"SELECT	
	RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
	RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentStepCL,RF.DocumentRequestCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.Shipper, CD.Consignee, CD.ContainerNo 
	, SC.CompanyName AS ShipperCompanyName
	, CC.CompanyName AS ConsigneeCompanyName
	, Departure.NameE
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
	FROM RequestForm AS RF 
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk 
		left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
		Left join Company AS SC on RF.ShipperPk=SC.CompanyPk 
		left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
		Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
		Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
	WHERE Arrival.OurBranchCode=" + OurBranchPk + @" and RF.DocumentStepCL=5 
	Order by CD.ContainerNo, CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC;";
		} else if (Gubun == "O" && Date == "A") {
			DB.SqlCmd.CommandText = @"SELECT	
	RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
	RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentStepCL,RF.DocumentRequestCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.Shipper, CD.Consignee, CD.ContainerNo
	, SC.CompanyName AS ShipperCompanyName
	, CC.CompanyName AS ConsigneeCompanyName
	, Departure.NameE
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
	FROM RequestForm AS RF 
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk 
		left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
		Left join Company AS SC on RF.ShipperPk=SC.CompanyPk 
		left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
		Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
		Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
	WHERE Arrival.OurBranchCode=" + OurBranchPk + @" and RF.DocumentStepCL=2 
	Order by CD.ContainerNo, CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC;";
		} else if (Gubun == "O" && Date != "A") {
			DB.SqlCmd.CommandText = @"SELECT	
	RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
	RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentStepCL,RF.DocumentRequestCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.Shipper, CD.Consignee, CD.ContainerNo
	, SC.CompanyName AS ShipperCompanyName
	, CC.CompanyName AS ConsigneeCompanyName
	, Departure.NameE
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
	FROM RequestForm AS RF 
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk 
		left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
		Left join Company AS SC on RF.ShipperPk=SC.CompanyPk 
		left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
		Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
		Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
	WHERE Arrival.OurBranchCode=" + OurBranchPk + @" and RF.DocumentStepCL=2 and (CD.ClearanceDate='" + Date + @"' or (isnull(CD.ClearanceDate, '-1')='-1' and RF.ArrivalDate='" + Date + @"' ))
	Order by CD.ContainerNo, CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC;";
		} else if (Gubun != "A") {
			DB.SqlCmd.CommandText = @"SELECT	
	RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
	RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentStepCL,RF.DocumentRequestCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.Shipper, CD.Consignee, CD.ContainerNo
	, SC.CompanyName AS ShipperCompanyName
	, CC.CompanyName AS ConsigneeCompanyName
	, Departure.NameE
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
	FROM RequestForm AS RF 
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk 
		left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
		Left join Company AS SC on RF.ShipperPk=SC.CompanyPk 
		left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
		Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
		Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
	WHERE Arrival.OurBranchCode=" + OurBranchPk + @" and RF.DocumentStepCL=5 and (CD.ClearanceDate='" + Date + @"' or (isnull(CD.ClearanceDate, '-1')='-1' and RF.ArrivalDate='" + Date + @"' ))
	Order by CD.ContainerNo, CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC;";
		}
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string TempCommercialDocumentHeadPk = "";
		while (RS.Read()) {
			if (TempCommercialDocumentHeadPk != RS["CommercialDocumentHeadPk"] + "") {
				string tempDocumentStep;
				switch (RS["DocumentStepCL"] + "") {
					case "1": tempDocumentStep = "미전송"; break;
					case "2": tempDocumentStep = "전송완"; break;
					case "3": tempDocumentStep = "자통"; break;
					case "4": tempDocumentStep = "샘플"; break;
					case "5": tempDocumentStep = "정산완료"; break;
					case "6": tempDocumentStep = "통관지시"; break;
					default: tempDocumentStep = "?"; break;
				}

				string IsFTA = "";
				string[] Documents = RS["DocumentRequestCL"].ToString().Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
				foreach (string T in Documents) {
					if (T == "34") {
						IsFTA = "True";
					}
				}

				TempCommercialDocumentHeadPk = RS["CommercialDocumentHeadPk"] + "";
				ReturnValue.Append(String.Format(BodyCommercial,
					tempDocumentStep,
					RS["CommercialDocumentHeadPk"] + "",
					RS["BLNo"] + "",
					RS["DepartureDate"] + "",
					RS["ArrivalDate"] + "",
					RS["NameE"] + "",
					Common.GetTransportWay(RS["TransportWayCL"] + ""),
					RS["ShipperPk"] + "",
					RS["ShipperCompanyName"] + "",
					RS["ShipperCode"] + "",
					RS["ConsigneePk"] + "",
					RS["ConsigneeCompanyName"] + "",
					RS["ConsigneeCode"] + "",
					RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + ""),
					Common.NumberFormat(RS["TotalGrossWeight"] + ""),
					Common.NumberFormat(RS["TotalVolume"] + ""),
					RS["RequestFormPk"] + "",
					RS["ContainerNo"] + "",
					IsFTA == "True" ? "<img src=\"../Images/CheckTrue.jpg\" style=\"width:20px;\" alt=\"\" />" : "&nbsp"));
			} else {
				ReturnValue.Append(String.Format(BodyRequest,
													RS["DepartureDate"] + "",
													RS["ArrivalDate"] + "",
													RS["NameE"] + "",
													Common.GetTransportWay(RS["TransportWayCL"] + ""),
													RS["ShipperPk"] + "",
													RS["ShipperCompanyName"] + "",
													RS["ShipperCode"] + "",
													RS["ConsigneePk"] + "",
													RS["ConsigneeCompanyName"] + "",
													RS["ConsigneeCode"] + "",
													RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + ""),
													Common.NumberFormat(RS["TotalGrossWeight"] + ""),
													Common.NumberFormat(RS["TotalVolume"] + ""),
													RS["RequestFormPk"] + ""));
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "</table>";
	}
	private String LoadList(string OurBranchPk, string Gubun, string Date) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("	<table border='0' cellpadding='0' cellspacing='0' style='width:1050px;' ><thead><tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; width:60px;  \">STAFF</td>" +
							  "		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; width:30px;  \">etc</td>" +
										"		<td class='THead1' style='width:100px;' >ContainerNo</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:150px; \" >Master BL</td>" +
							  "		<td class='THead1' style='width:80px; ' >Departure</td>" +
							  "		<td class='THead1' style='width:80px;' >Arrival</td>" +
							  "		<td class='THead1' style='width:90px;'>StartArea</td>" +
										"		<td class='THead1' style='width:90px;'>Transport</td>" +
										"		<td class='THead1'>Customer</td>" +
										"		<td class='THead1' style='width:50px;'>count</td>" +
										"		<td class='THead1' style='width:45px;'>Kg</td>" +
										"		<td class='THead1' style='width:45px;'>CBM</td>" +
										"		<td class='THead1' style='width:45px;'>운임</td>" +
										"		<td class='THead1' style='width:45px;'>세금</td>" +
										"		<td class='THead1' style='width:20px;'>F</td>" +
										"		</tr></thead>");
		string BodyCommercial = "<tr style=\"height:20px;\" >" +
											"		<td class='TBody1' style=\"text-align:center; color:green; {20}\">{0}</td>" +
								 "		<td class='TBody1' style=\"text-align:center; color:red;\">{21}</td>" +
											"		<td class='TBody1' >{17}</td>" +
											"		<td class='TBody1' style=\"text-align:center; font-weight:bold; {18}\" ><a onclick=\"Goto('CheckDescription','{1}');\"><span style=\"cursor:pointer;\">{2}</span></a></td>" +
											"		<td class='TBody1' style=\" {19}\" >{3}</td>" +
											"		<td class='TBody1' style=\" {19}\" >{4}</td>" +
											"		<td class='TBody1' >{5}</td>" +
											"		<td class='TBody1' ><a onclick=\"Goto('RequestForm', '{16}');\"><span style=\"cursor:pointer;\">{6}</span></a></td>" +
											"		<td class='TBody1' ><a onclick=\"Goto('Company', '{7}');\">{9}</a> ~ <a onclick=\"Goto('Company', '{10}');\">{12}</a></td>" +
											"		<td class='TBody1' >{13}</td>" +
											"		<td class='TBody1' >{14}</td>" +
											"		<td class='TBody1' >{15}</td>" +
											"		<td class='TBody1' >&nbsp;</td>" +
											"		<td class='TBody1' >&nbsp;</td>" +
											
											"		<td class='TBody1' >{22}</td>" +
											"		</tr>";
		string BodyRequest = "<tr style=\"height:20px;\">" +
										"		<td class='TBody1' >&nbsp;</td>" +
							  "		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >└---------></td>" +
										"		<td class='TBody1' >&nbsp;</td>" +

										"		<td class='TBody1' >{0}</td>" +
										"		<td class='TBody1' >{1}</td>" +
										"		<td class='TBody1' >{2}</td>" +
										"		<td class='TBody1' ><a onclick=\"Goto('RequestForm', '{13}');\">{3}</a></td>" +
										"		<td class='TBody1'><a onclick=\"Goto('Company', '{4}');\">{6}</a> ~ <a onclick=\"Goto('Company', '{7}');\">{9}</a></td>" +
										"		<td class='TBody1' >{10}</td>" +
										"		<td class='TBody1' >{11}</td>" +
										"		<td class='TBody1' >{12}</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"	</tr>";
		if (Date == "N") {
			DB.SqlCmd.CommandText = @"		SELECT	
	RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
	RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentStepCL,RF.DocumentRequestCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.Shipper, CD.Consignee, CD.ContainerNo
	, SC.CompanyName AS ShipperCompanyName
	, CC.CompanyName AS ConsigneeCompanyName
	, Departure.NameE
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
	, Tariff.TariffSum 
,CheckIsCodeNull.C 
,CheckLaw.C CheckLaw
,RHis3.C RHis3C
,RHis4.C RHis4C
,RHis5.C RHis5C
	FROM RequestForm AS RF 
		Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RFCH on RF.RequestFormPk=RFCH.[TABLE_PK] 
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk 
		left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
		Left join Company AS SC on RF.ShipperPk=SC.CompanyPk 
		left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
		Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
		Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
		left join (
			SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TariffSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='제세금' group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff.[REQUESTFORMCALCULATE_HEAD_PK]
left join (
		SELECT R.[RequestFormPk], COUNT(*) AS C
		  FROM [dbo].[RequestForm] AS R 
			left join [dbo].[RequestFormItems] AS RI ON R.RequestFormPk=RI.RequestFormPk 
		 WHERE RI.ItemCode is null  
		 GROUP BY R.RequestFormPk 
	
	) AS CheckIsCodeNull ON CheckIsCodeNull.RequestFormPk=RF.RequestFormPk
left join (
		SELECT R.[RequestFormPk], COUNT(*) AS C
		  FROM [dbo].[RequestForm] AS R 
			left join [dbo].[RequestFormItems] AS RI ON R.RequestFormPk=RI.RequestFormPk 
			left join [dbo].ClearanceItemCodeKOR AS CICK ON CICK.ItemCode=RI.ItemCode
		 WHERE CICK.Law_Nm is not null  
		 GROUP BY R.RequestFormPk 
	
	) AS CheckLaw ON CheckLaw.RequestFormPk=RF.RequestFormPk
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='3' group by [TABLE_NAME], [TABLE_PK]) AS RHis3 on RF.RequestFormPk = RHis3.[RequestFormPk]
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='4' group by [TABLE_NAME], [TABLE_PK]) AS RHis4 on RF.RequestFormPk = RHis4.[RequestFormPk]
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='5' group by [TABLE_NAME], [TABLE_PK]) AS RHis5 on RF.RequestFormPk = RHis5.[RequestFormPk]

	WHERE Arrival.OurBranchCode=" + OurBranchPk + @" 
	AND RF.StepCL<64 and isnull( CD.ClearanceDate, '0')='0'
	AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
	Order by CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC;";

		} else if (Gubun == "T" && Date == "A") {
			DB.SqlCmd.CommandText = @"SELECT	
	RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
	RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentStepCL,RF.DocumentRequestCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.Shipper, CD.Consignee, CD.ContainerNo
	, SC.CompanyName AS ShipperCompanyName
	, CC.CompanyName AS ConsigneeCompanyName
	, Departure.NameE
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
	, Tariff.TariffSum 
,CheckIsCodeNull.C 
,CheckLaw.C CheckLaw
,RHis3.C RHis3C
,RHis4.C RHis4C
,RHis5.C RHis5C
	FROM RequestForm AS RF 
		Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RFCH on RF.RequestFormPk=RFCH.[TABLE_PK]
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk 
		left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
		Left join Company AS SC on RF.ShipperPk=SC.CompanyPk 
		left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
		Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
		Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
		left join (
			SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TariffSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='제세금' group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff.[REQUESTFORMCALCULATE_HEAD_PK]

left join (
		SELECT R.[RequestFormPk], COUNT(*) AS C
		  FROM [dbo].[RequestForm] AS R 
			left join [dbo].[RequestFormItems] AS RI ON R.RequestFormPk=RI.RequestFormPk 
		 WHERE RI.ItemCode is null  
		 GROUP BY R.RequestFormPk 
	
	) AS CheckIsCodeNull ON CheckIsCodeNull.RequestFormPk=RF.RequestFormPk
left join (
		SELECT R.[RequestFormPk], COUNT(*) AS C
		  FROM [dbo].[RequestForm] AS R 
			left join [dbo].[RequestFormItems] AS RI ON R.RequestFormPk=RI.RequestFormPk 
			left join [dbo].ClearanceItemCodeKOR AS CICK ON CICK.ItemCode=RI.ItemCode
		 WHERE CICK.Law_Nm is not null  
		 GROUP BY R.RequestFormPk 
	
	) AS CheckLaw ON CheckLaw.RequestFormPk=RF.RequestFormPk
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='3' group by [TABLE_NAME], [TABLE_PK]) AS RHis3 on RF.RequestFormPk = RHis3.[RequestFormPk]
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='4' group by [TABLE_NAME], [TABLE_PK]) AS RHis4 on RF.RequestFormPk = RHis4.[RequestFormPk]
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='5' group by [TABLE_NAME], [TABLE_PK]) AS RHis5 on RF.RequestFormPk = RHis5.[RequestFormPk]
	WHERE Arrival.OurBranchCode=" + OurBranchPk + @" 
	AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
	AND RF.StepCL<64 and RF.DocumentStepCL in (1, 2, 5, 6)
	Order by CD.ContainerNo DESC, CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC;";
		} else if (Gubun == "O" && Date == "A") {
			DB.SqlCmd.CommandText = @"SELECT	
	RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
	RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentStepCL,RF.DocumentRequestCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.Shipper, CD.Consignee, CD.ContainerNo
	, SC.CompanyName AS ShipperCompanyName
	, CC.CompanyName AS ConsigneeCompanyName
	, Departure.NameE
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
	, Tariff.TariffSum 
,CheckIsCodeNull.C 
,CheckLaw.C CheckLaw
,RHis3.C RHis3C
,RHis4.C RHis4C
,RHis5.C RHis5C
	FROM RequestForm AS RF 
		Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RFCH on RF.RequestFormPk=RFCH.[TABLE_PK]
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk 
		left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
		Left join Company AS SC on RF.ShipperPk=SC.CompanyPk 
		left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
		Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
		Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
		left join (
			SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TariffSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='제세금' group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff.[REQUESTFORMCALCULATE_HEAD_PK]
left join (
		SELECT R.[RequestFormPk], COUNT(*) AS C
		  FROM [dbo].[RequestForm] AS R 
			left join [dbo].[RequestFormItems] AS RI ON R.RequestFormPk=RI.RequestFormPk 
		 WHERE RI.ItemCode is null  
		 GROUP BY R.RequestFormPk 
	
	) AS CheckIsCodeNull ON CheckIsCodeNull.RequestFormPk=RF.RequestFormPk
left join (
		SELECT R.[RequestFormPk], COUNT(*) AS C
		  FROM [dbo].[RequestForm] AS R 
			left join [dbo].[RequestFormItems] AS RI ON R.RequestFormPk=RI.RequestFormPk 
			left join [dbo].ClearanceItemCodeKOR AS CICK ON CICK.ItemCode=RI.ItemCode
		 WHERE CICK.Law_Nm is not null  
		 GROUP BY R.RequestFormPk 
	
	) AS CheckLaw ON CheckLaw.RequestFormPk=RF.RequestFormPk
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='3' group by [TABLE_NAME], [TABLE_PK]) AS RHis3 on RF.RequestFormPk = RHis3.[RequestFormPk]
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='4' group by [TABLE_NAME], [TABLE_PK]) AS RHis4 on RF.RequestFormPk = RHis4.[RequestFormPk]
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='5' group by [TABLE_NAME], [TABLE_PK]) AS RHis5 on RF.RequestFormPk = RHis5.[RequestFormPk]
	WHERE Arrival.OurBranchCode=" + OurBranchPk + @" 
	AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
	AND RF.StepCL<64 and RF.DocumentStepCL<2 
	Order by CD.ContainerNo DESC, CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC;";
		} else if (Gubun == "O" && Date != "A") {
			DB.SqlCmd.CommandText = @"SELECT	
	RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
	RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentStepCL,RF.DocumentRequestCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.Shipper, CD.Consignee, CD.ContainerNo
	, SC.CompanyName AS ShipperCompanyName
	, CC.CompanyName AS ConsigneeCompanyName
	, Departure.NameE
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
	, Tariff.TariffSum 
,CheckIsCodeNull.C 
,CheckLaw.C CheckLaw
,RHis3.C RHis3C
,RHis4.C RHis4C
,RHis5.C RHis5C
	FROM RequestForm AS RF 
		Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RFCH on RF.RequestFormPk=RFCH.[TABLE_PK]
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk 
		left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
		Left join Company AS SC on RF.ShipperPk=SC.CompanyPk 
		left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
		Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
		Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
		left join (
			SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TariffSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='제세금' group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff.[REQUESTFORMCALCULATE_HEAD_PK]
left join (
		SELECT R.[RequestFormPk], COUNT(*) AS C
		  FROM [dbo].[RequestForm] AS R 
			left join [dbo].[RequestFormItems] AS RI ON R.RequestFormPk=RI.RequestFormPk 
		 WHERE RI.ItemCode is null  
		 GROUP BY R.RequestFormPk 
	
	) AS CheckIsCodeNull ON CheckIsCodeNull.RequestFormPk=RF.RequestFormPk
left join (
		SELECT R.[RequestFormPk], COUNT(*) AS C
		  FROM [dbo].[RequestForm] AS R 
			left join [dbo].[RequestFormItems] AS RI ON R.RequestFormPk=RI.RequestFormPk 
			left join [dbo].ClearanceItemCodeKOR AS CICK ON CICK.ItemCode=RI.ItemCode
		 WHERE CICK.Law_Nm is not null  
		 GROUP BY R.RequestFormPk 
	
	) AS CheckLaw ON CheckLaw.RequestFormPk=RF.RequestFormPk
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='3' group by [TABLE_NAME], [TABLE_PK]) AS RHis3 on RF.RequestFormPk = RHis3.[RequestFormPk]
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='4' group by [TABLE_NAME], [TABLE_PK]) AS RHis4 on RF.RequestFormPk = RHis4.[RequestFormPk]
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='5' group by [TABLE_NAME], [TABLE_PK]) AS RHis5 on RF.RequestFormPk = RHis5.[RequestFormPk]
	WHERE Arrival.OurBranchCode=" + OurBranchPk + @" 
	AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
	AND RF.StepCL<64 and RF.DocumentStepCL<2 
	AND (CD.ClearanceDate='" + Date + @"' or (isnull(CD.ClearanceDate, '-1')='-1' and RF.ArrivalDate='" + Date + @"' ))
	Order by CD.ContainerNo DESC, CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC;";
		} else if (Gubun != "A") {
			DB.SqlCmd.CommandText = @"SELECT	
	RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
	RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentStepCL,RF.DocumentRequestCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.Shipper, CD.Consignee, CD.ContainerNo
	, SC.CompanyName AS ShipperCompanyName
	, CC.CompanyName AS ConsigneeCompanyName
	, Departure.NameE
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
	, Tariff.TariffSum 
	,CheckIsCodeNull.C 
,CheckLaw.C CheckLaw
,RHis3.C RHis3C
,RHis4.C RHis4C
,RHis5.C RHis5C
	FROM RequestForm AS RF 
		Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RFCH on RF.RequestFormPk=RFCH.[TABLE_PK]
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk 
		left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
		Left join Company AS SC on RF.ShipperPk=SC.CompanyPk 
		left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
		Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
		Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
		left join (
			SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) As TariffSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='제세금' group by [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff.[REQUESTFORMCALCULATE_HEAD_PK]
left join (
		SELECT R.[RequestFormPk], COUNT(*) AS C
		  FROM [dbo].[RequestForm] AS R 
			left join [dbo].[RequestFormItems] AS RI ON R.RequestFormPk=RI.RequestFormPk 
		 WHERE RI.ItemCode is null  
		 GROUP BY R.RequestFormPk 
	
	) AS CheckIsCodeNull ON CheckIsCodeNull.RequestFormPk=RF.RequestFormPk
left join (
		SELECT R.[RequestFormPk], COUNT(*) AS C
		  FROM [dbo].[RequestForm] AS R 
			left join [dbo].[RequestFormItems] AS RI ON R.RequestFormPk=RI.RequestFormPk 
			left join [dbo].ClearanceItemCodeKOR AS CICK ON CICK.ItemCode=RI.ItemCode
		 WHERE CICK.Law_Nm is not null  
		 GROUP BY R.RequestFormPk 
	
	) AS CheckLaw ON CheckLaw.RequestFormPk=RF.RequestFormPk
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='3' group by [TABLE_NAME], [TABLE_PK]) AS RHis3 on RF.RequestFormPk = RHis3.[RequestFormPk]
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='4' group by [TABLE_NAME], [TABLE_PK]) AS RHis4 on RF.RequestFormPk = RHis4.[RequestFormPk]
left join (select [TABLE_PK] RequestFormPk,count(*) C from [dbo].[HISTORY] where [CODE]='5' group by [TABLE_NAME], [TABLE_PK]) AS RHis5 on RF.RequestFormPk = RHis5.[RequestFormPk]
	WHERE Arrival.OurBranchCode=" + OurBranchPk + @" 
	AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
	AND RF.StepCL<64 and RF.DocumentStepCL in (1, 2, 5, 6) and (CD.ClearanceDate='" + Date + @"' or RF.ArrivalDate='" + Date + @"' ) 
	Order by CD.ContainerNo DESC, CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC;";
		}

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string TempCommercialDocumentHeadPk = "";
		while (RS.Read()) {
			if (TempCommercialDocumentHeadPk != RS["CommercialDocumentHeadPk"] + "") {
				string tempDocumentStep;
				switch (RS["DocumentStepCL"] + "") {
					case "1": tempDocumentStep = "미전송"; break;
					case "2": tempDocumentStep = "전송완"; break;
					case "3": tempDocumentStep = "자통"; break;
					case "4": tempDocumentStep = "샘플"; break;
					case "5": tempDocumentStep = "정산완료"; break;
					case "6": tempDocumentStep = "통관지시"; break;
					default: tempDocumentStep = "?"; break;
				}
				string IsFTA = "";
				string[] Documents = RS["DocumentRequestCL"].ToString().Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
				foreach (string T in Documents) {
					if (T == "34") {
						IsFTA = "True";
					}
				}
				TempCommercialDocumentHeadPk = RS["CommercialDocumentHeadPk"] + "";
				string Style = " ";
				string CheckLaw = " ";
				string GuanBugaseStyle = "";
				string jagaNsample = "";
				if (RS["C"] + "" != "") {
					Style = " background-color:skyblue; ";
				}
				if (RS["CheckLaw"] + "" != "") {
					CheckLaw = " background-color:#CECE77; ";
				}
				if (RS["RHis5C"] + "" != "") {
					GuanBugaseStyle = " background-color:#FFE08C; ";
				}
				if (RS["RHis3C"] + "" != "") {
					jagaNsample = "자통";
				}
				if (RS["RHis4C"] + "" != "") {
					jagaNsample = "샘플";
				}

				ReturnValue.Append(String.Format(BodyCommercial,
					tempDocumentStep,
					RS["CommercialDocumentHeadPk"] + "",
					RS["BLNo"] + "",
					RS["DepartureDate"] + "",
					RS["ArrivalDate"] + "",
					RS["NameE"] + "",
					Common.GetTransportWay(RS["TransportWayCL"] + ""),
					RS["ShipperPk"] + "",
					RS["ShipperCompanyName"] + "",
					RS["ShipperCode"] + "",
					RS["ConsigneePk"] + "",
					RS["ConsigneeCompanyName"] + "",
					RS["ConsigneeCode"] + "",
					RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + ""),
					Common.NumberFormat(RS["TotalGrossWeight"] + ""),
					Common.NumberFormat(RS["TotalVolume"] + ""),
					RS["RequestFormPk"] + "",
			   RS["ContainerNo"] + "", Style, CheckLaw, GuanBugaseStyle, jagaNsample,
			   IsFTA == "True" ? "<img src=\"../Images/CheckTrue.jpg\" style=\"width:20px;\" alt=\"\" />" : "&nbsp"));
			} else {
				ReturnValue.Append(String.Format(BodyRequest,
													RS["DepartureDate"] + "",
													RS["ArrivalDate"] + "",
													RS["NameE"] + "",
													Common.GetTransportWay(RS["TransportWayCL"] + ""),
													RS["ShipperPk"] + "",
													RS["ShipperCompanyName"] + "",
													RS["ShipperCode"] + "",
													RS["ConsigneePk"] + "",
													RS["ConsigneeCompanyName"] + "",
													RS["ConsigneeCode"] + "",
													RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + ""),
													Common.NumberFormat(RS["TotalGrossWeight"] + ""),
													Common.NumberFormat(RS["TotalVolume"] + ""),
													RS["RequestFormPk"] + ""));
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "</table>";
	}
	private String LoadList_SerchBL(string BLNo) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("	<table border='0' cellpadding='0' cellspacing='0' style='width:1050px;' ><thead><tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; width:60px;  \">STAFF</td>" +
										"		<td class='THead1' style='width:100px;' >ContainerNo</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:150px; \" >Master BL</td>" +
							  "		<td class='THead1' style='width:80px;' >Departure</td>" +
							  "		<td class='THead1' style='width:80px;' >Arrival</td>" +
										"		<td class='THead1' style='width:90px;'>StartArea</td>" +
										"		<td class='THead1' style='width:90px;'>Transport</td>" +
										"		<td class='THead1'>Customer</td>" +
										"		<td class='THead1' style='width:50px;'>count</td>" +
										"		<td class='THead1' style='width:45px;'>Kg</td>" +
										"		<td class='THead1' style='width:45px;'>CBM</td>" +
										"		<td class='THead1' style='width:45px;'>운임</td>" +
										"		<td class='THead1' style='width:45px;'>세금</td>" +
										"		<td class='THead1' style='width:20px;'>F</td>" +
										"		</tr></thead>");
		string BodyCommercial = "<tr style=\"height:20px;\">" +
											"		<td class='TBody1' style=\"text-align:center; color:green; \">{0}</td>" +
											"		<td class='TBody1' >{17}</td>" +
											"		<td class='TBody1' style=\"text-align:center; font-weight:bold;\" ><a onclick=\"Goto('CheckDescription','{1}');\">{2}</a></td>" +
											"		<td class='TBody1' >{3}</td>" +
											"		<td class='TBody1' >{4}</td>" +
											"		<td class='TBody1' >{5}</td>" +
											"		<td class='TBody1' ><a onclick=\"Goto('RequestForm', '{16}');\">{6}</a></td>" +
											"		<td class='TBody1' ><a onclick=\"Goto('Company', '{7}');\">{9}</a> ~ <a onclick=\"Goto('Company', '{10}');\">{12}</a></td>" +
											"		<td class='TBody1' >{13}</td>" +
											"		<td class='TBody1' >{14}</td>" +
											"		<td class='TBody1' >{15}</td>" +
											"		<td class='TBody1' >&nbsp;</td>" +
											"		<td class='TBody1' >&nbsp;</td>" +
											
											"		<td class='TBody1' >{18}</td>" +
											"		</tr>";
		string BodyRequest = "<tr style=\"height:20px;\">" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >└---------></td>" +
										"		<td class='TBody1' >&nbsp;</td>" +

										"		<td class='TBody1' >{0}</td>" +
										"		<td class='TBody1' >{1}</td>" +
										"		<td class='TBody1' >{2}</td>" +
										"		<td class='TBody1' ><a onclick=\"Goto('RequestForm', '{13}');\">{3}</a></td>" +
										"		<td class='TBody1'><a onclick=\"Goto('Company', '{4}');\">{6}</a> ~ <a onclick=\"Goto('Company', '{7}');\">{9}</a></td>" +
										"		<td class='TBody1' >{10}</td>" +
										"		<td class='TBody1' >{11}</td>" +
										"		<td class='TBody1' >{12}</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"		<td class='TBody1' >&nbsp;</td>" +
										"	</tr>";
		DB.SqlCmd.CommandText = @"		SELECT	 TOP 50
	RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, 
	RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentStepCL,RF.DocumentRequestCL
	, CD.CommercialDocumentHeadPk, CD.BLNo, CD.Shipper, CD.Consignee, CD.ContainerNo
	, SC.CompanyName AS ShipperCompanyName
	, CC.CompanyName AS ConsigneeCompanyName
	, Departure.NameE
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
	FROM RequestForm AS RF 
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk 
		left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
		Left join Company AS SC on RF.ShipperPk=SC.CompanyPk 
		left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
		Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
		Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  		
	 	 WHERE CD.BLNo like '%" + BLNo + @"%' 
	Order by CD.CommercialDocumentHeadPk ASC, RF.RequestFormPk ASC;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string TempCommercialDocumentHeadPk = "";
		while (RS.Read()) {
			if (TempCommercialDocumentHeadPk != RS["CommercialDocumentHeadPk"] + "") {
				string tempDocumentStep;
				switch (RS["DocumentStepCL"] + "") {
					case "1":
						tempDocumentStep = "미전송";
						break;
					case "2":
						tempDocumentStep = "전송완";
						break;
					case "3":
						tempDocumentStep = "자통";
						break;
					case "4":
						tempDocumentStep = "샘플";
						break;
					case "5":
						tempDocumentStep = "정산완료";
						break;
					case "6":
						tempDocumentStep = "통관지시";
						break;
					default:
						tempDocumentStep = "완료";
						break;
				}

				string IsFTA = "";
				string[] Documents = RS["DocumentRequestCL"].ToString().Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
				foreach (string T in Documents) {
					if (T == "34") {
						IsFTA = "True";
					}
				}
				TempCommercialDocumentHeadPk = RS["CommercialDocumentHeadPk"] + "";

				string tempBLNo = (RS["BLNo"] + "").Replace(BLNo, "<span style=\"color:red;\" >" + BLNo + "</span>");
				ReturnValue.Append(String.Format(BodyCommercial,
					tempDocumentStep,
					RS["CommercialDocumentHeadPk"] + "",
					tempBLNo,
					RS["DepartureDate"] + "",
					RS["ArrivalDate"] + "",
					RS["NameE"] + "",
					Common.GetTransportWay(RS["TransportWayCL"] + ""),
					RS["ShipperPk"] + "",
					RS["ShipperCompanyName"] + "",
					RS["ShipperCode"] + "",
					RS["ConsigneePk"] + "",
					RS["ConsigneeCompanyName"] + "",
					RS["ConsigneeCode"] + "",
					RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + ""),
					Common.NumberFormat(RS["TotalGrossWeight"] + ""),
					Common.NumberFormat(RS["TotalVolume"] + ""),
					RS["RequestFormPk"] + "",
					RS["ContainerNo"] + "",
					IsFTA == "True" ? "<img src=\"../Images/CheckTrue.jpg\" style=\"width:20px;\" alt=\"\" />" : "&nbsp"));
			} else {
				ReturnValue.Append(String.Format(BodyRequest,
													RS["DepartureDate"] + "",
													RS["ArrivalDate"] + "",
													RS["NameE"] + "",
													Common.GetTransportWay(RS["TransportWayCL"] + ""),
													RS["ShipperPk"] + "",
													RS["ShipperCompanyName"] + "",
													RS["ShipperCode"] + "",
													RS["ConsigneePk"] + "",
													RS["ConsigneeCompanyName"] + "",
													RS["ConsigneeCode"] + "",
													RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + ""),
													Common.NumberFormat(RS["TotalGrossWeight"] + ""),
													Common.NumberFormat(RS["TotalVolume"] + ""),
													RS["RequestFormPk"] + ""));
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "</table>";
	}
}