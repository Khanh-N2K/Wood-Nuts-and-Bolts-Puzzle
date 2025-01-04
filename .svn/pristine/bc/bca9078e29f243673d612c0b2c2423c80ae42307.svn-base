using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animas
{
    public class BombStorage : MonoBehaviour
    {
        public static BombStorage Instance;
        [SerializeField] GameObject bombPref;
        List<GameObject> activeBombs = new List<GameObject>();
        List<GameObject> deactiveBombs = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
        }
        public GameObject GetBomb()
        {
            GameObject bomb;
            if (deactiveBombs.Count > 0)
            {
                bomb = deactiveBombs[0];
                deactiveBombs.RemoveAt(0);
            }
            else
            {
                bomb = Instantiate(bombPref,transform);
            }

            activeBombs.Add(bomb);
            bomb.gameObject.SetActive(true);
            return bomb;
        }
        public GameObject ReturnBomb(GameObject bomb)
        {
            bomb.gameObject.SetActive(false);
            activeBombs.Remove(bomb);
            deactiveBombs.Add(bomb);
            return bomb;
        }
        public void ReturnBomb(GameObject bomb, float time)
        {
            StartCoroutine(IEReturnBomb(bomb, time));
        }
        public IEnumerator IEReturnBomb(GameObject bomb,float time)
        {
            yield return new WaitForSeconds(time);
            ReturnBomb(bomb);
        }
    }
}
