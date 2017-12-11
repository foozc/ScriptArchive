
public class CSVItem {
	
}
// 玩家信息/
public class UserInfoItem{
	
	[CSVColumnAttributer("level")]
	public int level;
	
	[CSVColumnAttributer("maxExp")]
	public int maxExp;
	
	[CSVColumnAttributer("maxScore")]
	public int maxScore;
	
	[CSVColumnAttributer("gold")]
	public int gold;
	
	[CSVColumnAttributer("dropItems")]
	public string dropItems;
	
	[CSVColumnAttributer("dropRates")]
	public string dropRates;
	
}

//游戏中商店建筑属性/
public class GameShopBuilding
{
	[CSVColumnAttributer("Level")]
	public int level;
	
	[CSVColumnAttributer("MaxResource")]
	public int maxResource;
	
	[CSVColumnAttributer("RefreshTime")]
	public float refreshTime;
	
	[CSVColumnAttributer("Hp")]
	public int hp;
	
	[CSVColumnAttributer("UpdateTime")]
	public int updateTime;
	
	[CSVColumnAttributer("Consumption")]
	public int consumption;
}

//炮塔属性/
public class GameTurret
{
	[CSVColumnAttributer("Level")]
	public int level;
	
	[CSVColumnAttributer("Attack")]
	public int attack;
	
	[CSVColumnAttributer("Hp")]
	public int hp;
	
	[CSVColumnAttributer("UpdateTime")]
	public float updateTime;
	
	[CSVColumnAttributer("Consumption")]
	public int consumption;
}

//基地属性/
public class GameStrongHold
{
	[CSVColumnAttributer("Level")]
	public int level;

	[CSVColumnAttributer("MaxResource")]
	public int maxResource;
	
	[CSVColumnAttributer("Hp")]
	public int hp;
	
	[CSVColumnAttributer("UpdateTime")]
	public int updateTime;
	
	[CSVColumnAttributer("Consumption")]
	public int consumption;
}

//城墙属性/
public class GameDefenseWall
{
	[CSVColumnAttributer("Level")]
	public int level;
	
	[CSVColumnAttributer("ExtraHp")]
	public int extraHp;
	
	[CSVColumnAttributer("Hp")]
	public int hp;
	
	[CSVColumnAttributer("UpdateTime")]
	public int updateTime;
	
	[CSVColumnAttributer("Consumption")]
	public int consumption;
}

//攻城车属性/
public class GameChariots
{
	[CSVColumnAttributer("Level")]
	public int level;
	
	[CSVColumnAttributer("Attack")]
	public int attack;
	
	[CSVColumnAttributer("Hp")]
	public int hp;
	
	[CSVColumnAttributer("UpdateTime")]
	public float updateTime;
	
	[CSVColumnAttributer("Consumption")]
	public int consumption;
}

//兵线属性/
public class GameArmy
{
	[CSVColumnAttributer("level")]
	public int level;
	
	[CSVColumnAttributer("perOutPut")]
	public int perOutPut;

	[CSVColumnAttributer("maxOutPut")]
	public int maxOutPut;
	
	[CSVColumnAttributer("hp")]
	public int hp;
	
	[CSVColumnAttributer("updateTime")]
	public int updateTime;
	
	[CSVColumnAttributer("consumption")]
	public int consumption;
}

//水泉属性/
public class GameCardle
{
	[CSVColumnAttributer("Level")]
	public int level;
	
	[CSVColumnAttributer("Regain")]
	public int regain;
	
	[CSVColumnAttributer("Hp")]
	public int hp;
	
	[CSVColumnAttributer("UpdateTime")]
	public int updateTime;
	
	[CSVColumnAttributer("Consumption")]
	public int consumption;
}

//野战区属性/
public class GameJevj
{
	[CSVColumnAttributer("Level")]
	public int level;
	
	[CSVColumnAttributer("PerOutPut")]
	public int perOutPut;
	
	[CSVColumnAttributer("MaxOutPut")]
	public int maxOutPut;
	
	[CSVColumnAttributer("Hp")]
	public int hp;
	
	[CSVColumnAttributer("UpdateTime")]
	public int updateTime;
	
	[CSVColumnAttributer("Consumption")]
	public int consumption;
}


