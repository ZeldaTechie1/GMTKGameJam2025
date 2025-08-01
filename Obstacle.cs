using Godot;
using System;


public partial class Obstacle : Node3D
{
    [ExportGroup("Basic Obstacle Settings")]
    [Export]
    Node3D ObstacleObject;
    [Export]
    ObstacleType ObstacleType_Type = ObstacleType.Small;
    [Export]
    bool ObstacleActive;

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
        if (ObstacleObject != null)
        {
            Origin = ObstacleObject.Position;
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
            ObstacleObject.RotationDegrees += SpinSpeed * (float)delta * -1;
        }
        else
        {
            ObstacleObject.RotationDegrees += SpinSpeed * (float)delta;
        }

    }
    public void VerticalMove(double delta)
    {
        Vector3 MoveSpeed = new Vector3(0, VerticalMoveSpeed, 0);

        if (Math.Abs(Origin.DistanceTo(ObstacleObject.Position)) > MaxVerticalDistanceFromOrigin)
        {
            VerticalMoveSwap = !VerticalMoveSwap;
        }

        if (VerticalMoveSwap)
        {
            ObstacleObject.Position += MoveSpeed * (float)delta;
        }
        else
        {
            ObstacleObject.Position += MoveSpeed * (float)delta * -1;

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


        if (Math.Abs(Origin.DistanceTo(ObstacleObject.Position)) > MaxHorizontalDistanceFromOrigin)
        {
            HorizontalMoveSwap = !HorizontalMoveSwap;
        }

        if (HorizontalMoveSwap)
        {
            ObstacleObject.Position += MoveSpeed * (float)delta ;
        }
        else
        {
            ObstacleObject.Position += MoveSpeed * (float)delta * -1;

        }
         
    }






}
public enum ObstacleType
{
    Small,
    Large,
    Medium

}


