using UnityEngine;

namespace Assets.Scripts.Controllers.StateMachine
{
    class ZombieStateMachine : MonoBehaviour
    {
        public static ZombieSearchState Searching = new ZombieSearchState();
        public static ZombieRunningState Running = new ZombieRunningState();

        public ZombieState currentState = Searching;
    }
}
