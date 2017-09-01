using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Web;

namespace Components
{
	public class Email
	{
		private SmtpClient SMTP;
		public Email() {
			SMTP = new SmtpClient("smtp.cafe24.com", 587) {
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential("korea@nn21.com", "intl2017!"),
				Timeout = 20000
			};
		}
		public Email(string Where) {
			if (Where == "Local") {
				SMTP = new SmtpClient() {
					DeliveryMethod = SmtpDeliveryMethod.Network,
					Timeout = 20000
				};
			}
		}
		public String SendMobileMsgKorea(string From, string To, string Msg) {
			Encoding en = Encoding.Default;
			//To = "01031370410";
			int bytecount = en.GetByteCount(Msg);
			if (bytecount > 90) {
				SendLMSKorea(From, To, Msg);
			} else {
				SendSMSKorea(From, To, Msg);
			}
			return "1";
		}

		public string SendSMSKorea(string From, string To, string Msg) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.mymunja.co.kr/Remote/RemoteSms.html?remote_id=intl2000&remote_pass=972179&remote_num=1&remote_phone=" + To + "&remote_callback=0327728481&remote_msg=" + Msg);
			WebResponse response = request.GetResponse();
			string result = (((HttpWebResponse)response).StatusDescription);
			Stream dataStream = response.GetResponseStream();

			StreamReader reader = new StreamReader(dataStream);
			string responseFromServer = reader.ReadToEnd();
			result += responseFromServer;
			reader.Close();
			dataStream.Close();
			response.Close();
			return result;
		}
		public string SendLMSKorea(string From, string To, string Msg) {
			string url = "http://www.mymunja.co.kr/Remote/RemoteMms.html?remote_id=intl2000&remote_pass=972179&remote_num=1&remote_phone=" + To + "&remote_callback=0327728481&remote_msg=" + Msg;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			WebResponse response = request.GetResponse();
			string result = (((HttpWebResponse)response).StatusDescription);
			Stream dataStream = response.GetResponseStream();

			StreamReader reader = new StreamReader(dataStream);
			string responseFromServer = reader.ReadToEnd();
			result += responseFromServer;
			reader.Close();
			dataStream.Close();
			response.Close();
			return result;
		}


		public void SendMailByLocal(string ToEmailAddress, string ToEmailName, string Title, string Contents) {
			MailAddress From = new MailAddress("korea@nn21.com", "주식회사 아이엘");
			MailAddress To = new MailAddress(ToEmailAddress, ToEmailName);
			MailMessage Mail = new MailMessage(From, To) { Subject = Title, Body = Contents, IsBodyHtml = true };
			SMTP.Send(Mail);
		}

		public void SendMailByCafe24(string FromEmail, string FromName, string ToEmail, string ToName, string Title, string Body) {
			MailAddress From = new MailAddress(FromEmail, FromName);
			MailAddress To = new MailAddress(ToEmail, ToName);
			MailMessage MSG = new MailMessage(From, To) { Subject = Title, Body = Body, IsBodyHtml = true };
			SMTP.Send(MSG);
		}
		public void SendMailByCafe24_Document(string FromEmail, string FromName, string ToEmail, string ToName, string Title, string Body, string ServerFilePath = null) {
			MailAddress From = new MailAddress(FromEmail, FromName);
			MailAddress To = new MailAddress(ToEmail, ToName);
			MailMessage MSG = new MailMessage(From, To) { Subject = Title, Body = Body, IsBodyHtml = true };
			if (ServerFilePath + "" != "") {
				ServerFilePath = System.Web.HttpUtility.UrlDecode(ServerFilePath);
				Attachment Attached = new Attachment(ServerFilePath);
				MSG.Attachments.Add(Attached);
			}
			SMTP.Send(MSG);
		}
		public void SendMailByCafe24(string ToEmail, string ToName, string Title, string Body) {
			MailAddress From = new MailAddress("korea@nn21.com", "아이엘국제물류주식회사");
			MailAddress To = new MailAddress(ToEmail, ToName);
			MailMessage MSG = new MailMessage(From, To) { Subject = Title, Body = Body, IsBodyHtml = true };
			SMTP.Send(MSG);
		}
	}
}