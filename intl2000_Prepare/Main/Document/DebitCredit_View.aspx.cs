using Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Document_DebitCredit_View : System.Web.UI.Page
{
	protected sDebitCredit Current;
	protected string Html_InnerPrice;
	protected string PageType;

	protected void Page_Load(object sender, EventArgs e) {
		Document D = new Document();
		PageType = Request.Params["PageType"];

		Current = D.LoadDebitCredit(Request.Params["S"]);
		Html_InnerPrice = MakeHtml_DocumentBody(Current.InnerPrice, Current.TotalPrice);
	}
	private string MakeHtml_DocumentBody(List<sDocumentBody> InnerPrice, decimal TotalPrice) {
		StringBuilder ReturnValue = new StringBuilder();
		string TempBL = "";
		List<string> Summary_Title = new List<string>();
		List<int> Summary_Count = new List<int>();
		List<decimal> Summary_Amount = new List<decimal>();

		for (var i = 0; i < InnerPrice.Count; i++) {
			string currentBL = "";
			if (TempBL != InnerPrice[i].Value0) {
				TempBL = InnerPrice[i].Value0;
				currentBL = TempBL;
			}
			ReturnValue.AppendFormat(@"
				<tr>
					<td style=""border-left: solid 1px black; width:150px; padding-left:20px;height:23px;   "">{2}</td>
					<td colspan=""2"" style=""border-right: solid 1px black;"">{3}</td>
					<td style=""border-right: solid 1px black; width:120px; "">&nbsp;</td>
					<td style=""border-right: solid 1px black; width:120px; text-align:center; "">{4}</td>
				</tr>", i, InnerPrice[i].DocumentBodyPk, currentBL, InnerPrice[i].Value1, (InnerPrice[i].ValueDecimal0 == 0 ? "" : Math.Round(InnerPrice[i].ValueDecimal0, 2).ToString()));
			if (InnerPrice[i].ValueDecimal0 > 0) {
				bool isin = false;
				for (var j = 0; j < Summary_Title.Count; j++) {
					if (Summary_Title[j] == InnerPrice[i].Value1) {
						Summary_Count[j]++;
						Summary_Amount[j] += InnerPrice[i].ValueDecimal0;
						isin = true;
						break;
					}
				}
				if (!isin) {
					Summary_Title.Add(InnerPrice[i].Value1);
					Summary_Count.Add(1);
					Summary_Amount.Add(InnerPrice[i].ValueDecimal0);
				}
			}
		}
		ReturnValue.Append(@"
			<tr>
				<td colspan=""3"" style=""border-left: solid 1px black; border-top: solid 1px black; border-right: solid 1px black; border-bottom: solid 1px black; text-align: center; font-size: 15px; font-weight: bold;"">TOTAL</td>
				<td style=""border-right: solid 1px black; border-top: solid 1px black; border-bottom: solid 1px black; text-align: center; font-size: 15px; font-weight: bold;"">&nbsp;</td>
				<td style=""border-right: solid 1px black; border-top: solid 1px black; border-bottom: solid 1px black; text-align: center; font-size: 15px; font-weight: bold;"">" + Utility.NumberFormat(TotalPrice) + @"</td>
			</tr>
			<tr>
				<td colspan=""3"" style=""border-left: solid 1px black; border-right: solid 1px black; border-bottom: solid 1px black; text-align: center; font-size: 15px; font-weight: bold;"">SHARE</td>
				<td style=""border-right: solid 1px black; border-bottom: solid 1px black;"">&nbsp;</td>
				<td style=""border-right: solid 1px black; border-bottom: solid 1px black; text-align: center; font-size: 15px; font-weight: bold;"">YOUR</td>
			</tr>");
		for (var j = 0; j < Summary_Title.Count; j++) {
			ReturnValue.AppendFormat(@"
				<tr>
					<td colspan=""2"" style=""border-left: solid 1px black; text-align:center; padding-left:20px;height:23px;   "">&nbsp;&nbsp;{0}</td>
					<td style=""border-right: solid 1px black; width:30px; text-align:right; padding-right:10px; "">{1}</td>
					<td style=""border-right: solid 1px black;"">&nbsp;</td>
					<td style=""border-right: solid 1px black; text-align:center;"">{2}</td>
				</tr>", Summary_Title[j], Summary_Count[j], Utility.NumberFormat(Summary_Amount[j]));
		}
		ReturnValue.Append(@"
			<tr>
				<td colspan=""3"" style=""border-left: solid 1px black; border-right: solid 1px black;"">&nbsp;</td>
				<td style=""border-right: solid 1px black;"">&nbsp;</td>
				<td style=""border-right: solid 1px black;"">&nbsp;</td>
			</tr>
			<tr>
				<td colspan=""3"" style=""border-left: solid 1px black; border-right: solid 1px black; border-bottom: solid 1px black; text-align: center; font-size: 15px; font-weight: bold;"">TOTAL</td>
				<td  style=""border-right: solid 1px black; border-bottom: solid 1px black;"">&nbsp;</td>
				<td  style=""border-right: solid 1px black; border-bottom: solid 1px black; text-align: center; font-size: 15px; font-weight: bold;"">" + Utility.NumberFormat(TotalPrice) + @"</td>
			</tr>
			<tr>
				<td colspan=""5"" style=""text-align: right; font-size:20px; padding-right:20px; font-weight: bold;"">CREDIT AMOUNT &nbsp;&nbsp;&nbsp; [ USD ]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Utility.NumberFormat(TotalPrice) + @"</td>
			</tr>

");

		return ReturnValue.ToString();
	}

}