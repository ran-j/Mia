Imports System.Threading
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
    Dim AlertSet As New AlertClass 'Classe responsavel pelos alertas
    Dim Scan As New Scanner(My.Settings.VirusTotalKey) 'Classe responsavel por scanear arquivos
    Dim ConversationClass As New Conversation 'Classe que e reponsavel por responder usuario
    Dim PS1Emu As New PS1 'Classe de emulador de PS1

    Dim ArduinoList As New List(Of Arduino)() 'Lista de arduinos salvo

    Public Event LoadCompleted()

    Sub Init1() 'Starta o processamento
        Thinking.ReceiveConversation(TheMemory.LoadConversation())
        Thinking.ReceiveQuestions(TheMemory.LoadQuestions)
        Thinking.Dates(TheMemory.LoadDates())
        Thinking.Preferences(TheMemory.LoadPreferences())

        'Desconmentar somente quando liberar a alpha
        If (My.Settings.WindowsStart = 1) Then
            'Adiciona o programa ao start do windowns 
            'AddtoWindowsStart()
        Else
            'Remove o iniciar com o windows
            'RemovetoWindowsStart()
        End If

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

    Function RequestIstanceOfPS1Emu() As PS1
        'Retorna instancia da classe network
        Return PS1Emu
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

#Region "Alert HUD"

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
#End Region

    Function AddtoWindowsStart()
#Disable Warning
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
            SetAlertText("Erro ao adiconar o programa ao iniciar")
            Main.ShowWarning()
            Debug.Print(ex.Message)
        Finally
            My.Computer.Registry.CurrentUser.Close()
        End Try
#Enable Warning
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
            SetAlertText("Erro ao remover o programa ao iniciar")
            Main.ShowWarning()
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

    Function RequestRemovableDrives() As String()
        'Retorna a lista de dispositivos conectados
        Return Sys.GetRemovableDrives()
    End Function

    Sub RequestUpvolume(Handle)
        'Aumenta o volume
        Sys.VolumeUp(Handle)
    End Sub

    Sub RequestDownvolume(Handle)
        'Diminui o volume
        Sys.VolumeDown(Handle)
    End Sub

    Sub RequestMutevolume(Handle)
        'Mutar o volume
        Sys.MuteVolume(Handle)
    End Sub

    Sub RequestDeleteAllFilesandFolders(Folder As String)
        'Deleta arquivos de uma pasta
        Sys.DeleteAllFilesandFolders(Folder)
    End Sub

    Function RequestAudioCardList() As List(Of String)
        'Retorna lista de audio
        Return Sys.GetAudioCardList()
    End Function

    Sub RequestGetIps()
        'Retorna ips conectados na rede
        Dim t12 As Thread
        t12 = New Thread(AddressOf Net.GetIPsConnected)
        t12.Start()
    End Sub

    Function RequestGetHostNameFromIP(IP As String) As String
        'Retorna o nome do host por IP
        Return Net.GetHostNameFromIP(IP)
    End Function

    Function RequestPortOpen(PortNumber As Integer)
        'Retorna se a porta esta aberta
        Dim out As Integer = Net.PortOpen(PortNumber)

        If (out = 0) Then
            Return "Nem uma"
        ElseIf (out = 1) Then
            Return "UDP"
        ElseIf (out = 2) Then
            Return "TCP"
        Else
            Return "TCP e UDP"
        End If

    End Function

    Function RequestVerifyText(text As String) As String
        Debug.Print("Processando texto: " + text)
        'verifica o texto corrige e retorna ele redondo
        Return Interpreter.VerifyText(text)
    End Function

    Function RequestConversation(Number As Integer)
        'Responde uma pergunta do usuário
        Return ConversationClass.AnwserConversation(Number)
    End Function

    Function RequestGamelist() As List(Of String)
        'Retorna array com lista de games
        Return TheMemory.LoadGamesList()
    End Function

    Sub RequestRunGame(Gamespath As String)
        'inicia uma emulação de jogo de ps1
        PS1Emu.RunGame(Gamespath)
    End Sub

    Function RequestArduinoList() As List(Of Arduino)
        'Retorna array com arduinos setados
        Return ArduinoList
    End Function

    Sub RequestregisterArduino(ArduinoOBJ As Arduino)
        'Adiciona arduino no array
        ArduinoList.Add(ArduinoOBJ)
    End Sub
End Class
