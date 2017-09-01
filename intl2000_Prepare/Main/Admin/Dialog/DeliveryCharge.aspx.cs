using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_Dialog_DeliveryCharge : System.Web.UI.Page
{
	private DBConn DB;
	protected String Head;
	protected StringBuilder st_StandardPrice;
	protected String RequestFormPk;

	protected void Page_Load(object sender, EventArgs e)
	{
		RequestFormPk = Request.Params["S"];
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
	}
	
}