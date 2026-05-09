using Godot;
using System;

public partial class UITurnIcon : TextureRect
{
    [Export] Texture2D partyMemberBorder;
    [Export] Texture2D enemyBorder;
    TextureRect _actorIcon;

    /*
        Upper and lower bounds describing the icon's movement.
    */
    Vector2 positionRange = Vector2.Zero;
    public Vector2 PositionRange
    {
        get { return positionRange; }
        set
        {
            positionRange = value;
            float newX = (float)Mathf.Lerp(positionRange.X, positionRange.Y, Progress);
			Position = new Vector2(newX, Position.Y);
        }
    }

    /*
        Determines where on the turn bar the icon is currently located (0.0 - 1.0)
    */
    double progress = 0.0;
    public double Progress
    {
        get { return progress; }
        set
        {
            progress = Mathf.Clamp(value, 0.0, 1.0);
			float newX = (float)Mathf.Lerp(PositionRange.X, PositionRange.Y, progress);
			Position = new Vector2(newX, Position.Y);
        }
    }

    public override void _Ready()
    {
        _actorIcon = GetNode<TextureRect>("ActorIcon");
    }

    public void Setup(BattleActor battleActor)
    {
        if (battleActor.GetIsPlayer())
        {
            Texture = partyMemberBorder;
        } else {
            Texture = enemyBorder;
        }

        _actorIcon.Texture = battleActor.GetBattleIcon();
    }
}
