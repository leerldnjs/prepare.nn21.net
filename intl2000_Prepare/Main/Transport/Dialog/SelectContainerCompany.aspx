<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectContainerCompany.aspx.cs" Inherits="Transport_Dialog_SelectContainerCompany" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="http://code.jquery.com/jquery-latest.js"></script> 
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <input type="text" name="ASDF" value="<%=Request.Params["Value"] %>" />
            <tr><td>Company</td><td><select id="st_Company">
            <option value="">선택하세요</option>
            <option value="1">Test1</option>
            <option value="2">Test2</option>
            <option value="3">Test3</option>
            <option value="4">Test4</option>
            </select></td></tr>
            <tr><td>Data1</td><td><input type="text" id="Data1" value="Data1"/></td></tr>
            <tr><td>Data2</td><td><input type="text" id="Data2" value="Data2"/></td></tr>
            <tr><td>Data3</td><td><input type="text" id="Data3" value="Data3"/></td></tr>
            <tr><td>Comment</td><td><input type="text" id="Comment" /></td></tr>
            <tr><td><input type="button" id="BTN_Select" value="선택" onclick="SelectCompany()"/>

                <input type="button" id="TEST" value="선택" />

                </td>
                <td><input type="button" value="취소" onclick="window.returnValue = true; returnValue = 'N'; self.close();"/></td></tr>
        </table>
    
    </div>
        <script type="text/javascript">
            $(document).ready(function () {
                $("#TEST").on("click", function () {
                    $(opener.document).find("#TEST").val("value");
                });

            });

            function SelectCompany() {
                var CompanyPk = $("#st_Company").val();

                $.ajax({
                    type: "POST",
                    url: "/WebService/TransportP.asmx/LoadList_Saved_ContainerCompany",
                    data: "{CompanyPk:\"" + $("#CompanyPk").val() + "\"}",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        var LoadResult = result.d;
                        var CompanyData = LoadResult.split(",!");
                        $("#Data1").val(CompanyData[1]);
                        $("#Data2").val(CompanyData[2]);
                        $("#Data3").val(CompanyData[3]);
                    },
                    error: function (result) {
                        alert('failure : ' + result);
                    }
                });
            }

            
        </script>

    </form>
</body>
</html>
