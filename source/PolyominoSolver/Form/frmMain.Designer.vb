<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.btnSetBoard = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.cmbWidth = New System.Windows.Forms.ComboBox()
        Me.cmbHeight = New System.Windows.Forms.ComboBox()
        Me.lblWidth = New System.Windows.Forms.Label()
        Me.lblHeight = New System.Windows.Forms.Label()
        Me.btnSetPiece = New System.Windows.Forms.Button()
        Me.btnStartSolve = New System.Windows.Forms.Button()
        Me.radAllFree = New System.Windows.Forms.RadioButton()
        Me.radRotationFree = New System.Windows.Forms.RadioButton()
        Me.radOnlyMove = New System.Windows.Forms.RadioButton()
        Me.pnlRotation = New System.Windows.Forms.Panel()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuConfig = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMultiThread = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblCurrentBoard = New System.Windows.Forms.Label()
        Me.pnlRotation.SuspendLayout()
        Me.MenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSetBoard
        '
        Me.btnSetBoard.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSetBoard.Location = New System.Drawing.Point(220, 132)
        Me.btnSetBoard.Name = "btnSetBoard"
        Me.btnSetBoard.Size = New System.Drawing.Size(140, 50)
        Me.btnSetBoard.TabIndex = 3
        Me.btnSetBoard.Text = "盤面の設定へ"
        Me.btnSetBoard.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnClose.Location = New System.Drawing.Point(220, 402)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(140, 50)
        Me.btnClose.TabIndex = 9
        Me.btnClose.Text = "終了"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'cmbWidth
        '
        Me.cmbWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbWidth.Font = New System.Drawing.Font("MS UI Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbWidth.FormattingEnabled = True
        Me.cmbWidth.Location = New System.Drawing.Point(120, 70)
        Me.cmbWidth.Name = "cmbWidth"
        Me.cmbWidth.Size = New System.Drawing.Size(125, 29)
        Me.cmbWidth.TabIndex = 1
        '
        'cmbHeight
        '
        Me.cmbHeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbHeight.Font = New System.Drawing.Font("MS UI Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbHeight.FormattingEnabled = True
        Me.cmbHeight.Location = New System.Drawing.Point(340, 70)
        Me.cmbHeight.Name = "cmbHeight"
        Me.cmbHeight.Size = New System.Drawing.Size(125, 29)
        Me.cmbHeight.TabIndex = 2
        '
        'lblWidth
        '
        Me.lblWidth.AutoSize = True
        Me.lblWidth.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblWidth.Location = New System.Drawing.Point(123, 40)
        Me.lblWidth.Name = "lblWidth"
        Me.lblWidth.Size = New System.Drawing.Size(120, 19)
        Me.lblWidth.TabIndex = 5
        Me.lblWidth.Text = "盤面の最大幅"
        '
        'lblHeight
        '
        Me.lblHeight.AutoSize = True
        Me.lblHeight.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblHeight.Location = New System.Drawing.Point(338, 40)
        Me.lblHeight.Name = "lblHeight"
        Me.lblHeight.Size = New System.Drawing.Size(133, 19)
        Me.lblHeight.TabIndex = 6
        Me.lblHeight.Text = "盤面の最大高さ"
        '
        'btnSetPiece
        '
        Me.btnSetPiece.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSetPiece.Location = New System.Drawing.Point(220, 200)
        Me.btnSetPiece.Name = "btnSetPiece"
        Me.btnSetPiece.Size = New System.Drawing.Size(140, 50)
        Me.btnSetPiece.TabIndex = 4
        Me.btnSetPiece.Text = "ピースの設定へ"
        Me.btnSetPiece.UseVisualStyleBackColor = True
        '
        'btnStartSolve
        '
        Me.btnStartSolve.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnStartSolve.Location = New System.Drawing.Point(220, 330)
        Me.btnStartSolve.Name = "btnStartSolve"
        Me.btnStartSolve.Size = New System.Drawing.Size(140, 50)
        Me.btnStartSolve.TabIndex = 8
        Me.btnStartSolve.Text = "解く"
        Me.btnStartSolve.UseVisualStyleBackColor = True
        '
        'radAllFree
        '
        Me.radAllFree.AutoSize = True
        Me.radAllFree.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.radAllFree.Location = New System.Drawing.Point(17, 16)
        Me.radAllFree.Name = "radAllFree"
        Me.radAllFree.Size = New System.Drawing.Size(132, 23)
        Me.radAllFree.TabIndex = 5
        Me.radAllFree.Text = "回転・反転可"
        Me.radAllFree.UseVisualStyleBackColor = True
        '
        'radRotationFree
        '
        Me.radRotationFree.AutoSize = True
        Me.radRotationFree.Checked = True
        Me.radRotationFree.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.radRotationFree.Location = New System.Drawing.Point(175, 16)
        Me.radRotationFree.Name = "radRotationFree"
        Me.radRotationFree.Size = New System.Drawing.Size(170, 23)
        Me.radRotationFree.TabIndex = 6
        Me.radRotationFree.TabStop = True
        Me.radRotationFree.Text = "回転可・反転不可"
        Me.radRotationFree.UseVisualStyleBackColor = True
        '
        'radOnlyMove
        '
        Me.radOnlyMove.AutoSize = True
        Me.radOnlyMove.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.radOnlyMove.Location = New System.Drawing.Point(371, 16)
        Me.radOnlyMove.Name = "radOnlyMove"
        Me.radOnlyMove.Size = New System.Drawing.Size(151, 23)
        Me.radOnlyMove.TabIndex = 7
        Me.radOnlyMove.Text = "回転・反転不可"
        Me.radOnlyMove.UseVisualStyleBackColor = True
        '
        'pnlRotation
        '
        Me.pnlRotation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlRotation.Controls.Add(Me.radOnlyMove)
        Me.pnlRotation.Controls.Add(Me.radRotationFree)
        Me.pnlRotation.Controls.Add(Me.radAllFree)
        Me.pnlRotation.Location = New System.Drawing.Point(23, 264)
        Me.pnlRotation.Name = "pnlRotation"
        Me.pnlRotation.Size = New System.Drawing.Size(536, 55)
        Me.pnlRotation.TabIndex = 5
        '
        'MenuStrip
        '
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuConfig})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(584, 24)
        Me.MenuStrip.TabIndex = 10
        Me.MenuStrip.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuLoad, Me.mnuSave})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(67, 20)
        Me.mnuFile.Text = "ファイル(&F)"
        '
        'mnuLoad
        '
        Me.mnuLoad.Name = "mnuLoad"
        Me.mnuLoad.Size = New System.Drawing.Size(113, 22)
        Me.mnuLoad.Text = "読込(&R)"
        '
        'mnuSave
        '
        Me.mnuSave.Name = "mnuSave"
        Me.mnuSave.Size = New System.Drawing.Size(113, 22)
        Me.mnuSave.Text = "保存(&S)"
        '
        'mnuConfig
        '
        Me.mnuConfig.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuMultiThread})
        Me.mnuConfig.Name = "mnuConfig"
        Me.mnuConfig.Size = New System.Drawing.Size(82, 20)
        Me.mnuConfig.Text = "探索設定(&C)"
        '
        'mnuMultiThread
        '
        Me.mnuMultiThread.CheckOnClick = True
        Me.mnuMultiThread.Name = "mnuMultiThread"
        Me.mnuMultiThread.Size = New System.Drawing.Size(166, 22)
        Me.mnuMultiThread.Text = "高速モードで解く(&S)"
        '
        'lblCurrentBoard
        '
        Me.lblCurrentBoard.AutoSize = True
        Me.lblCurrentBoard.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblCurrentBoard.Location = New System.Drawing.Point(199, 108)
        Me.lblCurrentBoard.Name = "lblCurrentBoard"
        Me.lblCurrentBoard.Size = New System.Drawing.Size(195, 16)
        Me.lblCurrentBoard.TabIndex = 11
        Me.lblCurrentBoard.Text = "※現在の設定：幅10　高さ10"
        '
        'frmMain
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(584, 464)
        Me.Controls.Add(Me.lblCurrentBoard)
        Me.Controls.Add(Me.pnlRotation)
        Me.Controls.Add(Me.btnStartSolve)
        Me.Controls.Add(Me.btnSetPiece)
        Me.Controls.Add(Me.lblHeight)
        Me.Controls.Add(Me.lblWidth)
        Me.Controls.Add(Me.cmbHeight)
        Me.Controls.Add(Me.cmbWidth)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSetBoard)
        Me.Controls.Add(Me.MenuStrip)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Polyomino Solver"
        Me.pnlRotation.ResumeLayout(False)
        Me.pnlRotation.PerformLayout()
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnSetBoard As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents cmbWidth As ComboBox
    Friend WithEvents cmbHeight As ComboBox
    Friend WithEvents lblWidth As Label
    Friend WithEvents lblHeight As Label
    Friend WithEvents btnSetPiece As Button
    Friend WithEvents btnStartSolve As Button
    Friend WithEvents radAllFree As RadioButton
    Friend WithEvents radRotationFree As RadioButton
    Friend WithEvents radOnlyMove As RadioButton
    Friend WithEvents pnlRotation As Panel
    Friend WithEvents MenuStrip As MenuStrip
    Friend WithEvents mnuFile As ToolStripMenuItem
    Friend WithEvents mnuLoad As ToolStripMenuItem
    Friend WithEvents mnuSave As ToolStripMenuItem
    Friend WithEvents mnuConfig As ToolStripMenuItem
    Friend WithEvents mnuMultiThread As ToolStripMenuItem
    Friend WithEvents lblCurrentBoard As Label
End Class
