using UnityEngine;

namespace Behaviours
{
    public class ShieldPartBehavior : MonoBehaviour
    {
        private int numHits;
        private int currentHits;
        private int laserProjectileLayer;

        public int NumHits
        {
            set => numHits = value;
        }

        private void Start()
        {
            laserProjectileLayer = LayerMask.NameToLayer(ConstValues.LASER_ENEMY_LAYER);
            currentHits = numHits;
        }
        
        private void OnTriggerEnter2D(Collider2D obj)
        {
            if (obj.gameObject.layer != laserProjectileLayer)
            {
                return;
            }
            
            //Destroy laser
            Destroy(obj.gameObject);

            OnNumHits();
        }

        private void OnNumHits()
        {
            if (currentHits == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                --currentHits;
                LeanTween.scale(gameObject, Vector2.one * 1.50f, 0.5f).setEasePunch(); 
            }
        }
    }
}
