
Imports System.Math

Public Class Form1

    Public _p2 As Double        'For intergrator
    Public _yold As Double      'For 1st order
    Public valve_pos = 0         'For speed limit
    Public time As Double = 0

    Public Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Integrator
        Dim τ, p1, p2, dt As Double

        dt = NumericUpDown5.Value       'time step [sec]
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

        dt = NumericUpDown5.Value       'time step [sec]
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

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click, NumericUpDown9.ValueChanged, NumericUpDown8.ValueChanged, NumericUpDown7.ValueChanged, NumericUpDown6.ValueChanged, NumericUpDown4.ValueChanged
        'See page 64-65, Regeltechniek, 7e druk
        'de klep is vervangen door een fan
        'de fan wordt lineair geacht
        'R_fan = dp/Volume_Flow zie formule (4.3)
        'Capaciteit tank-system-duct zie formule (4.5)

        Dim v, mol, R, temp, τ As Double
        Dim cap_duct As Double
        v = NumericUpDown4.Value                '[m3] system volume

        mol = NumericUpDown6.Value / 1000       '[kg/mol]
        R = 8.314 / mol                         'Specific gas constant
        temp = NumericUpDown7.Value + 273       '[K]

        cap_duct = v / (R * temp)               '[..]

        Dim fan_flow, fan_dp, R_fan As Double
        fan_flow = NumericUpDown8.Value / 3600  '[Am3/s]
        fan_dp = NumericUpDown9.Value           '[Pa]
        R_fan = fan_dp / fan_flow               '[..]

        τ = R_fan * cap_duct                    '[s]

        TextBox2.Text = cap_duct.ToString("0.000")
        TextBox3.Text = R_fan.ToString("0")
        TextBox4.Text = τ.ToString("0.0")
        TextBox5.Text = R.ToString("0.0")
    End Sub
End Class


