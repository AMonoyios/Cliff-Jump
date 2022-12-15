using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolSettings", menuName = "ScriptableObjects/PoolSetupSettings")]
public class PoolSetupSettingsSO : ScriptableObject
{
    [System.Serializable]
    public class PoolEntry
    {
        public string id;
        public GameObject prefab;
        public int size;
        [TagSelector]
        public string parentTag;

        public PoolEntry(string id, GameObject prefab, int size, string parentTag = "Untagged")
        {
            this.id = id;
            this.prefab = prefab;
            this.size = size;
            this.parentTag = parentTag;
        }
    }

    public List<PoolEntry> PoolEntries;
}
