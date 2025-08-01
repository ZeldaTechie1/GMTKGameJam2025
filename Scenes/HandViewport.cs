using Godot;
using System;

public partial class HandViewport : SubViewport
{
    [Export] Camera3D camera;
    public override void _Ready()
    {
        this.camera = camera;
    }
}
