using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObjectGrab : MonoBehaviour
{

    public float forceAmount = 500;

    Rigidbody grabRigid;
    Camera mainCam;
    Vector3 originalScreenPos;
    Vector3 originalRigidbodyPos;
    float cursorSelectDistance;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GetComponent<Camera>();
    }

    void Update()
    {
        if (!mainCam)
            return;
        //Check if we are hovering over a rigidbody and if we are then select it
        if (Input.GetMouseButtonDown(0))
        {           
            grabRigid = GetRigidbodyFromMouseClick();
        }
        //Release object when letting go of mouse button
        if (Input.GetMouseButtonUp(0) && grabRigid)
        {   
            grabRigid = null;
        }
    }

    void FixedUpdate()
    {
        //calculations for rigigd body to follow mouse position when grabbed
        if (grabRigid)
        {
            Vector3 mouseOffset = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cursorSelectDistance)) - originalScreenPos;
            grabRigid.velocity = (originalRigidbodyPos + mouseOffset - grabRigid.transform.position) * forceAmount * Time.deltaTime;
        }
    }

    Rigidbody GetRigidbodyFromMouseClick()
    {
        RaycastHit rayData = new RaycastHit();
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out rayData);
        if (hit)
        {
            if (rayData.collider.gameObject.GetComponent<Rigidbody>())
            {
                cursorSelectDistance = Vector3.Distance(ray.origin, rayData.point);
                originalScreenPos = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cursorSelectDistance));
                originalRigidbodyPos = rayData.collider.transform.position;
                return rayData.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        return null;
    }
}

