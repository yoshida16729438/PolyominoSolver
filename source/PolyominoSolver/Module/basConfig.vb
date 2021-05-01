Option Compare Binary
Option Strict On
Option Explicit On

''' <summary>
''' 【コンフィグ読み込み】
''' </summary>
Public Module basConfig

#Region "●定数"

    ''' <summary>
    ''' 【コンフィグファイルパス】
    ''' </summary>
    Private Const m_strConfigFilePath As String = "Config.rc"

#Region "●初期値"

    ''' <summary>
    ''' 【盤面最小サイズ初期値】
    ''' </summary>
    Private Const m_intInitialBoardMinimumSize As Integer = 4

    ''' <summary>
    ''' 【盤面最大サイズ初期値】
    ''' </summary>
    Private Const m_intInitialBoardMaximumSize As Integer = 10

    ''' <summary>
    ''' 【ピース最大サイズ初期値】
    ''' </summary>
    Private Const m_intInitialPieceSize As Integer = 5

    ''' <summary>
    ''' 【ピクセルサイズ初期値（盤面・ピース共通）】
    ''' </summary>
    Private Const m_intInitialPixelSize As Integer = 50

    ''' <summary>
    ''' 【回転・反転での重複解答カウント初期値】
    ''' </summary>
    Public Const g_strInitialAnsDuplication As String = "0"

#End Region

#Region "●項目名"

    ''' <summary>
    ''' 【盤面最小サイズ項目名】
    ''' </summary>
    Private Const m_strBoardMinName = "BoardMinimumSize"

    ''' <summary>
    ''' 【盤面最大サイズ項目名】
    ''' </summary>

    Private Const m_strBoardMaxName = "BoardMaximumSize"

    ''' <summary>
    ''' 【盤面ピクセルサイズ項目名】
    ''' </summary>
    Private Const m_strBoardPixName = "BoardPixelSize"

    ''' <summary>
    ''' 【ピース最大サイズ項目名】
    ''' </summary>
    Private Const m_strPieceMaxName = "PieceMaximumSize"

    ''' <summary>
    ''' 【ピースピクセルサイズ項目名】
    ''' </summary>
    Private Const m_strPiecePixName = "PiecePixelSize"

    ''' <summary>
    ''' 【カラーコード項目名】
    ''' </summary>
    Private Const m_strColCodeName = "PieceColorCode"

    ''' <summary>
    ''' 【回転反転解答重複扱い項目名】
    ''' </summary>
    Private Const m_strAnsDupliName = "AnswerDuplication"

#End Region

#End Region

#Region "●変数"

    ''' <summary>
    ''' 【コンフィグデータ】
    ''' </summary>
    Public g_insConfig As New clsConfigData

    ''' <summary>
    ''' 【カラーコード読み込みエラー】
    ''' </summary>
    Private m_intColorErrorCounter As Integer = 0

    '初期値

    ''' <summary>
    ''' 【デフォルトカラーコード】
    ''' </summary>
    Public g_strDefaultColorCode() As String = {
        "#FE0000",
        "#0000FF",
        "#00FFFF",
        "#FFFF00",
        "#3CB371",
        "#00FF00",
        "#B0BEC5",
        "#DA70D6",
        "#004D40",
        "#800080"
        }

#End Region

#Region "●コンフィグデータクラス"

    ''' <summary>
    ''' 【コンフィグデータクラス】
    ''' </summary>
    Public Class clsConfigData

        ''' <summary>
        ''' 【盤面最小サイズ】
        ''' </summary>
        Public intBoardMinimumSize As Integer = m_intInitialBoardMinimumSize

        ''' <summary>
        ''' 【盤面最大サイズ】
        ''' </summary>
        Public intBoardMaximumSize As Integer = m_intInitialBoardMaximumSize

        ''' <summary>
        ''' 【盤面設定のピクセルサイズ】
        ''' </summary>
        Public intBoardPixelSize As Integer = m_intInitialPixelSize

        ''' <summary>
        ''' 【ピースの幅・高さサイズ】
        ''' </summary>
        Public intPieceSize As Integer = m_intInitialPieceSize

        ''' <summary>
        ''' 【ピースのピクセルサイズ】
        ''' </summary>
        Public intPiecePixelSize As Integer = m_intInitialPixelSize

        ''' <summary>
        ''' 【ピースの色】
        ''' </summary>
        Public lstPieceColor As List(Of Color)

        ''' <summary>
        ''' 【回転・反転して重なる解答を重複解とするか】
        ''' </summary>
        Public strAnsDuplication As String

        ''' <summary>
        ''' 【回転・反転して重なる解答を重複解とするか】
        ''' </summary>
        ''' <returns>
        ''' True ：重複解とする
        ''' False：異なる解とする
        ''' </returns>
        Public ReadOnly Property blnAnsDuplication As Boolean
            Get
                Return Me.strAnsDuplication = g_strInitialAnsDuplication
            End Get
        End Property

        Public Sub New()
            Me.intBoardMinimumSize = 0
            Me.intBoardMaximumSize = 0
            Me.intBoardPixelSize = 0
            Me.intPieceSize = 0
            Me.intPiecePixelSize = 0
            Me.strAnsDuplication = "0"
            Me.lstPieceColor = New List(Of Color)
        End Sub

    End Class

#End Region

#Region "●読み込み"

    ''' <summary>
    ''' 【コンフィグファイル読込】
    ''' </summary>
    Public Sub g_subLoadConfig()

        m_intColorErrorCounter = 0

        'ファイルが存在しない場合は新規生成
        If Not IO.File.Exists(m_strConfigFilePath) Then m_subOutputConfig_DefaultValue()

        '各数値読込
        Using insReader As New IO.StreamReader(m_strConfigFilePath, Text.Encoding.UTF8)
            While insReader.Peek > -1
                Dim strLine() As String = insReader.ReadLine.Split("="c)

                If strLine.Length = 1 Then Continue While

                Dim strParam As String = strLine(1).Trim

                Select Case strLine(0)
                    Case m_strBoardMaxName : g_insConfig.intBoardMaximumSize = m_intSetSize(strParam)   '盤面最大サイズ
                    Case m_strBoardMinName : g_insConfig.intBoardMinimumSize = m_intSetSize(strParam)   '盤面最小サイズ
                    Case m_strBoardPixName : g_insConfig.intBoardPixelSize = m_intSetSize(strParam)     '盤面ピクセルサイズ
                    Case m_strPieceMaxName : g_insConfig.intPieceSize = m_intSetSize(strParam)          'ピース最大サイズ
                    Case m_strPiecePixName : g_insConfig.intPiecePixelSize = m_intSetSize(strParam)     'ピースピクセルサイズ
                    Case m_strAnsDupliName : g_insConfig.strAnsDuplication = strParam                   '重複解答
                    Case m_strColCodeName : m_subAddColor(strParam)                                     'カラーコード
                End Select
            End While
        End Using

        '未設定および制限の確認
        m_subCheckValues()

    End Sub

    ''' <summary>
    ''' 各数値系項目を受けて、数値が正の整数なら取り込む
    ''' </summary>
    ''' <param name="strData">確認対象文字列</param>
    ''' <returns></returns>
    Private Function m_intSetSize(strData As String) As Integer

        Dim intValue As Integer

        '正の整数値が記載されていれば、それを返す
        If Integer.TryParse(strData, intValue) AndAlso intValue > 0 Then
            Return intValue
        Else
            '0でいったん設定
            Return 0
        End If
    End Function

    ''' <summary>
    ''' カラーコードを受け取ってその値が不正でないか確認、不正でなければ追加
    ''' </summary>
    ''' <param name="strData">カラーコード値（#付き）</param>
    Private Sub m_subAddColor(strData As String)

        Dim strColorCode As String = strData.Replace("#"c, "&H")

        '非負整数に変換可能、かつ0～16,777,215の間なら許可
        Dim intColorCode As Integer
        Try
            intColorCode = CInt(strColorCode)
        Catch ex As Exception
            m_intColorErrorCounter += 1
            Exit Sub
        End Try

        If intColorCode >= 0 AndAlso intColorCode <= &HFFFFFF Then
            g_insConfig.lstPieceColor.Add(ColorTranslator.FromWin32(intColorCode))
        Else
            m_intColorErrorCounter += 1
        End If

    End Sub

    ''' <summary>
    ''' 各設定値が未設定でないか確認→制限確認
    ''' </summary>
    Private Sub m_subCheckValues()

        'エラーメッセージまとめ用
        Dim strErrorMessage As String = String.Empty

        With g_insConfig

            'まず未設定でないか確認
            If .intBoardMinimumSize = 0 Then
                strErrorMessage &= "・盤面の幅/高さの最小値" & vbCrLf
                .intBoardMinimumSize = m_intInitialBoardMinimumSize
            End If

            If .intBoardMaximumSize = 0 Then
                strErrorMessage &= "・盤面の幅/高さの最大値" & vbCrLf
                .intBoardMaximumSize = m_intInitialBoardMaximumSize
            End If

            '盤面サイズの最小値<最大値か確認
            If .intBoardMinimumSize > .intBoardMaximumSize Then
                strErrorMessage &= "・盤面の幅/高さの最小値>最大値" & vbCrLf
                .intBoardMinimumSize = m_intInitialBoardMinimumSize
                .intBoardMaximumSize = m_intInitialBoardMaximumSize
            End If

            If .intBoardPixelSize = 0 Then
                strErrorMessage &= "・盤面のピクセルサイズ" & vbCrLf
                .intBoardPixelSize = m_intInitialPixelSize
            End If

            If .intPieceSize = 0 Then
                strErrorMessage &= "・ピースの幅/高さの最大値" & vbCrLf
                .intPieceSize = m_intInitialPieceSize
            End If

            If .intPiecePixelSize = 0 Then
                strErrorMessage &= "・ピースのピクセルサイズ" & vbCrLf
                .intPiecePixelSize = m_intInitialPixelSize
            End If

            If .strAnsDuplication <> "0" AndAlso .strAnsDuplication <> "1" Then
                strErrorMessage &= "・盤面全体の回転・反転で重複する解を別解とするか" & vbCrLf
                .strAnsDuplication = g_strInitialAnsDuplication
            End If

            '色については、一色もなかった場合のみ、初期値を投入する
            If .lstPieceColor.Count = 0 Then
                strErrorMessage &= "・ピースの色"
                For Each strCode In g_strDefaultColorCode
                    m_subAddColor(strCode)
                Next

            ElseIf m_intColorErrorCounter <> 0 Then

                If strErrorMessage <> String.Empty Then strErrorMessage &= vbCrLf & "また、"

                strErrorMessage &= "不正なカラーコード" & m_intColorErrorCounter.ToString & "件をスキップしました。"

            End If

            If strErrorMessage <> String.Empty Then
                strErrorMessage = "以下の項目について、設定がない、または不正な値のため、初期値で設定しました。" & vbCrLf & strErrorMessage
                MsgBox(strErrorMessage, vbExclamation)
            End If

        End With
    End Sub

#End Region

#Region "●コンフィグファイルがない場合に初期値で作成"

    ''' <summary>
    ''' 【初期値コンフィグファイル作成】
    ''' </summary>
    Private Sub m_subOutputConfig_DefaultValue()

        Dim lstInitValue As New List(Of String)
        lstInitValue.Clear()
        With lstInitValue
            .Add("#コンフィグを全て初期値に戻したい場合、")
            .Add("#このファイルを削除して起動することで再生成されます。")
            .Add("#設定方法は説明書をご確認ください。")
            .Add("")
            .Add("#盤面の幅・高さの最小値/最大値")
            .Add(m_strBoardMinName & "=" & m_intInitialBoardMinimumSize.ToString)
            .Add(m_strBoardMaxName & "=" & m_intInitialBoardMaximumSize.ToString)
            .Add("")
            .Add("#盤面のピクセルサイズ")
            .Add(m_strBoardPixName & "=" & m_intInitialPixelSize.ToString)
            .Add("")
            .Add("#ピースの最大幅・高さ")
            .Add(m_strPieceMaxName & "=" & m_intInitialPieceSize.ToString)
            .Add("")
            .Add("#ピースのピクセルサイズ")
            .Add(m_strPiecePixName & "=" & m_intInitialPixelSize.ToString)
            .Add("")
            .Add("#解答表示時のピースの色")

            For Each strCode In g_strDefaultColorCode
                .Add(m_strColCodeName & "=" & strCode)
            Next

            .Add("")
            .Add("#盤面全体の回転・反転で重複する解を別解とするか否か")
            .Add(m_strAnsDupliName & "=" & g_strInitialAnsDuplication)

        End With

        Dim insWriter = New IO.StreamWriter(m_strConfigFilePath, False, Text.Encoding.UTF8)

        For Each strValue In lstInitValue
            insWriter.WriteLine(strValue)
        Next
        insWriter.Close()
    End Sub

#End Region

End Module
