using Godot;
using System;
using static Player;

public partial  class SimpleEnemyAI : Enemy
{
	[Export]
	private string name_;
	
	public override void _Ready()
	{
		name_ = "";
		Random rnd = new Random();
		string letters = "qwertyuiopasdfghjklzxcvbnm";
		for (int i = 0; i != rnd.Next(1, 10); i++) {
			name_ += letters[rnd.Next(0, letters.Length)];
		}
	}
}
