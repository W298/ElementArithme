public class SpecialCardObject : CardObject
{
	public override void Init(Card specialCard, bool isEnemy = false)
	{
		base.Init(specialCard, isEnemy);
		this.card = specialCard;
	}
}
