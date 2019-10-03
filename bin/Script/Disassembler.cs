namespace System.Text.RegularExpressions
{
	internal class Disassembler
	{
		public static void DisassemblePattern(ushort[] image)
		{
			DisassembleBlock(image, 0, 0);
		}

		public static void DisassembleBlock(ushort[] image, int pc, int depth)
		{
			while (pc < image.Length)
			{
				PatternCompiler.DecodeOp(image[pc], out OpCode op, out OpFlags _);
				Console.Write(FormatAddress(pc) + ": ");
				Console.Write(new string(' ', depth * 2));
				Console.Write(DisassembleOp(image, pc));
				Console.WriteLine();
				int num;
				switch (op)
				{
				case OpCode.False:
				case OpCode.True:
				case OpCode.Until:
					num = 1;
					break;
				case OpCode.Position:
				case OpCode.Reference:
				case OpCode.Character:
				case OpCode.Category:
				case OpCode.NotCategory:
				case OpCode.In:
				case OpCode.Open:
				case OpCode.Close:
				case OpCode.Sub:
				case OpCode.Branch:
				case OpCode.Jump:
					num = 2;
					break;
				case OpCode.Range:
				case OpCode.Balance:
				case OpCode.IfDefined:
				case OpCode.Test:
				case OpCode.Anchor:
					num = 3;
					break;
				case OpCode.Repeat:
				case OpCode.FastRepeat:
				case OpCode.Info:
					num = 4;
					break;
				case OpCode.String:
					num = image[pc + 1] + 2;
					break;
				case OpCode.Set:
					num = image[pc + 2] + 3;
					break;
				default:
					num = 1;
					break;
				}
				pc += num;
			}
		}

		public static string DisassembleOp(ushort[] image, int pc)
		{
			PatternCompiler.DecodeOp(image[pc], out OpCode op, out OpFlags flags);
			string text = op.ToString();
			if (flags != 0)
			{
				text = text + "[" + flags.ToString("f") + "]";
			}
			switch (op)
			{
			case OpCode.Info:
			{
				text = text + " " + image[pc + 1];
				string text2 = text;
				text = text2 + " (" + image[pc + 2] + ", " + image[pc + 3] + ")";
				break;
			}
			case OpCode.Character:
				text = text + " '" + FormatChar((char)image[pc + 1]) + "'";
				break;
			case OpCode.Category:
			case OpCode.NotCategory:
				text = text + " /" + (Category)image[pc + 1];
				break;
			case OpCode.Range:
				text = text + " '" + FormatChar((char)image[pc + 1]) + "', ";
				text = text + " '" + FormatChar((char)image[pc + 2]) + "'";
				break;
			case OpCode.Set:
				text = text + " " + FormatSet(image, pc + 1);
				break;
			case OpCode.String:
				text = text + " '" + ReadString(image, pc + 1) + "'";
				break;
			case OpCode.Position:
				text = text + " /" + (Position)image[pc + 1];
				break;
			case OpCode.Reference:
			case OpCode.Open:
			case OpCode.Close:
				text = text + " " + image[pc + 1];
				break;
			case OpCode.Balance:
			{
				string text2 = text;
				text = text2 + " " + image[pc + 1] + " " + image[pc + 2];
				break;
			}
			case OpCode.IfDefined:
			case OpCode.Anchor:
				text = text + " :" + FormatAddress(pc + image[pc + 1]);
				text = text + " " + image[pc + 2];
				break;
			case OpCode.In:
			case OpCode.Sub:
			case OpCode.Branch:
			case OpCode.Jump:
				text = text + " :" + FormatAddress(pc + image[pc + 1]);
				break;
			case OpCode.Test:
				text = text + " :" + FormatAddress(pc + image[pc + 1]);
				text = text + ", :" + FormatAddress(pc + image[pc + 2]);
				break;
			case OpCode.Repeat:
			case OpCode.FastRepeat:
			{
				text = text + " :" + FormatAddress(pc + image[pc + 1]);
				string text2 = text;
				text = text2 + " (" + image[pc + 2] + ", ";
				text = ((image[pc + 3] != ushort.MaxValue) ? (text + image[pc + 3]) : (text + "Inf"));
				text += ")";
				break;
			}
			}
			return text;
		}

		private static string ReadString(ushort[] image, int pc)
		{
			int num = image[pc];
			char[] array = new char[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (char)image[pc + i + 1];
			}
			return new string(array);
		}

		private static string FormatAddress(int pc)
		{
			return pc.ToString("x4");
		}

		private static string FormatSet(ushort[] image, int pc)
		{
			int num2 = image[pc++];
			int num4 = (image[pc++] << 4) - 1;
			string str = "[";
			bool flag = false;
			char c = '\0';
			for (int i = 0; i <= num4; i++)
			{
				bool flag2 = (image[pc + (i >> 4)] & (1 << (i & 0xF))) != 0;
				if (flag2 && !flag)
				{
					c = (char)(num2 + i);
					flag = true;
				}
				else if (flag & (!flag2 || i == num4))
				{
					char c2 = (char)(num2 + i - 1);
					str += FormatChar(c);
					if (c2 != c)
					{
						str = str + "-" + FormatChar(c2);
					}
					flag = false;
				}
			}
			return str + "]";
		}

		private static string FormatChar(char c)
		{
			if (c == '-' || c == ']')
			{
				return "\\" + c;
			}
			if (char.IsLetterOrDigit(c) || char.IsSymbol(c))
			{
				return c.ToString();
			}
			if (char.IsControl(c))
			{
				return "^" + (char)(64 + c);
			}
			int num = c;
			return "\\u" + num.ToString("x4");
		}
	}
}
