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
    Dead = 3
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
        }
    }

    void Start () {



        rig = GetComponent<Rigidbody2D>();
        gh = GetComponent<GravityHander>();
        spriteRender = GetComponent<SpriteRenderer>();
      
        gameView = GameObject.FindGameObjectWithTag("CPU").GetComponent<GameView>();
        animator = gameObject.GetComponent<Animator>();

        CurColor = EColor.W;

        gobjStartPos = GameObject.FindGameObjectWithTag("startpos");
        transform.position = gobjStartPos.transform.position;

        _State = EState.Normal;
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
    }

	void Update () {

        if (_State == EState.Normal)
        {
            rig.velocity = new Vector2(speedX, rig.velocity.y);
            if (Input.GetMouseButtonDown(0))
            {
                Change();
            }
        }
        else if (_State == EState.ReadyToGo)
        {
            rig.velocity = new Vector2(0f, rig.velocity.y);
            if (Input.GetMouseButtonDown(0))
            {
                ContinueGo();
            }
        }
        else if (_State == EState.Dead)
        {
            rig.velocity = new Vector2(0f, 0f);
        }
        
	}

    void ContinueGo()
    {
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
        RaycastHit2D rh = Physics2D.Raycast(rayOri, Vector2.up, 100f, 1 << LayerMask.NameToLayer("ground"));
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
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("forceplatform"))
        {
            Cube cube = other.GetComponent<Cube>();
            if (cube != null)
            {
                Vector2 force = Vector2.zero;
                if ((cube.cubeColor == EColor.B && curColor == EColor.W) || (cube.cubeColor == EColor.W && curColor == EColor.B))
                {
                    // 异色
                    force = new Vector2(0f, other.transform.position.y - transform.position.y);
                }
                else
                {
                    // 同色
                    force = new Vector2(0f, transform.position.y - other.transform.position.y);
                }

               force.Normalize();
               
                force *= 800;

                rig.velocity = new Vector2(rig.velocity.x, 0f);

                rig.AddForce(force);
            }
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
    }

    IEnumerator CoGetHeart(GameObject gobjHeart) 
    {
        int heartid = int.Parse(gobjHeart.name);
        gameView.GetAHeart(heartid);
        DestroyObject(gobjHeart);
        GameObject gobjEff = Tools.LoadResourcesGameObject("Prefabs/eff_get_heart");
        gobjEff.transform.position = gobjHeart.transform.position;
        yield return new WaitForSeconds(0.15f);
        DestroyObject(gobjEff);
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

        AudioManager._Instance.PauseSound("bgm");

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

        AudioManager._Instance.ContineSound("bgm");

        yield return 0;
    }

    int GetGravityDir()
    {
        return gh.GravityDir;
    }
}
