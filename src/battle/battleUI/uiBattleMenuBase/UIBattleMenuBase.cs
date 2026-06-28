using Godot;
using System;

public abstract partial class UIBattleMenuBase : Control
{
	[Signal] public delegate void DescriptionUpdateEventHandler(string newDescName);

	public void Enable()
	{
		Show();
		SetProcess(true);
		SetProcessInput(true);
	}

	public void Disable()
	{
		Hide();
		SetProcess(false);
		SetProcessInput(false);
	}
}
