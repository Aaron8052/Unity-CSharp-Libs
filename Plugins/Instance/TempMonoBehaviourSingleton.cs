// @FengYan 2024 Copyright Reserved.

using UnityEngine;

namespace CsLibs.Singleton
{
	/// <summary>
	/// 使用时自动创建的MonoBehaviour单例
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class TempMonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{

		static T _instance;
		public static T Instance
		{
			get
			{
				if (!_instance)
				{
					_instance = FindObjectOfType<T>() ?? NewInstance();
				}
				return _instance;
			}
		}

		static bool isInited;
		static void InitTempSingleton()
		{
			if (isInited)
				return;
			
			Application.quitting += OnAppQuit;
			isInited = true;
		}

		static bool appQuit;
		static void OnAppQuit()
		{
			appQuit = true;
		}
		static T NewInstance()
		{
			InitTempSingleton();
			
			if (appQuit)
				return null;
			
			var typeName = typeof(T).Name;
			var instance = new GameObject($"[Instance] {typeName}").AddComponent<T>();
			DontDestroyOnLoad(instance);
			return instance;
		}
	}
	
}
