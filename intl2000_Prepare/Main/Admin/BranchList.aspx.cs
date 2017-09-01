using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_BranchList : System.Web.UI.Page
{
    protected String BranchLIST;
    protected String ACCOUNTID;
    private String COMPANYPK;
    private DBConn DB;
    private Int32 PageNo;
    private Int32 PageLength;

    protected void Page_Load(object sender, EventArgs e)
    {
        DB = new DBConn();
        ACCOUNTID = Session["ID"] + "";
        COMPANYPK = Session["CompanyPk"] + "";

        PageLength = 20;
        PageNo = Request.Params["pageNo"] + "" == "" ? 1 : Int32.Parse(Request.Params["pageNo"]);

        SetBranchList(PageNo);
    }
    protected Boolean SetBranchList(int PageNo)
    {
        Int32 totalpagecount;
        DateTime Now = DateTime.Now;
        
        string M1 = DateTime.Now.AddMonths(-1).Date.ToString().Substring(2, 8).Replace("-", "");
        string M3 = DateTime.Now.AddMonths(-3).Date.ToString().Substring(2, 8).Replace("-", "");
        string M6 = DateTime.Now.AddMonths(-6).Date.ToString().Substring(2, 8).Replace("-", "");

        DB.DBCon.Open();
            
        totalpagecount = GetTotalPageCount();

        string TableBody =  "<table border='0' cellpadding='0' cellspacing='1' style=\"width:850px; padding-top:10px\" ><thead><tr height='30px'>" +
                                    "	<td class='THead1' style='width:100px;' >" + GetGlobalResourceObject("Member", "BranchCode") + "</td>" +
                                    "	<td class='THead1' style='width:100px;' >" + GetGlobalResourceObject("RequestForm", "Region") + "</td>" +
                                    "	<td class='THead1' style='width:250px;'>" + GetGlobalResourceObject("Member", "CompanyName") + "</td>" +
                                    "	<td class='THead1' >Memo</td>{0}" +
                                    "<tr height='10px'><td colspan='6' >&nbsp;</td></tr><TR Height='20px'><td colspan='6' style='background-color:#F5F5F5; text-align:center; padding:20px; '>{1}</TD></TR></Table>";

		string TableRow = "	<tr><td class='{0}'><a href=\"BranchInfo.aspx?M=View&S={1}\">{2}</a></td>" +
                            "	<td class='{0}' >{3}</td>" +
                            "	<td class='{0}' ><a href=\"BranchInfo.aspx?M=View&S={1}\">{4}</a></td>" +
                            "	<td class='{0}'>{5}</td></tr>";
                
        
        bool CheckCompareCompanyCode = false;

        DB.SqlCmd.CommandText = @"
SELECT C.CompanyPk	     
     , C.GubunCL 
     , C.CompanyCode	
     , C.CompanyName		 
     , C.Memo 
     , RC.Name
     , RC.OurBranchCode
FROM Company C
left join RegionCode as RC on RC.RegionCode=C.RegionCode 
WHERE GubunCL = 9;";

        StringBuilder ReturnValue = new StringBuilder();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();

        if (totalpagecount != 0)
        {
            for (int i = 1; i < PageNo; i++)
            {
                for (int j = 0; j < PageLength; j++)
                {
                    RS.Read();
                }
            }
        }
        string beforecompany = "";
        for (int i = 0; i < PageLength; i++)
        {
            if (RS.Read())
            {
                if (CheckCompareCompanyCode)
                {
                    if (beforecompany != RS[0] + "")
                    {
                        beforecompany = RS[0] + "";
                    }
                    else
                    {
                        continue;
                    }
                }
                string style = RS["GubunCL"] + "" == "0" ? "TBody1G" : "TBody1";

                string[] RowData = new string[] { 
					                                style, 
					                                RS["CompanyPk"]+"", 
					                                RS["CompanyCode"]+"", 
					                                RS["Name"] + "" == "" ? "&nbsp;" : RS["Name"]+"", 
					                                RS["CompanyName"] + "" == "" ? "&nbsp;" : RS["CompanyName"]+"", 
					                                RS["Memo"] + "" == "" ? "&nbsp;" : RS["Memo"]+"" 
                                                };
                ReturnValue.Append(string.Format(TableRow, RowData));
            }
            else
            {
                break;
            }
        }
        RS.Dispose();
        DB.DBCon.Close();

        BranchLIST = string.Format(TableBody,
                            ReturnValue + "",
                            new Common().SetPageListByNoBranch(PageLength, PageNo, totalpagecount, "BranchList.aspx"));
        return true;
    }
    private Int32 GetTotalPageCount()
    {   
        DB.SqlCmd.CommandText = "SELECT Count(*) FROM Company WHERE GubunCL = 9";
        Int32 totalpagecount = (int)DB.SqlCmd.ExecuteScalar();
        return totalpagecount;
    }
}