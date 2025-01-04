using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animas
{
    public class ItemKey : MonoBehaviour
    {
        [SerializeField] private BoardScrewHole lockedHole;
        private bool isEnable = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isEnable) return;

            if (collision.TryGetComponent<Plate>(out var plate))
            {
                isEnable = false;
                OnActive();
                //plate.GetBomb();
                //Explode();
            }
        }

        public void OnActive()
        {
            //effect here
            lockedHole.Unlock();
            gameObject.SetActive(false);
        }
    }
}