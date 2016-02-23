Imports System.Drawing
Imports System.Windows.Forms
Imports GTA
Imports GTA.Native
Imports GTA.Math
Imports DriftStreetLS.Enums
Imports System.Net
Imports System.Security.Cryptography

Public Class Resources

    Public Shared PrivateKey As String = "DSLSROCKS"
    Public Shared AddScoreURL As String = "http://notmental.ml/dsls/AddScore.php?"
    Const passphrase As String = "DSLSROCKS"

    Public Shared Sub DisplayHelpTextThisFrame(ByVal [text] As String)
        Dim arguments As InputArgument() = New InputArgument() {"STRING"}
        Native.Function.Call(Hash._0x8509B634FBE7DA11, arguments)
        Dim argumentArray2 As InputArgument() = New InputArgument() {[text]}
        Native.Function.Call(Hash._0x6C188BE134E074AA, argumentArray2)
        Dim argumentArray3 As InputArgument() = New InputArgument() {0, 0, 1, -1}
        Native.Function.Call(Hash._0x238FFE5C7B0498A6, argumentArray3)
    End Sub

    Public Shared Sub DrawText(ByVal [Text] As String, ByVal Position As PointF, ByVal Scale As Single, ByVal color As Color, ByVal Font As GTAFont, ByVal Alignment As GTAFontAlign, ByVal Options As GTAFontStyleOptions)
        Dim arguments As InputArgument() = New InputArgument() {Font}
        Native.Function.Call(Hash._0x66E0276CC5F6B9DA, arguments)
        Dim argumentArray2 As InputArgument() = New InputArgument() {1.0!, Scale}
        Native.Function.Call(Hash._0x07C837F9A01C34C9, argumentArray2)
        Dim argumentArray3 As InputArgument() = New InputArgument() {color.R, color.G, color.B, color.A}
        Native.Function.Call(Hash._0xBE6B23FFA53FB442, argumentArray3)
        If Options.HasFlag(GTAFontStyleOptions.DropShadow) Then
            Native.Function.Call(Hash._0x1CA3E9EAC9D93E5E, New InputArgument(0 - 1) {})
        End If
        If Options.HasFlag(GTAFontStyleOptions.Outline) Then
            Native.Function.Call(Hash._0x2513DFB0FB8400FE, New InputArgument(0 - 1) {})
        End If
        If Alignment.HasFlag(GTAFontAlign.Center) Then
            Dim argumentArray4 As InputArgument() = New InputArgument() {1}
            Native.Function.Call(Hash._0xC02F4DBFB51D988B, argumentArray4)
        ElseIf Alignment.HasFlag(GTAFontAlign.Right) Then
            Dim argumentArray5 As InputArgument() = New InputArgument() {1}
            Native.Function.Call(Hash._0x6B3C4650BC8BEE47, argumentArray5)
        End If
        Dim argumentArray6 As InputArgument() = New InputArgument() {"jamyfafi"}
        Native.Function.Call(Hash._0x25FBB336DF1804CB, argumentArray6)
        PushBigString([Text])
        Dim argumentArray7 As InputArgument() = New InputArgument() {(Position.X / 1280.0!), (Position.Y / 720.0!)}
        Native.Function.Call(Hash._0xCD015E5BB0D96A57, argumentArray7)
    End Sub

    Public Shared Sub PushBigString(ByVal [Text] As String)
        Dim strArray As String() = SplitStringEveryNth([Text], &H63)
        Dim i As Integer
        For i = 0 To strArray.Length - 1
            Dim arguments As InputArgument() = New InputArgument() {[Text].Substring((i * &H63), strArray(i).Length)}
            Native.Function.Call(Hash._0x6C188BE134E074AA, arguments)
        Next i
    End Sub

    Private Shared Function SplitStringEveryNth(ByVal [text] As String, ByVal Nth As Integer) As String()
        Dim list As New List(Of String)
        Dim item As String = ""
        Dim num As Integer = 0
        Dim i As Integer
        For i = 0 To [text].Length - 1
            item = (item & [text].Chars(i).ToString)
            num += 1
            If ((i <> 0) AndAlso (num = Nth)) Then
                list.Add(item)
                item = ""
                num = 0
            End If
        Next i
        If (item <> "") Then
            list.Add(item)
        End If
        Return list.ToArray
    End Function

    Public Shared Function IsDrifting(Vehicle As Vehicle) As Boolean
        Dim forwardvel = Vector3.Dot(Vehicle.Velocity, Vehicle.ForwardVector)
        Dim result As Boolean

        If forwardvel > -5 Then
            Dim leftvel = Vector3.Dot(Vehicle.Velocity, -Vehicle.RightVector)
            Dim rightvel = Vector3.Dot(Vehicle.Velocity, Vehicle.RightVector)

            If leftvel > 2 AndAlso rightvel < -2.1 OrElse rightvel > 2 AndAlso leftvel < 2.1 Then
                If Not Native.Function.Call(Of Boolean)(Hash.HAS_ENTITY_COLLIDED_WITH_ANYTHING, Vehicle.Handle) AndAlso Vehicle.Speed > 0.5 AndAlso Not Vehicle.IsInAir Then
                    result = True
                Else
                    result = False
                End If
            End If
        End If
        Return result
    End Function

    Public Shared Sub SetInteriorActive(X As Single, Y As Single, Z As Single)
        Dim interiorID As Integer = Native.Function.Call(Of Integer)(Hash.GET_INTERIOR_AT_COORDS, X, Y, Z)
        Native.Function.Call(Hash._0x2CA429C029CCF247, New InputArgument() {interiorID})
        Native.Function.Call(Hash.SET_INTERIOR_ACTIVE, interiorID, True)
        Native.Function.Call(Hash.DISABLE_INTERIOR, interiorID, False)
    End Sub

    Public Shared Function getNumVehMod(_vehicle As Vehicle, _modType As Integer) As Integer
        If Native.Function.Call(Of Integer)(Hash.GET_NUM_VEHICLE_MODS, _vehicle, _modType) > 1 Then
            Return Native.Function.Call(Of Integer)(Hash.GET_NUM_VEHICLE_MODS, _vehicle, _modType) - 1
        End If
        Return 0
    End Function

    Public Shared Function GetHowManyPossibleModForThisVehicle(vehicle As Vehicle, modType As Integer) As Integer
        Return Native.Function.Call(Of Integer)(Hash.GET_NUM_VEHICLE_MODS, vehicle, modType)
    End Function

    Public Shared Sub SetModKit(_Vehicle As Vehicle, VehicleCfgFile As String)
        Try
            Native.Function.Call(Hash.SET_VEHICLE_MOD_KIT, _Vehicle, 0)
            _Vehicle.DirtLevel = 0F
            _Vehicle.PrimaryColor = ReadCfgValue("PrimaryColor", VehicleCfgFile)
            _Vehicle.SecondaryColor = ReadCfgValue("SecondaryColor", VehicleCfgFile)
            _Vehicle.PearlescentColor = ReadCfgValue("PearlescentColor", VehicleCfgFile)
            If ReadCfgValue("HasCustomPrimaryColor", VehicleCfgFile) = "True" Then _Vehicle.CustomPrimaryColor = Color.FromArgb(ReadCfgValue("CustomPrimaryColorRed", VehicleCfgFile), ReadCfgValue("CustomPrimaryColorGreen", VehicleCfgFile), ReadCfgValue("CustomPrimaryColorBlue", VehicleCfgFile))
            If ReadCfgValue("HasCustomSecondaryColor", VehicleCfgFile) = "True" Then _Vehicle.CustomSecondaryColor = Color.FromArgb(ReadCfgValue("CustomSecondaryColorRed", VehicleCfgFile), ReadCfgValue("CustomSecondaryColorGreen", VehicleCfgFile), ReadCfgValue("CustomSecondaryColorBlue", VehicleCfgFile))
            _Vehicle.RimColor = ReadCfgValue("RimColor", VehicleCfgFile)
            If ReadCfgValue("HasNeonLightBack", VehicleCfgFile) = "True" Then _Vehicle.SetNeonLightsOn(VehicleNeonLight.Back, True)
            If ReadCfgValue("HasNeonLightFront", VehicleCfgFile) = "True" Then _Vehicle.SetNeonLightsOn(VehicleNeonLight.Front, True)
            If ReadCfgValue("HasNeonLightLeft", VehicleCfgFile) = "True" Then _Vehicle.SetNeonLightsOn(VehicleNeonLight.Left, True)
            If ReadCfgValue("HasNeonLightRight", VehicleCfgFile) = "True" Then _Vehicle.SetNeonLightsOn(VehicleNeonLight.Right, True)
            _Vehicle.NeonLightsColor = Color.FromArgb(ReadCfgValue("NeonColorRed", VehicleCfgFile), ReadCfgValue("NeonColorGreen", VehicleCfgFile), ReadCfgValue("NeonColorBlue", VehicleCfgFile))
            _Vehicle.WheelType = ReadCfgValue("WheelType", VehicleCfgFile)
            _Vehicle.Livery = ReadCfgValue("Livery", VehicleCfgFile)
            Native.Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, _Vehicle, CInt(ReadCfgValue("PlateType", VehicleCfgFile)))
            _Vehicle.NumberPlate = ReadCfgValue("PlateNumber", VehicleCfgFile)
            _Vehicle.WindowTint = ReadCfgValue("WindowTint", VehicleCfgFile)
            _Vehicle.SetMod(VehicleMod.Spoilers, ReadCfgValue("Spoiler", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.FrontBumper, ReadCfgValue("FrontBumper", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.RearBumper, ReadCfgValue("RearBumper", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.SideSkirt, ReadCfgValue("SideSkirt", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.Frame, ReadCfgValue("Frame", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.Grille, ReadCfgValue("Grille", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.Hood, ReadCfgValue("Hood", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.Fender, ReadCfgValue("Fender", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.RightFender, ReadCfgValue("RightFender", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.Roof, ReadCfgValue("Roof", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.Exhaust, ReadCfgValue("Exhaust", VehicleCfgFile), True)
            If ReadCfgValue("FrontTireVariation", VehicleCfgFile) = "True" Then _Vehicle.SetMod(VehicleMod.FrontWheels, ReadCfgValue("FrontWheels", VehicleCfgFile), True) Else _Vehicle.SetMod(VehicleMod.FrontWheels, ReadCfgValue("FrontWheels", VehicleCfgFile), False)
            If ReadCfgValue("BackTireVariation", VehicleCfgFile) = "True" Then _Vehicle.SetMod(VehicleMod.BackWheels, ReadCfgValue("BackWheels", VehicleCfgFile), True) Else _Vehicle.SetMod(VehicleMod.BackWheels, ReadCfgValue("BackWheels", VehicleCfgFile), False)
            _Vehicle.SetMod(VehicleMod.Suspension, ReadCfgValue("Suspension", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.Engine, ReadCfgValue("Engine", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.Brakes, ReadCfgValue("Brakes", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.Transmission, ReadCfgValue("Transmission", VehicleCfgFile), True)
            _Vehicle.SetMod(VehicleMod.Armor, ReadCfgValue("Armor", VehicleCfgFile), True)
            _Vehicle.SetMod(25, ReadCfgValue("TwentyFive", VehicleCfgFile), True)
            _Vehicle.SetMod(26, ReadCfgValue("TwentySix", VehicleCfgFile), True)
            _Vehicle.SetMod(27, ReadCfgValue("TwentySeven", VehicleCfgFile), True)
            _Vehicle.SetMod(28, ReadCfgValue("TwentyEight", VehicleCfgFile), True)
            _Vehicle.SetMod(29, ReadCfgValue("TwentyNine", VehicleCfgFile), True)
            _Vehicle.SetMod(30, ReadCfgValue("ThirtyZero", VehicleCfgFile), True)
            _Vehicle.SetMod(31, ReadCfgValue("ThirtyOne", VehicleCfgFile), True)
            _Vehicle.SetMod(32, ReadCfgValue("ThirtyTwo", VehicleCfgFile), True)
            _Vehicle.SetMod(33, ReadCfgValue("ThirtyThree", VehicleCfgFile), True)
            _Vehicle.SetMod(34, ReadCfgValue("ThirtyFour", VehicleCfgFile), True)
            _Vehicle.SetMod(35, ReadCfgValue("ThirtyFive", VehicleCfgFile), True)
            _Vehicle.SetMod(36, ReadCfgValue("ThirtySix", VehicleCfgFile), True)
            _Vehicle.SetMod(37, ReadCfgValue("ThirtySeven", VehicleCfgFile), True)
            _Vehicle.SetMod(38, ReadCfgValue("ThirtyEight", VehicleCfgFile), True)
            _Vehicle.SetMod(39, ReadCfgValue("ThirtyNine", VehicleCfgFile), True)
            _Vehicle.SetMod(40, ReadCfgValue("ForthyZero", VehicleCfgFile), True)
            _Vehicle.SetMod(41, ReadCfgValue("ForthyOne", VehicleCfgFile), True)
            _Vehicle.SetMod(42, ReadCfgValue("ForthyTwo", VehicleCfgFile), True)
            _Vehicle.SetMod(43, ReadCfgValue("ForthyThree", VehicleCfgFile), True)
            _Vehicle.SetMod(44, ReadCfgValue("ForthyFour", VehicleCfgFile), True)
            _Vehicle.SetMod(45, ReadCfgValue("ForthyFive", VehicleCfgFile), True)
            _Vehicle.SetMod(46, ReadCfgValue("ForthySix", VehicleCfgFile), True)
            _Vehicle.SetMod(47, ReadCfgValue("ForthySeven", VehicleCfgFile), True)
            _Vehicle.SetMod(48, ReadCfgValue("ForthyEight", VehicleCfgFile), True)
            If ReadCfgValue("XenonHeadlights", VehicleCfgFile) = "True" Then _Vehicle.ToggleMod(VehicleToggleMod.XenonHeadlights, True)
            If ReadCfgValue("Turbo", VehicleCfgFile) = "True" Then _Vehicle.ToggleMod(VehicleToggleMod.Turbo, True)
            _Vehicle.ToggleMod(VehicleToggleMod.TireSmoke, True)
            _Vehicle.TireSmokeColor = Color.FromArgb(ReadCfgValue("TyreSmokeColorRed", VehicleCfgFile), ReadCfgValue("TyreSmokeColorGreen", VehicleCfgFile), ReadCfgValue("TyreSmokeColorBlue", VehicleCfgFile))
            _Vehicle.SetMod(VehicleMod.Horns, ReadCfgValue("Horn", VehicleCfgFile), True)
            If ReadCfgValue("BulletproofTyres", VehicleCfgFile) = "False" Then Native.Function.Call(Hash.SET_VEHICLE_TYRES_CAN_BURST, _Vehicle, False)
            _Vehicle.RoofState = CInt(ReadCfgValue("VehicleRoof", VehicleCfgFile))
            If ReadCfgValue("ExtraOne", VehicleCfgFile) = "True" Then Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 1, 0) Else Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 1, -1)
            If ReadCfgValue("ExtraTwo", VehicleCfgFile) = "True" Then Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 2, 0) Else Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 2, -1)
            If ReadCfgValue("ExtraThree", VehicleCfgFile) = "True" Then Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 3, 0) Else Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 3, -1)
            If ReadCfgValue("ExtraFour", VehicleCfgFile) = "True" Then Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 4, 0) Else Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 4, -1)
            If ReadCfgValue("ExtraFive", VehicleCfgFile) = "True" Then Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 5, 0) Else Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 5, -1)
            If ReadCfgValue("ExtraSix", VehicleCfgFile) = "True" Then Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 6, 0) Else Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 6, -1)
            If ReadCfgValue("ExtraSeven", VehicleCfgFile) = "True" Then Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 7, 0) Else Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 7, -1)
            If ReadCfgValue("ExtraEight", VehicleCfgFile) = "True" Then Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 8, 0) Else Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 8, -1)
            If ReadCfgValue("ExtraNine", VehicleCfgFile) = "True" Then Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 9, 0) Else Native.Function.Call(Hash.SET_VEHICLE_EXTRA, _Vehicle, 9, -1)
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Function GetRankIndex(value As Integer) As Integer
        Dim result As Integer = 0
        Select Case value
                Case 0 To 89
                    result = 0
                Case 90 To 199
                    result = 1
                Case 200 To 349
                    result = 2
                Case 350 To 499
                    result = 3
                Case 500 To 739
                    result = 4
                Case 740 To 999
                    result = 5
                Case 1000 To 1599
                    result = 6
                Case 1600 To 1999
                    result = 7
                Case 2000 To 2699
                    result = 8
                Case 2700 To 3499
                    result = 9
                Case 3500 To 4199
                    result = 10
                Case 4200 To 5399
                    result = 11
                Case 5400 To 6599
                    result = 12
                Case 6600 To 7999
                    result = 13
                Case 8000 To 9299
                    result = 14
                Case 9300 To 10899
                    result = 15
                Case 10900 To 12199
                    result = 16
                Case 12200 To 13499
                    result = 17
                Case 13500 To 13899
                    result = 18
                Case 13900 To 14299
                    result = 19
                Case 14300 To 14799
                    result = 20
                Case 14800 To 15299
                    result = 21
                Case 15300 To 15799
                    result = 22
                Case 15800 To 16399
                    result = 23
                Case 16400 To 16999
                    result = 24
                Case 17000 To 18399
                    result = 25
                Case 18400 To 18999
                    result = 26
                Case 19000 To 24299
                    result = 27
                Case 24300 To 24999
                    result = 28
                Case 25000 To 29999
                    result = 29
                Case 30000 To 39999
                    result = 30
                Case 40000 To 49999
                    result = 31
                Case 50000 To 59999
                    result = 32
                Case 60000 To 69999
                    result = 33
                Case 70000 To 79999
                    result = 34
                Case 80000 To 89999
                    result = 35
                Case 90000 To 99999
                    result = 36
                Case 100000 To 109999
                    result = 37
                Case 110000 To 119999
                    result = 38
                Case 120000 To 129999
                    result = 39
                Case 130000 To 139999
                    result = 40
                Case 140000 To 149999
                    result = 41
                Case 150000 To 159999
                    result = 42
                Case 160000 To 169999
                    result = 43
                Case 170000 To 179999
                    result = 44
                Case 180000 To 189999
                    result = 45
                Case 190000 To 199999
                    result = 46
                Case 200000 To 219999
                    result = 47
                Case 220000 To 229999
                    result = 48
                Case 230000 To 239999
                    result = 49
                Case 240000 To 249999
                    result = 50
                Case 250000 To 259999
                    result = 51
                Case 260000 To 269999
                    result = 52
                Case 270000 To 279999
                    result = 53
                Case 280000 To 289999
                    result = 54
                Case 290000 To 299999
                    result = 55
                Case 300000 To 309999
                    result = 56
                Case 310000 To 319999
                    result = 57
                Case 320000 To 329999
                    result = 58
                Case 330000 To 339999
                    result = 59
                Case 340000 To 349999
                    result = 60
                Case 350000 To 359999
                    result = 61
                Case 360000 To 369999
                    result = 62
                Case 370000 To 379999
                    result = 63
                Case 380000 To 389999
                    result = 64
                Case 390000 To 399999
                    result = 65
                Case 400000 To 449999
                    result = 66
                Case 450000 To 499999
                    result = 67
                Case 500000 To 549999
                    result = 68
                Case 550000 To 599999
                    result = 69
                Case 600000 To 649999
                    result = 70
                Case 650000 To 699999
                    result = 71
                Case 700000 To 749999
                    result = 72
                Case 750000 To 799999
                    result = 73
                Case 800000 To 849999
                    result = 74
                Case 850000 To 899999
                    result = 75
                Case 900000 To 949999
                    result = 76
                Case 950000 To 999999
                    result = 77
                Case 1000000 To 1499999
                    result = 78
                Case 1500000 To 1999999
                    result = 79
                Case 2000000 To 2499999
                    result = 80
                Case 2500000 To 2999999
                    result = 81
                Case 3000000 To 3499999
                    result = 82
                Case 3500000 To 3999999
                    result = 83
                Case 4000000 To 4499999
                    result = 84
                Case 4500000 To 4999999
                    result = 85
                Case 5000000 To 5499999
                    result = 86
                Case 5500000 To 5999999
                    result = 87
                Case 6000000 To 6499999
                    result = 88
                Case 6500000 To 6999999
                    result = 89
                Case 7000000 To 7499999
                    result = 90
                Case 7500000 To 7999999
                    result = 91
                Case 8000000 To 8499999
                    result = 92
                Case 8500000 To 8999999
                    result = 93
                Case 9000000 To 9499999
                    result = 94
                Case 9500000 To 9999999
                    result = 95
                Case 10000000 To 10499999
                    result = 96
                Case 10500000 To 10999999
                    result = 97
                Case 11000000 To 11499999
                    result = 98
                Case 11500000 To 11999999
                    result = 99
                Case 12000000 To 12499999
                    result = 100
                Case 12500000 To 12999999
                    result = 101
                Case 13000000 To 13999999
                    result = 102
                Case 14000000 To 14999999
                    result = 103
                Case 15000000 To 15999999
                    result = 104
                Case 16000000 To 16999999
                    result = 106
                Case 17000000 To 17999999
                    result = 107
                Case 18000000 To 18999999
                    result = 108
                Case 19000000 To 19999999
                    result = 109
                Case 20000000 To 20999999
                    result = 110
                Case 21000000 To 21999999
                    result = 111
                Case 22000000 To 22999999
                    result = 112
                Case 23000000 To 23999999
                    result = 113
                Case 24000000 To 24999999
                    result = 114
                Case 25000000 To 25999999
                    result = 115
                Case 26000000 To 26999999
                    result = 116
                Case 27000000 To 27999999
                    result = 117
                Case 28000000 To 28999999
                    result = 118
                Case 29000000 To 29999999
                    result = 119
                Case Else
                    result = 120
            End Select
            Return result
    End Function

    Public Shared Sub DrawTexture()
        UI.DrawTexture(DSMenu.mainDir & "credits.png", 0, 0, 5000, New Point(0, 0), New Size(UI.WIDTH, UI.HEIGHT), 0.0, Color.White)
    End Sub

    Public Shared Function Md5Sum(strToEncrypt As String) As String
        Dim ue As New System.Text.UTF8Encoding()
        Dim bytes As Byte() = ue.GetBytes(strToEncrypt)
        Dim md5 As New MD5CryptoServiceProvider()
        Dim hashBytes As Byte() = md5.ComputeHash(bytes)
        Dim hashString As String = ""
        For i As Integer = 0 To hashBytes.Length - 1
            hashString += Convert.ToString(hashBytes(i), 16).PadLeft(2, "0"c)
        Next
        Return hashString.PadLeft(32, "0"c)
    End Function

    Public Shared Sub AddScore(name As String, score As Integer)
        Try
            Dim hash As String = Md5Sum((name & score) & PrivateKey)
            Dim client As WebClient = New WebClient()
            client.DownloadString(Convert.ToString(AddScoreURL + "name=" & name & "&score=" & score & "&hash=") & hash)
        Catch ex As Exception
            UI.Notify(ex.Message)
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Public Shared Function EncryptData(Message As String) As String
        Dim Results As Byte()
        Dim UTF8 As New System.Text.UTF8Encoding()
        Dim HashProvider As New MD5CryptoServiceProvider()
        Dim TDESKey As Byte() = HashProvider.ComputeHash(UTF8.GetBytes(passphrase))
        Dim TDESAlgorithm As New TripleDESCryptoServiceProvider()
        TDESAlgorithm.Key = TDESKey
        TDESAlgorithm.Mode = CipherMode.ECB
        TDESAlgorithm.Padding = PaddingMode.PKCS7
        Dim DataToEncrypt As Byte() = UTF8.GetBytes(Message)
        Try
            Dim Encryptor As ICryptoTransform = TDESAlgorithm.CreateEncryptor()
            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length)
        Finally
            TDESAlgorithm.Clear()
            HashProvider.Clear()
        End Try
        Return Convert.ToBase64String(Results)
    End Function

    Public Shared Function DecryptString(Message As String) As String
        Dim Results As Byte()
        Dim UTF8 As New System.Text.UTF8Encoding()
        Dim HashProvider As New MD5CryptoServiceProvider()
        Dim TDESKey As Byte() = HashProvider.ComputeHash(UTF8.GetBytes(passphrase))
        Dim TDESAlgorithm As New TripleDESCryptoServiceProvider()
        TDESAlgorithm.Key = TDESKey
        TDESAlgorithm.Mode = CipherMode.ECB
        TDESAlgorithm.Padding = PaddingMode.PKCS7
        Dim DataToDecrypt As Byte() = Convert.FromBase64String(Message)
        Try
            Dim Decryptor As ICryptoTransform = TDESAlgorithm.CreateDecryptor()
            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length)
        Finally
            TDESAlgorithm.Clear()
            HashProvider.Clear()
        End Try
        Return UTF8.GetString(Results)
    End Function

    Public Shared Sub CreateVehicle(VehHash As Integer, xyz As Vector3, head As Single)
        Try
            If DSMenu.PlayerVehicle = Nothing Then
                Dim model = New Model(VehHash)
                model.Request(250)
                If model.IsInCdImage AndAlso model.IsValid Then
                    While Not model.IsLoaded
                        Script.Wait(50)
                    End While
                    DSMenu.PlayerVehicle = World.CreateVehicle(model, xyz, head)
                End If
                model.MarkAsNoLongerNeeded()
            Else
                DSMenu.PlayerVehicle.Delete()
                Dim model = New Model(VehHash)
                model.Request(250)
                If model.IsInCdImage AndAlso model.IsValid Then
                    While Not model.IsLoaded
                        Script.Wait(50)
                    End While
                    DSMenu.PlayerVehicle = World.CreateVehicle(model, xyz, head)
                End If
                model.MarkAsNoLongerNeeded()
            End If
        Catch ex As Exception
            logger.Log(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

End Class
