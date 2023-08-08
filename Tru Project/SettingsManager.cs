using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;


// 세팅값을 저장하기 위한 클래스
[System.Serializable]
public class SettingValue
{
    public string _Panoramic_Thickess;
    public string _Saftyzone_Radial;
    public string _Saftyzone_Apical;
    public string _Finetuning_Translation;
    public string _Finetuning_Rotation;
    public string _Guide_Thickness;
    public string _Guide_Height;
    public string _Offset_Teeth;
    public string _Offset_Sleeve;
    public string _Offset_Sleeve2;
    public string _Offset_Anchor;
    public string _Blocksize_Cuboid;
    public string _Blocksize_Cuboid2;
    public string _Blocksize_Cylinder;
    public string _Blocksize_CuboidWindowXY;
    public string _Blocksize_CuboidWindowXY2;
    public string _Blocksize_CylinderWindow;
    public string _IDtag_Textsize;
    public string _Moveall_interval;
    public string _Interface_Cementgap;
    public string _Interface_Extracementgap;
    public string _Interface_Distanceformmargin;
    public string _Interface_Horizontaldistance;
    public string _Interface_Slopedistance;
    public string _Interface_Verticaldistance;
    public string _Interface_Angle;
    public string _Wax_addradius;
    public string _Wax_addradius2;
    public string _Wax_removeradius;
    public string _Wax_removeradius2;
    public string _Wax_smoothingradius;
    public string _Wax_smoothingradius2;
    public string _Morphing_radius;
    public string _Morphing_radius2;
    public string _Minimum_Thickness;
    public string _Contact_Proximal;
    public string _Contact_OcclusalAll;
    public string _Contact_OcclusalPart;
    public string _Bridge_Area;
    public string _Offset_Extracrownhole;

    public int _Display_Language = 0;
    public int _Display_Toothnumbering = 0;
    public bool _Autosave_UseOption_Radiobutton = false;
    public int _Autosave_UseOption = 0;
    public int _Export_Foldername = 0;
    public bool _Sleeve_Gluechannel = false;
    public bool _Sleeve_Protectsleevearea = false;
    public int _IDTag_Type = 0;
    public int _Crown_Library = 0;
    public bool _Screwhole_Use = false;
    public bool _Item_Overview = false;
    public bool _Item_ImplantPlanning = false;
    public bool _Item_Guide = false;
    public bool _Item_Temporarycrown = false;
    public bool _Item_Capturedimagelist = false;
    public bool _AutoMeasure_Show = false;

    public string _CaseData_Path;
    public string _Database_Path;
    public string _Designfile_path;
}

public class SettingsManager : MonoBehaviour
{
    public SettingValue SettingValueData;

    // 세팅값에 해당하는 UI들
    public Text Panoramic_Thickess;
    public Text Saftyzone_Radial;
    public Text Saftyzone_Apical;
    public Text Finetuning_Translation;
    public Text Finetuning_Rotation;
    public Text Guide_Thickness;
    public Text Guide_Height;
    public Text Offset_Teeth;
    public Text Offset_Sleeve;
    public Text Offset_Sleeve2;
    public Text Offset_Anchor;
    public Text Blocksize_Cuboid;
    public Text Blocksize_Cuboid2;
    public Text Blocksize_Cylinder;
    public Text Blocksize_CuboidWindowXY;
    public Text Blocksize_CuboidWindowXY2;
    public Text Blocksize_CylinderWindow;
    public Text IDtag_Textsize;
    public Text Moveall_interval;
    public Text Interface_Cementgap;
    public Text Interface_Extracementgap;
    public Text Interface_Distanceformmargin;
    public Text Interface_Horizontaldistance;
    public Text Interface_Slopedistance;
    public Text Interface_Verticaldistance;
    public Text Interface_Angle;
    public Text Wax_addradius;
    public Text Wax_addradius2;
    public Text Wax_removeradius;
    public Text Wax_removeradius2;
    public Text Wax_smoothingradius;
    public Text Wax_smoothingradius2;
    public Text Morphing_radius;
    public Text Morphing_radius2;
    public Text Minimum_Thickness;
    public Text Contact_Proximal;
    public Text Contact_OcclusalAll;
    public Text Contact_OcclusalPart;
    public Text Bridge_Area;
    public Text Offset_Extracrownhole;

    public Dropdown Display_Language_UI;
    public Dropdown Display_Toothnumbering_UI;
    public Toggle Autosave_UseOption_Radiobutton_UI;
    public Dropdown Autosave_UseOption_UI;
    public Dropdown Export_Foldername_UI;
    public Toggle Sleeve_Gluechannel_UI;
    public Toggle Sleeve_Protectsleevearea_UI;
    public Dropdown IDTag_Type_UI;
    public Dropdown Crown_Library_UI;
    public Toggle Screwhole_Use_UI;
    public Toggle Item_Overview_UI;
    public Toggle Item_ImplantPlanning_UI;
    public Toggle Item_Guide_UI;
    public Toggle Item_Temporarycrown_UI;
    public Toggle Item_Capturedimagelist_UI;
    public Toggle AutoMeasure_Show_UI;

    public int Display_Language = 0;
    public int Display_Toothnumbering = 0;
    public bool Autosave_UseOption_Radiobutton = false;
    public int Autosave_UseOption = 0;
    public int Export_Foldername = 0;
    public bool Sleeve_Gluechannel = false;
    public bool Sleeve_Protectsleevearea = false;
    public int IDTag_Type = 0;
    public int Crown_Library = 0;
    public bool Screwhole_Use = false;
    public bool Item_Overview = false;
    public bool Item_ImplantPlanning = false;
    public bool Item_Guide = false;
    public bool Item_Temporarycrown = false;
    public bool Item_Capturedimagelist = false;
    public bool AutoMeasure_Show = false;

    public string CaseData_Path;
    public string Database_Path;
    public string Designfile_path;

    public Text text_CaseData_Path;
    public Text text_Database_Path;
    public Text text_DesignFile_Path;


    public GameObject Database_Path_option;

    // 저장된 세팅값 불러오기
    public void delayStart()
    {
        string[] SerachFiles = Directory.GetFiles(Application.persistentDataPath, "PYLON_SettingValue.json");


        for (int i = 0; i < SerachFiles.Length; i++)
        {
            string data = File.ReadAllText(SerachFiles[i]);

            SettingValue tmp = JsonConvert.DeserializeObject<SettingValue>(data);   // 저장되어있던 케이스 내용이 담긴 json 파일을 가져오는 변수.

            SettingValueData = tmp;
        }
        if (SerachFiles.Length > 0)
        {
            LoadValue(true);
        }
        else
        {
            LoadValue(false);
        }

        LoadImage();
    }

    // 저장한 파일이 있다면, 해당 파일로부터 저장되어있는 케이스 값들을 불러오는 함수.
    public void LoadValue(bool haveSavefile)
    {
        if (haveSavefile)
        {
            Panoramic_Thickess.text = SettingValueData._Panoramic_Thickess;
            Saftyzone_Radial.text = SettingValueData._Saftyzone_Radial;
            Saftyzone_Apical.text = SettingValueData._Saftyzone_Apical;
            Finetuning_Translation.text = SettingValueData._Finetuning_Translation;
            Finetuning_Rotation.text = SettingValueData._Finetuning_Rotation;
            Guide_Thickness.text = SettingValueData._Guide_Thickness;
            Guide_Height.text = SettingValueData._Guide_Height;
            Offset_Teeth.text = SettingValueData._Offset_Teeth;
            Offset_Sleeve.text = SettingValueData._Offset_Sleeve;
            Offset_Sleeve2.text = SettingValueData._Offset_Sleeve2;
            Offset_Anchor.text = SettingValueData._Offset_Anchor;
            Blocksize_Cuboid.text = SettingValueData._Blocksize_Cuboid;
            Blocksize_Cuboid2.text = SettingValueData._Blocksize_Cuboid2;
            Blocksize_Cylinder.text = SettingValueData._Blocksize_Cylinder;
            Blocksize_CuboidWindowXY.text = SettingValueData._Blocksize_CuboidWindowXY;
            Blocksize_CuboidWindowXY2.text = SettingValueData._Blocksize_CuboidWindowXY2;
            Blocksize_CylinderWindow.text = SettingValueData._Blocksize_CylinderWindow;
            IDtag_Textsize.text = SettingValueData._IDtag_Textsize;
            Moveall_interval.text = SettingValueData._Moveall_interval;
            Interface_Cementgap.text = SettingValueData._Interface_Cementgap;
            Interface_Extracementgap.text = SettingValueData._Interface_Extracementgap;
            Interface_Distanceformmargin.text = SettingValueData._Interface_Distanceformmargin;
            Interface_Horizontaldistance.text = SettingValueData._Interface_Horizontaldistance;
            Interface_Slopedistance.text = SettingValueData._Interface_Slopedistance;
            Interface_Verticaldistance.text = SettingValueData._Interface_Verticaldistance;
            Interface_Angle.text = SettingValueData._Interface_Angle;
            Wax_addradius.text = SettingValueData._Wax_addradius;
            Wax_addradius2.text = SettingValueData._Wax_addradius2;
            Wax_removeradius.text = SettingValueData._Wax_removeradius;
            Wax_removeradius2.text = SettingValueData._Wax_removeradius2;
            Wax_smoothingradius.text = SettingValueData._Wax_smoothingradius;
            Wax_smoothingradius2.text = SettingValueData._Wax_smoothingradius2;
            Morphing_radius.text = SettingValueData._Morphing_radius;
            Morphing_radius2.text = SettingValueData._Morphing_radius2;
            Minimum_Thickness.text = SettingValueData._Minimum_Thickness;
            Contact_Proximal.text = SettingValueData._Contact_Proximal;
            Contact_OcclusalAll.text = SettingValueData._Contact_OcclusalAll;
            Contact_OcclusalPart.text = SettingValueData._Contact_OcclusalPart;
            Bridge_Area.text = SettingValueData._Bridge_Area;
            Offset_Extracrownhole.text = SettingValueData._Offset_Extracrownhole;

            CaseData_Path = SettingValueData._CaseData_Path;
            Database_Path = SettingValueData._Database_Path;
            Designfile_path = SettingValueData._Designfile_path;

            StaticManager.instance.scr_SettingsManager.text_CaseData_Path.text = CaseData_Path;
            StaticManager.instance.scr_SettingsManager.text_DesignFile_Path.text = Designfile_path;
            StaticManager.instance.scr_SettingsManager.text_Database_Path.text = Database_Path;
        }
        else
        {
            Panoramic_Thickess.text = "0";
            Saftyzone_Radial.text = "0";
            Saftyzone_Apical.text = "0";
            Finetuning_Translation.text = "0";
            Finetuning_Rotation.text = "0";
            Guide_Thickness.text = "0";
            Guide_Height.text = "0";
            Offset_Teeth.text = "0";
            Offset_Sleeve.text = "0";
            Offset_Sleeve2.text = "0";
            Offset_Anchor.text = "0";
            Blocksize_Cuboid.text = "0";
            Blocksize_Cuboid2.text = "0";
            Blocksize_Cylinder.text = "0";
            Blocksize_CuboidWindowXY.text = "0";
            Blocksize_CuboidWindowXY2.text = "0";
            Blocksize_CylinderWindow.text = "0";
            IDtag_Textsize.text = "0";
            Moveall_interval.text = "0";
            Interface_Cementgap.text = "0";
            Interface_Extracementgap.text = "0";
            Interface_Distanceformmargin.text = "0";
            Interface_Horizontaldistance.text = "0";
            Interface_Slopedistance.text = "0";
            Interface_Verticaldistance.text = "0";
            Interface_Angle.text = "0";
            Wax_addradius.text = "0";
            Wax_addradius2.text = "0";
            Wax_removeradius.text = "0";
            Wax_removeradius2.text = "0";
            Wax_smoothingradius.text = "0";
            Wax_smoothingradius2.text = "0";
            Morphing_radius.text = "0";
            Morphing_radius2.text = "0";
            Minimum_Thickness.text = "0";
            Contact_Proximal.text = "0";
            Contact_OcclusalAll.text = "0";
            Contact_OcclusalPart.text = "0";
            Bridge_Area.text = "0";
            Offset_Extracrownhole.text = "0";

            CaseData_Path = Application.persistentDataPath;
            Database_Path = Application.persistentDataPath;
            Designfile_path = Application.persistentDataPath;

            StaticManager.instance.scr_SettingsManager.text_CaseData_Path.text = CaseData_Path;
            StaticManager.instance.scr_SettingsManager.text_DesignFile_Path.text = Designfile_path;
            StaticManager.instance.scr_SettingsManager.text_Database_Path.text = Database_Path;
        }

        Display_Language = SettingValueData._Display_Language;
        Display_Toothnumbering = SettingValueData._Display_Toothnumbering;
        //Autosave_UseOption_Radiobutton = SettingValueData._Autosave_UseOption_Radiobutton;
        Autosave_UseOption = SettingValueData._Autosave_UseOption;
        Export_Foldername = SettingValueData._Export_Foldername;
        Sleeve_Gluechannel = SettingValueData._Sleeve_Gluechannel;
        Sleeve_Protectsleevearea = SettingValueData._Sleeve_Protectsleevearea;
        IDTag_Type = SettingValueData._IDTag_Type;
        Crown_Library = SettingValueData._Crown_Library;
        Screwhole_Use = SettingValueData._Screwhole_Use;
        Item_Overview = SettingValueData._Item_Overview;
        Item_ImplantPlanning = SettingValueData._Item_ImplantPlanning;
        Item_Guide = SettingValueData._Item_Guide;
        Item_Temporarycrown = SettingValueData._Item_Temporarycrown;
        Item_Capturedimagelist = SettingValueData._Item_Capturedimagelist;
        AutoMeasure_Show = SettingValueData._AutoMeasure_Show;

        Display_Language_UI.value = Display_Language;
        Display_Toothnumbering_UI.value = Display_Toothnumbering;
        // Autosave_UseOption_Radiobutton_UI.isOn = Autosave_UseOption_Radiobutton;
        Autosave_UseOption_UI.value = Autosave_UseOption;
        Export_Foldername_UI.value = Export_Foldername;
        Sleeve_Gluechannel_UI.isOn = Sleeve_Gluechannel;
        Sleeve_Protectsleevearea_UI.isOn = Sleeve_Protectsleevearea;
        IDTag_Type_UI.value = IDTag_Type;
        Crown_Library_UI.value = Crown_Library;
        Screwhole_Use_UI.isOn = Screwhole_Use;
        Item_Overview_UI.isOn = Item_Overview;
        Item_ImplantPlanning_UI.isOn = Item_ImplantPlanning;
        Item_Guide_UI.isOn = Item_Guide;
        Item_Temporarycrown_UI.isOn = Item_Temporarycrown;
        Item_Capturedimagelist_UI.isOn = Item_Capturedimagelist;
        AutoMeasure_Show_UI.isOn = AutoMeasure_Show;

    }


    // 케이스에 설정한 값을 각종 UI에 저장하는 함수.
    public void SaveValue()
    {
        SettingValueData._Panoramic_Thickess = Panoramic_Thickess.text;
        SettingValueData._Saftyzone_Radial = Saftyzone_Radial.text;
        SettingValueData._Saftyzone_Apical = Saftyzone_Apical.text;
        SettingValueData._Finetuning_Translation = Finetuning_Translation.text;
        SettingValueData._Finetuning_Rotation = Finetuning_Rotation.text;
        SettingValueData._Guide_Thickness = Guide_Thickness.text;
        SettingValueData._Guide_Height = Guide_Height.text;
        SettingValueData._Offset_Teeth = Offset_Teeth.text;
        SettingValueData._Offset_Sleeve = Offset_Sleeve.text;
        SettingValueData._Offset_Sleeve2 = Offset_Sleeve2.text;
        SettingValueData._Offset_Anchor = Offset_Anchor.text;
        SettingValueData._Blocksize_Cuboid = Blocksize_Cuboid.text;
        SettingValueData._Blocksize_Cuboid2 = Blocksize_Cuboid2.text;
        SettingValueData._Blocksize_Cylinder = Blocksize_Cylinder.text;
        SettingValueData._Blocksize_CuboidWindowXY = Blocksize_CuboidWindowXY.text;
        SettingValueData._Blocksize_CuboidWindowXY2 = Blocksize_CuboidWindowXY2.text;
        SettingValueData._Blocksize_CylinderWindow = Blocksize_CylinderWindow.text;
        SettingValueData._IDtag_Textsize = IDtag_Textsize.text;
        SettingValueData._Moveall_interval = Moveall_interval.text;
        SettingValueData._Interface_Cementgap = Interface_Cementgap.text;
        SettingValueData._Interface_Extracementgap = Interface_Extracementgap.text;
        SettingValueData._Interface_Distanceformmargin = Interface_Distanceformmargin.text;
        SettingValueData._Interface_Horizontaldistance = Interface_Horizontaldistance.text;
        SettingValueData._Interface_Slopedistance = Interface_Slopedistance.text;
        SettingValueData._Interface_Verticaldistance = Interface_Verticaldistance.text;
        SettingValueData._Interface_Angle = Interface_Angle.text;
        SettingValueData._Wax_addradius = Wax_addradius.text;
        SettingValueData._Wax_addradius2 = Wax_addradius2.text;
        SettingValueData._Wax_removeradius = Wax_removeradius.text;
        SettingValueData._Wax_removeradius2 = Wax_removeradius2.text;
        SettingValueData._Wax_smoothingradius = Wax_smoothingradius.text;
        SettingValueData._Wax_smoothingradius2 = Wax_smoothingradius2.text;
        SettingValueData._Morphing_radius = Morphing_radius.text;
        SettingValueData._Morphing_radius2 = Morphing_radius2.text;
        SettingValueData._Minimum_Thickness = Minimum_Thickness.text;
        SettingValueData._Contact_Proximal = Contact_Proximal.text;
        SettingValueData._Contact_OcclusalAll = Contact_OcclusalAll.text;
        SettingValueData._Contact_OcclusalPart = Contact_OcclusalPart.text;
        SettingValueData._Bridge_Area = Bridge_Area.text;
        SettingValueData._Offset_Extracrownhole = Offset_Extracrownhole.text;

        SettingValueData._Display_Language = Display_Language_UI.value;
        SettingValueData._Display_Toothnumbering = Display_Toothnumbering_UI.value;
        // SettingValueData._Autosave_UseOption_Radiobutton = Autosave_UseOption_Radiobutton;
        SettingValueData._Autosave_UseOption = Autosave_UseOption_UI.value;
        SettingValueData._Export_Foldername = Export_Foldername_UI.value;
        SettingValueData._Sleeve_Gluechannel = Sleeve_Gluechannel_UI.isOn;
        SettingValueData._Sleeve_Protectsleevearea = Sleeve_Protectsleevearea_UI.isOn;
        SettingValueData._IDTag_Type = IDTag_Type_UI.value;
        SettingValueData._Crown_Library = Crown_Library_UI.value;
        SettingValueData._Screwhole_Use = Screwhole_Use_UI.isOn;
        SettingValueData._Item_Overview = Item_Overview_UI.isOn;
        SettingValueData._Item_ImplantPlanning = Item_ImplantPlanning_UI.isOn;
        SettingValueData._Item_Guide = Item_Guide_UI.isOn;
        SettingValueData._Item_Temporarycrown = Item_Temporarycrown_UI.isOn;
        SettingValueData._Item_Capturedimagelist = Item_Capturedimagelist_UI.isOn;
        SettingValueData._AutoMeasure_Show = AutoMeasure_Show_UI.isOn;

        SettingValueData._CaseData_Path = CaseData_Path;
        SettingValueData._Database_Path = Database_Path;
        SettingValueData._Designfile_path = Designfile_path;
    }

    public void LoadImage()
    {
        int k = 0;
        Transform tf = StaticManager.instance.CaptureList;

        for(int i = 0; i < tf.childCount; ++i)
        {
            tf.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
            tf.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < StaticManager.instance.scr_DrawCaptureArea.Length; ++i)
        {
            StaticManager.instance.scr_DrawCaptureArea[i].ResetThumbnail();
        }

        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            if (file.Extension == ".png")
            {
                byte[] byteTexture = System.IO.File.ReadAllBytes(file.FullName);
                if(byteTexture.Length > 0)
                {
                    Texture2D tex = new Texture2D(0, 0);
                    tex.LoadImage(byteTexture);
                    Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    tf.GetChild(k).GetChild(0).GetComponent<Image>().sprite = sprite;

                    float width = 0f;

                    if (tex.width >= 1920)
                    {
                        width = tex.width / 12.8f;
                    }
                    else
                    {
                        width = tex.width / 3.6f;
                    }

                    tf.GetChild(k).GetChild(0).GetComponent<Button>().enabled = true;
                    tf.GetChild(k).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(width, 87f);
                    tf.GetChild(k).GetComponent<CaptureImageSlot_Scr>().ImagePath = file.FullName;
                    tf.GetChild(k++).gameObject.SetActive(true);
                }
            }
        }

        if (k != 0) 
        {
            tf.GetChild(0).GetComponent<CaptureImageSlot_Scr>().OnClickImageSlot();
        }
    }
}