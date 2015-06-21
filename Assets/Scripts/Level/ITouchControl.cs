using System;
using UnityEngine;

namespace RuzikOdyssey
{
	public interface ITouchControl
	{
		void TriggerTouch();
		bool HitTest(Vector2 position);
	}
}

