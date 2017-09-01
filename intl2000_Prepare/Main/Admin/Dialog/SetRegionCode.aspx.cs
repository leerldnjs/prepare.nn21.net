using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_Dialog_SetRegionCode : System.Web.UI.Page
{
    protected String RegionList;
    protected StringBuilder NewAreaOption;
    protected StringBuilder NewCountryOption;
    protected StringBuilder NewDetailOption;
    protected StringBuilder NewBranchOption;
    protected StringBuilder NewUseOption;
    private DBConn DB;
    protected void Page_Load(object sender, EventArgs e)
    {
        RegionList = LoadBoardCodeLib();
    }
    private string LoadBoardCodeLib()
    {
        StringBuilder ReturnValue = new StringBuilder();
        NewAreaOption = new StringBuilder();
        NewCountryOption = new StringBuilder();
        NewDetailOption = new StringBuilder();
        NewBranchOption = new StringBuilder();
        DB = new DBConn();

        DB.SqlCmd.CommandText =
@"SELECT R.REGIONCODEPK, 
         R.REGIONCODE, 
         SUBSTRING(R.REGIONCODE,1,3) COUNTRYCODE, 
         SUBSTRING(R.REGIONCODE,1,6) AREACODE, 
         SUBSTRING(R.REGIONCODE,1,9) DETAILCODE, 
         R.NAME, 
         R.NAMEE, 
         R.OURBRANCHCODE, 
         C.COMPANYNAME, 
         R.GUBUN
FROM     REGIONCODE R 
LEFT JOIN COMPANY C 
    ON R.OurBRANCHCODE = C.COMPANYPK 
ORDER  BY REGIONCODE;";

        DB.DBCon.Open();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        while (RS.Read())
        {
            string RegionCode = RS["RegionCode"] + "";
            string Name = RS["Name"] + "";
            string NameE = RS["NameE"] + "";
            string companyname = RS["Companyname"] + "";
			string BranchCode = RS["OURBRANCHCODE"] + "";
            string CountryCode = RS["CountryCode"] + ""; 
            string AreaCode = RS["AreaCode"] + "";
            string DetailCode = RS["DetailCode"] + "";
            string GUBUN = RS["GUBUN"] + "";
            string GUBUNTEXT = RS["GUBUN"] + "" == "1" ? "Y" : "N";

            //국가코드
            if (RegionCode.Length == 1)
            {
                if (ReturnValue + "" != "")
                {
                    ReturnValue.Append("</td></tr></table>");
                }
                ReturnValue.Append(string.Format("<table border=\"1\" align=\"center\"  cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#708090\"  style=\"border-collapse:collapse;width:550px;background-color:#FFFFFF; margin-bottom:10px;\" >" +
					"<tr><td bgcolor=\"#D1E3E5\" align=\"center\" style=\"height:35px; width:190px;\"><b>" +
                     RegionCode + " : " + Name + " / " + NameE + "</b></td><td bgcolor=\"#D1E3E5\" style=\"width:130px; padding-left:5px;\" align=\"center\">" + companyname + "</td>" +
                    "<td bgcolor=\"#D1E3E5\" style=\"width:30px; padding-left:5px;\" align=\"center\">" + GUBUNTEXT + "</td>" +
                    "</td><td bgcolor=\"#D1E3E5\" style=\"width:100px; padding-left:5px;\">" +
                    "<input type=\"button\" onclick=\"SetModify('CountryCode', '" + RegionCode + "', '" + Name + "', '" + NameE + "', '" + BranchCode + "', '" + GUBUN + "');\" value=\"수정\" />" +
                    "<input type=\"button\" onclick=\"DeleteRegion( '" + RegionCode + "');\" style=\"color:red;\" value=\"삭제\" />" +
                    "</td></tr>", Name, NameE, RegionCode));
                NewCountryOption.Append("<option value=\"" + CountryCode + "\">" + Name + "</option>");
            }
            //지역코드
            else if (RegionCode.Length == 4)
            {
				ReturnValue.Append("<tr><td bgcolor=\"#F0F5F6\"  align=\"left\"  style=\"height:30px; width:190px;\">" + RegionCode + " : " + Name + " / " + NameE + "</td>" +
                    "<td style=\"padding-left:5px; width:130px;\" align=\"center\">" + companyname + "</td>" +
                    "<td style=\"padding-left:5px; width:30px;\" align=\"center\">" + GUBUNTEXT + "</td>" +
                    "<td style=\"padding-left:5px; width:100px;\">" +
                    "<input type=\"button\" onclick=\"SetModify('AreaCode', '" + RegionCode + "', '" + Name + "', '" + NameE + "', '" + BranchCode + "', '" + GUBUN + "');\" value=\"수정\" />" +
                    "<input type=\"button\" onclick=\"DeleteRegion('" + RegionCode + "');\" style=\"color:red;\" value=\"삭제\" /></li>");
                NewAreaOption.Append("<option value=\"" + AreaCode + "\">" + Name + "</option>");
            }
            //디테일코드
            else 
            {
				ReturnValue.Append("<tr><td bgcolor=\"#F0F5F6\"  align=\"left\"  style=\"height:30px; width:190px;\">" + RegionCode + " : " + Name + " / " + NameE + "</td>" +
                    "<td style=\"padding-left:5px; width:130px;\" align=\"center\">" + companyname + "</td>" +
                    "<td style=\"padding-left:5px; width:30px;\" align=\"center\">" + GUBUNTEXT + "</td>" +
                    "<td style=\"padding-left:5px; width:100px;\">" +
                    "<input type=\"button\" onclick=\"SetModify('DetailCode', '" + RegionCode + "', '" + Name + "', '" + NameE + "', '" + BranchCode + "', '" + GUBUN + "');\" value=\"수정\" />" +
                    "<input type=\"button\" onclick=\"DeleteRegion('" + RegionCode + "');\" style=\"color:red;\" value=\"삭제\" /></li>");
                NewDetailOption.Append("<option value=\"" + DetailCode + "\">" + Name + "</option>");
            }
			//else 
			//{
			//	ReturnValue.Append("<tr><td bgcolor=\"#F0F5F6\"  align=\"left\"  style=\"height:30px; width:240px;\">" + RegionCode + " : " + Name + " / " + NameE + "</td>" +
			//		"<td style=\"padding-left:5px; width:130px;\" align=\"center\">" + companyname + "</td>" +
			//		"<td style=\"padding-left:5px; width:30px;\" align=\"center\">" + GUBUNTEXT + "</td>" +
			//		"<td style=\"padding-left:5px; width:100px;\">" +
			//		"<input type=\"button\" onclick=\"SetModify('NewDetailCode', '" + RegionCode + "', '" + Name + "', '" + NameE + "', '" + BranchCode + "', '" + GUBUN + "');\" value=\"수정\" />" +
			//		"<input type=\"button\" onclick=\"DeleteRegion('" + RegionCode + "');\" style=\"color:red;\" value=\"삭제\" /></li>");
			//	//NewDetailOption.Append("<option value=\"" + DetailCode + "\">" + Name + "</option>");
			//}
        }

        if (ReturnValue + "" != "")
        {
            ReturnValue.Append("</td></tr></table>");
        }

        RS.Dispose();
        
        DB.SqlCmd.CommandText =
@"select CompanyName,CompanyPk from Company where GubunCL=9;";
        
        SqlDataReader RS1 = DB.SqlCmd.ExecuteReader();
        while (RS1.Read())
        {
            string CompanyName = RS1["CompanyName"] + "";
            string CompanyPk = RS1["CompanyPk"] + "";
            NewBranchOption.Append("<option value=\"" + CompanyPk + "\">" + CompanyName + "</option>");
        }
        DB.DBCon.Close();
        return ReturnValue + "";
    }
}