using Godot;
using System;
using static Player;

public partial class Enemy : CharacterBody3D
{
	[Export]
	public int Speed { get; set; } = 14;
	[Export]
	public bool canRotate = true;
	[Export]
	public int FallAcceleration { get; set; } = 75;
	
	public bool Jumped = false;
	[Export]
	protected bool CanJump = true;
	
	protected delegate void Event();
	protected Event _on_move;
	protected Event _not_move;

	protected Vector3 _targetVelocity = Vector3.Zero;
	protected Vector3 _velocity;

	public override void _PhysicsProcess(double delta)
	{
		var players = GetTree().GetNodesInGroup("players");
		CharacterBody3D player = (CharacterBody3D)players[0];
		
		if (canRotate) {
			this.LookAt(new Vector3(player.Position.X, this.Position.Y, player.Position.Z));
		}
		Vector3 direction = GlobalPosition.DirectionTo(player.GlobalPosition);

		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;
		if (direction.Y != 0 && !Jumped && CanJump) 
		{
			_targetVelocity.Y = direction.Y * Speed;
			Jumped = true;
		}

		if (!IsOnFloor())
		{
			_targetVelocity.Y -= FallAcceleration * (float)delta;
		}
		else
		{
			Jumped = false;
		}
		if (_targetVelocity.X + _targetVelocity.Z > 0.1) {
			_on_move();
		} else {
			_not_move();
		}
		Velocity = _targetVelocity;
		MoveAndSlide();
	}
}
