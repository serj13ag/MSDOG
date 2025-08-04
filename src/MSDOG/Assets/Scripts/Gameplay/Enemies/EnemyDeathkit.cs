using DG.Tweening;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyDeathkit : MonoBehaviour
    {
        [SerializeField] private GameObject[] _parts;
        [SerializeField] private Transform _forceCenter;
        [SerializeField] private float _force;
        [SerializeField] private float _forceRadius;
        [SerializeField] private float _hideCooldown = 5f;
        [SerializeField] private float _torqueMultiplier = 10f;

        public void Init()
        {
            foreach (var part in _parts)
            {
                var rb = part.gameObject.AddComponent<Rigidbody>();

                var meshCollider = part.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;

                rb.AddExplosionForce(_force, _forceCenter.transform.position, _forceRadius);

                var randomTorque = new Vector3(
                    Random.Range(-_torqueMultiplier, _torqueMultiplier),
                    Random.Range(-_torqueMultiplier, _torqueMultiplier),
                    Random.Range(-_torqueMultiplier, _torqueMultiplier)
                );
                rb.AddTorque(randomTorque);
            }

            DOTween.Sequence()
                .InsertCallback(_hideCooldown, () =>
                {
                    foreach (var part in _parts)
                    {
                        part.GetComponent<Rigidbody>().isKinematic = true;
                    }
                })
                .Insert(_hideCooldown, transform.DOMoveY(-2f, 5f))
                .OnComplete(() => Destroy(gameObject));
        }
    }
}