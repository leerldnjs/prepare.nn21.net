using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Components;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// JaemuIMG의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
// [System.Web.Script.Services.ScriptService]
public class JaemuIMG : System.Web.Services.WebService {
	private DBConn DB;
    public JaemuIMG () {

        //디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
        //InitializeComponent(); 
    }
	[WebMethod]
	public string TEST123() {
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"select Value from CompanyAdditionalInfomation
where Title=64";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder returnvalue = new StringBuilder();
		while (RS.Read()) {
			
			String[] up= RS["Value"].ToString().Split(Common.Splite11, StringSplitOptions.None);
			
			returnvalue.Append("<tr><td>" + up[0] + "</td><td>" + up[1] + "</td></tr>");
		}
		RS.Dispose();
		DB.DBCon.Close();
		
		return @"<table>"+ returnvalue + @"</table>";
	}
	[WebMethod]
	public string TEST() {
        DB = new DBConn();
        DB.SqlCmd.CommandText = @"
select c.companypk,companycode,CompanyName,hscode,Description from company c
left join (SELECT [CompanyPk],REPLACE(REPLACE([HSCode],'.',''),'-','') as [HSCode] ,Description     
  FROM [INTL2010].[dbo].[ClearanceItemCodeKOR]
  where hscode is not null and REPLACE(REPLACE([HSCode],'.',''),'-','') in (SELECT [HSCode]
  FROM [INTL2010].[dbo].[Lib_HSCode]
  where Lib_HSCodePk in (SELECT Lib_HSCodePk FROM [INTL2010].[dbo].[Lib_HSCode_TariffRate]   
where Lib_HSCodePk not in (
SELECT Lib_HSCodePk
  FROM [INTL2010].[dbo].[Lib_HSCode_TariffRate]    
 where kind='' and TariffRate0='0'
 union all
  select Lib_HSCodePk from [INTL2010].[dbo].[Lib_HSCode_TariffRate]    
  where Kind='c' and TariffRate0='0')
  and UseLimit_Description='0'
  group by Lib_HSCodePk 
  ) group by HSCode)and Deleted is null
  group by companypk,HSCode,Description
  ) as d on c.companypk = d.CompanyPk
where c.companypk in (SELECT [CompanyPk]
  FROM [INTL2010].[dbo].[ClearanceItemCodeKOR]
  where hscode is not null and REPLACE(REPLACE([HSCode],'.',''),'-','') in (SELECT [HSCode]
  FROM [INTL2010].[dbo].[Lib_HSCode]
  where Lib_HSCodePk in (SELECT Lib_HSCodePk FROM [INTL2010].[dbo].[Lib_HSCode_TariffRate]   
where Lib_HSCodePk not in (
SELECT Lib_HSCodePk
  FROM [INTL2010].[dbo].[Lib_HSCode_TariffRate]    
 where kind='' and TariffRate0='0'
 union all
  select Lib_HSCodePk from [INTL2010].[dbo].[Lib_HSCode_TariffRate]    
  where Kind='c' and TariffRate0='0')
  and UseLimit_Description='0'
  group by Lib_HSCodePk 
  ) group by HSCode)and Deleted is null
  group by companypk
  )order by companypk, HSCode ASC 

";
        DB.DBCon.Open();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        List<string[]> baseData = new List<string[]>();
        List<StringBuilder> Description = new List<StringBuilder>();
        string PkHSCode = "";
        while (RS.Read())
        {
            if (PkHSCode != RS["companypk"] + "" + RS["hscode"])
            {
                PkHSCode = RS["companypk"] + "" + RS["hscode"];
                baseData.Add(new string[] { "" + RS["companypk"], RS["companycode"] + "", RS["companyname"] + "", RS["hscode"] + "" });
                Description.Add(new StringBuilder().Append("" + RS["Description"]));
            }
            else
            {
                Description[Description.Count - 1].Append(", " + RS["Description"]);
            }
        }
        RS.Dispose();
        DB.DBCon.Close();
        StringBuilder ReturnValue = new StringBuilder();
        for (var i = 0; i < Description.Count; i++) {
            ReturnValue.Append("<tr><td>" + baseData[i][0] + "</td><td>" + baseData[i][1] + "</td><td>" + baseData[i][2] + "</td><td>" + baseData[i][3] + "</td><td>" + Description[i] + "</td></tr>");
        }
        return "<table>" + ReturnValue + "</table>";
	}
	[WebMethod]
	public List<string[]> LoadTariff(string BLList)
	{
		DB = new DBConn();
		List<string[]> ReturnValue = new List<string[]>();

		DB.SqlCmd.CommandText = @"
SELECT CD.CommercialDocumentHeadPk, CD.BLNo
	, TariffG.Value AS Guanse, TariffB.Value AS Bugase, TariffP.Value AS fee 
	, R.DocumentStepCL
FROM CommercialDocument AS CD 
	left join (
		SELECT CommercialDocumentHeadPk, Title, MonetaryUnitCL, [Value]
		FROM CommercialDocumentTariff 
		WHERE Title='관세'
	) AS TariffG ON CD.CommercialDocumentHeadPk=TariffG.CommercialDocumentHeadPk 
	left join (
		SELECT CommercialDocumentHeadPk, Title, MonetaryUnitCL, [Value]
		FROM CommercialDocumentTariff 
		WHERE Title='부가세'
	) AS TariffB ON CD.CommercialDocumentHeadPk=TariffB.CommercialDocumentHeadPk 
	left join (
		SELECT CommercialDocumentHeadPk, Title, MonetaryUnitCL, [Value]
		FROM CommercialDocumentTariff 
		WHERE Title='관세사비'
	) AS TariffP ON CD.CommercialDocumentHeadPk=TariffP.CommercialDocumentHeadPk 
	left join CommerdialConnectionWithRequest AS CCWR ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk
	left join RequestForm AS R ON CCWR.RequestFormPk=R.RequestFormPk
WHERE CD.BLNo in (" + BLList + ");";
		
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read())
		{
			ReturnValue.Add(new string[] { RS[0] + "", RS[1] + "", RS[2] + "", RS[3] + "", RS[4] + "", RS[5] + "" });
		}
		RS.Dispose();

		//return new List<string[]>();
		return ReturnValue;
	}
	[WebMethod]
	public String ExecNonQuery(string Query)
	{
		DB = new DBConn();
		DB.SqlCmd.CommandText = Query;
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
}