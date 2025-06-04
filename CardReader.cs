using Godot;
using System;

public partial class CardReader : Node3D
{
	[Export]
	private int level_ = 0;
	
	public int Level => level_;
	
	private MeshInstance3D model_;
	
	public bool active = false;
	
	private int activeFor = 0;
	
	public override void _Ready() {
		model_ = GetNode<Area3D>("Area3D").GetNode<MeshInstance3D>("Cardtaker");
		switch (level_){
			case 0:
				model_.Mesh = GD.Load<Mesh>("res://Models/MyModels/cardtaker0.obj");
				break;
			case 1:
				model_.Mesh = GD.Load<Mesh>("res://Models/MyModels/cardtaker1.obj");
				break;
			case 5:
				model_.Mesh = GD.Load<Mesh>("res://Models/MyModels/cardtaker5.obj");
				break;
			default:
				break;
		}
	}
	
	public override void _Process(double delta) {
		if (active) {
			activeFor++;
			if (activeFor > 150) {
				active = false;
				activeFor = 0;
				GD.Print("Stop");
			}
		}
	}
}
