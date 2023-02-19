using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { kill = 0, Arrive };

public class Enemy : MonoBehaviour
{
    private int             wayPointCount;
    private Transform[]     wayPoints;
    private int             currentIndex = 0;
    private Movement2D      movement2D;
    private EnemySpawner    enemySpawner;
    [SerializeField]
    private int             gold = 10;

    public void Setup(EnemySpawner enemySpawner,Transform[] wayPoints)
    {
        movement2D          = GetComponent<Movement2D>();
        this.enemySpawner   = enemySpawner;

        // 적 이동경로 WayPoints 정보 설정
        wayPointCount       = wayPoints.Length;
        this.wayPoints      = new Transform[wayPointCount];
        this.wayPoints      = wayPoints;

        // 적의 위치를 첫번째 wayPoints 위치로 설정
        transform.position  = wayPoints[currentIndex].position;

        // 적 이동/목표지점 설정 코루틴 함수 시작
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        // 다음 이돌 방향 설정
        NextMoveTo();

        while (true)
        {
            transform.Rotate(Vector3.forward * 10);

            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed) 
            {
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        if (currentIndex < wayPoints.Length - 1)
        {
            transform.position = wayPoints[currentIndex].position;

            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            gold = 0;
            //Destroy(gameObject);
            OnDie(EnemyDestroyType.Arrive);
        }
    }

    public void OnDie(EnemyDestroyType type)
    {
        enemySpawner.DestoryEnemy(type, this, gold);
    }
}
