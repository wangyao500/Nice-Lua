﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;
using AssetBundles;

[InitializeOnLoad]
public class AddressableTools
{


    [MenuItem("NICELua/Marked AssetsPackage Addressable", false, 52)]
    public static void RunCheckAssetBundle()
    {
        var start = DateTime.Now;
        CheckAssetBundles.Run();

        //更新assetmap
        List<AddressableAssetEntry> assets = new List<AddressableAssetEntry>();
        AASUtility.GetSettings().GetAllAssets(assets, false,
            (g) => { return g.name.Equals("lua"); });

        string[] address = assets.Select(e => e.address).ToArray();

        string assetFolder = Path.Combine(Application.dataPath, AssetBundleConfig.AssetsFolderName);
        
        var assetPathMap = Path.Combine(assetFolder, AssetBundleConfig.AssetsPathMapFileName);

        GameUtility.SafeWriteAllLines(assetPathMap, address);
        AssetDatabase.Refresh();


        //特殊文件
        string app_version = Path.Combine(assetFolder, "app_version.bytes");
        string assets_map = Path.Combine(assetFolder, "AssetsMap.bytes");
        string channel_name = Path.Combine(assetFolder, "channel_name.bytes");
        string notice_version = Path.Combine(assetFolder, "notice_version.bytes");
        string res_version = Path.Combine(assetFolder, "res_version.bytes");

        SingleFileAddress("global", app_version);
        SingleFileAddress("global", assets_map);
        SingleFileAddress("global", channel_name);
        SingleFileAddress("global", notice_version);
        SingleFileAddress("global", res_version);
        Debug.Log("Finished CheckAssetBundles.Run! use " + (DateTime.Now - start).TotalSeconds + "s");
        
    }

    public static void SingleFileAddress(string group, string path)
    {
        string relativePath = path.Substring(path.IndexOf("Assets\\") );
       
        AASUtility.AddAssetToGroup(AssetDatabase.AssetPathToGUID(relativePath), group);
    }
}

