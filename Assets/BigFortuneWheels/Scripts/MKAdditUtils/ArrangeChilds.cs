using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mkey
{
    public class ArrangeChilds : MonoBehaviour
    {
        public float dist = 3.8f;
        public float startAngle = 0;
        public void PolarArray()
        {
            List<Transform> objects = new List<Transform>(transform.GetComponentsInChildren<Transform>(true));
            objects.Remove(transform);

            int length = objects.Count;
            float dAngleDeg = 360f / length;
            float startAngleRad = startAngle * Mathf.Deg2Rad;
            // set position
            for (int i = 0; i < length; i++)
            {
                float angleDeg = 90.0f - i * dAngleDeg;
                float angleRad = angleDeg * Mathf.Deg2Rad;
                objects[i].localPosition = new Vector3(dist * Mathf.Cos(angleRad + startAngleRad), dist * Mathf.Sin(angleRad + startAngleRad), 0);
                objects[i].localEulerAngles = new Vector3(0, 0, angleDeg + startAngle);
            }
            foreach (var item in objects)
            {
                Debug.Log(item.name);
            }
        }
    }
}