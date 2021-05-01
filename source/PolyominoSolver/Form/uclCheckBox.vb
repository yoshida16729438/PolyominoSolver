Option Compare Binary
Option Explicit On
Option Strict On

''' <summary>
''' 【チェックボックス表示】
''' </summary>
Public Class uclCheckBox

#Region "●定数"

    ''' <summary>
    ''' 【線パネルの幅】
    ''' </summary>
    Private Const m_int_LineWidth As Integer = 2

#End Region

#Region "●変数"

    ''' <summary>
    ''' 【チェックボックスインスタンス配列】
    ''' </summary>
    Private m_insCheckBoxes As CheckBox()

    ''' <summary>
    ''' 【動作モード】
    ''' </summary>
    Private m_enmMode As Enum_Mode

    ''' <summary>
    ''' 【列数】
    ''' </summary>
    Private m_intColumnNum As Integer

    ''' <summary>
    ''' 【行数】
    ''' </summary>
    Private m_intRowNum As Integer

    ''' <summary>
    ''' 【境界表示用パネルリスト】
    ''' </summary>
    Private m_lstPanel As List(Of Panel)

#End Region

#Region "●Enum"

    ''' <summary>
    ''' 【モード設定】
    ''' </summary>
    Public Enum Enum_Mode

        盤面設定
        解答表示
        ピース設定
        ピース表示

    End Enum

#End Region

#Region "●初期化"

    ''' <summary>
    ''' 【初期化（New時）】
    ''' </summary>
    ''' <param name="enmMode"></param>
    Public Sub subInit(intColumnNum As Integer, intRowNum As Integer, enmMode As Enum_Mode)

        Me.m_intColumnNum = intColumnNum
        Me.m_intRowNum = intRowNum
        Me.m_enmMode = enmMode

        'サイズ調整
        Me.m_subChangeSize()
    End Sub

    ''' <summary>
    ''' 【配列に合わせて自身のサイズを設定】
    ''' </summary>
    Private Sub m_subChangeSize()

        Dim intCheckBoxSize As Integer = Me.m_intCheckBoxSize

        Me.Size = New Size(intCheckBoxSize * Me.m_intColumnNum + m_int_LineWidth * 2 + 10, intCheckBoxSize * Me.m_intRowNum + m_int_LineWidth * 2 + 10)

    End Sub

#End Region

#Region "●画面初期化"

    ''' <summary>
    ''' 【Load】
    ''' </summary>
    Private Sub m_uclCheckBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'なぜか再設定が必要
        Me.m_subChangeSize()

        'チェックボックス表示
        Me.m_subMakeCheckBoxes()

        '初期化
        Me.m_lstPanel = New List(Of Panel)

        'モード別表示
        Select Case Me.m_enmMode

            Case Enum_Mode.盤面設定

                '全てチェック外しておく
                Me.subChangeCheckedState(False)

            Case Enum_Mode.解答表示

                'チェックボックス無効化
                Me.m_subUnableChk()

                '全体を囲う線を表示
                Me.m_subDrawOuterLine()

            Case Enum_Mode.ピース表示

                'チェックボックス無効化
                Me.m_subUnableChk()
        End Select
    End Sub

    ''' <summary>
    ''' 【チェックボックスを作成】
    ''' </summary>
    Private Sub m_subMakeCheckBoxes()

        Dim intCheckBoxSize As Integer = Me.m_intCheckBoxSize

        Dim intLocation_X As Integer
        Dim intLocation_Y As Integer

        Me.m_insCheckBoxes = New CheckBox(Me.m_intColumnNum * Me.m_intRowNum - 1) {}
        For intC As Integer = 0 To Me.m_insCheckBoxes.Count - 1
            Me.m_insCheckBoxes(intC) = New CheckBox
            With Me.m_insCheckBoxes(intC)
                .Anchor = AnchorStyles.Top
                .Appearance = Appearance.Button
                .AutoSize = False
                .BackColor = Color.White
                .Checked = False
                .Text = String.Empty
                .Size = New Size(intCheckBoxSize, intCheckBoxSize)
                .Name = "chkSetting_" & intC.ToString
                .TabIndex = intC
                .TabStop = False

                intLocation_X = (intC Mod Me.m_intColumnNum) * intCheckBoxSize + m_int_LineWidth
                intLocation_Y = (intC \ Me.m_intColumnNum) * intCheckBoxSize + m_int_LineWidth
                .Location = New Point(intLocation_X, intLocation_Y)

                AddHandler .CheckedChanged, AddressOf Me.m_chkXXX_CheckedChanged
                AddHandler .PreviewKeyDown, AddressOf Me.m_chkXXX_PreviewKeyDown
                AddHandler .Enter, AddressOf Me.m_chkXXX_Enter
                AddHandler .Leave, AddressOf Me.m_chkXXX_Leave
            End With
        Next

        '先頭のみTabStop有効化
        'デザイナーが表示できなくなる対策のif文
        If Me.m_insCheckBoxes.Count > 0 Then
            Me.m_insCheckBoxes(0).TabStop = True
        End If

        '画面へ追加
        Me.Controls.AddRange(Me.m_insCheckBoxes)

    End Sub

    ''' <summary>
    ''' 【チェックボックスのサイズ取得】
    ''' </summary>
    ''' <returns></returns>
    Private Function m_intCheckBoxSize() As Integer

        Select Case Me.m_enmMode
            Case Enum_Mode.盤面設定, Enum_Mode.解答表示
                Return g_insConfig.intBoardPixelSize
            Case Else
                Return g_insConfig.intPiecePixelSize
        End Select

    End Function

    ''' <summary>
    ''' 【チェックボックス無効化】
    ''' </summary>
    Private Sub m_subUnableChk()

        For Each insChk In Me.m_insCheckBoxes
            insChk.Enabled = False
        Next

    End Sub

    ''' <summary>
    ''' 【外周の線を引く】
    ''' </summary>
    Private Sub m_subDrawOuterLine()

        Me.m_pnlDrawLine(0, 0, Me.m_intColumnNum, 0)
        Me.m_pnlDrawLine(0, 0, 0, Me.m_intRowNum)
        Me.m_pnlDrawLine(0, Me.m_intRowNum, Me.m_intColumnNum, Me.m_intRowNum)
        Me.m_pnlDrawLine(Me.m_intColumnNum, 0, Me.m_intColumnNum, Me.m_intRowNum)

    End Sub

    ''' <summary>
    ''' 【チェックボックスのCheckedを一括設定】
    ''' </summary>
    ''' <param name="blnChecked"></param>
    Public Sub subChangeCheckedState(blnChecked As Boolean)

        For Each insChk In Me.m_insCheckBoxes
            insChk.Checked = blnChecked
        Next

    End Sub

#End Region

#Region "●データ→画面"

    ''' <summary>
    ''' 【データを画面に反映】
    ''' </summary>
    ''' <param name="insArray">ピースまたは盤面（設定）配列</param>
    Public Sub subShowData(insArray As cls2DIntArray)

        'チェック状態を反映
        Me.m_subReflectCheckedState(insArray)

        'チェックボックスの色を変える
        Me.m_subChangeCheckBoxColor(insArray)

    End Sub

    ''' <summary>
    ''' 【データを画面に反映（探索結果）】
    ''' </summary>
    ''' <param name="insBoardData"></param>
    Public Sub subShowData(insBoardData As clsBoardData)

        'チェック状態を反映
        Me.m_subReflectCheckedState(insBoardData.insBoard)

        'チェックボックスの色を変える
        Me.m_subChangeCheckBoxColor(insBoardData.insBoard)

        '外周以外の線を消す
        Me.m_subDeleteLine()

        '線を引き直す
        Me.m_subDrawHorizontalLine(insBoardData)
        Me.m_subDrawVerticalLine(insBoardData)

    End Sub

    ''' <summary>
    ''' 【チェック状態反映】
    ''' </summary>
    ''' <param name="insArray">データ配列</param>
    Private Sub m_subReflectCheckedState(insArray As cls2DIntArray)

        For intRow As Integer = 0 To Me.m_intRowNum - 1
            For intColumn As Integer = 0 To Me.m_intColumnNum - 1
                Me.m_insCheckBoxes(intRow * Me.m_intColumnNum + intColumn).Checked = (insArray.intValue(intColumn, intRow) <> 0)
            Next
        Next

    End Sub

    ''' <summary>
    ''' 【チェックボックスの色を変更】
    ''' </summary>
    ''' <param name="insArray">表示データ</param>
    Private Sub m_subChangeCheckBoxColor(insArray As cls2DIntArray)

        For intRow As Integer = 0 To Me.m_intRowNum - 1
            For intColumn As Integer = 0 To Me.m_intColumnNum - 1
                Me.m_insCheckBoxes(intRow * Me.m_intColumnNum + intColumn).BackColor = Me.m_insColor(insArray.intValue(intColumn, intRow))
            Next
        Next

    End Sub

    ''' <summary>
    ''' 【番号に応じて色を決定】
    ''' </summary>
    ''' <param name="intPieceNum">配列中の値</param>
    ''' <returns></returns>
    Private Function m_insColor(intPieceNum As Integer) As Color

        Select Case intPieceNum
            Case 0 : Return Color.White
            Case 1 : Return Color.Black
            Case Else : Return g_insConfig.lstPieceColor((intPieceNum - 2) Mod g_insConfig.lstPieceColor.Count)
        End Select

    End Function

    ''' <summary>
    ''' 【表示中の線を消す（外周以外）】
    ''' </summary>
    Private Sub m_subDeleteLine()

        For Each pnlLine In Me.m_lstPanel
            Me.Controls.Remove(pnlLine)
        Next

        'リスト初期化
        Me.m_lstPanel.Clear()

    End Sub

    ''' <summary>
    ''' 【チェックボックス間に境界表示用の線を引く（縦）】
    ''' </summary>
    ''' <param name="insBoardData">データ配列</param>
    Private Sub m_subDrawVerticalLine(insBoardData As clsBoardData)

        '一つ左の列と違う値なら線を引く
        Dim intRow_start As Integer
        For intCol As Integer = 1 To insBoardData.insBoard.intWidth - 1
            For intRow As Integer = 0 To insBoardData.insBoard.intHeight - 1

                If insBoardData.insBoard.intValue(intCol, intRow) <> insBoardData.insBoard.intValue(intCol - 1, intRow) OrElse
                     insBoardData.insPieceInfo.intValue(intCol, intRow) <> insBoardData.insPieceInfo.intValue(intCol - 1, intRow) Then

                    '開始位置を記憶
                    intRow_start = intRow
                    intRow += 1

                    '左右で異なる値が続く限り下へ進む
                    While intRow <= insBoardData.insBoard.intHeight - 1 AndAlso
                            (insBoardData.insBoard.intValue(intCol, intRow) <> insBoardData.insBoard.intValue(intCol - 1, intRow) OrElse
                            insBoardData.insPieceInfo.intValue(intCol, intRow) <> insBoardData.insPieceInfo.intValue(intCol - 1, intRow))
                        intRow += 1
                    End While

                    '同じ値または下限になるところまで線を引く
                    Me.m_lstPanel.Add(Me.m_pnlDrawLine(intCol, intRow_start, intCol, intRow))
                End If
            Next
        Next

    End Sub

    ''' <summary>
    ''' 【チェックボックス間に境界表示用の線を引く（横）】
    ''' </summary>
    ''' <param name="insBoardData">データ配列</param>
    Private Sub m_subDrawHorizontalLine(insBoardData As clsBoardData)

        '一つ上の行と違う値なら線を引く
        Dim intCol_start As Integer
        For intRow As Integer = 1 To insBoardData.insBoard.intHeight - 1
            For intCol As Integer = 0 To insBoardData.insBoard.intWidth - 1

                If insBoardData.insBoard.intValue(intCol, intRow) <> insBoardData.insBoard.intValue(intCol, intRow - 1) OrElse
                    insBoardData.insPieceInfo.intValue(intCol, intRow) <> insBoardData.insPieceInfo.intValue(intCol, intRow - 1) Then

                    '開始位置を記録
                    intCol_start = intCol
                    intCol += 1

                    '同じ値になるまで右へ移動
                    While intCol <= insBoardData.insBoard.intWidth - 1 AndAlso
                            (insBoardData.insBoard.intValue(intCol, intRow) <> insBoardData.insBoard.intValue(intCol, intRow - 1) OrElse
                            insBoardData.insPieceInfo.intValue(intCol, intRow) <> insBoardData.insPieceInfo.intValue(intCol, intRow - 1))
                        intCol += 1
                    End While

                    '線を引く
                    Me.m_lstPanel.Add(Me.m_pnlDrawLine(intCol_start, intRow, intCol, intRow))
                End If
            Next
        Next

    End Sub

    ''' <summary>
    ''' 【境界表示用の線を引く】チェックボックスの左上を基準とする
    ''' </summary>
    ''' <param name="intCol_start">開始行</param>
    ''' <param name="intRow_start">開始列</param>
    ''' <param name="intCol_end">終了行</param>
    ''' <param name="intRow_end">終了列</param>
    Private Function m_pnlDrawLine(intCol_start As Integer, intRow_start As Integer, intCol_end As Integer, intRow_end As Integer) As Panel

        Dim intCheckBoxSize As Integer = Me.m_intCheckBoxSize

        Dim insPanel As New Panel
        With insPanel
            .Name = "pnlLine" & (Me.m_lstPanel.Count + 1).ToString
            .BackColor = Color.Black
            .Anchor = AnchorStyles.Top

            '地点：チェックボックスの間に来るように数値を1だけずらしておく
            .Location = New Point(intCheckBoxSize * intCol_start + m_int_LineWidth - 1, intCheckBoxSize * intRow_start + m_int_LineWidth - 1)

            'サイズ：座標から算出、そのままだと細いので+2して補正
            .Size = New Size(intCheckBoxSize * (intCol_end - intCol_start) + m_int_LineWidth, intCheckBoxSize * (intRow_end - intRow_start) + m_int_LineWidth)

        End With

        Me.Controls.Add(insPanel)

        'チェックボックスの上に表示するように前後を変更
        insPanel.BringToFront()

        Return insPanel

    End Function

#End Region

#Region "●画面→データ"

    ''' <summary>
    ''' 【チェックボックスの状況から配列を作成】
    ''' </summary>
    ''' <returns></returns>
    Public Function insGetData() As cls2DIntArray

        Dim insReturn As New cls2DIntArray(Me.m_intColumnNum, Me.m_intRowNum)

        For intIndex As Integer = 0 To Me.m_insCheckBoxes.Count - 1
            insReturn.intValue(intIndex) = If(Me.m_insCheckBoxes(intIndex).Checked, 1, 0)
        Next

        Return insReturn

    End Function

#End Region

#Region "●チェックボックス関連イベント"

    ''' <summary>
    ''' 【チェック状態変化時色変更】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_chkXXX_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim insChkBox = CType(sender, CheckBox)
        If insChkBox.Checked Then
            insChkBox.BackColor = Color.Black
        Else
            insChkBox.BackColor = Color.White
        End If

    End Sub

    ''' <summary>
    ''' 【方向キーでの移動】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_chkXXX_PreviewKeyDown(ByVal sender As Object, ByVal e As PreviewKeyDownEventArgs) 'Handles MyBase.PreviewKeyDown

        '（ほぼありえないが）1列または1行しかない場合はスキップ
        If Me.m_intColumnNum = 1 OrElse
                Me.m_intRowNum = 1 Then
            Exit Sub
        End If

        '上下キー押下時のチェックボックス位置をタブインデックスから取得
        Dim intChkIndex As Integer = DirectCast(sender, CheckBox).TabIndex

        Select Case e.KeyCode
            Case Keys.Right

                '一番右側に表示されるチェックボックスの場合のみ対処
                'KeyDownにて移動が発生してしまうので、一つ左に移動することで相殺
                If intChkIndex Mod Me.m_intColumnNum = Me.m_intColumnNum - 1 Then Me.ActiveControl = Me.m_insCheckBoxes(intChkIndex - 1)

            Case Keys.Left
                '一番左側にいた場合は、一つ右に移動して相殺
                If intChkIndex Mod Me.m_intColumnNum = 0 Then Me.ActiveControl = Me.m_insCheckBoxes(intChkIndex + 1)

            Case Keys.Up
                '一番上の行にいた場合は、移動しないように相殺
                'それ以外の場合は、一列上に移動できるように調整
                If intChkIndex \ Me.m_intColumnNum = 0 Then
                    Me.ActiveControl = Me.m_insCheckBoxes(intChkIndex + 1)
                Else
                    Me.ActiveControl = Me.m_insCheckBoxes(intChkIndex - Me.m_intColumnNum + 1)
                End If

            Case Keys.Down
                '一番下の行にいた場合は、移動しないように相殺
                'それ以外の場合は、一列下に移動できるように調整
                If intChkIndex \ Me.m_intColumnNum = Me.m_intRowNum - 1 Then
                    Me.ActiveControl = Me.m_insCheckBoxes(intChkIndex - 1)
                Else
                    Me.ActiveControl = Me.m_insCheckBoxes(intChkIndex + Me.m_intColumnNum - 1)
                End If

        End Select

    End Sub

    ''' <summary>
    ''' 【Enter】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_chkXXX_Enter(sender As Object, e As EventArgs)

        '現在いるチェックボックスのみTabStop=Trueにする
        For Each insChk In Me.m_insCheckBoxes
            insChk.TabStop = False
        Next

        DirectCast(sender, CheckBox).TabStop = True
        DirectCast(sender, CheckBox).ForeColor = Color.MediumBlue

    End Sub

    ''' <summary>
    ''' 【Leave】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_chkXXX_Leave(sender As Object, e As EventArgs)

        'LeaveでTabStopを操作すると、uclから抜けたときに戻れなくなるので放置
        DirectCast(sender, CheckBox).ForeColor = SystemColors.ControlText

    End Sub

#End Region

End Class
