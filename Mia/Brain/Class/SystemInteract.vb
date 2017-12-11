Imports System.IO
Imports System.Reflection
Imports OpenHardwareMonitor
Imports OpenHardwareMonitor.Hardware
Public Class SystemInteract
    'Atenção importar OpenHardwareMonitor do nuget
    Dim lngNumberOfDirectories As Integer

    Private m_memoryCounter As System.Diagnostics.PerformanceCounter
    Private m_CPUCounter As System.Diagnostics.PerformanceCounter

    Private Declare Function SystemParametersInfo Lib "user32.dll" Alias "SystemParametersInfoA" (ByVal uAction As Int32, ByVal uParam As Int32, ByVal lpvParam As Int32, ByVal fuWinIni As Int32) As Int32
    Private Declare Function SHEmptyRecycleBin Lib "shell32.dll" Alias "SHEmptyRecycleBinA" (ByVal hWnd As Int32, ByVal pszRootPath As String, ByVal dwFlags As Int32) As Int32
    Private Declare Function SHUpdateRecycleBinIcon Lib "shell32.dll" () As Int32

    Private Const SHERB_NOCONFIRMATION = &H1
    Private Const SHERB_NOSOUND = &H4


    Const SPI_SETMOUSESPEED As Int32 = 113
    Const SPIF_UPDATEINIFILE As Int32 = &H1

    Public Sub New()
        m_memoryCounter = New System.Diagnostics.PerformanceCounter()
        m_memoryCounter.CategoryName = "Memory"
        m_memoryCounter.CounterName = "Available MBytes"

        m_CPUCounter = New System.Diagnostics.PerformanceCounter()
        m_CPUCounter.CategoryName = "Processor"
        m_CPUCounter.CounterName = "% Processor Time"
        m_CPUCounter.InstanceName = "_Total"
    End Sub

    Function GetBootMode()
        'Retornar qual o tipo de inicialização
        Return SystemInformation.BootMode.ToString
    End Function

    Public Sub EmptyRecycleBin(Handle) 'precisa passar o handle Me.Handle
        'exclui tudo da lixeira
        SHEmptyRecycleBin(Handle.ToInt32, "", SHERB_NOCONFIRMATION + SHERB_NOSOUND)
        SHUpdateRecycleBinIcon()
    End Sub

    Public Function GetAvailableMemory() As Single
        'Pegar tamanho de memoria livre
        Return m_memoryCounter.NextValue()
    End Function

    Public Function GetCPULoad() As Single
        'TotaldeusoCPU
        Return m_CPUCounter.NextValue()
    End Function

    Public Function GetCPUTemperature()
        'pegar cpu temp
        Dim computer As New Computer()
        computer.Open()
        computer.CPUEnabled = True

        Dim saida = 0

        Dim cpu = computer.Hardware.Where(Function(h) h.HardwareType = HardwareType.CPU).FirstOrDefault()

        If cpu IsNot Nothing Then
            cpu.Update()

            Dim tempSensors = cpu.Sensors.Where(Function(s) s.SensorType = SensorType.Temperature)
            tempSensors.ToList.ForEach(Sub(s) If (s.Name.Equals("CPU Package")) Then saida = s.Value)
        End If
        's.Value.ToString + " . " + s.Name

        Return saida

    End Function

    Function GetResolution() As String
        'pegar resoluçao da tela ou telas
        Dim numberofmonitors As Integer = Screen.AllScreens.Length
        Dim monitores As String = SystemInformation.MonitorCount.ToString

        Dim WidthMonitor As Integer = 0
        Dim HeightMonitor As Integer = 0

        For value As Integer = 0 To numberofmonitors
            On Error Resume Next
            WidthMonitor = WidthMonitor + Screen.AllScreens(value).Bounds.Width
            HeightMonitor = HeightMonitor + Screen.AllScreens(value).Bounds.Height
        Next

        Return WidthMonitor.ToString + "," + HeightMonitor.ToString + "," + monitores
    End Function

    Function GetResolutionOf(Monitor As Integer) As String
        Try
            'pegar resoluçao da tela ou telas
            Dim numberofmonitors As Integer = Screen.AllScreens.Length

            Dim WidthMonitor As Integer = 0
            Dim HeightMonitor As Integer = 0

            WidthMonitor = WidthMonitor + Screen.AllScreens(Monitor).Bounds.Width
            HeightMonitor = HeightMonitor + Screen.AllScreens(Monitor).Bounds.Height

            Return WidthMonitor.ToString + "," + HeightMonitor.ToString
        Catch ex As Exception
            Return "530,550"
        End Try

    End Function


    Function GetTempSize()
        'pegar o tamanho da pasta temp
        Dim temp As String = Path.GetTempPath() 'pasta temp

        Dim tamanhopasta As String = FormatBytes(DirectorySize(temp, True))
        Dim tamanho As Integer = tamanhopasta.Substring(3)
        Dim tipo As String = tamanhopasta.Substring(0, 2)

        If ((tamanho > 1) And (tipo.Contains("GB")) Or (tipo.Contains("TB"))) Then
            Return 0
        Else
            Return 1
        End If
    End Function

    Default Public Property FormatBytes(ByVal BytesCaller As ULong) As String
        'formatar tamanho de bytes
        Get
            Try
                Dim DoubleBytes As Double
                Select Case BytesCaller
                    Case Is >= 1099511627776
                        DoubleBytes = CDbl(BytesCaller / 1099511627776) 'TB
                        Return "TB;" & FormatNumber(DoubleBytes, 2)
                    Case 1073741824 To 1099511627775
                        DoubleBytes = CDbl(BytesCaller / 1073741824) 'GB
                        Return "GB;" & FormatNumber(DoubleBytes, 2)
                    Case 1048576 To 1073741823
                        DoubleBytes = CDbl(BytesCaller / 1048576) 'MB
                        Return "MB;" & FormatNumber(DoubleBytes, 2)
                    Case 1024 To 1048575
                        DoubleBytes = CDbl(BytesCaller / 1024) 'KB
                        Return "KB;" & FormatNumber(DoubleBytes, 2)
                    Case 0 To 1023
                        DoubleBytes = BytesCaller ' bytes
                        Return "bytes;" & FormatNumber(DoubleBytes, 2)
                    Case Else
                        Return ""
                End Select
            Catch
                Return ""
            End Try
        End Get
        Set(value As String)
            'nada mesmo
        End Set
    End Property

    Overloads Function DirectorySize(ByVal sPath As String, ByVal bRecursive As Boolean) As Long
        'tamanho do diretorio
        Dim Size As Long = 0
        Dim diDir As New DirectoryInfo(sPath)

        Try
            Dim fil As FileInfo

            For Each fil In diDir.GetFiles()
                Size += fil.Length
            Next fil

            If bRecursive = True Then
                Dim diSubDir As DirectoryInfo

                For Each diSubDir In diDir.GetDirectories()
                    Size += DirectorySize(diSubDir.FullName, True)
                    lngNumberOfDirectories += 1
                Next diSubDir
            End If

            Return Size

        Catch ex As System.IO.FileNotFoundException
            Return 0
        Catch exx As Exception
            Return 0
        End Try

    End Function

    Function GetMouseSpeed() As Integer
        'pegar velocidade do mouse
        Return System.Windows.Forms.SystemInformation.MouseSpeed
    End Function

    Sub SetMouseSpeed(speed As String)
        'setar velocidade do mouse
        SystemParametersInfo(SPI_SETMOUSESPEED, 0, Integer.Parse(speed), SPIF_UPDATEINIFILE)
    End Sub

    Sub MoveMouse(Cursor As Cursor, Location As Point, Size As Size, positionX As Integer, positionY As Integer)
        'como usar MoveMouse(Me.Cursor,Me.Location,me.Size,50,50)
        'mover o mouse na tela
        Cursor = New Cursor(Cursor.Current.Handle)
        Cursor.Position = New Point(Cursor.Position.X - positionX, Cursor.Position.Y - positionY)
        Cursor.Clip = New Rectangle(Location, Size)
    End Sub

End Class
