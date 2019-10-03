namespace System.Text.RegularExpressions
{
	internal class Interpreter : BaseMachine
	{
		private struct IntStack
		{
			private int[] values;

			private int count;

			public int Top => values[count - 1];

			public int Count
			{
				get
				{
					return count;
				}
				set
				{
					if (value > count)
					{
						throw new SystemException("can only truncate the stack");
					}
					count = value;
				}
			}

			public int Pop()
			{
				return values[--count];
			}

			public void Push(int value)
			{
				if (values == null)
				{
					values = new int[8];
				}
				else if (count == values.Length)
				{
					int num = values.Length;
					num += num >> 1;
					int[] array = new int[num];
					for (int i = 0; i < count; i++)
					{
						array[i] = values[i];
					}
					values = array;
				}
				values[count++] = value;
			}
		}

		private class RepeatContext
		{
			private int start;

			private int min;

			private int max;

			private bool lazy;

			private int expr_pc;

			private RepeatContext previous;

			private int count;

			public int Count
			{
				get
				{
					return count;
				}
				set
				{
					count = value;
				}
			}

			public int Start
			{
				get
				{
					return start;
				}
				set
				{
					start = value;
				}
			}

			public bool IsMinimum => min <= count;

			public bool IsMaximum => max <= count;

			public bool IsLazy => lazy;

			public int Expression => expr_pc;

			public RepeatContext Previous => previous;

			public RepeatContext(RepeatContext previous, int min, int max, bool lazy, int expr_pc)
			{
				this.previous = previous;
				this.min = min;
				this.max = max;
				this.lazy = lazy;
				this.expr_pc = expr_pc;
				start = -1;
				count = 0;
			}
		}

		private enum Mode
		{
			Search,
			Match,
			Count
		}

		private ushort[] program;

		private int program_start;

		private string text;

		private int text_end;

		private int group_count;

		private int match_min;

		private QuickSearch qs;

		private int scan_ptr;

		private RepeatContext repeat;

		private RepeatContext fast;

		private IntStack stack = default(IntStack);

		private RepeatContext deep;

		private Mark[] marks;

		private int mark_start;

		private int mark_end;

		private int[] groups;

		public Interpreter(ushort[] program)
		{
			this.program = program;
			qs = null;
			group_count = ReadProgramCount(1) + 1;
			match_min = ReadProgramCount(3);
			program_start = 7;
			groups = new int[group_count];
		}

		private int ReadProgramCount(int ptr)
		{
			int num = program[ptr + 1];
			num <<= 16;
			return num + program[ptr];
		}

		public override Match Scan(Regex regex, string text, int start, int end)
		{
			this.text = text;
			text_end = end;
			scan_ptr = start;
			if (Eval(Mode.Match, ref scan_ptr, program_start))
			{
				return GenerateMatch(regex);
			}
			return Match.Empty;
		}

		private void Reset()
		{
			ResetGroups();
			fast = (repeat = null);
		}

		private bool Eval(Mode mode, ref int ref_ptr, int pc)
		{
			int ref_ptr2 = ref_ptr;
			while (true)
			{
				ushort num = program[pc];
				OpCode opCode = (OpCode)(num & 0xFF);
				OpFlags opFlags = (OpFlags)(num & 0xFF00);
				int num13;
				int num12;
				bool flag;
				int num15;
				int num11;
				RepeatContext repeatContext;
				int start;
				int count;
				int count2;
				switch (opCode)
				{
				default:
					continue;
				case OpCode.Anchor:
				{
					num13 = program[pc + 1];
					num12 = program[pc + 2];
					flag = ((opFlags & OpFlags.RightToLeft) != OpFlags.None);
					num11 = ((!flag) ? (ref_ptr2 + num12) : (ref_ptr2 - num12));
					num15 = text_end - match_min + num12;
					int num16 = 0;
					OpCode opCode3 = (OpCode)(program[pc + 3] & 0xFF);
					if (opCode3 == OpCode.Position && num13 == 6)
					{
						switch (program[pc + 4])
						{
						case 2:
							break;
						case 3:
							goto IL_0165;
						case 4:
							goto IL_0234;
						default:
							goto end_IL_0028;
						}
						if (flag || num12 == 0)
						{
							if (flag)
							{
								ref_ptr2 = num12;
							}
							if (TryMatch(ref ref_ptr2, pc + num13))
							{
								goto case OpCode.True;
							}
							break;
						}
						break;
					}
					if (qs != null || (opCode3 == OpCode.String && num13 == 6 + program[pc + 4]))
					{
						bool flag4 = (ushort)(program[pc + 3] & 0x400) != 0;
						if (qs == null)
						{
							bool ignore = (ushort)(program[pc + 3] & 0x200) != 0;
							string @string = GetString(pc + 3);
							qs = new QuickSearch(@string, ignore, flag4);
						}
						while ((flag && num11 >= num16) || (!flag && num11 <= num15))
						{
							if (flag4)
							{
								num11 = qs.Search(text, num11, num16);
								if (num11 != -1)
								{
									num11 += qs.Length;
								}
							}
							else
							{
								num11 = qs.Search(text, num11, num15);
							}
							if (num11 < 0)
							{
								break;
							}
							ref_ptr2 = ((!flag4) ? (num11 - num12) : (num11 + num12));
							if (TryMatch(ref ref_ptr2, pc + num13))
							{
								goto case OpCode.True;
							}
							num11 = ((!flag4) ? (num11 + 1) : (num11 - 2));
						}
						break;
					}
					if (opCode3 == OpCode.True)
					{
						while ((flag && num11 >= num16) || (!flag && num11 <= num15))
						{
							ref_ptr2 = num11;
							if (TryMatch(ref ref_ptr2, pc + num13))
							{
								goto case OpCode.True;
							}
							num11 = ((!flag) ? (num11 + 1) : (num11 - 1));
						}
						break;
					}
					for (; (flag && num11 >= num16) || (!flag && num11 <= num15); num11 = ((!flag) ? (num11 + 1) : (num11 - 1)))
					{
						ref_ptr2 = num11;
						if (!Eval(Mode.Match, ref ref_ptr2, pc + 3))
						{
							continue;
						}
						ref_ptr2 = ((!flag) ? (num11 - num12) : (num11 + num12));
						if (!TryMatch(ref ref_ptr2, pc + num13))
						{
							continue;
						}
						goto case OpCode.True;
					}
					break;
				}
				case OpCode.Position:
					if (!IsPosition((Position)program[pc + 1], ref_ptr2))
					{
						break;
					}
					pc += 2;
					continue;
				case OpCode.String:
				{
					bool flag5 = (opFlags & OpFlags.RightToLeft) != OpFlags.None;
					bool flag6 = (opFlags & OpFlags.IgnoreCase) != OpFlags.None;
					int num17 = program[pc + 1];
					if (flag5)
					{
						ref_ptr2 -= num17;
						if (ref_ptr2 < 0)
						{
							break;
						}
					}
					else if (ref_ptr2 + num17 > text_end)
					{
						break;
					}
					pc += 2;
					for (int k = 0; k < num17; k++)
					{
						char c = text[ref_ptr2 + k];
						if (flag6)
						{
							c = char.ToLower(c);
						}
						if (c != program[pc++])
						{
							goto end_IL_0028;
						}
					}
					if (!flag5)
					{
						ref_ptr2 += num17;
					}
					continue;
				}
				case OpCode.Reference:
				{
					bool flag2 = (opFlags & OpFlags.RightToLeft) != OpFlags.None;
					bool flag3 = (opFlags & OpFlags.IgnoreCase) != OpFlags.None;
					int lastDefined2 = GetLastDefined(program[pc + 1]);
					if (lastDefined2 < 0)
					{
						break;
					}
					int index = marks[lastDefined2].Index;
					int length = marks[lastDefined2].Length;
					if (flag2)
					{
						ref_ptr2 -= length;
						if (ref_ptr2 < 0)
						{
							break;
						}
					}
					else if (ref_ptr2 + length > text_end)
					{
						break;
					}
					pc += 2;
					if (flag3)
					{
						for (int i = 0; i < length; i++)
						{
							if (char.ToLower(text[ref_ptr2 + i]) != char.ToLower(text[index + i]))
							{
								goto end_IL_0028;
							}
						}
					}
					else
					{
						for (int j = 0; j < length; j++)
						{
							if (text[ref_ptr2 + j] != text[index + j])
							{
								goto end_IL_0028;
							}
						}
					}
					if (!flag2)
					{
						ref_ptr2 += length;
					}
					continue;
				}
				case OpCode.Character:
				case OpCode.Category:
				case OpCode.NotCategory:
				case OpCode.Range:
				case OpCode.Set:
					if (!EvalChar(mode, ref ref_ptr2, ref pc, multi: false))
					{
						break;
					}
					continue;
				case OpCode.In:
				{
					int num10 = pc + program[pc + 1];
					pc += 2;
					if (!EvalChar(mode, ref ref_ptr2, ref pc, multi: true))
					{
						break;
					}
					pc = num10;
					continue;
				}
				case OpCode.Open:
					Open(program[pc + 1], ref_ptr2);
					pc += 2;
					continue;
				case OpCode.Close:
					Close(program[pc + 1], ref_ptr2);
					pc += 2;
					continue;
				case OpCode.BalanceStart:
				{
					int ptr = ref_ptr2;
					if (!Eval(Mode.Match, ref ref_ptr2, pc + 5) || !Balance(program[pc + 1], program[pc + 2], (program[pc + 3] == 1) ? true : false, ptr))
					{
						break;
					}
					pc += program[pc + 4];
					continue;
				}
				case OpCode.IfDefined:
				{
					int lastDefined = GetLastDefined(program[pc + 2]);
					pc = ((lastDefined >= 0) ? (pc + 3) : (pc + program[pc + 1]));
					continue;
				}
				case OpCode.Sub:
					if (!Eval(Mode.Match, ref ref_ptr2, pc + 2))
					{
						break;
					}
					pc += program[pc + 1];
					continue;
				case OpCode.Test:
				{
					int cp4 = Checkpoint();
					int ref_ptr3 = ref_ptr2;
					if (Eval(Mode.Match, ref ref_ptr3, pc + 3))
					{
						pc += program[pc + 1];
						continue;
					}
					Backtrack(cp4);
					pc += program[pc + 2];
					continue;
				}
				case OpCode.Branch:
					while (true)
					{
						int cp2 = Checkpoint();
						if (Eval(Mode.Match, ref ref_ptr2, pc + 2))
						{
							break;
						}
						Backtrack(cp2);
						pc += program[pc + 1];
						if ((ushort)(program[pc] & 0xFF) == 0)
						{
							goto end_IL_0028;
						}
					}
					goto case OpCode.True;
				case OpCode.Jump:
					pc += program[pc + 1];
					continue;
				case OpCode.Repeat:
					repeat = new RepeatContext(repeat, ReadProgramCount(pc + 2), ReadProgramCount(pc + 4), (opFlags & OpFlags.Lazy) != OpFlags.None, pc + 6);
					if (Eval(Mode.Match, ref ref_ptr2, pc + program[pc + 1]))
					{
						goto case OpCode.True;
					}
					repeat = repeat.Previous;
					break;
				case OpCode.Until:
					repeatContext = repeat;
					if (deep != repeatContext)
					{
						start = repeatContext.Start;
						count = repeatContext.Count;
						while (!repeatContext.IsMinimum)
						{
							repeatContext.Count++;
							repeatContext.Start = ref_ptr2;
							deep = repeatContext;
							if (!Eval(Mode.Match, ref ref_ptr2, repeatContext.Expression))
							{
								goto IL_09bc;
							}
							if (deep == repeatContext)
							{
								continue;
							}
							goto case OpCode.True;
						}
						if (ref_ptr2 == repeatContext.Start)
						{
							repeat = repeatContext.Previous;
							deep = null;
							if (!Eval(Mode.Match, ref ref_ptr2, pc + 1))
							{
								repeat = repeatContext;
								break;
							}
						}
						else if (repeatContext.IsLazy)
						{
							while (true)
							{
								repeat = repeatContext.Previous;
								deep = null;
								int cp3 = Checkpoint();
								if (Eval(Mode.Match, ref ref_ptr2, pc + 1))
								{
									break;
								}
								Backtrack(cp3);
								repeat = repeatContext;
								if (repeatContext.IsMaximum)
								{
									goto end_IL_0028;
								}
								repeatContext.Count++;
								repeatContext.Start = ref_ptr2;
								deep = repeatContext;
								if (!Eval(Mode.Match, ref ref_ptr2, repeatContext.Expression))
								{
									repeatContext.Start = start;
									repeatContext.Count = count;
									goto end_IL_0028;
								}
								if (deep != repeatContext)
								{
									break;
								}
								if (ref_ptr2 == repeatContext.Start)
								{
									goto end_IL_0028;
								}
							}
						}
						else
						{
							count2 = stack.Count;
							while (true)
							{
								if (!repeatContext.IsMaximum)
								{
									int num14 = Checkpoint();
									int value = ref_ptr2;
									int start2 = repeatContext.Start;
									repeatContext.Count++;
									repeatContext.Start = ref_ptr2;
									deep = repeatContext;
									if (!Eval(Mode.Match, ref ref_ptr2, repeatContext.Expression))
									{
										repeatContext.Count--;
										repeatContext.Start = start2;
										Backtrack(num14);
									}
									else
									{
										if (deep != repeatContext)
										{
											break;
										}
										stack.Push(num14);
										stack.Push(value);
										if (ref_ptr2 != repeatContext.Start)
										{
											continue;
										}
									}
								}
								repeat = repeatContext.Previous;
								goto IL_0bf5;
							}
							stack.Count = count2;
						}
					}
					goto case OpCode.True;
				case OpCode.FastRepeat:
				{
					fast = new RepeatContext(fast, ReadProgramCount(pc + 2), ReadProgramCount(pc + 4), (opFlags & OpFlags.Lazy) != OpFlags.None, pc + 6);
					fast.Start = ref_ptr2;
					int cp = Checkpoint();
					pc += program[pc + 1];
					ushort num2 = program[pc];
					int num3 = -1;
					int num4 = -1;
					int num5 = 0;
					OpCode opCode2 = (OpCode)(num2 & 0xFF);
					if (opCode2 == OpCode.Character || opCode2 == OpCode.String)
					{
						OpFlags opFlags2 = (OpFlags)(num2 & 0xFF00);
						if ((opFlags2 & OpFlags.Negate) == OpFlags.None)
						{
							if (opCode2 == OpCode.String)
							{
								int num6 = 0;
								if ((opFlags2 & OpFlags.RightToLeft) != 0)
								{
									num6 = program[pc + 1] - 1;
								}
								num3 = program[pc + 2 + num6];
							}
							else
							{
								num3 = program[pc + 1];
							}
							num4 = (((opFlags2 & OpFlags.IgnoreCase) == OpFlags.None) ? num3 : char.ToUpper((char)num3));
							num5 = (((opFlags2 & OpFlags.RightToLeft) != 0) ? (-1) : 0);
						}
					}
					if (fast.IsLazy)
					{
						if (!fast.IsMinimum && !Eval(Mode.Count, ref ref_ptr2, fast.Expression))
						{
							fast = fast.Previous;
							break;
						}
						while (true)
						{
							int num7 = ref_ptr2 + num5;
							if (num3 < 0 || (num7 >= 0 && num7 < text_end && (num3 == text[num7] || num4 == text[num7])))
							{
								deep = null;
								if (Eval(Mode.Match, ref ref_ptr2, pc))
								{
									break;
								}
							}
							if (fast.IsMaximum)
							{
								goto IL_0e57;
							}
							Backtrack(cp);
							if (Eval(Mode.Count, ref ref_ptr2, fast.Expression))
							{
								continue;
							}
							goto IL_0e8e;
						}
						fast = fast.Previous;
					}
					else
					{
						if (!Eval(Mode.Count, ref ref_ptr2, fast.Expression))
						{
							fast = fast.Previous;
							break;
						}
						int num8 = (fast.Count > 0) ? ((ref_ptr2 - fast.Start) / fast.Count) : 0;
						while (true)
						{
							int num9 = ref_ptr2 + num5;
							if (num3 < 0 || (num9 >= 0 && num9 < text_end && (num3 == text[num9] || num4 == text[num9])))
							{
								deep = null;
								if (Eval(Mode.Match, ref ref_ptr2, pc))
								{
									break;
								}
							}
							fast.Count--;
							if (fast.IsMinimum)
							{
								ref_ptr2 -= num8;
								Backtrack(cp);
								continue;
							}
							goto IL_0fab;
						}
						fast = fast.Previous;
					}
					goto case OpCode.True;
				}
				case OpCode.True:
				case OpCode.Balance:
					ref_ptr = ref_ptr2;
					switch (mode)
					{
					case Mode.Match:
						return true;
					case Mode.Count:
						fast.Count++;
						if (!fast.IsMaximum && (!fast.IsLazy || !fast.IsMinimum))
						{
							pc = fast.Expression;
							continue;
						}
						return true;
					}
					break;
				case OpCode.False:
				case OpCode.Info:
					break;
					IL_0fab:
					fast = fast.Previous;
					break;
					IL_0234:
					if (num11 == scan_ptr)
					{
						ref_ptr2 = ((!flag) ? (scan_ptr - num12) : (scan_ptr + num12));
						if (TryMatch(ref ref_ptr2, pc + num13))
						{
							goto case OpCode.True;
						}
						break;
					}
					break;
					IL_0165:
					if (num11 == 0)
					{
						ref_ptr2 = 0;
						if (TryMatch(ref ref_ptr2, pc + num13))
						{
							goto case OpCode.True;
						}
						num11++;
					}
					for (; (flag && num11 >= 0) || (!flag && num11 <= num15); num11 = ((!flag) ? (num11 + 1) : (num11 - 1)))
					{
						if (num11 != 0 && text[num11 - 1] != '\n')
						{
							continue;
						}
						ref_ptr2 = ((!flag) ? ((num11 != 0) ? (num11 - num12) : num11) : ((num11 != num15) ? (num11 + num12) : num11));
						if (!TryMatch(ref ref_ptr2, pc + num13))
						{
							continue;
						}
						goto case OpCode.True;
					}
					break;
					IL_0c31:
					repeat = repeatContext;
					break;
					IL_09bc:
					repeatContext.Start = start;
					repeatContext.Count = count;
					break;
					IL_0e57:
					fast = fast.Previous;
					break;
					IL_0e8e:
					fast = fast.Previous;
					break;
					IL_0bf5:
					while (true)
					{
						deep = null;
						if (Eval(Mode.Match, ref ref_ptr2, pc + 1))
						{
							break;
						}
						if (stack.Count != count2)
						{
							repeatContext.Count--;
							ref_ptr2 = stack.Pop();
							Backtrack(stack.Pop());
							continue;
						}
						goto IL_0c31;
					}
					stack.Count = count2;
					goto case OpCode.True;
					end_IL_0028:
					break;
				}
				break;
			}
			switch (mode)
			{
			case Mode.Match:
				return false;
			case Mode.Count:
				if (!fast.IsLazy && fast.IsMinimum)
				{
					return true;
				}
				ref_ptr = fast.Start;
				return false;
			default:
				return false;
			}
		}

		private bool EvalChar(Mode mode, ref int ptr, ref int pc, bool multi)
		{
			bool flag = false;
			char c = '\0';
			bool flag3;
			do
			{
				ushort num = program[pc];
				OpCode opCode = (OpCode)(num & 0xFF);
				OpFlags opFlags = (OpFlags)(num & 0xFF00);
				pc++;
				bool flag2 = (opFlags & OpFlags.IgnoreCase) != OpFlags.None;
				if (!flag)
				{
					if ((opFlags & OpFlags.RightToLeft) != 0)
					{
						if (ptr <= 0)
						{
							return false;
						}
						c = text[--ptr];
					}
					else
					{
						if (ptr >= text_end)
						{
							return false;
						}
						c = text[ptr++];
					}
					if (flag2)
					{
						c = char.ToLower(c);
					}
					flag = true;
				}
				flag3 = ((opFlags & OpFlags.Negate) != OpFlags.None);
				switch (opCode)
				{
				case OpCode.True:
					return true;
				case OpCode.False:
					return false;
				case OpCode.Character:
					if (c == program[pc++])
					{
						return !flag3;
					}
					break;
				case OpCode.Category:
					if (CategoryUtils.IsCategory((Category)program[pc++], c))
					{
						return !flag3;
					}
					break;
				case OpCode.NotCategory:
					if (!CategoryUtils.IsCategory((Category)program[pc++], c))
					{
						return !flag3;
					}
					break;
				case OpCode.Range:
				{
					int num6 = program[pc++];
					int num7 = program[pc++];
					if (num6 <= c && c <= num7)
					{
						return !flag3;
					}
					break;
				}
				case OpCode.Set:
				{
					int num2 = program[pc++];
					int num3 = program[pc++];
					int num4 = pc;
					pc += num3;
					int num5 = c - num2;
					if (num5 < 0 || num5 >= num3 << 4 || (program[num4 + (num5 >> 4)] & (1 << (num5 & 0xF))) == 0)
					{
						break;
					}
					return !flag3;
				}
				}
			}
			while (multi);
			return flag3;
		}

		private bool TryMatch(ref int ref_ptr, int pc)
		{
			Reset();
			int ref_ptr2 = ref_ptr;
			marks[groups[0]].Start = ref_ptr2;
			if (Eval(Mode.Match, ref ref_ptr2, pc))
			{
				marks[groups[0]].End = ref_ptr2;
				ref_ptr = ref_ptr2;
				return true;
			}
			return false;
		}

		private bool IsPosition(Position pos, int ptr)
		{
			switch (pos)
			{
			case Position.Start:
			case Position.StartOfString:
				return ptr == 0;
			case Position.StartOfLine:
				return ptr == 0 || text[ptr - 1] == '\n';
			case Position.StartOfScan:
				return ptr == scan_ptr;
			case Position.End:
				return ptr == text_end || (ptr == text_end - 1 && text[ptr] == '\n');
			case Position.EndOfLine:
				return ptr == text_end || text[ptr] == '\n';
			case Position.EndOfString:
				return ptr == text_end;
			case Position.Boundary:
				if (text_end == 0)
				{
					return false;
				}
				if (ptr == 0)
				{
					return IsWordChar(text[ptr]);
				}
				if (ptr == text_end)
				{
					return IsWordChar(text[ptr - 1]);
				}
				return IsWordChar(text[ptr]) != IsWordChar(text[ptr - 1]);
			case Position.NonBoundary:
				if (text_end == 0)
				{
					return false;
				}
				if (ptr == 0)
				{
					return !IsWordChar(text[ptr]);
				}
				if (ptr == text_end)
				{
					return !IsWordChar(text[ptr - 1]);
				}
				return IsWordChar(text[ptr]) == IsWordChar(text[ptr - 1]);
			default:
				return false;
			}
		}

		private bool IsWordChar(char c)
		{
			return CategoryUtils.IsCategory(Category.Word, c);
		}

		private string GetString(int pc)
		{
			int num = program[pc + 1];
			int num2 = pc + 2;
			char[] array = new char[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (char)program[num2++];
			}
			return new string(array);
		}

		private void Open(int gid, int ptr)
		{
			int num = groups[gid];
			if (num < mark_start || marks[num].IsDefined)
			{
				num = CreateMark(num);
				groups[gid] = num;
			}
			marks[num].Start = ptr;
		}

		private void Close(int gid, int ptr)
		{
			marks[groups[gid]].End = ptr;
		}

		private bool Balance(int gid, int balance_gid, bool capture, int ptr)
		{
			int num = groups[balance_gid];
			if (num == -1 || marks[num].Index < 0)
			{
				return false;
			}
			if (gid > 0 && capture)
			{
				Open(gid, marks[num].Index + marks[num].Length);
				Close(gid, ptr);
			}
			groups[balance_gid] = marks[num].Previous;
			return true;
		}

		private int Checkpoint()
		{
			mark_start = mark_end;
			return mark_start;
		}

		private void Backtrack(int cp)
		{
			for (int i = 0; i < groups.Length; i++)
			{
				int num = groups[i];
				while (cp <= num)
				{
					num = marks[num].Previous;
				}
				groups[i] = num;
			}
		}

		private void ResetGroups()
		{
			int num = groups.Length;
			if (marks == null)
			{
				marks = new Mark[num * 10];
			}
			for (int i = 0; i < num; i++)
			{
				groups[i] = i;
				marks[i].Start = -1;
				marks[i].End = -1;
				marks[i].Previous = -1;
			}
			mark_start = 0;
			mark_end = num;
		}

		private int GetLastDefined(int gid)
		{
			int num = groups[gid];
			while (num >= 0 && !marks[num].IsDefined)
			{
				num = marks[num].Previous;
			}
			return num;
		}

		private int CreateMark(int previous)
		{
			if (mark_end == marks.Length)
			{
				Mark[] array = new Mark[marks.Length * 2];
				marks.CopyTo(array, 0);
				marks = array;
			}
			int num = mark_end++;
			marks[num].Start = (marks[num].End = -1);
			marks[num].Previous = previous;
			return num;
		}

		private void GetGroupInfo(int gid, out int first_mark_index, out int n_caps)
		{
			first_mark_index = -1;
			n_caps = 0;
			for (int num = groups[gid]; num >= 0; num = marks[num].Previous)
			{
				if (marks[num].IsDefined)
				{
					if (first_mark_index < 0)
					{
						first_mark_index = num;
					}
					n_caps++;
				}
			}
		}

		private void PopulateGroup(Group g, int first_mark_index, int n_caps)
		{
			int num = 1;
			for (int previous = marks[first_mark_index].Previous; previous >= 0; previous = marks[previous].Previous)
			{
				if (marks[previous].IsDefined)
				{
					Capture cap = new Capture(text, marks[previous].Index, marks[previous].Length);
					g.Captures.SetValue(cap, n_caps - 1 - num);
					num++;
				}
			}
		}

		private Match GenerateMatch(Regex regex)
		{
			GetGroupInfo(0, out int first_mark_index, out int n_caps);
			if (!needs_groups_or_captures)
			{
				return new Match(regex, this, text, text_end, 0, marks[first_mark_index].Index, marks[first_mark_index].Length);
			}
			Match match = new Match(regex, this, text, text_end, groups.Length, marks[first_mark_index].Index, marks[first_mark_index].Length, n_caps);
			PopulateGroup(match, first_mark_index, n_caps);
			for (int i = 1; i < groups.Length; i++)
			{
				GetGroupInfo(i, out first_mark_index, out n_caps);
				Group g;
				if (first_mark_index < 0)
				{
					g = Group.Fail;
				}
				else
				{
					g = new Group(text, marks[first_mark_index].Index, marks[first_mark_index].Length, n_caps);
					PopulateGroup(g, first_mark_index, n_caps);
				}
				match.Groups.SetValue(g, i);
			}
			return match;
		}
	}
}
