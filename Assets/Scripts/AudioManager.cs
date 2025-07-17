using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio References")]
    public AudioSource audioSource;
    public AudioClip[] parts;     // 0 to 4
    public AudioClip fillClip;

    private int currentPartIndex = 0;
    private bool isPlayingSequence = false;
    private bool isFillQueued = false;
    private int nextPartAfterFill = -1;

    private bool isEndingSequence = false;
    private int remainingFillRepeats = 0;

    void Awake()
    {
        // Singleton pattern & Don't Destroy
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void Update()
    {
        if (!audioSource.isPlaying && isPlayingSequence)
        {
            PlayNextInSequence();
        }
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    /// <summary>
    /// Başlangıç parçasından itibaren müzik sekansını başlatır.
    /// </summary>
    public void PlaySequence()
    {
        currentPartIndex = 0;
        isPlayingSequence = true;
        isEndingSequence = false;
        isFillQueued = false;

        audioSource.clip = parts[currentPartIndex];
        audioSource.Play();
    }

    /// <summary>
    /// Belirtilen part index'inden başlatarak sekanslı müzik çalmaya devam eder.
    /// </summary>
    public void PlaySequenceFrom(int index)
    {
        if (index < 0 || index >= parts.Length)
        {
            Debug.LogWarning("Invalid music part index.");
            return;
        }

        StopAll(); // herhangi bir şey oynuyorsa durdur
        currentPartIndex = index;
        isPlayingSequence = true;
        isEndingSequence = false;
        isFillQueued = false;

        audioSource.clip = parts[currentPartIndex];
        audioSource.Play();
    }

    /// <summary>
    /// Bir sonraki parçadan önce fill çalınmasını sıraya alır.
    /// </summary>
    public void QueueFill()
    {
        if (!isPlayingSequence || isFillQueued || isEndingSequence)
            return;

        isFillQueued = true;
        nextPartAfterFill = currentPartIndex + 1;
    }

    /// <summary>
    /// Fill parçasını belirli sayıda çalıp ardından müziği sonlandırır.
    /// </summary>
    public void EndSequence(int fillRepeatCount)
    {
        if (fillRepeatCount <= 0)
        {
            Debug.LogWarning("Fill repeat count must be greater than 0.");
            return;
        }

        isEndingSequence = true;
        isPlayingSequence = true; // update döngüsüne devam
        isFillQueued = false;
        remainingFillRepeats = fillRepeatCount;

        if (!audioSource.isPlaying)
        {
            PlayNextInSequence();
        }
    }

    /// <summary>
    /// Sekanslı ilerleme mantığına göre bir sonraki parçayı veya fill'i çalar.
    /// </summary>
    private void PlayNextInSequence()
    {
        if (isEndingSequence)
        {
            if (remainingFillRepeats > 0)
            {
                audioSource.clip = fillClip;
                audioSource.Play();
                remainingFillRepeats--;
            }
            else
            {
                StopAll();
            }
            return;
        }

        if (isFillQueued)
        {
            audioSource.clip = fillClip;
            isFillQueued = false;
            currentPartIndex = nextPartAfterFill;
            audioSource.Play();
            return;
        }

        currentPartIndex++;
        if (currentPartIndex < parts.Length)
        {
            audioSource.clip = parts[currentPartIndex];
            audioSource.Play();
        }
        else
        {
            isPlayingSequence = false;
            PlaySequenceFrom(3);
        }
    }

    /// <summary>
    /// Belirli bir ses klibini sekans dışı (tek başına) çalar.
    /// </summary>
    public void PlaySingle(AudioClip clip)
    {
        StopAll();
        audioSource.clip = clip;
        audioSource.Play();
    }

    /// <summary>
    /// Tüm müzikleri ve oynatma durumlarını durdurur.
    /// </summary>
    public void StopAll()
    {
        audioSource.Stop();
        isPlayingSequence = false;
        isEndingSequence = false;
        isFillQueued = false;
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Örnek senaryo
        if (scene.name == "MainMenu")
        {
            //PlaySingle(parts[0]); // Menüde sadece part0
            PlaySequenceFrom(0);
        }
        else if (scene.name == "Game")
        {

            //PlaySequenceFrom(0); // Start sequence from beginning
        }
    }

}
