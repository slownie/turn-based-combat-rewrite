using Godot;
using System;

public partial class UIFusionMenuEntry : UIBattleMenuEntryBase
{
    [Export] Color hpColor = new Color("#ff0000");
	[Export] Color mpColor = new Color("#00ff0d");

    Label _costTypeText;
    Label _costAmountText;
    TextureRect _partnerIcon;

    public override void _Ready()
    {
        base._Ready();
        _costTypeText = GetNode<Label>("HBoxContainer/CostType");
        _costAmountText = GetNode<Label>("HBoxContainer/CostAmount");
        _partnerIcon = GetNode<TextureRect>("HBoxContainer/PartnerIcon");
    }

    public void Setup(FusionSkillResource skillResource, Texture2D partnerIcon, bool enabled)
    {
        _enabled = enabled;
        if (!_enabled) _nameText.Modulate = disabledColor;

        _icon.Texture = skillResource.GetIcon();
        _nameText.Text = skillResource.GetSkillName();
        _partnerIcon.Texture = partnerIcon;

        if (skillResource.GetSkillCostType() == UseableSkillResource.SkillCostType.HP)
        {
            _costTypeText.Text = "HP";
            _costTypeText.Modulate = hpColor;
        } else {
            _costTypeText.Text = "MP";
            _costTypeText.Modulate = mpColor;
        }

        _costAmountText.Text = skillResource.GetSkillCostAmount().ToString();
    }
}
