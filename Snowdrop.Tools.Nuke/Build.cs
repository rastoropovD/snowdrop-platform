using System;
using System.IO;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.IO.CompressionTasks;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
public sealed partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Publish);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")] readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
                         .Before(Restore)
                         .Executes(() =>
                         {
                             EnsureCleanDirectory(ArtifactsDirectory);
                         });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Project Api => Solution.GetProject("Snowdrop.Api");

    Target Publish => _ => _
                           .DependsOn(Restore, Clean, BlTests, DalTests)
                           .Executes(() =>
                           {
                               DotNetPublish(s => s
                                                  .SetProject(Api)
                                                  .SetConfiguration(Configuration)
                                                  .SetOutput(ArtifactsDirectory / Api.Name));
                           });

    Target Archive => _ => _
                           .DependsOn(Publish)
                           .Executes(() =>
                           {
                               CompressZip(
                                   ArtifactsDirectory / Api.Name,
                                   ArtifactsDirectory / $"{Api.Name}.zip",
                                   fileMode: FileMode.Create
                               );
                           });

    [PathExecutable("az")] readonly Tool Az = default;
    Action<OutputType, string> AzureLogger => (type, s) => Logger.Info(s);
    const string WebAppName = "snowdrop-linux";
    const string ResourceGroup = "snowdrop";
    const string Subscription = "2642dbb4-fbde-45f9-a712-55f749706d40";

    Target Deploy => _ => _
                          .DependsOn(Archive)
                          .Executes(() =>
                          {
                              Logger.Info("Test");
                              Az($"webapp stop --name {WebAppName} --resource-group {ResourceGroup} --subscription {Subscription}", customLogger: AzureLogger);

                              Az($"webapp deployment source config-zip --name {WebAppName} --resource-group {ResourceGroup} --subscription {Subscription} --src {ArtifactsDirectory / $"{Api.Name}.zip"}", customLogger: AzureLogger);

                              Az($"webapp start --name {WebAppName} --resource-group {ResourceGroup} --subscription {Subscription}", customLogger: AzureLogger);
                          });
}