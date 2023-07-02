using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP : MonoBehaviour
{
    static TP[] tps;
    static bool canTP = true;
    static List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    private void Start()
    {
        if(tps == null)
        {
            tps = FindObjectsOfType<TP>();
        }
        if (meshRenderers.Count <= 0)
        {
            foreach (var tp in tps)
            {
                meshRenderers.Add(tp.GetComponent<MeshRenderer>());
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print("SomethingEntered");
        var player = other.GetComponent<CharacterMovementHandler>();
        if (player)
        {
            print("PlayerEntered");
            if(tps[0] == this)
            {
                Activate(player, 1);
            }
            else
            {
                Activate(player, 0);
            }
        }
    }

    private void Activate(CharacterMovementHandler player, int id)
    {
        if (canTP)
        {
            player.SetPosition(tps[id].transform.position);
            StartCoroutine(Teleport());
        }
    }

    private static IEnumerator Teleport()
    {
        canTP = false;
        foreach (var renderer in meshRenderers)
        {
            print("Renderer Disabled");
            renderer.enabled = false;
        }
        yield return new WaitForSeconds(5);
        canTP = true;
        foreach (var renderer in meshRenderers)
        {
            renderer.enabled = true;
        }
    }
}
