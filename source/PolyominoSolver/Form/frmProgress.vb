Option Compare Binary
Option Explicit On
Option Strict On
Imports System.ComponentModel
Imports System.Runtime.InteropServices

''' <summary>
''' 【プログレスバー表示】
''' </summary>
Public Class frmProgress

#Region "●定数"

    ''' <summary>
    ''' 【ダミーマス値】
    ''' </summary>
    Private Const M_INT_DUMMY_VALUE As Integer = -1

#End Region

#Region "●変数"

    ''' <summary>
    ''' 回転・反転の可否モード指定
    ''' </summary>
    Public Enum Enum_Mode
        回転反転可 = 7
        回転可反転不可 = 3
        回転反転不可 = 0
    End Enum

    ''' <summary>
    ''' 【回転・反転モード】
    ''' </summary>
    Private ReadOnly m_enmMode As Enum_Mode

    ''' <summary>
    ''' 【盤面】
    ''' </summary>
    Private ReadOnly m_insBoardData As clsBoardData

    ''' <summary>
    ''' 【空白ありパターン用盤面リスト】
    ''' </summary>
    Private m_lstBoardData As List(Of clsBoardData)

    ''' <summary>
    ''' 【ピースリスト】
    ''' </summary>
    Private ReadOnly m_lstPointList As List(Of clsPointList)

    ''' <summary>
    ''' 【解答候補リスト追加用ロックオブジェクト】
    ''' </summary>
    Private m_insLockObj_Temp As Object

    ''' <summary>
    ''' 【解答確定リスト追加用ロックオブジェクト】
    ''' </summary>
    Private m_insLockObj_Ans As Object

    ''' <summary>
    ''' 【進捗報告用ロックオブジェクト】
    ''' </summary>
    Private m_insLockObj_ReportProgress As Object

    ''' <summary>
    ''' 【解答追加タスクリスト】
    ''' </summary>
    Private m_lstAnswerTasks As List(Of Task)

    ''' <summary>
    ''' 【解答候補リスト】
    ''' </summary>
    Private m_lstResult_Temp As List(Of clsBoardData)

    ''' <summary>
    ''' 【解答確定リスト】
    ''' </summary>
    Private m_lstResult As List(Of clsBoardData)

    ''' <summary>
    ''' 【解答文字列ハッシュセット】
    ''' </summary>
    Private m_hssResultString As HashSet(Of String)

    ''' <summary>
    ''' 【開始時刻】
    ''' </summary>
    Private m_datStartTime As Date

    ''' <summary>
    ''' 【マルチスレッドで探索するか】
    ''' </summary>
    Public blnMultiThread As Boolean = False

    ''' <summary>
    ''' 【PrevIndexリスト（重複解答排除用）】
    ''' </summary>
    Private m_lstPrevIndex As List(Of List(Of Integer))

#End Region

#Region "●プロパティ"

    ''' <summary>
    ''' 【結果リスト】
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property lstResult As List(Of clsBoardData)
        Get
            Return Me.m_lstResult
        End Get
    End Property

    ''' <summary>
    ''' 【盤面のピクセル数とピースのピクセル数の差】
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property intGapOfPixelNum As Integer

#End Region

#Region "●初期化"

    ''' <summary>
    ''' 【New】
    ''' </summary>
    ''' <param name="enmMode">探索モード</param>
    ''' <param name="insBoard">盤面</param>
    ''' <param name="lstSourcePiece">ピースリスト</param>
    Public Sub New(enmMode As Enum_Mode, insBoard As cls2DIntArray, lstSourcePiece As List(Of cls2DIntArray))

        ' この呼び出しはデザイナーで必要です。
        Me.InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

        Me.m_enmMode = enmMode
        Me.m_insBoardData = New clsBoardData(insBoard)

        'ピースをcls2DIntArrayからclsPointListへ変換
        Me.m_lstPointList = Me.m_lstGetPointList(lstSourcePiece)

        '盤面とピースのピクセル数の差を算出
        Me.intGapOfPixelNum = Me.m_intCheckPixelNum(lstSourcePiece)

    End Sub

    ''' <summary>
    ''' 【盤面/ピースの整数値合計算出】
    ''' </summary>
    ''' <param name="insArray">配列</param>
    ''' <returns></returns>
    Private Function m_intSumOfArray(insArray As cls2DIntArray) As Integer

        Dim intSum As Integer = 0

        For intIndex As Integer = 0 To insArray.intCount - 1
            intSum += Math.Abs(insArray.intValue(intIndex))
        Next

        Return intSum

    End Function

    ''' <summary>
    ''' 【盤面のマスの数とピースのマスの合計を比較】
    ''' </summary>
    ''' <returns></returns>
    ''' <param name="lstPiece">ピース元データ</param>
    Private Function m_intCheckPixelNum(lstPiece As List(Of cls2DIntArray)) As Integer

        Dim intBoardSize As Integer = Me.m_insBoardData.insBoard.intCount - Me.m_intSumOfArray(Me.m_insBoardData.insBoard)
        Dim intPieceSize As Integer = 0
        For Each insPiece In lstPiece
            intPieceSize += Me.m_intSumOfArray(insPiece)
        Next

        Return intBoardSize - intPieceSize

    End Function

    ''' <summary>
    ''' 【ピースをcls2DIntArrayからclsPointListへ変換】
    ''' </summary>
    ''' <returns></returns>
    ''' <param name="lstSource">元データ</param>
    Private Function m_lstGetPointList(lstSource As List(Of cls2DIntArray)) As List(Of clsPointList)

        Dim lstPiece As New List(Of clsPointList)
        Me.m_lstPrevIndex = New List(Of List(Of Integer))

        '振る番号を設定
        '-1：ダミー、0：未設置、1：壁とするため2からスタート
        Dim intNext As Integer = 2

        For Each insSource As cls2DIntArray In lstSource
            'PointList生成
            Dim insPiece As New clsPointList(insSource, Me.m_enmMode)
            insPiece.intAssociatedValue = intNext

            '重複確認（先頭要素が存在しなければOK）
            '探索は後ろから（同一形状があった場合、既存の一番後ろを参照することで確実に異なるピースを参照する）
            For intC As Integer = lstPiece.Count - 1 To 0 Step -1
                If lstPiece(intC).lstPieceString.Contains(insPiece.lstPieceString(0)) Then
                    insPiece.intAssociatedValue = lstPiece(intC).intAssociatedValue
                    insPiece.intPrevIndex = intC
                    Me.m_lstPrevIndex(insPiece.intAssociatedValue - 2).Add(insPiece.intPrevIndex)
                    Exit For
                End If
            Next

            '重複がなかった場合は最大値更新
            If insPiece.intAssociatedValue = intNext Then
                intNext += 1
                Me.m_lstPrevIndex.Add(New List(Of Integer))
                Me.m_lstPrevIndex.Last.Add(insPiece.intPrevIndex)
            End If

            '追加
            lstPiece.Add(insPiece)
        Next

        Return lstPiece

    End Function

#End Region

#Region "●Form関連"

    ''' <summary>
    ''' 【Load】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_frmProgress_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.m_lstResult = New List(Of clsBoardData)
        Me.m_hssResultString = New HashSet(Of String)
        Me.m_lstAnswerTasks = New List(Of Task)
        Me.m_lstResult_Temp = New List(Of clsBoardData)
        Me.m_insLockObj_Temp = New Object
        Me.m_insLockObj_Ans = New Object
        Me.m_insLockObj_ReportProgress = New Object
        Me.lblPastTime.Text = "経過時間：0秒"
        Me.lblState.Text = "準備中"
        Me.Text = "準備中"
        Me.btnCancel.Enabled = False

    End Sub

    ''' <summary>
    ''' 【Shown】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_frmProgress_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown

        '表示更新
        Application.DoEvents()

        Cursor.Current = Cursors.WaitCursor

        '空白ありパターンの場合、空白パターンごとの盤面リスト作成
        '空白多い場合は時間がかかるのでここで計算
        If Me.intGapOfPixelNum > 0 Then Me.m_subMakeDummyBoardList()

        'プログレスバー最大値設定
        Me.m_subInitProgressBar()
        Me.ttpShinchoku.SetToolTip(Me.prbProgress, "完了：0/" & Me.prbProgress.Maximum.ToString)

        Me.lblState.Text = "探索中"
        Me.Text = "探索中"
        Cursor.Current = Cursors.Default
        Me.btnCancel.Enabled = True

        '開始
        Me.bgwSolve.RunWorkerAsync()

        '開始時刻記録
        Me.m_datStartTime = Now

        '経過時間表示用タイマー開始
        Me.tmrPastTime.Start()

    End Sub

    ''' <summary>
    ''' 【FormClosing】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_frmProgress_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        e.Cancel = True

        Select Case Me.DialogResult
            Case DialogResult.OK,
                 DialogResult.Abort,
                 DialogResult.No

                e.Cancel = False

            Case Else

                If MsgBox("中止した場合、ここまでの探索は全て破棄されます。本当に中止してよろしいですか？", vbYesNo Or vbDefaultButton2 Or vbExclamation) = MsgBoxResult.Yes Then

                    Me.lblState.Text = "停止中"
                    Me.bgwSolve.CancelAsync()
                End If

        End Select

    End Sub

#End Region

#Region "●BackgroundWorker"

    ''' <summary>
    ''' 【DoWork】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_bgwSolve_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgwSolve.DoWork

        'デバッグ用：ピース全表示
        Me.m_subShowPieces()

        If Me.intGapOfPixelNum = 0 Then

            If Me.blnMultiThread Then
                Me.m_subSolve_NoEmpty_Multi(Me.m_insBoardData)
            Else
                Me.m_subSolve_NoEmpty_Single(Me.m_insBoardData)
            End If

        Else
            If Me.blnMultiThread Then
                Me.m_subSolve_WithEmpty_Multi()
            Else
                Me.m_subSolve_WithEmpty_Single()
            End If

        End If

        e.Cancel = Me.bgwSolve.CancellationPending

    End Sub

    ''' <summary>
    ''' 【進捗更新】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_bgwSolve_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles bgwSolve.ProgressChanged

        Me.prbProgress.Value += 1
        Me.ttpShinchoku.SetToolTip(Me.prbProgress, String.Format("完了：{0}/{1}", Me.prbProgress.Value.ToString, Me.prbProgress.Maximum.ToString))

    End Sub

    ''' <summary>
    ''' 【完了】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_bgwSolve_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bgwSolve.RunWorkerCompleted

        Me.btnCancel.Enabled = False

        '解答追加タスク待ち受け
        Me.m_subWaitForAnswerTask()

        '計測停止
        Me.tmrPastTime.Stop()

        '画面を前に持ってくる
        Me.Activate()

        If e.Error IsNot Nothing Then

            MsgBox("処理中にエラーが発生しました。開発者に報告のご協力をお願いいたします。", vbExclamation)

            Me.DialogResult = DialogResult.Abort
            Me.Close()

        ElseIf e.Cancelled Then

            MsgBox("処理を中断しました。", vbInformation)
            Me.DialogResult = DialogResult.No
            Me.Close()
        Else
            Me.lblState.Text = "完了"

            '点滅させる
            Me.m_subFlashTaskBar()

            If Me.m_lstResult.Count = 0 Then
                MsgBox("解答がありません。解けないパズルです。", vbInformation)
            Else
                MsgBox(Me.m_lstResult.Count & "件の解答が見つかりました。", vbInformation)
            End If

            Me.DialogResult = DialogResult.OK
            Me.Close()

        End If
    End Sub

    ''' <summary>
    ''' 【ピースを全て表示（デバッグ用）】
    ''' </summary>
    Private Sub m_subShowPieces()

#If Not DEBUG Then
        Exit Sub
#End If

        Dim strPiece As String
        Dim inX As Integer
        Dim inY As Integer
        For intIndex As Integer = 0 To Me.m_lstPointList.Count - 1
            For intDirection As Integer = 0 To Me.m_lstPointList(intIndex).intDirectionNum - 1
                With Me.m_lstPointList(intIndex).insRotate(intDirection)
                    Debug.WriteLine((intIndex + 1).ToString & "-" & (intDirection + 1).ToString)
                    strPiece = String.Empty
                    For intY As Integer = 0 To .intHeight - 1
                        inY = intY  '警告回避
                        For intX As Integer = 0 To .intWidth - 1
                            inX = intX  '警告回避
                            If .lstPoint.Exists(Function(x) x.X = inX AndAlso x.Y = inY) Then
                                strPiece &= "■"
                            Else
                                strPiece &= "  "
                            End If
                        Next
                        strPiece &= vbCrLf
                    Next
                    Debug.WriteLine(strPiece)
                End With
            Next
        Next

    End Sub

    ''' <summary>
    ''' 【解答追加タスク終了待ち受け】
    ''' </summary>
    Private Sub m_subWaitForAnswerTask()

        While True

            'タスクが完了したものをリストから削除し、リストが0件になれば終了
            For intIndex As Integer = Me.m_lstAnswerTasks.Count - 1 To 0 Step -1

                If Me.m_lstAnswerTasks(intIndex).IsCompleted Then
                    Me.m_lstAnswerTasks.RemoveAt(intIndex)
                End If

            Next

            If Me.m_lstAnswerTasks.Count = 0 Then Exit Sub

            '少し待つ
            Threading.Thread.Sleep(100)

        End While

    End Sub

#End Region

#Region "●BackgroundWorkerの処理"

    ''' <summary>
    ''' 【空白マスなしパターン探索（マルチスレッド版）】
    ''' </summary>
    ''' <param name="insBoardData">盤面</param>
    Private Sub m_subSolve_NoEmpty_Multi(insBoardData As clsBoardData)

        '同一インスタンスへのマルチスレッドからのアクセス回避用
        'インデクサ（ピース番号、回転番号）
        Dim insIndexer As New cls2DIntArray(Me.m_lstPointList.Count, Me.m_enmMode + 1)
        '各スレッド用ピースリスト
        Dim lstP As New List(Of List(Of clsPointList))
        '各スレッド用盤面
        Dim lstBoardData As New List(Of clsBoardData)

        Dim intNum As Integer = 0
        For intPieceIndex As Integer = 0 To Me.m_lstPointList.Count - 1
            For intPieceRotate As Integer = 0 To Me.m_lstPointList(intPieceIndex).intDirectionNum - 1
                insIndexer.intValue(intPieceIndex, intPieceRotate) = intNum
                lstP.Add(Me.m_lstCopyPieceList)
                lstBoardData.Add(insBoardData.insCopy)
                intNum += 1
            Next
        Next

        'マルチスレッド処理
        Parallel.For(0, Me.m_lstPointList.Count,
                     Sub(intPieceIndex)

                         Dim lstPiec_t As New List(Of clsPointList)(Me.m_lstPointList)

                         Parallel.For(0, Me.m_lstPointList(intPieceIndex).intDirectionNum,
                                      Sub(intDirectionNum)

                                          If Not Me.bgwSolve.CancellationPending Then

                                              Dim intIndex As Integer = insIndexer.intValue(intPieceIndex, intDirectionNum)

                                              '重複ピースは飛ばす（1ピース目として使用しない）
                                              If lstP(intIndex)(intPieceIndex).intPrevIndex = clsPointList.G_INT_DEF_PREV_INDEX Then

                                                  '探索
                                                  Me.m_subTryPut(lstBoardData(intIndex), lstP(intIndex), intPieceIndex, intDirectionNum)

                                                  '進捗更新
                                                  SyncLock Me.m_insLockObj_ReportProgress
                                                      Me.bgwSolve.ReportProgress(0)
                                                  End SyncLock
                                              End If
                                          End If

                                      End Sub)

                         '一時リストから確定リストに移動する
                         SyncLock Me.m_insLockObj_Ans
                             Me.m_lstAnswerTasks.Add(Task.Run(Sub() Me.m_subCheckDuplicateAns()))
                         End SyncLock

                     End Sub)
    End Sub

    ''' <summary>
    ''' 【空白なしパターン探索（シングルスレッド版）】
    ''' </summary>
    ''' <param name="insBoardData">盤面</param>
    Private Sub m_subSolve_NoEmpty_Single(insBoardData As clsBoardData)

        For intPieceIndex As Integer = 0 To Me.m_lstPointList.Count - 1
            For intDirectionNum As Integer = 0 To Me.m_lstPointList(intPieceIndex).intDirectionNum - 1

                If Not Me.bgwSolve.CancellationPending Then

                    '重複ピースは飛ばす（1ピース目として使用しない）
                    If Me.m_lstPointList(intPieceIndex).intPrevIndex = clsPointList.G_INT_DEF_PREV_INDEX Then

                        '探索
                        Me.m_subTryPut(insBoardData, Me.m_lstPointList, intPieceIndex, intDirectionNum)

                        '進捗更新
                        Me.bgwSolve.ReportProgress(0)
                    End If
                End If
            Next

            '一時リストから確定リストに移動する（ここだけ非同期）
            SyncLock Me.m_insLockObj_Ans
                Me.m_lstAnswerTasks.Add(Task.Run(Sub() Me.m_subCheckDuplicateAns()))
            End SyncLock
        Next

    End Sub

    ''' <summary>
    ''' 【空白ありパターン探索（マルチスレッド版）】
    ''' </summary>
    Private Sub m_subSolve_WithEmpty_Multi()

        '各空白マスパターンを空白なしパターンのマルチスレッド版に渡す
        For Each insBoardData As clsBoardData In Me.m_lstBoardData
            If Not Me.bgwSolve.CancellationPending Then
                Me.m_subSolve_NoEmpty_Multi(insBoardData)
            End If
        Next

    End Sub

    ''' <summary>
    ''' 【空白ありパターン探索（シングルスレッド版）】
    ''' </summary>
    Private Sub m_subSolve_WithEmpty_Single()

        '各空白マスパターンを空白なしパターンのシングルスレッド版に渡す
        For Each insBoardData As clsBoardData In Me.m_lstBoardData
            If Not Me.bgwSolve.CancellationPending Then
                Me.m_subSolve_NoEmpty_Single(insBoardData)
            End If
        Next
    End Sub

    ''' <summary>
    ''' 【ピースリストのコピーを作成する】
    ''' </summary>
    ''' <returns></returns>
    Private Function m_lstCopyPieceList() As List(Of clsPointList)

        Dim lstCopy As New List(Of clsPointList)
        For Each insPoint As clsPointList In Me.m_lstPointList
            lstCopy.Add(insPoint.insCopy)
        Next

        Return lstCopy

    End Function

#End Region

#Region "●探索"

    ''' <summary>
    ''' 【探索】
    ''' </summary>
    ''' <param name="insBoardData">現在の盤面</param>
    ''' <param name="lstPiece">未使用ピースリスト</param>
    Private Sub m_subSolve(insBoardData As clsBoardData, lstPiece As List(Of clsPointList))

        '未使用ピースあれば探索続行
        If lstPiece.Exists(Function(x) Not x.blnUse) Then

            '未使用ピースを使って探索
            For intIndex As Integer = 0 To lstPiece.Count - 1

                If Not lstPiece(intIndex).blnUse Then

                    For intDirection As Integer = 0 To lstPiece(intIndex).intDirectionNum - 1
                        '設置
                        Me.m_subTryPut(insBoardData, lstPiece, intIndex, intDirection)
                    Next
                End If
            Next
        Else
            '全ピース設置済み=解答候補が一つ完成
            Me.m_subAddAnswer_Temp(insBoardData.insCopy)
        End If

    End Sub

    ''' <summary>
    ''' 【設置を試す】
    ''' </summary>
    ''' <param name="insBoardData">盤面</param>
    ''' <param name="lstPiece">ピースリスト</param>
    ''' <param name="intIndex">ピースのインデックス</param>
    ''' <param name="intDirection">回転</param>
    Private Sub m_subTryPut(insBoardData As clsBoardData, lstPiece As List(Of clsPointList), intIndex As Integer, intDirection As Integer)

        '同一形状がある場合、前のインデックスを参照し、それが未使用なら飛ばす
        If lstPiece(intIndex).intPrevIndex > clsPointList.G_INT_DEF_PREV_INDEX Then
            If Not lstPiece(lstPiece(intIndex).intPrevIndex).blnUse Then Exit Sub
        End If

        '設置可能な場所を探す
        Dim intEmpty_X As Integer
        Dim intEmpty_Y As Integer
        Me.m_subFindEmptyPlace(intEmpty_X, intEmpty_Y, insBoardData.insBoard)

        'ピースを取得
        Dim insPiece As clsPointList.clsPointData = lstPiece(intIndex).insRotate(intDirection)

        '盤面に置けるか確認
        For intY As Integer = insPiece.intHeight - 1 To 0 Step -1
            If Not Me.bgwSolve.CancellationPending Then
                Dim intPutPlace_Y As Integer = intEmpty_Y - intY

                If Me.m_blnIsAbleToPutPiece(insBoardData.insBoard, insPiece, intEmpty_X, intPutPlace_Y) Then

                    '盤面に置く
                    Me.m_subPutPiece(insBoardData, insPiece, intEmpty_X, intPutPlace_Y, lstPiece(intIndex).intAssociatedValue, lstPiece(intIndex).intPrevIndex)

                    '使用済みにする
                    lstPiece(intIndex).blnUse = True

                    '再帰
                    Me.m_subSolve(insBoardData, lstPiece)

                    '取り除く
                    Me.m_subPutPiece(insBoardData, insPiece, intEmpty_X, intPutPlace_Y, 0, 0)

                    '未使用に戻す
                    lstPiece(intIndex).blnUse = False

                    Exit For
                End If
            End If
        Next

    End Sub

    ''' <summary>
    ''' 【未設置の場所を見つける】
    ''' </summary>
    ''' <param name="intX">X座標</param>
    ''' <param name="intY">Y座標</param>
    ''' <param name="insBoard">盤面</param>
    Private Sub m_subFindEmptyPlace(ByRef intX As Integer, ByRef intY As Integer, insBoard As cls2DIntArray)

        '横長の盤面で高速になるよう左から縦方向で探索
        For intX = 0 To insBoard.intWidth - 1
            For intY = 0 To insBoard.intHeight - 1
                If insBoard.intValue(intX, intY) = 0 Then Exit Sub
            Next
        Next

    End Sub

    ''' <summary>
    ''' 【ピースの設置可否判断】
    ''' </summary>
    ''' <param name="insBoard">盤面</param>
    ''' <param name="insPiece">ピース</param>
    ''' <param name="intPutPlace_X">ピースの左上を合わせる位置</param>
    ''' <param name="intPutPlace_Y">ピースの左上を合わせる位置</param>
    ''' <returns></returns>
    Private Function m_blnIsAbleToPutPiece(insBoard As cls2DIntArray,
                                           insPiece As clsPointList.clsPointData,
                                           intPutPlace_X As Integer,
                                           intPutPlace_Y As Integer) As Boolean

        'はみ出す場合は不可
        If intPutPlace_X < 0 OrElse
            intPutPlace_Y < 0 OrElse
            insBoard.intWidth - 1 < intPutPlace_X + insPiece.intWidth - 1 OrElse
            insBoard.intHeight - 1 < intPutPlace_Y + insPiece.intHeight - 1 Then
            Return False
        End If

        '設置しても埋まっていない一番左かつ一番上のマスが埋まらない場合は無意味
        Dim intEmpty_Y As Integer
        For intEmpty_Y = intPutPlace_Y To insBoard.intHeight - 1
            If insBoard.intValue(intPutPlace_X, intEmpty_Y) = 0 Then Exit For
        Next
        If intEmpty_Y <> insPiece.lstPoint(0).Y + intPutPlace_Y Then Return False

        '設置するときに重なる場合は不可
        For Each insPoint As Point In insPiece.lstPoint
            If insBoard.intValue(intPutPlace_X + insPoint.X, intPutPlace_Y + insPoint.Y) <> 0 Then Return False
        Next

        Return True

    End Function

    ''' <summary>
    ''' 【ピースを設置する】
    ''' </summary>
    ''' <param name="insBoardData">盤面</param>
    ''' <param name="insPiece">ピース</param>
    ''' <param name="intPutPlace_X">ピースの左上を合わせる位置</param>
    ''' <param name="intPutPlace_Y">ピースの左上を合わせる位置</param>
    ''' <param name="intValue">配置する数値</param>
    ''' <param name="intPrevIndex">同一形状ピースの前インデックス</param>
    Private Sub m_subPutPiece(insBoardData As clsBoardData,
                              insPiece As clsPointList.clsPointData,
                              intPutPlace_X As Integer,
                              intPutPlace_Y As Integer,
                              intValue As Integer,
                              intPrevIndex As Integer)

        'ピース側で0でない場所を代入
        For Each insPoint As Point In insPiece.lstPoint
            insBoardData.insBoard.intValue(intPutPlace_X + insPoint.X, intPutPlace_Y + insPoint.Y) = intValue
            insBoardData.insPieceInfo.intValue(intPutPlace_X + insPoint.X, intPutPlace_Y + insPoint.Y) = intPrevIndex
        Next

    End Sub

#End Region

#Region "●空白あり盤面探索準備"

    ''' <summary>
    ''' 【ダミーマス配置箇所リスト】
    ''' </summary>
    Private m_lstDummyPointCombination As List(Of List(Of Integer))

    ''' <summary>
    ''' 【空白ありパターン用盤面リストを作成する】
    ''' </summary>
    Private Sub m_subMakeDummyBoardList()

        Me.m_lstBoardData = New List(Of clsBoardData)
        Me.m_lstDummyPointCombination = New List(Of List(Of Integer))

        '空白マスリスト取得
        Dim lstEmptyPoint As List(Of Point) = Me.m_lstEmptyPoint

        '組み合わせリスト作成
        Me.m_subMakeDummyPointList(lstEmptyPoint.Count, New List(Of Integer))

        '盤面リストに変換
        Me.m_subMakeBoardList(lstEmptyPoint)

    End Sub

    ''' <summary>
    ''' 【ダミーマス配置候補地リスト化】
    ''' </summary>
    ''' <returns></returns>
    Private Function m_lstEmptyPoint() As List(Of Point)

        Dim lstPoint As New List(Of Point)
        For intY As Integer = 0 To Me.m_insBoardData.insBoard.intHeight - 1
            For intX As Integer = 0 To Me.m_insBoardData.insBoard.intWidth - 1
                If Me.m_insBoardData.insBoard.intValue(intX, intY) = 0 Then
                    lstPoint.Add(New Point(intX, intY))
                End If
            Next
        Next

        Return lstPoint
    End Function

    ''' <summary>
    ''' 【ダミーマス配置位置の組み合わせ】
    ''' </summary>
    ''' <param name="intEmptyPointNum">空白マスの数</param>
    ''' <param name="lstCombination">組み合わせリスト</param>
    Private Sub m_subMakeDummyPointList(intEmptyPointNum As Integer, lstCombination As List(Of Integer))

        If lstCombination.Count = Me.intGapOfPixelNum Then

            Me.m_lstDummyPointCombination.Add(New List(Of Integer)(lstCombination))
        Else

            Dim intStart As Integer
            If lstCombination.Count = 0 Then
                intStart = 0
            Else
                intStart = lstCombination.Last + 1
            End If

            For intC As Integer = intStart To intEmptyPointNum - Me.intGapOfPixelNum + lstCombination.Count
                lstCombination.Add(intC)
                Me.m_subMakeDummyPointList(intEmptyPointNum, lstCombination)
                lstCombination.RemoveAt(lstCombination.Count - 1)
            Next
        End If

    End Sub

    ''' <summary>
    ''' 【組み合わせリスト→盤面リストに変換】
    ''' </summary>
    ''' <param name="lstEmptyPoint">空白マスリスト</param>
    Private Sub m_subMakeBoardList(lstEmptyPoint As List(Of Point))

        Dim insBoardData As clsBoardData
        Dim intX As Integer
        Dim intY As Integer
        Dim hssBoardString As New HashSet(Of String)
        Dim blnDuplicate As Boolean
        Dim strRotateBoard As String()

        For Each lstCombi As List(Of Integer) In Me.m_lstDummyPointCombination

            insBoardData = Me.m_insBoardData.insCopy

            '組み合わせで指定された位置を-1に変更する
            For Each intIndex As Integer In lstCombi

                intX = lstEmptyPoint(intIndex).X
                intY = lstEmptyPoint(intIndex).Y

                insBoardData.insBoard.intValue(intX, intY) = M_INT_DUMMY_VALUE
            Next

            '重複がないことを確認して追加
            blnDuplicate = False
            strRotateBoard = Me.m_strBoardRotate(insBoardData)
            For Each strRotate As String In strRotateBoard
                If hssBoardString.Contains(strRotate) Then
                    blnDuplicate = True
                    Exit For
                End If
            Next

            If Not blnDuplicate Then
                Me.m_lstBoardData.Add(insBoardData)
                hssBoardString.Add(strRotateBoard(0))
            End If
        Next

    End Sub

#End Region

#Region "●タスクバー点滅"

    ''' <summary>
    ''' 【タスクバーを点滅させる】
    ''' </summary>
    Private Sub m_subFlashTaskBar()

        Dim insFlashInfo As FlashWInfo
        With insFlashInfo
            .cbSize = Marshal.SizeOf(insFlashInfo)
            .hwnd = Me.Handle
            .dwFlags = &H3
            .uCount = 3
            .dwTimeout = 100
        End With

        m_intFlashWindow(insFlashInfo)

    End Sub

    ''' <summary>
    ''' 【タスクバー点滅API】
    ''' </summary>
    ''' <param name="insFWInfo"></param>
    ''' <returns></returns>
    <DllImport("User32", EntryPoint:="FlashWindowEx", CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Auto)>
    Private Shared Function m_intFlashWindow(ByRef insFWInfo As FlashWInfo) As Integer
    End Function

    ''' <summary>
    ''' 【タスクバー点滅用引数】
    ''' </summary>
    <StructLayout(LayoutKind.Sequential)>
    Private Structure FlashWInfo

        ''' <summary>
        ''' 【FlashWinfo構造体のサイズ】
        ''' </summary>
        Dim cbSize As Integer

        ''' <summary>
        ''' 【点滅対象のウィンドウハンドル】
        ''' </summary>
        Dim hwnd As IntPtr

        ''' <summary>
        ''' 【点滅するものを指定】
        ''' </summary>
        Dim dwFlags As Integer

        ''' <summary>
        ''' 【点滅回数】
        ''' </summary>
        Dim uCount As Integer

        ''' <summary>
        ''' 【点滅間隔】
        ''' </summary>
        Dim dwTimeout As Integer

    End Structure

#End Region

#Region "●ボタン"

    ''' <summary>
    ''' 【キャンセルボタン】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' 【Enter】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnXXX_Enter(sender As Object, e As EventArgs) Handles btnCancel.Enter
        DirectCast(sender, Button).ForeColor = Color.MediumBlue
    End Sub

    ''' <summary>
    ''' 【Leave】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_btnXXX_Leave(sender As Object, e As EventArgs) Handles btnCancel.Leave
        DirectCast(sender, Button).ForeColor = SystemColors.ControlText
    End Sub

#End Region

#Region "●Timer"

    ''' <summary>
    ''' 【経過時間表示】
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub m_tmrPastTime_Tick(sender As Object, e As EventArgs) Handles tmrPastTime.Tick

        Dim spnPast As TimeSpan = Now - Me.m_datStartTime
        Dim strFormat As String = "経過時間："
        If spnPast.Days > 0 Then
            strFormat &= "{3}日"
        End If

        If spnPast.Hours > 0 Then
            strFormat &= "{2}時間"
        End If

        If spnPast.Minutes > 0 Then
            strFormat &= "{1}分"
        End If

        strFormat &= "{0}秒"

        Me.lblPastTime.Text = String.Format(strFormat, spnPast.Seconds, spnPast.Minutes, spnPast.Hours, spnPast.Days)

    End Sub

#End Region

#Region "●解答追加"

    ''' <summary>
    ''' 【解答一時リストへ追加】
    ''' </summary>
    ''' <param name="insAns">解答候補</param>
    Private Sub m_subAddAnswer_Temp(insAns As clsBoardData)

        'テンポラリ用ロック取得して排他で仮リストへ追加
        SyncLock Me.m_insLockObj_Temp
            Me.m_lstResult_Temp.Add(insAns)
        End SyncLock

    End Sub

    ''' <summary>
    ''' 【テンポラリ解答リストから解答リストへ移動】
    ''' </summary>
    Private Sub m_subCheckDuplicateAns()

        Dim lstTempAns As New List(Of clsBoardData)

        '現在出ている仮解答を全て回収
        SyncLock Me.m_insLockObj_Temp
            lstTempAns.AddRange(Me.m_lstResult_Temp)
            Debug.WriteLine(Me.m_lstResult_Temp.Count)
            Me.m_lstResult_Temp.Clear()
        End SyncLock

        '無駄にSynclockの奪い合いに参加しないよう終了
        If lstTempAns.Count = 0 Then Exit Sub

        'ダミーマスがある場合は戻す
        For intIndex As Integer = 0 To lstTempAns.Count - 1
            Me.m_subFixDummyValue(lstTempAns(intIndex).insBoard)
        Next

        '回転・反転した文字列を用意
        '単に結合では被りが発生しうるので、アスタリスクを区切りに入れる
        Dim lstTempAnsString As New List(Of String())
        For Each insAns As clsBoardData In lstTempAns
            lstTempAnsString.Add(Me.m_strBoardRotate(insAns))
        Next

        '重複確認して解答リストへ追加
        SyncLock Me.m_insLockObj_Ans

            'そのままの向きは重複しないので飛ばす
            For intIndex As Integer = 0 To lstTempAns.Count - 1

                If Not Me.m_blnHasDuplicateAns(lstTempAnsString(intIndex)) Then
                    Me.m_lstResult.Add(lstTempAns(intIndex))
                    Me.m_hssResultString.Add(lstTempAnsString(intIndex)(0))
                End If
            Next

        End SyncLock

    End Sub

    ''' <summary>
    ''' 【重複解答有無確認】
    ''' </summary>
    ''' <param name="strAnsList">解答文字列</param>
    ''' <returns></returns>
    Private Function m_blnHasDuplicateAns(strAnsList As String()) As Boolean

        'インデックス0は重複しないため飛ばす
        For intC As Integer = 1 To strAnsList.Length - 1
            If Me.m_hssResultString.Contains(strAnsList(intC)) Then
                Return True
            End If
        Next

        Return False
    End Function

    ''' <summary>
    ''' 【ダミーマスを元に戻す】
    ''' </summary>
    ''' <param name="insAns"></param>
    Private Sub m_subFixDummyValue(insAns As cls2DIntArray)

        Dim intVal As Integer

        For intIndex As Integer = 0 To insAns.intCount - 1
            intVal = insAns.intValue(intIndex)
            insAns.intValue(intIndex) = If(intVal = M_INT_DUMMY_VALUE, 0, intVal)
        Next

    End Sub

    ''' <summary>
    ''' 【盤面の回転・文字列化を全通り取得】
    ''' </summary>
    ''' <param name="insBoardData"></param>
    ''' <returns></returns>
    Private Function m_strBoardRotate(insBoardData As clsBoardData) As String()

        Dim lstRotate As New List(Of String)
        Dim insRotate As clsBoardData

        '正方形なら90度ごと、長方形なら180度ごとに文字列取得
        For intC As Integer = 0 To If(g_insConfig.blnAnsDuplication, Me.m_enmMode, 0) Step If(insBoardData.insBoard.intHeight = insBoardData.insBoard.intWidth, 1, 2)

            '回転配列取得
            insRotate = insBoardData.insRotate(intC)

            'PieceInfoを回転に対応するように番号を振りなおす
            Me.m_subChangeOrderOfPieceInfo(insRotate)

            '文字列を取得
            '1を二つ並べた場合と11等を区別するため、セパレータとしてアスタリスク挿入
            lstRotate.Add(insRotate.strArrayToString("*"))
        Next

        Return lstRotate.ToArray
    End Function

    ''' <summary>
    ''' 【ピース情報を整列】
    ''' </summary>
    ''' <param name="insBoardData"></param>
    Private Sub m_subChangeOrderOfPieceInfo(insBoardData As clsBoardData)

        'ピース情報取得（元を変えないようコピー）
        Dim lstPrevIndex As New List(Of List(Of Integer))
        For Each lstPrev As List(Of Integer) In Me.m_lstPrevIndex
            lstPrevIndex.Add(New List(Of Integer)(lstPrev))
        Next

        '置き換え用dictionary準備（無効値として-2で初期化）
        Dim lstDic As New List(Of Dictionary(Of Integer, Integer))
        For Each lstPrev As List(Of Integer) In Me.m_lstPrevIndex
            lstDic.Add(New Dictionary(Of Integer, Integer))
            For Each intPrev As Integer In lstPrev
                lstDic.Last.Add(intPrev, -2)
            Next
        Next

        '置き換え
        For intX As Integer = 0 To insBoardData.insBoard.intWidth - 1
            For intY As Integer = 0 To insBoardData.insBoard.intHeight - 1
                If insBoardData.insBoard.intValue(intX, intY) >= 2 Then
                    With lstDic(insBoardData.insBoard.intValue(intX, intY) - 2)
                        If .Item(insBoardData.insPieceInfo.intValue(intX, intY)) = -2 Then
                            '未設定の場合：リストから未使用の最小番号を取る
                            .Item(insBoardData.insPieceInfo.intValue(intX, intY)) = lstPrevIndex(insBoardData.insBoard.intValue(intX, intY) - 2)(0)
                            lstPrevIndex(insBoardData.insBoard.intValue(intX, intY) - 2).RemoveAt(0)
                        End If

                        '置き換え
                        insBoardData.insPieceInfo.intValue(intX, intY) = .Item(insBoardData.insPieceInfo.intValue(intX, intY))
                    End With
                End If
            Next
        Next

    End Sub

#End Region

#Region "●その他"

    ''' <summary>
    ''' 【プログレスバー初期化】
    ''' </summary>
    Private Sub m_subInitProgressBar()

        Me.prbProgress.Value = 0
        Me.prbProgress.Minimum = 0

        '各ピースを置ける向きの合計算出（重複ピースは飛ばす）
        Dim intSum As Integer = 0
        For Each insPointList As clsPointList In Me.m_lstPointList
            If insPointList.intPrevIndex < 0 Then
                intSum += insPointList.intDirectionNum
            End If
        Next

        '最大値を設定
        If Me.intGapOfPixelNum = 0 Then

            '空白なし：そのまま
            Me.prbProgress.Maximum = intSum
        Else
            '空白あり：空白部分の組合せをさらにかける
            Me.prbProgress.Maximum = Me.m_lstBoardData.Count * intSum
        End If

    End Sub

#End Region

End Class