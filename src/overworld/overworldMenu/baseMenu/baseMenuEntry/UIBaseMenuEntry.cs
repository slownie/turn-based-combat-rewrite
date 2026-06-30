using Godot;
using System;

public partial class UIBaseMenuEntry : Control
{
	[Export] Color choiceColor = new Color("#fbff00");
	[Export] Color defaultColor = new Color("#ffffff");

	Label _textLabel;
	public enum MainMenuType { Skills, Items, Equipment, Party, Settings, Save };
	MainMenuType _mainMenuType;

	bool highlighted = false;
	public bool Highlighted
	{
		get { return highlighted; }
		set
		{
			highlighted = value;
			if (highlighted)
			{
				_textLabel.Modulate = choiceColor;
			} else {
				_textLabel.Modulate = defaultColor;
			}
		}
	}

	public override void _Ready()
	{
		_textLabel = GetNode<Label>("HBoxContainer/Label");
	}

	public void Setup(MainMenuType menuType)
	{
		_mainMenuType = menuType;

		string text = "";
		switch (_mainMenuType)
		{
			case MainMenuType.Skills:
			{
				text = "Skills";
				break;
			}

			case MainMenuType.Items:
			{
				text = "Items";
				break;
			}

			case MainMenuType.Equipment:
			{
				text = "Equipment";
				break;
			}

			case MainMenuType.Party:
			{
				text = "Party";
				break;
			}

			case MainMenuType.Settings:
			{
				text = "Settings";
				break;
			}

			case MainMenuType.Save:
			{
				text = "Save";
				break;
			}
		}

		_textLabel.Text = text;
	}

	public MainMenuType GetMenuType() { return _mainMenuType; }
}
