Imports System.Configuration
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports CefSharp
Imports CefSharp.WinForms
Imports DataCrawling.AutoPDFToImageConverter.Others
Imports Polly
Imports UglyToad.PdfPig
Imports UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter
Imports UglyToad.PdfPig.DocumentLayoutAnalysis.ReadingOrderDetector
Imports UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor
Imports UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor

Public Class Searching

#Region "Variables"
    Public chromiumWebBrowser As ChromiumWebBrowser
    Dim mainDashboardForm As Main
    Dim ctb As CaseTypeBLL = New CaseTypeBLL()
    Dim commaSeparatedData As String
    Public MainFrameRenderCount As Int16 = 0
    Dim recordModelList As New List(Of RecordModel)
    Dim recordModel As RecordModel
    Dim partyModelList As New List(Of PartyModel)
    Dim partyModel As PartyModel
    Dim extractedDataModelList As New List(Of ExtractedDataModel)
    Dim extractedDataModel As ExtractedDataModel
    Dim isRecordListFilled As Boolean
    Dim isPartyListFilled As Boolean
    Dim isAccountDetailsInPdf As Boolean
    Dim isFormDateSelected As Boolean = False
    Dim downloadedDirectoryPath As String = Path.Combine(Environment.CurrentDirectory, "DC")
    Public isPDFDownloaded As Integer = 0
    Public Property CaseTypes As List(Of CasetypeDTO)

#Region "Pattern"
    Dim regexp As String = "(^[A-Za-z]{3,}\s+([A-Za-z][.]\s+)?[A-Za-z]{3,}\s+,+\s+([A-Za-z]{3,}[.])$)"
    'Dim regexpSpecial As String = "(^[A-Za-z]{3,}\s+([A-Za-z][.]\s+)?[A-Za-z]{3,},+\s+([A-Za-z]{3,})$)"
    Dim firstlinereg As Regex = New Regex(regexp, RegexOptions.Compiled)
    ' Dim firstlineregSpecial As Regex = New Regex(regexpSpecial, RegexOptions.Compiled)
    Dim regexpAddress2 As String = "([0-9]{2,4}\s[A-Za-z].*$)"
    Dim firstlineregAdd As Regex = New Regex(regexpAddress2, RegexOptions.Compiled)
    Dim regexpZip As String = "([\d]{5})"
    Dim firstlineregZip As Regex = New Regex(regexpZip, RegexOptions.Compiled)
#End Region

#End Region

#Region "Methods and Events"
    Public Sub Searching(frmMain As Main)
        mainDashboardForm = frmMain
    End Sub

    Private Sub Searching_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        mainDashboardForm.Show()
    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        Try
            'Check for the oldpdf on path if available we will delete it first
            DeletePdfFileFromPath()
            'Check For DB connection
            If Not Common.IsDBConnected() Then
                MessageBox.Show("Database or Server Connection is not available. Please check your Database or Server connetion!", "Data Crawler", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                CrawlerLogger.LogError("Database or Server Connection is not available at Search button click")
                Return
            End If
            If Common.IsInternetConnected() Then
                btn_search.Enabled = True
                'If txtBox_BusinessName.Text <> "" Then
                If CheckBoxComboBox1.Text.ToString().Trim().Contains("----Select All----") Then
                    commaSeparatedData = String.Join(",", CaseTypes.AsEnumerable().[Select](Function(x) x.propCaseValue.ToString()).ToArray())
                Else
                    Dim checkRecord = CheckBoxComboBox1.Text.ToString().Trim().Replace(", ", ",")
                    If String.IsNullOrEmpty(checkRecord) Then
                        MessageBox.Show("Please check atleast one Case Type!", "Data Crawler", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    Dim splitValue As String() = checkRecord.Split(",")
                    Dim pers As CasetypeDTO = New CasetypeDTO
                    Dim strCTValue As String = ""
                    For index = 0 To splitValue.Length - 1
                        pers = CaseTypes.FirstOrDefault(Function(p) p.propCaseType = splitValue(index))
                        strCTValue += (pers.propCaseValue & ",")
                    Next

                    commaSeparatedData = strCTValue.TrimEnd(",")

                End If

                chromiumWebBrowser = New ChromiumWebBrowser("https://myeclerk.myorangeclerk.com/Cases/Search")
                chromiumWebBrowser.Size = New Size(1300, 782)
                chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded("document.getElementById('ct').value=" + "'" + commaSeparatedData + "'")
                chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded("document.getElementById('businessName').value=" + "'" + txtBox_BusinessName.Text + "'")
                chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded("document.getElementById('caseNumber').value=" + "'" + txtBox_CaseNo.Text + "'")
                If isFormDateSelected Then
                    chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded("document.getElementById('DateFrom').value=" + "'" + dateFrom.Value.ToString("M/d/yy") + "'")
                    chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded("document.getElementById('DateTo').value=" + "'" + dateTo.Value.ToString("M/d/yy") + "'")
                End If
                chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded("document.getElementById('caseSearch').click()")
                'chromiumWebBrowser.DownloadHandler = New Downloader()
                Dim downer As Downloader = New Downloader()
                chromiumWebBrowser.DownloadHandler = downer
                mainDashboardForm.pnlMain.Controls.Clear()
                mainDashboardForm.pnlMain.Controls.Add(chromiumWebBrowser)
                mainDashboardForm.Show()
                chromiumWebBrowser.Dock = DockStyle.Fill
                Me.Hide()
                mainDashboardForm.DashboardToolStripMenuItem.Enabled = False
                AddHandler chromiumWebBrowser.FrameLoadEnd, AddressOf BrowserOnFrameEnd
                AddHandler chromiumWebBrowser.LoadingStateChanged, AddressOf Browser_LoadingStateChanged
                AddHandler downer.OnDownloadUpdatedFired, AddressOf OnDownloadUpdatedFired
                mainDashboardForm.toolStripLabel.Text = "Searching the criteria of: " + txtBox_BusinessName.Text
                mainDashboardForm.toolStripProgressBar.Value = 10
                'Else
                '    MessageBox.Show("Please Enter the Bussiness Name!", "Data Crawler", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                '    txtBox_BusinessName.Select()
                'End If
            Else
                MessageBox.Show("Internet Connection is not available. Please check your internet connetion!", "Data Crawler", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                CrawlerLogger.LogError("Internet connection not available.")
                btn_search.Enabled = False
            End If
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred when click on Search button. Message: " + ex.Message)
        End Try
    End Sub

    Private Sub OnDownloadUpdatedFired(sender As Object, e As DownloadItem)
        isPDFDownloaded = e.PercentComplete
    End Sub
    Public Sub BrowserOnFrameEnd(sender As Object, e As FrameLoadEndEventArgs)
        Try
            If (chromiumWebBrowser.IsBrowserInitialized And MainFrameRenderCount = 1) Then
                'Get all records in 1 page
                chromiumWebBrowser.GetMainFrame().EvaluateScriptAsync(" $('#caseList_length option[value=" + "-1" + "]').attr('selected', 'selected').change();")
                CrawlerLogger.LogInfo("GetTableData Started...")
                GetTableData()
                CrawlerLogger.LogInfo("GetTableData Completed...")
            End If
            MainFrameRenderCount = MainFrameRenderCount + 1
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred when Browser on Frame End event executed. Message: " + ex.Message)
        End Try
    End Sub
    Private Sub Browser_LoadingStateChanged(sender As Object, e As LoadingStateChangedEventArgs)
        If Not e.IsLoading Then
            Dim script = "$(document).ready(function () {
        $('#header').click(function () {
            return false;
        });
        $('.footer').click(function () {
            return false;
        });
    });"
            chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded(script)
        End If
    End Sub
    Private Sub Searching_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            comboBox_caseType.Select()
            dateTo.Format = DateTimePickerFormat.Custom
            dateTo.CustomFormat = "M/d/yy"
            dateFrom.Format = DateTimePickerFormat.Custom
            dateFrom.CustomFormat = "M/d/yy"
            CheckBoxComboBox1.Height = 34
            Dim dataTable As DataTable
            dataTable = ctb.GetCaseType()
            comboBox_caseType.DataSource = dataTable
            comboBox_caseType.ValueMember = "CTValue"
            comboBox_caseType.DisplayMember = "CaseType"

            CaseTypes = New List(Of CasetypeDTO)
            For index = 0 To dataTable.Rows.Count - 1
                Dim _casetype = New CasetypeDTO
                _casetype.propID = dataTable.Rows(index)("id")
                _casetype.propCaseType = dataTable.Rows(index)("casetype")
                _casetype.propCaseValue = dataTable.Rows(index)("CTValue")
                CaseTypes.Add(_casetype)
            Next
            CheckBoxComboBox1.Items.AddRange(CaseTypes.AsEnumerable().[Select](Function(x) x.propCaseType).ToArray())

            CheckBoxComboBox1.ValueMember = "propCaseValue"
            commaSeparatedData = String.Join(",", dataTable.AsEnumerable().[Select](Function(x) x.Field(Of String)("CTValue").ToString()).ToArray())
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred while loading Search Form. Message: " + ex.Message)
        End Try
    End Sub
    Private Sub dateFrom_ValueChanged(sender As Object, e As EventArgs) Handles dateTo.ValueChanged, dateFrom.ValueChanged
        IsDateValid()
    End Sub
    ''' <summary>
    ''' Compare Date From and Date To 
    ''' </summary>
    ''' <returns></returns>
    Private Function IsDateValid() As Boolean
        Try
            If dateTo.Value < dateFrom.Value Then
                MessageBox.Show("Date To should be greater than Date From!", "Data Crawler", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                SendKeys.Send("{RIGHT 1}")
                dateTo.Select()
                btn_search.Enabled = False
                Return False
            Else
                btn_search.Enabled = True
                isFormDateSelected = True
                Return False
            End If
            isFormDateSelected = True
        Catch ex As Exception
            Return False
        End Try
    End Function
    ''' <summary>
    ''' All records will be converted into Record Model class
    ''' </summary>
    Private Async Sub GetTableData()
        Try
            'chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded("document.getElementsByTagName('input')[1].value='pending';")
            'Thread.Sleep(3 * 1000)
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              mainDashboardForm.toolStripProgressBar.Value = 20
                              mainDashboardForm.toolStripLabel.Text = "Fetching the all records of Pending status of: " + txtBox_BusinessName.Text
                          End Sub)
            End If
            Const getTableScript As String = " var tdDetailList = [];" _
                        & "  (function () { " _
                        & "   var tdDetailTable = document.getElementById('caseList');" _
                        & "   for (var i = 0, row; row = tdDetailTable.rows[i]; i++) {" _
                        & "   var tdDetail = {SrNo : row.cells[0].innerHTML, CaseNumber : row.cells[1].innerHTML, Description : row.cells[2].innerHTML, Type : row.cells[3].innerHTML, Status : row.cells[4].innerHTML, DOB : row.cells[5].innerHTML, JudgeName : row.cells[6].innerHTML, Date : row.cells[7].innerHTML}; " _
                        & "   if(row.cells[4].innerHTML.indexOf('Pending') !== -1) { tdDetailList.push(tdDetail); } " _
                        & " } return tdDetailList })();"

            Dim jsResponse = Await chromiumWebBrowser.GetMainFrame().EvaluateScriptAsync(getTableScript)
            If jsResponse.Success And isRecordListFilled = False AndAlso jsResponse.Result IsNot Nothing AndAlso jsResponse.Result.Count > 0 Then
                For Each item In jsResponse.Result
                    Dim srNo As String = item.SrNo.ToString()
                    Dim caseNumber As String = item.CaseNumber.ToString()
                    Dim dob As String = item.DOB.ToString()
                    Dim caseDate As String = item.Date.ToString()
                    Dim description As String = item.Description.ToString()
                    Dim judgeName As String = item.JudgeName.ToString()
                    Dim status As String = item.Status.ToString()
                    Dim type As String = item.Type.ToString()

                    recordModel = New RecordModel()
                    recordModel.SrNo = srNo
                    recordModel.CaseNumber = caseNumber
                    recordModel.DOB = dob
                    recordModel.CaseDate = caseDate
                    recordModel.Description = description
                    recordModel.JudgeName = judgeName
                    recordModel.Status = status
                    recordModel.Type = type
                    recordModelList.Add(recordModel)
                Next
            Else
                CrawlerLogger.LogError("At GetTableData() JS Response status is false and JS Result - " + jsResponse?.Result?.ToString())
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  mainDashboardForm.toolStripProgressBar.Value = 20
                                  mainDashboardForm.toolStripLabel.Text = "There is no data with pending status for Business Name: " + txtBox_BusinessName.Text
                              End Sub)
                End If
            End If
            isRecordListFilled = True
            CrawlerLogger.LogInfo("MainDataProcess Started...")
            MainDataProcess()
            CrawlerLogger.LogInfo("MainDataProcess Completed...")
            Thread.Sleep(2 * 1000)
            MainFrameRenderCount = 0
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              mainDashboardForm.DashboardToolStripMenuItem.Enabled = True
                              mainDashboardForm.toolStripProgressBar.Value = 100
                              mainDashboardForm.toolStripLabel.Text = "All data has been extracted and inserted into DB Successfully!"
                          End Sub)
            End If
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at GetTableData(). Message: " + ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Each data will be processed
    ''' </summary>
    Private Sub MainDataProcess()
        Try
            Dim isCallTwiceParty As Integer = 0
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              mainDashboardForm.toolStripProgressBar.Value = 30
                              mainDashboardForm.toolStripLabel.Text = "Opening the party details and extract the name of plaintiff type for Business Name: " + txtBox_BusinessName.Text
                          End Sub)
            End If

            For Each recordItem In recordModelList
                Dim srId As Double = (CDbl(recordItem.SrNo) - 1)
CallParty:
                'Dim recordDetailsJsResponse = chromiumWebBrowser.EvaluateScriptAsync("document.getElementsByClassName('caseLink')[" + srId.ToString() + "].click();")
                Dim recordDetailsJsResponse = chromiumWebBrowser.GetMainFrame().EvaluateScriptAsync("document.getElementsByClassName('caseLink')[" + srId.ToString() + "].click();")
                Thread.Sleep(2 * 1000)
                If recordDetailsJsResponse.Status Then

                    'Wait Screen to extract data properly And stop to load another case for 3 second
                    Thread.Sleep(2 * 1000)

                    'At this point, each page details will be rendered on our UI. Now, we can Extract required data.
                    Const getPartiesTableScript As String = " var tdDetailList = [];" _
                            & "  (function () { " _
                            & "   var tdDetailTable = document.getElementById('partiesCollapse').getElementsByTagName('table')[0];" _
                            & "    for (var i = 0, row; row = tdDetailTable.rows[i]; i++) {" _
                            & "        var tdDetail = {Name : row.cells[0].innerHTML, Type : row.cells[1].innerHTML, Attorney : row.cells[2].innerHTML, AttyPhone : row.cells[3].innerHTML }; " _
                            & "        if(row.cells[1].innerHTML.indexOf('Plaintiff') !== -1) { tdDetailList.push(tdDetail); } " _
                            & " } return tdDetailList })();"

                    'Dim partiesTableJsResponse = chromiumWebBrowser.EvaluateScriptAsync(getPartiesTableScript)
                    Dim partiesTableJsResponse = chromiumWebBrowser.GetMainFrame().EvaluateScriptAsync(getPartiesTableScript)
                    Thread.Sleep(3 * 1000)
                    If partiesTableJsResponse.Result.Success And isPartyListFilled = False AndAlso partiesTableJsResponse.Result IsNot Nothing Then
                        For Each item In partiesTableJsResponse.Result.Result
                            Dim partyName As String = item.Name.ToString()
                            Dim partyType As String = item.Type.ToString()
                            Dim attorney As String = item.Attorney.ToString()
                            Dim attyPhoneNo As String = item.AttyPhone.ToString()

                            partyModel = New PartyModel()
                            partyModel.Name = partyName
                            partyModel.Type = partyType
                            partyModel.Attorney = attorney
                            partyModel.AttyPhoneNo = attyPhoneNo
                            partyModel.Srno = recordItem.SrNo
                            partyModelList.Add(partyModel)
                        Next

                        'Add Party Model(Parties Details) into RecordModel
                        recordItem.PartyModelList = partyModelList

                        DownloadPDF(recordItem, recordItem.SrNo)
                        RenderPreviousPage()
                        isCallTwiceParty = 0

                    Else
                        CrawlerLogger.LogError("At MainDataProcess() JS Response status is false while extract Partied from table. Result - " + partiesTableJsResponse?.Result?.Result?.ToString())
                        isCallTwiceParty = isCallTwiceParty + 1
                        If isCallTwiceParty < 3 Then
                            CrawlerLogger.LogInfo("At MainDataProcess() GoTo CallParty again Receving js response fasle . CallPartyBlock - " + isCallTwiceParty.ToString())
                            GoTo CallParty
                        End If
                    End If
                Else
                    CrawlerLogger.LogError("At MainDataProcess() JS Response status is false while click on CaseNo Link.")
                End If
            Next
            isPartyListFilled = True
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at MainDataProcess(). Message: " + ex.Message)
        End Try
    End Sub


    ''' <summary>
    ''' Download pdfs from Docket Event table
    ''' </summary>
    ''' <param name="caseDetails"></param>
    Private Sub DownloadPDF(caseDetails As RecordModel, _srNo As String)
        Try
            'Create directory that will be contain donwnloaded pdfs
            If Not Directory.Exists(downloadedDirectoryPath) Then
                Directory.CreateDirectory(downloadedDirectoryPath)
                CrawlerLogger.LogInfo("DC directory has been created.")
            End If
            DeletePdfFileFromPath()
            Const downloadPDFScript As String = "(function () { " _
                                     & "		var tdDetailTable = document.getElementById('docketEventsCollapse').getElementsByTagName('table')[0]; " _
                                     & "		var isNoticeFound = false; " _
                                     & "		for (var i = 0, row; row = tdDetailTable.rows[i]; i++)  " _
                                     & "		{ " _
                                     & "			 var ts = document.getElementsByClassName('noprint'); " _
                                     & "			 for(let j = 0; j <= ts.length; j++) " _
                                     & "			 { " _
                                     & "				if(ts[j] != null && (ts[j].text =='Notice to Appear Scheduled' || ts[j].text =='Notice to Appear')) " _
                                     & "				{ " _
                                     & "					var link = document.createElement('a');       " _
                                     & "					link.href = ts[j].href;               " _
                                     & "					link.download = 'NoticeAppearScheduled'+ j + '.pdf';      " _
                                     & "					document.body.appendChild(link);  " _
                                     & "					link.click();               " _
                                     & "					document.body.removeChild(link); " _
                                     & "					isNoticeFound = true; " _
                                     & "					return 'NoticeAppearScheduled'+ j + '.pdf' " _
                                     & "				} " _
                                     & "			 } " _
                                     & "			 if(!isNoticeFound) " _
                                     & "			 { " _
                                     & "			        for(let j = 0; j <= ts.length; j++) " _
                                     & "			        { " _
                                     & "				        if(ts[j] != null && ts[j].text == 'Complaint') " _
                                     & "				        { " _
                                     & "				    	    var link = document.createElement('a');       " _
                                     & "				    	    link.href = ts[j].href;               " _
                                     & "				    	    link.download = 'Complaint'+ j + '.pdf';      " _
                                     & "				    	    document.body.appendChild(link);  " _
                                     & "				    	    link.click();               " _
                                     & "				    	    document.body.removeChild(link); " _
                                     & "				    	    return 'Complaint'+ j + '.pdf' " _
                                     & "				        } " _
                                     & "				        else if(ts[j] != null && ts[j].text == 'Statement of Claim') " _
                                     & "				        { " _
                                     & "				    	    var link = document.createElement('a');       " _
                                     & "				    	    link.href = ts[j].href;               " _
                                     & "				    	    link.download = 'StatementOfClaim'+ j + '.pdf';      " _
                                     & "				    	    document.body.appendChild(link);  " _
                                     & "				    	    link.click();               " _
                                     & "				    	    document.body.removeChild(link); " _
                                     & "				    	    return 'StatementOfClaim'+ j + '.pdf' " _
                                     & "				        } " _
                                     & "			        } " _
                                     & "			 } " _
                                     & "		   }   " _
                                     & "	})()"



            Dim docketTableJsResponse = chromiumWebBrowser.EvaluateScriptAsync(downloadPDFScript)
            CrawlerLogger.LogInfo("Executed script to download PDF from website")
            ' Thread.Sleep(20 * 1000)
            Dim pdfFileName = docketTableJsResponse.Result?.Result
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              mainDashboardForm.toolStripProgressBar.Value = 50
                              If String.IsNullOrEmpty(pdfFileName) Then
                                  mainDashboardForm.toolStripLabel.Text = "Downloading the pdf file for one by one case: Not Found!"
                              Else
                                  mainDashboardForm.toolStripLabel.Text = "Downloading the pdf file, File: " + pdfFileName

                              End If
                          End Sub)
            End If
            If Not String.IsNullOrEmpty(pdfFileName) Then
                While isPDFDownloaded < 100
                    mainDashboardForm.toolStripLabel.Text = "Downloading the pdf file, File: " + pdfFileName + " " + CStr(isPDFDownloaded) + "%"
                End While
            End If
            Thread.Sleep(3 * 1000)
            If Not String.IsNullOrEmpty(pdfFileName) Then
                CrawlerLogger.LogInfo("Downloaded PDF from website successfully. File name - " + pdfFileName)
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  mainDashboardForm.toolStripProgressBar.Value = 55
                                  mainDashboardForm.toolStripLabel.Text = "Downloaded PDF from website successfully. File name - " + pdfFileName
                              End Sub)
                End If
                Thread.Sleep(3 * 1000)
                Dim fullPdfPath = Path.Combine(downloadedDirectoryPath, pdfFileName.ToString())
                If File.Exists(fullPdfPath) AndAlso CanRead(fullPdfPath, _srNo) Then
                    If fullPdfPath.Contains("StatementOfClaim") Then
                        If IsAvailableFile(fullPdfPath) Then
                            If txtBox_BusinessName.Text?.ToLower().Contains("goldman sachs") Then
                                ExtractDataFromStatementPDFGoldMan(fullPdfPath, recordModel, _srNo)
                            ElseIf txtBox_BusinessName.Text?.ToLower().Contains("persolve") Then
                                ExtractDataFromStatementPDFPersolve(fullPdfPath, recordModel, _srNo)
                            ElseIf CheckBoxComboBox1.Text.Trim().ToLower() = "eviction" Then
                                ExtractDataFromStatementPDFTenant(fullPdfPath, recordModel, _srNo)
                            Else
                                ExtractDataFromStatementPDF(fullPdfPath, recordModel, _srNo)
                            End If

                            isAccountDetailsInPdf = False
                        Else
                            CrawlerLogger.LogError("pdf file not available for Statement of Claims.")
                        End If
                    ElseIf fullPdfPath.Contains("Complaint") Then
                        If txtBox_BusinessName.Text?.ToLower().Contains("goldman sachs") Then
                            CrawlerLogger.LogInfo("Goldman Sachs type pdf file not available for Complaint.")
                            DeletePerticularFile(fullPdfPath)
                            Return
                        End If
                        If txtBox_BusinessName.Text?.ToLower().Contains("persolve") Then
                            CrawlerLogger.LogInfo("Persolve type pdf file not available for Complaint.")
                            DeletePerticularFile(fullPdfPath)
                            Return
                        End If
                        If IsAvailableFile(fullPdfPath) Then
                            If isAccountDetailsInPdf = False Then
                                ExtractDataFromComplaintPDF(fullPdfPath, recordModel, _srNo)
                                isAccountDetailsInPdf = False
                            ElseIf CheckBoxComboBox1.Text.Trim().ToLower() = "eviction" Then
                                ExtractDataFromStatementPDFTenant(fullPdfPath, recordModel, _srNo)
                            End If
                            ExtractDataFromStatementPDF(fullPdfPath, recordModel, _srNo)
                        Else
                            CrawlerLogger.LogError("pdf file not available for Complaint.")
                        End If
                    ElseIf fullPdfPath.Contains("NoticeAppearScheduled") Then
                        If txtBox_BusinessName.Text?.ToLower().Contains("goldman sachs") Then
                            CrawlerLogger.LogInfo("Goldman Sachs type pdf file not available for Notice to Appear Scheduled.")
                            DeletePerticularFile(fullPdfPath)
                            Return
                        End If
                        If txtBox_BusinessName.Text?.ToLower().Contains("persolve") Then
                            CrawlerLogger.LogInfo("Persolve type pdf file not available for Notice to Appear Scheduled.")
                            DeletePerticularFile(fullPdfPath)
                            Return
                        End If
                        If IsAvailableFile(fullPdfPath) Then
                            ExtractDataFromNoticePDF(fullPdfPath, recordModel, _srNo)
                            isAccountDetailsInPdf = False
                        Else
                            CrawlerLogger.LogError("pdf file not available for Notice to Appeared Scheduled.")
                        End If
                    End If
                Else
                    CrawlerLogger.LogError("PDF file is not exists or it is not readable. Full PDF Path - " + fullPdfPath)
                    If Me.InvokeRequired Then
                        Me.Invoke(Sub()
                                      mainDashboardForm.toolStripProgressBar.Value = 60
                                      mainDashboardForm.toolStripLabel.Text = "PDF file is not exists or it is not readable. "
                                  End Sub)
                    End If
                    Thread.Sleep(2 * 1000)
                    If File.Exists(fullPdfPath) Then
                        DeletePerticularFile(fullPdfPath)
                    End If
                End If
            End If

            'Delete Directory
            DeletePdfFileFromPath()
            DeleteJPGTXTFileFromPath()
            isPDFDownloaded = 0
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at DownloadPDF(). Message: " + ex.Message)
        End Try
    End Sub

    Private Sub DeletePerticularFile(fullPdfPath As String)
        Try
            If File.Exists(fullPdfPath) Then
                GC.Collect()
                GC.WaitForPendingFinalizers()
                File.Delete(fullPdfPath)
                CrawlerLogger.LogInfo("Perticular File has been deleted successfully. File Name - " + fullPdfPath)
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  mainDashboardForm.toolStripProgressBar.Value = 80
                                  mainDashboardForm.toolStripLabel.Text = "deleting the downloaded pdf file"
                              End Sub)
                End If
                Thread.Sleep(1 * 1000)
            End If
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at DeletePerticularFile(). Message: " + ex.Message)
        End Try
    End Sub

    Private Sub DeleteJPGTXTFileFromPath()
        Try
            Dim downloadedPath As String = Path.Combine(Environment.CurrentDirectory, "OCRTEXT")
            If Directory.Exists(downloadedDirectoryPath) Then
                Dim files = Directory.GetFiles(downloadedPath)
                For Each item In files
                    If File.Exists(item) Then
                        File.Delete(item)
                        CrawlerLogger.LogInfo("File has been deleted successfully. File Name - " + item)
                        If Me.InvokeRequired Then
                            Me.Invoke(Sub()
                                          mainDashboardForm.toolStripProgressBar.Value = 80
                                          mainDashboardForm.toolStripLabel.Text = "deleting the downloaded pdf file"
                                      End Sub)
                        End If
                    End If
                    Thread.Sleep(1 * 1000)
                Next
            End If
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at DeleteJPGTXTFileFromPath(). Message: " + ex.Message)
        End Try
    End Sub

    Private Sub ExtractDataFromStatementPDFTenant(fullPdfPath As String, recordModel As RecordModel, srNo As String)
        Try
            'First add current case details into ExtractedDataModel
            extractedDataModel = New ExtractedDataModel()
            extractedDataModel.CaseDetails = recordModel
            extractedDataModel.CaseDetails.PartyModelList = partyModelList
            Dim _caseNo As String = recordModelList.Where(Function(o) o.SrNo = srNo).Select(Function(o) o.CaseNumber).FirstOrDefault()
            Using pdfDoc = PdfDocument.Open(fullPdfPath)
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  mainDashboardForm.toolStripProgressBar.Value = 55
                                  mainDashboardForm.toolStripLabel.Text = "Reading PDF of downloaded and Extracting the data"
                              End Sub)
                End If
                Thread.Sleep(2 * 1000)
                For Each page In pdfDoc.GetPages()
                    ' Either extract based on order in the underlying document with newlines and spaces.
                    Dim text = ContentOrderTextExtractor.GetText(page)
                    If String.IsNullOrEmpty(text) Then
                        CrawlerLogger.LogError("This PDF is might be Handwritten Or Scanned! Case No - " + _caseNo)
                    Else
                        Dim prev As String = String.Empty
                        If text.Contains("TENANT EVICTION") Then
                            Dim theLines As String() = text.Split(ChrW(10))
                            isAccountDetailsInPdf = True

                            For iDx As Integer = 0 To theLines.Length - 1
                                If theLines(iDx).Contains("PLAINTIFF") OrElse theLines(iDx).Contains("vs") Then
                                    Dim fnames As String = theLines(iDx + 2)
                                    Dim Lnames As String = theLines(iDx + 3)
                                    extractedDataModel.FirstName = fnames
                                    extractedDataModel.LastName = Lnames
                                End If
                                If theLines(iDx).Contains("as follows:") Then
                                    Dim charLocation As Integer = theLines(iDx + 1).IndexOf(".", StringComparison.Ordinal)
                                    Dim charLocation2 As Integer = theLines(iDx + 1).IndexOf(":", StringComparison.Ordinal)
                                    Dim add1 As String = theLines(iDx + 1).Substring(0, charLocation)
                                    Dim add2 As String = theLines(iDx + 1).Substring(charLocation, charLocation2).Trim().TrimEnd(","c)
                                    extractedDataModel.Address1 = add1
                                    extractedDataModel.Address2 = add2
                                    Dim regexp As String = "([^,\d\s][^,\d]*$)"
                                    Dim Firstlinereg As Regex = New Regex(regexp, RegexOptions.Compiled)
                                    Dim m As Match = Firstlinereg.Match(add2)
                                    If m.Success Then
                                        Dim city As String = m.Groups(0).Value
                                        extractedDataModel.City = city
                                    End If
                                    Dim stateZip As String = theLines(iDx + 1).Substring(theLines(iDx + 1).LastIndexOf(","c) + 1).Trim()
                                    Dim stateInfo As String() = stateZip.Split(" "c)
                                    extractedDataModel.State = stateInfo(0)
                                    extractedDataModel.PostalCode = stateInfo(1)
                                    Exit For
                                End If
                            Next
                            CrawlerLogger.LogInfo("Data has been successfully extracted for Statement of Claims type PDF. Case No - " + _caseNo)
                            If Me.InvokeRequired Then
                                Me.Invoke(Sub()
                                              mainDashboardForm.toolStripProgressBar.Value = 55
                                              mainDashboardForm.toolStripLabel.Text = "Data has been successfully extracted for Statement of Claims type PDF. Case No -" + _caseNo
                                          End Sub)
                            End If
                            Thread.Sleep(2 * 1000)
                        Else
                            CrawlerLogger.LogInfo("PDF file format is different than implemented mechanism while processing Statement Of Claims Type. Case No - " + _caseNo)
                        End If
                    End If
                Next
            End Using
            If Not String.IsNullOrEmpty(extractedDataModel.FirstName) Then
                InsertDataIntoDB(srNo)
            Else
                CrawlerLogger.LogError("FirstName is not present. Case No: " + _caseNo)
            End If
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at ExtractDataFromStatementPDFTenant(). Message: " + ex.Message)
        End Try
    End Sub
    Private Sub ExtractDataFromStatementPDFPersolve(fullPdfPath As String, recordModel As RecordModel, srNo As String)
        Try
            'First add current case details into ExtractedDataModel
            extractedDataModel = New ExtractedDataModel()
            extractedDataModel.CaseDetails = recordModel
            extractedDataModel.CaseDetails.PartyModelList = partyModelList
            Dim _caseNo As String = recordModelList.Where(Function(o) o.SrNo = srNo).Select(Function(o) o.CaseNumber).FirstOrDefault()
            Using pdfDoc = PdfDocument.Open(fullPdfPath)
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  mainDashboardForm.toolStripProgressBar.Value = 55
                                  mainDashboardForm.toolStripLabel.Text = "Reading PDF of downloaded and Extracting the data"
                              End Sub)
                End If
                Thread.Sleep(2 * 1000)
                For Each page In pdfDoc.GetPages()
                    ' Either extract based on order in the underlying document with newlines and spaces.
                    Dim text = ContentOrderTextExtractor.GetText(page)
                    If String.IsNullOrEmpty(text) Then
                        CrawlerLogger.LogError("This PDF is might be Handwritten Or Scanned! Case No - " + _caseNo)
                    Else
                        Dim prev As String = String.Empty
                        If text.Contains("SIMPLE INTEREST RETAIL INSTALLMENT CONTRACT") Then
                            Dim theLines As String() = text.Split(ChrW(10))
                            isAccountDetailsInPdf = True

                            For iDx As Integer = 0 To theLines.Length - 1
                                If theLines(iDx).Contains("Buyer (and Co-Buyer) Name and Address") Then
                                    Dim _names As String() = theLines(iDx + 1).Split(" "c)
                                    If _names.Length = 2 Then
                                        extractedDataModel.FirstName = _names(0)
                                        extractedDataModel.LastName = _names(1)
                                    Else
                                        extractedDataModel.FirstName = _names(0)
                                        extractedDataModel.MiddleName = _names(1)
                                        extractedDataModel.LastName = _names(2)
                                    End If
                                    extractedDataModel.Address1 = theLines(iDx + 2)
                                    extractedDataModel.Address2 = theLines(iDx + 3)
                                    Dim stateInfo As String() = theLines(iDx + 4).Split(" "c)
                                    If stateInfo.Length > 3 Then
                                        extractedDataModel.City = stateInfo(0) + " " + stateInfo(1)
                                        extractedDataModel.State = stateInfo(2)
                                        extractedDataModel.PostalCode = stateInfo(3)
                                    Else
                                        extractedDataModel.City = stateInfo(0)
                                        extractedDataModel.State = stateInfo(1)
                                        extractedDataModel.PostalCode = stateInfo(2)
                                    End If
                                End If
                            Next
                            CrawlerLogger.LogInfo("Data has been successfully extracted for Statement of Claims type PDF. Case No - " + _caseNo)
                            If Me.InvokeRequired Then
                                Me.Invoke(Sub()
                                              mainDashboardForm.toolStripProgressBar.Value = 55
                                              mainDashboardForm.toolStripLabel.Text = "Data has been successfully extracted for Statement of Claims type PDF. Case No -" + _caseNo
                                          End Sub)
                            End If
                            Thread.Sleep(2 * 1000)
                        Else
                            CrawlerLogger.LogInfo("PDF file format is different than implemented mechanism while processing Statement Of Claims Type. Case No - " + _caseNo)
                        End If
                    End If
                Next
            End Using
            If Not String.IsNullOrEmpty(extractedDataModel.FirstName) Then
                InsertDataIntoDB(srNo)
            Else
                CrawlerLogger.LogError("FirstName is not present. Case No: " + _caseNo)
            End If
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at ExtractDataFromStatementPDFPersolve(). Message: " + ex.Message)
        End Try
    End Sub
    Private Sub ExtractDataFromStatementPDFGoldMan(fullPdfPath As String, recordModel As RecordModel, srNo As String)
        Try
            'First add current case details into ExtractedDataModel
            extractedDataModel = New ExtractedDataModel()
            extractedDataModel.CaseDetails = recordModel
            extractedDataModel.CaseDetails.PartyModelList = partyModelList
            Dim _caseNo As String = recordModelList.Where(Function(o) o.SrNo = srNo).Select(Function(o) o.CaseNumber).FirstOrDefault()
            Using pdfDoc = PdfDocument.Open(fullPdfPath)
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  mainDashboardForm.toolStripProgressBar.Value = 55
                                  mainDashboardForm.toolStripLabel.Text = "Reading PDF of downloaded and Extracting the data"
                              End Sub)
                End If
                Thread.Sleep(2 * 1000)
                For Each page In pdfDoc.GetPages()
                    ' Either extract based on order in the underlying document with newlines and spaces.
                    Dim text = ContentOrderTextExtractor.GetText(page)
                    If String.IsNullOrEmpty(text) Then
                        CrawlerLogger.LogError("This PDF is might be Handwritten Or Scanned! Case No - " + _caseNo)
                    Else
                        Dim prev As String = String.Empty
                        If text.Contains("INSTALLMENT LOAN AGREEMENT") Then
                            Dim theLines As String() = text.Split(ChrW(10))
                            isAccountDetailsInPdf = True
                            Dim words = page.GetWords(NearestNeighbourWordExtractor.Instance)
                            Dim blocks = DocstrumBoundingBoxes.Instance.GetBlocks(words)
                            Dim orderedBlocks = DefaultReadingOrderDetector.Instance.[Get](blocks)
                            For Each item In orderedBlocks
                                Dim strBlock As String = item.Text
                                Dim strBlockSplit As String() = strBlock.Split(ChrW(10))
                                For i As Integer = 0 To strBlockSplit.Length - 1
                                    If strBlockSplit(i).Contains("Borrower") Then
                                        Dim _names As String() = strBlockSplit(i + 1).Split(" "c)
                                        If _names.Length = 2 Then
                                            extractedDataModel.FirstName = _names(0)
                                            extractedDataModel.LastName = _names(1)
                                        Else
                                            extractedDataModel.FirstName = _names(0)
                                            extractedDataModel.MiddleName = _names(1)
                                            extractedDataModel.LastName = _names(2)

                                        End If
                                        If strBlockSplit.Length = 5 Then
                                            Dim address As String = strBlockSplit(i + 2)
                                            extractedDataModel.Address1 = address
                                            Dim address2 As String = strBlockSplit(i + 3)
                                            extractedDataModel.Address1 = address2
                                            Dim stateInfo As String() = strBlockSplit(4).Split(","c)
                                            If stateInfo.Length > 3 Then
                                                extractedDataModel.City = stateInfo(0) + " " + stateInfo(1)
                                                extractedDataModel.State = stateInfo(2)
                                                extractedDataModel.PostalCode = stateInfo(3)
                                            Else
                                                extractedDataModel.City = stateInfo(0)
                                                extractedDataModel.State = stateInfo(1)
                                                extractedDataModel.PostalCode = stateInfo(2)
                                            End If
                                            Exit For
                                        End If
                                    End If
                                Next
                            Next
                            CrawlerLogger.LogInfo("Data has been successfully extracted for Statement of Claims type PDF. Case No - " + _caseNo)
                            If Me.InvokeRequired Then
                                Me.Invoke(Sub()
                                              mainDashboardForm.toolStripProgressBar.Value = 55
                                              mainDashboardForm.toolStripLabel.Text = "Data has been successfully extracted for Statement of Claims type PDF. Case No -" + _caseNo
                                          End Sub)
                            End If
                            Thread.Sleep(2 * 1000)
                        Else
                            CrawlerLogger.LogInfo("PDF file format is different than implemented mechanism while processing Statement Of Claims Type. Case No - " + _caseNo)
                        End If
                    End If
                Next
            End Using
            If Not String.IsNullOrEmpty(extractedDataModel.FirstName) Then
                InsertDataIntoDB(srNo)
            Else
                CrawlerLogger.LogError("FirstName is not present. Case No: " + _caseNo)
            End If
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at ExtractDataFromStatementPDFGoldMan(). Message: " + ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Extract required data from downloaded PDF files of type Notice To Appeared
    ''' </summary>
    ''' <param name="fullPdfPath"></param>
    ''' <param name="recordModel"></param>
    ''' <param name="srNo"></param>
    Private Sub ExtractDataFromNoticePDF(fullPdfPath As String, recordModel As RecordModel, srNo As String)
        Try
            'First add current case details into ExtractedDataModel
            extractedDataModel = New ExtractedDataModel()
            extractedDataModel.CaseDetails = recordModel
            extractedDataModel.CaseDetails.PartyModelList = partyModelList

            Dim _caseNo As String = recordModelList.Where(Function(o) o.SrNo = srNo).Select(Function(o) o.CaseNumber).FirstOrDefault()
            Using pdfDoc = PdfDocument.Open(fullPdfPath)
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  mainDashboardForm.toolStripProgressBar.Value = 55
                                  mainDashboardForm.toolStripLabel.Text = "Reading PDF of downloaded and Extracting the data"
                              End Sub)
                End If
                Thread.Sleep(2 * 1000)
                For Each page In pdfDoc.GetPages()
                    ' Either extract based on order in the underlying document with newlines and spaces.
                    Dim text = ContentOrderTextExtractor.GetText(page)
                    If String.IsNullOrEmpty(text) Then
                        CrawlerLogger.LogError("This PDF is might be Handwritten or Scanned! Case No - " + _caseNo)
                        CrawlerLogger.LogError("Going For OCR! Case No - " + _caseNo)
                        'If Not String.IsNullOrEmpty(extractedDataModel.FirstName) Then
                        ExportPDFToImage(fullPdfPath, srNo)
                        Exit For
                        'End If
                    Else
                        Dim prev As String = String.Empty
                        If text.Contains("NOTICE TO PLAINTIFF") OrElse text.Contains("NOTICE TO PLAINTIFF AND DEFENDANTS") Then
                            Dim theLines As String() = text.Split(ChrW(10))
                            isAccountDetailsInPdf = True
                            Dim words = page.GetWords(NearestNeighbourWordExtractor.Instance)
                            Dim blocks = DocstrumBoundingBoxes.Instance.GetBlocks(words)
                            Dim orderedBlocks = DefaultReadingOrderDetector.Instance.[Get](blocks)
                            For Each item In orderedBlocks
                                Dim strBlock As String = item.Text
                                Dim strBlockSplit As String() = strBlock.Split(ChrW(10))

                                For i As Integer = 0 To strBlockSplit.Length - 1
                                    If prev.Contains("NOTICE TO PLAINTIFF (S) AND DEFENDANT (S)") OrElse prev.Contains("NOTICE TO PLAINTIFF AND DEFENDANTS") OrElse prev.Contains("NOTICE TO PLAINTIFF(S) AND DEFENDANT(S)") Then
                                        Dim _names As String() = strBlockSplit(i).Split(" "c)
                                        If _names.Length = 3 Then
                                            extractedDataModel.FirstName = _names(0)
                                            extractedDataModel.MiddleName = _names(1)
                                            extractedDataModel.LastName = _names(2)
                                        Else
                                            extractedDataModel.FirstName = _names(0)
                                            extractedDataModel.LastName = _names(1)
                                        End If
                                        If strBlockSplit.Length = 3 Then
                                            Dim address As String = strBlockSplit(1)
                                            extractedDataModel.Address1 = address
                                            Dim stateInfo As String() = strBlockSplit(2).Split(" "c)
                                            If stateInfo.Length > 3 Then
                                                extractedDataModel.City = stateInfo(0) + " " + stateInfo(1)
                                                extractedDataModel.State = stateInfo(2)
                                                extractedDataModel.PostalCode = stateInfo(3)
                                            Else
                                                extractedDataModel.City = stateInfo(0)
                                                extractedDataModel.State = stateInfo(1)
                                                extractedDataModel.PostalCode = stateInfo(2)
                                            End If
                                        Else
                                            Dim address As String = strBlockSplit(1)
                                            extractedDataModel.Address1 = address
                                            extractedDataModel.Address2 = strBlock(2)
                                            Dim stateInfo As String() = strBlockSplit(3).Split(" "c)
                                            If stateInfo.Length > 3 Then
                                                extractedDataModel.City = stateInfo(0) + " " + stateInfo(1)
                                                extractedDataModel.State = stateInfo(2)
                                                extractedDataModel.PostalCode = stateInfo(3)
                                            Else
                                                extractedDataModel.City = stateInfo(0)
                                                extractedDataModel.State = stateInfo(1)
                                                extractedDataModel.PostalCode = stateInfo(2)
                                            End If
                                        End If
                                        Exit For 'terminate the for loop'
                                    End If
                                Next
                                prev = item.Text
                            Next
                            CrawlerLogger.LogInfo("Data has been successfully extracted for Notice to Appeared Scheduled type PDF. Case No - " + _caseNo)
                            If Me.InvokeRequired Then
                                Me.Invoke(Sub()
                                              mainDashboardForm.toolStripProgressBar.Value = 55
                                              mainDashboardForm.toolStripLabel.Text = "Data has been successfully extracted for Notice to Appeared Scheduled type PDF. Case No -" + _caseNo
                                          End Sub)
                            End If
                            Thread.Sleep(2 * 1000)
                        Else
                            CrawlerLogger.LogInfo("PDF file format is different than implemented mechanism while processing Notice to Appeared Scheduled. Case No - " + _caseNo)
                        End If
                    End If
                Next
            End Using
            If Not String.IsNullOrEmpty(extractedDataModel.FirstName) Then
                InsertDataIntoDB(srNo)
            Else
                CrawlerLogger.LogError("FirstName is not present. Case No: " + _caseNo)
            End If

        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at ExtractDataFromNoticePDF(). Message: " + ex.Message)
        End Try

    End Sub

    Private Sub ExportPDFToImage(fullPdfPath As String, srNo As String)
        '' Dim tmpnewpath As String = "F:\Ashish Data\Data\OCRTEXT"
        Dim downloadedPath As String = Path.Combine(Environment.CurrentDirectory, "OCRTEXT")
        If Not Directory.Exists(downloadedPath) Then
            Directory.CreateDirectory(downloadedPath)
            CrawlerLogger.LogInfo("OCRTEXT directory has been created.")
        End If
        Try
            Dim objPDFConvertToImage As PDFConvertToImage = New PDFConvertToImage()
            objPDFConvertToImage.OutputToMultipleFile = True
            objPDFConvertToImage.FirstPageToConvert = -1
            objPDFConvertToImage.LastPageToConvert = -1
            objPDFConvertToImage.OutputFormat = "jpeg"
            objPDFConvertToImage.ResolutionX = 300
            objPDFConvertToImage.ResolutionY = 300
            objPDFConvertToImage.JPEGQuality = 75
            objPDFConvertToImage.DisablePrecompiledFonts = False
            objPDFConvertToImage.FitPage = False
            objPDFConvertToImage.GraphicsAlphaBit = 4
            objPDFConvertToImage.TextAlphaBit = 4
            objPDFConvertToImage.DisableFontMap = False
            objPDFConvertToImage.DisablePlatformFonts = False
            objPDFConvertToImage.DisablePrecompiledFonts = False
            Try
                If Not objPDFConvertToImage.Convert(fullPdfPath, Path.Combine(downloadedPath, "Notice.jpg"), 3) Then
                    ''Throw New Exception()
                End If
            Catch ex As Exception

            End Try
            objPDFConvertToImage = Nothing
            ConvertImgToText(downloadedPath, srNo)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ConvertImgToText(tmpnewpath As String, srNo As String)
        Try
            Dim processPath As String = ConfigurationManager.AppSettings("tessaract").ToString()
            '' Dim processPath As String = "F:\Ashish Data\Data\Tesseract-OCR\Tesseract-OCR\tesseract.exe"
            Dim jpegPath As String = tmpnewpath
            Dim destinationPath As String = tmpnewpath
            Dim tmpwork As DirectoryInfo = New DirectoryInfo(jpegPath)
            Dim files As FileInfo() = tmpwork.GetFiles("*.jpg", SearchOption.TopDirectoryOnly)

            For Each file As FileInfo In files
                Dim timeout As Integer = 3000
                Dim inputArgs As String = String.Concat(""""c, file.FullName, """"c)
                Dim outputArgs As String = String.Concat(""""c, destinationPath + "\" + file.Name.Replace(".jpg", ""), """"c)
                Dim commandArgs As String = String.Empty

                commandArgs = String.Concat(inputArgs, " ", outputArgs)

                Using process As New Process()
                    process.StartInfo.FileName = processPath
                    process.StartInfo.Arguments = commandArgs
                    process.StartInfo.UseShellExecute = False
                    process.StartInfo.RedirectStandardOutput = True
                    process.StartInfo.RedirectStandardError = True
                    process.StartInfo.CreateNoWindow = True
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    process.Start()
                    process.BeginOutputReadLine()
                    process.BeginErrorReadLine()
                    process.WaitForExit()
                    GC.Collect()
                End Using
                Dim textPath As String = destinationPath + "\" + file.Name.Replace(".jpg", ".txt")
                ReadTextFile(textPath, srNo)
            Next
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at ConvertImgToText(). Message: " + ex.Message)
        End Try
    End Sub

    Private Sub ReadTextFile(textPath As String, srNo As String)
        extractedDataModel = New ExtractedDataModel()
        extractedDataModel.CaseDetails = recordModel
        extractedDataModel.CaseDetails.PartyModelList = partyModelList
        Dim textFile As String = textPath
        If File.Exists(textFile) Then
            Dim lines As String() = File.ReadAllLines(textFile)
            For i As Integer = 0 To lines.Length - 1
                If lines(i).ToLower().Contains("vs") Then
                    Dim _name = lines(i + 1).Split(" "c)
                    If _name.Length = 2 Then
                        extractedDataModel.FirstName = _name(0)
                        extractedDataModel.LastName = _name(1)
                    Else
                        If _name.Length < 2 AndAlso _name(0) = "" Then
                            If lines(i + 2).Length > 2 AndAlso lines(i + 2).Split(" "c)(0) IsNot "" Then
                                extractedDataModel.FirstName = lines(i + 2).Split(" "c)(0)
                                extractedDataModel.MiddleName = lines(i + 2).Split(" "c)(1)
                                extractedDataModel.LastName = lines(i + 2).Split(" "c)(2)
                            End If
                        Else
                            extractedDataModel.FirstName = _name(0)
                            extractedDataModel.MiddleName = _name(1)
                            extractedDataModel.LastName = _name(2)
                        End If
                    End If

                End If
                If lines(i).Contains("NOTICE TO PLAINTIFF (S) AND DEFENDANT (S)") Then
                    If lines(i + 1) Is "" Then
                        If lines(i + 2) IsNot "" AndAlso lines(i + 3) IsNot "" AndAlso lines(i + 4) IsNot "" Then
                            Dim address As String = lines(i + 3)
                            Dim stateCityPostal As String = lines(i + 4)
                            Dim result = stateCityPostal.Split(" "c)
                            Dim stateName As String
                            Dim cityName As String
                            Dim postalCode As String
                            If result?.Length > 0 Then
                                stateName = result.Where(Function(o) o.Length = 2).LastOrDefault()
                                Dim stateIndex = stateCityPostal.IndexOf(stateName) 'state index
                                cityName = stateCityPostal.Substring(0, stateIndex - 1)
                                Dim postalCodeIndex = stateIndex + 2 'state index
                                postalCode = stateCityPostal.Substring(postalCodeIndex).Trim()
                            End If
                            extractedDataModel.City = cityName
                            extractedDataModel.State = stateName
                            extractedDataModel.PostalCode = postalCode
                            extractedDataModel.Address1 = address
                            Exit For
                        ElseIf lines(i + 2) IsNot "" Then
                            Dim address As String = lines(i + 4)
                            Dim toBeSearched As String = ","
                            Dim code As String = address.Substring(address.IndexOf(toBeSearched) + toBeSearched.Length)
                            extractedDataModel.Address1 = code
                            Dim stateCity As String = lines(i + 8)
                            Dim result = stateCity.Substring(stateCity.Trim().LastIndexOf(","c) - 7).Split(","c)
                            extractedDataModel.City = result(0)
                            Dim stateInfo = result(1).Trim().Split(" "c)
                            extractedDataModel.State = stateInfo(0).Trim()
                            extractedDataModel.PostalCode = stateInfo(1).Trim()
                            Exit For
                        End If
                    End If

                End If

            Next
            If Not String.IsNullOrEmpty(extractedDataModel.FirstName) Then
                InsertDataIntoDB(srNo)
            End If

        End If
    End Sub

    ''' <summary>
    ''' Extract required data from downloaded PDF files of type Statement of Claims
    ''' </summary>
    ''' <param name="fullPdfPath"></param>
    ''' <param name="recordModel"></param>
    ''' <param name="srNo"></param>
    Private Sub ExtractDataFromStatementPDF(fullPdfPath As String, recordModel As RecordModel, srNo As String)
        Try
            'First add current case details into ExtractedDataModel
            extractedDataModel = New ExtractedDataModel()
            extractedDataModel.CaseDetails = recordModel
            extractedDataModel.CaseDetails.PartyModelList = partyModelList
            Dim _caseNo As String = recordModelList.Where(Function(o) o.SrNo = srNo).Select(Function(o) o.CaseNumber).FirstOrDefault()
            Using pdfDoc = PdfDocument.Open(fullPdfPath)
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  mainDashboardForm.toolStripProgressBar.Value = 55
                                  mainDashboardForm.toolStripLabel.Text = "Reading PDF of downloaded and Extracting the data"
                              End Sub)
                End If
                Thread.Sleep(2 * 1000)
                For Each page In pdfDoc.GetPages()
                    ' Either extract based on order in the underlying document with newlines and spaces.
                    Dim text = ContentOrderTextExtractor.GetText(page)
                    If String.IsNullOrEmpty(text) Then
                        CrawlerLogger.LogError("This PDF is might be Handwritten Or Scanned! Case No - " + _caseNo)
                    Else
                        Dim prev As String = String.Empty
                        If text.ToLower().Contains("account summary") OrElse text.ToLower().Contains("amount enclosed") OrElse text.ToLower().Contains("summary of account") Then
                            Dim theLines As String() = text.Split(ChrW(10))
                            isAccountDetailsInPdf = True
                            Dim words = page.GetWords(NearestNeighbourWordExtractor.Instance)
                            Dim blocks = DocstrumBoundingBoxes.Instance.GetBlocks(words)
                            Dim orderedBlocks = DefaultReadingOrderDetector.Instance.[Get](blocks)
                            For Each item In orderedBlocks
                                Dim strBlock As String = item.Text
                                Dim strBlockSplit As String() = strBlock.Split(ChrW(10))

                                For i As Integer = 0 To strBlockSplit.Length - 1
                                    Dim regexp As String = "(^[A-Z]{3,}\s[A-Z]{1}\s[A-Z]{3,}$)"
                                    Dim Firstlinereg As Regex = New Regex(regexp, RegexOptions.Compiled)
                                    Dim m As Match = Firstlinereg.Match(strBlockSplit(i))

                                    Dim regexpAdd As String = "(^[A-Z]{3,}\s+[A-Z]{2}\s[0-9]{5}(?:-[0-9]{4})?$)"
                                    Dim FirstlineregAdd As Regex = New Regex(regexpAdd, RegexOptions.Compiled)
                                    Dim mAdd As Match = FirstlineregAdd.Match(strBlockSplit(i))

                                    If prev.ToLower().Contains("new balance") Then
                                        Dim regexp1 As String = "(^\$?(([1-9]\d{0,2}(,\d{3})*)|0)?\.\d{1,2}$)"
                                        Dim Firstlinereg1 As Regex = New Regex(regexp1, RegexOptions.Compiled)
                                        Dim m1 As Match = Firstlinereg1.Match(strBlockSplit(i))
                                        If m1.Success Then
                                            Dim _newBalance As String = strBlockSplit(0)
                                            If extractedDataModel.NewBalance Is Nothing AndAlso _newBalance.StartsWith("$") Then
                                                extractedDataModel.NewBalance = _newBalance
                                            End If
                                        End If
                                    ElseIf strBlockSplit(i).ToLower().Contains("new balance") Then
                                        If strBlockSplit.Length > 1 Then
                                            Try
                                                Dim _newBalance As String = strBlockSplit(i + 1)
                                                If extractedDataModel.NewBalance Is Nothing AndAlso _newBalance.StartsWith("$") Then
                                                    extractedDataModel.NewBalance = _newBalance
                                                End If
                                            Catch ex As Exception

                                            End Try
                                        End If
                                    End If

                                    If m.Success Then
                                        Dim _names As String() = m.Groups(0).Value.Split(" "c)
                                        extractedDataModel.FirstName = _names(0)
                                        extractedDataModel.MiddleName = _names(1)
                                        extractedDataModel.LastName = _names(2)
                                        If strBlockSplit.Length = 3 Then
                                            Dim address As String = strBlockSplit(i + 1)
                                            extractedDataModel.Address1 = address
                                            Dim stateInfo As String() = strBlockSplit(i + 2).Split(" "c)
                                            If stateInfo.Length > 3 Then
                                                extractedDataModel.City = stateInfo(0) + " " + stateInfo(1)
                                                extractedDataModel.State = stateInfo(2)
                                                extractedDataModel.PostalCode = stateInfo(3)
                                            Else
                                                extractedDataModel.City = stateInfo(0)
                                                extractedDataModel.State = stateInfo(1)
                                                extractedDataModel.PostalCode = stateInfo(2)
                                            End If
                                        ElseIf strBlockSplit.Length > 3 Then
                                            Dim address As String = strBlockSplit(i + 1)
                                            extractedDataModel.Address1 = address
                                            Dim address2 As String = strBlockSplit(i + 2)
                                            Dim regexpAddCheck As String = "(\b[A-Z]{2,}\s+\d{5}(-\d{4})?\b)"
                                            Dim FirstlineregAddCheck As Regex = New Regex(regexpAddCheck, RegexOptions.Compiled)
                                            Dim mAdd2Check As Match = FirstlineregAddCheck.Match(address2)
                                            If mAdd2Check.Success Then
                                                Dim stateInfCheck As String() = strBlockSplit(i + 2).Split(" "c)
                                                If stateInfCheck.Length > 3 Then
                                                    extractedDataModel.City = stateInfCheck(0) + " " + stateInfCheck(1)
                                                    extractedDataModel.State = stateInfCheck(2)
                                                    extractedDataModel.PostalCode = stateInfCheck(3)
                                                ElseIf stateInfCheck.Length = 3 Then
                                                    extractedDataModel.City = stateInfCheck(0)
                                                    extractedDataModel.State = stateInfCheck(1)
                                                    extractedDataModel.PostalCode = stateInfCheck(2)
                                                End If
                                            Else
                                                extractedDataModel.Address2 = address2
                                                Dim stateInfo As String() = strBlockSplit(i + 3).Split(" "c)
                                                If stateInfo.Length > 3 Then
                                                    extractedDataModel.City = stateInfo(0) + " " + stateInfo(1)
                                                    extractedDataModel.State = stateInfo(2)
                                                    extractedDataModel.PostalCode = stateInfo(3)
                                                ElseIf stateInfo.Length = 3 Then
                                                    extractedDataModel.City = stateInfo(0)
                                                    extractedDataModel.State = stateInfo(1)
                                                    extractedDataModel.PostalCode = stateInfo(2)
                                                End If
                                            End If

                                        End If
                                    ElseIf mAdd.Success Then
                                        If Not String.IsNullOrEmpty(extractedDataModel.FirstName) Then
                                            Dim _detailsName As String = strBlockSplit(i - 2)
                                            Dim _names As String() = _detailsName.Split(" "c)
                                            If _names.Length = 2 Then
                                                extractedDataModel.FirstName = _names(0)
                                                extractedDataModel.LastName = _names(1)
                                            End If
                                            extractedDataModel.Address1 = strBlockSplit(i - 1)
                                            Dim _stateInfo As String() = strBlockSplit(i).Split(" "c)
                                            If _stateInfo.Length > 3 Then
                                                extractedDataModel.City = _stateInfo(0) + " " + _stateInfo(1)
                                                extractedDataModel.State = _stateInfo(2)
                                                extractedDataModel.PostalCode = _stateInfo(3)
                                            Else
                                                extractedDataModel.City = _stateInfo(0)
                                                extractedDataModel.State = _stateInfo(1)
                                                extractedDataModel.PostalCode = _stateInfo(2)
                                            End If
                                        End If
                                    End If
                                Next
                                prev = item.Text
                            Next
                            CrawlerLogger.LogInfo("Data has been successfully extracted for Statement of Claims type PDF. Case No - " + _caseNo)
                            If Me.InvokeRequired Then
                                Me.Invoke(Sub()
                                              mainDashboardForm.toolStripProgressBar.Value = 55
                                              mainDashboardForm.toolStripLabel.Text = "Data has been successfully extracted for Statement of Claims type PDF. Case No -" + _caseNo
                                          End Sub)
                            End If
                            Thread.Sleep(2 * 1000)
                        Else
                            CrawlerLogger.LogInfo("PDF file format is different than implemented mechanism while processing Statement Of Claims Type. Case No - " + _caseNo)
                        End If
                    End If
                Next
            End Using
            If Not String.IsNullOrEmpty(extractedDataModel.FirstName) Then
                InsertDataIntoDB(srNo)
            Else
                CrawlerLogger.LogError("FirstName is not present. Case No: " + _caseNo)
            End If
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at ExtractDataFromStatementPDF(). Message: " + ex.Message)
        End Try

    End Sub


    ''' <summary>
    ''' It will go back to previous page
    ''' </summary>
    Private Sub RenderPreviousPage()
        Try
            If chromiumWebBrowser.CanGoBack Then
                chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded("window.history.go(-1)")
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  mainDashboardForm.toolStripProgressBar.Value = 55
                                  mainDashboardForm.toolStripLabel.Text = "Move back to take the another party details name"
                              End Sub)
                End If
                Thread.Sleep(2 * 1000)
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  mainDashboardForm.toolStripProgressBar.Value = 30
                                  mainDashboardForm.toolStripLabel.Text = "Opening the party details and extract the name of plaintiff type for Business Name: " + txtBox_BusinessName.Text
                              End Sub)
                End If
                CrawlerLogger.LogInfo("Page go back is successful from case details page.")

                'Wait screen to extract data properly and stop to load another case for 3 second
                Thread.Sleep(3 * 1000)

                'Get all records in 1 page
                chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded(" $('#caseList_length option[value=" + "-1" + "]').attr('selected', 'selected').change();")
                CrawlerLogger.LogInfo("All(paging fileter) records has been loaded in table")

                'Wait screen to extract data properly and stop to load another case for 2 second
                Thread.Sleep(3 * 1000)

                'chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded("document.getElementsByTagName('input')[1].value='pending';")
                'Thread.Sleep(3 * 1000)
            End If
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at RenderPreviousPage(). Message: " + ex.Message)
        End Try
    End Sub


    ''' <summary>
    ''' Extract required data from downloaded PDF files of type Complaint
    ''' </summary>
    ''' <param name="fullPdfPath"></param>
    ''' <param name="caseDetails"></param>
    Private Sub ExtractDataFromComplaintPDF(fullPdfPath As String, caseDetails As RecordModel, _srNo As String)
        Try
            'First add current case details into ExtractedDataModel
            extractedDataModel = New ExtractedDataModel()
            extractedDataModel.CaseDetails = caseDetails
            extractedDataModel.CaseDetails.PartyModelList = partyModelList


            Dim address1 As String = ""
            Dim address2 As String = String.Empty
            Dim Name As String = String.Empty
            Dim citystate As String = String.Empty

            Dim _caseNo As String = recordModelList.Where(Function(o) o.SrNo = _srNo).Select(Function(o) o.CaseNumber).FirstOrDefault()
            Using pdfDoc = PdfDocument.Open(fullPdfPath)
                CrawlerLogger.LogInfo("File open successfully for Data Extraction process. Case No - " + caseDetails.CaseNumber)
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  mainDashboardForm.toolStripProgressBar.Value = 55
                                  mainDashboardForm.toolStripLabel.Text = "Reading PDF of downloaded and Extracting the data"
                              End Sub)
                End If
                Thread.Sleep(2 * 1000)
                For Each page In pdfDoc.GetPages()

                    ' Either extract based on order in the underlying document with newlines and spaces.
                    Dim text = ContentOrderTextExtractor.GetText(page)
                    If String.IsNullOrEmpty(text) Then
                        CrawlerLogger.LogError("This PDF is might be Handwritten or Scanned! Case No - " + caseDetails.CaseNumber)
                    Else
                        If text.Contains("Isl") OrElse text.Contains("/s/") OrElse text.ToLower().Contains("bar no") OrElse text.ToLower().Contains("respectfully submitted") Then
                            Dim theLines As String() = text.Split(ChrW(10))
                            For i As Integer = 0 To theLines.Length - 1

                                Dim m1 As Match = firstlinereg.Match(theLines(i).Replace(vbCr, ""))
                                ' Dim m1Special As Match = firstlineregSpecial.Match(theLines(i).TrimEnd().Replace(vbCr, ""))
                                Dim m2 As Match = firstlineregAdd.Match(theLines(i).Replace(vbCr, ""))

                                If m1.Success Then
                                    Name = theLines(i)
                                    Dim _names As String() = Name.Split(" ")
                                    extractedDataModel.FirstName = _names(0)
                                    extractedDataModel.MiddleName = _names(1)
                                    extractedDataModel.LastName = _names(2)
                                ElseIf theLines(i).ToLower().Contains("esq") Then
                                    Dim leftString = theLines(i).Substring(0, theLines(i).IndexOf(",")).Replace("Isl", "").Replace("/s/", "").Trim()
                                    Name = leftString
                                    Dim _names As String() = Name.Split(" ")
                                    If _names.Length = 3 Then
                                        extractedDataModel.FirstName = _names(0)
                                        extractedDataModel.MiddleName = _names(1)
                                        extractedDataModel.LastName = _names(2)
                                    ElseIf _names.Length = 4 Then
                                        extractedDataModel.FirstName = _names(0)
                                        extractedDataModel.MiddleName = _names(1)
                                        extractedDataModel.LastName = _names(2) + " " + _names(3)
                                    Else
                                        extractedDataModel.FirstName = _names(0)
                                        extractedDataModel.LastName = _names(1)
                                    End If

                                ElseIf theLines(i).ToLower().Contains("bar no") OrElse theLines(i).ToLower().Contains("bar number") Then
                                    ' Need to add the plus(+) sign to concatinate the address
                                    address1 = address1 & " " & theLines(i).Replace(vbCr, "")
                                    extractedDataModel.Address1 = address1.Trim()
                                ElseIf theLines(i).ToLower().Contains("pllc") OrElse theLines(i).ToLower().Contains("llp") OrElse theLines(i).ToLower().Contains("p.a.") Then
                                    ' Need to add the plus(+) sign to concatinate the address
                                    address1 = address1 & " " & theLines(i).Replace(vbCr, "")
                                    extractedDataModel.Address1 = address1.Trim()
                                ElseIf m2.Success Then
                                    address2 = theLines(i)
                                    extractedDataModel.Address2 = address2.Trim()
                                    If Not address1.ToLower().Contains(theLines(i - 1).ToLower().Replace(vbCr, "")) AndAlso address1 <> String.Empty Then
                                        address1 += theLines(i - 1)
                                        extractedDataModel.Address1 = address1.Trim()
                                    End If

                                    Dim state As String = theLines(i + 1)
                                    Dim m3 As Match = firstlineregZip.Match(state.Replace(vbCr, ""))

                                    If m3.Success Then
                                        citystate = state
                                        Dim _city As String = citystate.Substring(0, citystate.IndexOf(","c))
                                        extractedDataModel.City = _city
                                        Dim _stateZip As String = citystate.Substring(citystate.LastIndexOf(","c) + 1)
                                        Dim _GetStateZip() As String = _stateZip.TrimStart().Split(" ")
                                        extractedDataModel.State = _GetStateZip(0)
                                        extractedDataModel.PostalCode = _GetStateZip(1)
                                    End If
                                End If
                            Next
                            CrawlerLogger.LogInfo("Data has been successfully extracted for Complaint type PDF. Case No - " + _caseNo)
                            If Me.InvokeRequired Then
                                Me.Invoke(Sub()
                                              mainDashboardForm.toolStripProgressBar.Value = 55
                                              mainDashboardForm.toolStripLabel.Text = "Data has been successfully extracted for Complaint type PDF. Case No -" + _caseNo
                                          End Sub)
                            End If
                            Thread.Sleep(2 * 1000)
                        Else
                            CrawlerLogger.LogInfo("PDF file format is different than implemented mechanism while processing Complaint Type. Case No - " + caseDetails.CaseNumber)
                        End If
                    End If

                Next

            End Using

            If Not String.IsNullOrEmpty(extractedDataModel.FirstName) Then
                InsertDataIntoDB(_srNo)
            Else
                CrawlerLogger.LogError("FirstName is not present. Case No: " + _caseNo)
            End If
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at ExtractDataFromComplaintPDF(). Message: " + ex.Message)
        End Try

    End Sub


    ''' <summary>
    ''' Delete PDF file once data has been extracted.
    ''' </summary>
    Private Sub DeletePdfFileFromPath()
        Try
            If Directory.Exists(downloadedDirectoryPath) Then
                Dim files = Directory.GetFiles(downloadedDirectoryPath)
                For Each item In files
                    If File.Exists(item) Then
                        GC.Collect()
                        GC.WaitForPendingFinalizers()
                        File.Delete(item)
                        CrawlerLogger.LogInfo("File has been deleted successfully. File Name - " + item)
                        If Me.InvokeRequired Then
                            Me.Invoke(Sub()
                                          mainDashboardForm.toolStripProgressBar.Value = 80
                                          mainDashboardForm.toolStripLabel.Text = "deleting the downloaded pdf file"
                                      End Sub)
                        End If
                    End If
                    Thread.Sleep(1 * 1000)
                Next
            End If
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at DeletePdfFileFromPath(). Message: " + ex.Message)
        End Try

    End Sub


    ''' <summary>
    ''' Insert Data into Database
    ''' </summary>
    ''' <param name="_srNo"></param>
    ''' 
    Private Sub InsertDataIntoDB(_srNo As String)
        Try
            Dim dsData As DataSet = New DataSet()
            Dim dtCoumn As DataTable = New DataTable()
            dtCoumn.TableName = "CourtRecord_t"
            dtCoumn.Columns.Add("Fname")
            dtCoumn.Columns.Add("Mname")
            dtCoumn.Columns.Add("Lname")
            dtCoumn.Columns.Add("Address1")
            dtCoumn.Columns.Add("Address2")
            dtCoumn.Columns.Add("State")
            dtCoumn.Columns.Add("City")
            dtCoumn.Columns.Add("ZipCode")
            dtCoumn.Columns.Add("CaseNumber")
            dtCoumn.Columns.Add("CaseType")
            dtCoumn.Columns.Add("CaseDate")
            dtCoumn.Columns.Add("PartyName")
            dtCoumn.Columns.Add("NewBalance")
            dtCoumn.Columns.Add("BusinessName")
            dtCoumn.Columns.Add("DateFrom")
            dtCoumn.Columns.Add("DateTo")

            Dim rowData As DataRow = Nothing
            rowData = dtCoumn.NewRow()
            rowData("Fname") = extractedDataModel.FirstName.Trim()
            rowData("Mname") = If(extractedDataModel.MiddleName Is Nothing, String.Empty, extractedDataModel.MiddleName.Trim())
            rowData("Lname") = extractedDataModel.LastName.Trim()
            rowData("Address1") = If(extractedDataModel.Address1 Is Nothing, String.Empty, extractedDataModel.Address1.Trim())
            rowData("Address2") = If(extractedDataModel.Address2 Is Nothing, String.Empty, extractedDataModel.Address2.Trim())
            rowData("State") = If(extractedDataModel.State Is Nothing, String.Empty, extractedDataModel.State.Trim())
            rowData("City") = If(extractedDataModel.City Is Nothing, String.Empty, extractedDataModel.City.Trim())
            rowData("ZipCode") = If(extractedDataModel.PostalCode Is Nothing, String.Empty, extractedDataModel.PostalCode.Trim())
            Dim _caseData = recordModelList.Where(Function(o) o.SrNo = _srNo)
            Dim _caseNo As String = _caseData.Select(Function(o) o.CaseNumber).FirstOrDefault()
            Dim _caseType As String = _caseData.Select(Function(o) o.Type).FirstOrDefault()
            Dim _caseDate As String = _caseData.Select(Function(o) o.CaseDate).FirstOrDefault()
            rowData("CaseNumber") = _caseNo.Substring(_caseNo.IndexOf(">") + 1, _caseNo.IndexOf("</a>") - _caseNo.IndexOf(">") - 1)
            rowData("CaseType") = _caseType.Trim()
            rowData("CaseDate") = _caseDate.Trim()
            Dim _partyData = partyModelList.Where(Function(o) o.Srno = _srNo)
            Dim _pName As String = _partyData.Select(Function(o) o.Name).FirstOrDefault()
            rowData("PartyName") = _pName.Trim()
            rowData("NewBalance") = If(extractedDataModel.NewBalance Is Nothing, String.Empty, extractedDataModel.NewBalance)
            rowData("BusinessName") = txtBox_BusinessName.Text.Trim()
            If isFormDateSelected = True Then
                rowData("DateFrom") = dateFrom.Value.ToString("yyyy-MM-dd")
                rowData("DateTo") = dateTo.Value.ToString("yyyy-MM-dd")
            Else
                rowData("DateFrom") = ""
                rowData("DateTo") = ""
            End If
            dtCoumn.Rows.Add(rowData)
            dsData.Tables.Add(dtCoumn)

            Dim result As Int16 = ctb.InsertDataIntoDb(dsData.GetXml())
            If result > 0 Then
                'MessageBox.Show("Data Inserted Successfully!")
                CrawlerLogger.LogInfo("Extracted Data inserted into DataBase successfully. Case No - " + _caseNo)
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  mainDashboardForm.toolStripProgressBar.Value = 60
                                  mainDashboardForm.toolStripLabel.Text = "Inserting the data into db for this caseNo: " + _caseNo.Substring(_caseNo.IndexOf(">") + 1, _caseNo.IndexOf("</a>") - _caseNo.IndexOf(">") - 1)
                              End Sub)
                End If
                Thread.Sleep(2 * 1000)
            Else
                CrawlerLogger.LogInfo("Extracted Data cannot be inserted into DataBase! Case No - " + _caseNo)
            End If

        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at InsertDataIntoDB(). Message: " + ex.Message)
        End Try

    End Sub


    ''' <summary>
    ''' Will check that PDF is downloaded or not
    ''' </summary>
    ''' <param name="filePath"></param>
    ''' <returns></returns>
    Public Shared Function IsAvailableFile(filePath As String) As Boolean
        Try
            Dim retryPolicy As Policy = Policy.Handle(Of IOException)().WaitAndRetry(3, Function(i) TimeSpan.FromSeconds(Math.Pow(2, i)))
            retryPolicy.Execute(Sub()
                                    Using stream As FileStream = File.OpenRead(filePath)
                                        stream.Close()
                                    End Using
                                End Sub)
            Return True
        Catch ex As Exception
            CrawlerLogger.LogError("Exception occurred at IsAvailableFile() for file path - " + filePath)
            Return False
        End Try

    End Function


    ''' <summary>
    ''' PDF will check that is it readable or not.
    ''' </summary>
    ''' <param name="filePath"></param>
    ''' <returns></returns>
    Public Function CanRead(filePath As String, srNo As String) As Boolean
        Dim _caseNo As String = recordModelList.Where(Function(o) o.SrNo = srNo).Select(Function(o) o.CaseNumber).FirstOrDefault()
        Try
            Dim buffer As Byte() = Nothing
            Dim fs As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
            Dim br As BinaryReader = New BinaryReader(fs)
            Dim numBytes As Long = New FileInfo(filePath).Length
            buffer = br.ReadBytes(5)
            Dim enc = New ASCIIEncoding()
            Dim header = enc.GetString(buffer)

            If buffer(0) = &H25 AndAlso buffer(1) = &H50 AndAlso buffer(2) = &H44 AndAlso buffer(3) = &H46 Then
                CrawlerLogger.LogInfo("This PDF is readable. file path - " + filePath + " Case No - " + _caseNo)
                Return header.StartsWith("%PDF-")
            End If

            CrawlerLogger.LogError("This PDF is corrupted OR might be in not in PDF format. Case No - " + _caseNo)
            Return False
        Catch ex As Exception
            CrawlerLogger.LogError("This PDF is corrupted OR might be in not in PDF format. Case No - " + _caseNo)
            Return False
        End Try
    End Function
    Private Sub CheckBoxComboBox1_CheckBoxCheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxComboBox1.CheckBoxCheckedChanged
        'If CheckBoxComboBox1.Text.ToString().Trim().Contains("----Select All----") Then
        '    For index = 0 To CheckBoxComboBox1.Items.Count - 1
        '        If CheckBoxComboBox1.CheckBoxItems(0).Checked = False Then
        '            CheckBoxComboBox1.CheckBoxItems(index).Checked = False
        '        Else
        '            CheckBoxComboBox1.CheckBoxItems(index).Checked = True
        '        End If
        '    Next
        'ElseIf Not CheckBoxComboBox1.Text.ToString().Trim().Contains("----Select All----") AndAlso CheckBoxComboBox1.SelectedIndex <> -1 Then
        '    CheckBoxComboBox1.CheckBoxItems(CheckBoxComboBox1.SelectedIndex).Checked = True
        'Else
        '    If CheckBoxComboBox1.CheckBoxItems(0).Checked = False Then
        '        For index = 0 To CheckBoxComboBox1.Items.Count - 1
        '            CheckBoxComboBox1.CheckBoxItems(index).Checked = False
        '        Next
        '    End If

        'End If





    End Sub

    Private Sub chkAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkAll.CheckedChanged
        For index = 0 To CheckBoxComboBox1.Items.Count - 1
            CheckBoxComboBox1.CheckBoxItems(index).Checked = chkAll.Checked
        Next
    End Sub





#End Region

End Class
