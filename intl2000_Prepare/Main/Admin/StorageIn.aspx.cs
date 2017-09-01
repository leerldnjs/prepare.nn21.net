using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_StorageIn : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected StringBuilder PrepareTransport;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] == null) {
			Response.Redirect("../Default.aspx");
		}
		MEMBERINFO = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
	}
}