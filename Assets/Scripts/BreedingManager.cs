using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BreedingManager : MonoBehaviour
{
    public InventorySlot Left, Right, Output;

    public GameObject OutputPrefab;

    public List<InventoryItem> RandomSeeds;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Breed()
    {
        if (Output.GetInventoryItem() == null &&
            Left.GetInventoryItem() != null &&
            Right.GetInventoryItem() != null)
        {
            var itemRes = Combine(Left.GetInventoryItem(), Right.GetInventoryItem());

            var tmp = GameObject.Instantiate(OutputPrefab, Output.transform);
            tmp.GetComponent<DraggableItem>().InventoryItem = itemRes;


            Left.Take();
            Right.Take();
        }
    }

    private InventoryItem Combine(InventoryItem item1, InventoryItem item2)
    {
        InventoryItem output = new InventoryItem();
        output.Name = item1.Name;
        output.Genes = new List<Gene>();
        output.Quantity = Random.Range(1, 3);

        for (int i = 0; i < item1.Genes.Count; i++)
        {
            Gene currentGene = new Gene();
            currentGene.Name = item1.Genes[i].Name;
            currentGene.DominantValue = item1.Genes[i].DominantValue;
            currentGene.RecessiveValue = item1.Genes[i].RecessiveValue;

            Gene left = item1.Genes[i];
            Gene right = item2.Genes[i];
			float t = Random.Range(0.0f, 1.0f);

            string leftString = "";
            string rightString = "";

            switch (left.Type)
            {
                case GeneType.HOMOZYGOUS:
                    leftString = "AA";
                    break;
                case GeneType.HETEROZYGOUS:
                    leftString = "Aa";
                    break;
                case GeneType.HOMOZYGOUS_RECESSIVE:
                    leftString = "aa";
                    break;
            }
			switch (right.Type)
			{
				case GeneType.HOMOZYGOUS:
					rightString = "AA";
					break;
				case GeneType.HETEROZYGOUS:
					rightString = "Aa";
					break;
				case GeneType.HOMOZYGOUS_RECESSIVE:
					rightString = "aa";
					break;
			}

            int xIdx = Random.Range(0, 2);
            int yIdx = Random.Range(0, 2);

            string res = leftString[xIdx].ToString() + rightString[yIdx];

            if (res == "aA")
                res = "Aa";

            switch (res)
            {
                case "AA":
                    currentGene.Type = GeneType.HOMOZYGOUS;
                    break;
				case "Aa":
                    currentGene.Type = GeneType.HETEROZYGOUS;
					break;
				case "aa":
                    currentGene.Type = GeneType.HOMOZYGOUS_RECESSIVE;
					break;
			}
			output.Genes.Add(currentGene);
        }

        return output;
    }

    public InventoryItem GetRandomSeed()
    {
        //      var res = new InventoryItem();

        //      res.Name = "Seed";

        //      res.Genes = new List<Gene>();

        //      res.Genes.Add(new Gene() { Name = "Speed 1", DominantValue = 1.5f, RecessiveValue = 1, Type = RandomGeneType() });
        //res.Genes.Add(new Gene() { Name = "Speed 2", DominantValue = 1.5f, RecessiveValue = 1, Type = RandomGeneType() });
        //res.Genes.Add(new Gene() { Name = "Speed 3", DominantValue = 0.5f, RecessiveValue = 3, Type = RandomGeneType() });

        int idx = Random.Range(0, RandomSeeds.Count);
        var res = RandomSeeds[idx];

        return res;
	}

    public static GeneType RandomGeneType()
    {
        float t = Random.Range(0.0f, 1.0f);

        if (t < (1.0f / 3.0f))
        {
            return GeneType.HOMOZYGOUS;
        }
        else if (t < (2.0f / 3.0f))
        {
            return GeneType.HETEROZYGOUS;
        }
        else
        {
            return GeneType.HOMOZYGOUS_RECESSIVE;
        }
    }
}
