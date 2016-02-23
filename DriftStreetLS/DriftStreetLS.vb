Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports GTA
Imports GTA.Native
Imports GTA.Math
Imports INMNativeUI
Imports DriftStreetLS.Resources
Imports System.Globalization

Public Class DriftStreetLS
    Inherits Script

    Public Shared PlayerOne As Player
    Public Shared PlayerPed As Ped
    Public Shared PlayerCash As Integer 'Decimal
    Public Shared PlayerHighscore As Integer
    Public Shared PlayerEXP As Integer
    Public Shared playerRank As Integer
    Public Shared Score As Integer = 0
    Public Shared ExtraMoney As Integer 'Double
    Public Shared ExtraEXP As Integer 'Double
    Public Shared Streak As String = ""
    Public Shared Price As Integer = 0 'Decimal
    Public Shared IsDrifting As Boolean = False
    Public Shared ModActivate As Boolean = False
    Public Shared Multiplier As Integer = 1
    Public Shared driftDispTimer As Timer
    Public Shared timerPool As TimerBarPool
    Public Shared DisplayText As String = "DRIFTING"

    Public Shared HighScoreTextBar As TextTimerBar
    Public Shared BankTextBar As TextTimerBar

    Public Sub New()
        Try
            PlayerOne = Game.Player
            PlayerPed = Game.Player.Character

            AddHandler Tick, AddressOf OnTick
            timerPool = New TimerBarPool()
            driftDispTimer = New Timer(3000)

            DrawTimerBars()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub DrawTimerBars()
        Try
            HighScoreTextBar = New TextTimerBar("BEST", PlayerHighscore.ToString("###,###"))
            BankTextBar = New TextTimerBar("BANK", "$" & PlayerCash.ToString("###,###"))
            timerPool.Add(HighScoreTextBar)
            timerPool.Add(BankTextBar)
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub UpdateTimerBars()
        Try
            timerPool.Remove(HighScoreTextBar)
            timerPool.Remove(BankTextBar)
            HighScoreTextBar = New TextTimerBar("BEST", PlayerHighscore.ToString("###,###"))
            BankTextBar = New TextTimerBar("BANK", "$" & PlayerCash.ToString("###,###"))
            timerPool.Add(HighScoreTextBar)
            timerPool.Add(BankTextBar)
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub OnTick(o As Object, e As EventArgs)
        Try
            PlayerOne = Game.Player
            PlayerPed = Game.Player.Character

            If ModActivate = True AndAlso PlayerPed.IsInVehicle AndAlso PlayerPed.CurrentVehicle = DSMenu.PlayerVehicle Then
                timerPool.Draw()
                UpdateTimerBars()
                If ReadCfgValue("Traffic", DSMenu.saveFile) = "False" Then Native.Function.Call(Hash.SET_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, 0.0)
                If ReadCfgValue("Police", DSMenu.saveFile) = "True" Then PlayerOne.WantedLevel = 5 Else PlayerOne.WantedLevel = 0

                If Resources.IsDrifting(DSMenu.PlayerVehicle) = True Then
                    IsDrifting = True
                Else
                    IsDrifting = False
                End If

                If Native.Function.Call(Of Boolean)(Hash.HAS_ENTITY_COLLIDED_WITH_ANYTHING, DSMenu.PlayerVehicle.Handle) Then
                    driftDispTimer.Enabled = False
                    Score = 0
                    Streak = ""
                    Multiplier = 1
                End If

                Select Case Score
                    Case 0 To 1000
                        Streak = ""
                    Case 1001 To 5000
                        Streak = "GOOD DRIFT!"
                        Multiplier = 2
                    Case 5001 To 10000
                        Streak = "GREAT DRIFT!"
                        Multiplier = 3
                    Case 10001 To 50000
                        Streak = "SUPERB DRIFT!"
                        Multiplier = 4
                    Case 50001 To 100000
                        Streak = "COLOSSAL DRIFT!"
                        Multiplier = 5
                    Case 100001 To 500000
                        Streak = "OUTRAGEOUS DRIFT"
                        Multiplier = 6
                    Case 500001 To 1000000
                        Streak = "INSANE DRIFT"
                        Multiplier = 7
                    Case Else
                        Streak = "DRIFT KING!"
                        Multiplier = 8
                End Select
            End If

            If IsDrifting Then
                Score = Score + 1 * DSMenu.PlayerVehicle.Speed * Multiplier
                DisplayText = "DRIFTING"
                driftDispTimer.Start()
            End If

            If Not DSMenu.PlayerVehicle = Nothing AndAlso ModActivate = True Then
                If DSMenu.PlayerVehicle.IsInAir Then
                    Score = Score + 2 * DSMenu.PlayerVehicle.Speed * Multiplier
                    DisplayText = "JUMPING"
                    driftDispTimer.Start()
                End If
            End If

            If driftDispTimer.Enabled Then
                DrawText(DisplayText, New PointF(UI.WIDTH / 2, 100), 1.0, Color.White, Enums.GTAFont.Title, Enums.GTAFontAlign.Center, Enums.GTAFontStyleOptions.Outline)
                DrawText(Score.ToString("###,###") & " Points", New PointF(UI.WIDTH / 2, 140), 1.0, Color.LightSkyBlue, Enums.GTAFont.Title2, Enums.GTAFontAlign.Center, Enums.GTAFontStyleOptions.Outline)
                DrawText(Streak, New PointF(UI.WIDTH / 2, 180), 1.0, Color.Yellow, Enums.GTAFont.Script, Enums.GTAFontAlign.Center, Enums.GTAFontStyleOptions.Outline)

                If Game.GameTime > driftDispTimer.Waiter Then
                    driftDispTimer.Enabled = False
                    If Score > PlayerHighscore Then
                        PlayerHighscore = Score
                        AddScore(PlayerOne.Name, Score)
                    End If
                    Dim NewCash As Integer = (Score / 500) 'Double
                    NewCash *= ExtraMoney
                    Dim NewCash2 As Integer = NewCash 'Double
                    NewCash2 += (Score / 500)
                    PlayerCash = PlayerCash + NewCash2
                    Dim NewEXP As Integer = (Score / 100) 'Double
                    NewEXP *= ExtraEXP
                    Dim NewEXP2 As Integer = NewEXP 'Double
                    NewEXP2 += (Score / 100)
                    PlayerEXP = PlayerEXP + NewEXP2
                    Dim NewRank As Integer = GetRankIndex(PlayerEXP)
                    If NewRank <> playerRank Then BigMessageThread.MessageInstance.ShowRankupMessage("Rank Up!", "Rank", NewRank, 5000)
                    playerRank = NewRank
                    Score = 0
                    Streak = ""
                    Multiplier = 1
                End If
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub
End Class
