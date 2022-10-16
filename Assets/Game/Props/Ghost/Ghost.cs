using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float detectionDistance = 10;
    public float followOffset = 0.5f; [Tooltip("Without this the follower hugs the player non-consensually")]
    public float speed = 5f;

    Transform player;
    bool sounded;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanFollow()) FollowPlayer();
        CheckForSound();
    }

    bool CanFollow()
    {
        return GetPlayerDistance() < detectionDistance && GetPlayerDistance() > followOffset;
    }

    void CheckForSound()
    {
        if (CanFollow() && !sounded)
        {
            gameObject.GetComponent<AudioSource>().Play();
            sounded = true;
            print("Sounded:" + sounded);
        }
        else if (!CanFollow() && sounded)
        {
            sounded = false;
            print("Sounded:" + sounded);
        }
    }

    float GetPlayerDistance()
    {
        return Vector3.Distance(transform.localPosition, player.localPosition);
    }

    void FollowPlayer()
    {
        FacePlayer();
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void FacePlayer()
    {
        transform.LookAt(player);
    }
}
