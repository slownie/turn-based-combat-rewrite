using Godot;
using System;

public partial class OverworldBaseMenu : UIOverworldMenuBase
{
	[Export] PackedScene partyMenuEntryScene;
	
	Godot.Collections.Array<UIPartyMenuEntry> _partyEntries = [];
	UIGoldDisplay _goldDisplay;

    public override void _Ready()
	{
		_goldDisplay = GetNode<UIGoldDisplay>("UIGoldDisplay");
	}

	public void Setup(GameState gameState)
	{
		foreach(ActivePartyMember partyMember in gameState.GetActivePartyMembers())
		{
			UIPartyMenuEntry partyMenuEntry = partyMenuEntryScene.Instantiate() as UIPartyMenuEntry;
			AddChild(partyMenuEntry);

			partyMenuEntry.Setup(partyMember);
		}

		_goldDisplay.Setup(gameState.GetGoldAmount());
	}
}
