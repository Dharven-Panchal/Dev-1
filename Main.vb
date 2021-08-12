Imports System.Configuration
Imports CefSharp
Imports CefSharp.WinForms
Imports log4net
Public Class Main

#Region "Variables"
    Public chromiumWebBrowser As ChromiumWebBrowser
    Dim UserName As String = ConfigurationManager.AppSettings("UserName").ToString()
    Dim Password As String = ConfigurationManager.AppSettings("Password").ToString()
#End Region
#Region "Methods and Events"
    Public Sub Main()
        '' pnlMain.Enabled = False
        Dim cefSettings As CefSettings = New CefSettings()
        'Initialize Cef Sharp with the provided settings
        Cef.Initialize(cefSettings)
    End Sub
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DashboardToolStripMenuItem.Enabled = True

        'Check For internet connection
        If Common.IsInternetConnected() Then
            Config.XmlConfigurator.Configure()
            CrawlerLogger.LogInfo("App started OR Main Form Loaded")
            DashboardToolStripMenuItem.Enabled = True
            ReportToolStripMenuItem.Enabled = True
            NewSearchToolStripMenuItem.Enabled = True
            NewSearchToolStripMenuItem.Visible = False
        Else
            MessageBox.Show("Internet Connection is not available. Please check your internet connetion!", "Data Crawler", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            CrawlerLogger.LogError("Internet connection not available.")
            DashboardToolStripMenuItem.Enabled = False
            ReportToolStripMenuItem.Enabled = False
            NewSearchToolStripMenuItem.Enabled = False
        End If

    End Sub
    ''' <summary>
    ''' Initialize Chromim with provided URL.
    ''' </summary>
    Private Sub InitializeChromium()
        Try
            CrawlerLogger.LogInfo("Chromium initialization started..")

            'Create a Browser Control component with provided URL
            chromiumWebBrowser = New ChromiumWebBrowser("https://myeclerk.myorangeclerk.com/Account/Login")
            pnlMain.Controls.Clear()
            pnlMain.Controls.Add(chromiumWebBrowser)

            'Make browser to fill the Form
            chromiumWebBrowser.Dock = DockStyle.Fill

            'Fill login credential in login page to automate login process
            chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded("document.getElementById('Email').value= " + "'" + UserName + "'")
            chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded("document.getElementById('Password').value= " + "'" + Password + "'")
            chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded("document.getElementById('loginButton').click()")

            CrawlerLogger.LogInfo("Chromium has been initialized..")
            AddHandler chromiumWebBrowser.LoadingStateChanged, AddressOf Browser_LoadingStateChanged

        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred while Initializing Chroumium at InitializeChromium(). Message: " + ex.Message)
        End Try
    End Sub
    Private Sub Browser_LoadingStateChanged(sender As Object, e As LoadingStateChangedEventArgs)
        If Not e.IsLoading Then
            Dim script = "$(document).ready(function () {        $('a').click(function () {            return false;        });    });"
            chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded(script)
            Me.Invoke(Sub()
                          toolStripLabel.Text = "Logged in to portal!.. Click on New Search button to start process"
                          toolStripProgressBar.Value = 5
                      End Sub)
        End If
    End Sub
    Private Sub DashboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DashboardToolStripMenuItem.Click
        'At initialization, start chromium
        InitializeChromium()
        NewSearchToolStripMenuItem.Visible = True
        DashboardToolStripMenuItem.Enabled = False
    End Sub
    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim result = MessageBox.Show("Do you want to close application?", "Data Crawler", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            CrawlerLogger.LogInfo("Main Form is closing..")
            Cef.Shutdown()
            If chromiumWebBrowser IsNot Nothing AndAlso Not chromiumWebBrowser.IsDisposed Then
                chromiumWebBrowser.Dispose()
            End If
            CrawlerLogger.LogInfo("Cefsharp has been shut down..")
            Me.Dispose()
        Else
            e.Cancel = True
        End If
    End Sub
    Private Sub NewSearchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewSearchToolStripMenuItem.Click
        CrawlerLogger.LogInfo("User pressed New Search to open Search Form..")
        Dim searchingForm As New Searching
        searchingForm.Searching(Me)
        searchingForm.Show()
        Me.Hide()
    End Sub

    Private Sub ReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportToolStripMenuItem.Click
        'Check For internet connection
        If Not Common.IsInternetConnected() Then
            MessageBox.Show("Internet Connection is not available. Please check your internet connetion!", "Data Crawler", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            CrawlerLogger.LogError("Internet connection not available.")
            Return
        End If

        'Check For DB connection
        If Not Common.IsDBConnected() Then
            MessageBox.Show("Database or Server Connection is not available. Please check your Database or Server connetion!", "Data Crawler", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            CrawlerLogger.LogError("Database or Server Connection is not available at Report button click")
            Return
        End If

        Dim reportForm As New Report
        CrawlerLogger.LogInfo("User pressed Report to open Report Form..")
        reportForm.Show()
    End Sub
#End Region

End Class