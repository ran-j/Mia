Public Class InteractForm

    Dim Google As GoogleEngine
    Dim NoPendence As Boolean = True 'se tiver pendencias o valor vai ser falso
    Dim OldmousePosition As Integer = 0
    Dim AFKTimes As Integer = 0
    Dim MiaBr As Brain = Main.GetMiaBraind()

    Private Sub InteractForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Cria uma instancia da Minha Api do google
        Google = New GoogleEngine(GoogleEngine.GoogleLang.Portuguese, WebBrowser1)
        'Impedir de selecionar + C nos testos
        RichTextBox1.SelectionStart = RichTextBox1.TextLength
        RichTextBox1.ScrollToCaret()

        AFKDETECTORI.Enabled = True
    End Sub

    Private Sub ProcessText(text As String)
        'processa e responde o usuário na tela

        Dim VerifyText As String = MiaBr.RequestVerifyText(text)

        If (NoPendence And VerifyText.Contains("oi")) Then
            SetText(MiaBr.RequestConversation(1))

        ElseIf (NoPendence And VerifyText.Contains("emular ps1") Or VerifyText.Contains("emular jogo de ps1")) Then
            SetText("Selecione o jogo de PS1.")

            Me.OpenFileDialog1.Multiselect = False
            Me.OpenFileDialog1.FileName = "Selecionar o jogo"
            Me.OpenFileDialog1.InitialDirectory = "C:\"
            Me.OpenFileDialog1.Filter = "Jogos|*.bin;*.iso;*.cdz"
            Me.OpenFileDialog1.CheckFileExists = True
            Me.OpenFileDialog1.CheckPathExists = True

            Dim dr As DialogResult = Me.OpenFileDialog1.ShowDialog()

            If dr = System.Windows.Forms.DialogResult.OK Then
                MiaBr.RequestRunGame(Me.OpenFileDialog1.FileNames.First.ToString)
                Main.WindowState = FormWindowState.Minimized
            Else
                SetText("Operação cancelada.")
            End If

        ElseIf (NoPendence) Then
            'Caso o programa não saiba o mesmo pesquisa no google kkkkk
            Dim GoogleCheckText As String = Google.CheckWord(VerifyText)

            If (GoogleCheckText.Equals("0.")) Then 'Verifica se o texto entrante está correto

                Dim Output As String = DoGoogleQuery(VerifyText)

                If Not (Output.Equals("1.")) Then
                    SetText(Output)
                Else
                    Debug.Print("Texto correto")
                    SetText(MiaBr.RequestConversation(2))
                End If

            Else
                Dim Output As String = DoGoogleQuery(VerifyText)

                If Not (Output.Equals("1.")) Then
                    SetText(Output)
                Else
                    Debug.Print("Texto incorreto")
                    SetText(MiaBr.RequestConversation(2))
                End If

            End If
        End If

    End Sub

    Function DoGoogleQuery(Text As String) As String
        'se não estiver o google tenta corrigir
        Dim QUERY As String = "https://www.google.com.br" + "/search?q=" + Text.Replace(" ", "+")
        WebBrowser1.Navigate(QUERY)
        Google.WaitForPageLoad()
        Dim Output = Google.ResponsiveAnwser(WebBrowser1.Document)

        Debug.Print(QUERY)
        Debug.Print(Output)

        Return Output
    End Function


#Region "RichBox Control"

    Private Sub RichTextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles RichTextBox1.KeyDown
        'RichTextBox1.Lines.Length - 1
        If (e.KeyCode = Keys.Enter) And Not RichTextBox1.ReadOnly Then

            Dim line As Integer = RichTextBox1.GetLineFromCharIndex(RichTextBox1.SelectionStart)

            Dim Texto As String = RichTextBox1.Lines(RichTextBox1.Lines.Length - 1).Substring(6)
            Dim TextoVeiry As String = RichTextBox1.Lines(RichTextBox1.Lines.Length - 1).Substring(6).Replace(" ", "")

            If Not (TextoVeiry <> "") Then
                e.Handled = True
            Else

                RichTextBox1.ReadOnly = True

                Dim col As Integer = RichTextBox1.SelectionStart - RichTextBox1.GetFirstCharIndexFromLine(line)

                ProcessText(Texto)

                On Error Resume Next
                RichTextBox1.AppendText(vbNewLine)
                Dim caretPosition = RichTextBox1.SelectionStart
                RichTextBox1.AppendText("Voce:>")
                RichTextBox1.Select(caretPosition, 0)
                RichTextBox1.ScrollToCaret()

                If (col = 0) Then
                    RichTextBox1.SelectionStart = RichTextBox1.TextLength
                    RichTextBox1.ScrollToCaret()
                End If

            End If

        ElseIf (e.KeyCode = Keys.Back) Or (e.KeyCode = Keys.Left) Then

            Dim line As Integer = RichTextBox1.GetLineFromCharIndex(RichTextBox1.SelectionStart)
            Dim col As Integer = RichTextBox1.SelectionStart - RichTextBox1.GetFirstCharIndexFromLine(line)

            If (col = 0 Or col = 6) Then
                e.Handled = True
            End If

        ElseIf (e.KeyCode = Keys.Up) Then
            e.Handled = True
        End If
        'Dim s As String = e.KeyCode.ToString
        'MsgBox(s)
    End Sub

    Private Sub RichTextBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles RichTextBox1.MouseDown
        RichTextBox1.[Select](RichTextBox1.Text.Length, 0)
    End Sub
    Private Sub RichTextBox1_SelectionChanged(sender As Object, e As EventArgs) Handles RichTextBox1.SelectionChanged
        RichTextBox1.SelectionLength = 0
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged
        Dim line As Integer = RichTextBox1.GetLineFromCharIndex(RichTextBox1.SelectionStart)
        Dim col As Integer = RichTextBox1.SelectionStart - RichTextBox1.GetFirstCharIndexFromLine(line)

        If (col = 0) Then
            RichTextBox1.SelectionStart = RichTextBox1.TextLength
            RichTextBox1.ScrollToCaret()
        End If
    End Sub

    Sub SetText(Text As String)
        Dim line As Integer = RichTextBox1.GetLineFromCharIndex(RichTextBox1.SelectionStart)
        Dim col As Integer = RichTextBox1.SelectionStart - RichTextBox1.GetFirstCharIndexFromLine(line)

        If (col > 0) Then
            RichTextBox1.AppendText(vbNewLine)
            RichTextBox1.AppendText(vbNewLine & "Mia:>" + Text)
        Else
            RichTextBox1.AppendText("Mia:>" + Text & vbNewLine)
            RichTextBox1.AppendText(vbNewLine & "Voce:>")
        End If

        RichTextBox1.ReadOnly = False

    End Sub

    Private Sub RichTextBox1_MouseClick(sender As Object, e As MouseEventArgs) Handles RichTextBox1.MouseClick
        'Impedir de selecionar + C nos testos
        RichTextBox1.SelectionStart = RichTextBox1.TextLength
        RichTextBox1.ScrollToCaret()
    End Sub

    Sub ClearRichTextBox1()
        'limpa o richtextbox
        RichTextBox1.Clear()
        RichTextBox1.Update()
    End Sub


#End Region

    Private Sub AFKDETECTORI_Tick(sender As Object, e As EventArgs) Handles AFKDETECTORI.Tick
        If (Me.WindowState = FormWindowState.Normal) Then
            If (OldmousePosition = MousePosition.X) Then
                OldmousePosition = MousePosition.X 'pegar movimento
                Debug.Print("Painel de interação está aberto")
                Debug.Print("Posiçao do mouse: " + OldmousePosition.ToString)
                Dim avisotext As String = MiaBr.RequestWarnings(14)
                Main.Voz.SpeechMoreThanOnce(avisotext)
                SetText(avisotext)

                If (AFKTimes = 3) Then
                    Debug.Print("Usuário AFK")
                    AFKTimes = 0
                    Me.Close()
                End If

                AFKTimes = AFKTimes + 1
            Else
                OldmousePosition = MousePosition.X 'pegar movimento
                Debug.Print("Posiçao do mouse: " + OldmousePosition.ToString)
            End If
        End If
    End Sub

    Private Sub InteractForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Main.AFKDetector.Enabled = True
    End Sub

End Class