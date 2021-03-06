﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fr8Data.Crates;
using Fr8Data.DataTransferObjects;
using Fr8Data.Helpers;
using Fr8Data.Manifests;
using Fr8Data.States;
using Newtonsoft.Json;

namespace Fr8Data.Control
{
    public class UpstreamData
    {
        public static readonly UpstreamData Value = new UpstreamData();

        private UpstreamData()
        {
        }
    }

    /// <summary>
    /// This interface is applied to controls and control data items (e.g. radio buttons)
    /// that support nested controls.
    /// </summary>
    public interface ISupportsNestedFields
    {
        IList<ControlDefinitionDTO> Controls { get; }
    }

    public interface IResettable
    {
        void Reset(List<string> fieldNames = null);
    }

    public class ControlTypes
    {
        public const string TextBox = "TextBox";
        public const string TextBoxBig = "TextBoxBig";
        public const string CheckBox = "CheckBox";
        public const string DropDownList = "DropDownList";
        public const string RadioButtonGroup = "RadioButtonGroup";
        public const string FilterPane = "FilterPane";
        public const string MappingPane = "MappingPane";
        public const string TextBlock = "TextBlock";
        public const string FilePicker = "FilePicker";
        public const string Routing = "Routing";
        public const string FieldList = "FieldList";
        public const string Button = "Button";
        public const string TextSource = "TextSource";
        public const string TextArea = "TextArea";
        public const string QueryBuilder = "QueryBuilder";
        public const string ManagePlan = "ManagePlan";
        public const string Duration = "Duration";
        public const string RunPlanButton = "RunPlanButton";
        public const string UpstreamDataChooser = "UpstreamDataChooser";
        public const string UpstreamFieldChooser = "UpstreamFieldChooser";
        public const string UpstreamCrateChooser = "UpstreamCrateChooser";
        public const string DatePicker = "DatePicker";
        public const string CrateChooser = "CrateChooser";
        public const string ContainerTransition = "ContainerTransition";
        public const string MetaControlContainer = "MetaControlContainer";
        public const string ControlList = "ControlList";
        public const string SelectData = "SelectData";
        public const string ExternalObjectChooser = "ExternalObjectChooser";
        public const string BuildMessageAppender = "BuildMessageAppender";
    }

    public class SelectData : ControlDefinitionDTO
    {
        public SelectData()
        {
            Type = ControlTypes.SelectData;
        }

        [JsonProperty("activityTemplateId")]
        public string ActivityTemplateId { get; set; }

        [JsonProperty("activityTemplateName")]
        public string ActivityTemplateName { get; set; }

        [JsonProperty("subPlanId")]
        public string SubPlanId { get; set; }

        [JsonProperty("externalObjectName")]
        public string ExternalObjectName { get; set; }
    }

    public class ExternalObjectChooser : ControlDefinitionDTO
    {
        public ExternalObjectChooser()
        {
            Type = ControlTypes.ExternalObjectChooser;
        }

        [JsonProperty("activityTemplateId")]
        public string ActivityTemplateId { get; set; }

        [JsonProperty("subPlanId")]
        public string SubPlanId { get; set; }

        [JsonProperty("externalObjectName")]
        public string ExternalObjectName { get; set; }
    }

    public class CheckBox : ControlDefinitionDTO
    {
        public CheckBox()
        {
            Type = ControlTypes.CheckBox;
        }
    }

    public class RunPlanButton : ControlDefinitionDTO
    {
        public RunPlanButton()
        {
            Type = ControlTypes.RunPlanButton;
        }
    }

    public class DropDownList : ControlDefinitionDTO
    {
        [JsonProperty("listItems")]
        public List<ListItem> ListItems { get; set; }

        [JsonProperty("selectedKey")]
        public string selectedKey { get; set; }

        [JsonProperty("hasRefreshButton")]
        public bool HasRefreshButton { get; set; }

        [JsonProperty("selectedItem")]
        public FieldDTO SelectedItem { get; set; }

        public DropDownList() : base()
        {
            ListItems = new List<ListItem>();
            Type = "DropDownList";
            HasRefreshButton = false;
        }

        public void SelectByKey(string key)
        {
            SelectItem(ListItems?.FirstOrDefault(x => x.Key == key));
        }

        public void SelectByValue(string value)
        {
            SelectItem(ListItems?.FirstOrDefault(x => x.Value == value));
        }

        private void SelectItem(ListItem newItem)
        {
            selectedKey = newItem?.Key;
            Value = newItem?.Value;
        }
    }

    public class RadioButtonGroup : ControlDefinitionDTO, IContainerControl
    {
        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        [JsonProperty("radios")]
        public List<RadioButtonOption> Radios { get; set; }

        public RadioButtonGroup()
        {
            Radios = new List<RadioButtonOption>();
            Type = ControlTypes.RadioButtonGroup;
        }

        public IEnumerable<IControlDefinition> EnumerateChildren()
        {
            return Radios;
        }
    }

    public class TextBox : ControlDefinitionDTO
    {
        public TextBox()
        {
            Type = ControlTypes.TextBox;
        }
    }

    public class DatePicker : ControlDefinitionDTO
    {
        public DatePicker()
        {
            Type = ControlTypes.DatePicker;
        }
    }

    public class QueryBuilder : ControlDefinitionDTO
    {
        public QueryBuilder()
        {
            Type = ControlTypes.QueryBuilder;
        }
    }

    public class ContainerTransition : ControlDefinitionDTO
    {
        [JsonProperty("transitions")]
        public List<ContainerTransitionField> Transitions { get; set; }
        public ContainerTransition()
        {
            Type = ControlTypes.ContainerTransition;
            this.Transitions = new List<ContainerTransitionField>();
        }
    }

    public class TextBoxMetaDescriptionDTO : ControlMetaDescriptionDTO
    {
        public TextBoxMetaDescriptionDTO() : base("TextBoxMetaDescriptionDTO", "TextBox")
        {
            // this.Controls.Add(new TextBox { });
        }

        public override ControlDefinitionDTO CreateControl()
        {
            return new TextBox()
            {
                Label = this.Controls.First().Value
            };
        }
    }

    public class TextBlockMetaDescriptionDTO : ControlMetaDescriptionDTO
    {
        public TextBlockMetaDescriptionDTO() : base("TextBlockMetaDescriptionDTO", "TextBlock")
        {
            // this.Controls.Add(new TextArea());
        }

        public override ControlDefinitionDTO CreateControl()
        {
            return new TextBlock()
            {
                Value = this.Controls.First().Value
            };
        }
    }

    public class FilePickerMetaDescriptionDTO : ControlMetaDescriptionDTO
    {
        public static string[] FileExtensions = { "xlsx" };
        public FilePickerMetaDescriptionDTO() : base("FilePickerMetaDescriptionDTO", "File Picker")
        {
            /*
            this.Controls.Add(new TextBox());
            this.Controls.Add(new DropDownList() { ListItems = FileExtensions.Select(x => new ListItem { Key = x, Value = x}).ToList()});
            */
        }

        public override ControlDefinitionDTO CreateControl()
        {
            return new FilePicker
            {
                Label = this.Controls.First().Value
            };
        }
    }

    public class SelectDataMetaDescriptionDTO : ControlMetaDescriptionDTO
    {
        public SelectDataMetaDescriptionDTO()
            : base("SelectDataMetaDescriptionDTO", "Select Data")
        {
        }

        public override ControlDefinitionDTO CreateControl()
        {
            return new ExternalObjectChooser
            {
                Label = this.Controls[0].Value,
                Name = "SelectData",
                ActivityTemplateId = ((SelectData)this.Controls[1]).ActivityTemplateId
            };
        }
    }


    [JsonConverter(typeof(ControlMetaDescriptionDTOConverter))]
    public class ControlMetaDescriptionDTO
    {
        [JsonProperty("controls")]
        public List<ControlDefinitionDTO> Controls { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public ControlMetaDescriptionDTO(string type, string description)
        {
            this.Type = type;
            this.Description = description;
            Controls = new List<ControlDefinitionDTO>();
        }

        public virtual ControlDefinitionDTO CreateControl()
        {
            throw new NotImplementedException();
        }
    }

    public class MetaControlContainer : ControlDefinitionDTO
    {
        [JsonProperty("metaDescriptions")]
        [ForcePropertySync]
        public List<ControlMetaDescriptionDTO> MetaDescriptions { get; set; }

        public MetaControlContainer() : base(ControlTypes.MetaControlContainer)
        {
            MetaDescriptions = new List<ControlMetaDescriptionDTO>();
        }

        public List<ControlDefinitionDTO> CreateControls()
        {
            return MetaDescriptions.Select(m => m.CreateControl()).ToList();
        }
    }

    public class FilterPane : ControlDefinitionDTO
    {
        [JsonProperty("fields")]
        public List<FilterPaneField> Fields { get; set; }

        public FilterPane()
        {
            Type = ControlTypes.FilterPane;
        }
    }

    public class MappingPane : ControlDefinitionDTO
    {
        public MappingPane()
        {
            Type = ControlTypes.MappingPane;
        }
    }

    public class Generic : ControlDefinitionDTO
    {
        public Generic()
        {
            Type = ControlTypes.TextBox; // Yes, default to TextBox
        }
    }

    public class TextBoxBig : ControlDefinitionDTO
    {
        public TextBoxBig()
        {
            Type = ControlTypes.TextBoxBig; // Text box with multilines
        }
    }

    public class TextArea : ControlDefinitionDTO
    {
        [JsonProperty("isReadOnly")]
        public bool IsReadOnly { get; set; }

        public TextArea() :
            base(ControlTypes.TextArea)
        {
        }
    }

    public class TextBlock : ControlDefinitionDTO
    {
        [JsonProperty("class")]
        public string CssClass
        {
            get;
            set;
        }

        public TextBlock()
        {
            Type = ControlTypes.TextBlock;
        }
    }

    public class FilePicker : ControlDefinitionDTO
    {
        public FilePicker()
        {
            Type = ControlTypes.FilePicker;
        }
    }

    public class FieldList : ControlDefinitionDTO
    {
        public FieldList()
        {
            Type = ControlTypes.FieldList;
        }

        public override void Reset(List<string> fieldNames)
        {
            if (fieldNames != null)
            {
                //key-value pairs are immutable, we need to crate a new List
                var newList = new List<KeyValuePair<string, string>>();
                var keyValuePairs = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(Value);
                foreach (var keyValuePair in keyValuePairs)
                {
                    if (fieldNames.Any(n => n == keyValuePair.Key))
                    {
                        newList.Add(new KeyValuePair<string, string>(keyValuePair.Key, ""));
                    }
                    else
                    {
                        newList.Add(keyValuePair);
                    }
                }
                Value = JsonConvert.SerializeObject(newList);
            }
            else
            {
                Value = "[]";
            }
        }
    }

    public class TextSource : DropDownList
    {
        public const string SpecificValueSource = "specific";

        public const string UpstreamValueSrouce = "upstream";

        [JsonProperty("initialLabel")]
        public string InitialLabel;

        [JsonProperty("upstreamSourceLabel")]
        public string UpstreamSourceLabel;

        [JsonProperty("textValue")]
        public string TextValue;

        [JsonProperty("valueSource")]
        public string ValueSource;

        [JsonIgnore]
        public FieldDTO UpstreamField => SelectedItem;

        public TextSource()
        {
            Type = ControlTypes.TextSource;
        }

        public TextSource(string initialLabel, string upstreamSourceLabel, string name) : this()
        {
            InitialLabel = initialLabel;
            Name = name;
            Source = new FieldSourceDTO
            {
                Label = upstreamSourceLabel,
                ManifestType = CrateManifestTypes.StandardDesignTimeFields
            };
        }

        public string GetValue(ICrateStorage payloadCrateStorage)
        {
            switch (ValueSource)
            {
                case null:
                case SpecificValueSource:
                    return TextValue;
                case UpstreamValueSrouce:
                    if (payloadCrateStorage == null)
                    {
                        throw new Exception("Can't resolve upstream value without payload crate storage provided");
                    }
                    //This is for backward compatibility as controls in existing activites may not be reconfigured to use full field information
                    if (SelectedItem == null)
                    {
                        return payloadCrateStorage.FindField(this.selectedKey);
                    }
                    return payloadCrateStorage.FindField(SelectedItem);
                default:
                    return null;
            }
        }

        public bool CanGetValue(ICrateStorage payloadCrateStorage)
        {
            if (HasSpecificValue)
            {
                return true;
            }

            if (ValueSource == UpstreamValueSrouce && payloadCrateStorage == null)
            {
                return false;
            }

            return true;
        }

        public bool HasValue => !string.IsNullOrEmpty(ValueSource) && (HasUpstreamValue || HasSpecificValue);
        public bool HasUpstreamValue => ValueSource == UpstreamValueSrouce && !string.IsNullOrEmpty(Value);
        public bool HasSpecificValue => ValueSource == SpecificValueSource && !string.IsNullOrEmpty(TextValue);
        public bool ValueSourceIsNotSet => string.IsNullOrEmpty(ValueSource);
    }

    public class Button : ControlDefinitionDTO
    {
        [JsonProperty("class")]
        public string CssClass;

        /// <summary>
        /// Where the button was clicked before the current /configure request was sent.
        /// Used to recognize 'click' event on server-side.
        /// </summary>
        [JsonProperty("clicked")]
        public bool Clicked;

        public Button()
        {
            Type = ControlTypes.Button;
        }
    }

    public class Duration : ControlDefinitionDTO
    {
        public Duration()
        {
            Type = ControlTypes.Duration;
            InnerLabel = "Choose duration:";
        }

        [JsonProperty("value")]
        public new TimeSpan Value
        {
            get
            {
                return new TimeSpan(this.Days, this.Hours, this.Minutes, 0);
            }
        }

        [JsonProperty("innerLabel")]
        public string InnerLabel { get; set; }

        [JsonProperty("days")]
        public Int32 Days { get; set; }

        [JsonProperty("hours")]
        public Int32 Hours { get; set; }

        [JsonProperty("minutes")]
        public Int32 Minutes { get; set; }

    }




    public class FieldSourceDTO
    {
        [JsonProperty("manifestType")]
        public string ManifestType { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("filterByTag")]
        public string FilterByTag { get; set; }

        [JsonProperty("requestUpstream")]
        public bool RequestUpstream { get; set; }

        [JsonProperty("availabilityType")]
        public AvailabilityType AvailabilityType { get; set; }

        public FieldSourceDTO()
        {
        }

        public FieldSourceDTO(string manifestType, string label)
        {
            ManifestType = manifestType;
            Label = label;
        }
    }

    public class ControlEvent
    {
        public static ControlEvent RequestConfig
        {
            get
            {
                return new ControlEvent("onChange", "requestConfig");
            }
        }

        public static ControlEvent RequestConfigOnClick
        {
            get
            {
                return new ControlEvent("onClick", "requestConfig");
            }
        }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("handler")]
        public string Handler { get; set; }

        public ControlEvent(string name, string handler)
        {
            Name = name;
            Handler = handler;
        }

        public ControlEvent()
        {
        }
    }

    public class ControlList : ControlDefinitionDTO
    {
        [JsonProperty("controlGroups")]
        [ForcePropertySync]
        public IList<IList<ControlDefinitionDTO>> ControlGroups { get; }

        [JsonProperty("templateContainer")]
        public ListTemplate TemplateContainer { get; set; }

        [JsonProperty("addControlGroupButtonText")]
        public string AddControlGroupButtonText { get; set; }
        [JsonProperty("noDataMessage")]
        public string NoDataMessage { get; set; }

        public ControlList()
        {
            ControlGroups = new List<IList<ControlDefinitionDTO>>();
            Type = ControlTypes.ControlList;
        }

        public ControlList(ListTemplate Template) : this()
        {
            this.TemplateContainer = Template;
        }
    }

    public class ListTemplate : IContainerControl, IControlDefinition
    {
        [JsonProperty("template")]
        public IList<ControlDefinitionDTO> Template { get; }
        [JsonProperty("name")]
        public string Name { get; set; }

        public ListTemplate()
        {
            Template = new List<ControlDefinitionDTO>();
        }

        public IEnumerable<IControlDefinition> EnumerateChildren()
        {
            return Template;
        }
    }

    public class RadioButtonOption : ISupportsNestedFields, IContainerControl, IControlDefinition
    {
        public RadioButtonOption()
        {
            Controls = new List<ControlDefinitionDTO>();
        }

        [JsonProperty("selected")]
        public bool Selected { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("controls")]
        public IList<ControlDefinitionDTO> Controls { get; set; }

        public IEnumerable<IControlDefinition> EnumerateChildren()
        {
            return Controls;
        }
    }

    public enum ContainerTransitions
    {
        JumpToActivity = 0,
        LaunchAdditionalPlan,
        JumpToSubplan,
        StopProcessing,
        SuspendProcessing,
        ProceedToNextActivity
    }

    public class ContainerTransitionField
    {
        [JsonProperty("conditions")]
        public List<FilterConditionDTO> Conditions { get; set; }

        [JsonProperty("transition")]
        public ContainerTransitions Transition { get; set; }

        [JsonProperty("targetNodeId")]
        public Guid? TargetNodeId;
    }

    public class FilterPaneField
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ListItem
    {
        [JsonProperty("selected")]
        public bool Selected { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class UpstreamDataChooser : ControlDefinitionDTO
    {
        public UpstreamDataChooser()
        {
            Type = ControlTypes.UpstreamDataChooser;
        }

        [JsonProperty("selectedManifest")]
        public string SelectedManifest { get; set; }

        [JsonProperty("selectedLabel")]
        public string SelectedLabel { get; set; }

        [JsonProperty("selectedFieldType")]
        public string SelectedFieldType { get; set; }
    }

    public class CrateDetails
    {
        [JsonProperty("manifestType")]
        public DropDownList ManifestType { get; set; }

        [JsonProperty("label")]
        public DropDownList Label { get; set; }
    }

    public class UpstreamCrateChooser : ControlDefinitionDTO
    {
        public UpstreamCrateChooser()
        {
            Type = ControlTypes.UpstreamCrateChooser;
        }

        [JsonProperty("selectedCrates")]
        public List<CrateDetails> SelectedCrates { get; set; } = new List<CrateDetails>();

        [JsonProperty("multiSelection")]
        public bool MultiSelection { get; set; }

    }

    public class CrateChooser : ControlDefinitionDTO
    {
        public CrateChooser()
        {
            Type = ControlTypes.CrateChooser;
        }

        [JsonProperty("crateDescriptions")]
        public List<CrateDescriptionDTO> CrateDescriptions { get; set; }

        [JsonProperty("singleManifestOnly")]
        public bool SingleManifestOnly { get; set; }

        [JsonProperty("requestUpstream")]
        public bool RequestUpstream { get; set; }

        [JsonIgnore]
        public bool HasValue
        {
            get { return CrateDescriptions != null && CrateDescriptions.Any(x => x.Selected); }
        }

        public bool CanGetValue(ICrateStorage crateStorage)
        {
            return crateStorage != null;
        }

        public Crate GetValue(ICrateStorage crateStorage)
        {
            var selectedCrate = CrateDescriptions?.FirstOrDefault(x => x.Selected);

            if (selectedCrate == null)
            {
                return null;
            }

            return crateStorage.FirstOrDefault(x => x.Label == selectedCrate.Label && x.ManifestType.Type == selectedCrate.ManifestType);
        }
    }

    public class UpstreamFieldChooser : ControlDefinitionDTO
    {
        public UpstreamFieldChooser()
        {
            Type = ControlTypes.UpstreamFieldChooser;
        }
    }

    public class DocumentationDTO
    {
        public DocumentationDTO(string displayMechanism, string contentPath)
        {
            this.DisplayMechanism = displayMechanism;
            this.ContentPath = contentPath;
        }

        [JsonProperty("displayMechanism")]
        public string DisplayMechanism { get; set; }

        [JsonProperty("contentPath")]
        public string ContentPath { get; set; }

        [JsonProperty("url")]
        public string URL
        {
            get
            {
                if (string.IsNullOrEmpty(ContentPath))
                    return "/activites/documentation";
                return string.Format("/activites/documentation/{0}", ContentPath);
            }
        }
    }
    public class BuildMessageAppender : TextArea
    {
        public BuildMessageAppender()
        {
            Type = ControlTypes.BuildMessageAppender;
        }
    }
}