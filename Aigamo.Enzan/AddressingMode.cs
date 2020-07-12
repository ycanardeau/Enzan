using System.IO;

namespace Aigamo.Enzan
{
	public readonly struct AddressingMode
	{
		public static readonly AddressingMode Empty = new();

		public ModRM ModRM { get; }
		public Sib Sib { get; }
		public uint Displacement { get; }

		public AddressingMode(BinaryReader reader)
		{
			ModRM = new ModRM(reader.ReadByte());

			var hasSib = ModRM.Mod != 3 && ModRM.RM == 4;
			Sib = hasSib ? new Sib(reader.ReadByte()) : Sib.Empty;
			var hasDisplacement32 = (ModRM.Mod == 0 && ModRM.RM == 5) || ModRM.Mod == 2 || (ModRM.Mod == 0 && Sib.Base == 5);
			var hasDisplacement8 = ModRM.Mod == 1;
			Displacement = true switch
			{
				_ when hasDisplacement32 => reader.ReadUInt32(),
				_ when hasDisplacement8 => reader.ReadByte(),
				_ => 0
			};
		}

		public bool IsConstant
		{
			get
			{
				if (ModRM.Mod != 0)
					return false;

				if (ModRM.RM == 5)
					return true;

				if (ModRM.RM != 4)
					return false;

				if (Sib.Index == 4 && Sib.Base == 5)
					return true;

				return false;
			}
		}
	}
}
