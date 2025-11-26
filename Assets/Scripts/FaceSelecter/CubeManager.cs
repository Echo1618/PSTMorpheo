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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            // Calcul du delta de rotation entre la frame actuelle et la précédente
            Quaternion deltaRotation = controller.transform.rotation * Quaternion.Inverse(lastControllerRotation);

            // Convertir le delta en angles d'Euler
            Vector3 deltaEuler = deltaRotation.eulerAngles;

            // On veut seulement la rotation autour de Y (gauche/droite)
            float deltaYaw = Mathf.DeltaAngle(0, deltaEuler.y) * rotationSpeed;

            // Appliquer la rotation autour de l'axe Y local
            transform.Rotate(Vector3.up, deltaYaw, Space.World);

            // Mettre à jour la dernière rotation connue du contrôleur
            lastControllerRotation = controller.transform.rotation;
        }

        DetectFrontFace();
    }

    public void onClicked(string faceTag)
    {
        StartCoroutine(Animations(faceTag));
        //Debug.Log("CubeManager:" + faceTag);
        GameManager.StartGame(faceTag);
    }

     IEnumerator Animations(string faceTag)
    {
        Transform cube = this.transform;
        Transform cam = MainCamera.transform;

        Vector3 startPos = cube.position;
        Quaternion Rot =cube.rotation;

        Vector3 faceDirect = GetFaceNormal(faceTag);
        Quaternion targetRot = Quaternion.FromToRotation(faceDirect, Vector3.up) * cube.rotation;
        Vector3 targetPos = cam.position + cam.forward * 1.0f + Vector3.down * 0.6f; // �J�����O

        float duration = 1.0f;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duration; //calculation
            float smooth = Mathf.SmoothStep(0, 1, t); //smooth
            cube.position = Vector3.Lerp(startPos, targetPos, smooth); //position
            cube.rotation = Quaternion.Slerp(Rot, targetRot, smooth); // rotation
            yield return null;
        }
    }

    Vector3 GetFaceNormal(string faceTag)
    {
        switch (faceTag)
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
        Debug.Log("Holding");
        isHolding = true;
        lastControllerRotation = controller.transform.rotation; // On garde la rotation de départ
    }

    private void DetectFrontFace()
    {

        Vector3 toCamera = (Camera.main.transform.position - transform.position).normalized;

        Vector3[] faceDirections = new Vector3[]
        {
            transform.forward,   // front
            -transform.forward,  // back
            transform.up,        // up
            -transform.up,       // down
            transform.right,     // right
            -transform.right     // left
        };

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

        // Debug
        Debug.Log("Face visible : " + faceNames[bestFaceIndex]);
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
