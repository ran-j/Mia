Imports System.IO.Ports
Public Class Arduino
    Dim WithEvents SerialPort1 As SerialPort
    Dim Str As String
    Public Event ErroOpenConnection(erro As String)
    Public Event TextReceived(text As String)

    Sub New(Port As String, Optional BaudRate As Integer = 9600)
        Try
            If SerialPort1.IsOpen Then
                SerialPort1.Close()

                With SerialPort1
                    .PortName = Port
                    .BaudRate = BaudRate
                    .Parity = IO.Ports.Parity.None
                    .DataBits = 8
                    .StopBits = IO.Ports.StopBits.One
                End With
                SerialPort1.Open()
            Else
                RaiseEvent ErroOpenConnection("Port Close")
                Debug.Print("Erro")
            End If
        Catch ex As Exception
            RaiseEvent ErroOpenConnection(ex.Message)
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub SerialPort1_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Str = SerialPort1.ReadExisting()
        Debug.Print(Str.ToString)

        RaiseEvent TextReceived(Str)
    End Sub

    Function WriteCommand(Command)
        Try
            SerialPort1.Write(Command)
            Return "Success"
        Catch ex As Exception
            Debug.Print(ex.Message)
            Return "0"
        End Try
    End Function


End Class
