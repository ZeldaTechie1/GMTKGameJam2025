using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class CameraRotate : Node3D
{
    [Export] LevelManager levelManager;
	[Export] float mouse_sensitivity = .005f;
    [Export(PropertyHint.Range, "-90,0,0.1")] float min_vertical_angle = -Mathf.Pi / 2;
    [Export(PropertyHint.Range, "0,90,0.1")] float max_vertical_angle = Mathf.Pi / 4;
    bool inPlay;

    public override void _Ready()
    {
        levelManager.PlayStarted += () =>
        {
            inPlay = true;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        };
        levelManager.PlayEnded += () =>
        {
            inPlay = false;
            Input.MouseMode = Input.MouseModeEnum.Visible;
        };
        
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!inPlay)
            return;

        base._UnhandledInput(@event);

		if (@event is InputEventMouseMotion motion)
		{
            //rotate x component on up axis, rotate y component on model left axis
			Rotation -= ((Vector3.Up * (motion.Relative.X * mouse_sensitivity)) + (Vector3.ModelLeft * (motion.Relative.Y * mouse_sensitivity)));
            //clamps x rotation to -90° and 45°, also wraps y rotation so that we always have values from 0°s - 360°
            Vector3 clampedRotation = new Vector3(Mathf.Clamp(Rotation.X,Mathf.DegToRad(min_vertical_angle),Mathf.DegToRad(max_vertical_angle)),
                                                  Mathf.Wrap(Rotation.Y,0.0f,Mathf.Tau),
                                                  Rotation.Z);
            Rotation = clampedRotation;
		}
    }
}
