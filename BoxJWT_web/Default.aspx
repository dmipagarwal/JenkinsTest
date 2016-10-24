<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BoxJWT_web.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-2.1.1.min.js"></script>
    <script type="text/javascript">
        function ShowLoading(that) {
            $("#SpinnerContainer").show();
            $("#Spinner").show();
        }
        function Hideloading() {
            $("#SpinnerContainer").hide();
            $("#Spinner").hide();
        }
        function FileAction(obj) {
            debugger;
            var action = obj.innerHTML;
            var fileId = obj.value;
            $("#hidSelectedFileId").val(fileId);
            $("#hiddata").val(action);
            $("#btnhid").click();
        }
        function OpenFileInNewPage(url) {
            debugger;
            window.open(url, "_blank");
        }
    </script>
    <link href="css/dtree.css" rel="stylesheet" />
    <style type="text/css">
        div#SpinnerContainer {
            position: absolute;
            display: none;
            left: 0;
            right: 0;
            top: 0;
            bottom: 0;
            background-color: #000;
            opacity: 0.4;
            filter: alpha(opacity=40); /* For IE8 and earlier */
            z-index: 1000; /* Important to set this */
        }

        div#Spinner {
            position: absolute;
            display: none;
            width: 50px;
            height: 50px;
            top: 48%;
            left: 48%;
            z-index: 1001;
            overflow: auto;
        }
    </style>
</head>
<body>
    <div id="SpinnerContainer"></div>
    <div id="Spinner" style="background: url(images/spinner.gif) no-repeat center #fff;"></div>
    <form id="form1" runat="server">
        <div id="main">
            <div style="padding-left: 10px; padding-top: 20px;">
                <div style="width: 300px; float: left;">
                    <div style="height: 600px; overflow: auto">
                        <asp:TreeView ID="TreeView1" OnTreeNodePopulate="PopulateNode" Width="250" ExpandDepth="0" runat="server" onclick="ShowLoading(this);" OnTreeNodeExpanded="TreeView1_TreeNodeExpanded">
                            <SelectedNodeStyle BackColor="LavenderBlush" />
                            <Nodes>
                                <asp:TreeNode Text="4700quarryrun" SelectAction="Expand" PopulateOnDemand="true" />
                            </Nodes>
                        </asp:TreeView>
                    </div>

                </div>
                <div style="width: 800px; float: left;">
                    <div style="height: 600px; overflow: auto">
                        <asp:Repeater runat="server" ID="rptFileslist">
                            <HeaderTemplate>
                                <table cellspacing="0" cellpadding="0" border="1" width="780">
                                    <tr>
                                        <th scope="col" style="width: 80px; display: none">File Id
                                        </th>
                                        <th scope="col" style="width: 120px">File Name
                                        </th>
                                        <th scope="col" style="width: 100px">Extention
                                        </th>
                                        <th scope="col" style="width: 100px">Action
                                        </th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="display: none">
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Key") %>' /></td>
                                    <td style="padding: 0px 0px 0px 5px;">
                                        <asp:Label ID="lblFilename" runat="server" Text='<%# Eval("Name") %>' /></td>
                                    <td style="text-align: center">
                                        <asp:Label ID="lblext" runat="server" Text='<%# Eval("ext") %>' /></td>
                                    <td style="padding: 10px 10px 10px 15px;">
                                        <div style="float: left;">
                                            <button type="button" value="<%# Eval("Key") %>" onclick="FileAction(this);">Download</button>
                                        </div>
                                        <div style="float: left; padding-left: 15px;">
                                            <button type="button" value="<%# Eval("Key") %>" onclick="FileAction(this);">
                                            Preview
                                        </div>
                                        <div style="clear: both"></div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>            
        </div>

        <asp:HiddenField runat="server" ID="hiddata" />
        <asp:HiddenField runat="server" ID="hidSelectedFolderId" />
        <asp:HiddenField runat="server" ID="hidSelectedFileId" />
        <div style="display: none">
            <asp:Button ID="btnhid" runat="server" OnClick="btnhid_Click" Text="hidden button" />
        </div>
    </form>
</body>
</html>
