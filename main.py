import os
import pydicom as dicom
import matplotlib.pyplot as plt
import gdcm
import pillow_jpls
from PIL import Image

#path = ("./img/변정숙_1701-0002_0000.dcm")
folderPath = "C:\\Users\\solu0\\Downloads\\#26#27\\#26#27\\BJS_CT"
#folderPath = "C:\\Users\\solu0\\Downloads\\DICOM 파일\\#11\\KJH_CT"

X = []
for top, dir, f in os.walk(folderPath):
    for fileName in f:
        ds = dicom.dcmread(folderPath + '/' + fileName)
        X.append(ds)

window = plt.figure(figsize=(20,32))
n_cols = 6
n_row = (len(X))

for idx in range(0, (len(X))):
    window.add_subplot(n_row,n_cols,idx+1)
    plt.imshow(X[idx].pixel_array,cmap=plt.cm.bone)

"""
reader = gdcm.ImageReader()
reader.SetFileName("./img/변정숙_1701-0002_0000.dcm")
ret = reader.Read()
"""