Imports System.Drawing.Drawing2D
Imports MySql.Data.MySqlClient
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.Win32
Public Class Form2

    Public Structure connCredentials
        Dim server, uid, pswd, db As String
    End Structure
    Public condition As String
    Public TypeFieldArrayCount As Integer = 0
    Public conOne As connCredentials
    Public themeState As Integer '0 for Light Mode;1 for Dark Mode
    Public dbname As String = "sakila"
    Public globalCounter = -1, globalIndex As Integer = 0
    Public entryContainer() As entryItem
    Public TypeFieldArray() As entryItem
    Public conditionContainer As entryItem
    Public Sub showData(ByVal tableName As String)
        Dim sqlConn As New MySqlConnection
        Dim sqlCmd As New MySqlCommand
        Dim sqlDA As New MySqlDataAdapter
        Dim sqlDT As New DataTable
        Dim sqlDR As MySqlDataReader
        sqlDT.Clear()
        sqlConn.Close()
        sqlConn.ConnectionString = "server=" + conOne.server + ";" + "user id=" + conOne.uid + ";" + "password=" + conOne.pswd + ";" + "database=" + conOne.db + ";"
        sqlConn.Open()
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandText = "select * from " + tableName
        sqlDR = sqlCmd.ExecuteReader
        sqlDT.Load(sqlDR)
        sqlDR.Close()
        sqlConn.Close()
        DataGridView1.DataSource = sqlDT

        If DataGridView1.Columns.Count < 5 Then
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        Else
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
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
        sqlCon.Open()
        sqlCmd.Connection = sqlCon

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
        sqlCon.Open()
        sqlCmd.Connection = sqlCon

        If ComboBox1.SelectedItem <> Nothing Then
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

        Else
            MsgBox("Please select a table first", MsgBoxStyle.Information, "Alert")
        End If

    End Sub

    Public Sub executeMySqlQuery(ByVal sqlquery As String) 'ByVal item As ComboBox.ObjectCollection)
        MsgBox(sqlquery, 1, "Final Query")
        If MsgBoxResult.Ok Then
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
                Exit Sub
            End Try
            sqlConn.Close()
            resetEditingPanel()
            showData(ComboBox1.SelectedItem.ToString)
        End If
    End Sub
    Public Sub resetEditingPanel()
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




    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel8.Visible = False
        Panel5.Visible = False
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
        conOne.server = "localhost"
        conOne.uid = "root"
        conOne.pswd = "Raina@1999"
        conOne.db = "demo"
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
        Button11.Location = New Point(250, 358)
        Button10.Visible = True
        Button10.Enabled = True
        Button10.Location = New Point(80, 358)
        Button12.Location = New Point(149, 358)
        Button12.Size = New Size(81, 34)
        Button11.Enabled = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        showEditingPanel("DELETE")
        Button11.Location = New Point(80, 358)
        Button10.Visible = False
        Button10.Enabled = False
        Button12.Location = New Point(250, 358)
        Button12.Size = New Size(142, 34)
        Button11.Enabled = True
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Panel8.Visible = True
        Panel5.Visible = False
        Panel8.Location = New Point(422, 200)
        fillComboBox3()



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


        Panel2.Visible = False
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Button11.Text = "FIND"
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        resetEditingPanel()
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
        TypeFieldArrayCount = 0
        globalCounter = -1
        globalIndex = 0
    End Sub

    Private Sub Form2_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick
        Panel2.Visible = False
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        If ComboBox2.SelectedItem IsNot Nothing Then
            If RichTextBox1.Text.Equals(" ") Then
                MsgBox("Text Box is Empty, Enter some value", 1, "ALERT!")
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


            '//--- Here will be the code to implicitly parse the value intp the suitable one----//
            entryContainer(globalCounter).parseValue()
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
            ComboBox3.Items.Clear()

            showEditingPanel("UPDATE")
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

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
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
            Case "CREATE"
        End Select
    End Sub

End Class