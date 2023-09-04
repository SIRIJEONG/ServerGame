using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // �ó׸ӽ� ���� �ڵ�
using Photon.Pun; // PUN ���� �ڵ�

public class MainCamera : MonoBehaviourPun
{
    public Transform target;  // �÷��̾� ĳ������ Transform
    public float distance = 5.0f;  // ī�޶�� ĳ���� ������ �Ÿ�
    public float height = 1.0f;  // ī�޶��� ����
    public float smoothSpeed = 10.0f;  // ī�޶� �̵� ������ ��wa

    private Vector3 offset;  // �ʱ� ī�޶�� ĳ������ ������

    private void Start()
    {

        //if (photonView.IsMine)
        //{


        //    CinemachineVirtualCamera followCam = FindAnyObjectByType<CinemachineVirtualCamera>();

        //    followCam.Follow = transform;
        //    followCam.LookAt = target;

        //}

        offset = new Vector3(0f, height, -distance);
    }

    private void LateUpdate()
    {
        // ī�޶� ��ġ ������Ʈ
        Vector3 targetPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // ī�޶� ĳ���͸� �ٶ󺸵��� ȸ��
        transform.LookAt(target);

    }
}

