using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickChange : MonoBehaviour
{
    //�÷��̾� ���濡 �ִ� ������Ʈ�� ���� ������Ʈ�� �� �ڵ�

    public Transform player; // ���� �÷��̾� ������Ʈ

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
            // �÷��̾��� ��ġ�� ��������� ������Ʈ ��ġ ������Ʈ
            transform.position = player.position;
        }

        // ���� ������Ʈ���� ������ ��������
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
        // ���� ���� ��Ȱ��ȭ
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

        // ���ο� ���� ����
        meshFilter.sharedMesh = newMesh;

        renderer.sharedMaterial = newMaterial;
        renderer.material.shader = newShader;

        // ���ο� ���� Ȱ��ȭ
        meshFilter.gameObject.SetActive(true);
        renderer.gameObject.SetActive(true);
    }

}
