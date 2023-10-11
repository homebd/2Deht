using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float xRotateSpeed = 100f;
    [SerializeField] private float yRotateSpeed = 100f;

    private Camera eyes;
    // Start is called before the first frame update

    private void Awake() {
        eyes = Camera.main;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * hAxis + transform.forward * vAxis;

        transform.position += moveDirection * Time.deltaTime * moveSpeed;
    }

    void Rotate()
    {
        float xRotate = -Input.GetAxis("Mouse Y");
        float yRotate = Input.GetAxis("Mouse X");

        Vector3 rotateBody = new Vector3(0, yRotate * yRotateSpeed, 0);
        Vector3 rotateHead = new Vector3(Mathf.Clamp(xRotate * xRotateSpeed, -60, 60), 0, 0);

        transform.eulerAngles += rotateBody * Time.deltaTime;
        eyes.transform.eulerAngles += rotateHead * Time.deltaTime;
    }
}

