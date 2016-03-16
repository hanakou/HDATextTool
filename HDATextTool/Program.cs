using System;
using System.IO;

using HDATextTool.IO;

namespace HDATextTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("HDATextTool by gdkchan");
            Console.WriteLine("Version 0.1.0");
            Console.ResetColor();
            Console.Write(Environment.NewLine);

            if (args.Length < 3)
            {
                PrintUsage();
                return;
            }
            else
            {
                switch (args[0])
                {
                    case "-xhda": HarvestDataArchive.Unpack(args[1], args[2]); break;
                    case "-chda": HarvestDataArchive.Pack(args[2], args[1]); break;
                    case "-xtxt": File.WriteAllText(args[3], HarvestText.Decode(args[1], args[2])); break;
                    case "-ctxt": HarvestText.Encode(File.ReadAllText(args[1]), args[2], args[3]); break;
                    case "-fixelf": HarvestElf.Fix(args[1], uint.Parse(args[2]), uint.Parse(args[3])); break;
                    default: TextOut.PrintError("Invalid command \"" + args[0] + "\" used!"); return;
                }
            }

            Console.Write(Environment.NewLine);
            TextOut.PrintSuccess("Finished!");
        }

        private static void PrintUsage()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Usage:");
            Console.ResetColor();
            Console.Write(Environment.NewLine);
            
            Console.WriteLine("tool.exe -xhda file.hda out_folder  Extract HDA data");
            Console.WriteLine("tool.exe -chda in_folder file.hda  Create HDA from folder");
            Console.WriteLine("tool.exe -xtxt text.bin pointers.bin out.txt  Extract text");
            Console.WriteLine("tool.exe -ctxt in.txt text.bin pointers.bin  Create text");
            Console.WriteLine("tool.exe -fixelf SLUS_202.51 lba file_size  Recalculate LBA table");
            Console.Write(Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("How to use the -fixelf command:");
            Console.ResetColor();
            Console.Write(Environment.NewLine);

            Console.WriteLine("You should always use this whenever you replace a file on the CD.");
            Console.WriteLine("It will recalculate the LBAs inside the executable.");
            Console.WriteLine("If you rebuild the image without fixing the executable,");
            Console.WriteLine("the game will most likely crash.");
            Console.WriteLine("First, get the LBA value of the file you're going to modify on");
            Console.WriteLine("your CD image editing software.");
            Console.WriteLine("Then, look the size of the modified HDA file generated with this tool.");
            Console.WriteLine("Now call the -fixelf command with the path to the ELF,");
            Console.WriteLine("the LBA value and the new size of the file.");
            Console.WriteLine("You should insert one file at a time, and do this process.");
            Console.WriteLine("Example: HDATextTool -fixelf 541 12345 (always decimal, not hex).");
        }
    }
}
