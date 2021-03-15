using UnityEngine;

public class LandingGuide : MonoBehaviour
{
    [SerializeField] private PlaneController plane;
    private float minHeight = -2f;
    private float maxDistance = 4000f;

    [SerializeField] private GameObject landingVisuals;
    private bool guidesActive;

    private void Awake()
    {
        guidesActive = PlayerPrefs.GetInt("Guides") == 1;
        
        if (plane != null)
            landingVisuals.SetActive(guidesActive);
        else
        {
            Debug.LogError("Plane is not assigned in Landing Guide!");
        }
    }

    private void LateUpdate()
    {
        if (!guidesActive || plane == null) return;

        if ((plane.gameObject.transform.position.y > transform.position.y + minHeight) && 
            (Vector3.Distance(plane.gameObject.transform.position, transform.position) < maxDistance))
        {
            landingVisuals.SetActive(true);
        } else
        {
            landingVisuals.SetActive(false);
        }
    }
}
