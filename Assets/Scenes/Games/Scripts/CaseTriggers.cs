using UnityEngine;

public class CaseTriggers : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int index = int.Parse(gameObject.name.Replace("Case", ""));
            MinesweeperGameManager gameManager = FindObjectOfType<MinesweeperGameManager>();
            if (gameManager != null)
            {
                gameManager.SetTrigger(index, true);
            }
        }
    }
}
