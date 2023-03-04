using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PaddleMoveInputArrow : PaddleMoveInput
{
    public override IReadOnlyReactiveProperty<bool> MoveDirection => _isMoveDirection;

    // --------------------------------------------------

    private readonly ReactiveProperty<bool> _isMoveDirection = new ReactiveProperty<bool>(true);

    // --------------------------------------------------

    public override bool IsPaddleMoveInput()
    {
        bool isMove = false;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _isMoveDirection.Value = true;
            isMove = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _isMoveDirection.Value = false;
            isMove = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            isMove = false;
        }
        return isMove;
    }
}
