using UnityEngine;
using UnityEngine.Tilemaps;

namespace Minigame_CapybaraRunner
{
    public class Capybara : MonoBehaviour
    {
        [Header("--- STATES PARAMETERS ---")]
        [Header("Munching")]
        [SerializeField] private Tilemap groundTileMap;
        [SerializeField] private Transform mouthTransform;
        [SerializeField] private GameObject addGoldText;
        [SerializeField] private GameObject minusGoldText;
        [SerializeField] private HeartBasedBar_DecreasedOverTime stomachBar;

        [Header("Movement")]
        [SerializeField] private float speed;

        // --- READ-ONLY PROPERTIES ---
        // Munching
        public Tilemap GroundTileMap { get => this.groundTileMap; }
        public Transform MouthTransform { get => mouthTransform; }
        public HeartBasedBar StomachBar { get => this.stomachBar; }

        // Movement
        public float GetSpeed { get => this.speed; }

        private void Start()
        {
            this.stomachBar.onDecreaseUnderMinPoint += this.MinusGold;
        }

        public void AddGold()
        {
            GoldManager.Instance.AddGold(1);
            Effect.Vibrate();
            this.addGoldText.SetActive(true);
        }

        public void MinusGold()
        {
            GoldManager.Instance.AddGold(-1);
            Handheld.Vibrate();
            this.minusGoldText.SetActive(true);
        }
    }
}
