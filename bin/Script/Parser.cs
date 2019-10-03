using System.Collections;
using System.Globalization;

namespace System.Text.RegularExpressions.Syntax
{
	internal class Parser
	{
		private string pattern;

		private int ptr;

		private ArrayList caps;

		private Hashtable refs;

		private int num_groups;

		private int gap;

		public Parser()
		{
			caps = new ArrayList();
			refs = new Hashtable();
		}

		public static int ParseDecimal(string str, ref int ptr)
		{
			return ParseNumber(str, ref ptr, 10, 1, int.MaxValue);
		}

		public static int ParseOctal(string str, ref int ptr)
		{
			return ParseNumber(str, ref ptr, 8, 1, 3);
		}

		public static int ParseHex(string str, ref int ptr, int digits)
		{
			return ParseNumber(str, ref ptr, 16, digits, digits);
		}

		public static int ParseNumber(string str, ref int ptr, int b, int min, int max)
		{
			int num = ptr;
			int num2 = 0;
			int i = 0;
			if (max < min)
			{
				max = int.MaxValue;
			}
			for (; i < max; i++)
			{
				if (num >= str.Length)
				{
					break;
				}
				int num4 = ParseDigit(str[num++], b, i);
				if (num4 < 0)
				{
					num--;
					break;
				}
				num2 = num2 * b + num4;
			}
			if (i < min)
			{
				return -1;
			}
			ptr = num;
			return num2;
		}

		public static string ParseName(string str, ref int ptr)
		{
			if (char.IsDigit(str[ptr]))
			{
				int num = ParseNumber(str, ref ptr, 10, 1, 0);
				if (num > 0)
				{
					return num.ToString();
				}
				return null;
			}
			int num2 = ptr;
			while (IsNameChar(str[ptr]))
			{
				ptr++;
			}
			if (ptr - num2 > 0)
			{
				return str.Substring(num2, ptr - num2);
			}
			return null;
		}

		public static string Escape(string str)
		{
			string text = string.Empty;
			foreach (char c in str)
			{
				switch (c)
				{
				case ' ':
				case '#':
				case '$':
				case '(':
				case ')':
				case '*':
				case '+':
				case '.':
				case '?':
				case '[':
				case '\\':
				case '^':
				case '{':
				case '|':
					text = text + "\\" + c;
					break;
				case '\t':
					text += "\\t";
					break;
				case '\n':
					text += "\\n";
					break;
				case '\r':
					text += "\\r";
					break;
				case '\f':
					text += "\\f";
					break;
				default:
					text += c;
					break;
				}
			}
			return text;
		}

		public static string Unescape(string str)
		{
			if (str.IndexOf('\\') == -1)
			{
				return str;
			}
			return new Parser().ParseString(str);
		}

		public RegularExpression ParseRegularExpression(string pattern, RegexOptions options)
		{
			this.pattern = pattern;
			ptr = 0;
			caps.Clear();
			refs.Clear();
			num_groups = 0;
			try
			{
				RegularExpression regularExpression = new RegularExpression();
				ParseGroup(regularExpression, options, null);
				ResolveReferences();
				regularExpression.GroupCount = num_groups;
				return regularExpression;
				IL_0053:
				RegularExpression result;
				return result;
			}
			catch (IndexOutOfRangeException)
			{
				throw NewParseException("Unexpected end of pattern.");
				IL_0065:
				RegularExpression result;
				return result;
			}
		}

		public int GetMapping(Hashtable mapping)
		{
			int count = caps.Count;
			mapping.Add("0", 0);
			for (int i = 0; i < count; i++)
			{
				CapturingGroup capturingGroup = (CapturingGroup)caps[i];
				string key = (capturingGroup.Name == null) ? capturingGroup.Index.ToString() : capturingGroup.Name;
				if (mapping.Contains(key))
				{
					if ((int)mapping[key] != capturingGroup.Index)
					{
						throw new SystemException("invalid state");
					}
				}
				else
				{
					mapping.Add(key, capturingGroup.Index);
				}
			}
			return gap;
		}

		private void ParseGroup(Group group, RegexOptions options, Assertion assertion)
		{
			bool flag = group is RegularExpression;
			Alternation alternation = null;
			string text = null;
			Group group2 = new Group();
			Expression expression = null;
			bool flag2 = false;
			while (true)
			{
				ConsumeWhitespace(IsIgnorePatternWhitespace(options));
				if (ptr >= pattern.Length)
				{
					break;
				}
				char c = pattern[ptr++];
				switch (c)
				{
				case '^':
				{
					Position pos = (!IsMultiline(options)) ? Position.Start : Position.StartOfLine;
					expression = new PositionAssertion(pos);
					goto default;
				}
				case '$':
				{
					Position pos2 = (!IsMultiline(options)) ? Position.End : Position.EndOfLine;
					expression = new PositionAssertion(pos2);
					goto default;
				}
				case '.':
				{
					Category cat = (!IsSingleline(options)) ? Category.Any : Category.AnySingleline;
					expression = new CharacterClass(cat, negate: false);
					goto default;
				}
				case '\\':
				{
					int num = ParseEscape();
					if (num >= 0)
					{
						c = (char)num;
					}
					else
					{
						expression = ParseSpecial(options);
						if (expression == null)
						{
							c = pattern[ptr++];
						}
					}
					goto default;
				}
				case '[':
					expression = ParseCharacterClass(options);
					goto default;
				case '(':
				{
					bool flag4 = IsIgnoreCase(options);
					expression = ParseGroupingConstruct(ref options);
					if (expression == null)
					{
						if (text != null && IsIgnoreCase(options) != flag4)
						{
							group2.AppendExpression(new Literal(text, IsIgnoreCase(options)));
							text = null;
						}
						continue;
					}
					goto default;
				}
				case ')':
					flag2 = true;
					break;
				case '|':
					if (text != null)
					{
						group2.AppendExpression(new Literal(text, IsIgnoreCase(options)));
						text = null;
					}
					if (assertion != null)
					{
						if (assertion.TrueExpression == null)
						{
							assertion.TrueExpression = group2;
						}
						else
						{
							if (assertion.FalseExpression != null)
							{
								throw NewParseException("Too many | in (?()|).");
							}
							assertion.FalseExpression = group2;
						}
					}
					else
					{
						if (alternation == null)
						{
							alternation = new Alternation();
						}
						alternation.AddAlternative(group2);
					}
					group2 = new Group();
					continue;
				case '*':
				case '+':
				case '?':
					throw NewParseException("Bad quantifier.");
				default:
					ConsumeWhitespace(IsIgnorePatternWhitespace(options));
					if (ptr < pattern.Length)
					{
						char c2 = pattern[ptr];
						int min = 0;
						int max = 0;
						bool lazy = false;
						bool flag3 = false;
						switch (c2)
						{
						case '*':
						case '+':
						case '?':
							ptr++;
							flag3 = true;
							switch (c2)
							{
							case '?':
								min = 0;
								max = 1;
								break;
							case '*':
								min = 0;
								max = int.MaxValue;
								break;
							case '+':
								min = 1;
								max = int.MaxValue;
								break;
							}
							break;
						case '{':
							if (ptr + 1 < pattern.Length)
							{
								int num2 = ptr;
								ptr++;
								flag3 = ParseRepetitionBounds(out min, out max, options);
								if (!flag3)
								{
									ptr = num2;
								}
							}
							break;
						}
						if (flag3)
						{
							ConsumeWhitespace(IsIgnorePatternWhitespace(options));
							if (ptr < pattern.Length && pattern[ptr] == '?')
							{
								ptr++;
								lazy = true;
							}
							Repetition repetition = new Repetition(min, max, lazy);
							if (expression == null)
							{
								repetition.Expression = new Literal(c.ToString(), IsIgnoreCase(options));
							}
							else
							{
								repetition.Expression = expression;
							}
							expression = repetition;
						}
					}
					if (expression == null)
					{
						if (text == null)
						{
							text = string.Empty;
						}
						text += c;
					}
					else
					{
						if (text != null)
						{
							group2.AppendExpression(new Literal(text, IsIgnoreCase(options)));
							text = null;
						}
						group2.AppendExpression(expression);
						expression = null;
					}
					if (flag && ptr >= pattern.Length)
					{
						break;
					}
					continue;
				}
				break;
			}
			if (flag && flag2)
			{
				throw NewParseException("Too many )'s.");
			}
			if (!flag && !flag2)
			{
				throw NewParseException("Not enough )'s.");
			}
			if (text != null)
			{
				group2.AppendExpression(new Literal(text, IsIgnoreCase(options)));
			}
			if (assertion != null)
			{
				if (assertion.TrueExpression == null)
				{
					assertion.TrueExpression = group2;
				}
				else
				{
					assertion.FalseExpression = group2;
				}
				group.AppendExpression(assertion);
			}
			else if (alternation != null)
			{
				alternation.AddAlternative(group2);
				group.AppendExpression(alternation);
			}
			else
			{
				group.AppendExpression(group2);
			}
		}

		private Expression ParseGroupingConstruct(ref RegexOptions options)
		{
			if (pattern[ptr] != '?')
			{
				Group group;
				if (IsExplicitCapture(options))
				{
					group = new Group();
				}
				else
				{
					group = new CapturingGroup();
					caps.Add(group);
				}
				ParseGroup(group, options, null);
				return group;
			}
			ptr++;
			switch (pattern[ptr])
			{
			case ':':
			{
				ptr++;
				Group group6 = new Group();
				ParseGroup(group6, options, null);
				return group6;
			}
			case '>':
			{
				ptr++;
				Group group2 = new NonBacktrackingGroup();
				ParseGroup(group2, options, null);
				return group2;
			}
			case '-':
			case 'i':
			case 'm':
			case 'n':
			case 's':
			case 'x':
			{
				RegexOptions options2 = options;
				ParseOptions(ref options2, negate: false);
				if (pattern[ptr] == '-')
				{
					ptr++;
					ParseOptions(ref options2, negate: true);
				}
				if (pattern[ptr] == ':')
				{
					ptr++;
					Group group3 = new Group();
					ParseGroup(group3, options2, null);
					return group3;
				}
				if (pattern[ptr] == ')')
				{
					ptr++;
					options = options2;
					return null;
				}
				throw NewParseException("Bad options");
			}
			case '!':
			case '<':
			case '=':
			{
				ExpressionAssertion expressionAssertion2 = new ExpressionAssertion();
				if (!ParseAssertionType(expressionAssertion2))
				{
					goto case '\'';
				}
				Group group7 = new Group();
				ParseGroup(group7, options, null);
				expressionAssertion2.TestExpression = group7;
				return expressionAssertion2;
			}
			case '\'':
			{
				char c = (pattern[ptr] != '<') ? '\'' : '>';
				ptr++;
				string text2 = ParseName();
				if (pattern[ptr] == c)
				{
					if (text2 == null)
					{
						throw NewParseException("Bad group name.");
					}
					ptr++;
					CapturingGroup capturingGroup = new CapturingGroup();
					capturingGroup.Name = text2;
					caps.Add(capturingGroup);
					ParseGroup(capturingGroup, options, null);
					return capturingGroup;
				}
				if (pattern[ptr] == '-')
				{
					ptr++;
					string text3 = ParseName();
					if (text3 == null || pattern[ptr] != c)
					{
						throw NewParseException("Bad balancing group name.");
					}
					ptr++;
					BalancingGroup balancingGroup = new BalancingGroup();
					balancingGroup.Name = text2;
					if (balancingGroup.IsNamed)
					{
						caps.Add(balancingGroup);
					}
					refs.Add(balancingGroup, text3);
					ParseGroup(balancingGroup, options, null);
					return balancingGroup;
				}
				throw NewParseException("Bad group name.");
			}
			case '(':
			{
				ptr++;
				int num = ptr;
				string text = ParseName();
				Assertion assertion;
				if (text == null || pattern[ptr] != ')')
				{
					ptr = num;
					ExpressionAssertion expressionAssertion = new ExpressionAssertion();
					if (pattern[ptr] == '?')
					{
						ptr++;
						if (!ParseAssertionType(expressionAssertion))
						{
							throw NewParseException("Bad conditional.");
						}
					}
					else
					{
						expressionAssertion.Negate = false;
						expressionAssertion.Reverse = false;
					}
					Group group4 = new Group();
					ParseGroup(group4, options, null);
					expressionAssertion.TestExpression = group4;
					assertion = expressionAssertion;
				}
				else
				{
					ptr++;
					assertion = new CaptureAssertion(new Literal(text, IsIgnoreCase(options)));
					refs.Add(assertion, text);
				}
				Group group5 = new Group();
				ParseGroup(group5, options, assertion);
				return group5;
			}
			case '#':
				ptr++;
				while (pattern[ptr++] != ')')
				{
					if (ptr >= pattern.Length)
					{
						throw NewParseException("Unterminated (?#...) comment.");
					}
				}
				return null;
			default:
				throw NewParseException("Bad grouping construct.");
			}
		}

		private bool ParseAssertionType(ExpressionAssertion assertion)
		{
			if (pattern[ptr] == '<')
			{
				switch (pattern[ptr + 1])
				{
				case '=':
					assertion.Negate = false;
					break;
				case '!':
					assertion.Negate = true;
					break;
				default:
					return false;
				}
				assertion.Reverse = true;
				ptr += 2;
			}
			else
			{
				switch (pattern[ptr])
				{
				case '=':
					assertion.Negate = false;
					break;
				case '!':
					assertion.Negate = true;
					break;
				default:
					return false;
				}
				assertion.Reverse = false;
				ptr++;
			}
			return true;
		}

		private void ParseOptions(ref RegexOptions options, bool negate)
		{
			while (true)
			{
				switch (pattern[ptr])
				{
				default:
					return;
				case 'i':
					if (negate)
					{
						options &= ~RegexOptions.IgnoreCase;
					}
					else
					{
						options |= RegexOptions.IgnoreCase;
					}
					break;
				case 'm':
					if (negate)
					{
						options &= ~RegexOptions.Multiline;
					}
					else
					{
						options |= RegexOptions.Multiline;
					}
					break;
				case 'n':
					if (negate)
					{
						options &= ~RegexOptions.ExplicitCapture;
					}
					else
					{
						options |= RegexOptions.ExplicitCapture;
					}
					break;
				case 's':
					if (negate)
					{
						options &= ~RegexOptions.Singleline;
					}
					else
					{
						options |= RegexOptions.Singleline;
					}
					break;
				case 'x':
					if (negate)
					{
						options &= ~RegexOptions.IgnorePatternWhitespace;
					}
					else
					{
						options |= RegexOptions.IgnorePatternWhitespace;
					}
					break;
				}
				ptr++;
			}
		}

		private Expression ParseCharacterClass(RegexOptions options)
		{
			bool negate = false;
			if (pattern[ptr] == '^')
			{
				negate = true;
				ptr++;
			}
			bool flag = IsECMAScript(options);
			CharacterClass characterClass = new CharacterClass(negate, IsIgnoreCase(options));
			if (pattern[ptr] == ']')
			{
				characterClass.AddCharacter(']');
				ptr++;
			}
			int num = -1;
			int num2 = -1;
			bool flag2 = false;
			bool flag3 = false;
			while (ptr < pattern.Length)
			{
				num = pattern[ptr++];
				if (num == 93)
				{
					flag3 = true;
					break;
				}
				if (num == 45 && num2 >= 0 && !flag2)
				{
					flag2 = true;
					continue;
				}
				if (num == 92)
				{
					num = ParseEscape();
					if (num < 0)
					{
						num = pattern[ptr++];
						switch (num)
						{
						case 98:
							num = 8;
							break;
						case 68:
						case 100:
							characterClass.AddCategory((!flag) ? Category.Digit : Category.EcmaDigit, num == 68);
							goto IL_01ec;
						case 87:
						case 119:
							characterClass.AddCategory((!flag) ? Category.Word : Category.EcmaWord, num == 87);
							goto IL_01ec;
						case 83:
						case 115:
							characterClass.AddCategory((!flag) ? Category.WhiteSpace : Category.EcmaWhiteSpace, num == 83);
							goto IL_01ec;
						case 80:
						case 112:
							{
								characterClass.AddCategory(ParseUnicodeCategory(), num == 80);
								goto IL_01ec;
							}
							IL_01ec:
							if (flag2)
							{
								throw NewParseException("character range cannot have category \\" + num);
							}
							num2 = -1;
							continue;
						}
					}
				}
				if (flag2)
				{
					if (num < num2)
					{
						throw NewParseException("[" + num2 + "-" + num + "] range in reverse order.");
					}
					characterClass.AddRange((char)num2, (char)num);
					num2 = -1;
					flag2 = false;
				}
				else
				{
					characterClass.AddCharacter((char)num);
					num2 = num;
				}
			}
			if (!flag3)
			{
				throw NewParseException("Unterminated [] set.");
			}
			if (flag2)
			{
				characterClass.AddCharacter('-');
			}
			return characterClass;
		}

		private bool ParseRepetitionBounds(out int min, out int max, RegexOptions options)
		{
			min = (max = 0);
			ConsumeWhitespace(IsIgnorePatternWhitespace(options));
			int num;
			if (pattern[ptr] == ',')
			{
				num = -1;
			}
			else
			{
				num = ParseNumber(10, 1, 0);
				ConsumeWhitespace(IsIgnorePatternWhitespace(options));
			}
			int num2;
			switch (pattern[ptr++])
			{
			case '}':
				num2 = num;
				break;
			case ',':
				ConsumeWhitespace(IsIgnorePatternWhitespace(options));
				num2 = ParseNumber(10, 1, 0);
				ConsumeWhitespace(IsIgnorePatternWhitespace(options));
				if (pattern[ptr++] != '}')
				{
					return false;
				}
				break;
			default:
				return false;
			}
			if (num > int.MaxValue || num2 > int.MaxValue)
			{
				throw NewParseException("Illegal {x, y} - maximum of 2147483647.");
			}
			if (num2 >= 0 && num2 < num)
			{
				throw NewParseException("Illegal {x, y} with x > y.");
			}
			min = num;
			if (num2 > 0)
			{
				max = num2;
			}
			else
			{
				max = int.MaxValue;
			}
			return true;
		}

		private Category ParseUnicodeCategory()
		{
			if (pattern[ptr++] != '{')
			{
				throw NewParseException("Incomplete \\p{X} character escape.");
			}
			string text = ParseName(pattern, ref ptr);
			if (text == null)
			{
				throw NewParseException("Incomplete \\p{X} character escape.");
			}
			Category category = CategoryUtils.CategoryFromName(text);
			if (category == Category.None)
			{
				throw NewParseException("Unknown property '" + text + "'.");
			}
			if (pattern[ptr++] != '}')
			{
				throw NewParseException("Incomplete \\p{X} character escape.");
			}
			return category;
		}

		private Expression ParseSpecial(RegexOptions options)
		{
			int num = ptr;
			bool flag = IsECMAScript(options);
			Expression expression = null;
			switch (pattern[ptr++])
			{
			case 'd':
				expression = new CharacterClass((!flag) ? Category.Digit : Category.EcmaDigit, negate: false);
				break;
			case 'w':
				expression = new CharacterClass((!flag) ? Category.Word : Category.EcmaWord, negate: false);
				break;
			case 's':
				expression = new CharacterClass((!flag) ? Category.WhiteSpace : Category.EcmaWhiteSpace, negate: false);
				break;
			case 'p':
				expression = new CharacterClass(ParseUnicodeCategory(), negate: false);
				break;
			case 'D':
				expression = new CharacterClass((!flag) ? Category.Digit : Category.EcmaDigit, negate: true);
				break;
			case 'W':
				expression = new CharacterClass((!flag) ? Category.Word : Category.EcmaWord, negate: true);
				break;
			case 'S':
				expression = new CharacterClass((!flag) ? Category.WhiteSpace : Category.EcmaWhiteSpace, negate: true);
				break;
			case 'P':
				expression = new CharacterClass(ParseUnicodeCategory(), negate: true);
				break;
			case 'A':
				expression = new PositionAssertion(Position.StartOfString);
				break;
			case 'Z':
				expression = new PositionAssertion(Position.End);
				break;
			case 'z':
				expression = new PositionAssertion(Position.EndOfString);
				break;
			case 'G':
				expression = new PositionAssertion(Position.StartOfScan);
				break;
			case 'b':
				expression = new PositionAssertion(Position.Boundary);
				break;
			case 'B':
				expression = new PositionAssertion(Position.NonBoundary);
				break;
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
			{
				ptr--;
				int num2 = ParseNumber(10, 1, 0);
				if (num2 < 0)
				{
					ptr = num;
					return null;
				}
				Reference reference2 = new BackslashNumber(IsIgnoreCase(options), flag);
				refs.Add(reference2, num2.ToString());
				expression = reference2;
				break;
			}
			case 'k':
			{
				char c = pattern[ptr++];
				switch (c)
				{
				case '<':
					c = '>';
					break;
				default:
					throw NewParseException("Malformed \\k<...> named backreference.");
				case '\'':
					break;
				}
				string text = ParseName();
				if (text == null || pattern[ptr] != c)
				{
					throw NewParseException("Malformed \\k<...> named backreference.");
				}
				ptr++;
				Reference reference = new Reference(IsIgnoreCase(options));
				refs.Add(reference, text);
				expression = reference;
				break;
			}
			default:
				expression = null;
				break;
			}
			if (expression == null)
			{
				ptr = num;
			}
			return expression;
		}

		private int ParseEscape()
		{
			int num = ptr;
			if (num >= pattern.Length)
			{
				throw new ArgumentException($"Parsing \"{pattern}\" - Illegal \\ at end of pattern.", pattern);
			}
			switch (pattern[ptr++])
			{
			case 'a':
				return 7;
			case 't':
				return 9;
			case 'r':
				return 13;
			case 'v':
				return 11;
			case 'f':
				return 12;
			case 'n':
				return 10;
			case 'e':
				return 27;
			case '\\':
				return 92;
			case '0':
			{
				ptr--;
				int num3 = ptr;
				int num4 = ParseOctal(pattern, ref ptr);
				if (num4 == -1 && num3 == ptr)
				{
					return 0;
				}
				return num4;
			}
			case 'x':
			{
				int num2 = ParseHex(pattern, ref ptr, 2);
				if (num2 < 0)
				{
					throw NewParseException("Insufficient hex digits");
				}
				return num2;
			}
			case 'u':
			{
				int num2 = ParseHex(pattern, ref ptr, 4);
				if (num2 < 0)
				{
					throw NewParseException("Insufficient hex digits");
				}
				return num2;
			}
			case 'c':
			{
				int num2 = pattern[ptr++];
				if (num2 >= 64 && num2 <= 95)
				{
					return num2 - 64;
				}
				throw NewParseException("Unrecognized control character.");
			}
			default:
				ptr = num;
				return -1;
			}
		}

		private string ParseName()
		{
			return ParseName(pattern, ref ptr);
		}

		private static bool IsNameChar(char c)
		{
			switch (char.GetUnicodeCategory(c))
			{
			case UnicodeCategory.ModifierLetter:
				return false;
			case UnicodeCategory.ConnectorPunctuation:
				return true;
			default:
				return char.IsLetterOrDigit(c);
			}
		}

		private int ParseNumber(int b, int min, int max)
		{
			return ParseNumber(pattern, ref ptr, b, min, max);
		}

		private static int ParseDigit(char c, int b, int n)
		{
			switch (b)
			{
			case 8:
				if (c >= '0' && c <= '7')
				{
					return c - 48;
				}
				return -1;
			case 10:
				if (c >= '0' && c <= '9')
				{
					return c - 48;
				}
				return -1;
			case 16:
				if (c >= '0' && c <= '9')
				{
					return c - 48;
				}
				if (c >= 'a' && c <= 'f')
				{
					return 10 + c - 97;
				}
				if (c >= 'A' && c <= 'F')
				{
					return 10 + c - 65;
				}
				return -1;
			default:
				return -1;
			}
		}

		private void ConsumeWhitespace(bool ignore)
		{
			while (ptr < pattern.Length)
			{
				if (pattern[ptr] == '(')
				{
					if (ptr + 3 >= pattern.Length || pattern[ptr + 1] != '?' || pattern[ptr + 2] != '#')
					{
						break;
					}
					ptr += 3;
					while (ptr < pattern.Length && pattern[ptr++] != ')')
					{
					}
				}
				else if (ignore && pattern[ptr] == '#')
				{
					while (ptr < pattern.Length && pattern[ptr++] != '\n')
					{
					}
				}
				else
				{
					if (!ignore || !char.IsWhiteSpace(pattern[ptr]))
					{
						break;
					}
					while (ptr < pattern.Length && char.IsWhiteSpace(pattern[ptr]))
					{
						ptr++;
					}
				}
			}
		}

		private string ParseString(string pattern)
		{
			this.pattern = pattern;
			ptr = 0;
			StringBuilder stringBuilder = new StringBuilder(pattern.Length);
			while (ptr < pattern.Length)
			{
				int num = pattern[ptr++];
				if (num == 92)
				{
					num = ParseEscape();
					if (num < 0)
					{
						num = pattern[ptr++];
						if (num == 98)
						{
							num = 8;
						}
					}
				}
				stringBuilder.Append((char)num);
			}
			return stringBuilder.ToString();
		}

		private void ResolveReferences()
		{
			int num = 1;
			Hashtable hashtable = new Hashtable();
			ArrayList arrayList = null;
			foreach (CapturingGroup cap in caps)
			{
				if (cap.Name == null)
				{
					hashtable.Add(num.ToString(), cap);
					cap.Index = num++;
					num_groups++;
				}
			}
			foreach (CapturingGroup cap2 in caps)
			{
				if (cap2.Name != null)
				{
					if (hashtable.Contains(cap2.Name))
					{
						CapturingGroup capturingGroup3 = (CapturingGroup)hashtable[cap2.Name];
						cap2.Index = capturingGroup3.Index;
						if (cap2.Index == num)
						{
							num++;
						}
						else if (cap2.Index > num)
						{
							arrayList.Add(cap2);
						}
					}
					else
					{
						if (char.IsDigit(cap2.Name[0]))
						{
							int num3 = 0;
							int num4 = ParseDecimal(cap2.Name, ref num3);
							if (num3 == cap2.Name.Length)
							{
								cap2.Index = num4;
								hashtable.Add(cap2.Name, cap2);
								num_groups++;
								if (num4 == num)
								{
									num++;
								}
								else
								{
									if (arrayList == null)
									{
										arrayList = new ArrayList(4);
									}
									arrayList.Add(cap2);
								}
								continue;
							}
						}
						string key = num.ToString();
						while (hashtable.Contains(key))
						{
							int num5 = ++num;
							key = num5.ToString();
						}
						hashtable.Add(key, cap2);
						hashtable.Add(cap2.Name, cap2);
						cap2.Index = num++;
						num_groups++;
					}
				}
			}
			gap = num;
			if (arrayList != null)
			{
				HandleExplicitNumericGroups(arrayList);
			}
			foreach (Expression key2 in refs.Keys)
			{
				string text = (string)refs[key2];
				if (!hashtable.Contains(text))
				{
					if (!(key2 is CaptureAssertion) || char.IsDigit(text[0]))
					{
						BackslashNumber backslashNumber = key2 as BackslashNumber;
						if (backslashNumber == null || !backslashNumber.ResolveReference(text, hashtable))
						{
							throw NewParseException("Reference to undefined group " + ((!char.IsDigit(text[0])) ? "name " : "number ") + text);
						}
					}
				}
				else
				{
					CapturingGroup capturingGroup4 = (CapturingGroup)hashtable[text];
					if (key2 is Reference)
					{
						((Reference)key2).CapturingGroup = capturingGroup4;
					}
					else if (key2 is CaptureAssertion)
					{
						((CaptureAssertion)key2).CapturingGroup = capturingGroup4;
					}
					else if (key2 is BalancingGroup)
					{
						((BalancingGroup)key2).Balance = capturingGroup4;
					}
				}
			}
		}

		private void HandleExplicitNumericGroups(ArrayList explicit_numeric_groups)
		{
			int num = gap;
			int i = 0;
			int count = explicit_numeric_groups.Count;
			explicit_numeric_groups.Sort();
			for (; i < count; i++)
			{
				CapturingGroup capturingGroup = (CapturingGroup)explicit_numeric_groups[i];
				if (capturingGroup.Index > num)
				{
					break;
				}
				if (capturingGroup.Index == num)
				{
					num++;
				}
			}
			gap = num;
			int num2 = num;
			for (; i < count; i++)
			{
				CapturingGroup capturingGroup2 = (CapturingGroup)explicit_numeric_groups[i];
				if (capturingGroup2.Index == num2)
				{
					capturingGroup2.Index = num - 1;
					continue;
				}
				num2 = capturingGroup2.Index;
				capturingGroup2.Index = num++;
			}
		}

		private static bool IsIgnoreCase(RegexOptions options)
		{
			return (options & RegexOptions.IgnoreCase) != RegexOptions.None;
		}

		private static bool IsMultiline(RegexOptions options)
		{
			return (options & RegexOptions.Multiline) != RegexOptions.None;
		}

		private static bool IsExplicitCapture(RegexOptions options)
		{
			return (options & RegexOptions.ExplicitCapture) != RegexOptions.None;
		}

		private static bool IsSingleline(RegexOptions options)
		{
			return (options & RegexOptions.Singleline) != RegexOptions.None;
		}

		private static bool IsIgnorePatternWhitespace(RegexOptions options)
		{
			return (options & RegexOptions.IgnorePatternWhitespace) != RegexOptions.None;
		}

		private static bool IsECMAScript(RegexOptions options)
		{
			return (options & RegexOptions.ECMAScript) != RegexOptions.None;
		}

		private ArgumentException NewParseException(string msg)
		{
			msg = "parsing \"" + pattern + "\" - " + msg;
			return new ArgumentException(msg, pattern);
		}
	}
}
