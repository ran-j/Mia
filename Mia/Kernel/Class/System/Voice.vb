Imports System.Speech.Synthesis

Public Class Voice
    Dim Voice As New SpeechSynthesizer()

    Public Event VoiceSpeakCompleted()
    Public Event VoiceSpeakImprogres()

    Sub New()
        Voice.SetOutputToDefaultAudioDevice()
        Voice.Rate = 1

        AddHandler Voice.SpeakCompleted, AddressOf SpeakCompleted
        AddHandler Voice.SpeakProgress, AddressOf SpeakImprogres

        Debug.Print("Iniciando sistema de voz e setando o volume da voz para " + My.Settings.Volume.ToString)
        Setvolume(My.Settings.Volume)
    End Sub

    Sub SpeechMoreThanOnce(Word As String)
        'Fala mais de um
        Voice.SpeakAsync(Word)
    End Sub

    Sub SpeechJustOnce(Word As String)
        'Fala travada
        Voice.Speak(Word)
    End Sub

    Sub Setvolume(Vol As Integer)
        'salva o volume no array
        If (Vol <> My.Settings.Volume) Then
            My.Settings.Volume = Vol
            Voice.Volume = (Vol)

            If (Vol = 0) Then
                CancelSpeeck()
            End If
        End If
        'aumenta o volume
        Voice.Volume = (Vol)
        Debug.Print("Setando o volume para " + Vol.ToString)
    End Sub

    Sub CancelSpeeck()
        'Cancela oque esta sendo falado
        Voice.SpeakAsyncCancelAll()
    End Sub

    Sub SpeakCompleted()
        'Termina de falar
        RaiseEvent VoiceSpeakCompleted()
    End Sub

    Sub SpeakImprogres()
        'Fala em andamento
        RaiseEvent VoiceSpeakImprogres()
    End Sub

End Class
