using Proto;
using UnityEngine;

public class PlayerProxy : MonoBehaviour, IProxy<FrameInfo>
{
    [SerializeField] private float speed = 1;
    private float tickTime;
    private float step;

    public void Execute(FrameInfo frame)
    {
        if (frame.direction != 0)
        {
            MoveAxis(frame.direction);
        }
    }

    private static Vector3 startDir = new Vector3(0,1,0);
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private void Awake()
    {
        step = 1f / 15f;
        startPosition = targetPosition = transform.position;
    }

    private void MoveAxis(ushort axis)
    {
        transform.position = startPosition = targetPosition;
        var angle = axis - 1;
        transform.rotation = Quaternion.AngleAxis(angle, startDir);
        targetPosition = speed*(1f / 15f) * transform.forward + transform.position;
        tickTime = 0f;
    }

    private void Update()
    {
        tickTime += Time.deltaTime;
        transform.position = Vector3.Lerp(startPosition,targetPosition, tickTime/ step);
    }

    public void Reset(FrameInfo data)
    {
        
    }

    public void Snapshoot()
    {
        
    }
}

public interface IProxy<T>
{
    public void Execute(T data);
    public void Reset(T data);
    public void Snapshoot();
}
