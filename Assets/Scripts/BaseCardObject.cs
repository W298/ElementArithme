using UnityEngine.UI;

public class BaseCardObject : CardObject
{
	public override void Init(Card baseCard)
	{
		this.card = baseCard;
		GetComponentInChildren<Text>().text = ((BaseCard)baseCard).ToString();
	}
}
