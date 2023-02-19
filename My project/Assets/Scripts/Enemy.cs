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

        // �� �̵���� WayPoints ���� ����
        wayPointCount       = wayPoints.Length;
        this.wayPoints      = new Transform[wayPointCount];
        this.wayPoints      = wayPoints;

        // ���� ��ġ�� ù��° wayPoints ��ġ�� ����
        transform.position  = wayPoints[currentIndex].position;

        // �� �̵�/��ǥ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        // ���� �̵� ���� ����
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
