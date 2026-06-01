using Godot;
using System;

public partial class UIFusionMenuEntry : Control
{
    [Export] Color hpColor = new Color("#ff0000");
	[Export] Color mpColor = new Color("#00ff0d");
    [Export] Color disabledColor = new Color("#A9A9A9");

    bool _enabled = false;

    TextureRect _icon;
    Label _nameText;
    Label _costTypeText;
    Label _costAmountText;
    TextureRect _partnerIcon;


    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("HBoxContainer/Icon");
        _nameText = GetNode<Label>("HBoxContainer/Name");
        _costTypeText = GetNode<Label>("HBoxContainer/CostType");
        _costAmountText = GetNode<Label>("HBoxContainer/CostAmount");
        _partnerIcon = GetNode<TextureRect>("HBoxContainer/PartnerIcon");
    }

    public void Setup(UseableSkillResource skillResource, bool enabled)
    {
        _enabled = enabled;
        if (!_enabled) _nameText.Modulate = disabledColor;

        _icon.Texture = skillResource.GetIcon();
        _nameText.Text = skillResource.GetSkillName();

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

    public bool IsEnabled() { return _enabled; }
}
