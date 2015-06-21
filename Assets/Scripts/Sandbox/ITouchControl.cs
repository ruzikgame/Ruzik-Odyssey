using UnityEngine;
using System.Collections.Generic;
using RuzikOdyssey.Level;
using RuzikOdyssey;
using System;
using System.Linq;

namespace Sandbox.RuzikOdyssey.Player
{

	public interface ITouchControl
	{
		void TriggerTouch();
		bool HitTest(Vector2 position);
	}

}
