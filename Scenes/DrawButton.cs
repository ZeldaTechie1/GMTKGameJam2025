using Godot;
using System;

public partial class DrawButton : TextureButton
{
    [Export]
    CardManager cardManager;

   
     
     public override void _Ready()
    {
        ButtonUp += ()=>cardManager.EmitDrawCard();
    }
}
