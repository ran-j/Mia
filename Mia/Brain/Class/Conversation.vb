Public Class Conversation
    'Respostas para eventos e avisos
    Private Rand As New Random()
    Dim ValueGerated As Integer
    Dim CacheValue As Integer

    Dim usu As String = My.Settings.User

    'Respostas Prontas
    Protected comprimento As String() = New String() {"Olá senhor " + usu, "Oi senhor " + usu + ", tudo bom ?", "Oi :)"}
    Protected naoentendi As String() = New String() {"Desculpe-me senhor, porém não entendi o que foi pedido", "Não compreendi senhor " + usu, "Sinto muito porém não entendi senhor " + usu, "Não entendi senhor"}

    Public Function AnwserConversation(ByVal operaçao As Integer)
        Dim Output = "Não entendi"

        If (operaçao = 1) Then
            Output = comprimento(GenerateNunber(comprimento.Length))
        ElseIf (operaçao = 2) Then
            Output = naoentendi(GenerateNunber(naoentendi.Length))
        End If

        Return Output
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
