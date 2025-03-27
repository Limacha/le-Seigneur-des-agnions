using System.Collections;
using UnityEngine;

public class WindCycle : MonoBehaviour
{
    public GameObject WindObj;

    // Start is called before the first frame update
    void Start()
    {
        float actualRotation = 0;
        StartCoroutine(WindLoop(actualRotation));
    }

    IEnumerator WindLoop(float actualRotation)
    {
        float targetAngle = 0;
        float startAngle = actualRotation;
        float addedRotation = Random.Range(0f, 359f);
        if (actualRotation + addedRotation > 360) 
        {
            targetAngle = (actualRotation + addedRotation) - 360;
        }
        else
        {
            targetAngle = actualRotation + addedRotation;
        }
        
        float duration = 500f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newAngle = Mathf.Lerp(startAngle, targetAngle, elapsedTime / duration);
            WindObj.transform.rotation = Quaternion.Euler(0, newAngle, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        WindObj.transform.rotation = Quaternion.Euler(0, targetAngle, 0);






        yield return new WaitForSeconds(500);
    }
}
