using UnityEngine;
using System.Collections;

namespace RuzikOdyssey.Common.UI
{
	public class Label : MonoBehaviour
	{
		private void Awake()
		{
			if (gameObject.GetComponent<GUIText>() == null) 
				throw new UnityException("Label script can be attached only to a game object with GUIText component.");
		}

		public string Text
		{
			get { return GetComponent<GUIText>().text; }
			set 
			{
				Log.Debug("Ammo is {0}", value);

				GetComponent<GUIText>().text = value; 
			}
		}
	}
}