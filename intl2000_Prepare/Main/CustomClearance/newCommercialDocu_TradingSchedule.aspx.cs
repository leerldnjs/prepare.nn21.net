using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Components;
using System.Data.SqlClient;

public partial class CustomClearance_newCommercialDocu_TradingSchedule : System.Web.UI.Page
{
	
	protected String[] MemberInfo;
	protected String gubun;
	protected String Item;
	private DBConn DB;
	protected String TODAY;
	protected String Date, CompanyNo, Name, CompanyName, PresidentName, CompanyAddress, upjong, uptae, TotalAmount1, TotalQuantity, TotalPrice, TotalAmount, TotalTax, MisuAmout ;


    protected void Page_Load(object sender, EventArgs e)
    {
		try { MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None); }
		catch (Exception) { Response.Redirect("../Default.aspx"); }
		//자기문서아니면 안보이게 만들기!!!!!!
		TODAY = DateTime.Now.Date.ToShortDateString().Replace("-", "");
		gubun = Request.Params["G"] + "" == "" ? "View" : Request.Params["G"] + "";
		if (Request.Params["S"] + "" != "")
		{
			Item = ItemLoad(Request.Params["S"]);
			TradingScheduleLoad(Request.Params["S"]);
		}
		
    }
	private String ItemLoad(string TradingScheduleHeadPk)
	{
		DB = new DBConn();
		DB.DBCon.Open();
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:694px; \"><tr>" +
										"<td style=\"height:25px; width:70px; text-align:center; border-left:solid 1px black; border-right:solid 1px black; border-bottom:solid 1px black;\" >날짜</td>" +
										"<td style=\"width:187px; text-align:center; border-right:solid 1px black; border-bottom:solid 1px black;\" >품목</td>" +
										"<td style=\"width:70px; text-align:center; border-right:solid 1px black; border-bottom:solid 1px black;\" >규격</td>" +
										"<td style=\"width:70px; text-align:center; border-right:solid 1px black; border-bottom:solid 1px black;\" >수량</td>" +
										"<td style=\"width:70px; text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >단가</td>" +
										"<td style=\"width:160px; text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >공급가액</td>" +
										"<td style=\" text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >세액</td></tr>");
		DB.SqlCmd.CommandText = "EXEC	SP_SelectItemWithTradingSchedulePk @TradingScheduleHeadPk = " + TradingScheduleHeadPk + ";";
		string EachRow = "<tr><td style=\"height:22px; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black; padding:2px; \" >{0}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{1}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{2}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{3}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{4}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{5}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{6}</td></tr>";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			string[] RowData = new string[]{
				((RS["date"] + "") == "" ? "&nbsp;" : RS["date"] + ""),
				((RS["description"] + "") == "" ? "&nbsp;" : RS["description"] + ""), 
				((RS["volume"] + "") == "" ? "&nbsp;" : RS["volume"] + ""),
				((RS["quantity"] + "") == "" ? "&nbsp;" : RS["quantity"] + ""),
				((RS["price"] + "") == "" ? "&nbsp;" : RS["price"] + ""),
				((RS["amount"] + "") == "" ? "&nbsp;" : RS["amount"] + ""),
				((RS["tax"] + "") == "" ? "&nbsp;" : RS["tax"] + "")
				//((RS["Volume"] + "") == "" ? "&nbsp;" : Common.NumberFormat(RS["Volume"] + "") + "")
			};

			ReturnValue.Append(String.Format(EachRow, RowData));
		}

		RS.Dispose();
		DB.DBCon.Close();
		ReturnValue.Append("</table>");
		return ReturnValue + "";
	}
	private void TradingScheduleLoad(string TradingScheduleHeadPk)
	{
		DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = "SELECT [Date],[Name],[CompanyNo],[CompanyName],[PresidentName],[CompanyAddress],[upjong],[uptae]" +
		                                              ",[TotalQuantity],[TotalPrice],[TotalAmount] ,[TotalTax],[TTotalAmount],[MisuAmout]" +
													  "FROM [newCommercialDocument]"+
                                                      "where  [CommercialDocumentHeadPk]= " + TradingScheduleHeadPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			Date = RS["Date"] + "";
			CompanyNo = RS["CompanyNo"] + "";
			Name = RS["Name"] + "";
			CompanyName = RS["CompanyName"] + "";
			PresidentName = RS["PresidentName"] + "";
			CompanyAddress = RS["CompanyAddress"] + "";
			upjong = RS["upjong"] + "";
			uptae = RS["uptae"] + "";
			TotalAmount1 = RS["TTotalAmount"] + "";
			TotalQuantity = RS["TotalQuantity"] + "";
			TotalPrice = RS["TotalPrice"] + "";
			TotalAmount = RS["TotalAmount"] + "";
			TotalTax = RS["TotalTax"] + "";
			MisuAmout = RS["MisuAmout"] + "";
		}

		RS.Dispose();
		DB.DBCon.Close();
	}
}