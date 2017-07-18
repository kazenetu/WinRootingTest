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
        Dim leftX As Integer = upperLineQuery.Min(Function(i) i.Location.X)
        Dim rightX As Integer = upperLineQuery.Max(Function(i) i.Location.X)

        ' 最初に選択されるラベルを取得する
        Dim isReverce As Boolean = False
        Dim targetX As Integer
        If startLabel.Location.X > leftX + (rightX - leftX) / 2 Then
            targetX = rightX
            isReverce = True
        Else
            targetX = leftX
        End If
        Dim target As Label = upperLineQuery.Where(Function(item) item.Location.X = targetX).First()

        ' リストを作成する
        Dim result As New List(Of Label)
        result.Add(startLabel)

        ' 戻り値リストの追加と追加対象リストの削除
        result.Add(target)
        labels.Remove(target)

        ' 2つ目以降の経路選択
        While (labels.LongCount > 0)

            Dim query As IEnumerable(Of Label) = Nothing

            ' 優先度1. 選択オブジェクトと比較して「左または右で上または同じ位置のオブジェクト」を探す
            If isReverce Then
                query = labels.Where(Function(item) item.Location.X < target.Location.X AndAlso item.Location.Y <= target.Location.Y)
            Else
                query = labels.Where(Function(item) item.Location.X > target.Location.X AndAlso item.Location.Y <= target.Location.Y)
            End If

            If Not query.Any() Then
                ' 優先度2. 選択オブジェクトと比較して「左または右のオブジェクト」を探す
                If isReverce Then
                    query = labels.Where(Function(item) item.Location.X < target.Location.X)
                Else
                    query = labels.Where(Function(item) item.Location.X > target.Location.X)
                End If
            End If
            If Not query.Any() Then
                ' 優先度3. すべてのオブジェクトを対象とする
                query = labels.Where(Function(item) True)
            End If

            ' 絞り込んだ条件から一番近いオブジェクトを次の移動先オブジェクトとして選択する
            target = query.OrderBy(Function(item) getDistance(item.Location, target.Location)).First()

            ' 戻り値リストの追加と追加対象リストの削除
            result.Add(target)
            labels.Remove(target)
        End While


        Return result
    End Function

    ''' <summary>
    ''' 描画ラインを生成、取得する
    ''' </summary>
    ''' <param name="rootList">アイテムの経路</param>
    ''' <returns>描画対象リスト</returns>
    Private Function createLines(ByVal rootList As List(Of Label)) As List(Of List(Of Point))
        Dim result As New List(Of List(Of Point))

        Dim centerPos As New Point(rootList(0).Width / 2, rootList(0).Height / 2)
        Dim spaceSize As Integer = 10

        Dim isRight As Boolean = False

        Dim targetPos As Point = Nothing
        Dim nextPos As Point = Nothing
        Dim targetLinePos As Point = Nothing

        ' 開始を作成
        result.Add(Me.createLineStart(rootList, spaceSize, isRight))
        Dim isRightStart As Boolean = isRight

        ' 中継点を作成
        For index As Integer = 1 To rootList.Count - 2
            Dim listItem As New List(Of Point)

            targetPos = rootList(index).Location + centerPos
            nextPos = rootList(index + 1).Location + centerPos

            ' 描画開始位置のデフォルト(アイコン上部からスタート)
            targetLinePos = targetPos
            targetLinePos.Y -= centerPos.Y
            listItem.Add(targetLinePos)

            Dim targetDir As Point = targetPos - nextPos

            ' 斜め線を追加
            Dim addX = spaceSize
            If Not isRightStart Then
                addX *= -1
            End If
            If isRightStart Then
                If targetDir.X > 0 Then
                    If Math.Abs(targetDir.Y) <= centerPos.Y * 2 Then
                        addX *= -1
                    Else
                        If isRight <> isRightStart Then
                            addX *= -1
                        End If
                    End If
                End If
            Else
                If targetDir.X < 0 Then
                    If Math.Abs(targetDir.Y) <= centerPos.Y * 2 Then
                        addX *= -1
                    Else
                        If isRight <> isRightStart Then
                            addX *= -1
                        End If
                    End If
                End If
            End If

            targetLinePos.X += addX
            targetLinePos.Y -= spaceSize
            listItem.Add(targetLinePos)
            isRight = targetDir.X < 0

            ' 次のアイテムまでの線を描画
            If Not targetDir.Y = 0 Then
                ' 横線を描画
                Dim lineX As Integer = centerPos.X * 2
                If targetDir.Y > 0 Then
                    ' 下から上へ
                    lineX = centerPos.X * 1
                End If
                If addX < 0 Then
                    lineX *= -1
                End If
                targetLinePos.X += lineX
                listItem.Add(targetLinePos)

                ' 縦線を描画
                targetLinePos.Y += targetDir.Y * -1
                listItem.Add(targetLinePos)
            End If
            ' 次のアイテムまでの横線を描画
            targetLinePos.X = nextPos.X
            listItem.Add(targetLinePos)
            ' 短い縦線を描画
            targetLinePos.Y += spaceSize
            listItem.Add(targetLinePos)

            result.Add(listItem)
        Next

        Return result
    End Function

    Private Function createLineStart(ByVal rootList As List(Of Label), ByVal spaceSize As Integer, ByRef isRight As Boolean) As List(Of Point)
        Dim result As New List(Of Point)
        Dim centerPos As New Point(rootList(0).Width / 2, rootList(0).Height / 2)

        Dim targetPos As Point = rootList(0).Location + centerPos
        Dim nextPos As Point = rootList(1).Location + centerPos
        Dim targetLinePos As Point = Nothing

        ' 描画開始位置のデフォルト(アイコン上部からスタート)
        targetLinePos = targetPos
        targetLinePos.Y -= centerPos.Y

        Dim targetDir As Point = targetPos - nextPos

        If targetDir.X < 0 Then
            isRight = True
        End If
        If targetDir.Y < 0 Then
            ' 次のアイコンが下にある場合、アイコン下部から開始
            targetLinePos.Y += centerPos.Y * 2
        End If
        result.Add(targetLinePos)

        ' 斜め線を追加
        Dim addX = spaceSize
        If targetDir.Y = 0 Then
            If targetDir.X > 0 Then
                addX *= -1
            End If
        Else
            If targetDir.X > 0 Then
                addX *= -1
            End If
        End If
        targetLinePos.X += addX
        If targetDir.Y < 0 Then
            ' アイコン下部から開始
            targetLinePos.Y += spaceSize
        Else
            targetLinePos.Y -= spaceSize
        End If
        result.Add(targetLinePos)

        ' 次のアイテムまでの線を描画
        If Not targetDir.Y = 0 Then
            ' 縦線を描画
            targetLinePos.Y = nextPos.Y - (spaceSize + centerPos.Y)
            result.Add(targetLinePos)
        End If
        ' 次のアイテムまでの横線を描画
        targetLinePos.X = nextPos.X
        result.Add(targetLinePos)
        ' 短い縦線を描画
        targetLinePos.Y += spaceSize
        result.Add(targetLinePos)

        Return result
    End Function


    ''' <summary>
    ''' 二点の距離を計算・取得する
    ''' </summary>
    ''' <param name="srcFrom">対象位置From</param>
    ''' <param name="srcTo">対象位置To</param>
    ''' <returns></returns>
    Private Function getDistance(ByVal srcFrom As Point, ByVal srcTo As Point) As Double
        Dim targetPos As Point = srcFrom - srcTo
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

            Dim pg As New Pen(Color.LawnGreen, 2)
            Dim lines = Me.createLines(rootings)

            For Each items In lines
                ptStart = items(0)
                For i As Integer = 1 To items.LongCount - 1
                    ptEnd = items(i)

                    gr.DrawLine(pg, ptStart, ptEnd)

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
