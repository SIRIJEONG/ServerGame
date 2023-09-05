
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

    // �������� ���� �� ȣ���� �޼���
    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine)
        {
            // ���� �÷��̾ �ƴ� ��쿡�� �������� ó��
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // �÷��̾ ����� ���, ���⿡�� ��� ó�� ������ ����
            Die();
        }
    }

    private void Die()
    {
            Destroy(gameObject);       
        // ��� ó�� ������ ����
        // ���� ���, �÷��̾ �ٽ� ��ȯ�ϰų� ���� ���� ó�� ���� ����
    }
}
