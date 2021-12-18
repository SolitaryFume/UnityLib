using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(2, new[] { "pb" })]
public class PbImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var text = File.ReadAllText(ctx.assetPath);
        var asset = new TextAsset(text);
        ctx.AddObjectToAsset("main obj", asset);
        ctx.SetMainObject(asset);
    }
}