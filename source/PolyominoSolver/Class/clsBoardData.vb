Option Compare Binary
Option Explicit On
Option Strict On

''' <summary>
''' 【解答探索用結果クラス】
''' </summary>
Public Class clsBoardData

    ''' <summary>
    ''' 【盤面】
    ''' </summary>
    Public ReadOnly insBoard As cls2DIntArray

    ''' <summary>
    ''' 【同一形状ピースがある場合の一つ前のインデックス】
    ''' </summary>
    Public ReadOnly insPieceInfo As cls2DIntArray

    ''' <summary>
    ''' 【New】
    ''' </summary>
    ''' <param name="insBoard"></param>
    Public Sub New(insBoard As cls2DIntArray)

        Me.insBoard = insBoard
        Me.insPieceInfo = New cls2DIntArray(insBoard.intWidth, insBoard.intHeight)

    End Sub

    ''' <summary>
    ''' 【New（インスタンスコピー用）】
    ''' </summary>
    ''' <param name="insBoard"></param>
    ''' <param name="insPieceInfo"></param>
    Private Sub New(insBoard As cls2DIntArray, insPieceInfo As cls2DIntArray)

        Me.insBoard = insBoard.insCopy
        Me.insPieceInfo = insPieceInfo.insCopy

    End Sub

    ''' <summary>
    ''' 【インスタンスコピー】
    ''' </summary>
    ''' <returns></returns>
    Public Function insCopy() As clsBoardData
        Return New clsBoardData(Me.insBoard, Me.insPieceInfo)
    End Function

    ''' <summary>
    ''' 【回転・反転】
    ''' </summary>
    ''' <param name="intRotate"></param>
    ''' <returns></returns>
    Public Function insRotate(intRotate As Integer) As clsBoardData
        Return New clsBoardData(Me.insBoard.insRotate(intRotate), Me.insPieceInfo.insRotate(intRotate))
    End Function

    ''' <summary>
    ''' 【文字列化】
    ''' </summary>
    ''' <param name="strSeparator"></param>
    ''' <returns></returns>
    Public Function strArrayToString(Optional strSeparator As String = "") As String
        Return Me.insBoard.strArrayToString(strSeparator) & Me.insPieceInfo.strArrayToString(strSeparator)
    End Function

End Class
