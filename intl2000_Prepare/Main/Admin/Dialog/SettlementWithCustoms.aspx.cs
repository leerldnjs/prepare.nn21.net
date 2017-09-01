using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Dialog_SettlementWithCustoms : System.Web.UI.Page
{
	protected String BLNo;
	protected String AccountId;
	protected void Page_Load(object sender, EventArgs e)
	{
		BLNo = Request.Params["BLNo"] + "";
		AccountId = Request.Params["AccountId"] + "";
	}
}