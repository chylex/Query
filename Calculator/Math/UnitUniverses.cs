using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Calculator.Math;

sealed class UnitUniverses {
	private readonly FrozenDictionary<Unit, UnitUniverse> unitToUniverse;
	
	public WordLookupTrieNode UnitLookupByWords { get; }

	internal UnitUniverses(params UnitUniverse[] universes) {
		Dictionary<Unit, UnitUniverse> unitToUniverseBuilder = new (ReferenceEqualityComparer.Instance);
		WordLookupTrieNode.Builder unitLookupByWordsBuilder = new ();
		
		foreach (UnitUniverse universe in universes) {
			foreach (Unit unit in universe.AllUnits) {
				unitToUniverseBuilder.Add(unit, universe);
				unitLookupByWordsBuilder.Add(unit);
			}
		}

		unitToUniverse = unitToUniverseBuilder.ToFrozenDictionary(ReferenceEqualityComparer.Instance);
		UnitLookupByWords = unitLookupByWordsBuilder.Build();
	}
	
	public bool TryGetUniverse(Unit unit, [NotNullWhen(true)] out UnitUniverse? universe) {
		return unitToUniverse.TryGetValue(unit, out universe);
	}
	
	public sealed record WordLookupTrieNode(Unit? Unit, FrozenDictionary<string, WordLookupTrieNode> Children) {
		internal sealed class Builder {
			private sealed record Node(Unit? Unit, Dictionary<string, Node> Children) {
				internal static Node Create(Unit? unit = null) {
					return new Node(unit, new Dictionary<string, Node>());
				}
				
				internal Node Child(string word) {
					if (!Children.TryGetValue(word, out Node? child)) {
						Children.Add(word, child = Create());
					}
					
					return child;
				}
			}
			
			private readonly Node root = Node.Create();
			
			public void Add(Unit unit) {
				Add(unit.ShortName, unit);
				
				foreach (string longName in unit.LongNames) {
					Add(longName, unit);
				}
			}
			
			private void Add(string name, Unit unit) {
				Node node = root;
				string[] words = name.Split(' ');

				foreach (string word in words.AsSpan(..^1)) {
					node = node.Child(word);
				}
				
				node.Children.Add(words[^1], Node.Create(unit));
			}

			public WordLookupTrieNode Build() {
				return Build(root);
			}
			
			private WordLookupTrieNode Build(Node node) {
				return new WordLookupTrieNode(node.Unit, node.Children.ToFrozenDictionary(static kvp => kvp.Key, kvp => Build(kvp.Value)));
			}
		}
	}
}
