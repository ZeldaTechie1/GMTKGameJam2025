using Godot;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

public partial class BasicPlayer : CharacterBody3D
{
	[Export] LevelManager levelManager;
	[Export] public float CurrentSpeed = 0.0f;
	[Export] public float Acceleration = 1.0f;
	[Export] public float Deceleration = 0.5f;
	[Export] public float JumpVelocity = 4.5f;

	[Export] Camera3D PlayerCamera;
	[Export] MeshInstance3D Graphics;
	bool wasOnFloor;
	bool appliedModifier;
	ModifierType currentModifier;
	Vector3 LaunchDirection;

	public override void _Ready()
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		if (levelManager.currentState != LevelState.InPlay)
			return;

		Vector3 velocity = GetRealVelocity();

		// Add the gravity.
		if (!IsOnFloorOnly())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");
		//this makes the movement based off the camera look direction idk wtf a basis is but this works
		Basis cameraBasis = PlayerCamera.GlobalTransform.Basis;
		cameraBasis = cameraBasis.Rotated(cameraBasis.X, -cameraBasis.GetEuler().X);//this fixes the issue where the player would slow down the more up/down you look

		Vector3 direction = (cameraBasis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		Vector3 floorNormal = GetFloorNormal();
		if (direction == Vector3.Zero)
		{
			direction = -Graphics.Basis.Z;
		}

		DebugDraw3D.DrawArrowRay(GlobalPosition + (Vector3.ModelTop * 2), floorNormal, 1, Colors.Blue);

		if (IsOnFloor())
		{
			direction = direction.Slide(floorNormal);
		}

		DebugDraw3D.DrawArrowRay(GlobalPosition + (Vector3.ModelTop * 2), direction, 1);
		Vector3 lookAtDirection = direction + Graphics.GlobalPosition;

		if (IsOnFloor())
		{
			Graphics.LookAt(lookAtDirection, floorNormal);
		}

		if (inputDir != Vector2.Zero)
		{
			velocity += (direction * Acceleration * (IsOnFloor() ? 1 : .3f));
		}
		else if (IsOnFloor() && Mathf.RadToDeg(GetFloorAngle()) < 10)
		{
			Vector3 horizontalVelocity = new Vector3(velocity.X, 0, velocity.Z);
			velocity -= (Deceleration * horizontalVelocity.Normalized());

			if (horizontalVelocity.Length() < .2f)
				velocity = new Vector3(0, velocity.Y, 0);
		}

		if (!appliedModifier)
		{
			if (currentModifier == ModifierType.Die)
			{
				levelManager.PlayerDead();
			}
			if (currentModifier == ModifierType.Launch)
			{
				velocity = LaunchDirection;
			}
            if (currentModifier == ModifierType.Bounce)
            {
                velocity += LaunchDirection;
            }
            appliedModifier = true;
            if (currentModifier == ModifierType.Slip)
            {
                velocity = GetRealVelocity();
				appliedModifier = false;
            }
            if (currentModifier == ModifierType.Speed)
            {
                velocity *= 1.1f;
                appliedModifier = false;
            }
            if (currentModifier == ModifierType.Slow)
            {
                velocity /= 1.1f;
                appliedModifier = false;
            }
        }

        
        Velocity = velocity;
		
		CurrentSpeed = velocity.Length();
		DebugDraw3D.DrawArrowRay(GlobalPosition + (Vector3.ModelTop * 2), velocity, velocity.Length() * (float)delta, Colors.Red, is_absolute_size: true);
		MoveAndSlide();
	}

	public void AffectPlayer(ModifierType modifierType, bool isEntering, Vector3 launchDirection)
	{
        GD.Print("Squaw");
		if (isEntering)
		{
			currentModifier = modifierType;
			appliedModifier = false;
			LaunchDirection = launchDirection;
		}
		else
		{
			currentModifier = ModifierType.None;
		}
	}
}
