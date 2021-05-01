Option Compare Binary
Option Explicit On
Option Strict On

Imports System.Xml.Serialization

''' <summary>
''' 【データファイル作成クラス】
''' </summary>
Public Class clsDataFileMaker

#Region "●変数"

    ''' <summary>
    ''' XML出力用インスタンス
    ''' </summary>
    Private m_insDataFile As clsDataFile

    ''' <summary>
    ''' 【盤面】
    ''' </summary>
    Private m_insBoard As cls2DIntArray

    ''' <summary>
    ''' 【ピースリスト】
    ''' </summary>
    Private m_lstPiece As List(Of cls2DIntArray)

    ''' <summary>
    ''' 【盤面読込成否】
    ''' </summary>
    Private m_blnLoadBoard As Boolean

    ''' <summary>
    ''' 【ピース読込成否】
    ''' </summary>
    Private m_blnLoadPiece As Boolean

#End Region

#Region "●プロパティ"

    ''' <summary>
    ''' 【盤面読込成否】
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property blnLoadBoard As Boolean
        Get
            Return Me.m_blnLoadBoard
        End Get
    End Property

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
    ''' 【ピース読込成否】
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property blnLoadPiece As Boolean
        Get
            Return Me.m_blnLoadPiece
        End Get
    End Property

    ''' <summary>
    ''' 【ピースリスト】
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property lstPiece As List(Of cls2DIntArray)
        Get
            Return Me.m_lstPiece
        End Get
    End Property

    ''' <summary>
    ''' 【エラー有無】
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property blnError As Boolean
        Get
            Return Me.m_insDataFile.strType = "1"
        End Get
    End Property

#End Region

#Region "●書き出し"

    ''' <summary>
    ''' 【書き出しデータ作成】
    ''' </summary>
    ''' <param name="insBoard">盤面</param>
    ''' <param name="lstPiece">ピースリスト</param>
    ''' <param name="intPieceSize">ピースサイズ</param>
    ''' <param name="blnError">エラー有無</param>
    Public Sub subSetData(insBoard As cls2DIntArray,
                          lstPiece As List(Of cls2DIntArray),
                          intPieceSize As Integer,
                          Optional blnError As Boolean = False)

        Me.m_insDataFile = New clsDataFile
        Me.m_insDataFile.intPieceSize = intPieceSize
        Me.m_insDataFile.strType = If(blnError, "1", "0")

        With My.Application.Info.Version
            Me.m_insDataFile.strVersion = String.Format("{0}.{1}", .Major, .Minor)
        End With

        Me.m_subSetBoardString(insBoard)
        Me.m_subSetPieceStringList(lstPiece)

    End Sub

    ''' <summary>
    ''' 【書出】
    ''' </summary>
    ''' <param name="strPath">ファイルパス</param>
    Public Function blnSave(Optional strPath As String = "ExceptionData.xml") As Boolean

        'XML出力
        Dim insSerializer As New XmlSerializer(GetType(clsDataFile))

        Try
            Using insWriter As New IO.StreamWriter(strPath, False, New Text.UTF8Encoding)
                insSerializer.Serialize(insWriter, Me.m_insDataFile)
                insWriter.Flush()
            End Using

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 【盤面を文字化する】
    ''' </summary>
    ''' <param name="insBoard">盤面</param>
    Private Sub m_subSetBoardString(insBoard As cls2DIntArray)

        '盤面未設定の場合
        If insBoard Is Nothing Then
            Me.m_insDataFile.intBoardWidth = 0
            Me.m_insDataFile.intBoardHeight = 0
            Me.m_insDataFile.strBoard = String.Empty
            Exit Sub
        End If

        '幅・高さ設定
        Me.m_insDataFile.intBoardWidth = insBoard.intWidth
        Me.m_insDataFile.intBoardHeight = insBoard.intHeight

        '盤面を文字化
        Me.m_insDataFile.strBoard = Me.m_strBinToHex(insBoard.strArrayToString)

    End Sub

    ''' <summary>
    ''' 【ピースの文字列化】
    ''' </summary>
    ''' <param name="lstPiece">ピースリスト</param>
    Private Sub m_subSetPieceStringList(lstPiece As List(Of cls2DIntArray))

        'ピースをすべて文字化してリストへ追加
        Me.m_insDataFile.lstPiece_String = New List(Of String)
        For Each insPiece As cls2DIntArray In lstPiece
            Me.m_insDataFile.lstPiece_String.Add(Me.m_strBinToHex(insPiece.strArrayToString))
        Next

    End Sub

#End Region

#Region "●読み込み"

    ''' <summary>
    ''' 【ファイルから読み込む】
    ''' </summary>
    ''' <param name="strPath">ファイルパス</param>
    Public Function blnLoad(strPath As String) As Boolean

        'デシリアライズ
        Me.m_insDataFile = Me.m_insDeserialize(strPath)
        If Me.m_insDataFile Is Nothing Then Return False

        '読み込み成功の場合、データを成形
        Me.m_blnLoadBoard = Me.m_blnGetBoard
        Me.m_blnLoadPiece = Me.m_blnGetPiece

        Return True

    End Function

    ''' <summary>
    ''' 【デシリアライズ】
    ''' </summary>
    ''' <param name="strPath">ファイルパス</param>
    ''' <returns></returns>
    Private Function m_insDeserialize(strPath As String) As clsDataFile

        Dim insSerializer As New XmlSerializer(GetType(clsDataFile))
        Dim insResult As clsDataFile
        Dim insSetting As New Xml.XmlReaderSettings With {.CheckCharacters = False}

        Using insReader As New IO.StreamReader(strPath, New Text.UTF8Encoding)
            Using insXMLReader = Xml.XmlReader.Create(insReader, insSetting)
                Try
                    insResult = CType(insSerializer.Deserialize(insXMLReader), clsDataFile)
                Catch ex As Exception
                    insResult = Nothing
                    MsgBox("ファイルの読込に失敗しました。", MsgBoxStyle.Exclamation)
                End Try
            End Using
        End Using

        Return insResult

    End Function

    ''' <summary>
    ''' 【盤面文字列→配列】
    ''' </summary>
    Private Function m_blnGetBoard() As Boolean

        With Me.m_insDataFile

            '盤面データない場合：読込成功扱い
            If String.IsNullOrEmpty(.strBoard) Then
                Me.m_insBoard = Nothing
                Return True
            End If

            If .intBoardWidth = Nothing OrElse
                .intBoardWidth <= 0 Then
                MsgBox("盤面の読込に失敗しました。（幅不明）", MsgBoxStyle.Exclamation)
                Me.m_insBoard = Nothing
                Return False
            End If

            If .intBoardHeight = Nothing OrElse
                .intBoardHeight <= 0 Then
                MsgBox("盤面の読込に失敗しました。（高さ不明）", MsgBoxStyle.Exclamation)
                Me.m_insBoard = Nothing
                Return False
            End If

            '配列化
            Me.m_insBoard = Me.m_insStringTo2DArray(.strBoard, .intBoardWidth, .intBoardHeight)

            If Me.m_insBoard Is Nothing Then
                MsgBox("盤面の読込に失敗しました。（異常データ）", MsgBoxStyle.Exclamation)
                Return False
            Else
                Return True
            End If

        End With

    End Function

    ''' <summary>
    ''' 【ピース文字列→配列】
    ''' </summary>
    Private Function m_blnGetPiece() As Boolean

        Me.m_lstPiece = New List(Of cls2DIntArray)

        With Me.m_insDataFile

            'ピースない場合：読込成功扱い
            If .lstPiece_String Is Nothing OrElse
                .lstPiece_String.Count = 0 Then
                Return True
            End If

            'サイズ不明の場合
            If .intPieceSize = Nothing OrElse
                .intPieceSize <= 0 Then
                MsgBox("ピースの読込に失敗しました。（サイズ不明）", MsgBoxStyle.Exclamation)
                Return False
            End If

            '配列化
            Dim intErrorPieceCount As Integer = 0
            For Each strPiece In .lstPiece_String

                Dim insPiece As cls2DIntArray = Me.m_insStringTo2DArray(strPiece, .intPieceSize, .intPieceSize)
                If insPiece Is Nothing Then
                    intErrorPieceCount += 1
                Else
                    Me.m_lstPiece.Add(insPiece)
                End If
            Next

            If intErrorPieceCount > 0 Then
                MsgBox(intErrorPieceCount.ToString & "個のピースの読込に失敗しました。", MsgBoxStyle.Exclamation)
            End If

            Return True

        End With

    End Function

    ''' <summary>
    ''' 【文字列→配列】
    ''' </summary>
    ''' <param name="strArray">配列化したい文字列</param>
    ''' <param name="intArrayWidth">配列幅</param>
    ''' <param name="intArrayHeight">配列高さ</param>
    ''' <returns></returns>
    Private Function m_insStringTo2DArray(strArray As String, intArrayWidth As Integer, intArrayHeight As Integer) As cls2DIntArray

        '文字列長が幅×高さ/4より短かったらNothingで戻る
        If strArray.Length < CInt(Math.Ceiling(intArrayHeight * intArrayWidth / 4)) Then Return Nothing

        '文字列が16進文字列でなければNothingで戻る
        If Not System.Text.RegularExpressions.Regex.IsMatch(strArray, "^[0-9A-Fa-f]{" & strArray.Length & "}$") Then
            Return Nothing
        End If

        Dim strBin As String = Me.m_strHexToBin(strArray, intArrayWidth * intArrayHeight)
        If strBin = String.Empty Then Return Nothing

        Dim insArray As New cls2DIntArray(intArrayWidth, intArrayHeight)
        For intIndex As Integer = 0 To intArrayWidth * intArrayHeight - 1
            insArray.intValue(intIndex) = CInt(strBin.Substring(intIndex, 1))
        Next

        Return insArray

    End Function

#End Region

#Region "●2進数⇔16進数"

    ''' <summary>
    ''' 【2進数→16進数】
    ''' </summary>
    ''' <param name="strBin">2進文字列</param>
    ''' <returns></returns>
    Private Function m_strBinToHex(strBin As String) As String

        '前処理：文字列を4の倍数文字の長さにする
        Dim intLen As Integer = CInt(Math.Ceiling(strBin.Length / 4)) * 4
        strBin = strBin.PadLeft(intLen, "0"c)

        Dim strRet As String = String.Empty

        For intC As Integer = 0 To (strBin.Length \ 4) - 1

            '2進文字列→10進数→16進文字列
            strRet &= Convert.ToInt32(strBin.Substring(intC * 4, 4), 2).ToString("X")
        Next

        Return strRet

    End Function

    ''' <summary>
    ''' 【16進数→2進数】
    ''' </summary>
    ''' <param name="strHex">16進文字列</param>
    ''' <param name="intLen">変換後の長さ</param>
    ''' <returns></returns>
    Private Function m_strHexToBin(strHex As String, intLen As Integer) As String

        Dim strRet As String = String.Empty
        For Each chrHex As Char In strHex

            '16進文字列→10進数→2進文字列
            Try
                strRet &= Convert.ToString(CInt("&H" & chrHex), 2).PadLeft(4, "0"c)
            Catch ex As Exception
                Return String.Empty
            End Try
        Next

        Return Right(strRet, intLen)

    End Function

#End Region

#Region "●ファイル用クラス"

    ''' <summary>
    ''' 【ファイル出力用クラス】
    ''' </summary>
    <XmlRoot(ElementName:="Polyomino_Solver_Data_File")>
    Public Class clsDataFile

        ''' <summary>
        ''' 【ソフトウェアバージョン】
        ''' </summary>
        <XmlElement(ElementName:="Version", Type:=GetType(String), IsNullable:=False)>
        Public strVersion As String

        ''' <summary>
        ''' 【盤面】
        ''' </summary>
        <XmlElement(ElementName:="Board", Type:=GetType(String), IsNullable:=False)>
        Public strBoard As String

        ''' <summary>
        ''' 【盤面幅】
        ''' </summary>
        <XmlElement(ElementName:="BoardWidth", Type:=GetType(Integer), IsNullable:=False)>
        Public intBoardWidth As Integer

        ''' <summary>
        ''' 【盤面高さ】
        ''' </summary>
        <XmlElement(ElementName:="BoardHeight", Type:=GetType(Integer), IsNullable:=False)>
        Public intBoardHeight As Integer

        ''' <summary>
        ''' 【ピース】
        ''' </summary>
        <XmlArray(ElementName:="PieceList", IsNullable:=False),
            XmlArrayItem(ElementName:="Piece", Type:=GetType(String), IsNullable:=False)>
        Public lstPiece_String As List(Of String)

        ''' <summary>
        ''' 【ピースサイズ】
        ''' </summary>
        <XmlElement(ElementName:="PieceSize", Type:=GetType(Integer), IsNullable:=False)>
        Public intPieceSize As Integer

        ''' <summary>
        ''' 【ファイルのタイプ　0：通常保存用 1：異常時用】
        ''' </summary>
        <XmlElement(ElementName:="Type", Type:=GetType(String), IsNullable:=False)>
        Public strType As String

    End Class

#End Region

End Class
