using UnityEngine;
using System.Collections;
using UnityEditor;

public static class XTools{

    static int index = 0;

    [MenuItem("XTools/重命名 %&R")]
    public static void ReName()
    {
        foreach (GameObject gobjItem in Selection.gameObjects)
        {
            gobjItem.name = gobjItem.name + index;
            index++;
        }
    }

    [MenuItem("XTools/合并")]
    public static void Combime()
    {
        GameObject gobjFirst = null;
        foreach (GameObject gobjItem in Selection.gameObjects)
        {
            if (gobjFirst == null)
            {
                gobjFirst = gobjItem;
            }
            else
            {
                gobjItem.transform.parent = gobjFirst.transform;
            }
            
        }
    }
}
