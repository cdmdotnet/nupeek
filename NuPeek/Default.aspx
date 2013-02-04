﻿<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>NuGet Private Repository</title>
    <style>
        body { font-family: Calibri; }
    </style>
</head>
<body>
    <div>
        <h2>You are running NuPeek with NuGet.Server v<%= typeof(NuGet.Server.DataServices.Package).Assembly.GetName().Version %></h2>
        <p>
            Click <a href="<%= VirtualPathUtility.ToAbsolute("~/nuget/Packages") %>">here</a> to view your packages.
        </p>
        <fieldset style="width:800px">
            <legend><strong>Repository URLs</strong></legend>
            In the package manager settings, add the following URL to the list of 
            Package Sources:
            <blockquote>
                <strong><%= Helpers.GetRepositoryUrl(Request.Url, Request.ApplicationPath) %></strong>
            </blockquote>
            <% if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["apiKey"])) { %>
            To enable pushing packages to this feed using the nuget command line tool (nuget.exe). Set the api key appSetting in web.config.
            <% } %> 
            <% else { %>
            Use the command below to push packages to this feed using the nuget command line tool (nuget.exe).
            <% } %>
            <blockquote>
                <strong>nuget push myPackage.1.2.3.nupkg -s <%= Helpers.GetPushUrl(Request.Url, Request.ApplicationPath) %> <% = ConfigurationManager.AppSettings["apiKey"]%></strong>
            </blockquote>            
            <blockquote>
                <strong>nuget push myPackage.1.2.3.symbols.nupkg -s <%= Helpers.GetPushUrl(Request.Url, Request.ApplicationPath) %> <% = ConfigurationManager.AppSettings["apiKey"]%></strong>
            </blockquote>
        </fieldset>

        <% if (Request.IsLocal) { %>
        <p style="font-size:1.1em">
            To add packages to the feed put package files (.nupkg files) in the folder "<% = NuGet.Server.Infrastructure.PackageUtility.PackagePhysicalPath%>".
        </p>
        <% } %>
        <p>In case of problem, check the <a href="https://bitbucket.org/thinkbeforecoding/nupeek/wiki/Troubleshooting">troubleshooting</a> section on the project Wiki</p>
    </div>
</body>
</html>
