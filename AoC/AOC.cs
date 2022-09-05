using BepInEx;
using RoR2;
using static AOCMod.AssetLoad;
using BepInEx.Configuration;
using AoC;
using BepInEx.Logging;

namespace AOCMod
{

    //[BepInDependency(R2API.R2API.PluginGUID)]

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency("com.TPDespair.ZetAspects")]
    [BepInDependency("com.groovesalad.GrooveSaladSpikestripContent", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.plasmacore.PlasmaCoreSpikestripContent", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Skell.GoldenCoastPlus", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.KomradeSpectre.Aetherium", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("bubbet.bubbetsitems", BepInDependency.DependencyFlags.SoftDependency)]

    public class AOC : BaseUnityPlugin
    {
        //We will be using 2 modules from R2API: ItemAPI to add our item and LanguageAPI to add our language tokens.
        //[R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI))]

        //BaseUnityPlugin itself inherits from MonoBehaviour, so you can use this as a reference for what you can declare and use in your plugin class: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html

        //The Plugin GUID should be a unique ID for this plugin, which is human readable (as it is used in places like the config).
        //If we see this PluginGUID as it is on thunderstore, we will deprecate this mod. Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "CuteDoge";
        public const string PluginName = "ArtifactOfChosen";
        public const string PluginVersion = "1.1.0";

        private static ArtifactDef AoC;
        public static ManualLogSource log;
        //AOC = ArtifactOfChosen

        public static PluginInfo pluginInfo;

        public void Awake()
        {
            log = Logger;
            pluginInfo = Info;
            AOCConfig.Init(Paths.ConfigPath);
        }


        // Start is called before the first frame update
        void Start()
        {
            PopulateAssets();
            ContentPackProvider.Initialize();
            Artifact.InitializeArtifact();
        }

    }

}
