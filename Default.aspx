<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" rel="stylesheet" type="text/css" />   
    <link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.1.3/css/bootstrap.css" rel="Stylesheet" type="text/css" /> 
    <script type="text/javascript">
        var countemail = 0;
        $("[id*=lnkView]").live("click", function () {
            var subject = $(this).text();
            var row = $(this).closest("tr");
            $("#bodyview").html($(".body", row).html());
            countemail = $(".counteremail", row).val();
            showbodyinspan(countemail);
            // $("#dialog").show();
            return false;
        });
//        $("[id*=lnk_Add]").live("click", function () {
//            var row = $(this).closest("tr");
//            var countemail = $(".counteremail", row).val();
//            submitemaildata(countemail);
//            return false;
//        });

        $("[id*=tbnreply]").live("click", function () {
            var row = $(this).attr("id");
            var str = row.split('_');
            getdataforcontact(str[3])
            $("#hfcontactsave").val('');
            BackofficeEmailDetails(countemail);
        });
        $("[id*=lnkReplyAll]").live("click", function () {
            var row = $(this).attr("id");
            var str = row.split('_');
            getdataforcontact(str[3])
            $("#hfcontactsave").val('');
            BackofficeEmailDetails(countemail);
        });
        $("[id*=lnkForward]").live("click", function () {
            var row = $(this).attr("id");
            var str = row.split('_');
            getdataforcontact(str[3])
            $("#hfcontactsave").val('');
            BackofficeEmailDetails(countemail);
        });
        $("[id*=lnkAddcontact").live("click", function () {
            var row = $(this).attr("id");
            var href = $(this).attr('href')
            if (href = "BackOffice.aspx") {
                alert("all ready added");
                return false;
            }
            var str = row.split('_');
            getdataforcontact(str[3])
            $("#hfcontactsave").val('onlysave');
        });

        function getdataforcontact(str) {
            $("#txtconatctname").val($("#ctl00_ContentPlaceHolderBody_gvEmail_" + str + "_name").val());
            $("#txtcontactemail").val($("#ctl00_ContentPlaceHolderBody_gvEmail_" + str + "_email").val());
            $("#txtcontactcountry").val("INDIA");
            return false;
        }
        $(document).ready(function () {

            $("#btnsubmit").click(function () {
                SaveContact("Default2.aspx/btnSave");
                $("#tblcont").show();
            });
            $("#btncancel").click(function () {
                foredirectbackoffice();
            });
            $(".checkreademail").click(function () {
                $(this).css("font-weight", "normal")
            });
            $("#btnrefresh").click(function () {
                window.location.href = "Default2.aspx";
            });


        });
        function SaveContact(URL) {
            var URL = URL;
            // alert("shanish");
            if ($("#txtconatctname").val() == "") {
                $("#txtconatctname").focus();
                return false;
            }
            if ($("#txtcontactemail").hasClass("borderredclass")) {
                $("#txtcontactemail").focus();
                return false;
            }
            if ($("#txtcontactcountry").hasClass("borderredclass")) {
                $("#txtcontactcountry").focus();
                return false;
            }
            var Contact = {};
            Contact.Name = $("#txtconatctname").val();
            Contact.ContactName = $("#txtcompanyname").val();
            Contact.EmailId = $("#txtcontactemail").val();
            Contact.Country = $("#txtcontactcountry").val();
            submitdata(Contact, URL, OncontSuccess); //Methods for saving data when your C# methods having class parameters and only one parameters and name pf parameter must be objcont
        };

        function OncontSuccess(response) {
            if ($("#hfcontactsave").val() != "onlysave") {
                window.location.href = "BackOffice.aspx";
            }
            else
                window.location.href = "Default2.aspx";
        };
        //for save any data in data base
        function submitdata(classname, URL, methodforonsuccess) {
            $.ajax({
                type: "POST",
                url: URL,
                data: '{obj: ' + JSON.stringify(classname) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if ($("#hfcontactsave").val() != "onlysave") {
                        window.location.href = "BackOffice.aspx";
                    }
                    else
                        window.location.href = "Default2.aspx";
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        function foredirectbackoffice() {
            if ($("#hfcontactsave").val() != "onlysave") {
                window.location.href = "BackOffice.aspx";
            }
            else {
                $("#btncanceltop").trigger("click");
            }
        }
        function showbodyinspan(count) {
            $.ajax({
                type: "POST",
                url: "Default2.aspx/GetbodyData",
                data: '{count: ' + JSON.stringify(count) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#bodyview").append(response.d);
                    $("#dialog").show();
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        //--------------------------
        function submitemaildata(count) {
            $.ajax({
                type: "POST",
                url: "Default2.aspx/btnSaveEmail",
                data: '{count: ' + JSON.stringify(count) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert("save successfully");
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        //get email details for reply /all and forwor
        function BackofficeEmailDetails(count) {
            $.ajax({
                type: "POST",
                url: "Default2.aspx/BackofficeEmailDetails",
                data: '{count: ' + JSON.stringify(count) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //alert("Get details);
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
</script>
    <style type="text/css">
        #ctl00_ContentPlaceHolderBody_gvEmail tr:hover
        {
            background: #fff2e6 !important;
        }
        .borderredclass
        {
            border-color:red;
        }
         
       span
       {    
            font-size:12px !important;
       }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="container">
 <input type="hidden" id="hfcontactsave" />

    <table width="100%">
    <tr>
        <td width="55%" valign="bottom" style="padding-bottom:10px;">            
            <div>
                <table>
                 <tr>
                    <td>POP Server Address</td>
                    <td><asp:TextBox ID="txtpopserveraddress" runat="server" Text="pop.gmail.com"></asp:TextBox><input type="hidden" runat="server" id="Hidden1" /></td>
                </tr>
                 <tr>
                    <td>Port</td>
                    <td><asp:TextBox ID="txtport" runat="server" Text="995"></asp:TextBox><input type="hidden" runat="server" id="Hidden2" /></td>
                </tr>
                <tr>
                    <td>Email Id</td>
                    <td><asp:TextBox ID="txtEmailId" runat="server"></asp:TextBox><input type="hidden" runat="server" id="hfEmailId" /></td>
                </tr>
                 <tr>                     
                    <td>Password</td>
                    <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><input type="hidden" runat="server" id="hfPassword" /></td>
                </tr>
                 <tr>                     
                    <td>No of Email show</td>
                    <td><asp:TextBox ID="txtnoofemail" runat="server"></asp:TextBox><asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtnoofemail" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator><input type="hidden" runat="server" id="hfemail" /></td>
                </tr>
                <tr>
                <td></td>
                <td><asp:CheckBox ID ="rememberme" runat="server"  Text ="remember Password" /><br />
                </td>                
                </tr>
                    <tr>
                        <td>
                        <asp:HiddenField ID="hfcommand" runat="server" />
                            <asp:Button ID="Button1" runat="server" Text="Inbox" CssClass="btn" onclick="Button1_Click" /> Today Emails :<span id="noofmailtoday" runat="server"></span>
                        &nbsp;
                        </td>                         
                        <td>
                            <asp:Button ID="Button3" runat="server" Text="Outbox" CssClass="btn"  onclick="Button3_Click" />
                           <%-- &nbsp;<input type="button" id="btnrefresh"  value="Refresh" class="btn" />(First refresh before going to inbox to outbox or vise versa)
                        --%></td>
                    </tr>
                </table>
            </div>
        </td>            
    </tr>
    <tr>
        <td>
        <div id="stack2" class="modal" tabindex="-1" data-focus-on="input:first" style="background-color:White;height:400px;width:600px;position:relative;">
                <div class="modal-header">
                    <button type="button" id="btncanceltop" class="close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h3>
                        Add to contact Management</h3>
                        
                </div>
                <div class="modal-body">
                    <table>
                    <tr>
                        <td>
                            Contact name
                        </td>
                        <td>
                                
                            <input type="text" id="txtconatctname" class="textbox" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Company name
                        </td>
                        <td>
                               
                            <input type="text" id="txtcompanyname" class="textbox" /><span class="warning">(If you do not enter any company name then your contact name work as company name also)</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Emails
                        </td>
                        <td>
                              
                                <input type="text" id="txtcontactemail" class="textbox" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Country
                        </td>
                        <td>
                                <input type="text" id="txtcontactcountry" class="textbox" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" id="btnsubmit" value="Save" class="btn" />
                            <button type="button" data-dismiss="modal" class="btn" id="btncancel">
                        Cancel</button>
                        </td>
                    </tr>
                </table>
                        
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </td>
    </tr>
</table>
 
    <table width="100%">
        <tr>            
            <td style="width: 35%; vertical-align: top;">
                <div style="height: 600px; overflow: scroll;">
                    <asp:GridView runat="server" ID="gvEmail" AutoGenerateColumns="false"   Width="100%" 
                        AllowSorting="True">
                        <Columns>
                            <asp:TemplateField HeaderText="Inbox" ControlStyle-Font-Size="Small">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkView" runat="server" style='text-decoration: none;' Text='<%# Eval("Subject") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField HeaderText="Date" DataField="Date" Visible="false" />
                            <asp:TemplateField HeaderText="" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnk_Reply" runat="server" Text='<%# Eval("Reply") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnk_ReplyAll" runat="server" Text='<%# Eval("ReplyAll") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField> 
                             <asp:TemplateField HeaderText="" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnk_Forward" runat="server" Text='<%# Eval("Forward") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField> --%>
                            <asp:TemplateField HeaderText="" Visible="false">
                                <ItemTemplate>
                                   <%--     <asp:LinkButton ID="lnkattachment" runat="server" Text='Attachment' OnClick="ThisApplication_NewMail"></asp:LinkButton>
                              --%>  </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ControlStyle-BorderColor="white" HeaderText="Add">
                                <ItemTemplate>
                                 <asp:LinkButton ID="lnk_Add" runat="server"  Text="Add"></asp:LinkButton>                             
                                    <span class="body" style="display: none"> 
                                          <a class="btn" id="tbnreply" href='<%# Eval("linkforcont") %>' data-toggle='modal' runat="server" title='Reply'>Reply</a>
                                          <a class="btn" id="lnkReplyAll" href='<%# Eval("linkforcont") %>' data-toggle='modal' runat="server" title='Reply to All'>Reply to All</a>
                                          <a class="btn" id="lnkForward" href='<%# Eval("linkforcont") %>' data-toggle='modal' runat="server" title='Forword'>Forword</a>
                                          <a class="btn" id="lnkAddcontact" href='<%# Eval("linkforcont") %>' data-toggle='modal' runat="server" title='Add to Contact'>Add to Contact</a>
                                          <input type="hidden" id="name" class="contactname" runat="server" name="name" value='<%# Eval("Sender") %>'/>
                                          <input type="hidden" id="counteremail" class="counteremail" runat="server" name="name" value='<%# Eval("counteremail") %>'/>
                                          <input type="hidden" id="email" class="contactemail" runat="server" name="name" value='<%# Eval("Emails") %>'/>
                                         <br /> <br />                           
                                       <%-- <%# Eval("Body")%></span>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                                        <div style="text-align: left; border: 0px">
                                            <strong>No Data Available...</strong>
                                        </div>
                                    </EmptyDataTemplate>
                                    <RowStyle BackColor="#EFF3FB" Wrap="false" />
                                    <EditRowStyle BackColor="Black" />
                                    <SelectedRowStyle BackColor="#055C92" Font-Bold="False" ForeColor="#333333" />
                                    <PagerStyle BackColor="White" ForeColor="#2461BF" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="fixedHeader" BackColor="#CCDDEF" Font-Bold="False" ForeColor="Black"
                                         Wrap="false" />
                                    <AlternatingRowStyle BackColor="White" CssClass="odd" />
                    </asp:GridView>
                </div> 
            </td>
            <td style="width: 65%">
                <div style="height: 600Px; overflow: scroll; ">
                <div id="dialog" style="display:none;">
                 <a id="linkReply" href="#"></a>
                        <a id="Forword" href="#"></a>                       
                         <asp:LinkButton ID="lnkAttachment" runat="server" OnClick="Download" Text='<%# Eval("FileName") %>' />                   
                     <span id="bodyview"></span>
                    
                </div>
                </div> 
            </td> 
        </tr>
    </table>

</div>
   
</asp:Content>

