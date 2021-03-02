Imports System.Drawing.Drawing2D
Imports MySql.Data.MySqlClient
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.Win32
Imports System.Diagnostics

Public Class Form2
    Public panelButtonName As String
    Public Const findmode = 1
    Public condition As String
    Public TypeFieldArrayCount As Integer = 0
    Public conOne As connCredentials
    Public themeState As Integer '0 for Light Mode;1 for Dark Mode
    Public dbname As String = "sakila"
    Public globalCounter = -1, globalIndex As Integer = 0
    Public entryContainer() As entryItem
    Public TypeFieldArray() As entryItem
    Public conditionContainer As entryItem

    Public Sub showData(ByVal tableName As String, Optional ByVal mode As Integer = 0, Optional query As String = " ")
        Dim sqlConn As New MySqlConnection
        Dim sqlCmd As New MySqlCommand
        Dim sqlDT As New DataTable
        Dim sqlDR As MySqlDataReader
        sqlConn.ConnectionString = "server=" + conOne.server + ";" + "user id=" + conOne.uid + ";" + "password=" + conOne.pswd + ";" + "database=" + conOne.db + ";"
        Try
            sqlConn.Open()
            sqlCmd.Connection = sqlConn
        Catch ex As Exception
            MsgBox(ex.Message, 64, "MESSAGE")
        End Try
        If mode = 0 Then
            sqlCmd.CommandText = "SELECT * FROM " + tableName + ";"
        Else
            '//--CODE FOR FIND OPERATION SHOULD COME HERE--//
            sqlCmd.CommandText = query
        End If

        Try
            sqlDR = sqlCmd.ExecuteReader
        Catch ex As Exception
            MsgBox(ex.Message, 64, "MESSAGE")
            Exit Sub
        End Try
        sqlDT.Load(sqlDR)
        sqlDR.Close()
        sqlConn.Close()

        DataGridView1.DataSource = sqlDT
        If DataGridView1.Columns.Count < 5 Then
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Else
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        End If
        If mode = findmode Then
            closeresetEditingPanel()
        End If
    End Sub

    Public Sub fillComboBox1()   'will fill the combobox with the list of all the tables in the database
        Dim sqlConn As New MySqlConnection
        Dim sqlCmd As New MySqlCommand
        Dim sqlDA As New MySqlDataAdapter
        Dim sqlDT As New DataTable
        Dim sqlDR As MySqlDataReader
        sqlDT.Clear()
        sqlConn.Close()
        sqlConn.ConnectionString = "server=" + conOne.server + ";" + "user id=" + conOne.uid + ";" + "password=" + conOne.pswd + ";" + "database=" + conOne.db + ";"
        sqlCmd.Connection = sqlConn

        Try
            sqlConn.Open()
            sqlCmd.CommandText = "SHOW TABLES;"
            sqlDR = sqlCmd.ExecuteReader
            sqlDT.Load(sqlDR)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "MESSAGE")
            Exit Sub
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

    Public Sub fillComboBox2()  'is supposed to be called from the reset button
        Dim sqlDA As New MySqlDataAdapter
        Dim sqlDR As MySqlDataReader
        Dim sqlCmd As New MySqlCommand
        Dim DataTable As New DataTable
        Dim sqlCon As New MySqlConnection("server=" + conOne.server + ";" + "user id=" + conOne.uid + ";" + "password=" + conOne.pswd + ";" + "database=" + conOne.db + ";")
        sqlCon.Open()
        sqlCmd.Connection = sqlCon

        If ComboBox1.SelectedItem <> Nothing Then
            sqlCmd.CommandText = "SELECT * FROM " + ComboBox1.SelectedItem.ToString + ";"
            sqlDR = sqlCmd.ExecuteReader
            DataTable.Load(sqlDR)
            sqlCon.Close()

            For index = 0 To TypeFieldArrayCount - 1
                ComboBox2.Items.Add(DataTable.Columns(index).ColumnName)
            Next

            sqlDA.Dispose()
            DataTable.Clear()
            sqlDR.Dispose()
            sqlCon.Close()
        End If

    End Sub

    Public Sub showTables()
        Dim sqlConn As New MySqlConnection
        Dim sqlCmd As New MySqlCommand
        Dim sqlDA As New MySqlDataAdapter
        Dim sqlDT As New DataTable
        Dim sqlDR As MySqlDataReader
        sqlConn.ConnectionString = "server=" + conOne.server + ";" + "user id=" + conOne.uid + ";" + "password=" + conOne.pswd + ";" + "database=" + conOne.db + ";"
        sqlCmd.Connection = sqlConn
        sqlConn.Open()
        sqlCmd.CommandText = "SHOW TABLES;"
        sqlDR = sqlCmd.ExecuteReader
        sqlDT.Load(sqlDR)
        sqlDR.Close()
        sqlConn.Close()
        sqlDT.Clear()
    End Sub

    Public Sub fillComboBox3()  'is supposed to be called from the reset button
        Dim sqlDA As New MySqlDataAdapter
        Dim sqlDR As MySqlDataReader
        Dim sqlCmd As New MySqlCommand
        Dim DataTable As New DataTable
        Dim sqlCon As New MySqlConnection("server=" + conOne.server + ";" + "user id=" + conOne.uid + ";" + "password=" + conOne.pswd + ";" + "database=" + conOne.db + ";")
        ComboBox3.Items.Clear()

        Try
            sqlCon.Open()
            sqlCmd.Connection = sqlCon
        Catch ex As Exception
            MsgBox(ex.Message, 64, "MESSAGE")
            Exit Sub
        End Try

        If ComboBox1.SelectedItem IsNot Nothing Then
            sqlCmd.CommandText = "SELECT * FROM " + ComboBox1.SelectedItem.ToString + ";"
            sqlDR = sqlCmd.ExecuteReader
            DataTable.Load(sqlDR)
            sqlCon.Close()

            TypeFieldArrayCount = DataTable.Columns.Count
            ReDim TypeFieldArray(TypeFieldArrayCount) 'will init declare the array size as the noumber of columns in the table

            For index = 0 To TypeFieldArrayCount - 1
                ComboBox3.Items.Add(DataTable.Columns(index).ColumnName)
                TypeFieldArray(index) = New entryItem(DataTable.Columns(index).ColumnName, DataTable.Columns(index).DataType.ToString)
            Next

            'TypeFieldArrayCount = 0 'reset count for future reuse

            Panel2.Visible = False
            sqlDA.Dispose()
            DataTable.Clear()
            sqlDR.Dispose()
            sqlCon.Close()
        Else
            MsgBox("Please select a table first", MsgBoxStyle.Information, "Alert")
        End If
    End Sub

    Public Sub showEditingPanel(ByVal bname As String) 'ByVal item As ComboBox.ObjectCollection)
        Dim sqlDA As New MySqlDataAdapter
        Dim sqlDR As MySqlDataReader
        Dim sqlCmd As New MySqlCommand
        Dim DataTable As New DataTable
        Dim sqlCon As New MySqlConnection("server=" + conOne.server + ";" + "user id=" + conOne.uid + ";" + "password=" + conOne.pswd + ";" + "database=" + conOne.db + ";")
        Try
            sqlCon.Open()
            sqlCmd.Connection = sqlCon
        Catch ex As Exception
            MsgBox(ex.Message, 64, "MESSAGE")
            Exit Sub
        End Try

        If ComboBox1.SelectedItem IsNot Nothing Then
            sqlCmd.CommandText = "SELECT * FROM " + ComboBox1.SelectedItem.ToString + ";"
            sqlDR = sqlCmd.ExecuteReader
            DataTable.Load(sqlDR)
            sqlCon.Close()
            TypeFieldArrayCount = DataTable.Columns.Count
            ReDim TypeFieldArray(TypeFieldArrayCount) 'will init declare the array size as the noumber of columns in the table

            For index = 0 To TypeFieldArrayCount - 1
                ComboBox2.Items.Add(DataTable.Columns(index).ColumnName)
                TypeFieldArray(index) = New entryItem(DataTable.Columns(index).ColumnName, DataTable.Columns(index).DataType.ToString)
                'TypeFieldArray(index).getConstraint(DataTable.Constraints(index)," ")
                'MsgBox(TypeFieldArray(index).getFieldType, 1, " ")
            Next
            Panel2.Visible = False
            Panel5.Visible = True
            Button11.Text = bname
            sqlDA.Dispose()
            DataTable.Clear()
            sqlDR.Dispose()
            sqlCon.Close()
            Button11.Enabled = False
            Panel12.Visible = False
        Else

            If MsgBox("Please select a table first", MsgBoxStyle.Information, "Alert") = MsgBoxResult.Ok Then
                Panel2.Visible = False
                Panel12.Visible = True
            End If
        End If

    End Sub

    Public Sub executeMySqlQuery(ByVal sqlquery As String) 'ByVal item As ComboBox.ObjectCollection)
        If MsgBox(sqlquery, 1, "Final Query") = MsgBoxResult.Ok Then
            '//---- Now we attemt to actually insert into the table----//'
            Dim sqlConn As New MySqlConnection
            Dim sqlCmd As New MySqlCommand
            sqlConn.ConnectionString = "server=" + conOne.server + ";" + "user id=" + conOne.uid + ";" + "password=" + conOne.pswd + ";" + "database=" + conOne.db + ";"
            sqlCmd.Connection = sqlConn
            sqlConn.Open()
            sqlCmd.CommandText = sqlquery
            Try
                sqlCmd.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Caution!")
                onlyresetEditingPanel()
                Exit Sub
            End Try
            sqlConn.Close()
            closeresetEditingPanel()
            showData(ComboBox1.SelectedItem.ToString)
        End If
    End Sub

    Public Sub onlyresetEditingPanel()
        globalCounter = 0
        RichTextBox1.Clear()
        ComboBox2.SelectedIndex = -1
        ComboBox2.Items.Clear()
        ComboBox2.ResetText()
        fillComboBox2()
        Button10.Enabled = True
        ComboBox2.Enabled = True
        RichTextBox1.Enabled = True
        TypeFieldArrayCount = 0
        globalCounter = -1
        globalIndex = 0
    End Sub

    Public Sub closeresetEditingPanel()
        Panel5.Visible = False
        globalCounter = 0
        RichTextBox1.Clear()
        ComboBox2.Items.Clear()
        ComboBox2.ResetText()
        Button10.Enabled = True
        ComboBox2.Enabled = True
        RichTextBox1.Enabled = True
        TypeFieldArrayCount = 0
        globalCounter = -1
        globalIndex = 0
    End Sub

    Public Sub dropTable(query As String, tablename As String)
        If MsgBox("PRESS YES TO CONFIRM DELETE TABLE : " & tablename, MsgBoxStyle.YesNo, "WARNING!") _
            = MsgBoxResult.Yes Then
            '//---- Now we attemt to actually drop an entire table----//'
            Dim sqlCon As New MySqlConnection
            Dim sqlCmd As New MySqlCommand
            sqlCon.ConnectionString = "server=" + conOne.server + ";" + "user id=" + conOne.uid + ";" + "password=" + conOne.pswd + ";" + "database=" + conOne.db + ";"
            sqlCmd.Connection = sqlCon
            sqlCon.Open()
            sqlCmd.CommandText = query
            Try
                sqlCmd.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Caution!")
                Exit Sub
            End Try
            sqlCon.Close()
            If ComboBox1.Items.Contains(tablename) Then
                ComboBox1.Items.Remove(tablename)
            End If
        End If
    End Sub

    Public Sub initialLighUI()
        Button8.BackColor = Color.White
        Button2.BackColor = Color.White
        Button3.BackColor = Color.White
        Button7.BackColor = Color.White
        Button4.BackColor = Color.White
        Button5.BackColor = Color.White
        Button6.BackColor = Color.White
        Button15.BackColor = Color.White
        Button16.BackColor = Color.White
        Button17.BackColor = Color.White

        Label3.ForeColor = Color.Black
        Label4.ForeColor = Color.Black
        Label6.ForeColor = Color.Black
        Label5.ForeColor = Color.Black
        TextBox1.ForeColor = Color.Black
        TextBox1.BackColor = Color.FromArgb(255, 255, 255)
        RichTextBox1.ForeColor = Color.Black
        RichTextBox1.BackColor = Color.FromArgb(255, 255, 255)
        RichTextBox2.ForeColor = Color.Black
        RichTextBox2.BackColor = Color.FromArgb(255, 255, 255)
        ComboBox1.BackColor = Color.FromArgb(255, 255, 255)
        ComboBox2.BackColor = Color.FromArgb(255, 255, 255)
        ComboBox3.BackColor = Color.FromArgb(255, 255, 255)
        ComboBox1.ForeColor = Color.Black
        ComboBox2.ForeColor = Color.Black
        ComboBox3.ForeColor = Color.Black

        DataGridView1.DefaultCellStyle.BackColor = Color.White 'LIGHT MODE
        DataGridView1.BorderStyle = BorderStyle.FixedSingle 'LIGHT MODE
        DataGridView1.GridColor = Color.Black   'FIX MODE
        DataGridView1.DefaultCellStyle.ForeColor = Color.Black 'LIGHT MODE
        DataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(4, 212, 212)    'LIGHT MODE
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255)   'LIGHT MODE
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black 'LIGHT MODE    
        Panel2.BackColor = Color.White

        Me.BackColor = Color.White
        Button6.Text = "Dark Mode"
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initialLighUI()
        DataGridView1.RowHeadersWidth = 20  'PERMANENT
        DataGridView1.EnableHeadersVisualStyles = False 'PERMANENT
        Panel12.Visible = True
        Panel1.Visible = True
        Panel11.Visible = False
        Panel8.Visible = False
        Panel5.Visible = False
        Panel2.BringToFront()
        themeState = 0
        'Button15.BackColor = Color.White
        'Button16.BackColor = Color.White
        'Button17.BackColor = Color.White
        'Button7.BackColor = Color.White
        'Button8.BackColor = Color.White
        'Panel4.BackColor = Color.White
        'Panel2.BackColor = Color.White
        'Button2.BackColor = Color.White
        'Button3.BackColor = Color.White
        'Button4.BackColor = Color.White
        'Button5.BackColor = Color.White
        'Button6.BackColor = Color.White
        'Button6.Text = "Dark Mode"
        Panel2.Visible = False

        conOne.server = "localhost"
        conOne.uid = "root"
        conOne.pswd = "Raina@1999"
        conOne.db = "demo"

        Timer1.Interval = 3000
        Timer1.Start()
        fillComboBox1()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Panel1.Visible = False
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
        Button11.Location = New Point(250, 358)
        Button10.Visible = True
        Button10.Enabled = True
        Button10.Location = New Point(80, 358)
        Button12.Location = New Point(149, 358)
        Button12.Size = New Size(81, 34)
        Button11.Enabled = True
        Panel11.Visible = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        showEditingPanel("DELETE")
        Button11.Location = New Point(80, 358)
        Button10.Visible = False
        Button10.Enabled = False
        Button12.Location = New Point(250, 358)
        Button12.Size = New Size(142, 34)
        Button11.Enabled = True
        Panel11.Visible = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ComboBox1.SelectedItem IsNot Nothing Then
            panelButtonName = "UPDATE"
            Panel8.Visible = True
            Panel5.Visible = False
            Panel8.Location = New Point(422, 200)
            fillComboBox3()
        Else
            If MsgBox("Please select a table first", MsgBoxStyle.Information, "Alert") = MsgBoxResult.Ok Then
                Panel2.Visible = False
            End If
        End If
        Panel11.Visible = False
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
        MessageBox.Show("Dark mode is under development")
        Panel2.Visible = False
        Panel11.Visible = False
        If themeState = 0 Then  'if currently light theme
            Panel2.BackColor = Color.Black
            Button2.BackColor = Color.Black
            Button3.BackColor = Color.Black
            Button4.BackColor = Color.Black
            Button5.BackColor = Color.Black
            Button6.BackColor = Color.Black
            Button8.BackColor = Color.Black
            Button15.BackColor = Color.Black
            Button16.BackColor = Color.Black
            Button17.BackColor = Color.Black

            Label3.ForeColor = Color.White
            Label4.ForeColor = Color.White
            Label6.ForeColor = Color.White
            Label5.ForeColor = Color.White
            TextBox1.ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(46, 45, 45)
            RichTextBox1.ForeColor = Color.White
            RichTextBox1.BackColor = Color.FromArgb(69, 68, 68)
            RichTextBox2.ForeColor = Color.White
            RichTextBox2.BackColor = Color.FromArgb(69, 68, 68)
            ComboBox1.BackColor = Color.FromArgb(69, 68, 68)
            ComboBox2.BackColor = Color.FromArgb(69, 68, 68)
            ComboBox3.BackColor = Color.FromArgb(69, 68, 68)
            ComboBox1.ForeColor = Color.White
            ComboBox2.ForeColor = Color.White
            ComboBox3.ForeColor = Color.White

            Me.BackColor = Color.FromArgb(46, 45, 45)
            DataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(46, 45, 45) 'DARK MODE
            DataGridView1.BorderStyle = BorderStyle.FixedSingle 'DARK MODE
            DataGridView1.GridColor = Color.Black   'DARK MODE
            DataGridView1.DefaultCellStyle.ForeColor = Color.White 'DARK MODE
            DataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 97, 129)    'DARK MODE
            DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(2, 78, 78)   'DARK MODE
            DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White 'DARK MODE    
            Button7.BackColor = Color.Black
            'Button1.Image = My.Resources.MenuDarkMode
            Button6.Text = "Light Mode"
            themeState = 1
        ElseIf themeState = 1 Then  'If currently dark mode

            Button8.BackColor = Color.White
            Button2.BackColor = Color.White
            Button3.BackColor = Color.White
            Button7.BackColor = Color.White
            Button4.BackColor = Color.White
            Button5.BackColor = Color.White
            Button6.BackColor = Color.White
            Button15.BackColor = Color.White
            Button16.BackColor = Color.White
            Button17.BackColor = Color.White

            Label3.ForeColor = Color.Black
            Label4.ForeColor = Color.Black
            Label6.ForeColor = Color.Black
            Label5.ForeColor = Color.Black
            TextBox1.ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(255, 255, 255)
            RichTextBox1.ForeColor = Color.Black
            RichTextBox1.BackColor = Color.FromArgb(255, 255, 255)
            RichTextBox2.ForeColor = Color.Black
            RichTextBox2.BackColor = Color.FromArgb(255, 255, 255)
            ComboBox1.BackColor = Color.FromArgb(255, 255, 255)
            ComboBox2.BackColor = Color.FromArgb(255, 255, 255)
            ComboBox3.BackColor = Color.FromArgb(255, 255, 255)
            ComboBox1.ForeColor = Color.Black
            ComboBox2.ForeColor = Color.Black
            ComboBox3.ForeColor = Color.Black

            DataGridView1.DefaultCellStyle.BackColor = Color.White 'LIGHT MODE
            DataGridView1.BorderStyle = BorderStyle.FixedSingle 'LIGHT MODE
            DataGridView1.GridColor = Color.Black   'FIX MODE
            DataGridView1.DefaultCellStyle.ForeColor = Color.Black 'LIGHT MODE
            DataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(4, 212, 212)    'LIGHT MODE
            DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255)   'LIGHT MODE
            DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black 'LIGHT MODE    
            Panel2.BackColor = Color.White

            Me.BackColor = Color.White
            'Button1.Image = My.Resources.icons8_menu_50__1_
            Button6.Text = "Dark Mode"
            themeState = 0
        End If
    End Sub

    Private Sub Panel4_Paint(sender As Object, e As PaintEventArgs) Handles Panel4.Paint

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim target As String = "http://www.github.com/happikin/"
        Dim browser As String = "C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"
        Panel11.Visible = False
        Panel2.Visible = False
        If MessageBox.Show("Created By : Yashesvi Raina(Happikin) " & vbCrLf & "Press Yes to visit github", "About", MessageBoxButtons.YesNo, MessageBoxIcon.Information) _
            = DialogResult.Yes Then
            Try
                Process.Start(target)
            Catch ex As Exception
                Try
                    Process.Start(browser, target)
                Catch ex2 As Exception
                    MsgBox(ex2.Message, 64, " ")
                End Try
            End Try
        End If
    End Sub

    Private Sub RefreshButton1_Click(sender As Object, e As EventArgs) Handles RefreshButton1.Click
        fillComboBox1()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If ComboBox1.SelectedItem IsNot Nothing Then
            showData(ComboBox1.SelectedItem.ToString)
            Panel12.Visible = False
        Else
            MsgBox("Please select a table first", MsgBoxStyle.Information, "Message")
        End If
        Panel2.Visible = False
        Panel11.Visible = False
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If ComboBox1.SelectedItem IsNot Nothing Then
            panelButtonName = "FIND"
            Panel8.Visible = True
            Panel5.Visible = False
            Panel8.Location = New Point(422, 200)
            fillComboBox3()
            Button11.Text = "FIND"
            RichTextBox1.Visible = False
            Label4.Visible = False
            Panel12.Visible = False
        Else
            If MsgBox("Please select a table first", MsgBoxStyle.Information, "Alert") = MsgBoxResult.Ok Then
                Panel2.Visible = False
            End If
        End If
        'Panel11.Visible = False
        'Panel5.Visible = True

        'showEditingPanel("FIND")
        'Panel2.Visible = False
        'Panel11.Visible = False
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        closeresetEditingPanel()
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
        onlyresetEditingPanel()
    End Sub

    Private Sub Form2_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick
        Panel2.Visible = False
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If Button11.Text = "FIND" Then
            globalCounter += 1
            entryContainer(globalCounter) = New entryItem(ComboBox2.SelectedItem)
            ComboBox2.Items.Remove(ComboBox2.SelectedItem)

            If ComboBox2.Items.Count = 0 Then
                ComboBox2.Enabled = False
                TextBox1.Text = "Please build the query."
                Button10.Enabled = False
            End If
        Else
            If ComboBox2.SelectedItem IsNot Nothing Then
                If RichTextBox1.Text.Equals(" ") Then
                    MsgBox("Text Box is Empty, Enter some value", 64, "ALERT!")
                End If
                globalCounter = globalCounter + 1
                entryContainer(globalCounter) = New entryItem(ComboBox2.SelectedItem, RichTextBox1.Text, " ")

                '//----------------------------------------------------------------------//
                'Now we must find the corresponding type from the previously made type-field array and init the respective objects' typevalue
                For index = 0 To TypeFieldArrayCount - 1
                    If entryContainer(globalCounter).getColName = TypeFieldArray(index).getColName Then
                        entryContainer(globalCounter).setFieldType(TypeFieldArray(index).getFieldType)
                    End If
                Next
                'Now we have all our columns with their types and data in a single object
                '//----------------------------------------------------------------------//


                '//--- Here will be the code to implicitly parse the value into the suitable one for sql query syntax----//
                If Not entryContainer(globalCounter).parseValue() Then
                    onlyresetEditingPanel()
                End If
                '//---------------------------------------------------------------------------------//'

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
        End If

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        TextBox1.Clear()
        Timer2.Stop()
    End Sub


    Private Sub Panel5_Enter(sender As Object, e As EventArgs) Handles Panel5.Enter
        ReDim entryContainer(ComboBox2.Items.Count)     'will automatically create array of objects to contain max possible
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        RichTextBox2.Clear()
        ComboBox2.Items.Clear()
        ComboBox3.Items.Clear()
        Panel5.Visible = False
        Panel8.Visible = False
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        If ComboBox3.SelectedItem IsNot Nothing Then
            conditionContainer = New entryItem(ComboBox3.SelectedItem.ToString, RichTextBox2.Text, " ")
            For index = 0 To TypeFieldArrayCount - 1
                If conditionContainer.getConditionName = TypeFieldArray(index).getColName Then
                    conditionContainer.setConditionType(TypeFieldArray(index).getFieldType)
                    conditionContainer.parseValue()
                    Exit For
                End If
            Next
            Panel8.Visible = False
            'ComboBox3.Items.Clear()

            showEditingPanel(panelButtonName)
            Button11.Enabled = True
            Button11.Location = New Point(250, 358)
            Button10.Visible = True
            Button10.Enabled = True
            Button10.Location = New Point(80, 358)
            Button12.Location = New Point(149, 358)
            Button12.Size = New Size(81, 34)
            Button11.Enabled = True
        Else
            MsgBox("Select a constraint", 1, "Message!")
        End If
        RichTextBox2.Clear()
        ComboBox3.Items.Clear()

    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Panel11.Visible = True
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Panel11.Visible = False
        Panel2.Visible = False
        Me.Hide()
        Form3.Show()
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        Panel2.Visible = False
        Panel11.Visible = False
        If ComboBox1.SelectedItem IsNot Nothing Then
            dropTable("DROP TABLE " & ComboBox1.SelectedItem.ToString & ";", ComboBox1.SelectedItem.ToString)
        Else
            MsgBox("Please select a table first", 64, "Message")
        End If
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        Panel11.Visible = False
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)

    End Sub

    Private Sub DataGridView1_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        TextBox1.Clear()
        Dim tbname As String = ComboBox1.SelectedItem.ToString
        Dim query As String
        Select Case (Button11.Text)
            Case "INSERT"   'MODE : 0
                If globalCounter = ComboBox2.Items.Count Then
                    'PUT THE MODIFIED COE HERE WITH THE INSERT INTO TABLE VALUE(-----) type
                End If
                '//----First we develop an iterative mechanism for building a query from user input----//
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
                    'Dim a As String = entryContainer(1).getFieldType()
                    executeMySqlQuery(query)
                End If
            Case "DELETE"   'MODE : 1
                'we can also provide a choice pane to select from which 
                'type of consition clause must be chosen
                'like:-
                'o Where clause
                'o
                'o
                If ComboBox2.SelectedItem IsNot Nothing Then
                    query = "DELETE " & " FROM " & tbname & " WHERE " & ComboBox2.SelectedItem.ToString & " LIKE " _
                        & ControlChars.Quote & RichTextBox1.Text & ControlChars.Quote & ";"
                    executeMySqlQuery(query)

                End If
            Case "UPDATE"   'MODE : 2
                query = "UPDATE " & tbname & " SET "
                If entryContainer(0) IsNot Nothing Then
                    query = query & entryContainer(0).getColName & " = " &
                            entryContainer(0).getValue
                End If

                For index = 1 To globalCounter
                    If entryContainer(index) IsNot Nothing Then
                        query = query & " , " & entryContainer(index).getColName & " = " &
                            entryContainer(index).getValue
                    Else
                        Exit For
                    End If
                Next

                query = query & " WHERE " & conditionContainer.getConditionName &
                    " LIKE " & conditionContainer.getConditionValue & ";"
                executeMySqlQuery(query)

            Case "FIND"
                If entryContainer(0) IsNot Nothing Then
                    query = "SELECT " & entryContainer(0).getColName
                Else
                    Exit Sub
                End If
                For index = 1 To globalCounter

                    If entryContainer(index) IsNot Nothing Then
                        query &= "," + entryContainer(index).getColName
                    Else
                        Exit For
                    End If
                Next
                query = query + " FROM " & tbname & " WHERE " & conditionContainer.getConditionName &
                    " LIKE " & conditionContainer.getConditionValue & ";"
                showData(tbname, findmode, query)
        End Select
    End Sub

End Class