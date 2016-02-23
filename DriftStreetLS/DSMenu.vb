Imports System.Drawing
Imports System.Windows.Forms
Imports GTA
Imports GTA.Native
Imports GTA.Math
Imports GTA.Game
Imports INMNativeUI
Imports DriftStreetLS.Resources
Imports WMPLib
Imports System.Globalization
Imports System.Net

Public Class DSMenu
    Inherits Script

    Public Shared saveFile As String = Application.StartupPath & "\scripts\DriftStreetLS\save.cfg"
    Public Shared carDir As String = Application.StartupPath & "\scripts\DriftStreetLS\Cars\"
    Public Shared trackDir As String = Application.StartupPath & "\scripts\DriftStreetLS\Tracks\"
    Public Shared mainDir As String = Application.StartupPath & "\scripts\DriftStreetLS\"
    Public Shared colorDir As String = Application.StartupPath & "\scripts\DriftStreetLS\ModShop\Colors\"
    Public Shared kitDir As String = Application.StartupPath & "\scripts\DriftStreetLS\ModShop\Kit Sets\"
    Public Shared wheelDir As String = Application.StartupPath & "\scripts\DriftStreetLS\ModShop\Wheels\"
    Public Shared musicDir As String = Application.StartupPath & "\scripts\DriftStreetLS\Musics\"
    Public Shared PlayerOne As Player
    Public Shared PlayerPed As Ped
    Public Shared PlayerCash As Integer 'Decimal
    Public Shared PlayerHighscore As Integer = 0
    Public Shared Score As Integer = 0
    Public Shared Multiplier As Integer = 1
    Public Shared Streak As String = ""
    Public Shared VehiclePrice As Integer
    Public Shared PlayerVehicle As Vehicle = Nothing
    Public Shared Price As Integer = 0 'Decimal
    Public Shared HideHud As Boolean = False
    Public Shared ShowVehicleName As Boolean = False
    Public Shared VehicleName As String = Nothing
    Public Shared xyzBack As Vector3 = New Vector3(397.4078, -978.534, -99.5269)
    Public Shared xyz As Vector3 = New Vector3(405.3363, -970.8112, -99.52769)
    Public Shared xyzFront As Vector3 = New Vector3(418.0671, -958.4104, -99.5269)
    Public Shared LastXYZ As Vector3
    Public Shared dsMenu, grgMenu, tunMenu, msMenu, trkMenu, setMenu, topMenu, modeMenu As UIMenu
    Public Shared engMenu, brkMenu, traMenu, susMenu, armMenu, trbMenu As UIMenu
    Public Shared colMenu, bodyMenu, tintMenu, priMenu, secMenu, prlMenu, rimCMenu As UIMenu
    Public Shared kitMenu, whlMenu, w_typeMenu, wt_sprtMenu, wt_msclMenu, wt_lwrdMenu, wt_suvMenu, wt_offrMenu, wt_tnerMenu, wt_highMenu, w_smokeMenu, ligMenu As UIMenu
    Public Shared _menuPool As MenuPool
    Public Shared Player As WindowsMediaPlayer = New WindowsMediaPlayer
    Public Shared GarageDict As New Dictionary(Of String, String)
    Public Shared ParaColors As String() = {"[code]", "[price]", "[name]"}
    Public Shared ParaCColors As String() = {"[red]", "[green]", "[blue]", "[name]"}
    Public Shared ParaKits As String() = {"[name]", "[spoiler]", "[frontbumper]", "[rearbumper]", "[sideskirt]", "[frame]", "[grille]", "[hood]", "[fender]", "[rightfender]", "[roof]", "[exhaust]", "[rank]", "[price]", "[exp]", "[money]", "[desc]"}
    Public Shared itemPlay As New UIMenuItem("Play")
    Public Shared itemGarage As New UIMenuItem("Garage")
    Public Shared itemTuning As New UIMenuItem("Tuning")
    Public Shared itemModshop As New UIMenuItem("Modshop")
    Public Shared itemTrack As New UIMenuItem("Track")
    Public Shared itemSetting As New UIMenuItem("Settings")
    Public Shared itemCredits As New UIMenuItem("Credits")
    Public Shared itemExit As New UIMenuItem("Exit")
    Public Shared itemCar As UIMenuItem
    Public Shared itemEngine As New UIMenuItem("Engine")
    Public Shared itemEngine2(5) As UIMenuItem
    Public Shared itemBrakes As New UIMenuItem("Brakes")
    Public Shared itemBrakes2(4) As UIMenuItem
    Public Shared itemTrans As New UIMenuItem("Transmission")
    Public Shared itemTrans2(4) As UIMenuItem
    Public Shared itemSusp As New UIMenuItem("Suspension")
    Public Shared itemSusp2(5) As UIMenuItem
    Public Shared itemArmor As New UIMenuItem("Armor")
    Public Shared itemArmor2(6) As UIMenuItem
    Public Shared itemTurbo As New UIMenuItem("Turbo")
    Public Shared itemTurbo2(2) As UIMenuItem
    Public Shared itemWeather As New UIMenuListItem("Weather", New List(Of Object)() From {"Extra Sunny", "Clear", "Neutral", "Smog", "Foggy", "Overcast", "Clouds", "Clearing", "Rain", "Thunder", "Snow", "Blizzard", "Light Snow", "X-mas"}, 0)
    Public Shared itemTime As New UIMenuListItem("Time", New List(Of Object)() From {"Morning", "Noon", "Afternoon", "Evening", "Midnight", "Night"}, 0)
    Public Shared itemTraffic As New UIMenuListItem("Traffic", New List(Of Object)() From {"On", "Off"}, 0)
    Public Shared itemPolice As New UIMenuListItem("Police", New List(Of Object)() From {"On", "Off"}, 0)
    Public Shared itemVolume As New UIMenuListItem("Volume", New List(Of Object)() From {0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100}, 0)
    Public Shared itemMusic As New UIMenuListItem("Music", New List(Of Object)() From {"On", "Off"}, 0)
    Public Shared itemColor As New UIMenuItem("Color")
    Public Shared itemTint As New UIMenuItem("Tint Color")
    Public Shared itemPrimary As New UIMenuItem("Primary Color")
    Public Shared itemSecondary As New UIMenuItem("Secondary Color")
    Public Shared itemPearl As New UIMenuItem("Pearlescent Color")
    Public Shared itemRimCol As New UIMenuItem("Rim Color")
    Public Shared itemBody As New UIMenuItem("Bodyshop")
    Public Shared itemKit As New UIMenuItem("Kit Set")
    Public Shared itemWheel As New UIMenuItem("Wheels")
    Public Shared itemLights As New UIMenuItem("Lights")
    Public Shared itemLights2(2) As UIMenuItem
    Public Shared itemWheelType As New UIMenuItem("Wheel Type")
    Public Shared itemSmoke As New UIMenuItem("Tyre Smoke")
    Public Shared itemSportWhl As New UIMenuItem("Sport")
    Public Shared itemMuscleWhl As New UIMenuItem("Muscle")
    Public Shared itemLowridWhl As New UIMenuItem("Lowrider")
    Public Shared itemSUVWhl As New UIMenuItem("SUV")
    Public Shared itemOffrdWhl As New UIMenuItem("Offroad")
    Public Shared itemTunerWhl As New UIMenuItem("Tuner")
    Public Shared itemHighWhl As New UIMenuItem("High End")
    Public Shared itemTop10 As New UIMenuItem("Leaderboard")
    Public Shared itemLeaderboard As UIMenuItem
    Public Shared itemGameMode As New UIMenuItem("Game Modes")
    Public Shared itemFreeRoamMode As New UIMenuItem("Free Roam")
    Public Shared itemTimeAttackMode As New UIMenuItem("Time Attack")
    Public Shared itemSurvivalMode As New UIMenuItem("Survival")
    Public Shared itemCheckpointMode As New UIMenuItem("Checkpoint")
    Public Shared TaskDriveTimer As Timer
    Public Shared selectedModel As Integer = 0

    Public Shared TopScoresURL As String = "http://notmental.ml/dsls/TopScores.php"

    Public Sub New()
        Try
            PlayerOne = Game.Player
            PlayerPed = Game.Player.Character
            TaskDriveTimer = New Timer(3000)

            My.Settings.PlayerCash = ReadCfgValue("Bank", saveFile)
            My.Settings.PlayerHighscore = ReadCfgValue("Highscore", saveFile)
            My.Settings.PlayerLastVeh = ReadCfgValue("LastVeh", saveFile)
            My.Settings.Key = [Enum].Parse(GetType(Keys), ReadCfgValue("EnableKey", saveFile), False)
            My.Settings.PlayerLastTrack = ReadCfgValue("LastTrack", saveFile)
            My.Settings.EXP = ReadCfgValue("EXP", saveFile)
            My.Settings.PlayerLastMode = ReadCfgValue("LastMode", saveFile)
            My.Settings.Save()

            AddHandler Tick, AddressOf OnTick
            AddHandler KeyDown, AddressOf OnKeyDown
            _menuPool = New MenuPool()

            CreateMainMenu()
            CreateGarageMenu()
            CreateTuningMenu()
            CreateEngineMenu()
            CreateBrakesMenu()
            CreateTransmissionMenu()
            CreateSuspensionMenu()
            CreateArmorMenu()
            CreateTurboMenu()
            CreateModshopMenu()
            CreateColorMenu()
            CreatePrimaryColorMenu()
            CreateSecondaryColorMenu()
            CreatePearlescentColorMenu()
            CreateWheelColorMenu()
            CreateTintColorMenu()
            CreateBodyshopMenu()
            CreateKitSetMenu()
            CreateWheelMenu()
            CreateWheelTypeMenu()
            CreateHighEndWheelMenu()
            CreateLowriderWheelMenu()
            CreateMuscleWheelMenu()
            CreateOffroadWheelMenu()
            CreateSportWheelMenu()
            CreateSUVWheelMenu()
            CreateTunerWheelMenu()
            CreateTyreSmokeMenu()
            CreateLightsMenu()
            CreateTrackMenu()
            CreateSettingMenu()
            CreateLeaderboardMenu()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateMainMenu()
        Try
            dsMenu = New UIMenu("", "VERSION " & My.Application.Info.Version.ToString, New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            dsMenu.SetBannerType(Rectangle)
            dsMenu.MouseEdgeEnabled = False
            _menuPool.Add(dsMenu)
            dsMenu.AddItem(itemGarage)
            dsMenu.AddItem(itemTuning)
            dsMenu.AddItem(itemModshop)
            dsMenu.AddItem(itemTrack)
            dsMenu.AddItem(itemGameMode)
            dsMenu.AddItem(itemCredits)
            dsMenu.AddItem(itemSetting)
            dsMenu.AddItem(itemTop10)
            dsMenu.AddItem(itemPlay)
            dsMenu.AddItem(itemExit)
            dsMenu.RefreshIndex()
            AddHandler dsMenu.OnItemSelect, AddressOf MainMenuItemSelectHandler
            AddHandler dsMenu.OnMenuClose, AddressOf MainMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateGameModeMenu()
        Try
            modeMenu = New UIMenu("", "GAME MODE", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            modeMenu.SetBannerType(Rectangle)
            modeMenu.MouseEdgeEnabled = False
            _menuPool.Add(modeMenu)
            modeMenu.MenuItems.Clear()
            modeMenu.AddItem(itemCheckpointMode)
            modeMenu.AddItem(itemFreeRoamMode)
            modeMenu.AddItem(itemSurvivalMode)
            modeMenu.AddItem(itemTimeAttackMode)
            modeMenu.RefreshIndex()
            dsMenu.BindMenuToItem(modeMenu, itemGameMode)
            AddHandler modeMenu.OnItemSelect, AddressOf MainMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateLeaderboardMenu()
        Try
            topMenu = New UIMenu("", "LEADERBOARD", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            topMenu.SetBannerType(Rectangle)
            topMenu.MouseEdgeEnabled = False
            _menuPool.Add(topMenu)
            topMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            topMenu.RefreshIndex()
            dsMenu.BindMenuToItem(topMenu, itemTop10)
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshLeaderboardMenu()
        topMenu.MenuItems.Clear()
        Try
            Dim MyNameIsInLeaderboard As Boolean = False
            Dim Client As WebClient = New WebClient
            Dim Source As String = Client.DownloadString(TopScoresURL)
            Dim Source2 As String = Source.Remove(Source.Length - 1)
            Dim Lines() As String = Source2.Split("#"c)
            For Each s As String In Lines
                Dim result() As String = s.Split(","c)
                Dim name As String = result(0)
                Dim score As String = result(1)
                itemLeaderboard = New UIMenuItem(name)
                topMenu.AddItem(itemLeaderboard)
                With itemLeaderboard
                    If .Text = PlayerOne.Name Then
                        .Text = "~r~" & PlayerOne.Name
                        MyNameIsInLeaderboard = True
                    End If
                    .SetRightLabel(CInt(score).ToString("###,###"))
                End With
            Next
            If MyNameIsInLeaderboard = False Then
                itemLeaderboard = New UIMenuItem("~r~" & PlayerOne.Name)
                With itemLeaderboard
                    .SetRightLabel(My.Settings.PlayerHighscore.ToString("###,###"))
                End With
            End If
        Catch ex As Exception
            UI.Notify(ex.Message)
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
        topMenu.RefreshIndex()
    End Sub

    Public Shared Sub CreateGarageMenu()
        Try
            grgMenu = New UIMenu("", "GARAGE", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            grgMenu.SetBannerType(Rectangle)
            grgMenu.MouseEdgeEnabled = False
            _menuPool.Add(grgMenu)
            grgMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            grgMenu.RefreshIndex()
            dsMenu.BindMenuToItem(grgMenu, itemGarage)
            AddHandler grgMenu.OnItemSelect, AddressOf GarageMenuItemSelectedHandler
            AddHandler grgMenu.OnIndexChange, AddressOf GarageMenuIndexChangeHandler
            AddHandler grgMenu.OnMenuClose, AddressOf GarageMenuMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshGarageMenu()
        Try
            Dim currentCar As String = ReadCfgValue("LastVeh", saveFile)
            grgMenu.MenuItems.Clear()
            Dim DSLSVehicles() As String = IO.Directory.GetFiles(carDir)
            For Each File As String In IO.Directory.GetFiles(carDir)
                If IO.File.Exists(File) Then
                    Dim Own As String = ReadCfgValue("VehicleOwn", File)
                    Dim Price As Integer = CInt(ReadCfgValue("VehiclePrice", File))
                    Dim Name As String = ReadCfgValue("VehicleName", File)
                    Dim Hash As String = ReadCfgValue("VehicleHash", File)
                    itemCar = New UIMenuItem(Name)
                    grgMenu.AddItem(itemCar)
                    With itemCar
                        If Own = "True" Then
                            If currentCar = Hash Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightBadge(UIMenuItem.BadgeStyle.Tick)
                        Else
                            .SetRightLabel("$" & Price.ToString("###,###"))
                        End If
                        .SubString1 = Hash
                        .SubInteger1 = Price
                    End With
                End If
            Next
            grgMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateTrackMenu()
        Try
            trkMenu = New UIMenu("", "TRACKS", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            trkMenu.SetBannerType(Rectangle)
            trkMenu.MouseEdgeEnabled = False
            _menuPool.Add(trkMenu)
            trkMenu.MenuItems.Clear()
            For Each File As String In IO.Directory.GetFiles(trackDir, "*.trk")
                Dim item As New UIMenuItem(IO.Path.GetFileNameWithoutExtension(File), ReadCfgValue("Desc", File))
                trkMenu.AddItem(item)
                With item
                    .SetRightLabel(ReadCfgValue("Author", File))
                End With
            Next
            trkMenu.RefreshIndex()
            dsMenu.BindMenuToItem(trkMenu, itemTrack)
            AddHandler trkMenu.OnItemSelect, AddressOf TrackMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateSettingMenu()
        Try
            setMenu = New UIMenu("", "SETTINGS", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            setMenu.SetBannerType(Rectangle)
            setMenu.MouseEdgeEnabled = False
            _menuPool.Add(setMenu)
            setMenu.AddItem(itemWeather)
            setMenu.AddItem(itemTime)
            setMenu.AddItem(itemTraffic)
            setMenu.AddItem(itemPolice)
            setMenu.AddItem(itemVolume)
            setMenu.AddItem(itemMusic)
            setMenu.RefreshIndex()
            dsMenu.BindMenuToItem(setMenu, itemSetting)
            AddHandler setMenu.OnListChange, AddressOf SettingMenuListChangeHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateModshopMenu()
        Try
            msMenu = New UIMenu("", "MODSHOP", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            msMenu.SetBannerType(Rectangle)
            msMenu.MouseEdgeEnabled = False
            _menuPool.Add(msMenu)
            msMenu.AddItem(itemColor)
            msMenu.AddItem(itemBody)
            msMenu.RefreshIndex()
            dsMenu.BindMenuToItem(msMenu, itemModshop)
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateColorMenu()
        Try
            colMenu = New UIMenu("", "COLOR", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            colMenu.SetBannerType(Rectangle)
            colMenu.MouseEdgeEnabled = False
            _menuPool.Add(colMenu)
            colMenu.AddItem(itemPrimary)
            colMenu.AddItem(itemSecondary)
            colMenu.AddItem(itemPearl)
            colMenu.AddItem(itemRimCol)
            colMenu.AddItem(itemTint)
            colMenu.RefreshIndex()
            msMenu.BindMenuToItem(colMenu, itemColor)
            AddHandler colMenu.OnItemSelect, AddressOf ModshopMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

#Region "Color Menus"
    Public Shared Sub CreatePrimaryColorMenu()
        Try
            priMenu = New UIMenu("", "PRIMARY COLOR", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            priMenu.SetBannerType(Rectangle)
            priMenu.MouseEdgeEnabled = False
            _menuPool.Add(priMenu)
            priMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            priMenu.RefreshIndex()
            colMenu.BindMenuToItem(priMenu, itemPrimary)
            AddHandler priMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler priMenu.OnIndexChange, AddressOf MSColorIndexChangeHandler
            AddHandler priMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshPrimaryColorMenu()
        Try
            priMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("PrimaryColor", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(colorDir & "color.ini", ParaColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                priMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("code")
                    .SubInteger2 = Format(i)("price")
                    .SubString1 = "PrimaryColor"
                    If currentMod = Format(i)("code") Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger2.ToString("###,###"))
                End With
            Next
            priMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateSecondaryColorMenu()
        Try
            secMenu = New UIMenu("", "SECONDARY COLOR", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            secMenu.SetBannerType(Rectangle)
            secMenu.MouseEdgeEnabled = False
            _menuPool.Add(secMenu)
            secMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            secMenu.RefreshIndex()
            colMenu.BindMenuToItem(secMenu, itemSecondary)
            AddHandler secMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler secMenu.OnIndexChange, AddressOf MSColorIndexChangeHandler
            AddHandler secMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshSecondaryColorMenu()
        Try
            secMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("SecondaryColor", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(colorDir & "color.ini", ParaColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                secMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("code")
                    .SubInteger2 = Format(i)("price")
                    .SubString1 = "SecondaryColor"
                    If currentMod = Format(i)("code") Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger2.ToString("###,###"))
                End With
            Next
            secMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreatePearlescentColorMenu()
        Try
            prlMenu = New UIMenu("", "PEARLESCENT COLOR", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            prlMenu.SetBannerType(Rectangle)
            prlMenu.MouseEdgeEnabled = False
            _menuPool.Add(prlMenu)
            prlMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            prlMenu.RefreshIndex()
            colMenu.BindMenuToItem(prlMenu, itemPearl)
            AddHandler prlMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler prlMenu.OnIndexChange, AddressOf MSColorIndexChangeHandler
            AddHandler prlMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshPearlescentColorMenu()
        Try
            prlMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("PearlescentColor", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(colorDir & "pearl_color.ini", ParaColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                prlMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("code")
                    .SubInteger2 = Format(i)("price")
                    .SubString1 = "PearlescentColor"
                    If currentMod = Format(i)("code") Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger2.ToString("###,###"))
                End With
            Next
            prlMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateWheelColorMenu()
        Try
            rimCMenu = New UIMenu("", "RIM COLOR", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            rimCMenu.SetBannerType(Rectangle)
            rimCMenu.MouseEdgeEnabled = False
            _menuPool.Add(rimCMenu)
            rimCMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            rimCMenu.RefreshIndex()
            colMenu.BindMenuToItem(rimCMenu, itemRimCol)
            AddHandler rimCMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler rimCMenu.OnIndexChange, AddressOf MSColorIndexChangeHandler
            AddHandler rimCMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshWheelColorMenu()
        Try
            rimCMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("RimColor", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(colorDir & "color.ini", ParaColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                rimCMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("code")
                    .SubInteger2 = Format(i)("price")
                    .SubString1 = "RimColor"
                    If currentMod = Format(i)("code") Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger2.ToString("###,###"))
                End With
            Next
            rimCMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateTintColorMenu()
        Try
            tintMenu = New UIMenu("", "TINT COLOR", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            tintMenu.SetBannerType(Rectangle)
            tintMenu.MouseEdgeEnabled = False
            _menuPool.Add(tintMenu)
            tintMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            tintMenu.RefreshIndex()
            colMenu.BindMenuToItem(tintMenu, itemTint)
            AddHandler tintMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler tintMenu.OnIndexChange, AddressOf MSColorIndexChangeHandler
            AddHandler tintMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshTintColorMenu()
        Try
            tintMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("WindowTint", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(colorDir & "tint_color.ini", ParaColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                tintMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("code")
                    .SubInteger2 = Format(i)("price")
                    .SubString1 = "WindowTint"
                    If currentMod = Format(i)("code") Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger2.ToString("###,###"))
                End With
            Next
            tintMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub
#End Region

    Public Shared Sub CreateBodyshopMenu()
        Try
            bodyMenu = New UIMenu("", "BODYSHOP", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            bodyMenu.SetBannerType(Rectangle)
            bodyMenu.MouseEdgeEnabled = False
            _menuPool.Add(bodyMenu)
            bodyMenu.AddItem(itemKit)
            bodyMenu.AddItem(itemWheel)
            bodyMenu.AddItem(itemLights)
            bodyMenu.RefreshIndex()
            msMenu.BindMenuToItem(bodyMenu, itemBody)
            AddHandler bodyMenu.OnItemSelect, AddressOf ModshopMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateKitSetMenu()
        Try
            kitMenu = New UIMenu("", "KIT SET", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            kitMenu.SetBannerType(Rectangle)
            kitMenu.MouseEdgeEnabled = False
            _menuPool.Add(kitMenu)
            kitMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            kitMenu.RefreshIndex()
            bodyMenu.BindMenuToItem(kitMenu, itemKit)
            AddHandler kitMenu.OnItemSelect, AddressOf KitSetMenuItemSelectHandler
            AddHandler kitMenu.OnIndexChange, AddressOf KitSetIndexChangeHandler
            AddHandler kitMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshKitSetMenu()
        Try
            kitMenu.MenuItems.Clear()
            Dim currentMod As String = ReadCfgValue("KitSet", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(kitDir & PlayerVehicle.Model.Hash.ToString() & ".ini", ParaKits)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"), Format(i)("desc"))
                kitMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("spoiler")
                    .SubInteger2 = Format(i)("frontbumper")
                    .SubInteger3 = Format(i)("rearbumper")
                    .SubInteger4 = Format(i)("sideskirt")
                    .SubInteger5 = Format(i)("frame")
                    .SubInteger6 = Format(i)("grille")
                    .SubInteger7 = Format(i)("hood")
                    .SubInteger8 = Format(i)("fender")
                    .SubInteger9 = Format(i)("rightfender")
                    .SubString1 = Format(i)("roof")
                    .SubString2 = Format(i)("exhaust")
                    .SubString3 = Format(i)("rank")
                    .SubString4 = Format(i)("price")
                    .SubString5 = Format(i)("exp")
                    .SubString6 = Format(i)("money")
                    If currentMod = Format(i)("name") Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & CInt(.SubString4).ToString("###,###"))
                    If GetRankIndex(My.Settings.EXP) < CInt(.SubString3) Then .Enabled = False
                End With
            Next
            kitMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateWheelMenu()
        Try
            whlMenu = New UIMenu("", "WHEELS", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            whlMenu.SetBannerType(Rectangle)
            whlMenu.MouseEdgeEnabled = False
            _menuPool.Add(whlMenu)
            whlMenu.AddItem(itemWheelType)
            whlMenu.AddItem(itemSmoke)
            whlMenu.RefreshIndex()
            bodyMenu.BindMenuToItem(whlMenu, itemWheel)
            AddHandler whlMenu.OnItemSelect, AddressOf WheelTypeMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

#Region "Wheels"
    Public Shared Sub CreateWheelTypeMenu()
        Try
            w_typeMenu = New UIMenu("", "WHEEL TYPE", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            w_typeMenu.SetBannerType(Rectangle)
            w_typeMenu.MouseEdgeEnabled = False
            _menuPool.Add(w_typeMenu)
            w_typeMenu.AddItem(itemHighWhl)
            w_typeMenu.AddItem(itemSportWhl)
            w_typeMenu.AddItem(itemLowridWhl)
            w_typeMenu.AddItem(itemMuscleWhl)
            w_typeMenu.AddItem(itemOffrdWhl)
            w_typeMenu.AddItem(itemSUVWhl)
            w_typeMenu.AddItem(itemTunerWhl)
            w_typeMenu.RefreshIndex()
            whlMenu.BindMenuToItem(w_typeMenu, itemWheelType)
            AddHandler w_typeMenu.OnItemSelect, AddressOf WheelTypeMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateHighEndWheelMenu()
        Try
            wt_highMenu = New UIMenu("", "HIGH END", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            wt_highMenu.SetBannerType(Rectangle)
            wt_highMenu.MouseEdgeEnabled = False
            _menuPool.Add(wt_highMenu)
            wt_highMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            wt_highMenu.RefreshIndex()
            w_typeMenu.BindMenuToItem(wt_highMenu, itemHighWhl)
            AddHandler wt_highMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler wt_highMenu.OnIndexChange, AddressOf MSColorIndexChangeHandler
            AddHandler wt_highMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshHighEndWheelMenu()
        Try
            wt_highMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("FrontWheels", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim currentType As Integer = ReadCfgValue("WheelType", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(wheelDir & "highend.ini", ParaColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                wt_highMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("code")
                    .SubInteger2 = Format(i)("price")
                    .SubInteger3 = 7
                    .SubString1 = "Wheels"
                    If currentMod = Format(i)("code") AndAlso currentType = 7 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger2.ToString("###,###"))
                End With
            Next
            wt_highMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateLowriderWheelMenu()
        Try
            wt_lwrdMenu = New UIMenu("", "LOWRIDER", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            wt_lwrdMenu.SetBannerType(Rectangle)
            wt_lwrdMenu.MouseEdgeEnabled = False
            _menuPool.Add(wt_lwrdMenu)
            wt_lwrdMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            wt_lwrdMenu.RefreshIndex()
            w_typeMenu.BindMenuToItem(wt_lwrdMenu, itemLowridWhl)
            AddHandler wt_lwrdMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler wt_lwrdMenu.OnIndexChange, AddressOf MSColorIndexChangeHandler
            AddHandler wt_lwrdMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshLowriderWheelMenu()
        Try
            wt_lwrdMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("FrontWheels", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim currentType As Integer = ReadCfgValue("WheelType", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(wheelDir & "lowrider.ini", ParaColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                wt_lwrdMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("code")
                    .SubInteger2 = Format(i)("price")
                    .SubInteger3 = 2
                    .SubString1 = "Wheels"
                    If currentMod = Format(i)("code") AndAlso currentType = 2 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger2.ToString("###,###"))
                End With
            Next
            wt_lwrdMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateMuscleWheelMenu()
        Try
            wt_msclMenu = New UIMenu("", "MUSCLE", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            wt_msclMenu.SetBannerType(Rectangle)
            wt_msclMenu.MouseEdgeEnabled = False
            _menuPool.Add(wt_msclMenu)
            wt_msclMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            wt_msclMenu.RefreshIndex()
            w_typeMenu.BindMenuToItem(wt_msclMenu, itemMuscleWhl)
            AddHandler wt_msclMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler wt_msclMenu.OnIndexChange, AddressOf MSColorIndexChangeHandler
            AddHandler wt_msclMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshMuscleWheelMenu()
        Try
            wt_msclMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("FrontWheels", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim currentType As Integer = ReadCfgValue("WheelType", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(wheelDir & "muscle.ini", ParaColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                wt_msclMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("code")
                    .SubInteger2 = Format(i)("price")
                    .SubInteger3 = 1
                    .SubString1 = "Wheels"
                    If currentMod = Format(i)("code") AndAlso currentType = 1 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger2.ToString("###,###"))
                End With
            Next
            wt_msclMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateOffroadWheelMenu()
        Try
            wt_offrMenu = New UIMenu("", "OFFROAD", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            wt_offrMenu.SetBannerType(Rectangle)
            wt_offrMenu.MouseEdgeEnabled = False
            _menuPool.Add(wt_offrMenu)
            wt_offrMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            wt_offrMenu.RefreshIndex()
            w_typeMenu.BindMenuToItem(wt_offrMenu, itemOffrdWhl)
            AddHandler wt_offrMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler wt_offrMenu.OnIndexChange, AddressOf MSColorIndexChangeHandler
            AddHandler wt_offrMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshOffroadWheelMenu()
        Try
            wt_offrMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("FrontWheels", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim currentType As Integer = ReadCfgValue("WheelType", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(wheelDir & "offroad.ini", ParaColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                wt_offrMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("code")
                    .SubInteger2 = Format(i)("price")
                    .SubInteger3 = 4
                    .SubString1 = "Wheels"
                    If currentMod = Format(i)("code") AndAlso currentType = 4 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger2.ToString("###,###"))
                End With
            Next
            wt_offrMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateSportWheelMenu()
        Try
            wt_sprtMenu = New UIMenu("", "SPORT", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            wt_sprtMenu.SetBannerType(Rectangle)
            wt_sprtMenu.MouseEdgeEnabled = False
            _menuPool.Add(wt_sprtMenu)
            wt_sprtMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            wt_sprtMenu.RefreshIndex()
            w_typeMenu.BindMenuToItem(wt_sprtMenu, itemSportWhl)
            AddHandler wt_sprtMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler wt_sprtMenu.OnIndexChange, AddressOf MSColorIndexChangeHandler
            AddHandler wt_sprtMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshSportWheelMenu()
        Try
            wt_sprtMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("FrontWheels", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim currentType As Integer = ReadCfgValue("WheelType", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(wheelDir & "sport.ini", ParaColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                wt_sprtMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("code")
                    .SubInteger2 = Format(i)("price")
                    .SubInteger3 = 0
                    .SubString1 = "Wheels"
                    If currentMod = Format(i)("code") AndAlso currentType = 0 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger2.ToString("###,###"))
                End With
            Next
            wt_sprtMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateSUVWheelMenu()
        Try
            wt_suvMenu = New UIMenu("", "SUV", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            wt_suvMenu.SetBannerType(Rectangle)
            wt_suvMenu.MouseEdgeEnabled = False
            _menuPool.Add(wt_suvMenu)
            wt_suvMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            wt_suvMenu.RefreshIndex()
            w_typeMenu.BindMenuToItem(wt_suvMenu, itemSUVWhl)
            AddHandler wt_suvMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler wt_suvMenu.OnIndexChange, AddressOf MSColorIndexChangeHandler
            AddHandler wt_suvMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshSUVWheelMenu()
        Try
            wt_suvMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("FrontWheels", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim currentType As Integer = ReadCfgValue("WheelType", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(wheelDir & "suv.ini", ParaColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                wt_suvMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("code")
                    .SubInteger2 = Format(i)("price")
                    .SubInteger3 = 3
                    .SubString1 = "Wheels"
                    If currentMod = Format(i)("code") AndAlso currentType = 3 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger2.ToString("###,###"))
                End With
            Next
            wt_suvMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateTunerWheelMenu()
        Try
            wt_tnerMenu = New UIMenu("", "TUNER", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            wt_tnerMenu.SetBannerType(Rectangle)
            wt_tnerMenu.MouseEdgeEnabled = False
            _menuPool.Add(wt_tnerMenu)
            wt_tnerMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            wt_tnerMenu.RefreshIndex()
            w_typeMenu.BindMenuToItem(wt_tnerMenu, itemTunerWhl)
            AddHandler wt_tnerMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler wt_tnerMenu.OnIndexChange, AddressOf MSColorIndexChangeHandler
            AddHandler wt_tnerMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshTunerWheelMenu()
        Try
            wt_tnerMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("FrontWheels", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim currentType As Integer = ReadCfgValue("WheelType", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(wheelDir & "tuner.ini", ParaColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                wt_tnerMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("code")
                    .SubInteger2 = Format(i)("price")
                    .SubInteger3 = 5
                    .SubString1 = "Wheels"
                    If currentMod = Format(i)("code") AndAlso currentType = 5 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger2.ToString("###,###"))
                End With
            Next
            wt_tnerMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub
#End Region

    Public Shared Sub CreateTyreSmokeMenu()
        Try
            w_smokeMenu = New UIMenu("", "TYRE SMOKE", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            w_smokeMenu.SetBannerType(Rectangle)
            w_smokeMenu.MouseEdgeEnabled = False
            _menuPool.Add(w_smokeMenu)
            w_smokeMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            w_smokeMenu.RefreshIndex()
            whlMenu.BindMenuToItem(w_smokeMenu, itemSmoke)
            AddHandler w_smokeMenu.OnItemSelect, AddressOf MSColorMenuItemSelectHandler
            AddHandler w_smokeMenu.OnMenuClose, AddressOf MSColorMenuCloseHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshTyreSmokeMenu()
        Try
            w_smokeMenu.MenuItems.Clear()
            Dim currentModRed As Integer = ReadCfgValue("TyreSmokeColorRed", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim currentModGreen As Integer = ReadCfgValue("TyreSmokeColorGreen", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim currentModBlue As Integer = ReadCfgValue("TyreSmokeColorBlue", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            Dim Format As New Reader(colorDir & "custom_color.ini", ParaCColors)
            For i As Integer = 0 To Format.Count - 1
                Dim item As New UIMenuItem(Format(i)("name"))
                w_smokeMenu.AddItem(item)
                With item
                    .SubInteger1 = Format(i)("red")
                    .SubInteger2 = Format(i)("green")
                    .SubInteger3 = Format(i)("blue")
                    .SubInteger4 = 1000
                    .SubString1 = "Smoke"
                    If currentModRed = Format(i)("red") AndAlso currentModGreen = Format(i)("green") AndAlso currentModBlue = Format(i)("blue") Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$" & .SubInteger4.ToString("###,###"))
                End With
            Next
            w_smokeMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateLightsMenu()
        Try
            ligMenu = New UIMenu("", "LIGHTS", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            ligMenu.SetBannerType(Rectangle)
            ligMenu.MouseEdgeEnabled = False
            _menuPool.Add(ligMenu)
            ligMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            ligMenu.RefreshIndex()
            bodyMenu.BindMenuToItem(ligMenu, itemLights)
            AddHandler ligMenu.OnItemSelect, AddressOf UpgradeMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshLightsMenu()
        Try
            ligMenu.MenuItems.Clear()
            Dim currentMod As String = ReadCfgValue("XenonHeadlights", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            itemLights2(0) = New UIMenuItem("None")
            With itemLights2(0)
                If currentMod = "False" Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$1,000")
                .SubString2 = "False"
                .SubInteger2 = 1000
                .SubString1 = "XenonHeadlights"
            End With
            ligMenu.AddItem(itemLights2(0))
            itemLights2(1) = New UIMenuItem("Xenon Headlights")
            With itemLights2(1)
                If currentMod = "True" Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$2,000")
                .SubString2 = "True"
                .SubInteger2 = 2000
                .SubString1 = "XenonHeadlights"
            End With
            ligMenu.AddItem(itemLights2(1))
            ligMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateTuningMenu()
        Try
            tunMenu = New UIMenu("", "TUNING", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            tunMenu.SetBannerType(Rectangle)
            tunMenu.MouseEdgeEnabled = False
            _menuPool.Add(tunMenu)
            tunMenu.AddItem(itemEngine)
            tunMenu.AddItem(itemBrakes)
            tunMenu.AddItem(itemTrans)
            tunMenu.AddItem(itemSusp)
            tunMenu.AddItem(itemArmor)
            tunMenu.AddItem(itemTurbo)
            tunMenu.RefreshIndex()
            dsMenu.BindMenuToItem(tunMenu, itemTuning)
            AddHandler tunMenu.OnItemSelect, AddressOf TunningMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

#Region "Tuning Menus"
    Public Shared Sub CreateEngineMenu()
        Try
            engMenu = New UIMenu("", "ENGINE", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            engMenu.SetBannerType(Rectangle)
            engMenu.MouseEdgeEnabled = False
            _menuPool.Add(engMenu)
            engMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            engMenu.RefreshIndex()
            tunMenu.BindMenuToItem(engMenu, itemEngine)
            AddHandler engMenu.OnItemSelect, AddressOf UpgradeMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshEngineMenu()
        Try
            engMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("Engine", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            itemEngine2(0) = New UIMenuItem("Stock Engine")
            With itemEngine2(0)
                If currentMod = -1 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$100")
                .SubInteger1 = -1
                .SubInteger2 = 100
                .SubString1 = "Engine"
            End With
            engMenu.AddItem(itemEngine2(0))
            itemEngine2(1) = New UIMenuItem("EMS Upgrade Level 1")
            With itemEngine2(1)
                If currentMod = 0 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$200")
                .SubInteger1 = 0
                .SubInteger2 = 200
                .SubString1 = "Engine"
            End With
            engMenu.AddItem(itemEngine2(1))
            itemEngine2(2) = New UIMenuItem("EMS Upgrade Level 2")
            With itemEngine2(2)
                If currentMod = 1 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$400")
                .SubInteger1 = 1
                .SubInteger2 = 400
                .SubString1 = "Engine"
            End With
            engMenu.AddItem(itemEngine2(2))
            itemEngine2(3) = New UIMenuItem("EMS Upgrade Level 3")
            With itemEngine2(3)
                If currentMod = 2 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$800")
                .SubInteger1 = 2
                .SubInteger2 = 800
                .SubString1 = "Engine"
            End With
            engMenu.AddItem(itemEngine2(3))
            itemEngine2(4) = New UIMenuItem("EMS Upgrade Level 4")
            With itemEngine2(4)
                If currentMod = 3 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$1,600")
                .SubInteger1 = 3
                .SubInteger2 = 1600
                .SubString1 = "Engine"
            End With
            engMenu.AddItem(itemEngine2(4))
            engMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateBrakesMenu()
        Try
            brkMenu = New UIMenu("", "BRAKES", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            brkMenu.SetBannerType(Rectangle)
            brkMenu.MouseEdgeEnabled = False
            _menuPool.Add(brkMenu)
            brkMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            brkMenu.RefreshIndex()
            tunMenu.BindMenuToItem(brkMenu, itemBrakes)
            AddHandler brkMenu.OnItemSelect, AddressOf UpgradeMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshBrakesMenu()
        Try
            brkMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("Brakes", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            itemBrakes2(0) = New UIMenuItem("Stock Brakes")
            With itemBrakes2(0)
                If currentMod = -1 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$100")
                .SubInteger1 = -1
                .SubInteger2 = 100
                .SubString1 = "Brakes"
            End With
            brkMenu.AddItem(itemBrakes2(0))
            itemBrakes2(1) = New UIMenuItem("Street Brakes")
            With itemBrakes2(1)
                If currentMod = 0 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$200")
                .SubInteger1 = 0
                .SubInteger2 = 200
                .SubString1 = "Brakes"
            End With
            brkMenu.AddItem(itemBrakes2(1))
            itemBrakes2(2) = New UIMenuItem("Sport Brakes")
            With itemBrakes2(2)
                If currentMod = 1 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$400")
                .SubInteger1 = 1
                .SubInteger2 = 400
                .SubString1 = "Brakes"
            End With
            brkMenu.AddItem(itemBrakes2(2))
            itemBrakes2(3) = New UIMenuItem("Race Brakes")
            With itemBrakes2(3)
                If currentMod = 2 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$800")
                .SubInteger1 = 2
                .SubInteger2 = 800
                .SubString1 = "Brakes"
            End With
            brkMenu.AddItem(itemBrakes2(3))
            brkMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateTransmissionMenu()
        Try
            traMenu = New UIMenu("", "TRANSMISSION", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            traMenu.SetBannerType(Rectangle)
            traMenu.MouseEdgeEnabled = False
            _menuPool.Add(traMenu)
            traMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            traMenu.RefreshIndex()
            tunMenu.BindMenuToItem(traMenu, itemTrans)
            AddHandler traMenu.OnItemSelect, AddressOf UpgradeMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshTransmissionMenu()
        Try
            traMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("Transmission", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            itemTrans2(0) = New UIMenuItem("Stock Transmission")
            With itemTrans2(0)
                If currentMod = -1 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$100")
                .SubInteger1 = -1
                .SubInteger2 = 100
                .SubString1 = "Transmission"
            End With
            traMenu.AddItem(itemTrans2(0))
            itemTrans2(1) = New UIMenuItem("Street Transmission")
            With itemTrans2(1)
                If currentMod = 0 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$200")
                .SubInteger1 = 0
                .SubInteger2 = 200
                .SubString1 = "Transmission"
            End With
            traMenu.AddItem(itemTrans2(1))
            itemTrans2(2) = New UIMenuItem("Sport Transmission")
            With itemTrans2(2)
                If currentMod = 1 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$400")
                .SubInteger1 = 1
                .SubInteger2 = 400
                .SubString1 = "Transmission"
            End With
            traMenu.AddItem(itemTrans2(2))
            itemTrans2(3) = New UIMenuItem("Race Transmission")
            With itemTrans2(3)
                If currentMod = 2 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$800")
                .SubInteger1 = 2
                .SubInteger2 = 800
                .SubString1 = "Transmission"
            End With
            traMenu.AddItem(itemTrans2(3))
            traMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateSuspensionMenu()
        Try
            susMenu = New UIMenu("", "SUSPENSION", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            susMenu.SetBannerType(Rectangle)
            susMenu.MouseEdgeEnabled = False
            _menuPool.Add(susMenu)
            susMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            susMenu.RefreshIndex()
            tunMenu.BindMenuToItem(susMenu, itemSusp)
            AddHandler susMenu.OnItemSelect, AddressOf UpgradeMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshSuspensionMenu()
        Try
            susMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("Suspension", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            itemSusp2(0) = New UIMenuItem("Stock Suspension")
            With itemSusp2(0)
                If currentMod = -1 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$100")
                .SubInteger1 = -1
                .SubInteger2 = 100
                .SubString1 = "Suspension"
            End With
            susMenu.AddItem(itemSusp2(0))
            itemSusp2(1) = New UIMenuItem("Lowered Suspension")
            With itemSusp2(1)
                If currentMod = 0 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$200")
                .SubInteger1 = 0
                .SubInteger2 = 200
                .SubString1 = "Suspension"
            End With
            susMenu.AddItem(itemSusp2(1))
            itemSusp2(2) = New UIMenuItem("Street Suspension")
            With itemSusp2(2)
                If currentMod = 1 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$400")
                .SubInteger1 = 1
                .SubInteger2 = 400
                .SubString1 = "Suspension"
            End With
            susMenu.AddItem(itemSusp2(2))
            itemSusp2(3) = New UIMenuItem("Sport Suspension")
            With itemSusp2(3)
                If currentMod = 2 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$800")
                .SubInteger1 = 2
                .SubInteger2 = 800
                .SubString1 = "Suspension"
            End With
            susMenu.AddItem(itemSusp2(3))
            itemSusp2(4) = New UIMenuItem("Race Suspension")
            With itemSusp2(4)
                If currentMod = 3 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$1,600")
                .SubInteger1 = 3
                .SubInteger2 = 1600
                .SubString1 = "Suspension"
            End With
            susMenu.AddItem(itemSusp2(4))
            susMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateArmorMenu()
        Try
            armMenu = New UIMenu("", "ARMOR", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            armMenu.SetBannerType(Rectangle)
            armMenu.MouseEdgeEnabled = False
            _menuPool.Add(armMenu)
            armMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            armMenu.RefreshIndex()
            tunMenu.BindMenuToItem(armMenu, itemArmor)
            AddHandler armMenu.OnItemSelect, AddressOf UpgradeMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshArmorMenu()
        Try
            armMenu.MenuItems.Clear()
            Dim currentMod As Integer = ReadCfgValue("Armor", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            itemArmor2(0) = New UIMenuItem("Stock Armor")
            With itemArmor2(0)
                If currentMod = -1 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("FREE")
                .SubInteger1 = -1
                .SubInteger2 = 0
                .SubString1 = "Armor"
            End With
            armMenu.AddItem(itemArmor2(0))
            itemArmor2(1) = New UIMenuItem("Armor Upgrade 20%")
            With itemArmor2(1)
                If currentMod = 0 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$100")
                .SubInteger1 = 0
                .SubInteger2 = 200
                .SubString1 = "Armor"
            End With
            armMenu.AddItem(itemArmor2(1))
            itemArmor2(2) = New UIMenuItem("Armor Upgrade 40%")
            With itemArmor2(2)
                If currentMod = 1 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$200")
                .SubInteger1 = 1
                .SubInteger2 = 400
                .SubString1 = "Armor"
            End With
            armMenu.AddItem(itemArmor2(2))
            itemArmor2(3) = New UIMenuItem("Armor Upgrade 60%")
            With itemArmor2(3)
                If currentMod = 2 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$400")
                .SubInteger1 = 2
                .SubInteger2 = 800
                .SubString1 = "Armor"
            End With
            armMenu.AddItem(itemArmor2(3))
            itemArmor2(4) = New UIMenuItem("Armor Upgrade 80%")
            With itemArmor2(4)
                If currentMod = 3 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$800")
                .SubInteger1 = 3
                .SubInteger2 = 1600
                .SubString1 = "Armor"
            End With
            armMenu.AddItem(itemArmor2(4))
            itemArmor2(5) = New UIMenuItem("Armor Upgrade 100%")
            With itemArmor2(5)
                If currentMod = 4 Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$1,600")
                .SubInteger1 = 4
                .SubInteger2 = 3200
                .SubString1 = "Armor"
            End With
            armMenu.AddItem(itemArmor2(5))
            armMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub CreateTurboMenu()
        Try
            trbMenu = New UIMenu("", "TURBO", New Point(0, 187))
            Dim Rectangle = New UIResRectangle()
            Rectangle.Color = Color.FromArgb(0, 0, 0, 0)
            trbMenu.SetBannerType(Rectangle)
            trbMenu.MouseEdgeEnabled = False
            _menuPool.Add(trbMenu)
            trbMenu.AddItem(New UIMenuItem("Nothing Here") With {.Enabled = False})
            trbMenu.RefreshIndex()
            tunMenu.BindMenuToItem(trbMenu, itemTurbo)
            AddHandler trbMenu.OnItemSelect, AddressOf UpgradeMenuItemSelectHandler
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub RefreshTurboMenu()
        Try
            trbMenu.MenuItems.Clear()
            Dim currentMod As String = ReadCfgValue("Turbo", carDir & PlayerVehicle.Model.Hash().ToString & ".cfg")
            itemTurbo2(0) = New UIMenuItem("None")
            With itemTurbo2(0)
                If currentMod = "False" Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$2,500")
                .SubString2 = "False"
                .SubInteger2 = 2500
                .SubString1 = "Turbo"
            End With
            trbMenu.AddItem(itemTurbo2(0))
            itemTurbo2(1) = New UIMenuItem("Turbo Tuning")
            With itemTurbo2(1)
                If currentMod = "True" Then .SetRightBadge(UIMenuItem.BadgeStyle.Car) Else .SetRightLabel("$5,000")
                .SubString2 = "True"
                .SubInteger2 = 5000
                .SubString1 = "Turbo"
            End With
            trbMenu.AddItem(itemTurbo2(1))
            trbMenu.RefreshIndex()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub
#End Region

    Public Shared Sub UpgradeMenuItemSelectHandler(sender As UIMenu, selectedItem As UIMenuItem, index As Integer)
        Try
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "Engine" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    Native.Function.Call(Hash.SET_VEHICLE_MOD_KIT, PlayerVehicle, 0)
                    PlayerVehicle.SetMod(VehicleMod.Engine, selectedItem.SubInteger1, False)
                    WriteCfgValue("Engine", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshEngineMenu()
                End If
            End If
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "Brakes" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    Native.Function.Call(Hash.SET_VEHICLE_MOD_KIT, PlayerVehicle, 0)
                    PlayerVehicle.SetMod(VehicleMod.Brakes, selectedItem.SubInteger1, False)
                    WriteCfgValue("Brakes", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshBrakesMenu()
                End If
            End If
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "Transmission" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    Native.Function.Call(Hash.SET_VEHICLE_MOD_KIT, PlayerVehicle, 0)
                    PlayerVehicle.SetMod(VehicleMod.Transmission, selectedItem.SubInteger1, False)
                    WriteCfgValue("Transmission", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshTransmissionMenu()
                End If
            End If
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "Suspension" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    Native.Function.Call(Hash.SET_VEHICLE_MOD_KIT, PlayerVehicle, 0)
                    PlayerVehicle.SetMod(VehicleMod.Suspension, selectedItem.SubInteger1, False)
                    WriteCfgValue("Suspension", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshSuspensionMenu()
                End If
            End If
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "Armor" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    Native.Function.Call(Hash.SET_VEHICLE_MOD_KIT, PlayerVehicle, 0)
                    PlayerVehicle.SetMod(VehicleMod.Armor, selectedItem.SubInteger1, False)
                    WriteCfgValue("Armor", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshArmorMenu()
                End If
            End If
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "Turbo" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    Native.Function.Call(Hash.SET_VEHICLE_MOD_KIT, PlayerVehicle, 0)
                    If selectedItem.SubString2 = "True" Then PlayerVehicle.ToggleMod(VehicleToggleMod.Turbo, True) Else PlayerVehicle.ToggleMod(VehicleToggleMod.Turbo, False)
                    WriteCfgValue("Turbo", selectedItem.SubString2, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshTurboMenu()
                End If
            End If
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "XenonHeadlights" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    Native.Function.Call(Hash.SET_VEHICLE_MOD_KIT, PlayerVehicle, 0)
                    If selectedItem.SubString2 = "True" Then PlayerVehicle.ToggleMod(VehicleToggleMod.XenonHeadlights, True) Else PlayerVehicle.ToggleMod(VehicleToggleMod.XenonHeadlights, False)
                    WriteCfgValue("XenonHeadlights", selectedItem.SubString2, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshLightsMenu()
                End If
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub KitSetMenuItemSelectHandler(sender As UIMenu, selectedItem As UIMenuItem, index As Integer)
        Try
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car Then
                If My.Settings.PlayerCash > CInt(selectedItem.SubString4) Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - CInt(selectedItem.SubString4)
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    Native.Function.Call(Hash.SET_VEHICLE_MOD_KIT, PlayerVehicle, 0)
                    PlayerVehicle.SetMod(VehicleMod.Spoilers, selectedItem.SubInteger1, True)
                    PlayerVehicle.SetMod(VehicleMod.FrontBumper, selectedItem.SubInteger2, True)
                    PlayerVehicle.SetMod(VehicleMod.RearBumper, selectedItem.SubInteger3, True)
                    PlayerVehicle.SetMod(VehicleMod.SideSkirt, selectedItem.SubInteger4, True)
                    PlayerVehicle.SetMod(VehicleMod.Frame, selectedItem.SubInteger5, True)
                    PlayerVehicle.SetMod(VehicleMod.Grille, selectedItem.SubInteger6, True)
                    PlayerVehicle.SetMod(VehicleMod.Hood, selectedItem.SubInteger7, True)
                    PlayerVehicle.SetMod(VehicleMod.Fender, selectedItem.SubInteger8, True)
                    PlayerVehicle.SetMod(VehicleMod.RightFender, selectedItem.SubInteger9, True)
                    PlayerVehicle.SetMod(VehicleMod.Roof, CInt(selectedItem.SubString1), True)
                    PlayerVehicle.SetMod(VehicleMod.Exhaust, CInt(selectedItem.SubString2), True)
                    WriteCfgValue("Spoiler", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("FrontBumper", selectedItem.SubInteger2, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("RearBumper", selectedItem.SubInteger3, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("SideSkirt", selectedItem.SubInteger4, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("Frame", selectedItem.SubInteger5, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("Grille", selectedItem.SubInteger6, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("Hood", selectedItem.SubInteger7, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("Fender", selectedItem.SubInteger8, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("RightFender", selectedItem.SubInteger9, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("Roof", selectedItem.SubString1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("Exhaust", selectedItem.SubString2, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("ExtraMoney", selectedItem.SubString6, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("ExtraEXP", selectedItem.SubString5, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("KitSet", selectedItem.Text, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshKitSetMenu()
                End If
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub MSColorMenuItemSelectHandler(sender As UIMenu, selectedItem As UIMenuItem, index As Integer)
        Try
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "PrimaryColor" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    PlayerVehicle.PrimaryColor = selectedItem.SubInteger1
                    WriteCfgValue("PrimaryColor", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshPrimaryColorMenu()
                End If
            End If
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "SecondaryColor" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    PlayerVehicle.SecondaryColor = selectedItem.SubInteger1
                    WriteCfgValue("SecondaryColor", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshSecondaryColorMenu()
                End If
            End If
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "PearlescentColor" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    PlayerVehicle.PearlescentColor = selectedItem.SubInteger1
                    WriteCfgValue("PearlescentColor", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshPearlescentColorMenu()
                End If
            End If
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "RimColor" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    PlayerVehicle.RimColor = selectedItem.SubInteger1
                    WriteCfgValue("RimColor", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshWheelColorMenu()
                End If
            End If
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "WindowTint" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    PlayerVehicle.WindowTint = selectedItem.SubInteger1
                    WriteCfgValue("WindowTint", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshTintColorMenu()
                End If
            End If
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "Wheels" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger2
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    Native.Function.Call(Hash.SET_VEHICLE_MOD_KIT, PlayerVehicle, 0)
                    PlayerVehicle.WheelType = selectedItem.SubInteger3
                    PlayerVehicle.SetMod(VehicleMod.FrontWheels, selectedItem.SubInteger1, False)
                    WriteCfgValue("WheelType", selectedItem.SubInteger3, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("FrontWheels", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshHighEndWheelMenu()
                    RefreshLowriderWheelMenu()
                    RefreshMuscleWheelMenu()
                    RefreshOffroadWheelMenu()
                    RefreshSportWheelMenu()
                    RefreshSUVWheelMenu()
                    RefreshTunerWheelMenu()
                End If
            End If
            If Not selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car AndAlso selectedItem.SubString1 = "Smoke" Then
                If My.Settings.PlayerCash > selectedItem.SubInteger2 Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - selectedItem.SubInteger4
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    WriteCfgValue("TyreSmokeColorRed", selectedItem.SubInteger1, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("TyreSmokeColorGreen", selectedItem.SubInteger2, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    WriteCfgValue("TyreSmokeColorBlue", selectedItem.SubInteger3, carDir & PlayerVehicle.Model.Hash() & ".cfg")
                    RefreshTyreSmokeMenu()
                End If
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub WheelTypeMenuItemSelectHandler(sender As UIMenu, selectedItem As UIMenuItem, index As Integer)
        Try
            If selectedItem Is itemHighWhl Then
                RefreshHighEndWheelMenu()
            ElseIf selectedItem Is itemLowridWhl Then
                RefreshLowriderWheelMenu()
            ElseIf selectedItem Is itemMuscleWhl Then
                RefreshMuscleWheelMenu()
            ElseIf selectedItem Is itemOffrdWhl Then
                RefreshOffroadWheelMenu()
            ElseIf selectedItem Is itemSportWhl Then
                RefreshSportWheelMenu()
            ElseIf selectedItem Is itemSUVWhl Then
                RefreshSUVWheelMenu()
            ElseIf selectedItem Is itemTunerWhl Then
                RefreshTunerWheelMenu()
            ElseIf selectedItem Is itemSmoke Then
                RefreshTyreSmokeMenu()
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub ModshopMenuItemSelectHandler(sender As UIMenu, selectedItem As UIMenuItem, index As Integer)
        Try
            If selectedItem Is itemPrimary Then
                RefreshPrimaryColorMenu()
            ElseIf selectedItem Is itemSecondary Then
                RefreshSecondaryColorMenu()
            ElseIf selectedItem Is itemPearl Then
                RefreshPearlescentColorMenu()
            ElseIf selectedItem Is itemRimCol Then
                RefreshWheelColorMenu()
            ElseIf selectedItem Is itemTint Then
                RefreshTintColorMenu()
            ElseIf selectedItem Is itemKit Then
                RefreshKitSetMenu()
            ElseIf selectedItem Is itemLights Then
                RefreshLightsMenu()
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub TunningMenuItemSelectHandler(sender As UIMenu, selectedItem As UIMenuItem, index As Integer)
        Try
            If selectedItem Is itemEngine Then
                RefreshEngineMenu()
            ElseIf selectedItem Is itemBrakes Then
                RefreshBrakesMenu()
            ElseIf selectedItem Is itemTrans Then
                RefreshTransmissionMenu()
            ElseIf selectedItem Is itemSusp Then
                RefreshSuspensionMenu()
            ElseIf selectedItem Is itemArmor Then
                RefreshArmorMenu()
            ElseIf selectedItem Is itemTurbo Then
                RefreshTurboMenu()
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub TrackMenuItemSelectHandler(sender As UIMenu, selectedItem As UIMenuItem, index As Integer)
        Try
            My.Settings.PlayerLastTrack = selectedItem.Text & ".trk"
            My.Settings.Save()
            WriteCfgValue("LastTrack", selectedItem.Text & ".trk", saveFile)
            sender.GoBack()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub GarageMenuItemSelectedHandler(sender As UIMenu, selectedItem As UIMenuItem, index As Integer)
        Try
            Dim VehHash As Integer = CInt(selectedItem.SubString1)
            Dim VehPrice As Integer = selectedItem.SubInteger1
            If Not (selectedItem.RightBadge = UIMenuItem.BadgeStyle.Car Or selectedItem.RightBadge = UIMenuItem.BadgeStyle.Tick) Then
                If My.Settings.PlayerCash > VehPrice Then
                    Dim PlayerCashNew As Integer = My.Settings.PlayerCash - VehPrice
                    My.Settings.PlayerCash = PlayerCashNew
                    My.Settings.PlayerLastVeh = VehHash
                    My.Settings.Save()
                    WriteCfgValue("Bank", PlayerCashNew, saveFile)
                    WriteCfgValue("LastVeh", VehHash, saveFile)
                    WriteCfgValue("VehicleOwn", "True", carDir & VehHash & ".cfg")
                    selectedItem.SetRightLabel("")
                    selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car)
                    RefreshGarageMenu()
                End If
            Else
                My.Settings.PlayerLastVeh = VehHash
                My.Settings.Save()
                WriteCfgValue("LastVeh", VehHash, saveFile)
                RefreshGarageMenu()
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub KitSetIndexChangeHandler(sender As UIMenu, index As Integer)
        Try
            Native.Function.Call(Hash.SET_VEHICLE_MOD_KIT, PlayerVehicle, 0)
            PlayerVehicle.SetMod(VehicleMod.Spoilers, sender.MenuItems(index).SubInteger1, True)
            PlayerVehicle.SetMod(VehicleMod.FrontBumper, sender.MenuItems(index).SubInteger2, True)
            PlayerVehicle.SetMod(VehicleMod.RearBumper, sender.MenuItems(index).SubInteger3, True)
            PlayerVehicle.SetMod(VehicleMod.SideSkirt, sender.MenuItems(index).SubInteger4, True)
            PlayerVehicle.SetMod(VehicleMod.Frame, sender.MenuItems(index).SubInteger5, True)
            PlayerVehicle.SetMod(VehicleMod.Grille, sender.MenuItems(index).SubInteger6, True)
            PlayerVehicle.SetMod(VehicleMod.Hood, sender.MenuItems(index).SubInteger7, True)
            PlayerVehicle.SetMod(VehicleMod.Fender, sender.MenuItems(index).SubInteger8, True)
            PlayerVehicle.SetMod(VehicleMod.RightFender, sender.MenuItems(index).SubInteger9, True)
            PlayerVehicle.SetMod(VehicleMod.Roof, CInt(sender.MenuItems(index).SubString1), True)
            PlayerVehicle.SetMod(VehicleMod.Exhaust, CInt(sender.MenuItems(index).SubString2), True)
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub MSColorIndexChangeHandler(sender As UIMenu, index As Integer)
        Try
            If sender.MenuItems(index).SubString1 = "PrimaryColor" Then
                PlayerVehicle.PrimaryColor = sender.MenuItems(index).SubInteger1
            End If
            If sender.MenuItems(index).SubString1 = "SecondaryColor" Then
                PlayerVehicle.SecondaryColor = sender.MenuItems(index).SubInteger1
            End If
            If sender.MenuItems(index).SubString1 = "PearlescentColor" Then
                PlayerVehicle.PearlescentColor = sender.MenuItems(index).SubInteger1
            End If
            If sender.MenuItems(index).SubString1 = "RimColor" Then
                PlayerVehicle.RimColor = sender.MenuItems(index).SubInteger1
            End If
            If sender.MenuItems(index).SubString1 = "WindowTint" Then
                PlayerVehicle.WindowTint = sender.MenuItems(index).SubInteger1
            End If
            If sender.MenuItems(index).SubString1 = "Wheels" Then
                PlayerVehicle.WheelType = sender.MenuItems(index).SubInteger3
                PlayerVehicle.SetMod(VehicleMod.FrontWheels, sender.MenuItems(index).SubInteger1, False)
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub GarageMenuIndexChangeHandler(sender As UIMenu, index As Integer)
        Try
            PlayerVehicle.IsDriveable = True
            Native.Function.Call(Hash.SET_VEHICLE_ENGINE_ON, PlayerVehicle, True, True, True)
            Native.Function.Call(Hash.SET_VEH_RADIO_STATION, PlayerVehicle, "OFF")
            PlayerPed.Task.DriveTo(PlayerVehicle, xyzFront, 0.01, 4.0)
            TaskDriveTimer.Start()
            selectedModel = CInt(sender.MenuItems(index).SubString1)
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub SettingMenuListChangeHandler(sender As UIMenu, item As UIMenuListItem, index As Integer)
        Try
            If item Is itemWeather Then
                Dim selectedItem As String = item.IndexToItem(index).ToString()
                Select Case selectedItem
                    Case "Extra Sunny"
                        WriteCfgValue("Weather", "0", saveFile)
                    Case "Clear"
                        WriteCfgValue("Weather", "1", saveFile)
                    Case "Neutral"
                        WriteCfgValue("Weather", "9", saveFile)
                    Case "Smog"
                        WriteCfgValue("Weather", "3", saveFile)
                    Case "Foggy"
                        WriteCfgValue("Weather", "4", saveFile)
                    Case "Overcast"
                        WriteCfgValue("Weather", "5", saveFile)
                    Case "Clouds"
                        WriteCfgValue("Weather", "2", saveFile)
                    Case "Clearing"
                        WriteCfgValue("Weather", "8", saveFile)
                    Case "Rain"
                        WriteCfgValue("Weather", "6", saveFile)
                    Case "Thunder"
                        WriteCfgValue("Weather", "7", saveFile)
                    Case "Snow"
                        WriteCfgValue("Weather", "10", saveFile)
                    Case "Blizzard"
                        WriteCfgValue("Weather", "11", saveFile)
                    Case "Light Snow"
                        WriteCfgValue("Weather", "12", saveFile)
                    Case "X-mas"
                        WriteCfgValue("Weather", "13", saveFile)
                End Select
            ElseIf item Is itemTime Then
                Dim selectedItem As String = item.IndexToItem(index).ToString()
                Select Case selectedItem
                    Case "Morning"
                        WriteCfgValue("Time", "7", saveFile)
                    Case "Noon"
                        WriteCfgValue("Time", "12", saveFile)
                    Case "Afternoon"
                        WriteCfgValue("Time", "18", saveFile)
                    Case "Evening"
                        WriteCfgValue("Time", "23", saveFile)
                    Case "Midnight"
                        WriteCfgValue("Time", "0", saveFile)
                    Case "Night"
                        WriteCfgValue("Time", "5", saveFile)
                End Select
            ElseIf item Is itemTraffic Then
                Dim selectedItem As String = item.IndexToItem(index).ToString()
                Select Case selectedItem
                    Case "On"
                        WriteCfgValue("Traffic", "True", saveFile)
                    Case "Off"
                        WriteCfgValue("Traffic", "False", saveFile)
                End Select
            ElseIf item Is itemPolice Then
                Dim selectedItem As String = item.IndexToItem(index).ToString()
                Select Case selectedItem
                    Case "On"
                        WriteCfgValue("Police", "True", saveFile)
                    Case "Off"
                        WriteCfgValue("Police", "False", saveFile)
                End Select
            ElseIf item Is itemVolume Then
                Dim selectedItem As Integer = CInt(item.IndexToItem(index).ToString())
                Select Case selectedItem
                    Case 0
                        WriteCfgValue("Volume", "0", saveFile)
                        Player.settings.volume = 0
                    Case 10
                        WriteCfgValue("Volume", "10", saveFile)
                        Player.settings.volume = 10
                    Case 20
                        WriteCfgValue("Volume", "20", saveFile)
                        Player.settings.volume = 20
                    Case 30
                        WriteCfgValue("Volume", "30", saveFile)
                        Player.settings.volume = 30
                    Case 40
                        WriteCfgValue("Volume", "40", saveFile)
                        Player.settings.volume = 40
                    Case 50
                        WriteCfgValue("Volume", "50", saveFile)
                        Player.settings.volume = 50
                    Case 60
                        WriteCfgValue("Volume", "60", saveFile)
                        Player.settings.volume = 60
                    Case 70
                        WriteCfgValue("Volume", "70", saveFile)
                        Player.settings.volume = 70
                    Case 80
                        WriteCfgValue("Volume", "80", saveFile)
                        Player.settings.volume = 80
                    Case 90
                        WriteCfgValue("Volume", "90", saveFile)
                        Player.settings.volume = 90
                    Case 100
                        WriteCfgValue("Volume", "100", saveFile)
                        Player.settings.volume = 100
                End Select
            ElseIf item Is itemMusic Then
                Dim selectedItem As String = item.IndexToItem(index).ToString()
                Select Case selectedItem
                    Case "On"
                        WriteCfgValue("Music", "True", saveFile)
                        Player.URL = musicDir & "pl.wpl"
                        Player.controls.play()
                        Player.settings.volume = CInt(ReadCfgValue("Volume", saveFile))
                        Player.settings.setMode("loop", True)
                    Case "Off"
                        WriteCfgValue("Music", "False", saveFile)
                        Player.close()
                End Select
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub MainMenuItemSelectHandler(sender As UIMenu, selectedItem As UIMenuItem, index As Integer)
        Try
            If selectedItem Is itemPlay Then
                Select Case My.Settings.PlayerLastMode
                    Case "Checkpoint"

                    Case "Freeroam"

                    Case "Timeattack"

                    Case "Survival"

                    Case Else
                        UI.Notify("You didn't choose a Game Mode.")
                        Exit Sub
                End Select
                FadeScreenOut(500)
                Wait(1000)
                World.Weather = CInt(ReadCfgValue("Weather", saveFile))
                Native.Function.Call(Hash.SET_CLOCK_TIME, CInt(ReadCfgValue("Time", saveFile)), 0, 0)
                DriftStreetLS.PlayerCash = My.Settings.PlayerCash
                DriftStreetLS.PlayerHighscore = My.Settings.PlayerHighscore
                DriftStreetLS.PlayerEXP = My.Settings.EXP
                DriftStreetLS.playerRank = GetRankIndex(My.Settings.EXP)
                DriftStreetLS.ExtraMoney = CInt(ReadCfgValue("ExtraMoney", carDir & PlayerVehicle.Model.Hash.ToString() & ".cfg")) 'Convert.ToDouble(), New CultureInfo("en-US")
                DriftStreetLS.ExtraEXP = CInt(ReadCfgValue("ExtraEXP", carDir & PlayerVehicle.Model.Hash.ToString() & ".cfg")) 'Convert.ToDouble(), New CultureInfo("en-US")
                sender.Visible = False
                World.DestroyAllCameras()
                World.RenderingCamera = Nothing
                Dim NewPlayerPosition As Vector3 = New Vector3(ReadCfgValue("PlayerX", trackDir & My.Settings.PlayerLastTrack), ReadCfgValue("PlayerY", trackDir & My.Settings.PlayerLastTrack), ReadCfgValue("PlayerZ", trackDir & My.Settings.PlayerLastTrack))
                PlayerPed.Position = NewPlayerPosition
                CreateVehicle(My.Settings.PlayerLastVeh, NewPlayerPosition, ReadCfgValue("PlayerH", trackDir & My.Settings.PlayerLastTrack))
                PlayerPed.SetIntoVehicle(PlayerVehicle, VehicleSeat.Driver)
                SetModKit(PlayerVehicle, carDir & My.Settings.PlayerLastVeh & ".cfg")
                DriftStreetLS.ModActivate = True
                Player.close()
                HideHud = False
                Wait(500)
                FadeScreenIn(500)
            ElseIf selectedItem Is itemExit Then
                ExitGame()
            ElseIf selectedItem Is itemGarage Then
                RefreshGarageMenu()
            ElseIf selectedItem Is itemCredits Then
                DrawTexture()
            ElseIf selectedItem Is itemTop10 Then
                RefreshLeaderboardMenu()
            ElseIf selectedItem Is itemSurvivalMode Then
                My.Settings.PlayerLastMode = "Survival"
                My.Settings.Save()
                WriteCfgValue("LastMode", "Survival", saveFile)
            ElseIf selectedItem Is itemFreeRoamMode Then
                My.Settings.PlayerLastMode = "Freeroam"
                My.Settings.Save()
                WriteCfgValue("LastMode", "Freeroam", saveFile)
            ElseIf selectedItem Is itemTimeAttackMode Then
                My.Settings.PlayerLastMode = "Timeattack"
                My.Settings.Save()
                WriteCfgValue("LastMode", "Timeattack", saveFile)
            ElseIf selectedItem Is itemCheckpointMode Then
                My.Settings.PlayerLastMode = "Checkpoint"
                My.Settings.Save()
                WriteCfgValue("LastMode", "Checkpoint", saveFile)
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub MSColorMenuCloseHandler(sender As UIMenu)
        Try
            SetModKit(PlayerVehicle, carDir & PlayerVehicle.Model.Hash.ToString() & ".cfg")
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub GarageMenuMenuCloseHandler(sender As UIMenu)
        Try
            PlayerVehicle.IsDriveable = True
            Native.Function.Call(Hash.SET_VEHICLE_ENGINE_ON, PlayerVehicle, True, True, True)
            Native.Function.Call(Hash.SET_VEH_RADIO_STATION, PlayerVehicle, "OFF")
            PlayerPed.Task.DriveTo(PlayerVehicle, xyzFront, 0.01, 4.0)
            TaskDriveTimer.Start()
            selectedModel = My.Settings.PlayerLastVeh
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub MainMenuCloseHandler(sender As UIMenu)
        Try
            sender.Visible = True
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub ExitGame()
        Try
            FadeScreenOut(500)
            Wait(1000)
            PlayerPed.Task.ClearAllImmediately()
            TaskDriveTimer.Enabled = False
            World.DestroyAllCameras()
            World.RenderingCamera = Nothing
            If Not PlayerVehicle = Nothing Then PlayerVehicle.Delete()
            HideHud = False
            Player.close()
            If DriftStreetLS.ModActivate = False Then
                If Not PlayerVehicle = Nothing Then PlayerVehicle.Delete()
                PlayerPed.Position = LastXYZ
            End If
            dsMenu.Visible = False
            Wait(500)
            FadeScreenIn(500)
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub StartGame()
        Try
            FadeScreenOut(500)
            Wait(1000)
            World.RenderingCamera = World.CreateCamera(New Vector3(406.3065, -963.5696, -99.00932), New Vector3(0.7160863, 0, 179.0708), 30)
            DriftStreetLS.ModActivate = False
            PlayerPed.Position = xyz
            HideHud = True
            CreateVehicle(My.Settings.PlayerLastVeh, xyz, 314.2483)
            PlayerVehicle.IsDriveable = False
            PlayerPed.SetIntoVehicle(PlayerVehicle, VehicleSeat.Driver)
            SetModKit(PlayerVehicle, carDir & My.Settings.PlayerLastVeh & ".cfg")
            dsMenu.Visible = Not dsMenu.Visible
            If ReadCfgValue("Music", saveFile) = "True" Then
                Player.URL = musicDir & "pl.wpl"
                Player.controls.play()
                Player.settings.volume = CInt(ReadCfgValue("Volume", saveFile))
                Player.settings.setMode("loop", True)
            End If
            Wait(500)
            FadeScreenIn(500)
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Sub AIDriveTask(Model As Integer)
        Try
            CreateVehicle(Model, xyzBack, 314.2483)
            PlayerPed.SetIntoVehicle(PlayerVehicle, VehicleSeat.Driver)
            Native.Function.Call(Hash.SET_VEH_RADIO_STATION, PlayerVehicle, "OFF")
            SetModKit(PlayerVehicle, carDir & Model & ".cfg")
            PlayerPed.Task.DriveTo(PlayerVehicle, xyz, 0.1, 5)
            Native.Function.Call(Hash.SET_VEHICLE_ENGINE_ON, PlayerVehicle, False, True, True)
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Sub OnTick(o As Object, e As EventArgs)
        Try
            PlayerOne = Game.Player
            PlayerPed = Game.Player.Character

            If TaskDriveTimer.Enabled Then
                If Game.GameTime > TaskDriveTimer.Waiter Then
                    TaskDriveTimer.Enabled = False
                    AIDriveTask(selectedModel)
                End If
            End If

            If HideHud Then
                Native.Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME)
                Dim formatCash As String = "$" & My.Settings.PlayerCash.ToString("###,###")
                Dim formatScore As String = My.Settings.PlayerHighscore.ToString("###,###")
                Dim formatLevel As String = GetRankIndex(My.Settings.EXP)
                DrawText("Rank: " & formatLevel, New PointF(dsMenu.WidthOffset, UI.HEIGHT - 110), 1.0, Color.White, Enums.GTAFont.Title, Enums.GTAFontAlign.Left, Enums.GTAFontStyleOptions.Outline)
                DrawText("Bank: " & formatCash, New PointF(dsMenu.WidthOffset, UI.HEIGHT - 80), 1.0, Color.White, Enums.GTAFont.Title, Enums.GTAFontAlign.Left, Enums.GTAFontStyleOptions.Outline)
                DrawText("Best: " & formatScore, New PointF(dsMenu.WidthOffset, UI.HEIGHT - 50), 1.0, Color.White, Enums.GTAFont.Title, Enums.GTAFontAlign.Left, Enums.GTAFontStyleOptions.Outline)
                UI.DrawTexture(mainDir & "logo.png", 0, 0, 60, New Point(dsMenu.WidthOffset, 20), New Size(450, 166), 0.0, Color.White)
            End If

            _menuPool.ProcessMenus()
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Sub OnKeyDown(o As Object, e As KeyEventArgs)
        Try
            If e.KeyCode = My.Settings.Key Then
                If _menuPool.IsAnyMenuOpen = False Then
                    If DriftStreetLS.ModActivate = True Then
                        My.Settings.PlayerHighscore = DriftStreetLS.PlayerHighscore
                        My.Settings.PlayerCash = DriftStreetLS.PlayerCash
                        My.Settings.EXP = DriftStreetLS.PlayerEXP
                        My.Settings.Save()
                        WriteCfgValue("Highscore", My.Settings.PlayerHighscore, saveFile)
                        WriteCfgValue("Bank", My.Settings.PlayerCash, saveFile)
                        WriteCfgValue("EXP", My.Settings.EXP, saveFile)
                        StartGame()
                    Else
                        LastXYZ = New Vector3(PlayerPed.Position.X, PlayerPed.Position.Y, PlayerPed.Position.Z)
                        SetInteriorActive(404.2812, -963.2419, -99.00419) 'Underground Parking
                        StartGame()
                    End If
                End If
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub
End Class
