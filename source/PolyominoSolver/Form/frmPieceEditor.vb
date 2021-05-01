Option Compare Binary
Option Explicit On
Option Strict On

''' <summary>
''' 【ピースの作成・表示画面】
''' </summary>
Public Class frmPieceEditor

#Region "●定数"

    ''' <summary>
    ''' 【チェックボックス表示開始位置（X方向）】
    ''' </summary>
    Private Const m_intCheckBoxStartLocation_X As Integer = 50

    ''' <summary>
    ''' 【チェックボックス表示開始位置（Y方向）】
    ''' </summary>
    Private Const m_intCheckBoxStartLocation_Y As Integer = 50

#End Region

#Region "●変数"

    ''' <summary>
    ''' 【モード】
    ''' </summary>
    Private ReadOnly m_enmMode As uclCheckBox.Enum_Mode

    ''' <summary>
    ''' 【表示中のピースの番号】
    ''' </summary>
    Private m_intPieceIndex As Integer

    ''' <summary>
    ''' 【ピースリスト】
    ''' </summary>
    Private ReadOnly m_lstPiece As List(Of cls2DIntArray)

#End Region

#Region "●初期化"

    ''' <summary>
    ''' 【New】
    ''' </summary>
    ''' <param name="enmMode"></param>
    ''' <param name="lstPiece"></param>
    Public Sub New(enmMode As uclCheckBox.Enum_Mode, lstPiece As List(Of cls2DIntArray))

        ' この呼び出しはデザイナーで必要です。
        Me.InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

        Me.m_enmMode = enmMode
        Me.m_lstPiece = lstPiece

        'UCL初期化
        Me.uclChk.subInit(g_insConfig.intPieceSize, g_insConfig.intPieceSize, enmMode)

        'フォームサイズ指定　ここでやらないとCenterParentが働かない
        Dim intFormWidth = Me.uclChk.Width + m_intCheckBoxStartLocation_X * 2 + SystemInformation.FrameBorderSize.Width * 2
        Dim intFormHeight = Me.uclChk.Height + m_intCheckBoxStartLocation_Y + 100 + SystemInformation.FrameBorderSize.Height * 2

        Me.Size = New Size(intFormWidth, intFormHeight)
        Me.MinimumSize = Me.Size

    End Sub

#End Region

#Region "●画面表示関連"

    ''' <summary>
    ''' 【Load】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_frmPieceEditorOrViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.SuspendLayout()

        If Me.m_enmMode = uclCheckBox.Enum_Mode.ピース設定 Then
            Me.m_subSetVisual_EditMode()
        Else
            Me.m_subSetVisual_ViewMode()
        End If

        '画面反映
        Me.ResumeLayout()

        'モードに応じてフォーカスまたは無効化
        If Me.m_enmMode = uclCheckBox.Enum_Mode.ピース設定 Then
            Me.uclChk.Focus()
        Else
            Me.uclChk.Enabled = False
        End If

    End Sub

#Region "●編集モード用表示"

    ''' <summary>
    ''' 【編集モード用初期化】
    ''' </summary>
    Private Sub m_subSetVisual_EditMode()

        Me.Text = "ピースの設定"
        Me.lblAllPieceNum.Visible = False
        Me.nudPieceIndex.Visible = False
        Me.btnOK.Text = "追加"

        '全てChecked=False
        Me.uclChk.subChangeCheckedState(False)

        '使い方ラベル表示位置修正
        Me.m_subMoveHowToUse()

    End Sub

    ''' <summary>
    ''' 【使い方ラベル表示位置変更】
    ''' </summary>
    Private Sub m_subMoveHowToUse()

        Dim insFont As New Font("MS UI Gothic", 14.25)

        Dim strText As String = "ピースの形状を{0}黒に設定してください"

        Dim intLabelWidth As Integer = TextRenderer.MeasureText(String.Format(strText, String.Empty), insFont).Width

        If Me.Width > intLabelWidth Then
            Me.lblHowToUse.Text = String.Format(strText, String.Empty)
            Me.lblHowToUse.Location = New Point((Me.Width - Me.lblHowToUse.Width) \ 2, 15)
        Else
            Me.lblHowToUse.Text = String.Format(strText, vbCrLf)
            Me.lblHowToUse.Location = New Point((Me.Width - Me.lblHowToUse.Width) \ 2, 7)
        End If

    End Sub

#End Region

#Region "●確認モード用表示"

    ''' <summary>
    ''' 【確認モード用初期化】
    ''' </summary>
    Private Sub m_subSetVisual_ViewMode()

        Me.Text = "ピースの表示・削除"
        Me.lblHowToUse.Visible = False
        Me.nudPieceIndex.Minimum = 1
        Me.nudPieceIndex.Maximum = Me.m_lstPiece.Count
        Me.nudPieceIndex.Value = 1
        Me.btnOK.Text = "削除"
        Me.lblAllPieceNum.Text = "/" & Me.m_lstPiece.Count.ToString
        Me.lblAllPieceNum.Location = New Point(Me.Width \ 2 - 10, Me.lblAllPieceNum.Location.Y)

        Me.m_subShowPiece(0)
        Me.nudPieceIndex.Location = New Point(Me.lblAllPieceNum.Location.X - Me.nudPieceIndex.Width, Me.nudPieceIndex.Location.Y)

    End Sub

    ''' <summary>
    ''' 【ピースを表示】
    ''' </summary>
    ''' <param name="intIndex">ピースインデックス</param>
    Private Sub m_subShowPiece(intIndex As Integer)

        '表示中インデックス保存
        Me.m_intPieceIndex = intIndex

        'ピース表示
        Me.uclChk.subShowData(Me.m_lstPiece(intIndex))

    End Sub

#End Region

#End Region

#Region "●ボタン関連"

    ''' <summary>
    ''' 【キャンセル】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' 【追加/削除】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        If Me.m_enmMode = uclCheckBox.Enum_Mode.ピース設定 Then

            '追加ボタン動作
            Dim insPiece As cls2DIntArray = Me.uclChk.insGetData

            '0マス設定は不可
            If Not Me.m_blnChecked_AtLeast1(insPiece) Then
                MsgBox("ピースの形状を設定してください", MsgBoxStyle.Information)
                Exit Sub
            End If

            If MsgBox("この形状でピースを追加しますか？", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then

                'ピース追加
                Me.m_lstPiece.Add(insPiece)

                If MsgBox("ピースを追加しました。" & vbCrLf & "続けて作成しますか？", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton1) = MsgBoxResult.Yes Then

                    'リセット
                    Me.uclChk.subChangeCheckedState(False)
                    Me.uclChk.Focus()
                Else
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                End If

            End If

        Else
            '削除ボタン動作
            If MsgBox("表示中のピースを削除しますか？", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then
                Me.m_lstPiece.RemoveAt(Me.m_intPieceIndex)

                If Not Me.m_blnIsPieceExist Then
                    Me.Close()
                Else
                    Me.m_subShowPieceAfterDelete()

                    Me.lblAllPieceNum.Text = "/" & Me.m_lstPiece.Count

                End If

            End If
        End If

    End Sub

    ''' <summary>
    ''' 【1マス以上チェックされているか】
    ''' </summary>
    ''' <param name="insPiece">ピース</param>
    ''' <returns></returns>
    Private Function m_blnChecked_AtLeast1(insPiece As cls2DIntArray) As Boolean

        For intIndex As Integer = 0 To insPiece.intCount - 1
            If insPiece.intValue(intIndex) <> 0 Then Return True
        Next

        Return False

    End Function

    ''' <summary>
    ''' 【削除後、まだピースがあるか】
    ''' </summary>
    ''' <returns></returns>
    Private Function m_blnIsPieceExist() As Boolean

        If Me.m_lstPiece.Count = 0 Then
            MsgBox("ピースがありません。", MsgBoxStyle.Information)
            Return False
        End If

        Return True

    End Function

    ''' <summary>
    ''' 【削除後、次のピースを表示】
    ''' </summary>
    Private Sub m_subShowPieceAfterDelete()

        '基本的には削除された次のピースを表示する
        '最後尾が削除された場合は、前のピース
        If Me.m_intPieceIndex = Me.m_lstPiece.Count Then
            Me.m_intPieceIndex -= 1
        End If

        'NumericUpDownの最大値と現在値を更新
        Me.nudPieceIndex.Maximum = Me.m_lstPiece.Count
        Me.nudPieceIndex.Value = Me.m_intPieceIndex + 1

        '再表示
        Me.m_subShowPiece(Me.m_intPieceIndex)

    End Sub

#End Region

#Region "●NumericUpDown"

    ''' <summary>
    ''' 【ピース番号選択】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_nudPieceIndex_ValueChanged(sender As Object, e As EventArgs) Handles nudPieceIndex.ValueChanged

        Me.m_subShowPiece(CInt(Me.nudPieceIndex.Value) - 1)

    End Sub

#End Region

#Region "●Form"

    ''' <summary>
    ''' 【FormClosing】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_frmPieceEditor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        '別の場所でクローズ前チェックしてるなら不要
        If Me.m_enmMode = uclCheckBox.Enum_Mode.ピース設定 AndAlso
            Me.DialogResult <> DialogResult.OK Then

            If MsgBox("ピースの作成を終了しますか？", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Yes Then
                e.Cancel = True
            End If

        End If

    End Sub

#End Region

#Region "●Enter・Leave"

    ''' <summary>
    ''' 【Enter】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnXXX_Enter(sender As Object, e As EventArgs) Handles btnOK.Enter, btnCancel.Enter
        DirectCast(sender, Button).ForeColor = Color.MediumBlue
    End Sub

    ''' <summary>
    ''' 【Leave】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnXXX_Leave(sender As Object, e As EventArgs) Handles btnOK.Leave, btnCancel.Leave
        DirectCast(sender, Button).ForeColor = SystemColors.ControlText
    End Sub

#End Region

End Class