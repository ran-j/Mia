Public Class Main
    Public Shared MiaBrain As New Process 'Cria instancia do controlador principal do cerebro

    Dim arguments As String() = Environment.GetCommandLineArgs() 'pega os argumentos
    Public OldmousePosition As Integer = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OldmousePosition = MousePosition.X 'pega a posição do mouse

        AddHandler MiaBrain.LoadCompleted, AddressOf LoadC 'adiciona evento de carregamento

        MiaBrain.Init1() 'Starta o processamento
    End Sub

    Private Sub AFKDetector_Tick(sender As Object, e As EventArgs) Handles AFKDetector.Tick
        If (OldmousePosition = MousePosition.X) Then 'verificar se o mouse esta no mesmo lugar ou ele se movimento.
            MsgBox("Hey esta ai ?")
            OldmousePosition = 0
        Else
            OldmousePosition = MousePosition.X 'pegar movimento
        End If
    End Sub

    Private Sub LoadC()
        'evento de quando termina de carregar os arquivos
        AFKDetector.Enabled = True
        Debug.Print("Carregamento completo")
    End Sub



End Class
