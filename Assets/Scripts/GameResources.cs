using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public static class GameResources{

    public static Dictionary<int, LevelData> dicLevelDatas = new Dictionary<int, LevelData>();

    public static void AddLevelData(LevelData levelData)
    {
        if (!dicLevelDatas.ContainsKey(levelData.id))
        {
            dicLevelDatas.Add(levelData.id, levelData);
        }
    }

    public static LevelData GetLevelData(int id)
    {
        LevelData levelData = null;
        if (dicLevelDatas.ContainsKey(id))
        {
            levelData = dicLevelDatas[id];
        }
        return levelData;
    }

    public static void InitLevelData()
    {
        JSONNode jdNodes = JSONNode.Parse((Resources.Load("GameData/Levels", typeof(TextAsset)) as TextAsset).ToString());
        for (int i = 0; i < jdNodes.Count; i++)
        {
            LevelData levelData = new LevelData(jdNodes[i]);
            AddLevelData(levelData);
        }

        GameManager.maxLevelCount = jdNodes.Count;
    }
}
