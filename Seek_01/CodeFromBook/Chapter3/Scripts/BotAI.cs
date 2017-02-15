using UnityEngine;
using System.Collections;
using Pathfinding.RVO;

namespace Pathfinding {

	[RequireComponent(typeof(Seeker))]
	public class BotAI : AIPath {		

		
		public new void Start () 
		{			
			base.Start ();
		}		

		
		public override Vector3 GetFeetPosition ()
		{
			return tr.position;
		}
		
		protected new void Update () {
			
			//Get velocity in world-space
			Vector3 velocity;
			if (canMove) {
				
				//Calculate desired velocity
				Vector3 dir = CalculateVelocity (GetFeetPosition());
				
				//Rotate towards targetDirection (filled in by CalculateVelocity)
				RotateTowards (targetDirection);
				
				dir.y = 0;
				if (dir.sqrMagnitude < 0.2f) { 
					dir = Vector3.zero;
				}
				
				if (controller != null)
					controller.SimpleMove (dir);
				else
					Debug.LogWarning ("No NavmeshController or CharacterController attached to GameObject");
				
				velocity = controller.velocity;
			} else {
				velocity = Vector3.zero;
			}
			
			
			//Animation
			
			//Calculate the velocity relative to this transform's orientation
			/*
			Vector3 relVelocity = tr.InverseTransformDirection (velocity);
			relVelocity.y = 0;


			if (velocity.sqrMagnitude > 0.2f) 
			{
				float speed = relVelocity.z;
			}*/
		}
	}
}
