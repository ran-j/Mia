﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Descartar substituições de formulário para limpar a lista de componentes.
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

    'Exigido pelo Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'OBSERVAÇÃO: o procedimento a seguir é exigido pelo Windows Form Designer
    'Pode ser modificado usando o Windows Form Designer.  
    'Não o modifique usando o editor de códigos.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.AFKDetector = New System.Windows.Forms.Timer(Me.components)
        Me.UI = New System.Windows.Forms.PictureBox()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.NotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.CMS = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RestaurarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FecharToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CMSVazio = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Alert = New System.Windows.Forms.PictureBox()
        Me.Config = New System.Windows.Forms.PictureBox()
        CType(Me.UI, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CMS.SuspendLayout()
        CType(Me.Alert, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Config, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AFKDetector
        '
        Me.AFKDetector.Interval = 50000
        '
        'UI
        '
        resources.ApplyResources(Me.UI, "UI")
        Me.UI.Name = "UI"
        Me.UI.TabStop = False
        '
        'NotifyIcon
        '
        Me.NotifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        resources.ApplyResources(Me.NotifyIcon, "NotifyIcon")
        '
        'CMS
        '
        Me.CMS.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.CMS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RestaurarToolStripMenuItem, Me.FecharToolStripMenuItem})
        Me.CMS.Name = "CMS"
        resources.ApplyResources(Me.CMS, "CMS")
        '
        'RestaurarToolStripMenuItem
        '
        Me.RestaurarToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.RestaurarToolStripMenuItem.ForeColor = System.Drawing.Color.DeepPink
        Me.RestaurarToolStripMenuItem.Name = "RestaurarToolStripMenuItem"
        resources.ApplyResources(Me.RestaurarToolStripMenuItem, "RestaurarToolStripMenuItem")
        '
        'FecharToolStripMenuItem
        '
        Me.FecharToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.FecharToolStripMenuItem.ForeColor = System.Drawing.Color.DeepPink
        Me.FecharToolStripMenuItem.Name = "FecharToolStripMenuItem"
        resources.ApplyResources(Me.FecharToolStripMenuItem, "FecharToolStripMenuItem")
        '
        'CMSVazio
        '
        Me.CMSVazio.Name = "CMS"
        resources.ApplyResources(Me.CMSVazio, "CMSVazio")
        '
        'Alert
        '
        resources.ApplyResources(Me.Alert, "Alert")
        Me.Alert.Name = "Alert"
        Me.Alert.TabStop = False
        '
        'Config
        '
        resources.ApplyResources(Me.Config, "Config")
        Me.Config.Name = "Config"
        Me.Config.TabStop = False
        '
        'Main
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.Config)
        Me.Controls.Add(Me.Alert)
        Me.Controls.Add(Me.UI)
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Main"
        Me.ShowInTaskbar = False
        Me.TopMost = True
        Me.TransparencyKey = System.Drawing.Color.Transparent
        CType(Me.UI, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CMS.ResumeLayout(False)
        CType(Me.Alert, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Config, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents AFKDetector As Timer
    Friend WithEvents UI As PictureBox
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents NotifyIcon As NotifyIcon
    Friend WithEvents CMS As ContextMenuStrip
    Friend WithEvents CMSVazio As ContextMenuStrip
    Friend WithEvents RestaurarToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FecharToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Alert As PictureBox
    Friend WithEvents Config As PictureBox
End Class
