using Godot;
using System;
using static CardReader;
using static Item;
using static GroundItem;
using static PlayerGUILive;
using static Requirements;
using static Damage;
using static MOG;

public partial  class Player : CharacterBody3D
{
	[Export]
	public PackedScene ThrowItem = GD.Load<PackedScene>("res://ground_item.tscn");
	
	[Export]
	public PackedScene GUIshka = GD.Load<PackedScene>("res://GUI.tscn");
	
	[Export]
	public int Speed { get; set; } = 14;
	[Export]
	public int FallAcceleration { get; set; } = 75;
	
	private Requirements checkReq = new Requirements();
	
	public bool Jumped = false;
	public float MouseSensitivity = 0.0002f;
	public float MaxAngle = 90f;
	
	private bool _initialized = true;
	public bool Init => _initialized;
	
	private Node scene;
	
	public Item[] inventory = {new Item(), new Item(), new Item()};
	
	private Vector3 _targetVelocity = Vector3.Zero;
	private Vector3 _velocity;
	private Camera3D _camera;
	private RayCast3D _raycast;
	private RayCast3D _shooting;
	public SpotLight3D _flashlight;
	private MeshInstance3D _held_item;
	
	private PlayerGUILive _guishka;
	
	public Camera3D Camera => _camera;
	
	private double wait = 0.0;
	
	public override void _Ready()
	{
		scene = this.GetParent();
		_camera = GetNode<Camera3D>("Camera3D");
		_raycast = _camera.GetNode<RayCast3D>("RayCast3D");
		_shooting = _camera.GetNode<RayCast3D>("ShootingRange");
		_flashlight = _camera.GetNode<SpotLight3D>("SpotLight3D");
		_held_item = _camera.GetNode<MeshInstance3D>("HeldItem");
		_guishka = (PlayerGUILive)_camera.GetNode("Control");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		AddToGroup("players");
	}
	
	public Player(Vector3 pos, Item[] inventory) {
		
		CollisionShape3D collider = new CollisionShape3D();
		collider.Shape = new CapsuleShape3D();
		AddChild(collider);
		
		_camera = new Camera3D();
		_camera.Position = new Vector3(0.0f, 1.135f, 0.0f);
		_camera.Name = "Camera3D";
		AddChild(_camera);
		
		_flashlight = new SpotLight3D();
		_flashlight.SpotRange = 200.0f;
		_flashlight.SpotAngle = 40.0f;
		_flashlight.LightEnergy = 6.0f;
		_flashlight.Name = "SpotLight3D";
		_camera.AddChild(_flashlight);
		
		_raycast = new RayCast3D();
		_raycast.TargetPosition = new Vector3(0.0f, -5.0f, 0.0f);
		_raycast.Rotation = new Vector3(89.54f, 0.0f, 0.0f);
		_raycast.Name = "RayCast3D";
		_raycast.CollideWithAreas = true;
		_raycast.AddException(this);
		_camera.AddChild(_raycast);
		
		/*MeshInstance3D cube = new MeshInstance3D();
		cube.Mesh = new BoxMesh();
		cube.Position = new Vector3(0.0f, -5.0f, 0.0f);
		cube.Scale = new Vector3(.1f, .1f, .1f);
		_raycast.AddChild(cube);*/
		
		PlayerGUILive _guishka = GUIshka.Instantiate<PlayerGUILive>();
		_guishka.Name = "Control";
		_camera.AddChild(_guishka);
		
		_shooting = new RayCast3D();
		_shooting.TargetPosition = new Vector3(0.0f, -30.0f, 0.0f);
		_shooting.Rotation = new Vector3(90.0f, 0.0f, 0.0f);
		_shooting.Name = "ShootingRange";
		_camera.AddChild(_shooting);
		
		MeshInstance3D _held_item = new MeshInstance3D();
		_held_item.Position = new Vector3(0.489f, -0.343f, -0.631f);
		_held_item.Rotation = new Vector3(-34.6f, 98.5f, 43.8f);
		_held_item.Scale = new Vector3(0.1f, 0.1f, 0.1f);
		_held_item.Name = "HeldItem";
		_camera.AddChild(_held_item);
		
		MeshInstance3D model = new MeshInstance3D();
		model.Mesh = new CapsuleMesh();
		model.Scale = new Vector3(0.9f, 1.8f, 0.9f);
		AddChild(model);
		
		this.Position = pos;
		this.inventory = inventory;
		
		Input.MouseMode = Input.MouseModeEnum.Captured;
		AddToGroup("players");
	}
	
	public override void _Process(double delta)
	{
		_guishka.slots = inventory;
		if (inventory[_guishka.activeItem - 1] != new Item()) {
			_held_item.Mesh = inventory[_guishka.activeItem - 1].MyMesh;
		} else {
			_held_item.Mesh = null;
		}
		Vector2 mouseMovement = Input.GetLastMouseVelocity();
		Vector2 center = GetViewport().GetVisibleRect().Size;
		_camera.RotateX((float)-mouseMovement.Y * MouseSensitivity);
		_camera.RotationDegrees = new Vector3(Mathf.Clamp(_camera.RotationDegrees.X, -MaxAngle, MaxAngle), _camera.RotationDegrees.Y, 0);
		this.RotateY((float)-mouseMovement.X * MouseSensitivity);
		GetViewport().WarpMouse(center / 2);
		if (wait > 0.0) {
			wait -= 1.0;
		}
		if (Input.IsActionPressed("throw")) {
			if (inventory[_guishka.activeItem - 1] != new Item()) {
				GroundItem groundItemScene = ThrowItem.Instantiate<GroundItem>();
				
				Item transfering = inventory[_guishka.activeItem - 1];
				groundItemScene.heldItem = new Item(transfering.Name, transfering.SysName, transfering.MaxDurability, transfering.durability_, transfering.MeshPath);
				
				inventory[_guishka.activeItem - 1] = new Item();
				
				scene.AddChild(groundItemScene);
				groundItemScene.Scale = new Vector3(0.1f, 0.1f, 0.1f);
				groundItemScene.Position = new Vector3(this.Position.X, this.Position.Y - 1.574f, this.Position.Z);

			}
		}
		if (Input.IsActionPressed("shoot")) {
			if (inventory[_guishka.activeItem - 1].Name == "Pistolet") {
				bool shot = false;
				for (int i = 0; i != 3; i++) {
					if (!shot && inventory[i].Name == "Pistolet Bullet") {
						if (inventory[i].durability_ == 1) {
							inventory[i] = new Item();
						} else {
							inventory[i].durability_ -= 1;
						}
						shot = true;
						GD.Print("BAM");
						if (_shooting.IsColliding()) {
							try {
								MOG cr = (MOG)_shooting.GetCollider();
								GD.Print("Found MOG");
								Damage dmg = new Damage("gunshot", 33);
								cr.GetDamage(dmg);
							} catch {
							}
						}
					}
				}
			}
		}
		if (Input.IsActionPressed("interraction")) {
			if (_raycast.IsColliding()) {
				try {
					Area3D obj = (Area3D)_raycast.GetCollider();
					CardReader cr = (CardReader)obj.GetParentNode3D();
					if (checkReq.cardRequirement(inventory[_guishka.activeItem - 1].Name) >= cr.Level) {
						cr.active = true;
					} else {
						GD.Print("Access denied");
					}
				} catch {
				}
				try {
					GD.Print(((Node3D)_raycast.GetCollider()).Name);
					Area3D obj = (Area3D)_raycast.GetCollider();
					GroundItem cr = (GroundItem)obj.GetParentNode3D();
					Array.Resize(ref inventory, inventory.Length + 1);
					if (inventory[_guishka.activeItem - 1].canAdd(cr.heldItem)) {
						Item transfering = cr.heldItem;
						inventory[_guishka.activeItem - 1] += new Item(transfering.Name, transfering.SysName, transfering.MaxDurability, transfering.durability_, transfering.MeshPath);
						_guishka.slots = inventory;
						cr.QueueFree();
					}
				} catch {
				}
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		var direction = Vector3.Zero;

		float cameraYaw = this.RotationDegrees.Y * Mathf.Pi / 180.0f;
		
		if (Input.IsActionPressed("turn_flash"))
		{
			if (wait == 0.0) {
				if (_flashlight.LightEnergy == 6.0f) {
					_flashlight.LightEnergy = 0f;
				} else {
					_flashlight.LightEnergy = 6.0f;
				}
				wait = 20.0;
			}
		}
		if (Input.IsActionPressed("move_backwards"))
		{
			direction.X += (float)Math.Sin(cameraYaw);
			direction.Z += (float)Math.Cos(cameraYaw);
		}
		if (Input.IsActionPressed("move_forward"))
		{
			direction.X -= (float)Math.Sin(cameraYaw);
			direction.Z -= (float)Math.Cos(cameraYaw);
		}
		if (Input.IsActionPressed("move_right"))
		{
			direction.X += (float)Math.Sin(cameraYaw + Mathf.Pi / 2);
			direction.Z += (float)Math.Cos(cameraYaw + Mathf.Pi / 2);
		}
		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= (float)Math.Sin(cameraYaw + Mathf.Pi / 2);
			direction.Z -= (float)Math.Cos(cameraYaw + Mathf.Pi / 2);
		}
		if (Input.IsActionPressed("jump") && !Jumped)
		{
			Jumped = true;
			direction.Y += 2.0f;
		}
		if (Input.IsActionPressed("first_item")) {
			_guishka.activeItem = 1;
		}
		if (Input.IsActionPressed("second_item")) {
			_guishka.activeItem = 2;
		}
		if (Input.IsActionPressed("third_item")) {
			_guishka.activeItem = 3;
		}
		if (Input.IsActionPressed("next_item")) {
			_guishka.activeItem = (_guishka.activeItem % 3) + 1;
		}
		if (Input.IsActionPressed("previous_item")) {
			if (_guishka.activeItem == 1) {
				_guishka.activeItem = 3;
			} else {
				_guishka.activeItem = _guishka.activeItem - 1;
			}
		}

		if (direction.Length() > 0)
		{
			direction = direction.Normalized();
		}

		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;
		if (direction.Y != 0) 
		{
			_targetVelocity.Y = direction.Y * Speed;
		}

		if (!IsOnFloor())
		{
			_targetVelocity.Y -= FallAcceleration * (float)delta;
		}
		else
		{
			Jumped = false;
		}

		Velocity = _targetVelocity;
		MoveAndSlide();
	}
}
