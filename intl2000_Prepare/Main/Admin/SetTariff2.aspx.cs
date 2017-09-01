using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_Dialog_SetTariff : System.Web.UI.Page
{
	private DBConn DB;
	protected String Header;
	protected String TBList;
	protected String[] MemberInfo;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../Default.aspx"); }
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		if (MemberInfo[0] == "Customs")
		{
			LogedWithoutRecentRequest111.Visible = false;
			Loged1.Visible = true;
			Header = "";
		}
		else
		{
			Header = "<p><a href=\"RequestList.aspx?G=Arrival\">출발지 입고완료</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"CheckDescriptionList.aspx\">BL List</a>" +
				"&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href=\"SetTariff.aspx\"><strong>세금맞추기</strong></a></p>";
		}

		TBList = "";
		//TBList = LoadList(MemberInfo[1]);
	}
}