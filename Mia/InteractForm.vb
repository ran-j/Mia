Public Class InteractForm

    Private Sub InteractForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RichTextBox1.SelectionStart = RichTextBox1.TextLength
        RichTextBox1.ScrollToCaret()
    End Sub

    Private Sub ProcessText(text As String)
        'processa e responde o usuário na tela
        Dim VerifyText As String = MiaBr.RequestVerifyText(text)

        If (VerifyText.Contains("oi")) Then
            RichTextBox1.AppendText(vbNewLine)
            RichTextBox1.AppendText(vbNewLine & "Mia:>" + MiaBr.RequestConversation(1))
        End If

    End Sub

#Region "RichBox Control"

    Dim MiaBr As Brain = Main.GetMiaBraind()

    Private Sub RichTextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles RichTextBox1.KeyDown
        'RichTextBox1.Lines.Length - 1
        If (e.KeyCode = Keys.Enter) Then

            Dim line As Integer = RichTextBox1.GetLineFromCharIndex(RichTextBox1.SelectionStart)
            Try
                Dim Texto As String = RichTextBox1.Lines(line).ToString.Substring(6).Replace(" ", "")

                If Not (Texto <> "") Then
                    e.Handled = True
                Else
                    Dim col As Integer = RichTextBox1.SelectionStart - RichTextBox1.GetFirstCharIndexFromLine(line)

                    ProcessText(Texto)

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
            Catch ex As Exception
                e.Handled = True
            End Try

        ElseIf (e.KeyCode = Keys.Back) Or (e.KeyCode = Keys.Left) Then

            Dim line As Integer = RichTextBox1.GetLineFromCharIndex(RichTextBox1.SelectionStart)
            Dim col As Integer = RichTextBox1.SelectionStart - RichTextBox1.GetFirstCharIndexFromLine(line)

            If (col = 0 Or col = 6) Then
                e.Handled = True
            End If

        ElseIf (e.KeyCode = Keys.UP) Then
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
#End Region

End Class