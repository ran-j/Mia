Public Class JsonClass
    Private Sub New()
        Debug.Print("Iae kkkk")
    End Sub
End Class


#Region "Bing Imagem"
Public Class Imagenss
    Public Property startdate As String
    Public Property fullstartdate As String
    Public Property enddate As String
    Public Property url As String
    Public Property urlbase As String
    Public Property copyright As String
    Public Property copyrightlink As String
    Public Property quiz As String
    Public Property wp As Boolean
    Public Property hsh As String
    Public Property drk As Integer
    Public Property top As Integer
    Public Property bot As Integer
    Public Property hs As Object()
End Class

Public Class Tooltips
    Public Property loading As String
    Public Property previous As String
    Public Property [next] As String
    Public Property walle As String
    Public Property walls As String
End Class

Public Class Bing
    Public Property images As Imagenss()
    Public Property tooltips As Tooltips
End Class
#End Region


#Region "Temperatura"


Public Class Units
    Public Property Distance As String
    Public Property Pressure As String
    Public Property Speed As String
    Public Property Temperature As String
End Class


Public Class Location
    Public Property city As String
    Public Property country As String
    Public Property region As String
End Class

Public Class Wind
    Public Property chill As String
    Public Property direction As String
    Public Property speed As String
End Class

Public Class Atmosphere
    Public Property humidity As String
    Public Property pressure As String
    Public Property rising As String
    Public Property visibility As String
End Class

Public Class Astronomy
    Public Property sunrise As String
    Public Property sunset As String
End Class

Public Class Imagens
    Public Property title As String
    Public Property width As String
    Public Property height As String
    Public Property link As String
    Public Property url As String
End Class

Public Class Condition
    Public Property code As String
    Public Property [date] As String
    Public Property temp As String
    Public Property text As String
End Class

Public Class Forecast
    Public Property code As String
    Public Property [date] As String
    Public Property day As String
    Public Property high As String
    Public Property low As String
    Public Property text As String
End Class

Public Class Guid
    Public Property isPermaLink As String
End Class

Public Class Item
    Public Property title As String
    Public Property lat As String
    Public Property [long] As String
    Public Property link As String
    Public Property pubDate As String
    Public Property condition As Condition
    Public Property forecast As Forecast()
    Public Property description As String
    Public Property guid As Guid
End Class

Public Class Channel
    Public Property units As Units
    Public Property title As String
    Public Property link As String
    Public Property description As String
    Public Property language As String
    Public Property lastBuildDate As String
    Public Property ttl As String
    Public Property location As Location
    Public Property wind As Wind
    Public Property atmosphere As Atmosphere
    Public Property astronomy As Astronomy
    Public Property image As Imagens
    Public Property item As Item
End Class

Public Class Results
    Public Property channel As Channel
End Class

Public Class Query
    Public Property count As Integer
    Public Property created As DateTime
    Public Property lang As String
    Public Property results As Results
End Class

Public Class Weather
    Public Property query As Query
End Class

#End Region