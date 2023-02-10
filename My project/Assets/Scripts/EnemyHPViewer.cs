using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPViewer : MonoBehaviour
{
    private EnemyHp enemyHp;
    private Slider hpSlider;

    public void Setup(EnemyHp enemyHp)
    {
        this.enemyHp = enemyHp;
        hpSlider = GetComponent<Slider>();
    }
    void Update()
    {
        hpSlider.value = enemyHp.CurrentHP / enemyHp.MaxHP;
    }


}
