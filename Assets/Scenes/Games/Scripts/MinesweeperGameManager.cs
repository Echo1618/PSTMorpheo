using UnityEngine;

public class MinesweeperGameManager : MonoBehaviour
{
    private bool[] isTriggered = new bool[9];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTrigger(int index, bool state)
    {
        if (index < 0 || index >= isTriggered.Length)
            return;

        isTriggered[index] = state;
    }
}
