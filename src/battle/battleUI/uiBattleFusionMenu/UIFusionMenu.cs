using Godot;
using System;

public partial class UIFusionMenu : UIBattleMenuBase
{
    [Export] PackedScene fusionMenuEntry;
    [Export] PackedScene targetCursorScene;

    [Signal] public delegate void FusionSkillSelectedEventHandler(FusionSkillResource selectedSkill);
    [Signal] public delegate void FusionSkillSelectionCancelledEventHandler();

    Godot.Collections.Array<FusionSkillResource> _fusionSkillList = [];
    Godot.Collections.Array<UIFusionMenuEntry> _entries = [];
    int _index = 0;
    int index
    {
        get { return _index; }
        set
        {
            _index = value;
		
			// Clamp
			if (_entries.Count <= _index) _index = 0;
			if (_index < 0) _index = _entries.Count - 1;

            if (_targetCursor != null)
            {
                _targetCursor.Position = new Vector2(_entries[_index].Position.X, _entries[_index].Position.Y + 16);
            }
        }
    }

    UITargetCursor _targetCursor;

    public override void _Ready()
    {
        SetProcessInput(false);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("AButton") && _entries[index].IsEnabled())
        {
            EmitSignal(SignalName.FusionSkillSelected, _fusionSkillList[index]);
        }

		// Quit Selection
		if (@event.IsActionPressed("BButton"))
		{
			EmitSignal(SignalName.FusionSkillSelectionCancelled);
		}

        if (@event.IsActionPressed("MoveDown"))
		{
			index += 1;
		}

		if (@event.IsActionPressed("MoveUp"))
		{
			index -= 1;
		}
    }

    public void Setup(BattleActor currentActor, Godot.Collections.Array<BattleActor> partyMembers)
    {
        _fusionSkillList = currentActor.GetFusionSkills();

        for (int i=0; i < _fusionSkillList.Count; i++)
        {
            FusionSkillResource useableSkill = _fusionSkillList[i];
            UIFusionMenuEntry fusionEntry = fusionMenuEntry.Instantiate() as UIFusionMenuEntry;
            AddChild(fusionEntry);

            BattleActor partner = BattleConsts.FindActorByFusionID(useableSkill.GetFusionID(), partyMembers);

            fusionEntry.Position = new Vector2(fusionEntry.Position.X, fusionEntry.Position.Y + (32 * i));

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

            

            _entries.Add(fusionEntry);
        }

        _targetCursor = targetCursorScene.Instantiate() as UITargetCursor;
        AddChild(_targetCursor);

        index = 0;

        SetProcessInput(true);
    }
}
