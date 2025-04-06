using System.Collections;
using UnityEngine;

public class TimeBomb : MonoBehaviour
{
    private float m_Time = 0;
    public IEnumerator TimeBombing()
    {
        while (m_Time < 5f)
        {
            m_Time += Time.deltaTime;
            if (gameObject.transform.localScale.x > 0)
                gameObject.transform.localScale -= new Vector3(0.001f, 0, 0.001f);
            else
                break;
            yield return null;// new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
}
