Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Configuration

Public Class DataAccess

#Region "Variables"
    Dim ConnectionString As String = ConfigurationManager.AppSettings("CONNSTRING").ToString()
    Dim con As SqlConnection = New SqlConnection()
    Dim dt As DataTable = New DataTable()
#End Region

#Region "Methods"
    ''' <summary>
    ''' Returns data of Case Type from DB
    ''' </summary>
    ''' <returns></returns>
    Public Function Read() As DataTable
        con.ConnectionString = ConnectionString
        If ConnectionState.Closed = con.State Then con.Open()
        Dim cmd As SqlCommand = con.CreateCommand
        cmd.CommandType = CommandType.StoredProcedure
        cmd = New SqlCommand("sp_GetCaseTypes", con)
        Try
            Dim rd As SqlDataReader = cmd.ExecuteReader()
            dt.Load(rd)
            Return dt
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at Read() in DataAccess class. Message: " + ex.Message)
            Return Nothing
        End Try
    End Function
    Public Function GetReportData(_fromDate As String, _toDate As String, _businessName As String) As DataTable
        con.ConnectionString = ConnectionString
        If ConnectionState.Closed = con.State Then con.Open()
        'Dim cmd As SqlCommand = con.CreateCommand
        Dim cmd = New SqlCommand("sp_GetReports", con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@FromDate", _fromDate)
        cmd.Parameters.AddWithValue("@ToDate", _toDate)
        cmd.Parameters.AddWithValue("@businessname", _businessName)
        Try
            Dim rd As SqlDataReader = cmd.ExecuteReader()
            dt.Load(rd)
            Return dt
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at GetReportData() in DataAccess class. Message: " + ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function InsertRecord(_sXml As String) As Int16
        Using con As New SqlConnection(ConnectionString)
            con.Open()
            Dim cmd As New SqlCommand("sp_InsertCaseRecord", con)
            cmd.CommandType = CommandType.StoredProcedure
            ''Add  SqlParameter to the command.
            cmd.Parameters.AddWithValue("@sXml", _sXml.Trim())
            Try
                Dim result = cmd.ExecuteNonQuery()
                Return result
            Catch ex As Exception
                CrawlerLogger.LogError("Exception occurred at InsertRecord() in DataAccess class. Message: " + ex.Message)
                Return 0
            End Try
        End Using
    End Function

#End Region

End Class
