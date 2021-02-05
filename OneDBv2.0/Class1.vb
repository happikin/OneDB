Public Class entryItem
    Dim colName As String
    Dim value As String
    Public Sub New(ByVal a As String, ByVal b As String)
        colName = a
        value = b
    End Sub
    Public Function getColName() As String
        Return colName
    End Function

    Public Function getValue() As String
        Return value
    End Function
End Class