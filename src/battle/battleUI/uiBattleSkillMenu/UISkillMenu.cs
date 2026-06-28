using Godot;
using System;

// This is some dogshit but it's what I deserve for trying to cram two
// menus worth of functionality into one.

public partial class UISkillMenu : UIBattleMenuBase
{
    [Export] PackedScene skillMenuEntry;
    [Export] PackedScene fusionMenuEntry;
    [Export] PackedScene targetCursorScene;

    [Signal] public delegate void SkillSelectedEventHandler(UseableSkillResource selectedSkill);
    [Signal] public delegate void FusionSkillSelectedEventHandler(FusionSkillResource fusionSkill);
    [Signal] public delegate void SkillSelectionCancelledEventHandler();

    Godot.Collections.Array<UISkillMenuEntry> _skillEntries = [];
    Godot.Collections.Array<UIFusionMenuEntry> _fusionEntries = [];
    
    // Godot/C# casting doesn't allow for something like Godot.Collections.Array<BaseMenuEntry> activeEntries
    // So this unfortunately results in us wrapping everything in a MenuMode if check
    enum MenuMode { Skill, Fusion }
    MenuMode _currentMenuMode = MenuMode.Skill;
    int _index = 0;
    int index
    {
        get { return _index; }
        set
        {
            _index = value;
		
			// Clamp
            if (_currentMenuMode == MenuMode.Skill)
            {
                if (_skillEntries.Count <= _index) _index = 0;
			    if (_index < 0) _index = _skillEntries.Count - 1;
            } else {
                if (_fusionEntries.Count <= _index) _index = 0;
                if (_index < 0) _index = _fusionEntries.Count - 1;    
            }

            if (_targetCursor != null)
            {
                if (_currentMenuMode == MenuMode.Skill)
                {
                    _targetCursor.Position = new Vector2(_skillEntries[_index].Position.X, _skillEntries[_index].Position.Y + 16);
                } else {
                    _targetCursor.Position = new Vector2(_fusionEntries[_index].Position.X, _fusionEntries[_index].Position.Y + 16);
                }
            }

            // Update Desc
            if (_currentMenuMode == MenuMode.Skill) {
                EmitSignal(SignalName.DescriptionUpdate, _skillEntries[_index].GetMenuDesc());
            } else {
                EmitSignal(SignalName.DescriptionUpdate, _fusionEntries[_index].GetMenuDesc());
            }
        }
    }

    UITargetCursor _targetCursor;

    Control _skillMenu;
    Control _fusionMenu;

    public override void _Ready()
    {
        _skillMenu = GetNode<Control>("SkillMenuEntries");
        _fusionMenu = GetNode<Control>("FusionMenuEntries");


        SetProcessInput(false);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("AButton"))
        {
            if (_currentMenuMode == MenuMode.Skill)
            {
                if (_skillEntries[index].IsEnabled())
                {
                    EmitSignal(SignalName.SkillSelected, _skillEntries[index].GetUseableSkill());
                }
            } else {
                if (_fusionEntries[index].IsEnabled())
                {
                    EmitSignal(SignalName.FusionSkillSelected, _fusionEntries[index].GetFusionSkill());
                }                
            }
        }

		// Quit Selection
		if (@event.IsActionPressed("BButton"))
		{
			EmitSignal(SignalName.SkillSelectionCancelled);
		}

        if (@event.IsActionPressed("MoveDown"))
		{
			index += 1;
		}

		if (@event.IsActionPressed("MoveUp"))
		{
			index -= 1;
		}

        if (@event.IsActionPressed("LButton") || @event.IsActionPressed("RButton"))
        {
            if (_currentMenuMode == MenuMode.Skill)
            {
                // Check if can swap
                if (0 < _fusionEntries.Count)
                {
                    SwapMenu();
                }
            } else {
                // Check if can swap
                if (0 < _skillEntries.Count)
                {
                    SwapMenu();
                }
            }
        }
    }

    public void Setup(BattleActor currentActor, Godot.Collections.Array<BattleActor> partyMembers)
    {
        for (int i=0; i < currentActor.GetUseableSkills().Count; i++)
        {
            UseableSkillResource useableSkill = currentActor.GetUseableSkills()[i];
            UISkillMenuEntry skillEntry = skillMenuEntry.Instantiate() as UISkillMenuEntry;
            _skillMenu.AddChild(skillEntry);
            skillEntry.Position = new Vector2(skillEntry.Position.X, skillEntry.Position.Y + (32 * i));

            // Can we use this skill?
            bool skillIsUseable = true;
            if (!currentActor.IgnoreSkillCosts)
            {
                if (useableSkill.GetSkillCostType() == UseableSkillResource.SkillCostType.HP)
                {
                    if (currentActor.GetCurHP() <= useableSkill.GetSkillCostAmount() || !currentActor.CanSelectPhysSkills)
                    {
                        skillIsUseable = false;
                    }
                } else {
                    if (currentActor.GetCurMP() < useableSkill.GetSkillCostAmount() || !currentActor.CanSelectElemSkills)
                    {
                        skillIsUseable = false;
                    }
                }
            }

            skillEntry.Setup(useableSkill, skillIsUseable);
            _skillEntries.Add(skillEntry);
        }

        for (int i=0; i < currentActor.GetFusionSkills().Count; i++)
        {
            FusionSkillResource fusionSkill = currentActor.GetFusionSkills()[i];
            BattleActor partnerActor = BattleConsts.FindActorByFusionID(fusionSkill.GetFusionID(), partyMembers);

            UIFusionMenuEntry fusionEntry = fusionMenuEntry.Instantiate() as UIFusionMenuEntry;
            _fusionMenu.AddChild(fusionEntry);
            fusionEntry.Position = new Vector2(fusionEntry.Position.X, fusionEntry.Position.Y + (32 * i));

            // Can we use this skill?
            bool userCanUseSkill = true;
            if (!currentActor.IgnoreSkillCosts)
            {
                if (fusionSkill.GetSkillCostType() == UseableSkillResource.SkillCostType.HP)
                {
                    if (currentActor.GetCurHP() <= fusionSkill.GetSkillCostAmount() || !currentActor.CanSelectPhysSkills)
                    {
                        userCanUseSkill = false;
                    }
                } else {
                    if (currentActor.GetCurMP() < fusionSkill.GetSkillCostAmount() || !currentActor.CanSelectElemSkills)
                    {
                        userCanUseSkill = false;
                    }
                }
            }

            bool partnerCanUseSkill = true;
            if (!partnerActor.IgnoreSkillCosts)
            {
                if (fusionSkill.GetSkillCostType() == UseableSkillResource.SkillCostType.HP)
                {
                    if (partnerActor.GetCurHP() <= fusionSkill.GetSkillCostAmount() || !partnerActor.CanSelectPhysSkills)
                    {
                        partnerCanUseSkill = false;
                    }
                } else {
                    if (partnerActor.GetCurMP() < fusionSkill.GetSkillCostAmount() || !partnerActor.CanSelectElemSkills)
                    {
                        partnerCanUseSkill = false;
                    }
                }
            }

            if (partnerActor.SelectRandomAction) partnerCanUseSkill = false;


            fusionEntry.Setup(fusionSkill, partnerActor.GetBattleIcon(), userCanUseSkill && partnerCanUseSkill);
            _fusionEntries.Add(fusionEntry);
        }

        _targetCursor = targetCursorScene.Instantiate() as UITargetCursor;
        AddChild(_targetCursor);

        if (_fusionEntries.Count <= 0) { SwapMenu(); } 

        index = 0;
        SetProcessInput(true);
    }

    private void SwapMenu()
    {
        if (_currentMenuMode == MenuMode.Skill)
        {
            _currentMenuMode = MenuMode.Fusion;
            _skillMenu.Hide();
            _fusionMenu.Show();
        } else {
            _currentMenuMode = MenuMode.Skill;
            _fusionMenu.Hide();
            _skillMenu.Show();
        }
        index = index;
    }
}
