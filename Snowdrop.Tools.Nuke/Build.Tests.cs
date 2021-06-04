
    using Nuke.Common;
    using Nuke.Common.ProjectModel;
    using Nuke.Common.Tooling;
    using Nuke.Common.Tools.DotNet;
    using static Nuke.Common.Tools.DotNet.DotNetTasks;

    public sealed partial class Build
    {
        Project BlTestsProject => Solution.GetProject("Snowdrop.BL.Tests.Unit");
        Project DalTestsProject => Solution.GetProject("Snowdrop.DAL.Tests");

        [PathExecutable("dotnet")] readonly Tool DotNet = default;
        
        Target BlTests => _ => _
                               .After(Restore)
                               .Executes(() =>
                               {
                                   DotNetTest(settings => settings
                                                          .SetProjectFile(BlTestsProject)
                                                          .SetConfiguration(Configuration));
                               });

        Target DalTests => _ => _
                                .After(Restore)
                                .Executes(() =>
                                {
                                    DotNetTest(settings => settings
                                                           .SetProjectFile(DalTestsProject)
                                                           .SetConfiguration(Configuration));
                                    // DotNet($"test {DalTestsProject.Name}");
                                });
    }