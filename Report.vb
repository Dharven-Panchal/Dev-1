Imports System.IO
Imports ClosedXML.Excel
Public Class Report
    Dim ctb As CaseTypeBLL = New CaseTypeBLL()
    Private bitmap As Bitmap
    Private Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click
        Try
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

            If dtpFromDate.Value > dtpToDate.Value Then
                MessageBox.Show("To Date is less than From Date. Please select proper DateRange.", "Data Crawler", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                Dim _toDate As String = dtpToDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                Dim _fromDate As String = dtpFromDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                Cursor.Current = Cursors.WaitCursor
                Dim dtReport As DataTable
                dtReport = ctb.GetReport(_fromDate, _toDate, txtBusinessNameSearch.Text)
                If dtReport IsNot Nothing AndAlso dtReport.Rows.Count > 0 Then
                    btnPrint.Enabled = True
                    btnExport.Enabled = True
                    dgvReportView.DataSource = dtReport
                    dgvReportView.DefaultCellStyle.WrapMode = DataGridViewTriState.True
                    ' dgvReportView.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                    dgvReportView.Columns(0).Width = 95
                    dgvReportView.Columns(1).Width = 60
                    dgvReportView.Columns(4).Width = 55
                    dgvReportView.Columns(5).Width = 55
                    dgvReportView.Columns(6).Width = 65
                    dgvReportView.Columns(9).Width = 50
                    dgvReportView.Columns(10).Width = 50
                    dgvReportView.Columns(11).Width = 65
                    dgvReportView.Columns(12).Width = 60
                    'dgvReportView.Rows(2).Height = 45
                    For i = 0 To dgvReportView.Rows.Count - 1
                        Dim r As DataGridViewRow = dgvReportView.Rows(i)
                        r.Height = 55
                    Next
                    For Each item As DataGridViewColumn In dgvReportView.Columns
                        item.SortMode = DataGridViewColumnSortMode.NotSortable
                    Next

                Else
                    btnPrint.Enabled = False
                    btnExport.Enabled = False
                    dgvReportView.DataSource = Nothing
                    CrawlerLogger.LogInfo("Report not contain any data at execution of GetReport()")
                    MessageBox.Show("Sorry could not found data! Please Try with different search input", "Data Crawler", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
                Cursor.Current = Cursors.Default
            End If
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred when click on Report button. Message: " + ex.Message)
        End Try
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Try
            CrawlerLogger.LogInfo("Data exported to excel initialized.")
            'Creating DataTable.
            Dim dt As New DataTable()
            If dgvReportView.DataSource IsNot Nothing AndAlso dgvReportView.Rows.Count > 0 Then
                'Adding the Columns.
                For Each column As DataGridViewColumn In dgvReportView.Columns
                    dt.Columns.Add(column.HeaderText, column.ValueType)
                Next

                'Adding the Rows.
                For Each row As DataGridViewRow In dgvReportView.Rows
                    dt.Rows.Add()
                    For Each cell As DataGridViewCell In row.Cells
                        dt.Rows(dt.Rows.Count - 1)(cell.ColumnIndex) = cell.Value.ToString()
                    Next
                Next
                'Exporting to Excel.
                Dim folderPath As String = Path.Combine(Environment.CurrentDirectory, "Excel")
                If Not Directory.Exists(folderPath) Then
                    Directory.CreateDirectory(folderPath)
                End If

                Using wb As New XLWorkbook()
                    wb.Worksheets.Add(dt, "CaseReport")

                    'Set the color of Header Row.
                    'A resembles First Column while M resembles Third column.
                    wb.Worksheet(1).Cells("A1:M1").Style.Fill.BackgroundColor = XLColor.DarkGreen
                    For i As Integer = 1 To dt.Rows.Count

                        'A resembles First Column while M resembles Third column.
                        'Header row is at Position 1 and hence First row starts from Index 2.
                        Dim cellRange As String = String.Format("A{0}:M{0}", i + 1)

                        If i Mod 2 <> 0 Then
                            wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.GreenYellow
                        Else
                            wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.Yellow
                        End If
                    Next
                    'Adjust widths of Columns.
                    wb.Worksheet(1).Columns().AdjustToContents()

                    'Save the Excel file.
                    wb.SaveAs(folderPath & "\CaseReport_" + DateTime.Now.ToString("yyyyMMdd") + " .xlsx")
                    CrawlerLogger.LogInfo("Data has been exported into excel format successfully at " + folderPath)
                    MessageBox.Show("Excel file has been exported successfully at " + folderPath, "Data Crawler", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Using
            Else
                CrawlerLogger.LogInfo("Data not available to be exported!")
            End If


        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred when click on Export button. Message: " + ex.Message)
        End Try

    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try
            Dim dgvPrint As DGVPrintHelper.DGVPrinter = New DGVPrintHelper.DGVPrinter()
            dgvPrint.Title = "Case Report"
            dgvPrint.SubTitle = String.Format("Date : {0}", DateTime.Now.ToString())
            dgvPrint.SubTitleFormatFlags = StringFormatFlags.LineLimit Or StringFormatFlags.NoClip
            dgvPrint.PageNumbers = True
            dgvPrint.PageNumberInHeader = False
            dgvPrint.PorportionalColumns = True
            dgvPrint.PageSettings.Landscape = True
            dgvPrint.PageSettings.Margins = New Printing.Margins(20, 3, 20, 3)
            dgvPrint.HeaderCellAlignment = StringAlignment.Near
            dgvPrint.Footer = "Data Crawler"
            dgvPrint.FooterSpacing = 10
            dgvPrint.PrintDataGridView(dgvReportView)
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred when click on Print button. Message: " + ex.Message)
        End Try
    End Sub

    Private Sub PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument.PrintPage
        'Try
        '    'Dim bm As New Bitmap(Me.dgvReportView.Width, Me.dgvReportView.Height)
        '    'dgvReportView.DrawToBitmap(bm, New Rectangle(0, 0, Me.dgvReportView.Width, Me.dgvReportView.Height))
        '    e.Graphics.DrawImage(bitmap, 0, 0)
        'Catch ex As Exception
        '    CrawlerLogger.LogError("Exception occurred when click on Print Page button. Message: " + ex.Message)
        'End Try
    End Sub

    Private Sub Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnPrint.Enabled = False
        btnExport.Enabled = False
    End Sub
End Class