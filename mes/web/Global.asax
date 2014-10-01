<%@ Application Language="C#" %>

<script runat="server">
    private log4net.ILog _logger;

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        System.IO.FileInfo fi = new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
        log4net.Config.XmlConfigurator.ConfigureAndWatch(fi);
        _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DAR.Resolver.Start("Biz", null, null);

        _logger.Info("MES Application started.");
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
        _logger.Info("MES Application stopped.");
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
