using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Member_OwnCustomerList : System.Web.UI.Page
{
	public String OwnCustomerListHTML;
	protected void Page_Load(object sender, EventArgs e) {
		//수정예정
		if (Session["MemberInfo"] == null) { Response.Redirect("../Default.aspx"); }
		int pageNo;
		if (Request["PageNo"] == "" || Request["PageNo"] == null) { pageNo = 1; } else { pageNo = Int32.Parse(Request["PageNo"]); }

		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } } catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }
		OwnCustomerListHTML = OwnCustomerList("C", Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[1], pageNo, "등록된 거래처내역이 없습니다.");
	}
	public String OwnCustomerList(string Gubun, string pk, int pageNo, string EmptyMSG) {
		int eachPageCount = 20;
		StringBuilder tableRow = new StringBuilder();
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "select count(*) from CompanyCustomerList where ListOwnerCompanyPk=" + pk + " and Gubun>99 ";
		DB.DBCon.Open();
		int totalRecord = (Int32)DB.SqlCmd.ExecuteScalar();
		if (totalRecord == 0) // 값이 하나도 없을때
		{
			//수정예정
			DB.DBCon.Close();
			tableRow.Append(@"<tr><td class='UnderLineDotted' style='height:20px; '></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td></tr>
												<tr><td class='UnderLineDotted' style='height:20px; '></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td></tr>
												<tr><td class='UnderLineDotted' style='height:20px; '></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td></tr>
												<tr><td class='UnderLineDotted' colspan='8' style='text-align:center; height:20px; '> " + EmptyMSG + @" </td></tr>
												<tr><td class='UnderLineDotted' style='height:20px; '></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td></tr>
												<tr><td class='UnderLineDotted' style='height:20px; '></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td></tr>
												<tr><td class='UnderLineDotted' style='height:20px; '></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td><td class='UnderLineDotted'></td></tr>");
		} else {
			DB.SqlCmd.CommandText = @"	
SELECT 
	top " + eachPageCount + @" 
	C.CompanyCode , C.CompanyTEL, C.CompanyFAX, C.PresidentEmail
	, CR.CompanyRelationPk, CR.TargetCompanyPk, CR.TargetCompanyNick, CR.Memo 
FROM CompanyRelation as CR 
	left join Company as C on CR.TargetCompanyPk=C.CompanyPk 
WHERE 
	CR.MainCompanyPk='" + pk + @"' and 
	CR.GubunCL=1 and 
	CR.CompanyRelationPk NOT IN ( 
		SELECT TOP (" + eachPageCount * (pageNo - 1) + @") CompanyRelationPk 
		From CompanyRelation 
		where MainCompanyPk='" + pk + @"' and GubunCL=1 
		Order by [CompanyRelationPk] DESC
	) 
Order by CR.[CompanyRelationPk] desc;";
			SqlDataReader returnData = DB.SqlCmd.ExecuteReader();
			string tempString = string.Empty;
			int RowCount = 1;
			while (returnData.Read()) {
				tableRow.Append("<tr>" +
							"<td class='UnderLineDotted' style='height:25px; text-align:center; '>" + RowCount + "</td>" +
							"<td class='UnderLineDotted' style='height:25px; text-align:center; '>" + returnData["CompanyCode"] + "</td>" +
							"<td class='UnderLineDotted' style='text-align:center;' ><a onclick='GotoOwnCustomerListView(" + returnData["CompanyRelationPk"] + ")' ><span style='cursor:hand;'>" +
								returnData["TargetCompanyNick"] + "</span></a></td>" +
							"<td class='UnderLineDotted' style='text-align:center;' >" + returnData["CompanyTEL"] + "<br />" + returnData["CompanyFAX"] + "</td>" +
							"<td class='UnderLineDotted' style='text-align:center;' >" + returnData["PresidentEmail"] + "</td>" +
							"<td class='UnderLineDotted'>" + returnData["Memo"] + "</td>" +
	"<td class='UnderLineDotted' style='text-align:center;'>" +
								"<input type='button' value='view' onclick='GotoOwnCustomerListView(" + returnData["CompanyRelationPk"] + ")' /></td>" +
							"<td class='UnderLineDotted' style='text-align:center;'>" +
								"<input type='button' value='del' onclick='CompanyCustomerListDel(" + returnData["CompanyRelationPk"] + ")' /></a></td></tr>");
				RowCount++;
			}
			returnData.Close();
			DB.DBCon.Close();
			if (Gubun == "C") {
				tableRow.Append("<tr height='10px'><td colspan='8' >&nbsp;</td></tr><TR Height='20px'><td colspan='8' style='background-color:#F5F5F5; text-align:center; padding:20px; '>" + new Common().SetPageListByNo(eachPageCount, pageNo, totalRecord, "OwnCustomerList.aspx", "?") + "</TD></TR></Table>");
			} else {
				tableRow.Append("<tr height='10px'><td colspan='8' >&nbsp;</td></tr><TR Height='20px'><td colspan='8' style='background-color:#F5F5F5; text-align:center; padding:20px; '>" + new Common().SetPageListByNo(eachPageCount, pageNo, totalRecord, "CompanyInfoCustomer.aspx", "?S=" + pk + "&") + "</TD></TR></Table>");
			}
		}
		return tableRow.ToString();
	}   //Member/OwnCustomerList.aspx 거래처 리스트
}