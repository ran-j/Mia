﻿Public Class Process
    'principal que controla as outras

    'Controladores, cada um responsavel por uma coisa
    Dim TheMemory As New Memory 'Salva e busca dados salvos
    Dim Thinking As New Think(TimeOfDay.ToString("h:mm:ss tt")) 'Processa comandos do usuário
    Dim Action As New Act 'Toda a parte de ação que responde a usuário
    Dim Interpreter As New Interpreter ' interpretador de texto

    Public Event LoadCompleted()

    Sub Init1() 'Starta o processamento
        Thinking.ReceiveConversation(TheMemory.LoadConversation())
        Thinking.ReceiveQuestions(TheMemory.LoadQuestions)
        Thinking.Dates(TheMemory.LoadDates())
        Thinking.Preferences(TheMemory.LoadPreferences())

        RaiseEvent LoadCompleted() 'termiona o processamento e avisa
    End Sub


    Sub Init5() 'Significa o fim do programa

    End Sub

    Function SalveRequest(TargedForSave) As Boolean
        'Recbe solicitações para salvar e verifica se o mesmo consegue salvar
        Dim status As Boolean = False

        Return status
    End Function

    Function RequestWarnings(WarningCode As Integer) As String
        If (WarningCode > 0) Then
            'reorna uma aviso
            Return Thinking.RequestWarnings(WarningCode)
        Else
            Return "Erro"
        End If

    End Function

    Function RequestQuestion() As String
        'retorna uma pergunta rando pro usuário
        Return Thinking.RequestQuestion()
    End Function

    Function RequestVerifyText(text)
        'verifica o texto corrige e retorna ele redondo
        Return Interpreter.VerifyText(text)
    End Function

End Class
