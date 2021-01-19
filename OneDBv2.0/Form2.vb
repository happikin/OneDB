Imports System.Drawing.Drawing2D
Imports MySql.Data.MySqlClient
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.Win32
Public Class Form2

    Dim sqlConn As New MySqlConnection
    Dim sqlCmd As New MySqlCommand
    Dim sqlDA As New MySqlDataAdapter
    Dim sqlDT As New DataTable
    Dim sqlDR As MySqlDataReader

    Public Sub refreshGrid()
        sqlConn.ConnectionString = "server=localhost;user id=root;password=Raina@1999;database=sakila;"
        sqlCmd.Connection = sqlConn
        sqlConn.Open()
        sqlCmd.CommandText = "SHOW TABLES;"
        'sqlCmd.ExecuteNonQuery()
        sqlDR = sqlCmd.ExecuteReader
        sqlDT.Load(sqlDR)
        sqlDR.Close()
        DataGridView1.DataSource = sqlDT
        sqlCmd.Parameters.Clear()
        sqlConn.Close()
        For index = 0 To sqlDT.Rows.Count - 1
            ComboBox1.Items.Add(sqlDT.Rows(index).Item(0).ToString)
        Next
        MsgBox(DataGridView1.Rows(0).Cells(0).Value.ToString, 1, MsgBoxStyle.Information)
    End Sub
    Public Sub showTables()
        sqlConn.ConnectionString = "server=localhost;user id=root;password=Raina@1999;database=sakila;"
        sqlCmd.Connection = sqlConn
        sqlConn.Open()
        sqlCmd.CommandText = "SHOW TABLES;"
        'qlCmd.ExecuteNonQuery()
        sqlDR = sqlCmd.ExecuteReader
        sqlDT.Load(sqlDR)
        sqlDR.Close()
        'DataGridView1.DataSource = sqlDT
        'sqlCmd.Parameters.Clear()
        sqlConn.Close()
        For index = 0 To sqlDT.Rows.Count - 1
            ComboBox1.Items.Add(sqlDT.Rows(index).Item(0).ToString)
        Next
        'MsgBox(DataGridView1.Rows(0).Cells(0).Value.ToString, 1, MsgBoxStyle.Information)
    End Sub


    Public themeState As Integer '0 for Light Mode;1 for Dark Mode
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        showTables()
        DataGridView1.GridColor = Color.Teal
        DataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        themeState = 0
        Panel2.BringToFront()
        Panel4.BackColor = Color.White
        Panel2.BackColor = Color.White
        Button2.BackColor = Color.White
        Button3.BackColor = Color.White
        Button4.BackColor = Color.White
        Button5.BackColor = Color.White
        Button6.BackColor = Color.White
        Button6.Text = "Dark Mode"
        Panel1.Visible = True
        Panel2.Visible = False
        Timer1.Interval = 3000
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Panel1.Visible = False
        'Panel2.Visible = True
        Timer1.Stop()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Panel2.Enabled = True
        Panel2.Visible = True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Panel2.Visible = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Panel2.Visible = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Panel2.Visible = False
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub CloseButton1_Click(sender As Object, e As EventArgs) Handles RButton1.Click
        MessageBox.Show("Good Bye!", "!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
        End
    End Sub

    Private Sub MinimizeButton1_Click(sender As Object, e As EventArgs) Handles MinimizeButton1.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click   'TOGGLE BUTTON
        If themeState = 0 Then  'if light theme
            Panel2.BackColor = Color.Black
            Button2.BackColor = Color.Black
            Button3.BackColor = Color.Black
            Button4.BackColor = Color.Black
            Button5.BackColor = Color.Black
            Button6.BackColor = Color.Black
            Me.BackColor = Color.Black
            Button1.Image = My.Resources.MenuDarkMode
            Button6.Text = "Light Mode"
            themeState = 1
        ElseIf themeState = 1 Then  'If dark mode
            Panel2.BackColor = Color.White
            Button2.BackColor = Color.White
            Button3.BackColor = Color.White
            Button4.BackColor = Color.White
            Button5.BackColor = Color.White
            Button6.BackColor = Color.White
            Me.BackColor = Color.White
            Button1.Image = My.Resources.icons8_menu_50__1_
            Button6.Text = "Dark Mode"
            themeState = 0
        End If
    End Sub

    Private Sub Panel4_Paint(sender As Object, e As PaintEventArgs) Handles Panel4.Paint

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        MessageBox.Show("Created By : Yashesvi Raina(Happikin)   https://github.com/happikin ", "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub RefreshButton1_Click(sender As Object, e As EventArgs) Handles RefreshButton1.Click
        refreshGrid()

    End Sub
End Class