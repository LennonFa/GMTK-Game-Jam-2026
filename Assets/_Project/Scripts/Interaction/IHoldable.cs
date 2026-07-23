using UnityEngine;

public interface IHoldable
{
    void Hold(GameObject parent, Transform holdingPosition);
    void Release();
}
