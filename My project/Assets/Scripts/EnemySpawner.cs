using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject    enemyPrefab;
    [SerializeField]
    private GameObject      enemyHPSliderPrefab;    //  �� ü���� ��Ÿ���� Slider UI ������
    [SerializeField]
    private Transform       canvasTransform;        // UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    //[SerializeField]
    //private float         spawnTime;
    [SerializeField]
    private Transform[]     wayPoints;              // ���� ���������� �̵� ���
    [SerializeField]
    private PlayerHP        playerHP;               // �÷��̾��� ü�� ������Ʈ
    [SerializeField]
    private PlayerGold      playerGold;             // �÷��̾��� ��� ������Ʈ
    private Wave            currentWave;            // ���� ���̺� ����
    private int             currentEnemyCount;      // ���� ���̺꿡 �����ִ� �� ���� (���̺� ���۽� max�� ����, �� ����� -1)
    private List<Enemy>     enemyList;              // ���� �ʿ� �����ϴ� ��� ���� ����

    public List<Enemy>      EnemyList => enemyList;

    public int              CurrentEnemyCount => currentEnemyCount;
    public int              MaxEnemyCount => currentWave.maxEnemyCount;

    private void Awake()
    {
        enemyList = new List<Enemy>();
        //StartCoroutine("SpawnEnemy");
    }

    public void StartWave(Wave wave)
    {
        // �Ű������� �޾ƿ� ���̺� ���� ����
        currentWave = wave;
        // ���� ���̺��� �ִ� �� ���ڸ� ����
        currentEnemyCount = currentWave.maxEnemyCount;
        // ���� ���̺� ����
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        // ���� ���̺꿡�� ������ �� ����
        int spawnEnemyCount = 0;

        //while(true)
        // ���� ���̺꿡�� �����Ǿ�� �ϴ� ���� ���ڸ�ŭ ���� �����ϰ� �ڷ�ƾ ����
        while ( spawnEnemyCount < currentWave.maxEnemyCount)
        {
            //GameObject    clone = Instantiate(enemyPrefab);
            // ���̺꿡 �����ϴ� ���� ������ ���� ������ �� ������ ���� �����ϵ��� �����ϰ�, �� ������Ʈ ����
            int             enemyIndex      = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject      clone           = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy           enemy = clone.GetComponent<Enemy>();    // ��� ������ ���� Enemy ������Ʈ

            // this�� �� �ڽ� (�ڽ��� EnemySpawner ����)
            enemy.Setup(this, wayPoints);
            enemyList.Add(enemy);

            SpawnEnemyHPSlider(clone);

            // ���� ���̺꿡�� ������ ���� ���� +1
            spawnEnemyCount++;

            //yield return new WaitForSeconds(spawnTime);
            yield return new WaitForSeconds(currentWave.spawnTime);

        }
    }

    public void DestoryEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        if(type == EnemyDestroyType.Arrive)
        {
            playerHP.TakeDamage(1);
        }
        else if (type == EnemyDestroyType.kill)
        {
            playerGold.CurrentGold += gold;
        }

        currentEnemyCount--;
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);

        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.localScale = Vector3.one;

        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);

        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHp>());
    }
}
