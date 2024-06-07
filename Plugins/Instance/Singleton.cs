// @FengYan 2024 Copyright Reserved.

using System;

namespace CsLibs.Singleton
{
    /// <summary>
    /// 非MonoBehaviour单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : IDisposable where T : new()
    {
        static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                    (_instance as Singleton<T>)?.OnAwake();
                    //Application.quitting += (_instance as Singleton<T>)!.Dispose;
                }

                return _instance;
            }
        }
        public static T INSTANCE => Instance;
        protected virtual void OnAwake() { }

        /*~Singleton()
        {
            //Dispose();
        }*/
        public virtual void Dispose() { }
    }
}
