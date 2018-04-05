using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int MaxLife = 10;
    public int currentLife;
    public List<GameObject> followingAis;

    private void Start()
    {
        currentLife = MaxLife;
        followingAis = new List<GameObject>();
    }

    public void GetDamage(int damage)
    {
        currentLife -= damage;
        if(currentLife < 1)
        {
            Destroy(gameObject);
        }
    }

    public void FreeTheAis(GameObject safeZoneTarget)
    {
        foreach(var ai in followingAis)
        {
            ai.GetComponent<NPCHuman>().SaveHuman(safeZoneTarget);
        }

        followingAis.Clear();
    }
}
