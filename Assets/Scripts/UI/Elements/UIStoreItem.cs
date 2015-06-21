using UnityEngine;
using System;

namespace RuzikOdyssey.UI.Elements
{
	public sealed class UIStoreItem : MonoBehaviour
	{
		[SerializeField]
		private UISprite imageSprite;

		[SerializeField]
		private UIButton buyButton;

		[SerializeField]
		private UISprite containerSprite;

		[SerializeField]
		private int gapBetweenItems;

		[SerializeField]
		private UILabel itemTitle;

		public int GapBetweenItems 
		{ 
			get { return this.gapBetweenItems; }
			set { this.gapBetweenItems = value; }
		}
		public UISprite ImageSprite { get { return this.imageSprite; } }
		public UISprite ContainerSprite { get { return this.containerSprite; } }
		public UIButton BuyButton { get { return this.buyButton; } }
		public UILabel ItemTitle { get { return this.itemTitle; } }
	}
}
