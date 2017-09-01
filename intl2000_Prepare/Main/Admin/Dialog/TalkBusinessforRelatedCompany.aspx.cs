using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Dialog_TalkBusinessforRelatedCompany : System.Web.UI.Page
{
   private String[] MEMBERINFO;
   protected string Html;
   protected string CompanyName;
    protected void Page_Load(object sender, EventArgs e)
    {
       if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../../Default.aspx"); }
       MEMBERINFO = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
       Response.Expires = 0;
       Response.Cache.SetNoStore();
       Response.AppendHeader("Pragma", "no-cache");
       Html = LoadRelatedCompany(Request.Params["S"]);
    }
    
    private string LoadRelatedCompany(string Companypk) {
       DBConn DB = new DBConn();
       DB.SqlCmd.CommandText = @"
SELECT 
	CR.CompanyRelationPk, CR.TargetCompanyPk, CR.GubunCL, CR.TargetCompanyNick 
	, C.GubunCL AS CompanyGubun, C.CompanyCode, C.CompanyName 
FROM 
	CompanyRelation AS CR 
	left join Company AS C ON CR.TargetCompanyPk=C.CompanyPk 
WHERE 
	MainCompanyPk=" + Companypk + @" 
ORDER BY 
	GubunCL ASC;";
       string rowformat="<input type=\"checkbox\" id=\"{0}\" value=\"{0}\"/> <label for=\"{0}\">{1}</label><br />";
       StringBuilder Htmlformat = new StringBuilder();
       DB.DBCon.Open();
       SqlDataReader RS = DB.SqlCmd.ExecuteReader();
       while (RS.Read()) {
          Htmlformat.Append(string.Format(rowformat, RS["TargetCompanyPk"].ToString(), RS["CompanyCode"].ToString()+" / "+RS["CompanyName"].ToString()));       
       }
       RS.Dispose();
       DB.DBCon.Close();

       return "<tr>"+
              "   <td  id =\"checktd\" >" + Htmlformat + "</td>" +
              "</tr>";
    
    }
}