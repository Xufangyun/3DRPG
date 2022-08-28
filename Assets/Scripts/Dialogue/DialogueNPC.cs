using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour
{
    public DialogueData_SO dialogueEmpty;

    public DialogueData_SO dialogueFinish;

    private bool isCanTalk;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (isCanTalk&&Input.GetKeyDown(KeyCode.R))
        {
            DialogController.Instance.ShowDialogueEmpty();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        DialogController.Instance.RigistDialogueNPC(this);
        if (other.CompareTag("Player"))
        {
            isCanTalk = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCanTalk = false;
        }
        EventHandler.CallCloseDialogue();
    }
}
