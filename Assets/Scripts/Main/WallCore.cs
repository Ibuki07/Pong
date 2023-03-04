using UnityEngine;

namespace Wall
{
    public class WallCore : MonoBehaviour
    {
        private void Start()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var collisionWallHandler = collider.gameObject.GetComponent<IWallCollisionHandler>();
            collisionWallHandler?.OnCollisionWall(this);
        }
    }
}