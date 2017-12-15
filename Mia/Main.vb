Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Public Class Main
    Public Shared MiaBrain As Brain 'Cria instancia do controlador principal do cerebro

    Dim arguments As String() = Environment.GetCommandLineArgs() 'pega os argumentos

    Public OldmousePosition As Integer = 0

    Private WithEvents Net As MyNetwork

    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    Public Const HT_CAPTION As Integer = &H2

    <DllImportAttribute("user32.dll")>
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function

    <DllImportAttribute("user32.dll")>
    Public Shared Function ReleaseCapture() As Boolean
    End Function


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            'So deixa abrir uma aplicação
            Dim procs() As Process = Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName)
            If procs.Length > 1 Then
                MsgBox("A Aplicação já está sendo executada")
                Debug.Print("Ja esta aberta")
                CloseForm.Enabled = True
            Else
                MiaBrain = New Brain
                Net = MiaBrain.RequestIstanceOfNetClass()

                'Spaw position
                Dim resolution As String() = MiaBrain.RequestResolutionOff(0).ToString.Split(",")
                Dim x As Integer = CInt(resolution(0)) - 280
                Dim y As Integer = CInt(resolution(1)) - 300
                Me.DesktopLocation = New Point(x, y)
                'Deixar transparente
                Me.BackColor = Color.FromArgb(255, 255, 255)

                OldmousePosition = MousePosition.X 'pega a posição do mouse

                'Tooltip com saldaçao
                ToolTip.SetToolTip(Me.UI, "Olá")

                AddHandler MiaBrain.LoadCompleted, AddressOf LoadC 'adiciona evento de carregamento

                MiaBrain.Init1() 'Starta o processamento

                'Quando o sistema bloquear
                AddHandler SystemEvents.SessionSwitch, AddressOf CheckLockUnlock

            End If
        Catch ex As Exception
            Debug.Print("Erro: " + ex.Message)
            CloseForm.Enabled = True
        End Try
    End Sub

    Private Sub AFKDetector_Tick(sender As Object, e As EventArgs) Handles AFKDetector.Tick
        If (Me.WindowState = FormWindowState.Normal) Then
            If (OldmousePosition = MousePosition.X) Then 'verificar se o mouse esta no mesmo lugar ou ele se movimento.

                AFKDetector.Enabled = False

                MsgBox(MiaBrain.RequestWarnings(14))

                OldmousePosition = 0
                Me.WindowState = FormWindowState.Minimized
            Else
                OldmousePosition = MousePosition.X 'pegar movimento
                Debug.Print("Posiçao do mouse: " + OldmousePosition.ToString)
            End If
        End If
    End Sub

    Private Sub LoadC()
        'evento de quando termina de carregar os arquivos
        AFKDetector.Enabled = True
        Debug.Print("Carregamento completo")

        'Iniciar o monitoramento da net
        Net.StartMonitoring()
        'Net.StopMonitoring 

    End Sub



#Region " Form Control "

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown, UI.MouseDown
        'Mover o programa com o click do mouse
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
        End If
    End Sub

    Private Sub UI_DoubleClick(sender As Object, e As EventArgs) Handles UI.DoubleClick
        'Minimizar com click na foto
        Me.WindowState = FormWindowState.Minimized
        NetSpeed()
    End Sub

    Private Sub UI_MouseClick(sender As Object, e As MouseEventArgs) Handles UI.MouseClick
        'Mostra o botão para abrir o menu
        If (Config.Visible) Then
            Config.Visible = False
        Else
            Config.Visible = True
        End If

    End Sub

    Private Sub Config_Click(sender As Object, e As EventArgs) Handles Config.Click
        'Oculta o botão para abrir o menu
        Config.Visible = False
    End Sub

    Private Sub Alert_Click(sender As Object, e As EventArgs) Handles Alert.Click
        'Dispara o alerta salvo
        Dim text = MiaBrain.GetAlertText()

        MsgBox(text(0), MsgBoxStyle.Exclamation)

        MiaBrain.ClearAlertTextTextbyName(text(0))

        If (MiaBrain.GetArrayTextSize() = 0) Then
            Alert.Visible = False
        End If

    End Sub

    Private Sub Main_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If (Me.WindowState = FormWindowState.Minimized) Then
            'Desabilita o AFK detector
            AFKDetector.Enabled = False

            'Ocultar o programa
            Me.Hide()

            'Solta notificação
            NotifyIcon.ShowBalloonTip("5000", "Aviso", MiaBrain.RequestWarnings(1), ToolTipIcon.Info)

            'Troca o contexte menu
            NotifyIcon.ContextMenuStrip = CMS

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

        Else


        End If


    End Sub
#End Region

#Region "Net"

    Private Sub NetSpeed()
        Dim resp As Integer = MiaBrain.RequestNetSpeed()

        If (resp = 0) Then
            MsgBox("net otima")
        ElseIf (resp = 1) Then
            MsgBox("net razoavel")
        ElseIf (resp = 2) Then
            MsgBox("net bem ruim")
        ElseIf (resp = 3) Then
            MsgBox("net muito ruim")
        Else
            MsgBox("carai maluco muito fucking alto")
        End If
    End Sub

    Delegate Sub AddToList1Callback(ByVal ConnectionState As InternetConnectionState, ByVal IsStable As Boolean)
    Delegate Sub AddToList2Callback(ByVal ConnectionState As InternetConnectionState, ByVal IsStable As Boolean)

    Dim UmaVez As Integer = 0

    Sub AddToList1(ByVal ConnectionState As InternetConnectionState, ByVal IsStable As Boolean)
        If Me.InvokeRequired = True Then
            Dim d As New AddToList1Callback(AddressOf AddToList1)
            Me.Invoke(d, ConnectionState, IsStable)
        Else
            Debug.Print(Now & " - State: " & ConnectionState.ToString() & ", Stable: " & IsStable)

            If Not (ConnectionState.ToString().Equals("Connected")) Then
                UmaVez = 1
                MiaBrain.SetAlertText("Desconectado")
                Alert.Visible = True
            Else
                If (UmaVez = 1) Then
                    UmaVez = 0
                    Alert.Visible = False
                    MsgBox("Conectado de novo")
                End If
            End If

        End If

    End Sub

    Sub AddToList2(ByVal ConnectionState As InternetConnectionState, ByVal IsStable As Boolean)
        If Me.InvokeRequired = True Then
            Dim d As New AddToList2Callback(AddressOf AddToList2)
            Me.Invoke(d, ConnectionState, IsStable)
        Else
            Debug.Print(Now & " - State: " & ConnectionState.ToString() & ", Stable: " & IsStable)
            Debug.Print(Now & " - State: " & ConnectionState.ToString() & ", Stable: " & IsStable)

            If Not (ConnectionState.ToString().Equals("Connected")) Then
                If (UmaVez = 0) Then
                    Debug.Print("Desconectado")
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
#End Region


End Class
