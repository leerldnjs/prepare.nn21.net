using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Dialog_CollectPayment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
    }
}