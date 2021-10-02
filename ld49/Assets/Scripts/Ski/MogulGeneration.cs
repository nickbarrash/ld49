using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MogulGeneration : MonoBehaviour
{
    public GameObject mogulPrefab;
    public string seed;

    private int moguls = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMogulsAlgo1();
    }

    public void GenerateMogulsAlgo1()
    {
    
        for(float i = 0; i < 1000; i++)
        {
            var step = GetHashMod(i.ToString(), 100);

            int moguls = 0;
            if (step > 90)
                moguls = 5;
            else if (step > 80)
                moguls = 4;
            else if (step > 60)
                moguls = 3;
            else if (step > 40)
                moguls = 2;
            else
                moguls = 1;

            for (int j = moguls; j > 0; j--)
            {
                var x = (float)GetHashMod(".x." + i + " " + j, 30);
                Debug.Log(x);
                var force = (float)GetHashMod(".velo." + i + " " + j, 10);
                CreateMogul(new Vector2(x, -1 * i), force);
            }
        }
    }

    private int GetHashMod(string adjust, int mod)
    {
        return Random.Range(-1 * mod, mod)/*(seed + ","  + adjust).GetHashCode()*/ % mod;
    }

    public void CreateMogul(Vector2 position, float force)
    {
        var tmpMogulGameObject = Instantiate(mogulPrefab, transform);
        tmpMogulGameObject.transform.position = position;
        tmpMogulGameObject.name = $"Mogul_{moguls++}";
        var mogul = tmpMogulGameObject.GetComponent<Mogul>();
        mogul.force = force;
    }
}
