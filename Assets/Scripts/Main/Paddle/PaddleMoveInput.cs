using Paddle;
using UniRx;
using UnityEngine;

public class PaddleMoveInput
{
    public IReadOnlyReactiveProperty<bool> MoveDirection => _moveDirection;
    public IReadOnlyReactiveProperty<bool> Move => _move;
    
    private readonly ReactiveProperty<bool> _moveDirection = new ReactiveProperty<bool>(true);
    private readonly ReactiveProperty<bool> _move = new ReactiveProperty<bool>(false);
    private IPaddleLocalPositionAdapter _paddlePosition;

    public PaddleMoveInput(IPaddleLocalPositionAdapter paddlePosition)
    {
        _paddlePosition = paddlePosition;
    }

    public bool IsPaddleMoveInput()
    {
        if(_paddlePosition.LocalPosition.x < 0)
        {
            InputWASD();
        }
        if(_paddlePosition.LocalPosition.x > 0)
        {
            InputArrow();
        }
        return _move.Value;
    }

    public void InputArrow()
    {
        _move.Value = false;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _moveDirection.Value = true;
            _move.Value = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _moveDirection.Value = false;
            _move.Value = true;
        }
    }

    public void InputWASD()
    {
        _move.Value = false;
        if (Input.GetKey(KeyCode.W))
        {
            _moveDirection.Value = true;
            _move.Value = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _moveDirection.Value = false;
            _move.Value = true;
        }
    }
}
