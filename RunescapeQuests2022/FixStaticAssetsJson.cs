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
    //TODO: Error checking, null reference fixes
    public class FixStaticAssetsJson
    {
        public FixStaticAssetsJson()
        {
            //Current application path
            var currentPath = AppDomain.CurrentDomain.BaseDirectory;
            var staticWebAssets = currentPath + AppDomain.CurrentDomain.FriendlyName + ".staticwebassets.json";
            var staticWebAssetsRuntime = currentPath + AppDomain.CurrentDomain.FriendlyName + ".staticwebassets.runtime.json";

            //json options to keep indentation
            JsonSerializerOptions options = new()
            { 
                WriteIndented = true

            };

            //open and deserialize the static web assets file
            var staticWebAssetsJson = JsonSerializer.Deserialize<StaticWebAssets>(File.ReadAllText(staticWebAssets));
            //first reference to the incorrect path, lets change it to the current app directory
            staticWebAssetsJson.DiscoveryPatterns[0].ContentRoot = currentPath + "wwwroot\\";

            //the static web asset file also has references to each file within wwwroot, which also contains the incorrect path
            foreach(var item in staticWebAssetsJson.Assets)
            {
                item.ContentRoot = currentPath + "wwwroot\\";
                //get the file name from the saved path
                var wwwrootStr = item.Identity.Substring(item.Identity.IndexOf("wwwroot"));
                item.Identity = currentPath + wwwrootStr;
            }
            //Seralize the class and write it back into the file 
            var staticWebAssetsSerialized = JsonSerializer.Serialize(staticWebAssetsJson, options);
            File.WriteAllText(staticWebAssets, staticWebAssetsSerialized);

            //Next file is the Static Web Assets Runtime file, read and deserialize the file
            RuntimeAssetJson runtimeAssetJson = JsonSerializer.Deserialize<RuntimeAssetJson>(File.ReadAllText(staticWebAssetsRuntime));
            //only reference is the content roots, fix this, serialize the class, and save it 
            runtimeAssetJson.ContentRoots[0] = currentPath + "wwwroot\\";
            var runtimeAssetJsonConvert = JsonSerializer.Serialize(runtimeAssetJson, options);
            File.WriteAllText(staticWebAssetsRuntime, runtimeAssetJsonConvert);
        }
    }

    //Static Web Assets Runtime Json Class
    public class RuntimeAssetJson
    {
        public List<string> ContentRoots { get; set; }
        //we require nothing in root so just save as dynamic and let the runtime deal with it
        public dynamic Root { get; set; }
    }

    //Static Web Asset Json Classes
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
}
