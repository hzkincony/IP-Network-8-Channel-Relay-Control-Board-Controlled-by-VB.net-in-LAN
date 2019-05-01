Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text
Imports System.Windows.Forms
Imports System.ComponentModel
Imports Microsoft.VisualBasic.Strings
Imports System.IO.Ports






Public Class Form1
    Dim InByte() As Byte
    Public mode As Integer
    Public g_delay As Integer
    Public pos1 As Integer
    Public pos2 As Integer
    Public input As Byte
    Dim MyThread As Thread
    Dim MyThreadCom As Thread
    Public MySocket As Socket
    Dim g_Connect As Byte
    Public MyChattingState As Boolean
    Public MyChattingStateCom As Boolean
    Dim MyInfo As ASCIIEncoding
    Dim uni As UnicodeEncoding
    Dim MySendByte() As System.Byte
    Dim MySendString As String
    Dim MyReceivedByte(60) As System.Byte
    Dim i As Integer, flag As Integer
    Dim MyReceivedString As String
    Dim readinput As String
    Public RS232 As SerialPort
    Dim mBaudRate As Integer
    Dim mParity As IO.Ports.Parity
    Dim mDataBit As Integer
    Dim mStopbit As IO.Ports.StopBits
    Dim mPortName As String
    Public data(20) As Byte '串口发送字节
    Public senddata As String
    Dim rs232ReceiveSring As String


    Private Sub SetConnectButton()
        Select Case g_Connect
            Case 0 '连接已断开
                Button1.Enabled = True
                TextBox2.Enabled = True
                TextBox1.Enabled = True
                Com_Box.Enabled = True
                Button3.Enabled = True





                GroupBox9.Enabled = False





                Button1.Text = "Connect(&Q)"
                Label3.Text = "Disconnect"
                mode = 0
            Case 1 '连接成功
                Button1.Enabled = True
                TextBox2.Enabled = False
                TextBox1.Enabled = False
                'GroupBox7.Enabled = True
                'GroupBox6.Enabled = True
                'GroupBox5.Enabled = True
                'GroupBox4.Enabled = True
                ' GroupBox3.Enabled = True
                ' GroupBox8.Enabled = True
                GroupBox9.Enabled = True
                Com_Box.Enabled = False
                Button3.Enabled = False
                mode = 1
                ' GroupBox12.Enabled = True
                Button1.Text = "Disconnect(&N)"
                Label3.Text = "Connect"
            Case 2 '正在连接
                Button1.Enabled = False
                TextBox2.Enabled = False
                TextBox1.Enabled = False
                Com_Box.Enabled = False
                Button3.Enabled = False
                Button1.Text = "Connect(&Q)"
                Label3.Text = "Connect Now"
        End Select
    End Sub
    Private Function change(ByVal s As String) As String
        change = AscW(s)
        If change < 0 Then
            change = change + 65536
        End If
    End Function
    Public Sub MyChatProcessCom()
        Dim ReadCount As Integer
        If mode = 2 Then
            Do While (MyChattingStateCom)
                'Dim rx_relay As Integer, rx As Byte

                If RS232.BytesToRead > 0 Then

                    ReDim InByte(RS232.BytesToRead - 1) '声明阵列空间
                    '呼叫Read方法
                    ReadCount = RS232.Read(InByte, 0, RS232.BytesToRead)
                    rs232ReceiveSring = Encoding.ASCII.GetString(InByte).Trim()
                    TextBox5.Text = TextBox5.Text + rs232ReceiveSring
                    TextBox5.Text = TextBox5.Text + vbCrLf
                    TextBox5.ScrollToCaret()

                    If String.Compare(rs232ReceiveSring, 0, "RELAY-GET_INPUT-", 0, 12) = 0 Then

                        pos1 = InStr(1, rs232ReceiveSring, ",")
                        pos2 = InStr(pos1 + 1, rs232ReceiveSring, ",")
                        pos2 = pos2 - pos1 - 1
                        readinput = Mid(rs232ReceiveSring, pos1 + 1, pos2)
                        input = Val(readinput)
                        If (input And &H1) = 0 Then
                            CheckBox33.Checked = True
                        Else
                            CheckBox33.Checked = False
                        End If
                        If (input And &H2) = 0 Then
                            CheckBox34.Checked = True
                        Else
                            CheckBox34.Checked = False
                        End If
                        If (input And &H4) = 0 Then
                            CheckBox35.Checked = True
                        Else
                            CheckBox35.Checked = False
                        End If
                        If (input And &H8) = 0 Then
                            CheckBox36.Checked = True
                        Else
                            CheckBox36.Checked = False
                        End If
                        If (input And &H10) = 0 Then
                            CheckBox37.Checked = True
                        Else
                            CheckBox37.Checked = False
                        End If
                        If (input And &H20) = 0 Then
                            CheckBox38.Checked = True
                        Else
                            CheckBox38.Checked = False
                        End If
                        If (input And &H40) = 0 Then
                            CheckBox39.Checked = True
                        Else
                            CheckBox39.Checked = False
                        End If
                        If (input And &H80) = 0 Then
                            CheckBox40.Checked = True
                        Else
                            CheckBox40.Checked = False
                        End If
                    End If
                End If
            Loop
        End If
    End Sub

    Public Sub MyChatProcess()

        If MySocket.Connected Then
            Do While (MyChattingState)
                MySocket.Receive(MyReceivedByte, MyReceivedByte.Length, SocketFlags.None)
                MyReceivedString = Encoding.ASCII.GetString(MyReceivedByte).Trim()
                Me.TextBox5.Text = TextBox5.Text + MyReceivedString + vbNewLine
                Me.TextBox5.Text = TextBox5.Text + vbNewLine
                TextBox5.ScrollToCaret()

                If String.Compare(MyReceivedString, 0, "EVENT-RUN-OK", 0, 12) = 0 Then





                End If
                If String.Compare(MyReceivedString, 0, "RFSTUY_315M-STUDY-OK", 0, 21) = 0 Then

                    Thread.Sleep(2000)

                End If

                If String.Compare(MyReceivedString, 0, "RFSTUY_433M-STUDY-OK", 0, 21) = 0 Then

                    Thread.Sleep(2000)

                End If
                If String.Compare(MyReceivedString, 0, "RF1101-READ-", 0, 12) = 0 Then
                    If String.Compare(MyReceivedString, 14, "DS18B20", 0, 7) = 0 Then


                    End If
                    If String.Compare(MyReceivedString, 14, "DHT11", 0, 5) = 0 Then


                    End If
                End If
                If String.Compare(MyReceivedString, 0, "RF1101-READ-", 0, 12) = 0 Then
                    If String.Compare(MyReceivedString, 14, "DS18B20", 0, 7) = 0 Then


                    End If

                End If
                If String.Compare(MyReceivedString, 0, "RELAY-GET_INPUT-", 0, 12) = 0 Then

                    pos1 = InStr(1, MyReceivedString, ",")
                    pos2 = InStr(pos1 + 1, MyReceivedString, ",")
                    pos2 = pos2 - pos1 - 1
                    readinput = Mid(MyReceivedString, pos1 + 1, pos2)
                    input = Val(readinput)
                    If (input And &H1) = 0 Then
                        CheckBox33.Checked = True
                    Else
                        CheckBox33.Checked = False
                    End If
                    If (input And &H2) = 0 Then
                        CheckBox34.Checked = True
                    Else
                        CheckBox34.Checked = False
                    End If
                    If (input And &H4) = 0 Then
                        CheckBox35.Checked = True
                    Else
                        CheckBox35.Checked = False
                    End If
                    If (input And &H8) = 0 Then
                        CheckBox36.Checked = True
                    Else
                        CheckBox36.Checked = False
                    End If
                    If (input And &H10) = 0 Then
                        CheckBox37.Checked = True
                    Else
                        CheckBox37.Checked = False
                    End If
                    If (input And &H20) = 0 Then
                        CheckBox38.Checked = True
                    Else
                        CheckBox38.Checked = False
                    End If
                    If (input And &H40) = 0 Then
                        CheckBox39.Checked = True
                    Else
                        CheckBox39.Checked = False
                    End If
                    If (input And &H80) = 0 Then
                        CheckBox40.Checked = True
                    Else
                        CheckBox40.Checked = False
                    End If
                End If
                MySendString = ""
                For i = 0 To 60
                    MyReceivedByte(i) = 0
                Next i
            Loop
        End If
    End Sub

    Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Call SaveSetting("Myapp", "ConFig", "ipfile", TextBox1.Text)
        Call SaveSetting("Myapp", "ConFig", "portfile", TextBox2.Text)

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mode = 0
        g_Connect = 0
        MyChattingState = True
        Control.CheckForIllegalCrossThreadCalls = False




        GroupBox9.Enabled = False


        MyInfo = New ASCIIEncoding()
        TextBox1.Text = GetSetting("Myapp", "ConFig", "ipfile")
        TextBox2.Text = GetSetting("Myapp", "ConFig", "portfile")

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Control.CheckForIllegalCrossThreadCalls = False
        MyInfo = New ASCIIEncoding()
        For Each sp As String In SerialPort.GetPortNames()
            Com_Box.Items.Add(sp)
        Next
        Com_Box.Sorted = True  '排序
        If Com_Box.Items.Count = 0 Then
            MsgBox("No serial detected")
        Else
            Com_Box.SelectedIndex = 0 '第一个是默认选项
            mPortName = Com_Box.SelectedItem.ToString   '欲打开的通信端口 
            mBaudRate = "115200"   '通信速度
            mParity = "0" '同位位检查设定
            mDataBit = "8"    '数据位设定值
            mStopbit = "1"  '停止位设定值
            RS232 = New IO.Ports.SerialPort(mPortName, mBaudRate, mParity, mDataBit, mStopbit)
        End If

    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If g_Connect = 0 Then
            g_Connect = 2
            SetConnectButton()
            Try

                uni = New UnicodeEncoding()
                MySocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                MySocket.Connect(IPAddress.Parse(TextBox1.Text), Int32.Parse(TextBox2.Text))
                Dim MyDelegate As New ThreadStart(AddressOf MyChatProcess)
                MyThread = New Thread(MyDelegate)
                MyChattingState = True
                MyThread.Start()
                g_Connect = 1
                Button11.Enabled = True
                SetConnectButton()
            Catch ex As Exception
                g_Connect = 0
                SetConnectButton()



            End Try
        Else
            g_Connect = 0
            SetConnectButton()
            MyThread.Abort()
            Button11.Enabled = False
            If MySocket IsNot Nothing Then
                MySocket.Close()
            End If
        End If
    End Sub



    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "RFSTUY_315M-SEND-1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        TextBox5.Text = ""
        TextBox17.Text = ""
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "PT2262_315M-WRITE-3,12,262128"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
        Thread.Sleep(1000)
        MySendString = "PT2262_315M-SEND-3"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "PT2262_315M-WRITE-4,12,3211248"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
        Thread.Sleep(1000)
        MySendString = "PT2262_315M-SEND-4"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub
    Private Sub TextBox10_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Then '限制输入的必须为数字
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub
    Private Sub TextBox9_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "EVENT-RUN-1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "EVENT-WRITE-1,TL" + "01"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySendByte(16) = 0
        MySendByte(17) = 1
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub




    Private Sub TextBox17_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox17.TextChanged
        TextBox17.SelectionStart = Len(TextBox17.Text)
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged
        TextBox5.SelectionStart = Len(TextBox5.Text)
    End Sub


    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "RFSTUY_315M-STUDY-1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()

    End Sub

    Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "RFSTUY_433M-STUDY-1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "RFSTUY_433M-SEND-1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "RFSTUY_433M-SEND-1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            MySendString = "RELAY-SET-1,1,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,1,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub
    Private Sub CheckBox16_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox16.CheckedChanged
        If CheckBox16.Checked = True Then
            MySendString = "RELAY-SET-1,9,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,9,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            MySendString = "RELAY-SET-1,2,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,2,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub
    Private Sub CheckBox14_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox14.CheckedChanged
        If CheckBox14.Checked = True Then
            MySendString = "RELAY-SET-1,10,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,10,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            MySendString = "RELAY-SET-1,3,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,3,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = True Then
            MySendString = "RELAY-SET-1,4,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,4,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked = True Then
            MySendString = "RELAY-SET-1,5,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,5,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked = True Then
            MySendString = "RELAY-SET-1,6,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,6,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox7.CheckedChanged
        If CheckBox7.Checked = True Then
            MySendString = "RELAY-SET-1,7,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,7,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox8.CheckedChanged
        If CheckBox8.Checked = True Then
            MySendString = "RELAY-SET-1,8,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,8,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "TIME-READ-NOW"
        MySendByte = MyInfo.GetBytes(MySendString)
        If (mode = 1) Then
            MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        Else
            RS232.Write(MySendString, 0, MySendString.Length)
        End If
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "TELEPHONE-WRITE_USER-1,+86"
        MySendByte = MyInfo.GetBytes(MySendString)
        If (mode = 1) Then
            MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        Else
            RS232.Write(MySendString, 0, MySendString.Length)
        End If
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)


    End Sub

    Private Sub CheckBox9_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "PT2262_315M-WRITE-5,12,851952"
        MySendByte = MyInfo.GetBytes(MySendString)
        If (mode = 1) Then
            MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        Else
            RS232.Write(MySendString, 0, MySendString.Length)
        End If
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
        Thread.Sleep(1000)
        MySendString = "PT2262_315M-SEND-5"
        MySendByte = MyInfo.GetBytes(MySendString)
        If (mode = 1) Then
            MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        Else
            RS232.Write(MySendString, 0, MySendString.Length)
        End If
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "EVENT-SET_WARNING-ON"
        MySendByte = MyInfo.GetBytes(MySendString)
        If (mode = 1) Then
            MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        Else
            RS232.Write(MySendString, 0, MySendString.Length)
        End If
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()

        Thread.Sleep(1000)
        MySendString = "EVENT-SET_WARNING-ON"
        MySendByte = MyInfo.GetBytes(MySendString)
        If (mode = 1) Then
            MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        Else
            RS232.Write(MySendString, 0, MySendString.Length)
        End If
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
        Thread.Sleep(1000)
        MySendString = "EVENT-SET_WARNING-ON"
        MySendByte = MyInfo.GetBytes(MySendString)
        If (mode = 1) Then
            MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        Else
            RS232.Write(MySendString, 0, MySendString.Length)
        End If
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()

        Thread.Sleep(1000)

        MySendString = "EVENT-SET_WARNING-ON"
        MySendByte = MyInfo.GetBytes(MySendString)
        If (mode = 1) Then
            MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        Else
            RS232.Write(MySendString, 0, MySendString.Length)
        End If
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()

    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "EVENT-SET_WARNING-OFF"
        MySendByte = MyInfo.GetBytes(MySendString)
        If (mode = 1) Then
            MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        Else
            RS232.Write(MySendString, 0, MySendString.Length)
        End If
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button11_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        MySendString = "RELAY-SCAN_DEVICE-NOW"
        MySendByte = MyInfo.GetBytes(MySendString)
        If (mode = 1) Then
            MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        Else
            RS232.Write(MySendString, 0, MySendString.Length)
        End If
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
        g_delay = 0
        Timer1.Enabled = True
        While g_delay <> 1
            System.Windows.Forms.Application.DoEvents()
        End While
        Timer1.Enabled = False
        g_delay = 0
        MySendString = "RELAY-TEST-NOW"
        MySendByte = MyInfo.GetBytes(MySendString)
        If (mode = 1) Then
            MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        Else
            RS232.Write(MySendString, 0, MySendString.Length)
        End If
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        g_delay = 1
    End Sub

    Private Sub GroupBox2_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_CONFIG-SEND-02"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        '  TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        '  TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-WRITE-1,"
        MySendString = MySendString
        MySendString = MySendString + ",1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        '   TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        '   TextBox17.ScrollToCaret()
        Thread.Sleep(1000)
        MySendString = "ZIGBEE_LIGHT-WRITE-2,"
        MySendString = MySendString
        MySendString = MySendString + ",2"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        '  TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        '  TextBox17.ScrollToCaret()

        Thread.Sleep(1000)
        MySendString = "ZIGBEE_LIGHT-WRITE-3,"
        MySendString = MySendString
        MySendString = MySendString + ",3"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        ' TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        ' TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button17_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-SEND-1,1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        'TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        'TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-READ-1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        'TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        'TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button29_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-READ-3"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        'TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        'TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button28_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-SEND-3,0"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        'TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        'TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-SEND-3,1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        'TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        'TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-READ-2"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        'TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        'TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-SEND-2,0"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        'TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        'TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-SEND-2,1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        'TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        'TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-SEND-1,0"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        'TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        'TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button28_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-WRITE-02,37507,1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()

    End Sub

    Private Sub Button29_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-WRITE-01,63119,1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button30_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_INFRARED-WRITE-1," + ","
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button31_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_INFRARED-STUDY-1,1,"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_INFRARED-SEND-1,1,"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub GroupBox13_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button36_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-SEND-3,1"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        'TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        'TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-SEND-3,0"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        'TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        'TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MySendString = "ZIGBEE_LIGHT-READ-3"
        MySendByte = MyInfo.GetBytes(MySendString)
        MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        'TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        'TextBox17.ScrollToCaret()
    End Sub

    Private Sub CheckBox12_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox12.CheckedChanged
        If CheckBox12.Checked = True Then
            MySendString = "RELAY-SET-1,11,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,11,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox10.CheckedChanged
        If CheckBox10.Checked = True Then
            MySendString = "RELAY-SET-1,12,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,12,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox9_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox9.CheckedChanged
        If CheckBox9.Checked = True Then
            MySendString = "RELAY-SET-1,13,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,13,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox11_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox11.CheckedChanged
        If CheckBox11.Checked = True Then
            MySendString = "RELAY-SET-1,14,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,14,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox13_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox13.CheckedChanged
        If CheckBox13.Checked = True Then
            MySendString = "RELAY-SET-1,15,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,15,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox15_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox15.CheckedChanged
        If CheckBox15.Checked = True Then
            MySendString = "RELAY-SET-1,16,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,16,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox24_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox24.CheckedChanged
        If CheckBox24.Checked = True Then
            MySendString = "RELAY-SET-1,17,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,17,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox22_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox22.CheckedChanged
        If CheckBox22.Checked = True Then
            MySendString = "RELAY-SET-1,18,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,18,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox20_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox20.CheckedChanged
        If CheckBox20.Checked = True Then
            MySendString = "RELAY-SET-1,19,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,19,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox18_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox18.CheckedChanged
        If CheckBox18.Checked = True Then
            MySendString = "RELAY-SET-1,20,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,20,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox17_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox17.CheckedChanged
        If CheckBox17.Checked = True Then
            MySendString = "RELAY-SET-1,21,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,21,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox19_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox19.CheckedChanged
        If CheckBox19.Checked = True Then
            MySendString = "RELAY-SET-1,22,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,22,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox21_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox21.CheckedChanged
        If CheckBox21.Checked = True Then
            MySendString = "RELAY-SET-1,23,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,23,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox23_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox23.CheckedChanged
        If CheckBox23.Checked = True Then
            MySendString = "RELAY-SET-1,24,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,24,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox32_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox32.CheckedChanged
        If CheckBox32.Checked = True Then
            MySendString = "RELAY-SET-1,25,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,25,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox30_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox30.CheckedChanged
        If CheckBox30.Checked = True Then
            MySendString = "RELAY-SET-1,26,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,26,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox28_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox28.CheckedChanged
        If CheckBox28.Checked = True Then
            MySendString = "RELAY-SET-1,27,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,27,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox26_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox26.CheckedChanged
        If CheckBox26.Checked = True Then
            MySendString = "RELAY-SET-1,28,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,28,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox25_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox25.CheckedChanged
        If CheckBox25.Checked = True Then
            MySendString = "RELAY-SET-1,29,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,29,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox27_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox27.CheckedChanged
        If CheckBox27.Checked = True Then
            MySendString = "RELAY-SET-1,30,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,30,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox29_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox29.CheckedChanged
        If CheckBox29.Checked = True Then
            MySendString = "RELAY-SET-1,31,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,31,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub CheckBox31_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox31.CheckedChanged
        If CheckBox31.Checked = True Then
            MySendString = "RELAY-SET-1,32,1"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        Else
            MySendString = "RELAY-SET-1,32,0"
            MySendByte = MyInfo.GetBytes(MySendString)
            If (mode = 1) Then
                MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
            Else
                RS232.Write(MySendString, 0, MySendString.Length)
            End If
            TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
            TextBox17.ScrollToCaret()
        End If
    End Sub

    Private Sub GroupBox2_Enter_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        MySendString = "RELAY-GET_INPUT-1"
        MySendByte = MyInfo.GetBytes(MySendString)
        If (mode = 1) Then
            MySocket.Send(MySendByte, MySendByte.Length, SocketFlags.None)
        Else
            RS232.Write(MySendString, 0, MySendString.Length)
        End If
        TextBox17.Text = TextBox17.Text + MySendString + vbCrLf
        TextBox17.ScrollToCaret()
    End Sub

    Private Sub Button3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If Com_Box.Items.Count = 0 Then
            MsgBox("Please select the serial port")
        Else

            If RS232.IsOpen Then
                Button3.Text = "Open"
                Button1.Enabled = True
                TextBox2.Enabled = True
                TextBox1.Enabled = True
                Com_Box.Enabled = True
                Button3.Enabled = True




                Button11.Enabled = False
                GroupBox9.Enabled = False


                RS232.Close()
                MyThreadCom.Abort()
                mode = 0
            Else
                mPortName = Com_Box.SelectedItem.ToString   '欲打开的通信端口   
                mBaudRate = "115200"   '通信速度
                mParity = "0" '同位位检查设定
                mDataBit = "8"    '数据位设定值
                mStopbit = "1"  '停止位设定值
                RS232 = New IO.Ports.SerialPort(mPortName, mBaudRate, mParity, mDataBit, mStopbit)
                Button3.Text = "Close"  '致能传送按钮
                Com_Box.Enabled = False

                RS232.Open()  '打开通信端口
                Button11.Enabled = True
                Button1.Enabled = True
                TextBox2.Enabled = False
                TextBox1.Enabled = False
                'GroupBox7.Enabled = True
                'GroupBox6.Enabled = True
                'GroupBox5.Enabled = True
                'GroupBox4.Enabled = True
                ' GroupBox3.Enabled = True
                ' GroupBox8.Enabled = True
                GroupBox9.Enabled = True
                Button1.Enabled = False
                mode = 2
                Dim MyDelegateCom As New ThreadStart(AddressOf MyChatProcessCom)
                MyThreadCom = New Thread(MyDelegateCom)
                MyChattingStateCom = True
                MyThreadCom.Start()
            End If

        End If
    End Sub
End Class

