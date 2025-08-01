using Godot;
using System;


public partial class Obstacle : Node3D
{
    [Export]
    Node3D ObstacleObject;

    [Export]
    bool EnableSpin;
    [Export]
    bool ReverseSpinSpin;
    [Export]
    Vector3 SpinSpeed = new Vector3(0, 50, 0);

    [Export]
    bool EnableBounce;
    [Export]
    Vector3 BounceSpeed = new Vector3(0, 50, 0);
    [Export]
    float MaxDistanceFromOrigin= 100.0f;
    bool BounceSwap = true;

     Vector3 Origin;

    [Export]
    bool EnablePush;

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
        if (EnableBounce)
        {
            Bounce(delta);
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
    public void Bounce(double delta)
    {
        if (Math.Abs(Origin.DistanceTo(ObstacleObject.Position))>MaxDistanceFromOrigin)
        {
            BounceSwap=!BounceSwap;
        }

        if (BounceSwap)
        {
            ObstacleObject.Position += BounceSpeed * (float)delta ;
        }
        else
        {
            ObstacleObject.Position += BounceSpeed * (float)delta * -1;

        }
         
    }






}



