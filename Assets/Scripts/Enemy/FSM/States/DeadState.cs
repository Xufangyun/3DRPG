using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadState : FSMState
{
    GameObject npc;
    Animator anim;
    public DeadState(FSMController fSMController, GameObject npc) :base(fSMController)
    {
        stateID = StateID.Dead;
        this.npc = npc;
        anim = npc.GetComponent<Animator>();
    }

    public override void DoBeforeEnter()
    {
        npc.GetComponentInChildren<ShaderController>().SetDissolve();
    }

    public override void Act()
    {
        anim.SetBool("Death", true);
        npc.GetComponent<Collider>().enabled = false;
        Object.Destroy(npc.gameObject,4);
    }

    public override void DoAfterLeave()
    {
    }

    public override void Reason()
    {
    }

}
