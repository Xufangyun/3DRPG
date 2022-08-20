using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VictoryState : FSMState
{
    GameObject npc;
    Animator anim;
    public VictoryState(FSMController fSMController, GameObject npc) : base(fSMController)
    {
        stateID = StateID.Vitory;
        this.npc = npc;
        anim = npc.GetComponent<Animator>();
    }

    public override void DoBeforeEnter()
    {
    }

    public override void Act()
    {
        anim.SetBool("Victory", true);
    }

    public override void DoAfterLeave()
    {
    }

    public override void Reason()
    {
    }
}
