using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace RendleLabs.LegacyWorkspaceLoader
{
    public interface IWorkspaceLoader
    {
        void SetFrameworkDirectory(string frameworkDirectory);
        Task<AdhocWorkspace> LoadAsync(string solutionPath);
    }
}