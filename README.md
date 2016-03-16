# HDATextTool
HDA (un)packer and text editing tool for Harvest Moon: Save the Homeland

This tool will allow you to modify text on the Harvest Moon: Save the Homeland PS2 game. It can decompress and extract data from the HDA files, and can also create HDA files back. It also can extract text from the files inside the HDA. It's possible to edit other kind of data aswell, like graphics, provided you have the appropriate program for that.

Also, I was too lazy to code a compressor, so once you re-create the HDA, all the data will be stored on the uncompressed form, so the file will probably be a bit bigger when recreated. But don't worry, the game accepts it just fine.

- Extracting and creating HDAs:

You can use the -xhda command to extract an HDA file. You can use it like this: "HDATextTool -xhda EVTMSG12.HDA EVTMSG12". In this case, it will create a "EVTMSG12" folder with the HDA contents.

To create an HDA, the process it's similar, you can use the -chda command like this: "HDATextTool -chda EVTMSG12 EVTMSG12.HDA". But be careful, any file inside the folder will be inserted on the HDA. So if you created files inside the folder, delete or move them before re-packing!

The file "EVTMSG12.HDA" is just a example (it contains the texts of the intro), but you can do it on any HDA file.

- Extracting and creating texts:

Texts even after decompressed, have another layer of proprietary encoding that will make hard to edit the text, so once again, you need to use the tool to extract the text in a format that is easy to edit.

To extract the text, you can use the -xtxt command. The texts are separated into two files, one containing the texts, and another containing the pointers to the text. You need to supply both to the tool, otherwise it won't know where each dialog is located. It is usually (but no always!) the file right after the text. So if "File_00001.bin" have text, "File_00002.bin" is the pointers for that text. You can extract with this command: "HDATextTool -xtxt File_00001.bin File_00002.bin texts.txt".

Re-creating the texts it's similar, you can use "HDATextTool -ctxt texts.txt File_00001.bin File_00002.bin" to do the inverse of the previous step, and re-create the texts.
Remember to delete the *.txt file before creating the HDA!

- Fixing the ELF LBA table:

The last step to modify ANY DATA into the game, is fixing the lba table on the elf.
First, use your CD image editing software to insert the modified HDA file into the ISO, and rebuild the image. You will notice that the game no longer works. This is because the elf have an internal table with the file positions, and when the iso is rebuild, files are moved around and the game will load the wrong data and eventually crash.
Fixing this is not very complicated. First extract the "SLUS_202.51" file from the CD, and place it at the same folder of the tool. After, go to your image editing software, and locate the LBA of the file you modified. After this, look the new size of the file you modified. When you have both, use the command "HDATextTool -fixelf SLUS_202.51 lba_here new_size_here". All values in decimal. You should see a message saying that the patching process was successful. Now insert the modified "SLUS_202.51" file back into the ISO, rebuild it and yay, the game works now!
You should insert one file at a time, and do this after inserting each file. Be careful, one mistake and the game will no longer work.


