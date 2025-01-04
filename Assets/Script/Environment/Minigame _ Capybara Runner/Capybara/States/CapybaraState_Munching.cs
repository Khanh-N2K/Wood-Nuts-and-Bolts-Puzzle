using UnityEngine;
using UnityEngine.Tilemaps;

namespace Minigame_CapybaraRunner
{
    public class CapybaraState_Munching : StateMachineBehaviour
    {
        [Header("--- REFERENCES ---")]
        [SerializeField] private TileBase grassTile;
        [SerializeField] private float searchVerticalRange;
        [SerializeField] private string goOneBlockTrigger;

        private Capybara capybara;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.capybara = animator.GetComponent<Capybara>();
            Vector3 mouthPos = capybara.MouthTransform.position;
            Tilemap tileMap = capybara.GroundTileMap;

            HeartBasedBar stomachBar = capybara.StomachBar;
            if (this.SearchGrassTileVertical(mouthPos, tileMap))
            {
                stomachBar.RestoreFull();
                capybara.AddGold();
                animator.SetTrigger(this.goOneBlockTrigger);
            }
            else
            {
                stomachBar.Minus1Point();
                capybara.MinusGold();
            }
        }

        private bool SearchGrassTileVertical(Vector3 searchPos, Tilemap tileMap)
        {
            for (int i = 0; i <= this.searchVerticalRange; i++)
            {
                Vector3Int intSearchPos = tileMap.WorldToCell(searchPos + Vector3.up * i);
                if (this.SearchGrassTileAtPoint(intSearchPos, tileMap))
                    return true;
                
                intSearchPos = tileMap.WorldToCell(searchPos + Vector3.up * (-i));
                if (this.SearchGrassTileAtPoint(intSearchPos, tileMap))
                    return true;

            }

            return false;
        }

        private bool SearchGrassTileAtPoint(Vector3Int point, Tilemap tileMap)
        {
            TileBase tileFound = tileMap.GetTile(point);
            if (tileFound == this.grassTile)
            {
                return true;
            }
            return false;
        }
    }
}