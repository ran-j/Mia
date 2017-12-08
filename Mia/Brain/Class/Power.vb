Option Explicit On
Imports System.Runtime.InteropServices
Public Class Power

    Private Const MONITOR_OFF As Integer = 2
    Private SC_MONITORPOWER As Integer = &HF170
    Private WM_SYSCOMMAND As Integer = &H112

    Private Declare Function ExitWindowsEx Lib "user32" (ByVal dwOptions As Integer, ByVal dwReserved As Integer) As Integer

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    End Function

    'The sendmessage command unlocks (almost) endless possibilities. Think of it as a function which tells the operating system to do something. Each thing has a seperate signature.
    <DllImport("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As Integer, ByVal hMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function

    Function BatteryPercent()
        Dim POWER As PowerStatus = SystemInformation.PowerStatus
        Dim PERCENT As Single = POWER.BatteryLifePercent

        Return PERCENT * 100
    End Function

    Function BatteryStatus()
        Dim Battery As PowerStatus = SystemInformation.PowerStatus
        If Battery.PowerLineStatus = PowerLineStatus.Online Then
            Return "Charging"
        Else
            Return "Discharging"
        End If
    End Function

    Sub PowerOffPc()
        Application.SetSuspendState(PowerState.Suspend, True, True)
    End Sub

    Sub HibernatePC()
        Application.SetSuspendState(PowerState.Hibernate, True, True)
    End Sub

    Sub TurnOffLCD()
        Debug.Print("Desligando Monitor")
        Dim num As Integer = 0
        num = SendMessage(FindWindow(Nothing, Nothing).ToInt32, Me.WM_SYSCOMMAND, Me.SC_MONITORPOWER, 2)
    End Sub

    Sub TurnOnLCD()
        Debug.Print("Ligando Monitor")
        Dim num As Integer = 0
        num = SendMessage(FindWindow(Nothing, Nothing).ToInt32, Me.WM_SYSCOMMAND, Me.SC_MONITORPOWER, -1)
    End Sub

End Class
