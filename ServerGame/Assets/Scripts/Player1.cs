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





    private bool change = false;
    private Mesh originMesh;
    private Material originMaterial;
    //private Shader originShader;

    public static GameObject hitObject;
    public static Mesh hitMesh = null;
    public static Material hitMaterial = null;
    public static Shader hitShader = null;

    // ������ �����̴� ������Ʈ

    public GameObject aa;       // �����ϴ� ������Ʈ �����ϴ� ������Ʈ
    public GameObject bb;       // �÷��̾� ������Ʈ



    public static bool isAttack = false;

    private Animator animator;

    private bool isJumping;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {








        if (Input.GetMouseButtonDown(1))
        {


            if (!change)
            {
                Vector3 playerPosition = transform.position;

                // y�� ������ �����Ͽ� ����ĳ��Ʈ ���� ��ġ ����
                Vector3 raycastStartPos = new Vector3(playerPosition.x, playerPosition.y + 0.5f, playerPosition.z);


                RaycastHit hitInfo;

                // ChangeAble�̶� �±װ� �޸� ������Ʈ�θ� ���� ����
                if (Physics.Raycast(raycastStartPos, transform.forward, out hitInfo) && hitInfo.collider.CompareTag("Ground"))
                {
                    // ���� ������Ʈ�� ���� ��������
                    hitObject = hitInfo.collider.gameObject;


                    MeshFilter meshFilter = hitObject.GetComponent<MeshFilter>();
                    if (meshFilter != null)
                    {
                        hitMesh = meshFilter.sharedMesh;
                    }

                    Renderer renderer = hitObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        hitMaterial = renderer.sharedMaterial;
                        //hitShader = hitMaterial.shader;
                    }
                    //ChangePlayer(hitMesh, hitMaterial, hitShader);

                    aa.SetActive(true);
                    bb.SetActive(false);

                    change = true;
                }
            }
            else                    //���� Ǯ���� �ڵ�
            {
                MeshFilter meshFilter = GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    meshFilter.sharedMesh = originMesh;
                }

                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial = originMaterial;
                    //renderer.material.shader = originShader;
                }
                aa.SetActive(false);
                bb.SetActive(true);

                change = false;
            }
        }






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