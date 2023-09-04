using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Player1 : MonoBehaviour
{
    // �ִϸ��̼��� �ִ� �÷��̾� ������Ʈ

    public Transform player; // ���� �÷��̾� ������Ʈ
    public GameObject attackHitBox;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody rb;
    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private PhotonAnimatorView photonAnimatorView; // PhotonAnimatorView ���� �߰�

    bool isFireReady;
    float fireDelay;





    public static bool isAttack = false;

    private Animator animator;

    private bool isJumping;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        isJumping = PlayerController.isJumping;
        animator.SetBool("Jump", isJumping);

        //Debug.Log(isJumping);
        if (player != null)
        {
            // �÷��̾��� ��ġ�� ��������� ������Ʈ ��ġ ������Ʈ
            transform.position = player.position;
        }
        // ������ ó��
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
        // �ִϸ��̼� ó��
        bool isRunning = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;
        bool isMovingBackward = verticalInput < -0.1f;



        animator.SetFloat("xDir", horizontalInput);
        animator.SetFloat("yDir", verticalInput);


        // ���� ó��
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            animator.SetBool("Jump", true);
        }



        // ��ġ �ִϸ��̼��� �����ϴ� �ڷ�ƾ ����
        if (Input.GetMouseButtonDown(0) && !isAttack)
        {
            StartCoroutine(PerformPunch());
        }



    }

    IEnumerator PerformPunch()
    {
        // ��ġ �ִϸ��̼��� ����
        animator.SetBool("Punch", true);
        isAttack = true;

        // ���⼭ �ִϸ��̼��� ���̳� Ư�� �̺�Ʈ�� ��ٸ� �� �ֽ��ϴ�.
        // ���� ���, �ִϸ��̼��� ���̸�ŭ ����Ϸ���:
        float punchAnimationLength = 1f;// ��ġ �ִϸ��̼��� ���̸� ���;� �մϴ�.
        yield return new WaitForSeconds(punchAnimationLength);

        // ��ġ �ִϸ��̼� ����
        animator.SetBool("Punch", false);
        isAttack = false;
    }



}