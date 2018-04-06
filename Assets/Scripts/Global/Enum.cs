using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enum {

    public enum TimerTypeEnum
    {
        Countdown = 0, //Timer that starts from XX:XX ends at 00:00
        Normal = 1 // Timer that starts from 00:00
    };

    public enum PlayerSpawnerTypeEnum
    {
        WithPositionList, //Spawner that uses a list of gameobjects to spawn players on their position
        RandomXY // Spawner that uses random positions on an 2D plane
    };

    public enum LoggerMessageType
    {
        Normal,
        Important,
        Error
    };

    public enum InteractableType
    {
        SingleAction,
        ComboCarriable
    };

}
