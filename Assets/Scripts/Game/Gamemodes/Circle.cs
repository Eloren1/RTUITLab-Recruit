using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Collider))]
public class Circle : MonoBehaviour
{ // Circle это круг, а не кольцо, надо бы исправить
    private bool collected = false;

    public void Collected()
    {
        if (!collected)
        {
            collected = true;

            FindObjectOfType<Competition>().CircleCollected();

            Destroy(gameObject);
        }
    }
}
