Imports GTA
Imports GTA.Math
Imports GTA.Native
Imports System.Windows.Forms

Public Class InitialDIntensifies
    Inherits Script
    Private ExtraNoGripi As Integer = 50
    Private inter As Integer = 10
    Private imilisec As Integer
    Private Tractionhack As Boolean = False
    Private hackstatus As Boolean = False
    Private PlayerOne As Player
    Private PlayerPed As Ped

    Public Sub New()
        PlayerOne = Game.Player
        PlayerPed = Game.Player.Character

        AddHandler Tick, AddressOf OnTick
    End Sub

    Private Sub OnTick(sender As Object, e As EventArgs)
        PlayerOne = Game.Player
        PlayerPed = Game.Player.Character

        If Tractionhack AndAlso CanWeUse(PlayerOne.LastVehicle) AndAlso CanWeUse((PlayerPed.CurrentVehicle)) AndAlso Not PlayerOne.LastVehicle.IsInAir Then
            If Game.IsControlPressed(0, GTA.Control.VehicleAccelerate) AndAlso (PlayerOne.LastVehicle.Speed < 10.0F OrElse System.Math.Abs(Native.Function.Call(Of Vector3)(Hash.GET_ENTITY_SPEED_VECTOR, PlayerOne.LastVehicle, True).X) > 2.0F) Then
                Native.Function.Call(Hash.APPLY_FORCE_TO_ENTITY, PlayerOne.LastVehicle, 3, 0F, 0.2F, 0F,
                    0F, 0F, 0F, 0, True, True,
                    True, True, True)
            End If
            If Game.IsControlPressed(0, GTA.Control.VehicleBrake) AndAlso (PlayerOne.LastVehicle.Speed > 2.0F) Then
                Native.Function.Call(Hash.APPLY_FORCE_TO_ENTITY, PlayerOne.LastVehicle, 3, 0F, -0.2F, 0F,
                    0F, 0F, 0F, 0, True, True,
                    True, True, True)
            End If
            If Game.IsControlPressed(0, GTA.Control.VehicleHandbrake) AndAlso Not Game.IsControlPressed(2, GTA.Control.VehicleAccelerate) Then
                Dim veh As Vector3 = Native.Function.Call(Of Vector3)(Hash.GET_ENTITY_SPEED_VECTOR, PlayerOne.LastVehicle, True) * -0.05F
                Native.Function.Call(Hash.APPLY_FORCE_TO_ENTITY, PlayerOne.LastVehicle, 3, veh.X, veh.Y / 4, 0F,
                    0F, 0F, 0F, 0, True, True,
                    True, True, True)
            End If
            If PlayerOne.LastVehicle.Speed > 5.0F AndAlso Native.Function.Call(Of Vector3)(Hash.GET_ENTITY_SPEED_VECTOR, PlayerOne.LastVehicle, True).Y > 0F Then
                If Game.IsControlPressed(2, GTA.Control.VehicleMoveLeftOnly) AndAlso Native.Function.Call(Of Vector3)(Hash.GET_ENTITY_ROTATION_VELOCITY, PlayerOne.LastVehicle).Z < 0.8F Then
                    Native.Function.Call(Hash.APPLY_FORCE_TO_ENTITY, PlayerOne.LastVehicle, 3, -0.2F, 0F, 0F,
                        0F, 2.0F, -0.2F, 0, True, True,
                        True, True, True)
                End If
                If Game.IsControlPressed(0, GTA.Control.VehicleMoveRightOnly) AndAlso Native.Function.Call(Of Vector3)(Hash.GET_ENTITY_ROTATION_VELOCITY, PlayerOne.LastVehicle).Z > -0.8F Then
                    Native.Function.Call(Hash.APPLY_FORCE_TO_ENTITY, PlayerOne.LastVehicle, 3, 0.2F, 0F, 0F,
                        0F, 2.0F, -0.2F, 0, True, True,
                        True, True, True)
                End If
            End If
        End If

        If Game.GameTime > imilisec + inter Then
            imilisec = Game.GameTime
            If Tractionhack AndAlso (CanWeUse(PlayerOne.LastVehicle) AndAlso (Native.Function.Call(Of Boolean)(Hash.IS_THIS_MODEL_A_CAR, PlayerOne.LastVehicle.Model)) OrElse Native.Function.Call(Of Boolean)(Hash.IS_THIS_MODEL_A_QUADBIKE, PlayerOne.LastVehicle.Model)) Then
                If hackstatus Then
                    Native.Function.Call(Hash.SET_VEHICLE_REDUCE_GRIP, PlayerOne.LastVehicle, False)
                Else
                    imilisec = imilisec + ExtraNoGripi
                    Native.Function.Call(Hash.SET_VEHICLE_REDUCE_GRIP, PlayerOne.LastVehicle, True)
                End If
                hackstatus = Not hackstatus
            End If
        End If

        If Game.Player.Character.IsInVehicle Then
            If CanWeUse(PlayerOne.LastVehicle) AndAlso CanWeUse((Game.Player.Character.CurrentVehicle)) Then
                Tractionhack = Not Tractionhack
                Native.Function.Call(Hash.SET_VEHICLE_REDUCE_GRIP, PlayerOne.LastVehicle, False)
            End If
        End If
    End Sub

    Private Function CanWeUse(entity As Entity) As Boolean
        Return entity IsNot Nothing AndAlso entity.Exists()
    End Function
End Class
