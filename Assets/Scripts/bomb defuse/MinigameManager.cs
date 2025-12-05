using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinigameManager : MonoBehaviour
{
    // animation
    public GameObject purpos;
    public Transform bomb;

    Dictionary<int, Vector3> faceDirections = new Dictionary<int, Vector3>()
    {
        { 0, -Vector3.right },  // left
        { 1,  Vector3.forward },// front
        { 2,  Vector3.right },  // right
        { 3, -Vector3.forward } // back
    };

    public void StartMoving(int faceIndex)
    {
        Debug.Log("MinigameManager, faceIndex =" + faceIndex);
        Transform purposObject = purpos.transform;

        StartCoroutine(movingAndStart(purposObject, faceIndex));
    }

    private IEnumerator movingAndStart(Transform purposObject, int randomNumber)
    {
        //  回転させる対象
        Transform Cube = bomb.transform;

        // ① 現在の位置・回転を記録
        Vector3 currentPos = Cube.position;
        Quaternion currentRot = Cube.rotation;

        // Cubeが向きたい方向を数字をもとに決める
        Debug.Log("randomNumber =" + randomNumber);
        Vector3 desiredDirection = faceDirections[randomNumber];
        desiredDirection.Normalize();
        //Debug.Log("向きたい方向→" + desiredDirection);

        // ローカルのZ+を基準に指定の方向へ向くよう指示
        Quaternion purRot = Quaternion.LookRotation(desiredDirection, Vector3.up * 1.0f);

        //目標位置
        Vector3 purPos = purpos.transform.position + purpos.transform.forward;


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

}
