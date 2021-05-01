Option Compare Binary
Option Explicit On
Option Strict On

''' <summary>
''' 【位置のみを記録するクラス】
''' </summary>
Public Class clsPointList

#Region "●定数"

    ''' <summary>
    ''' 【前インデックス初期値】
    ''' </summary>
    Public Const G_INT_DEF_PREV_INDEX As Integer = -1

#End Region

#Region "●変数"

    ''' <summary>
    ''' 【回転・反転形状リスト】
    ''' </summary>
    Private ReadOnly m_lstRotate As List(Of clsPointData)

    ''' <summary>
    ''' 【使用済みかどうか】
    ''' </summary>
    Public blnUse As Boolean

    ''' <summary>
    ''' 【関連付けられた値】
    ''' </summary>
    Public intAssociatedValue As Integer

    ''' <summary>
    ''' 【元データ配列】
    ''' </summary>
    Private ReadOnly m_insMinimumArray As cls2DIntArray

    ''' <summary>
    ''' 【回転】
    ''' </summary>
    Private ReadOnly m_intRotate_Max As Integer

    ''' <summary>
    ''' 【重複排除用ピース文字化リスト】
    ''' </summary>
    Public ReadOnly lstPieceString As List(Of String)

    ''' <summary>
    ''' 【同一形状ピースがある場合のインデックス】
    ''' </summary>
    Public intPrevIndex As Integer

#End Region

#Region "●プロパティ"

    ''' <summary>
    ''' 【回転したピースを取得】
    ''' </summary>
    ''' <param name="intIndex"></param>
    ''' <returns></returns>
    Public ReadOnly Property insRotate(intIndex As Integer) As clsPointData
        Get
            Return Me.m_lstRotate(intIndex)
        End Get
    End Property

    ''' <summary>
    ''' 【回転・反転の向きの総数】
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property intDirectionNum As Integer
        Get
            Return Me.m_lstRotate.Count
        End Get
    End Property

#End Region

#Region "●Integer配列→Pointリスト変換"

    ''' <summary>
    ''' 【初期化】
    ''' </summary>
    ''' <param name="insSourceArray">元データ配列</param>
    ''' <param name="intRotate_Max">回転・反転</param>
    Public Sub New(insSourceArray As cls2DIntArray, intRotate_Max As Integer)

        Me.blnUse = False
        Me.intPrevIndex = G_INT_DEF_PREV_INDEX
        Me.intAssociatedValue = 1
        Me.m_intRotate_Max = intRotate_Max
        Me.m_insMinimumArray = insSourceArray.insGetMinimumArray

        'リスト作成
        Me.m_lstRotate = New List(Of clsPointData)
        Me.lstPieceString = New List(Of String)
        Me.m_subMakeRotateList()

    End Sub

    ''' <summary>
    ''' 【回転・反転形状リストを取得】
    ''' </summary>
    Private Sub m_subMakeRotateList()

        Dim insArray As cls2DIntArray
        Dim strArray As String
        For intRotate As Integer = 0 To Me.m_intRotate_Max

            insArray = Me.m_insMinimumArray.insRotate(intRotate)
            strArray = insArray.strArrayToString & insArray.intWidth.ToString & insArray.intWidth.ToString

            With insArray

                '重複ない場合のみ追加
                If Not Me.lstPieceString.Contains(strArray) Then

                    Dim insPointData As New clsPointData
                    insPointData.intHeight = .intHeight
                    insPointData.intWidth = .intWidth

                    '左上を基準とし、0でない値のある位置の相対座標を記録
                    '一番左の列から縦に探索
                    For intX As Integer = 0 To .intWidth - 1
                        For intY As Integer = 0 To .intHeight - 1
                            If .intValue(intX, intY) <> 0 Then
                                insPointData.lstPoint.Add(New Point(intX, intY))
                            End If
                        Next
                    Next

                    Me.m_lstRotate.Add(insPointData)

                    '重複排除
                    Me.lstPieceString.Add(strArray)
                End If
            End With
        Next

    End Sub

#End Region

#Region "●Pointデータクラス"

    ''' <summary>
    ''' 【Pointデータ】
    ''' </summary>
    Public Class clsPointData

        ''' <summary>
        ''' 【Pointリスト】
        ''' </summary>
        Public lstPoint As List(Of Point)

        ''' <summary>
        ''' 【幅】
        ''' </summary>
        Public intWidth As Integer

        ''' <summary>
        ''' 【高さ】
        ''' </summary>
        Public intHeight As Integer

        ''' <summary>
        ''' 【初期化】
        ''' </summary>
        Public Sub New()
            Me.lstPoint = New List(Of Point)
        End Sub

    End Class

#End Region

    ''' <summary>
    ''' 【コピー】
    ''' </summary>
    ''' <returns></returns>
    Public Function insCopy() As clsPointList

        Dim insCop As New clsPointList(Me.m_insMinimumArray, Me.m_intRotate_Max)
        insCop.blnUse = Me.blnUse
        insCop.intAssociatedValue = Me.intAssociatedValue
        insCop.intPrevIndex = Me.intPrevIndex
        Return insCop

    End Function

End Class
