Imports System.ComponentModel

Partial Public Class AngularMaterialDateBox
    Inherits System.Web.UI.UserControl
    Public ctrl As String
    Public data As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ctrl = getClientID()
        txtDate.Attributes.Add("style", "display: none")
    End Sub
    'Get the id of the control rendered on client side
    ' Very essential for Angular Component to locate the textbox
    Public Function getClientID() As String
        Return txtDate.ClientID
    End Function



    Public Property text() As String
        Get
            If IsDate(txtDate.Text) Then
                Return Format(CDate(txtDate.Text), "MM/dd/yyyy")
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            If IsDate(Value) Then
                txtDate.Text = Format(CDate(Value), "MM/dd/yyyy")
                Me.data = txtDate.Text
            Else
                txtDate.Text = Value
                Me.data = txtDate.Text
            End If
            Dim datMyDate As DateTime
            'If (DateTime.TryParseExact(Value, DateFormat, DBNull.Value, System.Globalization.DateTimeStyles.None, datMyDate)) Then
            If (datMyDate.Date = DateTime.MaxValue.Date) Then
                txtDate.Text = ""
            End If
            ' End If
        End Set
    End Property

    Public Property Enabled() As Boolean
        Get
        End Get
        Set(ByVal value As Boolean)
            txtDate.Enabled = value

        End Set
    End Property

    Public Event DateChanged As EventHandler
    Protected Sub txtDate_OnTextChanged(sender As Object, e As EventArgs)
        RaiseEvent DateChanged(Me, e)
    End Sub
End Class