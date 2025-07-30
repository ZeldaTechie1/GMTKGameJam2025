using Godot;
using System;
using System.Diagnostics;

public partial class BasicPlayer : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	[Export] Camera3D PlayerCamera;
	[Export] MeshInstance3D Graphics;

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloorOnly())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloorOnly())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");
		//this makes the movement based off the camera look direction idk wtf a basis is but this works
		Basis cameraBasis = PlayerCamera.GlobalTransform.Basis;
		cameraBasis = cameraBasis.Rotated(cameraBasis.X, -cameraBasis.GetEuler().X);//this fixes the issue where the player would slow down the more up/down you look

		Vector3 direction = (cameraBasis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		Vector3 lookAtDirection = new Vector3(direction.X, 0, direction.Z) + Graphics.GlobalPosition;
		if (direction.Length() > Mathf.Epsilon)
			Graphics.LookAt(lookAtDirection,Vector3.Up);

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
