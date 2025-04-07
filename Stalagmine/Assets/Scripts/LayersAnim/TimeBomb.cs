using System.Collections;
using UnityEngine;

public class TimeBomb : MonoBehaviour
{
    public IEnumerator TimeBombing()
    {
        float m_Time = 0;
        Vector3 startScale = transform.localScale;

        while (m_Time < 2f)
        {
            transform.localScale = Vector3.Lerp(startScale, new Vector3(0,-20,0), m_Time / 2f);
            m_Time += Time.deltaTime;

            yield return null;
        }

        /*while (m_Time < 2f)
        {
            m_Time += Time.deltaTime;
            if (gameObject.transform.localScale.x > 0)
                gameObject.transform.localScale -= new Vector3(0.001f, 0, 0.001f);
            else
                break;
            yield return null;// new WaitForSeconds(0.01f);
        }*/
        Destroy(gameObject);
    }
}
