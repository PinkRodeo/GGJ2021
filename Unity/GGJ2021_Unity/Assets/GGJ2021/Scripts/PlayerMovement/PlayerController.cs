using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    private bool _movingDown = true;
    [SerializeField]
    private float ForceIntensity = 10;
    [SerializeField]
    private int MaxSpeed = 50;
    [SerializeField]
    private float RotSpeed = 0.2f;

    public AnimationCurve steering;
    Vector3 mousesaved = new Vector3();
    Vector3 mouseoffset = new Vector3();

    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        mousesaved = Input.mousePosition;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector3 mouseRotRaw = mousesaved - new Vector3(Screen.width /2 , Screen.height/2 , 0);
        Vector3 mEdit = new Vector3(mousesaved.x / Screen.width, mousesaved.y / Screen.height);
        mEdit = new Vector3(steering.Evaluate(mEdit.x), steering.Evaluate(mEdit.y), 0);
        mEdit = mEdit * 2;
        Vector3 norm = new Vector3(mEdit.x - 1, mEdit.y - 1, mEdit.z - 1);
        // Debug.Log(norm);
        //0 0.3 0.6 1
        //
        Vector3 mouseRotNorm = norm;
        transform.Rotate(new Vector3(-mouseRotNorm.y * RotSpeed, mouseRotNorm.x * RotSpeed, 0));
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.rotation.eulerAngles.z);


        if (mousesaved != Input.mousePosition)   // set the mouse saved positon
        {
            mouseoffset = Input.mousePosition - mousesaved;
            mousesaved = Input.mousePosition;

        }
        else
        {
            mouseoffset = mouseoffset * 0.6f; // when not moving mouse, decrease stear ?? will be normalized so doesnt do anything?
        }

        if (_movingDown)
        {
            _rb.AddForce(transform.forward * ForceIntensity, ForceMode.Force);
            _rb.velocity = new Vector3(Mathf.Clamp(_rb.velocity.x, -MaxSpeed, MaxSpeed), Mathf.Clamp(_rb.velocity.y, -MaxSpeed, MaxSpeed), Mathf.Clamp(_rb.velocity.z, -MaxSpeed, MaxSpeed));

            //transform.rotation.eulerAngles.Set()
        }
    }
}
