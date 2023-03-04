using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class PaddleMoveInput : MonoBehaviour
{
    public abstract IReadOnlyReactiveProperty<bool> MoveDirection { get; }

    // --------------------------------------------------

    public abstract bool IsPaddleMoveInput();
}
