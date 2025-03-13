using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI CountdownText;
    public float initialTime = 60f; // 初始倒计时时间（秒）
    private float timeRemaining; // 剩余时间
    private bool timerIsRunning; // 计时器是否在运行

    private void Start()
    {
        CountdownText=GetComponent<TextMeshProUGUI>();
        StartTimer(); // 启动计时器
    }

    private void OnEnable()
    {
        Item07Effect.OnItem07Effect += AddTime;
    }

    private void OnDisable()
    {
        Item07Effect.OnItem07Effect -= AddTime;
    }

    private void Update()
    {
        UpdateTimerDisplay();
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // 减少剩余时间
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    // 启动计时器
    public void StartTimer()
    {
        timeRemaining = initialTime;
        timerIsRunning = true;
    }

    // 增加时间
    public void AddTime(float secondsToAdd)
    {
        timeRemaining += secondsToAdd;
    }

    // 获取剩余时间
    public float GetRemainingTime()
    {
        return timeRemaining;
    }
    
    private void UpdateTimerDisplay()
    {
        int seconds = Mathf.FloorToInt(timeRemaining); // 计算秒
        int milliseconds = Mathf.FloorToInt((timeRemaining - seconds) * 100); // 计算毫秒

        // 格式化为两位数显示
        CountdownText.text = string.Format("{0:00}:{1:00}", seconds, milliseconds);
    }
}
