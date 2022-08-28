using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public Text dialogue;
    public GameObject panel;

    private void OnEnable()
    {
        EventHandler.ShowDialogue += OnShowDialogue;
        EventHandler.CloseDialogue += OnCloseDialogue;
    }
    private void OnDisable()
    {
        EventHandler.ShowDialogue -= OnShowDialogue;
        EventHandler.CloseDialogue -= OnCloseDialogue;
    }

    private void OnCloseDialogue()
    {
        panel.SetActive(false);
        this.dialogue.text = string.Empty;
    }

    private void OnShowDialogue(string dialogue)
    {
        if (dialogue != string.Empty)
        {
            this.dialogue.text = dialogue;
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
            this.dialogue.text = string.Empty;
        }
        
    }
}
