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

        Dim minX As Integer = Integer.MaxValue
        Dim maxX As Integer = Integer.MinValue
        Dim minY As Integer = Integer.MaxValue
        Dim maxY As Integer = Integer.MinValue

        ' アイテム追加
        Dim labels As New SortedList(Of Integer, SortedList(Of Integer, Label))
        For Each item As Control In Me.Controls
            If TypeOf item Is Label AndAlso Not item Is Label1 Then
                Dim label As Label = DirectCast(item, Label)
                Dim pos As Point = label.Location
                pos.X += label.Width / 2
                pos.Y += label.Height / 2

                If minX > pos.X Then
                    minX = pos.X
                End If
                If maxX < pos.X Then
                    maxX = pos.X
                End If
                If minY > pos.Y Then
                    minY = pos.Y
                End If
                If maxY < pos.Y Then
                    maxY = pos.Y
                End If

                If Not labels.ContainsKey(pos.Y) Then
                    labels.Add(pos.Y, New SortedList(Of Integer, Label))
                End If
                labels(pos.Y).Add(pos.X, item)
            End If
        Next

        ' ルーティングパスの作成
        Dim pt As Point = startLabel.Location
        pt.X += startLabel.Width / 2
        pt.Y += startLabel.Height / 2
        Dim rootings As New List(Of Label)
        Dim isReverse As Boolean = False
        If pt.X > minX + (maxX - minX) / 2 Then
            isReverse = True
        End If
        For Each keyValue In labels
            Dim target = keyValue.Value.OrderBy(Function(item) item.Key).ToList()
            If isReverse Then
                target = keyValue.Value.OrderByDescending(Function(item) item.Key).ToList()
            End If

            For Each item In target
                rootings.Add(item.Value)
            Next
            isReverse = Not isReverse
        Next
        rootings.Insert(0, startLabel)


        ' ルーティングパスのクリア
        Me.clearRooting()

        'Draw
        Dim ptStart As Point = rootings(0).Location
        ptStart.X += rootings(0).Width / 2
        ptStart.Y += rootings(0).Height / 2
        Dim ptEnd As Point

        Dim p As New Pen(Color.Black, 3)
        Using gr As Graphics = Me.CreateGraphics()

            For i As Integer = 1 To rootings.LongCount - 1
                ptEnd = rootings(i).Location
                ptEnd.X += rootings(i).Width / 2
                ptEnd.Y += rootings(i).Height / 2

                gr.DrawLine(p, ptStart, ptEnd)

                ptStart = ptEnd
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
