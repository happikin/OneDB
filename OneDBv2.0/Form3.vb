Public Class Form3
    Public entryAsColumns() As DataGridViewColumnCollection
    Public entryAsRows() As DataGridViewRowCollection
    Dim conOne As connCredentials
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ReDim entryAsRows(DataGridView1.Rows.Count)

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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        For index = 0 To DataGridView1.Rows.Count - 1
            entryAsRows(index).Add(DataGridView1.Rows(index))
        Next
    End Sub
End Class