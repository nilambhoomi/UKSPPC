<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/site.master" CodeFile="ViewDesignation.aspx.cs" Inherits="ViewDesignation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Designation					
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>
            <div class="space-12"></div>

            <asp:UpdatePanel runat="server" ID="upMain">
                <ContentTemplate>




                    <div class="row">
                        <div class="alert alert-success" runat="server" id="divSuccess" style="display: none">
                            Designation Deleted Successfully.
                        </div>
                        <div class="alert alert-danger" runat="server" id="divfail" style="display: none">
                            Sorry !! this record is associated with other data.
                        </div>
                    </div>

                    <div class="col-xs-12">


                        <div class="row">
                            <div class="col-sm-3" style="padding-left: 0px">
                                <asp:TextBox ID="txtSearch" CssClass="form-control" placeholder="Search" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-success" Text="Search" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-success" Text="Refresh" OnClick="btnRefresh_Click" />
                                <asp:Button runat="server" ID="btnAdd" Text="Add New" CssClass="btn btn-primary" PostBackUrl="~/AddDesignation.aspx" />
                                <asp:HiddenField ID="hfPatientId" runat="server"></asp:HiddenField>
                            </div>
                            <div class="col-sm-6" style="float: right">
                                <asp:DropDownList runat="server" ID="ddlPage" AutoPostBack="true" Style="float: right; width: 70px" CssClass="form-control" OnSelectedIndexChanged="ddlPage_SelectedIndexChanged">
                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                    <asp:ListItem Text="50" Value="30"></asp:ListItem>
                                    <asp:ListItem Text="100" Value="40"></asp:ListItem>
                                    <%--  <asp:ListItem Text="All" Value="0"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>


                    <div class="clearfix"></div>
                    <br />
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvDesignationDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="pager">
                                    <Columns>

                                        <asp:BoundField DataField="designation" HeaderText="Provider" />
                                      
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HyperLink runat="server" CssClass="btn btn-link" ID="hlEdit"  NavigateUrl='<%# "~/AddDesignation.aspx?id="+Eval("id") %>' Text="Edit"></asp:HyperLink>
                                                <asp:LinkButton runat="server" ID="lnkDelete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this record ?')" CausesValidation="false" OnClick="lnkDelete_Click" CommandArgument='<%# Eval("id") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                                </asp:GridView>
                                <div runat="server" id="div_page">
                                    Page
            <label runat="server" id="lbl_page_no" style="display: inline"></label>
                                    of
            <label runat="server" id="lbl_total_page" style="display: inline"></label>
                                </div>
                                <div>
                                    <ul class="pagination">
                                        <asp:Repeater ID="rptPager" runat="server">
                                            <ItemTemplate>
                                                <li>
                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                        CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "active" : "" %>'
                                                        OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="space-20"></div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

     <asp:HiddenField runat="server" ID="hdId" />

    <script src="Scripts/jquery-1.8.2.js"></script>
    <script src="Scripts/jquery-ui-1.8.24.js"></script>
    <link href="Style/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {

            SetAutoComplete();

        });


        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    SetAutoComplete();
                }
            });
        };

        function SetAutoComplete() {
            $("#<%=txtSearch.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'Search.aspx/GetDesignation',
                        data: "{ 'prefix': '" + request.term + "'}",

                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('_')[1],
                                    val: item.split('_')[0]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdId.ClientID %>").val(i.item.val);
                    $('#<%= txtSearch.ClientID %>').val(i.item.label);
                    document.getElementById("<%=btnSearch.ClientID %>").click();

                },
                minLength: 1
            });
        }
    </script>
</asp:Content>
