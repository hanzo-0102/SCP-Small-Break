using Godot;
using System;
using static Item;

public partial class PlayerGUILive : Control
{
	public Item[] slots = new Item[3];
	
	private Sprite2D Item1, Item2, Item3;
	
	private Label Amount1, Amount2, Amount3;
	
	private Label CurItem;
	
	public int activeItem = 1;
	
	public override void _Ready() {
		Item empty = new Item();
		slots[0] = empty;
		slots[1] = empty;
		slots[2] = empty;
		Item1 = GetNode<PanelContainer>("PanelContainer").GetNode<Sprite2D>("Slot1").GetNode<Sprite2D>("Item1");
		Item2 = GetNode<PanelContainer>("PanelContainer").GetNode<Sprite2D>("Slot2").GetNode<Sprite2D>("Item2");
		Item3 = GetNode<PanelContainer>("PanelContainer").GetNode<Sprite2D>("Slot3").GetNode<Sprite2D>("Item3");
		Amount1 = GetNode<PanelContainer>("PanelContainer").GetNode<Sprite2D>("Slot1").GetNode<Label>("Amount1");
		Amount2 = GetNode<PanelContainer>("PanelContainer").GetNode<Sprite2D>("Slot2").GetNode<Label>("Amount2");
		Amount3 = GetNode<PanelContainer>("PanelContainer").GetNode<Sprite2D>("Slot3").GetNode<Label>("Amount3");
		CurItem = GetNode<Label>("CurItem");
	}
	
	public override void _Process(double delta) {
		CurItem.Text = slots[activeItem - 1].Name;
		Item1.Texture = GD.Load<Texture2D>("res://Textures/GUI/Items/" + slots[0].SysName + ".png");
		Item2.Texture = GD.Load<Texture2D>("res://Textures/GUI/Items/" + slots[1].SysName + ".png");
		Item3.Texture = GD.Load<Texture2D>("res://Textures/GUI/Items/" + slots[2].SysName + ".png");
		if (slots[0].MaxDurability > 1) {
			Amount1.Text = slots[0].durability_.ToString();
		} else {
			Amount1.Text = "";
		}
		if (slots[1].MaxDurability > 1) {
			Amount2.Text = slots[1].durability_.ToString();
		} else {
			Amount2.Text = "";
		}
		if (slots[2].MaxDurability > 1) {
			Amount3.Text = slots[2].durability_.ToString();
		} else {
			Amount3.Text = "";
		}
	}
}
