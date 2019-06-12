namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents the product sorting
    /// </summary>
    public enum CampgroundSortingEnum
    {
        /// <summary>
        /// Position (display order)
        /// </summary>
        Position = 0,
        /// <summary>
        /// Name: A to Z
        /// </summary>
        NameAsc = 5,
        /// <summary>
        /// Name: Z to A
        /// </summary>
        NameDesc = 6,
        /// <summary>
        /// Position (display order)
        /// </summary>
        Type = 10,
        /// <summary>
        /// Name: A to Z
        /// </summary>
        Amenities = 15,
        /// <summary>
        /// Name: Z to A
        /// </summary>
        Activities = 16,
        /// <summary>
        /// Name: Z to A
        /// </summary>
        Services = 17,
        ///// <summary>
        ///// Price: Low to High
        ///// </summary>
        //PriceAsc = 10,
        ///// <summary>
        ///// Price: High to Low
        ///// </summary>
        //PriceDesc = 11,
        /// <summary>
        /// Product creation date
        /// </summary>
        CreatedOn = 25,
    }
}