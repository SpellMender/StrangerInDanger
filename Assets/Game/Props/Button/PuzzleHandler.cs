using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
    public Animator set1_button1;
    public Animator set1_button2;
    public Animator set1_button3;

    public Animator set2_button1;
    public Animator set2_button2;
    public Animator set2_button3;

    public Animator set3_button1;
    public Animator set3_button2;
    public Animator set3_button3;

    public Animator door1;
    public Animator door2;
    public Animator door3;

    Queue<int> sequence;
    Queue<int> tempQueue;
    int[] sequenceArray = new int[3];
    int[] correctSequence = new int[] { 2, 3, 1 };

    public AudioSource doorSound;

    // Start is called before the first frame update
    void Start()
    {
        sequence = new Queue<int>();
        tempQueue = new Queue<int>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Puzzle_ButtonPress(Animator pressed)
    {
        if (set1_button1 == pressed)
        {
            if (!door1.GetBool("isOpen"))
            {
                doorSound.Play();
            }
            door1.SetBool("isOpen", true);
            set1_button2.SetBool("isPressed", false);
            set1_button3.SetBool("isPressed", false);
        }
        if (set1_button2 == pressed)
        {
            set1_button1.SetBool("isPressed", false);
            set1_button3.SetBool("isPressed", false);
        }
        if (set1_button3 == pressed)
        {
            set1_button1.SetBool("isPressed", false);
            set1_button2.SetBool("isPressed", false);
            CheckGreen();
        }

        if (set2_button1 == pressed)
        {
            set2_button2.SetBool("isPressed", false);
            set2_button3.SetBool("isPressed", false);
            sequence.Enqueue(1);
            CheckSequence();
        }
        if (set2_button2 == pressed)
        {
            set2_button1.SetBool("isPressed", false);
            set2_button3.SetBool("isPressed", false);
            sequence.Enqueue(2);
            CheckSequence();
        }
        if (set2_button3 == pressed)
        {
            set2_button1.SetBool("isPressed", false);
            set2_button2.SetBool("isPressed", false);
            sequence.Enqueue(3);
            CheckSequence();
            CheckGreen();
        }
        if (set3_button1 == pressed)
        {
            set3_button2.SetBool("isPressed", false);
            set3_button3.SetBool("isPressed", false);
        }
        if (set3_button2 == pressed)
        {
            set3_button1.SetBool("isPressed", false);
            set3_button3.SetBool("isPressed", false);
        }
        if (set3_button3 == pressed)
        {
            set3_button1.SetBool("isPressed", false);
            set3_button2.SetBool("isPressed", false);
            CheckGreen();
        }
    }
    void CheckSequence()
    {
        while(sequence.Count > 3)
        {
            sequence.Dequeue();
        }
        print("Sequence Length: " + sequence.Count);
        if (sequence.Count == 3)
        {
            tempQueue = sequence;
            for (int i = 0; i < 3; i++)
            {
                sequenceArray[i] = tempQueue.Dequeue();
                sequence.Enqueue(sequenceArray[i]);
            }

           //DEBUG
            string sequenceString = "";
            for (int i = 0; i < sequenceArray.Length; i++)
            {
                sequenceString += sequenceArray[i];
            }
            string correctString = "";
            for (int i = 0; i < correctSequence.Length; i++)
            {
                correctString += correctSequence[i];
            }
            print("Sequence String" + sequenceString);
            print("Correct String" + correctString);
           //END DEBUG

            for (int i = 0; i < sequenceArray.Length; i++)
            {
                if(sequenceArray[i] != correctSequence[i])
                {
                    return;
                }
            }
            if (!door2.GetBool("isOpen"))
            {
                doorSound.Play();
            }
            door2.SetBool("isOpen", true);
        }
    }
    void CheckGreen()
    {
        if (set1_button3.GetBool("isPressed") && set2_button3.GetBool("isPressed") && set3_button3.GetBool("isPressed"))
        {
            if (!door3.GetBool("isOpen"))
            {
                doorSound.Play();
            }
            door3.SetBool("isOpen", true);
        }
    }
}