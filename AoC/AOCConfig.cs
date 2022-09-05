using AOCMod;
using BepInEx.Configuration;
using System.IO;

namespace AoC
{
    public class AOCConfig
    {
        public static ConfigEntry<string> list;
        public static ConfigEntry<AOCMode> mode;
        public static ConfigEntry<int> NumOfAspectsPerStageConf;
        public static ConfigEntry<string> FirstArtifactGivenConf;
        public static ConfigEntry<bool> multistack;
        public static ConfigEntry<bool> postTeleporterAspect;

        public static void Init(string configFile)
        {
            var config = new ConfigFile(Path.Combine(configFile, AOC.PluginGUID + ".cfg"), true);
            list = config.Bind("General", "AspectList", "ZetAspectBlue, ZetAspectEarth, ZetAspectHaunted, ZetAspectLunar, ZetAspectPoison, ZetAspectRed, ZetAspectVoid, ZetAspectWhite, ZetAspectGold, ZetAspectSanguine, ZetAspectSepia, ZetAspectPlated, ZetAspectWarped, ZetAspectVeiled, ZetAspectAragonite", "List of items to choose from.");
            mode = config.Bind("General", "Distribute Mode", AOCMode.RandomEachStage, "How the aspect should be given. AllRandom gives different random aspects to each, AllSame gives the same random aspect set to everyone. All other options only gives it to one person.");
            NumOfAspectsPerStageConf = config.Bind(
                "General",
                "Artifacts Per Stage",
                1,
                "This is the amount of artifacts thet will be given to the chosen player each stage"
            );
            multistack = config.Bind(
                "General",
                "Multi Stack",
                true,
                "Whether multiple stacks of artifacts should be same."
            );
            FirstArtifactGivenConf = config.Bind(
                "General",
                "First Artifact Given",
                "Random",
                "Specifies if you want to get a concrete artifact at the start. Available options: ZetAspectBlue, ZetAspectEarth, ZetAspectHaunted, ZetAspectLunar, ZetAspectPoison, ZetAspectRed, ZetAspectVoid, ZetAspectWhite. Anything else will make it random. Also integrations Aspects are supported too."
            );
            postTeleporterAspect = config.Bind("General", "Retain Aspect Post Teleporter", false, "Set to true to remove the aspect at the start of next stage instead.");
        }

        public enum AOCMode
        {
            Host,
            Random,
            RandomEachStage,
            AllRandom,
            AllSame
        }
    }
}
