using Godot;
using System;

public partial class OverworldSkillsMenu : UIOverworldMenuBase
{
	[Export] PackedScene _skillEntryScene;

	Godot.Collections.Array<ActivePartyMember> _partyMembers = [];
	ActivePartyMember _currentPartyMember = null;
	ActivePartyMember currentPartyMember
	{
		get { return _currentPartyMember; }
		set
		{
			_currentPartyMember = value;
			_partyMemberLabel.Text = _currentPartyMember.GetPartyMemberName();
		}
	}

	int _index = 0;
	int index
	{
		get { return _index; }
		set
		{
			_index = value;
			if (_partyMembers.Count <= _index) _index = 0;
			if (_index < 0) _index = _partyMembers.Count - 1;
			_currentPartyMember = _partyMembers[_index];
		}
	}

	VBoxContainer _equippedSkillsContainer;
	VBoxContainer _learnedSkillsContainer;

	Label _partyMemberLabel;

	enum MenuMode { Equip, Swap }
	MenuMode _currentMenuMode = MenuMode.Equip;

	public override void _Ready()
	{
		
		_equippedSkillsContainer = GetNode<VBoxContainer>("EquippedSkillsContainer");
		_learnedSkillsContainer = GetNode<VBoxContainer>("LearnedSkillsContainer");

		_partyMemberLabel = GetNode<Label>("PartyMemberLabel");
	}

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("LButton")) index -= 1;
		if (@event.IsActionPressed("RButton")) index += 1;

		if (@event.IsActionPressed("BButton")) EmitSignal(SignalName.ExitMenu);
	}

	public void Setup(Godot.Collections.Array<ActivePartyMember> partyMembers)
	{
		_partyMembers = partyMembers;

		index = 0;
		GD.Print(currentPartyMember.GetPartyMemberName());
	}
}
