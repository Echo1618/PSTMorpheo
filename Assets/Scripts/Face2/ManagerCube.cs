using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCube : MonoBehaviour
{
    public examplee examplee;
    public GameObject example;
    public GameObject[] targetObjects;

    private Dictionary<string, Vector3> faceTable = new Dictionary<string, Vector3>()
    {
        { "0", Vector3.forward },
        { "1", Vector3.right },
        { "2", Vector3.back },
        { "3", Vector3.left }
    };
    public void recieve(int randomNumber, bool randombool)
    {
        if (randombool == true)
        {
            Debug.Log("受け取ったナンバー：" + randomNumber);
            GameObject target = targetObjects[randomNumber];
            Debug.Log("タグ名：" + target.tag);
            StartCoroutine(movingAndStart(target));

            target.SendMessage("OnCommandReceived");
        }
    }

    private IEnumerator movingAndStart(GameObject target)  
    {
        string tags = target.tag;
        //Debug.Log(" name : " + target.name + " tag : " + target.tag + " transform : " + target.transform);
        //// name : face_2 tag : 2 transform : face_2 (UnityEngine.Transform)

        //  回転の対象物
        Transform Cube = this.transform;


        //対象物の現在位置・角度
        Vector3 currentPos = Cube.position;
        Quaternion currentRot = Cube.rotation;
        Debug.Log("Cube位置:"+currentPos + " 角度:" + currentRot);


        //目標位置
        Vector3 purPos = example.transform.position + example.transform.forward * 1.5f + Vector3.down *0.6f;

        Vector3 currentForward = Cube.transform.forward;

        // ② face の本来の向きを取得
        Vector3 targetDirection = faceTable[target.tag];

        // ③ 現在→目標方向への回転
        Quaternion addRot = Quaternion.FromToRotation(currentForward, targetDirection);

        // ④ 現在の回転に足す
        Quaternion purRot = addRot * Cube.rotation;

        float duration = 1.0f;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration; //calculation
            float smooth = Mathf.SmoothStep(0, 1, t); //smooth
            transform.position = Vector3.Lerp(currentPos, purPos, smooth); //position
            transform.rotation = Quaternion.Slerp(currentRot, purRot, smooth); // rotation
            yield return null;
        }

        transform.position = purPos;
        transform.rotation = purRot;
    }
}
