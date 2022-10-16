using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StrangerDanger : MonoBehaviour
{
    Interactable interactable;
    bool isDanger = false;

    public GameObject stranger;
    public GameObject danger;

    Player player;

    float waitTime = 250f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable.dialogueManager.isActive && !isDanger)
        {
            isDanger = true;
            player.disabled = true;
        }
        if (isDanger && !interactable.dialogueManager.isActive)
        {
            gameObject.GetComponent<AudioSource>().Play();
            stranger.SetActive(false);
            danger.SetActive(true);
            isDanger = false;
        }
        if (danger.activeSelf)
        {
            SceneChange();
        }
        
    }

    void SceneChange()
    {
        if (waitTime <= 0)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            waitTime -= 1;
        }
    }
}
