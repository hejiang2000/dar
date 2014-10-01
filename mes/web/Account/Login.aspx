<%@ Page Title="<%$ Resources: LoginTitle %>" Language="C#" MasterPageFile="Account.Master" className="Account.Login" %>
<script runat="server" language="cs">
    protected void Page_Load(object sender, EventArgs e)
    {
        //RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + System.Web.HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
    }
</script>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources: LoginTitle %>" />
    </h2>
    <p>
        <asp:Literal runat="server" Text="<%$ Resources: LoginDescription %>" />
        <%--<asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="false">Register</asp:HyperLink> if you don't have an account.--%>
    </p>
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false"
    FailureText="<%$ Resources: FailureText %>"
    >
        <LayoutTemplate>
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="LoginUserValidationGroup"/>
            <div class="accountInfo">
                <fieldset class="login">
                    <legend><asp:Literal runat="server" Text="<%$ Resources: AccountInformation %>" /></legend>
                    <p>
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Username %>" />:</asp:Label>
                        <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                             CssClass="failureNotification" ErrorMessage="<%$ Resources: UsernamePrompt %>" ToolTip="<%$ Resources: UsernamePrompt %>" 
                             ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: Password %>" />:</asp:Label>
                        <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                             CssClass="failureNotification" ErrorMessage="<%$ Resources: PasswordPrompt %>" ToolTip="<%$ Resources: PasswordPrompt %>" 
                             ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:CheckBox ID="RememberMe" runat="server"/>
                        <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources: RememberMe %>" /></asp:Label>
                    </p>
                </fieldset>
                <p class="submitButton">
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="<%$ Resources: LoginText %>" ValidationGroup="LoginUserValidationGroup"/>
                </p>
            </div>
        </LayoutTemplate>
    </asp:Login>
</asp:Content>
