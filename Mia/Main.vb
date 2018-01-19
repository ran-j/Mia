Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.Win32

Public Class Main

#Region "Variaveis"
    Dim MiaBrain As Brain 'Cria instancia do controlador principal do cerebro

    Private WithEvents Net As MyNetwork

    Dim scan As Scanner

    Public Voz As New Voice 'Inicia sistema de voz

    Dim PS1EMULAITOR As PS1

    Dim Arguments As String() = Environment.GetCommandLineArgs() 'pega os argumentos

    Dim Google As GoogleEngine

    Dim NoPendence As Boolean = True 'se tiver pendencias o valor vai ser falso

    Public OldmousePosition As Integer = 0

    Dim Recog As RecognizerEngine

    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    Public Const HT_CAPTION As Integer = &H2

    Dim MouseclickUI As Integer = 0

    <DllImportAttribute("user32.dll")>
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function

    <DllImportAttribute("user32.dll")>
    Public Shared Function ReleaseCapture() As Boolean
    End Function

    'Used to detected if any of the messages are any of these constants values.
    Private Const WM_DEVICECHANGE As Integer = &H219
    Private Const DBT_DEVICEARRIVAL As Integer = &H8000
    Private Const DBT_DEVICEREMOVECOMPLETE As Integer = &H8004
    Private Const DBT_DEVTYP_VOLUME As Integer = &H2  '
    '
    'Get the information about the detected volume.
    Private Structure DEV_BROADCAST_VOLUME
        Dim Dbcv_Size As Integer
        Dim Dbcv_Devicetype As Integer
        Dim Dbcv_Reserved As Integer
        Dim Dbcv_Unitmask As Integer
        Dim Dbcv_Flags As Short
    End Structure
    'Posiçao do form
    Dim FormPosition As Point
    'Para saber se o usuário está jogando
    Dim GameOpen = 0

    Dim HasNet As Integer = 0
    Dim NetStable As Integer = 0
    Dim Listen As Integer = 1

#Region " win32 "
    Private Declare Auto Function FindWindow Lib "user32" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Private Declare Auto Function SendMessage Lib "user32" (ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    Private Declare Auto Function SetForegroundWindow Lib "user32" (ByVal hWnd As IntPtr) As Boolean
    Private Declare Auto Function keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer) As Boolean
    Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
    Private Declare Auto Function GetWindowText Lib "user32" (ByVal hwnd As IntPtr, ByVal lpString As String, ByVal cch As IntPtr) As IntPtr
    Private Declare Auto Function SetWindowText Lib "user32" (ByVal hwnd As IntPtr, ByVal lpString As String) As Boolean
    Private Declare Auto Function EnumChildWindows Lib "user32" (ByVal hWndParent As Long, ByVal lpEnumFunc As Long, ByVal lParam As Long) As Long
#End Region

#End Region

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            'So deixa abrir uma aplicação
            Dim procs() As Process = Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName)
            If procs.Length > 1 Then
                Me.Visible = False
                MsgBox("A Aplicação já está sendo executada")
                Debug.Print("Ja está aberta")
                CloseForm.Enabled = True
            Else
                OldmousePosition = MousePosition.X 'pega a posição do mouse

                'Tooltip com saldaçao
                ToolTip.SetToolTip(Me.UI, "Olá")

                MiaBrain = New Brain

                Google = New GoogleEngine(GoogleEngine.GoogleLang.Portuguese, WebBrowser1) 'Cria uma instancia da Minha Api do google
                Net = MiaBrain.RequestIstanceOfNetClass()
                scan = MiaBrain.RequestIstanceOfScanClass()
                PS1EMULAITOR = MiaBrain.RequestIstanceOfPS1Emu()

                Try
                    Recog = New RecognizerEngine() 'Carrega os comandos de voz

                    AddHandler Recog.TextRecognized, AddressOf SpeechText 'reconhece um comando de voz
                Catch ex As Exception
                    Debug.Print(ex.Message)
                    Throwalert("Erro ao iniciar o modulo de voz")
                End Try

                AddHandler MiaBrain.LoadCompleted, AddressOf LoadC 'adiciona evento de carregamento
                AddHandler scan.ScanCompleted, AddressOf ScanCompleted 'adiciona evento de scan de virus
                AddHandler Net.IpsCaptured, AddressOf IPsConected 'retorna os ips capturados
                AddHandler PS1EMULAITOR.ErroEmulator, AddressOf ErroEmu 'erro de emulador de ps1

                AddHandler Voz.VoiceSpeakImprogres, AddressOf VoiceInprogress 'evento de quando o programa esta falando  
                AddHandler Voz.VoiceSpeakCompleted, AddressOf VoiceCompleted 'evento de quando o programa terminou de falar

                MiaBrain.Init1() 'Starta o processamento

                'Quando o sistema bloquear
                AddHandler SystemEvents.SessionSwitch, AddressOf CheckLockUnlock

            End If
        Catch ex As Exception
            Debug.Print("Erro: " + ex.Message)
            CloseForm.Enabled = True
        End Try
    End Sub

    Private Sub SpeechText(Text As String)
        'Recebe o texto da escrita
        If (Listen = 1) Then
            ProcessText(Text)
        Else
            Debug.Print("Reconhecido testo " + Text)
        End If
    End Sub

    Private Sub LoadC()
        'evento de quando termina de carregar os arquivos
        AFKDetector.Enabled = True
        Debug.Print("Carregamento completo")

        'Comprimento inicial
        Select Case TimeOfDay

            Case "00:00" To "5:30"
                Debug.Print("Madrugrada")
                Voz.SpeechMoreThanOnce(MiaBrain.RequestWarnings(4))
            Case "00:00 " To "11:59"
                Debug.Print("Manhã")
                Voz.SpeechMoreThanOnce(MiaBrain.RequestWarnings(5))
            Case "12:00" To "17:59"
                Debug.Print("Tarde")
                Voz.SpeechMoreThanOnce(MiaBrain.RequestWarnings(6))
            Case "18:00" To "23:59"
                Debug.Print("Noite")
                Voz.SpeechMoreThanOnce(MiaBrain.RequestWarnings(7))

        End Select

        'Iniciar o monitoramento da net
        Net.StartMonitoring() 'Net.StopMonitoring
    End Sub

    Public Sub ProcessText(text As String)
        'processa e responde o usuário na tela
        On Error Resume Next

        Dim VerifyText As String = MiaBrain.RequestVerifyText(text)

        If (NoPendence And VerifyText.Contains("oi")) Then
            InteractForm.SetText(MiaBrain.RequestConversation(1))

        ElseIf (NoPendence And VerifyText.Contains("emular ps1") Or VerifyText.Contains("emular jogo de ps1") Or VerifyText.Contains("emular jogo de play station")) Then
            InteractForm.SetText("Selecione o jogo de PS1.")

            Me.OpenFileDialog1.Multiselect = False
            Me.OpenFileDialog1.FileName = "Selecione o jogo"
            Me.OpenFileDialog1.InitialDirectory = "C:\"
            Me.OpenFileDialog1.Filter = "Jogo|*.bin;*.iso;*.cdz"
            Me.OpenFileDialog1.CheckFileExists = True
            Me.OpenFileDialog1.CheckPathExists = True

            Dim dr As DialogResult = Me.OpenFileDialog1.ShowDialog()

            If dr = System.Windows.Forms.DialogResult.OK Then
                MiaBrain.RequestRunGame(Me.OpenFileDialog1.FileNames.First.ToString)

                InteractForm.SetText("Abrindo emulador.")

                Me.WindowState = FormWindowState.Minimized
            Else
                InteractForm.SetText("Operação cancelada.")
            End If

        ElseIf (NoPendence And VerifyText.Contains("previão do tempo")) Then

            InteractForm.SetText(MiaBrain.RequestWarnings(17))

            Dim Temp As String() = MiaBrain.RequestWeather().Split(",")

            Dim graus As String = Temp(0)

            If (graus > 28) Then
                InteractForm.SetText("Hoje está muito quente, a temperatura é de " + graus + " graus, e o céu está " + Temp(1))

                InteractForm.SetText(MiaBrain.RequestWarnings(18))

            ElseIf (graus < 18) Then
                InteractForm.SetText("Hoje está muito frio, a temperatura é de " + graus + " graus, e o céu está " + Temp(1))

                InteractForm.SetText(MiaBrain.RequestWarnings(19))
            Else
                InteractForm.SetText("Hoje está com um clima agradavel, a temperatura é de " + graus + " graus, e o céu está " + Temp(1))

                InteractForm.SetText(MiaBrain.RequestWarnings(18))
            End If

        ElseIf (NoPendence) Then
            'Caso o programa não saiba o mesmo pesquisa no google kkkkk
            Dim GoogleCheckText As String = Google.CheckWord(VerifyText)

            If (GoogleCheckText.Equals("0.")) Then 'Verifica se o texto entrante está correto

                Dim Output As String = DoGoogleQuery(VerifyText)

                If Not (Output.Equals("1.")) Then
                    InteractForm.SetText(Output)
                Else
                    Debug.Print("Texto correto")
                    InteractForm.SetText(MiaBrain.RequestConversation(2))
                End If

            Else
                Dim Output As String = DoGoogleQuery(GoogleCheckText)

                If Not (Output.Equals("1.")) Then
                    InteractForm.SetText(Output)
                Else
                    Debug.Print("Texto incorreto")
                    InteractForm.SetText(MiaBrain.RequestConversation(2))
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


#Region "Events"
    Sub ScanCompleted(Results As List(Of String))
        'Scan de virus completo
        MsgBox(Results.Count.ToString)

        For Each result In Results
            Debug.Print(result)
        Next
    End Sub

    Private Sub ErroEmu(text As String)
        'mostra o erro no alerta se o mesmo for por falta de arquivos do programa
        If (text.Equals("Pasta do emulador não encontrada")) Then
            Throwalert("Pasta do emulador não encontrada")
        Else
            MsgBox(text, MsgBoxStyle.Critical)
        End If

    End Sub

    Private Sub VoiceCompleted()
        'libera para escutar de novo
        Debug.Print("Programa terminou de falar")
        Listen = 1
    End Sub

    Private Sub VoiceInprogress()
        'tarva a escuta enquanto o programa fala
        Debug.Print("Programa falando")
        Listen = 0
    End Sub

#End Region

#Region "Net"

    Private Function NetSpeed()
        Dim resp As Integer = MiaBrain.RequestNetSpeed()

        If (resp = 0) Then
            Return "ótima"
        ElseIf (resp = 1) Then
            Return "razoavel"
        ElseIf (resp = 2) Then
            Return "bem ruim"
        ElseIf (resp = 3) Then
            Return "muito ruim"
        Else
            Return "absurdamente ruim"
        End If

    End Function

    Delegate Sub AddToList1Callback(ByVal ConnectionState As InternetConnectionState, ByVal IsStable As Boolean)
    Delegate Sub AddToList2Callback(ByVal ConnectionState As InternetConnectionState, ByVal IsStable As Boolean)

    Sub AddToList1(ByVal ConnectionState As InternetConnectionState, ByVal IsStable As Boolean)
        If Me.InvokeRequired = True Then
            Dim d As New AddToList1Callback(AddressOf AddToList1)
            Me.Invoke(d, ConnectionState, IsStable)
        Else
            Debug.Print(Now & " - State: " & ConnectionState.ToString())

            If Not (ConnectionState.ToString().Equals("Connected")) Then
                HasNet = 1
                Dim avisos As String = MiaBrain.RequestWarnings(3)
                Voz.SpeechMoreThanOnce(avisos)
                'alerta no programa
                MiaBrain.SetAlertText(avisos + " em " + Now)
                ShowWarning()
                'alerta popup
                Dim TH As Thread = New Thread(AddressOf MiaBrain.ShowCustonsNotification)
                TH.SetApartmentState(ApartmentState.STA)
                TH.Start(New Object() {"Atenção !", FrmNotification.Icons.Error, "Internet Caiu", "Sem conexão de internet"})
            Else
                If (HasNet = 1) Then
                    HasNet = 0
                    Voz.SpeechMoreThanOnce(MiaBrain.RequestWarnings(2))
                    Dim TH As Thread = New Thread(AddressOf MiaBrain.ShowCustonsNotification)
                    TH.SetApartmentState(ApartmentState.STA)
                    TH.Start(New Object() {"Atenção !", FrmNotification.Icons.Clean, "Internet Retornou", "Com conexão de internet"})
                End If
            End If
        End If

    End Sub

    Sub AddToList2(ByVal ConnectionState As InternetConnectionState, ByVal IsStable As Boolean)
        If Me.InvokeRequired = True Then
            Dim d As New AddToList2Callback(AddressOf AddToList2)
            Me.Invoke(d, ConnectionState, IsStable)
        Else
            Debug.Print(Now & " - Connection Stable: " & IsStable)
            Debug.Print(Now & " - Connection Stable: " & IsStable)

            If Not (IsStable) Then
                If (NetStable = 0) Then
                    NetStable = 1
                    Dim avisos As String = MiaBrain.RequestWarnings(15)
                    Voz.SpeechMoreThanOnce(avisos)
                    'alerta no programa
                    MiaBrain.SetAlertText(avisos + " em " + Now)
                    ShowWarning()
                    'alerta popup
                    Dim TH As Thread = New Thread(AddressOf MiaBrain.ShowCustonsNotification)
                    TH.SetApartmentState(ApartmentState.STA)
                    TH.Start(New Object() {"Atenção !", FrmNotification.Icons.Error, "Internet Ocilando", "Detectado ocilação de internet"})
                End If
            Else
                If (NetStable = 1) Then
                    NetStable = 0
                    Voz.SpeechMoreThanOnce(MiaBrain.RequestWarnings(16))
                    Dim TH As Thread = New Thread(AddressOf MiaBrain.ShowCustonsNotification)
                    TH.SetApartmentState(ApartmentState.STA)
                    TH.Start(New Object() {"Atenção !", FrmNotification.Icons.Clean, "Internet Normalizada", "Internet estabilizada"})
                End If
            End If
        End If
    End Sub

    Private Sub Conn_InternetConnectionStableChanged(IsStable As Boolean, ConnectionState As InternetConnectionState) Handles Net.InternetConnectionStableChanged
        AddToList2(ConnectionState, IsStable)
    End Sub

    Private Sub Conn_InternetConnectionStateChanged(ConnectionState As InternetConnectionState) Handles Net.InternetConnectionStateChanged
        AddToList1(ConnectionState, False)
    End Sub

    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        NotifyIcon.Visible = False
        NotifyIcon.Dispose()
    End Sub

    Private Sub IPsConected(IP As List(Of String))
        MsgBox(IP.Count.ToString)
    End Sub

#End Region

#Region " Form Control "

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown, UI.MouseDown
        'Mover o programa com o click do mouse
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)

            If Not (FormPosition.X <> Me.Location.X Or FormPosition.Y <> Me.Location.Y) Then
                If (MouseclickUI = 1) Then
                    MouseclickUI = 0
                    AFKDetector.Enabled = False
                    InteractForm.Show()
                Else
                    MouseclickUI = MouseclickUI + 1
                End If
            Else
                Debug.Print("Só mudou de posicao")
                FormPosition = Me.Location
            End If
        End If
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        'Deixar transparente
        Me.BackColor = Color.FromArgb(255, 255, 255)
        'Spaw position
        Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - 280, Screen.PrimaryScreen.WorkingArea.Height - 270)

        'Carrega a posiçao do form
        FormPosition = Me.Location
    End Sub

    Private Sub UI_DoubleClick(sender As Object, e As EventArgs) Handles UI.DoubleClick
        'Minimizar com click na foto
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub UI_MouseClick(sender As Object, e As MouseEventArgs) Handles UI.MouseClick
        'Mostra o botão para abrir o menu
        If (Config.Visible) Then
            Config.Visible = False
            HideUIicons.Enabled = False
        Else
            HideUIicons.Enabled = True
            Config.Visible = True
        End If
    End Sub

    Private Sub Config_Click(sender As Object, e As EventArgs) Handles Config.Click
        'Oculta o botão para abrir o menu
        HideUIicons.Enabled = False
        Config.Visible = False
    End Sub

    Private Sub Alert_Click(sender As Object, e As EventArgs) Handles Alert.Click
        'Dispara o alerta salvo
        Dim text = MiaBrain.GetAlertText()
        'pega sempre o primeiro alerta
        MsgBox(text(0), MsgBoxStyle.Exclamation)

        MiaBrain.ClearAlertTextTextbyName(text(0))

        If (MiaBrain.GetArrayTextSize() = 0) Then
            Alert.Visible = False
        End If

    End Sub

    Private Sub AFKDetector_Tick(sender As Object, e As EventArgs) Handles AFKDetector.Tick
        If (Me.WindowState = FormWindowState.Normal) Then

            If (OldmousePosition = MousePosition.X) Then 'verificar se o mouse esta no mesmo lugar ou ele se movimento.

                AFKDetector.Enabled = False

                OldmousePosition = 0

                Me.WindowState = FormWindowState.Minimized

                Debug.Print("Usuário AFK")
            Else
                OldmousePosition = MousePosition.X 'pegar movimento
                Debug.Print("Posiçao do mouse: " + OldmousePosition.ToString)
            End If

        End If
    End Sub

    Private Sub Main_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If (Me.WindowState = FormWindowState.Minimized) Then

            'FEcha o form de interação 
            InteractForm.Close()


            'Cancela oque o programa esta falando
            Voz.CancelSpeeck()

            'Desabilita o AFK detector
            AFKDetector.Enabled = False

            'Ocultar o programa
            Me.Hide()

            If (GameOpen = 0) Then
                'Solta notificação
                NotifyIcon.ShowBalloonTip("5000", "Aviso", MiaBrain.RequestWarnings(1), ToolTipIcon.Info)
            End If

            'Troca o contexte menu
            NotifyIcon.ContextMenuStrip = CMSI1

            Debug.Print("Minimizou")
        ElseIf (Me.WindowState = FormWindowState.Normal) Then
            'Ativa AFK Detector
            AFKDetector.Enabled = True

            'Mostrar o programa
            Me.Show()

            'Troca o contexte menu
            NotifyIcon.ContextMenuStrip = CMSVazio

            Debug.Print("Maximizou")
        End If
    End Sub

    Private Sub NotifyIcon_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon.MouseDoubleClick
        'evento de maximizar pelo Notifyicon
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        Me.Activate()
        Me.Focus()
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Private Sub RestaurarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestaurarToolStripMenuItem.Click
        'Restaura o programa
        Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub FecharToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FecharToolStripMenuItem.Click
        'fechar o programa
        Me.Close()
    End Sub
    Private Sub Closer_Tick(sender As Object, e As EventArgs) Handles CloseForm.Tick
        'Fecha o form
        On Error Resume Next
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub CheckLockUnlock(ByVal sender As Object, ByVal e As SessionSwitchEventArgs)
        'Se o user bloquear
        If e.Reason = SessionSwitchReason.SessionLock Then
            Debug.Print("Computador bloqueado")
            Voz.SpeechMoreThanOnce(MiaBrain.RequestWarnings(11))
        Else
            Voz.SpeechMoreThanOnce(MiaBrain.RequestWarnings(12))
            Debug.Print("Computador liberado")
        End If
    End Sub

    Protected Overrides Sub WndProc(ByRef M As System.Windows.Forms.Message)
        'Evento de adicionar e remover pen driver
        If M.Msg = WM_DEVICECHANGE Then
            Select Case M.WParam
                Case DBT_DEVICEARRIVAL
                    Dim DevType As Integer = Runtime.InteropServices.Marshal.ReadInt32(M.LParam, 4)
                    If DevType = DBT_DEVTYP_VOLUME Then
                        Dim Vol As New DEV_BROADCAST_VOLUME
                        Vol = Runtime.InteropServices.Marshal.PtrToStructure(M.LParam, GetType(DEV_BROADCAST_VOLUME))
                        If Vol.Dbcv_Flags = 0 Then
                            For i As Integer = 0 To 20
                                If Math.Pow(2, i) = Vol.Dbcv_Unitmask Then
                                    Dim Usb As String = Chr(65 + i) + ":\"
                                    MsgBox("Looks like a USB device was plugged in!" & vbNewLine & vbNewLine & "The drive letter is: " & Usb.ToString)
                                    Exit For
                                End If
                            Next
                        End If
                    End If

                Case DBT_DEVICEREMOVECOMPLETE
                    Dim DevType As Integer = Runtime.InteropServices.Marshal.ReadInt32(M.LParam, 4)
                    If DevType = DBT_DEVTYP_VOLUME Then
                        Dim Vol As New DEV_BROADCAST_VOLUME
                        Vol = Runtime.InteropServices.Marshal.PtrToStructure(M.LParam, GetType(DEV_BROADCAST_VOLUME))
                        If Vol.Dbcv_Flags = 0 Then
                            For i As Integer = 0 To 20
                                If Math.Pow(2, i) = Vol.Dbcv_Unitmask Then
                                    Dim Usb As String = Chr(65 + i) + ":\"
                                    MsgBox("Looks like a volume device was removed!" & vbNewLine & vbNewLine & "The drive letter is: " & Usb.ToString)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
            End Select
        End If
        MyBase.WndProc(M)
    End Sub

    Public Function GetMiaBraind() As Brain
        'Retorna Instancia do Brains
        Return MiaBrain
    End Function

    Sub ShowWarning()
        'mostra o alerta de HUD
        If Me.InvokeRequired Then
            Me.Invoke(New Action(AddressOf ShowWarning))
        Else
            Me.Alert.Visible = True
        End If
    End Sub

    Private Sub UI_MouseHover(sender As Object, e As EventArgs) Handles UI.MouseHover
        'zera os clicks ao mover pela UI
        MouseclickUI = 0
    End Sub

    Public Sub Throwalert(Alert As String)
        'Mostra o alerta
        MiaBrain.SetAlertText(Alert)
        ShowWarning()
    End Sub

#End Region

#Region "Voice vol"
    Private Sub AltaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AltaToolStripMenuItem.Click
        'volume maximo
        Voz.Setvolume(100)
    End Sub

    Private Sub BaixaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BaixaToolStripMenuItem.Click
        'voluem baixo
        Voz.Setvolume(30)
    End Sub

    Private Sub MudaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MudaToolStripMenuItem.Click
        'mudo
        Voz.Setvolume(0)
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        'volume maximo
        Voz.Setvolume(100)
    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
        'voluem baixo
        Voz.Setvolume(30)
    End Sub

    Private Sub ToolStripMenuItem5_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem5.Click
        'mudo
        Voz.Setvolume(0)
    End Sub
#End Region

    Private Sub GamerVerify_Tick(sender As Object, e As EventArgs) Handles GamerVerify.Tick
        'Verifica se algum jogo foi aberto
        Dim GameCount As Integer
        Dim GameList = MiaBrain.RequestGamelist()

        For Each Game In GameList
            GameCount = FindWindow(vbNullString, Game)
            If (GameCount > 0 And GameOpen = 0) Then
                'Abriu jogo
                GameOpen = 1
                Voz.SpeechMoreThanOnce(MiaBrain.RequestWarnings(10) + ", e o ping está " + NetSpeed())
                Me.WindowState = FormWindowState.Minimized
            Else
                'Fechou o jogo
                If (GameOpen = 1 And GameCount <= 0) Then
                    GameOpen = 0
                    MsgBox("Bom jogo")
                End If
            End If
        Next
    End Sub

    Private Sub HideUIicons_Tick(sender As Object, e As EventArgs) Handles HideUIicons.Tick
        Debug.Print("Ocultando icons do UI")
        HideUIicons.Enabled = False
        Config.Visible = False
    End Sub
End Class
