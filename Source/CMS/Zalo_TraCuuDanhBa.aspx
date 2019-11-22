<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Zalo_TraCuuDanhBa.aspx.cs" Inherits="CMS.Zalo_TraCuuDanhBa" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <span>
                <asp:Label ID="lblTitle" runat="server" Text="" style="font-weight:bold"></asp:Label></span><br />
            <div runat="server" style="margin-left: 20px;" id="divGV">
                <span style="color:red">
                    <asp:Literal ID="ltrGoiGVCN" runat="server"></asp:Literal>
                </span>
                <br />
                <span style="color:red">Giáo viên bộ môn:</span><br />
                <div runat="server" style="margin-left: 20px;">
                    <asp:Literal ID="ltrGVBM" runat="server"></asp:Literal>
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <telerik:RadGrid ID="RadGrid2" runat="server" AllowPaging="True" GroupPanelPosition="Top"
                        AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True">
                        <ClientSettings>
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>
                        <MasterTableView>
                            <NoRecordsTemplate>
                                <div style="padding: 20px 10px;">
                                    Không có bản ghi nào!
                                </div>
                            </NoRecordsTemplate>
                            <CommandItemSettings AddNewRecordText="Thêm mới bản ghi" CancelChangesText="Hủy"
                                RefreshText="Làm mới" SaveChangesText="Lưu thay đổi" ShowAddNewRecordButton="true" />
                            <EditFormSettings>
                                <PopUpSettings />
                                <EditColumn UpdateText="Lưu" CancelText="Hủy" InsertText="Thêm" ButtonType="PushButton"></EditColumn>
                            </EditFormSettings>
                            <HeaderStyle CssClass="head-list-grid" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Container.DataSetIndex+1 %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="HO_TEN" FilterControlAltText="Filter HO_TEN column" HeaderText="Tên Giáo viên" SortExpression="HO_TEN" UniqueName="HO_TEN" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="SDT" FilterControlAltText="Filter SDT column" HeaderText="Số điện thoại" SortExpression="SDT" UniqueName="SDT" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="GOI_DIEN" HeaderText="Gọi điện" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <a href="tel: <%# Eval("SDT") %>" onclick="_gaq.push(['_trackEvent', 'Contact', 'Call Now Button', 'Phone']);" id="callnowbutton" style="display: block; height: 100px; width: 100px; background: url(https://edu.onesms.vn/img/calling.jpg) center 0px no-repeat; text-decoration: none; background-size: 100% 100%;">&nbsp;</a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
            <div runat="server" style="margin-left: 20px;" id="divCHPH">
                <div runat="server" style="margin-left: 20px; margin-top: 20px;">
                    <asp:Literal ID="ltrPhuHuynh" runat="server"></asp:Literal>
                </div>
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GroupPanelPosition="Top"
                    AutoGenerateColumns="False" PageSize="100" AllowMultiRowSelection="True">
                    <ClientSettings>
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView DataKeyNames="ID_HOC_SINH" ClientDataKeyNames="ID_HOC_SINH">
                        <NoRecordsTemplate>
                            <div style="padding: 20px 10px;">
                                Không có bản ghi nào!
                            </div>
                        </NoRecordsTemplate>
                        <CommandItemSettings AddNewRecordText="Thêm mới bản ghi" CancelChangesText="Hủy"
                            RefreshText="Làm mới" SaveChangesText="Lưu thay đổi" ShowAddNewRecordButton="true" />
                        <EditFormSettings>
                            <PopUpSettings />
                            <EditColumn UpdateText="Lưu" CancelText="Hủy" InsertText="Thêm" ButtonType="PushButton"></EditColumn>
                        </EditFormSettings>
                        <HeaderStyle CssClass="head-list-grid" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="STT" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Container.DataSetIndex+1 %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="TEN_PHU_HUYNH" FilterControlAltText="Filter TEN_PHU_HUYNH column" HeaderText="Tên phụ huynh" SortExpression="TEN_PHU_HUYNH" UniqueName="TEN_PHU_HUYNH" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SDT_NHAN_TIN" FilterControlAltText="Filter SDT_NHAN_TIN column" HeaderText="Số điện thoại" SortExpression="SDT_NHAN_TIN" UniqueName="SDT_NHAN_TIN" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="GOI_DIEN" HeaderText="Gọi điện" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <a href="tel: <%# Eval("SDT_NHAN_TIN") %>" onclick="_gaq.push(['_trackEvent', 'Contact', 'Call Now Button', 'Phone']);" id="callnowbutton" style="display: block; height: 100px; width: 100px; background: url(https://edu.onesms.vn/img/calling.jpg) center 0px no-repeat; text-decoration: none; background-size: 100% 100%;">&nbsp;</a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </form>
</body>
</html>
