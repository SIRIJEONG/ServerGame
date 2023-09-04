using Photon.Pun;
using UnityEngine;
using UnityEngine.UI; // UI 네임스페이스 추가

public class PlayerController : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody rb;
    private Animator animator;
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private PhotonAnimatorView photonAnimatorView; // PhotonAnimatorView 변수 추가

    private bool change = false;
    private Mesh originMesh;
    private Material originMaterial;
    //private Shader originShader;

    public static GameObject hitObject;
    public static Mesh hitMesh = null;
    public static Material hitMaterial = null;
    public static Shader hitShader = null;

    // 실제로 움직이는 오브잭트

    public GameObject aa;       // 변신하는 오브젝트 저장하는 오브잭트
    public GameObject bb;       // 플레이어 오브젝트




    Vector3 moveMent = new Vector3();

    public static bool isJumping = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        photonAnimatorView = GetComponent<PhotonAnimatorView>(); // PhotonAnimatorView 초기화

    }


    private void Update()
    {

        Playermoving();



        if (Input.GetMouseButtonDown(1))
        {


            if (!change)
            {
                Vector3 playerPosition = transform.position;

                // y값 오프셋 적용하여 레이캐스트 시작 위치 생성
                Vector3 raycastStartPos = new Vector3(playerPosition.x, playerPosition.y + 0.5f, playerPosition.z);


                RaycastHit hitInfo;

                // ChangeAble이란 태그가 달린 오브잭트로만 변신 가능
                if (Physics.Raycast(raycastStartPos, transform.forward, out hitInfo) && hitInfo.collider.CompareTag("Ground"))
                {
                    // 맞은 오브젝트의 정보 가져오기
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
            else                    //변신 풀리는 코드
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
        // 움직임 처리
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
        // 애니메이션 처리
        bool isRunning = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;
        bool isMovingBackward = verticalInput < -0.1f;


        // 점프 처리
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            animator.SetBool("Jump", true);
        }

        float mouseX = Input.GetAxis("Mouse X");
        if (Mathf.Abs(mouseX) > 0.1f)
        {
            // 회전 각도를 계산하여 적용
            Vector3 newRotation = transform.eulerAngles + new Vector3(0f, mouseX, 0f);
            transform.eulerAngles = newRotation;
        }
    }
}
