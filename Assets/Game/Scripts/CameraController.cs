using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    private Player player;

    public float minDistanceFromPlayer;
    private Transform originalParent;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        originalParent = transform.parent;
    }

    void Update()
    {
        if (transform.position.y > player.transform.position.y)
            return;

        if (Vector2.Distance(new Vector2(0, player.transform.position.y), new Vector2(0, transform.position.y)) > minDistanceFromPlayer)
        {
            transform.parent = player.transform;
        }
        else if (transform.parent != originalParent)
        {
            transform.parent = originalParent;
        }
    }
	
}
