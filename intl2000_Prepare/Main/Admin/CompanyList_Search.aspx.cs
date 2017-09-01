using Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_CompanyList_Search : System.Web.UI.Page {
	protected String COMPANYLIST;
	protected String Memo;
	protected String ACCOUNTID;
	protected String NAVIGATION;
	private String COMPANYPK;
	private DBConn DB;
	private Int32 PageNo;
	private Int32 PageLength;
	protected String[] MEMBERINFO;
	protected String BranchCode, Criteriadate, monthago, Hmonthago;
	protected void Page_Load(object sender, EventArgs e) {
		try {
			MEMBERINFO = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}
		DB = new DBConn();
		ACCOUNTID = Session["ID"] + "";
		COMPANYPK = Session["CompanyPk"] + "";

		PageLength = 30;
		PageNo = Request.Params["pageNo"] + "" == "" ? 1 : Int32.Parse(Request.Params["pageNo"]);

		//string BranchCode, string monthago, string Criteriadate
		BranchCode = Request.Params["B"] + "" == "" ? "KR" : Request.Params["B"] + "";
		int Tempmonth = 0 - int.Parse(Request.Params["M"] + "" == "" ? "1" : Request.Params["M"] + "");
		DateTime dt = DateTime.Today.AddMonths(Tempmonth);
		monthago = dt.ToString("yyyyMMdd");
		Hmonthago = Request.Params["M"] + "" == "" ? "1" : Request.Params["M"] + "";
		Criteriadate = Request.Params["D"] + "" == "" ? "20150101" : Request.Params["D"] + "";

		SetCompanyList(BranchCode, PageNo, monthago, Criteriadate);
	}
	protected Boolean SetCompanyList(string BranchCode, int PageNo, string monthago, string Criteriadate) {
		Int32 totalpagecount = 0;
		DateTime Now = DateTime.Now;
		string AllType = "";
		string M1 = DateTime.Now.AddMonths(-1).Date.ToString().Substring(2, 8).Replace("-", "");
		string M3 = DateTime.Now.AddMonths(-3).Date.ToString().Substring(2, 8).Replace("-", "");
		string M6 = DateTime.Now.AddMonths(-6).Date.ToString().Substring(2, 8).Replace("-", "");

		DB.DBCon.Open();

		totalpagecount = GetTotalPageCount(BranchCode, monthago, Criteriadate);



		string TableBody = AllType + "<table border='0' cellpadding='0' cellspacing='1' style=\"width:1050px;\" ><thead><tr height='30px'>" +
										"	<td class='THead1' style='width:60px;' >" + GetGlobalResourceObject("Member", "CompanyCode") + "</td>" +
										"	<td class='THead1' style='width:50px;' >" + GetGlobalResourceObject("RequestForm", "Region") + "</td>" +
										"	<td class='THead1' style='width:200px;'>" + GetGlobalResourceObject("Member", "CompanyName") + "</td>" +
										"	<td class='THead1' style='' >Business Note</td>" +
										"	<td class='THead1' style='width:90px;' colspan='2' >Out Bound</td>" +
										"	<td class='THead1' style='width:90px;' colspan='2' >In Bound</td></tr></thead>{0}" +
										"<tr height='10px'><td colspan='8' >&nbsp;</td></tr><TR Height='20px'><td colspan='8' style='background-color:#F5F5F5; text-align:center; padding:20px; '>{1}</TD></TR></Table>";
		string TableRow = "	<tr><td class='{0}'><a href=\"CompanyView.aspx?M=View&S={1}\">{2}</a></td>" +
										"	<td class='{0}' >{3}</td>" +
										"	<td class='{0}' ><a href=\"CompanyView.aspx?M=View&S={1}\">{4}</a></td>" +
										"	<td class='{0}' style='text-align:left; padding-left:8px;' onclick=\"Goto({1});\">{5}</td>" +
										"	<td class='{0}' style='width:20px;' >{6}</td>" +
										"	<td class='{0}' style='width:70px;' >{7}</td>" +
										"	<td class='{0}' style='width:20px;' >{8}</td>" +
										"	<td class='{0}' style='width:70px;' >{9}</td></tr>";

		DB.SqlCmd.CommandText = string.Format(@"
SELECT C.CompanyPk, C.GubunCL , C.CompanyCode, C.CompanyName
		, SR.SC, CR.CC
		, IL.InBoundLast 
		, OL.OutBoundLast 
		, RC.Name, RC.OurBranchCode
		, TB.[CONTENTS],TB.[REGISTERD]
	FROM Company AS C 
		left join ( SELECT shipperPk AS PK, count(*) AS SC FROM RequestForm where StepCL>49 Group By ShipperPk ) AS SR ON C.CompanyPk=SR.PK 
		left join ( SELECT ConsigneePk AS PK, count(*) AS CC FROM RequestForm where StepCL>52 Group By ConsigneePk) AS CR On C.CompanyPk=CR.PK
		left join ( SELECT ShipperPk, max(DepartureDate) as OutBoundLast FROM RequestForm where StepCL>49 Group By ShipperPk) AS OL on C.CompanyPk=OL.ShipperPk
		left join ( SELECT ConsigneePk, max(ArrivalDate) as InBoundLast FROM RequestForm where StepCL>52 Group By ConsigneePk) AS IL on C.CompanyPk=IL.ConsigneePk 
		left join RegionCode as RC on RC.RegionCode=C.RegionCode 
		left join (SELECT [CONTENTS],[TABLE_PK],[REGISTERD] FROM [dbo].[COMMENT] WHERE [CATEGORY] = 'Company_Info' ) as TB on TB.[TABLE_PK] = C.CompanyPk
	WHERE C.GubunCL<2 and left(C.CompanyCode, 2)='{0}' 
     and (isnull(IL.InBoundLast,'19999999')<'{1}' and isnull(IL.InBoundLast,'19999999')>'{2}')
	 and (isnull(OL.OutBoundLast,'19999999')<'{1}' and isnull(IL.InBoundLast,'19999999')>'{2}')
	and companypk in (
select pk from 
(select ShipperPk as pk  from RequestForm
where RequestDate>'{2}'  and ShipperPk is not null
group by ShipperPk 
union all
select ConsigneePk as pk from RequestForm
where RequestDate>'{2}' and RequestDate<'{1}' and ConsigneePk is not null
group by ConsigneePk 
) as t group by pk)
	ORDER  BY C.CompanyCode, C.CompanyName, TB.[Registerd] DESC;", BranchCode, monthago, Criteriadate);


		string beforecompany = "";
		string style = "";
		string Inbound = "";
		string Outbound = "";
		string[] RowData = new string[11];

		StringBuilder ReturnValue = new StringBuilder();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (totalpagecount != 0) {
			for (int i = 1; i < PageNo; i++) {
				for (int j = 0; j < PageLength; j++) {
					RS.Read();
				}
			}
		}
		for (int i = 0; i < PageLength; i++) {
			if (RS.Read()) {

				if (beforecompany != RS[0] + "") {
					beforecompany = RS[0] + "";
				} else {
					continue;
				}

				style = RS["GubunCL"] + "" == "0" ? "TBody1G" : "TBody1";
				Inbound = RS["InBoundLast"] + "" == "" ? "&nbsp;" : (RS["InBoundLast"] + "").Substring(2, 6);
				if (string.Compare(Inbound, M1) > 0) {
					Inbound = "<span style='color:black;'>" + Inbound + "</span>";
				} else if (string.Compare(Inbound, M3) > 0) {
					Inbound = "<span style='color:blue;'>" + Inbound + "</span>";
				} else if (string.Compare(Inbound, M6) > 0) {
					Inbound = "<span style='color:green;'>" + Inbound + "</span>";
				} else {
					Inbound = "<span style='color:gray;'>" + Inbound + "</span>";
				}

				Outbound = RS["OutBoundLast"] + "" == "" ? "&nbsp;" : (RS["OutBoundLast"] + "").Substring(2, 6);
				if (string.Compare(Outbound, M1) > 0) {
					Outbound = "<span style='color:black;'>" + Outbound + "</span>";
				} else if (string.Compare(Outbound, M3) > 0) {
					Outbound = "<span style='color:blue;'>" + Outbound + "</span>";
				} else if (string.Compare(Outbound, M6) > 0) {
					Outbound = "<span style='color:green;'>" + Outbound + "</span>";
				} else {
					Outbound = "<span style='color:gray;'>" + Outbound + "</span>";
				}
				RowData[0] = style;
				RowData[1] = RS["CompanyPk"] + "";
				RowData[2] = RS["CompanyCode"] + "";
				RowData[3] = RS["Name"] + "" == "" ? "&nbsp;" : RS["Name"] + "";
				RowData[4] = RS["CompanyName"] + "" == "" ? "&nbsp;" : RS["CompanyName"] + "";
				RowData[5] = RS["Contents"] + "" == "" ? "&nbsp;" : RS["Contents"] + " / " + RS["Registerd"].ToString().Substring(0, 10);
				RowData[6] = RS["SC"] + "" == "" ? "&nbsp;" : RS["SC"] + "";
				RowData[7] = Outbound;
				RowData[8] = RS["CC"] + "" == "" ? "&nbsp;" : RS["CC"] + "";
				RowData[9] = Inbound;
				ReturnValue.Append(string.Format(TableRow, RowData));
			} else {
				break;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		COMPANYLIST = string.Format(TableBody,
			ReturnValue + "",
	new Common().SetPageListByNo(PageLength, PageNo, totalpagecount, "CompanyList_Search.aspx", "?M=" + Hmonthago + "&B=" + BranchCode + "&D=" + Criteriadate + "&"));
		return true;
	}
	private Int32 GetTotalPageCount(string BranchCode, string monthago, string Criteriadate) {
		DB.SqlCmd.CommandText = string.Format(@"
select count(*) from company C
left join ( SELECT ShipperPk, max(DepartureDate) as OutBoundLast FROM RequestForm where StepCL>49 Group By ShipperPk) AS OL on C.CompanyPk=OL.ShipperPk
left join ( SELECT ConsigneePk, max(ArrivalDate) as InBoundLast FROM RequestForm where StepCL>52 Group By ConsigneePk) AS IL on C.CompanyPk=IL.ConsigneePk 
where GubunCL<2
and left(companycode,2)='{0}'
and (isnull(IL.InBoundLast,'19999999')<'{1}' and isnull(IL.InBoundLast,'19999999')>'{2}')
and (isnull(OL.OutBoundLast,'19999999')<'{1}' and isnull(IL.InBoundLast,'19999999')>'{2}')", BranchCode, monthago, Criteriadate);

		Int32 totalpagecount = (int)DB.SqlCmd.ExecuteScalar();
		return totalpagecount;
	}
}