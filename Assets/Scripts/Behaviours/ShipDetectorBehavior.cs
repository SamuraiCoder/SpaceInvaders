using Data;
using Events;
using pEventBus;
using UnityEngine;

namespace Behaviours
{
    public class ShipDetectorBehavior : MonoBehaviour, IEventReceiver<EnemyNotifySurroundingsEvent>
    {
        private Rigidbody2D rb2D;
        private bool checkSurroundings;
        private float upRayDistance;
        private float downRayDistance;
        private float leftRayDistance;
        private float rightRayDistance;
        private void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
            EventBus.Register(this);

            upRayDistance = ConstValues.SPACING_ENEMY_ROW;
            downRayDistance = ConstValues.SPACING_ENEMY_ROW;
            leftRayDistance = ConstValues.SPACING_ENEMY_COLUMN;
            rightRayDistance = ConstValues.SPACING_ENEMY_COLUMN;
        }

        private void OnDestroy()
        {
            EventBus.UnRegister(this);
        }

        private void FixedUpdate()
        {
            if (!checkSurroundings)
            {
                return;
            }

            OnCheckSurroundings();
        }

        private void OnCheckSurroundings()
        {
            checkSurroundings = false;

            var position = transform.position;
            RaycastHit2D upHit = Physics2D.Raycast(position, Vector2.up, upRayDistance);
            RaycastHit2D downHit = Physics2D.Raycast(position, Vector2.down, downRayDistance);
            RaycastHit2D leftHit = Physics2D.Raycast(position, Vector2.left, leftRayDistance);
            RaycastHit2D rightHit = Physics2D.Raycast(position, Vector2.right, rightRayDistance);

            var enemyLayer = LayerMask.NameToLayer(ConstValues.ENEMY_LAYER);

            var upCollider = upHit.collider;
            var downCollider = downHit.collider;
            var leftCollider = leftHit.collider;
            var rightCollider = rightHit.collider;
            
            var newEnemyData = new EnemyData();
            
            if (upCollider != null && upCollider.gameObject.activeSelf && upCollider.gameObject.layer == enemyLayer)
            {
                //Debug.Log($"I am {gameObject.name} and I have a friend {upCollider.gameObject.name} UP");
                newEnemyData.FriendEnemyUp = upCollider.gameObject.name;
            }
            
            if (downCollider != null && downCollider.gameObject.activeSelf && downCollider.gameObject.layer == enemyLayer)
            {
                //Debug.Log($"I am {gameObject.name} and I have a friend {downCollider.gameObject.name} DOWN");
                newEnemyData.FriendEnemyDown = downCollider.gameObject.name;
            }
            
            if (leftCollider != null && leftCollider.gameObject.activeSelf && leftCollider.gameObject.layer == enemyLayer)
            {
                //Debug.Log($"I am {gameObject.name} and I have a friend {leftCollider.gameObject.name} LEFT");
                newEnemyData.FriendEnemyLeft = leftCollider.gameObject.name;
            }
            
            if (rightCollider != null && rightCollider.gameObject.activeSelf && rightCollider.gameObject.layer == enemyLayer)
            {
                //Debug.Log($"I am {gameObject.name} and I have a friend {rightCollider.gameObject.name} RIGHT");
                newEnemyData.FriendEnemyRight = rightCollider.gameObject.name;
            }
            
            //Notify AIMovementService for current ship surroundings
            EventBus<EnemySendSurroundingsEvent>.Raise(new EnemySendSurroundingsEvent
            {
                EnemyShipName = gameObject.name,
                EnemyDataInfo = newEnemyData
            });
        }

        public void OnEvent(EnemyNotifySurroundingsEvent e)
        {
            if (e.EnemyShipName != gameObject.name)
            {
                return;
            }

            checkSurroundings = true;
        }

        private void OnDrawGizmos()
        {
            var position = gameObject.transform.position;
            Gizmos.DrawRay(position, Vector2.up * upRayDistance);
            Gizmos.DrawRay(position, Vector2.down * downRayDistance);
            Gizmos.DrawRay(position, Vector2.left * leftRayDistance);
            Gizmos.DrawRay(position, Vector2.right * rightRayDistance);
        }
    }
}
