Imports System.IO
Imports System.Text

Public Class Memory
    Dim QuestionPath As String = Application.StartupPath & "\perlist.dll"
    Dim GameListPath As String = Application.StartupPath & "\glist.dll"
    Dim CommandsPath As String = Application.StartupPath & "\clist.dll"

    Sub SaveContent(Target As Object)
        'Salvar conteudo na memoria
    End Sub

    Function LoadConversation()
        'Carregar para memoria os dados salvos e retorna as falas

        Return "arrayDeFalas"
    End Function

    Function LoadDates()
        'Carregar para memoria datas salvas

        Return "arrayDeDatas"
    End Function

    Function LoadPreferences()
        'Carregar para memoria as preferencias do usuário

        Return "arrayDeDatas"
    End Function

    Public Function LoadQuestions() As List(Of String)
        'carrega as perguntas de um txt

        Dim Questions As New List(Of String)()
        Try
            If System.IO.File.Exists(QuestionPath) Then

                Dim readText() As String = File.ReadAllLines(QuestionPath, Encoding.Default)

                For Each line In readText
                    If (Not line = Nothing) Then
                        Questions.Add(line)
                    End If

                Next

            Else
                MsgBox("Erro ao procurar o dicionario", MsgBoxStyle.Exclamation)
                'adiciona uma perunta basica
                Questions.Add("Quantos anos voce tem ?")
                Questions.Add("Qual o seu nome todo ?")
                Questions.Add("Voce gosta de animais ?")
            End If
        Catch ex As Exception
            Questions.Add("Como se chama o seu pai ?")
            Questions.Add("Voce gosta de animais ?")
            MsgBox("Erro critico ao carregar o dicionario", MsgBoxStyle.Critical)

            My.Settings.Erros = My.Settings.Erros + 1
        End Try

        Return Questions

    End Function

    Public Function LoadGamesList() As List(Of String)
        'carrega a lista de jogos

        Dim GameList As New List(Of String)()
        Try
            If System.IO.File.Exists(GameListPath) Then

                Dim readText() As String = File.ReadAllLines(QuestionPath, Encoding.Default)

                For Each line In readText
                    If (Not line = Nothing) Then
                        GameList.Add(line)
                    End If

                Next

            Else
                'Debug.Print("Erro ao procurar o dicionario")
                'adiciona uma jogo basica
                GameList.Add("Counter-Strike: Global Offensive")
            End If
        Catch ex As Exception
            GameList.Add("Counter-Strike: Global Offensive")
            'Debug.Print("Erro critico ao carregar a lista de jogos")
        End Try

        Return GameList

    End Function

    Sub SaveGamesList(SaveGamesListArray)
        'Atualizar a lista de jogos salvos
        Try
            Using sw As StreamWriter = New StreamWriter(GameListPath, True)
                For Each Game In SaveGamesListArray
                    sw.WriteLine(Game)
                Next
            End Using
        Catch ex As Exception
            Main.Throwalert("Erro ao salvar")
            Debug.Print(ex.Message)
            My.Settings.Erros = My.Settings.Erros + 1
        End Try
    End Sub

    Sub SaveCommandsList(SaveCommandsArray)
        'Salvar comandos
        Try
            Using sw As StreamWriter = New StreamWriter(CommandsPath, True)
                For Each NewCommand In SaveCommandsArray
                    sw.WriteLine(NewCommand)
                Next
            End Using
        Catch ex As Exception
            Main.Throwalert("Erro ao salvar")
            Debug.Print(ex.Message)
            My.Settings.Erros = My.Settings.Erros + 1
        End Try
    End Sub



    Function LoadCache()
        'Carregar para memoria alguma coisa q o programa precisa lembrar

        Return "arrayDeDatas"
    End Function

    Sub Wipe()
        'voltar o programa nas confs de fabrica

    End Sub

End Class

