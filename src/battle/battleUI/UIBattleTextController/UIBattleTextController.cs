using Godot;
using System;

public partial class UIBattleTextController : Control
{
	[Export] PackedScene battleTextScene;

	[ExportCategory("Damage")]
	[Export] Color damageColor = new Color("#ff0000");
	[Export] Color critColor = new Color("#FF5C00");
	
	[ExportCategory("Healing")]
	[Export] Color healColor = new Color("#00ff00");

	[ExportCategory("Rejuvenation")]
	[Export] Color rejuvenationColor = new Color("#00eeff");

	[ExportCategory("Miss")]
	[Export] Color missColor = new Color("#ffffff");




	public void Setup(Godot.Collections.Array<BattleActor> actors)
	{
		foreach(BattleActor actor in actors)
		{
			actor.DamageReceived += OnActorDamageReceived;
			actor.HealReceived += OnActorHealReceived;
			actor.RejuvenateReceived += OnActorRejuvenateReceived;

			actor.MissReceived += OnActorMissedReceived;
		}
	}

	private void OnActorDamageReceived(BattleActor actor, int damage, bool didCrit)
	{
		Color useColor = damageColor;
		if (didCrit) useColor = critColor;

		UIBattleText battleText = battleTextScene.Instantiate() as UIBattleText;
		AddChild(battleText);

		Vector2 textPosition = new Vector2(actor.GlobalPosition.X, actor.GlobalPosition.Y - 16);
		battleText.Setup(textPosition, damage.ToString(), useColor);
	}

	private void OnActorHealReceived(BattleActor actor, int heal)
	{
		Color useColor = healColor;

		UIBattleText battleText = battleTextScene.Instantiate() as UIBattleText;
		AddChild(battleText);

		Vector2 textPosition = new Vector2(actor.GlobalPosition.X, actor.GlobalPosition.Y - 16);
		battleText.Setup(textPosition, heal.ToString(), useColor);
	}

	private void OnActorRejuvenateReceived(BattleActor actor, int rejuvenate)
	{
		Color useColor = rejuvenationColor;

		UIBattleText battleText = battleTextScene.Instantiate() as UIBattleText;
		AddChild(battleText);

		Vector2 textPosition = new Vector2(actor.GlobalPosition.X, actor.GlobalPosition.Y - 64);
		battleText.Setup(textPosition, rejuvenate.ToString(), useColor);
	}

	private void OnActorMissedReceived(BattleActor actor)
	{
		Color useColor = missColor;

		UIBattleText battleText = battleTextScene.Instantiate() as UIBattleText;
		AddChild(battleText);

		Vector2 textPosition = new Vector2(actor.GlobalPosition.X, actor.GlobalPosition.Y - 64);
		battleText.Setup(textPosition, "Missed!", useColor);
	}
}
