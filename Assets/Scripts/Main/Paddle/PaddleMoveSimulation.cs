using UnityEngine;

namespace Paddle
{
    public class PaddleMoveSimulation
    {
        public Vector3 Velocity { get; set; }

        public PaddleMoveSimulation(Vector3 initialVelocity)
        {
            // ���x�����������܂��B
            Velocity = initialVelocity;
        }

        public Vector3 UpdatePosition(Vector3 localPosition, float fixedDeltaTime)
        {
            // ���݂̈ʒu�ɑ��x�ƌo�ߎ��Ԃ��l�������ړ��ʂ������܂��B
            localPosition += Velocity * fixedDeltaTime;
            // �ړ���̈ʒu��Ԃ��܂��B
            return localPosition;
        }
    }
}