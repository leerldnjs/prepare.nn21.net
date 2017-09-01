using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;
using System.Data;

public partial class Admin_CompanyList : System.Web.UI.Page
{
	protected String COMPANYLIST;
	protected String BTN_companyaddinfo199_3157;
	protected String Memo;
	protected String Txt_companyaddinfo199_3157;
	protected String ACCOUNTID;
	protected String NAVIGATION;
	private String COMPANYPK;
	private DBConn DB;
	private Int32 PageNo;
	private Int32 PageLength;
	protected String[] MEMBERINFO;

	protected void Page_Load(object sender, EventArgs e) {
		try {
			MEMBERINFO = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}
		if (Session["Type"] + "" != "OurBranch" && Session["Type"] + "" != "ShippingBranch") {
			Response.Redirect("../Default.aspx");
		}

		DB = new DBConn();
		ACCOUNTID = Session["ID"] + "";
		COMPANYPK = Session["CompanyPk"] + "";

		PageLength = 30;
		PageNo = Request.Params["pageNo"] + "" == "" ? 1 : Int32.Parse(Request.Params["pageNo"]);

		string gubun = Request.Params["Gubun"] == null ? "Own" : Request.Params["Gubun"];
		string TypeSerch = "";
		string navigation = "";
		if (MEMBERINFO[2] == "ilic31" || MEMBERINFO[2] == "ilic01" || MEMBERINFO[2] == "ilic66" || MEMBERINFO[2] == "ilic03") {
			navigation = "<p><input type=\"button\" {0} value=\"My\" onclick=\"window.location='CompanyList.aspx?Gubun=Own'\" /> " +
												"<input type=\"button\" {1} value=\"New\" onclick=\"window.location='CompanyList.aspx?Gubun=New'\" /> " +
												"<input type=\"button\" {2} value=\"All\" onclick=\"window.location='CompanyList.aspx?Gubun=All'\" /> " +
												"<input type=\"button\" {2} value=\"KR_거래일자정렬\" onclick=\"window.location='CompanyList.aspx?Gubun=KR_'\" /> " +
												"<input type=\"button\" {3} value=\"Del\" onclick=\"window.location='CompanyList.aspx?Gubun=Del'\" /> "+
												"<input type=\"button\" value=\"지역검색\" onclick=\"window.location='/Admin/CompanyList_SearchAddress.aspx'\" /></p>";
			switch (gubun) {
				case "Own":
					NAVIGATION = string.Format(navigation, "style='font-weight:bold;'", "", "", "");
					break;
				case "New":
					NAVIGATION = string.Format(navigation, "", "style='font-weight:bold;'", "", "");
					break;
				case "All":
					NAVIGATION = string.Format(navigation, "", "", "style='font-weight:bold;'", "");
					break;
				case "Del":
					NAVIGATION = string.Format(navigation, "", "", "", "style='font-weight:bold;'");
					break;
				default:
					NAVIGATION = string.Format(navigation, "", "", "", "");
					break;
			}
			TypeSerch = "Default";
		} else if (MEMBERINFO[1] == "7898" || Session["Type"] + "" == "ShippingBranch") {
			navigation = "<p><input type=\"button\" {0} value=\"My\" onclick=\"window.location='CompanyList.aspx?Gubun=Own'\" /> ";
			switch (gubun) {
				case "Own":
					NAVIGATION = string.Format(navigation, "style='font-weight:bold;'", "", "");
					break;
				case "New":
					NAVIGATION = string.Format(navigation, "", "style='font-weight:bold;'", "");
					break;
				case "All":
					NAVIGATION = string.Format(navigation, "", "", "style='font-weight:bold;'");
					break;
				default:
					NAVIGATION = string.Format(navigation, "", "", "");
					break;
			}
			TypeSerch = "Default";
			gubun = "ShippingBranch";
		} else {
			navigation = "<p><input type=\"button\" {0} value=\"My\" onclick=\"window.location='CompanyList.aspx?Gubun=Own'\" /> " +
								"<input type=\"button\" {1} value=\"New\" onclick=\"window.location='CompanyList.aspx?Gubun=New'\" /> " +
								"<input type=\"button\" {2} value=\"All\" onclick=\"window.location='CompanyList.aspx?Gubun=All'\" /></p>";
			switch (gubun) {
				case "Own":
					NAVIGATION = string.Format(navigation, "style='font-weight:bold;'", "", "");
					break;
				case "New":
					NAVIGATION = string.Format(navigation, "", "style='font-weight:bold;'", "");
					break;
				case "All":
					NAVIGATION = string.Format(navigation, "", "", "style='font-weight:bold;'");
					break;
				default:
					NAVIGATION = string.Format(navigation, "", "", "");
					break;
			}
			TypeSerch = "Default";
		}

		if (MEMBERINFO[0] == "Customs") {
			LogedWithoutRecentRequest11.Visible = false;
			Loged1.Visible = true;
		}

		SetCompanyList(gubun, Request.Params["Value"] + "", PageNo, TypeSerch);
		if (gubun == "All") {
			LoadMemo();
			Txt_companyaddinfo199_3157 = "<textarea rows=\"5\" cols=\"100\" id=\"TB_MEMO\" style=\"width: 600px\">" + Memo + "</textarea>";
			if (ACCOUNTID == "ilman" || ACCOUNTID == "ilic00" || ACCOUNTID == "ilic66" || ACCOUNTID == "ilic31") {
				BTN_companyaddinfo199_3157 = "<input type=\"button\" style=\"margin-left:3px;\" value=\"입력\" onclick=\"companyaddinfo199_3157();\" />";
			}
		}
	}
	private string LoadMemo() {
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"
SELECT [Value]
  FROM [dbo].[CompanyAdditionalInfomation]
  where [Title]=199 and [CompanyPk]=3157;
";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			Memo = RS["Value"] + "";
		}
		RS.Dispose();
		DB.DBCon.Close();
		return "1";
	}
	protected Boolean SetCompanyList(string Gubun, string SerchValue, int PageNo, string TypeSerch) {
		Int32 totalpagecount = 0;
		string Totalrowcount = "0";
		DateTime Now = DateTime.Now;
		string AllType = "";
		string M1 = DateTime.Now.AddMonths(-1).Date.ToString().Substring(2, 8).Replace("-", "");
		string M3 = DateTime.Now.AddMonths(-3).Date.ToString().Substring(2, 8).Replace("-", "");
		string M6 = DateTime.Now.AddMonths(-6).Date.ToString().Substring(2, 8).Replace("-", "");

		DB.DBCon.Open();

		if (Gubun == "Own" || Gubun == "All" || Gubun == "KR_") {
			totalpagecount = GetTotalPageCount(Gubun, SerchValue);
			if (Gubun == "All") {
				AllType = GetCompanyCodeLeft2(SerchValue);
			}
		} else if (Gubun == "Del") {
			totalpagecount = GetTotalPageCount_DEL(Gubun, SerchValue);
		} else if (Gubun == "SerchItem") {
			Totalrowcount = GetTotalRowCount(Gubun, SerchValue);
		} else {
			totalpagecount = 0;
		}


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
		string TableRow_Colspan = "	<tr><td class='{0}' rowspan='{10}'><a href=\"CompanyView.aspx?M=View&S={1}\">{2}</a></td>" +
										"	<td class='{0}' rowspan='{10}'>{3}</td>" +
										"	<td class='{0}' rowspan='{10}'><a href=\"CompanyView.aspx?M=View&S={1}\">{4}</a></td>" +
										"	<td class='{0}' style='text-align:left; padding-left:8px;' onclick=\"Goto({1});\">{5}</td>" +
										"	<td class='{0}' style='width:20px;' rowspan='{10}'>{6}</td>" +
										"	<td class='{0}' style='width:70px;' rowspan='{10}'>{7}</td>" +
										"	<td class='{0}' style='width:20px;' rowspan='{10}'>{8}</td>" +
										"	<td class='{0}' style='width:70px;' rowspan='{10}'>{9}</td></tr>";
		string TableRow_ElseColspan = "<tr><td class='{0}' style='text-align:left; padding-left:8px;' onclick=\"Goto({1});\">{5}</td></tr>";

		bool CheckCompareCompanyCode = false;
		bool CheckContents = false;
		bool CheckColspan = false;
		
		switch (Gubun) {
			case "All":
				if (SerchValue == "") {
					DB.SqlCmd.CommandText = "EXEC SP_CompanyList_All @PageLength=" + PageLength + ",@PageNo=" + PageNo + ";";
				} else {
					DB.SqlCmd.CommandText = "EXEC SP_CompanyList_All_Code @Code='" + SerchValue + "';";

				}
				CheckContents = true;
				break;
			case "KR_":
				DB.SqlCmd.CommandText = @"
SELECT C.CompanyPk, C.GubunCL , C.CompanyCode, C.CompanyName
		, SR.SC, CR.CC
		, IL.InBoundLast
		, OL.OutBoundLast
		, RC.Name, RC.OurBranchCode
		, TB.Contents,TB.Registerd
	FROM Company AS C
		left join (SELECT shipperPk AS PK, count(*) AS SC FROM RequestForm where StepCL > 49 Group By ShipperPk) AS SR ON C.CompanyPk = SR.PK
		left join (SELECT ConsigneePk AS PK, count(*) AS CC FROM RequestForm where StepCL > 52 Group By ConsigneePk) AS CR On C.CompanyPk = CR.PK
		left join (SELECT ShipperPk, max(DepartureDate) as OutBoundLast FROM RequestForm where StepCL > 49 Group By ShipperPk) AS OL on C.CompanyPk = OL.ShipperPk
		left join (SELECT ConsigneePk, max(ArrivalDate) as InBoundLast FROM RequestForm where StepCL > 52 Group By ConsigneePk) AS IL on C.CompanyPk = IL.ConsigneePk
		left join RegionCode as RC on RC.RegionCode = C.RegionCode
		left join (SELECT [CONTENTS],[TABLE_PK],[REGISTERD] FROM [dbo].[COMMENT] WHERE [CATEGORY] = 'Company_Info' ) as TB on TB.[TABLE_PK] = C.CompanyPk
		left join (select pk ,max(RequestLast) as RequestLast from 
					(SELECT ShipperPk as pk, max(RequestDate) as RequestLast FROM RequestForm where StepCL > 49 Group By ShipperPk
					union all
					SELECT ConsigneePk as pk, max(RequestDate) as RequestLast FROM RequestForm where StepCL > 52 Group By ConsigneePk) as t
					group by pk 
					) as lastRequest on C.companypk= lastRequest.pk
	WHERE C.GubunCL < 2 and left(C.CompanyCode, 2)= 'KR' 
	ORDER BY lastRequest.RequestLast ,C.CompanyCode, C.CompanyName, TB.[REGISTERD] DESC; ";
				CheckContents = true;
				break;
			case "Own":
				if (TypeSerch == "Default") {
					DB.SqlCmd.CommandText = " EXEC SP_CompanyList_Own @PageLength=" + PageLength + ",@PageNo=" + PageNo + ",@CompanyPk=" + COMPANYPK + ";";
				} else {
					DB.SqlCmd.CommandText = " EXEC SP_CompanyList_Own_byQZ @CompanyPk=" + COMPANYPK + ";";
				}
				CheckContents = true;
				break;
			case "New":
				DB.SqlCmd.CommandText = "EXEC SP_CompanyList_New;";
				CheckContents = true;
				break;
			case "Serch":
				if (TypeSerch == "Default") {
					DB.SqlCmd.CommandText = "EXEC SP_CompanyList_Serch @Gubun='C', @SerchValue='" + SerchValue + "';";
				} else {
					DB.SqlCmd.CommandText = "EXEC SP_CompanyList_Serch_byQZ @Gubun='C', @SerchValue='" + SerchValue + "';";
				}
				PageLength = 777;
				CheckCompareCompanyCode = true;
				break;
			case "SerchAll":
				if (TypeSerch == "Default") {
					DB.SqlCmd.CommandText = "EXEC SP_CompanyList_Serch @Gubun='A', @SerchValue='" + SerchValue + "';";
				} else {
					DB.SqlCmd.CommandText = "EXEC SP_CompanyList_Serch_byQZ @Gubun='A', @SerchValue='" + SerchValue + "';";
				}
				PageLength = 777;
				CheckCompareCompanyCode = true;
				break;
			case "SerchItem":
				if (TypeSerch == "Default") {
					DB.SqlCmd.CommandText = "EXEC SP_CompanyList_Serch @Gubun='I', @SerchValue='" + SerchValue + "';";
				} else {
					DB.SqlCmd.CommandText = "EXEC SP_CompanyList_Serch_byQZ @Gubun='I', @SerchValue='" + SerchValue + "';";
				}
				PageLength = 777;
				CheckColspan = true;
				break;
			case "Del":
				DB.SqlCmd.CommandText = "EXEC SP_CompanyList_Del @PageLength=" + PageLength + ",@PageNo=" + PageNo + ";";
				CheckContents = true;
				break;
			case "ShippingBranch":
				if (TypeSerch == "Default") {
					DB.SqlCmd.CommandText = @"
SELECT C.CompanyPk, C.GubunCL , C.CompanyCode, C.CompanyName
		, SR.SC, CR.CC
		, IL.InBoundLast
		, OL.OutBoundLast
		, RC.Name, RC.OurBranchCode
		, TB.[CONTENTS],TB.[REGISTERD]
	FROM Company AS C
		left join (SELECT shipperPk AS PK, count(*) AS SC FROM RequestForm where StepCL > 49 Group By ShipperPk) AS SR ON C.CompanyPk = SR.PK
		left join (SELECT ConsigneePk AS PK, count(*) AS CC FROM RequestForm where StepCL > 52 Group By ConsigneePk) AS CR On C.CompanyPk = CR.PK
		left join (SELECT ShipperPk, max(DepartureDate) as OutBoundLast FROM RequestForm where StepCL > 49 Group By ShipperPk) AS OL on C.CompanyPk = OL.ShipperPk
		left join (SELECT ConsigneePk, max(ArrivalDate) as InBoundLast FROM RequestForm where StepCL > 52 Group By ConsigneePk) AS IL on C.CompanyPk = IL.ConsigneePk
		left join RegionCode as RC on RC.RegionCode = C.RegionCode
		left join (SELECT [CONTENTS],[TABLE_PK],[REGISTERD] FROM [dbo].[COMMENT] WHERE [CATEGORY] = 'Company_Info' ) as TB on TB.[TABLE_PK] = C.CompanyPk
		left join (select pk ,max(RequestLast) as RequestLast from 
					(SELECT ShipperPk as pk, max(RequestDate) as RequestLast FROM RequestForm where StepCL > 49 Group By ShipperPk
					union all
					SELECT ConsigneePk as pk, max(RequestDate) as RequestLast FROM RequestForm where StepCL > 52 Group By ConsigneePk) as t
					group by pk 
					) as lastRequest on C.companypk= lastRequest.pk
	WHERE C.GubunCL < 2 and C.CompanyPk in (SELECT [TargetCompanyPk]
  FROM [INTL2010].[dbo].[CompanyRelation] 
  WHERE MainCompanyPk=" + Session["CompanyPk"] + @" and GubunCL=70) and (C.CompanyCode like '%"+SerchValue+ @"%' or C.CompanyName like '%" + SerchValue + @"%' )
	ORDER BY lastRequest.RequestLast ,C.CompanyCode, C.CompanyName, TB.[Registerd] DESC; ";

				} else {
					DB.SqlCmd.CommandText = @"
SELECT C.CompanyPk, C.GubunCL , C.CompanyCode, C.CompanyName
		, SR.SC, CR.CC
		, IL.InBoundLast
		, OL.OutBoundLast
		, RC.Name, RC.OurBranchCode
		, TB.[CONTENTS],TB.[REGISTERD]
	FROM Company AS C
		left join (SELECT shipperPk AS PK, count(*) AS SC FROM RequestForm where StepCL > 49 Group By ShipperPk) AS SR ON C.CompanyPk = SR.PK
		left join (SELECT ConsigneePk AS PK, count(*) AS CC FROM RequestForm where StepCL > 52 Group By ConsigneePk) AS CR On C.CompanyPk = CR.PK
		left join (SELECT ShipperPk, max(DepartureDate) as OutBoundLast FROM RequestForm where StepCL > 49 Group By ShipperPk) AS OL on C.CompanyPk = OL.ShipperPk
		left join (SELECT ConsigneePk, max(ArrivalDate) as InBoundLast FROM RequestForm where StepCL > 52 Group By ConsigneePk) AS IL on C.CompanyPk = IL.ConsigneePk
		left join RegionCode as RC on RC.RegionCode = C.RegionCode
		left join (SELECT [CONTENTS],[TABLE_PK],[REGISTERD] FROM [dbo].[COMMENT] WHERE [CATEGORY] = 'Company_Info' ) as TB on TB.[TABLE_PK] = C.CompanyPk
		left join (select pk ,max(RequestLast) as RequestLast from 
					(SELECT ShipperPk as pk, max(RequestDate) as RequestLast FROM RequestForm where StepCL > 49 Group By ShipperPk
					union all
					SELECT ConsigneePk as pk, max(RequestDate) as RequestLast FROM RequestForm where StepCL > 52 Group By ConsigneePk) as t
					group by pk 
					) as lastRequest on C.companypk= lastRequest.pk
	WHERE C.GubunCL < 2 and C.CompanyPk in (SELECT [TargetCompanyPk]
  FROM [INTL2010].[dbo].[CompanyRelation] 
  WHERE MainCompanyPk=" + Session["CompanyPk"] + @" and GubunCL=70)
	ORDER BY lastRequest.RequestLast ,C.CompanyCode, C.CompanyName, TB.[REGISTERD] DESC; ";
				}
				break;

		}
		string beforecompany = "";
		string style = "";
		string Inbound = "";
		string Outbound = "";
		string RowSpanStyle = "";
		string[] RowData = new string[11];
		if (CheckColspan) {
			DataTable ReturnValue = new DataTable();
			SqlDataAdapter DA = new SqlDataAdapter(DB.SqlCmd);
			DA.Fill(ReturnValue);
			DB.DBCon.Close();
			DataTable RS = ReturnValue;

			StringBuilder ReturnValueS = new StringBuilder();
			bool isFirstRow = true;
			for (var i = 0; i < RS.Rows.Count; i++) {
				DataRow row = RS.Rows[i];
				style = row["GubunCL"] + "" == "0" ? "TBody1G" : "TBody1";
				Inbound = row["InBoundLast"] + "" == "" ? "&nbsp;" : (row["InBoundLast"] + "").Substring(2, 6);
				if (string.Compare(Inbound, M1) > 0) {
					Inbound = "<span style='color:black;'>" + Inbound + "</span>";
				} else if (string.Compare(Inbound, M3) > 0) {
					Inbound = "<span style='color:blue;'>" + Inbound + "</span>";
				} else if (string.Compare(Inbound, M6) > 0) {
					Inbound = "<span style='color:green;'>" + Inbound + "</span>";
				} else {
					Inbound = "<span style='color:gray;'>" + Inbound + "</span>";
				}

				Outbound = row["OutBoundLast"] + "" == "" ? "&nbsp;" : (row["OutBoundLast"] + "").Substring(2, 6);
				if (string.Compare(Outbound, M1) > 0) {
					Outbound = "<span style='color:black;'>" + Outbound + "</span>";
				} else if (string.Compare(Outbound, M3) > 0) {
					Outbound = "<span style='color:blue;'>" + Outbound + "</span>";
				} else if (string.Compare(Outbound, M6) > 0) {
					Outbound = "<span style='color:green;'>" + Outbound + "</span>";
				} else {
					Outbound = "<span style='color:gray;'>" + Outbound + "</span>";
				}
				isFirstRow = beforecompany == row["CompanyPk"] + "" ? false : true;
				if (isFirstRow) {
					beforecompany = RS.Rows[i]["CompanyPk"] + "";
					var countRowspan = 1;
					while (true) {
						if (i + countRowspan < RS.Rows.Count && beforecompany == RS.Rows[i + countRowspan]["CompanyPk"] + "") {
							countRowspan++;
							continue;
						}
						break;
					}
					if (countRowspan > 1) {
						RowSpanStyle = countRowspan.ToString();
					} else {
						RowSpanStyle = countRowspan.ToString();
					}
				}
				if (isFirstRow) {
					RowData[0] = style;
					RowData[1] = row["CompanyPk"] + "";
					RowData[2] = row["CompanyCode"] + "";
					RowData[3] = row["Name"] + "" == "" ? "&nbsp;" : row["Name"] + "";
					RowData[4] = row["CompanyName"] + "" == "" ? "&nbsp;" : row["CompanyName"] + "";
					RowData[5] = row["Contents"] + "" == "" ? "&nbsp;" : row["Contents"] + " / " + row["Registerd"].ToString().Substring(0, 10);
					RowData[6] = row["SC"] + "" == "" ? "&nbsp;" : row["SC"] + "";
					RowData[7] = Outbound;
					RowData[8] = row["CC"] + "" == "" ? "&nbsp;" : row["CC"] + "";
					RowData[9] = Inbound;
					RowData[10] = RowSpanStyle;

					ReturnValueS.Append(string.Format(TableRow_Colspan, RowData));
				} else {
					RowData[5] = row["Contents"] + "" == "" ? "&nbsp;" : row["Contents"] + "";
					ReturnValueS.Append(string.Format(TableRow_ElseColspan, RowData));
				}
			}
			COMPANYLIST = string.Format(TableBody,
				   ReturnValueS + "",
				   new Common().SetPageListByNo(PageLength, PageNo, totalpagecount, "CompanyList.aspx", "?Gubun=" + Gubun + "&Value=" + SerchValue + "&"));
		} else {
			StringBuilder ReturnValue = new StringBuilder();
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (CheckContents) {
				while (RS.Read()) {
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
				}
				RS.Dispose();
				DB.DBCon.Close();
				COMPANYLIST = string.Format(TableBody,
					ReturnValue + "",
					new Common().SetPageListByNo(PageLength, PageNo, totalpagecount, "CompanyList.aspx", "?Gubun=" + Gubun + "&Value=" + SerchValue + "&"));

			} else {
				if (totalpagecount != 0) {
					for (int i = 1; i < PageNo; i++) {
						for (int j = 0; j < PageLength; j++) {
							RS.Read();
						}
					}
				}
				for (int i = 0; i < PageLength; i++) {
					if (RS.Read()) {
						if (CheckCompareCompanyCode) {
							if (beforecompany != RS[0] + "") {
								beforecompany = RS[0] + "";
							} else {
								continue;
							}
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
					new Common().SetPageListByNo(PageLength, PageNo, totalpagecount, "CompanyList.aspx", "?Gubun=" + Gubun + "&Value=" + SerchValue + "&"));
			}
		}
		return true;
	}
	private Int32 GetTotalPageCount(string Gubun, string Value) {
		switch (Gubun) {
			case "Own":
				DB.SqlCmd.CommandText = "SELECT Count(*) FROM Company AS C left join RegionCode AS RC ON C.RegionCode=RC.RegionCode WHERE C.GubunCL<2 and RC.OurBranchCode=" + COMPANYPK;
				break;
			case "All":
				if (Value == "") {
					DB.SqlCmd.CommandText = "SELECT Count(*) FROM Company WHERE GubunCL <2 ";
				} else {
					DB.SqlCmd.CommandText = "SELECT Count(*) FROM Company WHERE GubunCL <2 and Left(CompanyCode, 2)='" + Value + "' ";
				}
				break;
			case "KR_":
				DB.SqlCmd.CommandText = "SELECT Count(*) FROM Company WHERE GubunCL <2 and Left(CompanyCode, 2)='KR' ";
				break;
		}
		Int32 totalpagecount = (int)DB.SqlCmd.ExecuteScalar();
		return totalpagecount;
	}
	private Int32 GetTotalPageCount_DEL(string Gubun, string Value) {
		DB.SqlCmd.CommandText = "SELECT Count(*) FROM Company WHERE GubunCL =5 and CompanyCode is not null";
		Int32 totalpagecount = (int)DB.SqlCmd.ExecuteScalar();
		return totalpagecount;
	}
	private String GetCompanyCodeLeft2(string Value) {
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"
	SELECT left ([CompanyCode], 2), COUNT(*) AS C 
	FROM Company 
	WHERE GubunCL <2
	GROUP BY left ([CompanyCode], 2) 
	ORDER BY C DESC";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if ((RS[1] + "").Length > 0) {
				ReturnValue.Append("<input type=\"button\" value=\"" + RS[0] + "[" + RS[1] + "]\" onclick=\"window.location='CompanyList.aspx?Gubun=All&Value=" + RS[0] + "'\" /> ");
			} else {
				break;
			}
		}
		RS.Dispose();
		return ReturnValue + "";
	}
	private string GetTotalRowCount(string Gubun, string Value) {
		DB.SqlCmd.CommandText = @"SELECT count(*)
										FROM Company AS C 
			left join (SELECT [CONTENTS],[TABLE_PK],[REGISTERD] FROM [dbo].[COMMENT] ) as TB on TB.[TABLE_PK] = C.CompanyPk
		WHERE C.GubunCL<2 and TB.[CONTENTS] Like '%" + Value + @"%';";
		string totalpagecount = DB.SqlCmd.ExecuteScalar() + "";
		return totalpagecount;
	}
}