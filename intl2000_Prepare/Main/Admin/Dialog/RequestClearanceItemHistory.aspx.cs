using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_Dialog_RequestClearanceItemHistory : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		Response.Write(new RequestP().HTMLClearanceItemHistory(Request.Params["S"] + "", Request.Params["C"] + "", Int32.Parse(Request.Params["L"] + "")));
	}
}