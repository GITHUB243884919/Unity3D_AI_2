using UnityEngine;
using System.Collections;

public class ChaseState : FSMState
{
    public ChaseState(Transform[] wp) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Chasing;

        curRotSpeed = 6.0f;
        curSpeed = 160.0f;

        //find next Waypoint position
        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        //Set the target position as the player position
        destPos = player.position;

        //Check the distance with player tank
        //When the distance is near, transition to attack state
        float dist = Vector3.Distance(npc.position, destPos);
        if (dist <= attackDistance)
        {
            Debug.Log("Switch to Attack state");
            npc.GetComponent<AIController>().SetTransition(Transition.ReachPlayer);
        }
        //Go back to patrol is it become too far
        else if (dist >= chaseDistance)
        {
            Debug.Log("Switch to Patrol state");
            npc.GetComponent<AIController>().SetTransition(Transition.LostPlayer);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        //Rotate to the target point
        destPos = player.position;

        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Go Forward
        //npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);
		CharacterController controller = npc.GetComponent<CharacterController>();
		controller.SimpleMove(npc.transform.forward * Time.deltaTime * curSpeed);

		Animation animComponent = npc.GetComponent<Animation>();
		animComponent.CrossFade("Run");
    }
}
