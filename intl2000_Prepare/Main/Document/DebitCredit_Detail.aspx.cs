using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Document_DebitCredit_Detail : System.Web.UI.Page
{
	protected sDebitCredit Current;
	protected string Html_InnerPrice;
	private DBConn DB;

	protected void Page_Load(object sender, EventArgs e) {
		Current = new sDebitCredit();
		//Current = MakeDebitCredit("16866");
		Document D = new Document();
		Current = D.LoadDebitCredit(Request.Params["S"]);
		Html_InnerPrice = MakeHtml_DocumentBody(Current.InnerPrice);
	}
	private string MakeHtml_DocumentBody(List<sDocumentBody> InnerPrice) {
		StringBuilder ReturnValue = new StringBuilder();
		string TempBL = "";
		for (var i = 0; i < InnerPrice.Count; i++) {
			string currentBL = "";
			if (TempBL != InnerPrice[i].Value0) {
				TempBL = InnerPrice[i].Value0;
				currentBL = TempBL;
			}
			ReturnValue.AppendFormat(@"
				<tr>
					<td style=""width:150px; "">
						<input type=""hidden"" name=""InnerPrice_{0}_DocumentBodyPk"" id=""InnerPrice_{0}_DocumentBodyPk"" value='{1}' />
						<input type=""text"" class=""form-control text-center"" name=""InnerPrice_{0}_BLNo"" id=""InnerPrice_{0}_BLNo"" value=""{2}"" /></td>
					<td>
						<input type=""text"" class=""form-control"" name=""InnerPrice_{0}_Title"" id=""InnerPrice_{0}_Title"" value=""{3}"" /></td>
					<td style=""width:130px; "">&nbsp;</td>
					<td style=""width:130px; "">
						<input type=""text"" class=""form-control text-center"" name=""InnerPrice_{0}_Collect"" id=""InnerPrice_{0}_Collect"" value=""{4}"" /></td>
					<td style=""width:10px; padding-top:7px; "">
						<label style='color:red; cursor:pointer;' onclick=""DeleteRow('{0}');""  >X</label>
					</td>
				</tr>", i, InnerPrice[i].DocumentBodyPk, currentBL, InnerPrice[i].Value1, (InnerPrice[i].Value2 == "" ? "" : Math.Round(InnerPrice[i].ValueDecimal0, 2).ToString()));
		}
		return ReturnValue.ToString();
	}
}