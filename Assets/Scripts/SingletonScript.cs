using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	private static T instance;

	// The variable used for accessing the instance of the singleton
	public static T inst
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<T>();
			}
			return instance;
		}
	}
}
