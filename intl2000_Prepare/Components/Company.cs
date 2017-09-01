using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
	public class Company
	{
		public List<string> CompanyView(string Pk, out string RegionCode, out string CompanyName, out string PresidentName, out string CompanyNo, out string TEL, out string FAX, out string Email, out string CompanyAddress) {
			DBConn DB = new DBConn();
			List<string> Additional = new List<string>();
			DB.SqlCmd.CommandText = " SELECT CompanyName, RegionCode, CompanyAddress, CompanyTEL, CompanyFAX, PresidentName, PresidentEmail, CompanyNo FROM Company WHERE CompanyPk=" + Pk + ";";
			DB.DBCon.Open();
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			RS.Read();
			CompanyName = RS[0] + "";
			RegionCode = RS[1] + "";
			CompanyAddress = RS[2] + "";
			TEL = RS[3] + "";
			FAX = RS[4] + "";
			PresidentName = RS[5] + "";
			Email = RS[6] + "";
			CompanyNo = RS[7] + "";
			RS.Dispose();
			DB.SqlCmd.CommandText = "SELECT Title, Value FROM CompanyAdditionalInfomation where companypk=" + Pk + " order by Title ";
			RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) { Additional.Add(RS[0] + "^^^" + RS[1]); }
			RS.Dispose();
			DB.DBCon.Close();
			return Additional;
		}   //Member/MyCompanyView.aspx 
	}
}