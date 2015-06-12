using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameManager{

   // 总心数
    static int maxHearts;
    public static int _MaxHearts
    {
        get { return GameManager.maxHearts; }
        set { GameManager.maxHearts = value; }
    }

    // 当前已获得心数
    public static int _CurHearts
    {
        get 
        {
            int count = 0;
            foreach (PlayerLevelData item in dicPlayerLevelDatas.Values)
            {
                count += item.hearts.Length;
            }
            return count;
        }
    }

    public static Dictionary<int, PlayerLevelData> dicPlayerLevelDatas = new Dictionary<int, PlayerLevelData>();
    public static PlayerLevelData GetPlayerLevelData(int levelId)
    {
        PlayerLevelData pld = null;
        if (dicPlayerLevelDatas.ContainsKey(levelId))
        {
            pld = dicPlayerLevelDatas[levelId];
        }
        return pld;
    }

    public static  int curLevelId;

    public static bool hasInit = false;

    public static bool enableMusic = true;

    public static bool enableSound = true;

    public static int maxLevelCount = 1;
}
