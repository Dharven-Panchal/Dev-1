Imports log4net

Friend Class CrawlerLogger

    Private Shared logger As ILog = LogManager.GetLogger("CrawlerLog")

    Public Shared Sub LogInfo(str As String)
        logger.Info(str)
    End Sub

    Public Shared Sub LogError(str As String)
        logger.Error(str)
    End Sub

    Public Shared Sub LogFatal(str As String)
        logger.Fatal(str)
    End Sub

End Class
