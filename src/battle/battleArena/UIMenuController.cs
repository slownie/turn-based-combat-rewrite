using Godot;
using System;

public partial class UIMenuController : Control
{
	[Export] PackedScene mainMenuScene;
	[Export] PackedScene skillMenuScene;
	[Export] PackedScene itemMenuScene;
	[Export] PackedScene targetMenuScene;

	[Signal] public delegate void ActionTargetConfirmedEventHandler(UseableActionResource selectedAction, Godot.Collections.Array<BattleActor> selectedTargets);
	[Signal] public delegate void SkillUsedEventHandler(UseableSkillResource selectedSkill);
	[Signal] public delegate void FusionSkillUsedEventHandler(FusionSkillResource selectedFusionSkill);
	[Signal] public delegate void ItemUsedEventHandler(int itemIndex, int quantity);

	ActorController _actorController;
	Godot.Collections.Array<BattleActor> _battleActors = [];

	BattleActor _currentPartyActor;
	Godot.Collections.Array<InventoryItem> _battleInventory = [];
	UseableSkillResource _selectedSkill;
	FusionSkillResource _selectedFusionSkill;
	UseableItemResource _selectedItem;
	int _selectedItemIndex = -1;

	Godot.Collections.Array<UIBattleMenuBase> _menuStack = [];
	UIBattleMenuBase _currentMenu;

	Vector2 _menuPosition = new Vector2(160, 16);

	Label _descLabel;

    public override void _Ready()
	{
		_descLabel = GetNode<Label>("DescLabel");
	}

	public void BindServices(ActorController actorController, Godot.Collections.Array<BattleActor> battleActors)
	{
		_actorController = actorController;
		_battleActors = battleActors;
	}

	public void PartyTurnStart(BattleActor currentPartyActor, Godot.Collections.Array<InventoryItem> battleInventory)
	{
		_currentPartyActor = currentPartyActor;

		_battleInventory = battleInventory;

		bool skillMenuEnabled = 0 < _currentPartyActor.GetUseableSkills().Count; 
		bool itemMenuEnabled = 0 < _battleInventory.Count;

		CreateMainMenu(skillMenuEnabled, itemMenuEnabled);
	}

	private void CreateMainMenu(bool skillMenuEnabled, bool itemMenuEnabled)
	{
		UIBattleMainMenu mainMenu = mainMenuScene.Instantiate() as UIBattleMainMenu;
		AddChild(mainMenu);

		mainMenu.ActionSelected += OnSkillSelected;
		mainMenu.SkillMenuRequested += CreateSkillMenu;
		mainMenu.ItemMenuRequested += CreateItemMenu;

		mainMenu.Setup(skillMenuEnabled, itemMenuEnabled);
		LoadMenu(mainMenu);
	}

	private void CreateSkillMenu()
	{
		UISkillMenu skillMenu = skillMenuScene.Instantiate() as UISkillMenu;
		AddChild(skillMenu);

		Godot.Collections.Array<BattleActor> partyMembers = _actorController.GetPartyMembers(_battleActors);
		// Really shouldn't make a difference but its a bit cleaner
		partyMembers.Remove(_currentPartyActor);

		skillMenu.SkillSelected += OnSkillSelected;
		skillMenu.FusionSkillSelected += OnFusionSelected;
		skillMenu.SkillSelectionCancelled += UnloadMenu;
		skillMenu.DescriptionUpdate += OnEntryChanged;

		// If the player reaches this menu, then there are useable skills
		// Even if _useableSkills is empty, skillMenu.Setup will execute without errors
		skillMenu.Setup(_currentPartyActor, partyMembers);
		LoadMenu(skillMenu);

		skillMenu.Position = _menuPosition;
	}

	private void CreateItemMenu()
	{
		UIItemMenu itemMenu = itemMenuScene.Instantiate() as UIItemMenu;
		AddChild(itemMenu);

		itemMenu.ItemSelected += OnItemSelected;
		itemMenu.ItemSelectionCancelled += UnloadMenu;
		itemMenu.DescriptionUpdate += OnEntryChanged;

		itemMenu.Setup(_battleInventory);
		LoadMenu(itemMenu);

		itemMenu.Position = _menuPosition;
	}

	private void OnEntryChanged(string newEntryText)
	{
		_descLabel.Text = newEntryText;
	}

	private void OnSkillSelected(UseableSkillResource useableSkillResource)
	{
		_selectedSkill = useableSkillResource;
		CreateTargetCursor(_selectedSkill.GetUseableActionResource());
	}

	private void OnFusionSelected(FusionSkillResource fusionSkillResource)
	{
		_selectedFusionSkill = fusionSkillResource;
		CreateTargetCursor(_selectedFusionSkill.GetUseableActionResource());
	}

	private void OnItemSelected(int selectedItemIndex)
	{
		_selectedItemIndex = selectedItemIndex;
		_selectedItem = _battleInventory[_selectedItemIndex].GetItemResource() as UseableItemResource;
		CreateTargetCursor(_selectedItem.GetUseableActionResource());
	}

	private void CreateTargetCursor(UseableActionResource useableActionResource)
	{
		UITargetCursorController targetMenu = targetMenuScene.Instantiate() as UITargetCursorController;
		AddChild(targetMenu);

		targetMenu.TargetsSelected += OnTargetsSelected;
		targetMenu.TargetsCancelled += UnloadMenu;

		Godot.Collections.Array<BattleActor> _partyTargets = [];
		Godot.Collections.Array<BattleActor> _enemyTargets = [];

		// Provide targeting parameters to TargetCursorController

		// 1. Are we targeting the party, enemies, both, or the self?
		if (useableActionResource.GetTargetOppositeSide())
		{
			_enemyTargets = _actorController.GetEnemies(_battleActors);
		}

		if (useableActionResource.GetTargetSameSide())
		{
			_partyTargets = _actorController.GetPartyMembers(_battleActors);
		}

		if (useableActionResource.GetTargetSelfOnly())
		{
			_partyTargets.Add(_currentPartyActor);
		}

		// 2. Are we targeting dead or alive actors?
		if (useableActionResource.GetTargetDeadOnly())
		{
			// We are only targeting dead party members
			_partyTargets = _actorController.GetDeadActors(_partyTargets);
		} else {
			if (_enemyTargets.Count != 0) _enemyTargets = _actorController.GetLiveActors(_enemyTargets);
			if (_partyTargets.Count != 0) _partyTargets = _actorController.GetLiveActors(_partyTargets);
		}

		// 3. Pass data to controller
		targetMenu.Setup(_partyTargets, _enemyTargets, useableActionResource.GetCursorMode());
		LoadMenu(targetMenu);

		_descLabel.Text = "";
	}

	private void OnTargetsSelected(Godot.Collections.Array<BattleActor> selectedActors)
	{
		if (_selectedSkill != null)
		{
			EmitSignal(SignalName.SkillUsed, _selectedSkill);
			EmitSignal(SignalName.ActionTargetConfirmed, _selectedSkill.GetUseableActionResource(), selectedActors);
		}

		if (_selectedFusionSkill != null)
		{
			EmitSignal(SignalName.FusionSkillUsed, _selectedFusionSkill);
			EmitSignal(SignalName.ActionTargetConfirmed, _selectedFusionSkill.GetUseableActionResource(), selectedActors);
		}

		if (_selectedItemIndex != -1)
		{
			// You could include the item quantity here but I can't think of a situation where you would want that
			EmitSignal(SignalName.ItemUsed, _selectedItemIndex, 1);
			EmitSignal(SignalName.ActionTargetConfirmed, _selectedItem.GetUseableActionResource(), selectedActors);
		}



		Cleanup();
	}

	private void LoadMenu(UIBattleMenuBase battleMenu)
	{
		if (_currentMenu != null) _currentMenu.Disable();
		_menuStack.Insert(0, battleMenu);
		
		_currentMenu = _menuStack[0];
		_currentMenu.Enable();
	}

	private void UnloadMenu()
	{
		_descLabel.Text = "";
		// Only unload the menu if we are not on the main menu
		if (1 < _menuStack.Count)
		{
			_currentMenu.QueueFree();
			_menuStack.RemoveAt(0);

			_currentMenu = _menuStack[0];
			_currentMenu.Enable();
		}
	}

	private void Cleanup()
	{
		// Destroy all menus/targetting
		foreach (Node node in GetChildren())
		{
			// Really stupid way of doing this
			if (node != _descLabel) node.QueueFree();
		}

		_currentMenu = null;
		_menuStack = [];

		_currentPartyActor = null; 
		_selectedSkill = null;
		_selectedFusionSkill = null;
		_selectedItem = null;
		_selectedItemIndex = -1;
	}
}
