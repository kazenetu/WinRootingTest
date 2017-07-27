<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.showRooting = New System.Windows.Forms.Button()
        Me.drawGreen = New System.Windows.Forms.CheckBox()
        Me.addLabel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.DimGray
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(50, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 60)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "開始位置"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.DimGray
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(100, 150)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 60)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "中継点"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'showRooting
        '
        Me.showRooting.Location = New System.Drawing.Point(528, 12)
        Me.showRooting.Name = "showRooting"
        Me.showRooting.Size = New System.Drawing.Size(109, 23)
        Me.showRooting.TabIndex = 7
        Me.showRooting.Text = "ルーティング表示"
        Me.showRooting.UseVisualStyleBackColor = True
        '
        'drawGreen
        '
        Me.drawGreen.AutoSize = True
        Me.drawGreen.Checked = True
        Me.drawGreen.CheckState = System.Windows.Forms.CheckState.Checked
        Me.drawGreen.Location = New System.Drawing.Point(538, 54)
        Me.drawGreen.Name = "drawGreen"
        Me.drawGreen.Size = New System.Drawing.Size(81, 16)
        Me.drawGreen.TabIndex = 8
        Me.drawGreen.Text = "緑線を表示"
        Me.drawGreen.UseVisualStyleBackColor = True
        '
        'addLabel
        '
        Me.addLabel.Location = New System.Drawing.Point(404, 12)
        Me.addLabel.Name = "addLabel"
        Me.addLabel.Size = New System.Drawing.Size(96, 23)
        Me.addLabel.TabIndex = 9
        Me.addLabel.Text = "中継点の追加"
        Me.addLabel.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(702, 447)
        Me.Controls.Add(Me.addLabel)
        Me.Controls.Add(Me.drawGreen)
        Me.Controls.Add(Me.showRooting)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents showRooting As Button
    Friend WithEvents drawGreen As CheckBox
    Friend WithEvents addLabel As Button
End Class
