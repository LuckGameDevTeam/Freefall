using System.Collections;



public class DebugEx 
{
	public static void Debug(string debugMessage)
	{
		if(UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log(debugMessage);
		}

	}

	public static void DebugWarning(string debugMessage)
	{
		if(UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.LogWarning(debugMessage);
		}
	}

	public static void DebugError(string debugMessage)
	{
		if(UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.LogError(debugMessage);
		}
	}
}
