using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : Singleton<DialogController>
{
    private DialogueNPC curDialogueNPC;

    private Stack<string> dialogueEmptyStack;

    private Stack<string> dialogueFinishStack;

    private bool isTaking;


    protected override void Awake()
    {
        base.Awake();
        dialogueEmptyStack = new Stack<string>();
        dialogueFinishStack = new Stack<string>();
    }

    //注册当前对话NPC
    public void RigistDialogueNPC(DialogueNPC dialogueNPC)
    {
        curDialogueNPC = dialogueNPC;
        FillDialogueStack();
    }

    //填充对话堆栈
    private void FillDialogueStack()
    {
        dialogueEmptyStack.Clear();
        dialogueFinishStack.Clear();

        for (int i = curDialogueNPC.dialogueEmpty.dialogueList.Count - 1; i > -1; i--)
        {
            dialogueEmptyStack.Push(curDialogueNPC.dialogueEmpty.dialogueList[i]);
        }
        for (int i = curDialogueNPC.dialogueFinish.dialogueList.Count - 1; i > -1; i--)
        {
            dialogueFinishStack.Push(curDialogueNPC.dialogueFinish.dialogueList[i]);
        }
    }

    public void ShowDialogueEmpty()
    {
        if (!isTaking)
            StartCoroutine(DialogueRoutine(dialogueEmptyStack));
    }
    public void ShowDialogueFinish()
    {
        if (!isTaking)
            StartCoroutine(DialogueRoutine(dialogueFinishStack));
    }

    private IEnumerator DialogueRoutine(Stack<string> data)
    {
        isTaking = true;
        if (data.Count!=0)
        {
            EventHandler.CallShowDialogue(data.Pop());
            yield return null;
            isTaking = false;
        }
        else
        {
            EventHandler.CallShowDialogue(string.Empty);
            FillDialogueStack();
            isTaking = false;
        }
    }
}
