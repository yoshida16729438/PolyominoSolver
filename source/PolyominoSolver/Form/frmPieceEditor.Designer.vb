<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPieceEditor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPieceEditor))
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.lblAllPieceNum = New System.Windows.Forms.Label()
        Me.lblHowToUse = New System.Windows.Forms.Label()
        Me.uclChk = New PolyominoSolver.uclCheckBox()
        Me.nudPieceIndex = New System.Windows.Forms.NumericUpDown()
        CType(Me.nudPieceIndex, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(159, 210)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 36)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnOK.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnOK.Location = New System.Drawing.Point(53, 210)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(100, 36)
        Me.btnOK.TabIndex = 7
        Me.btnOK.Text = "追加"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'lblAllPieceNum
        '
        Me.lblAllPieceNum.AutoSize = True
        Me.lblAllPieceNum.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblAllPieceNum.Location = New System.Drawing.Point(146, 21)
        Me.lblAllPieceNum.Name = "lblAllPieceNum"
        Me.lblAllPieceNum.Size = New System.Drawing.Size(49, 19)
        Me.lblAllPieceNum.TabIndex = 13
        Me.lblAllPieceNum.Text = "/100"
        '
        'lblHowToUse
        '
        Me.lblHowToUse.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblHowToUse.AutoSize = True
        Me.lblHowToUse.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblHowToUse.Location = New System.Drawing.Point(68, 9)
        Me.lblHowToUse.Name = "lblHowToUse"
        Me.lblHowToUse.Size = New System.Drawing.Size(164, 38)
        Me.lblHowToUse.TabIndex = 14
        Me.lblHowToUse.Text = "ピースの形状を" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "黒に設定してください"
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
        Me.uclChk.TabIndex = 5
        '
        'nudPieceIndex
        '
        Me.nudPieceIndex.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.nudPieceIndex.Location = New System.Drawing.Point(70, 19)
        Me.nudPieceIndex.Name = "nudPieceIndex"
        Me.nudPieceIndex.Size = New System.Drawing.Size(76, 26)
        Me.nudPieceIndex.TabIndex = 1
        Me.nudPieceIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmPieceEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(309, 262)
        Me.Controls.Add(Me.lblHowToUse)
        Me.Controls.Add(Me.nudPieceIndex)
        Me.Controls.Add(Me.lblAllPieceNum)
        Me.Controls.Add(Me.uclChk)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPieceEditor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "ピースの設定"
        CType(Me.nudPieceIndex, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents lblAllPieceNum As Label
    Friend WithEvents lblHowToUse As Label
    Friend WithEvents uclChk As uclCheckBox
    Friend WithEvents nudPieceIndex As NumericUpDown
End Class
