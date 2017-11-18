Imports System.IO
Imports System.Text

Public Class Memory
    Dim QuestionPath As String = Application.StartupPath & "\perlist.dll"

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

    Friend Function LoadQuestions() As List(Of String)
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
        End Try

        Return Questions

    End Function

    Function LoadCache()
        'Carregar para memoria alguma coisa q o programa precisa lembrar

        Return "arrayDeDatas"
    End Function

    Sub Wipe()
        'voltar o programa nas confs de fabrica

    End Sub

End Class

