
Imports System.IO

Public Class Form1

    Public _p2 As Double        'For intergrator
    Public _yold As Double      'For 1st order
    Public time As Double = 0

    Public Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Integrator
        Dim τ, p1, p2, dt As Double

        dt = 1                          'time step [sec]
        p1 = NumericUpDown1.Value       'Input value
        τ = NumericUpDown3.Value        'Time constant
        p2 = Integrator(p1, τ, dt)

        TextBox1.Text = p2.ToString("0.00")
        NumericUpDown2.Value = p2       'Output
        time += dt
        Chart1.Series(0).Points.AddXY(time, p2)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        '1st order
        Dim τ, p1, p2, dt As Double
        dt = 1                      'time step [sec]
        p1 = NumericUpDown1.Value
        τ = NumericUpDown3.Value
        p2 = First_order(p1, τ, dt)

        TextBox1.Text = p2.ToString("0.000")
        NumericUpDown2.Value = p2
        time += dt
        Chart1.Series(0).Points.AddXY(time, p2)
    End Sub

    Public Function First_order(kx As Double, τ As Double, dt As Double) As Double
        'τ.(dy/dt) + y = Kx 
        'Hieruit volgt dy= (Kx-y) * dt/τ
        'kx = input, tou= time constant, dt= time step
        'y is de output
        Dim dy, y As Double 'output

        dy = (kx - _yold) * dt / τ
        y = _yold + dy
        _yold = y
        Return (y)
    End Function

    Public Function Integrator(x As Double, τ As Double, dt As Double) As Double
        'teruggekoppelde integrator 
        'Hieruit volgt dy=x/τ*dt
        'P1 = input, tou= time constant, dt= time step
        'return value is output value
        Dim dp As Double
        dp = (x / τ) * dt
        _p2 = _p2 + dp
        Return (_p2)
    End Function

    Private Sub Init_Chart1()
        Dim i As Integer
        Try
            Chart1.Series.Clear()
            Chart1.ChartAreas.Clear()
            Chart1.Titles.Clear()
            Chart1.ChartAreas.Add("ChartArea0")

            For i = 0 To 1
                Chart1.Series.Add(i.ToString)
                Chart1.Series(i.ToString).ChartArea = "ChartArea0"
                Chart1.Series(i.ToString).ChartType = DataVisualization.Charting.SeriesChartType.Line
                Chart1.Series(i.ToString).BorderWidth = 3
            Next

            Chart1.Titles.Add("CONTROL ENGINEERING")
            Chart1.Titles(0).Font = New Font("Arial", 12, System.Drawing.FontStyle.Bold)
            Chart1.Series(0).Color = Color.Black
            Chart1.ChartAreas("ChartArea0").AxisX.Title = "[sec]"
            Chart1.ChartAreas("ChartArea0").AxisY.Title = "Value"
        Catch ex As Exception
            MessageBox.Show("Init Chart1 failed")
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Init_Chart1()
    End Sub
End Class


