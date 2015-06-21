using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Common
{
	public class BarController : MonoBehaviour 
	{
		public int totalFragmentsInBar = 10;
		public bool inverseFragments = false;

		private IList<GameObject> barFragments;

		private void Start()
		{
			var innerBarTransform = this.gameObject.transform.FindChild("InnerBar");
			if (innerBarTransform == null) throw new UnityException("Failed to get inner bar transform");

			barFragments = new List<GameObject>();
			for (int i = 0; i < innerBarTransform.childCount; i++)
			{
				barFragments.Add(innerBarTransform.GetChild(i).gameObject);
			}
		}

		public void ShowLevel(int healthLevel)
		{
			int numberOfBarFragmentsToShow = (int) (healthLevel / totalFragmentsInBar) + 1;
			if (healthLevel <= 0) numberOfBarFragmentsToShow = 0;
			if (healthLevel >= 100) numberOfBarFragmentsToShow = totalFragmentsInBar;

			ShowHealthBarFragments(numberOfBarFragmentsToShow);
		}
		
		private void ShowHealthBarFragments(int numberOfFragments)
		{
			for (var i = 0; i < barFragments.Count; i++)
			{
				barFragments[i].GetComponent<Renderer>().enabled = inverseFragments 
					// Hide the first N fragments if the bar is inversed
					? (i >= totalFragmentsInBar - numberOfFragments) 
					// Hide the last N fragments if the bar is in regular order
					: (i < numberOfFragments) ;
			}
		}
	}
}
