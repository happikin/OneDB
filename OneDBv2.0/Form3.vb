Public Class Form3
    Public entryAsColumns() As DataGridViewColumnCollection
    Public entryAsRows() As DataGridViewRowCollection
    Dim conOne As connCredentials
    Public query, tableName As String

    Public Structure Constraints
        Public PK, NN, AI As Boolean
    End Structure
    Dim keys As New Constraints

    Public Sub RecordConstraint(rowindex As Integer)
        keys.PK = DataGridView1.Rows(rowindex).Cells(2).Value
        keys.NN = DataGridView1.Rows(rowindex).Cells(3).Value
        keys.AI = DataGridView1.Rows(rowindex).Cells(4).Value
    End Sub
    Public Function GetConstraintString() As String
        'TO USE THIS FIRST CALL RecordConstraint() subprocedure
        Dim constraintstring As String = " "
        If keys.PK = True Then
            constraintstring &= "PRIMARY KEY"
        End If
        If keys.NN = True Then
            constraintstring &= "NOT NULL"
        End If
        If keys.AI = True Then
            constraintstring &= "AUTO INCREMENT"
        End If
        Return constraintstring
    End Function
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        conOne.server = "localhost"
        conOne.uid = "root"
        conOne.pswd = "Raina@1999"
        conOne.db = "demo"

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Me.Close()
        Me.Dispose()
        Form2.Show()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        'DataGridView1.Rows.Item(DataGridView1.CurrentRow.Index).DataGridView.BeginEdit(True)
    End Sub

    Private Sub DataGridView1_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DataGridView1.EditingControlShowing
        If TypeOf e.Control Is DataGridViewComboBoxEditingControl Then
            'CType(e.Control, ComboBox).Items.Add(e.Control.Text)
            CType(e.Control, ComboBox).DropDownStyle = ComboBoxStyle.DropDown
            CType(e.Control, ComboBox).AutoCompleteSource = AutoCompleteSource.ListItems
            CType(e.Control, ComboBox).AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        End If
    End Sub

    Private Sub DataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError
        If (e.ColumnIndex = 1) Then

            MessageBox.Show("Data error occurs:" + e.Exception.Message)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim rowCount As Integer = DataGridView1.Rows.Count - 1
        Dim createquery As String
        '//----FIRST WE MADE AN ARRAY OF ALL ROWS IN REQUIRED WAY----//
        If rowCount > 0 Then
            Dim DataGridRowContainer(rowCount) As TableRowContainer
            For index = 0 To rowCount - 1
                RecordConstraint(index)
                DataGridRowContainer(index) = New TableRowContainer _
                    (DataGridView1.Rows(index).Cells(0).Value, DataGridView1.Rows(index).Cells(1).Value, GetConstraintString())
            Next
            '//----NOW WE MUST BUILD THE QUERY----//
            createquery = "CREATE TABLE " & tableName & "("
            For index = 0 To rowCount - 1
                createquery &= DataGridRowContainer(index).getField & DataGridRowContainer(index).getColType
            Next

            MsgBox(DataGridView1.Rows.Count, 1, "")
        Else
            MsgBox(DataGridView1.Rows.Count, 1, "")
        End If

        'ReDim entryAsRows(DataGridView1.Rows.Count)
        'tableName = TextBox1.Text
        'Dim field = 0, typefield = 1, pk = 2, nn = 3, kn = 4, nj As Integer = 5
        'Dim dataType As String = DataGridView1.Rows(0).Cells(typefield).Value
        ''For index = 0 To DataGridView1.Rows.Count - 1
        ''    If entryAsRows(index) IsNot Nothing Then
        ''        entryAsRows(index).Add(DataGridView1.Rows(index))
        ''    Else
        ''        Exit For
        ''        'ReDim entryAsRows(0)    'to prevent any null reference exception
        ''    End If
        ''Next
        'query = "CREATE TABLE " & tableName & " ( "
        'For indexrows = 0 To DataGridView1.Rows.Count - 1
        '    For indexcolumns = 0 To DataGridView1.Columns.Count - 1
        '        query &= DataGridView1.Rows(indexrows).Cells(indexcolumns).Value
        '        'If DataGridView1.Rows(indexrows).Cells(indexcolumns).ValueType =  Then

        '        'End If
        '    Next
        'Next
        RecordConstraint(0)
        MsgBox(keys.PK, 1, "")

    End Sub
End Class