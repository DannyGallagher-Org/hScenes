using UnityEngine;

namespace Levels
{
	public class SceneTransitionSpace : MonoBehaviour
	{
		public AdditiveScene Source;
		public Transform[] Others = new Transform[0];

		private void OnDrawGizmos() 
		{
			Gizmos.DrawIcon(transform.position, "Transition.png", true);

			var othersLength = Others.Length;
			if(othersLength >= 1)
				Gizmos.DrawIcon(Others[0].transform.position, "TransitionOther.png", true);
			
			if(othersLength >= 2)
				Gizmos.DrawIcon(Others[1].transform.position, "TransitionOther.png", true);
			
			if(othersLength >= 3)
				Gizmos.DrawIcon(Others[2].transform.position, "TransitionOther.png", true);
			
			if(othersLength >= 4)
				Gizmos.DrawIcon(Others[3].transform.position, "TransitionOther.png", true);
		}
	}
}
