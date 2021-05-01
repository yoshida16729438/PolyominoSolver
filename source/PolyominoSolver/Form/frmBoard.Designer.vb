<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmBoard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBoard))
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblAllResultNum = New System.Windows.Forms.Label()
        Me.lblHowToUse = New System.Windows.Forms.Label()
        Me.uclChk = New PolyominoSolver.uclCheckBox()
        Me.nudPieceIndex = New System.Windows.Forms.NumericUpDown()
        CType(Me.nudPieceIndex, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnOK.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnOK.Location = New System.Drawing.Point(47, 213)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(100, 36)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "決定"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(153, 213)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 36)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblAllResultNum
        '
        Me.lblAllResultNum.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblAllResultNum.AutoSize = True
        Me.lblAllResultNum.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblAllResultNum.Location = New System.Drawing.Point(139, 19)
        Me.lblAllResultNum.Name = "lblAllResultNum"
        Me.lblAllResultNum.Size = New System.Drawing.Size(49, 19)
        Me.lblAllResultNum.TabIndex = 4
        Me.lblAllResultNum.Text = "/100"
        '
        'lblHowToUse
        '
        Me.lblHowToUse.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblHowToUse.AutoSize = True
        Me.lblHowToUse.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblHowToUse.Location = New System.Drawing.Point(65, 7)
        Me.lblHowToUse.Name = "lblHowToUse"
        Me.lblHowToUse.Size = New System.Drawing.Size(164, 38)
        Me.lblHowToUse.TabIndex = 5
        Me.lblHowToUse.Text = "設置不可部分を" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "黒に設定してください"
        Me.lblHowToUse.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'uclChk
        '
        Me.uclChk.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uclChk.Location = New System.Drawing.Point(50, 50)
        Me.uclChk.Name = "uclChk"
        Me.uclChk.Size = New System.Drawing.Size(14, 14)
        Me.uclChk.TabIndex = 3
        '
        'nudPieceIndex
        '
        Me.nudPieceIndex.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.nudPieceIndex.Location = New System.Drawing.Point(67, 17)
        Me.nudPieceIndex.Name = "nudPieceIndex"
        Me.nudPieceIndex.Size = New System.Drawing.Size(76, 26)
        Me.nudPieceIndex.TabIndex = 1
        Me.nudPieceIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmBoard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(300, 261)
        Me.Controls.Add(Me.uclChk)
        Me.Controls.Add(Me.lblHowToUse)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.nudPieceIndex)
        Me.Controls.Add(Me.lblAllResultNum)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBoard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmBoard"
        CType(Me.nudPieceIndex, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblAllResultNum As Label
    Friend WithEvents lblHowToUse As Label
    Friend WithEvents uclChk As uclCheckBox
    Friend WithEvents nudPieceIndex As NumericUpDown
End Class
