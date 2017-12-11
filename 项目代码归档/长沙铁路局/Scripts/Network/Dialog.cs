
namespace Assets.Scripts.Network
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Dialog")]
  public partial class Dialog : global::ProtoBuf.IExtensible
  {
    public Dialog() {}
    
    private int _style = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"style", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int style
    {
      get { return _style; }
      set { _style = value; }
    }
    private bool _isOK = default(bool);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"isOK", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(default(bool))]
    public bool isOK
    {
      get { return _isOK; }
      set { _isOK = value; }
    }
    private string _text = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"text", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string text
    {
      get { return _text; }
      set { _text = value; }
    }
    private string _textOption = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"textOption", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string textOption
    {
      get { return _textOption; }
      set { _textOption = value; }
    }
    private string _textOkButton = "";
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"textOkButton", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string textOkButton
    {
      get { return _textOkButton; }
      set { _textOkButton = value; }
    }
    private com.hawk.core.model.enums.AloneState _checkBoxState = com.hawk.core.model.enums.AloneState.NoneAlone;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"checkBoxState", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(com.hawk.core.model.enums.AloneState.NoneAlone)]
    public com.hawk.core.model.enums.AloneState checkBoxState
    {
      get { return _checkBoxState; }
      set { _checkBoxState = value; }
    }
    private int _itemIcon = default(int);
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"itemIcon", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int itemIcon
    {
      get { return _itemIcon; }
      set { _itemIcon = value; }
    }
    private string _itemName = "";
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"itemName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string itemName
    {
      get { return _itemName; }
      set { _itemName = value; }
    }
    private int _itemNum = default(int);
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"itemNum", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int itemNum
    {
      get { return _itemNum; }
      set { _itemNum = value; }
    }
    private int _redirFuncId = default(int);
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"redirFuncId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int redirFuncId
    {
      get { return _redirFuncId; }
      set { _redirFuncId = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}