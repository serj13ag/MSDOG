using DG.Tweening;
using UnityEngine;

namespace Core.Enemies
{
    public class EnemyDeathkit : MonoBehaviour
    {
        [SerializeField] private GameObject[] _parts;
        [SerializeField] private Transform _forceCenter;
        [SerializeField] private float _force;
        [SerializeField] private float _forceRadius;
        [SerializeField] private float _hideCooldown = 5f;

        public void Init()
        {
            foreach (var part in _parts)
            {
                var rb = part.gameObject.AddComponent<Rigidbody>();

                var meshCollider = part.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;

                rb.AddExplosionForce(_force, _forceCenter.transform.position, _forceRadius);

                // var randomTorque = new Vector3(
                //     Random.Range(-torqueMultiplier, torqueMultiplier),
                //     Random.Range(-torqueMultiplier, torqueMultiplier),
                //     Random.Range(-torqueMultiplier, torqueMultiplier)
                // );
                // rb.AddTorque(randomTorque);
                //
                // Vector3 direction = (part.position - impactPoint).normalized;
                // rb.AddForce(direction * explosionForce * 0.5f);
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