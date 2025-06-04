using Godot;
using System;
using static CardReader;

public class NullReader{
	public bool active;
	
	public NullReader() {
		active = false;
	}
}

public partial class Door : Node3D
{
	[Export]
	public CardReader parent1, parent2;
	
	private float toMove = 3f;
	
	private int wait = 0;
	
	private float startZ;
	
	private RigidBody3D doormesh;
	
	public override void _Ready() {
		doormesh = GetNode<RigidBody3D>("RigidBody3D");
		startZ = doormesh.Position.Z;
	}
	
	private void MoveBack() {
		if (wait == 100) {
			if (doormesh.Position.Z < startZ) {
				doormesh.Position = new Vector3(doormesh.Position.X, doormesh.Position.Y, doormesh.Position.Z + 0.02f);
			}
		} else {
			wait++;
		}
	}
	
	private void MoveForward() {
		if (doormesh.Position.Z > startZ - toMove) {
			doormesh.Position = new Vector3(doormesh.Position.X, doormesh.Position.Y, doormesh.Position.Z - 0.02f);
			wait = 0;
		}
	}
	
	public override void _Process(double delta) {
		if (parent1 != null && parent2 != null) {
			if (parent1.active || parent2.active) {
				MoveForward();
			} else {
				MoveBack();
			}
		} else if (parent1 != null) {
			if (parent1.active) {
				MoveForward();
			} else {
				MoveBack();
			}
		} else if (parent2 != null) {
			if (parent2.active) {
				MoveForward();
			} else {
				MoveBack();
			}
		}
	}
}
