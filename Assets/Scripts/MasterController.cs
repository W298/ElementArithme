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
	public PlayerInfo PlayerInfo = new("Hansu", 20, 0,
	new List<Card>() {
		new NumberCard { Number = 1 },
		new NumberCard { Number = 2 },
		new NumberCard { Number = 3 },
		new DegreeCard { Type = DegreeType.PI2 },
		new OperatorCard { Type = OperatorType.Sin },
		new OperatorCard { Type = OperatorType.Divide },
		new OperatorCard { Type = OperatorType.BracketL },
		new OperatorCard { Type = OperatorType.BracketR },
		new OperatorCard { Type = OperatorType.BracketL },
		new OperatorCard { Type = OperatorType.BracketR }
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
