using Godot;
using System;

public partial class UISkillMenuEntry : Control
{
    [Export] Color hpColor = new Color("#ff0000");
	[Export] Color mpColor = new Color("#00ff0d");

    TextureRect _icon;
    Label _nameText;
    Label _costTypeText;
    Label _costAmountText;

    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("HBoxContainer/Icon");
        _nameText = GetNode<Label>("HBoxContainer/Name");
        _costTypeText = GetNode<Label>("HBoxContainer/CostType");
        _costAmountText = GetNode<Label>("HBoxContainer/CostAmount");
    }

    public void Setup(UseableSkillResource skillResource)
    {
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
}
