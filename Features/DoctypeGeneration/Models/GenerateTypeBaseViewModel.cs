namespace Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Models
{
    internal class GenerateTypeBaseViewModel
    {
        private string _name;
        public string Name { get => _name ?? "NameWasEmpty"; set => _name = value; }
    }
}
