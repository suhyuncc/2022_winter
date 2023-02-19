using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject    enemyPrefab;
    [SerializeField]
    private GameObject      enemyHPSliderPrefab;    //  적 체력을 나타내는 Slider UI 프리팹
    [SerializeField]
    private Transform       canvasTransform;        // UI를 표현하는 Canvas 오브젝트의 Transform
    //[SerializeField]
    //private float         spawnTime;
    [SerializeField]
    private Transform[]     wayPoints;              // 현재 스테이지의 이동 경로
    [SerializeField]
    private PlayerHP        playerHP;               // 플레이어의 체력 컴포넌트
    [SerializeField]
    private PlayerGold      playerGold;             // 플레이어의 골드 컴포넌트
    private Wave            currentWave;            // 현재 웨이브 정보
    private int             currentEnemyCount;      // 현재 웨이브에 남아있는 적 숫자 (웨이브 시작시 max로 설정, 적 사망시 -1)
    private List<Enemy>     enemyList;              // 현재 맵에 존재하는 모든 적의 정보

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
        // 매개변수로 받아온 웨이브 정보 저장
        currentWave = wave;
        // 현재 웨이브의 최대 적 숫자를 저장
        currentEnemyCount = currentWave.maxEnemyCount;
        // 현재 웨이브 시작
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        // 현재 웨이브에서 생성한 적 숫자
        int spawnEnemyCount = 0;

        //while(true)
        // 현재 웨이브에서 생성되어야 하는 적의 숫자만큼 적을 생성하고 코루틴 종료
        while ( spawnEnemyCount < currentWave.maxEnemyCount)
        {
            //GameObject    clone = Instantiate(enemyPrefab);
            // 웨이브에 등장하는 적의 종류가 여러 종류일 때 임의의 적이 등장하도록 설정하고, 적 오브젝트 생성
            int             enemyIndex      = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject      clone           = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy           enemy = clone.GetComponent<Enemy>();    // 방금 생성된 적의 Enemy 컴포넌트

            // this는 나 자신 (자신의 EnemySpawner 정보)
            enemy.Setup(this, wayPoints);
            enemyList.Add(enemy);

            SpawnEnemyHPSlider(clone);

            // 현재 웨이브에서 생성한 적의 숫자 +1
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
