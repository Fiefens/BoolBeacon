using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeTests
{
    [UnityTest]
    public IEnumerator ControlBlock_Spawns_And_Initializes()
    {
        var go = new GameObject("TestBlock");
        go.AddComponent<Rigidbody>().isKinematic = true;
        var cb = go.AddComponent<ControlBlock>();

        yield return null;

        Assert.IsNotNull(cb, "ControlBlock did not initialize.");
        Assert.IsTrue(cb.enabled);
    }

    [UnityTest]
    public IEnumerator ControlBlock_Moves_When_Direction_Is_Set()
    {
        var go = new GameObject("TestBlock");
        go.AddComponent<Rigidbody>().isKinematic = true;

        var cb = go.AddComponent<ControlBlock>();
        cb.SetDirection(Vector3.right);

        GameManager.activeBlock = go; 

        Vector3 start = go.transform.position;

        yield return new WaitForSeconds(0.5f);

        Assert.AreNotEqual(start, go.transform.position, "ControlBlock did not move.");
    }


    [UnityTest]
    public IEnumerator ControlBlock_Launch_Disables_Movement()
    {
        var go = new GameObject("TestBlock");
        var rb = go.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        var cb = go.AddComponent<ControlBlock>();

        cb.Launch(new Vector3(0, 10, 0));

        yield return new WaitForFixedUpdate();

        Assert.IsFalse(cb.enabled, "ControlBlock was not disabled after launch.");
        Assert.IsFalse(rb.isKinematic, "Rigidbody should no longer be kinematic.");
    }

    [UnityTest]
    public IEnumerator Database_Registers_TruthValue()
    {
        Database.AssignedTruths.Clear();
        char testLetter = Database.GetNextLetter();

        Database.RegisterBlock(testLetter, true);

        yield return null;

        Assert.IsTrue(Database.AssignedTruths.ContainsKey(testLetter),
            "Truth value was not registered.");
        Assert.IsTrue(Database.AssignedTruths[testLetter]);
    }
}
