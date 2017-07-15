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
    ''' Labelルーティングの生成
    ''' </summary>
    ''' <returns></returns>
    Private Function getRootingList() As List(Of Label)
        Dim startLabel As Label = Label1

        ' ラベルリスト作成
        Dim labels As New List(Of Label)
        For Each item As Control In Me.Controls
            If TypeOf item Is Label AndAlso Not item Is Label1 Then
                labels.Add(item)
            End If
        Next

        ' 上段の左端と右端を取得する
        Dim minY As Integer = labels.Min(Function(item) item.Location.Y)
        Dim upperLineQuery = labels.Where(Function(item) item.Location.Y = minY)
        Dim minX As Integer = upperLineQuery.Min(Function(i) i.Location.X)
        Dim maxX As Integer = upperLineQuery.Max(Function(i) i.Location.X)

        ' 対象のラベルを選択する
        Dim targetX As Integer
        If startLabel.Location.X > minX + (maxX - minX) / 2 Then
            targetX = maxX
        Else
            targetX = minX
        End If
        Dim target As Label = upperLineQuery.Where(Function(item) item.Location.X = targetX).First()

        ' リストを作成する
        Dim result As New List(Of Label)
        result.Add(startLabel)

        ' 戻り値リストの追加と追加対象リストの削除
        result.Add(target)
        labels.Remove(target)

        ' 経路選択
        While (labels.LongCount > 0)
            target = labels.OrderBy(Function(item) getDistance(item.Location, target.Location)).First()

            ' 戻り値リストの追加と追加対象リストの削除
            result.Add(target)
            labels.Remove(target)
        End While


        Return result
    End Function

    Private Function getDistance(ByVal srcPos As Point, ByVal descPos As Point) As Double
        Dim targetPos As Point = srcPos - descPos
        Return Math.Sqrt((targetPos.X * targetPos.X) + (targetPos.Y * targetPos.Y))
    End Function

    ''' <summary>
    ''' ルーティング設定
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub showRooting_Click(sender As Object, e As EventArgs) Handles showRooting.Click
        ' ルーティングパスのクリア
        Me.clearRooting()

        ' ルーティングリストを取得
        Dim rootings As List(Of Label) = Me.getRootingList()

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
