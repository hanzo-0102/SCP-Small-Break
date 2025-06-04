using Godot;
using System;

public class Damage
{
	private string type;
	private int amount;
	
	public int Amount => amount;
	public string Type => type;
	
	public Damage(string t, int a) {
		type = t;
		amount = a;
	}
	
	public override string ToString() {
		return amount + " damage of type " + type;
	}
	
	public static int operator -(int a, Damage b) {
		if (a - b.Amount <= 0) {
			return -1;
		} else {
			return a - b.Amount;
		}
	}
}
