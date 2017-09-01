using Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Document_DebitCredit_List : System.Web.UI.Page
{
	protected string Html_List;
	protected void Page_Load(object sender, EventArgs e) {
		Html_List = MakeHtml_List_DebitCredit();
	}
	private string MakeHtml_List_DebitCredit() {
		string query = @"
SELECT 
	T.[TransferId]
      ,T.[CompanyId_From]
      ,T.[CompanyId_To]
      ,T.[MonetaryUnit]
      ,T.[Amount]
      ,T.[Status] AS StatusTransfer
      ,T.[Date_Send]
	, D.[DocumentPk]
      ,D.[TypePk]
      ,D.[Status] AS StatusDocument
      ,D.[Value0]
      ,D.[Value1]
      ,D.[Value2]
      ,D.[Value3]
      ,D.[Value4]
      ,D.[Value5]
      ,D.[Value6]
      ,D.[Value7]
      ,D.[Value8]
      ,D.[Value9]
      ,D.[Value10]
      ,D.[Value11]
      ,D.[Value12]
      ,D.[Value13]
      ,D.[Value14]
      ,D.[Value15]
      ,D.[Value16]
      ,D.[Value17]
      ,D.[ValueInt0]
      ,D.[ValueDecimal0]
      ,D.[ParentsType]
      ,D.[ParentsId]
	  , TBBH.[TRANSPORT_WAY]
  FROM [INTL2010].[dbo].[Document] AS D 
	left join [INTL2010].[dbo].[Transfer] AS T ON D.ParentsId=T.TransferId 
	left join [INTL2010].[dbo].[TRANSPORT_HEAD] AS TBBH ON TBBH.[TRANSPORT_PK]=D.TypePk
	WHERE  D.[Type]='DebitCredit'
	AND ISNULL(T.Date_Send, '3015/09/26') >= '2017/01/01'
  	ORDER BY  ISNULL(T.Date_Send, '3015/09/26') DESC, T.TransferId ASC, [Value15] DESC, D.[DocumentPk] ASC;
";
		DataTable RS = Utility.SelectAnyQuery(query);

		StringBuilder ReturnValue = new StringBuilder();

		string TransferId = "";
		for (var i = 0; i < RS.Rows.Count; i++) {
			if (TransferId != RS.Rows[i]["TransferId"] + "") {
				TransferId = RS.Rows[i]["TransferId"] + "";
				ReturnValue.Append(@"
								<tr id='Pn_EachRow" + i + @"' 
										data-amount='" + RS.Rows[i]["ValueDecimal0"].ToString() + @"' 
										data-documentpk='" + RS.Rows[i]["DocumentPk"].ToString() + @"' 
										style='background-color:#FFFACD; '
								>
									<td><strong>" + RS.Rows[i]["Date_Send"] + @"</strong></td>
									<td colspan='6'>Total Amount : " + Utility.NumberFormat(RS.Rows[i]["Amount"].ToString()) + @"&nbsp;&nbsp;
										<input type='button' class='btn btn-xs btn-success' value='요약 인쇄' onclick=""Goto('DebitCredit_First', '" + TransferId + @"');"" />
										<input type='button' class='btn btn-xs btn-success' value='열기' id='BTN_Transfer" + TransferId + @"' onclick=""Row_ShowHide('" + TransferId + @"');"" />
										<input type='button' class='btn btn-xs btn-danger' style='display:none;' id='BTN_CancelTransfer" + TransferId + @"' value='삭제' onclick=""Cancel_Transfer('" + TransferId + @"');"" />
									</td>
									<td>
										<input type='button' class='btn btn-xs btn-success' value='열기 debit' onclick=""OpenAll_Transfer('Debit','" + TransferId + @"');"" />
										<input type='button' class='btn btn-xs btn-success' value='열기 credit' onclick=""OpenAll_Transfer('Credit', '" + TransferId + @"');"" />
									</td>
								</tr>");
			}
			ReturnValue.Append(@"
								<tr id='Pn_EachRow" + i + @"' 
										data-amount='" + RS.Rows[i]["ValueDecimal0"].ToString() + @"' 
										data-documentpk='" + RS.Rows[i]["DocumentPk"].ToString() + @"' 
										data-typepk='" + RS.Rows[i]["TypePk"].ToString() + @"' 
										data-transferid='" + TransferId + @"'																				>
									<td>" + RS.Rows[i]["Value15"] + @"</td>
									<td>" + RS.Rows[i]["TRANSPORT_WAY"].ToString() + @"</td>
									<td>" + RS.Rows[i]["Value16"] + @"</td>
									<td>" + RS.Rows[i]["Value17"] + @"</td>
									<td>" + RS.Rows[i]["Value8"] + @"</td>
									<td>" + RS.Rows[i]["Value9"] + @"</td>
									<td " + (TransferId == "" ? @"onclick=""ChooseRow('Pn_EachRow" + i + @"')"" " : "") + @" style='cursor:pointer; text-align:right; padding-right:20px; '>$ " + Utility.NumberFormat(RS.Rows[i]["ValueDecimal0"].ToString(), "###,###,###,###,###,##0.00") + @"</td>
									<td>
										<input type='button' class='btn btn-xs btn-success' value='수정' onclick=""Goto('DebitCredit_Detail', '" + RS.Rows[i]["TypePk"] + @"');"" />
										<input type='button' class='btn btn-xs btn-primary' value='debit' onclick=""Goto('Debit_View', '" + RS.Rows[i]["TypePk"] + @"');"" />
										<input type='button' class='btn btn-xs btn-primary' value='credit' onclick=""Goto('Credit_View', '" + RS.Rows[i]["TypePk"] + @"');"" />
									</td>
								</tr>");
		}
		RS.Dispose();

		if (ReturnValue + "" != "") {
			return @"
						<table class=""table"">
							<thead>
								<tr>
									<th>issue date</th>
									<th>&nbsp;</th>
									<th>Departure</th>
									<th>Arrival</th>
									<th>Vessel</th>
									<th>ContainerNo</th>
									<th>Amount</th>
									<th>BTN</th>
								</tr>
							</thead><tbody id='Pn_EachRow'>" + ReturnValue.ToString() + "</tbody></table >";
		}
		return "";
	}
}