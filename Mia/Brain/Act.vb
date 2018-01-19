Imports System.Net
Imports System.Security.Permissions
Imports Newtonsoft.Json

<PermissionSet(SecurityAction.Demand, Name:="FullTrust")>
<Runtime.InteropServices.ComVisibleAttribute(True)>
Public Class Act
    Dim WithEvents WebBrowser2 As New WebBrowser

    Private Sub WebBrowser2_DocumentCompleted(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser2.DocumentCompleted
        Debug.Print("Pagina carregada")
    End Sub

    Dim searchText As String

    Sub New()
        'Configura o web Browser
        Me.WebBrowser2.ObjectForScripting = Me
        Me.WebBrowser2.ScriptErrorsSuppressed = True
        Me.WebBrowser2.Dock = DockStyle.Fill
        Me.WebBrowser2.ScrollBarsEnabled = True
    End Sub

    Function GetWeather() As String
        'Retorna temperatura
        Try
            Using wc As New System.Net.WebClient
                Dim jsonString As String = wc.DownloadString("https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='campos dos goytacazes, rj')&format=json")
                Dim weatherObject As Weather = JsonConvert.DeserializeObject(Of Weather)(jsonString)

                Dim CelsiusValue = (5 / 9) * (Convert.ToInt32(weatherObject.query.results.channel.item.condition.temp) - 32)
                CelsiusValue = Math.Round(CelsiusValue, 0)

                Return CelsiusValue.ToString + "," + weatherObject.query.results.channel.item.condition.text.Replace("Mostly Cloudy", "Parcialmente nublado").Replace("Partly Cloudy", "parcialmente nublado")

            End Using
        Catch ex As Exception
            My.Settings.Erros = My.Settings.Erros + 1
            Return "Vazio"
        End Try

    End Function

    Sub SearchGoogle(Search As String)
        Dim query As String = "https://www.google.com.br/search?q=" + Search.Replace(" ", "+")

        Dim WebForm As New Form With {
            .Height = 800,
            .Width = 1000,
            .Text = "Pesquisa Web"
        }

        Me.WebBrowser2.Height = WebForm.Height
        Me.WebBrowser2.Width = WebForm.Width

        WebForm.Controls.Add(WebBrowser2)

        WebBrowser2.Navigate(query)

        WebForm.Show()

    End Sub

    Function Clickbyid(ByVal id)
        'Clica em um botão
        On Error Resume Next
        If WebBrowser2.Document.GetElementById(id) IsNot Nothing Then
            Dim Headers As String = "" 'insert headers if you want to

            WebBrowser2.Navigate("javascript:document.getElementById('" & id & "').click();", "_self", Nothing, Headers)

            'This keeps the function active until the browser has finished loading
            Do While WebBrowser2.ReadyState <> WebBrowserReadyState.Complete
                Application.DoEvents()
            Loop
            Return 1
        Else

            MessageBox.Show("Could not find link by id" & id)
            Return Nothing
        End If

    End Function



End Class
