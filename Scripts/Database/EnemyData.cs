using System;
using System.Collections.Generic;
using UnityEngine;
namespace App.BaseSystem.DataStores.ScriptableObjects.Enemy
{
    [CreateAssetMenu(menuName = "ScriptableObject/Data/Enemy")]
    public class EnemyData : ScriptableObject
    {

        [SerializeField]
        public string enemyName;
        [Serializable]
        public struct dropItem
        {
            public GameObject obj;
            public int num;
        }

        [SerializeField] List<dropItem> damagedDropItems = new List<dropItem>();
        [SerializeField] List<dropItem> dropItems = new List<dropItem>();
        [SerializeField] GameObject burrage;
        [SerializeField] float HP;
        [SerializeField] float DEF;
        public string GetEnemyName()
        {
            return enemyName;
        }
        public List<dropItem> GetDropItems()
        {
            return dropItems;
        }
        public List<dropItem> GetDamagedDropItems()
        {
            return damagedDropItems;
        }
        public GameObject GetBurrage()
        {
            return burrage;
        }
        public float GetHP()
        {
            return HP;
        }
        public float GetDEF()
        {
            return DEF;
        }
    }
}