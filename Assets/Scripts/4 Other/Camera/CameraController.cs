using UnityEngine;
public class CameraController : MonoBehaviour
{

    #region Variables
    public Transform cameraTransform;

    [Space(10)]
    [Header("Camera Movement")]
    public bool disableAllMovement;
    public bool disableLateralMovement;
    public bool disableMousePan;
    [Space(10)]
    [Range(0, 10)]
    public float movementTime;
    [Range(0, 1)]
    public float normalSpeed;
    [Range(0, 5)]
    public float fastSpeed;
    private float movementSpeed;
    [Range(0, 100)]
    public float mousePanBorderThickness;
    Vector3 newPosition;

    [Space(10)]
    [Header("Zoom")]
    public bool disableZoom;
    public bool DisableDynamicZoom;
    private bool disabledZoom;
    [Range(0, 1)]
    public float dynamicZoomSpeed;
    [Space(10)]
    public Vector3 zoomAmountNormal;
    public Vector3 zoomAmountFast;
    Vector3 newZoom;

    [Space(10)]
    [Header("Rotation")]
    public bool disableRotation;
    public bool disableRotationRevert;
    bool isReverting;
    Vector3 rotateStartPosition;
    Vector3 rotateEndPosition;
    Quaternion newRotation;

    [Space(10)]
    [Header("Bounds")]
    [Space(10)]
    [Range(-250, 250)]
    public float minHeight;
    [Range(-250, 250)]
    public float maxHeight;
    [Space(10)]
    [Range(0, 750)]
    public float topBounds; //+z
    [Range(-750, 0)]
    public float bottomBounds; //-z
    [Space(10)]
    [Range(-750, 0)]
    public float leftBounds; //-x
    [Range(0, 750)]
    public float rightBounds; //+x

    [Space(10)]
    [Header("Debug")]
    public bool enableLateralMovementDebug;
    public bool enableZoomDebug;
    public bool enableRotationDebug;
    #endregion

    void Start()
    {
        Application.targetFrameRate = 300;  //Framerate set

        newPosition = transform.position;
        newZoom = cameraTransform.localPosition;
        newRotation = transform.rotation;
    }

    private void Update()
    {
        #region
        if (newPosition.z >= topBounds)
        {
            newPosition.z = topBounds;
        }
        if (newPosition.z <= bottomBounds)
        {
            newPosition.z = bottomBounds;
        }
        if (newPosition.x <= leftBounds)
        {
            newPosition.x = leftBounds;
        }
        if (newPosition.x >= rightBounds)
        {
            newPosition.x = rightBounds;
        }
        #endregion

        if (disableAllMovement == false)
        {
            LateralMovement();
            Zoom();
            Rotation();
        }

        if (enableLateralMovementDebug == true || enableZoomDebug == true || enableRotationDebug == true)
        {
            Debugg();
        }
    }

    void LateralMovement()
    {
        if (disableLateralMovement == false)
        {
            if (Inputs.ShiftHold())
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - mousePanBorderThickness && disableMousePan == false)
            {
                newPosition += (transform.forward * movementSpeed);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= mousePanBorderThickness && disableMousePan == false)
            {
                newPosition += (transform.forward * -movementSpeed);        
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= mousePanBorderThickness && disableMousePan == false)
            {
                newPosition += (transform.right * -movementSpeed);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - mousePanBorderThickness && disableMousePan == false)
            {
                newPosition += (transform.right * movementSpeed);
            }

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime); //set position
        }
    }

    void Zoom()
    {
        if (disableZoom == false)
        {

            if (disabledZoom == true)
            {
                DisableDynamicZoom = false;
                disabledZoom = false;
            }

            if (Input.mouseScrollDelta.y != 0) //Zoom calculation
            {
                if (newZoom.y <= minHeight) //min y bounds
                {
                    newZoom.y = minHeight; newZoom.z = -minHeight;
                }

                if (newZoom.y >= maxHeight) //max y bounds
                {
                    newZoom.y = maxHeight; newZoom.z = -maxHeight;
                }

                if (Inputs.ShiftHold())
                {
                    newZoom += (Input.mouseScrollDelta.y) * zoomAmountFast;
                }
                else
                {
                    newZoom += (Input.mouseScrollDelta.y) * zoomAmountNormal;
                }

                if (DisableDynamicZoom == false)
                {
                    newZoom += (Input.mouseScrollDelta.y) * (zoomAmountNormal * transform.position.y) * dynamicZoomSpeed;
                }
            }

           
        }

        if (disableZoom == true)
        {
            newZoom.y = 15; newZoom.z = -15;
            DisableDynamicZoom = true;
            disabledZoom = true;
        }
    }

    void Rotation()
    {
        if (disableRotation == false)
        {
            if (Inputs.MMBDown() || Input.GetKeyDown(KeyCode.LeftAlt)) //Rotation calculation 1
            {
                rotateStartPosition = Input.mousePosition;
            }

            if (Inputs.MMBHold() || Input.GetKey(KeyCode.LeftAlt)) //Rotation calculation 2
            {
                rotateEndPosition = Input.mousePosition;

                Vector3 rotationDifference = rotateStartPosition - rotateEndPosition;

                rotateStartPosition = rotateEndPosition;

                newRotation *= Quaternion.Euler(Vector3.up * (-rotationDifference.x / 5f));
            }

            if (disableRotationRevert == false)

                if (isReverting == false)
                {
                    if (Inputs.MMBUp() || Input.GetKeyUp(KeyCode.LeftAlt))
                    {
                        isReverting = true;
                        newRotation.y = 0;
                        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * (movementTime * Mathf.Abs(transform.rotation.y) * 10)); //set rotation
                        isReverting = false;
                    }
                }
        }

        if (disableRotation == true)
        {
            newRotation.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * (movementTime * 20)); //set rotation
        }
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime); //set zoom position
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime); //set rotation         
    }

    void Debugg()
    {
        if (Input.anyKeyDown)
        {
            if (enableLateralMovementDebug == true)
            {
                Debug.Log("Lateral movement: " + newPosition);
            }


            if (enableZoomDebug == true)
            {
                Debug.Log("Zoom: " + newZoom);
            }


            if (enableRotationDebug == true)
            {
                Debug.Log("Rotation: " + newRotation);
            }
        }
    }
}

    
