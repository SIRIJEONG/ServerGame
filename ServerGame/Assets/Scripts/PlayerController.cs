using Photon.Pun;
using UnityEngine;
using UnityEngine.UI; // UI ���ӽ����̽� �߰�

public class PlayerController : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody rb;
    private Animator animator;
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




    Vector3 moveMent = new Vector3();

    public static bool isJumping = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        photonAnimatorView = GetComponent<PhotonAnimatorView>(); // PhotonAnimatorView �ʱ�ȭ

    }


    private void Update()
    {

        Playermoving();



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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            animator.SetBool("Jump", isJumping);
        }
    }


    private void Playermoving()
    {
        // ������ ó��
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
        // �ִϸ��̼� ó��
        bool isRunning = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;
        bool isMovingBackward = verticalInput < -0.1f;


        // ���� ó��
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            animator.SetBool("Jump", true);
        }

        float mouseX = Input.GetAxis("Mouse X");
        if (Mathf.Abs(mouseX) > 0.1f)
        {
            // ȸ�� ������ ����Ͽ� ����
            Vector3 newRotation = transform.eulerAngles + new Vector3(0f, mouseX, 0f);
            transform.eulerAngles = newRotation;
        }
    }
}
