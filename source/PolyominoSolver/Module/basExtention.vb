Option Compare Binary
Option Explicit On
Option Strict On

''' <summary>
''' 【拡張】
''' </summary>
Module basExtention

    ''' <summary>
    ''' 【配列幅】
    ''' </summary>
    ''' <param name="intArray">Integer二次元配列</param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension, DebuggerStepThrough>
    Public Function intWidth(intArray(,) As Integer) As Integer
        Return intArray.GetLength(0)
    End Function

    ''' <summary>
    ''' 【配列高さ】
    ''' </summary>
    ''' <param name="intArray">Integer二次元配列</param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension, DebuggerStepThrough>
    Public Function intHeight(intArray(,) As Integer) As Integer
        Return intArray.GetLength(1)
    End Function

    ''' <summary>
    ''' 【二次元配列コピー】
    ''' </summary>
    ''' <param name="intArray">コピー元Integer二次元配列</param>
    ''' <returns></returns>
    <Runtime.CompilerServices.Extension, DebuggerStepThrough>
    Public Function intCopy(intArray(,) As Integer) As Integer(,)

        Dim intCopyArray(intArray.intWidth - 1, intArray.intHeight - 1) As Integer

        Array.Copy(intArray, intCopyArray, intArray.intHeight * intArray.intWidth)

        Return intCopyArray

    End Function

End Module
