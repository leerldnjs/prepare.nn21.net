using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transport_TransportList :System.Web.UI.Page
{
	protected static string Html_TransportList;
	protected string Html_SelectStorage;
	protected static string BranchPk;
	protected static string AccountID;
	protected static string Gubun;
	protected static int PageLenth = 10;
	protected static int TotalPage;
	protected String[] MEMBERINFO;

	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("/Default.aspx");
		}
		else {
			MEMBERINFO = (Session["MemberInfo"] + "").Split(Common.Splite11, StringSplitOptions.None);
		}
		BranchPk = MEMBERINFO[1];
		AccountID = MEMBERINFO[2];
		Gubun = Request.Params["G"];
		Html_TransportList = MakeHtml_TransportList("1");
		Html_SelectStorage = MakeHtml_SelectStorage(BranchPk);
	}

	[WebMethod]
	public static string MakeHtml_TransportList(string PageNo) {
		TransportC TransC = new TransportC();
		List<sTransportHead> Head = new List<sTransportHead>();
		StringBuilder ReturnValue = new StringBuilder();
		string QueryWhere = " ";
		if (Gubun == "in") {
			QueryWhere = "AND HEAD.[BRANCHPK_TO] = " + BranchPk;
		}
		else {
			QueryWhere = "AND HEAD.[BRANCHPK_FROM] = " + BranchPk;
		}

		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"SELECT
			HEAD.[TRANSPORT_PK] AS TRANSPORT_PK
			,HEAD.[DATETIME_TO]AS DATETIME_TO
			,HEAD.[TRANSPORT_WAY] AS TRANSPORT_WAY
			,HEAD.[BRANCHPK_FROM] AS BRANCHPK_FROM
			,HEAD.[BRANCHPK_TO] AS BRANCHPK_TO
			,Company.[CompanyName] AS BRANCHNAME_FROM
			,HEAD.[VESSELNAME] AS VESSELNAME
			,HEAD.[DATETIME_FROM] AS DATETIME_FROM
			,HEAD.[AREA_FROM] AS AREA_FROM
			,HEAD.[AREA_TO] AS AREA_TO
			,HEAD.[TRANSPORT_STATUS] AS TRANSPORT_STATUS
			,HEAD.[WAREHOUSE_PK_ARRIVAL] 
			,ISNULL(SCODE.[StorageName], '창고미지정') AS StorageName
		FROM [dbo].[TRANSPORT_PACKED] AS PACKED
		INNER JOIN [dbo].[TRANSPORT_HEAD] AS HEAD ON PACKED.[TRANSPORT_HEAD_PK] = HEAD.[TRANSPORT_PK] 
		LEFT JOIN [dbo].[Company] AS COMPANY ON HEAD.[BRANCHPK_FROM] = Company.[CompanyPk]
		LEFT JOIN [dbo].[OurBranchStorageCode] AS SCODE ON HEAD.[WAREHOUSE_PK_ARRIVAL] = SCODE.OurBranchStoragePk
		WHERE ISNULL(HEAD.[TRANSPORT_STATUS], 1) BETWEEN 1 AND 2" + QueryWhere + @" 	
		ORDER BY DATETIME_TO DESC, TRANSPORT_WAY ;";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int Count = RS.Cast<object>().Count();
		RS.Close();
		RS = DB.SqlCmd.ExecuteReader();
		string Current = "";
		for (int i = 0; i < (Int32.Parse(PageNo) - 1) * PageLenth; i++) {
			if (RS.Read()) {
				continue;
			}
		}
		for (int i = 0; i < PageLenth; i++) {
			if (RS.Read()) {
				if (Current != RS["DATETIME_TO"].ToString().Substring(0, 8)) {
					ReturnValue.Append("<tr class=\"text-xs\" style=\"background-color:whitesmoke;\"><td></td><td></td><td></td>");
					ReturnValue.Append("<td><a href=\"#\" onclick=\"GoAbjiDown('" + RS["DATETIME_TO"].ToString().Substring(0, 8) + "', '" + RS["BRANCHPK_TO"] + "', '" + Gubun + "')\">" + RS["DATETIME_TO"].ToString().Substring(0, 8) + "&nbsp<i class=\"i i-download2 icon\"></i><span class=\"font-bold\"> Excel</span><a></td>");
					ReturnValue.Append("<td><a href=\"#\" onclick =\"GoAbjiDown2('" + RS["DATETIME_TO"].ToString().Substring(0, 8) + "', '" + RS["BRANCHPK_TO"] + "', '" + Gubun + "', 'ALL')\">月 재무자료&nbsp<i class=\"i i-download2 icon\"></i><span class=\"font-bold\"> Excel</span><a></td>");
					ReturnValue.Append("<td><a href=\"#\" onclick=\"GoAbjiDown2('" + RS["DATETIME_TO"].ToString().Substring(0, 8) + "', '" + RS["BRANCHPK_TO"] + "', '" + Gubun + "', 'ALL')\">日 재무자료&nbsp<i class=\"i i-download2 icon\"></i><span class=\"font-bold\"> Excel</span><a></td></tr>");
				}
				ReturnValue.Append("<tr class=\"text-sm\">");
				ReturnValue.Append("<td>" + RS["TRANSPORT_WAY"] + "" + "_From_" + RS["BRANCHNAME_FROM"] + "" + "</td>");
				ReturnValue.Append("<td>" + RS["VESSELNAME"] + "" + "</td>");
				ReturnValue.Append("<td onclick=\"GoView('" + RS["TRANSPORT_PK"] + "'" + ")\">" + (RS["DATETIME_FROM"].ToString().Substring(0, 8)) + " - " + (RS["DATETIME_TO"].ToString().Substring(0, 8)) + "</td>");
				ReturnValue.Append("<td>" + RS["AREA_FROM"] + "" + " - " + RS["AREA_TO"] + "" + "</td>");
				ReturnValue.Append("<td>" + RS["StorageName"] + "<br />" + Common.GetBetweenBranchTransportStepCL(RS["TRANSPORT_STATUS"] + "") + "</td>");
				ReturnValue.Append("<td><a href=\"#\" onclick=\"GoExcelDown('" + RS["TRANSPORT_PK"] + "')\"><i class=\"i i-download2 icon\"></i><span class=\"font-bold\"> Excel</span><a></td></tr>");
				
				Current = RS["DATETIME_TO"].ToString().Substring(0, 8);
			}
		}

		TotalPage = (Count / PageLenth) + 1;
		RS.Close();
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	private string MakeHtml_SelectStorage(string BranchPk) {
		StringBuilder ReturnValue = new StringBuilder();

		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"SELECT 
			OBSC.OurBranchStoragePk AS VALUE, OBSC.StorageName AS DATA
		FROM [dbo].[TRANSPORT_HEAD] AS HEAD
		left join ( SELECT SUM([StorageCode])/COUNT(StorageCode) AS StorageCode,[TransportBetweenBranchPk] FROM OurBranchStorageOut Group by [TransportBetweenBranchPk] ) AS Storage ON		Storage.TransportBetweenBranchPk=HEAD.BRANCHPK_FROM 
		left join OurBranchStorageCode AS OBSC ON OBSC.OurBranchStoragePk=Storage.StorageCode 
		WHERE HEAD.BRANCHPK_TO=" + BranchPk + @" AND ISNULL(HEAD.[TRANSPORT_STATUS], 1)<>0 AND OBSC.StorageName IS NOT NULL
		GROUP BY OBSC.OurBranchStoragePk, OBSC.StorageName 
		ORDER BY COUNT(OBSC.StorageName) DESC;";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		ReturnValue.Append("<option value=\"ALL\">ALL</option>");
		while (RS.Read()) {
			ReturnValue.Append("<option value='" + RS["VALUE"] + "'>" + RS["DATA"] + "</option>");
		}

		RS.Close();
		DB.DBCon.Close();
		return ReturnValue + "";
	}
}