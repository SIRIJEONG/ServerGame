using Photon.Pun;
using UnityEngine;
using UnityEngine.UI; // UI ���� �ڵ�

// �÷��̾� ĳ������ ����ü�μ��� ������ ���
public class UserHealth : LivingEntity
{
    public Slider healthSlider; // ü���� ǥ���� UI �����̴�

    public AudioClip deathClip; // ��� �Ҹ�
    public AudioClip hitClip; // �ǰ� �Ҹ�
    public AudioClip itemPickupClip; // ������ ���� �Ҹ�

    private AudioSource playerAudioPlayer; // �÷��̾� �Ҹ� �����
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����

    private PlayerController playerMovement; // �÷��̾� ������ ������Ʈ
    //private PlayerShooter playerShooter; // �÷��̾� ���� ������Ʈ

    private void Awake()
    {
        // ����� ������Ʈ�� ��������
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerMovement = GetComponent<PlayerController>();
        //playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        // LivingEntity�� OnEnable() ���� (���� �ʱ�ȭ)
        base.OnEnable();

        // ü�� �����̴� Ȱ��ȭ
        healthSlider.gameObject.SetActive(true);
        // ü�� �����̴��� �ִ밪�� �⺻ ü�°����� ����
        healthSlider.maxValue = startingHealth;
        // ü�� �����̴��� ���� ���� ü�°����� ����
        healthSlider.value = health;

        // �÷��̾� ������ �޴� ������Ʈ�� Ȱ��ȭ
        playerMovement.enabled = true;
        //playerShooter.enabled = true;
    }

    // ü�� ȸ��
    [PunRPC]
    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity�� RestoreHealth() ���� (ü�� ����)
        base.RestoreHealth(newHealth);
        // ü�� ����
        healthSlider.value = health;
    }


    // ������ ó��
    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint,
        Vector3 hitDirection)
    {
        if (!dead)
        {
            // ������� ���� ��쿡�� ȿ������ ���
            playerAudioPlayer.PlayOneShot(hitClip);
        }

        // LivingEntity�� OnDamage() ����(������ ����)
        base.OnDamage(damage, hitPoint, hitDirection);
        // ���ŵ� ü���� ü�� �����̴��� �ݿ�
        healthSlider.value = health;
    }

    public override void Die()
    {
        // LivingEntity�� Die() ����(��� ����)
        base.Die();

        // ü�� �����̴� ��Ȱ��ȭ
        healthSlider.gameObject.SetActive(false);

        // ����� ���
        playerAudioPlayer.PlayOneShot(deathClip);

        // �ִϸ������� Die Ʈ���Ÿ� �ߵ����� ��� �ִϸ��̼� ���
        playerAnimator.SetTrigger("Die");

        // �÷��̾� ������ �޴� ������Ʈ�� ��Ȱ��ȭ
        playerMovement.enabled = false;
        //playerShooter.enabled = false;

        // 5�� �ڿ� ������
        Invoke("Respawn", 5f);
    }



    // ��Ȱ ó��
    public void Respawn()
    {
        // ���� �÷��̾ ���� ��ġ�� ���� ����
        if (photonView.IsMine)
        {
            // �������� �ݰ� 5���� ������ ������ ��ġ ����
            Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
            // ���� ��ġ�� y���� 0���� ����
            randomSpawnPos.y = 0f;

            // ������ ���� ��ġ�� �̵�
            transform.position = randomSpawnPos;
        }

        // ������Ʈ���� �����ϱ� ���� ���� ������Ʈ�� ��� ���ٰ� �ٽ� �ѱ�
        // ������Ʈ���� OnDisable(), OnEnable() �޼��尡 �����
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}