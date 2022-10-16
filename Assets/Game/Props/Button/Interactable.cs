using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Highlight Variables
    Material material;
    [SerializeField] [Range(0f, 1f)] float highlightIntensity = 0.25f;
    [SerializeField] Color highlightColor = Color.white;
    [SerializeField] float interactDistance = 5;
    Vector3 playerPosition;

    // Dialogue Variables
    public Dialogue dialogue;
    DialogueManager dialogueManager;

    // Button Variables
    [HideInInspector] public Animator animator;

    public GameObject promptText;

    // Start is called before the first frame update
    void Start()
    {
        // Acquires the material attached to the GameObject
        material = GetComponent<Renderer>().material;
        // Acquires DialogueManager object in scene
        dialogueManager = FindObjectOfType<DialogueManager>();

        animator = GetComponent<Animator>();
        promptText.SetActive(false);
    }

    // OnMouseOver is called every frame the mouse is over the collider
    private void OnMouseOver()
    {
        // Acquires position of player
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        // Highlighted if the transform is within the Interaction Distance
        if (Vector3.Distance(playerPosition, transform.position) < interactDistance)
        {
            Highlight();
            promptText.SetActive(true);
        }
        // Must be unhighlighted if we are beyond the interactDistance
        else
        {
            UnHighlight();
            promptText.SetActive(false);
        }
        // Interact when pressing E during MouseOver
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    // OnMouseOver is called when the mouse is no longer over the collider
    private void OnMouseExit()
    {
        UnHighlight();
        promptText.SetActive(false);
    }

    // Creates an emission effect from the material to highlight the object
    private void Highlight()
    {
        // Enables Emission feature of material (Disabled by default)
        material.EnableKeyword("_EMISSION");
        // Emits a white color at the given intensity
        material.SetColor("_EmissionColor", highlightColor * highlightIntensity);
    }

    // Removes the highlight effect from the material
    private void UnHighlight()
    {
        // Disables Emission feature of material
        material.DisableKeyword("_EMISSION");
    }

    // Interact behavior of this game object
    public void Interact()
    {
        GetDialogue();
        PushButton();
    }

    public void GetDialogue()
    {
        if (dialogue.sentences.Length > 0)
        {
            // Makes sure a dialogue is not currently active
            if (!dialogueManager.isActive)
            {
                // Starts this object's dialogue
                //dialogueManager.StartDialogue(dialogue);
            }
        }
    }

    public void PushButton()
    {
        if (animator != null)
        {
            if (!animator.GetBool("isPressed"))
            {
                animator.SetBool("isPressed", true);
                //puzzleCheck.Puzzle_ButtonPress(GetComponent<Animator>());
            }
        }
    }
}
