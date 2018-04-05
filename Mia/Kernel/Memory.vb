Imports System.IO
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports Microsoft.SqlServer

Public Class Memory

#Region "Tables"

    Dim Notes As String = "CREATE TABLE `Notes` (
	                            `id`	INTEGER PRIMARY KEY AUTOINCREMENT,
	                            `Title`	TEXT,
	                            `Note`	TEXT
                                );"

    Dim GameList As String = "CREATE TABLE `GameList` (
	                            `id`	INTEGER PRIMARY KEY AUTOINCREMENT,
	                            `Name`	TEXT
                                );"

    Dim Question As String = "CREATE TABLE `Question` (
	                            `id`	INTEGER PRIMARY KEY AUTOINCREMENT,
	                            `QuestionText`	TEXT
                                );"
#End Region

    Dim MPath As String = IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "Data.db")
    Dim ConnectionString As String = String.Format("Data Source = {0}", MPath)

    Sub New()
        'Se não existir o banco de dados o mesmo e criado.
        If Not (File.Exists(MPath)) Then
            Debug.Print("Criando BD.")
            Using sql As New SQLiteConnection(ConnectionString)
                Dim NotesT As New SQLiteCommand(Notes, sql)
                Dim GameListT As New SQLiteCommand(GameList, sql)
                Dim QuestionT As New SQLiteCommand(Question, sql)
                sql.Open()
                NotesT.ExecuteNonQuery()
                GameListT.ExecuteNonQuery()
                QuestionT.ExecuteNonQuery()
            End Using
        End If
    End Sub

#Region "CUDs"

    Sub AddNotes(Title As String, Note As String)
        Using sql As New SQLiteConnection(ConnectionString)
            Dim addQuery As String = "insert into Notes(Title,Note) VALUES (@Title,@Note)"
            Dim cmd As New SQLiteCommand(addQuery, sql)
            cmd.Parameters.AddWithValue("@Title", Title)
            cmd.Parameters.AddWithValue("@Note", Note)
            sql.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub RemoveNotesT(ID As String)
        Using sql As New SQLiteConnection(ConnectionString)
            Dim deleteQuery As String = "delete from Notes WHERE ID =@it"
            Dim cmd As New SQLiteCommand(deleteQuery, sql)
            cmd.Parameters.AddWithValue("@it", ID)
            sql.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub UpdateNotes(Value As String, ID As String)
        Using sql As New SQLiteConnection(ConnectionString)
            Dim insertUserQuery As String = "update Notes Set Note =@Cod WHERE ID =@Tit"
            Dim cmd As New SQLiteCommand(insertUserQuery, sql)
            cmd.Parameters.AddWithValue("@Tit", ID)
            cmd.Parameters.AddWithValue("@Cod", Value)
            sql.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub AddGameList(Name As String)
        Using sql As New SQLiteConnection(ConnectionString)
            Dim addQuery As String = "insert into GameList(Name) VALUES (@Name)"
            Dim cmd As New SQLiteCommand(addQuery, sql)
            cmd.Parameters.AddWithValue("@Name", Name)
            sql.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub RemoveGameList(ID As String)
        Using sql As New SQLiteConnection(ConnectionString)
            Dim deleteQuery As String = "delete from GameList WHERE ID =@it"
            Dim cmd As New SQLiteCommand(deleteQuery, sql)
            cmd.Parameters.AddWithValue("@it", ID)
            sql.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub AddQuestionT(QuestionText As String)
        Using sql As New SQLiteConnection(ConnectionString)
            Dim addQuery As String = "insert into Question(QuestionText) VALUES (@QuestionText)"
            Dim cmd As New SQLiteCommand(addQuery, sql)
            cmd.Parameters.AddWithValue("@QuestionText", QuestionText)
            sql.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub RemoveQuestionT(ID As String)
        Using sql As New SQLiteConnection(ConnectionString)
            Dim deleteQuery As String = "delete from Question WHERE ID =@it"
            Dim cmd As New SQLiteCommand(deleteQuery, sql)
            cmd.Parameters.AddWithValue("@it", ID)
            sql.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

#End Region

    Function ReadNotes()
        Dim NotesOutput As New List(Of NotesSchedule)
        Using sql As New SQLiteConnection(ConnectionString)
            Dim insertUserQuery As String = "Select * From Notes"
            Dim cmd As New SQLiteCommand(insertUserQuery, sql)
            sql.Open()
            Dim reader As SQLiteDataReader = cmd.ExecuteReader()

            While reader.Read()
                NotesOutput.Add(New NotesSchedule(reader.GetValue(0), reader.GetValue(1), reader.GetValue(2)))
            End While

            Return NotesOutput
        End Using
    End Function

    Function ReadGameList()
        Dim GameListOutput As New List(Of GameListSchedule)
        Using sql As New SQLiteConnection(ConnectionString)
            Dim insertUserQuery As String = "Select * From GameList"
            Dim cmd As New SQLiteCommand(insertUserQuery, sql)
            sql.Open()
            Dim reader As SQLiteDataReader = cmd.ExecuteReader()

            While reader.Read()
                GameListOutput.Add(New GameListSchedule(reader.GetValue(0), reader.GetValue(1)))
            End While

            Return GameListOutput
        End Using
    End Function

    Function ReadQuestion()
        Dim QuestionOutput As New List(Of QuestionSchedule)
        Using sql As New SQLiteConnection(ConnectionString)
            Dim insertUserQuery As String = "Select * From Question"
            Dim cmd As New SQLiteCommand(insertUserQuery, sql)
            sql.Open()
            Dim reader As SQLiteDataReader = cmd.ExecuteReader()

            While reader.Read()
                QuestionOutput.Add(New QuestionSchedule(reader.GetValue(0), reader.GetValue(1)))
            End While

            Return QuestionOutput
        End Using
    End Function

    Sub Wipe()
        'voltar o programa nas confs de fabrica
        Try
            File.Delete(MPath)
        Catch ex As Exception

        End Try
    End Sub

End Class

Public Class NotesSchedule
    Private id As Integer
    Private Titulo As String
    Private Note As String

    Sub New(_ID As Integer, _Titulo As String, _Note As String)
        id = _ID
        Titulo = _Titulo
        Note = _Note
    End Sub

    Public Function GetTitulo() As String
        Return Me.Titulo
    End Function

    Public Function Getnotes() As String
        Return Me.Note
    End Function

    Public Function GetId() As Integer
        Return Me.id
    End Function

End Class

Public Class GameListSchedule
    Private id As Integer
    Private Name As String

    Sub New(_ID As Integer, _Name As String)
        id = _ID
        Name = _Name
    End Sub

    Public Function GetName() As String
        Return Me.Name
    End Function

    Public Function GetId() As Integer
        Return Me.id
    End Function

End Class

Public Class QuestionSchedule
    Private id As Integer
    Private QuestionText As String

    Sub New(_ID As Integer, _QuestionText As String)
        id = _ID
        QuestionText = _QuestionText
    End Sub

    Public Function GetQuestionText() As String
        Return Me.QuestionText
    End Function

    Public Function GetId() As Integer
        Return Me.id
    End Function

End Class
