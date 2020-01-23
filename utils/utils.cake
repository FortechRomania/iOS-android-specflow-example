using System.Diagnostics;
using System.Xml;
using System.Net;
using System.Net.Sockets;

public void StartProcessWithException(string executablePath, string arguments, string standardInput)
{
    var args = new ProcessArgumentBuilder().Append(arguments);
    using (var process = new Process                              
    {
             StartInfo = new ProcessStartInfo(executablePath, args.Render())
                {
                    RedirectStandardInput = true,
                    UseShellExecute = false
                }
    })
    {
        process.Start();
        process.StandardInput.WriteLine(standardInput);
        process.StandardInput.Close();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new CakeException($"Process {executablePath} {arguments} << {standardInput} failed with code {process.ExitCode}");
        }
    }
}

public void StartProcessWithException(FilePath fileName, string processArguments)
{
    var exitCode = StartProcess(fileName, processArguments);

    if (exitCode != 0)
    {
        throw new CakeException($"Process {fileName} {processArguments} failed with code {exitCode}");
    }
}

public int StartProcessWithOutput(FilePath fileName, string processArguments, out IEnumerable<string> redirectedStandardOutput)
{
    var args = new ProcessArgumentBuilder().Append(processArguments);
    var processSettings = new ProcessSettings { Arguments = args, RedirectStandardOutput = true};
    return StartProcess(fileName, processSettings, out redirectedStandardOutput);
}

public void ClearProjectDependenciesForProject(string project)
{
    try 
    {
        XmlPoke(project, "//*[local-name()='ProjectReference']", null);
    } 
    catch (Exception e)
    {
        Information(e.Message);
    }
}

public void ZipDirectory(string sourceDirectoryPath, string sourceDirectoryName, string zipFilePath)
{
    var tempDirectory = Directory($"temp{new Random().Next()}");
    CreateDirectory(tempDirectory);
    MoveDirectory(sourceDirectoryPath, $"{tempDirectory.Path}/{sourceDirectoryName}");
    CreateDirectoryIfNeeded(new FileInfo(zipFilePath).Directory.FullName);
    Zip(tempDirectory, zipFilePath);
    DeleteDirectory(tempDirectory, new DeleteDirectorySettings { Recursive = true, Force = true });
}

public void ForceMoveFile(string source, string destination)
{
    if (FileExists(destination))
    {
        DeleteFile(destination);
    }

    CreateDirectoryIfNeeded(new FileInfo(destination).Directory.FullName);

    MoveFile(source, destination);
}

public void CreateDirectoryIfNeeded(string destination)
{
    if (!DirectoryExists(destination))
    {
        CreateDirectory(destination);
    }
}

public string ParseProjectName(string relativeProjectPath)
{
    return relativeProjectPath.Split('/').Last().Replace(".csproj", "");
}

public int GetAvailablePort()
{
    var listener = new TcpListener(IPAddress.Loopback, 0);
    listener.Start();
    int port = ((IPEndPoint)listener.LocalEndpoint).Port;
    listener.Stop();
  
    return port;
}

public void SetBuildNumberIfNeeded(MSBuildSettings buildSettings, string buildNumber)
{
    if (!string.IsNullOrEmpty(buildNumber))
    {
        buildSettings.WithProperty("SetVersion", "true")
                     .WithProperty("BuildNumber", buildNumber);
    }
}

public string ParseReleaseVersion(string projectPath)
{
    var xmlDoc = new XmlDocument();
    xmlDoc.Load(projectPath);

    return xmlDoc.GetElementsByTagName("ReleaseVersion")[0].InnerText;
}