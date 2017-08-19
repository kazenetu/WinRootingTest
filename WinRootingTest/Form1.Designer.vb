<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.addLabel = New System.Windows.Forms.Button()
        Me.drawGreen = New System.Windows.Forms.CheckBox()
        Me.showRooting = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Panel1.SuspendLayout()
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
        'Panel1
        '
        Me.Panel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.ComboBox1)
        Me.Panel1.Controls.Add(Me.addLabel)
        Me.Panel1.Controls.Add(Me.drawGreen)
        Me.Panel1.Controls.Add(Me.showRooting)
        Me.Panel1.Location = New System.Drawing.Point(523, 19)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(167, 191)
        Me.Panel1.TabIndex = 10
        '
        'addLabel
        '
        Me.addLabel.Location = New System.Drawing.Point(34, 3)
        Me.addLabel.Name = "addLabel"
        Me.addLabel.Size = New System.Drawing.Size(96, 23)
        Me.addLabel.TabIndex = 12
        Me.addLabel.Text = "中継点の追加"
        Me.addLabel.UseVisualStyleBackColor = True
        '
        'drawGreen
        '
        Me.drawGreen.AutoSize = True
        Me.drawGreen.Checked = True
        Me.drawGreen.CheckState = System.Windows.Forms.CheckState.Checked
        Me.drawGreen.Location = New System.Drawing.Point(34, 119)
        Me.drawGreen.Name = "drawGreen"
        Me.drawGreen.Size = New System.Drawing.Size(81, 16)
        Me.drawGreen.TabIndex = 11
        Me.drawGreen.Text = "緑線を表示"
        Me.drawGreen.UseVisualStyleBackColor = True
        '
        'showRooting
        '
        Me.showRooting.Location = New System.Drawing.Point(24, 77)
        Me.showRooting.Name = "showRooting"
        Me.showRooting.Size = New System.Drawing.Size(109, 23)
        Me.showRooting.TabIndex = 10
        Me.showRooting.Text = "ルーティング表示"
        Me.showRooting.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(12, 151)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(121, 20)
        Me.ComboBox1.TabIndex = 13
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(702, 447)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents addLabel As Button
    Friend WithEvents drawGreen As CheckBox
    Friend WithEvents showRooting As Button
    Friend WithEvents ComboBox1 As ComboBox
End Class
