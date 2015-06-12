using UnityEngine;
using System.Collections;

public class CameraControll : MonoBehaviour {

    public Transform target;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, -10f);
        }
    }

    public void StartFollow(Transform target) 
    {
        this.target = target;
    }

    public void StopFollow() 
    {
        this.target = null;
    }

    public void MoveToTarget(Transform moveTarget, float dur) 
    {
        Vector3 targetPos = new Vector3(moveTarget.position.x, moveTarget.position.y, -10f);
        iTween.MoveTo(gameObject, targetPos, dur);
    }
}
