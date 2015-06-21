using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InnerHealthBarController : MonoBehaviour
{
	private IList<GameObject> healthBarFragments;

	private void Start()
	{
		healthBarFragments = new List<GameObject>();
		for (int i = 0; i < transform.childCount; i++)
		{
			healthBarFragments.Add(transform.GetChild(i).gameObject);
		}
	}

	public void ShowHealthBarFragments(int numberOfFragments)
	{
		for (var i = 0; i < healthBarFragments.Count; i++)
		{
			if (i < numberOfFragments)
			{
				healthBarFragments[i].GetComponent<Renderer>().enabled = true;
			}
			else 
			{
				healthBarFragments[i].GetComponent<Renderer>().enabled = false;
			}
		}
	}
}

