using UnityEngine;
using System.Collections;

public class GravityHander : MonoBehaviour {

    private int gravityDir;

    float scaleY;

    public int GravityDir
    {
        get 
        { 
            return gravityDir; 
        }
        set 
        {
            if (gravityDir != value)
            {
                Vector3 scale = transform.localScale;
                if (value == 1)
                {
                    scale.y = -1 * scaleY;
                }
                else
                {
                    scale.y = scaleY;
                }
                transform.localScale = scale;
            }
            gravityDir = value;
            
           
        }
    }
    // Use this for initialization
	void Start () {
        scaleY = transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    void FixedUpdate(){
        rigidbody2D.AddForce(new Vector2(0f, 40f * gravityDir));
    }
}
