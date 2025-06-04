using Godot;
using System;
using static Player;

public partial  class SCP173 : Enemy
{
	private RayCast3D raycast_;
	private AudioStreamPlayer3D sound_;
	
	private int hold = 0;
	
	public override void _Ready() {
		raycast_ = GetNode<RayCast3D>("RayCast3D");
		sound_ = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
		_on_move = PechenkaZvuk;
		_not_move = PechenkaAntiZvuk;
		
		void PechenkaZvuk() {
			if (!sound_.Playing) {
				sound_.Stream = GD.Load<AudioStreamMP3>("res://Sounds/scp173_move.mp3");
				sound_.Play(1.0f);
			}
		}
		
		void PechenkaAntiZvuk() {
			sound_.Stop();
		}
	}
	
	public override void _Process(double delta)
	{
		if (IsVisibleToCamera()) {
			this.Speed = 0;
			this.canRotate = false;
			hold = 0;
		} else {
			if (hold > 15) {
				this.Speed = 14;
				this.canRotate = true;
			} else {
				hold++;
			}
		}
	}
	
	private bool IsVisibleToCamera()
	{
		var players = GetTree().GetNodesInGroup("players");
		Player player = (Player)players[0];
		bool canSee = false;
		raycast_.LookAt(new Vector3(player.Position.X, this.Position.Y + 1.5f, player.Position.Z));
		try {
			Player obj = (Player)raycast_.GetCollider();
			canSee = true;
		} catch {
			canSee = false;
		}
		Camera3D camera = player.Camera;
		if (camera != null)
		{
			return camera.IsPositionInFrustum(this.GlobalPosition) && canSee;
		}
		return false;
	}	
}
