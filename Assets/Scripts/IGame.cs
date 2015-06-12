using UnityEngine;
using System.Collections;
using SimpleJSON;

public static class IKey 
{
    public static string PlayerLevel = "playerlevel";    	
}


public class LevelData
{
    public int id;
    public int hearts;

    public LevelData() { }
    public LevelData(JSONNode data) 
    {
        this.id = data["id"].AsInt;
        this.hearts = data["hearts"].AsInt;
    }
}

public class UIItemLevel
{
    public LevelData levelData;
    public bool active;
}

/// <summary>
/// 玩家关卡记录节点
/// </summary>
public class PlayerLevelData
{
    public int levelId;
    public int[] hearts;

    public PlayerLevelData() { }
    public PlayerLevelData(int levelId, int[] hearts) 
    {
        this.levelId = levelId;
        this.hearts = hearts;
    }

    public PlayerLevelData(JSONNode data)
    {
        this.levelId = data["level_id"].AsInt;
        this.hearts = data["hearts"].AsArray.ToIntArr();
    }

    public JSONNode ToJsonData() 
    {
        JSONNode jd = new JSONClass();
        jd["level_id"].AsInt = levelId;
        JSONArray arr  = new JSONArray();
        for (int i = 0; i < hearts.Length; i++)
        {
            arr[i].AsInt = hearts[i];
        }
        jd["hearts"] = arr;

        return jd;
    }
}