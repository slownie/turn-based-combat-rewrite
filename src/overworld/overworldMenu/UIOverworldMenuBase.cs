using Godot;
using System;

public abstract partial class UIOverworldMenuBase : Control
{
	[Signal] public delegate void ExitMenuEventHandler();

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
