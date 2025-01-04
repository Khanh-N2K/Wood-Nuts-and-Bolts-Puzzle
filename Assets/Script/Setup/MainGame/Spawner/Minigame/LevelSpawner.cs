using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame_ImagePuzzle
{
    public class LevelSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MinigameImageSO imageSO;
        [SerializeField] private GameObject boltPrefab;
        [SerializeField] private GameObject nutsPrefab;
        [SerializeField] private WallHole outerNut;

        [Header("Params")]
        [SerializeField] private int targetWidth;

        [Header("Variables")]
        private GameObject boltHolder;
        private GameObject nutHolder;
        private float nutSize;
        private int currentPictureIndex = -1;

        private void Awake()
        {
            this.boltHolder = Config.Instance.boltHolder;
            this.nutHolder = Config.Instance.wallHoleHolder;

            this.nutSize = this.nutsPrefab.transform.GetComponent<WallHole>().StandardRadius * 2;
        }

        private void Start()
        {
            this.SpawnRandomPicture();
        }

        public void SpawnRandomPicture()
        {
            int newPictureIndex = this.currentPictureIndex;
            while(this.currentPictureIndex == newPictureIndex)
            {
                newPictureIndex = Random.Range(0, this.imageSO.list.Count);
            }
            this.currentPictureIndex = newPictureIndex;

            this.SpawnImage(newPictureIndex);
        }

        private void SpawnImage(int index)
        {
            this.DespawnOldImage();

            Color[] pixels = this.GetPixelsFromImage(index);
            int targetHeight = this.GetTargetHeight(index);
            this.SpawnNewImage(pixels, targetHeight);
        }
        private void DespawnOldImage()
        {
            GameObject boltObj;
            for(int i=0; i<boltHolder.transform.childCount; i++)
            {
                boltObj = boltHolder.transform.GetChild(i).gameObject;
                Destroy(boltObj);
            }

            GameObject nutObj;
            for(int i=0; i<nutHolder.transform.childCount; i++)
            {
                nutObj = nutHolder.transform.GetChild(i).gameObject;
                Destroy(nutObj);
            }
        }

        private Color[] GetPixelsFromImage(int imageIndex)
        {
            Texture2D originalTexture = this.imageSO.list[imageIndex];
            int targetHeight = this.GetTargetHeight(imageIndex);
            Texture2D resizedTexture = ResizeTexture(originalTexture, targetWidth, targetHeight);
            Color[] pixels = resizedTexture.GetPixels();
            return pixels;
        }

        private int GetTargetHeight(int imageIndex)
        {
            Texture2D originalTexture = this.imageSO.list[imageIndex];
            float aspectRatio = (float)originalTexture.height / originalTexture.width;
            int targetHeight = Mathf.RoundToInt(targetWidth * aspectRatio);
            return targetHeight;
        }

        Texture2D ResizeTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
            Graphics.Blit(source, rt);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;

            Texture2D result = new Texture2D(targetWidth, targetHeight);
            result.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            result.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(rt);

            return result;
        }

        private void SpawnNewImage(Color[] pixels, int targetHeight)
        {
            float spacing = this.nutSize;
            Vector3 startPosition = transform.position - new Vector3(targetWidth / 2f, targetHeight / 2f, 0) * spacing;
            List<Bolt> boltList = new();
            List<WallHole> nutList = new();
            for (int y = 0; y < targetHeight; y++)
            {
                for (int x = 0; x < targetWidth; x++)
                {
                    Color pixelColor = pixels[y * targetWidth + x];
                    if (pixelColor.a > 0)
                    {
                        Vector3 position = startPosition + new Vector3(x, y, 0) * spacing;

                        Bolt bolt = this.SpawnBolts(position, pixelColor);
                        boltList.Add(bolt);

                        WallHole nut = this.SpawnNuts(bolt.transform, pixelColor);
                        nutList.Add(nut);
                    }
                }
            }

            StartCoroutine(this.ShuffleBolts(boltList, nutList));
        }

        private Bolt SpawnBolts(Vector3 position, Color color)
        {
            GameObject boltObj = Instantiate(this.boltPrefab, position, Quaternion.identity, this.boltHolder.transform);
            boltObj.GetComponentInChildren<Minigame_Color>()
                    .SetColor(color);
            boltObj.transform.Find("Model").Find("FullBolt").GetComponent<SpriteRenderer>().material.color = color;
            return boltObj.GetComponent<Bolt>();
        }

        private WallHole SpawnNuts(Transform spawnTransform, Color color)
        {
            GameObject nutObj = Instantiate(this.nutsPrefab, spawnTransform.position, Quaternion.identity, this.nutHolder.transform);
            nutObj.GetComponentInChildren<Minigame_Color>()
                    .SetColor(color);

            return nutObj.GetComponent<WallHole>();
        }

        private IEnumerator ShuffleBolts(List<Bolt> boltList, List<WallHole> nutList)
        {
            for(int currentIndex=0; currentIndex<boltList.Count; currentIndex++)
            {
                int randomIndex = Random.Range(0, boltList.Count);

                Bolt tmpBolt = boltList[currentIndex];
                boltList[currentIndex] = boltList[randomIndex];
                boltList[randomIndex] = tmpBolt;

                boltList[currentIndex].transform.position = nutList[currentIndex].transform.position;
                boltList[randomIndex].transform.position = nutList[randomIndex].transform.position;

                boltList[currentIndex].transform.Find("Model").Find("Default").GetComponent<SpriteRenderer>().sortingOrder = 1000 - currentIndex;
                boltList[currentIndex].transform.Find("Model").Find("FullBolt").GetComponent<SpriteRenderer>().sortingOrder = 1000 - currentIndex;
                boltList[randomIndex].transform.Find("Model").Find("Default").GetComponent<SpriteRenderer>().sortingOrder = 1000 - currentIndex;
                boltList[randomIndex].transform.Find("Model").Find("FullBolt").GetComponent<SpriteRenderer>().sortingOrder = 1000 - currentIndex;

                boltList[currentIndex].ChangeWallHole(outerNut);
                boltList[randomIndex].ChangeWallHole(nutList[randomIndex]);
                boltList[currentIndex].ChangeWallHole(nutList[currentIndex]);
                
                yield return null;
            }
        }
    }
}
