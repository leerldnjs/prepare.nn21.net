using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Request_Dialog_MsgSendedList : System.Web.UI.Page
{
	protected string SendedHtml;
    protected void Page_Load(object sender, EventArgs e)
    {
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
		SendedHtml = MsgSendedListLoad(Request.Params["S"]);

    }
//	private String MsgSendedListLoad(string RequestFomrPk)
//	{
//		DBConn DB = new DBConn();
//		DB.SqlCmd.CommandText = @"
//SELECT M.GubunCL, M.GubunPk, M.SenderID, M.Receiver, M.ReceiverAddress, M.Title,  M.SendedTime, M.ReceiveTime, C.CompanyName ,MM.Contents
//FROM MsgSendedHistory AS M 
//left join Company AS C ON M.ToPk=C.CompanyPk 
//left join (select MsgSendHistoryPk,Contents from MsgSendedHistory where GubunCL in (0,1,2,3,6,7)) as MM on M.MsgSendHistoryPk=MM.MsgSendHistoryPk
//WHERE GubunPk=" + RequestFomrPk + ";";
//		DB.DBCon.Open();
//		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
//		StringBuilder ReturnValue = new StringBuilder();
//		while (RS.Read())
//		{
//			ReturnValue.Append(string.Format("<tr><td class='TBody1' style=\"height:20px; \">{0}</td><td class='TBody1'>{1}</td><td class='TBody1'>{2}</td><td class='TBody1'>{3}</td><td class='TBody1'>{4}</td><td class='TBody1'>{5}</td></tr>",
//				RS["SenderID"] + "",
//				GetSMSOrEmail(RS["GubunCL"] + ""),
//				RS["CompanyName"] + "",
//				RS["ReceiverAddress"] + "",
//				RS["SendedTime"] + "" == "" ? "&nbsp;" : (RS["SendedTime"] + "").Substring(2, (RS["SendedTime"] + "").Length - 5),
//				RS["ReceiveTime"] + "" == "" ? "&nbsp;" : (RS["ReceiveTime"] + "").Substring(2, (RS["ReceiveTime"] + "").Length - 5)));
//		}
//		return "<table border='0' cellpadding='0' cellspacing='0' style='width:750px;' ><thead><tr height='30px'>" +
//								"<td class='THead1' style='width:60px;' >Sender</td>" +
//								"<td class='THead1' style='width:80px;' >Type</td>" +
//								"<td class='THead1' >Company Name</td>" +
//								"<td class='THead1' style='width:130px;'>Receiver</td>" +
//								"<td class='THead1' style='width:130px;'>Send Time</td>" +
//								"<td class='THead1' style='width:130px;'>Read Time</td>" +
//							"</tr></thead>" + ReturnValue + "</table>";
//	}
	private String MsgSendedListLoad(string RequestFomrPk)
	{
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT M.GubunCL, M.GubunPk, M.SenderID, M.Receiver, M.ReceiverAddress, M.Title,  M.SendedTime, M.ReceiveTime, C.CompanyName ,MM.Contents
FROM MsgSendedHistory AS M 
left join Company AS C ON M.ToPk=C.CompanyPk 
left join (select MsgSendHistoryPk,Contents from MsgSendedHistory where GubunCL in (0,1,2,3,6,7)) as MM on M.MsgSendHistoryPk=MM.MsgSendHistoryPk
WHERE GubunPk=" + RequestFomrPk + " order by SendedTime desc;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder ReturnValue = new StringBuilder();
		while (RS.Read()) {
			ReturnValue.Append(string.Format("<tr><td class='TBody1' style=\"height:20px; \">{0}</td><td class='TBody1'>{1}</td><td class='TBody1'>{2}</td><td class='TBody1'>{3}</td><td class='TBody1'>{4}</td><td class='TBody1'>{5}</td></tr>",
				RS["SenderID"] + "",
				GetSMSOrEmail(RS["GubunCL"] + ""),
				RS["CompanyName"] + "",
				RS["ReceiverAddress"] + "",
				RS["SendedTime"] + "" == "" ? "&nbsp;" : (RS["SendedTime"] + "").Substring(2, (RS["SendedTime"] + "").Length - 5),
				RS["Contents"] + "" ));
		}
		return "<table border='0' cellpadding='0' cellspacing='0' style='width:850px;' ><thead><tr height='30px'>" +
								"<td class='THead1' style='width:40px;' >Sender</td>" +
								"<td class='THead1' style='width:60px;' >Type</td>" +
								"<td class='THead1' style='width:120px;' >Company Name</td>" +
								"<td class='THead1' style='width:100px;'>Receiver</td>" +
								"<td class='THead1' style='width:140px;'>Send Time</td>" +
								"<td class='THead1' >Contents</td>" +
							"</tr></thead>" + ReturnValue + "</table>";
	}
	private String GetSMSOrEmail(string GubunCL)
	{
		switch (GubunCL)
		{
			case "0": return "<span style='color:blue;'>SMS OK</span>";
			case "1": return "<span style='color:red;'>SMS Fail</span>";
			case "2": return "<span style='color:blue;'>SMS OK</span>";
			case "3": return "<span style='color:red;'>SMS Fail</span>";
			case "4": return "<span style='color:green;'>Email</span>";
			case "5": return "<span style='color:green;'>Email</span>";
			case "6": return "<span style='color:orange;'>SMS OK</span>";
			case "7": return "<span style='color:orange;'>SMS Fail</span>";
			default: return "";
		}
	}
}