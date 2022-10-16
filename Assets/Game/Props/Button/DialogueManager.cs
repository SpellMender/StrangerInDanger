using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Text nameText;
    [SerializeField] Text dialogueText;
    [SerializeField] int speedDelay = 1;

    Queue<string> sentences;
    Coroutine typeSentence;
    bool isTyping = false;
    string sentence;

    [HideInInspector] public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        dialogueBox.SetActive(isActive);
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if dialogue box is active and "E" is pressed
        //  - Occurs on the same frame as "StartDialogue"
        //  - Therefore setting active with "E" triggers "StepThroughDialogue"
        //  - This separation allows dialogueManager to function by itself once triggered
        if (isActive && Input.GetKeyDown(KeyCode.E))
        {
            StepThroughDialogue();
        }
        // Updates the active status of the dialogue box every frame
        dialogueBox.SetActive(isActive);
    }

    // Queues dialogue data for processing
    public void StartDialogue(Dialogue dialogue)
    {
        // Sets the nameText of the attached UI element
        nameText.text = dialogue.name;

        // Clears sentences in the dialogue manager queue
        sentences.Clear();

        // Assigns sentences to the dialogue manager queue
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        // Dialogue box is ready to be shown (referenced during Update())
        isActive = true;
    }

    // Activates the next step in the dialogue
    void StepThroughDialogue()
    {
        // Displays whole sentence if still typing
        if (isTyping)
        {
            // Stops typing the sentence
            StopCoroutine(typeSentence);
            isTyping = false;
            // Shows the whole sentence
            dialogueText.text = sentence;
        }
        // If not typing check for a new sentence
        else if (sentences.Count > 0)
        {
            // Pulls the current sentence from the queue (removing it)
            sentence = sentences.Dequeue();
            // Begins typing out the new sentence as a Coroutine
            typeSentence = StartCoroutine(TypeSentence(sentence));
        }
        // Deactivates the dialogue if the queue is empty
        else
        {
            isActive = false;
        }
    }

    // Types each letter of a sentence (used as a coroutine)
    IEnumerator TypeSentence(string sentence)
    {
        // We have begun typing out the sentence
        isTyping = true;
        // Start with an empty string
        dialogueText.text = "";
        // Evaluates the sentence as an array of characters
        foreach (char letter in sentence.ToCharArray())
        {
            // Adds one letter at a time to the dialogueText UI element
            dialogueText.text += letter;

            // Skips a number of frames equal to the speedDelay
            for (int i = 0; i < speedDelay; i++)
            {
                // Skips a frame before proceeding
                yield return null;
            }
        }
        // We are done typing out the sentence
        isTyping = false;
    }
}
