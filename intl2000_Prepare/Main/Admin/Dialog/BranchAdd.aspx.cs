﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_Dialog_BranchAdd : System.Web.UI.Page
{
    protected string CompanyCode1;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            switch (Session["Language"] + "")
            {
                case "en":
                    Page.UICulture = "en";
                    break;
                case "ko":
                    Page.UICulture = "ko";
                    break;
                case "zh":
                    Page.UICulture = "zh-cn";
                    break;
            }
        }
        catch (Exception)
        {
        }

        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
    }
}
