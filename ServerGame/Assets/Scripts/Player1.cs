using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Player1 : MonoBehaviour
{
    // 애니메이션이 있는 플레이어 오브잭트

    public Transform player; // 따라갈 플레이어 오브젝트
    public GameObject attackHitBox;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody rb;
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






        isJumping = PlayerController.isJumping;
        animator.SetBool("Jump", isJumping);

        //Debug.Log(isJumping);
        if (player != null)
        {
            // 플레이어의 위치를 따라오도록 오브젝트 위치 업데이트
            transform.position = player.position;
        }
        // 움직임 처리
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
        // 애니메이션 처리
        bool isRunning = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;
        bool isMovingBackward = verticalInput < -0.1f;



        animator.SetFloat("xDir", horizontalInput);
        animator.SetFloat("yDir", verticalInput);


        // 점프 처리
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            animator.SetBool("Jump", true);
        }



        // 펀치 애니메이션을 실행하는 코루틴 시작
        if (Input.GetMouseButtonDown(0) && !isAttack)
        {
            StartCoroutine(PerformPunch());
        }



    }

    IEnumerator PerformPunch()
    {
        // 펀치 애니메이션을 시작
        animator.SetBool("Punch", true);
        isAttack = true;

        // 여기서 애니메이션의 길이나 특정 이벤트를 기다릴 수 있습니다.
        // 예를 들어, 애니메이션의 길이만큼 대기하려면:
        float punchAnimationLength = 1f;// 펀치 애니메이션의 길이를 얻어와야 합니다.
        yield return new WaitForSeconds(punchAnimationLength);

        // 펀치 애니메이션 종료
        animator.SetBool("Punch", false);
        isAttack = false;
    }



}