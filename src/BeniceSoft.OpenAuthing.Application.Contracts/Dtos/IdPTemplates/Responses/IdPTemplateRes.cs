namespace BeniceSoft.OpenAuthing.Dtos.IdPTemplates.Responses;

public class IdPTemplateRes : IdPTemplateSimpleRes
{
    public List<IdPTemplateFieldRes> Fields { get; set; }
}

public class IdPTemplateFieldRes
{
    public string Name { get; set; }

    public string Label { get; set; }

    public string Placeholder { get; set; }

    public string HelpText { get; set; }

    public bool Required { get; set; }

    public string ExtraData { get; set; }

    public string Type { get; set; }
}