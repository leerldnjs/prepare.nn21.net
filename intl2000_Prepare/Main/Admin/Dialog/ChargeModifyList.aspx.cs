using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Dialog_ChargeModifyList : System.Web.UI.Page
{
	protected String Html_Table; 
    protected void Page_Load(object sender, EventArgs e)
    {
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
		Html_Table = MsgSendedListLoad(Request.Params["S"]);
    }
	private String MsgSendedListLoad(string RequestFomrPk) {
		DBConn DB = new DBConn();
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
 WHERE H.ModifyWhich='Charge' and RF.RequestFormPk=" + RequestFomrPk + " ORDER BY H.[Registerd] ASC;";
		String TableFormat = "<table border='0' cellpadding='0' cellspacing='0' style='width:850px;' ><thead><tr height='30px'>" +
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
		RS.Dispose();
		DB.DBCon.Close();
		return string.Format(TableFormat, ReturnValue + "", "&nbsp;");
	}

}