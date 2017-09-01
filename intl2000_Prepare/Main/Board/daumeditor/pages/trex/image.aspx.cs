using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Board_daumeditor_pages_trex_image : System.Web.UI.Page
{
    protected HttpPostedFile files;
    protected string TempFileName;
    protected string TempFile;
    protected string TempFileFullName;
    protected string yyyyMM;
    protected int FileSize;
    protected string FilePath;
    protected void Page_Load(object sender, EventArgs e)
    {

        string Saved_Directory = "";

        files = Request.Files["UploadFile"];
        if (files != null)
        {

            FileSize = files.ContentLength;
            yyyyMM = System.DateTime.Today.ToString("yyyyMM");

            Saved_Directory = Server.MapPath("~/UploadedFiles/Temp/" + yyyyMM + "/");

            if (!Directory.Exists(Saved_Directory))
            {
                DirectoryInfo di = Directory.CreateDirectory(Saved_Directory);
            }
            TempFile = files.FileName;
            string TempExt = TempFile.Substring(TempFile.LastIndexOf("."));
            TempFileName = TempFile.Substring(0, TempFile.LastIndexOf("."));
            int tempCount = 0;
            bool isLastHaveOurCount = false;
            while (CheckSameFile(Saved_Directory + TempFileName + TempExt))
            {
                // tempFileName 맨 뒤에 끝나는게 ) && lastIndexOf(() 부터 맨끝 바로앞까지가 숫자여야됌
                if (TempFileName.Substring(TempFileName.Length - 1) == ")")
                {
                    int tempCountStart = TempFileName.LastIndexOf("(");
                    if (tempCountStart > 0)
                    {
                        string tempString = TempFileName.Substring(tempCountStart + 1, TempFileName.Length - 2 - tempCountStart);

                        try
                        {
                            int tempInt = Int32.Parse(tempString);
                            isLastHaveOurCount = true;
                            tempCount = tempInt;
                        }
                        catch (Exception)
                        {
                            isLastHaveOurCount = false;
                        }
                    }
                }

                tempCount++;
                if (isLastHaveOurCount)
                {
                    TempFileName = TempFileName.Substring(0, TempFileName.LastIndexOf("(")) + "(" + tempCount + ")";
                }
                else
                {
                    TempFileName = TempFileName + "(" + tempCount + ")";
                }
            }
            TempFileFullName = TempFileName + TempExt;

            FilePath = Server.MapPath("~/UploadedFiles/Temp/" + yyyyMM + "/" + TempFileName + TempExt);

            files.SaveAs(FilePath);
        }

    }
    public bool CheckSameFile(string savedpathfileName)
    {
        if (System.IO.File.Exists(savedpathfileName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}