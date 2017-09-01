using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Components
{
	public class asAdmin : DBConn
	{
		public asAdmin() {

		}
		public String Get_SelectEmail(string RequestFormPk, string SorC) {
			SqlCmd.CommandText = "DECLARE @GubunPk int;" +
			"SELECT @GubunPk = [CUSTOMER_COMPANY_PK] FROM [dbo].[REQUESTFORMCALCULATE_HEAD] WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK]=" + RequestFormPk + " AND [REQUESTFORMCALCULATE_HEAD_PK] = " + SorC + ";" +
			"SELECT [Email] FROM Account_ WHERE CompanyPk=@GubunPk and isnull(Email, '')<>'' GROUP BY [Email];";

			StringBuilder option = new StringBuilder();
			DBCon.Open();
			SqlDataReader RS = SqlCmd.ExecuteReader();

			while (RS.Read()) {
				option.Append("<option value=\"" + RS[0] + "\">" + RS[0] + "</option>");
			}
			RS.Dispose();
			DBCon.Close();
			return "Email Send : <span id=\"PnEmail\" ><select id=\"TBEmail\">" + option + "</select><input type=\"button\" onclick=\"SendEmail();\" value=\"OK\" /></span>";
		}
		public String[] Get_SelectEmail(string CompanyPk) {
			SqlCmd.CommandText = "SELECT [Email] FROM Account_ WHERE CompanyPk=" + CompanyPk + " and isnull(Email, '')<>'' GROUP BY [Email];";

			DBCon.Open();
			SqlDataReader RS = SqlCmd.ExecuteReader();

			StringBuilder returnValue = new StringBuilder();
			Boolean isFirst = true;
			while (RS.Read()) {
				if (isFirst) {
					returnValue.Append(RS[0] + "");
					isFirst = false;
				} else {
					returnValue.Append("####" + RS[0]);
				}
			}
			RS.Dispose();
			DBCon.Close();

			return (returnValue + "").Split(new string[] { "####" }, StringSplitOptions.None);
		}
	}
}