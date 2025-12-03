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
        
    // animation 
    public Transform cube;
    public GameObject purpos;

    Dictionary<int, Vector3> faceDirections = new Dictionary<int, Vector3>()
    {
        { 0, -Vector3.right },  // left
        { 1,  Vector3.forward },// front
        { 2,  Vector3.right },  // right
        { 3, -Vector3.forward } // back
    };
    //Dictionary<int, Vector3> faceDirections = new Dictionary<int, Vector3>()
    //{
    //    { 0, Vector3.back },     // Left → Z-
    //    { 1, Vector3.right },     // Back → X+
    //    { 2, Vector3.forward },  // Right → Z+
    //    { 3, -Vector3.right },   // Front → X-（これが正面）
    //};


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
            Debug.Log("faceNum =" + faceNum);
            Transform purposObject = purpos.transform;

            StartCoroutine(movingAndStart(faceNum, purposObject));
            isSelected = false; 
        }
    }


private IEnumerator movingAndStart(int randomNumber, Transform purposObject)
    {
        //  ‰ñ“]‚³‚¹‚é‘ÎÛ
        Transform Cube = this.transform;

        // ‡@ Œ»Ý‚ÌˆÊ’uE‰ñ“]‚ð‹L˜^
        Vector3 currentPos = Cube.position;

        Quaternion currentRot = Cube.rotation;

        // Cube‚ªŒü‚«‚½‚¢•ûŒü‚ð”Žš‚ð‚à‚Æ‚ÉŒˆ‚ß‚é
        Debug.Log("randomNumber =" + randomNumber);
        Vector3 desiredDirection = faceDirections[randomNumber];
        desiredDirection.Normalize();
        //Debug.Log("Œü‚«‚½‚¢•ûŒü¨" + desiredDirection);

        // ƒ[ƒJƒ‹‚ÌZ+‚ðŠî€‚ÉŽw’è‚Ì•ûŒü‚ÖŒü‚­‚æ‚¤ŽwŽ¦
        Quaternion purRot = Quaternion.LookRotation(desiredDirection, Vector3.up * 1.0f);

        //–Ú•WˆÊ’u
        Vector3 purPos = purpos.transform.position + purpos.transform.forward ;


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
        Debug.Log("Face visible : " + face);
        return face;
    }

    public void NotHolding()
    {
        Debug.Log("Not Holding");
        isHolding = false;
    }

    public void Select() //int faceNum
    { 

        Debug.Log("choose" );
        isSelected = true;

    }
    public void Deselect()
    {
        Debug.Log("not choose");
        isSelected = false;
    }

}
