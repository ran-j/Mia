Public Class AlertClass
    Private TextAlert As New List(Of String)()

    Sub AppendValue(Value As String)
        'Adicionar valor no array de alerta
        TextAlert.Add(Value)
    End Sub

    Function GetArrayText() As List(Of String)
        'Retorna valor do array
        Return TextAlert
    End Function

    Sub ClearArrayTextByName(Name As String)
        'Limpa valores do array
        TextAlert.Remove(Name)
    End Sub

    Function GetArraySize() As Integer
        'Retorna o tamanho do array
        Return TextAlert.Count
    End Function



End Class
