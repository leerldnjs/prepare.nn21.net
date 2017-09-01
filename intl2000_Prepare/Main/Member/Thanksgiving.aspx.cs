using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Member_Thanksgiving : System.Web.UI.Page {
	protected string Image;
	protected String[] SubInfo;
	protected void Page_Load(object sender, EventArgs e) {
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");

		if (Session["MemberInfo"] == null) {
			Response.Redirect("../Default.aspx");
		}
		SubInfo = Session["SubInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		//if (SubInfo[0] == "") {
		//	Image = "<img src=\"../Images/한글.jpg\" align=\"absmiddle\" border=\"0\" style=\"width: 800px;\">";
		//} else if (SubInfo[0].Substring(0, 2) == "KR" || SubInfo[0].Substring(0, 2) == "kr" || SubInfo[0].Substring(0, 2) == "Kr" || SubInfo[0].Substring(0, 2) == "kR") {
		//	Image = "<img src=\"../Images/한글.jpg\" align=\"absmiddle\" border=\"0\" style=\"width: 800px;\">";
		//} else {
		//	Image = "<img src=\"../Images/중문.jpg\" align=\"absmiddle\" border=\"0\" style=\"width: 800px;\">";
		//}



		//Image = "<img src=\"../Images/노동절휴무안내1.jpg\" align=\"absmiddle\" border=\"1\" style=\"width: 600px;\">";
		Image += "<img src=\"../Images/20160722info.jpg\" align=\"absmiddle\" border=\"1\" style=\"width: 1200px;\">";


		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } } catch (Exception) { }

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
	}
}