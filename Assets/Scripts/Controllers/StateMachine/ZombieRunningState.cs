using UnityEngine;

namespace Assets.Scripts.Controllers.StateMachine
{
    class ZombieRunningState : ZombieState
    {
        public override void OnTriggerStay(GameObject gameObject, Collider other)
        {
            if (other.tag == PLAYER_TAG)
            {
                var lifeController = other.gameObject.GetComponent<PlayerController>();
                var zombieController = gameObject.GetComponent<ZombieController>();
                lifeController.GetDamage(zombieController.attackPower);
            }
        }

        public override void Update(GameObject gameObject)
        {
            var controller = gameObject.GetComponent<ZombieController>();
            if (controller.target == null)
            {
                controller.moveVelocity = Vector3.zero;
                var stateMachine = gameObject.GetComponent<ZombieStateMachine>();
                stateMachine.currentState = ZombieStateMachine.Searching;
            }
            else
            {
                Vector3 targetDirection = controller.target.transform.position - gameObject.transform.position;

                var moveInput = new Vector3(targetDirection.x, 0.0f, targetDirection.z).normalized;
                controller.moveVelocity = moveInput * controller.speed;
                Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + controller.moveVelocity, Color.red);

                gameObject.transform.LookAt(controller.target.transform);
            }
        }
    }
}
