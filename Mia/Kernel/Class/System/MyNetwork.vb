﻿Imports System.Net.NetworkInformation
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports System.Text
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Sockets

Public Enum InternetConnectionState
    Connected
    Disconnected
End Enum

Public Class MyNetwork

    Implements IDisposable

    Public Event IpsCaptured(IP As List(Of String))

    Protected MACLIST As String() = New String() {"D0:17:c2", "F4:0E:22", "A8:96:75", "B8:27:EB", "DC:35:F1"}
    Protected MACVendedor As String() = New String() {"Asus", "Samsung", "Motorola", "Raspberry Pi", "Positivo"}

    Public Shared Property CheckHostName As String = "www.google.com.br"
    Public Property ConnectionStableAfterSec As Integer = 10

    Private MonitoringStarted As Boolean = False
    Private StableCheckTimer As System.Threading.Timer
    Private IsFirstCheck As Boolean = True
    Private wConnectionIsStable As Boolean
    Private PrevInternetConnectionState As InternetConnectionState = InternetConnectionState.Disconnected

    Dim Host As String = Dns.GetHostEntry(Dns.GetHostName()).AddressList _
    .Where(Function(a As IPAddress) Not a.IsIPv6LinkLocal AndAlso Not a.IsIPv6Multicast AndAlso Not a.IsIPv6SiteLocal) _
    .First() _
    .ToString()

    Public Shared Event InternetConnectionStateChanged(ByVal ConnectionState As InternetConnectionState)
    Public Shared Event InternetConnectionStableChanged(ByVal IsStable As Boolean, ByVal ConnectionState As InternetConnectionState)

    Public Sub StartMonitoring()
        If MonitoringStarted = False Then
            AddHandler NetworkChange.NetworkAddressChanged, AddressOf NetworkAddressChanged
            MonitoringStarted = True
            NetworkAddressChanged(Me, Nothing)
        End If
    End Sub

    Public Sub StopMonitoring()
        If MonitoringStarted = True Then
            Try
                RemoveHandler NetworkChange.NetworkAddressChanged, AddressOf NetworkAddressChanged
            Catch ex As Exception
            End Try

            MonitoringStarted = False
        End If
    End Sub

    Public ReadOnly Property ConnectionIsStableNow As Boolean
        Get
            Return wConnectionIsStable
        End Get
    End Property

    <DllImport("wininet.dll")>
    Private Shared Function InternetGetConnectedState(ByRef Description As Integer, ByVal ReservedValue As Integer) As Boolean
    End Function

    Private Shared Function IsInternetAvailable() As Boolean
        Try
            Dim ConnDesc As Integer
            Dim conn As Boolean = InternetGetConnectedState(ConnDesc, 0)
            Return conn
        Catch
            Return False
        End Try
    End Function

    Private Shared Function IsInternetAvailableByDns() As Boolean
        Try
            Dim iheObj As IPHostEntry = Dns.GetHostEntry(CheckHostName)
            Return True
        Catch
            Return False
        End Try
    End Function

    Public Shared Function CheckInternetConnectionIsAvailable() As Boolean
        Return IsInternetAvailable() And IsInternetAvailableByDns()
    End Function

    Private Sub NetworkAddressChanged(sender As Object, e As EventArgs)
        wConnectionIsStable = False
        StableCheckTimer = New System.Threading.Timer(AddressOf ElapsedAndStable, Nothing, New TimeSpan(0, 0, ConnectionStableAfterSec), New TimeSpan(1, 0, 0))

        If IsFirstCheck Then
            If CheckInternetConnectionIsAvailable() Then
                PrevInternetConnectionState = InternetConnectionState.Connected
                RaiseEvent InternetConnectionStateChanged(InternetConnectionState.Connected)
            Else
                PrevInternetConnectionState = InternetConnectionState.Disconnected
                RaiseEvent InternetConnectionStateChanged(InternetConnectionState.Disconnected)
            End If

            IsFirstCheck = False
        Else
            If CheckInternetConnectionIsAvailable() Then
                If PrevInternetConnectionState <> InternetConnectionState.Connected Then
                    PrevInternetConnectionState = InternetConnectionState.Connected
                    RaiseEvent InternetConnectionStateChanged(InternetConnectionState.Connected)
                End If
            Else
                If PrevInternetConnectionState <> InternetConnectionState.Disconnected Then
                    PrevInternetConnectionState = InternetConnectionState.Disconnected
                    RaiseEvent InternetConnectionStateChanged(InternetConnectionState.Disconnected)
                End If
            End If
        End If
    End Sub

    Private Sub ElapsedAndStable()
        If wConnectionIsStable = False Then
            wConnectionIsStable = True
            Dim hasnet As Boolean = CheckInternetConnectionIsAvailable()
            RaiseEvent InternetConnectionStableChanged(True, IIf(hasnet, InternetConnectionState.Connected, InternetConnectionState.Disconnected))
        End If
    End Sub


#Region "Netspeed"
    Public Function Getnetspeed()
        'retorna o ping do pc
        Dim ping1 As Integer = pingnet()
        Dim ping2 As Integer = pingnet()
        Dim ping3 As Integer = pingnet()
        Dim ping4 As Integer = pingnet()

        Dim output As Integer = (ping1 + ping2 + ping3 + ping4) / 4

        Debug.Print("Ping 1: " + ping1.ToString)
        Debug.Print("Ping 2: " + ping2.ToString)
        Debug.Print("Ping 3: " + ping3.ToString)
        Debug.Print("Ping 4: " + ping4.ToString)

        Debug.Print("Ping total : " + output.ToString)

        If (output <= 69) Then
            Return 0
        ElseIf (output <= 80) Then
            Return 1
        ElseIf (output <= 200) Then
            Return 2
        ElseIf (output < 1000) Then
            Return 3
        Else
            Return 4
        End If

    End Function


    Public Function pingnet() As Integer
        Dim netspeed As Integer = 0

        Dim Result As Net.NetworkInformation.PingReply
        Dim SendPing As New Net.NetworkInformation.Ping
        Dim ResponseTime As Long
        Try
            Result = SendPing.Send("www.google.com")
            ResponseTime = Result.RoundtripTime
            If Result.Status = Net.NetworkInformation.IPStatus.Success Then
                netspeed = ResponseTime
            Else
                pingnet()
            End If
        Catch ex As Exception
            netspeed = 0
        End Try

        Return netspeed
    End Function
#End Region

#Region "IPs"

    Function PortOpen(ByVal PortNumber As Integer) As Integer
        Dim UDP As Boolean = False
        Dim TCP As Boolean = False

        If (IsPortOpen(PortNumber)) Then
            TCP = True
        End If
        If (IsPortOpen2(PortNumber)) Then
            UDP = True
        End If

        If (TCP And UDP) Then
            Return 3
        ElseIf (TCP) Then
            Return 2
        ElseIf (UDP) Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Private Function IsPortOpen(ByVal PortNumber As Integer) As Boolean
        Dim Client As TcpClient = Nothing
        Try
            Client = New TcpClient(Host, PortNumber)
            Return True
        Catch ex As SocketException
            Return False
        Finally
            If Not Client Is Nothing Then
                Client.Close()
            End If
        End Try
    End Function

    Private Function IsPortOpen2(ByVal PortNumber As Integer) As Boolean
        Dim Client As UdpClient = Nothing
        Try
            Client = New UdpClient(Host, PortNumber)
            Return True
        Catch ex As SocketException
            Return False
        Finally
            If Not Client Is Nothing Then
                Client.Close()
            End If
        End Try
    End Function

    Sub GetIPsConnected()
        Dim Ips As New List(Of String)()

        Dim IPFormat As String() = Host.Split(".")
        Dim Range As String = IPFormat(0) + "." + IPFormat(1) + "." + IPFormat(2) + ".{0}"
        Dim Timeout As Integer = 7000
        Dim count As Integer = 0

        For value As Integer = 2 To 254
            'Dim iap As String = range + value.ToString()
            Dim ip = String.Format(Range, value.ToString)
            If (count > 6) Then
                Debug.Print("TimeOut diminuido")
                Timeout = 100
            End If
            If My.Computer.Network.Ping(ip, Timeout) Then
                Debug.Print("O IP " + ip + " está conectado.")
                Ips.Add(ip)
            Else
                count = count + 1
            End If

            Debug.Print("Ping " + ip)
        Next

        RaiseEvent IpsCaptured(Ips)

    End Sub

    Sub Subir()

    End Sub

    Function GetHostNameFromIP(ByRef IP As String) As String
        'Pega o nome do host por IP
        Try
            Dim host As System.Net.IPHostEntry

            host = Net.Dns.GetHostEntry(IP)
            Return host.HostName
        Catch ex As Exception
            Return DiscoveryNameByMac(GetMACByIP(IP))
        End Try
    End Function

    Function DiscoveryNameByMac(MAC As String)
        'Descobrir nome do fabricante por MAC
        For value As Integer = 0 To MACLIST.Length
            If (MAC.Contains(MACLIST(value).ToString)) Then
                Return MACVendedor(value).ToString
                Exit Function
            End If
        Next
        Return "NULL"
    End Function

    Private Function GetMACByIP(ip As String)
        'Não utilizar fora dessa classe
        Try
            Dim Output As String
            Dim CMDprocess As New Process
            Dim StartInfo As New ProcessStartInfo With {
                .FileName = "cmd", 'starts cmd window
                .RedirectStandardInput = True,
                .RedirectStandardOutput = True,
                .UseShellExecute = False, 'required to redirect
                .WindowStyle = ProcessWindowStyle.Hidden,
                .CreateNoWindow = True
                }

            CMDprocess.StartInfo = StartInfo
            CMDprocess.Start()

            Dim SR As StreamReader = CMDprocess.StandardOutput
            Dim SW As StreamWriter = CMDprocess.StandardInput
            SW.WriteLine("arp -a " + ip) 'the command you wish to run.....
            SW.WriteLine("exit") 'exits command prompt window
            Output = SR.ReadToEnd 'returns results of the command window
            SW.Close()
            SR.Close()

            Dim Input = Output.ToString.Split("Type")
            Dim Filter1 = Input(1).Replace(ip, "").Replace(" ", "").Replace("dynamic", "").Replace("ype", "").Split(vbNewLine)
            Dim Filter2 = Filter1(1).Split("-")
            Dim OutputFormated = (Filter2(0) + ":" + Filter2(1) + ":" + Filter2(2)).ToString().ToUpper()
            Return OutputFormated
        Catch ex As Exception
            Return "Erro"
        End Try
    End Function

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                Try
                    RemoveHandler NetworkChange.NetworkAddressChanged, AddressOf NetworkAddressChanged
                Catch ex As Exception
                End Try
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class
