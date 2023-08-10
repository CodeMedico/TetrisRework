using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class falldebug : MonoBehaviour
{
    public float dropInterval = 0.3f;
    private float lastDropTime;
    private Vector2 screenBounds;
    private bool isFalling = true;
    bool shouldStop = false;
    public GameObject bottomObject;
    public GameObject playerBlockObject;

    void Start()
    {

    }

    void Update()
    {
        if (isFalling && Time.time - lastDropTime > dropInterval)
        {
            float distanceToBottom = Vector3.Distance(transform.position, bottomObject.transform.position);
            float distanceToPlayerBlock = Vector3.Distance(transform.position, playerBlockObject.transform.position);
            Debug.Log("Distance to bottom: " + distanceToBottom);
            if (distanceToBottom < 0.5f || distanceToPlayerBlock < 0.5f)
            {
                shouldStop = true;
            }
            // Move the object downward by one block
            if (!shouldStop)
            {
                transform.position += new Vector3(0, -1, 0);
                lastDropTime = Time.time;
            }
        }
    }


}
