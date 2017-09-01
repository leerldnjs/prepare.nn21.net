using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Admin_Dialog_TalkBusiness : System.Web.UI.Page
{
	protected string TalkList;
	private String[] MEMBERINFO;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../../Default.aspx"); }
		MEMBERINFO = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		TalkList = LoadTalkBusinessList(Request.Params["G"] + "" == "" ? "0" : Request.Params["G"] + "", Request.Params["S"]);

		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
	}

	private String LoadTalkBusinessList(string Gubun, string pk) {
		DBConn DB = new DBConn();
		string TableRow = "<tr><td class=\"{4}\" align='center' ><span style=\"color:Blue;\">{0}</span></td>" +
										"<td class=\"{4}\" style=\"text-align:left;\">{1} <span style=\"color:gray;\">- {2}</span></td>" +
										"<td class=\"{4}\" >{3}</td></tr>";
		HistoryC HisC = new HistoryC();
		List<sComment> Comment = new List<sComment>();
		StringBuilder Query = new StringBuilder();
		DB.DBCon.Open();
		Comment = HisC.LoadList_Comment("Company", pk, "'Basic', 'Basic_Important'", ref DB);
		
		for(int i = 0; i < Comment.Count; i++) {
			string deleteButton = "";
			/* 모두 삭제 가능 
			if (MEMBERINFO[2] == RS["AccountID"] + "" || MEMBERINFO[2][MEMBERINFO[2].Length - 1] == '0' || MEMBERINFO[2] == "ilic03" || MEMBERINFO[2] == "ilic01") {
				deleteButton = " <span style=\"cursor:hand; color:red;\" onclick=\"CommentDelete('" + Comment[i].Comment_Pk + "')\" >X</span>";
			}
			*/
			deleteButton = " <span style=\"cursor:hand; color:red;\" onclick=\"CommentDelete('" + Comment[i].Comment_Pk + "')\" >X</span>";
			string[] RowData = new string[] {
				Comment[i].Account_Id,
				(Comment[i].Contents).Replace("\r\n", "<br />"),
				Comment[i].Registerd.ToString().Substring(2, Comment[i].Registerd.Length - 5)+deleteButton,
				"<span style=\"cursor:hand;\" onclick=\"SetGubunCL('"+Comment[i].Comment_Pk+"', '"+Comment[i].Category+"')\">"+((Comment[i].Category+"")=="Basic"?"-":"+")+"</span>",
				(Comment[i].Category)=="Basic"?"TBody1":"TBody1G"
			};

			Query.Append(string.Format(TableRow, RowData));
		}

		DB.DBCon.Close();
		return Query + "";
	}
}