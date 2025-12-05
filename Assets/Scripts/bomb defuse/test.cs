using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEditor.Progress;
using System.Runtime.InteropServices;
public class test : MonoBehaviour
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

    public MinigameManager minigame;

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

        int faceNum = DetectFrontFace();

        if (isSelected)
        {
            Debug.Log("test faceNum =" + faceNum);
            minigame.StartMoving(faceNum);
            isSelected = false;
        }
    }

    public void Holding()
    {
        Debug.Log("Holding");
        isHolding = true;
        lastControllerRotation = controller.transform.rotation; // On garde la rotation de départ
    }

    private int DetectFrontFace()
    {

        Vector3 toCamera = (Camera.main.transform.position - transform.position).normalized;

        Vector3[] faceDirections = new Vector3[]
        {
            -transform.right,     // left
            transform.forward,   // front
            transform.right,     // right
            -transform.forward  // back
            //transform.up,        // up
            //-transform.up,       // down
        };

        int[] faceNames = { 0, 1, 2, 3 };

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

        int face = faceNames[bestFaceIndex];
        Debug.Log("Now Face visible : " + face);
        return face;
    }

    public void NotHolding()
    {
        Debug.Log("Not Holding");
        isHolding = false;
    }

    public void Select()
    {

        Debug.Log("choose");
        isSelected = true;

    }
    public void Deselect()
    {
        Debug.Log("not choose");
        isSelected = false;
    }

}
