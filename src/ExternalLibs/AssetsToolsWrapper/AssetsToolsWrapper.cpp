#include "pch.h"

#include "AssetsToolsWrapper.h"

void AssetsToolsWrapper::Api::Unpack(String^ inputFile, String^ outputFile) 
{
	IntPtr inputPathNative = Marshal::StringToHGlobalAnsi(inputFile);
	IntPtr outputPathNative = Marshal::StringToHGlobalAnsi(outputFile);
	IAssetsReader* reader = Create_AssetsReaderFromFile(static_cast<char*>(inputPathNative.ToPointer()), true, RWOpenFlags_Immediately);
	IAssetsWriter* writer = Create_AssetsWriterToFile(static_cast<char*>(outputPathNative.ToPointer()), true, true, RWOpenFlags_Immediately);
	Marshal::FreeHGlobal(inputPathNative);
	Marshal::FreeHGlobal(outputPathNative);

	AssetBundleFile* bundle = new AssetBundleFile();
	if (bundle->Read(reader, NULL, true))
	{
		bundle->Unpack(reader, writer);
	}
		
	bundle->Close();
	reader->Close();
	writer->Close();

	delete bundle;
	delete reader;
	delete writer;
}

void AssetsToolsWrapper::Api::Extract(String^ inputFile, String^ outputFolder, String^ defaultName) 
{
	System::IO::Directory::CreateDirectory(outputFolder);

	IntPtr inputPathNative = Marshal::StringToHGlobalAnsi(inputFile);
	IAssetsReader* reader = Create_AssetsReaderFromFile(static_cast<char*>(inputPathNative.ToPointer()), true, RWOpenFlags_Immediately);
	Marshal::FreeHGlobal(inputPathNative);

	AssetBundleFile* bundle = new AssetBundleFile();
	if (bundle->Read(reader, NULL, false))
	{
		for (DWORD i = 0; i < bundle->bundleInf6->directoryCount; i++)
		{
			AssetBundleDirectoryInfo06* dirInf = &bundle->bundleInf6->dirInf[i];
			if (bundle->IsAssetsFile(reader, dirInf))
			{
				QWORD offset = dirInf->GetAbsolutePos(bundle);
				IAssetsReaderFromReaderRange* reader2 = Create_AssetsReaderFromReaderRange(reader, offset, dirInf->decompressedSize);
				AssetsFile* assetsFile = new AssetsFile(reader2);
				Extract(assetsFile, outputFolder, defaultName);
				delete assetsFile;
			}
		}
	}

	bundle->Close();
	reader->Close();
	
	delete bundle;
	delete reader;
}

void AssetsToolsWrapper::Api::Repack(String^ inputFile, String^ workFolder, String^ outputFile, String^ defaultName) 
{
	IntPtr inputPathNative = Marshal::StringToHGlobalAnsi(inputFile);
	IAssetsReader* reader = Create_AssetsReaderFromFile(static_cast<char*>(inputPathNative.ToPointer()), true, RWOpenFlags_Immediately);
	Marshal::FreeHGlobal(inputPathNative);

	AssetBundleFile* bundle = new AssetBundleFile();
	if (bundle->Read(reader, NULL, false))
	{
		BundleReplacer** bundleReplacers = new BundleReplacer*[bundle->bundleInf6->directoryCount];
		int bundleReplacersCount = 0;

		for (DWORD i = 0; i < bundle->bundleInf6->directoryCount; i++)
		{
			AssetBundleDirectoryInfo06* dirInf = &bundle->bundleInf6->dirInf[i];
			if (bundle->IsAssetsFile(reader, dirInf))
			{
				QWORD offset = dirInf->GetAbsolutePos(bundle);
				IAssetsReaderFromReaderRange* reader2 = Create_AssetsReaderFromReaderRange(reader, offset, dirInf->decompressedSize);
				AssetsFile* assetsFile = new AssetsFile(reader2);
				
				AssetsFileTable* table = new AssetsFileTable(assetsFile);

				AssetsReplacer** replacers = new AssetsReplacer*[table->assetFileInfoCount];
				int replacersCount = 0;
				
				GetReplacers(assetsFile, workFolder, replacers, replacersCount, defaultName);
				
				bundleReplacers[bundleReplacersCount] = MakeBundleEntryModifierFromAssets(dirInf->name, NULL, assetsFile, replacers, replacersCount, -1);
				bundleReplacersCount++;
				
				reader2->Close();

				delete table;
			}
		}

		IntPtr outputPathNative = Marshal::StringToHGlobalAnsi(outputFile);
		IAssetsWriter* writer = Create_AssetsWriterToFile(static_cast<char*>(outputPathNative.ToPointer()), true, true, RWOpenFlags_Immediately);
		Marshal::FreeHGlobal(outputPathNative);

		bundle->Write(reader, writer, bundleReplacers, bundleReplacersCount);

		writer->Close();
	}

	bundle->Close();
	reader->Close();
	
	delete bundle;
	delete reader;
}

void AssetsToolsWrapper::Api::Extract(AssetsFile* assetsFile, String^ outputFolder, String^ defaultName)
{
	AssetsFileTable* table = new AssetsFileTable(assetsFile);
	if (table->GenerateQuickLookupTree())
	{
		for (unsigned int i = 0; i < table->assetFileInfoCount; i++)
		{
			AssetFileInfoEx fileInfo = table->pAssetFileInfo[i];
			char* name = new char[100];
			fileInfo.ReadName(assetsFile, name);
			String^ str = gcnew String(name);
			delete name;

			if (String::IsNullOrEmpty(str))
			{
				str = defaultName;
			}
			
			byte* data = new byte[fileInfo.curFileSize];
			QWORD read = assetsFile->pReader->Read(fileInfo.absolutePos, fileInfo.curFileSize, data);
			if (read == fileInfo.curFileSize)
			{
				String^ fileName = String::Format("{0}.{1}", str, fileInfo.index);
				String^ outputPath = System::IO::Path::Combine(outputFolder, fileName);

				array<unsigned char>^ fileData = gcnew array<unsigned char>(fileInfo.curFileSize);
				Marshal::Copy(IntPtr(data), fileData, 0, fileInfo.curFileSize);
				System::IO::File::WriteAllBytes(outputPath, fileData);
			}
			delete data;
		}
	}
	delete table;
}

void AssetsToolsWrapper::Api::GetReplacers(AssetsFile* assetsFile, String^ workFolder, AssetsReplacer** replacers, int &replacersCount, String^ defaultName) 
{
	AssetsFileTable* table = new AssetsFileTable(assetsFile);
	if (table->GenerateQuickLookupTree())
	{
		for (unsigned int i = 0; i < table->assetFileInfoCount; i++)
		{
			AssetFileInfoEx fileInfo = table->pAssetFileInfo[i];
			char* name = new char[100];
			fileInfo.ReadName(assetsFile, name);
			String^ str = gcnew String(name);
			delete name;

			if (String::IsNullOrEmpty(str))
			{
				str = defaultName;
			}
			
			String^ fileName = String::Format("{0}.{1}", str, fileInfo.index);
			String^ tempPath = System::IO::Path::Combine(workFolder, fileName);

			if (System::IO::File::Exists(tempPath))
			{
				Type_0D t = assetsFile->typeTree.pTypes_Unity5[fileInfo.curFileTypeOrIndex];

				array<unsigned char>^ fileData = System::IO::File::ReadAllBytes(tempPath);
				byte* data = new byte[fileData->Length];
				Marshal::Copy(fileData, 0, IntPtr(data), fileData->Length);
				
				replacers[replacersCount] = MakeAssetModifierFromMemory(0, fileInfo.index, t.classId, t.scriptIndex, data, fileData->Length, NULL);
				replacersCount++;
			}
		}
	}
	delete table;
}