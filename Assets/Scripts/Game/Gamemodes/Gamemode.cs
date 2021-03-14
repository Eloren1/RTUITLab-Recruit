using UnityEngine;

public abstract class Gamemode : MonoBehaviour
{
    public abstract void StartGame();

    public abstract void CompletedGame();

    public void SpawnPlane(GameObject plane, Transform spawn, Vector3 velocity, float thrust)
    {
        // Пока что самолет находится в иерархии и перемещается по сцене.
        // Позже для разных моделей самолетов тут необходимо будет реализовать
        // список префабов, которые не будут находиться на сцене. После чего 
        // нужный префаб самолета будет создаваться при помощи Instantiate().

        plane.transform.position = spawn.transform.position;
        plane.transform.rotation = spawn.transform.rotation;

        plane.GetComponent<PlaneController>().SetStartValues(velocity, thrust);
    }
}
