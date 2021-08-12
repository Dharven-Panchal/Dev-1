Public Class CaseTypeBLL

#Region "Methods"
    ''' <summary>
    ''' Returns data from returned by Data Access class
    ''' </summary>
    ''' <returns></returns>
    Public Function GetCaseType() As DataTable
        Try
            Dim objdal As DataAccess = New DataAccess()
            Return objdal.Read()
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at GetCaseType(). Message: " + ex.Message)
            Return Nothing
        End Try
    End Function
    Public Function GetReport(_fromDate As String, _toDate As String, _businessName As String) As DataTable
        Try
            Dim objdal As DataAccess = New DataAccess()
            Return objdal.GetReportData(_fromDate, _toDate, _businessName)
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at GetReport(). Message: " + ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function InsertDataIntoDb(_sXml As String) As Int16
        Try
            Dim objdal As DataAccess = New DataAccess()
            Return objdal.InsertRecord(_sXml)
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at InsertDataIntoDb(). Message: " + ex.Message)
            Return 0
        End Try

    End Function
#End Region

End Class
