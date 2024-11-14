using UnityEngine;

public interface IControllable
{
    public void SetMoveDirection(Vector2 dir);
    public void SetWalking(bool isWalking);
}
