using UnityEngine;
using UnityEngine.Events;

public class CardObject : MonoBehaviour
{
    public Card card;
    public UnityEvent<Card> CardClickEvent;

    public virtual void Init(Card baseCard)
    {
    }

    public void OnClick()
    {
        CardClickEvent.Invoke(card);
    }
}