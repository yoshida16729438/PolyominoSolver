Option Compare Binary
Option Explicit On
Option Strict On

''' <summary>
''' 【ピースの作成・確認】
''' </summary>
Public Class frmPieceMenu

    ''' <summary>
    ''' 【ピースリスト】
    ''' </summary>
    Private ReadOnly m_lstPiece As List(Of cls2DIntArray)

    ''' <summary>
    ''' 【New】
    ''' </summary>
    Public Sub New(lstPiece As List(Of cls2DIntArray))

        ' この呼び出しはデザイナーで必要です。
        Me.InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        Me.m_lstPiece = lstPiece
    End Sub

    ''' <summary>
    ''' 【ピースを作成】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnAddPiece_Click(sender As Object, e As EventArgs) Handles btnAddPiece.Click

        Dim insPieceEditor As New frmPieceEditor(uclCheckBox.Enum_Mode.ピース設定, Me.m_lstPiece)
        If insPieceEditor.ShowDialog() = DialogResult.Abort Then
            Me.DialogResult = DialogResult.Abort
            Me.Close()
        End If
        insPieceEditor.Dispose()
    End Sub

    ''' <summary>
    ''' 【ピースを確認】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnViewPiece_Click(sender As Object, e As EventArgs) Handles btnViewPiece.Click

        Dim insPieceEditor As New frmPieceEditor(uclCheckBox.Enum_Mode.ピース表示, Me.m_lstPiece)
        If insPieceEditor.ShowDialog() = DialogResult.Abort Then
            Me.DialogResult = DialogResult.Abort
            Me.Close()
        End If
        insPieceEditor.Dispose()
    End Sub

    ''' <summary>
    ''' 【閉じる】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' 【Activated】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_frmPieceMenu_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated

        Me.btnViewPiece.Enabled = (Me.m_lstPiece.Count > 0)

    End Sub

    ''' <summary>
    ''' 【Enter】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnXXX_Enter(sender As Object, e As EventArgs) Handles btnAddPiece.Enter, btnViewPiece.Enter, btnClose.Enter
        DirectCast(sender, Button).ForeColor = Color.MediumBlue
    End Sub

    ''' <summary>
    ''' 【Leave】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnXXX_Leave(sender As Object, e As EventArgs) Handles btnAddPiece.Leave, btnViewPiece.Leave, btnClose.Leave
        DirectCast(sender, Button).ForeColor = SystemColors.ControlText
    End Sub

End Class