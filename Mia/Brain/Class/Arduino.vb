Imports System.IO.Ports
Public Class Arduino
    Dim WithEvents SerialPort1 As New IO.Ports.SerialPort

    Dim _Port As String
    Dim _BaudRate As Int32 = 9600
    Private _Name As String

    Sub New(Name As String, Port As String, Optional BaudRate As Int32 = 9600)
        _Name = Name
        _Port = Port
        _BaudRate = BaudRate
    End Sub

    Public Function Getname()
        'retorna name
        Return _Name
    End Function

    Sub Connect()
        Debug.Print("Aberto concexão com arduino")

        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        End If

        Try
            With SerialPort1
                .PortName = _Port
                .BaudRate = _BaudRate
                .Encoding = Text.Encoding.ASCII
                .NewLine = Chr(13) + Chr(10)
            End With

            'Open the port and clear any junck in the input buffer
            SerialPort1.Open()
            SerialPort1.DiscardInBuffer()

        Catch Ex As Exception
            'Handle any exceptions here
            My.Settings.Erros = My.Settings.Erros + 1
        End Try
    End Sub


    Function WriteCommand(Command) As String
        Try
            'Manda comando para o arduino
            SerialPort1.WriteLine(Command)

            If SerialPort1.IsOpen Then
                SerialPort1.Close()
            End If

            Return "1"
        Catch ex As Exception
            Debug.Print(ex.Message)
            My.Settings.Erros = My.Settings.Erros + 1
            Return "0"
        End Try
    End Function

    Function ReceiveSerialData() As String
        Dim Incoming As String
        Try
            If Not SerialPort1.IsOpen Then
                Connect()
            End If

            Incoming = SerialPort1.ReadExisting()
            If Incoming Is Nothing Then
                Return "nothing" & vbCrLf
            Else
                Return Incoming
            End If
        Catch ex As TimeoutException
            Return "Error: Serial Port read timed out."
            My.Settings.Erros = My.Settings.Erros + 1
        End Try

    End Function

End Class
