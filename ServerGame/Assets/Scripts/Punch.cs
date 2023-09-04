using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{

    public int punchDamage = 10;

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �÷��̾��� ��ġ�� ���� �������� ���� ������� Ȯ��
        if (other.CompareTag("Player"))
        {
            // �÷��̾�� �浹�� ���濡�� �������� ������ ���� ������ �÷��̾� ��ũ��Ʈ�� ������
            PlayerHealth enemyHealth = other.GetComponent<PlayerHealth>();

            if (enemyHealth != null)
            {
                // �������� ����
                enemyHealth.TakeDamage(punchDamage);
            }
        }
    }
}
