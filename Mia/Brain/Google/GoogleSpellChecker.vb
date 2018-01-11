Imports System
Imports System.Linq
Imports HtmlAgilityPack
Imports System.Collections.Generic
Imports System.Net
Imports System.Threading.Tasks

Namespace GoogleSpellChecker

    Enum GoogleLang
        Portuguese = 0
        English = 1
    End Enum

    Class GoogleSpellChecker

        Private serverLang As String = String.Empty

        Public Sub New(ByVal Language As GoogleLang)
            Select Case Language
                Case GoogleLang.English
                    serverLang = "com"
                Case GoogleLang.Portuguese
                    serverLang = "com.br"
            End Select
        End Sub

        Public Function CheckAsync(ByVal word As String) As Task(Of String)
            Return New TaskFactory(Of String)().StartNew(Function() Check(word))
        End Function

        Public Function Check(ByVal word As String) As String
            Dim Checkpotput As String = word
            Try
                Debug.Print("https://www.google.{0}/search?q={1}", serverLang, word.Replace(" ", "+"))
                Dim url As String = String.Format("https://www.google.{0}/search?q={1}", serverLang, word)
                Dim client As WebClient = New WebClient()
                Dim gHtml As String = client.DownloadString(url)
                Dim doc As HtmlDocument = New HtmlDocument()
                doc.LoadHtml(gHtml)
                client.Dispose()
                Dim Corretions As List(Of HtmlNode) = doc.DocumentNode.Descendants("a").Where(Function(x) x.Attributes IsNot Nothing AndAlso x.Attributes("class") IsNot Nothing AndAlso x.Attributes("class").Value.Contains("spell")).ToList()
                If Corretions.Count > 0 Then
                    Debug.Print(Corretions.ToString)
                    Checkpotput = Corretions.FirstOrDefault().InnerText
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            Return Checkpotput
        End Function
    End Class
End Namespace