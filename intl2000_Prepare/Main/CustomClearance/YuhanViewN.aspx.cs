using Components;
using Components.cClearance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomClearance_YuhanViewN : System.Web.UI.Page
{
	protected sYuhan_MasterN Current;
	protected string Saved_HouseBL;
	protected void Page_Load(object sender, EventArgs e) {
		string TBBHPk = Request.Params["S"] + "";

		YuhanN Y = new YuhanN();
		Current = Y.Load_YuhanMaster(TBBHPk);

		StringBuilder Html_HouseBL = new StringBuilder();
		for (var i = 0; i < Current.HouseBL.Count; i++) {
			HouseBLN each = Current.HouseBL[i];
			string ShipperAddress = each.ShipperAddress;

			string ConsigneeAddress = each.ConsigneeAddress;
			Html_HouseBL.Append("<p data-hsn='" + each.HSN + "' data-blno='" + each.BLNo + "' data-PackedCount='" + Utility.NumberFormat(each.PackedCount) + "' data-PackingUnit='" + each.PackingUnit + "' data-Weight='" + Utility.NumberFormat(each.Weight) + "' data-Volume='" + Utility.NumberFormat(each.Volume) + "'  data-ShipperName='" + (each.ShipperName + "" == "" ? "" : each.ShipperName.ToUpper()) + "' data-ShipperAddress='" + ShipperAddress.ToUpper() + "'  data-ConsigneeName='" + (each.ConsigneeName + "" == "" ? "" : each.ConsigneeName.ToUpper()) + "' data-ConsigneeAddress='" + ConsigneeAddress.ToUpper() + "' data-ConsigneeSaupjaNo='" + each.ConsigneeSaupjaNo + "' data-Description='" + (each.Description + "" == "" ? "" : each.Description.ToUpper()) + "' >&nbsp;</p>");
		}
		Saved_HouseBL = Html_HouseBL.ToString();
	}
}