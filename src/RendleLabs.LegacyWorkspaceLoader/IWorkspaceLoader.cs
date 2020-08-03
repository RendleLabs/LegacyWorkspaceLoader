using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace RendleLabs.AdhocWorkspaceLoader
{
    public interface IWorkspaceLoader
    {
        void SetFrameworkDirectory(string frameworkDirectory);
        Task<AdhocWorkspace> LoadAsync(string solutionPath);
    }
}