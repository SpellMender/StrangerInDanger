using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ghost : MonoBehaviour
{
    public float detectionDistance = 10;
    public float followOffset = 0.5f; [Tooltip("Without this the follower hugs the player non-consensually")]
    public float speed = 5f;

    public Player playerScript;
    Transform player;
    bool sounded;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanFollow()) FollowPlayer();
        CheckForSound();

        TouchPlayer();
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

    void TouchPlayer()
    {
        if (GetPlayerDistance() < 1f)
        {
            SceneManager.LoadScene(0);
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
