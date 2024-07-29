using System.Collections.Generic;

namespace Query.Core {
	sealed class CommandHistory {
		private readonly List<string> queries = new List<string>();
		private readonly List<string> results = new List<string>();

		public IList<string> Queries => queries;

		public IList<string> Results => results;

		public void AddQuery(string text) {
			queries.Add(text);
		}

		public void AddResult(string text) {
			results.Add(text);
		}

		public void Clear() {
			queries.Clear();
			results.Clear();
		}
	}
}
