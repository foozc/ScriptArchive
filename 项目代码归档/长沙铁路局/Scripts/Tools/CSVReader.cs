using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

public class CSVClassAttributer : System.Attribute {
}

public class CSVColumnAttributer : System.Attribute {
	public string 	sColumnName;
	public int 		iIndex;
	
	public CSVColumnAttributer(string sName) {
		sColumnName = sName;
	}
}

internal class CSVItemColumn {
	public int 		iIndex { get; set; }
	public string 	sColumnName { get; set; }
}

internal class CSVItemHead {
	internal int 	iColumnCount { get; set; }
	internal CSVItemColumn[] columns { get; set; }
	internal CSVItemColumn this[int index] {
		get{
			return columns[index];
		}
	}
}

internal class CSVClassFieldInfoAttr {
	public CSVColumnAttributer	attr { get; set; }
	public FieldInfo	filedInfo { get; set; }
}

internal class CSVClassFieldInfo {
	public int 			iColumnIndex { get; set; }
	public string 		sColumnName { get; set; }
	public FieldInfo	filedInfo { get; set; }
}

public class CSVReader {
	
	private static System.Collections.Generic.Dictionary<System.Type, List<CSVClassFieldInfoAttr>> _Fields = new System.Collections.Generic.Dictionary<System.Type, List<CSVClassFieldInfoAttr>>();
	private static string[] LINESPLIT = new string[] {"\r\n"};

    public static List<T> LoadCSV<T>(string csvPath,bool IsResouces=true)
    {
        bool error = false;
        string[] rowsText=null;
        TextAsset asset = null;
        try
        {
            if (IsResouces)
            {
                var obj = Resources.Load(csvPath, typeof(TextAsset));
                asset = obj as TextAsset;
                error = asset != null && !string.IsNullOrEmpty(asset.text);
                if (error) rowsText = asset.text.Split(LINESPLIT, 0);
            }
            else
            {
                string aa = File.OpenText(csvPath).ReadToEnd();
                       error = !string.IsNullOrEmpty(aa);
                       if (error) rowsText = aa.Split(LINESPLIT, 0);
            }
            if (error)
            {          
                int iRowCount = rowsText.Length - 1;
                if (iRowCount > 0)
                {
                    List<T> csvData = new List<T>();

                    //System.Type tType = typeof(T);
                    List<CSVClassFieldInfoAttr> fieldInfos = _GetCSVColumnFieldInfos<T>();
                    if (fieldInfos != null)
                    {
                        int fieldCount = fieldInfos.Count;
                        string[] columnText = rowsText[0].Split(',');
                        int iColumnCount = columnText.Length;
                        //Debug.Log("Count:	"+iColumnCount);
                        CSVClassFieldInfo[] classFields = new CSVClassFieldInfo[iColumnCount];
                        //System.Type curType = typeof(CSVColumnAttributer);
                        for (int i = 0; i < iColumnCount; i++)
                        {
                            CSVClassFieldInfo fieldInfoItem = new CSVClassFieldInfo() { iColumnIndex = i, sColumnName = columnText[i] };
                            for (int j = 0; j < fieldCount; j++)
                            {
                                if (string.Equals(fieldInfos[j].attr.sColumnName, fieldInfoItem.sColumnName))
                                {
                                    fieldInfoItem.filedInfo = fieldInfos[j].filedInfo;
                                    break;
                                }
                            }
                            classFields[i] = fieldInfoItem;
                        }

                        for (int i = 1; i < iRowCount; i++)
                        {
                            T tData = System.Activator.CreateInstance<T>();
                            string[] colText = rowsText[i].Split(',');
                            for (int j = 0; j < iColumnCount; j++)
                            {
                                if (classFields[j].filedInfo != null)
                                {
                                    // Debug.Log(" :: " + classFields[j].filedInfo.Name + ":"+ classFields[j].filedInfo.FieldType.Name.ToLower());
                                    switch (classFields[j].filedInfo.FieldType.Name.ToLower())
                                    {
                                        case "int":
                                        case "int32":
                                            if (!string.IsNullOrEmpty(colText[j]))
                                            {
                                                classFields[j].filedInfo.SetValue(tData, int.Parse(colText[j]));
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, 0);
                                            }
                                            break;
                                        case "uint":
                                            if (!string.IsNullOrEmpty(colText[j]))
                                            {
                                                classFields[j].filedInfo.SetValue(tData, uint.Parse(colText[j]));
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, 0);
                                            }
                                            break;
                                        case "long":
                                            if (!string.IsNullOrEmpty(colText[j]))
                                            {
                                                classFields[j].filedInfo.SetValue(tData, long.Parse(colText[j]));
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, 0L);
                                            }
                                            break;
                                        case "bool":
                                            bool bValue = false;
                                            bool.TryParse(colText[j], out bValue);
                                            classFields[j].filedInfo.SetValue(tData, bValue);
                                            break;
                                        case "double":
                                            if (!string.IsNullOrEmpty(colText[j]))
                                            {
                                                classFields[j].filedInfo.SetValue(tData, double.Parse(colText[j]));
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, 0.0d);
                                            }
                                            break;
                                        case "float":
                                        case "single":
                                            if (!string.IsNullOrEmpty(colText[j]))
                                            {
                                                classFields[j].filedInfo.SetValue(tData, float.Parse(colText[j]));
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, 0.0f);
                                            }
                                            break;
                                        case "string":
                                            //classFields[j].filedInfo.SetValue(tData, colText[j]);
                                            if (!string.IsNullOrEmpty(colText[j]))
                                            {
                                                classFields[j].filedInfo.SetValue(tData, colText[j]);
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, "");
                                            }
                                            break;
                                        default:
                                            if (classFields[j].filedInfo.FieldType.IsEnum)
                                            {
                                                if (!string.IsNullOrEmpty(colText[j]))
                                                {
                                                    classFields[j].filedInfo.SetValue(tData, int.Parse(colText[j]));
                                                }
                                                else
                                                {
                                                    classFields[j].filedInfo.SetValue(tData, 0);
                                                }
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, colText[j]);
                                            }
                                            break;
                                    }
                                }
                            }
                            csvData.Add(tData);
                        }
                    }
                    Resources.UnloadAsset(asset);
                    return csvData;
                }
            }
            return null;
        }
        finally
        {
            if (asset != null)
            {
                Resources.UnloadAsset(asset);
            }
        }
    }  
    public static List<T> LoadCSVByScriptObject<T>(string csvPath, bool IsResouces = true) where T:ScriptableObject
    {
        bool error = false;
        string[] rowsText = null;
        TextAsset asset = null;
        try
        {
            if (IsResouces)
            {
                var obj = Resources.Load(csvPath, typeof(TextAsset));
                asset = obj as TextAsset;
                error = asset != null && !string.IsNullOrEmpty(asset.text);
                if (error) rowsText = asset.text.Split(LINESPLIT, 0);
            }
            else
            {
                string aa = File.OpenText(csvPath).ReadToEnd();
                error = !string.IsNullOrEmpty(aa);
                if (error) rowsText = aa.Split(LINESPLIT, 0);
            }
            if (error)
            {
                int iRowCount = rowsText.Length - 1;
                if (iRowCount > 0)
                {
                    List<T> csvData = new List<T>();

                    //System.Type tType = typeof(T);
                    List<CSVClassFieldInfoAttr> fieldInfos = _GetCSVColumnFieldInfos<T>();
                    if (fieldInfos != null)
                    {
                        int fieldCount = fieldInfos.Count;
                        string[] columnText = rowsText[0].Split(',');
                        int iColumnCount = columnText.Length;
                        //Debug.Log("Count:	"+iColumnCount);
                        CSVClassFieldInfo[] classFields = new CSVClassFieldInfo[iColumnCount];
                        //System.Type curType = typeof(CSVColumnAttributer);
                        for (int i = 0; i < iColumnCount; i++)
                        {
                            CSVClassFieldInfo fieldInfoItem = new CSVClassFieldInfo() { iColumnIndex = i, sColumnName = columnText[i] };
                            for (int j = 0; j < fieldCount; j++)
                            {
                                if (string.Equals(fieldInfos[j].attr.sColumnName, fieldInfoItem.sColumnName))
                                {
                                    fieldInfoItem.filedInfo = fieldInfos[j].filedInfo;
                                    break;
                                }
                            }
                            classFields[i] = fieldInfoItem;
                        }

                        for (int i = 1; i < iRowCount; i++)
                        {
                            T tData = ScriptableObject.CreateInstance<T>();
                            string[] colText = rowsText[i].Split(',');
                            for (int j = 0; j < iColumnCount; j++)
                            {
                                if (classFields[j].filedInfo != null)
                                {
                                    // Debug.Log(" :: " + classFields[j].filedInfo.Name + ":"+ classFields[j].filedInfo.FieldType.Name.ToLower());
                                    switch (classFields[j].filedInfo.FieldType.Name.ToLower())
                                    {
                                        case "int":
                                        case "int32":
                                            if (!string.IsNullOrEmpty(colText[j]))
                                            {
                                                classFields[j].filedInfo.SetValue(tData, int.Parse(colText[j]));
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, 0);
                                            }
                                            break;
                                        case "uint":
                                            if (!string.IsNullOrEmpty(colText[j]))
                                            {
                                                classFields[j].filedInfo.SetValue(tData, uint.Parse(colText[j]));
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, 0);
                                            }
                                            break;
                                        case "long":
                                            if (!string.IsNullOrEmpty(colText[j]))
                                            {
                                                classFields[j].filedInfo.SetValue(tData, long.Parse(colText[j]));
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, 0L);
                                            }
                                            break;
                                        case "bool":
                                            bool bValue = false;
                                            bool.TryParse(colText[j], out bValue);
                                            classFields[j].filedInfo.SetValue(tData, bValue);
                                            break;
                                        case "double":
                                            if (!string.IsNullOrEmpty(colText[j]))
                                            {
                                                classFields[j].filedInfo.SetValue(tData, double.Parse(colText[j]));
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, 0.0d);
                                            }
                                            break;
                                        case "float":
                                        case "single":
                                            if (!string.IsNullOrEmpty(colText[j]))
                                            {
                                                classFields[j].filedInfo.SetValue(tData, float.Parse(colText[j]));
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, 0.0f);
                                            }
                                            break;
                                        case "string":
                                            //classFields[j].filedInfo.SetValue(tData, colText[j]);
                                            if (!string.IsNullOrEmpty(colText[j]))
                                            {
                                                classFields[j].filedInfo.SetValue(tData, colText[j]);
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, "");
                                            }
                                            break;
                                        default:
                                            if (classFields[j].filedInfo.FieldType.IsEnum)
                                            {
                                                if (!string.IsNullOrEmpty(colText[j]))
                                                {
                                                    classFields[j].filedInfo.SetValue(tData, int.Parse(colText[j]));
                                                }
                                                else
                                                {
                                                    classFields[j].filedInfo.SetValue(tData, 0);
                                                }
                                            }
                                            else
                                            {
                                                classFields[j].filedInfo.SetValue(tData, colText[j]);
                                            }
                                            break;
                                    }
                                }
                            }
                            csvData.Add(tData);
                        }
                    }
                    Resources.UnloadAsset(asset);
                    return csvData;
                }
            }
            return null;
        }
        finally
        {
            if (asset != null)
            {
                Resources.UnloadAsset(asset);
            }
        }
    }
	static List<CSVClassFieldInfoAttr>	_GetCSVColumnFieldInfos<T> () {
		System.Type tType = typeof(T);
		if (!_Fields.ContainsKey(tType)) {
			FieldInfo[] fieldInfos = tType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfos.Length > 0){
				List<CSVClassFieldInfoAttr> fields = new List<CSVClassFieldInfoAttr>();
				int iCount = fieldInfos.Length;
				System.Type cusAttrType = typeof(CSVColumnAttributer);
				for(int i = 0; i < iCount; i++){
					if(fieldInfos[i].IsDefined(cusAttrType, true)) {
						fields.Add(
							new CSVClassFieldInfoAttr(){ 
								attr = fieldInfos[i].GetCustomAttributes(cusAttrType, true)[0] as CSVColumnAttributer,
								filedInfo = fieldInfos[i],
							}
						);
					}
				}
				_Fields.Add(tType, fields);
				return fields;
			}
		}
		else{
			return _Fields[tType];
		}
		return null;
	}
		
}
