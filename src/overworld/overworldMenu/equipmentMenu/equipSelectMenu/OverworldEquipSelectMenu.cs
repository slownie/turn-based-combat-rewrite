using Godot;
using System;

public partial class OverworldEquipSelectMenu : UIOverworldMenuBase
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("BButton"))
		{
			EmitSignal(SignalName.ExitMenu);
		}
	}

	public void Setup(GameState _gameState, int partyMemberIndex, EquipmentItemResource.EquipmentType equipmentType)
	{
		_gameState.GetActivePartyMembers()[partyMemberIndex].GetCharacterStats().ApplyStrength(6);
	}
}
