using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CubeManager : MonoBehaviour
{
    public Camera MainCamera;
    public GameManager GameManager;

    // Telekinesis variables
    public GameObject controller;
    private Quaternion lastControllerRotation;
    public float rotationSpeed = 1f;
    public bool isHolding = false;

    //Selection variables
    public bool isSelected = false;

    // Cached/runtime state
    private Camera cachedCamera;
    private int lastFaceIndex = -1;
    private Coroutine animationCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        cachedCamera = MainCamera ? MainCamera : Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding && controller != null)
        {
            // Compute yaw robustly from controller forward vectors projected to horizontal plane
            Quaternion currentRotation = controller.transform.rotation;

            Vector3 lastForward = lastControllerRotation * Vector3.forward;
            Vector3 currentForward = currentRotation * Vector3.forward;

            Vector3 lastProj = Vector3.ProjectOnPlane(lastForward, Vector3.up);
            Vector3 currProj = Vector3.ProjectOnPlane(currentForward, Vector3.up);

            if (lastProj.sqrMagnitude > 0.0001f && currProj.sqrMagnitude > 0.0001f)
            {
                lastProj.Normalize();
                currProj.Normalize();
                float deltaYaw = Vector3.SignedAngle(lastProj, currProj, Vector3.up);
                // rotationSpeed is treated as degrees per second scale
                float appliedYaw = deltaYaw * rotationSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, appliedYaw, Space.World);
            }

            // Update last rotation
            lastControllerRotation = currentRotation;
        }

        DetectFrontFace();
    }

    public void onClicked(string faceTag)
    {
        // Stop any running animation before starting a new one
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        animationCoroutine = StartCoroutine(Animations(faceTag));

        if (GameManager != null)
            GameManager.StartGame(faceTag);
        else
            Debug.LogWarning("GameManager reference is null in CubeManager.onClicked");
    }

    IEnumerator Animations(string faceTag)
    {
        if (cachedCamera == null)
            cachedCamera = MainCamera ? MainCamera : Camera.main;

        if (cachedCamera == null)
        {
            Debug.LogWarning("No camera found for animation; aborting animation.");
            yield break;
        }

        Transform cube = this.transform;
        Transform cam = cachedCamera.transform;

        Vector3 startPos = cube.position;
        Quaternion Rot = cube.rotation;

        Vector3 faceDirect = GetFaceNormal(faceTag);
        Quaternion targetRot = Quaternion.FromToRotation(faceDirect, Vector3.up) * cube.rotation;
        Vector3 targetPos = cam.position + cam.forward * 1.0f + Vector3.down * 0.6f;

        float duration = 1.0f;
        float t = 0;
        while (t < 1f)
        {
            // If the cube is grabbed while animating, stop the animation.
            if (isHolding)
                yield break;

            t += Time.deltaTime / duration;
            float smooth = Mathf.SmoothStep(0, 1, t);
            cube.position = Vector3.Lerp(startPos, targetPos, smooth);
            cube.rotation = Quaternion.Slerp(Rot, targetRot, smooth);
            yield return null;
        }

        animationCoroutine = null;
    }

    Vector3 GetFaceNormal(string faceTag)
    {
        // Expecting lowercase tags: forward, back, up, down, left, right
        switch (faceTag.ToLowerInvariant())
        {
            case "up": return transform.up;
            case "down": return -transform.up;
            case "forward": return transform.forward;
            case "back": return -transform.forward;
            case "left": return -transform.right;
            case "right": return transform.right;
            default: return transform.forward;
        }
    }
    
    public void Holding()
    {
        if (controller == null)
        {
            Debug.LogWarning("Controller reference is null in Holding()");
            return;
        }

        // Stop any running animation when the user grabs the cube
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        isHolding = true;
        Debug.Log("Holding");
        lastControllerRotation = controller.transform.rotation; // initial controller rotation
    }

    private void DetectFrontFace()
    {

        if (cachedCamera == null)
            cachedCamera = MainCamera ? MainCamera : Camera.main;

        if (cachedCamera == null)
            return;

        Vector3 toCamera = (cachedCamera.transform.position - transform.position).normalized;

        Vector3[] faceDirections = new Vector3[]
        {
            transform.forward,   // front
            -transform.forward,  // back
            transform.up,        // up
            -transform.up,       // down
            transform.right,     // right
            -transform.right     // left
        };

        string[] faceTags = { "forward", "back", "up", "down", "right", "left" };
        string[] faceNames = { "Front", "Back", "Top", "Bottom", "Right", "Left" };

        float bestDot = -1f;
        int bestFaceIndex = 0;

        for (int i = 0; i < faceDirections.Length; i++)
        {
            float dot = Vector3.Dot(faceDirections[i], toCamera);
            if (dot > bestDot)
            {
                bestDot = dot;
                bestFaceIndex = i;
            }
        }

        if (bestFaceIndex != lastFaceIndex)
        {
            lastFaceIndex = bestFaceIndex;
            Debug.Log("Face visible : " + faceNames[bestFaceIndex] + " (" + faceTags[bestFaceIndex] + ")");
        }
    }

    public void NotHolding()
    {
        Debug.Log("Not Holding");
        isHolding = false;
    }

    public void Select()
    {
        isSelected = true;
    }
    public void Deselect()
    {
        isSelected = false;
    }
}
