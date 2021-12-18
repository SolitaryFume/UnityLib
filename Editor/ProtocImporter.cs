using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;


[ScriptedImporter(2, new[] { "proto"})]
public class ProtocImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        //var prefax = Path.GetExtension(ctx.assetPath).Substring(1);
        var text = File.ReadAllText(ctx.assetPath, System.Text.Encoding.UTF8);
        var asset = new TextAsset(text);
        ctx.AddObjectToAsset("main obj", asset);
        ctx.SetMainObject(asset);
    }
}
