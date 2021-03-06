﻿Public Class entryItem
    Private colName As String
    Private value As Object
    Private fieldType As String
    Private condition As String


    Public Sub New(ByVal a As String, ByVal b As String, Optional ByVal c As String = " ") 'Instantiation for entryContainer
        colName = a
        value = b
        fieldType = c
    End Sub
    Public Sub New(ByVal a As String, ByVal c As String) 'To instantiate objects of TypeFieldArray
        colName = a
        fieldType = c   'is also used to store the condtion name
    End Sub
    Public Sub New(ByVal column As String)
        colName = column
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
    Public Function getConditionName() As String
        Return colName
    End Function
    Public Function getConditionType() As String
        Return fieldType
    End Function
    Public Sub setConditionValue(a As String)
        value = a
    End Sub
    Public Sub setConditionType(a As String)
        fieldType = a
    End Sub
    Public Function getConditionValue() As String
        Return value
    End Function
    Public Function parseValue() As Boolean
        Try
            Select Case (fieldType)
                Case "System.String"
                    value = ControlChars.Quote & value.ToString & ControlChars.Quote
                Case "System.Int32"
                    value = Convert.ToInt32(value)
                Case "System.Date"
                    value = Convert.ToDateTime(value)
                Case "System.Int16"
                    value = Convert.ToInt16(value)
            End Select
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Alert")
            Return False
            Exit Function
        End Try
    End Function
End Class