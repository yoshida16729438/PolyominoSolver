Option Compare Binary
Option Explicit On
Option Strict On

''' <summary>
''' 【メイン画面】
''' </summary>
Public Class frmMain

#Region "●変数"

    ''' <summary>
    ''' 盤面配列（行、列）設置不可は-1
    ''' </summary>
    Private m_insBoard As cls2DIntArray

    ''' <summary>
    ''' ピースリスト（行、列）実体部分は自然数
    ''' </summary>
    Private m_lstPiece As List(Of cls2DIntArray)

    ''' <summary>
    ''' 結果リスト
    ''' </summary>
    Private m_lstResult As List(Of cls2DIntArray)

#End Region

#Region "●Load"

    ''' <summary>
    ''' 【Load】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_frmStart_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.m_lstPiece = New List(Of cls2DIntArray)
        Me.m_lstResult = New List(Of cls2DIntArray)

        'コンフィグ読込&反映
        Me.m_subLoadConfig()

        'ログ読込
        Me.m_subLoadData()

        '盤面ラベル表示
        Me.m_subUpdateCurrentBoardLabel()

    End Sub

    ''' <summary>
    ''' 【コンフィグ読込】
    ''' </summary>
    Private Sub m_subLoadConfig()

        '読込
        g_subLoadConfig()

        'コンボボックスに反映
        For intC = g_insConfig.intBoardMinimumSize To g_insConfig.intBoardMaximumSize
            Me.cmbWidth.Items.Add(intC)
            Me.cmbHeight.Items.Add(intC)
        Next

        Me.cmbWidth.SelectedIndex = 0
        Me.cmbHeight.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' 【ファイルあれば読込】
    ''' </summary>
    Private Sub m_subLoadData()

        'コマンドライン解析
        Dim strArgs As String() = Environment.GetCommandLineArgs

        'コマンドラインが2要素（実行ファイルのパス、読み込むファイルのパス）の場合はファイル読み込み
        If strArgs.Count >= 2 AndAlso
                IO.File.Exists(strArgs(1)) Then
            Me.m_subLoadFile(strArgs(1))
        End If

    End Sub

#End Region

#Region "●Enter・Leave"

    ''' <summary>
    ''' 【Enter】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_ctrXXX_Enter(sender As Object, e As EventArgs) Handles _
            btnSetBoard.Enter,
            btnSetPiece.Enter,
            btnStartSolve.Enter,
            btnClose.Enter,
            radOnlyMove.Enter,
            radRotationFree.Enter,
            radAllFree.Enter

        DirectCast(sender, Control).ForeColor = Color.MediumBlue

    End Sub

    ''' <summary>
    ''' 【Leave】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_ctrXXX_Leave(sender As Object, e As EventArgs) Handles _
            btnSetBoard.Leave,
            btnSetPiece.Leave,
            btnStartSolve.Leave,
            btnClose.Leave,
            radOnlyMove.Leave,
            radRotationFree.Leave,
            radAllFree.Leave

        DirectCast(sender, Control).ForeColor = SystemColors.ControlText

    End Sub

#End Region

#Region "●Button"

    ''' <summary>
    ''' 【終了】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        Me.Dispose()
    End Sub

    ''' <summary>
    ''' 【盤面作成】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnSetBoard_Click(sender As Object, e As EventArgs) Handles btnSetBoard.Click

        Dim insBoard_set As New cls2DIntArray(CInt(Me.cmbWidth.Text), CInt(Me.cmbHeight.Text))

        If Me.m_insBoard IsNot Nothing Then

            '盤面サイズ選択が変更されている場合
            If Me.m_insBoard.intWidth <> CInt(Me.cmbWidth.Text) OrElse
                    Me.m_insBoard.intHeight <> CInt(Me.cmbHeight.Text) Then
                If MsgBox("盤面の幅か高さが変更されています。" & vbCrLf &
                          "このまま編集を続ける場合、盤面の設定がリセットされます。" & vbCrLf & vbCrLf &
                          "続行しますか？", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Yes Then
                    Exit Sub
                End If
            Else
                'サイズ選択に変更がない場合：設定済の盤面を渡す
                insBoard_set = Me.m_insBoard
            End If
        End If

        Dim insBoard As New frmBoard(insBoard_set, uclCheckBox.Enum_Mode.盤面設定)

        If insBoard.ShowDialog = DialogResult.OK Then

            '盤面更新
            Me.m_insBoard = insBoard.insBoard
        End If
        insBoard.Dispose()

        Me.m_subUpdateCurrentBoardLabel()

    End Sub

    ''' <summary>
    ''' 【ピース作成】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnSetPiece_Click(sender As Object, e As EventArgs) Handles btnSetPiece.Click

        Dim insPiece As New frmPieceMenu(Me.m_lstPiece)
        insPiece.ShowDialog()

        insPiece.Dispose()
    End Sub

    ''' <summary>
    ''' 【探索開始】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnStartSolve_Click(sender As Object, e As EventArgs) Handles btnStartSolve.Click

        '開始前確認
        If Not Me.m_blnCheckBeforeSolve Then Exit Sub

        Dim insMode As frmProgress.Enum_Mode
        If Me.radAllFree.Checked Then
            insMode = frmProgress.Enum_Mode.回転反転可
        ElseIf Me.radRotationFree.Checked Then
            insMode = frmProgress.Enum_Mode.回転可反転不可
        Else
            insMode = frmProgress.Enum_Mode.回転反転不可
        End If

        '最小サイズのピースリストを作成
        Dim lstPiece As New List(Of cls2DIntArray)
        For Each insPiece In Me.m_lstPiece
            lstPiece.Add(insPiece.insGetMinimumArray)
        Next

        Dim insSolve As New frmProgress(insMode, Me.m_insBoard, lstPiece)
        insSolve.blnMultiThread = Me.mnuMultiThread.Checked

        Select Case insSolve.intGapOfPixelNum
            Case Is > 0

                If MsgBox(String.Format("ピースに設定されているマス目の合計が盤面より{0}マスだけ少ないですが、開始してよろしいですか？", insSolve.intGapOfPixelNum),
                    MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Yes Then

                    Exit Sub
                End If

            Case Is < 0
                MsgBox("ピースに設定されているマス目の合計が盤面よりも多いため、開始できません。", MsgBoxStyle.Exclamation)
                Exit Sub

        End Select

        Select Case insSolve.ShowDialog
            Case DialogResult.OK

                If insSolve.lstResult.Count > 0 Then
                    Dim insBoard As New frmBoard(insSolve.lstResult(0).insBoard, uclCheckBox.Enum_Mode.解答表示)
                    insBoard.lstResult = insSolve.lstResult
                    insBoard.ShowDialog()
                End If

            Case DialogResult.Abort
                Dim insExc As New clsDataFileMaker
                insExc.subSetData(Me.m_insBoard, Me.m_lstPiece, g_insConfig.intPieceSize, True)
                insExc.blnSave()
                Me.Close()
        End Select
    End Sub

#End Region

#Region "●Menu"

    ''' <summary>
    ''' 【読込】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_mnuLoad_Click(sender As Object, e As EventArgs) Handles mnuLoad.Click

        If MsgBox("ファイルの読込を行った場合、現在設定中の内容はリセットされます。" & vbCrLf &
                  "よろしいですか？", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        Using insDialog As New OpenFileDialog
            With insDialog
                .CheckFileExists = True
                .CheckPathExists = True
                .DefaultExt = ".xml"
                .Filter = "ﾃﾞｰﾀﾌｧｲﾙ(*.xml)|*.xml|全てのﾌｧｲﾙ(*.*)|*.*"
                .FilterIndex = 0
                .Multiselect = False
                .Title = "データファイルの選択"
                .ValidateNames = True

                If .ShowDialog = DialogResult.OK Then
                    Me.m_subLoadFile(.FileName)
                    Me.m_subUpdateCurrentBoardLabel()
                End If
            End With
        End Using

    End Sub

    ''' <summary>
    ''' 【保存】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_mnuSave_Click(sender As Object, e As EventArgs) Handles mnuSave.Click

        Using insDialog As New SaveFileDialog
            With insDialog
                .AddExtension = True
                .DefaultExt = ".xml"
                .FileName = "Polyomino_Solver_Data.xml"
                .Filter = "ﾃﾞｰﾀﾌｧｲﾙ(*.xml)|*.xml|全てのﾌｧｲﾙ(*.*)|*.*"
                .FilterIndex = 0
                .OverwritePrompt = True
                .Title = "保存先ファイルの指定"

                If .ShowDialog = DialogResult.OK Then

                    Dim insDataFileMaker As New clsDataFileMaker
                    insDataFileMaker.subSetData(Me.m_insBoard, Me.m_lstPiece, g_insConfig.intPieceSize)
                    If insDataFileMaker.blnSave(.FileName) Then
                        MsgBox("ファイルを保存しました。", MsgBoxStyle.Information)
                    Else
                        MsgBox("ファイルの保存に失敗しました。", MsgBoxStyle.Exclamation)
                    End If
                End If
            End With
        End Using

    End Sub

    ''' <summary>
    ''' 【高速モード（マルチスレッド）】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_mnuMultiThread_CheckedChanged(sender As Object, e As EventArgs) Handles mnuMultiThread.CheckedChanged

        If Me.mnuMultiThread.Checked Then

            If MsgBox("本設定を有効にして探索した場合、探索時間の短縮が期待できる代わりに、ＣＰＵ使用率が劇的に増加します。" & vbCrLf & vbCrLf &
                      "本当に本設定を有効にしてよろしいですか？", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Yes Then
                Me.mnuMultiThread.Checked = False
            End If

        End If

    End Sub

#End Region

#Region "●ファイル読込"

    ''' <summary>
    ''' 【ファイル読込】
    ''' </summary>
    ''' <param name="strPath">ファイルパス</param>
    Private Sub m_subLoadFile(strPath As String)

        Dim insData As New clsDataFileMaker
        With insData

            If .blnLoad(strPath) Then

                '盤面
                If .blnLoadBoard Then

                    If .insBoard IsNot Nothing Then
                        Me.m_insBoard = .insBoard
                        Me.m_subSelectItemInComboBox(Me.cmbWidth, .insBoard.intWidth)
                        Me.m_subSelectItemInComboBox(Me.cmbHeight, .insBoard.intHeight)
                    Else
                        Me.m_insBoard = Nothing
                    End If

                End If

                'ピース
                If .blnLoadPiece Then

                    If .lstPiece IsNot Nothing Then
                        Me.m_lstPiece = .lstPiece

                        If .lstPiece.Count > 0 Then
                            g_insConfig.intPieceSize = .lstPiece(0).intHeight
                        End If
                    Else
                        Me.m_lstPiece.Clear()
                    End If

                End If

                '盤面・ピース両方読込成功時のみメッセージ表示
                If .blnLoadBoard AndAlso .blnLoadPiece Then
                    MsgBox("データを読み込みました。", MsgBoxStyle.Information)
                End If

            End If

        End With
    End Sub

    ''' <summary>
    ''' 【コンボボックスの値を選択する（なければ追加する）】
    ''' </summary>
    ''' <param name="insCombo">コンボボックスインスタンス</param>
    ''' <param name="intValue">選択する値</param>
    Private Sub m_subSelectItemInComboBox(insCombo As ComboBox, intValue As Integer)

        With insCombo
            If Not .Items.Contains(intValue) Then
                .Items.Add(intValue)
            End If

            .SelectedItem = intValue
        End With
    End Sub

#End Region

#Region "●解答探索開始前確認"

    ''' <summary>
    ''' 【解答探索開始前確認】
    ''' </summary>
    ''' <returns></returns>
    Private Function m_blnCheckBeforeSolve() As Boolean

        '盤面設定確認
        If Me.m_insBoard Is Nothing Then
            MsgBox("盤面が設定されていません。", MsgBoxStyle.Exclamation)
            Me.btnSetBoard.Focus()
            Return False
        End If

        'ピースが設定されているか
        If Me.m_lstPiece.Count = 0 Then
            MsgBox("ピースが１つも設定されていません。", MsgBoxStyle.Exclamation)
            Me.btnSetPiece.Focus()
            Return False
        End If

        '盤面サイズ>=ピースサイズ
        If Not Me.m_blnBoardIsLargerThenPieces Then
            MsgBox("盤面サイズよりも大きいピースがあるため、解くことができません。", MsgBoxStyle.Exclamation)
            Return False
        End If

        Return True

    End Function

    ''' <summary>
    ''' 【ピースの最大サイズが盤面の最大サイズを超えていないかの簡易チェック】
    ''' </summary>
    ''' <returns></returns>
    Private Function m_blnBoardIsLargerThenPieces() As Boolean

        '盤面最小サイズ取得
        Dim intBoard_Min As Integer = Math.Min(Me.m_insBoard.intHeight, Me.m_insBoard.intWidth)
        Dim intBoard_Max As Integer = Math.Max(Me.m_insBoard.intHeight, Me.m_insBoard.intWidth)

        '各ピースのサイズと比較
        Dim insPiece_MinArray As cls2DIntArray
        Dim intPiece_Min As Integer
        Dim intPiece_Max As Integer

        For Each insPiece In Me.m_lstPiece
            insPiece_MinArray = insPiece.insGetMinimumArray
            intPiece_Min = Math.Min(insPiece_MinArray.intWidth, insPiece_MinArray.intHeight)
            intPiece_Max = Math.Max(insPiece_MinArray.intWidth, insPiece_MinArray.intHeight)

            If intBoard_Max < intPiece_Max OrElse
                    intBoard_Min < intPiece_Min Then
                Return False
            End If
        Next

        Return True
    End Function

#End Region

#Region "●現在の盤面ラベル"

    ''' <summary>
    ''' 【現在の盤面ラベルの表示更新】
    ''' </summary>
    Private Sub m_subUpdateCurrentBoardLabel()

        If Me.m_insBoard Is Nothing Then
            Me.lblCurrentBoard.Text = "※現在の設定： 未設定"
        Else
            Dim strFormat As String = "※現在の設定： 幅{0}　高さ{1}"

            Me.lblCurrentBoard.Text = String.Format(strFormat, Me.m_insBoard.intWidth, Me.m_insBoard.intHeight)
        End If

    End Sub

#End Region

End Class
