using Godot;
using System;
using static Item;

public partial class GroundItem : Node3D
{
	[Export]
	public string Name = "Scientist_Card";
	
	[Export]
	public string SysName = "uchenicard";
	
	[Export]
	public int Amount = 1;
	
	[Export]
	public int MaxAmount = -1;
	
	[Export]
	public string ResPath = "res://Models/MyModels/kartochka_ucheni.obj";
	
	public Item heldItem;
	
	public override void _Ready() {
		if (heldItem == null) {
			if (MaxAmount == -1) {
				heldItem = new Item(Name, SysName, Amount, ResPath);
			} else {
				heldItem = new Item(Name, SysName, MaxAmount, Amount, ResPath);
			}
		} else {
			Name = heldItem.Name;
			SysName = heldItem.SysName;
			MaxAmount = heldItem.MaxDurability;
			Amount = heldItem.durability_;
			ResPath = heldItem.MeshPath;
		}
		MeshInstance3D itm = GetNode<MeshInstance3D>("Item");
		itm.Mesh = GD.Load<Mesh>(ResPath);
	}
	
	public void update_self() {
		if (heldItem == null) {
			if (MaxAmount == -1) {
				heldItem = new Item(Name, SysName, Amount, ResPath);
			} else {
				heldItem = new Item(Name, SysName, MaxAmount, Amount, ResPath);
			}
		} else {
			Name = heldItem.Name;
			SysName = heldItem.SysName;
			MaxAmount = heldItem.MaxDurability;
			Amount = heldItem.durability_;
			ResPath = heldItem.MeshPath;
		}
		MeshInstance3D itm = GetNode<MeshInstance3D>("Item");
		itm.Mesh = GD.Load<Mesh>(ResPath);
	}
}
