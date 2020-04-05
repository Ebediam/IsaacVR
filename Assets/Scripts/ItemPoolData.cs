using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemPoolData : ScriptableObject
{

    public List<ItemData> itemPool;
    public List<ItemData> pickupPool;
}
