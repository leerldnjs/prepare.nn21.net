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

public partial class CustomClearance_HSCode_Tariff : System.Web.UI.Page
{
	protected String[] MemberInfo;
	protected string HtmlList, Html_Paging, CompanyPk, MakeHtml_HscodeHistory;
	protected string Search_HtmlList, Search_Html_Paging;
	protected String SearchValue;
	private int PageNo;
	private DBConn DB;
	private StringBuilder SumHsCode;
	protected void Page_Load(object sender, EventArgs e) {
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
		SearchValue = Request.Params["SearchValue"] + "";
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);



		try {
			PageNo = Int32.Parse(Request["PageNo"]);
		} catch (Exception) {
			PageNo = 1;
		}


		// 순서 바꾸면 에러남 밑에꺼
		if (MemberInfo[0] == "OurBranch") {
			CompanyPk = Request.Params["S"] + "";
			LogedWithoutRecentRequest.Visible = true;
			Loged1.Visible = false;
		} else {
			LogedWithoutRecentRequest.Visible = false;
			Loged1.Visible = true;
			CompanyPk = MemberInfo[1] + "";
		}
		DB = new DBConn();
		MakeHtml_HscodeHistory = LoadHscodeHistory();
		if (MemberInfo[0] == "OurBranch") {
			MakeHtml_HscodeList_Company();
			MakeHtml_HscodeList();
		} else {
			MakeHtml_HscodeList_Company();
		}


	}
	private string LoadHscodeHistory() {

		DB.SqlCmd.CommandText = string.Format(@"
		SELECT [HSCode],[Description]
        FROM [ClearanceItemCodeKOR] 
		WHERE [CompanyPk]={0} 
		and [Deleted] is null 
		and [HSCode] is not null
		and Description <> '김경민'
		GROUP BY [HSCode],[Description]
		ORDER BY [HSCode] desc;", CompanyPk);
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder ReturnValue = new StringBuilder();
		SumHsCode = new StringBuilder();
		StringBuilder TempDescription = new StringBuilder();
		int i = 0;
		string TempHsCode = "";
		string TempDescription_One = "";
		bool isfirst = true;
		while (RS.Read()) {
			if (isfirst) {
				TempHsCode = RS["HSCode"].ToString().Replace(".", "").Replace("-", "").Trim();
				TempDescription_One = RS["Description"].ToString();
				SumHsCode.Append("," + RS["HSCode"].ToString().Replace(".", "").Replace("-", "").Trim());
			}

			if (TempHsCode == RS["HSCode"].ToString().Replace(".", "").Replace("-", "").Trim()) {
				if (isfirst) {
					isfirst = false;
				}
				TempDescription.Append("," + RS["Description"] + "");
				continue;
			} else {



				if (i == 0) {
					ReturnValue.Append("<tr>");
				}
				ReturnValue.Append("<td><strong>" + TempHsCode + "</strong></td><td onclick=\"popDescription('" + TempDescription.ToString().Replace('"', ' ').Replace("'", " ") + "')\">" + TempDescription_One + "</td>");
				TempDescription = new StringBuilder();
				TempDescription.Append("," + RS["Description"] + "");
				TempHsCode = RS["HSCode"].ToString().Replace(".", "").Replace("-", "").Trim();
				TempDescription_One = RS["Description"] + "";
				SumHsCode.Append("," + RS["HSCode"].ToString().Replace(".", "").Replace("-", "").Trim());
				i++;
				if (i == 2) {
					ReturnValue.Append("</tr>");
					i = 0;
				}
			}
		}
		ReturnValue.Append("<td><strong>" + TempHsCode + "</strong></td><td onclick=\"popDescription('" + TempDescription.ToString() + "')\"> " + TempDescription_One + "</td>");



		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue.ToString();
	}

	private string MakeHtml_HscodeList() {

		int TotalLength = 0;
		int PageLength = 30;
		IEnumerable<DataRow> Company;

		if (SearchValue == "") {
			Company = LoadHscodeList(PageLength, PageNo, out TotalLength);

		} else {
			Company = LoadHscodeList(PageLength, PageNo, out TotalLength, SearchValue);
		}
		string TableBody = @"
				<table class=""table"" >
					<thead>
						<tr>
							<th>HSCode</th>
							<th>Comment</th>
							<th>기본</th>
							<th>WTO</th>
							<th>아태-CO</th>
							<th>FTA 유형</th>							
							<th>FTA 2016</th>
							<th>FTA 2017</th>
							<th>FTA 2018</th>
							<th>FTA 2019</th>
						</tr>
					</thead>
					<tbody>{0}</tbody>
				</table>";
		string RowFormat = @"
				<tr>
					<td>{0}</td>
					<td>{1}</td>
					<td>{2}</td>
					<td>{3}</td>
					<td>{4}</td>
					<td>{5}</td>					
					<td>{6}</td>
					<td>{7}</td>
					<td>{8}</td>
					<td>{9}</td>
				</tr>";
		StringBuilder InnerHtml = new StringBuilder();
		foreach (DataRow Each in Company) {
			string[] ListData = new string[11];
			ListData[0] = Each["HSCode"] + "";
			ListData[1] = Each["Comment"] + "";
			ListData[2] = Common.NumberFormat(Each["DTariff"] + "");
			ListData[3] = Common.NumberFormat(Each["CTariff"] + "");
			ListData[4] = Common.NumberFormat(Each["E1Tariff"] + "");
			ListData[5] = Common.NumberFormat(Each["UseLimit_Description"] + "");			
			ListData[6] = Common.NumberFormat(Each["F2016Tariff"] + "");
			ListData[7] = Common.NumberFormat(Each["F2017Tariff"] + "");
			ListData[8] = Common.NumberFormat(Each["F2018Tariff"] + "");
			ListData[9] = Common.NumberFormat(Each["F2019Tariff"] + "");

			InnerHtml.AppendFormat(RowFormat, ListData);
		}

		Search_HtmlList = string.Format(TableBody, InnerHtml + "");

		if (MemberInfo[0] == "OurBranch") {
			Search_Html_Paging = new Common().LoadHtml_Paging_Post(PageLength, PageNo, TotalLength, "/CustomClearance/HSCode_Tariff.aspx?S=" + CompanyPk + "&" + (SearchValue == "" ? "" : "SearchValue=" + SearchValue));
		} else {
			Search_Html_Paging = new Common().LoadHtml_Paging_Post(PageLength, PageNo, TotalLength, "/CustomClearance/HSCode_Tariff.aspx?" + (SearchValue == "" ? "" : "SearchValue=" + SearchValue));
		}

		return "1";
	}
	private string MakeHtml_HscodeList_Company() {
		if (SumHsCode + "" != "") {
			IEnumerable<DataRow> Company = LoadHscodeList_Company();

			string TableBody = @"
				<table class=""table"" >
					<thead>
						<tr>
							<th>HSCode</th>
							<th>Comment</th>
							<th>기본</th>
							<th>WTO</th>
							<th>아태-CO</th>
							<th>FTA 유형</th>
							<th>FTA 2016</th>
							<th>FTA 2017</th>
							<th>FTA 2018</th>
							<th>FTA 2019</th>
						</tr>
					</thead>
					<tbody>{0}</tbody>
				</table>";
			string RowFormat = @"
				<tr>
					<td>{0}</td>
					<td>{1}</td>
					<td>{2}</td>
					<td>{3}</td>
					<td>{4}</td>
					<td>{5}</td>
					<td>{6}</td>
					<td>{7}</td>
					<td>{8}</td>
					<td>{8}</td>
				</tr>";
			StringBuilder InnerHtml = new StringBuilder();
			foreach (DataRow Each in Company) {
				string[] ListData = new string[11];
				ListData[0] = Each["HSCode"] + "";
				ListData[1] = Each["Comment"] + "";
				ListData[2] = Common.NumberFormat(Each["DTariff"] + "");
				ListData[3] = Common.NumberFormat(Each["CTariff"] + "");
				ListData[4] = Common.NumberFormat(Each["E1Tariff"] + "");
				ListData[5] = Common.NumberFormat(Each["UseLimit_Description"] + "");				
				ListData[6] = Common.NumberFormat(Each["F2016Tariff"] + "");
				ListData[7] = Common.NumberFormat(Each["F2017Tariff"] + "");
				ListData[8] = Common.NumberFormat(Each["F2018Tariff"] + "");
				ListData[9] = Common.NumberFormat(Each["F2019Tariff"] + "");

				InnerHtml.AppendFormat(RowFormat, ListData);
			}

			HtmlList = string.Format(TableBody, InnerHtml + "");
			Html_Paging = "";
		} else {
			HtmlList = "<strong>HSCode가 없습니다</strong>";
			Html_Paging = "";
		}
		return "1";
	}

	private IEnumerable<DataRow> LoadHscodeList(int PageLength, int PageNo, out int TotalCount, string SearchValue = null) {
		string SerchValue_QueryWhere = "";
		if (SearchValue != null) {
			SerchValue_QueryWhere = string.Format(" and [HSCode] like '{0}%'", SearchValue.Trim());
		} else {
			SerchValue_QueryWhere = " and 1<>1";
		}

		string TotalCountQuery = string.Format(@"
		SELECT count(*)
		FROM [INTL2010].[dbo].[Lib_HSCode] WHERE 1=1 {0}", SerchValue_QueryWhere);
		string strTotalCount = Utility.ExecuteScalar(TotalCountQuery);
		TotalCount = strTotalCount == "" ? 0 : Int32.Parse(strTotalCount);

		string Query = string.Format(@"
SELECT 
 L.HSCode,L.Comment
,D.TariffRate0 DTariff
,C.TariffRate0 CTariff
,E1.TariffRate0 E1Tariff
,F2015.UseLimit_Description 
,F2015.TariffRate0 F2015Tariff
,F2016.TariffRate0 F2016Tariff
,F2017.TariffRate0 F2017Tariff
,F2018.TariffRate0 F2018Tariff
,F2019.TariffRate0 F2019Tariff
FROM [INTL2010].[dbo].[Lib_HSCode] L
left join (select * from [dbo].[Lib_HSCode_TariffRate] where kind='') D on L.Lib_HSCodePk=D.Lib_HSCodePk
left join (select * from [dbo].[Lib_HSCode_TariffRate] where kind='C') C on L.Lib_HSCodePk=C.Lib_HSCodePk
left join (select * from [dbo].[Lib_HSCode_TariffRate] where kind='E1') E1 on L.Lib_HSCodePk=E1.Lib_HSCodePk
left join (SELECT * FROM [dbo].[Lib_HSCode_TariffRate] where Kind='F' and UseLimit_Date='2015') F2015 on L.Lib_HSCodePk=F2015.Lib_HSCodePk
left join (SELECT * FROM [dbo].[Lib_HSCode_TariffRate] where Kind='F' and UseLimit_Date='2016') F2016 on L.Lib_HSCodePk=F2016.Lib_HSCodePk
left join (SELECT * FROM [dbo].[Lib_HSCode_TariffRate] where Kind='F' and UseLimit_Date='2017') F2017 on L.Lib_HSCodePk=F2017.Lib_HSCodePk
left join (SELECT * FROM [dbo].[Lib_HSCode_TariffRate] where Kind='F' and UseLimit_Date='2018') F2018 on L.Lib_HSCodePk=F2018.Lib_HSCodePk
left join (SELECT * FROM [dbo].[Lib_HSCode_TariffRate] where Kind='F' and UseLimit_Date='2019') F2019 on L.Lib_HSCodePk=F2019.Lib_HSCodePk
where 1=1 {0}", SerchValue_QueryWhere);

		DataTable Resource = Utility.SelectAnyQuery(Query);
		IEnumerable<DataRow> RS;
		if (PageLength == 0) {
			RS = Resource.Select();
		} else {
			RS = Resource.Select().Skip((PageNo - 1) * PageLength).Take(PageLength);
		}
		return RS;
	}
	private IEnumerable<DataRow> LoadHscodeList_Company() {

		string Query = string.Format(@"
SELECT 
 L.HSCode,L.Comment
,D.TariffRate0 DTariff
,C.TariffRate0 CTariff
,E1.TariffRate0 E1Tariff
,F2015.UseLimit_Description 
,F2015.TariffRate0 F2015Tariff
,F2016.TariffRate0 F2016Tariff
,F2017.TariffRate0 F2017Tariff
,F2018.TariffRate0 F2018Tariff
,F2019.TariffRate0 F2019Tariff
FROM [INTL2010].[dbo].[Lib_HSCode] L
left join (select * from [dbo].[Lib_HSCode_TariffRate] where kind='') D on L.Lib_HSCodePk=D.Lib_HSCodePk
left join (select * from [dbo].[Lib_HSCode_TariffRate] where kind='C') C on L.Lib_HSCodePk=C.Lib_HSCodePk
left join (select * from [dbo].[Lib_HSCode_TariffRate] where kind='E1') E1 on L.Lib_HSCodePk=E1.Lib_HSCodePk
left join (SELECT * FROM [dbo].[Lib_HSCode_TariffRate] where Kind='F' and UseLimit_Date='2015') F2015 on L.Lib_HSCodePk=F2015.Lib_HSCodePk
left join (SELECT * FROM [dbo].[Lib_HSCode_TariffRate] where Kind='F' and UseLimit_Date='2016') F2016 on L.Lib_HSCodePk=F2016.Lib_HSCodePk
left join (SELECT * FROM [dbo].[Lib_HSCode_TariffRate] where Kind='F' and UseLimit_Date='2017') F2017 on L.Lib_HSCodePk=F2017.Lib_HSCodePk
left join (SELECT * FROM [dbo].[Lib_HSCode_TariffRate] where Kind='F' and UseLimit_Date='2018') F2018 on L.Lib_HSCodePk=F2018.Lib_HSCodePk
left join (SELECT * FROM [dbo].[Lib_HSCode_TariffRate] where Kind='F' and UseLimit_Date='2019') F2019 on L.Lib_HSCodePk=F2019.Lib_HSCodePk
where 1=1 and L.HSCode in ({0}) ORDER BY [HSCode] desc", SumHsCode.ToString().Substring(1));

		DataTable Resource = Utility.SelectAnyQuery(Query);
		IEnumerable<DataRow> RS;
		RS = Resource.Select();

		return RS;
	}
}
