using UnityEngine;
using System.Collections;

public class DeadState : FSMState
{
    public DeadState() 
    {
        stateID = FSMStateID.Dead;
    }

    public override void Reason(Transform player, Transform npc)
    {

    }

    public override void Act(Transform player, Transform npc)
    {        
		Animation animComponent = npc.GetComponent<Animation>();
		//animComponent.CrossFade("death");
    }
}
