Imports System.IO
Imports System.Net

Public Class GoogleEngine

    Enum GoogleLang
        Portuguese = 0
        English = 1
    End Enum

    Private serverLang As String = String.Empty

    Private Shared Spell As GoogleSpellChecker.GoogleSpellChecker

    Dim WebBrowser1 As WebBrowser
    Private Property pageready As Boolean = False

    Sub New(ByVal Language As GoogleLang, Web As WebBrowser)
        WebBrowser1 = Web
        Select Case Language
            Case GoogleLang.English
                serverLang = "com"
            Case GoogleLang.Portuguese
                serverLang = "com.br"
        End Select

        Spell = New GoogleSpellChecker.GoogleSpellChecker(Language)
    End Sub

    Public Function ResponsiveAnwser(Documento As HtmlDocument)
        'Pergunta ao google e ele responde
        Dim PageElement As HtmlElementCollection = Documento.GetElementsByTagName("div")
        For Each CurElement As HtmlElement In PageElement
            If (CurElement.GetAttribute("className") = "_XWk") Or (CurElement.GetAttribute("className") = "vk_gy vk_sh whenis") Or (CurElement.GetAttribute("className") = "_oDd") Or (CurElement.GetAttribute("className") = "_Jig") Or (CurElement.GetAttribute("className") = "_cgc kno-fb-ctx") Then
                Return CurElement.GetAttribute("innertext")
            End If
        Next
        Return "1."
    End Function

    Function CheckWord(Word As String) As String
        'Corretor de palavras do google
        Dim correctWord As String = Spell.Check(Word.Replace(" ", "+"))

        If Not correctWord.Equals(Word) Then
            Return correctWord
        Else
            Return "0."
        End If
    End Function


#Region "Page Loading Functions"
    Public Sub WaitForPageLoad()
        AddHandler WebBrowser1.DocumentCompleted, New WebBrowserDocumentCompletedEventHandler(AddressOf PageWaiter)
        While Not pageready
            Application.DoEvents()
        End While
        pageready = False
    End Sub

    Private Sub PageWaiter(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
        If WebBrowser1.ReadyState = WebBrowserReadyState.Complete Then
            pageready = True
            RemoveHandler WebBrowser1.DocumentCompleted, New WebBrowserDocumentCompletedEventHandler(AddressOf PageWaiter)
        End If
    End Sub

#End Region
End Class


