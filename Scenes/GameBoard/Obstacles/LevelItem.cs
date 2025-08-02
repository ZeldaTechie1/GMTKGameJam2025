using Godot;
using System;


public partial class LevelItem : Node3D
{
    [ExportGroup("Basic Item Settings")]
    [Export]
    Node3D LevelItemObject;
    [Export]
    LevelItemSize ItemSize = LevelItemSize.Small;

    [Export]
    LevelItemType ItemType=LevelItemType.Aid;
    
    [Export]
    bool ItemActive;

    [ExportGroup("Spin")]
    [Export]
    bool EnableSpin;
    [Export]
    bool ReverseSpinSpin;
    [Export]
    Vector3 SpinSpeed = new Vector3(0, 50, 0);

    [ExportGroup("Vertical Movement")]
    [Export]
    bool EnableVerticalMove;
    [Export]
    float VerticalMoveSpeed = 50;
    [Export]
    float MaxVerticalDistanceFromOrigin = 100.0f;
    bool VerticalMoveSwap = true;


    [ExportGroup("Horizontal Movement")]
    [Export]
    bool EnableHorizontalMovement;
    [Export]
    float HorizontalMoveSpeed = 50;
    [Export]
    float MaxHorizontalDistanceFromOrigin = 100.0f;

    [ExportSubgroup("Movment Axis")]
    [Export]
    bool X_Axis;
    [Export]
    bool Z_Axis;


    bool HorizontalMoveSwap = true;



    Vector3 Origin;
    public override void _Ready()
    {
        if (LevelItemObject != null)
        {
            Origin = LevelItemObject.Position;
        }

    }

    public override void _Process(double delta)
    {
        if (EnableSpin)
        {
            Spin(delta, ReverseSpinSpin);

        }
        if (EnableVerticalMove)
        {
            VerticalMove(delta);
        }
        if (EnableHorizontalMovement)
        {
            HorizontalMove(delta);
        }

    }

    public void Spin(double delta, bool reverseSpin = false)
    {
        if (reverseSpin)
        {
            LevelItemObject.RotationDegrees += SpinSpeed * (float)delta * -1;
        }
        else
        {
            LevelItemObject.RotationDegrees += SpinSpeed * (float)delta;
        }

    }
    public void VerticalMove(double delta)
    {
        Vector3 MoveSpeed = new Vector3(0, VerticalMoveSpeed, 0);

        if (Math.Abs(Origin.DistanceTo(LevelItemObject.Position)) > MaxVerticalDistanceFromOrigin)
        {
            VerticalMoveSwap = !VerticalMoveSwap;
        }

        if (VerticalMoveSwap)
        {
            LevelItemObject.Position += MoveSpeed * (float)delta;
        }
        else
        {
            LevelItemObject.Position += MoveSpeed * (float)delta * -1;

        }

    }
    public void HorizontalMove(double delta)
    {

        if (X_Axis == false && Z_Axis == false)
        {
            X_Axis = true;
        }


        Vector3 MoveSpeed = new Vector3(0, 0, 0);

        if (X_Axis)
        {
            MoveSpeed.X = HorizontalMoveSpeed;
        }
        if (Z_Axis)
        {
            MoveSpeed.Z = HorizontalMoveSpeed;
        }


        if (Math.Abs(Origin.DistanceTo(LevelItemObject.Position)) > MaxHorizontalDistanceFromOrigin)
        {
            HorizontalMoveSwap = !HorizontalMoveSwap;
        }

        if (HorizontalMoveSwap)
        {
            LevelItemObject.Position += MoveSpeed * (float)delta;
        }
        else
        {
            LevelItemObject.Position += MoveSpeed * (float)delta * -1;

        }

    }






}
public enum LevelItemType
{
    Obstacle,
    Aid,
    Default

}
public enum LevelItemLocation
{
    Floor,
    Floating,
    Default
}
public enum LevelItemSize
{
    Small,
    Large,
    Medium

}


