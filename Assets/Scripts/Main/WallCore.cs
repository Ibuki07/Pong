using UniRx.Triggers;
using UnityEngine;
using UniRx;

namespace Wall
{
    public class WallCore : MonoBehaviour
    {
        private void Start()
        {
            this.OnTriggerEnter2DAsObservable().Subscribe(collider =>
            {
                var collisionWallHandler = collider.gameObject.GetComponent<IWallCollisionHandler>();
                collisionWallHandler?.OnCollisionWall(this);
            });
        }
    }
}