Imports System.Net

Public Class Act

    Function GetWeather()
        'Retorna temperatura
        Using webClient As WebClient = New System.Net.WebClient()
            Dim n As New WebClient()
            Dim json = n.DownloadString("https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='campos dos goytacazes, rj')&format=json")
            Dim valueOriginal As String = json.ToString()

            Dim JsonRecebido As String()

            JsonRecebido = valueOriginal.Split(",")
            Dim umidade As String = "Vazio"
            Dim temperatura As String = "Vazio"
            Dim ceu As String = "Vazio"

            For Each Linhas In JsonRecebido
                Debug.WriteLine(Linhas.ToString())

                If (Linhas.ToString.StartsWith("""atmosphere")) Then

                    umidade = Linhas.Substring(26, 2)
                End If

                If (Linhas.ToString.StartsWith("""temp"":")) Then
                    Dim CelsiusValue = (5 / 9) * (Convert.ToInt32(Linhas.Substring(8, 2)) - 32)
                    CelsiusValue = Math.Round(CelsiusValue, 0)

                    temperatura = CelsiusValue.ToString
                End If

                If (Linhas.ToString.StartsWith("""text"":")) Then
                    ceu = Linhas.Substring(8, 6)
                    Exit For
                End If

            Next
            Return umidade + "," + temperatura + "," + ceu
        End Using
    End Function




End Class
