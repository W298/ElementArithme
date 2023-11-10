using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

public class NumberCard : Card
{
	public int Number;

	public override string ToString()
	{
		return Number.ToString();
	}
}

public enum OperatorType
{
	Add, Subtract, Multiply, Divide
}

public class OperatorCard : Card
{
	public OperatorType Type;

	public override string ToString()
	{
		switch (Type)
		{
			case OperatorType.Add:
				return "+";
			case OperatorType.Subtract:
				return "-";
			case OperatorType.Multiply:
				return "*";
			case OperatorType.Divide:
				return "/";
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

	private int currentTurn = 1;
	private int maxTurn = 4;
	private bool isPlayerTurn = true;
	private List<Card> m_cardSequence = new();
	private float m_targetNumber = 0;
	private float m_currentNumber = 0;
	private EnemyInfo m_enemyInfo = new("Frostbite", 100);

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
		string expression = m_cardSequence.Aggregate("", (current, card) => current + card);
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
	}

	public bool IsValid(Card newCard)
	{
		Card prevCard = m_cardSequence.Count > 0 ? m_cardSequence[^1] : null;
		if (prevCard == null) return true;

		if (prevCard is NumberCard && newCard is NumberCard) return false;
		if (prevCard is OperatorCard && newCard is OperatorCard) return false;

		return true;
	}

	private void Start()
    {
		m_stageText.text = "Stage " + MasterController.Instance.currentStageIndex;
		m_playerNameText.text = MasterController.Instance.PlayerInfo.Name;
	    m_enemyNameText.text = m_enemyInfo.Name;

		m_targetNumberText.text = m_targetNumber.ToString();
		m_currentNumberText.text = m_currentNumber.ToString();

	    PlaceCard(true, new NumberCard { Number = 1 });
		SwitchTurn();
	    PlaceCard(false, new OperatorCard() { Type = OperatorType.Divide });
	    SwitchTurn();
	    PlaceCard(true, new NumberCard { Number = 5 });
	    SwitchTurn();
	    PlaceCard(false, new NumberCard { Number = 2 });
	    SwitchTurn();
	}
}
