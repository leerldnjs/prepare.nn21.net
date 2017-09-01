using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;


public partial class UploadedFiles_FileList : System.Web.UI.Page
{
   protected String[] MemberInfo;
   protected String FileListHTML;
   protected String G;
   protected String T;
   protected String V;
   protected String CompanyPK;
   protected String HtmlButton;
   protected void Page_Load(object sender, EventArgs e)
   {
      try
      {
         MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
      }
      catch (Exception)
      {
         Response.Redirect("../Default.aspx");
      }

      if (MemberInfo[0] == "OurBranch")
      {
         HtmlButton = "<input type=\"text\" id=\"TBSerchValue\" />&nbsp;<input type=\"button\" value=\"serch\" onclick=\"BTN_Serch_Click();\" />";
         G = Request.Params["G"] + "" == "" ? "4" : Request.Params["G"] + "";
         T = Request.Params["T"] + "";
         V = Request.Params["V"] + "";
         FileListHTML = LoadFileList(G, T, V,
            MemberInfo[2],
            Request.Params["PageNo"] + "" == "" ? 1 : Int32.Parse(Request.Params["PageNo"] + ""));
      }
      else
      {
         LogedWithoutRecentRequest1.Visible = false;
         Loged1.Visible = true;
         HtmlButton = "";

         G = Request.Params["G"] + "" == "" ? "4" : Request.Params["G"] + "";
         T = "CompanyPK";
         V = MemberInfo[1];
         FileListHTML = LoadFileList(G, T, V,
            MemberInfo[2],
            Request.Params["PageNo"] + "" == "" ? 1 : Int32.Parse(Request.Params["PageNo"] + ""));
      }
   }
   private String LoadFileList(string GubunCL, string SerchType, string SerchValue, string AccountID, Int32 PageNo)
   {
      string ROW = "<tr height=\"20px; \">" +
         "<td class=\"{0}\" ><a href=\"../Admin/CompanyInfo.aspx?M=View&S={9}\">{4}</a></td>" +
         "<td class=\"{0}\" ><a href=\"../Admin/CompanyInfo.aspx?M=View&S={11}\">{10}</a></td>" +
         "<td class=\"{0}\" >{6}</a></td>" +
         "<td class=\"{0}\">{7}</td>" +
         "<td class=\"{0}\" style=\"text-align:center;\">{8}</td>" +
         "<td class=\"{0}\" style=\"text-align:center;\"><input type=\"button\" value=\"C\" onclick=\"ImgCommentAdd('{1}')\" />{12}" +
         "</td>" +
         "<td class=\"{0}\" style=\"text-align:center;\"><img onclick=\"ImgDown('{1}')\" style=\"width:25px; height:25px; \" src=\"../Images/Download.png\" /></td></tr>";

      string ROWComment = "<tr height=\"20px; \">" +
         "<td class=\"{0}\" colspan='1' >&nbsp;</td>" +
         "<td class=\"{0}\" colspan='6' style=\"text-align:left;\" >{1}</td>" +
         "<td class=\"{0}\" colspan='1' style=\"text-align:left;\" >{2}</td></tr>";

      string TABLE = "<table id=\"ImageTable\" border='0' cellpadding='0' cellspacing='0' style=\"width:850px;\" ><thead><tr height='30px'>" +
                           "<td class='THead1' style='width:200px;' >Company Name</td>" +
                           "<td class='THead1' style='width:200px;' >RelationCompany</td>" +
                           "<td class='THead1' style='width:200px;' >File Name</td>" +
                           "<td class='THead1' >ID</td>" +
                           "<td class='THead1' style='width:150px;'>Registerd</td>" +
                           "<td class='THead1' style='width:70px;'>&nbsp;</td>" +
                           "<td class='THead1' style='width:30px;'>&nbsp;</td>" +
                        "</tr></thead>{0}</table>";
      //asAdmin asA = new asAdmin();
      StringBuilder ReturnValue = new StringBuilder();
      DBConn DB = new DBConn();
      if (SerchValue == "")
      {
         DB.SqlCmd.CommandText = @"
SELECT  Count(*)
FROM 
	[File] AS F
WHERE 
	F.GubunCL=" + GubunCL + ";";
      }
      else
      {
         switch (SerchType)
         {
            case "CompanyCode":
               DB.SqlCmd.CommandText = @"
SELECT  Count(*)
FROM 
	[File] AS F
	left join Company AS C ON F.GubunPk=C.CompanyPk
WHERE 
	F.GubunCL=" + GubunCL + " and C.CompanyCode='" + SerchValue + "';";
               break;
            case "CompanyPK":
               DB.SqlCmd.CommandText = @"
SELECT  Count(*)
FROM 
	[File] AS F
	left join Company AS C ON F.GubunPk=C.CompanyPk
WHERE 
	F.GubunCL=" + GubunCL + " and C.CompanyPK='" + SerchValue + "';";
               break;
         }
      }
      DB.DBCon.Open();
      Int32 TotalCount = (int)DB.SqlCmd.ExecuteScalar();
      if (TotalCount == 0)
      {
         DB.DBCon.Close();
         return "";
      }
      Int32 PageLength = 40;

      if (SerchValue == "")
      {
         DB.SqlCmd.CommandText = @"
				SELECT F.FilePk, F.Title, F.GubunPk, F.PhysicalPath, F.FileName, F.AccountID, F.Registerd 
					, C.CompanyPk, C.CompanyCode, C.CompanyName 
					, FAI.Type 
					, FC.FileCommentPk, FC.FileAttachedPk, FC.AccountID AS AID, FC.Comment, isnull(FC.Registerd , F.Registerd) AS CommentRegisterd 
					, FC.AttachedFile,FC.[PhysicalPath] EmailPhysicalPath
,FAIR.[RelationPk]
,RC.CompanyPk RCPk,RC.CompanyCode RCCode,RC.CompanyName RCName
				FROM [File] AS F 
					left join Company AS C ON F.GubunPk=C.CompanyPk 
					left join ( 
						SELECT FilePk, [Type] FROM FileAdditionalInfo WHERE [Type]='0' and AccountID='" + AccountID + @"'
						) AS FAI ON F.FilePk=FAI.FilePk 
left join ( 
						SELECT FilePk, [RelationPk] FROM FileAdditionalInfo WHERE [Type]='4' 
						) AS FAIR ON F.FilePk=FAIR.FilePk 
left join Company as RC on FAIR.[RelationPk]=RC.CompanyPk

					left join (
						SELECT FC.FileCommentPk, FC.FilePk, FC.FileAttachedPk, FC.AccountID, FC.Comment, FC.Registerd ,F.PhysicalPath 
							, F.FileName AS AttachedFile
						FROM FileComment AS FC
							left join [File] AS F ON FC.FileAttachedPk=F.FilePk
						WHERE isnull(F.GubunCL, 30)=30
					) AS FC ON F.FilePk=FC.FilePk 
				WHERE F.GubunCL=" + GubunCL + " ORDER BY F.FilePk DESC,  F.Registerd DESC, FC.Registerd  ASC;";
      }
      else
      {
         switch (SerchType)
         {
            case "CompanyCode":
               DB.SqlCmd.CommandText = @"
SELECT F.FilePk, F.Title, F.GubunPk, F.PhysicalPath, F.FileName, F.AccountID, F.Registerd 
	, C.CompanyPk, C.CompanyCode, C.CompanyName 
	, FAI.Type 
	, FC.FileCommentPk, FC.FileAttachedPk, FC.AccountID AS AID, FC.Comment, isnull(FC.Registerd , F.Registerd) AS CommentRegisterd 
	, FC.AttachedFile,FC.[PhysicalPath] EmailPhysicalPath
,FAIR.[RelationPk]
,RC.CompanyPk RCPk,RC.CompanyCode RCCode,RC.CompanyName RCName
FROM [File] AS F 
	left join Company AS C ON F.GubunPk=C.CompanyPk 
	left join ( 
		SELECT FilePk, [Type] FROM FileAdditionalInfo WHERE [Type]='0' and AccountID='" + AccountID + @"'
		) AS FAI ON F.FilePk=FAI.FilePk 
left join ( 
						SELECT FilePk, [RelationPk] FROM FileAdditionalInfo WHERE [Type]='4' 
						) AS FAIR ON F.FilePk=FAIR.FilePk 
left join Company as RC on FAIR.[RelationPk]=RC.CompanyPk
	left join (
		SELECT FC.FileCommentPk, FC.FilePk, FC.FileAttachedPk, FC.AccountID, FC.Comment, FC.Registerd ,F.PhysicalPath 
			, F.FileName AS AttachedFile
		FROM FileComment AS FC
			left join [File] AS F ON FC.FileAttachedPk=F.FilePk
		WHERE isnull(F.GubunCL, 30)=30
	) AS FC ON F.FilePk=FC.FilePk 
WHERE F.GubunCL=" + GubunCL + " and C.CompanyCode='" + SerchValue + "' ORDER BY F.FilePk DESC,  F.Registerd DESC, FC.Registerd  ASC;";
               break;
            case "CompanyPK":
               DB.SqlCmd.CommandText = @"
SELECT F.FilePk, F.Title, F.GubunPk, F.PhysicalPath, F.FileName, F.AccountID, F.Registerd 
	, C.CompanyPk, C.CompanyCode, C.CompanyName 
	, FAI.Type 
	, FC.FileCommentPk, FC.FileAttachedPk, FC.AccountID AS AID, FC.Comment, isnull(FC.Registerd , F.Registerd) AS CommentRegisterd 
	, FC.AttachedFile,FC.[PhysicalPath] EmailPhysicalPath
,FAIR.[RelationPk]
,RC.CompanyPk RCPk,RC.CompanyCode RCCode,RC.CompanyName RCName
FROM [File] AS F 
	left join Company AS C ON F.GubunPk=C.CompanyPk 
	left join ( 
		SELECT FilePk, [Type] FROM FileAdditionalInfo WHERE [Type]='0' and AccountID='" + AccountID + @"'
		) AS FAI ON F.FilePk=FAI.FilePk 
left join ( 
						SELECT FilePk, [RelationPk] FROM FileAdditionalInfo WHERE [Type]='4' 
						) AS FAIR ON F.FilePk=FAIR.FilePk 
left join Company as RC on FAIR.[RelationPk]=RC.CompanyPk

	left join (
		SELECT FC.FileCommentPk, FC.FilePk, FC.FileAttachedPk, FC.AccountID, FC.Comment, FC.Registerd ,F.PhysicalPath 
			, F.FileName AS AttachedFile
		FROM FileComment AS FC
			left join [File] AS F ON FC.FileAttachedPk=F.FilePk
left join Account_ A on FC.AccountID=A.AccountID
		WHERE isnull(F.GubunCL, 30)=30 and A.companyPk='" + SerchValue + @"'
	) AS FC ON F.FilePk=FC.FilePk 
WHERE F.GubunCL=" + GubunCL + " and C.CompanyPK='" + SerchValue + "' ORDER BY F.FilePk DESC,  F.Registerd DESC , FC.Registerd  ASC;";
               break;
         }
      }
      //Response.Write(DB.SqlCmd.CommandText);
      SqlDataReader RS = DB.SqlCmd.ExecuteReader();
      string FilePk = "";
      string FileCommentPk = "";
      string EmailPhysicalPath = "";
      string BTN_FileSend = "";
      int PageCounter = 0;

      if (PageNo == 1)
      {
         RS.Read();
      }
      else
      {
         while (RS.Read())
         {
            if (PageCounter > (PageNo - 1) * PageLength)
            {
               if (FilePk != RS["FilePk"] + "")
               {
                  break;
               }
            }

            if (FilePk != RS["FilePk"] + "")
            {
               FilePk = RS["FilePk"] + "";
               PageCounter++;
            }
         }
      }

      FilePk = RS["FilePk"] + "";
      PageCounter = 0;

      string[] RowData = new string[13];
      RowData[0] = RS["Type"] + "" == "" ? "TBody1G" : "TBody1";
      RowData[1] = RS["FilePk"] + "";
      RowData[2] = RS["Title"] + "" == "" ? "&nbsp;" : RS["Title"] + "";
      RowData[3] = RS["CompanyCode"] + "";
      DateTime registerd = DateTime.Parse(RS["Registerd"] + "");
      string newimg = "";
      if (DateTime.Now.AddDays(-1) < registerd)
      {
         newimg = " <img src=\"../Images/Board/new.gif\" align=\"absmiddle\">";
      }
      RowData[4] = RS["CompanyCode"] + "" + RS["CompanyName"] + " " + newimg;
      Byte[] filename = Encoding.Default.GetBytes(RS["PhysicalPath"] + "");
      RowData[5] = "./" + HttpUtility.UrlEncode(filename);
      RowData[6] = RS["FileName"] + "";
      RowData[7] = RS["AccountID"] + "";
      RowData[8] = (RS["Registerd"] + "").Substring(0, (RS["Registerd"] + "").Length - 3);
      RowData[9] = RS["CompanyPk"] + "";
      RowData[10] = RS["RCCode"].ToString() + RS["RCName"].ToString() == "" ? "&nbsp;" : RS["RCCode"] + "/" + RS["RCName"];
      RowData[11] = RS["RCPk"] + "";
      if (AccountID == RS["AccountID"] + "" || AccountID == "ilic00" || AccountID == "ilic01" || AccountID == "ilic30" || AccountID == "ilic55" || AccountID == "ilic66" || AccountID == "ilic77")
      {
         RowData[12] = "<input type=\"button\" value=\"D\" style=\"padding:0px; width:20px; color:red;\" onclick=\"ImgDelete('" + RS["FilePk"] + "')\" />";
      }
      else { RowData[12] = ""; }

      ReturnValue.Append(String.Format(ROW, RowData));

      if (RS["Comment"] + "" != "")
      {
         if (RS["FileAttachedPk"] + "" != "")
         {
            EmailPhysicalPath = Server.MapPath(RS["EmailPhysicalPath"] + "");
            EmailPhysicalPath = Server.UrlEncode(EmailPhysicalPath);
            BTN_FileSend = "<input type=\"Button\" value=\"Email\" onclick=\"SendMailByCafe24_Document('" + MemberInfo[1] + "','" + EmailPhysicalPath + "','" + RS["CompanyPk"] + "','" + RS["RCPk"] + "')\" />";
         }
         else
         {
            BTN_FileSend = "";
         }
         FileCommentPk = RS["FileCommentPk"] + "";

         string Date = "<span style=\"color:gray;\"> -" + ("" + RS["Registerd"]).Substring(0, ("" + RS["Registerd"]).Length - 3) + "</span>";

         ReturnValue.Append(string.Format(ROWComment,
            "TBody1",
            "<span style=\"color:blue;\" >" + RS["AID"] + "</span>&nbsp;" + RS["Comment"] + (RS["FileAttachedPk"] + "" == "" ? "" : "&nbsp;&nbsp;&nbsp;&nbsp;<img onclick=\"ImgDown('" + RS["FileAttachedPk"] + "" + "')\" style=\"width:15px; height:15px; \" src=\"../Images/Download.png\" /><span onclick=\"ImgDown('" + RS["FileAttachedPk"] + "" + "')\" style=\"color:orange; cursor:hand;\" >" + RS["AttachedFile"] + "</span>") + Date +
            (AccountID == RS["AID"] + "" ? "<span style=\"color:red; cursor:hand;\" onclick=\"CommentDelete('" + RS["FileCommentPk"] + "')\" >X</span>" : "") + "&nbsp;&nbsp;&nbsp;&nbsp;" + BTN_FileSend, "&nbsp;"));
      }

      while (RS.Read())
      {
         if (PageCounter > PageLength)
         {
            if (FilePk != RS["FilePk"] + "")
            {
               break;
            }
         }

         if (FilePk != RS["FilePk"] + "")
         {
            FilePk = RS["FilePk"] + "";
            PageCounter++;

            RowData = new string[13];
            RowData[0] = RS["Type"] + "" == "" ? "TBody1G" : "TBody1";
            RowData[1] = RS["FilePk"] + "";
            RowData[2] = RS["Title"] + "" == "" ? "&nbsp;" : RS["Title"] + "";
            RowData[3] = RS["CompanyCode"] + "";
            registerd = DateTime.Parse(RS["Registerd"] + "");
            newimg = "";
            if (DateTime.Now.AddHours(-12) < registerd)
            {
               newimg = " <img src=\"../Images/Board/new.gif\" align=\"absmiddle\">";
            }
            RowData[4] = RS["CompanyCode"] + "" + RS["CompanyName"] + " " + newimg;
            filename = Encoding.Default.GetBytes(RS["PhysicalPath"] + "");
            RowData[5] = "./" + HttpUtility.UrlEncode(filename);
            RowData[6] = RS["FileName"] + "";
            RowData[7] = RS["AccountID"] + "";
            RowData[8] = (RS["Registerd"] + "").Substring(0, (RS["Registerd"] + "").Length - 3);
            RowData[9] = RS["CompanyPk"] + "";
            RowData[10] = RS["RCCode"].ToString() + RS["RCName"].ToString() == "" ? "&nbsp;" : RS["RCCode"] + "/" + RS["RCName"];
            RowData[11] = RS["RCPk"] + "";
            if (AccountID == RS["AccountID"] + "" || AccountID == "ilic00" || AccountID == "ilic01" || AccountID == "ilic30" || AccountID == "ilic55" || AccountID == "ilic66" || AccountID == "ilic77")
            {
               RowData[12] = "<input type=\"button\" value=\"D\" style=\"padding:0px; width:20px; color:red;\" onclick=\"ImgDelete('" + RS["FilePk"] + "')\" />";
            }
            else { RowData[12] = ""; }
            ReturnValue.Append(String.Format(ROW, RowData));

            if (RS["Comment"] + "" != "")
            {
               if (RS["FileAttachedPk"] + "" != "")
               {
                  EmailPhysicalPath = Server.MapPath(RS["EmailPhysicalPath"] + "");
                  EmailPhysicalPath = Server.UrlEncode(EmailPhysicalPath);
                  BTN_FileSend = "<input type=\"Button\" value=\"Email\" onclick=\"SendMailByCafe24_Document('" + MemberInfo[1] + "','" + EmailPhysicalPath + "','" + RS["CompanyPk"] + "','" + RS["RCPk"] + "')\" />";
               }
               else
               {
                  BTN_FileSend = "";
               }
               FileCommentPk = RS["FileCommentPk"] + "";
               registerd = DateTime.Parse(RS["CommentRegisterd"] + "");
               newimg = "";
               if (DateTime.Now.AddHours(-12) < registerd)
               {
                  newimg = " <img src=\"../Images/Board/new.gif\" align=\"absmiddle\">";
               }
               string Date = "<span style=\"color:gray;\"> -" + ("" + RS["CommentRegisterd"]).Substring(0, ("" + RS["CommentRegisterd"]).Length - 3) + "</span>";
               ReturnValue.Append(string.Format(ROWComment,
                  "TBody1",
                  "<span style=\"color:blue;\" >" + RS["AID"] + "</span>&nbsp;" + RS["Comment"] + (RS["FileAttachedPk"] + "" == "" ? "" : "&nbsp;&nbsp;&nbsp;&nbsp;<img onclick=\"ImgDown('" + RS["FileAttachedPk"] + "" + "')\" style=\"width:15px; height:15px; \" src=\"../Images/Download.png\" /><span onclick=\"ImgDown('" + RS["FileAttachedPk"] + "" + "')\" style=\"color:orange; cursor:hand;\" >" + RS["AttachedFile"] + "</span>") + Date + newimg +
                  (AccountID == RS["AID"] + "" ? "<span style=\"color:red; cursor:hand;\" onclick=\"CommentDelete('" + RS["FileCommentPk"] + "')\" >X</span>" : "") + "&nbsp;&nbsp;&nbsp;&nbsp;" + BTN_FileSend, "&nbsp;"));
            }
         }
         else
         {
            if (FileCommentPk != RS["FileCommentPk"] + "")
            {
               if (RS["FileAttachedPk"] + "" != "")
               {
                  EmailPhysicalPath = Server.MapPath(RS["EmailPhysicalPath"] + "");
                  EmailPhysicalPath = Server.UrlEncode(EmailPhysicalPath);
                  BTN_FileSend = "<input type=\"Button\" value=\"Email\" onclick=\"SendMailByCafe24_Document('" + MemberInfo[1] + "','" + EmailPhysicalPath + "','" + RS["CompanyPk"] + "','" + RS["RCPk"] + "')\" />";
               }
               else
               {
                  BTN_FileSend = "";
               }
               FileCommentPk = RS["FileCommentPk"] + "";
               registerd = DateTime.Parse(RS["CommentRegisterd"] + "");
               newimg = "";
               if (DateTime.Now.AddHours(-12) < registerd)
               {
                  newimg = " <img src=\"../Images/Board/new.gif\" align=\"absmiddle\">";
               }

               string Date = "<span style=\"color:gray;\"> -" + ("" + RS["CommentRegisterd"]).Substring(0, ("" + RS["CommentRegisterd"]).Length - 3) + "</span>";
               ReturnValue.Append(string.Format(ROWComment,
               "TBody1",
               "<span style=\"color:blue;\" >" + RS["AID"] + "</span>&nbsp;" + RS["Comment"] + (RS["FileAttachedPk"] + "" == "" ? "" : "&nbsp;&nbsp;&nbsp;&nbsp;<img onclick=\"ImgDown('" + RS["FileAttachedPk"] + "" + "')\" style=\"width:15px; height:15px; \" src=\"../Images/Download.png\" /><span onclick=\"ImgDown('" + RS["FileAttachedPk"] + "" + "')\" style=\"color:orange; cursor:hand;\" >" + RS["AttachedFile"] + "</span>") + Date + newimg +
               (AccountID == RS["AID"] + "" ? "<span style=\"color:red; cursor:hand;\" onclick=\"CommentDelete('" + RS["FileCommentPk"] + "')\" >X</span>" : "") + "&nbsp;&nbsp;&nbsp;&nbsp;" + BTN_FileSend, ""));
            }
         }
      }

      RS.Dispose();

      DB.DBCon.Close();
      string PageBottom = "<TR Height='20px'><td colspan='8' style='background-color:#F5F5F5; text-align:center; padding:20px; '>" + new Common().SetPageListByNo(PageLength, PageNo, TotalCount, "FileList.aspx", "?G=" + GubunCL + "&T=" + SerchType + "&V=" + SerchValue + "&") + "</TD></TR></Table>";
      return String.Format(TABLE, ReturnValue + PageBottom);
   }
}