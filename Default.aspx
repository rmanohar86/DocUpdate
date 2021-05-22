<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DocUpdate2.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<div>
        <h1> Document Upload</h1><br />
        <p>
            Please upload the documents from the JSON input file by clicking the below button
            <br /><br />
               <asp:Button id="Button1"
                Text="Upload"
                OnClick="UploadBtn_Click" 
                runat="server"/>
            <br />
            <br />
        </p>
    </div>

            <div>
    <table id="DocumentsTable" runat="server" Height="800" width="1000" >
        <tr>
            <td > 
            <asp:GridView  id="DocumentsTbl" runat="server" Font-Size="10pt" Height="800" Width="700"  autogeneratecolumns="False" 
                 OnRowCommand="DocumentsTbl_RowCommand" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="Solid" BorderColor="#CCCCCC" >
                <SelectedRowStyle Font-Bold="True" ForeColor="White" BackColor="#669999"></SelectedRowStyle>
                <RowStyle ForeColor="#000066" />
                   <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="White" BackColor="#006699"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="PropertyId" HeaderText="PropertyId"  ItemStyle-HorizontalAlign="Center"   />
                        <asp:BoundField DataField="DocType"   HeaderText="DocType"  ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField DataField="FileName"  HeaderText="FileName"  ItemStyle-HorizontalAlign="Center"  />                        
                        <asp:TemplateField HeaderText="DocBlob">
                            <ItemTemplate>
                                 <asp:Button ID="Button1" runat="server" CausesValidation="false" CommandName="Download" CommandArgument='<%#Eval("PropertyId")+","+ Eval("DocType")%>' Text="Download the file" />                                 
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
            </asp:GridView>
                </td>
        </tr>
    </table>
        </div>
    </form>
</body>
</html>
