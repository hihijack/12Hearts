using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameView : MonoBehaviour {

    public AudioClip bgm;

    public CameraControll cameraControll;

    List<int> heartCurThisLevel = new List<int>();
    CommonCPU comCPU;

    Player player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
       
    }

	// Use this for initialization
	void Start () {
        player._State = EState.Normal;
        comCPU = gameObject.AddComponent<CommonCPU>();
        comCPU.InitUI();
        InitCurHeartList();

        ShowUIMain();

        if (bgm != null)
        {
            AudioManager._Instance.PlaySound(bgm, true, "bgm");
        }
	}

    void InitCurHeartList()
    {
        PlayerLevelData pld = GameManager.GetPlayerLevelData(GameManager.curLevelId);
        GameObject gobjHeartsRoot = GameObject.Find("hearts");
        if (pld != null)
        {
            for (int i = 0; i < pld.hearts.Length; i++)
            {
                int heartId = pld.hearts[i];
                heartCurThisLevel.Add(heartId);
                // 移除已获得的心
                GameObject gobjHeart = Tools.GetGameObjectInChildByPathSimple(gobjHeartsRoot, heartId.ToString());
                DestroyObject(gobjHeart);
            }
        }

    }

	// Update is called once per frame
	void Update ()
    {
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //if (player._PowState == EPowState.Normal)
            //{
            //    player._PowState = EPowState.TimeSlow;
            //}
            //else if (player._PowState == EPowState.TimeSlow)
            //{
            //    player._PowState = EPowState.Normal;
            //}
        }
#endif
    }

    public void SetTimeSlowEffShow(bool isShow) 
    {
        GameObject gobjTimeSlowEff = Tools.GetGameObjectInChildByPathSimple(cameraControll.gameObject, "eff_timeslow");
        if (gobjTimeSlowEff != null)
        {
            gobjTimeSlowEff.SetActive(isShow);
        }
        else
        {
            if (isShow)
            {
                gobjTimeSlowEff = Tools.LoadResourcesGameObject("Prefabs/eff_timeslow", cameraControll.gameObject, 0f, 0f, 10f);
                gobjTimeSlowEff.name = "eff_timeslow";
            }
        }
    }

    void ShowUIMain()
    {
        comCPU.GeneralShowUIMain("Prefabs/UI/ui_level_main");
        for (int i = 0; i < GameManager._MaxHearts; i++)
        {
            GameObject gobjHeartsGrid = Tools.GetGameObjectInChildByPathSimple(comCPU._UIMain, "grid_hearts");
            GameObject gobjHeart = Tools.AddNGUIChild(gobjHeartsGrid, "Prefabs/UI/item_heart");
            gobjHeart.name = i.ToString();
            UISprite spriteheart = gobjHeart.GetComponent<UISprite>();
            if (GameManager._CurHearts <= i)
            {
                spriteheart.color = Color.black;
            }

            UIGrid uiGrid = gobjHeartsGrid.GetComponent<UIGrid>();
            uiGrid.Reposition();
        }
        // 关卡名字
        LevelData ld = GameResources.GetLevelData(GameManager.curLevelId);
        UILabel txtname = Tools.GetComponentInChildByPath<UILabel>(comCPU._UIMain, "name");
        txtname.text = ld.name;
    }

    public void GetAHeart(int heartid)
    {
        heartCurThisLevel.Add(heartid);
        GameObject gobjHeartsGrid = Tools.GetGameObjectInChildByPathSimple(comCPU._UIMain, "grid_hearts");
        GameObject heartItem = Tools.GetGameObjectInChildByPathSimple(gobjHeartsGrid, (GameManager._CurHearts + heartCurThisLevel.Count - 1).ToString());
        if (heartItem != null)
        {
            UISprite spriteheart = heartItem.GetComponent<UISprite>();
            spriteheart.color = Color.white;
        }
    }

    /// <summary>
    /// 当抵达终点
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoOnTouchEndPos(GameObject gobjEndPos)
    {
        //淡出音乐
        AudioManager._Instance.FadeOutSound("bgm", 0.8f);
        // 传送门旋转
        AutoRotate ar = gobjEndPos.AddComponent<AutoRotate>();
        ar.speed = 5f;

        GameObject gobjPlayer = player.gameObject;
        //DestroyObject(player);
        //Rigidbody2D rig2D = gobjPlayer.GetComponent<Rigidbody2D>();
        //DestroyObject(rig2D);
        //GravityHander gh = gobjPlayer.GetComponent<GravityHander>();
        //DestroyObject(gh);
        //BoxCollider2D bc2D = gobjPlayer.GetComponent<BoxCollider2D>();
        //DestroyObject(bc2D);
        //yield return 0;
        // 玩家旋转
        AutoRotate arPlayer = gobjPlayer.AddComponent<AutoRotate>();
        arPlayer.speed = 5f;
        iTween.ScaleTo(gobjPlayer, new Vector3(0.1f, 0.1f, 1), 0.8f);
        iTween.MoveTo(gobjPlayer, gobjEndPos.transform.position, 0.8f);
        yield return new WaitForSeconds(0.8f);
        OnCompleteLevel();
    }

    /// <summary>
    /// 完成关卡
    /// </summary>
    public void OnCompleteLevel()
    {

        SaveRecord();

        GameManager.curLevelId++;

        if (GameManager.curLevelId <= GameManager.maxLevelCount)
        {
            Application.LoadLevel("open");
        }
        else
        {
            Application.LoadLevel("gameover");
        }
    }

    public void SaveRecord() 
    {
        if (GameManager.dicPlayerLevelDatas.ContainsKey(GameManager.curLevelId))
        {
            PlayerLevelData pld = GameManager.dicPlayerLevelDatas[GameManager.curLevelId];
            pld.hearts = heartCurThisLevel.ToArray();
        }
        else
        {
            PlayerLevelData pld = new PlayerLevelData(GameManager.curLevelId, heartCurThisLevel.ToArray());
            GameManager.dicPlayerLevelDatas.Add(pld.levelId, pld);
        }
        comCPU.SavePlayerLevelData();
    }


    public void ShowContinueGoTip(GameObject gobj)
    {
        GameObject gobjTip = Tools.GetGameObjectInChildByPathSimple(gobj, "continue_tip");
        gobjTip.SetActive(true);
        BlinkGobj bg = gobjTip.GetComponent<BlinkGobj>();
        if (bg == null)
        {
            bg = gobjTip.AddComponent<BlinkGobj>();
        }
        bg.StartBlink();
    }

    public void HideContineGoTip(GameObject gobj) 
    {
        GameObject gobjTip = Tools.GetGameObjectInChildByPathSimple(gobj, "continue_tip");
        gobjTip.SetActive(false);
    }
}
