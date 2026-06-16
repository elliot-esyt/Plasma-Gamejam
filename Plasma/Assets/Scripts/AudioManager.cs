using UnityEngine;

public class AudioManager : MonoBehaviour
{

    // variables
    public static AudioManager Instance;

    [Header("weapon")]
    [SerializeField] private AudioClip gunShootClip;
    [SerializeField] private AudioClip swordLoopClip;
    [SerializeField] private AudioClip staffUseClip;

    [Header("player")]
    [SerializeField] private AudioClip playerDamageClip;
    [SerializeField] private AudioClip playerDeathClip;

    [Header("enemy dmg")]
    [SerializeField] private AudioClip lightEnemyDamageClip;
    [SerializeField] private AudioClip normalEnemyDamageClip;
    [SerializeField] private AudioClip tankEnemyDamageClip;
    [SerializeField] private AudioClip bossEnemyDamageClip;

    [Header("ui")]
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip buyClip;

    [Header("wave progress")]
    [SerializeField] private AudioClip waveClearClip;

    [Header("vol")]
    [SerializeField] [Range(0f, 1f)] private float sfxVolume = 1f;

    private AudioSource sfxSource;
    private AudioSource swordLoopSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // singleton 
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject); // keeps so it works in other scene 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxSource = gameObject.AddComponent<AudioSource>(); // add aduio companent
        sfxSource.playOnAwake = false; // prevent auto palyerback

        swordLoopSource = gameObject.AddComponent<AudioSource>();  // handles sword loop
        swordLoopSource.playOnAwake = false;
        swordLoopSource.loop = true;
        swordLoopSource.clip = swordLoopClip;
        swordLoopSource.volume = sfxVolume;
    }

    private void PlayClip(AudioClip clip) // play audio clip
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // plays sound
    public void PlayGunShoot()       => PlayClip(gunShootClip);
    public void PlayStaffUse()       => PlayClip(staffUseClip);
    public void PlayPlayerDamage()   => PlayClip(playerDamageClip);
    public void PlayPlayerDeath()    => PlayClip(playerDeathClip);
    public void PlayButtonClick()    => PlayClip(buttonClickClip);
    public void PlayBuy()            => PlayClip(buyClip);
    public void PlayWaveClear()      => PlayClip(waveClearClip);

    public void PlayEnemyDamage(EnemyType type) // enemy dmg based on type
    {
        switch (type)
        {
            case EnemyType.Light:  PlayClip(lightEnemyDamageClip);  break;
            case EnemyType.Normal: PlayClip(normalEnemyDamageClip); break;
            case EnemyType.Tank:   PlayClip(tankEnemyDamageClip);   break;
            case EnemyType.Boss:   PlayClip(bossEnemyDamageClip);   break;
        }
    }

    public void StartSwordLoop() // start loop
    {
        if (swordLoopClip == null || swordLoopSource == null) return;
        if (!swordLoopSource.isPlaying) swordLoopSource.Play();
    }

    public void StopSwordLoop() // end sword loop 
    {
        if (swordLoopSource == null) return;
        if (swordLoopSource.isPlaying) swordLoopSource.Stop();
    }
}