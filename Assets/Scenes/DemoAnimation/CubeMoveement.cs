using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CubeMoveement : MonoBehaviour
{
    //[SerializeField] private GameObject player;
    [SerializeField] private List<Transform> points;
    [SerializeField] private int duration = 5, pointIndex = 0;
    [SerializeField] private float stepDelay = 0;

    //#region DoTween DoPath
    //private async void Start()
    //{
    //    await Task.Delay(1000);
    //    var positions = points.Select(element => element.position).ToArray();
    //    stepDelay = points.Count / duration;
    //    transform.DOPath(positions, duration, PathType.CatmullRom).OnWaypointChange((pt) =>
    //    {
    //        //if (pt < positions.Length)
    //        {
    //            transform.DOLocalMoveY(2f, stepDelay / 2).OnComplete(() => { transform.DOLocalMoveY(-2f, stepDelay / 2); });
    //            transform.DOPunchScale(new Vector3(0, 0.05f, 0), stepDelay / 2, 2, 0.5f).SetLoops(-1);
    //            //DOTween.Sequence().SetTarget(transform)
    //            //            //.Append(transform.DOLocalMoveY(1.5f, 0.1f).SetEase(Ease.Linear))
    //            //            //.Append(transform.DOLocalMoveY(-1.5f, 0.1f).SetEase(Ease.Linear)).SetEase(Ease.Linear);
    //            //            .Append(transform.DOLocalMoveY(2f, stepDelay / 2).SetEase(Ease.Linear))
    //            //            .Append(transform.DOLocalMoveY(-2f, stepDelay / 2).SetEase(Ease.Linear)).SetEase(Ease.Linear);
    //        }
    //    }).OnComplete(() => { transform.DOFlip(); });

    //}
    //#endregion

    #region Slerp

    //private void Update()
    //{
    //    //if (pointIndex > points.Count) return;
    //    //if (Time.time < duration)
    //    //{
    //    //    fraction += Time.deltaTime;
    //    //    transform.position = Vector3.Slerp(transform.position, points[pointIndex].position, fraction);
    //    //}
    //    //if (transform.position == points[pointIndex].position)
    //    //{
    //    //    pointIndex++;
    //    //}
    //    if (Time.time < duration)
    //    {
    //        fraction += Time.deltaTime;
    //        transform.position = Vector3.Slerp(points[0].position, points[1].position, fraction);
    //    }

    //}
    #endregion

    #region DoTween Jump
    public float fraction;
    public GameObject[] effect;
    private async void Start()
    {
        await Task.Delay(2000);
        while (pointIndex < points.Count)
        {
            //if (Time.time < duration)
            {
                //fraction += Time.deltaTime;
                transform.DOJump(points[pointIndex].position, 2, 1, 0.5f).OnComplete(() => { /*effect[pointIndex].transform.position = transform.position;*/ effect[pointIndex].SetActive(true); /* Instantiate(effect, transform.position, Quaternion.identity);*/ });
                transform.DOPunchScale(new Vector3(0, 0.02f, 0), 0.5f, 2, 0.5f);/*Vector3.Slerp(transform.position, points[pointIndex].position, fraction)*/;
            }
            await Task.Delay(600);
            //if (transform.position == points[pointIndex].position)
            {
                //fraction = 0;
                pointIndex++;
            }
        }
        //if (Time.time < duration)
        //{
        //    fraction += Time.deltaTime;
        //    transform.position = Vector3.Slerp(points[0].position, points[1].position, fraction);
        //}

    }

    #endregion














    //public Transform sunrise;
    //public Transform sunset;

    //// Time to move from sunrise to sunset position, in seconds.
    //public float journeyTime = 1.0f;

    //// The time at which the animation started.
    //private float startTime;
    //public bool draw;

    //Vector3 center;
    //Vector3 riseRelCenter;
    //Vector3 setRelCenter;
    //[SerializeField] float fracComplete;
    //[SerializeField] bool completed;
    //void Start()
    //{
    //    // Note the time at the start of the animation.
    //    startTime = Time.time;
    //}
    //void Update()
    //{
    //    if (completed) return;
    //    // The center of the arc
    //    center = (sunrise.position + sunset.position) * 0.5F;
    //    // move the center a bit downwards to make the arc vertical
    //    center -= new Vector3(0, 1, 0);

    //    // Interpolate over the arc relative to center
    //    riseRelCenter = sunrise.position - center;
    //    setRelCenter = sunset.position - center;

    //    // The fraction of the animation that has happened so far is
    //    // equal to the elapsed time divided by the desired time for
    //    // the total journey.
    //    fracComplete = (Time.time - startTime) / journeyTime;

    //    transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
    //    transform.position += center;
    //    if (fracComplete >= 1)
    //    {
    //        startTime = Time.time;
    //        pointIndex++;
    //        if (pointIndex < points.Count - 1)
    //        {
    //            sunrise = points[pointIndex];
    //            sunset = points[pointIndex + 1];
    //        }
    //        else
    //        {
    //            completed = true;
    //        }
    //    }
    //}
    //private void OnDrawGizmos()
    //{
    //    if (draw)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawSphere(center, 0.2f);
    //        Gizmos.DrawSphere(riseRelCenter, 0.2f);
    //        Gizmos.DrawSphere(setRelCenter, 0.2f);
    //    }
    //}
}
