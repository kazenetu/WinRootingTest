Public Class Form1

    Private Sub Label1_MouseDown(sender As Object, e As MouseEventArgs) Handles Label1.MouseDown
        Dim target As Label = DirectCast(sender, Label)
        target.Tag = e.Location
    End Sub

    Private Sub Label_MouseMove(sender As Object, e As MouseEventArgs) Handles Label1.MouseMove
        If Not e.Button = MouseButtons.Left Then
            Return
        End If

        Dim target As Label = DirectCast(sender, Label)
        Dim basePoint As Point = DirectCast(target.Tag, Point)
        target.Left = CInt((target.Left + e.Location.X - basePoint.X) / 50) * 50
        target.Top = CInt((target.Top + e.Location.Y - basePoint.Y) / 50) * 50
    End Sub

End Class
