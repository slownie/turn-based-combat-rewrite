using Godot;
using System;

public partial class UISkillMenuEntry : UIBattleMenuEntryBase
{
    [Export] Color hpColor = new Color("#ff0000");
	[Export] Color mpColor = new Color("#00ff0d");

    UseableSkillResource _skillResource;

    Label _costTypeText;
    Label _costAmountText;

    public override void _Ready()
    {
        base._Ready();
        _costTypeText = GetNode<Label>("HBoxContainer/CostType");
        _costAmountText = GetNode<Label>("HBoxContainer/CostAmount");
    }

    public void Setup(UseableSkillResource skillResource, bool enabled)
    {
        _skillResource = skillResource;

        _enabled = enabled;
        if (!_enabled) _nameText.Modulate = disabledColor;

        _icon.Texture = _skillResource.GetIcon();
        _nameText.Text = _skillResource.GetSkillName();

        if (_skillResource.GetSkillCostType() == UseableSkillResource.SkillCostType.HP)
        {
            _costTypeText.Text = "HP";
            _costTypeText.Modulate = hpColor;
        } else {
            _costTypeText.Text = "MP";
            _costTypeText.Modulate = mpColor;
        }

        _costAmountText.Text = _skillResource.GetSkillCostAmount().ToString();
    }

    public UseableSkillResource GetUseableSkill() { return _skillResource; }
}
