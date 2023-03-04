using UnityEngine;

namespace Paddle
{
    public class PaddleSimulation
    {
        public Vector3 Velocity;

        // --------------------------------------------------

        public PaddleSimulation(Vector3 initialVelocity)
        {
            // 速度を初期化します。
            Velocity = initialVelocity;
        }

        public Vector3 UpdatePosition(Vector3 localPosition, float fixedDeltaTime)
        {
            // 現在の位置に速度と経過時間を考慮した移動量を加えます。
            localPosition += Velocity * fixedDeltaTime;
            // 移動後の位置を返します。
            return localPosition;
        }
    }
}