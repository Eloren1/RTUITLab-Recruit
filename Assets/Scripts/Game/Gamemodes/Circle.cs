using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Collider))]
public class Circle : MonoBehaviour
{ // Circle ��� ����, � �� ������, ���� �� ���������
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
