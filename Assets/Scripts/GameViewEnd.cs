using UnityEngine;
using System.Collections;

public class GameViewEnd : MonoBehaviour {

    public UILabel txtMine;
    public UILabel txtGod;

    string[] strs = new string[] 
    {
        "嘿，你来了",
        "终于找到你了...神",
        "你的勇气与坚韧值得敬佩",
        "你创造的这些奇怪的东西吗？",
        "没错,所有的一切,所有的所有",
        "那个一直在我旁边唠叨的是谁",
        "这个空间",
        "它说你快挂了",
        "是我的一部分",
        "你神格分裂了吗",
        "是的",
        "一直这样吗?",
        "从那天开始",
        "普鲁托...是怎么回事",
        "它们也是我",
        "所以是你自己摧毁了天空之城吗?",
        "普鲁托不受控制",
        "那一天到底发生了什么",
        "神失恋了",
        "...相当痛苦，不是吗",
        "软弱的神必须被杀死",
        "那么,12克心，到底是什么",
        "12克心，12字符，一直想说的话......",
        "但是已经没意义了",
        "谢谢你，陌生人"
    };

    CommonCPU comCPU;

    Player player;

    int curIndex = -1;

    bool enablenext = false;

    public bool _Enablenext
    {
        get { return enablenext; }
        set { enablenext = value;}
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Use this for initialization
    void Start()
    {

        comCPU = gameObject.AddComponent<CommonCPU>();
        comCPU.InitUI();
        player._State = EState.Idle;
        StartCoroutine(CoStartTime());
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0))
        {
            if (_Enablenext)
            {
                ShowNextTxt();
            }
        }
    }

    IEnumerator CoStartTime() 
    {
        yield return new WaitForSeconds(1.5f);
        ShowNextTxt();
    }

    void ShowNextTxt() 
    {
        _Enablenext = false;

        curIndex++;
        if (curIndex >= strs.Length)
        {
            OnShowEnd();
            return;
        }

        bool isMineWord = false;
        if ((curIndex % 2) != 0)
        {
            isMineWord = true;
        }

        if (curIndex == 22 || curIndex == 23 || curIndex == 24)
        {
            isMineWord = false;
        }

        if (isMineWord)
        {
            txtGod.text = "";
            StartCoroutine(CoSetText(txtMine, strs[curIndex]));
        }
        else
        {
            StartCoroutine(CoSetText(txtGod, strs[curIndex]));
            txtMine.text = "";
        }
    }

    IEnumerator CoSetText(UILabel txt, string str) 
    {
        int len = 1;
        while (true)
        {
            txt.text = str.Substring(0, len);
            len++;
            if (len > str.Length)
            {
                OnShowOneEnd();
                break;
            }
            yield return new WaitForSeconds(0.06f);
        }
    }

    void OnShowOneEnd() 
    {
        _Enablenext = true;
    }

    void OnShowEnd() 
    {
        Application.LoadLevel("gameover");
    }
}
