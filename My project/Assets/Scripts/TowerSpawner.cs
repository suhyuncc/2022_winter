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
        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        // 1. Ÿ���� �Ǽ��� ��ŭ ���� ������ Ÿ�� �Ǽ� X
        if(towerBulidGold > playerGold.CurrentGold)
        {
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();

        // 2. ���� Ÿ���� ��ġ�� �̹� ī���� �Ǽ��ǿ� ������ Ÿ�� �Ǽ� X
        if (tile.IsBuildTower == true) 
        {
            return;
        }

        // Ÿ���� �Ǽ��Ǿ� �������� ����
        tile.IsBuildTower = true;
        // Ÿ�� �Ǽ��� �ʿ��� ��常ŭ ����
        playerGold.CurrentGold -= towerBulidGold;
        // ������ Ÿ���� ��ġ�� Ÿ�� �Ǽ� (Ÿ�Ϻ��� z�� -1�� ��ġ�� ��ġ)
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerPrefab, position, Quaternion.identity);
        // Ÿ�� ���⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }
}
