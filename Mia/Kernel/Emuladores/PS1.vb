Imports System.IO
Imports System.Threading


Public Class PS1

    Private Bin As String = Application.StartupPath + "\PS1"
    Private Emulador As String = Application.StartupPath + "\PS1\Launch.bat"
    Private jogo As String = "Vazio"

    Public Event ErroEmulator(text As String)


    Sub New()
        If Not Directory.Exists(Bin) Then
            Debug.Print("Pasta do emulador não encontrada")

            Dim t12 As Thread
            t12 = New Thread(AddressOf Erroinicial)
            t12.Start()
        End If
    End Sub

    Sub Erroinicial()
        Threading.Thread.Sleep(1000)
        RaiseEvent ErroEmulator("Pasta do emulador não encontrada")
    End Sub

    Private Sub Emulate()
        'Inicia emulação
        If Not (jogo.Equals("Vazio")) Then

            Try
                Using sw As StreamWriter = New StreamWriter(Emulador, False)
                    sw.WriteLine("@echo off")
                    sw.WriteLine("cd " + """" + Bin + """")
                    sw.WriteLine("echo Nao fechar essa janela.")
                    sw.WriteLine("psxfin.exe " + """" + jogo + """")
                End Using

                Dim startInfo As New ProcessStartInfo(Emulador) With {
                    .WindowStyle = ProcessWindowStyle.Minimized
                }

                Process.Start(startInfo)

                Threading.Thread.Sleep(500)

                My.Computer.FileSystem.DeleteFile(Emulador)
            Catch ex As Exception
                My.Settings.Erros = My.Settings.Erros + 1
                Debug.Print(ex.Message)
            End Try

        Else

            RaiseEvent ErroEmulator("Jogo não setado")

        End If

    End Sub

    Private Function SetGame(GamePath As String) As Boolean
        'Verifica se o jogo caminho existe e se o mesmo e um jogo
        If File.Exists(GamePath) Then
            Dim extension As String = Path.GetExtension(GamePath)

            If (extension.Equals(".bin") Or extension.Equals(".cue") Or extension.Equals(".iso") Or extension.Equals(".cdz")) Then
                jogo = GamePath
                Return True
            Else
                RaiseEvent ErroEmulator("Arquivo informado não é um jogo de PS1")
                Return False
            End If
        Else
            RaiseEvent ErroEmulator("Diretório: " + GamePath + " não existe.")
            Return False
        End If
    End Function

    Sub RunGame(GamePath As String)
        'Inicia o processo
        If (SetGame(GamePath)) Then
            Emulate()
        Else
            Debug.Print("Erro ao setar o jogo")
        End If
    End Sub

End Class
