using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Car.Control;
using Car.Combat;

namespace Car.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealth;
        [SerializeField] GameObject deathFX;
        [SerializeField] Transform fxParent;
        [SerializeField] GameObject[] objsToOffOnDeath;
        [SerializeField] float invincibilityDuration = 2.5f;
        [SerializeField] float invinsibilityDeltaTime = 0.15f;
        [SerializeField] GameObject model; 
        [SerializeField] Light pointLight1;
        [SerializeField] Light pointLight2;
        [SerializeField] AudioClip deathSound;
        Vector3 initialScale;
        Color pointLightInitCol1;
        Color pointLightInitCol2;
        public float health;
        Fighter fighter;
        bool isInvincible = false;
        bool isDead = false;


        public event Action onAiCarDied;
        public event Action onHealthUpdated;

        void Awake()
        {
            fighter = GetComponent<Fighter>();
        }
        void Start()
        {
            health = maxHealth;
            onHealthUpdated();
            initialScale = model.transform.localScale;
            pointLightInitCol1 = pointLight1.color;
            pointLightInitCol2 = pointLight2.color;
        }

        public void AffectHealth(float delta)
        {
            if (isInvincible) return;
            if (isDead) return;

            health += delta;
            health = Mathf.Clamp(health, 0, maxHealth);

            onHealthUpdated();

            if (health <= 0)
            {
                Die();
                return;
            }

            StartCoroutine(BecomeInvincible());
            
        }

        public float GetHealthFraction()
        {
            if (maxHealth <= 0)
            {
                Debug.Log("Max Health of " + transform.gameObject.name + " is 0");
                maxHealth = 1;
            }

            return health / maxHealth;
        }

        private IEnumerator BecomeInvincible()
        {
            isInvincible = true;

            for (float i = 0; i < invincibilityDuration; i += invinsibilityDeltaTime)
            {
                if (model.transform.localScale == Vector3.one)
                {
                    ScaleModelTo(Vector3.zero);
                    SetPointLightColour(pointLight1, Color.black);
                    SetPointLightColour(pointLight2, Color.black);
                }
                else
                {
                    ScaleModelTo(initialScale);
                    SetPointLightColour(pointLight1, pointLightInitCol1);
                    SetPointLightColour(pointLight2, pointLightInitCol2);
                }
                yield return new WaitForSeconds(invinsibilityDeltaTime);
            }

            ScaleModelTo(initialScale);
            SetPointLightColour(pointLight1, pointLightInitCol1);
            SetPointLightColour(pointLight2, pointLightInitCol2);

            isInvincible = false;
        }

        private void SetPointLightColour(Light light, Color color)
        {
            light.color = color;
        }

        private void ScaleModelTo(Vector3 newScale)
        {
            model.transform.localScale = newScale;
        }

        private void Die()
        {
            print(gameObject.name + " has died.");
            isDead = true;
            AIController aiController = GetComponent<AIController>();

            AudioSource.PlayClipAtPoint(deathSound, transform.position);

            if (aiController != null)
            {
                aiController.Die();
                GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
                fx.transform.parent = fxParent;
                Destroy(this.gameObject, 0.2f);
                onAiCarDied();
            }
            else // is player
            {
                GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
                fx.transform.parent = fxParent;

                GetComponent<CarController>().Die();
                foreach(GameObject o in objsToOffOnDeath)
                {
                    o.SetActive(false);
                }

                
            }
        }

        public bool GetIsShieldUp()
        {
            return fighter.GetIsShieldUp();
        }
    }
}
