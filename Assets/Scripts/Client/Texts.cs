using System.Collections.Generic;

public class Texts
{
	public const string PLAYER_WAIT = "Waiting your opponent";
	public const string CONNECTING = "Connecting";

    public const string WINNER_TITLE = "You win!";
    public const string LOSER_TITLE = "You lose!";

	public const string COIN = "Coin toss...";
	public const string COIN_FIRST = "You pick first.";
	public const string COIN_SECOND = "You pick second.";
	

    public const string MISS = "MISS!";

    public static readonly Dictionary<EquipStep, string> StepsNames = new Dictionary<EquipStep, string>
    {
        {EquipStep.FirstHand, "LEFT HAND"},
        {EquipStep.SecondHand, "LEFT HAND"},
        {EquipStep.Armor, "HELM"}
    };
}


