﻿Imports System.Threading
Imports Microsoft.Win32

Public Class Brain
    'principal que controla as outras

    'Controladores, cada um responsavel por uma coisa
    Dim TheMemory As New Memory 'Salva e busca dados salvos
    Dim Thinking As New Think(TimeOfDay.ToString("h:mm:ss tt")) 'Processa comandos do usuário
    Dim Action As New Act 'Toda a parte de ação que responde a usuário
    Dim Interpreter As New Interpreter ' interpretador de texto

    'Classes Secundarias
    Dim Sys As New SystemInteract 'interaçao com o sistema
    Dim Net As New MyNetwork 'Interaçao com internet como verificaçoes e velocidade
    Dim PW As New Power 'Classe para interação com energia
    Dim AlertSet As New AlertClass
    Dim Scan As New Scanner("f9af76130ab62113a8ce2e022b9b61604a6717fdb0ca1d6cb2eb392835d4ea89") 'Classe responsavel por scanear arquivos

    Public Event LoadCompleted()

    Sub Init1() 'Starta o processamento
        Thinking.ReceiveConversation(TheMemory.LoadConversation())
        Thinking.ReceiveQuestions(TheMemory.LoadQuestions)
        Thinking.Dates(TheMemory.LoadDates())
        Thinking.Preferences(TheMemory.LoadPreferences())

        RaiseEvent LoadCompleted() 'termiona o processamento e avisa
    End Sub


    Sub Init5() 'Significa o fim do programa

    End Sub

    Function SalveRequest(TargedForSave) As Boolean
        'Recbe solicitações para salvar e verifica se o mesmo consegue salvar
        Dim status As Boolean = False

        Return status
    End Function

    Function RequestWarnings(WarningCode As Integer) As String
        If (WarningCode > 0) Then
            'reorna uma aviso
            Return Thinking.RequestWarnings(WarningCode)
        Else
            Return "Erro"
        End If

    End Function

    Function RequestQuestion() As String
        'retorna uma pergunta rando pro usuário
        Return Thinking.RequestQuestion()
    End Function

    Function RequestVerifyText(text) As String
        'verifica o texto corrige e retorna ele redondo
        Return Interpreter.VerifyText(text)
    End Function

    Function RequestWeather() As String
        'Retorna a previ'zão do tempo
        Dim response As String = Action.GetWeather()
        If (response.Contains("Vazio")) Then
            Return "Erro"
        Else
            Return response
        End If
    End Function

    Function RequestResolutionOff(monitor As Integer)
        'retorna a resoluçao de um monitor
        Return Sys.GetResolutionOf(monitor)

    End Function

    Function RequestNetSpeed() As Integer
        'Returna a felocidade em 4 valores melhor para pior : 0 1 2 3 4
        Return Net.Getnetspeed
    End Function

    Function RequestIstanceOfScanClass() As Scanner
        'Retorna instancia da classe Scanner
        Return Scan
    End Function

    Function RequestIstanceOfNetClass() As MyNetwork
        'Retorna instancia da classe network
        Return Net
    End Function

    Sub RequestHibernatePC()
        'Faz o pc Hibernar
        PW.HibernatePC()
    End Sub

    Sub RequestPowerPC()
        'Faz o pc Desligar
        PW.PowerOffPc()
    End Sub

    Sub RequestTurnOFFLCD()
        'Desliga o monitor
        PW.TurnOffLCD()
    End Sub

    Function RequestBaterryStatus() As String
        'Retorna status da bateria
        Return PW.BatteryStatus()
    End Function

    Function RequestBaterryPercent() As Integer
        'Retorna porcentagem da bateria
        Return PW.BatteryPercent()
    End Function

    Function RequestBootMode()
        'Retorna o tipo de boot
        Return Sys.GetBootMode()
    End Function

    Sub RequestEmptyRecycleBin(Handle)
        'limpa a lixeira
        Sys.EmptyRecycleBin(Handle)
    End Sub

    Function RequestGetAvailableMemory()
        'Retorna total de memoria disponivel
        Return Sys.GetAvailableMemory
    End Function

    Function RequestGetCPULoad()
        'Retorna total de uso da CPU
        Return Sys.GetCPULoad()
    End Function

    Function RequestGetTempSize()
        'Retorna Temp Size
        Return Sys.GetTempSize()
    End Function

    Function RequestGetMouseSpeed() As Integer
        'Retorna velocidade do mouse
        Return Sys.GetMouseSpeed
    End Function

    Sub RequestSetMouseSpeed(Speed As Integer)
        'Seta velocidade do mouse
        Sys.SetMouseSpeed(Speed)
    End Sub

    Sub SetAlertText(Text As String)
        'Seta um alerta para o texto
        AlertSet.AppendValue(Text)
    End Sub

    Function GetAlertText() As List(Of String)
        'retorna o texto de alerta
        Return AlertSet.GetArrayText()
    End Function

    Sub ClearAlertTextTextbyName(Name As String)
        'Remover valor do array
        AlertSet.ClearArrayTextByName(Name)
    End Sub

    Function GetArrayTextSize() As Integer
        'Retorna o tamanho do array
        Return AlertSet.GetArraySize
    End Function

    Function AddtoWindowsStart()
        'Adiciona a aplicaçao com o iniciar o windows
        Try
            Using key As RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
                If Not key.GetValue(My.Application.Info.Title) Is My.Application.Info.DirectoryPath & "\" & My.Application.Info.Title & ".exe" Then

                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", My.Application.Info.Title, My.Application.Info.DirectoryPath & "\" & My.Application.Info.Title & ".exe")

                    Return 1 'Sucesso
                End If
            End Using
        Catch ex As Exception
            Return 3 'erro
            Debug.Print(ex.Message)
        Finally
            My.Computer.Registry.CurrentUser.Close()
        End Try
    End Function

    Function RemovetoWindowsStart()
        'Remove a aplicaçao com o iniciar o windows
        Try
            Using key As RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
                If key.GetValue(My.Application.Info.Title) Is My.Application.Info.DirectoryPath & "\" & My.Application.Info.Title & ".exe" Then
                    key.DeleteValue(My.Application.Info.Title)
                    Return 1 'Sucesso
                Else
                    Return 2 'Nao inicia com o windows
                End If
            End Using
        Catch ex As Exception
            Return 3 'erro
            Debug.Print(ex.Message)
        Finally
            My.Computer.Registry.CurrentUser.Close()
        End Try
    End Function

    Public Sub ShowCustonsNotification(ByVal Config As Object)
        'Cria notificação customizada
        Dim FRM As New FrmNotification With {
            .Titulo = Config(0),
            .Icon = Config(1),
            .Nome = Config(2),
            .Deteccao = Config(3)
        }
        Application.Run(FRM)
    End Sub

    Sub RequestScanofFiles(File As String)
        'Solicita scan de file
        Dim t1 As Thread
        Scan.SetFile(File)
        t1 = New Thread(AddressOf Scan.ScanFile)
        t1.Start()
    End Sub


End Class
