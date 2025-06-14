using Godot;
using System;
using System.IO;
using System.Text;
using System.Globalization;

public partial class SaveLoad : Node
{
	[Export]
	public PackedScene playerclass {get; set;}
	
	public override void _Ready()
	{
		LoadGameData();
	}

	private async void LoadGameData()
	{
		try
		{
			using (FileStream fstream = File.OpenRead("saves\\save.dat"))
			{
				byte[] buffer = new byte[fstream.Length];
				await fstream.ReadAsync(buffer, 0, buffer.Length);
				string[] textFromFile = Encoding.Default.GetString(buffer).Split("\n");
				textFromFile[0] = textFromFile[0].Trim(new char[] { '(', ')' });

				double[] pos = {
					double.Parse(textFromFile[0].Split(", ")[0], CultureInfo.InvariantCulture),
					double.Parse(textFromFile[0].Split(", ")[1], CultureInfo.InvariantCulture),
					double.Parse(textFromFile[0].Split(", ")[2], CultureInfo.InvariantCulture)
				};
				Vector3 newpos = new Vector3((float)pos[0], (float)pos[1], (float)pos[2]);

				Item[] newinventory = { new Item(), new Item(), new Item() };
				for (int i = 0; i != 3; i++)
				{
					GD.Print(textFromFile[i + 1]);
					newinventory[i] = new Item(
						textFromFile[i + 1].Split()[0],
						textFromFile[i + 1].Split()[1],
						int.Parse(textFromFile[i + 1].Split()[2]),
						int.Parse(textFromFile[i + 1].Split()[3]),
						textFromFile[i + 1].Split()[4]
					);
				}
				Player player = new Player(newpos, newinventory);
				player.Name = "Player";
				player.Scale = new Vector3(1.3f, 1.3f, 1.3f);
				AddChild(player);
				GD.Print(player.Position + " " + player.Scale);
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr("Ошибка при загрузке данных: " + ex.Message);
			// Default spawn or other error handling
		}
	}

	public override void _Notification(int what)
	{
		if (what == NotificationWMCloseRequest)
		{
			SaveGameData();
		}
	}

	private async void SaveGameData()
	{
		try
		{
			using (FileStream fstream = new FileStream("saves\\save.dat", FileMode.OpenOrCreate))
			{
				string output = "";
				// Save player position
				output += this.GetNode<CharacterBody3D>("Player").Position.ToString() + "\n";
				
				
				// Save inventory items
				Player player = this.GetNode<Player>("Player");
				for (int i = 0; i != 3; i++)
				{
					output += player.inventory[i].ToString() + "\n";
				}
				
				// Write to file
				byte[] positionBuffer = Encoding.Default.GetBytes(output);
				await fstream.WriteAsync(positionBuffer, 0, positionBuffer.Length);
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr("Ошибка при сохранении данных: " + ex.Message);
		}
	}
}
