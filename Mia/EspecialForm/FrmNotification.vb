Imports System.Threading

Public Class FrmNotification
    Private Sub FrmNotification_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - 312, Screen.PrimaryScreen.WorkingArea.Height)
        Dim Y As Integer = 0
        Dim MyLastY As Integer = LastY
        LastY += 80
        Do Until Y >= MyLastY
            Application.DoEvents()
            Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - 312, Screen.PrimaryScreen.WorkingArea.Height - Y)
            Thread.Sleep(1)
            Y += 2
        Loop
        Thread.Sleep(5000)
        Dim MyLocation As Integer = Me.Location.Y
        Y = 0
        Do Until Y >= MyLastY
            Application.DoEvents()
            Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - 312, MyLocation + Y)
            Thread.Sleep(2)
            Y += 2
        Loop
        LastY = 80
        Me.Close()
    End Sub
End Class

Module NTPublic
    Public LastY As Integer = 80
End Module
