Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Collections
Imports System.IO
Imports System.Threading
Imports System.Configuration

Namespace AutoPDFToImageConverter.Others
    Public Class PDFConvertToImage
        Public Const GhostScriptDLLName As String = "gsdll32.dll"
        Private Shared useSimpleAnsiConversion As Boolean = True
        Private Const GS_OutputFileFormat As String = "-sOutputFile={0}"
        Private Const GS_DeviceFormat As String = "-sDEVICE={0}"
        Private Const GS_FirstParameter As String = "pdf2img"
        Private Const GS_ResolutionXFormat As String = "-r{0}"
        Private Const GS_ResolutionXYFormat As String = "-r{0}x{1}"
        Private Const GS_GraphicsAlphaBits As String = "-dGraphicsAlphaBits={0}"
        Private Const GS_TextAlphaBits As String = "-dTextAlphaBits={0}"
        Private Const GS_FirstPageFormat As String = "-dFirstPage={0}"
        Private Const GS_LastPageFormat As String = "-dLastPage={0}"
        Private Const GS_FitPage As String = "-dPDFFitPage"
        Private Const GS_PageSizeFormat As String = "-g{0}x{1}"
        Private Const GS_DefaultPaperSize As String = "-sPAPERSIZE={0}"
        Private Const GS_JpegQualityFormat As String = "-dJPEGQ={0}"
        Private Const GS_RenderingThreads As String = "-dNumRenderingThreads={0}"
        Private Const GS_Fixed1stParameter As String = "-dNOPAUSE"
        Private Const GS_Fixed2ndParameter As String = "-dBATCH"
        Private Const GS_Fixed3rdParameter As String = "-dSAFER"
        Private Const GS_FixedMedia As String = "-dFIXEDMEDIA"
        Private Const GS_QuiteOperation As String = "-q"
        Private Const GS_StandardOutputDevice As String = "-"
        Private Const GS_MultiplePageCharacter As String = "%"
        Private Const GS_FontPath As String = "-sFONTPATH={0}"
        Private Const GS_NoPlatformFonts As String = "-dNOPLATFONTS"
        Private Const GS_NoFontMap As String = "-dNOFONTMAP"
        Private Const GS_FontMap As String = "-sFONTMAP={0}"
        Private Const GS_SubstitutionFont As String = "-sSUBSTFONT={0}"
        Private Const GS_FCOFontFile As String = "-sFCOfontfile={0}"
        Private Const GS_FAPIFontMap As String = "-sFAPIfontmap={0}"
        Private Const GS_NoPrecompiledFonts As String = "-dNOCCFONTS"
        Private Shared mutex As System.Threading.Mutex
        <DllImport("kernel32.dll", EntryPoint:="RtlMoveMemory")>
        Private Shared Sub CopyMemory(ByVal Destination As IntPtr, ByVal Source As IntPtr, ByVal Length As UInteger)

        End Sub
        <DllImport(GhostScriptDLLName, EntryPoint:="gsapi_new_instance")>
        Private Shared Function gsapi_new_instance(<Out> ByRef pinstance As IntPtr, ByVal caller_handle As IntPtr) As Integer

        End Function
        <DllImport("gsdll32.dll", EntryPoint:="gsapi_init_with_args")>
        Private Shared Function gsapi_init_with_args(ByVal instance As IntPtr, ByVal argc As Integer, ByVal argv As IntPtr) As Integer

        End Function
        <DllImport("gsdll32.dll", EntryPoint:="gsapi_exit")>
        Private Shared Function gsapi_exit(ByVal instance As IntPtr) As Integer

        End Function
        <DllImport("gsdll32.dll", EntryPoint:="gsapi_delete_instance")>
        Private Shared Sub gsapi_delete_instance(ByVal instance As IntPtr)

        End Sub
        <DllImport("gsdll32.dll", EntryPoint:="gsapi_revision")>
        Private Shared Function gsapi_revision(ByRef pGSRevisionInfo As GS_Revision, ByVal intLen As Integer) As Integer

        End Function
        <DllImport("gsdll32.dll", EntryPoint:="gsapi_set_stdio")>
        Private Shared Function gsapi_set_stdio(ByVal lngGSInstance As IntPtr, ByVal gsdll_stdin As StdioCallBack, ByVal gsdll_stdout As StdioCallBack, ByVal gsdll_stderr As StdioCallBack) As Integer

        End Function
        Const e_Quit As Integer = -101
        Const e_NeedInput As Integer = -106
        Private _sDeviceFormat As String
        Private _sParametersUsed As String
        Private _iWidth As Integer
        Private _iHeight As Integer
        Private _iResolutionX As Integer
        Private _iResolutionY As Integer
        Private _iJPEGQuality As Integer
        Private _iFirstPageToConvert As Integer = -1
        Private _iLastPageToConvert As Integer = -1
        Private _iGraphicsAlphaBit As Integer = -1
        Private _iTextAlphaBit As Integer = -1
        Private _iRenderingThreads As Integer = -1

        'Public Property RenderingThreads As Integer
        '    Get
        '        Return _iRenderingThreads
        '    End Get
        '    Set(ByVal value As Integer)

        '        If value = 0 Then
        '            _iRenderingThreads = Environment.ProcessorCount
        '        Else
        '            _iRenderingThreads = value
        '        End If
        '    End Set
        'End Property

        Private _bFitPage As Boolean
        Private _bThrowOnlyException As Boolean = False
        Private _bRedirectIO As Boolean = False
        Private _bForcePageSize As Boolean = False
        Private _sDefaultPageSize As String
        Private _objHandle As IntPtr
        Private _didOutputToMultipleFile As Boolean = False
        Private myProcess As System.Diagnostics.Process
        Public output As StringBuilder
        Private _sFontPath As List(Of String) = New List(Of String)()
        Private _bDisablePlatformFonts As Boolean = False
        Private _bDisableFontMap As Boolean = False
        Private _sFontMap As List(Of String) = New List(Of String)()
        Private _sSubstitutionFont As String
        Private _sFCOFontFile As String
        Private _sFAPIFontMap As String
        Private _bDisablePrecompiledFonts As Boolean = False

        Public Property OutputFormat As String
            Get
                Return _sDeviceFormat
            End Get
            Set(ByVal value As String)
                _sDeviceFormat = value
            End Set
        End Property

        Public Property DefaultPageSize As String
            Get
                Return _sDefaultPageSize
            End Get
            Set(ByVal value As String)
                _sDefaultPageSize = value
            End Set
        End Property

        Public Property ForcePageSize As Boolean
            Get
                Return _bForcePageSize
            End Get
            Set(ByVal value As Boolean)
                _bForcePageSize = value
            End Set
        End Property

        Public Property ParametersUsed As String
            Get
                Return _sParametersUsed
            End Get
            Set(ByVal value As String)
                _sParametersUsed = value
            End Set
        End Property

        Public Property Width As Integer
            Get
                Return _iWidth
            End Get
            Set(ByVal value As Integer)
                _iWidth = value
            End Set
        End Property

        Public Property Height As Integer
            Get
                Return _iHeight
            End Get
            Set(ByVal value As Integer)
                _iHeight = value
            End Set
        End Property

        Public Property ResolutionX As Integer
            Get
                Return _iResolutionX
            End Get
            Set(ByVal value As Integer)
                _iResolutionX = value
            End Set
        End Property

        Public Property ResolutionY As Integer
            Get
                Return _iResolutionY
            End Get
            Set(ByVal value As Integer)
                _iResolutionY = value
            End Set
        End Property

        Public Property GraphicsAlphaBit As Integer
            Get
                Return _iGraphicsAlphaBit
            End Get
            Set(ByVal value As Integer)
                If (value > 4) Or (value = 3) Then Throw New ArgumentOutOfRangeException("The Graphics Alpha Bit must have a value between 1 2 and 4, or <= 0 if not set")
                _iGraphicsAlphaBit = value
            End Set
        End Property

        Public Property TextAlphaBit As Integer
            Get
                Return _iTextAlphaBit
            End Get
            Set(ByVal value As Integer)
                If (value > 4) Or (value = 3) Then Throw New ArgumentOutOfRangeException("The Text Alpha Bit must have a value between 1 2 and 4, or <= 0 if not set")
                _iTextAlphaBit = value
            End Set
        End Property

        Public Property FitPage As Boolean
            Get
                Return _bFitPage
            End Get
            Set(ByVal value As Boolean)
                _bFitPage = value
            End Set
        End Property

        Public Property JPEGQuality As Integer
            Get
                Return _iJPEGQuality
            End Get
            Set(ByVal value As Integer)
                _iJPEGQuality = value
            End Set
        End Property

        Public Property FirstPageToConvert As Integer
            Get
                Return _iFirstPageToConvert
            End Get
            Set(ByVal value As Integer)
                _iFirstPageToConvert = value
            End Set
        End Property

        Public Property LastPageToConvert As Integer
            Get
                Return _iLastPageToConvert
            End Get
            Set(ByVal value As Integer)
                _iLastPageToConvert = value
            End Set
        End Property

        Public Property ThrowOnlyException As Boolean
            Get
                Return _bThrowOnlyException
            End Get
            Set(ByVal value As Boolean)
                _bThrowOnlyException = value
            End Set
        End Property

        Public Property RedirectIO As Boolean
            Get
                Return _bRedirectIO
            End Get
            Set(ByVal value As Boolean)
                _bRedirectIO = value
            End Set
        End Property

        Public Property OutputToMultipleFile As Boolean
            Get
                Return _didOutputToMultipleFile
            End Get
            Set(ByVal value As Boolean)
                _didOutputToMultipleFile = value
            End Set
        End Property

        Public Property UseMutex As Boolean
            Get
                Return mutex IsNot Nothing
            End Get
            Set(ByVal value As Boolean)

                If Not value Then

                    If mutex IsNot Nothing Then
                        mutex.ReleaseMutex()
                        mutex.Close()
                        mutex = Nothing
                    End If
                Else
                    If mutex Is Nothing Then mutex = New System.Threading.Mutex(False, "MutexGhostscript")
                End If
            End Set
        End Property

        Public Property FontPath As List(Of String)
            Get
                Return _sFontPath
            End Get
            Set(ByVal value As List(Of String))
                _sFontPath = value
            End Set
        End Property

        Public Property DisablePlatformFonts As Boolean
            Get
                Return _bDisablePlatformFonts
            End Get
            Set(ByVal value As Boolean)
                _bDisablePlatformFonts = value
            End Set
        End Property

        Public Property DisableFontMap As Boolean
            Get
                Return _bDisableFontMap
            End Get
            Set(ByVal value As Boolean)
                _bDisableFontMap = value
            End Set
        End Property

        Public Property FontMap As List(Of String)
            Get
                Return _sFontMap
            End Get
            Set(ByVal value As List(Of String))
                _sFontMap = value
            End Set
        End Property

        Public Property SubstitutionFont As String
            Get
                Return _sSubstitutionFont
            End Get
            Set(ByVal value As String)
                _sSubstitutionFont = value
            End Set
        End Property

        Public Property FCOFontFile As String
            Get
                Return _sFCOFontFile
            End Get
            Set(ByVal value As String)
                _sFCOFontFile = value
            End Set
        End Property

        Public Property FAPIFontMap As String
            Get
                Return _sFAPIFontMap
            End Get
            Set(ByVal value As String)
                _sFAPIFontMap = value
            End Set
        End Property

        Public Property DisablePrecompiledFonts As Boolean
            Get
                Return _bDisablePrecompiledFonts
            End Get
            Set(ByVal value As Boolean)
                _bDisablePrecompiledFonts = value
            End Set
        End Property

        Public Sub New(ByVal objHandle As IntPtr)
            _objHandle = objHandle
        End Sub

        Public Sub New()
            _objHandle = IntPtr.Zero
        End Sub

        Public Function Convert(ByVal inputFile As String, ByVal outputFile As String) As Boolean
            Return Convert(inputFile, outputFile, _bThrowOnlyException, Nothing)
        End Function

        Public Function Convert(ByVal inputFile As String, ByVal outputFile As String, ByVal attempts As Integer) As Boolean
            Try
                Return Convert(inputFile, outputFile, _bThrowOnlyException, Nothing)
            Catch ex As Exception
                attempts -= 1
                System.Threading.Thread.Sleep(1000 * attempts)

                If attempts <= 0 Then
                    Throw ex
                End If

                Return Convert(inputFile, outputFile, _bThrowOnlyException, Nothing)
            End Try
        End Function

        Public Function Convert(ByVal inputFile As String, ByVal outputFile As String, ByVal attempts As Integer, ByVal isViaProcess As Boolean) As Boolean
            Try
                Return Convert(inputFile, outputFile, _bThrowOnlyException, Nothing, isViaProcess)
            Catch ex As Exception
                attempts -= 1
                System.Threading.Thread.Sleep(1000 * attempts)

                If attempts <= 0 Then
                    Throw ex
                End If

                Return Convert(inputFile, outputFile, _bThrowOnlyException, Nothing, isViaProcess)
            End Try
        End Function

        Public Function Convert(ByVal inputFile As String, ByVal outputFile As String, ByVal parameters As String) As Boolean
            Return Convert(inputFile, outputFile, _bThrowOnlyException, parameters)
        End Function

        Private Function Convert(ByVal inputFile As String, ByVal outputFile As String, ByVal throwException As Boolean, ByVal options As String, ByVal IsViaProcess As Boolean) As Boolean
            If String.IsNullOrEmpty(inputFile) Then Throw New ArgumentNullException("inputFile")
            If Not System.IO.File.Exists(inputFile) Then Throw New ArgumentException(String.Format("The file :'{0}' doesn't exist", inputFile), "inputFile")
            If String.IsNullOrEmpty(_sDeviceFormat) Then Throw New ArgumentNullException("Device")
            If mutex IsNot Nothing Then mutex.WaitOne()
            Dim result As Boolean = False

            Try
                ''  Dim programPath As String = "F:\Ashish Data\Data\gs9.25\gs9.25\bin\gswin32c.exe"
                Dim programPath As String = ConfigurationManager.AppSettings("gs").ToString()
                If Not String.IsNullOrEmpty(programPath) Then
                    programPath = programPath
                Else
                    Throw New Exception("Ghostscript path is not found in config.")
                End If

                result = ExecuteGhostscriptCommand(GetGeneratedArgs(inputFile, outputFile, options, IsViaProcess), programPath)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If mutex IsNot Nothing Then mutex.ReleaseMutex()
            End Try

            Return result
        End Function

        Private Function Convert(ByVal inputFile As String, ByVal outputFile As String, ByVal throwException As Boolean, ByVal options As String) As Boolean
            If String.IsNullOrEmpty(inputFile) Then Throw New ArgumentNullException("inputFile")
            If Not System.IO.File.Exists(inputFile) Then Throw New ArgumentException(String.Format("The file :'{0}' doesn't exist", inputFile), "inputFile")
            If String.IsNullOrEmpty(_sDeviceFormat) Then Throw New ArgumentNullException("Device")
            If mutex IsNot Nothing Then mutex.WaitOne()
            Dim result As Boolean = False

            Try
                result = ExecuteGhostscriptCommand(GetGeneratedArgs(inputFile, outputFile, options))
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If mutex IsNot Nothing Then mutex.ReleaseMutex()
            End Try

            Return result
        End Function

        Public Function Print(ByVal inputFile As String, ByVal printParametersFile As String) As Boolean
            If String.IsNullOrEmpty(inputFile) Then Throw New ArgumentNullException("inputFile")
            If Not System.IO.File.Exists(inputFile) Then Throw New ArgumentException(String.Format("The file :'{0}' doesn't exist", inputFile), "inputFile")
            If String.IsNullOrEmpty(printParametersFile) Then Throw New ArgumentNullException("printParametersFile")
            If Not System.IO.File.Exists(printParametersFile) Then Throw New ArgumentException(String.Format("The file :'{0}' doesn't exist", printParametersFile), "printParametersFile")
            Dim args As List(Of String) = New List(Of String)(7)
            args.Add("printPdf")
            args.Add("-dNOPAUSE")
            args.Add("-dBATCH")
            If _iFirstPageToConvert > 0 Then args.Add(String.Format("-dFirstPage={0}", _iFirstPageToConvert))
            If (_iLastPageToConvert > 0) AndAlso (_iLastPageToConvert >= _iFirstPageToConvert) Then args.Add(String.Format("-dLastPage={0}", _iLastPageToConvert))
            args.Add(printParametersFile)
            args.Add(inputFile)
            Dim result As Boolean = False
            If mutex IsNot Nothing Then mutex.WaitOne()

            Try
                result = ExecuteGhostscriptCommand(args.ToArray())
            Finally
                If mutex IsNot Nothing Then mutex.ReleaseMutex()
            End Try

            Return result
        End Function

        Private Function ExecuteGhostscriptCommand(ByVal command As String, ByVal programPath As String) As Boolean
            Dim procStartInfo As System.Diagnostics.ProcessStartInfo = New System.Diagnostics.ProcessStartInfo("cmd", "/c " & Path.GetFileName(programPath) & command)
            procStartInfo.RedirectStandardOutput = True
            procStartInfo.RedirectStandardError = True
            procStartInfo.UseShellExecute = False
            procStartInfo.CreateNoWindow = True
            procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            procStartInfo.WorkingDirectory = Path.GetDirectoryName(programPath)
            Dim proc As System.Diagnostics.Process = New System.Diagnostics.Process()
            proc.StartInfo = procStartInfo

            Try
                proc.Start()
                Dim result As String = proc.StandardError.ReadToEnd()
                proc.WaitForExit()

                If result.Trim() <> String.Empty Then
                    proc.Close()
                    Return False
                End If

                Return True
            Catch ex As Exception
                Throw New Exception("Error in image extraction via process")
            Finally
                proc.Close()
            End Try
        End Function

        Private Function ExecuteGhostscriptCommand(ByVal sArgs As String()) As Boolean
            Dim intReturn, intCounter, intElementCount As Integer
            Dim intGSInstanceHandle As IntPtr = IntPtr.Zero
            Dim aAnsiArgs As Object()
            Dim aPtrArgs As IntPtr()
            Dim aGCHandle As GCHandle()
            Dim callerHandle, intptrArgs As IntPtr
            Dim gchandleArgs As GCHandle
            intElementCount = sArgs.Length
            aAnsiArgs = New Object(intElementCount - 1) {}
            aPtrArgs = New IntPtr(intElementCount - 1) {}
            aGCHandle = New GCHandle(intElementCount - 1) {}

            For intCounter = 0 To intElementCount - 1
                aAnsiArgs(intCounter) = StringToAnsiZ(sArgs(intCounter))
                aGCHandle(intCounter) = GCHandle.Alloc(aAnsiArgs(intCounter), GCHandleType.Pinned)
                aPtrArgs(intCounter) = aGCHandle(intCounter).AddrOfPinnedObject()
            Next

            gchandleArgs = GCHandle.Alloc(aPtrArgs, GCHandleType.Pinned)
            intptrArgs = gchandleArgs.AddrOfPinnedObject()
            intReturn = -1

            Try
                intReturn = gsapi_new_instance(intGSInstanceHandle, _objHandle)

                If intReturn < 0 Then
                    ClearParameters(aGCHandle, gchandleArgs)
                    Throw New ApplicationException("I can't create a new istance of Ghostscript please verify no other istance are running!")
                End If

            Catch formatException As BadImageFormatException
                ClearParameters(aGCHandle, gchandleArgs)

                If IntPtr.Size = 8 Then
                    Throw New ApplicationException(String.Format("The gsdll32.dll you provide is not compatible with the current architecture that is 64bit," & "Please download any version above version 8.64 from the original website in the 64bit or x64 or AMD64 version!"))
                ElseIf IntPtr.Size = 4 Then
                    Throw New ApplicationException(String.Format("The gsdll32.dll you provide is not compatible with the current architecture that is 32bit," & "Please download any version above version 8.64 from the original website in the 32bit or x86 or i386 version!"))
                End If

            Catch ex As DllNotFoundException
                ClearParameters(aGCHandle, gchandleArgs)
                Throw New ApplicationException("The gsdll32.dll wasn't found in default dlls search path" & "or is not in correct version (doesn't expose the required methods). Please download " & "at least the version 8.64 from the original website")
            End Try

            callerHandle = IntPtr.Zero

            If _bRedirectIO Then
                Dim stdinCallback As StdioCallBack = New StdioCallBack(AddressOf gsdll_stdin)
                Dim stdoutCallback As StdioCallBack = New StdioCallBack(AddressOf gsdll_stdout)
                Dim stderrCallback As StdioCallBack = New StdioCallBack(AddressOf gsdll_stderr)
                intReturn = gsapi_set_stdio(intGSInstanceHandle, stdinCallback, stdoutCallback, stderrCallback)

                If output Is Nothing Then
                    output = New StringBuilder()
                Else
                    output.Remove(0, output.Length)
                End If

                myProcess = System.Diagnostics.Process.GetCurrentProcess()
                AddHandler myProcess.OutputDataReceived, New System.Diagnostics.DataReceivedEventHandler(AddressOf SaveOutputToImage)
            End If

            intReturn = -1

            Try
                intReturn = gsapi_init_with_args(intGSInstanceHandle, intElementCount, intptrArgs)
            Catch ex As Exception
                Throw New ApplicationException(ex.Message, ex)
            Finally
                ClearParameters(aGCHandle, gchandleArgs)
                gsapi_exit(intGSInstanceHandle)
                gsapi_delete_instance(intGSInstanceHandle)
                If (myProcess IsNot Nothing) AndAlso (_bRedirectIO) Then RemoveHandler myProcess.OutputDataReceived, New System.Diagnostics.DataReceivedEventHandler(AddressOf SaveOutputToImage)
            End Try

            Return (intReturn = 0) Or (intReturn = e_Quit)
        End Function

        Private Function ExecuteGhostscriptCommand(ByVal sArgs As String(), ByVal attempt As Integer) As Boolean
            Dim intReturn, intCounter, intElementCount As Integer
            Dim intGSInstanceHandle As IntPtr = IntPtr.Zero
            Dim aAnsiArgs As Object()
            Dim aPtrArgs As IntPtr()
            Dim aGCHandle As GCHandle()
            Dim callerHandle, intptrArgs As IntPtr
            Dim gchandleArgs As GCHandle
            intElementCount = sArgs.Length
            aAnsiArgs = New Object(intElementCount - 1) {}
            aPtrArgs = New IntPtr(intElementCount - 1) {}
            aGCHandle = New GCHandle(intElementCount - 1) {}

            For intCounter = 0 To intElementCount - 1
                aAnsiArgs(intCounter) = StringToAnsiZ(sArgs(intCounter))
                aGCHandle(intCounter) = GCHandle.Alloc(aAnsiArgs(intCounter), GCHandleType.Pinned)
                aPtrArgs(intCounter) = aGCHandle(intCounter).AddrOfPinnedObject()
            Next

            gchandleArgs = GCHandle.Alloc(aPtrArgs, GCHandleType.Pinned)
            intptrArgs = gchandleArgs.AddrOfPinnedObject()
            intReturn = -1

            Try
                intReturn = gsapi_new_instance(intGSInstanceHandle, _objHandle)

                If intReturn < 0 Then
                    ClearParameters(aGCHandle, gchandleArgs)

                    If attempt = 0 Then
                        Throw New ApplicationException("I can't create a new istance of Ghostscript please verify no other istance are running!")
                    Else
                        ExecuteGhostscriptCommand(sArgs, System.Threading.Interlocked.Decrement(attempt))
                    End If
                End If

            Catch formatException As BadImageFormatException
                ClearParameters(aGCHandle, gchandleArgs)

                If IntPtr.Size = 8 Then
                    Throw New ApplicationException(String.Format("The gsdll32.dll you provide is not compatible with the current architecture that is 64bit," & "Please download any version above version 8.64 from the original website in the 64bit or x64 or AMD64 version!"))
                ElseIf IntPtr.Size = 4 Then
                    Throw New ApplicationException(String.Format("The gsdll32.dll you provide is not compatible with the current architecture that is 32bit," & "Please download any version above version 8.64 from the original website in the 32bit or x86 or i386 version!"))
                End If

            Catch ex As DllNotFoundException
                ClearParameters(aGCHandle, gchandleArgs)
                Throw New ApplicationException("The gsdll32.dll wasn't found in default dlls search path" & "or is not in correct version (doesn't expose the required methods). Please download " & "at least the version 8.64 from the original website")
            End Try

            callerHandle = IntPtr.Zero

            If _bRedirectIO Then
                Dim stdinCallback As StdioCallBack = New StdioCallBack(AddressOf gsdll_stdin)
                Dim stdoutCallback As StdioCallBack = New StdioCallBack(AddressOf gsdll_stdout)
                Dim stderrCallback As StdioCallBack = New StdioCallBack(AddressOf gsdll_stderr)
                intReturn = gsapi_set_stdio(intGSInstanceHandle, stdinCallback, stdoutCallback, stderrCallback)

                If output Is Nothing Then
                    output = New StringBuilder()
                Else
                    output.Remove(0, output.Length)
                End If

                myProcess = System.Diagnostics.Process.GetCurrentProcess()
                AddHandler myProcess.OutputDataReceived, New System.Diagnostics.DataReceivedEventHandler(AddressOf SaveOutputToImage)
            End If

            intReturn = -1

            Try
                intReturn = gsapi_init_with_args(intGSInstanceHandle, intElementCount, intptrArgs)
            Catch ex As Exception
                Throw New ApplicationException(ex.Message, ex)
            Finally
                ClearParameters(aGCHandle, gchandleArgs)
                gsapi_exit(intGSInstanceHandle)
                gsapi_delete_instance(intGSInstanceHandle)
                If (myProcess IsNot Nothing) AndAlso (_bRedirectIO) Then RemoveHandler myProcess.OutputDataReceived, New System.Diagnostics.DataReceivedEventHandler(AddressOf SaveOutputToImage)
            End Try

            Return (intReturn = 0) Or (intReturn = e_Quit)
        End Function

        Private Sub ClearParameters(ByRef aGCHandle As GCHandle(), ByRef gchandleArgs As GCHandle)
            For intCounter As Integer = 0 To aGCHandle.Length - 1
                aGCHandle(intCounter).Free()
            Next

            gchandleArgs.Free()
        End Sub

        Private Sub SaveOutputToImage(ByVal sender As Object, ByVal e As System.Diagnostics.DataReceivedEventArgs)
            output.Append(e.Data)
        End Sub

        Private Function GetGeneratedArgs(ByVal inputFile As String, ByVal outputFile As String, ByVal otherParameters As String) As String()
            If Not String.IsNullOrEmpty(otherParameters) Then
                Return GetGeneratedArgs(inputFile, outputFile, otherParameters.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries))
            Else
                Return GetGeneratedArgs(inputFile, outputFile, CType(Nothing, String()))
            End If
        End Function

        Private Function GetGeneratedArgs(ByVal inputFile As String, ByVal outputFile As String, ByVal otherParameters As String, ByVal IsStringCommand As Boolean) As String
            If Not String.IsNullOrEmpty(otherParameters) Then
                Return GetGeneratedArgs(inputFile, outputFile, otherParameters.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries), IsStringCommand)
            Else
                Return GetGeneratedArgs(inputFile, outputFile, CType(Nothing, String()), IsStringCommand)
            End If
        End Function

        Private Function GetGeneratedArgs(ByVal inputFile As String, ByVal outputFile As String, ByVal presetParameters As String()) As String()
            Dim args As String()
            Dim lstExtraArgs As ArrayList = New ArrayList()

            If (presetParameters Is Nothing) OrElse (presetParameters.Length = 0) Then
                If _sDeviceFormat = "jpeg" AndAlso _iJPEGQuality > 0 AndAlso _iJPEGQuality < 101 Then lstExtraArgs.Add(String.Format(GS_JpegQualityFormat, _iJPEGQuality))

                If _iWidth > 0 AndAlso _iHeight > 0 Then
                    lstExtraArgs.Add(String.Format(GS_PageSizeFormat, _iWidth, _iHeight))
                Else

                    If Not String.IsNullOrEmpty(_sDefaultPageSize) Then
                        lstExtraArgs.Add(String.Format(GS_DefaultPaperSize, _sDefaultPageSize))
                        If _bForcePageSize Then lstExtraArgs.Add(GS_FixedMedia)
                    End If
                End If

                If _iGraphicsAlphaBit > 0 Then lstExtraArgs.Add(String.Format(GS_GraphicsAlphaBits, _iGraphicsAlphaBit))
                If _iTextAlphaBit > 0 Then lstExtraArgs.Add(String.Format(GS_TextAlphaBits, _iTextAlphaBit))
                If _bFitPage Then lstExtraArgs.Add(GS_FitPage)

                If _iResolutionX > 0 Then

                    If _iResolutionY > 0 Then
                        lstExtraArgs.Add(String.Format(GS_ResolutionXYFormat, _iResolutionX, _iResolutionY))
                    Else
                        lstExtraArgs.Add(String.Format(GS_ResolutionXFormat, _iResolutionX))
                    End If
                End If

                If _iFirstPageToConvert > 0 Then lstExtraArgs.Add(String.Format(GS_FirstPageFormat, _iFirstPageToConvert))

                If _iLastPageToConvert > 0 Then
                    If (_iFirstPageToConvert > 0) AndAlso (_iFirstPageToConvert > _iLastPageToConvert) Then Throw New ArgumentOutOfRangeException(String.Format("The 1st page to convert ({0}) can't be after then the last one ({1})", _iFirstPageToConvert, _iLastPageToConvert))
                    lstExtraArgs.Add(String.Format(GS_LastPageFormat, _iLastPageToConvert))
                End If

                _iRenderingThreads = 5
                If _iRenderingThreads > 0 Then lstExtraArgs.Add(String.Format(GS_RenderingThreads, _iRenderingThreads))

                If _bRedirectIO Then
                End If

                If (_sFontPath IsNot Nothing) AndAlso (_sFontPath.Count > 0) Then lstExtraArgs.Add(String.Format(GS_FontPath, String.Join(";", _sFontPath.ToArray())))
                If _bDisablePlatformFonts Then lstExtraArgs.Add(GS_NoPlatformFonts)
                If _bDisableFontMap Then lstExtraArgs.Add(GS_NoFontMap)
                If (_sFontMap IsNot Nothing) AndAlso (_sFontMap.Count > 0) Then lstExtraArgs.Add(String.Format(GS_FontMap, String.Join(";", _sFontMap.ToArray())))
                If Not String.IsNullOrEmpty(_sSubstitutionFont) Then lstExtraArgs.Add(String.Format(GS_SubstitutionFont, _sSubstitutionFont))
                If Not String.IsNullOrEmpty(_sFCOFontFile) Then lstExtraArgs.Add(String.Format(GS_FCOFontFile, _sFCOFontFile))

                If Not String.IsNullOrEmpty(_sFAPIFontMap) Then
                    lstExtraArgs.Add(String.Format(GS_FAPIFontMap, _sFAPIFontMap))
                End If

                If _bDisablePrecompiledFonts Then lstExtraArgs.Add(GS_NoPrecompiledFonts)
                Dim iFixedCount As Integer = 7
                Dim iExtraArgsCount As Integer = lstExtraArgs.Count
                args = New String(iFixedCount + lstExtraArgs.Count - 1) {}
                args(1) = GS_Fixed1stParameter
                args(2) = GS_Fixed2ndParameter
                args(3) = GS_Fixed3rdParameter
                args(4) = String.Format(GS_DeviceFormat, _sDeviceFormat)

                For i As Integer = 0 To iExtraArgsCount - 1
                    args(5 + i) = CStr(lstExtraArgs(i))
                Next
            Else
                args = New String(presetParameters.Length + 3 - 1) {}

                For i As Integer = 1 To presetParameters.Length
                    args(i) = presetParameters(i - 1)
                Next
            End If

            args(0) = GS_FirstParameter

            If (_didOutputToMultipleFile) AndAlso (Not outputFile.Contains(GS_MultiplePageCharacter)) Then
                Dim lastDotIndex As Integer = outputFile.LastIndexOf("."c)
                If lastDotIndex > 0 Then outputFile = outputFile.Insert(lastDotIndex, "%d")
            End If

            _sParametersUsed = String.Empty

            For i As Integer = 1 To args.Length - 2 - 1
                _sParametersUsed += " " & args(i)
            Next

            args(args.Length - 2) = String.Format(GS_OutputFileFormat, outputFile)
            args(args.Length - 1) = String.Format("{0}", inputFile)
            _sParametersUsed += " " & String.Format(GS_OutputFileFormat, String.Format("""{0}""", outputFile)) & " " & String.Format("""{0}""", inputFile)
            Return args
        End Function

        Private Function GetGeneratedArgs(ByVal inputFile As String, ByVal outputFile As String, ByVal presetParameters As String(), ByVal IsStringCommand As Boolean) As String
            Dim args As String()
            Dim lstExtraArgs As ArrayList = New ArrayList()

            If (presetParameters Is Nothing) OrElse (presetParameters.Length = 0) Then
                If _sDeviceFormat = "jpeg" AndAlso _iJPEGQuality > 0 AndAlso _iJPEGQuality < 101 Then lstExtraArgs.Add(String.Format(GS_JpegQualityFormat, _iJPEGQuality))

                If _iWidth > 0 AndAlso _iHeight > 0 Then
                    lstExtraArgs.Add(String.Format(GS_PageSizeFormat, _iWidth, _iHeight))
                Else

                    If Not String.IsNullOrEmpty(_sDefaultPageSize) Then
                        lstExtraArgs.Add(String.Format(GS_DefaultPaperSize, _sDefaultPageSize))
                        If _bForcePageSize Then lstExtraArgs.Add(GS_FixedMedia)
                    End If
                End If

                If _iGraphicsAlphaBit > 0 Then lstExtraArgs.Add(String.Format(GS_GraphicsAlphaBits, _iGraphicsAlphaBit))
                If _iTextAlphaBit > 0 Then lstExtraArgs.Add(String.Format(GS_TextAlphaBits, _iTextAlphaBit))
                If _bFitPage Then lstExtraArgs.Add(GS_FitPage)

                If _iResolutionX > 0 Then

                    If _iResolutionY > 0 Then
                        lstExtraArgs.Add(String.Format(GS_ResolutionXYFormat, _iResolutionX, _iResolutionY))
                    Else
                        lstExtraArgs.Add(String.Format(GS_ResolutionXFormat, _iResolutionX))
                    End If
                End If

                If _iFirstPageToConvert > 0 Then lstExtraArgs.Add(String.Format(GS_FirstPageFormat, _iFirstPageToConvert))

                If _iLastPageToConvert > 0 Then
                    If (_iFirstPageToConvert > 0) AndAlso (_iFirstPageToConvert > _iLastPageToConvert) Then Throw New ArgumentOutOfRangeException(String.Format("The 1st page to convert ({0}) can't be after then the last one ({1})", _iFirstPageToConvert, _iLastPageToConvert))
                    lstExtraArgs.Add(String.Format(GS_LastPageFormat, _iLastPageToConvert))
                End If

                _iRenderingThreads = 5
                If _iRenderingThreads > 0 Then lstExtraArgs.Add(String.Format(GS_RenderingThreads, _iRenderingThreads))

                If _bRedirectIO Then
                End If

                If (_sFontPath IsNot Nothing) AndAlso (_sFontPath.Count > 0) Then lstExtraArgs.Add(String.Format(GS_FontPath, String.Join(";", _sFontPath.ToArray())))
                If _bDisablePlatformFonts Then lstExtraArgs.Add(GS_NoPlatformFonts)
                If _bDisableFontMap Then lstExtraArgs.Add(GS_NoFontMap)
                If (_sFontMap IsNot Nothing) AndAlso (_sFontMap.Count > 0) Then lstExtraArgs.Add(String.Format(GS_FontMap, String.Join(";", _sFontMap.ToArray())))
                If Not String.IsNullOrEmpty(_sSubstitutionFont) Then lstExtraArgs.Add(String.Format(GS_SubstitutionFont, _sSubstitutionFont))
                If Not String.IsNullOrEmpty(_sFCOFontFile) Then lstExtraArgs.Add(String.Format(GS_FCOFontFile, _sFCOFontFile))

                If Not String.IsNullOrEmpty(_sFAPIFontMap) Then
                    lstExtraArgs.Add(String.Format(GS_FAPIFontMap, _sFAPIFontMap))
                End If

                If _bDisablePrecompiledFonts Then lstExtraArgs.Add(GS_NoPrecompiledFonts)
                Dim iFixedCount As Integer = 7
                Dim iExtraArgsCount As Integer = lstExtraArgs.Count
                args = New String(iFixedCount + lstExtraArgs.Count - 1) {}
                args(1) = GS_Fixed1stParameter
                args(2) = GS_Fixed2ndParameter
                args(3) = GS_Fixed3rdParameter
                args(4) = String.Format(GS_DeviceFormat, _sDeviceFormat)

                For i As Integer = 0 To iExtraArgsCount - 1
                    args(5 + i) = CStr(lstExtraArgs(i))
                Next
            Else
                args = New String(presetParameters.Length + 3 - 1) {}

                For i As Integer = 1 To presetParameters.Length
                    args(i) = presetParameters(i - 1)
                Next
            End If

            args(0) = GS_FirstParameter

            If (_didOutputToMultipleFile) AndAlso (Not outputFile.Contains(GS_MultiplePageCharacter)) Then
                Dim lastDotIndex As Integer = outputFile.LastIndexOf("."c)
                If lastDotIndex > 0 Then outputFile = outputFile.Insert(lastDotIndex, "%d")
            End If

            _sParametersUsed = String.Empty

            For i As Integer = 1 To args.Length - 2 - 1
                _sParametersUsed += " " & args(i)
            Next

            args(args.Length - 2) = String.Format(GS_OutputFileFormat, outputFile)
            args(args.Length - 1) = String.Format("{0}", inputFile)
            _sParametersUsed += " " & String.Format(GS_OutputFileFormat, String.Format("""{0}""", outputFile)) & " " & String.Format("""{0}""", inputFile)
            Return _sParametersUsed
        End Function

        Private Shared Function StringToAnsiZ(ByVal str As String) As Byte()
            If str Is Nothing Then str = String.Empty
            Return Encoding.[Default].GetBytes(str)
        End Function

        Public Shared Function AnsiZtoString(ByVal strz As IntPtr) As String
            If strz <> IntPtr.Zero Then
                Return Marshal.PtrToStringAnsi(strz)
            Else
                Return String.Empty
            End If
        End Function

        Public Shared Function CheckDll() As Boolean
            Return File.Exists(GhostScriptDLLName)
        End Function

        Public Function gsdll_stdin(ByVal intGSInstanceHandle As IntPtr, ByVal strz As IntPtr, ByVal intBytes As Integer) As Integer
            If intBytes = 0 Then
                Return 0
            Else
                Dim ich As Integer = Console.Read()

                If ich = -1 Then
                    Return 0
                Else
                    Dim bch As Byte = CByte(ich)
                    Dim gcByte As GCHandle = GCHandle.Alloc(bch, GCHandleType.Pinned)
                    Dim ptrByte As IntPtr = gcByte.AddrOfPinnedObject()
                    CopyMemory(strz, ptrByte, 1)
                    ptrByte = IntPtr.Zero
                    gcByte.Free()
                    Return 1
                End If
            End If
        End Function

        Public Function gsdll_stdout(ByVal intGSInstanceHandle As IntPtr, ByVal strz As IntPtr, ByVal intBytes As Integer) As Integer
            Dim aByte As Byte() = New Byte(intBytes - 1) {}
            Dim gcByte As GCHandle = GCHandle.Alloc(aByte, GCHandleType.Pinned)
            Dim ptrByte As IntPtr = gcByte.AddrOfPinnedObject()
            CopyMemory(ptrByte, strz, CUInt(intBytes))
            ptrByte = IntPtr.Zero
            gcByte.Free()
            Dim str As String = ""

            For i As Integer = 0 To intBytes - 1
                str += ChrW(aByte(i))
            Next

            output.Append(str)
            Return intBytes
        End Function

        Public Function gsdll_stderr(ByVal intGSInstanceHandle As IntPtr, ByVal strz As IntPtr, ByVal intBytes As Integer) As Integer
            Return gsdll_stdout(intGSInstanceHandle, strz, intBytes)
        End Function

        Public Function GetRevision() As GhostScriptRevision
            Dim intReturn As Integer
            Dim udtGSRevInfo As GS_Revision = New GS_Revision()
            Dim output As GhostScriptRevision
            Dim gcRevision As GCHandle
            gcRevision = GCHandle.Alloc(udtGSRevInfo, GCHandleType.Pinned)
            intReturn = gsapi_revision(udtGSRevInfo, 16)
            output.intRevision = udtGSRevInfo.intRevision
            output.intRevisionDate = udtGSRevInfo.intRevisionDate
            output.ProductInformation = AnsiZtoString(udtGSRevInfo.strProduct)
            output.CopyrightInformations = AnsiZtoString(udtGSRevInfo.strCopyright)
            gcRevision.Free()
            Return output
        End Function
    End Class

    Public Delegate Function StdioCallBack(ByVal handle As IntPtr, ByVal strptr As IntPtr, ByVal count As Integer) As Integer

    <StructLayout(LayoutKind.Sequential)>
    Structure GS_Revision
        Public strProduct As IntPtr
        Public strCopyright As IntPtr
        Public intRevision As Integer
        Public intRevisionDate As Integer
    End Structure

    Public Structure GhostScriptRevision
        Public ProductInformation As String
        Public CopyrightInformations As String
        Public intRevision As Integer
        Public intRevisionDate As Integer
    End Structure
End Namespace
