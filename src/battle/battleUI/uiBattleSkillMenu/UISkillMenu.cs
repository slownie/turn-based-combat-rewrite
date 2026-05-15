using Godot;
using System;

public partial class UISkillMenu : UIBattleMenuBase
{
    [Export] PackedScene skillMenuEntry;
    [Export] PackedScene targetCursorScene;

    [Signal] public delegate void SkillSelectedEventHandler(UseableSkillResource selectedSkill);
    [Signal] public delegate void SkillSelectionCancelledEventHandler();

    Godot.Collections.Array<UseableSkillResource> _skillList = [];
    Godot.Collections.Array<UISkillMenuEntry> _entries = [];
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
        if (@event.IsActionPressed("AButton"))
        {
            EmitSignal(SignalName.SkillSelected, _skillList[index]);
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
    }

    public void Setup(Godot.Collections.Array<UseableSkillResource> skillList)
    {
        _skillList = skillList;

        for (int i=0; i < _skillList.Count; i++)
        {
            UseableSkillResource useableSkill = _skillList[i];
            UISkillMenuEntry skillEntry = skillMenuEntry.Instantiate() as UISkillMenuEntry;
            AddChild(skillEntry);
            skillEntry.Position = new Vector2(skillEntry.Position.X, skillEntry.Position.Y + (32 * i));

            skillEntry.Setup(useableSkill);
            _entries.Add(skillEntry);
        }

        _targetCursor = targetCursorScene.Instantiate() as UITargetCursor;
        AddChild(_targetCursor);

        index = 0;

        SetProcessInput(true);
    }
}
