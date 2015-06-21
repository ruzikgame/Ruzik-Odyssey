using UnityEngine;
using RuzikOdyssey;
using Newtonsoft.Json;
using System;
using RuzikOdyssey.Common;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using RuzikOdyssey.Level;
using RuzikOdyssey.Domain;

namespace RuzikOdyssey.Infrastructure
{
	public sealed class GameContext
	{
		public const string GameModelKey = "GameModel";

		private static Dictionary<string, Lazy<TextAsset>> defaultsAssets = new Dictionary<string, Lazy<TextAsset>>();

		public GameContext()
		{
#if UNITY_EDITOR
			PlayerPrefs.DeleteAll();
#endif
		}

		public void Save()
		{
			PlayerPrefs.Save();
		}

		public T LoadDefauts<T>(JsonConverter customConverter = null)
		{
			var defaultsFile = GetDefaultsAsset<T>();

			if (defaultsFile == null) throw new UnityException("Failed to load defaults file");

			var defaults = customConverter == null 
				? JsonConvert.DeserializeObject<T>(defaultsFile.text)
				: JsonConvert.DeserializeObject<T>(defaultsFile.text, customConverter);
				
			if (defaults == null) throw new UnityException("Failed to deserialize defaults from the config file.");

			return defaults;
		}

		private TextAsset GetDefaultsAsset<T>()
		{
			if (!defaultsAssets.ContainsKey(typeof(T).FullName))
			{
				defaultsAssets.Add(
					typeof(T).FullName, 
					new Lazy<TextAsset>(() => Resources.Load(typeof(T).Name + "Defaults") as TextAsset));
			}

			var defaultsAsset = defaultsAssets[typeof(T).FullName].Value; 

			return defaultsAsset;
		}

		public void SaveEntity<T>(T entity)
		{
			var json = JsonConvert.SerializeObject(entity);
			PlayerPrefs.SetString(typeof(T).FullName, json);
		}

		public T LoadEntity<T>(JsonConverter customConverter = null) where T : class
		{
			var key = typeof(T).FullName;

			var json = PlayerPrefs.GetString(key);

			if (String.IsNullOrEmpty(json))
			{
				Log.Warning("Failed to load {0} from persistence storage. Loading defaults.", key);
				return LoadDefauts<T>(customConverter);
			}

			var entity = customConverter == null 
				? JsonConvert.DeserializeObject<T>(json)
				: JsonConvert.DeserializeObject<T>(json, customConverter);
			
			if (entity == null)
			{
				Log.Error("Failed to parse {0}", key);
				return LoadDefauts<T>();
			}

			return entity;
		}

		public IEnumerator LoadGameContentAsync(Action<GameContent> resultCallback, Action<int> progressCallback = null)
		{
			if (progressCallback != null) progressCallback(0);
			
			using (var www = new WWW(GameConfig.GameContentUrl))
			{
				while (!www.isDone)
				{
					if (progressCallback != null) progressCallback((int) (100 * www.progress));
					yield return null;
				}

				Log.Debug("Finished loading {0}", GameConfig.GameContentUrl);

				if (www.error != null) 
				{
					Log.Error("An error occured while downloading game content. Error: " + www.error);
					yield break;
				}

				GameContent content = null;
				try
				{
					content = JsonConvert.DeserializeObject<GameContent>(www.text);
				}
				catch (Exception ex)
				{
					Log.Error("An exeption occured while deserializing game content. Exception: {0}", ex.Message);
					yield break;
				}

				progressCallback(100);

				if (content == null) 
				{
					Log.Error("Failed to desirialize game content");
					yield break;
				}

				resultCallback(content);
			}
		}
	}
}
