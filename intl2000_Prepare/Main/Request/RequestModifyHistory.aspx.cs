using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Request_RequestModifyHistory : System.Web.UI.Page
{
	protected String[] MemberInformation;
	protected String HTMLModifyHistory;
	protected String HTMLTab;
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

		MemberInformation = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		HTMLModifyHistory = MemberInformation[0] == "OurBranch" ? LoadRequestModifyHistory(Request.Params["G"] + "") : "";
		if (Request.Params["G"] + "" == "Confirmed") {
			HTMLTab = "<a href=\"RequestModifyHistory.aspx\">미승인</a>  ||  <a href=\"RequestModifyHistory.aspx?G=Confirmed\"><strong>승인</strong></a>";
		} else {
			HTMLTab = "<a href=\"RequestModifyHistory.aspx\"><strong>미승인</strong></a>  ||  <a href=\"RequestModifyHistory.aspx?G=Confirmed\">승인</a>";
		}

		HTMLTab = "<p><a href=\"/Admin/RequestList.aspx?G=Arrival\">출발지 입고완료</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"/Admin/CheckDescriptionList.aspx\">BL List</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"/Request/RequestModifyHistory.aspx\"><strong>Modify History</strong></a></p>";
		
	}
	private String LoadRequestModifyHistory(string Type)
	{
		DBConn DB = new DBConn();
		if (Type == "Confirmed") {
			DB.SqlCmd.CommandText = @"
SELECT H.[Pk], H.[RequestFormPk], H.[Value], H.[AccountID], H.[Registerd], H.[Confirmed]
	, RF.[ShipperCode], RF.[ConsigneeCode], RF.[DepartureDate], RF.[ArrivalDate], TWCD.[Initial], DRC.Name AS DRCN, ARC.Name AS ARCN 
	, C.CompanyName
  FROM [RequestModifyHistory] AS H
	left join [RequestForm] AS RF ON H.RequestFormPk=RF.RequestFormPk
	left join [Account_] AS A ON A.AccountID=H.AccountID
	left join [Company] AS C ON C.CompanyPk=A.CompanyPk
	left join [TransportWayCLDescription] AS TWCD ON RF.[TransportWayCL]=TWCD.[TransportWayCL]
	left join RegionCode AS DRC ON DRC.RegionCode=RF.[DepartureRegionCode]
	left join RegionCode AS ARC ON ARC.RegionCode=RF.[ArrivalRegionCode]
 WHERE isnull(Confirmed, '')<>''
 ORDER BY H.[Registerd] DESC;";
		} else {
			DB.SqlCmd.CommandText = @"
/*
SELECT TOP 100 H.[Pk], H.[RequestFormPk], H.[Value], H.[AccountID], H.[Registerd], H.[Confirmed]
	, RF.[ShipperCode], RF.[ConsigneeCode], RF.[DepartureDate], RF.[ArrivalDate], TWCD.[Initial], DRC.Name AS DRCN, ARC.Name AS ARCN 
	, C.CompanyName
  FROM [RequestModifyHistory] AS H
	left join [RequestForm] AS RF ON H.RequestFormPk=RF.RequestFormPk
	left join [Account_] AS A ON A.AccountID=H.AccountID
	left join [Company] AS C ON C.CompanyPk=A.CompanyPk
	left join [TransportWayCLDescription] AS TWCD ON RF.[TransportWayCL]=TWCD.[TransportWayCL]
	left join RegionCode AS DRC ON DRC.RegionCode=RF.[DepartureRegionCode]
	left join RegionCode AS ARC ON ARC.RegionCode=RF.[ArrivalRegionCode]
 WHERE isnull(Confirmed, '')=''
 ORDER BY Confirmed DESC*/

SELECT H.[Pk], H.[RequestFormPk], H.[Value], H.[AccountID], H.[Registerd], H.[Confirmed]
	, RF.[ShipperCode], RF.[ConsigneeCode], RF.[DepartureDate], RF.[ArrivalDate], TWCD.[Initial], DRC.Name AS DRCN, ARC.Name AS ARCN 
	, C.CompanyName
  FROM [RequestModifyHistory] AS H
	left join [RequestForm] AS RF ON H.RequestFormPk=RF.RequestFormPk
	left join [Account_] AS A ON A.AccountID=H.AccountID
	left join [Company] AS C ON C.CompanyPk=A.CompanyPk
	left join [TransportWayCLDescription] AS TWCD ON RF.[TransportWayCL]=TWCD.[TransportWayCL]
	left join RegionCode AS DRC ON DRC.RegionCode=RF.[DepartureRegionCode]
	left join RegionCode AS ARC ON ARC.RegionCode=RF.[ArrivalRegionCode]
 WHERE H.ModifyWhich='Charge'
 ORDER BY H.[Registerd] DESC; ";
		}
		String TableFormat = "<table border='0' cellpadding='0' cellspacing='0' style='width:1050px;' ><thead><tr height='30px'>" +
						"<td class='THead1' style='width:110px;' >CompanyCode</td>" +
						"<td class='THead1' style='width:20px;' >&nbsp;</td>" +
						"<td class='THead1' style='width:220px;' >Schedule</td>" +
						"<td class='THead1' style='width:45px;'>Company</td>" +
						"<td class='THead1' >Value</td>" +
						"<td class='THead1' style='width:150px;'>BTN</td>" +
					"</tr></thead>{0}<TR><td colspan='12' style='background-color:#F5F5F5; text-align:center; padding:20px; '>{1}</TD></TR></Table>";
		String TableRowFormat = "	<tr>" +
						"<td class='{0}'><a href=\"../Admin/RequestView.aspx?g=s&pk={2} \">{7}</a></td>" +
						"<td class='{0}'>{9}</td>" +
						"<td class='{0}'><a href=\"../Admin/RequestView.aspx?g=s&pk={2} \">{8}</a></td>" +
						"<td class='{0}'>{4}</td>" +
						"<td class='{0}' {11}>{3}</td>" +
						"<td class='{0}' >{10}</td></tr>";
		StringBuilder ReturnValue = new StringBuilder();
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string BTN = "";

         if (MemberInformation[2] == "ilic00" || MemberInformation[2] == "ilic01" || MemberInformation[2] == "ilic06" || MemberInformation[2] == "ilic07" || MemberInformation[2] == "ilic08"  && Type != "Confirmed")
         {
				BTN = "<input type=\"button\" value=\"OK\" id=\"BTN[" + RS["Pk"] + "]\" onclick=\"ModifyHistoryConfirm('" + RS["Pk"] + "');\" />";
			}

			ReturnValue.Append(string.Format(TableRowFormat,
				RS["Value"] + "" == "41" ? "TBody1G" : "TBody1",		//0
				RS["Pk"] + "",		//1
				RS["RequestFormPk"] + "",		//2
				RS["Value"] + "" == "41" ? "승인" : RS["Value"].ToString().Substring(1).Replace("*", "<br />"),		//3
				RS["AccountID"] + "",		//4
				RS["Registerd"] + "",
				RS["Confirmed"] + "",		//6
				RS["ShipperCode"] + " ~ " + RS["ConsigneeCode"],
				RS["DepartureDate"] + "[" + RS["DRCN"] + "] ~ " + RS["ArrivalDate"] + "[" + RS["ARCN"] + "]",		//8
				RS["Initial"] + "",
				RS["Registerd"],		//10
				RS["Value"] + "" == "41" ? "" : " style=\"text-align:left; padding-left:10px; \" "
				));
		}


		return string.Format(TableFormat, ReturnValue + "", "&nbsp;");
	}
}