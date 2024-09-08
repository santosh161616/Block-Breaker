using UnityEngine;

public class WheelDeployer : MonoBehaviour
{
    [SerializeField] private GameObject SixSecWheel, SevenSecWheel, EightSecWheel, NineSecWheel, TenSecWheel, ElevenSecWheel, TwelveSecWheel;

    public void SetSpinWheel(string noOfSections)
    {
        GameObject obj = null;
        int val = int.Parse(noOfSections);
        switch (val)
        {
            case 6:
                obj = Instantiate(SixSecWheel, Vector3.zero, Quaternion.identity);
                break;

            case 7:
                Instantiate(SevenSecWheel);
                break;

            case 8:
                Instantiate(EightSecWheel);
                break;

            case 9:
                Instantiate(NineSecWheel);
                break;

            case 10:
                Instantiate(TenSecWheel);
                break;

            case 11:
                Instantiate(ElevenSecWheel);
                break;

            case 12:
                Instantiate(TwelveSecWheel);
                break;

        }
        if (obj != null)
            obj.transform.localScale = Vector3.one / 2;
    }
}
