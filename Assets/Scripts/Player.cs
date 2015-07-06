using UnityEngine;
using System.Collections;

public enum EColor
{
    B = 1,
    W = -1
}

public enum EState
{
    Normal = 1,
    ReadyToGo = 2,
    Dead = 3,
    Idle = 4
}

public enum EPowState
{
    Normal = 1,
    TimeSlow = 2
}

public class Player : MonoBehaviour {

    public Vector2 waterJumpForce = new Vector2(0f, 800f);
    public float waterDrag = 6f;
    public float speedX = 8f;
    private bool grounded = false;			// Whether or not the player is grounded.
    Rigidbody2D rig;
    bool canWaterJump = false;
    //public float acc;  //   加速度
    //public float maxSpeedX;
    //public float minSpeedX;

    bool inBubble = false;

    public bool _InBubble
    {
        get { return inBubble; }
        set { inBubble = value;}
    }

    GameObject gobjCurBubble;

    Cube gCurInCube; // 当前处于某个Cube中

    public Cube _CurInCube
    {
        get { return gCurInCube; }
        set 
        {
            gCurInCube = value;
        }
    }

    SpriteRenderer spriteRender;

    EColor curColor = EColor.B;

    GameObject gobjCheckPoint;
    Animator animator;


    GameObject gobjStartPos;

    public bool stop = false;
    Vector3 lastPos = Vector3.zero;

    public EColor CurColor
    {
        get { return curColor; }
        set 
        { 
            curColor = value;
            if (value == EColor.B)
            {
                //spriteRender.color = Color.black;
                animator.SetInteger("color", 1);
            }
            else 
            {
                //spriteRender.color = Color.white;
                animator.SetInteger("color", -1);
            }
        }
    }

    GravityHander gh;

    GameView gameView;

    EState state;

    public EState _State
    {
        get { return state; }
        set 
        {
            state = value;
            if (value == EState.ReadyToGo)
            {
                if (CurColor == EColor.W)
                {
                    animator.Play("blink_white");
                }
                else if(CurColor == EColor.B)
                {
                    animator.Play("blink");
                }
            }
            else if(value == EState.Normal)
            {
                if (CurColor == EColor.W)
                {
                    animator.Play("run_white");
                }
                else if (CurColor == EColor.B)
                {
                    animator.Play("run_black");
                }
            }
            else if (value == EState.Idle)
            {
                rig.velocity = new Vector2(0f, 0f);
                if (CurColor == EColor.W)
                {
                    animator.Play("blink_white");
                }
                else if (CurColor == EColor.B)
                {
                    animator.Play("blink");
                }
            }
        }
    }

    EPowState powState = EPowState.Normal;

    public EPowState _PowState
    {
        get { return powState; }
        set 
        { 
            powState = value;
            if (powState == EPowState.Normal)
            {
                Time.timeScale = 1f;
                gameView.SetTimeSlowEffShow(false);
            }
            else if (powState == EPowState.TimeSlow)
            {
                Time.timeScale = 0.5f;
                gameView.SetTimeSlowEffShow(true);
            }
        }
    }

    void Awake() 
    {
        rig = GetComponent<Rigidbody2D>();
        gh = GetComponent<GravityHander>();
        spriteRender = GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        CurColor = EColor.W;
    }

    void Start () {
      
        gameView = GameObject.FindGameObjectWithTag("CPU").GetComponent<GameView>();

        gobjStartPos = GameObject.FindGameObjectWithTag("startpos");
        transform.position = gobjStartPos.transform.position;
    }
	
	// Update is called once per frame

    public void Change()
    {
        if (CurColor == EColor.W)
        {
            CurColor = EColor.B;
        }
        else
        {
            CurColor = EColor.W;
        }

        if (canWaterJump)
        {
            rig.drag = 0f;
            canWaterJump = false;
            rig.AddForce(waterJumpForce);
        }

        if (_InBubble)
        {
            Vector2 forceBubble = new Vector2(0f, -1 * gh.GravityDir * 1200f);
            rig.velocity = new Vector2(rig.velocity.x, 0f);
            rig.AddForce(forceBubble);
            gobjCurBubble.SetActive(false);
            _InBubble = false;
        }

        if (_CurInCube != null)
        {
            //HanderACube(_CurInCube);
        }
    }

	void Update () {

        if (_State == EState.Normal)
        {
            rig.velocity = new Vector2(speedX, rig.velocity.y);
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X))
            {
                Change();
            }
        }
        else if (_State == EState.ReadyToGo)
        {
            rig.velocity = new Vector2(0f, rig.velocity.y);
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X))
            {
                ContinueGo();
            }
        }
        else if (_State == EState.Dead)
        {
            rig.velocity = new Vector2(0f, 0f);
        }
        else if (_State == EState.Idle)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X))
            {
                Change();
            }
        }
        
	}

    void ContinueGo()
    {
        if (speedX < 0)
        {
            ChangeDirAtOnce();
        }
        _State = EState.Normal;
        if (gobjCheckPoint != null)
        {
            // 显示提示
            gameView.HideContineGoTip(gobjCheckPoint);
        }
        else
        {
            // 显示提示
            gameView.HideContineGoTip(gobjStartPos);
        }
    }

    void FixedUpdate()
    {
        Vector2 rayOri = new Vector2(transform.position.x, -50f);
        RaycastHit2D rh = Physics2D.Raycast(rayOri, Vector2.up, 1000f, 1 << LayerMask.NameToLayer("ground"));
        if (rh)
        {
            if (rh.point.y > transform.position.y)
            {
                gh.GravityDir = 1;
            }
            else 
            {
                gh.GravityDir = -1;
            }
        }
    }
    
    void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("wind"))
        {
            Cube cube = other.GetComponent<Cube>();
            if (cube != null)
            {
                _CurInCube = cube;
                Vector2 force = Vector2.zero;
                float forceVal = 80f - Mathf.Abs(transform.position.y - other.transform.position.y) * 0.3f;
                if (forceVal < 0)
                {
                    forceVal = 0;
                }

                if ((cube.cubeColor == EColor.B && curColor == EColor.W) || (cube.cubeColor == EColor.W && curColor == EColor.B))
                {
                    // 异色
                    if (transform.position.y - other.transform.position.y > 0)
                    {
                        force = new Vector2(0f, forceVal);
                    }
                    else 
                    {
                        force = new Vector2(0f, -1 * forceVal);
                    }
                }
                else
                {
                    // 同色
                    if (transform.position.y - other.transform.position.y > 0)
                    {
                        force = new Vector2(0f, -1 * forceVal);
                    }
                    else
                    {
                        force = new Vector2(0f, forceVal);
                    }
                }
                

                rig.AddForce(force);
            }
        }
       
    }

    /// <summary>
    /// 对一个方块起反应
    /// </summary>
    void HanderACube(Cube cube) 
    {
        if (cube == null)
        {
            return;
        }

        Vector2 force = Vector2.zero;
        if ((cube.cubeColor == EColor.B && curColor == EColor.W) || (cube.cubeColor == EColor.W && curColor == EColor.B))
        {
            // 异色
            force = new Vector2(0f, cube.transform.position.y - transform.position.y);
        }
        else
        {
            // 同色
            force = new Vector2(0f, transform.position.y - cube.transform.position.y);
        }

        force.Normalize();

        force *= 800;

        rig.velocity = new Vector2(rig.velocity.x, 0f);

        rig.AddForce(force);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("forceplatform"))
        {
            Cube cube = other.GetComponent<Cube>();
            _CurInCube = cube;
            HanderACube(cube);
        }
        else if (other.CompareTag("dangerous"))
        {
            if (_State == EState.Normal)
            {
                KillPlayer();
            }
            
        }
        else if (other.CompareTag("water"))
        {
            rig.drag = waterDrag;
            canWaterJump = true;
        }
        else if (other.CompareTag("bubble"))
        {
            _InBubble = true;
            gobjCurBubble = other.gameObject;
        }
        else if (other.CompareTag("trapdoor"))
        {
            OnTriTrapDoor(other.gameObject);
        }
        else if (other.CompareTag("heart"))
        {
            StartCoroutine(CoGetHeart(other.gameObject));
        }
        else if (other.CompareTag("checkpoint"))
        {
            gobjCheckPoint = other.gameObject;
            Animator anim = gobjCheckPoint.GetComponent<Animator>();
            anim.Play("act");
        }
        else if (other.CompareTag("endpos"))
        {
            StartCoroutine(gameView.CoOnTouchEndPos(other.gameObject));
        }
        else if (other.CompareTag("trigger"))
        {
            ITrigger tri = other.GetComponent<ITrigger>();
            tri.OnTri();
        }
        else if (other.CompareTag("change_dir"))
        {
            StartCoroutine(CoChangeDir());
        }
        else if (other.CompareTag("change_dir_add_force"))
        {
            ChangeDirAtOnce();
            Vector2 forceBubble = new Vector2(0f, -1 * gh.GravityDir * 1200f);
            rig.velocity = new Vector2(rig.velocity.x, 0f);
            rig.AddForce(forceBubble);
        }
        else if (other.CompareTag("add_force"))
        {
            Vector2 forceBubble = new Vector2(0f, -1 * gh.GravityDir * 2400f);
            rig.velocity = new Vector2(rig.velocity.x, 0f);
            rig.AddForce(forceBubble);
        }
    }

    /// <summary>
    /// 改变横轴方向
    /// </summary>
    /// <returns></returns>
    IEnumerator CoChangeDir() 
    {
        ChangeDirAtOnce();
        yield return 0;
    }

    void ChangeDirAtOnce() 
    {
        speedX *= -1;
        if (speedX < 0)
        {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    IEnumerator CoGetHeart(GameObject gobjHeart) 
    {
        int heartid = int.Parse(gobjHeart.name);
        gameView.GetAHeart(heartid);
        DestroyObject(gobjHeart);
        GameObject gobjEff = Tools.LoadResourcesGameObject("Prefabs/eff_get_heart");
        gobjEff.transform.position = gobjHeart.transform.position;
        GobjLife gl = gobjEff.AddComponent<GobjLife>();
        gl.lifeTime = 0.15f;
        if (GameManager._CurHearts == 9 && heartid == 3)
        {
            gameView.SaveRecord();

            // 集齐12颗心
            AudioManager._Instance.StopSound("bgm");
            _State = EState.Idle;
            gameView.cameraControll.StopFollow();
            iTween.ShakePosition(gameView.cameraControll.gameObject, new Vector3(0.3f, 0.3f, 0f), 2.5f);
            yield return new WaitForSeconds(2.5f);
            TweenFOV tf = gameView.cameraControll.gameObject.AddComponent<TweenFOV>();
            tf.from = 9f;
            tf.to = 0.1f;
            tf.duration = 0.5f;
            tf.camera2d = true;
            tf.Play();
            yield return new WaitForSeconds(0.5f);
            Application.LoadLevel("level_end");
            
        }
        yield return 0;
    }

    /// <summary>
    /// 触发传送门
    /// </summary>
    /// <param name="trapdoor"></param>
    void OnTriTrapDoor(GameObject trapdoor) 
    {
        Trapdoor td = trapdoor.GetComponent<Trapdoor>();
        if ((int)CurColor * (int)td.colorType < 0)
        {
            Animator animTd = trapdoor.GetComponent<Animator>();
            animTd.SetTrigger("act");

            Transform otherDoor = td.tfOtherDoor;
            if (otherDoor != null)
            {
                transform.position = otherDoor.position;
                Animator animOtherDoor = otherDoor.GetComponent<Animator>();
                animOtherDoor.SetTrigger("act");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("water"))
        {
            rig.drag = 0f;
            canWaterJump = false;
        }
        else if (other.CompareTag("bubble"))
        {
            _InBubble = false;
        }
        else if (other.CompareTag("forceplatform"))
        {
            // 离开一个方块
            _CurInCube = null;
        }
    }

    void KillPlayer()
    {
        canWaterJump = false;
        rig.drag = 0f;
        rig.velocity = new Vector2(rig.velocity.x, 0f);
        _InBubble = false;

        GameObject gobjBubbleRoot = GameObject.Find("bubble_root");
        if (gobjBubbleRoot != null)
        {
            foreach (Transform tfItem in gobjBubbleRoot.transform)
            {
                tfItem.gameObject.SetActive(true);
            }
        }

        StartCoroutine(CoOnPlayerDead());
    }

    IEnumerator CoOnPlayerDead() 
    {
        _State = EState.Dead;
        gameView.cameraControll.StopFollow();

        //AudioManager._Instance.PauseSound("bgm");

        // 死亡动画
        iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0f), 0.4f);
        yield return new WaitForSeconds(0.5f);
        renderer.enabled = false;
        // 特效
        GameObject gobjDeadEff = Tools.LoadResourcesGameObject("Prefabs/eff_dead");
        SpriteRenderer srDeadEff = Tools.GetComponentInChildByPath<SpriteRenderer>(gobjDeadEff, "1");
        srDeadEff.color = (curColor == EColor.W ? Color.white : Color.black);
        gobjDeadEff.transform.position = transform.position;
        yield return new WaitForSeconds(0.15f);
        DestroyObject(gobjDeadEff);

        yield return new WaitForSeconds(0.1f);

        GameObject gobjRevive = null;
        if (gobjCheckPoint != null)
        {
            gobjRevive = gobjCheckPoint;
        }
        else
        {
            gobjRevive = gobjStartPos;
        }
        
        // 相机移动到复活点
        gameView.cameraControll.MoveToTarget(gobjRevive.transform, 0.8f);

        yield return new WaitForSeconds(0.8f);

        renderer.enabled = true;
        gameView.cameraControll.StartFollow(transform);

        if (gobjCheckPoint != null)
        {
            transform.position = gobjCheckPoint.transform.position;
            // 显示提示
            gameView.ShowContinueGoTip(gobjCheckPoint);
        }
        else
        {
            transform.position = gobjStartPos.transform.position;
            // 显示提示
            gameView.ShowContinueGoTip(gobjStartPos);
        }

        _State = EState.ReadyToGo;

        //AudioManager._Instance.ContineSound("bgm");

        yield return 0;
    }

    int GetGravityDir()
    {
        return gh.GravityDir;
    }
}
