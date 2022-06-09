namespace Dominion.Core.Dto.Interfaces
{
    public interface IIdAndDescription
    {
        int    Id          { get; set; }
        string Description { get; set; }
    }

    // https://stackoverflow.com/questions/79126/create-generic-method-constraining-t-to-an-enum/8086788#8086788
    //public interface IIdEnumAndDescription<TEnum> where TEnum : System.Enum
    //{
    //    TEnum  Id          { get; set; }
    //    string Description { get; set; }
    //}

    //public static class IdAndDescriptionFactory {
    //    public static T GetIdAndDescription<T>(int id, string description) where T : IIdAndDescription, new()
    //    {
    //        return new T() { Id = id, Description = description};
    //    }
    //}
}