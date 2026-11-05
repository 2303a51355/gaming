using UnityEngine;

public class CoinCollectorGame : MonoBehaviour
{
    public static bool GameEnded { get; private set; }
    public int totalCoins = 60;
    public float timeLimit = 300f;
    int collected;
    float timeLeft;
    bool ended;
    bool won;
    GUIStyle big, normal;
    Transform player;

    void Start()
    {
        GameEnded = false;
        timeLeft = timeLimit;
        var p = GameObject.FindWithTag("Player");
        if (p) player = p.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CollectCoin()
    {
        if (ended) return;
        collected++;
        if (collected >= totalCoins) Finish(true);
    }

    void Update()
    {
        if (ended)
        {
            if (Input.GetKeyDown(KeyCode.R)) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            if (Input.GetKeyDown(KeyCode.Escape)) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
            return;
        }
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0) { timeLeft = 0; Finish(false); }
        if (Input.GetKeyDown(KeyCode.Escape)) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
        if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
        { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
    }

    void Finish(bool success)
    {
        ended = true; GameEnded = true; won = success;
        Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
    }

    void OnGUI()
    {
        if (big == null)
        {
            big = new GUIStyle(GUI.skin.label) { fontSize = 30, alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
            normal = new GUIStyle(GUI.skin.label) { fontSize = 19, alignment = TextAnchor.MiddleCenter };
        }
        GUI.color = new Color(0.04f,0.06f,0.10f,0.82f);
        GUI.Box(new Rect(12,12,250,92), GUIContent.none);
        GUI.color = Color.white;
        GUI.Label(new Rect(22,18,230,35), "COINS  " + collected + " / " + totalCoins, big);
        GUI.Label(new Rect(22,56,230,35), "TIME  " + Mathf.CeilToInt(timeLeft / 60f) + ":" + (Mathf.CeilToInt(timeLeft) % 60).ToString("00"), normal);
        GUI.Label(new Rect(0, Screen.height-42, Screen.width, 30), "WASD: Move   •   Mouse: Look   •   Shift: Run   •   Collect all 60 coins!", normal);
        if (ended)
        {
            GUI.color = new Color(0,0,0,0.78f); GUI.Box(new Rect(0,0,Screen.width,Screen.height), GUIContent.none); GUI.color = Color.white;
            GUI.Label(new Rect(0,Screen.height/2-85,Screen.width,60), won ? "YOU WIN!" : "TIME'S UP!", big);
            GUI.Label(new Rect(0,Screen.height/2-25,Screen.width,40), "Coins collected: " + collected + " / " + totalCoins, normal);
            GUI.Label(new Rect(0,Screen.height/2+25,Screen.width,40), "Press R to play again", normal);
        }
    }
}
