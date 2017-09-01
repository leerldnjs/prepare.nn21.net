using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text;
using Components;
using System.Data.SqlClient;

/// <summary>
/// EmailSend의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class EmailSend : System.Web.Services.WebService {

	public EmailSend()
	{
		//디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
		//InitializeComponent(); 
	}

	[WebMethod]
	public String SendMailFromLocalhost()
	{
		SmtpClient Sender = new SmtpClient();
		MailMessage Mail = new MailMessage();
		Mail.From = new MailAddress("korea@nn21.com");
		Mail.To.Add("inlo12@daum.net");
		Mail.Subject = "안녕";
		Mail.Body = "hello";
		Mail.SubjectEncoding = System.Text.Encoding.Default;
		Mail.BodyEncoding = System.Text.Encoding.Default;
		try
		{
			Sender.Send(Mail);
			return "SendOK";
		}
		catch (Exception ex)
		{
			return "Fail : " + ex.Message;
		}
	}
	[WebMethod]
	public String SendSMSKorea()
	{
		/*
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT  replace([Mobile], '-', '') FROM [INTL2010].[dbo].[Account_] WHERE isnull([Mobile], '0')<>'0' and (left([Mobile], 3)='010' or left([Mobile], 3)='011' or left([Mobile], 3)='016' or left([Mobile], 3)='017' or left([Mobile], 3)='018' or left([Mobile], 3)='019' or left([Mobile], 3)='070' ) group by replace([Mobile], '-', '');";

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read())
		{
			new Email().SendMobileMsgKorea("01197070494", RS[0] + "", "\n\n.*\"'\"*. \n" +
				":^ㅡ^ :\n" +
				"\"*...*' \n" +
				"/＼   /＼\n" +
				"♣♣＼ ♣＼♣♣\n" +
				"둥근보름달보며 소원하는일 꼭 성취하시고 건강,웃음,행복이 가득한 추석명절 보내시기를 기원합니다. 아이엘국제물류주식회사 직원일동");
		}
		RS.Dispose();
		DB.DBCon.Close();
		return "1";
		 */

		/*
				DBConn DB = new DBConn();
				//DB.SqlCmd.CommandText = "SELECT  replace([Mobile], '-', '') FROM [INTL2010].[dbo].[Account_] WHERE isnull([Mobile], '0')<>'0' and (left([Mobile], 3)='010' or left([Mobile], 3)='011' or left([Mobile], 3)='016' or left([Mobile], 3)='017' or left([Mobile], 3)='018' or left([Mobile], 3)='019' or left([Mobile], 3)='070' ) group by replace([Mobile], '-', '');";
				DB.SqlCmd.CommandText = @"
		SELECT  replace([Mobile], '-', '') 
		FROM [INTL2010].[dbo].[Account_] WHERE isnull([Mobile], '0')<>'0' and (left([Mobile], 3)='011' or left([Mobile], 3)='016' or left([Mobile], 3)='017' or left([Mobile], 3)='018' or left([Mobile], 3)='019' or left([Mobile], 3)='070') 
		group by replace([Mobile], '-', '');";
				DB.DBCon.Open();
				SqlDataReader RS = DB.SqlCmd.ExecuteReader();
				while (RS.Read()) {
					new Email().SendMobileMsgKorea("0327728481", RS[0] + "", "한해동안 당사를 애용해 주신 고객님들께 깊은 감사의 인사를 올립니다. \n" +
					   "2012년 임진년에는 새로운 희망과 행복이 가득하시고, 사업번창을 기원합니다.\n" +
					  "새해 복 많이 받으세요.	\n" +
					  "- 아이엘국제물류 임직원일동");
				}
				RS.Dispose();
				DB.DBCon.Close();
			*/

		DBConn DB = new DBConn();
		//DB.SqlCmd.CommandText = "SELECT  replace([Mobile], '-', '') FROM [INTL2010].[dbo].[Account_] WHERE isnull([Mobile], '0')<>'0' and (left([Mobile], 3)='010' or left([Mobile], 3)='011' or left([Mobile], 3)='016' or left([Mobile], 3)='017' or left([Mobile], 3)='018' or left([Mobile], 3)='019' or left([Mobile], 3)='070' ) group by replace([Mobile], '-', '');";
		/*
		DB.SqlCmd.CommandText = @"
		SELECT replace(replace([Mobile], '-', ''), ' ', '') 
  FROM [INTL2010].[dbo].[Account_] 
  WHERE CompanyPk in (SELECT [ConsigneePk]
  FROM [INTL2010].[dbo].[RequestForm] 
  WHERE ArrivalDate > '2016' 
  GROUP BY [ConsigneePk]) 
  and isnull([Mobile], '0')<>'0' and (left([Mobile], 3)='011' or left([Mobile], 3)='016' or left([Mobile], 3)='017' or left([Mobile], 3)='018' or left([Mobile], 3)='010' or left([Mobile], 3)='070') 
  GROUP BY [Mobile]; ";
  */
		DB.SqlCmd.CommandText = @"SELECT replace(replace([Mobile], '-', ''), ' ', '') 
  FROM [INTL2010].[dbo].[Account_] 
  WHERE CompanyPk in (SELECT [ConsigneePk]
  FROM [INTL2010].[dbo].[RequestForm] 
  WHERE ArrivalDate > '2016' 
  AND DocumentRequestCL LIKE '%32%'
  GROUP BY [ConsigneePk]) 
  and isnull([Mobile], '0')<>'0' and (left([Mobile], 3)='011' or left([Mobile], 3)='016' or left([Mobile], 3)='017' or left([Mobile], 3)='018' or left([Mobile], 3)='010' or left([Mobile], 3)='070') 
  GROUP BY [Mobile]";

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			new Email().SendMobileMsgKorea("0327728481", RS[0] + "", "새해 복 많이 받으세요.\n\n2017년 희망 가득한 정유년 새해에는 뜻하신바 모든 일들이 성취되길 기원하며,\n모든 가정에 건강과 행복이 가득한 설날 명절 보내시기 바랍니다.\n\n(주)국제종합물류\n대표 이재구\n\nwww.nn21.com");
		}
		RS.Dispose();
		DB.DBCon.Close();




		return "2";		
	}
}
