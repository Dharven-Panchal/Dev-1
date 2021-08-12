<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.DashboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.toolStripLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.toolStripProgressBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.toolStripLabelCopy = New System.Windows.Forms.ToolStripStatusLabel()
        Me.NewSearchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(800, 450)
        Me.pnlMain.TabIndex = 1
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DashboardToolStripMenuItem, Me.ReportToolStripMenuItem, Me.NewSearchToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(800, 30)
        Me.MenuStrip1.TabIndex = 4
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'DashboardToolStripMenuItem
        '
        Me.DashboardToolStripMenuItem.Name = "DashboardToolStripMenuItem"
        Me.DashboardToolStripMenuItem.Size = New System.Drawing.Size(96, 24)
        Me.DashboardToolStripMenuItem.Text = "Dashboard"
        '
        'ReportToolStripMenuItem
        '
        Me.ReportToolStripMenuItem.Name = "ReportToolStripMenuItem"
        Me.ReportToolStripMenuItem.Size = New System.Drawing.Size(68, 24)
        Me.ReportToolStripMenuItem.Text = "Report"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolStripLabel, Me.toolStripProgressBar, Me.toolStripLabelCopy})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 424)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(800, 26)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'toolStripLabel
        '
        Me.toolStripLabel.AutoSize = False
        Me.toolStripLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.toolStripLabel.ForeColor = System.Drawing.Color.Red
        Me.toolStripLabel.Name = "toolStripLabel"
        Me.toolStripLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.toolStripLabel.Size = New System.Drawing.Size(700, 20)
        Me.toolStripLabel.Text = "Please Click on Dashboard to Start the process"
        Me.toolStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.toolStripLabel.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        '
        'toolStripProgressBar
        '
        Me.toolStripProgressBar.Name = "toolStripProgressBar"
        Me.toolStripProgressBar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.toolStripProgressBar.Size = New System.Drawing.Size(400, 18)
        '
        'toolStripLabelCopy
        '
        Me.toolStripLabelCopy.AutoSize = False
        Me.toolStripLabelCopy.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.toolStripLabelCopy.Name = "toolStripLabelCopy"
        Me.toolStripLabelCopy.Size = New System.Drawing.Size(500, 20)
        Me.toolStripLabelCopy.Text = "Copyright © Data Crawler 2021"
        Me.toolStripLabelCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'NewSearchToolStripMenuItem
        '
        Me.NewSearchToolStripMenuItem.Name = "NewSearchToolStripMenuItem"
        Me.NewSearchToolStripMenuItem.Size = New System.Drawing.Size(101, 24)
        Me.NewSearchToolStripMenuItem.Text = "New Search"
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.pnlMain)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "Main"
        Me.Text = "Data Crawler"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents pnlMain As Panel
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents DashboardToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents toolStripLabel As ToolStripStatusLabel
    Friend WithEvents toolStripProgressBar As ToolStripProgressBar
    Friend WithEvents toolStripLabelCopy As ToolStripStatusLabel
    Friend WithEvents NewSearchToolStripMenuItem As ToolStripMenuItem
End Class
