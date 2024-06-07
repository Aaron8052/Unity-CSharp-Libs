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
		void Awake()
		{
			if (instance && instance != this)
			{
				Destroy(this);
				return;
			}
			instance = GetComponent<T>() ?? FindObjectOfType<T>();
			OnAwake();
		}

		protected virtual void OnAwake(){}
	}
}
