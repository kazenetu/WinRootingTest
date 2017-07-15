Public Class Form1
    ''' <summary>
    ''' フォームロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ラベルのイベント設定
        setHandler(Label1)
        setHandler(Label2)
        setHandler(Label3)
        setHandler(Label4)
        setHandler(Label5)
        setHandler(Label6)
        setHandler(Label7)
        setHandler(Label8)

    End Sub

    ''' <summary>
    ''' ラベルのマウスイベントを追加
    ''' </summary>
    ''' <param name="target"></param>
    Private Sub setHandler(target As Label)
        AddHandler target.MouseDown, AddressOf Label_MouseDown
        AddHandler target.MouseMove, AddressOf Label_MouseMove

    End Sub

    ''' <summary>
    ''' ラベル マウスクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Label_MouseDown(sender As Object, e As MouseEventArgs)
        Dim target As Label = DirectCast(sender, Label)
        target.Tag = e.Location

        ' ルーティングパスのクリア
        Me.clearRooting()
    End Sub

    ''' <summary>
    ''' ラベル マウスドラッグ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Label_MouseMove(sender As Object, e As MouseEventArgs)
        If Not e.Button = MouseButtons.Left Then
            Return
        End If

        Dim target As Label = DirectCast(sender, Label)
        Dim basePoint As Point = DirectCast(target.Tag, Point)
        target.Left = CInt((target.Left + e.Location.X - basePoint.X) / 50) * 50
        target.Top = CInt((target.Top + e.Location.Y - basePoint.Y) / 50) * 50
    End Sub

    ''' <summary>
    ''' ルーティング設定
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub showRooting_Click(sender As Object, e As EventArgs) Handles showRooting.Click
        Dim startLabel As Label = Label1

        ' アイテム追加
        Dim labels As New SortedList(Of Integer, SortedList(Of Integer, Label))
        For Each item As Control In Me.Controls
            If TypeOf item Is Label AndAlso Not item Is Label1 Then
                Dim label As Label = DirectCast(item, Label)
                Dim pos As Point = label.Location
                pos.X += label.Width / 2
                pos.Y += label.Height / 2

                If Not labels.ContainsKey(pos.Y) Then
                    labels.Add(pos.Y, New SortedList(Of Integer, Label))
                End If
                labels(pos.Y).Add(pos.X, item)
            End If
        Next

        ' ルーティングパスのクリア
        Me.clearRooting()

        'Draw
        Dim ptStart As Point = startLabel.Location
        ptStart.X += startLabel.Width / 2
        ptStart.Y += startLabel.Height / 2

        Dim ptEnd As Point

        Dim p As New Pen(Color.Black, 3)
        Using gr As Graphics = Me.CreateGraphics()
            gr.FillRectangle(New SolidBrush(Me.BackColor), New RectangleF(0, 0, Me.Width, Me.Height))

            For Each keyValue In labels
                For Each l As Label In keyValue.Value.Values
                    ptEnd = l.Location + New Point(l.Width / 2, l.Height / 2)

                    gr.DrawLine(p, ptStart, ptEnd)

                    ptStart = ptEnd
                Next
            Next




        End Using
    End Sub

    ''' <summary>
    ''' ルーティングパスのクリア
    ''' </summary>
    Private Sub clearRooting()
        Using gr As Graphics = Me.CreateGraphics()
            gr.FillRectangle(New SolidBrush(Me.BackColor), New RectangleF(0, 0, Me.Width, Me.Height))
        End Using
    End Sub
End Class
