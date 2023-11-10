using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct EnemyInfo
{
	public string Name;
	public int HP;
	public List<Card> CardDeck;

	public EnemyInfo(string name, int hp, List<Card> initCardDeck = null)
	{
		Name = name;
		HP = hp;
		CardDeck = initCardDeck ?? new List<Card>();
	}
}

public class Card
{
}

public class BaseCard : Card
{
	public virtual string GetVisibleString()
	{
		return "";
	}
}

public class SpecialCard : Card
{
	
}

public class NumberCard : BaseCard
{
	public int Number;

	public override string GetVisibleString()
	{
		return Number.ToString();
	}

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
				return "pi/6";
			case DegreeType.PI3:
				return "pi/3";
			case DegreeType.PI2:
				return "pi/3";
			case DegreeType.PI:
				return "pi";
			case DegreeType.X2PI:
				return "2*pi";
		}

		return "";
	}
	
	public override string GetVisibleString()
	{
		switch (Type)
		{
			case DegreeType.PI6:
				return "pi/6";
			case DegreeType.PI3:
				return "pi/3";
			case DegreeType.PI2:
				return "pi/3";
			case DegreeType.PI:
				return "pi";
			case DegreeType.X2PI:
				return "2*pi";
		}

		return "";
	}
}

public enum OperatorType
{
	Multiply, Divide, Add, Subtract, BracketL, BracketR, Sqrt, Floor, Ceil, Round, Sin, Cos, Tan, 
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
			case OperatorType.Sqrt:
				return "sqrt";
			case OperatorType.Floor:
				return "floor";
			case OperatorType.Ceil:
				return "ceil";
			case OperatorType.Round:
				return "round";
			case OperatorType.Sin:
				return "sin";
			case OperatorType.Cos:
				return "cos";
			case OperatorType.Tan:
				return "tan";
		}

		return "";
	}

	public override string GetVisibleString()
	{
		return ToString();
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
	[SerializeField] private Text m_cycleText;
	
	[SerializeField] private Text m_targetNumberText;
	[SerializeField] private Text m_currentNumberText;

	[SerializeField] private GameObject m_baseCard;
	[SerializeField] private GameObject m_specialCard;

	[SerializeField] private GameObject m_sequenceContainer;
	[SerializeField] private GameObject m_deckContainer;
	[SerializeField] private GameObject m_enemyDeckContainer;
	
	[SerializeField] private GameObject m_turnIndicator;
	
	[SerializeField] private CharacterGenerator m_cg;
	
	[SerializeField] private Text m_tooltip;

	private bool isReverse = false;
	private int auto = 0;

	private int currentTurn = 1;
	private int maxTurn = 11;
	private int currentCycle = 1;
	private int maxCycle = 3;
	private bool isPlayerTurn = true;
	private List<Card> m_cardSequence = new();
	private float m_targetNumber = 0;
	private float m_currentNumber = 0;
	private float m_biasNumber = 20;
	private EnemyInfo m_enemyInfo = new("Frostbite", 100, new List<Card>()
	{
		new NumberCard { Number = 4 },
		new OperatorCard { Type = OperatorType.Cos },
		new OperatorCard { Type = OperatorType.Multiply },
		new OperatorCard { Type = OperatorType.BracketR },
		new OperatorCard { Type = OperatorType.Cos },
		new OperatorCard { Type = OperatorType.Sqrt },
		new OperatorCard { Type = OperatorType.Multiply },
		new DegreeCard { Type = DegreeType.PI },
		new OperatorCard { Type = OperatorType.Round },
		new OperatorCard { Type = OperatorType.Add },
		new NumberCard { Number = 2 },
	});

	private List<CardObject> m_cardListInDeck = new();
	private List<CardObject> m_cardListInSequence = new();
	private List<CardObject> m_cardListInEnemyDeck = new();

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
		string expression = "";
		for (var i = 0; i < m_cardSequence.Count; i++)
		{
			var card = m_cardSequence[i];
			if (card is OperatorCard && (int)(card as OperatorCard).Type >= 6)
			{
				if (++i >= m_cardSequence.Count)
					continue;
				expression += card + "(" + m_cardSequence[i] + ")";
			}
			else
			{
				expression += card;
			}
		}
		
		ExpressionEvaluator.Evaluate(expression, out float result);
		Debug.Log(expression);
		return result;
	}

	public bool PlaceCard(Card card)
	{
		if (!IsValid(card))
		{
			Debug.LogError("Invalid Sequence");
			return false;
		}

		m_cardSequence.Add(card);
		
		m_currentNumber = EvalSequence();
		m_currentNumberText.text = m_currentNumber.ToString("N");

		var cardInDeck = m_cardListInDeck.First(obj => obj.card == card);
		cardInDeck.transform.SetParent(m_sequenceContainer.transform);
		m_cardListInDeck.Remove(cardInDeck);
		m_cardListInSequence.Add(cardInDeck);
		
		// MasterController.Instance.UseCard(card);
		
		return true;
	}

	public bool EnemyPlaceCard(Card card)
	{
		if (!IsValid(card))
		{
			Debug.LogError("Invalid Sequence");
			return false;
		}

		m_cardSequence.Add(card);
		
		m_currentNumber = EvalSequence();
		m_currentNumberText.text = m_currentNumber.ToString("N");

		var cardInDeck = m_cardListInEnemyDeck.First(obj => obj.card == card);
		cardInDeck.transform.SetParent(m_sequenceContainer.transform);
		cardInDeck.transform.localScale = Vector3.one;
		m_cardListInEnemyDeck.Remove(cardInDeck);
		m_cardListInSequence.Add(cardInDeck);

		// m_enemyInfo.CardDeck.Remove(card);
		
		return true;
	}

	public bool NextCycle()
	{
		currentTurn = 1;
		isPlayerTurn = currentCycle % 2 != 1;
		m_cardSequence.Clear();
		m_targetNumber = -30;
		isReverse = true;
		m_currentNumber = 0;
		m_targetNumberText.text = m_targetNumber.ToString();
		m_currentNumberText.text = m_currentNumber.ToString();
		m_tooltip.text = "과의 차이 >= 30 이면 승리";
		
		foreach (var cardObject in m_cardListInSequence)
		{
			Destroy(cardObject.gameObject);
		}
		
		m_cardListInSequence.Clear();
		
		currentCycle++;
		
		if (currentCycle > maxCycle)
		{
			EndGame();
			return false;
		}
		
		UpdateTurnRelatedInfo();

		return true;
	}

	public bool SwitchTurn()
	{
		currentTurn++;
		if (currentTurn > maxTurn)
		{
			ApplyDamage();
			NextCycle();
			return false;
		}

		isPlayerTurn = !isPlayerTurn;
		UpdateTurnRelatedInfo();
		
		return true;
	}

	public void UpdateTurnRelatedInfo()
	{
		m_turnText.text = currentTurn + " / " + maxTurn;
		m_cycleText.text = currentCycle + " / " + maxCycle;
		
		m_turnIndicator.transform.GetChild(0).transform.gameObject.SetActive(isPlayerTurn);
		m_turnIndicator.transform.GetChild(1).transform.gameObject.SetActive(!isPlayerTurn);
	}

	private IEnumerator PlayerWinRoutine()
	{
		m_targetNumberText.text = "Player Win!";
		m_targetNumberText.fontSize = 60;
		yield return new WaitForSeconds(2.0f);;
		EndGame();
	}

	private IEnumerator EnemyWinRoutine()
	{
		m_targetNumberText.text = "Enemy Win!";
		m_targetNumberText.fontSize = 60;
		yield return new WaitForSeconds(2.0f);;
		EndGame();
	}

	public void ApplyDamage()
	{
		var cond = !isReverse ? Mathf.Abs(m_targetNumber - m_currentNumber) <= m_biasNumber : Mathf.Abs(m_targetNumber - m_currentNumber) > m_biasNumber;
		if (cond)
		{
			// Player win.
			var v = Mathf.Lerp(20, 5, Mathf.Abs(m_targetNumber - m_currentNumber) / m_biasNumber);
			SetEnemyHP(GetEnemyHP() - 50);
			
			if (GetEnemyHP() <= 0)
			{
                m_cg.newPlayer.Win();
				m_cg.enemyGameObj.GetComponent<Character>().Die();
				StartCoroutine(PlayerWinRoutine());
			}
			
			m_cg.newPlayer.Attack();
			m_cg.enemyGameObj.GetComponent<Character>().Hit();
		}
		else
		{
			SetPlayerHP(Mathf.Clamp(GetPlayerHP() - 50, 0, 100));
			
			if (GetPlayerHP() <= 0)
			{
				m_cg.newPlayer.Die();
				m_cg.enemyGameObj.GetComponent<Character>().Win();
				StartCoroutine(EnemyWinRoutine());
			}
			
			m_cg.newPlayer.Hit();
			m_cg.enemyGameObj.GetComponent<Character>().Attack();
		}
	}

	public void EndGame()
	{
		SceneManager.LoadScene("Scenes/StageSelect");
		StageController.Instance.isClear = true;
	}

	public bool IsValid(Card newCard)
	{
		return true;
		Card prevCard = m_cardSequence.Count > 0 ? m_cardSequence[^1] : null;
		if (prevCard == null) return true;

		if (prevCard is OperatorCard)
		{
			var p = (int)(prevCard as OperatorCard).Type;
			if (10 <= p && p <= 12 && newCard is not DegreeCard) return false;
		}

		if (prevCard is NumberCard && newCard is OperatorCard && (int)(newCard as OperatorCard).Type >= 4) return false;
		if (prevCard is DegreeCard && newCard is DegreeCard) return false;
		if (prevCard is DegreeCard && newCard is NumberCard) return false;
		if (prevCard is NumberCard && newCard is DegreeCard) return false;
		if (prevCard is NumberCard && newCard is NumberCard) return false;
		if (prevCard is OperatorCard && newCard is OperatorCard)
		{
			var p = (int)(prevCard as OperatorCard).Type;
			var n = (int)(newCard as OperatorCard).Type;

			return (2 <= p) && (2 <= n);
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

	public CardObject SpawnEnemyCardToEnemyDeck(Card card)
	{
		if (card is BaseCard)
		{
			var cardGameObj = Instantiate(m_baseCard, Vector3.zero, Quaternion.identity);
			cardGameObj.transform.SetParent(m_enemyDeckContainer.transform);
			cardGameObj.transform.localScale = Vector3.one;
			
			var cardObj = cardGameObj.GetComponent<BaseCardObject>();
			cardObj.Init(card, true);
			
			return cardObj;
		}
		else
		{
			var cardGameObj = Instantiate(m_specialCard, Vector3.zero, Quaternion.identity);
			cardGameObj.transform.SetParent(m_enemyDeckContainer.transform);
			cardGameObj.transform.localScale = Vector3.one;

			var cardObj = cardGameObj.GetComponent<SpecialCardObject>();
			cardObj.Init(card, true);
			
			return cardObj;
		}
	}

	private void Start()
    {
		m_stageText.text = "Stage " + MasterController.Instance.currentStageIndex;
		m_playerNameText.text = MasterController.Instance.PlayerInfo.Name;
	    m_enemyNameText.text = m_enemyInfo.Name;

	    m_targetNumber = 15;
        
		m_targetNumberText.text = m_targetNumber.ToString();
		m_currentNumberText.text = m_currentNumber.ToString("N");
		
		SetPlayerHP(GetPlayerHP());
		SetEnemyHP(GetEnemyHP());

		UpdateTurnRelatedInfo();
		
		foreach (var playerCard in MasterController.Instance.PlayerInfo.CardDeck)
		{
			var spawnCardObject = SpawnCardToDeck(playerCard);
			spawnCardObject.CardClickEvent.AddListener(OnPlayerCardSelect);
			m_cardListInDeck.Add(spawnCardObject);
		}
		
		foreach (var enemyCard in m_enemyInfo.CardDeck)
		{
			var spawnCardObject = SpawnEnemyCardToEnemyDeck(enemyCard);
			spawnCardObject.CardClickEvent.AddListener(OnEnemyCardSelect);
			m_cardListInEnemyDeck.Add(spawnCardObject);
		}
	}

	private IEnumerator DelayedEnemySelect()
	{
		yield return new WaitForSeconds(0.4f);
		var success = EnemyPlaceCard(m_enemyInfo.CardDeck[auto++]);
		if (success) SwitchTurn();
	}

	private void OnPlayerCardSelect(Card card)
	{
		if (!isPlayerTurn) return;
		
		var success = PlaceCard(card);
		if (success) SwitchTurn();

		StartCoroutine(DelayedEnemySelect());
	}
	
	private void OnEnemyCardSelect(Card card)
	{
		if (isPlayerTurn) return;
		
		var success = EnemyPlaceCard(card);
		if (success) SwitchTurn();
	}
}
