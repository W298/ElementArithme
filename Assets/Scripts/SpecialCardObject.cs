using UnityEngine;
using UnityEngine.UI;

public class SpecialCardObject : MonoBehaviour
{
	private Card specialCard;

	public void Init(SpecialCard specialCard)
	{
		this.specialCard = specialCard;
	}
}
