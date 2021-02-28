using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Collider))]
public class Circle : MonoBehaviour
{
    public void Collected()
    {
        FindObjectOfType<Competition>().CircleCollected();

        // SOUND: TODO: Play collected sound

        Destroy(gameObject);
    }
}
