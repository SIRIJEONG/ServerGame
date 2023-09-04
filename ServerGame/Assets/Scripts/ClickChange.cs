using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickChange : MonoBehaviour
{
    //플레이어 전방에 있는 오브잭트로 변할 오브잭트에 들어갈 코드

    public Transform player; // 따라갈 플레이어 오브젝트

    private bool change = false;
    private Mesh originMesh;
    private Material originMaterial;

    private void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            originMesh = meshFilter.sharedMesh;
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originMaterial = renderer.sharedMaterial;
            //originShader = originMaterial.shader;
        }
    }
    private void Update()
    {
        if (player != null)
        {
            // 플레이어의 위치를 따라오도록 오브젝트 위치 업데이트
            transform.position = player.position;
        }

        // 변할 오브잭트들의 정보를 가져오기
        Mesh hitMesh = PlayerController.hitMesh;
        Material hitMaterial = PlayerController.hitMaterial;
        Shader hitShader = PlayerController.hitShader;
        GameObject hitObject = PlayerController.hitObject;

        MeshFilter meshFilter = hitObject.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            hitMesh = meshFilter.sharedMesh;
        }

        Renderer renderer = hitObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            hitMaterial = renderer.sharedMaterial;
            hitShader = hitMaterial.shader;
        }

        Quaternion newRotation = Quaternion.Euler(hitObject.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = newRotation;

        ChangePlayer(hitMesh, hitMaterial, hitShader);
        //Debug.Log("Hit Object hitMesh: " + hitMesh);
        //Debug.Log("Hit Object hitMaterial: " + hitMaterial);
        //Debug.Log("Hit Object hitShader: " + hitShader);
    }

    private void ChangePlayer(Mesh newMesh, Material newMaterial, Shader newShader)
    {
        // 현재 외형 비활성화
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.gameObject.SetActive(false);
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Debug.Log("1");
            renderer.gameObject.SetActive(false);
        }

        // 새로운 외형 적용
        meshFilter.sharedMesh = newMesh;

        renderer.sharedMaterial = newMaterial;
        renderer.material.shader = newShader;

        // 새로운 외형 활성화
        meshFilter.gameObject.SetActive(true);
        renderer.gameObject.SetActive(true);
    }

}
