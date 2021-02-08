Public Class entryItem
    Private colName As String
    Private value As Object
    Private fieldType As String


    Public Sub New(ByVal a As String, ByVal b As String, Optional ByVal c As String = " ") 'Instantiation for entryContainer
        colName = a
        value = b
        fieldType = c
    End Sub
    Public Sub New(ByVal a As String, ByVal c As String) 'To instantiate objects of TypeFieldArray
        colName = a
        fieldType = c
    End Sub
    Public Function getColName() As String
        Return colName
    End Function

    Public Function getValue() As String
        Return value
    End Function

    Public Function getFieldType() As String
        Return fieldType
    End Function

    Public Sub setFieldType(a As String)
        fieldType = a
    End Sub

    Public Sub parseValue()
        Select Case (fieldType)
            Case "System.String"
                value = ControlChars.Quote & Convert.ToString(value) & ControlChars.Quote
            Case "System.Int32"
                value = Convert.ToInt32(value)
            Case "System.Date"
                value = Convert.ToDateTime(value)
            Case "System.Int16"
                value = Convert.ToInt16(value)
        End Select
    End Sub
End Class