using Godot;
using System;

public class Item
{
	protected string name_;
	
	public string Name => name_;
	
	protected Mesh mesh_;
	
	public Mesh MyMesh => mesh_;
	
	protected string sys_name_;
	
	public string SysName => sys_name_;
	
	protected string mesh_path_;
	
	public string MeshPath => mesh_path_;
	
	public int durability_;
	
	protected int max_durability_;
	
	public int MaxDurability => max_durability_;
	
	public Item() {
		max_durability_ = 0;
		durability_ = 0;
		sys_name_ = "";
		name_ = "";
		mesh_path_ = null;
		mesh_ = null;
	}
	
	public Item(string name, string sys_name, int durability, string mesh_path) {
		max_durability_ = durability;
		sys_name_ = sys_name;
		durability_ = max_durability_;
		name_ = name;
		mesh_path_ = mesh_path;
		mesh_ = (Mesh)GD.Load(mesh_path);
	}
	
	public Item(string name, string sys_name, int max_durability, int durability, string mesh_path) {
		max_durability_ = max_durability;
		sys_name_ = sys_name;
		durability_ = durability;
		name_ = name;
		mesh_path_ = mesh_path;
		mesh_ = (Mesh)GD.Load(mesh_path);
	}
	
	public bool canAdd(Item b) {
		return (this.Name == "" && this.durability_ == 0) || (this.Name == b.Name && this.durability_ + b.durability_ <= this.MaxDurability);
	}
	
	public static Item operator +(Item a, Item b) {
		if (a.Name == "" && a.durability_ == 0) {
			return b;
		} else if (b.Name == "" && b.durability_ == 0) {
			return a;
		} else if (a.Name == b.Name) {
			return new Item(a.Name, a.SysName, Math.Min(a.durability_ + b.durability_, a.max_durability_), a.mesh_path_);
		} else {
			throw new ArgumentException("Different item types");
		}
	}
	
	public override string ToString() {
		return (this.Name + " " + this.SysName + " " + this.durability_ + " " + this.MaxDurability + " " + this.MeshPath);
	}
}
