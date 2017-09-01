using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;

public partial class RequestForm_ItemModifyList : System.Web.UI.Page
{
	public string RequestFormPk;
	public string ItemModifyList;
	protected void Page_Load(object sender, EventArgs e)
	{
		ItemModifyList = new RequestP().ItemModifyList(Request.Params["S"],Request.Params["CL"]);

		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
	}
}
