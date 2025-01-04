using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace Minigame_CapybaraRunner
{
    public class TileMapSpawner : MonoBehaviour
    {
        [Header("References")]
        private Tilemap tileMap;
        [SerializeField] private Capybara capybara;

        [Header("Parameters")]
        [SerializeField] private List<TileBase> tileList = new();
        [SerializeField] private List<float> tileProportionList = new();
        [SerializeField] private int spawnLength;
        [SerializeField] private int spawnHeight;
        [Tooltip("Count from spawned position in x-axis")] [SerializeField] private int spawnTriggerOffset;
        [SerializeField] private Vector3Int spawnPos;

        private float capybaraSpeed;
        private Vector3 spawnTriggerPos;
        private Queue<Vector3Int> oldTilePositionQueue = new();
        private Queue<Vector3Int> currentTilePositionQueue = new();

        private float tileProportion_TotalWeight = 0;
        private List<float> tileProportion_CumulativeWeight_List = new();

        private void Awake()
        {
            this.tileMap = GetComponent<Tilemap>();

            this.capybaraSpeed = this.capybara.GetSpeed;
            foreach(float tileProportion in this.tileProportionList)
            {
                this.tileProportion_TotalWeight += tileProportion;
                this.tileProportion_CumulativeWeight_List.Add(tileProportion_TotalWeight);
            }
        }

        private void Start()
        {
            this.SpawnToRight();
        }

        private void SpawnToRight()
        {
            this.DespawnOldTiles();

            this.MoveAllQueueElementsToDifferentQueue(this.currentTilePositionQueue, this.oldTilePositionQueue);

            for (int yIndex=0; yIndex<this.spawnHeight; yIndex++)
            {
                for(int xIndex=0; xIndex<this.spawnLength; xIndex++)
                {
                    Vector3Int tilePosition = this.spawnPos + Vector3Int.right * xIndex + Vector3Int.up * yIndex;
                    this.SpawnRandomTile(tilePosition);

                    this.currentTilePositionQueue.Enqueue(tilePosition);
                }
            }

            this.spawnTriggerPos = spawnPos + Vector3Int.right * this.spawnTriggerOffset;
            spawnPos += Vector3Int.right * this.spawnLength;

            StartCoroutine(CheckSpawn());
        }

        private void DespawnOldTiles()
        {
            while(this.oldTilePositionQueue.Count > 0)
            {
                Vector3Int tilePosition = this.oldTilePositionQueue.Dequeue();
                this.tileMap.SetTile(tilePosition, null);
            }
        }

        private void MoveAllQueueElementsToDifferentQueue(Queue<Vector3Int> movedFrom_Queue, Queue<Vector3Int> moveTo_Queue)
        {
            while(movedFrom_Queue.Count > 0)
            {
                moveTo_Queue.Enqueue(movedFrom_Queue.Dequeue());
            }
        }

        private void SpawnRandomTile(Vector3Int tilePosition)
        {
            float randomWeight = Random.Range(0, this.tileProportion_TotalWeight);
            for(int i=0; i<this.tileProportion_CumulativeWeight_List.Count; i++)
            {
                if(randomWeight < this.tileProportion_CumulativeWeight_List[i])
                {
                    this.tileMap.SetTile(tilePosition, this.tileList[i]);
                    break;
                }
            }
        }

        private IEnumerator CheckSpawn()
        {
            float distanceToSpawnTrigger = this.spawnTriggerPos.x - this.capybara.transform.position.x;
            if (distanceToSpawnTrigger <= 0)
                this.SpawnToRight();
            else
            {
                float waitTime = (float)distanceToSpawnTrigger / this.capybaraSpeed;
                yield return new WaitForSeconds(waitTime);
                StartCoroutine(CheckSpawn());
            }
        }
    }
}
