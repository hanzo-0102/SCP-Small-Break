using Godot;
using System;
using static Damage;

public interface IBasicHealthSystem
{
	int Health { get; }
	void GetDamage(Damage dmg);
	void GetHeal(int amount);
}
