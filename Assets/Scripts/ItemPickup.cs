using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Outline ItemGlow;

    private void Start()
    {
        ItemGlow = GetComponent<Outline>();
    }

    public void EnableGlow()
    {
        ItemGlow.enabled = true;
    }

    public void DisableGlow()
    {
        ItemGlow.enabled = false;
    }
}
