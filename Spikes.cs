using Godot;
using System;

public partial class Spikes : MeshInstance3D
{
    [Export]
    bool Active;
    [Export]
    Vector3 EjectionSpeed = new Vector3(0, 0.05f, 0);
    [Export]
    Vector3 RetractionSpeed = new Vector3(0, 0.05f, 0);

    Vector3 StartingScale;



    bool Retracted = false;

    public override void _Ready()
    {
        StartingScale = this.Scale;

    }

    public override void _Process(double delta)
    {
        if (Active)
        {
            MoveSpikes(delta);
        }

    }

    public void MoveSpikes(double delta)
    {
        if (Retracted)
        {
            Vector3 rescale = this.Scale + EjectionSpeed * (float)delta;
            if (rescale.X > StartingScale.X || rescale.Y > StartingScale.Y || rescale.Z > StartingScale.Z)
            {
                GD.Print("Scale:" + this.Scale + "|Rescale:" + rescale);
                this.Scale = StartingScale;
                Retracted = false;
                GD.Print("Swap");
            }
            else
            {
                this.Scale = rescale;
            }

        }
        else
        {
            Vector3 rescale = this.Scale + EjectionSpeed * (float)delta * -1;
            if (rescale.X < 0 || rescale.Y < 0 || rescale.Z < 0)
            {

                Retracted = true;
                GD.Print("Swap");
            }
            else
            {

                this.Scale = rescale;
            }
        }

    }

}
public enum ObstacleType
{
    Small,
    Large,
    Medium

}
