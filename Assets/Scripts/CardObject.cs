using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    [SerializeField] protected Sprite m_enemySprite;
    
    public Card card;
    public UnityEvent<Card> CardClickEvent;

    public virtual void Init(Card card, bool isEnemy = false)
    {
        if (isEnemy)
        {
            GetComponent<Image>().sprite = m_enemySprite;
            GetComponentInChildren<Text>().color = Color.white;
        }
    }

    public void OnClick()
    {
        CardClickEvent.Invoke(card);
    }
}