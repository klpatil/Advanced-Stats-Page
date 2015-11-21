<%@ Page Language="C#" AutoEventWireup="true" CodeFile="stats.aspx.cs" Inherits="Sitecore.Hack.HI.Web.sitecore.admin.v2.stats"%>


<!DOCTYPE html>
<html lang="en">

<head>

    <meta charset="utf-8">    
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>Stats V2 Page</title>

    <!-- Bootstrap Core CSS -->
    <link href="css/bootstrap.min.css" rel="stylesheet">

    <!-- Custom CSS -->
    <link href="css/sb-admin.css" rel="stylesheet">

    <!-- Morris Charts CSS -->
    <link href="css/plugins/morris.css" rel="stylesheet">

    <!-- Custom Fonts -->
    <link href="font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->

    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/r/bs-3.3.5/jq-2.1.4,dt-1.10.8/datatables.min.css" />

</head>

<body>
    <form id="frmStats" runat="server">
        <center>
                <asp:Label ID="lblMessage" runat="server" Text="" 
                    role="alert"
                    CssClass="text-info" Font-Bold="false" Visible="false" />
            </center>
        <div id="wrapper">
            <div id="page-wrapper">
                <div class="container-fluid">
                    <div class="row">

                        <div class="col-lg-8">
                            <div class="form-group">
                                <label>Site</label>
                                <asp:DropDownList ID="ddlSites" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSites_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-2" style="margin-top: 22px;">
                            <div class="form-group">
                                <div class="checkbox">
                                    <label>

                                        <asp:CheckBox ID="chkIgnorePH" runat="server" Text="Ignore Placeholders?" AutoPostBack="true" OnCheckedChanged="chkIgnorePH_CheckedChanged" />
                                    </label>
                                </div>

                            </div>
                        </div>
                        <div class="col-lg-2" style="margin-top: 22px;">
                            <asp:Button ID="btnReset" CssClass="btn btn-default" Text="Reset Stats" runat="server" OnClick="btnReset_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="table-responsive">
                                <asp:Table ID="tblStats"
                                    CellPadding="0" CellSpacing="0" runat="server"
                                    CssClass="table table-bordered table-hover table-striped" Visible="false">
                                    <asp:TableHeaderRow TableSection="TableHeader">
                                        <%--http://stackoverflow.com/questions/21938008/bootstraps-tooltip-moves-table-cells-to-right-a-bit-on-hover--%>
                                        <asp:TableHeaderCell data-toggle="tooltip" data-placement="top" data-container="body" title="The name of the rendering">Rendering</asp:TableHeaderCell>
                                        <asp:TableHeaderCell data-toggle="tooltip" data-placement="top" data-container="body" title="The name of the website that the information for the rendering is being collected">Site</asp:TableHeaderCell>
                                        <asp:TableHeaderCell data-toggle="tooltip" data-placement="top" data-container="body" title="The number of times that the rendering has been called since the last time the stats page was reset">Count</asp:TableHeaderCell>
                                        <asp:TableHeaderCell data-toggle="tooltip" data-placement="top" data-container="body" title="The number of times the rendering was pulled from cache">From cache</asp:TableHeaderCell>
                                        <asp:TableHeaderCell data-toggle="tooltip" data-placement="top" data-container="body" title="The average time taken to render to output">Avg. time (ms)</asp:TableHeaderCell>
                                        <asp:TableHeaderCell data-toggle="tooltip" data-placement="top" data-container="body" title="The average number of items included in the rendering">Avg. items</asp:TableHeaderCell>
                                        <asp:TableHeaderCell data-toggle="tooltip" data-placement="top" data-container="body" title="The maximum amount of time taken to render the output">Max. time</asp:TableHeaderCell>
                                        <asp:TableHeaderCell data-toggle="tooltip" data-placement="top" data-container="body" title="The maximum number of items included in the rendering">Max. items</asp:TableHeaderCell>
                                        <asp:TableHeaderCell data-toggle="tooltip" data-placement="top" data-container="body" title="The total amount of time taken for all instances of this rendering since the last stats page reset occurred">Total time</asp:TableHeaderCell>
                                        <asp:TableHeaderCell data-toggle="tooltip" data-placement="top" data-container="body" title="The total number of items included in all instances of this rendering since the last stats page reset occurred">Total items</asp:TableHeaderCell>
                                        <asp:TableHeaderCell data-toggle="tooltip" data-placement="top" data-container="body" title="This the last time that stats were collected">Last run</asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                </asp:Table>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-12">
                                <asp:PlaceHolder ID="phChart" runat="server" Visible="false">
                                    <div class="panel panel-primary">
                                        <div class="panel-heading">

                                            <h3 class="panel-title">Top 10 slowest Renderings by Total Time</h3>
                                        </div>
                                        <div class="panel-body">
                                            <div id="morris-bar-chart"></div>                                           
                                        </div>
                                    </div>
                                </asp:PlaceHolder>
                            </div>


                        </div>
                        <!-- /.row -->

                    </div>
                    <!-- /.container-fluid -->

                </div>
                <!-- /#page-wrapper -->

            </div>
        </div>
        <!-- /#wrapper -->

        <!-- jQuery -->
        <script src="js/jquery.js"></script>

        <!-- Bootstrap Core JavaScript -->
        <script src="js/bootstrap.min.js"></script>

        <!-- Morris Charts JavaScript -->
        <script src="js/plugins/morris/raphael.min.js"></script>
        <script src="js/plugins/morris/morris.min.js"></script>
        <script src="js/plugins/morris/morris-data.js"></script>

        <script src="js/adminv2.js"></script>

        <script type="text/javascript" src="https://cdn.datatables.net/r/bs-3.3.5/jqc-1.11.3,dt-1.10.8/datatables.min.js"></script>
    </form>
</body>
</html>

