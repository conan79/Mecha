﻿/* Copyright (c) 2010 Michael Lidgren

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
USE OR OTHER DEALINGS IN THE SOFTWARE.

*/
using System;
using System.Diagnostics;
using System.Net;

namespace Lidgren.Network
{
	public partial class NetBuffer
	{
		
		/// Gets the internal data buffer
		
		public byte[] PeekDataBuffer() { return m_data; }

		//
		// 1 bit
		//
		
		/// Reads a 1-bit Boolean without advancing the read pointer
		
		public bool PeekBoolean()
		{
			NetException.Assert(m_bitLength - m_readPosition >= 1, c_readOverflowError);
			byte retval = NetBitWriter.ReadByte(m_data, 1, m_readPosition);
			return (retval > 0 ? true : false);
		}

		//
		// 8 bit 
		//
		
		/// Reads a Byte without advancing the read pointer
		
		public byte PeekByte()
		{
			NetException.Assert(m_bitLength - m_readPosition >= 8, c_readOverflowError);
			byte retval = NetBitWriter.ReadByte(m_data, 8, m_readPosition);
			return retval;
		}

		
		/// Reads an SByte without advancing the read pointer
		
		
		public sbyte PeekSByte()
		{
			NetException.Assert(m_bitLength - m_readPosition >= 8, c_readOverflowError);
			byte retval = NetBitWriter.ReadByte(m_data, 8, m_readPosition);
			return (sbyte)retval;
		}

		
		/// Reads the specified number of bits into a Byte without advancing the read pointer
		
		public byte PeekByte(int numberOfBits)
		{
			byte retval = NetBitWriter.ReadByte(m_data, numberOfBits, m_readPosition);
			return retval;
		}

		
		/// Reads the specified number of bytes without advancing the read pointer
		
		public byte[] PeekBytes(int numberOfBytes)
		{
			NetException.Assert(m_bitLength - m_readPosition >= (numberOfBytes * 8), c_readOverflowError);

			byte[] retval = new byte[numberOfBytes];
			NetBitWriter.ReadBytes(m_data, numberOfBytes, m_readPosition, retval, 0);
			return retval;
		}

		
		/// Reads the specified number of bytes without advancing the read pointer
		
		public void PeekBytes(byte[] into, int offset, int numberOfBytes)
		{
			NetException.Assert(m_bitLength - m_readPosition >= (numberOfBytes * 8), c_readOverflowError);
			NetException.Assert(offset + numberOfBytes <= into.Length);

			NetBitWriter.ReadBytes(m_data, numberOfBytes, m_readPosition, into, offset);
			return;
		}

		//
		// 16 bit
		//
		
		/// Reads an Int16 without advancing the read pointer
		
		public Int16 PeekInt16()
		{
			NetException.Assert(m_bitLength - m_readPosition >= 16, c_readOverflowError);
			uint retval = NetBitWriter.ReadUInt16(m_data, 16, m_readPosition);
			return (short)retval;
		}

		
		/// Reads a UInt16 without advancing the read pointer
		
		
		public UInt16 PeekUInt16()
		{
			NetException.Assert(m_bitLength - m_readPosition >= 16, c_readOverflowError);
			uint retval = NetBitWriter.ReadUInt16(m_data, 16, m_readPosition);
			return (ushort)retval;
		}

		//
		// 32 bit
		//
		
		/// Reads an Int32 without advancing the read pointer
		
		public Int32 PeekInt32()
		{
			NetException.Assert(m_bitLength - m_readPosition >= 32, c_readOverflowError);
			uint retval = NetBitWriter.ReadUInt32(m_data, 32, m_readPosition);
			return (Int32)retval;
		}

		
		/// Reads the specified number of bits into an Int32 without advancing the read pointer
		
		public Int32 PeekInt32(int numberOfBits)
		{
			NetException.Assert((numberOfBits > 0 && numberOfBits <= 32), "ReadInt() can only read between 1 and 32 bits");
			NetException.Assert(m_bitLength - m_readPosition >= numberOfBits, c_readOverflowError);

			uint retval = NetBitWriter.ReadUInt32(m_data, numberOfBits, m_readPosition);

			if (numberOfBits == 32)
				return (int)retval;

			int signBit = 1 << (numberOfBits - 1);
			if ((retval & signBit) == 0)
				return (int)retval; // positive

			// negative
			unchecked
			{
				uint mask = ((uint)-1) >> (33 - numberOfBits);
				uint tmp = (retval & mask) + 1;
				return -((int)tmp);
			}
		}

		
		/// Reads a UInt32 without advancing the read pointer
		
		
		public UInt32 PeekUInt32()
		{
			NetException.Assert(m_bitLength - m_readPosition >= 32, c_readOverflowError);
			uint retval = NetBitWriter.ReadUInt32(m_data, 32, m_readPosition);
			return retval;
		}

		
		/// Reads the specified number of bits into a UInt32 without advancing the read pointer
		
		
		public UInt32 PeekUInt32(int numberOfBits)
		{
			NetException.Assert((numberOfBits > 0 && numberOfBits <= 32), "ReadUInt() can only read between 1 and 32 bits");
			//NetException.Assert(m_bitLength - m_readBitPtr >= numberOfBits, "tried to read past buffer size");

			UInt32 retval = NetBitWriter.ReadUInt32(m_data, numberOfBits, m_readPosition);
			return retval;
		}

		//
		// 64 bit
		//
		
		/// Reads a UInt64 without advancing the read pointer
		
		
		public UInt64 PeekUInt64()
		{
			NetException.Assert(m_bitLength - m_readPosition >= 64, c_readOverflowError);

			ulong low = NetBitWriter.ReadUInt32(m_data, 32, m_readPosition);
			ulong high = NetBitWriter.ReadUInt32(m_data, 32, m_readPosition + 32);

			ulong retval = low + (high << 32);

			return retval;
		}

		
		/// Reads an Int64 without advancing the read pointer
		
		public Int64 PeekInt64()
		{
			NetException.Assert(m_bitLength - m_readPosition >= 64, c_readOverflowError);
			unchecked
			{
				ulong retval = PeekUInt64();
				long longRetval = (long)retval;
				return longRetval;
			}
		}

		
		/// Reads the specified number of bits into an UInt64 without advancing the read pointer
		
		
		public UInt64 PeekUInt64(int numberOfBits)
		{
			NetException.Assert((numberOfBits > 0 && numberOfBits <= 64), "ReadUInt() can only read between 1 and 64 bits");
			NetException.Assert(m_bitLength - m_readPosition >= numberOfBits, c_readOverflowError);

			ulong retval;
			if (numberOfBits <= 32)
			{
				retval = (ulong)NetBitWriter.ReadUInt32(m_data, numberOfBits, m_readPosition);
			}
			else
			{
				retval = NetBitWriter.ReadUInt32(m_data, 32, m_readPosition);
				retval |= (UInt64)NetBitWriter.ReadUInt32(m_data, numberOfBits - 32, m_readPosition + 32) << 32;
			}
			return retval;
		}

		
		/// Reads the specified number of bits into an Int64 without advancing the read pointer
		
		public Int64 PeekInt64(int numberOfBits)
		{
			NetException.Assert(((numberOfBits > 0) && (numberOfBits < 65)), "ReadInt64(bits) can only read between 1 and 64 bits");
			return (long)PeekUInt64(numberOfBits);
		}

		//
		// Floating point
		//
		
		/// Reads a 32-bit Single without advancing the read pointer
		
		public float PeekFloat()
		{
			return PeekSingle();
		}

		
		/// Reads a 32-bit Single without advancing the read pointer
		
		public float PeekSingle()
		{
			NetException.Assert(m_bitLength - m_readPosition >= 32, c_readOverflowError);

			if ((m_readPosition & 7) == 0) // read directly
			{
				float retval = BitConverter.ToSingle(m_data, m_readPosition >> 3);
				return retval;
			}

			byte[] bytes = PeekBytes(4);
			return BitConverter.ToSingle(bytes, 0);
		}

		
		/// Reads a 64-bit Double without advancing the read pointer
		
		public double PeekDouble()
		{
			NetException.Assert(m_bitLength - m_readPosition >= 64, c_readOverflowError);

			if ((m_readPosition & 7) == 0) // read directly
			{
				// read directly
				double retval = BitConverter.ToDouble(m_data, m_readPosition >> 3);
				return retval;
			}

			byte[] bytes = PeekBytes(8);
			return BitConverter.ToDouble(bytes, 0);
		}

		
		/// Reads a string without advancing the read pointer
		
		public string PeekString()
		{
			int wasReadPosition = m_readPosition;
			string retval = ReadString();
			m_readPosition = wasReadPosition;
			return retval;
		}
	}
}
