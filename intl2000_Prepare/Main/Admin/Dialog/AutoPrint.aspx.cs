using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Dialog_AutoPrint : System.Web.UI.Page
{
	protected String gubun;
	protected String pk;
    protected void Page_Load(object sender, EventArgs e)
    {
		gubun = Request.Params["G"] + "";
		pk = Request.Params["S"] + "";
    }
}