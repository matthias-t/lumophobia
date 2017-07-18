using UnityEngine;
using System.Collections;
 
 public class World : MonoBehaviour
{
    public static bool start;

    public float dragSpeed = 0.4f;
    public float rotateSpeed = 2f;
    public GameObject player;

    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private float rotation;
    private bool _isRotating;

    void Update()
    {
        if (!start) return;
        transform.Rotate(new Vector3(-rotateSpeed, 0, 0) * Time.deltaTime, Space.World);
        if (_isRotating) 
        {
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);
            // apply rotation
            rotation = -(_mouseOffset.x) * dragSpeed;
            //transform.Rotate(_rotation);
            transform.Rotate(new Vector3(0f, rotation, 0f), Space.World);
            // store mouse
            _mouseReference = Input.mousePosition;
        }
    }

    void OnMouseDown()
    {
        start = true;
        Player.text.gameObject.SetActive(false);

        Debug.Log("ok");
        // rotating flag
        _isRotating = true;

        // store mouse
        _mouseReference = Input.mousePosition;
    }

    void OnMouseUp()
    {
        // rotating flag
        _isRotating = false;
    }

}
