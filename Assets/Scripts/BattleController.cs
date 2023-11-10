using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct EnemyInfo
{
	public string Name;
	public int HP;

	public EnemyInfo(string name, int hp)
	{
		Name = name;
		HP = hp;
	}
}

public class Card
{
	
}

public class BaseCard : Card
{

}

public class SpecialCard : Card
{
	
}

public class NumberCard : BaseCard
{
	public int Number;

	public override string ToString()
	{
		return Number.ToString();
	}
}

public enum DegreeType
{
	PI6, PI3, PI2, PI, X2PI
}

public class DegreeCard : BaseCard
{
	public DegreeType Type;
	
	public override string ToString()
	{
		switch (Type)
		{
			case DegreeType.PI6:
				return "π/6";
			case DegreeType.PI3:
				return "π/3";
			case DegreeType.PI2:
				return "π/3";
			case DegreeType.PI:
				return "π";
			case DegreeType.X2PI:
				return "2π";
		}

		return "";
	}
}

public enum OperatorType
{
	Multiply, Divide, Add, Subtract, BracketL, BracketR, Sin
}

public class OperatorCard : BaseCard
{
	public OperatorType Type;

	public override string ToString()
	{
		switch (Type)
		{
			case OperatorType.Multiply:
				return "*";
			case OperatorType.Divide:
				return "/";
			case OperatorType.Add:
				return "+";
			case OperatorType.Subtract:
				return "-";
			case OperatorType.BracketL:
				return "(";
			case OperatorType.BracketR:
				return ")";
			case OperatorType.Sin:
				return "sin";
		}

		return "";
	}
}

public class BattleController : MonoBehaviour
{
	[SerializeField] private Text m_playerNameText;
	[SerializeField] private Text m_playerHPText;
	[SerializeField] private Image m_playerHPBar;

	[SerializeField] private Text m_enemyNameText;
	[SerializeField] private Text m_enemyHPText;
	[SerializeField] private Image m_enemyHPBar;

	[SerializeField] private Text m_stageText;
	[SerializeField] private Text m_turnText;

	[SerializeField] private Text m_targetNumberText;
	[SerializeField] private Text m_currentNumberText;

	[SerializeField] private GameObject m_baseCard;
	[SerializeField] private GameObject m_specialCard;

	[SerializeField] private GameObject m_sequenceContainer;
	[SerializeField] private GameObject m_deckContainer;

	private int currentTurn = 1;
	private int maxTurn = 10;
	private bool isPlayerTurn = true;
	private List<Card> m_cardSequence = new();
	private float m_targetNumber = 0;
	private float m_currentNumber = 0;
	private EnemyInfo m_enemyInfo = new("Frostbite", 100);

	private List<CardObject> m_cardListInDeck = new();
	private List<CardObject> m_cardListInSequence = new();

	public int GetPlayerHP() { return MasterController.Instance.PlayerInfo.HP; }
	public int GetEnemyHP() { return m_enemyInfo.HP; }

	public void SetPlayerHP(int newHP)
	{
		MasterController.Instance.PlayerInfo.HP = newHP;
		m_playerHPText.text = GetPlayerHP() + " / 100";
		m_playerHPBar.fillAmount = GetPlayerHP() / 100.0f;
	}

	public void SetEnemyHP(int newEnemyHP)
	{
		m_enemyInfo.HP = newEnemyHP;
		m_enemyHPText.text = GetEnemyHP() + " / 100";
		m_enemyHPBar.fillAmount = GetEnemyHP() / 100.0f;
	}

	public float EvalSequence()
	{
		string expression = m_cardSequence.Aggregate("", (current, card) =>
		{
			if (card is DegreeCard)
			{
				var str = card.ToString();
				str.Replace("π", "pi");
				return current + str;
			}

			return current + card;
		});
		ExpressionEvaluator.Evaluate(expression, out float result);
		return result;
	}

	public bool PlaceCard(bool isPlayer, Card card)
	{
		if (!IsValid(card))
		{
			Debug.LogError("Invalid Sequence");
			return false;
		}

		m_cardSequence.Add(card);
		
		m_currentNumber = EvalSequence();
		m_currentNumberText.text = m_currentNumber.ToString();

		var cardInDeck = m_cardListInDeck.First(obj => obj.card == card);
		cardInDeck.transform.SetParent(m_sequenceContainer.transform);
		m_cardListInDeck.Remove(cardInDeck);
		m_cardListInSequence.Add(cardInDeck);
		
		return true;
	}

	public bool SwitchTurn()
	{
		currentTurn++;
		if (currentTurn > maxTurn)
		{
			EndGame();
			return false;
		}

		isPlayerTurn = !isPlayerTurn;
		m_turnText.text = (isPlayerTurn ? "Player Turn" : "Enemy Turn  ") + currentTurn + " / " + maxTurn;

		return true;
	}

	public void EndGame()
	{
		Debug.Log(Mathf.Abs(m_targetNumber - m_currentNumber) <= 10 ? "Player Win!" : "Player Lose!");
		SceneManager.LoadScene("Scenes/StageSelect");
	}

	public bool IsValid(Card newCard)
	{
		Card prevCard = m_cardSequence.Count > 0 ? m_cardSequence[^1] : null;
		if (prevCard == null) return true;

		if (prevCard is OperatorCard)
		{
			var p = (int)(prevCard as OperatorCard).Type;
			if (6 <= p && p <= 8 && newCard is not DegreeCard) return false;
		}
		
		if (prevCard is NumberCard && newCard is NumberCard) return false;
		if (prevCard is OperatorCard && newCard is OperatorCard)
		{
			var p = (int)(prevCard as OperatorCard).Type;
			var n = (int)(newCard as OperatorCard).Type;

			return (2 <= p && p <= 5) && (2 <= n && n <= 5);
		}

		return true;
	}

	public CardObject SpawnCardToDeck(Card card)
	{
		if (card is BaseCard)
		{
			var cardGameObj = Instantiate(m_baseCard, Vector3.zero, Quaternion.identity);
			cardGameObj.transform.SetParent(m_deckContainer.transform);
			
			var cardObj = cardGameObj.GetComponent<BaseCardObject>();
			cardObj.Init(card);
			
			return cardObj;
		}
		else
		{
			var cardGameObj = Instantiate(m_specialCard, Vector3.zero, Quaternion.identity);
			cardGameObj.transform.SetParent(m_deckContainer.transform);

			var cardObj = cardGameObj.GetComponent<SpecialCardObject>();
			cardObj.Init(card);
			
			return cardObj;
		}
	}

	private void Start()
    {
		m_stageText.text = "Stage " + MasterController.Instance.currentStageIndex;
		m_playerNameText.text = MasterController.Instance.PlayerInfo.Name;
	    m_enemyNameText.text = m_enemyInfo.Name;

		m_targetNumberText.text = m_targetNumber.ToString();
		m_currentNumberText.text = m_currentNumber.ToString();
		
		foreach (var playerCard in MasterController.Instance.PlayerInfo.CardDeck)
		{
			var spawnCardObject = SpawnCardToDeck(playerCard);
			spawnCardObject.CardClickEvent.AddListener(OnPlayerCardSelect);
			m_cardListInDeck.Add(spawnCardObject);
		}
	}

	private void OnPlayerCardSelect(Card card)
	{
		// if (!isPlayerTurn) return;
		
		var success = PlaceCard(true, card);
		if (success) SwitchTurn();
	}
}
