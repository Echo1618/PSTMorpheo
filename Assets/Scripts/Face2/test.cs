using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEditor.Progress;
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

    // animation 
    public Transform cube;
    public GameObject purpos;

    Dictionary<int, Vector3> faceDirections = new Dictionary<int, Vector3>()
    {
        { 0, Vector3.right },
        { 1, Vector3.forward },
        { 2, Vector3.left },
        { 3, Vector3.back }
    };

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

    public void receive(int randomNumber, bool randombool)
    {
        if (randombool == true)
        {
            Transform purposObject = purpos.transform;

            Debug.Log("ƒ^[ƒQƒbƒg–Ê = " + randomNumber);

            StartCoroutine(movingAndStart(randomNumber, purposObject));
            //target.SendMessage("OnCommandReceived");
        }
    }

    private IEnumerator movingAndStart(int randomNumber, Transform purposObject)
    {
        //  ‰ñ“]‚³‚¹‚é‘ÎÛ
        Transform Cube = this.transform;

        // ‡@ Œ»İ‚ÌˆÊ’uE‰ñ“]‚ğ‹L˜^
        Vector3 currentPos = Cube.position;

        Quaternion currentRot = Cube.rotation;

        // Cube‚ªŒü‚«‚½‚¢•ûŒü‚ğ”š‚ğ‚à‚Æ‚ÉŒˆ‚ß‚é
        Vector3 desiredDirection = faceDirections[randomNumber];
        desiredDirection.Normalize();
        //Debug.Log("Œü‚«‚½‚¢•ûŒü¨" + desiredDirection);

        // ƒ[ƒJƒ‹‚ÌZ+‚ğŠî€‚Éw’è‚Ì•ûŒü‚ÖŒü‚­‚æ‚¤w¦
        Quaternion purRot = Quaternion.LookRotation(desiredDirection, Vector3.up);

        //–Ú•WˆÊ’u
        Vector3 purPos = purpos.transform.position + purpos.transform.forward * 1.0f;


        float duration = 1.0f;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            float smooth = Mathf.SmoothStep(0, 1, t);

            Cube.position = Vector3.Lerp(currentPos, purPos, smooth);
            Cube.rotation = Quaternion.Slerp(currentRot, purRot, smooth);
            yield return null;
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
        Debug.Log("choose");
        isSelected = true;
    }
    public void Deselect()
    {
        Debug.Log("not choose");
        isSelected = false;
    }

}
