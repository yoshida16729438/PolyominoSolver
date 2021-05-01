Option Compare Binary
Option Explicit On
Option Strict On

''' <summary>
''' 【盤面設定・結果表示】
''' </summary>
Public Class frmBoard

#Region "●定数"

    ''' <summary>
    ''' 最左上のチェックボックスのx座標
    ''' </summary>
    Private Const m_intCheckBoxStartLocation_x As Integer = 40

    ''' <summary>
    ''' 最左上のチェックボックスのy座標
    ''' </summary>
    Private Const m_intCheckBoxStartLocation_y As Integer = 40

#End Region

#Region "●変数等"

    ''' <summary>
    ''' 【盤面】
    ''' </summary>
    Private m_insBoard As cls2DIntArray

    ''' <summary>
    ''' 【画面を開くときのモード】
    ''' </summary>
    Private ReadOnly m_enmMode As uclCheckBox.Enum_Mode

#End Region

#Region "●プロパティ"

    ''' <summary>
    ''' 【盤面】
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property insBoard As cls2DIntArray
        Get
            Return Me.m_insBoard
        End Get
    End Property

    ''' <summary>
    ''' 【結果リスト】
    ''' </summary>
    ''' <returns></returns>
    Public Property lstResult As List(Of clsBoardData)

#End Region

#Region "●初期化"

    ''' <summary>
    ''' 【New】
    ''' </summary>
    ''' <param name="insBoard">盤面</param>
    ''' <param name="enmMode">モード</param>
    Public Sub New(insBoard As cls2DIntArray, enmMode As uclCheckBox.Enum_Mode)

        ' この呼び出しはデザイナーで必要です。
        Me.InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

        Me.m_insBoard = insBoard

        'モード指定
        Me.m_enmMode = enmMode

        'UCL初期化
        Me.uclChk.subInit(insBoard.intWidth, insBoard.intHeight, enmMode)

        'フォームサイズ指定　ここでやらないとCenterParentが働かない
        Dim intFormWidth = Me.uclChk.Width + m_intCheckBoxStartLocation_x * 2 + SystemInformation.FrameBorderSize.Width * 7
        Dim intFormHeight = Me.uclChk.Height + m_intCheckBoxStartLocation_y + 100 + SystemInformation.FrameBorderSize.Height * 2

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
    Private Sub m_frmBoard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '表示をいったん停止
        Me.SuspendLayout()

        If Me.m_enmMode = uclCheckBox.Enum_Mode.盤面設定 Then
            Me.m_subSetVisual_ForEditMode()
        Else
            Me.m_subSetVisual_ForResultMode()
        End If

        'ボタン移動
        Me.m_subMoveButton()

        '画面反映
        Me.ResumeLayout()

        'モードに応じて初期フォーカスまたは無効化
        If Me.m_enmMode = uclCheckBox.Enum_Mode.盤面設定 Then

            Me.uclChk.Focus()
        Else
            Me.uclChk.Enabled = False
        End If

    End Sub

#Region "●編集モード用"

    ''' <summary>
    ''' 【編集モード用初期化】
    ''' </summary>
    Private Sub m_subSetVisual_ForEditMode()

        Me.Text = "盤面の設定"
        Me.lblAllResultNum.Visible = False
        Me.nudPieceIndex.Visible = False

        '盤面表示
        Me.uclChk.subShowData(Me.m_insBoard)

        Me.m_subMoveHowToUseLabel()
    End Sub

    ''' <summary>
    ''' 【説明ラベル位置調整】
    ''' </summary>
    Private Sub m_subMoveHowToUseLabel()

        Dim insFont As New Font("MS UI Gothic", 14.25)

        Dim strText As String = "設置不可部分を{0}黒に設定してください"

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

#Region "●結果モード用"

    ''' <summary>
    ''' 【結果モード用表示】
    ''' </summary>
    Private Sub m_subSetVisual_ForResultMode()

        Me.Text = "結果の表示"
        Me.btnCancel.Text = "閉じる"
        Me.lblHowToUse.Visible = False
        Me.btnOK.Visible = False
        Me.nudPieceIndex.Value = 1
        Me.nudPieceIndex.Minimum = 1
        Me.nudPieceIndex.Maximum = Me.lstResult.Count
        Me.lblAllResultNum.Text = "/" & Me.lstResult.Count.ToString
        Me.lblAllResultNum.Location = New Point(Me.Width \ 2 - 10, Me.lblAllResultNum.Location.Y)

        '結果リスト先頭表示
        Me.m_subShowResult(0)
        Me.nudPieceIndex.Location = New Point(Me.lblAllResultNum.Location.X - Me.nudPieceIndex.Width, Me.nudPieceIndex.Location.Y)

        '閉じるボタンを中央へ移動
        Me.m_subMoveCancelButton()

    End Sub

    ''' <summary>
    ''' 【キャンセルボタン位置調整】
    ''' </summary>
    Private Sub m_subMoveCancelButton()

        '上下方向位置は画面サイズに合わせて自動調整される
        '左右位置のみ変更
        Dim intX As Integer = (Me.Size.Width - Me.btnCancel.Width) \ 2
        Me.btnCancel.Location = New Point(intX, Me.btnCancel.Location.Y)

    End Sub

    ''' <summary>
    ''' 結果表示モードの場合の画面表示機能
    ''' </summary>
    ''' <param name="intIndex">結果インデックス</param>
    Private Sub m_subShowResult(intIndex As Integer)

        '表示する結果を変更
        Me.uclChk.subShowData(Me.lstResult(intIndex))

    End Sub

#End Region

    ''' <summary>
    ''' 【ボタン位置調整】
    ''' </summary>
    Private Sub m_subMoveButton()

        Dim intHalf As Integer = Me.Width \ 2

        Me.btnOK.Location = New Point(intHalf - 3 - Me.btnOK.Width, Me.Height - 87)
        Me.btnCancel.Location = New Point(intHalf + 3, Me.btnOK.Location.Y)

    End Sub

#End Region

#Region "●ボタン関連"

    ''' <summary>
    ''' 【形状確定→frmStartに形状を渡す】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click

        If MsgBox("盤面の形状を確定しますか？", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then

            Me.m_insBoard = Me.uclChk.insGetData

            '決定でのクローズなのでOKを設定
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If

    End Sub

    ''' <summary>
    ''' 【閉じる】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

#End Region

#Region "●NumericUpDown"

    ''' <summary>
    ''' 【結果インデックス変更】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_nudResultIndex_ValueChanged(sender As Object, e As EventArgs) Handles nudPieceIndex.ValueChanged

        Me.m_subShowResult(CInt(Me.nudPieceIndex.Value) - 1)

    End Sub

#End Region

#Region "●Form"

    ''' <summary>
    ''' 【FormClosing】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_frmBoard_Closing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        '決定ボタンによるクローズならスルーしてよい
        If Me.DialogResult <> DialogResult.OK Then

            Dim strMessage As String

            If Me.m_enmMode = uclCheckBox.Enum_Mode.盤面設定 Then

                strMessage = "変更を破棄してよろしいですか？"
            Else
                strMessage = "結果の確認を終了しますか？"
            End If

            If MsgBox(strMessage, vbYesNo Or vbDefaultButton2 Or vbQuestion) <> MsgBoxResult.Yes Then

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