using UnityEngine;

public class PlayerState : MonoBehaviour
{
    //maybe add health and oxygen stuff here too

    public IHoldable HeldItem { get; set; }

    /// <summary>
    /// Removes held item from state and returns
    /// </summary>
    /// <returns></returns>
    public IHoldable TakeHeld()
    {
        HeldItem.Release();
        var taken = HeldItem;

        HeldItem = null;
        return taken;
    }
}

