using UnityEngine;
using System.Collections.Generic;

public class PadScript : MonoBehaviour
{
    [Header("Pad Cases")]
    [SerializeField] private List<GameObject> m_Cases = new List<GameObject>();

    [Header("Bomb Settings")]
    [SerializeField] private int m_NumberOfBombs = 3;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_Cases.Add(transform.GetChild(i).gameObject);
        }
        PlaceBombs(m_NumberOfBombs);
    }

    void Update()
    {
        
    }

    private void PlaceBombs(int numberOfBombs)
    {
        for (int i = 0; i < numberOfBombs; i++)
        {
            int randomIndex = GetRandomEmptyCase();
            if (randomIndex == -1)
            {
                Debug.LogError("PadScript: Unable to place all bombs due to lack of empty cases.");
                break;
            }
            m_Cases[randomIndex].GetComponent<CaseScript>().PlaceBomb();
        }
    }

    private int GetRandomEmptyCase()
    {
        if (m_Cases.Count == 0)
        {
            Debug.LogError("PadScript: No cases available to place bombs.");
            return -1;
        }
        int index;
        do
        {
            index = Random.Range(0, m_Cases.Count);
        } while (m_Cases[index].GetComponent<CaseScript>().IsBomb());
        return index;
    }

    public void RestartPad()
    {
        foreach (GameObject caseObj in m_Cases)
        {
            CaseScript caseScript = caseObj.GetComponent<CaseScript>();
            if (caseScript != null)
            {
                caseScript.ResetCase();
            }
        }
        PlaceBombs(m_NumberOfBombs);
    }
}
