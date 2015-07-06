using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GameViewMenu : MonoBehaviour {

    public UIButton btnPlay;
    CommonCPU comCPU;

    int curSelectLevelId = 1;
    GameObject gobjMapSelect;

    public Animator animPlayer;

    public AudioClip bgm;

    // Use this for initialization
	void Start () {
        comCPU.InitUI();
        btnPlay.onClick.Add(new EventDelegate(Btn_Player));
        InitGame();

        //animPlayer.Play("blink");

        if (bgm != null)
        {
            AudioManager._Instance.PlaySound(bgm, true, "bgm");
        }

        StartCoroutine(CoAnimBG());
	}

    void Awake()
    {
        comCPU = GetComponent<CommonCPU>();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //PlayerPrefs.DeleteAll();
        }
	}

    void InitGame()
    {
        if (!GameManager.hasInit)
        {
            GameResources.InitLevelData();
            comCPU.ReadPlayerLevelData();
            GameManager.hasInit = true;
        }
    }

    void Btn_Player()
    {
        ShowUIMap();
    }

    void ShowUIMap()
    {
        if (comCPU.GeneralShowUISecond("Prefabs/UI/ui_map"))
        {
            GameObject uiMap = comCPU._UISecond;

            int maxHearts = 0;
            int curHearts = 0;

            gobjMapSelect = Tools.GetGameObjectInChildByPathSimple(comCPU._UISecond, "select");

            foreach (LevelData levelData in GameResources.dicLevelDatas.Values)
            {
                int levelId = levelData.id;
                int hearts = levelData.hearts;
                GameObject gobjLevelItem = Tools.GetGameObjectInChildByPathSimple(uiMap, "levels/item_level_" + levelId);
                UISprite icon = Tools.GetComponentInChildByPath<UISprite>(gobjLevelItem, "icon");
                icon.color = Color.gray;
                UILabel txtHearts = Tools.GetComponentInChildByPath<UILabel>(gobjLevelItem, "txt_heart_nums");
                txtHearts.text = "0/" + hearts;

                UIItemLevel uil = new UIItemLevel();
                uil.levelData = levelData;
                uil.active = false;
                DataCache dc = gobjLevelItem.AddComponent<DataCache>();
                dc.data = uil;

                maxHearts += hearts;
            }
            
            if (GameManager.dicPlayerLevelDatas.Count > 0)
            {
                int maxLevelId = 0;
                foreach (PlayerLevelData  pld in GameManager.dicPlayerLevelDatas.Values)
                {
                    int levelId = pld.levelId;
                    int curHeart = pld.hearts.Length;

                    if (levelId > maxLevelId)
                    {
                        maxLevelId = levelId;
                    }

                    GameObject gobjLevelItem = Tools.GetGameObjectInChildByPathSimple(uiMap, "levels/item_level_" + levelId);
                    DataCache dc = gobjLevelItem.GetComponent<DataCache>();
                    UIItemLevel uil = dc.data as UIItemLevel;

                    UISprite icon = Tools.GetComponentInChildByPath<UISprite>(gobjLevelItem, "icon");
                    icon.color = Color.yellow;
                    UILabel txtHearts = Tools.GetComponentInChildByPath<UILabel>(gobjLevelItem, "txt_heart_nums");
                    txtHearts.text = curHeart + "/" + uil.levelData.hearts;
                    uil.active = true;

                    curHearts += curHeart;
                }

                // 开启下一关
                int nextLevelId = maxLevelId + 1;
                if (nextLevelId <= GameManager.maxLevelCount)
                {
                    GameObject gobjLevelItemNext = Tools.GetGameObjectInChildByPathSimple(uiMap, "levels/item_level_" + nextLevelId);
                    DataCache dcNext = gobjLevelItemNext.GetComponent<DataCache>();
                    UIItemLevel uilNext = dcNext.data as UIItemLevel;

                    UISprite iconNext = Tools.GetComponentInChildByPath<UISprite>(gobjLevelItemNext, "icon");
                    iconNext.color = Color.yellow;
                    UILabel txtHeartsNext = Tools.GetComponentInChildByPath<UILabel>(gobjLevelItemNext, "txt_heart_nums");
                    txtHeartsNext.text = 0 + "/" + uilNext.levelData.hearts;
                    uilNext.active = true;
                }
            }
            else
            {
                GameObject gobjLevelItem = Tools.GetGameObjectInChildByPathSimple(uiMap, "levels/item_level_1");
                DataCache dc = gobjLevelItem.GetComponent<DataCache>();
                UIItemLevel uil = dc.data as UIItemLevel;

                UISprite icon = Tools.GetComponentInChildByPath<UISprite>(gobjLevelItem, "icon");
                icon.color = Color.yellow;
                UILabel txtHearts = Tools.GetComponentInChildByPath<UILabel>(gobjLevelItem, "txt_heart_nums");
                txtHearts.text = 0 + "/" + uil.levelData.hearts;
                uil.active = true;
            }

            UIButton btnClose = Tools.GetComponentInChildByPath<UIButton>(uiMap, "btn_close");
            comCPU.GeneralAddBtnCloseUISecond(btnClose);
            UIButton btnNext = Tools.GetComponentInChildByPath<UIButton>(uiMap, "btn_next");
            btnNext.onClick.Add(new EventDelegate(BtnClick_NextLevel));
            UIButton btnPre = Tools.GetComponentInChildByPath<UIButton>(uiMap, "btn_pre");
            btnPre.onClick.Add(new EventDelegate(BtnClick_Pre));

            UIButton btnTranslate = Tools.GetComponentInChildByPath<UIButton>(uiMap, "btn_translate");
            btnTranslate.onClick.Add(new EventDelegate(BtnClick_Translate));

            GameManager._MaxHearts = maxHearts;

            // 选择第一关
            LevelData ldFirst = GameResources.GetLevelData(1);
            UILabel txtName = Tools.GetComponentInChildByPath<UILabel>(gobjMapSelect, "name");
            txtName.text = ldFirst.name;
        }
    }

    void BtnClick_NextLevel()
    {
        curSelectLevelId++;
        if (curSelectLevelId > GameResources.dicLevelDatas.Count)
        {
            curSelectLevelId = 1;
        }
        GameObject curlevelItem = Tools.GetGameObjectInChildByPathSimple(comCPU._UISecond, "levels/item_level_" + curSelectLevelId);

        DataCache dc = curlevelItem.GetComponent<DataCache>();
        UIItemLevel uil = dc.data as UIItemLevel;
        
        gobjMapSelect.transform.parent = curlevelItem.transform;
        gobjMapSelect.transform.localPosition = Vector3.zero;

        UILabel txtName = Tools.GetComponentInChildByPath<UILabel>(gobjMapSelect, "name");
        if (uil.active)
        {
            txtName.text = uil.levelData.name;
        }
        else
        {
            txtName.text = "???";
        }
    }

    void BtnClick_Pre()
    {
        curSelectLevelId--;
        if (curSelectLevelId < 1)
        {
            curSelectLevelId = GameResources.dicLevelDatas.Count;
        }
        GameObject curlevelItem = Tools.GetGameObjectInChildByPathSimple(comCPU._UISecond, "levels/item_level_" + curSelectLevelId);
        DataCache dc = curlevelItem.GetComponent<DataCache>();
        UIItemLevel uil = dc.data as UIItemLevel;

        gobjMapSelect.transform.parent = curlevelItem.transform;
        gobjMapSelect.transform.localPosition = Vector3.zero;
        
        UILabel txtName = Tools.GetComponentInChildByPath<UILabel>(gobjMapSelect, "name");
        if (uil.active)
        {
            txtName.text = uil.levelData.name;
        }
        else
        {
            txtName.text = "???";
        }
    }

    void BtnClick_Translate()
    {
        GameObject curlevelItem = Tools.GetGameObjectInChildByPathSimple(comCPU._UISecond, "levels/item_level_" + curSelectLevelId);

        DataCache dc = curlevelItem.GetComponent<DataCache>();
        UIItemLevel uil = dc.data as UIItemLevel;
        if (uil.active)
        {
            GameManager.curLevelId = uil.levelData.id;
            Application.LoadLevel("open");
        }
    }

    IEnumerator CoAnimBG()
    {
        while (true)
        {
            GameObject gobjcube = Tools.LoadResourcesGameObject("Prefabs/menu_cube");
            gobjcube.transform.position = new Vector3(10f, Random.Range(-5f, 5f), 0f);
            // 随机大小，颜色
            gobjcube.transform.localScale = new Vector3(Random.Range(10f, 30f), Random.Range(5f, 15f), 1f);
            SpriteRenderer sr = gobjcube.transform.GetComponent<SpriteRenderer>();
            sr.color = Tools.GetRandomColor();

            TweenPosition tp = gobjcube.AddComponent<TweenPosition>();
            tp.from = gobjcube.transform.position;
            tp.to = new Vector3(-30f, tp.from.y, tp.from.z);
            tp.duration = 2.1f;
            tp.PlayForward();

            GobjLife gl = gobjcube.AddComponent<GobjLife>();
            gl.lifeTime = 3f;

            yield return new WaitForSeconds(0.2f);
        }
        yield return 0;
    }
}
