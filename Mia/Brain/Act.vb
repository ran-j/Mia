Imports System.Net
Imports System.Security.Permissions

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
