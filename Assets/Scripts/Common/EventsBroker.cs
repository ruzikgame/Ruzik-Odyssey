using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Common
{
	/* REMARK
	 * Staticly subscribing to events prevents objects from being released.
	 * Make sure to unsubscribe before object is destoyed.
	 */
	public static class EventsBroker
	{
		private static Dictionary<string, List<Delegate>> subscriptions;

		static EventsBroker()
		{
			subscriptions = new Dictionary<string, List<Delegate>>();
		}

		public static void Publish<T>(string eventId, ref EventHandler<T> publishedEvent)
			where T : EventArgs
		{
			if (String.IsNullOrEmpty(eventId)) 
				throw new UnityException("Event can't be published with an empty event ID");

			publishedEvent += (sender, e) => ProxyEventHandler<T>(eventId, sender, e);
		}

		/// <summary>
		/// Subscribe the specified eventId and eventHandler.
		/// </summary>
		/// <param name="eventId">Event identifier.</param>
		/// <param name="eventHandler">Event handler.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <remarks>
		/// Staticly subscribing to events prevents objects from being released.
		/// Make sure to unsubscribe before object is destoyed.
		/// </remarks>
		public static void Subscribe<T>(string eventId, EventHandler<T> eventHandler)
			where T : EventArgs
		{
			if (String.IsNullOrEmpty(eventId)) 
				throw new UnityException("Can't subscribe for an event with an empty event ID");
			if (eventHandler == null) throw new UnityException("Subscribing event handler can't be null");

			List<Delegate> existingEventHandlers;

			if (subscriptions.TryGetValue(eventId, out existingEventHandlers))
			{
				existingEventHandlers.Add(eventHandler);
			}
			else 
			{
				subscriptions.Add(eventId, new List<Delegate> { eventHandler });
			}
		}

		public static void Unsubscribe<T>(string eventId, EventHandler<T> eventHandler)
			where T : EventArgs
		{
			if (String.IsNullOrEmpty(eventId))
			{
				Log.Warning("Can't unsubscribe from an event with an empty event ID");
				return;
			}

			if (eventHandler == null) 
			{
				Log.Warning("Unsubscrubing event handler can't be null");
				return;
			}
			
			List<Delegate> existingEventHandlers;
			
			if (subscriptions.TryGetValue(eventId, out existingEventHandlers))
			{
				var removed = existingEventHandlers.Remove(eventHandler);

				if (!removed) Log.Error("Failed to unsubscribe from event {0}", eventId);
			}
			else 
			{
				Log.Warning("Event {0} has not existing event handlers", eventId);
			}
		}

		public static void ClearSubscribtions()
		{
			subscriptions.Clear();
		}

		private static void ProxyEventHandler<T>(string eventId, object sender, T e)
			where T : EventArgs
		{
			List<Delegate> eventHandlers;
			if (!subscriptions.TryGetValue(eventId, out eventHandlers)) return;

			foreach (var eventHander in eventHandlers)
			{
				var eventHandler = eventHander as EventHandler<T>;
				if (eventHandler == null) 
				{
					Debug.LogError(String.Format("Event {0} subscriber has incorrect event arguments", eventId));
					return;
				}
				
				eventHandler.Invoke(sender, e);
			}


		}
	}
}
