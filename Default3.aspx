<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default3.aspx.cs" Inherits="Default3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:GridView runat="server" ID="gvEmail" AutoGenerateColumns="false"   Width="800px" 
                        AllowSorting="True">
                        <Columns>
                            <asp:TemplateField HeaderText="Inbox" ControlStyle-Font-Size="Small">                                
                                 <ItemTemplate>
                                    <asp:LinkButton ID="lnkView" runat="server" style='text-decoration: none;' Text='<%# Eval("Subject") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField>
                              <ItemTemplate>
                                    <asp:LinkButton ID="lnkView1" runat="server" style='text-decoration: none;' Text='<%# Eval("summary") %>'></asp:LinkButton>
                                </ItemTemplate>
                             </asp:TemplateField>
                              <asp:TemplateField>
                              <ItemTemplate>
                                    <asp:LinkButton ID="lnkView11" runat="server" style='text-decoration: none;' Text='<%# Eval("modified") %>'></asp:LinkButton>
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
</asp:Content>

