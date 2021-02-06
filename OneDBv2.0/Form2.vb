Imports System.Drawing.Drawing2D
Imports MySql.Data.MySqlClient
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.Win32
Public Class Form2

    Public dbname As String = "sakila"
    Public globalCounter = -1, globalIndex As Integer = 0
    Public entryContainer() As entryItem

    Public Sub showData(ByVal tableName As String)
        Dim sqlConn As New MySqlConnection
        Dim sqlCmd As New MySqlCommand
        Dim sqlDA As New MySqlDataAdapter
        Dim sqlDT As New DataTable
        Dim sqlDR As MySqlDataReader
        sqlDT.Clear()
        sqlConn.Close()
        sqlConn.ConnectionString = "server=localhost;user id=root;password=Raina@1999;database=" + dbname + ";"
        sqlConn.Open()
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandText = "select * from " + tableName
        sqlDR = sqlCmd.ExecuteReader
        sqlDT.Load(sqlDR)
        sqlDR.Close()
        sqlConn.Close()
        DataGridView1.DataSource = sqlDT
    End Sub


    Public Sub fillComboBox1()   'will fill the combobox with the list of all the tables in the database
        Dim sqlConn As New MySqlConnection
        Dim sqlCmd As New MySqlCommand
        Dim sqlDA As New MySqlDataAdapter
        Dim sqlDT As New DataTable
        Dim sqlDR As MySqlDataReader
        sqlDT.Clear()
        sqlConn.Close()
        sqlConn.ConnectionString = "server=localhost;user id=root;password=Raina@1999;database=" + dbname + ";"
        sqlCmd.Connection = sqlConn
        sqlConn.Open()
        sqlCmd.CommandText = "SHOW TABLES;"
        sqlDR = sqlCmd.ExecuteReader

        Try
            sqlDT.Load(sqlDR)
        Catch ex As Exception
            MsgBox("Nope not working", 1, MsgBoxStyle.Information)
        End Try

        sqlDR.Close()
        sqlCmd.Parameters.Clear()
        sqlConn.Close()
        For index = 0 To sqlDT.Rows.Count - 1
            If ComboBox1.Items.Contains(sqlDT.Rows(index).Item(0)) = False Then
                ComboBox1.Items.Add(sqlDT.Rows(index).Item(0).ToString)
            End If
        Next
        sqlDT.Clear()
    End Sub
    Public Sub showTables()
        Dim sqlConn As New MySqlConnection
        Dim sqlCmd As New MySqlCommand
        Dim sqlDA As New MySqlDataAdapter
        Dim sqlDT As New DataTable
        Dim sqlDR As MySqlDataReader
        sqlConn.ConnectionString = "server=localhost;user id=root;password=Raina@1999;database=" + dbname + ";"
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
        sqlDT.Clear()
        'MsgBox(DataGridView1.Rows(0).Cells(0).Value.ToString, 1, MsgBoxStyle.Information)
    End Sub

    Public Sub showEditingPanel(ByVal bname As String) 'ByVal item As ComboBox.ObjectCollection)
        Dim sqlDA As New MySqlDataAdapter
        Dim sqlDR As MySqlDataReader
        Dim sqlCmd As New MySqlCommand
        Dim DataTable As New DataTable
        Dim sqlCon As New MySqlConnection("server=localhost;user id=root;password=Raina@1999;database=" + dbname + ";")
        sqlCon.Open()
        sqlCmd.Connection = sqlCon

        If ComboBox1.SelectedItem <> Nothing Then
            sqlCmd.CommandText = "SELECT * FROM " + ComboBox1.SelectedItem.ToString + ";"
            sqlDR = sqlCmd.ExecuteReader
            DataTable.Load(sqlDR)
            sqlCon.Close()
            For index = 0 To DataTable.Columns.Count - 1
                ComboBox2.Items.Add(DataTable.Columns(index).ColumnName)
            Next
            Panel2.Visible = False
            Panel5.Visible = True
            Button11.Text = bname
            sqlDA.Dispose()
            DataTable.Clear()
            sqlDR.Dispose()
            sqlCon.Close()
            Button11.Enabled = False

        Else
            MsgBox("Please select a table first", MsgBoxStyle.Information, "Alert")
        End If

    End Sub

    Public Sub fillComboBox2()  'is supposed to be called from the reset button
        Dim sqlDA As New MySqlDataAdapter
        Dim sqlDR As MySqlDataReader
        Dim sqlCmd As New MySqlCommand
        Dim DataTable As New DataTable
        Dim sqlCon As New MySqlConnection("server=localhost;user id=root;password=Raina@1999;database=" + dbname + ";")
        sqlCon.Open()
        sqlCmd.Connection = sqlCon

        If ComboBox1.SelectedItem <> Nothing Then
            sqlCmd.CommandText = "SELECT * FROM " + ComboBox1.SelectedItem.ToString + ";"
            sqlDR = sqlCmd.ExecuteReader
            DataTable.Load(sqlDR)
            sqlCon.Close()
            For index = 0 To DataTable.Columns.Count - 1
                ComboBox2.Items.Add(DataTable.Columns(index).ColumnName)
            Next
            sqlDA.Dispose()
            DataTable.Clear()
            sqlDR.Dispose()
            sqlCon.Close()
        End If

    End Sub

    Public themeState As Integer '0 for Light Mode;1 for Dark Mode
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel5.Visible = False
        'DataGridView1.GridColor = Color.Teal
        'DataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken

        themeState = 0
        Panel2.BringToFront()
        Button7.BackColor = Color.White
        Button8.BackColor = Color.White
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

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Panel2.Enabled = True
        Panel2.Visible = True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        showEditingPanel("INSERT")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        showEditingPanel("DELETE")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        showEditingPanel("UPDATE")
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub CloseButton1_Click(sender As Object, e As EventArgs) Handles RButton1.Click

        End
    End Sub

    Private Sub MinimizeButton1_Click(sender As Object, e As EventArgs) Handles MinimizeButton1.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click   'TOGGLE BUTTON
        Panel2.Visible = False
        If themeState = 0 Then  'if light theme
            Panel2.BackColor = Color.Black
            Button2.BackColor = Color.Black
            Button3.BackColor = Color.Black
            Button4.BackColor = Color.Black
            Button5.BackColor = Color.Black
            Button6.BackColor = Color.Black
            Button8.BackColor = Color.Black

            Me.BackColor = Color.Black
            Button7.BackColor = Color.Black
            Button1.Image = My.Resources.MenuDarkMode
            Button6.Text = "Light Mode"
            themeState = 1
        ElseIf themeState = 1 Then  'If dark mode
            Panel2.BackColor = Color.White
            Button8.BackColor = Color.White
            Button2.BackColor = Color.White
            Button3.BackColor = Color.White
            Button7.BackColor = Color.White
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
        Panel2.Visible = False
        MessageBox.Show("Created By : Yashesvi Raina(Happikin)   https://github.com/happikin ", "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub RefreshButton1_Click(sender As Object, e As EventArgs) Handles RefreshButton1.Click
        fillComboBox1()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If ComboBox1.SelectedItem <> Nothing Then
            showData(ComboBox1.SelectedItem.ToString)
        Else
            MsgBox("Please select a table name!", MsgBoxStyle.Information, "Message")
        End If

        If DataGridView1.Columns.Count < 5 Then
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        Else
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        End If
        Panel2.Visible = False
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Button11.Text = "FIND"
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Panel5.Visible = False
        globalCounter = 0
        RichTextBox1.Clear()
        ComboBox2.Items.Clear()
        ComboBox2.ResetText()
        Button10.Enabled = True
        ComboBox2.Enabled = True
        RichTextBox1.Enabled = True
    End Sub

    Private Sub Panel5_Paint(sender As Object, e As PaintEventArgs) Handles Panel5.Paint
        If Panel5.Visible = True Then
            '    Dim sqlCmd As New MySqlCommand
            '    Dim sqlDA As New MySqlDataAdapter
            '    Dim sqlDT As New DataTable
            '    Dim sqlDR As MySqlDataReader
            '    Dim sqlCon As New MySqlConnection("server=localhost;user id=root;password=Raina@1999;database=" + dbname + ";")
            '    sqlCon.Open()
            '    sqlCmd.Connection = sqlCon

            '    sqlCmd.Parameters.AddWithValue(ComboBox2.SelectedItem.ToString, ComboBox2.SelectedValue)

            '    sqlCon.Close()
        End If

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        globalCounter = 0
        RichTextBox1.Clear()
        ComboBox2.SelectedIndex = -1
        ComboBox2.Items.Clear()
        ComboBox2.ResetText()
        fillComboBox2()
        Button10.Enabled = True
        ComboBox2.Enabled = True
        RichTextBox1.Enabled = True
    End Sub

    Private Sub Form2_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick
        Panel2.Visible = False
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        If ComboBox2.SelectedItem IsNot Nothing Then
            globalCounter = globalCounter + 1
            entryContainer(globalCounter) = New entryItem(ComboBox2.SelectedItem, RichTextBox1.Text)
            Button11.Enabled = True
            ComboBox2.Items.Remove(ComboBox2.SelectedItem)  'will remove the initialized column name from the drop down list
            RichTextBox1.Clear()
            If ComboBox2.Items.Count = 0 Then
                ComboBox2.Enabled = False
                RichTextBox1.Enabled = False
                TextBox1.Text = "Please build the query."
                Button10.Enabled = False
            End If
        Else
            TextBox1.Text = "Please recheck the selections!"
            Timer2.Start()
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        TextBox1.Clear()
        Timer2.Stop()
    End Sub


    Private Sub Panel5_Enter(sender As Object, e As EventArgs) Handles Panel5.Enter
        ReDim entryContainer(ComboBox2.Items.Count)     'will automatically create array of objects to contain max possible
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim sqlCmd As New MySqlCommand
        Dim tbname = ComboBox1.SelectedItem.ToString, query As String

        If globalCounter = ComboBox2.Items.Count Then
            'PUT THE MODIFIED COE HERE WITH THE INSERT INTO TABLE VALUE(-----) type
        End If

        If entryContainer(globalIndex) IsNot Nothing Then
            query = Button11.Text + " INTO " + tbname + "(" + entryContainer(0).getColName

            'globalIndex += 1

            For index = 1 To globalCounter

                If entryContainer(index) IsNot Nothing Then
                    query = query + "," + entryContainer(index).getColName
                Else
                    Exit For
                End If

            Next

            query = query + ") VALUES(" + entryContainer(0).getValue

            For index = 1 To globalCounter
                If entryContainer(index) IsNot Nothing Then
                    query = query + "," + entryContainer(index).getValue
                Else
                    Exit For
                End If
            Next
            query = query + ");"
            MsgBox(query, 1, "")
        End If

        'sqlCmd.Parameters.AddRange(entryContainer)
        'sqlCmd.CommandText = "insert into " + ComboBox1.SelectedItem.ToString + "values(@column1);"

    End Sub
End Class