using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour 
{
	private InnerHealthBarController innerHealthBarController;

	private const int totalFragmentsInHealthBar = 10;

	private void Start()
	{
		innerHealthBarController = gameObject.GetComponentInChildren<InnerHealthBarController>();
		if (innerHealthBarController == null) throw new UnityException("Failed to get inner health bar controller");
	}

	public void ShowHealthLevel(int healthLevel)
	{
		int healthBarFragmentsToShow = (int) (healthLevel / totalFragmentsInHealthBar) + 1;
		if (healthLevel <= 0) healthBarFragmentsToShow = 0;
		if (healthLevel >= 100) healthBarFragmentsToShow = totalFragmentsInHealthBar;

		innerHealthBarController.ShowHealthBarFragments(healthBarFragmentsToShow);
	}
}
