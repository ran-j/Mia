Public Class Think
    Dim TimeStarted
    Dim QuestionHandle As Questions
    Dim WarningsHandles As New Warnings

    Sub New(time)
        TimeStarted = time
    End Sub

    Sub ReceiveConversation(Conversation)
        'recebe os dados de conversação
    End Sub

    Sub ReceiveQuestions(ImportedQuestions As List(Of String))
        QuestionHandle = New Questions(ImportedQuestions) 'manda para classe de perguntas, perguntas para ela usar
    End Sub

    Sub Dates(Dates)
        'recebe datas salvas e faz algo com elas
    End Sub

    Sub Preferences(Preferences)
        'recebe as preferencias
    End Sub






    Function RequestQuestion() As String
        'retorna uma question pro usuário
        Return QuestionHandle.AskAQuestion
    End Function

    Function RequestWarnings(WarningCode As Integer) As String
        'retorna uma aviso pro usuário
        Return WarningsHandles.AnwserWarning(WarningCode)
    End Function

End Class

