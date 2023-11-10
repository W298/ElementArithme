using UnityEngine;
using UnityEngine.UI;

public class BaseCardObject : MonoBehaviour
{
	private BaseCard baseCard;

	public void Init(BaseCard baseCard)
	{
		this.baseCard = baseCard;
		GetComponentInChildren<Text>().text = baseCard.ToString();
	}
}
