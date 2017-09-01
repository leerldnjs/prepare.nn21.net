using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_CompanyInfo : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected String BTN_ModifyCompanyCode;
	protected String BTN_wldurtnwjd;
	protected String TB_ResponsibleStaff;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("../Default.aspx");
			//Response.Write("Asdf");
		}

		MEMBERINFO = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);



		switch (MEMBERINFO[0]) {
			case "OurBranch":
				BTN_ModifyCompanyCode = "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "rhrorqjsghwlwjd") + "\" onclick=\"ModifyCompanyCode();\" />";
				BTN_wldurtnwjd = "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "wldurtnwjd") + "\" onclick=\"document.getElementById('spRegionCodeChange').style.visibility='visible';\" />";
				TB_ResponsibleStaff = "<input type=\"text\" id=\"TB_ResponsibleStaff\"  onfocus =\"this.select();\" style=\"width:85px; text-align:center; font-size:20px;\"/>";
				break;
			case "Customs":
				Loged1.Visible = false;
				Loged2.Visible = true;
				BTN_ModifyCompanyCode = "";
				BTN_wldurtnwjd = "";
				TB_ResponsibleStaff = "<input type=\"text\" id=\"TB_ResponsibleStaff\" disabled=\"disabled\" onfocus =\"this.select();\" onblur=\"CheckStaff(this.value);\" style=\"width:85px; text-align:center; font-size:20px;\"/>";
				break;
			default:
				TB_ResponsibleStaff = "<input type=\"hidden\" id=\"TB_ResponsibleStaff\"  />";

				break;
		}

	}
}