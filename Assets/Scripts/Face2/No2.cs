using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class No2 : MonoBehaviour
{
    public Transform cube;
    //public ExampleScript exampleScript;
    public GameObject purpos;

    Dictionary<int, Vector3> faceDirections = new Dictionary<int, Vector3>()
    {
        { 0, Vector3.right },
        { 1, Vector3.forward },
        { 2, Vector3.left },
        { 3, Vector3.back }
    };

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
        // 回転させる対象
        Transform Cube = this.transform;

        // ① 現在の位置・回転を記録
        Vector3 currentPos = Cube.position;
        Quaternion currentRot = Cube.rotation;

        // Cubeが向きたい方向を数字をもとに決める
        Vector3 desiredDirection = faceDirections[randomNumber];
        desiredDirection.Normalize();
        //Debug.Log("向きたい方向→" + desiredDirection);

        // ローカルのZ+を基準に指定の方向へ向くよう指示
        Quaternion purRot = Quaternion.LookRotation(desiredDirection, Vector3.up);

        // 目標位置
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

}
