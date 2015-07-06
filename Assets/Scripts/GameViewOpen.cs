using UnityEngine;
using System.Collections;

public class GameViewOpen : MonoBehaviour {

    public UILabel txtTip;

    public GameObject gobjContinueTip;
    
    // Use this for initialization
    string[] strs;

    int index = -1;

    bool enableNext = false;



	void Start () {
        LevelData ld = GameResources.GetLevelData(GameManager.curLevelId);
        strs = ld.strOpens;
        ShowNextTxt();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.X) || Input.GetMouseButtonUp(0))
        {
            if (enableNext == true)
            {
                ShowNextTxt();
            }
            
        }
	}

    void ShowNextTxt() 
    {
        index++;
        if (index >= strs.Length)
        {
            OnShowEnd();
            return;
        }

        enableNext = false;
        gobjContinueTip.SetActive(false);
        string str = strs[index];
        StartCoroutine(ShowTxt(str));
    }

    void OnShowEnd() 
    {
        Application.LoadLevel("level_" + GameManager.curLevelId);
    }

    IEnumerator ShowTxt(string str) 
    {
        int showLen = 1;
        while (true)
        {
            txtTip.text = str.Substring(0, showLen);
            yield return new WaitForSeconds(0.08f);
            showLen++;
            if (showLen > str.Length)
            {
                // 显示完成
                enableNext = true;
                gobjContinueTip.SetActive(true);
                break;
            }
        }
    }
}
