using System.IO;

namespace HDATextTool.IO.Compression
{
    /// <summary>
    ///     Handles the compression format used on the game Harvest Moon: Save the Homeland.
    /// </summary>
    class HarvestCompression
    {
        /// <summary>
        ///     Decompresses data from Harvest Moon: Save the Homeland.
        /// </summary>
        /// <param name="Data">The byte array with the data to be decompressed</param>
        /// <returns>The decompressed data</returns>
        public static byte[] Decompress(byte[] Data)
        {
            int DataOffset = 0;

            using (MemoryStream Output = new MemoryStream())
            {
                while (DataOffset < Data.Length)
                {
                    int Back;
                    int Length;
                    int DirectCopy;
                    byte Header = Data[DataOffset++];

                    if (Header < 0x10) //0x00 ~ 0x0f
                    {
                        /*
                         * Direct copy
                         */

                        if (Header == 0)
                            Length = Data[DataOffset++] + 0x12;
                        else
                            Length = Header + 3;

                        Output.Write(Data, DataOffset, Length);
                        DataOffset += Length;
                    }
                    else
                    {
                        /*
                         * Compressed
                         */

                        if (Header < 0x20) //0x10 ~ 0x1f
                        {
                            if ((Length = (Header & 7) + 2) == 2)
                            {
                                while ((Header = Data[DataOffset++]) == 0) Length += 0xff;
                                Length += Header + 7;
                            }

                            DirectCopy = Data[DataOffset] & 3;
                            Back = (Data[DataOffset++] >> 2) | (Data[DataOffset++] << 6) | ((Header & 8) << 11);
                            if (Back != 0) Back += 0x4000; else break; //Compression end
                        }
                        else if (Header < 0x40) //0x20 ~ 0x3f
                        {
                            if ((Length = (Header & 0x1f) + 2) == 2)
                            {
                                while ((Header = Data[DataOffset++]) == 0) Length += 0xff;
                                Length += Header + 0x1f;
                            }

                            DirectCopy = Data[DataOffset] & 3;
                            Back = ((Data[DataOffset++] >> 2) | (Data[DataOffset++] << 6)) + 1;
                        }
                        else //0x40 ~ 0xff
                        {
                            Length = (Header >> 5) + 1;
                            DirectCopy = Header & 3;
                            Back = (((Header >> 2) & 7) | (Data[DataOffset++] << 3)) + 1;
                        }

                        //Go back and writes compressed data
                        long Position = Output.Position;
                        while (Length-- > 0)
                        {
                            Output.Seek(Position - Back, SeekOrigin.Begin);
                            int Value = Output.ReadByte();
                            Output.Seek(Position, SeekOrigin.Begin);
                            Output.WriteByte((byte)Value);
                            Position++;
                        }

                        //Writes remaining direct copy data
                        Output.Write(Data, DataOffset, DirectCopy);
                        DataOffset += DirectCopy;
                    }
                }

                return Output.ToArray();
            }
        }

        //TODO: Compression
    }
}
