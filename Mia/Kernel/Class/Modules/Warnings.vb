﻿Public Class Warnings
    'Respostas para eventos e avisos
    Private Rand As New Random()
    Dim ValueGerated As Integer
    Dim CacheValue As Integer

    Dim usu As String = My.Settings.User

    'Avisos
    Protected minimizadoav As String() = New String() {"Senhor estou ativa aqui no canto.", "Senhor estou aberta em em segundo plano se precisar", "Senhor se Precisar de algo estou em segundo plano"}
    Protected avisanetvoltou As String() = New String() {"Senhor " + usu + " a conexção com a internet foi restabelecida", "Senhor " + usu + " a conexção com a internet foi restaurada", "Senhor " + usu + " a conexção com a internet retornou"}
    Protected avisanetcaiu As String() = New String() {"Senhor " + usu + " a conexção com a internet foi perdida", "Senhor " + usu + " a conexção com a internet caiu", "Senhor " + usu + "a internet caiu"}
    Protected boamadrugada As String() = New String() {"Bom dia senhor " + usu + ", poise já e dia", "Bom dia senhor " + usu + ", está um pouco cedo não ?", "Bom dia senhor " + usu + ", apesar de ainda está um pocuo escuro"}
    Protected bomdiaavisa As String() = New String() {"Bom dia senhor " + usu + ", espero que tenho tido uma boa noite de sono", "Bom dia senhor " + usu + ", e bom falar com o senhor novamente", "Bom dia senhor " + usu + ", é vamos para mais um dia"}
    Protected boatardeavisa As String() = New String() {"Boa tarde senhor " + usu + "e um prazer em ter o senhor aqui", "Boa tarde senhor " + usu + ", estou feliz em vê lo", "Boa Tarde senhor " + usu}
    Protected boanoiteavisa As String() = New String() {"Boa noite senhor " + usu + ", mais um dia chegando ao fim", "Boa noite e bom descanço senhor " + usu, "Boa noite senhor " + usu + ", dia cheio hoje em"}
    Protected respondeavisa As String() = New String() {"Sim senhor " + usu + " estou aqui", "Em que posso ajudar senhor " + usu + " ?", "O que posso fazer pelo senhor " + usu + " ?"}
    Protected bloqueapcavisa As String() = New String() {"Sim senhor, bloqueando o computador", "Entendido ate mais tarde senhor " + usu, "Certo bloqueando o PC senhor " + usu}
    Protected vaijogar As String() = New String() {"Reparei que o senhor vai jogar e verifiquei o ping para o senhor.", "Senhor " + usu + " já que o senhor vai jogar eu verifiquei o ping para o senhor", "Senhor " + usu + " verifiquei o ping para o senhor"}
    Protected oususaiu As String() = New String() {"Até mais senhor " + usu, "Espero o seu retorno senhor " + usu, "Até logo mais senhor " + usu}
    Protected ousuvoltou As String() = New String() {"Olá senhor " + usu, "Bem vindo de volta senhor " + usu, "Bom ter o senhor de volta senhor " + usu}
    Protected avisatempo As String() = New String() {"Certo senhor " + usu + " estou consultando a previsão do tempo agora", "Tudo bem estou verificando a previsão do tempo para o senhor", "Entendido senhor " + usu + " estou baixando a previsão de hoje"}
    Protected avisaAFK As String() = New String() {"Está ai senhor " + usu + "?", "Senhor " + usu + ", o senhor está ae ?", "Senhor ?"}
    Protected InternetInstavel As String() = New String() {"Senhor " + usu + " a internet está instavel", "Senhor " + usu + ", verifiquei que a internet está instavel", "Senhor " + usu + " a internet ficou instavel"}
    Protected InternetEnstavel As String() = New String() {"Senhor " + usu + " a internet está estavel de novo", "Senhor " + usu + ", a internet está normalizada", "Internet normalizada Senhor " + usu}
    Protected PegandoPrevisaoTempo As String() = New String() {"Certo senhor " + usu + ", estou consultando a previsão do tempo agora", "Entendido, estou verificando a previsão do tempo para o senhor", "Ok senhor " + usu + ", estou baixando a previsão de hoje"}
    Protected RecomendaQuente As String() = New String() {"Recomendo muita agua e protetor solar", "Recomendo colocar recipiente com água ou pano molhado nos ambientes, isso ajuda a amenizar a baixa umidade e seus efeitos, assim como passar pano úmido sobre os móveis.", "Recomendo evitar exercícios físicos em locais de poluição atmosférica acentuada, como em vias de alto trânsito de veículos"}
    Protected RecomendaFrio As String() = New String() {"Recomendo promover boa circulação de ar, principalmente durante a noite, não fechando completamente os aposentos mas evitando correntes de ar frio", "Recomendo não fazer exercício físico intenso ou ao ar livre e evite arrefecer com a roupa transpirada no corpo", "Recomendo que comer com mais frequência: encurte as horas entre as refeições"}
    Protected NegaPorInternet As String() = New String() {"Sinto muito, mas estou sem acesso a internet", "Não consigo, estou sem acesso a internet", "Não posso, sem internet"}

    Public Function AnwserWarning(ByVal operaçao As Integer)
        Dim Output = "Desculpe, não sei responder isso"

        If (operaçao = 1) Then
            Output = minimizadoav(GenerateNunber(minimizadoav.Length))
        ElseIf (operaçao = 2) Then
            Output = avisanetvoltou(GenerateNunber(avisanetvoltou.Length))
        ElseIf (operaçao = 3) Then
            Output = avisanetcaiu(GenerateNunber(avisanetcaiu.Length))
        ElseIf (operaçao = 4) Then
            Output = boamadrugada(GenerateNunber(boamadrugada.Length))
        ElseIf (operaçao = 5) Then
            Output = bomdiaavisa(GenerateNunber(bomdiaavisa.Length))
        ElseIf (operaçao = 6) Then
            Output = boatardeavisa(GenerateNunber(boatardeavisa.Length))
        ElseIf (operaçao = 7) Then
            Output = boanoiteavisa(GenerateNunber(boanoiteavisa.Length))
        ElseIf (operaçao = 8) Then
            Output = respondeavisa(GenerateNunber(respondeavisa.Length))
        ElseIf (operaçao = 9) Then
            Output = bloqueapcavisa(GenerateNunber(bloqueapcavisa.Length))
        ElseIf (operaçao = 10) Then
            Output = vaijogar(GenerateNunber(vaijogar.Length))
        ElseIf (operaçao = 11) Then
            Output = oususaiu(GenerateNunber(oususaiu.Length))
        ElseIf (operaçao = 12) Then
            Output = ousuvoltou(GenerateNunber(ousuvoltou.Length))
        ElseIf (operaçao = 13) Then
            Output = avisatempo(GenerateNunber(avisatempo.Length))
        ElseIf (operaçao = 14) Then
            Output = avisaAFK(GenerateNunber(avisaAFK.Length))
        ElseIf (operaçao = 15) Then
            Output = InternetInstavel(GenerateNunber(InternetInstavel.Length))
        ElseIf (operaçao = 16) Then
            Output = InternetEnstavel(GenerateNunber(InternetEnstavel.Length))
        ElseIf (operaçao = 17) Then
            Output = PegandoPrevisaoTempo(GenerateNunber(PegandoPrevisaoTempo.Length))
        ElseIf (operaçao = 18) Then
            Output = RecomendaQuente(GenerateNunber(RecomendaQuente.Length))
        ElseIf (operaçao = 19) Then
            Output = RecomendaFrio(GenerateNunber(RecomendaFrio.Length))
        ElseIf (operaçao = 20) Then
            Output = NegaPorInternet(GenerateNunber(NegaPorInternet.Length))
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
