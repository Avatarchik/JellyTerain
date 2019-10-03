using System.Collections;

namespace System.Text.RegularExpressions
{
	internal class PatternCompiler : ICompiler
	{
		private class PatternLinkStack : LinkStack
		{
			private struct Link
			{
				public int base_addr;

				public int offset_addr;
			}

			private Link link;

			public int BaseAddress
			{
				set
				{
					link.base_addr = value;
				}
			}

			public int OffsetAddress
			{
				get
				{
					return link.offset_addr;
				}
				set
				{
					link.offset_addr = value;
				}
			}

			public int GetOffset(int target_addr)
			{
				return target_addr - link.base_addr;
			}

			protected override object GetCurrent()
			{
				return link;
			}

			protected override void SetCurrent(object l)
			{
				link = (Link)l;
			}
		}

		private ArrayList pgm;

		private int CurrentAddress => pgm.Count;

		public PatternCompiler()
		{
			pgm = new ArrayList();
		}

		public static ushort EncodeOp(OpCode op, OpFlags flags)
		{
			return (ushort)((int)op | (int)(flags & (OpFlags)65280));
		}

		public static void DecodeOp(ushort word, out OpCode op, out OpFlags flags)
		{
			op = (OpCode)(word & 0xFF);
			flags = (OpFlags)(word & 0xFF00);
		}

		public void Reset()
		{
			pgm.Clear();
		}

		public IMachineFactory GetMachineFactory()
		{
			ushort[] array = new ushort[pgm.Count];
			pgm.CopyTo(array);
			return new InterpreterFactory(array);
		}

		public void EmitFalse()
		{
			Emit(OpCode.False);
		}

		public void EmitTrue()
		{
			Emit(OpCode.True);
		}

		private void EmitCount(int count)
		{
			Emit((ushort)(count & 0xFFFF));
			Emit((ushort)((uint)count >> 16));
		}

		public void EmitCharacter(char c, bool negate, bool ignore, bool reverse)
		{
			Emit(OpCode.Character, MakeFlags(negate, ignore, reverse, lazy: false));
			if (ignore)
			{
				c = char.ToLower(c);
			}
			Emit(c);
		}

		public void EmitCategory(Category cat, bool negate, bool reverse)
		{
			Emit(OpCode.Category, MakeFlags(negate, ignore: false, reverse, lazy: false));
			Emit((ushort)cat);
		}

		public void EmitNotCategory(Category cat, bool negate, bool reverse)
		{
			Emit(OpCode.NotCategory, MakeFlags(negate, ignore: false, reverse, lazy: false));
			Emit((ushort)cat);
		}

		public void EmitRange(char lo, char hi, bool negate, bool ignore, bool reverse)
		{
			Emit(OpCode.Range, MakeFlags(negate, ignore, reverse, lazy: false));
			Emit(lo);
			Emit(hi);
		}

		public void EmitSet(char lo, BitArray set, bool negate, bool ignore, bool reverse)
		{
			Emit(OpCode.Set, MakeFlags(negate, ignore, reverse, lazy: false));
			Emit(lo);
			int num = set.Length + 15 >> 4;
			Emit((ushort)num);
			int num2 = 0;
			while (num-- != 0)
			{
				ushort num4 = 0;
				for (int i = 0; i < 16; i++)
				{
					if (num2 >= set.Length)
					{
						break;
					}
					if (set[num2++])
					{
						num4 = (ushort)(num4 | (ushort)(1 << i));
					}
				}
				Emit(num4);
			}
		}

		public void EmitString(string str, bool ignore, bool reverse)
		{
			Emit(OpCode.String, MakeFlags(negate: false, ignore, reverse, lazy: false));
			int length = str.Length;
			Emit((ushort)length);
			if (ignore)
			{
				str = str.ToLower();
			}
			for (int i = 0; i < length; i++)
			{
				Emit(str[i]);
			}
		}

		public void EmitPosition(Position pos)
		{
			Emit(OpCode.Position, OpFlags.None);
			Emit((ushort)pos);
		}

		public void EmitOpen(int gid)
		{
			Emit(OpCode.Open);
			Emit((ushort)gid);
		}

		public void EmitClose(int gid)
		{
			Emit(OpCode.Close);
			Emit((ushort)gid);
		}

		public void EmitBalanceStart(int gid, int balance, bool capture, LinkRef tail)
		{
			BeginLink(tail);
			Emit(OpCode.BalanceStart);
			Emit((ushort)gid);
			Emit((ushort)balance);
			Emit((ushort)(capture ? 1 : 0));
			EmitLink(tail);
		}

		public void EmitBalance()
		{
			Emit(OpCode.Balance);
		}

		public void EmitReference(int gid, bool ignore, bool reverse)
		{
			Emit(OpCode.Reference, MakeFlags(negate: false, ignore, reverse, lazy: false));
			Emit((ushort)gid);
		}

		public void EmitIfDefined(int gid, LinkRef tail)
		{
			BeginLink(tail);
			Emit(OpCode.IfDefined);
			EmitLink(tail);
			Emit((ushort)gid);
		}

		public void EmitSub(LinkRef tail)
		{
			BeginLink(tail);
			Emit(OpCode.Sub);
			EmitLink(tail);
		}

		public void EmitTest(LinkRef yes, LinkRef tail)
		{
			BeginLink(yes);
			BeginLink(tail);
			Emit(OpCode.Test);
			EmitLink(yes);
			EmitLink(tail);
		}

		public void EmitBranch(LinkRef next)
		{
			BeginLink(next);
			Emit(OpCode.Branch, OpFlags.None);
			EmitLink(next);
		}

		public void EmitJump(LinkRef target)
		{
			BeginLink(target);
			Emit(OpCode.Jump, OpFlags.None);
			EmitLink(target);
		}

		public void EmitRepeat(int min, int max, bool lazy, LinkRef until)
		{
			BeginLink(until);
			Emit(OpCode.Repeat, MakeFlags(negate: false, ignore: false, reverse: false, lazy));
			EmitLink(until);
			EmitCount(min);
			EmitCount(max);
		}

		public void EmitUntil(LinkRef repeat)
		{
			ResolveLink(repeat);
			Emit(OpCode.Until);
		}

		public void EmitFastRepeat(int min, int max, bool lazy, LinkRef tail)
		{
			BeginLink(tail);
			Emit(OpCode.FastRepeat, MakeFlags(negate: false, ignore: false, reverse: false, lazy));
			EmitLink(tail);
			EmitCount(min);
			EmitCount(max);
		}

		public void EmitIn(LinkRef tail)
		{
			BeginLink(tail);
			Emit(OpCode.In);
			EmitLink(tail);
		}

		public void EmitAnchor(bool reverse, int offset, LinkRef tail)
		{
			BeginLink(tail);
			Emit(OpCode.Anchor, MakeFlags(negate: false, ignore: false, reverse, lazy: false));
			EmitLink(tail);
			Emit((ushort)offset);
		}

		public void EmitInfo(int count, int min, int max)
		{
			Emit(OpCode.Info);
			EmitCount(count);
			EmitCount(min);
			EmitCount(max);
		}

		public LinkRef NewLink()
		{
			return new PatternLinkStack();
		}

		public void ResolveLink(LinkRef lref)
		{
			PatternLinkStack patternLinkStack = (PatternLinkStack)lref;
			while (patternLinkStack.Pop())
			{
				pgm[patternLinkStack.OffsetAddress] = (ushort)patternLinkStack.GetOffset(CurrentAddress);
			}
		}

		public void EmitBranchEnd()
		{
		}

		public void EmitAlternationEnd()
		{
		}

		private static OpFlags MakeFlags(bool negate, bool ignore, bool reverse, bool lazy)
		{
			OpFlags opFlags = OpFlags.None;
			if (negate)
			{
				opFlags |= OpFlags.Negate;
			}
			if (ignore)
			{
				opFlags |= OpFlags.IgnoreCase;
			}
			if (reverse)
			{
				opFlags |= OpFlags.RightToLeft;
			}
			if (lazy)
			{
				opFlags |= OpFlags.Lazy;
			}
			return opFlags;
		}

		private void Emit(OpCode op)
		{
			Emit(op, OpFlags.None);
		}

		private void Emit(OpCode op, OpFlags flags)
		{
			Emit(EncodeOp(op, flags));
		}

		private void Emit(ushort word)
		{
			pgm.Add(word);
		}

		private void BeginLink(LinkRef lref)
		{
			PatternLinkStack patternLinkStack = (PatternLinkStack)lref;
			patternLinkStack.BaseAddress = CurrentAddress;
		}

		private void EmitLink(LinkRef lref)
		{
			PatternLinkStack patternLinkStack = (PatternLinkStack)lref;
			patternLinkStack.OffsetAddress = CurrentAddress;
			Emit((ushort)0);
			patternLinkStack.Push();
		}
	}
}
