namespace System.Text.RegularExpressions.Syntax
{
	internal class RegularExpression : Group
	{
		private int group_count;

		public int GroupCount
		{
			get
			{
				return group_count;
			}
			set
			{
				group_count = value;
			}
		}

		public RegularExpression()
		{
			group_count = 0;
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			GetWidth(out int min, out int max);
			cmp.EmitInfo(group_count, min, max);
			AnchorInfo anchorInfo = GetAnchorInfo(reverse);
			LinkRef linkRef = cmp.NewLink();
			cmp.EmitAnchor(reverse, anchorInfo.Offset, linkRef);
			if (anchorInfo.IsPosition)
			{
				cmp.EmitPosition(anchorInfo.Position);
			}
			else if (anchorInfo.IsSubstring)
			{
				cmp.EmitString(anchorInfo.Substring, anchorInfo.IgnoreCase, reverse);
			}
			cmp.EmitTrue();
			cmp.ResolveLink(linkRef);
			base.Compile(cmp, reverse);
			cmp.EmitTrue();
		}
	}
}
