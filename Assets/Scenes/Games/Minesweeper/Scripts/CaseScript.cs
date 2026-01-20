using UnityEngine;

public class CaseScript : MonoBehaviour
{
    
    private bool m_IsFlagged = false;
    private GameObject m_FlagObject;

    [Header("Case Settings")]
    [SerializeField] private bool m_IsBomb = false;

    [Header("Flag Settings")]
    [SerializeField] private GameObject m_FlagPrefab;
    [SerializeField] private Transform m_FlagSpawnPoint;

    void Start()
    {
        if (m_FlagPrefab == null)
        {
            Debug.LogError($"CaseScript({this.name}): Flag Prefab is not assigned in the inspector.");
        }
        if (m_FlagSpawnPoint == null)
        {
            Debug.LogWarning($"CaseScript({this.name}): Flag Spawn Point is not assigned.");
        }
    }

    void Update()
    {
        
    }

    public void PlaceBomb()
    {
        m_IsBomb = true;
    }

    public bool IsBomb()
    {
        return m_IsBomb;
    }

    public void ToggleFlag(GameObject flaggedObject)
    {
        if (m_IsFlagged)
        {
            Destroy(this.m_FlagObject);
            m_IsFlagged = false;
        }
        else
        {
            Vector3 flagPosition = m_FlagSpawnPoint.position;
            this.m_FlagObject = Instantiate(m_FlagPrefab, flagPosition, Quaternion.identity);
            this.m_FlagObject.transform.SetParent(this.transform);
            Destroy(flaggedObject);
            m_IsFlagged = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MS_Flag"))
        {
            ToggleFlag(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Hand"))
        {
            if(m_IsFlagged)
            {
                return;
            }
            if (m_IsBomb)
            {
                Debug.Log("Boom! You hit a bomb!");
            }
            else
            {
                Debug.Log("Safe! No bomb here.");
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MS_Flag"))
        {
            ToggleFlag(collision.gameObject);
        }
    }

    public void ResetCase()
    {
        m_IsBomb = false;
        if (m_IsFlagged)
        {
            Destroy(this.m_FlagObject);
            m_IsFlagged = false;
        }
    }
}