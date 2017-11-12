Public Class Interpreter
    Dim Dict As New List(Of String)()

    Function VerifyText(ByVal text As String) As String
        If (Dict.Count > 0) Then

            Dim ThetExt() As String
            Dim Thereplace() As String
            Dim output As New List(Of String)()
            Dim result As String

            'quebra o texto por espaço
            ThetExt = text.Split(" ")

            'primeiro for para percorre o otexto
            For Each words In ThetExt

                Dim cacheoutput = words
                'for pra percorrer o dicionario e corrigir erros
                For Each line In Dict

                    'quebra as palarars do dicionario
                    Thereplace = line.Split(",")

                    'faz comparação de string
                    If (words.Equals(Thereplace(0)) And String.Compare(words, Thereplace(0)) = 0) Then
                        cacheoutput = Replace(words, Thereplace(0), Thereplace(1), 1, -1, Constants.vbTextCompare)
                    End If

                Next
                'salva o resultado em um array
                output.Add(cacheoutput)
            Next

            'junta as palavras divididas do array
            result = String.Join(" ", output)

            'retorna resultado
            Return result
        Else

            Return "Erro"
        End If

    End Function

    Sub BuildDict()
        'Como usar: primeiro palavra errada, segundo palavra certa
        Dict.Add("abri,abrir")
        Dict.Add("abir,abrir")
        Dict.Add("finaliza,finalizar")
        Dict.Add("finalisa,finalizar")
        Dict.Add("finalisar,finalizar")
        Dict.Add("finalizador,finalizar")
        Dict.Add("finalizado,finalizar")
    End Sub

End Class
