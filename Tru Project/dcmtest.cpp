#include "dcmtk/config/osconfig.h"
#include "dcmtk/dcmdata/dctk.h"
#include <iostream>

bool ReadPatientName(DcmFileFormat& fileformat, std::string& filePath)
{
    OFCondition status = fileformat.loadFile(filePath.c_str());
    if (!status.good())
    {
        std::cout << "Load Dimcom File Error: " << status.text() << std::endl;
        return false;
    }
    OFString PatientName;
    status = fileformat.getDataset()->findAndGetOFString(DCM_PatientName, PatientName);
    if (status.good())
    {
        std::cout << "Get PatientName:" << PatientName << std::endl;
    }
    else
    {
        std::cout << "Get PatientName Error:" << status.text() << std::endl;
        return false;
    }
    return true;
}

bool SavePatientName(DcmFileFormat& fileformat, std::string& filePath, const std::string& info)
{
    OFCondition status = fileformat.getDataset()->putAndInsertString(DCM_PatientName, info.c_str());
    if (status.good())
    {
        std::cout << "Save PatientName:" << info.c_str() << std::endl;
    }
    else
    {
        std::cout << "Save PatientName Error: " << status.text() << std::endl;
        return false;
    }

    status = fileformat.saveFile(filePath.c_str());
    if (!status.good())
    {
        std::cout << "Save Dimcom File Error: " << status.text() << std::endl;
        return false;
    }
    return true;
}

int main() {
	DcmFileFormat dicomFileformat;

	std::string dicomFile = "C:\\Users\\solu0\\Desktop\\DCT0000.dcm";

	ReadPatientName(dicomFileformat, dicomFile);

	SavePatientName(dicomFileformat, dicomFile, "test");

	ReadPatientName(dicomFileformat, dicomFile);

	system("pause");

	return 0;
}