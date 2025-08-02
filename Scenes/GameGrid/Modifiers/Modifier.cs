using Godot;
using System;

public enum ModifierType
{
    None,
    Speed,
    Slow,
    Slip,
    Launch,
    Bounce,
    Die
}

public partial class Modifier : Area3D
{

    [Export] ModifierType modifierType;
    [Export] float LaunchPower = 1;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExit;
    }

    private void OnBodyEntered(Node3D body)
    {
        if(body is BasicPlayer player)
        {
            player.AffectPlayer(modifierType, true,Basis.Y * LaunchPower);
        }
    }

    private void OnBodyExit(Node3D body)
    {
        GD.Print("exited");
        if (body is BasicPlayer player)
        {
            player.AffectPlayer(modifierType, false, Basis.Y * LaunchPower);
        }
    }
}
