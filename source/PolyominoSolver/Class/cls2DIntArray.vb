Option Compare Binary
Option Explicit On
Option Strict On

''' <summary>
''' 【Integer一次元配列を二次元配列のように扱うためのクラス】
''' </summary>
Public Class cls2DIntArray

#Region "●変数"

    ''' <summary>
    ''' 【実データ配列】
    ''' </summary>
    Private ReadOnly m_intArray As Integer()

    ''' <summary>
    ''' 【二次元配列としての幅】
    ''' </summary>
    Public ReadOnly intWidth As Integer

    ''' <summary>
    ''' 【二次元配列としての高さ】
    ''' </summary>
    Public ReadOnly intHeight As Integer

#End Region

#Region "●プロパティ"

    ''' <summary>
    ''' 【一次元配列としての外部アクセス】
    ''' </summary>
    ''' <param name="intIndex">配列インデックス</param>
    ''' <returns></returns>
    Public Property intValue(intIndex As Integer) As Integer
        Get
            Return Me.m_intArray(intIndex)
        End Get
        Set(value As Integer)
            Me.m_intArray(intIndex) = value
        End Set
    End Property

    ''' <summary>
    ''' 【二次元配列としての外部アクセス】
    ''' </summary>
    ''' <param name="intX">X座標</param>
    ''' <param name="intY">Y座標</param>
    ''' <returns></returns>
    Public Property intValue(intX As Integer, intY As Integer) As Integer
        Get
            Return Me.m_intArray(Me.intWidth * intY + intX)
        End Get
        Set(value As Integer)
            Me.m_intArray(Me.intWidth * intY + intX) = value
        End Set
    End Property

    ''' <summary>
    ''' 【要素数】
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property intCount As Integer
        Get
            Return Me.m_intArray.Count
        End Get
    End Property

#End Region

#Region "●回転・反転"

    ''' <summary>
    ''' 【回転・反転】
    ''' </summary>
    ''' <param name="intRotate">回転量 0:無回転 1～3:90～270度回転 4:反転 5～7:反転+回転</param>
    ''' <returns></returns>
    Public Function insRotate(intRotate As Integer) As cls2DIntArray

        If intRotate < 0 Then
            Throw New InvalidOperationException
        End If

        Dim insArray As cls2DIntArray = Me

        '左右反転
        If intRotate \ 4 >= 1 Then
            insArray = Me.m_insReverse(insArray)
            intRotate = intRotate Mod 4
        End If

        Select Case intRotate
            Case 1 : insArray = Me.m_insRotate90(insArray)
            Case 2 : insArray = Me.m_insRotate180(insArray)
            Case 3 : insArray = Me.m_insRotate270(insArray)
        End Select

        Return insArray

    End Function

    ''' <summary>
    ''' 【左90度回転】
    ''' </summary>
    ''' <param name="insSourceArray">元インスタンス</param>
    ''' <returns></returns>
    Private Function m_insRotate90(insSourceArray As cls2DIntArray) As cls2DIntArray

        Dim insRotateArray As New cls2DIntArray(insSourceArray.intHeight, insSourceArray.intWidth)

        '90度左に回した位置に反映
        For intRow As Integer = 0 To insRotateArray.intHeight - 1
            For intColumn As Integer = 0 To insRotateArray.intWidth - 1
                insRotateArray.intValue(intColumn, intRow) = insSourceArray.intValue(insSourceArray.intWidth - 1 - intRow, intColumn)
            Next
        Next

        Return insRotateArray

    End Function

    ''' <summary>
    ''' 【左180度回転】
    ''' </summary>
    ''' <param name="insSourceArray">元インスタンス</param>
    ''' <returns></returns>
    Private Function m_insRotate180(insSourceArray As cls2DIntArray) As cls2DIntArray

        Dim insRotateArray As New cls2DIntArray(insSourceArray.intWidth, insSourceArray.intHeight)
        For intRow As Integer = 0 To insRotateArray.intHeight - 1
            For intColumn As Integer = 0 To insRotateArray.intWidth - 1
                insRotateArray.intValue(intColumn, intRow) = insSourceArray.intValue(insSourceArray.intWidth - intColumn - 1, insSourceArray.intHeight - intRow - 1)
            Next
        Next

        Return insRotateArray
    End Function

    ''' <summary>
    ''' 【左270度回転】
    ''' </summary>
    ''' <param name="insSourceArray">元インスタンス</param>
    ''' <returns></returns>
    Private Function m_insRotate270(insSourceArray As cls2DIntArray) As cls2DIntArray

        Dim insRotateArray As New cls2DIntArray(insSourceArray.intHeight, insSourceArray.intWidth)
        For intRow As Integer = 0 To insRotateArray.intHeight - 1
            For intColumn As Integer = 0 To insRotateArray.intWidth - 1
                insRotateArray.intValue(intColumn, intRow) = insSourceArray.intValue(intRow, insSourceArray.intHeight - 1 - intColumn)
            Next
        Next

        Return insRotateArray
    End Function

    ''' <summary>
    ''' 【左右反転】
    ''' </summary>
    ''' <param name="insSourceArray">元インスタンス</param>
    ''' <returns></returns>
    Private Function m_insReverse(insSourceArray As cls2DIntArray) As cls2DIntArray

        Dim insReverseArray As New cls2DIntArray(insSourceArray.intWidth, insSourceArray.intHeight)

        For intRow As Integer = 0 To insSourceArray.intHeight - 1
            For intColumn As Integer = 0 To insSourceArray.intWidth - 1
                insReverseArray.intValue(intColumn, intRow) = insSourceArray.intValue(insSourceArray.intWidth - 1 - intColumn, intRow)
            Next
        Next

        Return insReverseArray

    End Function

#End Region

#Region "●その他"

    ''' <summary>
    ''' 【New】
    ''' </summary>
    ''' <param name="intWidth">幅</param>
    ''' <param name="intHeight">高さ</param>
    Public Sub New(intWidth As Integer, intHeight As Integer)
        Me.intWidth = intWidth
        Me.intHeight = intHeight

        Dim intArray(intWidth * intHeight - 1) As Integer
        Me.m_intArray = intArray
    End Sub

    ''' <summary>
    ''' 【コピー】
    ''' </summary>
    ''' <returns></returns>
    Public Function insCopy() As cls2DIntArray

        Dim insCopyArray As New cls2DIntArray(Me.intWidth, Me.intHeight)

        For intIndex As Integer = 0 To Me.m_intArray.Count - 1
            insCopyArray.intValue(intIndex) = Me.m_intArray(intIndex)
        Next

        Return insCopyArray

    End Function

    ''' <summary>
    ''' 【配列の文字列化】
    ''' </summary>
    ''' <returns></returns>
    ''' <param name="strSeparator">区切り文字</param>
    Public Function strArrayToString(Optional strSeparator As String = "") As String

        Dim insBuilder As New Text.StringBuilder

        For Each intValue As Integer In Me.m_intArray
            insBuilder.Append(intValue.ToString & strSeparator)
        Next

        Return insBuilder.ToString

    End Function

    ''' <summary>
    ''' 【最小サイズ配列に変換する】
    ''' </summary>
    ''' <returns></returns>
    Public Function insGetMinimumArray() As cls2DIntArray

        '最大値・最小値を仮設定
        Dim intRow_Minimum As Integer = Integer.MaxValue
        Dim intRow_Maximum As Integer = -1
        Dim intColumn_Minimum As Integer = Integer.MaxValue
        Dim intColumn_Maximum As Integer = -1

        For intRow As Integer = 0 To Me.intHeight - 1
            For intColumn As Integer = 0 To Me.intWidth - 1

                If Math.Abs(Me.intValue(intColumn, intRow)) > 0 Then

                    '最大値・最小値を更新
                    intRow_Minimum = Math.Min(intRow_Minimum, intRow)
                    intRow_Maximum = Math.Max(intRow_Maximum, intRow)
                    intColumn_Minimum = Math.Min(intColumn_Minimum, intColumn)
                    intColumn_Maximum = Math.Max(intColumn_Maximum, intColumn)
                End If
            Next
        Next

        '最小サイズで配列を作り直す
        Dim insReturn As New cls2DIntArray(intColumn_Maximum - intColumn_Minimum + 1, intRow_Maximum - intRow_Minimum + 1)

        For intRow As Integer = 0 To insReturn.intHeight - 1
            For intColumn As Integer = 0 To insReturn.intWidth - 1
                insReturn.intValue(intColumn, intRow) = Me.intValue(intColumn + intColumn_Minimum, intRow + intRow_Minimum)
            Next
        Next

        Return insReturn

    End Function

#End Region

End Class
