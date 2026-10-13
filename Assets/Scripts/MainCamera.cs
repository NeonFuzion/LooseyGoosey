using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] float followSpeed = 3;
    [SerializeField] Transform player;

    Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = player;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + Vector3.back * 10, Time.deltaTime * followSpeed);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void ResetTarget()
    {
        SetTarget(player);
    }
}
