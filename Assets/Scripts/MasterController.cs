using System.Collections.Generic;

public struct PlayerInfo
{
	public string Name;
	public int HP;
	public int Gold;
	public List<Card> CardDeck;

	public PlayerInfo(string name, int hp, int gold, List<Card> initCardDeck = null)
	{
		Name = name;
		HP = hp;
		Gold = gold;
		CardDeck = initCardDeck ?? new List<Card>();
	}
}

public class MasterController
{
	private static MasterController _battleController;
	public static MasterController Instance => _battleController ??= new MasterController();

	public int currentStageIndex = 0;
	public PlayerInfo PlayerInfo = new("Hansu", 100, 0,
	new List<Card>() {
		new NumberCard { Number = 15 },
		new NumberCard { Number = 16 },
		new DegreeCard { Type = DegreeType.PI2 },
		new DegreeCard { Type = DegreeType.PI },
		new DegreeCard { Type = DegreeType.PI },
		new OperatorCard { Type = OperatorType.Add },
		new OperatorCard { Type = OperatorType.Multiply },
		new OperatorCard { Type = OperatorType.Multiply },
		new OperatorCard { Type = OperatorType.BracketL },
		new OperatorCard { Type = OperatorType.Ceil },
		new OperatorCard { Type = OperatorType.Sqrt },
	});

	public void AddCard(Card card)
	{
		PlayerInfo.CardDeck.Add(card);
	}

	public void UseCard(Card card)
	{
		PlayerInfo.CardDeck.Remove(card);
	}
}
