<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPieceMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPieceMenu))
        Me.btnAddPiece = New System.Windows.Forms.Button()
        Me.btnViewPiece = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnAddPiece
        '
        Me.btnAddPiece.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAddPiece.Location = New System.Drawing.Point(50, 50)
        Me.btnAddPiece.Name = "btnAddPiece"
        Me.btnAddPiece.Size = New System.Drawing.Size(120, 50)
        Me.btnAddPiece.TabIndex = 0
        Me.btnAddPiece.Text = "ピースを作成"
        Me.btnAddPiece.UseVisualStyleBackColor = True
        '
        'btnViewPiece
        '
        Me.btnViewPiece.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnViewPiece.Location = New System.Drawing.Point(220, 50)
        Me.btnViewPiece.Name = "btnViewPiece"
        Me.btnViewPiece.Size = New System.Drawing.Size(120, 50)
        Me.btnViewPiece.TabIndex = 2
        Me.btnViewPiece.Text = "ピースを確認"
        Me.btnViewPiece.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnClose.Location = New System.Drawing.Point(390, 50)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(120, 50)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "閉じる"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmPieceMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(564, 152)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnViewPiece)
        Me.Controls.Add(Me.btnAddPiece)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPieceMenu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "ピースの追加・削除"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnAddPiece As Button
    Friend WithEvents btnViewPiece As Button
    Friend WithEvents btnClose As Button
End Class
