using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowPlayer : MonoBehaviour
{
    public GameObject followTarget;
    public float moveSpeed;
    private Vector3 targetPos;
    private bool cameraExists;

    
    void start()
    {
        if (!cameraExists)
        {
            cameraExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        targetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, -10);
        transform.position = Vector3.Lerp(transform.position, targetPos, 5);
    }
}
