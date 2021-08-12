<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Searching
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Searching))
        Me.lbl_CaseType = New System.Windows.Forms.Label()
        Me.lbl_FirstName = New System.Windows.Forms.Label()
        Me.lbl_MiddleName = New System.Windows.Forms.Label()
        Me.lbl_LastName = New System.Windows.Forms.Label()
        Me.lbl_BusinessName = New System.Windows.Forms.Label()
        Me.lbl_CaseNo = New System.Windows.Forms.Label()
        Me.lbl_CitationNo = New System.Windows.Forms.Label()
        Me.lbl_DateFrom = New System.Windows.Forms.Label()
        Me.lbl_DateTo = New System.Windows.Forms.Label()
        Me.btn_search = New System.Windows.Forms.Button()
        Me.txtBox_FirstName = New System.Windows.Forms.TextBox()
        Me.txtBox_MiddleName = New System.Windows.Forms.TextBox()
        Me.txtBox_LastName = New System.Windows.Forms.TextBox()
        Me.txtBox_BusinessName = New System.Windows.Forms.TextBox()
        Me.txtBox_CaseNo = New System.Windows.Forms.TextBox()
        Me.txtBox_CitationNo = New System.Windows.Forms.TextBox()
        Me.dateTo = New System.Windows.Forms.DateTimePicker()
        Me.dateFrom = New System.Windows.Forms.DateTimePicker()
        Me.comboBox_caseType = New System.Windows.Forms.ComboBox()
        Me.lbl_Info = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbl_CaseType
        '
        Me.lbl_CaseType.AutoSize = True
        Me.lbl_CaseType.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CaseType.Location = New System.Drawing.Point(152, 178)
        Me.lbl_CaseType.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_CaseType.Name = "lbl_CaseType"
        Me.lbl_CaseType.Size = New System.Drawing.Size(105, 23)
        Me.lbl_CaseType.TabIndex = 0
        Me.lbl_CaseType.Text = "Case Type"
        '
        'lbl_FirstName
        '
        Me.lbl_FirstName.AutoSize = True
        Me.lbl_FirstName.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_FirstName.Location = New System.Drawing.Point(152, 228)
        Me.lbl_FirstName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_FirstName.Name = "lbl_FirstName"
        Me.lbl_FirstName.Size = New System.Drawing.Size(106, 23)
        Me.lbl_FirstName.TabIndex = 2
        Me.lbl_FirstName.Text = "First Name"
        '
        'lbl_MiddleName
        '
        Me.lbl_MiddleName.AutoSize = True
        Me.lbl_MiddleName.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_MiddleName.Location = New System.Drawing.Point(133, 270)
        Me.lbl_MiddleName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_MiddleName.Name = "lbl_MiddleName"
        Me.lbl_MiddleName.Size = New System.Drawing.Size(125, 23)
        Me.lbl_MiddleName.TabIndex = 4
        Me.lbl_MiddleName.Text = "Middle Name"
        '
        'lbl_LastName
        '
        Me.lbl_LastName.AutoSize = True
        Me.lbl_LastName.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_LastName.Location = New System.Drawing.Point(152, 313)
        Me.lbl_LastName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_LastName.Name = "lbl_LastName"
        Me.lbl_LastName.Size = New System.Drawing.Size(105, 23)
        Me.lbl_LastName.TabIndex = 6
        Me.lbl_LastName.Text = "Last Name"
        '
        'lbl_BusinessName
        '
        Me.lbl_BusinessName.AutoSize = True
        Me.lbl_BusinessName.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_BusinessName.Location = New System.Drawing.Point(109, 354)
        Me.lbl_BusinessName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_BusinessName.Name = "lbl_BusinessName"
        Me.lbl_BusinessName.Size = New System.Drawing.Size(145, 23)
        Me.lbl_BusinessName.TabIndex = 8
        Me.lbl_BusinessName.Text = "Business Name"
        '
        'lbl_CaseNo
        '
        Me.lbl_CaseNo.AutoSize = True
        Me.lbl_CaseNo.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CaseNo.Location = New System.Drawing.Point(125, 396)
        Me.lbl_CaseNo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_CaseNo.Name = "lbl_CaseNo"
        Me.lbl_CaseNo.Size = New System.Drawing.Size(130, 23)
        Me.lbl_CaseNo.TabIndex = 10
        Me.lbl_CaseNo.Text = "Case Number"
        '
        'lbl_CitationNo
        '
        Me.lbl_CitationNo.AutoSize = True
        Me.lbl_CitationNo.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CitationNo.Location = New System.Drawing.Point(107, 438)
        Me.lbl_CitationNo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_CitationNo.Name = "lbl_CitationNo"
        Me.lbl_CitationNo.Size = New System.Drawing.Size(150, 23)
        Me.lbl_CitationNo.TabIndex = 12
        Me.lbl_CitationNo.Text = "Citation Number"
        '
        'lbl_DateFrom
        '
        Me.lbl_DateFrom.AutoSize = True
        Me.lbl_DateFrom.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_DateFrom.Location = New System.Drawing.Point(157, 482)
        Me.lbl_DateFrom.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_DateFrom.Name = "lbl_DateFrom"
        Me.lbl_DateFrom.Size = New System.Drawing.Size(104, 23)
        Me.lbl_DateFrom.TabIndex = 14
        Me.lbl_DateFrom.Text = "Date From"
        '
        'lbl_DateTo
        '
        Me.lbl_DateTo.AutoSize = True
        Me.lbl_DateTo.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_DateTo.Location = New System.Drawing.Point(176, 526)
        Me.lbl_DateTo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_DateTo.Name = "lbl_DateTo"
        Me.lbl_DateTo.Size = New System.Drawing.Size(79, 23)
        Me.lbl_DateTo.TabIndex = 16
        Me.lbl_DateTo.Text = "Date To"
        '
        'btn_search
        '
        Me.btn_search.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_search.BackColor = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(172, Byte), Integer))
        Me.btn_search.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_search.ForeColor = System.Drawing.Color.White
        Me.btn_search.Location = New System.Drawing.Point(269, 582)
        Me.btn_search.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_search.Name = "btn_search"
        Me.btn_search.Size = New System.Drawing.Size(235, 66)
        Me.btn_search.TabIndex = 18
        Me.btn_search.Text = "Search"
        Me.btn_search.UseVisualStyleBackColor = False
        '
        'txtBox_FirstName
        '
        Me.txtBox_FirstName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBox_FirstName.Font = New System.Drawing.Font("Arial", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBox_FirstName.Location = New System.Drawing.Point(269, 222)
        Me.txtBox_FirstName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtBox_FirstName.Name = "txtBox_FirstName"
        Me.txtBox_FirstName.Size = New System.Drawing.Size(511, 34)
        Me.txtBox_FirstName.TabIndex = 3
        '
        'txtBox_MiddleName
        '
        Me.txtBox_MiddleName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBox_MiddleName.Font = New System.Drawing.Font("Arial", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBox_MiddleName.Location = New System.Drawing.Point(269, 263)
        Me.txtBox_MiddleName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtBox_MiddleName.Name = "txtBox_MiddleName"
        Me.txtBox_MiddleName.Size = New System.Drawing.Size(511, 34)
        Me.txtBox_MiddleName.TabIndex = 5
        '
        'txtBox_LastName
        '
        Me.txtBox_LastName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBox_LastName.Font = New System.Drawing.Font("Arial", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBox_LastName.Location = New System.Drawing.Point(269, 306)
        Me.txtBox_LastName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtBox_LastName.Name = "txtBox_LastName"
        Me.txtBox_LastName.Size = New System.Drawing.Size(511, 34)
        Me.txtBox_LastName.TabIndex = 7
        '
        'txtBox_BusinessName
        '
        Me.txtBox_BusinessName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBox_BusinessName.Font = New System.Drawing.Font("Arial", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBox_BusinessName.Location = New System.Drawing.Point(269, 348)
        Me.txtBox_BusinessName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtBox_BusinessName.Name = "txtBox_BusinessName"
        Me.txtBox_BusinessName.Size = New System.Drawing.Size(511, 34)
        Me.txtBox_BusinessName.TabIndex = 9
        '
        'txtBox_CaseNo
        '
        Me.txtBox_CaseNo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBox_CaseNo.Font = New System.Drawing.Font("Arial", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBox_CaseNo.Location = New System.Drawing.Point(269, 390)
        Me.txtBox_CaseNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtBox_CaseNo.Name = "txtBox_CaseNo"
        Me.txtBox_CaseNo.Size = New System.Drawing.Size(511, 34)
        Me.txtBox_CaseNo.TabIndex = 11
        '
        'txtBox_CitationNo
        '
        Me.txtBox_CitationNo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBox_CitationNo.Font = New System.Drawing.Font("Arial", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBox_CitationNo.Location = New System.Drawing.Point(269, 432)
        Me.txtBox_CitationNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtBox_CitationNo.Name = "txtBox_CitationNo"
        Me.txtBox_CitationNo.Size = New System.Drawing.Size(511, 34)
        Me.txtBox_CitationNo.TabIndex = 13
        '
        'dateTo
        '
        Me.dateTo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dateTo.Font = New System.Drawing.Font("Arial", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dateTo.Location = New System.Drawing.Point(269, 517)
        Me.dateTo.Margin = New System.Windows.Forms.Padding(4)
        Me.dateTo.Name = "dateTo"
        Me.dateTo.Size = New System.Drawing.Size(235, 34)
        Me.dateTo.TabIndex = 17
        '
        'dateFrom
        '
        Me.dateFrom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dateFrom.Font = New System.Drawing.Font("Arial", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dateFrom.Location = New System.Drawing.Point(269, 475)
        Me.dateFrom.Margin = New System.Windows.Forms.Padding(4)
        Me.dateFrom.Name = "dateFrom"
        Me.dateFrom.Size = New System.Drawing.Size(235, 34)
        Me.dateFrom.TabIndex = 15
        '
        'comboBox_caseType
        '
        Me.comboBox_caseType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.comboBox_caseType.Font = New System.Drawing.Font("Arial", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.comboBox_caseType.FormattingEnabled = True
        Me.comboBox_caseType.Location = New System.Drawing.Point(269, 172)
        Me.comboBox_caseType.Margin = New System.Windows.Forms.Padding(4)
        Me.comboBox_caseType.Name = "comboBox_caseType"
        Me.comboBox_caseType.Size = New System.Drawing.Size(511, 34)
        Me.comboBox_caseType.TabIndex = 1
        '
        'lbl_Info
        '
        Me.lbl_Info.AutoSize = True
        Me.lbl_Info.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Info.Location = New System.Drawing.Point(29, 25)
        Me.lbl_Info.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_Info.Name = "lbl_Info"
        Me.lbl_Info.Size = New System.Drawing.Size(752, 46)
        Me.lbl_Info.TabIndex = 19
        Me.lbl_Info.Text = "Enter search criteria below. Name searches must includes first name and last name" &
    ". " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Date searches must include Date From and Date To values."
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lbl_CaseType)
        Me.Panel1.Controls.Add(Me.lbl_Info)
        Me.Panel1.Controls.Add(Me.lbl_FirstName)
        Me.Panel1.Controls.Add(Me.comboBox_caseType)
        Me.Panel1.Controls.Add(Me.lbl_MiddleName)
        Me.Panel1.Controls.Add(Me.dateFrom)
        Me.Panel1.Controls.Add(Me.lbl_LastName)
        Me.Panel1.Controls.Add(Me.dateTo)
        Me.Panel1.Controls.Add(Me.lbl_BusinessName)
        Me.Panel1.Controls.Add(Me.txtBox_CitationNo)
        Me.Panel1.Controls.Add(Me.lbl_CaseNo)
        Me.Panel1.Controls.Add(Me.txtBox_CaseNo)
        Me.Panel1.Controls.Add(Me.lbl_CitationNo)
        Me.Panel1.Controls.Add(Me.txtBox_BusinessName)
        Me.Panel1.Controls.Add(Me.lbl_DateFrom)
        Me.Panel1.Controls.Add(Me.txtBox_LastName)
        Me.Panel1.Controls.Add(Me.lbl_DateTo)
        Me.Panel1.Controls.Add(Me.txtBox_MiddleName)
        Me.Panel1.Controls.Add(Me.btn_search)
        Me.Panel1.Controls.Add(Me.txtBox_FirstName)
        Me.Panel1.Location = New System.Drawing.Point(12, 12)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(886, 782)
        Me.Panel1.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Black
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Location = New System.Drawing.Point(-3, 109)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(889, 2)
        Me.Label1.TabIndex = 20
        '
        'Searching
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(916, 809)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Searching"
        Me.Text = "Search Criteria"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lbl_CaseType As Label
    Friend WithEvents lbl_FirstName As Label
    Friend WithEvents lbl_MiddleName As Label
    Friend WithEvents lbl_LastName As Label
    Friend WithEvents lbl_BusinessName As Label
    Friend WithEvents lbl_CaseNo As Label
    Friend WithEvents lbl_CitationNo As Label
    Friend WithEvents lbl_DateFrom As Label
    Friend WithEvents lbl_DateTo As Label
    Friend WithEvents btn_search As Button
    Friend WithEvents txtBox_FirstName As TextBox
    Friend WithEvents txtBox_MiddleName As TextBox
    Friend WithEvents txtBox_LastName As TextBox
    Friend WithEvents txtBox_BusinessName As TextBox
    Friend WithEvents txtBox_CaseNo As TextBox
    Friend WithEvents txtBox_CitationNo As TextBox
    Friend WithEvents dateTo As DateTimePicker
    Friend WithEvents dateFrom As DateTimePicker
    Friend WithEvents comboBox_caseType As ComboBox
    Friend WithEvents lbl_Info As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
End Class
