﻿using System;
using System.IO;

namespace Jellyfish.Library
{
	internal static class NumberExtensions
	{
		internal static int Clamp(this int value, int min, int max)
		{
			return (value < min) ? min : (value > max) ? max : value;
		}
	}

	internal static class StreamExtensions
	{
		public static int ReadBlock(this Stream stream, byte[] buffer, int offset, ref int count)
		{
			int read = ReadBlock(stream, buffer, offset, count, count);
			count -= read;
			return read;
		}

		public static int ReadBlock(this Stream stream, byte[] buffer, int offset = 0, int count = int.MaxValue, int minCount = int.MaxValue)
		{
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer));
			}

			count = Math.Min(count, buffer.Length - offset);
			minCount = Math.Min(minCount, buffer.Length - offset);

			int total = 0;
			int read;
			do
			{
				total += read = stream.Read(buffer, offset + total, count - total);
			}
			while (read > 0 && total < count);

			if (total < minCount)
			{
				throw new EndOfStreamException();
			}

			return total;
		}

		public static int ReadWord(this Stream stream, bool optional = false)
		{
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

			int lowByte = stream.ReadByte();
			int highByte = stream.ReadByte();
			int word = lowByte | (highByte << 8);
			if ((word < 0) && !optional)
			{
				throw new EndOfStreamException();
			}

			return word;
		}
	}
}