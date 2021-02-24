Imports MySql.Data.MySqlClient
Public Class Form1
    Public conOne As New connCredentials
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Button1.Enabled = False
        Button2.Enabled = False
        End
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.TextLength <> 0 And TextBox2.TextLength <> 0 Then
            Dim sqlCon As New MySqlConnection
            Dim sqlCmd As New MySqlCommand
            Dim sqlDR As MySqlDataReader
            Dim sqlDT As New DataTable
            Dim uname, pswd As String
            sqlCon.ConnectionString = "server=" + conOne.server + ";" + "user id=" + conOne.uid + ";" + "password=" + conOne.pswd + ";" + "database=" + conOne.db + ";"
            Try
                sqlCon.Open()
                sqlCmd.Connection = sqlCon
                sqlCmd.CommandText = "SELECT * FROM onedb WHERE USERNAME LIKE " &
                   ControlChars.Quote & TextBox1.Text & ControlChars.Quote & ";"
                sqlDR = sqlCmd.ExecuteReader()
                sqlDT.Load(sqlDR)
                sqlDR.Close()
                sqlCon.Close()
            Catch ex As Exception
                MsgBox(ex.Message, 64, "MESSAGE")
                Exit Sub
            End Try
            If sqlDT.Rows.Count > 0 Then
                uname = sqlDT.Rows(0).Field(Of String)("username")
                pswd = sqlDT.Rows(0).Field(Of String)("password")
                If TextBox2.Text = pswd Then
                    Button1.Enabled = False
                    Button2.Enabled = False
                    Form2.Show()
                    Me.Hide()
                Else
                    MsgBox("INVALID PASSWORD", MsgBoxStyle.Information, "MESSAGE")
                    TextBox2.Clear()
                    Timer1.Start()
                    Panel3.BackColor = Color.DarkRed
                End If
            Else
                MsgBox("USER DOESN'T EXIST", MsgBoxStyle.Exclamation, "MESSAGE")
                TextBox2.Clear()
                TextBox1.Clear()
                Timer1.Start()
                Panel3.BackColor = Color.DarkRed
                Panel2.BackColor = Color.DarkRed
            End If
        Else
            MsgBox("Please enter your credentials", MsgBoxStyle.ApplicationModal, "Message")
        End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        conOne.db = "authdetails"
        conOne.pswd = "Raina@1999"
        conOne.server = "localhost"
        conOne.uid = "root"

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Panel3.BackColor = Color.Teal
        Panel2.BackColor = Color.Teal
        Timer1.Stop()
    End Sub
End Class
