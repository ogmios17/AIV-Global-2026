using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightersDataBinder : MonoBehaviour
{
    public List<GameObject> healthBars;
    public List<GameObject> manaBars;
    public List<GameObject> cards;
    [SerializeField] private float healthLoseAnimationDuration;
    [SerializeField] private float shakeThreshold;
    [SerializeField] private float shakeSpeed;
    [SerializeField] private float colorChangeSpeed;
    [SerializeField] private float manaAnimationDelay = 1;
    [SerializeField] private Color manaColor;
    private Color healthColor;

    private Jammer boundJammer;
    private int lastKnownHealth;

    void Awake()
    {
        healthColor = healthBars[0].GetComponent<SpriteRenderer>().color;
    }

    void Start()
    {
        cards[0].transform.parent.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Links this UI to a Jammer and keeps the health bars in sync via OnHealthChanged.
    /// Health is mutated ONLY through Jammer.TakeAHit/Cure; this component just animates.
    /// Call once after the fighter prefab is instantiated (see LoadFight).
    /// </summary>
    public void Bind(Jammer jammer)
    {
        if (boundJammer != null)
            boundJammer.OnHealthChanged -= OnHealthChanged;

        boundJammer = jammer;
        boundJammer.OnHealthChanged += OnHealthChanged;
        lastKnownHealth = jammer.MaxHealth;
        OnHealthChanged(jammer.Health, jammer.MaxHealth);
    }

    private void OnDestroy()
    {
        if (boundJammer != null)
            boundJammer.OnHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        // Animate the bars that were just lost...
        for (int i = currentHealth; i < lastKnownHealth && i < healthBars.Count; i++)
        {
            StartCoroutine(ChangeHealthColorCO(healthBars[i].GetComponent<SpriteRenderer>()));
            StartCoroutine(LoseHealthCO(healthBars[i]));
        }

        // ...and restore the ones that were just cured.
        for (int i = lastKnownHealth; i < currentHealth && i < healthBars.Count; i++)
        {
            healthBars[i].GetComponent<SpriteRenderer>().color = healthColor;
        }

        lastKnownHealth = currentHealth;
    }

    public void GainMana(int mana, Jammer player)
    {
        StartCoroutine(GainManaCO(mana, player));
    }

    public void UseMana(int mana, Jammer player)
    {
        if (mana > player.Mana) return;
        StartCoroutine(LoseManaCO(mana, player));
    }

    private IEnumerator ChangeHealthColorCO(SpriteRenderer sprite)
    {
        while (sprite.color.r > 0.01 || sprite.color.g > 0.01 || sprite.color.b > 0.01)
        {
            sprite.color = new Color(
                Mathf.Lerp(sprite.color.r, 0, Time.deltaTime * colorChangeSpeed),
                Mathf.Lerp(sprite.color.g, 0, Time.deltaTime * colorChangeSpeed),
                Mathf.Lerp(sprite.color.b, 0, Time.deltaTime * colorChangeSpeed));
            yield return null;
        }
        sprite.color = Color.black;
    }

    private IEnumerator GainManaCO(int mana, Jammer player)
    {
        for (int i = 0; i < mana; i++)
        {
            if (player.Mana < manaBars.Count)
            {
                manaBars[player.Mana].GetComponent<Animator>().SetTrigger("In");
                yield return new WaitForSecondsRealtime(manaAnimationDelay);
                player.GainMana(1);
            }
        }
    }

    private IEnumerator LoseManaCO(int mana, Jammer player)
    {
        for (int i = 0; i < mana; i++)
        {
            manaBars[player.Mana - 1].GetComponent<Animator>().SetTrigger("Out");
            yield return new WaitForSecondsRealtime(manaAnimationDelay);
            player.SpendMana(1);
        }
    }

    private IEnumerator LoseHealthCO(GameObject bar)
    {
        float timer = 0;
        Vector3 target, startPos;
        startPos = bar.transform.localPosition;

        while (timer < healthLoseAnimationDuration)
        {
            // Pick a random shake target inside the threshold, then move toward it.
            target = new Vector3(
                Random.Range(startPos.x - shakeThreshold, startPos.x + shakeThreshold),
                Random.Range(startPos.y - shakeThreshold, startPos.y + shakeThreshold),
                startPos.z);

            while (Mathf.Abs(bar.transform.localPosition.x - target.x) > 0.1f ||
                   Mathf.Abs(bar.transform.localPosition.y - target.y) > 0.1f)
            {
                timer += Time.deltaTime;
                bar.transform.localPosition = new Vector3(
                    Mathf.Lerp(bar.transform.localPosition.x, target.x, Time.deltaTime * shakeSpeed),
                    Mathf.Lerp(bar.transform.localPosition.y, target.y, Time.deltaTime * shakeSpeed),
                    startPos.z);
                yield return null;
            }

            yield return null;
        }

        bar.transform.localPosition = startPos;
    }
}
