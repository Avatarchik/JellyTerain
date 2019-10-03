// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Math
{
    class BigInteger : Object
    {
      // Fields:
  DEFAULT_LEN : UInt32
  WouldReturnNegVal : String
  length : UInt32
  data : UInt32[]
  smallPrimes : UInt32[]
  rng : RandomNumberGenerator
      // Properties:
  Rng : RandomNumberGenerator
      // Events:
      // Methods:
      public Void Mono.Math.BigInteger::.ctor()
      public Void Mono.Math.BigInteger::.ctor(Mono.Math.BigInteger/SignUInt32)
      public Void Mono.Math.BigInteger::.ctor(Mono.Math.BigInteger)
      public Void Mono.Math.BigInteger::.ctor(Mono.Math.BigIntegerUInt32)
      public Void Mono.Math.BigInteger::.ctorByte[])
      public Void Mono.Math.BigInteger::.ctorUInt32[])
      public Void Mono.Math.BigInteger::.ctorUInt32)
      public Void Mono.Math.BigInteger::.ctorUInt64)
      Void Mono.Math.BigInteger::.cctor()
      public Mono.Math.BigInteger Mono.Math.BigInteger::ParseString)
      public Mono.Math.BigInteger Mono.Math.BigInteger::Add(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Mono.Math.BigInteger Mono.Math.BigInteger::Subtract(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Int32 Mono.Math.BigInteger::Modulus(Mono.Math.BigIntegerInt32)
      public UInt32 Mono.Math.BigInteger::Modulus(Mono.Math.BigIntegerUInt32)
      public Mono.Math.BigInteger Mono.Math.BigInteger::Modulus(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Mono.Math.BigInteger Mono.Math.BigInteger::Divid(Mono.Math.BigIntegerInt32)
      public Mono.Math.BigInteger Mono.Math.BigInteger::Divid(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Mono.Math.BigInteger Mono.Math.BigInteger::Multiply(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Mono.Math.BigInteger Mono.Math.BigInteger::Multiply(Mono.Math.BigIntegerInt32)
      Security.Cryptography.RandomNumberGenerator Mono.Math.BigInteger::get_Rng()
      public Mono.Math.BigInteger Mono.Math.BigInteger::GenerateRandomInt32Security.Cryptography.RandomNumberGenerator)
      public Mono.Math.BigInteger Mono.Math.BigInteger::GenerateRandomInt32)
      public Void Mono.Math.BigInteger::RandomizeSecurity.Cryptography.RandomNumberGenerator)
      public Void Mono.Math.BigInteger::Randomize()
      public Int32 Mono.Math.BigInteger::BitCount()
      public Boolean Mono.Math.BigInteger::TestBitUInt32)
      public Boolean Mono.Math.BigInteger::TestBitInt32)
      public Void Mono.Math.BigInteger::SetBitUInt32)
      public Void Mono.Math.BigInteger::ClearBitUInt32)
      public Void Mono.Math.BigInteger::SetBitUInt32Boolean)
      public Int32 Mono.Math.BigInteger::LowestSetBit()
      public Byte[] Mono.Math.BigInteger::GetBytes()
      public Mono.Math.BigInteger/Sign Mono.Math.BigInteger::Compare(Mono.Math.BigInteger)
      public String Mono.Math.BigInteger::ToStringUInt32)
      public String Mono.Math.BigInteger::ToStringUInt32String)
      Void Mono.Math.BigInteger::Normalize()
      public Void Mono.Math.BigInteger::Clear()
      public Int32 Mono.Math.BigInteger::GetHashCode()
      public String Mono.Math.BigInteger::ToString()
      public Boolean Mono.Math.BigInteger::EqualsObject)
      public Mono.Math.BigInteger Mono.Math.BigInteger::GCD(Mono.Math.BigInteger)
      public Mono.Math.BigInteger Mono.Math.BigInteger::ModInverse(Mono.Math.BigInteger)
      public Mono.Math.BigInteger Mono.Math.BigInteger::ModPow(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Boolean Mono.Math.BigInteger::IsProbablePrime()
      public Mono.Math.BigInteger Mono.Math.BigInteger::NextHighestPrime(Mono.Math.BigInteger)
      public Mono.Math.BigInteger Mono.Math.BigInteger::GeneratePseudoPrimeInt32)
      public Void Mono.Math.BigInteger::Incr2()
      public Mono.Math.BigInteger Mono.Math.BigInteger::op_ImplicitUInt32)
      public Mono.Math.BigInteger Mono.Math.BigInteger::op_ImplicitInt32)
      public Mono.Math.BigInteger Mono.Math.BigInteger::op_ImplicitUInt64)
      public Mono.Math.BigInteger Mono.Math.BigInteger::op_Addition(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Mono.Math.BigInteger Mono.Math.BigInteger::op_Subtraction(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Int32 Mono.Math.BigInteger::op_Modulus(Mono.Math.BigIntegerInt32)
      public UInt32 Mono.Math.BigInteger::op_Modulus(Mono.Math.BigIntegerUInt32)
      public Mono.Math.BigInteger Mono.Math.BigInteger::op_Modulus(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Mono.Math.BigInteger Mono.Math.BigInteger::op_Division(Mono.Math.BigIntegerInt32)
      public Mono.Math.BigInteger Mono.Math.BigInteger::op_Division(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Mono.Math.BigInteger Mono.Math.BigInteger::op_Multiply(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Mono.Math.BigInteger Mono.Math.BigInteger::op_Multiply(Mono.Math.BigIntegerInt32)
      public Mono.Math.BigInteger Mono.Math.BigInteger::op_LeftShift(Mono.Math.BigIntegerInt32)
      public Mono.Math.BigInteger Mono.Math.BigInteger::op_RightShift(Mono.Math.BigIntegerInt32)
      public Boolean Mono.Math.BigInteger::op_Equality(Mono.Math.BigIntegerUInt32)
      public Boolean Mono.Math.BigInteger::op_Inequality(Mono.Math.BigIntegerUInt32)
      public Boolean Mono.Math.BigInteger::op_Equality(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Boolean Mono.Math.BigInteger::op_Inequality(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Boolean Mono.Math.BigInteger::op_GreaterThan(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Boolean Mono.Math.BigInteger::op_LessThan(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Boolean Mono.Math.BigInteger::op_GreaterThanOrEqual(Mono.Math.BigInteger,Mono.Math.BigInteger)
      public Boolean Mono.Math.BigInteger::op_LessThanOrEqual(Mono.Math.BigInteger,Mono.Math.BigInteger)
    }
}
