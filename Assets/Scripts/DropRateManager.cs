using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;

    //Will be called before the object is destroyed to spawn dropped items
    //Self-contained to easily attach to other objects without dependencies
    void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) //Stops the spawning error from appearing when stopping play mode
        {
            return;
        }

        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();

        foreach(Drops rate in drops)
        {
            if(randomNumber <= rate.dropRate)
            {
                possibleDrops.Add(rate);
            }
        }
        //To check if there are possible drops
        if(possibleDrops.Count > 0)
        {
            Drops drops = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Instantiate(drops.itemPrefab, transform.position, Quaternion.identity);
        }
    }
}
