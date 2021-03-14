using UnityEngine;

public abstract class Gamemode : MonoBehaviour
{
    public abstract void StartGame();

    public abstract void CompletedGame();

    public void SpawnPlane(GameObject plane, Transform spawn, Vector3 velocity, float thrust)
    {
        // ���� ��� ������� ��������� � �������� � ������������ �� �����.
        // ����� ��� ������ ������� ��������� ��� ���������� ����� �����������
        // ������ ��������, ������� �� ����� ���������� �� �����. ����� ���� 
        // ������ ������ �������� ����� ����������� ��� ������ Instantiate().

        plane.transform.position = spawn.transform.position;
        plane.transform.rotation = spawn.transform.rotation;

        plane.GetComponent<PlaneController>().SetStartValues(velocity, thrust);
    }
}
