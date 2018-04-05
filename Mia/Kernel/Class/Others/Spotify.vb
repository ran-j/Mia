' Spotify API V0.01 beta - a very quick First draft
' © august 3 2009 by Steffest
' This code is free to use in any way you want and comes with NO WARRANTIES
' tested with Spotify 0.3.18
' Usage:
' 
' Dim Spotify As New Spotify()
' 
' Spotify.PlayPause()
' Spotify.PlayPrev()
' Spotify.PlayNext()
' Spotify.Mute()
' Spotify.VolumeUp()
' Spotify.VolumeDown()
' Spotify.Nowplaying() (Gets the name of the current playing track)
' Spotify.Search("Artist",False) (Searches for "Artist")
' Spotify.Search("Artist",True) (Searches for "Artist" and starts playing the results)

Public Class Spotify

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

#Region " constants "
    Private Const WM_KEYDOWN = &H100
    Private Const WM_KEYUP = &H101
    Private Const WM_MOUSEACTIVATE = &H21
    Private Const KEYEVENTF_EXTENDEDKEY As Integer = &H1S
    Private Const KEYEVENTF_KEYUP As Integer = &H2S
#End Region

    Private w As Integer

    Public Sub New()
        w = FindWindow("SpotifyMainWindow", vbNullString)
    End Sub

    Public Function PlayPause() As Boolean
        On Error Resume Next
        SendMessage(w, WM_KEYDOWN, Keys.Space, 0)
        SendMessage(w, WM_KEYUP, Keys.Space, 0)
    End Function

    Public Function PlayPrev() As Boolean
        On Error Resume Next
        ' for some reason the PostMessage(w, WM_KEYDOWN, Keys.MediaNextTrack, 0) doesn't work
        ' sending ctrl+ commands to a windows still is a PITA ...
        SetForegroundWindow(w)
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.Left, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Left, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Sleep(100) ' wait until spotify has trapped the control key before releasing it
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
    End Function

    Public Function PlayNext() As Boolean
        On Error Resume Next
        SetForegroundWindow(w)
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.Right, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Right, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Sleep(100) ' wait until spotify has trapped the control key before releasing it
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
    End Function

    Public Function VolumeUp() As Boolean
        On Error Resume Next
        SetForegroundWindow(w)
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.Up, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Up, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Sleep(100) ' wait until spotify has trapped the control key before releasing it
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
    End Function

    Public Function Mute() As Boolean
        On Error Resume Next
        SetForegroundWindow(w)
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.ShiftKey, &H1D, 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Sleep(100) ' wait until spotify has trapped the control key before releasing it
        keybd_event(Keys.ShiftKey, &H1D, KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
    End Function

    Public Function VolumeDown() As Boolean
        On Error Resume Next
        SetForegroundWindow(w)
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Sleep(100) ' wait until spotify has trapped the control key before releasing it
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
    End Function

    Public Function Nowplaying() As String
        On Error Resume Next
        Dim lpText As String
        lpText = New String(Chr(0), 100)
        Dim intLength As Integer = GetWindowText(w, lpText, lpText.Length)
        If (intLength <= 0) OrElse (intLength > lpText.Length) Then Return "Unknown"
        Dim strTitle As String = lpText.Substring(0, intLength)
        strTitle = Mid(strTitle, 11)
        Return strTitle
    End Function

    Public Function Search(ByVal s As String, ByVal AndPlay As Boolean) As Boolean
        On Error Resume Next
        SetForegroundWindow(w)
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.L, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.L, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Sleep(100) ' wait until spotify has trapped the control key before releasing it
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)

        SendKeys.SendWait(s & Chr(13))

        If AndPlay Then
            ' this is a bit stupid but works in this version: press tab twice, then enter
            Sleep(100)
            SendKeys.SendWait(Chr(9) & Chr(9) & Chr(13))
        End If

    End Function

End Class