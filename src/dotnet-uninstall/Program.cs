// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using Microsoft.DotNet.Tools.Uninstall.Shared.BundleInfo;
using Microsoft.DotNet.Tools.Uninstall.Shared.Commands;
using Microsoft.DotNet.Tools.Uninstall.Shared.Configs;
using Microsoft.DotNet.Tools.Uninstall.Shared.Filterers;
using Microsoft.DotNet.Tools.Uninstall.Windows;

namespace Microsoft.DotNet.Tools.Uninstall
{
    internal class Program
    {
        internal static int Main(string[] args)
        {
            //return CommandLineConfigs.UninstallRootCommand.InvokeAsync(args).Result;

            var filtered = new MajorMinorOptionFilterer().Filter<SdkVersion>("2.1", Bundle<SdkVersion>.FilterWithSameBundleType(RegistryQuery.GetInstalledBundles()));

            Console.WriteLine("...........");
            foreach (var f in filtered)
            {
                Console.WriteLine(f.UninstallCommand);
            }

            Console.WriteLine("...........");

            ProcessHandler.ExecuteUninstallCommand(filtered);
            //UninstallCommandExec.Execute();

            return 0;
        }
    }
}
