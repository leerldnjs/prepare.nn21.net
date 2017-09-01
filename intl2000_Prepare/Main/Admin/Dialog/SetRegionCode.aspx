<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetRegionCode.aspx.cs" Inherits="Admin_Dialog_SetRegionCode" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../../Common/jquery-1.4.2.min.js"></script>
    <script type="text/javascript">
        var IsThirdRegion = false;
         //RegionCode dropdown 
        function delopt(selectCtrl, mTxt) {
            if (selectCtrl.options) {
                for (i = selectCtrl.options.length; i >= 0; i--) {
                    selectCtrl.options[i] = null;
                }
            }
            selectCtrl.options[0] = new Option(mTxt, "")
            selectCtrl.selectedIndex = 0;
        }

        function LoadRegionCodeCountry(selectID) {
            delopt(document.getElementById("RegionCode[0]"), "Country");
            delopt(document.getElementById("RegionCode[1]"), "Area");
            Admin.LoadReginoCodeCountry(function (result) {
                for (var i = 0; i < result.length; i++) {
                    var thisR = result[i].split("$");
                    document.getElementById(selectID).options[i + 1] = new Option(thisR[1], thisR[0]);
                }
            }, function (result) { alert("ERROR : " + result); });
        }

        function LoadRegionCode(thisC) {
            if (thisC == "0") {
                delopt(document.getElementById("RegionCode[1]"), "Area");
                
                Admin.LoadRegionCode(document.getElementById("RegionCode[" + thisC + "]").value, function (result) {
                    if (thisC == "0") {
                        if (result[0].substr(0, 1) == "7" || result[0].substr(0, 2) == "11") {
                            document.getElementById("RegionCode[1]").style.visibility = "visible";
                            IsThirdRegion = true;
                        }
                        else {
                            document.getElementById("RegionCode[1]").style.visibility = "visible";
                            IsThirdRegion = false;
                        }
                    }
                    for (var i = 1; i < result.length; i++) {
                        var thisR = result[i].split("$");
                        document.getElementById("RegionCode[" + (parseInt(thisC) + 1) + "]").options[i] = new Option(thisR[1], thisR[0]);
                    }
                }, function (result) { alert("ERROR : " + result); });
            }
            form1.RegionCode.value = document.getElementById("RegionCode[" + thisC + "]").value;
        }
        /////////////////
        function LoadRegionCodeCountry1(selectID) {
            delopt(document.getElementById("RegionCode[3]"), "Country");
            delopt(document.getElementById("RegionCode[4]"), "Area");
            delopt(document.getElementById("RegionCode[5]"), "Detail");
            Admin.LoadReginoCodeCountry(function (result) {
                for (var i = 0; i < result.length; i++) {
                    var thisR = result[i].split("$");
                    document.getElementById(selectID).options[i + 1] = new Option(thisR[1], thisR[0]);
                }
            }, function (result) { alert("ERROR : " + result); });
        }

        function LoadRegionCode1(thisC) {
            if (thisC == "3") {
                delopt(document.getElementById("RegionCode[4]"), "Area");
                delopt(document.getElementById("RegionCode[5]"), "Detail");
            }
            else if (thisC == "4" && IsThirdRegion == true) {
                delopt(document.getElementById("RegionCode[5]"), "Detail");
            }
            if (thisC == "3" || IsThirdRegion == true) {
                Admin.LoadRegionCode(document.getElementById("RegionCode[" + thisC + "]").value, function (result) {
                    if (thisC == "3") {
                        if (result[0].substr(0, 1) == "9" || result[0].substr(0, 2) == "12") {
                            document.getElementById("RegionCode[4]").style.visibility = "visible";
                            document.getElementById("RegionCode[5]").style.visibility = "visible";
                            IsThirdRegion = true;
                        }
                        else {
                            document.getElementById("RegionCode[4]").style.visibility = "visible";
                            document.getElementById("RegionCode[5]").style.visibility = "hidden";
                            IsThirdRegion = false;
                        }
                    }
                    for (var i = 1; i < result.length; i++) {
                        var thisR = result[i].split("$");
                        document.getElementById("RegionCode[" + (parseInt(thisC) + 1) + "]").options[i] = new Option(thisR[1], thisR[0]);
                    }
                }, function (result) { alert("ERROR : " + result); });
            }
            form1.RegionCode1.value = document.getElementById("RegionCode[" + thisC + "]").value;
        }
        ///////////////

        //new 지역 등록 
        function AddNew(Type) {
            var Code, Name, NameE, Mode, BranchCode, GUBUN;

            if (Type == "Country") {
                Code = $("#CountryCode").val();
                Name = $("#CountryName").val();
                NameE = $("#CountryNameE").val();
                GUBUN = $("#ddl_UseYN1").val();
                BranchCode = $("#ddl_Branch1").val();

                if ($("#CountryAdd").val() == "국가 수정") {
                    Mode = "Modify";
                }
                else {
                    Mode = "Add";
                }
                if (Name == "" || NameE == "" || BranchCode == "") {
                    alert("필수항목을 채워넣치 않았습니다.");
                    return false;
                }
            }
            else if (Type == "Area") {
                Code = $("#ddl_AreaCode1").val() + "!" + $("#AreaCode").val();
                Name = $("#AreaName").val();
                NameE = $("#AreaNameE").val();
                GUBUN = $("#ddl_UseYN2").val();
                BranchCode = $("#ddl_Branch2").val();

                if ($("#AreaAdd").val() == "지역 수정") {
                    Mode = "Modify";
                }
                else {
                    Mode = "Add";
                }
                if (Name == "" || NameE == "" || BranchCode == "") {
                    alert("필수항목을 채워넣치 않았습니다.");
                    return false;
                }
            }
            else if (Type == "Detail") {
                //Code = $("#RegionCode[0]").val()+$("#RegionCode[1]").val() + "!" + $("#DetailCode").val();
                Code = document.getElementById("HRegionCode").value + "!" + $("#DetailCode").val();
                Name = $("#DetailName").val();
                NameE = $("#DetailNameE").val();
                GUBUN = $("#ddl_UseYN3").val();
                BranchCode = $("#ddl_Branch3").val();
                if ($("#DetailAdd").val() == "상세 수정") {
                    Mode = "Modify";
                }
                else {
                    Mode = "Add";
                }
                if (Name == "" || NameE == "" || BranchCode == "") {
                    alert("필수항목을 채워넣치 않았습니다.");
                    return false;
                }
            }
            //else {
            //    Code = document.getElementById("RegionCode[5]").value + "!" + $("#NewDetailCode").val();
            //    Name = $("#NewDetailName").val();
            //    NameE = $("#NewDetailNameE").val();
            //    GUBUN = $("#ddl_UseYN3").val();
            //    BranchCode = $("#ddl_Branch4").val();
            //    if ($("#NewDetailAdd").val() == "New상세 수정") {
            //        Mode = "Modify";
            //    }
            //    else {
            //        Mode = "Add";
            //    }
            //    if (Name == "" || NameE == "" || BranchCode == "") {
            //        alert("필수항목을 채워넣치 않았습니다.");
            //        return false;
            //    }
            //}
            Admin.SaveNewRegionCode(Mode, Code, Name, NameE, BranchCode, GUBUN, function (result) {
                if (result == "1") {
                    alert("Success");
                    location.reload();
                }
                else if (result == "0") {
                    alert(Code + "는 이미 사용중입니다");
                    return false;
                }
                else {
                    alert(result);
                }
            }, function (result) { alert("ERROR : " + result); });
        }

        function DeleteRegion(Regioncode) {
            if (confirm("삭제하시겠습니까?")) {
                Admin.DeleteRegion(Regioncode, function (result) {
                    if (result == "1") {
                        alert("Success");
                        location.reload();
                    }
                    else {
                        alert(result);
                    }
                }, function (result) { alert("ERROR : " + result); });
            }
        }
        
        //수정 버튼 클릭스 오른쪽 박스로 데이터 전달
        function SetModify(mode, Regioncode, Name, NameE, BranchCode, GUBUN) {
            if (mode == "CountryCode") {
                $("#CountryCode").val(Regioncode);
                document.getElementById("CountryCode").disabled = "disabled";
                $("#CountryName").val(Name);
                $("#CountryNameE").val(NameE);
                $("#ddl_Branch1").val(BranchCode);
                $("#ddl_UseYN1").val(GUBUN);
                $("#CountryAdd").val("국가 수정");
            }
            else if (mode == "AreaCode") {
                $("#AreaName").val(Name);
                $("#AreaNameE").val(NameE);
                $("#ddl_AreaCode1").val(Regioncode.substr(0, 1));
                $("#AreaCode").val(Regioncode.substr(2, 2));
                document.getElementById("ddl_AreaCode1").disabled = "disabled";
                document.getElementById("AreaCode").disabled = "disabled";
                $("#ddl_Branch2").val(BranchCode);
                $("#ddl_UseYN2").val(GUBUN);
                $("#AreaAdd").val("지역 수정");
            }
            else if (mode == "DetailCode") {
                $("#DetailName").val(Name);
                $("#DetailNameE").val(NameE);
                document.getElementById("RegionCode[0]").value = Regioncode.substr(0, 1);
                document.getElementById("RegionCode[1]").value = Regioncode.substr(2, 2);
                $("#HRegionCode").val(Regioncode.substr(0, 4));
                $("#DetailCode").val(Regioncode.substr(5, 2));
                
                document.getElementById("RegionCode[0]").disabled = "disabled";
                document.getElementById("RegionCode[1]").disabled = "disabled";
                document.getElementById("DetailCode").disabled = "disabled";
                $("#ddl_Branch3").val(BranchCode);
                $("#ddl_UseYN3").val(GUBUN);
                $("#DetailAdd").val("상세 수정");
            }
            
            //else {
            //    $("#NewDetailName").val(Name);
            //    $("#NewDetailNameE").val(NameE);
            //    document.getElementById("RegionCode[3]").value = Regioncode.substr(0, 3);
            //    document.getElementById("RegionCode[4]").value = Regioncode.substr(0, 6);
            //    document.getElementById("RegionCode[5]").value = Regioncode.substr(0, 9);
            //    $("#NewDetailCode").val(Regioncode.substr(10, 2));
            //    document.getElementById("RegionCode[3]").disabled = "disabled";
            //    document.getElementById("RegionCode[4]").disabled = "disabled";
            //    document.getElementById("RegionCode[5]").disabled = "disabled";
            //    document.getElementById("NewDetailCode").disabled = "disabled";
            //    $("#ddl_Branch4").val(BranchCode);
            //    $("#ddl_UseYN4").val(GUBUN);
            //    $("#NewDetailAdd").val("New상세 수정");

            //}
        }
    </script>
</head>
<body style="margin: 0 auto; background-repeat: repeat-x; background-color: #FFFFFF;" background="../Images/Region/top_bg.gif">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="SM" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Admin.asmx" />
            </Services>
        </asp:ScriptManager>
        <table width="1000" align="center" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2" align="right" valign="middle" style="height: 50px;">
                    <a href="../RequestList.aspx?G=5051">
                        <img src="../../Images/Board/link.gif" align="absmiddle" border="0"></a>
                </td>
            </tr>
            <tr>
                <td valign="top" style="width: 600px">
                    <table style="width: 600px; margin-top: 20px;">
                        <tr>
                            <td style="height: 25px; padding-left: 10px;">
                                <img src="../../Images/Board/bul_org.gif" /><img src="../../Images/Board/bul_org.gif" />&nbsp;<b>Region Management</b></td>
                        </tr>
                        <tr>
                            <td><%=RegionList %></td>
                        </tr>
                    </table>
                </td>
                <td valign="top" style="width: 400px;">
                    <table style="width: 400px; margin-top: 20px;">
                        <tr>
                            <td style="height: 25px; padding-left: 10px;">
                                <img src="../../Images/Board/bul_org.gif" /><img src="../../Images/Board/bul_org.gif" />&nbsp;<b>Country Setting</b></td>
                        </tr>
                        <tr>
                            <td>
                                <table border="1" align="center" cellpadding="0" cellspacing="0" style="border-collapse: collapse; width: 400px; margin-bottom: 10px;">
                                    <tr>
                                        <td colspan="2" align="center" style="background-color: #708090; width: 400px; height: 35px;"><font color="#FFFFFF"><b>국가</b></font></td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">이름/영문</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <input type="text" id="CountryName" style="width: 100px; height: 18px; border: 1px solid #767676; color: #000000;" />
                                            <input type="text" id="CountryNameE" style="width: 100px; height: 18px; border: 1px solid #767676; color: #000000;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">코드</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <input type="text" id="CountryCode" style="width: 70px; height: 18px; border: 1px solid #767676; color: #000000;" /></td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">관리지사</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <select id="ddl_Branch1" style="width: 90px;">
                                                <option value="0">Branch</option>
                                                <%=NewBranchOption+"" %>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">사용여부</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <select id="ddl_UseYN1" style="width: 90px;">
                                                <option value="1">Y</option>
                                                <option value="0">N</option>
                                            </select>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" style="width: 400px; height: 40px;">
                                <input type="button" id="CountryAdd" value="국가 추가" onclick="AddNew('Country');" style="width: 120px; height: 30px;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 25px; padding-left: 10px;">
                                <img src="../../Images/Board/bul_org.gif" /><img src="../../Images/Board/bul_org.gif" />&nbsp;<b>Area Setting</b></td>
                        </tr>
                        <tr>
                            <td>
                                <table border="1" align="center" cellpadding="0" cellspacing="0" style="border-collapse: collapse; width: 400px; margin-bottom: 10px;">
                                    <tr>
                                        <td colspan="2" align="center" style="background-color: #708090; width: 400px; height: 35px;"><font color="#FFFFFF"><b>지역</b></font></td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">이름/영문</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <input type="text" id="AreaName" style="width: 100px; height: 18px; border: 1px solid #767676; color: #000000;" />
                                            <input type="text" id="AreaNameE" style="width: 100px; height: 18px; border: 1px solid #767676; color: #000000;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">코드</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <select id="ddl_AreaCode1" style="width: 90px;">
                                                <option value="0">Country</option>
                                                <%=NewCountryOption+"" %>
                                            </select>
                                            <input type="text" id="AreaCode" style="width: 70px; height: 18px; border: 1px solid #767676; color: #000000;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">관리지사</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <select id="ddl_Branch2" style="width: 90px;">
                                                <option value="0">Branch</option>
                                                <%=NewBranchOption+"" %>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">사용여부</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <select id="ddl_UseYN2" style="width: 90px;">
                                                <option value="1">Y</option>
                                                <option value="0">N</option>
                                            </select>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" style="width: 400px; height: 40px;">
                                <input type="button" id="AreaAdd" value="지역 추가" onclick="AddNew('Area');" style="width: 120px; height: 30px;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 25px; padding-left: 10px;">
                                <img src="../../Images/Board/bul_org.gif" /><img src="../../Images/Board/bul_org.gif" />&nbsp;<b>Detail Setting</b></td>
                        </tr>
                        <tr>
                            <td>
                                <table border="1" align="center" cellpadding="0" cellspacing="0" style="border-collapse: collapse; width: 400px; margin-bottom: 10px;">
                                    <tr>
                                        <td colspan="2" align="center" style="background-color: #708090; width: 400px; height: 35px;"><font color="#FFFFFF"><b>상세</b></font></td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">이름/영문</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <input type="text" id="DetailName" style="width: 100px; height: 18px; border: 1px solid #767676; color: #000000;" />
                                            <input type="text" id="DetailNameE" style="width: 100px; height: 18px; border: 1px solid #767676; color: #000000;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">코드</td>

                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <input type="hidden" id="RegionCode" />
                                            <input type="hidden" id="HRegionCode" />
                                            <select id="RegionCode[0]" style="width: 90px;" onchange="LoadRegionCode('0');">
                                                <option value="0">Country</option>
                                                <%=NewCountryOption+"" %>
                                            </select>
                                            <select id="RegionCode[1]" style="width: 90px;" name="office">
                                                <option value="0">Area</option>
                                                <%=NewAreaOption+"" %>
                                            </select>
                                            <input type="text" id="DetailCode" style="width: 70px; height: 18px; border: 1px solid #767676; color: #000000;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">관리지사</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <select id="ddl_Branch3" style="width: 90px;">
                                                <option value="0">Branch</option>
                                                <%=NewBranchOption+"" %>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">사용여부</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <select id="ddl_UseYN3" style="width: 90px;">
                                                <option value="1">Y</option>
                                                <option value="0">N</option>
                                            </select>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" style="width: 400px; height: 40px;">
                                <input type="button" id="DetailAdd" value="상세 추가" onclick="AddNew('Detail');" style="width: 120px; height: 30px;" />
                            </td>
                        </tr>

                        <%--<tr>
                            <td style="height: 25px; padding-left: 10px;">
                                <img src="../../Images/Board/bul_org.gif" /><img src="../../Images/Board/bul_org.gif" />&nbsp;<b>NewDetail Setting</b></td>
                        </tr>
                        <tr>
                            <td>
                                <table border="1" align="center" cellpadding="0" cellspacing="0" style="border-collapse: collapse; width: 400px; margin-bottom: 10px;">
                                    <tr>
                                        <td colspan="2" align="center" style="background-color: #708090; width: 400px; height: 35px;"><font color="#FFFFFF"><b>상세</b></font></td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">이름/영문</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <input type="text" id="NewDetailName" style="width: 100px; height: 18px; border: 1px solid #767676; color: #000000;" />
                                            <input type="text" id="NewDetailNameE" style="width: 100px; height: 18px; border: 1px solid #767676; color: #000000;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">코드</td>

                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <input type="hidden" id="RegionCode1" />
                                            <select id="RegionCode[3]" style="width: 90px;" onchange="LoadRegionCode1('3');">
                                                <option value="0">Country</option>
                                                <%=NewCountryOption+"" %>
                                            </select>
                                            <select id="RegionCode[4]" style="width: 90px;" name="office" onchange="LoadRegionCode1('4');">
                                                <option value="0">Area</option>
                                                <%=NewAreaOption+"" %>
                                            </select>
                                            <select id="RegionCode[5]" style="width: 90px;" name="Detail" onchange="LoadRegionCode1('5');">
                                                <option value="0">Detail</option>
                                                <%=NewDetailOption+"" %>
                                            </select>
                                            <input type="text" id="NewDetailCode" style="width: 70px; height: 18px; border: 1px solid #767676; color: #000000;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">관리지사</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <select id="ddl_Branch4" style="width: 90px;">
                                                <option value="0">Branch</option>
                                                <%=NewBranchOption+"" %>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">사용여부</td>
                                        <td style="width: 300px; height: 30px; padding-left: 10px;">
                                            <select id="ddl_UseYN4" style="width: 90px;">
                                                <option value="1">Y</option>
                                                <option value="0">N</option>
                                            </select>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" style="width: 400px; height: 40px;">
                                <input type="button" id="NewDetailAdd" value="New상세 추가" onclick="AddNew('NewDetail');" style="width: 120px; height: 30px;" />
                            </td>
                        </tr>--%>

                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
