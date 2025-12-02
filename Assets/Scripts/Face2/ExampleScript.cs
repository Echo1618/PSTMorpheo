using UnityEngine;
using System.Collections.Generic;

public class ExampleScript : MonoBehaviour
{
    public ManageCube2 managerCube;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<int> numberList = new List<int> { 0, 1, 2, 3 };
            List<bool> boolList = new List<bool> { true, false };

            // ƒ‰ƒ“ƒ_ƒ€‚É1‚Â‘I‘ğ
            int randomNumber = numberList[Random.Range(0, numberList.Count)];
            bool randomBool = boolList[Random.Range(0, boolList.Count)];

            // o—Í
            Debug.Log("Number: " + randomNumber + "  boolean: " + randomBool);
            managerCube.receive(randomNumber, randomBool);
        }
    }
}