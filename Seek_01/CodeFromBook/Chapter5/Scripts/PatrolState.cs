using UnityEngine;
using System.Collections;
public class PatrolState : FSMState
{    
	public PatrolState(Transform[] wp)     
	{
		waypoints = wp;        
		stateID = FSMStateID.Patrolling;
		curRotSpeed = 6.0f;        
		curSpeed = 80.0f;    
	}    
			
	public override void Reason(Transform player, Transform npc)    
	{
		//Check the distance with player tank        
		//When the distance is near, transition to chase state        
		if (Vector3.Distance(npc.position, player.position) <= chaseDistance)        
		{            
			Debug.Log("Switch to Chase State");            
			npc.GetComponent<AIController>().SetTransition(Transition.SawPlayer);
		}    
	}    
			
	public override void Act(Transform player, Transform npc)    
	{        
		//Find another random patrol point if the current point is reached		        
		if (Vector3.Distance(npc.position, destPos) <= arriveDistance)        
		{            
			Debug.Log("Reached to the destination point\ncalculating the next point");            
			FindNextPoint();        
		}        
		//Rotate to the target point        
		Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);        
		npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);        
		//Go Forward        		
		CharacterController controller = npc.GetComponent<CharacterController>();		
		controller.SimpleMove(npc.transform.forward * Time.deltaTime * curSpeed);		
		Animation animComponent = npc.GetComponent<Animation>();		
		animComponent.CrossFade("Walk");    
	}
}
