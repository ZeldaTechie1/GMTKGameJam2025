using Godot;
using System;

public partial class CameraFollow : Camera3D
{
	[Export] Node3D followLocation;
	[Export] float followSpeed;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = Position.Lerp(followLocation.Position,(float)delta*followSpeed);
	}
}
