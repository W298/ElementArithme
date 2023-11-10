public struct PlayerInfo
{
	public string Name;
	public int HP;

	public PlayerInfo(string name, int hp)
	{
		Name = name;
		HP = hp;
	}
}

public class MasterController
{
	private static MasterController _battleController;
	public static MasterController Instance => _battleController ??= new MasterController();

	public int currentStageIndex = 0;
	public PlayerInfo PlayerInfo = new("Hansu", 20);
}
