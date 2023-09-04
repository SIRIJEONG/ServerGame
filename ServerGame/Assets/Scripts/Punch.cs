using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{

    public int punchDamage = 10;

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 플레이어의 펀치로 인한 데미지를 받을 대상인지 확인
        if (other.CompareTag("Player"))
        {
            // 플레이어와 충돌한 상대방에게 데미지를 입히기 위해 상대방의 플레이어 스크립트를 가져옴
            PlayerHealth enemyHealth = other.GetComponent<PlayerHealth>();

            if (enemyHealth != null)
            {
                // 데미지를 입힘
                enemyHealth.TakeDamage(punchDamage);
            }
        }
    }
}
