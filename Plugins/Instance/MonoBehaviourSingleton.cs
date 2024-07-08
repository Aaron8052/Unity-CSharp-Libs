// @FengYan 2024 Copyright Reserved.

using UnityEngine;

namespace CsLibs.Singleton
{
	/// <summary>
	/// 预制体单例，需要在场景中提前挂在脚本
	/// </summary>
	/// <typeparam name="T">类型</typeparam>
	public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		protected static T instance;
		public static T Instance => instance ??= FindObjectOfType<T>();
		public static T INSTANCE => Instance;
		public static T TempInstance
		{
			get
			{
				if (!instance)
				{
					instance = FindObjectOfType<T>() ?? NewInstance();
				}
				return instance;
			}
		}
		void Awake()
		{
			if (instance && instance != this)
			{
				Destroy(this);
				return;
			}
			instance ??= GetComponent<T>() ?? FindObjectOfType<T>();
			OnAwake();
		}

		protected virtual void OnAwake(){}
		
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
