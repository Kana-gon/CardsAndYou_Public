using System.Collections;
using App.BaseSystem.DataStores.ScriptableObjects.Enemy;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/DataBase/Enemy")]
public class EnemyDataBase : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField]
    List<EnemyData> enemyList = new List<EnemyData>();

    public List<EnemyData> GetEnemyList()
    {
        return enemyList;
    }
    public EnemyData GetEnemyDataByName(string searchName)
    {
        return enemyList.Find(enemyName => enemyName.GetEnemyName() == searchName);
    }

    public List<EnemyData.dropItem> GetEnemyDropItem(string searchName)
    {
        return enemyList.Find(enemyName => enemyName.GetEnemyName() == searchName).GetDropItems();
    }
    public List<EnemyData.dropItem> GetEnemyDamagedDropItem(string searchName)
    {
        return enemyList.Find(enemyName => enemyName.GetEnemyName() == searchName).GetDamagedDropItems();
    }
}