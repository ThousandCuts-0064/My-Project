using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations;

public class NetworkSlot : NetworkBehaviour
{
    public NetworkObject Slot { get; private set; }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer) return;

        Slot = Instantiate(NetworkSlotPrefab.Get);
        Slot.Spawn();
        Slot.transform.SetParent(NetworkObject.transform);
        Slot.transform.localPosition = Vector3.zero;
        Slot.GetComponent<ParentConstraint>().SetSource(0, new ConstraintSource() { sourceTransform = transform, weight = 1 });
        SetParentConstraintClientRpc(Slot.NetworkObjectId);
    }

    [ClientRpc]
    private void SetParentConstraintClientRpc(ulong slotID)
    {
        NetworkManager.SpawnManager.SpawnedObjects[slotID].GetComponent<ParentConstraint>().SetSource(0, new ConstraintSource() { sourceTransform = transform, weight = 1 });
    }
}
