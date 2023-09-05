
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // 데미지를 입을 때 호출할 메서드
    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine)
        {
            // 로컬 플레이어가 아닌 경우에만 데미지를 처리
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // 플레이어가 사망한 경우, 여기에서 사망 처리 로직을 수행
            Die();
        }
    }

    private void Die()
    {
            Destroy(gameObject);       
        // 사망 처리 로직을 구현
        // 예를 들어, 플레이어를 다시 소환하거나 게임 오버 처리 등을 수행
    }
}
