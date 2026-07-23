using UnityEngine;

public interface IHoldable
{
    void Hold(GameObject parent, Vector3 holdingPosition);
    void Release();
}
