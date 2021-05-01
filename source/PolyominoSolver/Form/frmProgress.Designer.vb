<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProgress
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProgress))
        Me.prbProgress = New System.Windows.Forms.ProgressBar()
        Me.bgwSolve = New System.ComponentModel.BackgroundWorker()
        Me.lblState = New System.Windows.Forms.Label()
        Me.ttpShinchoku = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.tmrPastTime = New System.Windows.Forms.Timer(Me.components)
        Me.lblPastTime = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'prbProgress
        '
        Me.prbProgress.Location = New System.Drawing.Point(42, 83)
        Me.prbProgress.Name = "prbProgress"
        Me.prbProgress.Size = New System.Drawing.Size(400, 50)
        Me.prbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.prbProgress.TabIndex = 0
        '
        'bgwSolve
        '
        Me.bgwSolve.WorkerReportsProgress = True
        Me.bgwSolve.WorkerSupportsCancellation = True
        '
        'lblState
        '
        Me.lblState.AutoSize = True
        Me.lblState.Font = New System.Drawing.Font("MS UI Gothic", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblState.Location = New System.Drawing.Point(200, 20)
        Me.lblState.Name = "lblState"
        Me.lblState.Size = New System.Drawing.Size(82, 24)
        Me.lblState.TabIndex = 1
        Me.lblState.Text = "探索中"
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(192, 148)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 40)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "中止"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'tmrPastTime
        '
        Me.tmrPastTime.Interval = 1000
        '
        'lblPastTime
        '
        Me.lblPastTime.AutoSize = True
        Me.lblPastTime.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblPastTime.Location = New System.Drawing.Point(174, 54)
        Me.lblPastTime.Name = "lblPastTime"
        Me.lblPastTime.Size = New System.Drawing.Size(137, 15)
        Me.lblPastTime.TabIndex = 3
        Me.lblPastTime.Text = "経過時間：10分10秒"
        '
        'frmProgress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 204)
        Me.Controls.Add(Me.lblPastTime)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblState)
        Me.Controls.Add(Me.prbProgress)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmProgress"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "探索中"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents prbProgress As ProgressBar
    Friend WithEvents bgwSolve As System.ComponentModel.BackgroundWorker
    Friend WithEvents lblState As Label
    Friend WithEvents ttpShinchoku As ToolTip
    Friend WithEvents btnCancel As Button
    Friend WithEvents tmrPastTime As Timer
    Friend WithEvents lblPastTime As Label
End Class
