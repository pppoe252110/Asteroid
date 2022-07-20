using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBorders : MonoBehaviour
{
    public static Vector2 screenBorder;

    private static List<InBorderTarget> inBorderTargets;

    private void Awake()
    {
        inBorderTargets = new List<InBorderTarget>();
        screenBorder = Camera.allCameras[0].ScreenToWorldPoint(new Vector2(Screen.width,Screen.height));
    }

    public static void Clear()
    {
        inBorderTargets.Clear();
    }

    private void Update()
    {
        for (int i = 0; i < inBorderTargets.Count; i++)
        {
            Vector2 position = inBorderTargets[i].target.position;
            Vector2 size = inBorderTargets[i].size;

            if(Mathf.Abs(position.x) - size.x > screenBorder.x || Mathf.Abs(position.y) - size.y > screenBorder.y)
            {
                if (Mathf.Abs(position.x) - size.x > screenBorder.x)
                {
                    position.x = (screenBorder.x + size.x) * -Mathf.Sign(position.x);
                }
                if(Mathf.Abs(position.y) - size.y > screenBorder.y)
                {
                    position.y = (screenBorder.y + size.y) * -Mathf.Sign(position.y);
                }
                inBorderTargets[i].target.position = position;
            }
        }
    }

    public static void AddInBorderTarget(InBorderTarget target)
    {
        if(!inBorderTargets.Contains(target))
            inBorderTargets.Add(target);
    }
}
public class InBorderTarget
{
    public Transform target;
    public Vector2 size;

    public InBorderTarget(Transform target, Vector2 size)
    {
        this.target = target;
        this.size = size;
    }
}