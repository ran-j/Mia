Imports System.ComponentModel
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmNotification
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        InitializeComponent()
    End Sub

    Private MGDetected As Bitmap = Bitmap.FromStream(New MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAEYAAABGCAYAAABxLuKEAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAGvSURBVHja7NkvaBZhHAfwzxMciFaDZWVFBE0DYaAwUVHjxGIYbMjEYDMZdCIWq4pTmROsLgmCLFiNJrUKIhj818XX8L6I6Dt4HgbC7r7feNyXu/vAc9w9vzIYDCT/pgQmMIEJTGACE5jABCYwgQlMEpjAbEuY4SUPYhnffx8Znz1Yx2Ol+zDTuIXZysYPzCmedR3mERYaW2uKxcAEJjB/wqzS/JAriotdh1nCbUw0tK4rlrsOs4C72BmYrcNcVdzoOsxp3MNkQ2tRsdZ1mOMjmKnABCYwW4GZwQoONLTOKp52HWYf7uNIQ+uEYiMwPYWZGi2lYw2to4qXXYeBm7hS2fiKGcW7wAQmMH/DXKP6p/ANDiu+9AHmwugFXJMXONmHzfDABKYdZs5wXlSTJ5jvCwy8wqGKxn687RPMhOFm1U/jp5E78MlwWqlPMI13G5jABGY8zCzO4dsmXLvxGg/6BDOJ95WNS7jTF5hTeF7ZeIilwAQmMIEJTGAC879h9uJjZeM8Vvv05TuPM/i8yZm78AGXMeg+zDZIYAITmMAEJjCBCUxgAhOYwCSBqc6vAQCjfVOEWe54QwAAAABJRU5ErkJggg==")))
    Private MGClean As Bitmap = Bitmap.FromStream(New MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAEYAAABGCAYAAABxLuKEAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAKgSURBVHja7No7axRRFADgb3uxshICWopg4x/QxMRoUAkhkaggClpYCEY0wUcjETU+09san4ioaNT4+AWCoJZq4x8Q+1jkBsxjNzszO5Pd2Xu6TWaWez/unHvOna3Mzs6KsTQqESbCRJgIE2EiTISJMBEmwkSYCBMjwkSYwmBaw28LjmIt/mASPxfOuD1hJgLMGvzFHYy3O8w77Fjm7/dwqF1hXqO3xv8fYLjdYFZCmY8nGGwXmPfoTHD9fhWPyg7zFt2J81BFT5lhkq6U+fisYmtZYV6iL+W9d1UcKyNMmsfn/9io4lfZYD5iW4b7e0KOKdWulHWl7MJ02eqYGXRluH9ngC1V5TsdJpY2OsMjWKpeqVrvkyynLJlx/jAdOILt+IGpUF80w0rZixfLzzhfmIHQeyyOGzhTUO9TLfrwqvqM84MZwsMaV17GhZSTehMegbTRHZK1omH68bSOq69hrODdZzs+rTzjxsOsx+8EA72NkTqv/RAmljZ6w2qzGjCTOJlwwPXknLQNYe3dp0CY79iUYuDXcTannLKweFslmCnzx4PJYxwXG1zmJ0fJCWYzvmaYyARGG4TSFfKSZoCB0yFvyLByOnA4w3fsCWcymgkGzlv8rqa42Ifnmb4h58p3FFcLRtkdqmLNDAOncKulUArsrsdwJWeU6g1hE8PkjdNYlFU4j8kjIQ9W6eBbCkYo/ScaNPx+PMtlDa7SCd4IbmYc+kCdHXxLwcC5cCaTJmofMrU4jNAXXUo45CE8zn3jb4LD8CRF4LC537BoB5h6c84B3C+sfm6i1ye1Gs+D5t4uaEcYOB62847w+Vt41GYKb0ObDGYdTmBD+PzF3E9NtT5Mm0WEiTARJsJEmAgTYSJMhIkwESbCxIgwESZr/BsAKsdghMhR9lgAAAAASUVORK5CYII=")))
    Private MGError As Bitmap = Bitmap.FromStream(New MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAEYAAABGCAYAAABxLuKEAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAMHSURBVHja7NnNbxVVGAfgZ6I7Y/wLSNi4gqW7LiCEYmBBCFKSppAIkgBNrBXbYoFStGmxFKxG/EBTi4mfSU1DXGiEpQsTl/4JRhfCwoU7I2VxT02xX/fOnNs5bc5vdSd3JrnnuWfOvO+ZYmFhQc7yFBkmw2SYDJNhMkyGyTAZJsNkmAyTk2EyTK0wK1+6B914Ar/iXTxsw29/CR3h8zy+Wz66dGCew3XsCscPcA1TkVHOYgjbw/EvuIB7KcJ04sdVzryNE5FQJjC8ynfH8EVKMHtxd52zY+CshbIcp2aYnWEtaSZVcK7i9SbP3Ye7dcJ0Yho7WrhqFifbMFOW5m/0K8zUBfMNjpa4spWZ0yrKYuYUuuqCuYODJa9uBqcsSu0wwxgLNUtsnCoo8LbCa3XBPInTuFlhACvhtLLQrpS3MK3wZ92P6ysYrTCQpQtyVZRbOJNSgTceqs+yGQ2V8vtRUBJrCariiDTrkoOpC2cGp1JuIhczhYENQvk0dNo2AwxMhg64HpSEYYR2ob9NKJ/hxbVHly4MjY2qvsgoX2tshtnMMPCBxuZSjHyJnuZGlz4MXMYbFVGuY7D50W0OmN6KxZuwmE9tJZg3MRLpVnoHr24FmNHQS8XMe00t6AnDjITZ0o5MrVsnJQrTTpTFXMP5zQQzrLHRtBFZ/UmVGMw53NjgJnICF1OGOa+xe1ZHJv1/gysRmL5Q/pfNR7hf8bE+jkspwQyGhbBsvseB8Hl23eaw2QW5Zpju0L/EQFm6nVDlde4QPlH4qy6Y3vAPPRURJRbOuGLJbbXBMPM4VPLqH7B/nXOq4NT6wm0OL0SeKbFwvlU4UhdMV6hZtrUJpSzOvxhUmK5z8e3ATxFvn9XSytPqMOZTeFz34PM2orSCM/Bf5Z1IgbcWTpnbp8xtNfBYO5JQS/B82Ct5Nhz/o/Ha9OXI5f8YXsHT4fi3UNR9lXITeRy7w8/6HR/ij8gwz2hslS7+AT/j43SayC2cDJNhMkyGyTAZJsNkmAyTYTJMhsnJMBmmah4NAHpxg4SfKEFpAAAAAElFTkSuQmCC")))

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        Select Case _Icon
            Case Icons.Detected
                BackColor = Color.FromArgb(241, 181, 95)
            Case Icons.Clean
                BackColor = Color.FromArgb(108, 191, 109)
            Case Icons.Error
                BackColor = Color.FromArgb(219, 100, 96)
        End Select

        G.Clear(BackColor)

        Select Case _Icon
            Case Icons.Detected
                G.DrawImage(MGDetected, 5, 0, 70, 70)
            Case Icons.Clean
                G.DrawImage(MGClean, 5, 0, 70, 70)
            Case Icons.Error
                G.DrawImage(MGError, 5, 0, 70, 70)
        End Select

        G.DrawString(_Titulo, _TituloFont, New SolidBrush(ForeColor), 80, 12)

        G.DrawString("Nome do Arquivo: " & _Nome, _DescricaoFont, New SolidBrush(_DescricaoColor), 82, 32)

        G.DrawString("Taxa de Detecção: " & _Deteccao, _DescricaoFont, New SolidBrush(_DescricaoColor), 82, 48)

        Dim GD As New LinearGradientBrush(New Rectangle(-1, -1, Width + 1, 5), _LineColor, Color.Transparent, 90)

        G.FillRectangle(GD, GD.Rectangle)

        GD = New LinearGradientBrush(New Rectangle(-1, Height - 15, Width + 2, 16), Color.Transparent, _LineColor, 90)

        G.FillRectangle(GD, GD.Rectangle)

        GD = New LinearGradientBrush(New Rectangle(-1, -1, 5, Height + 1), _LineColor, Color.Transparent, LinearGradientMode.Horizontal)

        G.FillRectangle(GD, GD.Rectangle)

        GD = New LinearGradientBrush(New Rectangle(Width - 5, -1, 6, Height + 1), Color.Transparent, _LineColor, LinearGradientMode.Horizontal)

        G.FillRectangle(GD, GD.Rectangle)

        e.Graphics.DrawImage(B, 0, 0) : B.Dispose() : G.Dispose()
        MyBase.OnPaint(e)
    End Sub

#Region "PROPRIEDADES"
    Public Enum Icons
        Detected = 0
        Clean = 1
        [Error] = 2
    End Enum

    Private _Icon As Icons
    Public Overloads Property Icon As Icons
        Get
            Return _Icon
        End Get
        Set(value As Icons)
            _Icon = value
        End Set
    End Property

    Private _LineColor As Color = Color.Black
    Public Property LineColor As Color
        Get
            Return _LineColor
        End Get
        Set(value As Color)
            _LineColor = value
        End Set
    End Property

    Private _Titulo As String
    Public Property Titulo As String
        Get
            Return _Titulo
        End Get
        Set(value As String)
            _Titulo = value
        End Set
    End Property

    Private _TituloFont As Font = New Font("Microsoft Sans Serif", 12)
    Public Property TituloFont As Font
        Get
            Return _TituloFont
        End Get
        Set(value As Font)
            _TituloFont = value
        End Set
    End Property

    Private _Nome As String
    Public Property Nome As String
        Get
            Return _Nome
        End Get
        Set(value As String)
            _Nome = value
        End Set
    End Property

    Private _Deteccao As String
    Public Property Deteccao As String
        Get
            Return _Deteccao
        End Get
        Set(value As String)
            _Deteccao = value
        End Set
    End Property

    Private _DescricaoFont As Font = New Font("Segoe UI", 9, FontStyle.Bold)
    Public Property DescricaoFont As Font
        Get
            Return _DescricaoFont
        End Get
        Set(value As Font)
            _DescricaoFont = value
        End Set
    End Property

    Private _DescricaoColor As Color = Color.FromArgb(55, 71, 79)
    Public Property DescricaoColor As Color
        Get
            Return _DescricaoColor
        End Get
        Set(value As Color)
            _DescricaoColor = value
        End Set
    End Property

#End Region

#Region "FOCO"
    Protected Overrides ReadOnly Property ShowWithoutActivation() As Boolean
        Get
            Return True
        End Get
    End Property

    Private Const WS_EX_TOPMOST As Integer = &H8
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim createParams__1 As CreateParams = MyBase.CreateParams
            createParams__1.ExStyle = createParams__1.ExStyle Or WS_EX_TOPMOST
            Return createParams__1
        End Get
    End Property
#End Region

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'FrmNotification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(312, 80)
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(254, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmNotification"
        Me.Opacity = 0.9R
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)

    End Sub
End Class
