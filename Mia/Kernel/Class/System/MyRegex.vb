Imports System.Text.RegularExpressions

Public Class MyRegex

    Shared Function RemoveCharacterRegex(ByVal stringToCleanUp As String)
        Return Regex.Replace(stringToCleanUp, "[^A-Za-z0-9\-/]", "")
    End Function

    Shared Function ContainsRegex(ImputText As String, KeyWord As String) As Boolean
        Dim r As Regex = New Regex(ImputText, RegexOptions.IgnoreCase)

        If (r.IsMatch(KeyWord)) Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
