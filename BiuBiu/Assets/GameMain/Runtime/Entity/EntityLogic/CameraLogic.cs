using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    private Transform m_CameraTransform;
    private bool isFollowing;
    public float smooth;


    private readonly Vector3 offset = new Vector3(0f, 10f, -6f);

    // Update is called once per frame
    private void LateUpdate()
    {
        if(isFollowing)
        {
            Apply();
        }
    }

    private void Apply()
    {
        var pos = transform.position + offset;

        m_CameraTransform.position = Vector3.Lerp(m_CameraTransform.position, pos, smooth * Time.deltaTime);
    }

    public void OnStartFollowing()
    {
        if (Camera.main == null)
            return;
        m_CameraTransform = Camera.main.transform;
        isFollowing = true;
    }

    public void OnStopFollowing()
    {
        isFollowing = false;
    }
}
