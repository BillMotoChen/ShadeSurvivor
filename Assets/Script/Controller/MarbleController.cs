using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleController : MonoBehaviour
{
    private GameObject player;
    private float destroyDistance = 30f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToTarget = Vector3.Distance(this.transform.position, player.transform.position);
            if (distanceToTarget > destroyDistance)
            {
                Destroy(this.gameObject);
            }

        }        
    }
}
