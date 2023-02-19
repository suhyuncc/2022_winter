using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[]          waves;
    [SerializeField]
    private EnemySpawner    enemySpawner;
    private int             currentWaveIndex = -1;

    // 웨이브 정보 출력을 위한 Get 프로퍼티 (현재 웨이브, 총 웨이브)
    public int CurrentWave => currentWaveIndex + 1; // 시작이 0이기 때문에 +1
    public int MaxWave => waves.Length;

    public void StartWave()
    {
        if(enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length-1) 
        {
            currentWaveIndex ++;
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }
}

[System.Serializable]
public struct Wave
{
    public float        spawnTime;
    public int          maxEnemyCount;
    public GameObject[] enemyPrefabs;
}