#pragma once

#include "AssetBundleFileFormat.h"
#include "AssetsFileReader.h"
#include "AssetsFileTable.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace AssetsToolsWrapper 
{
	public ref class Api abstract sealed
	{
	public:

		static void Unpack(String^ inputFile, String^ outputFile);
		static void Extract(String^ inputFile, String^ outputFolder, String^ defaultName);
		static void Repack(String^ inputFile, String^ workFolder, String^ outputFile, String^ defaultName);

	private:

		static void Extract(AssetsFile* assetsFile, String^ outputFolder, String^ defaultName);
		static void GetReplacers(AssetsFile* assetsFile, String^ workFolder, AssetsReplacer** replacers, int &replacersCount, String^ defaultName) ;
	};
}
