using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RunescapeQuests2022
{
    //For some reason staticwebassets.json and staticwebassets.runtime.json saves my build directory for wwwroot on
    //compliation. I'm sure there is some way to fix it but I have no idea at this moment so I am making a dirty hack that will
    //load in the json, edit the files to be the application directory , and save it before any calls to wwwroot happen
    public class FixStaticAssetsJson
    {
        public FixStaticAssetsJson()
        {
            var currentPath = AppDomain.CurrentDomain.BaseDirectory;
            var staticWebAssets = currentPath + AppDomain.CurrentDomain.FriendlyName + ".staticwebassets.json";
            var staticWebAssetsJson = JsonSerializer.Deserialize<StaticWebAssets>(File.ReadAllText(staticWebAssets));
            staticWebAssetsJson.DiscoveryPatterns[0].ContentRoot = currentPath + "wwwroot\\";
            foreach(var item in staticWebAssetsJson.Assets)
            {
                item.ContentRoot = currentPath + "wwwroot\\";
                var wwwrootIndex = item.Identity.IndexOf("wwwroot");
                var wwwrootStr = item.Identity.Substring(wwwrootIndex);
                item.Identity = currentPath + wwwrootStr;
            }
            var jsonSerialized = JsonSerializer.Serialize(staticWebAssetsJson);
            File.WriteAllText(staticWebAssets, jsonSerialized);

            var staticWebAssetsRuntime = currentPath + AppDomain.CurrentDomain.FriendlyName + ".staticwebassets.runtime.json";
            var runtimeAssetText = File.ReadAllText(staticWebAssetsRuntime);
            RuntimeAssetJson runtimeAssetJson = JsonSerializer.Deserialize<RuntimeAssetJson>(runtimeAssetText);
            runtimeAssetJson.ContentRoots[0] = currentPath + "wwwroot\\";
            var runtimeAssetJsonConvert = JsonSerializer.Serialize(runtimeAssetJson);
            File.WriteAllText(staticWebAssetsRuntime, runtimeAssetJsonConvert);
        }
    }

    public class RuntimeAssetJson
    {
        public List<string> ContentRoots { get; set; }
        public dynamic Root { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class DiscoveryPattern
    {
        public string Name { get; set; }
        public string ContentRoot { get; set; }
        public string BasePath { get; set; }
        public string Pattern { get; set; }
    }

    public class Asset
    {
        public string Identity { get; set; }
        public string SourceId { get; set; }
        public string SourceType { get; set; }
        public string ContentRoot { get; set; }
        public string BasePath { get; set; }
        public string RelativePath { get; set; }
        public string AssetKind { get; set; }
        public string AssetMode { get; set; }
        public string AssetRole { get; set; }
        public string RelatedAsset { get; set; }
        public string AssetTraitName { get; set; }
        public string AssetTraitValue { get; set; }
        public string CopyToOutputDirectory { get; set; }
        public string CopyToPublishDirectory { get; set; }
        public string OriginalItemSpec { get; set; }
    }

    public class StaticWebAssets
    {
        public int Version { get; set; }
        public string Hash { get; set; }
        public string Source { get; set; }
        public string BasePath { get; set; }
        public string Mode { get; set; }
        public string ManifestType { get; set; }
        public List<object> RelatedManifests { get; set; }
        public List<DiscoveryPattern> DiscoveryPatterns { get; set; }
        public List<Asset> Assets { get; set; }
    }
}
