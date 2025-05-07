using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.Pool;

namespace CardGame
{
    public class ParticlesManager : MonoBehaviour
    {
        [SerializeField] private UIParticle _matchParticlePrefab;
        [SerializeField] private int _defaultCapacity = 6;
        [SerializeField] private int _maxSize = 12;

        private ObjectPool<UIParticle> _particlePool;

        void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.OnCardMatchSuccess, Callback_On_MatchSuccess);
        }
        void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.OnCardMatchSuccess, Callback_On_MatchSuccess);
        }
        private void Awake()
        {
            _particlePool = new ObjectPool<UIParticle>(
                CreateParticle,
                OnGetParticle,
                OnReleaseParticle,
                OnDestroyParticle,
                collectionCheck: true,
                defaultCapacity: _defaultCapacity,
                maxSize: _maxSize
            );
        }

        public UIParticle GetParticle()
        {
            return _particlePool.Get();
        }

        public void ReleaseParticle(UIParticle particle)
        {
            _particlePool.Release(particle);
        }


        //Callbacks

        private UIParticle CreateParticle()
        {
            var particle = Instantiate(_matchParticlePrefab, transform);
            return particle;
        }

        private void OnGetParticle(UIParticle particle)
        {
            particle.gameObject.SetActive(true);
        }

        private void OnReleaseParticle(UIParticle particle)
        {
            particle.gameObject.SetActive(false);
        }

        private void OnDestroyParticle(UIParticle particle)
        {
            Destroy(particle.gameObject);
        }

        private void Callback_On_MatchSuccess(object args)
        {
            List<BaseCard> matchedCards = args as List<BaseCard>;
            foreach (BaseCard card in matchedCards)
            {
                UIParticle particle = GetParticle();
                particle.gameObject.SetActive(true);
                particle.transform.position = card.transform.position;
                particle.Play();
                particle.DelayCallback(1f, ReleaseParticle);
            }

        }
    }
}
