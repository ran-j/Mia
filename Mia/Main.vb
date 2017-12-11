﻿Imports System.Runtime.InteropServices

Public Class Main
    Public Shared MiaBrain As New Process 'Cria instancia do controlador principal do cerebro

    Dim arguments As String() = Environment.GetCommandLineArgs() 'pega os argumentos
    Public OldmousePosition As Integer = 0

    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    Public Const HT_CAPTION As Integer = &H2

    <DllImportAttribute("user32.dll")>
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function

    <DllImportAttribute("user32.dll")>
    Public Shared Function ReleaseCapture() As Boolean
    End Function

    Dim Sys As New SystemInteract


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OldmousePosition = MousePosition.X 'pega a posição do mouse

        AddHandler MiaBrain.LoadCompleted, AddressOf LoadC 'adiciona evento de carregamento

        MiaBrain.Init1() 'Starta o processamento

        'Tooltip com saldaçao
        ToolTip1.SetToolTip(Me.PictureBox1, "Olá")

        'Deixar transparente
        Me.BackColor = Color.FromArgb(255, 255, 255)

        'Spaw position
        Dim resolution As String() = Sys.GetResolutionOf(0).ToString.Split(",")
        Dim x As Integer = CInt(resolution(0)) - 280
        Dim y As Integer = CInt(resolution(1)) - 300
        Me.DesktopLocation = New Point(x, y)
    End Sub

    Private Sub AFKDetector_Tick(sender As Object, e As EventArgs) Handles AFKDetector.Tick
        If (Me.WindowState = FormWindowState.Normal) Then
            If (OldmousePosition = MousePosition.X) Then 'verificar se o mouse esta no mesmo lugar ou ele se movimento.

                AFKDetector.Enabled = False
                MsgBox("Hey, esta ai ?")
                OldmousePosition = 0
                Me.WindowState = FormWindowState.Minimized
            Else
                OldmousePosition = MousePosition.X 'pegar movimento
                Debug.Print(OldmousePosition)
            End If
        End If
    End Sub

    Private Sub LoadC()
        'evento de quando termina de carregar os arquivos
        AFKDetector.Enabled = True
        Debug.Print("Carregamento completo")
    End Sub



#Region " Form Control "

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown, PictureBox1.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
        End If
    End Sub

    Private Sub PictureBox1_DoubleClick(sender As Object, e As EventArgs) Handles PictureBox1.DoubleClick
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Main_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If (Me.WindowState = FormWindowState.Minimized) Then
            'reseta AFK Detector e reseta tempo
            AFKDetector.Interval = 50000
            AFKDetector.Enabled = False

        ElseIf (Me.WindowState = FormWindowState.Normal) Then
            'Ativa AFK Detector
            AFKDetector.Enabled = True

        End If
    End Sub

#End Region

End Class
