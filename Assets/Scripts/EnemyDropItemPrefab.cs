using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropItemPrefab : MonoBehaviour
{
    // シーン内で唯一のインスタンスとして取り扱う
    public static EnemyDropItemPrefab EDIPInstance { get; private set; }

    [SerializeField] GameObject RecoverPPS;
    [SerializeField] GameObject RecoverPPM;
    [SerializeField] GameObject RecoverPPL;
    [SerializeField] GameObject RecoverAmmoS;
    [SerializeField] GameObject RecoverAmmoM;
    [SerializeField] GameObject RecoverAmmoL;

    void Awake()
    {
        if (EDIPInstance != null && EDIPInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        EDIPInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (RecoverPPS == null) Debug.LogError("RecoverPPS prefab is not assigned.", this);
        if (RecoverPPM == null) Debug.LogError("RecoverPPM prefab is not assigned.", this);
        if (RecoverPPL == null) Debug.LogError("RecoverPPL prefab is not assigned.", this);
        if (RecoverAmmoS == null) Debug.LogError("RecoverAmmoS prefab is not assigned.", this);
        if (RecoverAmmoM == null) Debug.LogError("RecoverAmmoM prefab is not assigned.", this);
        if (RecoverAmmoL == null) Debug.LogError("RecoverAmmoL prefab is not assigned.", this);
    }

    void Update() { }

    public GameObject GetPrefab(Item item)
    {
        if (item is recoverPPs)  return RecoverPPS;
        if (item is recoverPPm)  return RecoverPPM;
        if (item is recoverPPl)  return RecoverPPL;
        if (item is recoverAmmos) return RecoverAmmoS;
        if (item is recoverAmmom) return RecoverAmmoM;
        if (item is recoverAmmol) return RecoverAmmoL;
        Debug.LogWarning("No prefab found for item type: " + item.GetType().Name, this);
        return null;
    }
}
