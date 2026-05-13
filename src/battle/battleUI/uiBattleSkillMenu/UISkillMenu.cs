using Godot;
using System;

public partial class UISkillMenu : UIBattleMenuBase
{
    [Export] PackedScene skillMenuEntry;

    [Signal] public delegate void SkillSelectedEventHandler(UseableSkillResource selectedSkill);
    [Signal] public delegate void SkillSelectionCancelledEventHandler();

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("AButton"))
		{
		}

		// Quit Selection
		if (@event.IsActionPressed("BButton"))
		{
			EmitSignal(SignalName.SkillSelectionCancelled);
		}
    }

    public void Setup(Godot.Collections.Array<UseableSkillResource> skillList)
    {
        foreach (UseableSkillResource useableSkill in skillList)
        {
            UISkillMenuEntry skillEntry = skillMenuEntry.Instantiate() as UISkillMenuEntry;
            AddChild(skillEntry);

            skillEntry.Setup(useableSkill);
        }
    }
}
