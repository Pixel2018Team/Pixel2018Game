using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugLogger  {

	public static void Log(string message, Enum.LoggerMessageType type)
    {
        switch (type)
        {
            case Enum.LoggerMessageType.Normal:
                Debug.Log(message);
                break;

            case Enum.LoggerMessageType.Important:
                Debug.Log("<color=blue>"+message+"</color>");
                break;

            case Enum.LoggerMessageType.Error:
                Debug.Log("<color=red>" + message + "</color>");
                break;

            default:
                break;
        }
    }
}
