Public Class Questions
    'Faz perguntas
    Dim Questions As New List(Of String)()
    Dim Rand As New Random()
    Dim ValueGerated As Integer
    Dim CacheValue As Integer

    Sub New(QuestionsReceved As List(Of String))
        'carrega o dicionario com as perguntas
        For Each words In QuestionsReceved
            Questions.Add(words)
        Next
        Debug.Print(Questions.Count.ToString + " Question(s) load")
    End Sub

    Public Function AskAQuestion() As String 'faz uma pergunta para interagir com o usuário
        Return Questions(GenerateNunber(Questions.Count))
    End Function


    Public Function GenerateNunber(ByVal Val As Integer)
        'gera numero aletorio
        If (ValueGerated > 0) Then
            CacheValue = ValueGerated
        End If

        ValueGerated = Nothing

        ValueGerated = Rand.Next(Val)

        If (ValueGerated = CacheValue) Then

            ValueGerated = Rand.Next(Val)
            If (ValueGerated = CacheValue) Then
                If (CacheValue = 0) Then
                    CacheValue = Rand.Next(Val)
                    If (CacheValue = 0) Then
                        CacheValue = Rand.Next(Val)
                    End If
                Else
                    CacheValue = Rand.Next(CacheValue + (Val - CacheValue))
                End If

                Return CacheValue

            Else
                CacheValue = ValueGerated
                Return ValueGerated
            End If
        Else
            Return ValueGerated
        End If

    End Function

End Class
