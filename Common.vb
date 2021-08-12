Imports System.Configuration
Imports System.Data.SqlClient

Public Class Common

    ''' <summary>
    ''' Check internet connection availability.
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Shared Function IsInternetConnected() As Boolean
        Try

            Using client = New Net.WebClient()

                Using client.OpenRead("http://clients3.google.com/generate_204")
                    Return True
                End Using
            End Using

        Catch
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Check Database connection availability.
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Shared Function IsDBConnected() As Boolean
        Try
            Dim connectionString As String = ConfigurationManager.AppSettings("CONNSTRING").ToString()
            Using sqlConn As New SqlConnection(connectionString)
                sqlConn.Open()
                Return (sqlConn.State = ConnectionState.Open)
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
