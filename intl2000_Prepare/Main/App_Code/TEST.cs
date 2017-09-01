using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;

/// <summary>
/// SelectTrans의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class TEST : System.Web.Services.WebService
{
    private DBConn DB;
    public TEST()
    {
        //디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string IdDupCheck(string MemberId)
    {
        DB = new DBConn();      
        DB.SqlCmd.CommandText = "select count(*) from Account_ where Accountid='" + MemberId + "';";
        DB.DBCon.Open();
        string Check = DB.SqlCmd.ExecuteScalar() + "";
        DB.DBCon.Close();
        return Check;
    }
}
