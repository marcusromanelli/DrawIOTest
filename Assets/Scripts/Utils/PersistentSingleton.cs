using UnityEngine;

public class PersistentSingleton<T> : SingletonMB<T> where T : MonoBehaviour
{
	new public static T Instance
	{
		get
		{
			lock(_lock)
			{
				if (_instance == null)
				{
					var instances = FindObjectsOfType(typeof(T));

                    if (instances.Length > 1)
                    {
                        Debug.LogWarning("Il y a plusieurs managers du meme type sur la scène : " + typeof(T).Name);
                        return _instance;
                    }

					if (instances.Length <= 0)
						return null;

					_instance = (T)instances[0];
				}

				return _instance;
			}
		}
	}
}
