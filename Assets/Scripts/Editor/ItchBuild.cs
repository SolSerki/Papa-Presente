using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Menú "Papa Presente > Build para itch.io": arma el build de Windows (sin Development Build)
// y lo comprime en un zip listo para subir a itch.io.
public static class ItchBuild
{
    private const string BuildsFolder = "../Builds";
    private const string ExeName = "Papa Presente.exe";

    [MenuItem("Papa Presente/Build para itch.io (Windows)")]
    public static void BuildWindows()
    {
        string buildsDir = Path.GetFullPath(Path.Combine(Application.dataPath, "..", BuildsFolder));
        string playerDir = Path.Combine(buildsDir, "Windows");
        string zipPath = Path.Combine(buildsDir, "PapaPresente_Windows_" + PlayerSettings.bundleVersion + ".zip");

        if (Directory.Exists(playerDir))
            Directory.Delete(playerDir, true);

        var options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
            locationPathName = Path.Combine(playerDir, ExeName),
            target = BuildTarget.StandaloneWindows64,
            targetGroup = BuildTargetGroup.Standalone,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
        {
            Debug.LogError("[ItchBuild] El build falló: " + report.summary.result + " (" + report.summary.totalErrors + " errores)");
            return;
        }

        CreateZip(playerDir, zipPath);
        Debug.Log("[ItchBuild] Listo: " + zipPath);
        EditorUtility.RevealInFinder(zipPath);
    }

    // Comprime el build sin las carpetas que Unity marca como "no subir" (debug y backups)
    private static void CreateZip(string sourceDir, string zipPath)
    {
        if (File.Exists(zipPath))
            File.Delete(zipPath);

        using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
        {
            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relative = file.Substring(sourceDir.Length + 1).Replace('\\', '/');
                if (relative.Contains("DoNotShip") || relative.Contains("DontShip"))
                    continue;

                zip.CreateEntryFromFile(file, relative, System.IO.Compression.CompressionLevel.Optimal);
            }
        }
    }
}
