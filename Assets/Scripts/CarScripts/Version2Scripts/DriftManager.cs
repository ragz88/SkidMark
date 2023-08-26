using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class DriftManager : MonoBehaviour
{
    public Rigidbody car;
    public TMP_Text totalScoreText;
    public TMP_Text currentScoreText;
    public TMP_Text multiplierText;
    public TMP_Text driftAngleText;
    public bool isDrifting { get; set; }

    private float speed = 0f, driftAngle = 0f, driftMultiplier = 1f, currentScore, totalScore;
    private bool driftState = false;
    public float minSpeed = 5f;
    public float minDriftAngle = 10f;
    public float maxDriftAngle = 120f;
    public float driftTimeDelay = 0.2f;
    public GameObject driftObject;
    public Color normalDriftColour;
    public Color driftStoppingColour;
    public Color driftEndedColour;

    private IEnumerator stopDriftingCoroutine = null;
    void Start()
    {
        driftObject.SetActive(false);
    }

    void Update()
    {
        ManageDrift();
        ManageUI();
        isDrifting = driftState;
    }
    void ManageDrift()
    {
        speed = car.velocity.magnitude;
        driftAngle = Vector3.Angle(car.transform.forward, (car.velocity + car.transform.forward).normalized);
        if (driftAngle > 120)
        {
            driftAngle = 0;
        }
        if (driftAngle >= minDriftAngle && speed > minSpeed)
        {
            if (!driftState || stopDriftingCoroutine != null)
            {
                StartDrift();
            }
        }
        else
        {
            if (driftState && stopDriftingCoroutine == null)
            {
                StopDrift();
            }
        }
        if (driftState)
        {
            currentScore += Time.deltaTime * driftAngle * driftMultiplier;
            driftMultiplier += Time.deltaTime;
            driftObject.SetActive(true);
        }
    }

    async void StartDrift()
    {
        if (!driftState)
        {
            await Task.Delay(Mathf.RoundToInt(1000 * driftTimeDelay));
            driftMultiplier = 1;
        }
        if (stopDriftingCoroutine != null)
        {
            StopCoroutine(stopDriftingCoroutine);
            stopDriftingCoroutine = null;
        }
        currentScoreText.color = normalDriftColour;
        driftState = true;
    }
    void StopDrift()
    {
        stopDriftingCoroutine = StoppingDrift();
        StartCoroutine(stopDriftingCoroutine);
    }
    private IEnumerator StoppingDrift()
    {
        yield return new WaitForSeconds(0.1f);
        currentScoreText.color = driftStoppingColour;
        yield return new WaitForSeconds(driftTimeDelay * 4f);
        totalScore += currentScore; 
        driftState = false;
        currentScoreText.color = driftEndedColour;
        yield return new WaitForSeconds(0.5f);
        currentScore = 0;
        driftObject.SetActive(false);
    }

    void ManageUI()
    {
        totalScoreText.text = "Total: " + (totalScore).ToString("###,###,000");
        multiplierText.text = driftMultiplier.ToString("###,###,##0.0") + "X";
        currentScoreText.text = currentScore.ToString("###,###,000");
        driftAngleText.text = driftAngle.ToString("###,##0") + "°";
    }
}




