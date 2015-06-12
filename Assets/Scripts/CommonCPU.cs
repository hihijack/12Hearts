using UnityEngine;
using System.Collections;
using SimpleJSON;

public class CommonCPU : MonoBehaviour {

    public GameObject gUIRoot_Main;
    public GameObject gUIRoot_Second;
    public GameObject gUIRoot_Dialog;

    GameObject gUIMain;
    public GameObject _UIMain
    {
        get { return gUIMain; }
        set { gUIMain = value; }
    }

    GameObject gUISecond;
    public GameObject _UISecond
    {
        get { return gUISecond; }
        set { gUISecond = value; }
    }

    GameObject gUIDialog;
    public GameObject _UIDialog
    {
        get { return gUIDialog; }
        set { gUIDialog = value; }
    }



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

    public string GetPlayerLevelData()
    {
        string levelData = "";
        if (PlayerPrefs.HasKey(IKey.PlayerLevel))
        {
            levelData = PlayerPrefs.GetString(IKey.PlayerLevel);
        }

        Debug.Log("ReadData:" + levelData);
        return levelData;

    }

    public void ReadPlayerLevelData()
    {
        JSONNode jd = JSONNode.Parse(GetPlayerLevelData());
        if (jd != null)
        {
            for (int i = 0; i < jd.Count; i++)
            {
                JSONNode jdItem = jd[i];
                PlayerLevelData pld = new PlayerLevelData(jdItem);
                GameManager.dicPlayerLevelDatas.Add(pld.levelId, pld);
            }
        }
    }

    public void SavePlayerLevelData()
    {
        JSONArray arr = new JSONArray();
        foreach (PlayerLevelData item in GameManager.dicPlayerLevelDatas.Values)
        {
            JSONNode jd = item.ToJsonData();
            arr.Add(jd);
        }
        PlayerPrefs.SetString(IKey.PlayerLevel, arr.ToString());
        Debug.Log("SaveData:" + arr.ToString());
    }

    public void InitUI()
    {
        gUIRoot_Main = GameObject.Find("PanelMain");
        gUIRoot_Second = GameObject.Find("PaneSecond");
        gUIRoot_Dialog = GameObject.Find("PanelDialog");
    }

    public bool GeneralShowUIMain(string path)
    {
        bool sucess = false;
        _UIMain = Tools.AddNGUIChild(gUIRoot_Main, path);
        if (_UIMain != null)
        {
            sucess = true;
        }
        return sucess;
    }

    public bool GeneralShowUISecond(string path)
    {
        bool success = false;
        _UISecond = Tools.AddNGUIChild(gUIRoot_Second, path);
        if (_UISecond != null)
        {
            success = true;
        }
        return success;
    }

    public void GeneralCloseUISecond()
    {
        if (_UISecond != null)
        {
            DestroyObject(_UISecond);
        }
    }

    public void GeneralAddBtnCloseUISecond(UIButton btn)
    {
        btn.onClick.Add(new EventDelegate(GeneralCloseUISecond));
    }
}
