using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

// Cube Moving Animation and Minigame Object appear Animation

public class MinigameManager : MonoBehaviour
{
    public Transform bomb;
    public GameObject purpos;

    //minigames
    public Transform Face0Mini;
    public Transform Face1Mini;

    Dictionary<int, Vector3> faceDirections = new Dictionary<int, Vector3>()
    {
        { 0, -Vector3.right },  // left
        { 1, -Vector3.forward }, // back
        { 2,  Vector3.right },  // right
        { 3,  Vector3.forward },// front
    };

    private Dictionary<int, Transform> miniGames;

    private void Awake()
    {
        miniGames = new Dictionary<int, Transform>()
        {
            { 0, Face0Mini },
            { 1, Face1Mini },
            // 必要に応じて追加
        };

        foreach (var kvp in miniGames)
        {
            kvp.Value.gameObject.SetActive(false);
        }
    }

    public void StartMoving(int faceIndex)
    {
        Debug.Log("MinigameManager, faceIndex =" + faceIndex);

        Transform purposObject = purpos.transform;

        Transform miniObj = miniGames[faceIndex];
        miniObj.gameObject.SetActive(true);


        StartCoroutine(AppearFromCube(miniObj));

        StartCoroutine(movingAndStart(purposObject, faceIndex));
    }

    private IEnumerator movingAndStart(Transform purposObject, int randomNumber)
    {
        //  ‰ñ“]‚³‚¹‚é‘ÎÛ
        Transform Cube = bomb.transform;

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
        Vector3 purPos = purposObject.position + purposObject.forward;

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

    public IEnumerator AppearFromCube(Transform Mini)
    {
        // 初期、最終位置
        Vector3 startPos = Mini.localPosition;
        Vector3 endPos = new Vector3(startPos.x, startPos.y + 0.55f, startPos.z);
        // 上方向に少しだけせり出す

        //スケールを 0 → 1 にする
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        float t = 0f;
        float duration = 0.7f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float smooth = Mathf.SmoothStep(0, 1, t);

            Mini.localPosition = Vector3.Lerp(startPos, endPos, smooth);
            Mini.localScale = Vector3.Lerp(startScale, endScale, smooth);

            yield return null;
        }
    }


}
