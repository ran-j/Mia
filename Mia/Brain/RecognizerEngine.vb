Imports System.Globalization
Imports System.Threading
Imports System.Speech.Recognition
Imports System.IO
Imports System.Text

Public Class RecognizerEngine
    Private WithEvents RecognizerS As New SpeechRecognitionEngine
    Dim GrammarB As Grammar
    Private wordlist As String() = New String() {"Talvez"}

    Public Event TextRecognized(Text As String)

    Dim CommandsPath As String = Application.StartupPath & "\clist.dll"

    Sub New()

        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-GB")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-GB")

        RecognizerS.SetInputToDefaultAudioDevice()

        Dim AdditionalWords = LoadCommandsList()

        Debug.Print("Tamanho do carregamento de comados " + AdditionalWords.Count.ToString)

        For Each additionalword In AdditionalWords
            wordlist = AppendArray(wordlist, additionalword)
        Next

        Dim words As New Choices(wordlist)

        GrammarB = New Grammar(New GrammarBuilder(words))

        RecognizerS.LoadGrammar(GrammarB)

        RecognizerS.RecognizeAsync(RecognizeMode.Multiple)
    End Sub


    Private Sub RecognizerS_RecognizeCompleted(ByVal sender As Object, ByVal e As System.Speech.Recognition.RecognizeCompletedEventArgs) Handles RecognizerS.RecognizeCompleted
        RecognizerS.RecognizeAsync(RecognizeMode.Multiple)
    End Sub

    Private Sub RecognizerS_SpeechRecognized(ByVal sender As Object, ByVal e As System.Speech.Recognition.RecognitionEventArgs) Handles RecognizerS.SpeechRecognized
        RaiseEvent TextRecognized(e.Result.Text)
    End Sub


    Public Function AppendArray(Of T)(ByVal thisArray() As T, ByVal itemToAppend As T) As T()

        If thisArray Is Nothing Then thisArray = New T() {}
        Dim tempList As List(Of T) = thisArray.ToList
        tempList.Add(itemToAppend)
        Return tempList.ToArray

    End Function


    Public Function LoadCommandsList() As List(Of String)
        'carrega os comandos de voz

        Dim Commands As New List(Of String)()
        Try
            If System.IO.File.Exists(CommandsPath) Then

                Dim readText() As String = File.ReadAllLines(CommandsPath, Encoding.Default)

                For Each line In readText
                    If (Not line = Nothing) Then
                        Commands.Add(line)
                    End If

                Next

            Else
                Debug.Print("Dicionaroio de comandos não localizado")
                'adiciona uma jogo basica
            End If
        Catch ex As Exception
            Debug.Print("Erro critico ao carregar a lista de jogos")
        Finally
            Commands.Add("oi")
            Commands.Add("emular jogo de play station")
            Commands.Add("emular jogo de ps1")
            Commands.Add("como está o tempo hoje")
            Commands.Add("previão do tempo")
            Commands.Add("qual é a previão do tempo")
            Commands.Add("como está a internet")
            Commands.Add("velocidade da internet")
            Commands.Add("como está o seu sistema")
            Commands.Add("status do seu sistem")
            Commands.Add("como voce está")


            Debug.Print("Erro ao procurar o dicionario de comandos")
        End Try

        Return Commands

    End Function

End Class
