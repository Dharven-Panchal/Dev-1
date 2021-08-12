Imports System.IO
Imports System.Security
Imports CefSharp

Public Class Downloader
    Implements IDownloadHandler

    Public Event OnBeforeDownloadFired As EventHandler(Of DownloadItem)
    Public Event OnDownloadUpdatedFired As EventHandler(Of DownloadItem)

    Public Sub OnBeforeDownload(chromiumWebBrowser As IWebBrowser, browser As IBrowser, downloadItem As DownloadItem, callback As IBeforeDownloadCallback) Implements IDownloadHandler.OnBeforeDownload
        RaiseEvent OnBeforeDownloadFired(Me, downloadItem)
        If Not callback.IsDisposed Then
            Using callback
                Try
                    callback.Continue(Path.Combine(Environment.CurrentDirectory + "\DC", downloadItem.SuggestedFileName), False)
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try
            End Using
        End If
    End Sub

    Public Sub OnDownloadUpdated(chromiumWebBrowser As IWebBrowser, browser As IBrowser, downloadItem As DownloadItem, callback As IDownloadItemCallback) Implements IDownloadHandler.OnDownloadUpdated
        RaiseEvent OnDownloadUpdatedFired(Me, downloadItem)
    End Sub
End Class
