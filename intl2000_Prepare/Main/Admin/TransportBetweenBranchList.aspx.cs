using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_TransportBetweenBranchList : System.Web.UI.Page
{
	protected String[] MemberInfo;
	protected String InnerHtml;
	protected String FromRegion;
	protected String SelectedValue;
	private int PageNo;
	private string FromWhere;
	private DBConn DB;
	private int pagelength = 20;
	protected void Page_Load(object sender, EventArgs e) {
		//Response.Write(Session["MemberInfo"] + "");
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

		FromWhere = (Request["C"] + "").Trim();
		SelectedValue = (Request["S"] + "").Trim().ToString();
		if (SelectedValue == "") {
			SelectedValue = "ALL";
		}
		FromRegion = LoadGubunCountry(MemberInfo[1], Request.Params["G"], ref FromWhere);

		if (FromWhere == "") {
			InnerHtml = "데이터가 없습니다. ";
		} else {
			InnerHtml = LoadTransportBetweenBranchListLoad(MemberInfo[1], Request.Params["G"], PageNo, SelectedValue);
		}

		if (MemberInfo[0] == "Customs") {
			LogedWithoutRecentRequest1.Visible = false;
			Loged1.Visible = true;
		}
	}
	private String LoadGubunCountry(string BranchPk, string Gubun, ref string FromWhere) {
		DB = new DBConn();
		FromRegion = "";
		string inputstring;
		if (Gubun.Length >= 2 && Gubun.Substring(0, 2) == "in") {
			inputstring = "From ";
			DB.SqlCmd.CommandText = @"	
Select R.RegionCode, R.Name, R.NameE 
From RegionCode AS R 
	inner join (
		SELECT left(C.Regioncode,1) AS FromWhere 
		FROM [dbo].[TRANSPORT_HEAD] AS TBH 
		left join Company AS C ON TBH.[BRANCHPK_FROM]=CompanyPk 
		WHERE TBH.[BRANCHPK_TO]=" + BranchPk + @" 
		group by left(Regioncode,1)
	) 
AS C on R.RegionCode=C.FromWhere;";
		} else {
			inputstring = "To ";
			DB.SqlCmd.CommandText = "		Select R.RegionCode, R.Name, R.NameE " +
															"	From RegionCode AS R " +
															"		inner join " +
															"			(SELECT left(C.Regioncode,1) AS FromWhere FROM [dbo].[TRANSPORT_HEAD] AS TBH " +
															"			left join Company AS C ON TBH.[BRANCHPK_TO]=CompanyPk " +
															"			WHERE TBH.[BRANCHPK_FROM]=" + BranchPk +
															"			group by left(Regioncode,1)) " +
															"		AS C on R.RegionCode=C.FromWhere;";
		}
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (FromWhere.Trim() == "") {
				FromWhere = RS[0] + "";
			}
			string style = FromWhere == RS[0] + "" ? "style=\"font-weight:bold;\"" : "";
			FromRegion += "<input type=\"button\" " + style + " value=\"" + inputstring + RS["NameE"] + "\" onclick=\"gotoDifferantGubun('" + Gubun + "','" + RS[0] + "');\" />&nbsp;&nbsp;";
		}
		RS.Dispose();
		DB.DBCon.Close();

		if (BranchPk == "3157" && Gubun.Length >= 2 && Gubun.Substring(0, 2) == "in") {
			string style = Gubun == "inYW" ? "style=\"font-weight:bold;\"" : "";
			FromRegion += "<input type=\"button\" " + style + " value=\"" + "From YW" + "\" onclick=\"gotoDifferantGubun('inYW','2888');\" />&nbsp;&nbsp;";
		}
		return FromRegion;
	}


	private String LoadTransportBetweenBranchListLoad(string BranchPk, string Gubun, int PageNo, string SelectedValue) {
		DB = new DBConn();
		string QueryWhere = "";
		if (SelectedValue == "ALL") {

		} else {
			QueryWhere = " and OBSC.OurBranchStoragePk=" + SelectedValue;
		}
		if (Gubun == "in") {
			if (FromWhere != "") {
				DB.SqlCmd.CommandText = @"	
SELECT count(*) 
FROM [dbo].[TRANSPORT_HEAD] AS TBH 
	left join Company AS C ON TBH.[BRANCHPK_FROM]=CompanyPk 
left join (
			SELECT SUM([StorageCode])/COUNT(StorageCode) AS StorageCode,[TransportBetweenBranchPk]
			FROM OurBranchStorageOut
			Group by [TransportBetweenBranchPk]
			) AS Storage ON Storage.TransportBetweenBranchPk=TBH.[TRANSPORT_PK] 
		left join OurBranchStorageCode AS OBSC ON OBSC.OurBranchStoragePk=Storage.StorageCode 
WHERE TBH.[BRANCHPK_TO]=" + BranchPk + @" and left(Regioncode,1)=" + FromWhere + QueryWhere + ";";
			} else {
				DB.SqlCmd.CommandText = @"	SELECT count(*) 
															  	FROM [dbo].[TRANSPORT_HEAD] AS TBH 
															  		left join Company AS C ON TBH.[BRANCHPK_FROM]=CompanyPk 
															  left join (
			SELECT SUM([StorageCode])/COUNT(StorageCode) AS StorageCode,[TransportBetweenBranchPk]
			FROM OurBranchStorageOut
			Group by [TransportBetweenBranchPk]
			) AS Storage ON Storage.TransportBetweenBranchPk=TBH.[TRANSPORT_PK] 
		left join OurBranchStorageCode AS OBSC ON OBSC.OurBranchStoragePk=Storage.StorageCode 
															  	WHERE TBH.[BRANCHPK_TO]=" + BranchPk + QueryWhere + ";";

			}
		} else {
			DB.SqlCmd.CommandText = @"	SELECT count(*) 
															FROM [dbo].[TRANSPORT_HEAD] AS TBH 
																left join Company AS C ON TBH.[BRANCHPK_TO]=CompanyPk 
														left join (
			SELECT SUM([StorageCode])/COUNT(StorageCode) AS StorageCode,[TransportBetweenBranchPk]
			FROM OurBranchStorageOut
			Group by [TransportBetweenBranchPk]
			) AS Storage ON Storage.TransportBetweenBranchPk=TBH.[TRANSPORT_PK] 
		left join OurBranchStorageCode AS OBSC ON OBSC.OurBranchStoragePk=Storage.StorageCode 
															WHERE TBH.[BRANCHPK_FROM]=" + BranchPk + " and left(Regioncode,1)=" + FromWhere + QueryWhere + ";";
		}
		DB.DBCon.Open();

		string temptotalrowcount = DB.SqlCmd.ExecuteScalar() + "";
		int TotalRowCount = temptotalrowcount == "" ? 0 : Int32.Parse(temptotalrowcount);
		if (TotalRowCount == 0) {
			return "";
		}

		if (Gubun == "in") {
			DB.SqlCmd.CommandText = @"	
	SELECT TBH.[TRANSPORT_PK], TBH.[TRANSPORT_WAY], TBH.[VALUE_STRING_0], TBH.[BRANCHPK_FROM], TBH.[DATETIME_FROM], TBH.[DATETIME_TO]
		 , TBH.[VESSELNAME], TBH.[VOYAGE_NO], TBH.[VALUE_STRING_1], TBH.[VALUE_STRING_2], TBH.[VALUE_STRING_3], TBHStep.Step, C.CompanyName, RC.Name, OBSC.StorageName 
	FROM [dbo].[TRANSPORT_HEAD] AS TBH 
		left join Company AS C on TBH.[BRANCHPK_FROM]=C.CompanyPk 
		left join RegionCode AS RC On C.RegionCode=RC.RegionCode 
		left join TransportBBStep AS TBHStep ON TBH.[TRANSPORT_PK]=TBHStep.TransportBetweenBranchPk 
		left join (
			SELECT SUM([StorageCode])/COUNT(StorageCode) AS StorageCode,[TransportBetweenBranchPk]
			FROM OurBranchStorageOut
			Group by [TransportBetweenBranchPk]
			) AS Storage ON Storage.TransportBetweenBranchPk=TBH.[TRANSPORT_PK] 
		left join OurBranchStorageCode AS OBSC ON OBSC.OurBranchStoragePk=Storage.StorageCode 
	WHERE TBH.[BRANCHPK_TO]=" + BranchPk + " and isnull(TBHStep.Step, 1)<>0 and left(C.Regioncode,1)=" + FromWhere + QueryWhere +
"	ORDER BY [DATETIME_TO] DESC; ";
		} else {
			DB.SqlCmd.CommandText = @"	
SELECT TBH.TransportBetweenBranchPk, TBH.TransportCL, TBH.BLNo, TBH.FromBranchPk, TBH.ToDateTime, TBH.FromDateTime, TBH.Description, TBHStep.Step, C.CompanyName, RC.Name, '1' AS StorageName
FROM TransportBBHead AS TBH 
	left join Company AS C on TBH.ToBranchPk=C.CompanyPk 
	left join RegionCode AS RC On C.RegionCode=RC.RegionCode 
	left join TransportBBStep AS TBHStep ON TBH.TransportBetweenBranchPk=TBHStep.TransportBetweenBranchPk 
WHERE TBH.FromBranchPk=" + BranchPk + " and isnull(TBHStep.Step, 1)<>0 and left(C.Regioncode,1)=" + FromWhere + QueryWhere +
"	ORDER BY FromDateTime DESC; ";
		}
		string WorkingDate = "";
		StringBuilder TempRow = new StringBuilder();
		StringBuilder TableBody = new StringBuilder();

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string ROW = "<tr height=\"30px; \">" +
		"<td style=\"text-align:left;\" class=\"TBody1\" >{0}</td>" +
		"<td class=\"TBody1\" style=\"text-align:left;\" >{1}</td>" +
		"<td class=\"TBody1\" style=\"text-align:center;\" ><a href=\"TransportBetweenBranchView.aspx?G=" + Gubun + "&S={5}\"> {2}</td>" +
		"<td class=\"TBody1\">{3}</td>" +
		"<td class=\"TBody1\" style=\"text-align:center;\">{4}</td>" +
		"<td class=\"TBody1\" style=\"text-align:center;\"><input type=\"button\" value=\"Excel\" onclick=\"GoExcelDown('{5}');\" /></td></tr>";
		string TABLE = "<table border='0' cellpadding='0' cellspacing='0' style=\"width:850px;\" ><thead><tr height='30px'>" +
									"<td class='THead1' style='width:130px;' >Description</td>" +
									"<td class='THead1' style='width:300px;' >Title</td>" +
									"<td class='THead1' style='width:90px;' >Date</td>" +
									"<td class='THead1' >Area</td>" +
									"<td class='THead1' style='width:120px;' colspan='2'>{1}</td>" +
								"</tr></thead>{0}</table>";
		for (int i = 0; i < (PageNo - 1) * pagelength; i++) {
			if (RS.Read()) {
				continue;
			} else {
				break;
			}
		}
		for (int i = 0; i < pagelength; i++) {
			if (RS.Read()) {
				string[] description = (RS["Description"] + "").Split(Common.Splite321, StringSplitOptions.None);
				if (WorkingDate != (RS[5] + "").Substring(0, 10)) {
					WorkingDate = (RS[5] + "").Substring(0, 10);

					string FileDownForJaemu = "";
					if (MemberInfo[1] + "" == "3157") {
						FileDownForJaemu = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span onclick=\"GoAbjiDown2('" + WorkingDate.Substring(0, 7) + "', '" + BranchPk + "', '" + Gubun + "', 'ALL');\" style=\"cursor:hand;\"  >月</span>" +
									   "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span onclick=\"GoAbjiDown2('" + WorkingDate + "', '" + BranchPk + "', '" + Gubun + "', 'ALL');\" style=\"cursor:hand;\"  >재무자료</span>";
						//"&nbsp;&nbsp;<span onclick=\"GoAbjiDown2('" + WorkingDate + "', '" + BranchPk + "', '" + Gubun + "', '12');\" style=\"cursor:hand;\"  >(12)</span>" +
						//"&nbsp;&nbsp;<span onclick=\"GoAbjiDown2('" + WorkingDate + "', '" + BranchPk + "', '" + Gubun + "', '34');\" style=\"cursor:hand;\"  >(34)</span>" +
						//"&nbsp;&nbsp;<span onclick=\"GoAbjiDown2('" + WorkingDate + "', '" + BranchPk + "', '" + Gubun + "', '5');\" style=\"cursor:hand;\"  >(5)</span>" +
						//"&nbsp;&nbsp;<span onclick=\"GoAbjiDown2('" + WorkingDate + "', '" + BranchPk + "', '" + Gubun + "', '67');\" style=\"cursor:hand;\"  >(67)</span>" +
						//"&nbsp;&nbsp;<span onclick=\"GoAbjiDown2('" + WorkingDate + "', '" + BranchPk + "', '" + Gubun + "', '89');\" style=\"cursor:hand;\"  >(89)</span>" +
						//"&nbsp;&nbsp;<span onclick=\"GoAbjiDown2('" + WorkingDate + "', '" + BranchPk + "', '" + Gubun + "', '0');\" style=\"cursor:hand;\"  >(0)</span>";
					}
					TableBody.Append("<tr><td colspan=\"6\"><strong><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span onclick=\"GoAbjiDown('" + WorkingDate + "', '" + BranchPk + "', '" + Gubun + "');\" style=\"cursor:hand;\"  >" + WorkingDate + "</span></strong>" + FileDownForJaemu + "</td></tr>");
				}

				string Title = "";
				if (RS["TransportCL"] + "" == "3") {
					if (description[0].Length > 15) {
						Title += description[0].Substring(0, 14) + " <em>" + description[5] + "</em> " + description[7];
					} else {
						Title += description[0] + " <em>" + description[5] + "</em> " + description[7];
					}
				} else {
					Title = description[0] + "" == "" ? "&nbsp;" : description[0];
				}

				string step;
				if (Gubun == "in" && RS["Step"] + "" == "") {
					step = RS["StorageName"] + "</br>" + Common.GetBetweenBranchTransportStepCL(RS["Step"] + "");
				} else {
					step = Common.GetBetweenBranchTransportStepCL(RS["Step"] + "");
				}

				var companyname = RS["CompanyName"] + "";
				if (companyname.Length > 2) {
					companyname = companyname.Substring(0, 2);
				}
				var date = Common.DateFormat("MD~D", RS["FromDateTime"] + "", RS["ToDateTime"] + "");

				var description1 = "";
				var description2 = "";

				if (description.Length > 1) {
					description1 = description[1];
				}
				if (description.Length > 2) {
					description2 = description[2];
				}

				TableBody.Append(String.Format(ROW,
					Common.GetBetweenBranchTransportWay(RS["TransportCL"] + "") + (Gubun == "in" ? " From " : " To ") + companyname,
					Title,
					date, description1 + " ~ " + description2, step, RS[0] + ""));
			} else {
				break;
			}
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"
SELECT OBSC.OurBranchStoragePk, OBSC.StorageName 
FROM TransportBBHead AS TBH 
	left join TransportBBStep AS TBHStep ON TBH.TransportBetweenBranchPk=TBHStep.TransportBetweenBranchPk 
	left join ( SELECT SUM([StorageCode])/COUNT(StorageCode) AS StorageCode,[TransportBetweenBranchPk] FROM OurBranchStorageOut Group by [TransportBetweenBranchPk] ) AS Storage ON Storage.TransportBetweenBranchPk=TBH.TransportBetweenBranchPk 
	left join OurBranchStorageCode AS OBSC ON OBSC.OurBranchStoragePk=Storage.StorageCode 
WHERE TBH.ToBranchPk=" + BranchPk + @" and isnull(TBHStep.Step, 1)<>0 and OBSC.StorageName  is not null 
GROUP BY OBSC.OurBranchStoragePk, OBSC.StorageName 
ORDER BY COUNT(OBSC.StorageName) DESC ; ";
		RS = DB.SqlCmd.ExecuteReader();
		StringBuilder OptionStorageHistory = new StringBuilder();
		if ((Request["S"] + "").Trim().ToString() == "ALL") {
			OptionStorageHistory.Append("<option value='ALL' selected>ALL</option>");
		} else {
			OptionStorageHistory.Append("<option value='ALL'>ALL</option>");
		}

		while (RS.Read()) {
			if ((Request["S"] + "").Trim().ToString() == RS[0] + "") {
				OptionStorageHistory.Append("<option value='" + RS[0] + "' selected>" + RS[1] + "</option>");
			} else {
				OptionStorageHistory.Append("<option value='" + RS[0] + "'>" + RS[1] + "</option>");
			}

		}
		RS.Dispose();
		DB.DBCon.Close();


		string PageBottom = "<TR Height='20px'><td colspan='6' style='background-color:#F5F5F5; text-align:center; padding:20px; '>" + new Common().SetPageListByNo(pagelength, PageNo, TotalRowCount, "TransportBetweenBranchList.aspx", "?G=" + Gubun + "&C=" + FromWhere + "&S=" + (Request["S"] + "").Trim().ToString() + "&") + "</TD></TR></Table>";
		return string.Format(TABLE, TableBody + PageBottom, "<select style='width:120px;'  id=\"OptionStorageHistory\" onchange=\"GotoSelect('" + Gubun + "','" + FromWhere + "')\">" + OptionStorageHistory + "</select>");
	}
}