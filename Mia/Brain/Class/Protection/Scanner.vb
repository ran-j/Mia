Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq

Public Class Scanner
    'sacenar e monitorar o pc
    Dim Items As New Dictionary(Of WebClient, ListViewItem)
    Dim ControlAt As New Dictionary(Of WebClient, Label)
    Dim ResourceAt As New Dictionary(Of WebClient, String)

    Dim APIKEY As String
    Private File As String

    Public Event ScanCompleted(Results As List(Of String))

    Sub SetFile(Value As String)
        Me.file = Value
    End Sub

    Sub New(RAPIKEY As String)
        APIKEY = RAPIKEY
    End Sub


    Public Sub ScanFile()
        If (File.Equals("")) Then
            RaiseEvent ScanCompleted(New List(Of String))
            Exit Sub
        Else
            Try
                Using W As New WebClient
                    AddHandler W.UploadProgressChanged, AddressOf CARREGANDO
                    AddHandler W.UploadFileCompleted, AddressOf CONCLUIDO
                    W.QueryString.Add("apikey", APIKEY)
                    W.UploadFileTaskAsync(New Uri("https://www.virustotal.com/vtapi/v2/file/scan"), File)

                    Dim NewItem As New ListViewItem
                    Dim NewLB As New Label

                    Items.Add(W, NewItem)
                    ControlAt.Add(W, NewLB)
                End Using
            Catch ex As Exception
                RaiseEvent ScanCompleted(New List(Of String))
                Exit Sub
            End Try
        End If
    End Sub

    Private Sub GetReport(ByVal Resource As String, ByVal Item As ListViewItem, ByVal Labell As Label)
        Try
            Using W As New WebClient
                AddHandler W.DownloadProgressChanged, AddressOf CARREGANDO_GET
                AddHandler W.DownloadDataCompleted, AddressOf CONCLUIDO_GET
                W.QueryString.Add("resource", Resource)
                W.QueryString.Add("apikey", APIKEY)
                W.DownloadDataTaskAsync(New Uri("https://www.virustotal.com/vtapi/v2/file/report"))
                Items.Add(W, Item)
                ControlAt.Add(W, Labell)
                ResourceAt.Add(W, Resource)
            End Using
        Catch ex As Exception
            RaiseEvent ScanCompleted(New List(Of String))
            Exit Sub
        End Try
    End Sub

    Private Sub CONCLUIDO_GET(ByVal Sender As Object, ByVal e As DownloadDataCompletedEventArgs)
        If e.Result IsNot Nothing Then

            Dim Saida As New List(Of String)()
            Dim ResultJson As String = Encoding.Default.GetString(e.Result)
            If ResultJson.Contains("""response_code"": 1,") = True Then

                Debug.Print("Completo")
                Dim Result As New List(Of Object)

                Dim ser As JObject = JObject.Parse(ResultJson)
                Dim data As List(Of JToken) = ser("scans").ToList
                For Each item As JProperty In data

                    item.CreateReader()


                    If item.Value("result").ToString = "" Then
                        Debug.Print("nada")
                    Else
                        Saida.Add(item.Value("result").ToString)
                    End If

                    Debug.Print(item.Value("version").ToString)

                    Result.Add(New Object() {item.Name, item.Value("result").ToString, item.Value("version").ToString})
                Next
                Items(Sender).Tag = Result

                If (Saida.Count = 0) Then
                    Saida.Add("Clear")
                End If

                RaiseEvent ScanCompleted(Saida)

            Else
                Application.DoEvents()
                GetReport(ResourceAt(Sender), Items(Sender), ControlAt(Sender))
            End If
        End If
    End Sub

    Private Sub CONCLUIDO(ByVal Sender As Object, ByVal e As UploadFileCompletedEventArgs)
        If e.Result IsNot Nothing Then
            Dim ResultJson As String = Encoding.Default.GetString(e.Result)
            Dim MTA As Match = Regex.Match(ResultJson, "resource"": ""([0-9a-y]*)""")
            Debug.Print(MTA.Groups(1).Value)
            GetReport(MTA.Groups(1).Value, Items(Sender), ControlAt(Sender))
        End If
    End Sub

    Private Sub CARREGANDO(ByVal Sender As Object, ByVal e As UploadProgressChangedEventArgs)
        On Error Resume Next
        Debug.Print("Realizando Upload de arquivos")
    End Sub

    Private Sub CARREGANDO_GET(ByVal Sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        On Error Resume Next
        Debug.Print("Aguardando resposta")
    End Sub

End Class
