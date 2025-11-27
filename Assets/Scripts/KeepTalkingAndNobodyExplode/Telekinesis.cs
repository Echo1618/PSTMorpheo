using UnityEngine;

public class Telekinesis : MonoBehaviour
{
    public GameObject controller;
    private Quaternion lastControllerRotation;
    public bool isHolding = false;

    void Update()
    {
        if (isHolding)
        {
            // Calcul du delta de rotation entre la frame actuelle et la précédente
            Quaternion deltaRotation = controller.transform.rotation * Quaternion.Inverse(lastControllerRotation);

            // Appliquer la rotation relative à l’objet (sur lui-même)
            transform.rotation = deltaRotation * transform.rotation;

            // Mettre à jour la dernière rotation connue du contrôleur
            lastControllerRotation = controller.transform.rotation;
        }
    }

    public void Holding()
    {
        Debug.Log("Holding");
        isHolding = true;
        lastControllerRotation = controller.transform.rotation; // On garde la rotation de départ
    }

    public void NotHolding()
    {
        Debug.Log("Not Holding");
        isHolding = false;
    }
}
