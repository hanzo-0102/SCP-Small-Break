using Godot;
using System;
using static IBasicHealthSystem;
using static Damage;

public partial class MOG : CharacterBody3D, IBasicHealthSystem
{
	private int health_;
	private Label3D textHealth;
	public int Health => health_;
	private string deathReason;
	
	public override void _Ready() {
		textHealth = GetNode<Label3D>("Label3D");
		deathReason = "";
		health_ = 100;
	}
	
	public override void _Process(double delta) {
		if (deathReason == "") {
			textHealth.Text = Health.ToString() + "/100";
		} else {
			textHealth.Text = deathReason;
		}
	}
	
	public void GetDamage(Damage dmg) {
		health_ -= dmg;
		if (Health == -1) {
			deathReason = dmg.Type;
		}
	}
	
	public void GetHeal(int amount) {
		health_ = Math.Min(Health + amount, 100);
	}
}
