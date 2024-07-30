namespace Base;

public interface IApp {
	string[] RecognizedNames { get; }

	MatchConfidence GetConfidence(Command cmd);
	string? ProcessCommand(Command cmd);
}
