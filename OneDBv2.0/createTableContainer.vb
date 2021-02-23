Public Class TableRowContainer
    Dim field, type, constraint As String
    Public Sub New(a As String, b As String, c As String)
        field = a
        type = b
        constraint = c
    End Sub
    Public Sub GenConstraint()

    End Sub

    Public Function getField() As String
        Return field
    End Function

    Public Function getColType() As String
        Return type
    End Function

    Public Function getConstraint() As String
        Return constraint
    End Function
End Class
