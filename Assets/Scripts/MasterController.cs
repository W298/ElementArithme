using System.Collections.Generic;

public struct PlayerInfo
{
	public string Name;
	public int HP;
	public List<Card> CardDeck;

	public PlayerInfo(string name, int hp, List<Card> initCardDeck = null)
	{
		Name = name;
		HP = hp;
		CardDeck = initCardDeck ?? new List<Card>();
	}
}

public class MasterController
{
	private static MasterController _battleController;
	public static MasterController Instance => _battleController ??= new MasterController();

	public int currentStageIndex = 0;
	public PlayerInfo PlayerInfo = new("Hansu", 20,
	new List<Card>() {
		new NumberCard { Number = 1 },
		new NumberCard { Number = 2 },
		new NumberCard { Number = 3 },
	});

	public void GetCard(Card card)
	{
		PlayerInfo.CardDeck.Add(card);
	}
}
