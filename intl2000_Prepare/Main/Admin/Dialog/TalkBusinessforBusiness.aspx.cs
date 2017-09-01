using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_Dialog_TalkBusinessforBusiness : System.Web.UI.Page
{
	protected string TalkList;
	private String[] MEMBERINFO;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../../Default.aspx"); }
		MEMBERINFO = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		TalkList = LoadTalkBusinessList(Request.Params["G"] + "" == "" ? "0" : Request.Params["G"] + "", Request.Params["S"]);

		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
	}
	private String LoadTalkBusinessList(string Gubun, string pk)
	{
		DBConn DB = new DBConn();
		string TableRow = "<tr><td class=\"{3}\" align='center' ><span style=\"color:Blue;\">{0}</span></td>" +
										"<td class=\"{3}\" style=\"text-align:left;\">{1} <span style=\"color:gray;\">- {2}</span></td></tr>";

		DB.DBCon.Open();
		HistoryC HisC = new HistoryC();
		List<sComment> Comment = new List<sComment>();
		Comment = HisC.LoadList_Comment("Company", pk, "Company_Info", ref DB);
		StringBuilder Query = new StringBuilder();
		
		for (int i = 0; i < Comment.Count; i++) {
			string deleteButton = " <span style=\"cursor:hand; color:red;\" onclick=\"CommentDelete('" + Comment[i].Comment_Pk + "')\" >X</span>";
			string[] RowData = new string[] {
				Comment[i].Account_Id,
				(Comment[i].Contents).Replace("\r\n", "<br />"),
				Comment[i].Registerd.ToString().Substring(2, (Comment[i].Registerd).Length - 5)+deleteButton,
				"TBody1"
			};
			Query.Append(string.Format(TableRow, RowData));
		}

		DB.DBCon.Close();
		return Query + "";
	}
}