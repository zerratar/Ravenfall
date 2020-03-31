using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]
public class ServerItem : ServerObject
{
    public bool Equippable;
    public EquipmentSlot EquipmentSlot;
}
