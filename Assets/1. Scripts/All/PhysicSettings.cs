using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicSettings : MonoBehaviour
{
    /// <summary>
    /// �̰� ���� �Ⱦ���
    /// </summary>
    public static PhysicSettings ins;
    public void IgnoreLayerCollision(string Layer1Name, string Layer2Name, bool Boolen1 = false)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Layer1Name), LayerMask.NameToLayer(Layer2Name), Boolen1);
    }
    private void Awake()
    {
        ins = this;
        //IgnoreLayerCollision("Item", "Player", false);
    }
}
