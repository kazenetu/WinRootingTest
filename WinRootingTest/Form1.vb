Public Class Form1

    ''' <summary>
    ''' ルーティングモード
    ''' </summary>
    Private Enum RootingMode
        Auto = 0
        Left
        LeftToRight
        Right
    End Enum

    ''' <summary>
    ''' フォームロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ラベルのイベント設定
        setHandler(Label1)
        setHandler(Label2)

        ' コンボボックスの設定
        Me.ComboBox1.DisplayMember = "value"
        Me.ComboBox1.ValueMember = "key"
        Dim items As New List(Of KeyValuePair(Of RootingMode, String))
        items.Add(New KeyValuePair(Of RootingMode, String)(RootingMode.Auto, "自動"))
        items.Add(New KeyValuePair(Of RootingMode, String)(RootingMode.Left, "左端"))
        items.Add(New KeyValuePair(Of RootingMode, String)(RootingMode.LeftToRight, "左まわりで右端"))
        items.Add(New KeyValuePair(Of RootingMode, String)(RootingMode.Right, "右端"))
        Me.ComboBox1.DataSource = items

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
    ''' ラベルの追加
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub addLabel_Click(sender As Object, e As EventArgs) Handles addLabel.Click
        Dim newLabel As New Label()

        newLabel.Width = Label2.Width
        newLabel.Height = Label2.Height
        newLabel.BackColor = Label2.BackColor
        newLabel.ForeColor = Label2.ForeColor
        newLabel.Font = Label2.Font
        newLabel.Text = Label2.Text
        setHandler(newLabel)

        Me.Controls.Add(newLabel)

    End Sub

    ''' <summary>
    ''' ラベル マウスクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Label_MouseDown(sender As Object, e As MouseEventArgs)
        Dim target As Label = DirectCast(sender, Label)

        ' ルーティングパスのクリア
        Me.clearRooting()

        If e.Button = MouseButtons.Right Then
            If target IsNot Me.Label1 Then
                Me.Controls.Remove(target)
            End If
        Else
            target.Tag = e.Location
        End If
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
    ''' ルートの設定方向
    ''' </summary>
    Private Enum RootingDirection
        None
        All
        Holizontal
        Vertical
    End Enum

    ''' <summary>
    ''' 特殊ルートの設定フラグ
    ''' </summary>
    Private isSpecialMode As Boolean

    ''' <summary>
    ''' 常時左端からスタートフラグ
    ''' </summary>
    Private alwaysStartLeft As Boolean

    ''' <summary>
    ''' 中継点のみ対象フラグ
    ''' </summary>
    Private targetRelayPoints As Boolean

    ''' <summary>
    ''' Labelルーティングの生成
    ''' </summary>
    ''' <returns></returns>
    Private Function getRootingList(ByVal alwaysStartLeft As Boolean, ByVal targetRelayPoints As Boolean) As List(Of Label)
        Dim startLabel As Label = Label1

        ' ラベルリスト作成
        Dim labels As New List(Of Label)
        For Each item As Control In Me.Controls
            If TypeOf item Is Label AndAlso Not item Is Label1 Then
                labels.Add(item)
            End If
        Next

        ' 特殊ルート設定フラグをクリア
        Me.isSpecialMode = False

        ' 常時左端からスタートフラグを設定
        Me.alwaysStartLeft = alwaysStartLeft

        ' 中継点のみ対象フラグを設定
        Me.targetRelayPoints = targetRelayPoints

        ' 特殊ルート設定条件
        If labels.Min(Function(item) item.Location.X) <= startLabel.Location.X AndAlso
            labels.Max(Function(item) item.Location.X) >= startLabel.Location.X Then

            If labels.Min(Function(item) item.Location.Y) < startLabel.Location.Y AndAlso
                labels.Max(Function(item) item.Location.Y) > startLabel.Location.Y Then

                Me.isSpecialMode = True
            End If
        End If
        If Me.isSpecialMode Then
            Dim lines = labels.Select(Function(item) item.Location.Y).OrderBy((Function(itemY) itemY)).Distinct()
            If lines.Where(Function(itemY) itemY <= startLabel.Location.Y).Count Mod 2 = 1 Then
                ' 奇数の場合は特殊ルートを解除
                Me.isSpecialMode = False
            End If
        End If
        If Me.isSpecialMode Then
            Dim startYlines = labels.Where(Function(item) item.Location.Y = startLabel.Location.Y)
            If startYlines.Any() AndAlso startYlines.Max(Function(item) item.Location.X) < startLabel.Location.X Then
                ' 開始位置の右側に中継点がない場合は特殊ルートを解除
                Me.isSpecialMode = False
            End If
        End If

        ' 上段の左端と右端を取得する
        Dim minY As Integer = labels.Min(Function(item) item.Location.Y)
        Dim upperLineQuery = labels.Where(Function(item) item.Location.Y = minY)
        Dim leftX As Integer = upperLineQuery.Min(Function(i) i.Location.X)
        Dim rightX As Integer = upperLineQuery.Max(Function(i) i.Location.X)

        ' 特殊ルート設定確認と再設定
        If Not leftX = rightX AndAlso Me.isSpecialMode Then
            If startLabel.Location.X = rightX Then
                Me.isSpecialMode = False
            End If
        End If

        ' 最初に選択されるラベルを取得する
        Dim isReverce As Boolean = False
        Dim targetX As Integer
        If Me.isSpecialMode OrElse startLabel.Location.X > leftX + (rightX - leftX) / 2 Then
            targetX = rightX
            isReverce = True
        Else
            targetX = leftX
        End If

        ' 「常に左端」の場合は左端を選択
        If Me.alwaysStartLeft Then
            targetX = leftX
        End If

        ' 中継点のみ対象フラグの場合は左端を選択
        If Me.targetRelayPoints Then
            targetX = leftX
        End If

        ' デバッグ用：コンボボックスの設定値を反映
        Dim selectedValue As RootingMode = DirectCast(Me.ComboBox1.SelectedValue, RootingMode)
        Select Case selectedValue
            Case RootingMode.Left
                targetX = leftX
                Me.isSpecialMode = False
            Case RootingMode.LeftToRight
                targetX = rightX
                isReverce = True
                Me.isSpecialMode = True
            Case RootingMode.Right
                targetX = rightX
                isReverce = True
                Me.isSpecialMode = False
        End Select


        Dim target As Label = upperLineQuery.Where(Function(item) item.Location.X = targetX).First()

        ' リストを作成する
        Dim result As New List(Of Label)

        ' 「中継点のみ対象フラグ」以外の場合は開始位置をリストに追加
        If Not Me.targetRelayPoints Then
            result.Add(startLabel)
        End If

        ' 戻り値リストの追加と追加対象リストの削除
        result.Add(target)
        labels.Remove(target)

        ' 2つ目以降の経路選択
        While (labels.LongCount > 0)

            Dim direction As RootingDirection = RootingDirection.None
            Dim query As IEnumerable(Of Label) = Nothing

            ' 優先度1. 選択オブジェクトと比較して「左または右で上または同じ位置のオブジェクト」を探す（横方向）
            If isReverce Then
                query = labels.Where(Function(item) item.Location.X < target.Location.X AndAlso item.Location.Y <= target.Location.Y)
            Else
                query = labels.Where(Function(item) item.Location.X > target.Location.X AndAlso item.Location.Y <= target.Location.Y)
            End If
            If direction = RootingDirection.None AndAlso query.Any() Then
                direction = RootingDirection.Holizontal
            End If

            If Not query.Any() Then
                ' 優先度2. 選択オブジェクトと比較して「優先度1の逆方向のオブジェクト」を探す（横方向）
                If Not isReverce Then
                    query = labels.Where(Function(item) item.Location.X < target.Location.X AndAlso item.Location.Y <= target.Location.Y)
                Else
                    query = labels.Where(Function(item) item.Location.X > target.Location.X AndAlso item.Location.Y <= target.Location.Y)
                End If
            End If
            If direction = RootingDirection.None AndAlso query.Any() Then
                direction = RootingDirection.Holizontal
            End If

            If Not query.Any() Then
                ' 優先度3. 選択オブジェクトと比較して「左または右で下の位置のオブジェクト」を探す（縦方向）
                query = labels.Where(Function(item) item.Location.Y > target.Location.Y)
            End If
            If direction = RootingDirection.None AndAlso query.Any() Then
                direction = RootingDirection.Vertical
            End If

            If Not query.Any() Then
                ' 優先度4. すべてのオブジェクトを対象とする
                query = labels.Where(Function(item) True)
                direction = RootingDirection.All
            End If

            ' 絞り込んだ条件から一番近いオブジェクトを次の移動先オブジェクトとして選択する
            Dim oldX = target.Location.X
            Dim targetQuery = query.OrderBy(Function(item) item.Location.Y)
            If direction = RootingDirection.Vertical Then

                If Me.alwaysStartLeft Then
                    ' 「常に左端」の場合、常に左端を選択
                    targetQuery = targetQuery.ThenBy(Function(item) item.Location.X)
                Else
                    ' 状況に応じて左端・右端を選択
                    If isReverce Then
                        targetQuery = targetQuery.ThenBy(Function(item) item.Location.X)
                    Else
                        targetQuery = targetQuery.ThenByDescending(Function(item) item.Location.X)
                    End If
                End If
            Else
                targetQuery = targetQuery.ThenBy(Function(item) getDistance(item.Location, target.Location))
            End If
            target = targetQuery.First()

            ' 方向転換の判定
            If isReverce Then
                If target.Location.X >= oldX Then
                    isReverce = Not isReverce
                End If
            Else
                If target.Location.X <= oldX Then
                    isReverce = Not isReverce
                End If
            End If

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

        Dim checkedRootList As New List(Of Label)
        Dim targetPos As Point = Nothing
        Dim nextPos As Point = Nothing
        Dim targetLinePos As Point = Nothing

        Dim startIndex As Integer = 0
        If Me.targetRelayPoints Then
            ' 「中継点のみ対象フラグ」の場合は斜め線を右側に傾ける
            isRight = True
        Else
            ' 「中継点のみ対象フラグ」以外の場合は開始位置を作成
            result.Add(Me.createLineStart(rootList, spaceSize, isRight))
            Dim isRightStart As Boolean = isRight
            checkedRootList.Add(rootList(startIndex))
            startIndex += 1
        End If

        ' 中継点を作成
        For index As Integer = startIndex To rootList.Count - 2
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
            If Not isRight Then
                addX *= -1
            End If

            targetLinePos.X += addX
            targetLinePos.Y -= spaceSize
            listItem.Add(targetLinePos)

            ' 「緑線を表示」にチェックされていれば表示
            If Me.drawGreen.Checked Then
                ' 次のアイテムまでの線を描画
                If Not targetDir.Y = 0 Then
                    Dim isUp As Boolean = False

                    ' 横線を描画
                    Dim lineX As Integer = centerPos.X * 2
                    If targetDir.Y > 0 Then
                        ' 下から上へ
                        lineX = Math.Abs(targetDir.X) / 2
                        isUp = True
                    End If
                    Dim lineAddX = addX > 0
                    If Not isRight Then
                        lineAddX = Not lineAddX
                    End If

                    If isUp Then
                        targetLinePos.X += lineX
                    Else
                        lineX = centerPos.X * 2
                        If Not isRight Then
                            lineX *= -1
                        End If
                        targetLinePos.X += lineX

                        If isRight Then
                            If targetLinePos.X < nextPos.X Then
                                targetLinePos.X = nextPos.X
                            End If
                        Else
                            If targetLinePos.X > nextPos.X Then
                                targetLinePos.X = nextPos.X
                            End If
                        End If


                    End If

                    listItem.Add(targetLinePos)

                    If targetDir.X = 0 And False Then
                        ' 回り込む

                        isRight = lineAddX < 0

                        ' 縦線を描画
                        targetLinePos.Y = targetPos.Y + centerPos.Y * 2
                        listItem.Add(targetLinePos)

                        ' 次のアイテムまでの横線を描画
                        targetLinePos.X = nextPos.X
                        listItem.Add(targetLinePos)

                        ' 縦線を描画
                        targetLinePos.Y = nextPos.Y - spaceSize
                        listItem.Add(targetLinePos)

                    Else
                        ' 縦線を描画
                        If Me.alwaysStartLeft Then

                            ' 「常に左端」の場合
                            If targetDir.Y > 0 Then
                                targetLinePos.Y += (centerPos.Y + spaceSize) * -2
                            Else
                                targetLinePos.Y += (centerPos.Y + spaceSize) * 2
                            End If
                            targetLinePos.Y = nextPos.Y - (centerPos.Y * 2 - spaceSize)
                        Else
                            targetLinePos.Y += targetDir.Y * -1
                        End If
                        listItem.Add(targetLinePos)

                        If Not isUp Then
                            isRight = Not isRight
                        End If
                    End If
                Else
                    isRight = targetLinePos.X < nextPos.X
                End If

                ' 「常に左端」の場合は左端を選択
                If Me.alwaysStartLeft Then
                    isRight = True
                End If

                ' 次のアイテムまでの横線を描画
                targetLinePos.X = nextPos.X
                listItem.Add(targetLinePos)
                ' 短い縦線を描画
                targetLinePos.Y += spaceSize
                targetLinePos.Y = nextPos.Y + spaceSize
                listItem.Add(targetLinePos)
            End If

            checkedRootList.Add(rootList(index))
            result.Add(listItem)
        Next

        Return result
    End Function

    ''' <summary>
    ''' 開始位置から次のアイテムまでの線を描画
    ''' </summary>
    ''' <param name="rootList">アイテムの経路</param>
    ''' <param name="spaceSize">斜め線のサイズ</param>
    ''' <param name="isRight">右回りか否か</param>
    ''' <returns>描画対象リスト</returns>
    Private Function createLineStart(ByVal rootList As List(Of Label), ByVal spaceSize As Integer, ByRef isRight As Boolean) As List(Of Point)
        Dim result As New List(Of Point)
        Dim centerPos As New Point(rootList(0).Width / 2, rootList(0).Height / 2)

        Dim targetPos As Point = rootList(0).Location + centerPos
        Dim nextPos As Point = rootList(1).Location + centerPos
        Dim targetLinePos As Point = Nothing

        ' アイコン下部から開始
        targetLinePos = targetPos
        targetLinePos.Y += centerPos.Y
        result.Add(targetLinePos)

        ' 向きを計算
        Dim targetDir As Point = targetPos - nextPos
        Dim targetX = rootList(0).Location.X

        ' 開始位置を除外した中継点リストを取得する
        Dim query = rootList.Where(Function(item) Not item Is rootList(0))
        Dim minX = query.Min(Function(item) item.Location.X)
        Dim maxX = query.Max(Function(item) item.Location.X)

        ' 次のアイテムと同行の中継点リストを取得する
        Dim topQuery = query.Where(Function(item) item.Location.Y = rootList(1).Location.Y)

        ' 左側に線を引くか確認
        Dim isLineLeft As Boolean = True
        If Me.alwaysStartLeft Then

            ' 「常に左端」で開始位置右端の場合は右側にラインを引く
            If topQuery.Max(Function(item) item.Location.X) < targetX Then
                isLineLeft = False
            End If
        Else
            If topQuery.Count > 1 Then
                If topQuery.Max(Function(item) item.Location.X) = rootList(1).Location.X Then
                    isLineLeft = False
                End If
            Else
                If topQuery.Max(Function(item) item.Location.X) < targetX Then
                    isLineLeft = False
                End If
            End If
        End If

        ' 斜め線を追加
        Dim addX = spaceSize
        If targetDir.Y = 0 Then
            If targetDir.X > 0 Then
                addX *= -1
            End If
        Else
            If targetX <= minX + (maxX - minX) / 2 Then
                addX *= -1
            End If
        End If

        If targetDir.X < 0 Then
            isRight = True
        Else
            isRight = False
        End If

        targetLinePos.X += addX
        targetLinePos.Y += spaceSize
        result.Add(targetLinePos)

        ' 「緑線を表示」にチェックされていれば表示
        If Me.drawGreen.Checked Then
            Dim baseZero = 0

            ' 「常に左端」の場合は半分移動分まで許容する
            If Me.alwaysStartLeft Then
                baseZero = -50
            End If

            ' 高さが異なる場合は迂回する
            If targetDir.Y >= baseZero Then

                If Me.isSpecialMode Then

                    ' 折り返し線：特殊ルート：横線
                    Dim nextQuery = rootList.Where(Function(item) item.Location.Y <= rootList(0).Location.Y)
                    targetLinePos.X = nextQuery.Min(Function(item) item.Location.X) - centerPos.X * 2
                    result.Add(targetLinePos)

                    ' 特殊ルート：縦線を描画
                    targetLinePos.Y = nextPos.Y - (spaceSize + centerPos.Y * 2)
                    result.Add(targetLinePos)

                    isRight = False
                Else
                    ' 折り返し線：横線
                    If isLineLeft Then
                        ' 次のアイテムは左端
                        Dim leftQuery = rootList.Where(Function(item) item.Location.Y <= rootList(0).Location.Y)
                        targetLinePos.X = leftQuery.Min(Function(item) item.Location.X) - centerPos.X * 2

                        isRight = True
                    Else
                        ' 次のアイテムは右端
                        Dim rightQuery = rootList.Where(Function(item) item.Location.Y <= rootList(0).Location.Y)
                        targetLinePos.X = rightQuery.Max(Function(item) item.Location.X) + centerPos.X * 4

                        isRight = False
                    End If
                    result.Add(targetLinePos)

                    ' 縦線を描画
                    targetLinePos.Y = nextPos.Y - (spaceSize + centerPos.Y)
                    If Me.alwaysStartLeft Then
                        If targetDir.Y < 0 Then
                            targetLinePos.Y = targetPos.Y - (spaceSize + centerPos.Y)
                        ElseIf Not isLineLeft Then
                            targetLinePos.Y -= spaceSize
                        End If
                    End If

                    result.Add(targetLinePos)
                End If

            Else
                If minX <= targetX AndAlso targetX <= maxX Then

                    If targetDir.X = 0 Then
                        ' 縦線を描画
                        targetLinePos.Y = nextPos.Y - (spaceSize + centerPos.Y)
                        result.Add(targetLinePos)
                    End If

                    If addX < 0 Then
                        isRight = True
                    Else
                        isRight = False
                    End If
                Else

                    ' 縦線を描画
                    If Me.alwaysStartLeft Then
                        targetLinePos.Y = targetPos.Y + (spaceSize + centerPos.Y）
                        result.Add(targetLinePos)
                    Else
                        targetLinePos.Y = nextPos.Y - (spaceSize + centerPos.Y)
                        result.Add(targetLinePos)
                    End If

                    If targetDir.X = 0 Then
                        isRight = Not isRight
                    End If
                End If
            End If

            ' 次のアイテムまでの横線を描画
            targetLinePos.X = nextPos.X
            result.Add(targetLinePos)
            ' 短い縦線を描画
            targetLinePos.Y = nextPos.Y + spaceSize
            result.Add(targetLinePos)
        End If

        ' 「常に左端」の場合は右に設定
        If Me.alwaysStartLeft Then
            isRight = True
        End If


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
        Dim rootings As List(Of Label) = Me.getRootingList(Me.alwaysTurnLeft.Checked, Me.startRelayPoint.Checked)
        If rootings.Count < 2 Then
            Return
        End If

        ' 描画処理
        Dim ptStart As Point = rootings(0).Location
        ptStart.X += rootings(0).Width / 2
        ptStart.Y += rootings(0).Height / 2
        Dim ptEnd As Point

        Dim p As New Pen(Color.Black, 3)
        Using gr As Graphics = Me.CreateGraphics()

            ' 経路描画
            For i As Integer = 1 To rootings.LongCount - 1
                ptEnd = rootings(i).Location
                ptEnd.X += rootings(i).Width / 2
                ptEnd.Y += rootings(i).Height / 2

                gr.DrawLine(p, ptStart, ptEnd)

                ptStart = ptEnd
            Next

            ' 描画ライン
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
