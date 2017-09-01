using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;

public partial class Default : System.Web.UI.Page
{
	public String name = string.Empty;
	protected void Btn_ChangeLanguage_Click(object sender, EventArgs e) {
		Session["Language"] = Request.Params["Tb_LanguageSetInfo"];
		switch (Session["Language"] + "") {
			case "en": Page.UICulture = "en"; break;
			case "ko": Page.UICulture = "ko"; break;
			case "zh": Page.UICulture = "zh-cn"; break;
		}
		Btn_MemberJoin.Text = GetGlobalResourceObject("Member", "MemberJoin") + "";
		Btn_Login.Text = GetGlobalResourceObject("Member", "Login") + "";
	}
	protected void Page_Load(object sender, EventArgs e) {

		Components.Common CC = new Components.Common();
		if (Request.Cookies["IL"] != null) {
			/*
			string id = Request.Cookies["IL"]["ID"];
			string pwd = Request.Cookies["IL"]["PWD"];
			*/
			string id = "";
			string pwd = "";
			try {
				id = CC.AESDecrypt256(Request.Cookies["IL"]["ID"]);
				pwd = CC.AESDecrypt256(Request.Cookies["IL"]["PWD"]);
			} catch (Exception) {
				Clear_Cookie();
				Response.Redirect("/");
			}
			LoginP(id, pwd, true);
		} else if (Session["MemberInfo"] != null) {
			if (Session["MemberInfo"].ToString().Length > 7) {
				if (Session["MemberInfo"].ToString().Substring(0, 7) == "Company") {
					//Response.Redirect("Member/Intro.aspx" + Request.Url.Query);
					Response.Redirect("/");
				} else {
					//Response.Redirect("Admin/Intro.aspx" + Request.Url.Query);
					Response.Redirect("/");

				}
			}
		}

		switch (Request["Language"]) {
			case "en": Page.UICulture = "en"; break;
			case "ko": Page.UICulture = "ko"; break;
			case "zh": Page.UICulture = "zh-cn"; break;
		}
		Btn_Login.Text = GetGlobalResourceObject("Member", "Login") + "";
		Btn_MemberJoin.Text = GetGlobalResourceObject("Member", "MemberJoin") + "";
		Page.SetFocus(TB_ID);

	}
	private string Clear_Cookie() {
		HttpCookie aCookie;
		string cookieName;
		int limit = Request.Cookies.Count;
		for (int i = 0; i < limit; i++) {
			cookieName = Request.Cookies[i].Name;
			aCookie = new HttpCookie(cookieName);
			aCookie.Expires = DateTime.Now.AddDays(-1);
			Response.Cookies.Add(aCookie);
		}
		Session["MemberInfo"] = null;
		Session["SubInfo"] = null;

		return "1";
	}
	private void LoginP(string AccountID, string Password, bool IsFromCookie) {
		string MemberInfo = string.Empty;
		string SubInfo = string.Empty;
		string CompanyPk = string.Empty;

		int result = Login_(AccountID, Password, ref MemberInfo, ref SubInfo, ref CompanyPk);
		string alertmsg = "";
		if (result < 1) {
			Clear_Cookie();
		}
		switch (result) {
			case 0:
				alertmsg = GetGlobalResourceObject("Alert", "NoAccount") + "";
				break;
			case -1:
				alertmsg = GetGlobalResourceObject("Alert", "WrongPWD") + "";
				break;
			case -2:
				alertmsg = GetGlobalResourceObject("Alert", "NoAccount") + "";
				break;
			case 70:
				alertmsg = "Admin/RequestList.aspx?G=5051";
				break;
			case 90:
				alertmsg = "Member/Intro.aspx";
				break;
			case 92:
				alertmsg = "Logistics/StorageOut_Logistics.aspx";
				break;
			case 93:
				alertmsg = "Board/BoardMain.aspx";
				break;
			case 94:
				alertmsg = "Admin/StorageOutForCustoms.aspx";
				break;
		}

		if (result < 1) {
			Clear_Cookie();
			Response.Write("<script type='text/javascript'>alert('" + alertmsg + "');location.href = 'default.aspx';	</script>");
		} else {
			/////
			//new MemberP().Log(Request.ServerVariables["REMOTE_ADDR"], AccountID);
			////지워서 올려요

			if (!IsFromCookie || CheckBox1.Checked) {
				Components.Common CC = new Components.Common();
				HttpCookie ILCookie = new HttpCookie("IL");
				//ILCookie["ID"] = AccountID;
				//ILCookie["PWD"] = Password;

				ILCookie["ID"] = CC.AESEncrypt256(AccountID);
				ILCookie["PWD"] = CC.AESEncrypt256(Password);

				ILCookie.Expires = DateTime.Now.AddDays(100);
				Response.Cookies.Add(ILCookie);
			}

			Session["MemberInfo"] = MemberInfo;
			Session["Type"] = MemberInfo.Substring(0, MemberInfo.IndexOf("!"));
			Session["SubInfo"] = SubInfo;
			Session["ID"] = AccountID;
			Session["CompanyPk"] = CompanyPk;
			Response.Redirect(alertmsg);
		}
	}

	public Int32 Login_(string id, string pwd, ref string memberInfo, ref string subInfo, ref string companypk) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT 
	A.AccountPk, A.Password, A.GubunCL, A.CompanyPk, A.Duties, A.Name 
	, C.CompanyCode, CompanyName 
FROM Account_ AS A 
	left join Company AS C ON C.CompanyPk=A.CompanyPk  	
 WHERE A.AccountID='" + id + "';";
		DB.DBCon.Open();
		SqlDataReader result = DB.SqlCmd.ExecuteReader();
		result.Read();
		companypk = "";
		if (result.HasRows == false) {
			result.Dispose();
			DB.DBCon.Close();
			return 0;       //아이디 없음
		} else if (result["CompanyPk"] + "" == "") {
			result.Dispose();
			DB.DBCon.Close();
			return -2;      //에러 CompanyPk가 제대로 설정되지 않음...
		} else if (pwd != result["Password"].ToString().Trim()) {
			result.Dispose();
			DB.DBCon.Close();
			return -1;      // 비밀번호 다름
		} else {
			int ReturnValue = 0;
			string GubunCL = result["GubunCL"] + "";
			string Gubun = string.Empty;
			switch (GubunCL) {
				case "70":          //지점
					Gubun = "ShippingBranch";
					ReturnValue = 70;
					break;
				case "90":
					Gubun = "Company";
					ReturnValue = 90;
					break;
				case "91":
					Gubun = "Company";
					ReturnValue = 90;
					break;
				case "92":
					Gubun = "OurBranchOut";
					ReturnValue = 92;
					break;
				case "93":
					Gubun = "OurBranch";
					ReturnValue = 93;
					break;
				case "94":
					Gubun = "Customs";
					ReturnValue = 94;
					break;
			}
			string GubunPk = result["CompanyPk"] + "";
			memberInfo = Gubun + "!" + GubunPk + "!" + id + "!" + result["Name"] + "!" + result["Duties"];
			subInfo = result["CompanyCode"] + "!" + result["CompanyName"];
			companypk = GubunPk;
			result.Dispose();
			DB.SqlCmd.CommandText = "UPDATE Account_ SET LastVisit=getdate() WHERE AccountID='" + id + @"'; 
				INSERT INTO [dbo].[AccountLog]([AccountId],[Code],[Value]) VALUES ('" + id + "' ,'Login','" + Request.UserHostAddress + "' );";
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return ReturnValue;
		}
	}//로그인 하는거


	protected void Btn_Login_Click(object sender, EventArgs e) {
		LoginP(TB_ID.Text.Trim(), TB_PWD.Text.Trim(), false);
	}
	protected void Btn_MemberJoin_Click(object sender, EventArgs e) {
		Response.Redirect("Member/CompanyJoinStep1.aspx" + Request.Url.Query);
	}
}