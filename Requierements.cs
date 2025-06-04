using Godot;
using System;
using System.Collections.Generic;

public class Requirements
{
	public Dictionary<string, int> cards;
	
	public Requirements() {
		cards = new Dictionary<string, int>
		{
			{ "", 0 },
			{ "Scientist Card", 3 },
			{ "05 Card", 5 }
		};
	}
	
	public int cardRequirement(string name){
		if (cards.ContainsKey(name)) {
			return cards[name];
		} else {
			return 0;
		}
	}
	
}
