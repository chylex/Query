using System.Diagnostics.CodeAnalysis;

namespace Query;

interface IApp {
	bool TryRun(string command, [NotNullWhen(true)] out string? output);
}
