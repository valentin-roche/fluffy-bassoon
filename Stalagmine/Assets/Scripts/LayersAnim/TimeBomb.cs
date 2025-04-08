using System.Collections;
using UnityEngine;

public class TimeBomb : MonoBehaviour
{
    private float m_Time = 0;
    public IEnumerator TimeBombing()
    {
        float m_Time = 0;
        Vector3 startScale = transform.localScale;

        while (m_Time < 2f)
        {
            transform.localScale = Vector3.Lerp(startScale, new Vector3(0, -20, 0), m_Time / 2f);
            m_Time += Time.deltaTime;

            yield return null;
        }
        Destroy(gameObject);
    }
}
