using UnityEngine.UI;

public class BaseCardObject : CardObject
{
	public override void Init(Card baseCard, bool isEnemy = false)
	{
		base.Init(baseCard, isEnemy);
		this.card = baseCard;
		GetComponentInChildren<Text>().text = ((BaseCard)baseCard).GetVisibleString();
	}
}
