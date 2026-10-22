using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Menú "Papa Presente > Build para itch.io": arma el build (sin Development Build)
// y lo comprime en un zip listo para subir a itch.io.
public static class ItchBuild
{
    private const string BuildsFolder = "../Builds";

    [MenuItem("Papa Presente/Build para itch.io (Windows)")]
    public static void BuildWindows()
    {
        Build(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, "Windows", "Papa Presente.exe");
    }

    // Versión para jugar en el navegador de itch.io. Necesita el módulo "WebGL Build Support" de Unity Hub.
    [MenuItem("Papa Presente/Build para itch.io (Navegador - WebGL)")]
    public static void BuildWebGL()
    {
        if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.WebGL, BuildTarget.WebGL))
        {
            EditorUtility.DisplayDialog("Falta el módulo WebGL",
                "Instalalo desde Unity Hub: Installs > (tu versión de Unity) > Add modules > WebGL Build Support.", "OK");
            return;
        }

        // itch.io no manda los encabezados de compresión que espera Unity:
        // con Decompression Fallback el juego se descomprime solo en el navegador.
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
        PlayerSettings.WebGL.decompressionFallback = true;
        PlayerSettings.defaultWebScreenWidth = 960;
        PlayerSettings.defaultWebScreenHeight = 540;

        // En la carpeta del build queda index.html, que itch.io necesita en la raíz del zip
        Build(BuildTarget.WebGL, BuildTargetGroup.WebGL, "WebGL", "");
    }

    private static void Build(BuildTarget target, BuildTargetGroup group, string platformName, string fileName)
    {
        string buildsDir = Path.GetFullPath(Path.Combine(Application.dataPath, "..", BuildsFolder));
        string playerDir = Path.Combine(buildsDir, platformName);
        string zipPath = Path.Combine(buildsDir, "PapaPresente_" + platformName + "_" + PlayerSettings.bundleVersion + ".zip");

        if (Directory.Exists(playerDir))
            Directory.Delete(playerDir, true);

        if (EditorUserBuildSettings.activeBuildTarget != target)
            EditorUserBuildSettings.SwitchActiveBuildTarget(group, target);

        var options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
            locationPathName = string.IsNullOrEmpty(fileName) ? playerDir : Path.Combine(playerDir, fileName),
            target = target,
            targetGroup = group,
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
