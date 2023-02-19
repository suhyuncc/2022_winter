using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject      towerPrefab;
    [SerializeField]
    private int             towerBulidGold = 50;
    [SerializeField]
    private EnemySpawner    enemySpawner;
    [SerializeField]
    private PlayerGold      playerGold;

    public void SpawnTower(Transform tileTransform)
    {
        // 타워 건설 가능 여부 확인
        // 1. 타워를 건설할 만큼 돈이 없으면 타워 건설 X
        if(towerBulidGold > playerGold.CurrentGold)
        {
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();

        // 2. 현재 타일의 위치에 이미 카워가 건설되에 있으면 타워 건설 X
        if (tile.IsBuildTower == true) 
        {
            return;
        }

        // 타워가 건설되어 있음으로 설정
        tile.IsBuildTower = true;
        // 타워 건설에 필요한 골드만큼 감소
        playerGold.CurrentGold -= towerBulidGold;
        // 선택한 타일의 위치에 타워 건설 (타일보다 z축 -1의 위치에 배치)
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerPrefab, position, Quaternion.identity);
        // 타워 무기에 enemySpawner 정보 전달
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }
}
