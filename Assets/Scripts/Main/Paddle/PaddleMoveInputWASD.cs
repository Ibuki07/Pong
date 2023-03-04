using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PaddleMoveInputWASD : PaddleMoveInput
{
    public override IReadOnlyReactiveProperty<bool> MoveDirection => _isMoveDirection;

    // --------------------------------------------------

    private readonly ReactiveProperty<bool> _isMoveDirection = new ReactiveProperty<bool>(true);

    // --------------------------------------------------

    public override bool IsPaddleMoveInput()
    {
        bool isMove = false;
        if (Input.GetKey(KeyCode.W))
        {
            _isMoveDirection.Value = true;
            isMove = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _isMoveDirection.Value = false;
            isMove = true;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            isMove = false;
        }
        return isMove;
    }
}
